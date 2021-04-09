using CommonLibrary;
//using RehostedWorkflowDesigner.ImageCapture;
using System;
using System.Activities;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Drawing;
using System.Drawing.Design;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static System.ComponentModel.TypeConverter;

namespace Bot.Activity.ActivityLibrary.Activities
{
    public enum FindMethod
    {
        CaptureImage = 1, CaptureXY = 2
    }


    public struct POINT
    {
        public int X;
        public int Y;

        public static implicit operator System.Drawing.Point(POINT point)
        {
            return new System.Drawing.Point(point.X, point.Y);
        }
    }

    [Designer(typeof(Mouse_DragDrop_ActivityDesigner))]
    [ToolboxBitmap("Resources/MouseDragDrop.png")]
    public class Mouse_DragDrop : BaseNativeActivity
    {

        private const int MOUSEEVENTF_LEFTDOWN = 0x0002;
        private const int MOUSEEVENTF_LEFTUP = 0x0004;
        private const int MOUSEEVENTF_MOVE = 0x0001;


        [DllImport("user32.dll")]
        public static extern bool GetCursorPos(out POINT lpPoint);

        [DllImport("user32.dll", EntryPoint = "SetCursorPos")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool SetCursorPos(int x, int y);

        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        public static extern void mouse_event(uint dwFlags, int dx, int dy, int dwData, int dwExtraInfo);

        [Browsable(false)]
        [Category("Option")]
        [DisplayName("Method")]
        [Description("Select Type of Method")]
        public FindMethod Find_Method { get; set; }

        [Category("Input")]
        [DisplayName("Width")]
        [Description("Amount to X units to move")]
        public InArgument<int> XCooridinate { get; set; }

        [Category("Input")]
        [DisplayName("Height")]
        [Description("Amount to Y units to move")]
        public InArgument<int> YCooridinate { get; set; }

        [Browsable(false)]
        [Category("Input")]
        [DisplayName("Image Path")]
        [Description("Enter Image Path")]
        public InArgument<string> ImagePath { get; set; }

        [Browsable(true)]
        private double accuracy = 0.2;
        [RequiredArgument]
        [TypeConverter(typeof(FormatDoubleConverter))]
        [Description("0 to 1")]
        [Category("Input")]
        [DisplayName("Accuracy")]
        public double Accuracy { get { return accuracy; } set { accuracy = value; } }

        [Category("Output")]
        [DisplayName("Result")]
        [Description("Enter Result Variable")]
        public OutArgument<bool> Result { get; set; }

        [Browsable(false)]
        public string ImageId { get; set; }

        [Browsable(true)]
        
        [Category("Mouse Cooridinates")]
        [DisplayName("X_Inti Position")]
        [Description("Enter X Cooridinate")]
        public InArgument<int> X_Start { get; set; }

        [Browsable(true)]
        
        [Category("Mouse Cooridinates")]
        [DisplayName("Y_Inti Position")]
        [Description("Enter Y Coordinate")]
        public InArgument<int> Y_Start { get; set; }

        public Mouse_DragDrop()
        {
            ImagePath = new InArgument<string>();
            Find_Method = FindMethod.CaptureXY;
            X_Start = new InArgument<int>();
            Y_Start = new InArgument<int>();
           

        }
        int Xcoo = 0;
        int Ycoo = 0;
        int Xpos = 0;
        int Ypos = 0;

       
        protected override void Execute(NativeActivityContext context)
        {
           
            Xcoo = XCooridinate.Get(context);
            Ycoo = YCooridinate.Get(context);
             Xpos = X_Start.Get(context);
             Ypos = Y_Start.Get(context);

            try
            {
                switch (Find_Method)

                {
                    //case FindMethod.CaptureImage:
                    //    string sImagePath = ImagePath.Get(context);
                    //    bool result = false;

                    //    if (File.Exists(sImagePath))
                    //    {
                    //        GetSetClick getSetClick = new GetSetClick();
                    //        ImageRecognition imgRecognition = new ImageRecognition();
                    //        getSetClick = GetSetClick.Hover;
                    //        result = imgRecognition.DragDropImage(sImagePath, getSetClick, Xcoo, Ycoo, 10000, Accuracy);
                    //        Result.Set(context, result);
                    //    }

                    //    break;

                    case FindMethod.CaptureXY:
                        int endX = Xcoo;
                        int endY = Ycoo;

                        //endX = endX - Xpos;
                        //endY = endY - Ypos; 
                        SetCursorPos(Xpos, Ypos);

                        Thread.Sleep(1000);
                        mouse_event(MOUSEEVENTF_LEFTDOWN, 0, 0, 0, 0);
                        mouse_event(MOUSEEVENTF_LEFTDOWN, 0, 0, 0, 0);

                        Thread.Sleep(1000);
                        // mouse_event(MOUSEEVENTF_MOVE, endX, endY, 0, 0);
                        SetCursorPos(Xpos+Xcoo, Ypos+ Ycoo);
                        Thread.Sleep(1000);
                        mouse_event(MOUSEEVENTF_LEFTDOWN, 0, 0, 0, 0);
                        mouse_event(MOUSEEVENTF_LEFTUP, 0, 0, 0, 0);






                        break;
                }
               
            }
            catch (Exception ex)
            {
                Logger.Log.Logger.LogData(ex.Message + " in activity Image_FindAndClick", Logger.LogLevel.Error);
                if (!ContinueOnError)
                {
                    context.Abort();
                }
            }
        }

        public void WorkThreadFunction()
        {
            try
            {
                
            }
            catch (Exception)
            {
                // log errors
            }
        }

        public static System.Drawing.Point GetCursorPosition()
        {
            POINT lpPoint;
            GetCursorPos(out lpPoint);
            // NOTE: If you need error handling
            // bool success = GetCursorPos(out lpPoint);
            // if (!success)

            return lpPoint;
        }
    }

   
}
