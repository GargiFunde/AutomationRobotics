// <copyright file=SimulateMouseAction company=E2E BOTS>
// Copyright (c) 2018 All Rights Reserved
// </copyright>
// <author>Prabodhini Funde</author>
// <date> 03-10-2018 16:02:56</date>
// <summary></summary>

using System;
using System.Activities;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;

using System.Runtime.InteropServices;
using System.Windows;
using Logger;

namespace Bot.Activity.InputSimulator
{
    [ToolboxBitmap("Resources/SimulateMouseAction.png")]
    [Designer(typeof(SimulateMouseAction1))]
    public class SimulateMouseAction : BaseNativeActivity
    {
        [DllImport("User32.dll")]
        private static extern bool SetCursorPos(int X, int Y);
        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        public static extern void mouse_event(uint dwFlags, uint dx, uint dy, uint cButtons, uint dwExtraInfo);

        private const uint MOUSEEVENTF_LEFTDOWN = 0x0002;
        private const uint MOUSEEVENTF_LEFTUP = 0x0004;
        private const uint MOUSEEVENTF_MIDDLEDOWN = 0x0020;
        private const uint MOUSEEVENTF_MIDDLEUP = 0x0040;
        private const uint MOUSEEVENTF_MOVE = 0x0001;
        private const uint MOUSEEVENTF_RIGHTDOWN = 0x0008;
        private const uint MOUSEEVENTF_RIGHTUP = 0x0010;
        private const uint MOUSEEVENTF_WHEEL = 0x0800;

        [RequiredArgument]
        public InArgument<int> XAxis { get; set; }

        [RequiredArgument]
        public InArgument<int> YAxis { get; set; }

        public InArgument<int> ScrollUnit { get; set; }

        public OutArgument<bool> IsActionCompleted { get; set; }

        [RequiredArgument]
        [TypeConverter(typeof(Collection2PropertyConverter1))]
        public string MouseAction { get; set; }

        protected override void Execute(NativeActivityContext context)
        {
            try
            {
                bool isAction = false;
                int iX = XAxis.Get(context);
                int iY = YAxis.Get(context);
                int scrollUnit = ScrollUnit.Get(context);


                System.Drawing.Point p = new System.Drawing.Point(iX, iY);
                SetCursorPos(iX, iY);

                uint X = (uint)XAxis.Get(context);
                uint Y = (uint)YAxis.Get(context);

                if (MouseAction.ToUpper().Contains("LEFT CLICK")) { mouse_event(MOUSEEVENTF_LEFTDOWN | MOUSEEVENTF_LEFTUP, X, Y, 0, 0); isAction = true; }
                else if (MouseAction.ToUpper().Contains("RIGHT CLICK")) { mouse_event(MOUSEEVENTF_RIGHTDOWN | MOUSEEVENTF_RIGHTUP, X, Y, 0, 0); isAction = true; }
                else if (MouseAction.ToUpper().Contains("MIDDLE CLICK")) { mouse_event(MOUSEEVENTF_MIDDLEDOWN | MOUSEEVENTF_MIDDLEUP, X, Y, 0, 0); isAction = true; }
                else if (MouseAction.ToUpper().Contains("LEFT DOUBLE CLICK")) { mouse_event(MOUSEEVENTF_LEFTDOWN | MOUSEEVENTF_LEFTUP, X, Y, 0, 0); mouse_event(MOUSEEVENTF_LEFTDOWN | MOUSEEVENTF_LEFTUP, X, Y, 0, 0); isAction = true; }
                else if (MouseAction.ToUpper().Contains("RIGHT DOUBLE CLICK")) { mouse_event(MOUSEEVENTF_RIGHTDOWN | MOUSEEVENTF_RIGHTUP, X, Y, 0, 0); mouse_event(MOUSEEVENTF_RIGHTDOWN | MOUSEEVENTF_RIGHTUP, X, Y, 0, 0); isAction = true; }
                else if (MouseAction.ToUpper().Contains("MIDDLE DOUBLE CLICK")) { mouse_event(MOUSEEVENTF_MIDDLEDOWN | MOUSEEVENTF_MIDDLEUP, X, Y, 0, 0); mouse_event(MOUSEEVENTF_MIDDLEDOWN | MOUSEEVENTF_MIDDLEUP, X, Y, 0, 0); isAction = true; }
                else if (MouseAction.ToUpper().Contains("SCROLLDOWN"))
                {
                    int a = -120 * scrollUnit; //120 is one unit to scroll '-' indicates scrolldown
                    uint SCROLLDOWN = (uint)a;
                    mouse_event(MOUSEEVENTF_WHEEL, 0, 0, SCROLLDOWN, 0);
                    isAction = true;
                }
                else if (MouseAction.ToUpper().Contains("SCROLLUP"))
                {
                    int a = 120 * scrollUnit; //120 is one unit to scroll
                    uint SCROLLDOWN = (uint)a;
                    mouse_event(MOUSEEVENTF_WHEEL, 0, 0, SCROLLDOWN, 0);
                    isAction = true;
                }

                IsActionCompleted.Set(context, isAction);
                //Logging.Logging.LogWarningMsg("MouseAction", "DoMouseAction", "End");
            }
            catch (Exception ex)
            {
                Log.Logger.LogData(ex.Message + " in activity SimulateMouseAction", LogLevel.Error);
                if (!ContinueOnError)
                {
                    context.Abort();
                }
            }
        }
    }

    public class Collection2PropertyConverter1 : StringConverter
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
            mouseAction.Add("Left Click");
            mouseAction.Add("Right Click");
            mouseAction.Add("Middle Click");
            mouseAction.Add("Left Double Click");
            mouseAction.Add("Right Double Click");
            mouseAction.Add("Middle Double Click");
            mouseAction.Add("Scroll Down");
            mouseAction.Add("Scroll Up");

            return new StandardValuesCollection(mouseAction);
        }

    }
}
