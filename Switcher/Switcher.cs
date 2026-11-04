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

using dotSwitcher.Data;
using dotSwitcher.WinApi;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace dotSwitcher.Switcher
{
    public class SwitcherCore : IDisposable
    {
        public event EventHandler<SwitcherErrorArgs> Error;
        private List<KeyboardEventArgs> currentSelection = new List<KeyboardEventArgs>();
        private KeyboardHook kbdHook;
        private MouseHook mouseHook;
        private ISettings settings;
        private bool readyToSwitch;
        public SwitcherCore(ISettings settings)
        {
            this.settings = settings;
            kbdHook = new KeyboardHook();
            kbdHook.KeyboardEvent += ProcessKeyPress;
            mouseHook = new MouseHook();
            mouseHook.MouseEvent += ProcessMousePress;
            readyToSwitch = false;
        }

        public static bool IsPrintable(KeyboardEventArgs evtData)
        {
            if (evtData.Alt || evtData.Control || evtData.Win) { return false; }
            var keyCode = evtData.KeyCode;
            if (keyCode >= Keys.D0 && keyCode <= Keys.Z) { return true; }
            if (keyCode >= Keys.Oem1 && keyCode <= Keys.OemBackslash) { return true; }
            if (keyCode >= Keys.NumPad0 && keyCode <= Keys.NumPad9) { return true; }
            if (keyCode == Keys.Decimal) { return true; }
            return false;
        }

        public bool IsStarted()
        {
            return kbdHook.IsStarted() || mouseHook.IsStarted();
        }
        public void Start()
        {
            kbdHook.Start();
            mouseHook.Start();
        }
        public void Stop()
        {
            kbdHook.Stop();
            mouseHook.Stop();
        }

        // evtData.Handled must be set to true if the key is recognized as hotkey and doesn't need further processing
        private void ProcessKeyPress(object sender, KeyboardEventArgs evtData)
        {
            try
            {
                if (evtData.Type == KeyboardEventType.KeyDown)
                    OnKeyPress(evtData);
                else
                    onKeyRelease(evtData);
            }
            catch (Exception ex)
            {
                OnError(ex);
            }
        }

        private void ProcessMousePress(object sender, EventArgs evtData)
        {
            try
            {
                BeginNewSelection();
            }
            catch (Exception ex)
            {
                OnError(ex);
            }
        }

        private bool HaveModifiers(KeyboardEventArgs evtData)
        {
            var ctrl = evtData.Control;
            var alt = evtData.Alt;
            var win = evtData.Win;

            return ctrl || alt || win;
        }

        private bool HaveTrackingKeys(KeyboardEventArgs evtData)
        {
            var vkCode = evtData.KeyCode;

            return evtData.KeyCode == Keys.ControlKey ||
              vkCode == Keys.LControlKey ||
              vkCode == Keys.RControlKey ||
                // yes, don't interrupt the tracking on PrtSc!
              vkCode == Keys.PrintScreen ||
              vkCode == Keys.ShiftKey ||
              vkCode == Keys.RShiftKey ||
              vkCode == Keys.LShiftKey ||
              vkCode == Keys.NumLock ||
              vkCode == Keys.Scroll;
        }

        private void onKeyRelease(KeyboardEventArgs evtData)
        {
            if (evtData.Equals(settings.SwitchLayoutHotkey) && readyToSwitch)
            {
                SwitchLayout();
                evtData.Handled = false;
                return;
            }
        }

        private void OnKeyPress(KeyboardEventArgs evtData)
        {
            var vkCode = evtData.KeyCode;

            if (evtData.Equals(settings.SwitchLayoutHotkey))
            {
                readyToSwitch = true;
                return;
            }

            readyToSwitch = false;

            if (evtData.Equals(settings.SwitchHotkey))
            {
                ConvertLast();
                evtData.Handled = true;
                return;
            }

            if (evtData.Equals(settings.ConvertSelectionHotkey))
            {
                ConvertSelection();
                evtData.Handled = true;
                return;
            }

            if (this.HaveTrackingKeys(evtData))
                return;

            var notModified = !this.HaveModifiers(evtData);

            if (vkCode == Keys.Space && notModified) { AddToCurrentSelection(evtData); return; }
            if (vkCode == Keys.Back && notModified) { RemoveLast(); return; }
            if (IsPrintable(evtData))
            {
                if (GetPreviousVkCode() == Keys.Space && settings.SmartSelection == false) { BeginNewSelection(); }
                AddToCurrentSelection(evtData);
                return;
            }

            // default: 
            BeginNewSelection();
        }

        private void OnError(Exception ex)
        {
            if (Error != null)
            {
                Error(this, new SwitcherErrorArgs(ex));
            }
        }

        #region selection manipulation
        private Keys GetPreviousVkCode()
        {
            if (currentSelection.Count == 0) { return Keys.None; }
            return currentSelection[currentSelection.Count - 1].KeyCode;
        }
        private void BeginNewSelection() { currentSelection.Clear(); }
        private void AddToCurrentSelection(KeyboardEventArgs data) { currentSelection.Add(data); }
        private void RemoveLast()
        {
            if (currentSelection.Count == 0) { return; }
            currentSelection.RemoveAt(currentSelection.Count - 1);
        }
        #endregion

        private void ConvertSelection()
        {
            LowLevelAdapter.ReleasePressedFnKeys();
            Thread.Sleep(20);

            var selection = LowLevelAdapter.GetSelectedText();
            if (String.IsNullOrEmpty(selection))
            {
                return;
            }

            bool toCyrillic;
            var converted = LayoutConverter.Convert(selection, out toCyrillic);
            if (String.IsNullOrEmpty(converted) || converted == selection)
            {
                return;
            }

            LowLevelAdapter.SetClipboardAndPaste(converted);
            Thread.Sleep(settings.SwitchDelay);
            LowLevelAdapter.SetNextKeyboardLayout();
        }

        private void SwitchLayout()
        {
            BeginNewSelection();
            LowLevelAdapter.SetNextKeyboardLayout();
        }

        private void ConvertLast()
        {
            LowLevelAdapter.ReleasePressedFnKeys();
            var selection = currentSelection.ToList();
            if (selection.Count == 0) { return; }

            var backspaces = Enumerable.Repeat<Keys>(Keys.Back, selection.Count);

            foreach (var vkCode in backspaces) { LowLevelAdapter.SendKeyPress(vkCode, false); }
            Thread.Sleep(settings.SwitchDelay);
            LowLevelAdapter.SetNextKeyboardLayout();
            Thread.Sleep(settings.SwitchDelay);
            foreach (var data in selection)
            {
                LowLevelAdapter.SendKeyPress(data.KeyCode, data.Shift);
            }
        }

        public void Dispose()
        {
            Stop();
        }
    }

    public class SwitcherErrorArgs : EventArgs
    {
        public Exception Error { get; private set; }
        public SwitcherErrorArgs(Exception ex)
        {
            Error = ex;
        }
    }
}
