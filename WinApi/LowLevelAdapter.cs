/**
 *   DotSwitcher: a simple keyboard layout switcher
 *   Copyright (C) 2014-2019 Kirill Mokhovtsev / kurumpa
 *   Contact: kiev.programmer@gmail.com
 *
 *   This program is free software: you can redistribute it and/or modify
 *   it under the terms of the GNU General Public License as published by
 *   the Free Software Foundation, either version 3 of the License, or
 *   (at your option) any later version.
 *
 *   This program is distributed in the hope that it will be useful,
 *   but WITHOUT ANY WARRANTY; without even the implied warranty of
 *   MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 *   GNU General Public License for more details.
 *
 *   You should have received a copy of the GNU General Public License
 *   along with this program.  If not, see <https://www.gnu.org/licenses/>.
 */

using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Diagnostics;
using System.Text;
using System.Threading;
using System.ComponentModel;
using System.Reflection;
using System.Collections.Generic;

namespace dotSwitcher.WinApi
{
    public static partial class LowLevelAdapter
    {
        public static IntPtr SetHook(int type, HookProc callback)
        {
            var process = Process.GetCurrentProcess();
            var module = process.MainModule;
            var handle = GetModuleHandle(module.ModuleName);
            return SetWindowsHookEx(type, callback, handle, 0);
        }
        public static void ReleaseHook(IntPtr id)
        {
            UnhookWindowsHookEx(id);
        }
        public static IntPtr NextHook(int nCode, IntPtr wParam, IntPtr lParam)
        {
            return CallNextHookEx(IntPtr.Zero, nCode, wParam, lParam);
        }

        public static bool KeyPressed(Keys keyCode)
        {
            return (GetKeyState((int)keyCode) & 0x8000) == 0x8000;
        }

        private static IntPtr GetFocusedHandle()
        {
            var threadId = GetCurrentThreadId();
            var wndThreadId = GetWindowThreadProcessId(GetForegroundWindow(), IntPtr.Zero);

            if (threadId == wndThreadId)
            {
                return IntPtr.Zero;
            }

            AttachThreadInput(wndThreadId, threadId, true);
            IntPtr focusedHandle = GetFocus();
            AttachThreadInput(wndThreadId, threadId, false);
            return focusedHandle;
        }

        public static IntPtr GetCurrentLayout()
        {
            var wndThreadId = GetWindowThreadProcessId(GetForegroundWindow(), IntPtr.Zero);
            return GetKeyboardLayout(wndThreadId);
        }

        public static void SetNextKeyboardLayout()
        {
            IntPtr fgWnd = GetForegroundWindow();
            IntPtr hWnd = IntPtr.Zero;
            var threadId = GetWindowThreadProcessId(fgWnd, IntPtr.Zero);
            var info = new GUITHREADINFO();
            info.cbSize = Marshal.SizeOf(info);
            var success = GetGUIThreadInfo(threadId, ref info);

            var focusedHandle = GetFocusedHandle();
            if (success)
            {
                if (info.hwndCaret != IntPtr.Zero)
                {
                    hWnd = info.hwndCaret;
                }
                else if (info.hwndFocus != IntPtr.Zero)
                {
                    hWnd = info.hwndFocus;
                }
                else if (focusedHandle != IntPtr.Zero)
                {
                    hWnd = focusedHandle;
                }
                else if (info.hwndActive != IntPtr.Zero)
                {
                    hWnd = info.hwndActive;
                }
            }
            else
            {
                hWnd = focusedHandle;
            }
            if (hWnd == IntPtr.Zero) { hWnd = fgWnd; }

            // 1. Post language change request to the target window and foreground window
            PostMessage(hWnd, WM_INPUTLANGCHANGEREQUEST, INPUTLANGCHANGE_FORWARD, (uint)HKL_NEXT);
            if (hWnd != fgWnd && fgWnd != IntPtr.Zero)
            {
                PostMessage(fgWnd, WM_INPUTLANGCHANGEREQUEST, INPUTLANGCHANGE_FORWARD, (uint)HKL_NEXT);
            }

            // 2. Safely simulate Shift + Alt: Shift down FIRST, then Alt down, Alt up FIRST, then Shift up.
            // Releasing Alt while Shift is still held down guarantees Windows will NEVER trigger menu mode.
            var shiftDown = MakeKeyInput(Keys.LShiftKey, true);
            var shiftUp = MakeKeyInput(Keys.LShiftKey, false);
            var altDown = MakeKeyInput(Keys.LMenu, true);
            var altUp = MakeKeyInput(Keys.LMenu, false);

            SendInput(4, new INPUT[4] { shiftDown, altDown, altUp, shiftUp }, Marshal.SizeOf(typeof(INPUT)));
        }

        public static void SendCopy()
        {
            var ctrlDown = MakeKeyInput(Keys.LControlKey, true);
            var ctrlUp = MakeKeyInput(Keys.LControlKey, false);
            var cDown = MakeKeyInput(Keys.C, true);
            var cUp = MakeKeyInput(Keys.C, false);
            SendInput(4, new INPUT[4] { ctrlDown, cDown, cUp, ctrlUp }, Marshal.SizeOf(typeof(INPUT)));
        }

        public static void SendPaste()
        {
            var ctrlDown = MakeKeyInput(Keys.LControlKey, true);
            var ctrlUp = MakeKeyInput(Keys.LControlKey, false);
            var vDown = MakeKeyInput(Keys.V, true);
            var vUp = MakeKeyInput(Keys.V, false);
            SendInput(4, new INPUT[4] { ctrlDown, vDown, vUp, ctrlUp }, Marshal.SizeOf(typeof(INPUT)));
        }

        public static string GetSelectedText()
        {
            try
            {
                Clipboard.Clear();
            }
            catch { }

            SendCopy();

            // Wait up to 250ms with 25ms steps for clipboard to receive text from target app
            for (int i = 0; i < 10; i++)
            {
                Thread.Sleep(25);
                try
                {
                    if (Clipboard.ContainsText())
                    {
                        var text = Clipboard.GetText();
                        if (!string.IsNullOrEmpty(text))
                        {
                            return text;
                        }
                    }
                }
                catch { }
            }
            return null;
        }

        public static void SetClipboardAndPaste(string text)
        {
            for (int i = 0; i < 5; i++)
            {
                try
                {
                    Clipboard.SetText(text);
                    break;
                }
                catch
                {
                    Thread.Sleep(20);
                }
            }

            Thread.Sleep(25);
            SendPaste();
        }

        public static void SendKeyPress(Keys vkCode, bool shift = false)
        {
            var down = MakeKeyInput(vkCode, true);
            var up = MakeKeyInput(vkCode, false);

            if (shift)
            {
                var shiftDown = MakeKeyInput(Keys.ShiftKey, true);
                var shiftUp = MakeKeyInput(Keys.ShiftKey, false);
                SendInput(4, new INPUT[4] { shiftDown, down, up, shiftUp }, Marshal.SizeOf(typeof(INPUT)));
            }
            else
            {
                SendInput(2, new INPUT[2] { down, up }, Marshal.SizeOf(typeof(INPUT)));
            }
        }

        public static void ReleasePressedFnKeys()
        {
            ReleasePressedKey(Keys.RControlKey, false);
            ReleasePressedKey(Keys.LControlKey, false);
            ReleasePressedKey(Keys.LShiftKey, false);
            ReleasePressedKey(Keys.RShiftKey, false);
            ReleasePressedKey(Keys.LMenu, false);
            ReleasePressedKey(Keys.RMenu, false);
            ReleasePressedKey(Keys.LWin, false);
            ReleasePressedKey(Keys.RWin, false);
        }

        public static bool ReleasePressedKey(Keys keyCode, bool releaseTwice)
        {
            if (!KeyPressed(keyCode)) { return false; }
            var keyUp = MakeKeyInput(keyCode, false);
            if (releaseTwice)
            {
                var secondDown = MakeKeyInput(keyCode, true);
                var secondUp = MakeKeyInput(keyCode, false);
                SendInput(3, new INPUT[3] { keyUp, secondDown, secondUp }, Marshal.SizeOf(typeof(INPUT)));
            }
            else
            {
                SendInput(1, new INPUT[1] { keyUp }, Marshal.SizeOf(typeof(INPUT)));
            }
            return true;
        }

        public static void SendShowSettingsMessage()
        {
            PostMessage((IntPtr)HWND_BROADCAST, WM_SHOW_SETTINGS, 0, 0);
        }

        private static string GetAutorunPath()
        {
            return System.IO.Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.Startup),
                "dotSwitcher.lnk");
        }
        public static void CreateAutorunShortcut()
        {
            var currentPath = Assembly.GetExecutingAssembly().Location;
            var shortcutLocation = GetAutorunPath();
            var description = "Simple keyboard layout switcher";
            if (System.IO.File.Exists(shortcutLocation))
            {
                return;
            }
            Type shellType = Type.GetTypeFromProgID("WScript.Shell");
            dynamic shell = Activator.CreateInstance(shellType);
            dynamic shortcut = shell.CreateShortcut(shortcutLocation);
            shortcut.Description = description;
            shortcut.TargetPath = currentPath;
            shortcut.Save();
        }
        public static void DeleteAutorunShortcut()
        {
            System.IO.File.Delete(GetAutorunPath());
        }

        public static Keys ToKey(char ch)
        {
            var layout = GetCurrentLayout();

            short keyNumber = VkKeyScanEx(ch, layout);
            if (keyNumber == -1)
            {
                return System.Windows.Forms.Keys.None;
            }
            return (System.Windows.Forms.Keys)(((keyNumber & 0xFF00) << 8) | (keyNumber & 0xFF));
        }
    }
}
