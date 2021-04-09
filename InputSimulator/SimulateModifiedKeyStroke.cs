// <copyright file=SimulateModifiedKeyStroke company=E2E BOTS>
// Copyright (c) 2018 All Rights Reserved
// </copyright>
// <author>Prabodhini Funde</author>
// <date> 03-10-2018 16:02:56</date>
// <summary></summary>

using System;
using System.Activities;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Text;
using System.Linq;
using System.ComponentModel.Design;
using System.Collections;
using Logger;
using System.Drawing;

namespace Bot.Activity.InputSimulator
{
    [ToolboxBitmap("Resources/SimulateModifiedKeyStroke.png")]
    [Designer(typeof(SimulateModifiedKeyStroke1))]
    public class SimulateModifiedKeyStroke : BaseNativeActivity
    {
        [DllImport("user32.dll", SetLastError = true)]
        private static extern uint SendInput(uint numberOfInputs, INPUT[] inputs, int sizeOfInputStructure);
        [DllImport("user32.dll")]
        static extern uint MapVirtualKey(uint uCode, uint uMapType);
               
        [Editor(typeof(System.ComponentModel.Design.CollectionEditor), typeof(System.Drawing.Design.UITypeEditor))]
        public InArgument<ushort[]> ModifierKeyCodes { get; set; }
              
        [Editor(typeof(System.ComponentModel.Design.CollectionEditor), typeof(System.Drawing.Design.UITypeEditor))]
        public InArgument<ushort[]> KeyCodes { get; set; }

        protected override void Execute(NativeActivityContext context)
        {
            try
            {
                ushort[] modifierKeyCodes = null;
                if (ModifierKeyCodes != null)
                {
                    modifierKeyCodes = ModifierKeyCodes.Get(context);
                    modifierKeyCodes.ToList<ushort>().ForEach(delegate (ushort x)
                    {
                        SimulateKeyDown(x);
                    });
                }
                if (KeyCodes != null)
                {
                    ushort[] keyCodes = ModifierKeyCodes.Get(context);
                    keyCodes.ToList<ushort>().ForEach(delegate (ushort x)
                    {
                        SimulateKeyPress(x);
                    });
                }
                if (ModifierKeyCodes != null)
                {
                    modifierKeyCodes.Reverse<ushort>().ToList<ushort>().ForEach(delegate (ushort x)
                    {
                        SimulateKeyUp(x);
                    });
                }
            }
            catch (Exception ex)
            {
                Log.Logger.LogData(ex.Message + " in activity SimulateModifiedKeyStroke", LogLevel.Error);
                if (!ContinueOnError)
                {
                    context.Abort();
                }
            }
        }

        public static void SimulateKeyDown(ushort keyCode)
        {
            INPUT iNPUT = default(INPUT);
            iNPUT.Type = 1u;
            iNPUT.Data.Keyboard = default(KEYBDINPUT);
            iNPUT.Data.Keyboard.Vk = (ushort)keyCode;
            iNPUT.Data.Keyboard.Scan = 0;
            iNPUT.Data.Keyboard.Flags = 0u;
            iNPUT.Data.Keyboard.Time = 0u;
            iNPUT.Data.Keyboard.ExtraInfo = IntPtr.Zero;
            uint num = SendInput(1u, new INPUT[]
            {
                iNPUT
            }, Marshal.SizeOf(typeof(INPUT)));
            if (num == 0u)
            {
                throw new Exception(string.Format("The key down simulation for {0} was not successful.", keyCode));
            }
        }

        public static void SimulateKeyUp(ushort keyCode)
        {
            INPUT iNPUT = default(INPUT);
            iNPUT.Type = 1u;
            iNPUT.Data.Keyboard = default(KEYBDINPUT);
            iNPUT.Data.Keyboard.Vk = (ushort)keyCode;
            iNPUT.Data.Keyboard.Scan = 0;
            iNPUT.Data.Keyboard.Flags = 2u;
            iNPUT.Data.Keyboard.Time = 0u;
            iNPUT.Data.Keyboard.ExtraInfo = IntPtr.Zero;
            uint num = SendInput(1u, new INPUT[]
            {
                iNPUT
            }, Marshal.SizeOf(typeof(INPUT)));
            if (num == 0u)
            {
                throw new Exception(string.Format("The key up simulation for {0} was not successful.", keyCode));
            }
        }
        public static void SimulateKeyPress(ushort keyCode)
        {
            var down = new INPUT();
            down.Type = (UInt32)InputType.KEYBOARD;
            down.Data.Keyboard = new KEYBDINPUT();
            down.Data.Keyboard.Vk = (UInt16)keyCode;
            // Scan Code here, was 0
            down.Data.Keyboard.Scan = (ushort)MapVirtualKey((UInt16)keyCode, 0);
            down.Data.Keyboard.Flags = 0;
            down.Data.Keyboard.Time = 0;
            down.Data.Keyboard.ExtraInfo = IntPtr.Zero;

            var up = new INPUT();
            up.Type = (UInt32)InputType.KEYBOARD;
            up.Data.Keyboard = new KEYBDINPUT();
            up.Data.Keyboard.Vk = (UInt16)keyCode;
            // Scan Code here, was 0
            up.Data.Keyboard.Scan = (ushort)MapVirtualKey((UInt16)keyCode, 0);
            up.Data.Keyboard.Flags = (UInt32)KeyboardFlag.KEYUP;
            up.Data.Keyboard.Time = 0;
            up.Data.Keyboard.ExtraInfo = IntPtr.Zero;

            INPUT[] inputList = new INPUT[2];
            inputList[0] = down;
            inputList[1] = up;

            var numberOfSuccessfulSimulatedInputs = SendInput(2,
                 inputList, Marshal.SizeOf(typeof(INPUT)));
            if (numberOfSuccessfulSimulatedInputs == 0)
                throw new Exception(
                string.Format("The key press simulation for {0} was not successful.",
                keyCode));
        }
    }
   
}
