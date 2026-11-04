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
using System.Diagnostics;
using System.Runtime.Serialization;
using System.Text;
using System.Windows.Forms;

namespace dotSwitcher.Data
{
    public enum KeyboardEventType { Unknown, KeyDown, KeyUp }
    [Serializable]
    public class KeyboardEventArgs : KeyEventArgs, ISerializable
    {
        public KeyboardEventArgs() : base(Keys.None) { }
        public bool Win { get; set; }

        public KeyboardEventType Type;

        public KeyboardEventArgs(SerializationInfo info, StreamingContext context)
            : base((Keys)info.GetValue("keyData", typeof(Keys)))
        {
            Win = (bool)info.GetValue("winMod", typeof(bool));
            Type = KeyboardEventType.KeyDown;
        }
        public KeyboardEventArgs(Keys keyData, bool isWinDown, KeyboardEventType type = KeyboardEventType.KeyDown)
            : base(keyData)
        {
            Win = isWinDown;
            Type = type;
        }
        public new static readonly KeyboardEventArgs Empty = new KeyboardEventArgs(Keys.None, false, KeyboardEventType.Unknown);

        public override bool Equals(Object obj)
        {
            if (obj == null)
            {
                return false;
            }

            var p = obj as KeyboardEventArgs;
            if (p == null)
            {
                return false;
            }
            var equals = p.KeyData == KeyData &&
                p.KeyCode == KeyCode &&
                p.Win == Win;

            return equals;
        }

        public override int GetHashCode()
        {
            return (int)(base.GetHashCode() ^ Win.GetHashCode());
        }

        public bool HasModifiers()
        {
            return Modifiers != Keys.None || Win;
        }

        public override string ToString()
        {
            if (KeyData == Keys.None)
            {
                return "None";
            }

            var vk = KeyCode;

            // Handle modifier keys alone or combinations of modifiers
            bool isCtrlKey = vk == Keys.LControlKey || vk == Keys.RControlKey || vk == Keys.ControlKey;
            bool isShiftKey = vk == Keys.LShiftKey || vk == Keys.RShiftKey || vk == Keys.ShiftKey;
            bool isAltKey = vk == Keys.LMenu || vk == Keys.RMenu || vk == Keys.Menu;
            bool isWinKey = vk == Keys.LWin || vk == Keys.RWin;

            if (isCtrlKey || isShiftKey || isAltKey || isWinKey)
            {
                bool hasCtrl = Control || isCtrlKey;
                bool hasShift = Shift || isShiftKey;
                bool hasAlt = Alt || isAltKey;
                bool hasWin = Win || isWinKey;

                int modCount = (hasCtrl ? 1 : 0) + (hasShift ? 1 : 0) + (hasAlt ? 1 : 0) + (hasWin ? 1 : 0);
                if (modCount > 1)
                {
                    var sbMods = new StringBuilder();
                    if (hasCtrl) sbMods.Append("Ctrl + ");
                    if (hasAlt) sbMods.Append("Alt + ");
                    if (hasShift) sbMods.Append("Shift + ");
                    if (hasWin) sbMods.Append("Win + ");
                    return sbMods.ToString().TrimEnd(' ', '+');
                }

                if (vk == Keys.LControlKey) return "Left Ctrl";
                if (vk == Keys.RControlKey) return "Right Ctrl";
                if (vk == Keys.ControlKey) return "Ctrl";
                if (vk == Keys.LShiftKey) return "Left Shift";
                if (vk == Keys.RShiftKey) return "Right Shift";
                if (vk == Keys.ShiftKey) return "Shift";
                if (vk == Keys.LMenu) return "Left Alt";
                if (vk == Keys.RMenu) return "Right Alt";
                if (vk == Keys.Menu) return "Alt";
                if (isWinKey) return "Win";
            }

            var sb = new StringBuilder();
            if (Control) sb.Append("Ctrl + ");
            if (Alt) sb.Append("Alt + ");
            if (Shift) sb.Append("Shift + ");
            if (Win) sb.Append("Win + ");

            string keyName;
            switch (vk)
            {
                case Keys.Cancel:
                    keyName = "Pause";
                    break;
                case Keys.Return:
                    keyName = "Enter";
                    break;
                case Keys.Back:
                    keyName = "Backspace";
                    break;
                case Keys.Escape:
                    keyName = "Esc";
                    break;
                case Keys.Capital:
                    keyName = "CapsLock";
                    break;
                case Keys.Prior:
                    keyName = "PageUp";
                    break;
                case Keys.Next:
                    keyName = "PageDown";
                    break;
                case Keys.Oemtilde:
                    keyName = "~";
                    break;
                case Keys.OemQuestion:
                    keyName = "/";
                    break;
                case Keys.OemPeriod:
                    keyName = ".";
                    break;
                case Keys.Oemcomma:
                    keyName = ",";
                    break;
                case Keys.OemMinus:
                    keyName = "-";
                    break;
                case Keys.Oemplus:
                    keyName = "+";
                    break;
                case Keys.Oem1:
                    keyName = ";";
                    break;
                case Keys.OemOpenBrackets:
                    keyName = "[";
                    break;
                case Keys.Oem6:
                    keyName = "]";
                    break;
                case Keys.Oem7:
                    keyName = "'";
                    break;
                case Keys.Oem5:
                    keyName = "\\";
                    break;
                default:
                    keyName = vk.ToString();
                    break;
            }

            sb.Append(keyName);
            return sb.ToString();
        }

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("keyData", KeyData, typeof(Keys));
            info.AddValue("winMod", Win, typeof(bool));
        }
    }
}
