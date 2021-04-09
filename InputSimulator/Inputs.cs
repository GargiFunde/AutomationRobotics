// <copyright file=Inputs company=E2E BOTS>
// Copyright (c) 2018 All Rights Reserved
// </copyright>
// <author>Prabodhini Funde</author>
// <date> 03-10-2018 16:02:56</date>
// <summary></summary>

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Bot.Activity.InputSimulator
{
    internal struct INPUT
    {
        public uint Type;

        public MOUSEKEYBDHARDWAREINPUT Data;
    }
    internal struct KEYBDINPUT
    {
        public ushort Vk;

        public ushort Scan;

        public uint Flags;

        public uint Time;

        public IntPtr ExtraInfo;
    }
    [StructLayout(LayoutKind.Explicit)]
    internal struct MOUSEKEYBDHARDWAREINPUT
    {
        [FieldOffset(0)]
        public MOUSEINPUT Mouse;

        [FieldOffset(0)]
        public KEYBDINPUT Keyboard;

        [FieldOffset(0)]
        public HARDWAREINPUT Hardware;
    }
    internal struct MOUSEINPUT
    {
        public int X;

        public int Y;

        public uint MouseData;

        public uint Flags;

        public uint Time;

        public IntPtr ExtraInfo;
    }
    internal struct HARDWAREINPUT
    {
        public uint Msg;

        public ushort ParamL;

        public ushort ParamH;
    }
    public enum InputType : uint
    {
        MOUSE,
        KEYBOARD,
        HARDWARE
    }
    public enum KeyboardFlag : uint
    {
        EXTENDEDKEY = 1u,
        KEYUP,
        UNICODE = 4u,
        SCANCODE = 8u
    }
    public enum VirtualKeyCode : ushort
    {
        LBUTTON = 1,
        RBUTTON,
        CANCEL,
        MBUTTON,
        XBUTTON1,
        XBUTTON2,
        BACK = 8,
        TAB,
        CLEAR = 12,
        RETURN, //enter 
        SHIFT = 16,
        CONTROL,
        MENU,
        PAUSE,
        CAPITAL,
        KANA,
        HANGEUL = 21,
        HANGUL = 21,
        JUNJA = 23,
        FINAL,
        HANJA,
        KANJI = 25,
        ESCAPE = 27,
        CONVERT,
        NONCONVERT,
        ACCEPT,
        MODECHANGE,
        SPACE,
        PRIOR,
        NEXT,
        END,
        HOME,
        LEFT,
        UP,
        RIGHT,
        DOWN,
        SELECT,
        PRINT,
        EXECUTE,
        SNAPSHOT,
        INSERT,
        DELETE,
        HELP,
        VK_0,
        VK_1,
        VK_2,
        VK_3,
        VK_4,
        VK_5,
        VK_6,
        VK_7,
        VK_8,
        VK_9,
        VK_A = 65,
        VK_B,
        VK_C,
        VK_D,
        VK_E,
        VK_F,
        VK_G,
        VK_H,
        VK_I,
        VK_J,
        VK_K,
        VK_L,
        VK_M,
        VK_N,
        VK_O,
        VK_P,
        VK_Q,
        VK_R,
        VK_S,
        VK_T,
        VK_U,
        VK_V,
        VK_W,
        VK_X,
        VK_Y,
        VK_Z,
        LWIN,
        RWIN,
        APPS,
        SLEEP = 95,
        NUMPAD0,
        NUMPAD1,
        NUMPAD2,
        NUMPAD3,
        NUMPAD4,
        NUMPAD5,
        NUMPAD6,
        NUMPAD7,
        NUMPAD8,
        NUMPAD9,
        MULTIPLY,
        ADD,
        SEPARATOR,
        SUBTRACT,
        DECIMAL,
        DIVIDE,
        F1,
        F2,
        F3,
        F4,
        F5,
        F6,
        F7,
        F8,
        F9,
        F10,
        F11,
        F12,
        F13,
        F14,
        F15,
        F16,
        F17,
        F18,
        F19,
        F20,
        F21,
        F22,
        F23,
        F24,
        NUMLOCK = 144,
        SCROLL,
        LSHIFT = 160,
        RSHIFT,
        LCONTROL,
        RCONTROL,
        LMENU,
        RMENU,
        BROWSER_BACK,
        BROWSER_FORWARD,
        BROWSER_REFRESH,
        BROWSER_STOP,
        BROWSER_SEARCH,
        BROWSER_FAVORITES,
        BROWSER_HOME,
        VOLUME_MUTE,
        VOLUME_DOWN,
        VOLUME_UP,
        MEDIA_NEXT_TRACK,
        MEDIA_PREV_TRACK,
        MEDIA_STOP,
        MEDIA_PLAY_PAUSE,
        LAUNCH_MAIL,
        LAUNCH_MEDIA_SELECT,
        LAUNCH_APP1,
        LAUNCH_APP2,
        OEM_1 = 186,
        OEM_PLUS,
        OEM_COMMA,
        OEM_MINUS,
        OEM_PERIOD,
        OEM_2,
        OEM_3,
        OEM_4 = 219,
        OEM_5,
        OEM_6,
        OEM_7,
        OEM_8,
        OEM_102 = 226,
        PROCESSKEY = 229,
        PACKET = 231,
        ATTN = 246,
        CRSEL,
        EXSEL,
        EREOF,
        PLAY,
        ZOOM,
        NONAME,
        PA1,
        OEM_CLEAR
    }

    public class Collection2PropertyConverter : StringConverter
    {

        public override bool GetStandardValuesSupported(ITypeDescriptorContext context)
        {
            //true means show a combobox
            return true;
        }

        public override bool GetStandardValuesExclusive(ITypeDescriptorContext context)
        {
            //true will limit to list. false will show the list, but allow free-form entry
            return true;
        }

        public override System.ComponentModel.TypeConverter.StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
        {
            List<string> mouseAction = new List<string>();
            mouseAction.Add("Enter");
            mouseAction.Add("Tab");
            mouseAction.Add("Space");
            mouseAction.Add("Escape");
            mouseAction.Add("A");
            mouseAction.Add("B");
            mouseAction.Add("C");
            mouseAction.Add("D");

            return new StandardValuesCollection(mouseAction);
        }
       
    }
}
