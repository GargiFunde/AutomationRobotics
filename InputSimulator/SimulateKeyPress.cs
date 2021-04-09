using Logger;
using System;
using System.Activities;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Drawing;

namespace Bot.Activity.InputSimulator
{
    [ToolboxBitmap("Resources/SimulateKeyPress.png")]
    [Designer(typeof(SimulateKeyPress1))]
    public class SimulateKeyPress : BaseNativeActivity
    {
        [DllImport("user32.dll", SetLastError = true)]
        private static extern uint SendInput(uint numberOfInputs, INPUT[] inputs, int sizeOfInputStructure);

        [DllImport("user32.dll")]
        static extern uint MapVirtualKey(uint uCode, uint uMapType);

        [RequiredArgument]
        public InArgument<ushort> KeyCode { get; set; }
                 
        protected override void Execute(NativeActivityContext context)
        {
            try
            {
                ushort keyCode = KeyCode.Get(context);
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
            catch (Exception ex)
            {
                Log.Logger.LogData(ex.Message + " in activity SimulateKeyPress", LogLevel.Error);
                if (!ContinueOnError)
                {
                    context.Abort();
                }
            }
        }
    }
    
}
