// <copyright file=SimulateKeyUp company=E2E BOTS>
// Copyright (c) 2018 All Rights Reserved
// </copyright>
// <author>Prabodhini Funde</author>
// <date> 03-10-2018 16:02:56</date>
// <summary></summary>

using Logger;
using System;
using System.Activities;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Drawing;

namespace Bot.Activity.InputSimulator
{
    [ToolboxBitmap("Resources/SimulateKeyUp.png")]
    [Designer(typeof(SimulateKeyUp1))]
    public class SimulateKeyUp : BaseNativeActivity
    {
        [DllImport("user32.dll", SetLastError = true)]
        private static extern uint SendInput(uint numberOfInputs, INPUT[] inputs, int sizeOfInputStructure);

        [RequiredArgument]
        public InArgument<ushort> KeyCode { get; set; }
                 
        protected override void Execute(NativeActivityContext context)
        {
            try
            {
                ushort keyCode = KeyCode.Get(context);
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
            catch (Exception ex)
            {
                Log.Logger.LogData(ex.Message + " in activity SimulateKeyUp", LogLevel.Error);
                if (!ContinueOnError)
                {
                    context.Abort();
                }
            }
        }
    }
    
}
