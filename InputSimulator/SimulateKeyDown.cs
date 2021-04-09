// <copyright file=SimulateKeyDown company=E2E BOTS>
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
    [ToolboxBitmap("Resources/SimulateKeyDown.png")]
    [Designer(typeof(SimulateKeyDown1))]
    public class SimulateKeyDown : BaseNativeActivity
    {
        [DllImport("user32.dll", SetLastError = true)]
        private static extern uint SendInput(uint numberOfInputs, INPUT[] inputs, int sizeOfInputStructure);

        [RequiredArgument]
       // [TypeConverter(typeof(Collection2PropertyConverter))]
        public InArgument<ushort> KeyCode { get; set; }
        
        protected override void Execute(NativeActivityContext context)
        {
            try
            {
                ushort keyCode = KeyCode.Get(context);
                //  ushort keyAsciCode = 0;
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
                Log.Logger.LogData(ex.Message + " in activity SimulateKeyDown", LogLevel.Error);
                if (!ContinueOnError)
                {
                    context.Abort();
                }
            }
        }
    }
   
}


//////Not sure if below activities require: each method can be one activity

//    public static void SimulateModifiedKeyStroke(VirtualKeyCode modifierKeyCode, VirtualKeyCode keyCode)
//{
//    InputSimulator.SimulateKeyDown(modifierKeyCode);
//    InputSimulator.SimulateKeyDown(keyCode);
//    InputSimulator.SimulateKeyUp(modifierKeyCode);
//}

//public static void SimulateModifiedKeyStroke(IEnumerable<VirtualKeyCode> modifierKeyCodes, VirtualKeyCode keyCode)
//{
//    if (modifierKeyCodes != null)
//    {
//        modifierKeyCodes.ToList<VirtualKeyCode>().ForEach(delegate (VirtualKeyCode x)
//        {
//            InputSimulator.SimulateKeyDown(x);
//        });
//    }
//    InputSimulator.SimulateKeyPress(keyCode);
//    if (modifierKeyCodes != null)
//    {
//        modifierKeyCodes.Reverse<VirtualKeyCode>().ToList<VirtualKeyCode>().ForEach(delegate (VirtualKeyCode x)
//        {
//            InputSimulator.SimulateKeyUp(x);
//        });
//    }
//}

//public static void SimulateModifiedKeyStroke(VirtualKeyCode modifierKey, IEnumerable<VirtualKeyCode> keyCodes)
//{
//    InputSimulator.SimulateKeyDown(modifierKey);
//    if (keyCodes != null)
//    {
//        keyCodes.ToList<VirtualKeyCode>().ForEach(delegate (VirtualKeyCode x)
//        {
//            InputSimulator.SimulateKeyPress(x);
//        });
//    }
//    InputSimulator.SimulateKeyUp(modifierKey);
//}

//public static void SimulateModifiedKeyStroke(IEnumerable<VirtualKeyCode> modifierKeyCodes, IEnumerable<VirtualKeyCode> keyCodes)
//{
//    if (modifierKeyCodes != null)
//    {
//        modifierKeyCodes.ToList<VirtualKeyCode>().ForEach(delegate (VirtualKeyCode x)
//        {
//            InputSimulator.SimulateKeyDown(x);
//        });
//    }
//    if (keyCodes != null)
//    {
//        keyCodes.ToList<VirtualKeyCode>().ForEach(delegate (VirtualKeyCode x)
//        {
//            InputSimulator.SimulateKeyPress(x);
//        });
//    }
//    if (modifierKeyCodes != null)
//    {
//        modifierKeyCodes.Reverse<VirtualKeyCode>().ToList<VirtualKeyCode>().ForEach(delegate (VirtualKeyCode x)
//        {
//            InputSimulator.SimulateKeyUp(x);
//        });
//    }
//}
