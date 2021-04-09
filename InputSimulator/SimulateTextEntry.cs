// <copyright file=SimulateTextEntry company=E2E BOTS>
// Copyright (c) 2018 All Rights Reserved
// </copyright>
// <author>Prabodhini Funde</author>
// <date> 03-10-2018 16:02:56</date>
// <summary></summary>

using Logger;
using System;
using System.Activities;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;

namespace Bot.Activity.InputSimulator
{
    [ToolboxBitmap("Resources/SimulateTextEntry.png")]
    [Designer(typeof(SimulateTextEntry1))]
    public class SimulateTextEntry : BaseNativeActivity
    {
        [DllImport("user32.dll", SetLastError = true)]
        private static extern uint SendInput(uint numberOfInputs, INPUT[] inputs, int sizeOfInputStructure);

        [RequiredArgument]
        public InArgument<string> Text { get; set; }
                 
        protected override void Execute(NativeActivityContext context)
        {
            try
            {
                string text = Text.Get(context);
                if ((long)text.Length > 2147483647L)
                {
                    throw new ArgumentException(string.Format("The text parameter is too long. It must be less than {0} characters.", 2147483647u), "text");
                }
                byte[] bytes = Encoding.ASCII.GetBytes(text);
                int num = bytes.Length;
                INPUT[] array = new INPUT[num * 2];
                for (int i = 0; i < num; i++)
                {
                    ushort num2 = (ushort)bytes[i];
                    INPUT iNPUT = default(INPUT);
                    iNPUT.Type = 1u;
                    iNPUT.Data.Keyboard = default(KEYBDINPUT);
                    iNPUT.Data.Keyboard.Vk = 0;
                    iNPUT.Data.Keyboard.Scan = num2;
                    iNPUT.Data.Keyboard.Flags = 4u;
                    iNPUT.Data.Keyboard.Time = 0u;
                    iNPUT.Data.Keyboard.ExtraInfo = IntPtr.Zero;
                    INPUT iNPUT2 = default(INPUT);
                    iNPUT2.Type = 1u;
                    iNPUT2.Data.Keyboard = default(KEYBDINPUT);
                    iNPUT2.Data.Keyboard.Vk = 0;
                    iNPUT2.Data.Keyboard.Scan = num2;
                    iNPUT2.Data.Keyboard.Flags = 6u;
                    iNPUT2.Data.Keyboard.Time = 0u;
                    iNPUT2.Data.Keyboard.ExtraInfo = IntPtr.Zero;
                    if ((num2 & 65280) == 57344)
                    {
                        iNPUT.Data.Keyboard.Flags = (iNPUT.Data.Keyboard.Flags | 1u);
                        iNPUT2.Data.Keyboard.Flags = (iNPUT2.Data.Keyboard.Flags | 1u);
                    }
                    array[2 * i] = iNPUT;
                    array[2 * i + 1] = iNPUT2;
                }
                uint num3 = SendInput((uint)(num * 2), array, Marshal.SizeOf(typeof(INPUT)));
            }
            catch (Exception ex)
            {
                Log.Logger.LogData(ex.Message + " in activity SimulateTextEntry", LogLevel.Error);
                if (!ContinueOnError)
                {
                    context.Abort();
                }
            }
        }
    }
    
}
