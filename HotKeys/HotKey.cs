//using Logger;
//using System;
//using System.Activities;
//using System.ComponentModel;
//using System.Diagnostics;
//using System.Runtime.InteropServices;
//using System.Threading;
//using System.Windows.Forms;

//namespace HotKeys
//{
//    public enum HardKey
//    {

//        NULL = 0,
//        ENTER = 1,
//        ECS = 2,
//        CAPS = 3, ADD = 4, SUBTRACT = 5, MULTIPLY = 6, DIVIDE = 7, PAGEDOWN = 8, PAGEUP = 9, PRINTSCREEN = 10, DELETE = 11, INSERT = 12, HOME = 13, END = 14, DOWNARROW = 15, BREAK = 16,
//        BACKSPACE = 17, LEFTARROW = 18, TAB = 19, UPARROW = 20, RIGHTARROW = 21, F1 = 22, F2 = 23, F3 = 24, F4 = 25, F5 = 26, F6 = 27, F7 = 28, F8 = 29, F9 = 30, F10 = 31, F11 = 32, F12 = 33, LEFTMOUSE = 34, RIGHTMOUSE = 35

//    }
//    [Designer(typeof(ActivityDesigner1))]
//    public class HotKey : BaseNativeActivity, INotifyPropertyChanged
//    {
//        public void OnPropertyChanged(string property)
//        {
//            if (PropertyChanged != null)
//            {
//                PropertyChanged(this, new PropertyChangedEventArgs(property));
//            }
//        }
//        public event PropertyChangedEventHandler PropertyChanged;



//        private Boolean nameValueC;
//        private Boolean nameValueS;
//        private Boolean nameValueA;
//        private Boolean nameValueW;
//        string temp2;
//        string finalkey = null;
//        string alpha = null;
//        string s = null;
//        string fp = null;


//        [DllImport("User32.dll")]
//        static extern int SetForegroundWindow(IntPtr point);

//        public HotKey()
//        {
//            EmptyField = false;

//            //InitializeComponent();

//        }


//        [Category("Input Paramaters")]
//        [DisplayName("Ctrl")]

//        public Boolean Ctrl
//        {
//            get { return nameValueC; }
//            set
//            {
//                if (value != nameValueC)
//                {
//                    nameValueC = value;
//                    OnPropertyChanged("Ctrl");
//                }
//            }


//        }
//        [Category("Input Paramaters")]
//        [DisplayName("Alt")]
//        public Boolean Alt
//        {
//            get { return nameValueA; }
//            set
//            {
//                if (value != nameValueA)
//                {
//                    nameValueA = value;
//                    OnPropertyChanged("Alt");
//                }
//            }
//        }

//        [Category("Input Paramaters")]
//        [DisplayName("Shift")]
//        public Boolean Shift
//        {
//            get { return nameValueS; }
//            set
//            {
//                if (value != nameValueS)
//                {
//                    nameValueS = value;
//                    OnPropertyChanged("Shift");
//                }
//            }
//        }
//        [Category("Input Paramaters")]
//        [DisplayName("Win")]
//        public Boolean Win
//        {
//            get { return nameValueW; }
//            set
//            {
//                if (value != nameValueW)
//                {
//                    nameValueW = value;
//                    OnPropertyChanged("Win");
//                }
//            }
//        }







//        [Category("Input Paramaters")]
//        [DisplayName("Key")]
//        [Description("Select the Key")]
//        public HardKey Sk { get; set; }

//        [Category("Input")]
//        [DisplayName("Alphabet")]
//        [Description("Enter Alphabet in Block Letter Only")]
//        public InArgument<string> Alphabet { get; set; }

//        [Category("Input")]
//        [DisplayName("FolderPath")]
//        [Description("Enter Folder Path Including Name")]
//        public InArgument<string> FolderPath { get; set; }

//        [Category("Options")]
//        [DisplayName("EmptyField")]
//        public Boolean EmptyField { get; set; }

//        [Category("Options")]
//        [DisplayName("DelayAfter")]
//        [Description("In Milliseconds")]
//        public InArgument<Int32> DelayAfter { get; set; }

//        [Category("Options")]
//        [DisplayName("DelayBefore")]
//        [Description("In Milliseconds")]
//        public InArgument<Int32> DelayBefore { get; set; }


//        protected override void Execute(NativeActivityContext context)
//        {

//            try
//            {
//                if ((DelayBefore.Get(context)) > 0)
//                {

//                    Thread.Sleep(DelayBefore.Get(context));
//                }
//                fp = FolderPath.Get(context);
//                alpha = Alphabet.Get(context);
//                if (alpha != null)
//                {
//                    alpha = "(" + alpha + ")";
//                }

//                if (Ctrl == true & Alt == true)
//                {
//                    s = "^%";
//                    goto done1;
//                }
//                if (fp != null & Ctrl == true & Shift == true)
//                {

//                    System.IO.Directory.CreateDirectory(@fp);

//                    return;
//                }
//                if (Win == true)
//                {
//                    s = "^{ESC}";
//                }

//                if (Ctrl == true)
//                {
//                    s = "^";
//                }

//                if (Alt == true)
//                {
//                    s = "%";
//                }

//                if (Shift == true)
//                {
//                    s = "+";
//                }
//            //if (Win == true)
//            //{
//            //    s = "+";
//            //}
//            done1:

//                switch (Sk)
//                {
//                    case HardKey.ENTER:
//                        temp2 = "{Enter}";

//                        break;
//                    case HardKey.DELETE:
//                        temp2 = "{DEL}";

//                        break;
//                    case HardKey.ADD:
//                        temp2 = "{ADD}";

//                        break;
//                    case HardKey.CAPS:
//                        temp2 = "{CAPSLOCK}";

//                        break;
//                    case HardKey.DIVIDE:
//                        temp2 = "{DIVIDE}";

//                        break;
//                    case HardKey.ECS:
//                        temp2 = "{ESC}";
//                        break;
//                    case HardKey.MULTIPLY:
//                        temp2 = "{MULTIPLY}";
//                        break;
//                    case HardKey.NULL:
//                        temp2 = "";
//                        break;
//                    case HardKey.PAGEUP:
//                        temp2 = "{PGUP}";
//                        break;
//                    case HardKey.PAGEDOWN:
//                        temp2 = "{PGDN}";
//                        break;
//                    case HardKey.PRINTSCREEN:
//                        temp2 = "{PRTSC}";
//                        break;
//                    case HardKey.HOME:
//                        temp2 = "{HOME}";
//                        break;
//                    case HardKey.LEFTARROW:
//                        temp2 = "{LEFT}";
//                        break;
//                    case HardKey.DOWNARROW:
//                        temp2 = "{DOWN}";
//                        break;
//                    case HardKey.TAB:
//                        temp2 = "{TAB}";
//                        break;
//                    case HardKey.BACKSPACE:
//                        temp2 = "{BS}";
//                        break;
//                    case HardKey.BREAK:
//                        temp2 = "{BREAK}";
//                        break;
//                    case HardKey.SUBTRACT:
//                        temp2 = "{SUBTRACT}";
//                        break;
//                    case HardKey.RIGHTARROW:
//                        temp2 = "{RIGHT}";
//                        break;
//                    case HardKey.F1:
//                        temp2 = "{F1}";
//                        break;
//                    case HardKey.F2:
//                        temp2 = "{F2}";
//                        break;
//                    case HardKey.F3:
//                        temp2 = "{F3}";
//                        break;
//                    case HardKey.F4:
//                        temp2 = "{F4}";
//                        break;
//                    case HardKey.F5:
//                        temp2 = "{F5}";
//                        break;
//                    case HardKey.F6:
//                        temp2 = "{F6}";
//                        break;
//                    case HardKey.F7:
//                        temp2 = "{F7}";
//                        break;
//                    case HardKey.F8:
//                        temp2 = "{F8}";
//                        break;
//                    case HardKey.F9:
//                        temp2 = "{F9}";
//                        break;
//                    case HardKey.F10:
//                        temp2 = "{F10}";
//                        break;
//                    case HardKey.F11:
//                        temp2 = "{F11}";
//                        break;
//                    case HardKey.F12:
//                        temp2 = "{F12}";
//                        break;
//                    //case HardKey.RIGHTMOUSE:
//                    //    temp2 = "";
//                    default:
//                        //MessageBox.Show("Default");
//                        //MessageBox.Show(Message.ToString());

//                        temp2 = "";
//                        break;
//                }



//                if (EmptyField == true)
//                {
//                    Process p = Process.GetCurrentProcess();
//                    // Process p = Process.GetProcessesByName("Notepad").FirstOrDefault();
//                    p.WaitForInputIdle();
//                    if (p != null)
//                    {
//                        IntPtr h = p.MainWindowHandle;
//                        SetForegroundWindow(h);
//                        SendKeys.SendWait("^(A){DEL}");

//                    }

//                }



//                if ((DelayAfter.Get(context)) > 0)
//                {

//                    Thread.Sleep(DelayAfter.Get(context));
//                }

//                finalkey = s + temp2 + alpha;
//                Logger.Log.Logger.LogData("Control is " + finalkey, LogLevel.Info);

//                if (finalkey == "^%{DEL}")
//                {
//                    Process p = new Process();
//                    p.StartInfo.FileName = "taskmgr";
//                    p.StartInfo.CreateNoWindow = true;
//                    p.Start();
//                }
//                else
//                {
//                    Process p = Process.GetCurrentProcess();
//                    // Process p = Process.GetProcessesByName("Notepad").FirstOrDefault();
//                    p.WaitForInputIdle();
//                    if (p != null)
//                    {
//                        IntPtr h = p.MainWindowHandle;
//                        SetForegroundWindow(h);
//                        SendKeys.SendWait(finalkey);

//                    }

//                }

//            }
//            catch (Exception e)
//            {
//                Log.Logger.LogData("Exception is" + e, LogLevel.Error);
//            }
//        }

//    }
//}


using Logger;
using System;
using System.Activities;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Forms;

using WindowsInput;
using WindowsInput.Native;

namespace HotKeys
{
    public enum HardKey
    {

        NULL = 0,
        ENTER = 1,
        ECS = 2,
        CAPS = 3, ADD = 4, SUBTRACT = 5, MULTIPLY = 6, DIVIDE = 7, PAGEDOWN = 8, PAGEUP = 9, PRINTSCREEN = 10, DELETE = 11, INSERT = 12, HOME = 13, END = 14, DOWNARROW = 15, BREAK = 16,
        BACKSPACE = 17, LEFTARROW = 18, TAB = 19, UPARROW = 20, RIGHTARROW = 21, F1 = 22, F2 = 23, F3 = 24, F4 = 25, F5 = 26, F6 = 27, F7 = 28, F8 = 29, F9 = 30, F10 = 31, F11 = 32, F12 = 33, LEFTMOUSE = 34, RIGHTMOUSE = 35, DoubleClick = 36

    }

    [ToolboxBitmap("Resources/HotKey.png")]
    [Designer(typeof(ActivityDesigner1))]

    public class HotKey : BaseNativeActivity, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public void OnPropertyChanged(string property)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(property));
            }
        }


        private Boolean nameValueC;
        private Boolean nameValueS;
        private Boolean nameValueA;
        private Boolean nameValueW;
        string temp2;
        string finalkey = null;
        string alpha = null;
        string s = null;
        string fp = null;

        private const int MOUSEEVENTF_LEFTDOWN = 0x0002;
        private const int MOUSEEVENTF_LEFTUP = 0x0004;
        //private const int MOUSEEVENTF_RIGHTDOWN = 0x0008;
        //private const int MOUSEEVENTF_RIGHTUP = 0x0010;


        [DllImport("User32.dll")]
        static extern int SetForegroundWindow(IntPtr point);

        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        public static extern void mouse_event(uint dwFlags, int dx, int dy, int dwData, int dwExtraInfo);


        public HotKey()
        {
            EmptyField = false;
            InputSimulator s = new InputSimulator();
            //InitializeComponent();

        }


        [Category("Input Paramaters")]
        [DisplayName("Ctrl")]

        public Boolean Ctrl
        {
            get { return nameValueC; }
            set
            {
                if (value != nameValueC)
                {
                    nameValueC = value;
                    OnPropertyChanged("Ctrl");
                }
            }


        }

        [Category("Input Paramaters")]
        [DisplayName("Alt")]
        public Boolean Alt
        {
            get { return nameValueA; }
            set
            {
                if (value != nameValueA)
                {
                    nameValueA = value;
                    OnPropertyChanged("Alt");
                }
            }
        }

        [Category("Input Paramaters")]
        [DisplayName("Shift")]
        public Boolean Shift
        {
            get { return nameValueS; }
            set
            {
                if (value != nameValueS)
                {
                    nameValueS = value;
                    OnPropertyChanged("Shift");
                }
            }
        }

        [Category("Input Paramaters")]
        [DisplayName("Win")]
        public Boolean Win
        {
            get { return nameValueW; }
            set
            {
                if (value != nameValueW)
                {
                    nameValueW = value;
                    OnPropertyChanged("Win");
                }
            }
        }







        [Category("Input Paramaters")]
        [DisplayName("Key")]
        [Description("Select the Key")]
        public HardKey Sk { get; set; }

        [Category("Input")]
        [DisplayName("Alphabet")]
        [Description("Enter Alphabet in Block Letter Only")]
        public InArgument<string> Alphabet { get; set; }

        [Category("Input")]
        [DisplayName("FolderPath")]
        [Description("Enter Folder Path Including Name")]
        public InArgument<string> FolderPath { get; set; }

        [Category("Options")]
        [DisplayName("EmptyField")]
        public Boolean EmptyField { get; set; }

        [Category("Options")]
        [DisplayName("DelayAfter")]
        [Description("In Milliseconds")]
        public InArgument<Int32> DelayAfter { get; set; }

        [Category("Options")]
        [DisplayName("DelayBefore")]
        [Description("In Milliseconds")]
        public InArgument<Int32> DelayBefore { get; set; }

        //  public System.Type type { get; set; }

        [DllImport("user32.dll")]
        public static extern IntPtr GetWindowThreadProcessId(IntPtr hWnd, out uint ProcessId);

        [DllImport("user32.dll")]
        private static extern IntPtr GetForegroundWindow();


        protected override void Execute(NativeActivityContext context)
        {
            StringBuilder stringBuilder = new StringBuilder();
            string wins = string.Empty;

            try
            {
                if ((DelayBefore.Get(context)) > 0)
                {

                    Thread.Sleep(DelayBefore.Get(context));
                }
                fp = FolderPath.Get(context);
                alpha = Alphabet.Get(context);
                if (alpha != null)
                {

                    alpha = alpha.ToLower();
                    wins = alpha;

                }

                alpha = "(" + alpha + ")";
                //if (Win == true && wins != null)
                if (Win == true)
                {
                    VirtualKeyCode vkAlphabet = VirtualKeyCode.ESCAPE;
                    VirtualKeyCode vkWinKey = VirtualKeyCode.LWIN;
                    VirtualKeyCode vkAltKey = VirtualKeyCode.MENU;
                    VirtualKeyCode vkShiftKey = VirtualKeyCode.LSHIFT;
                    VirtualKeyCode vkControlKey = VirtualKeyCode.CONTROL;
                    VirtualKeyCode vkSkKey = VirtualKeyCode.ESCAPE;
                    //s = "^{ESC}";
                    //stringBuilder.Append(s);
                    var simu = new InputSimulator();

                    //simu.Keyboard.ModifiedKeyStroke(new[] { VirtualKeyCode.LSHIFT, VirtualKeyCode.LWIN , VirtualKeyCode.LSHIFT }, VirtualKeyCode.VK_1);
                    if (wins != null && wins != "")
                    {
                        switch (wins)
                        {
                            case "a":
                                vkAlphabet = VirtualKeyCode.VK_A;
                                break;

                            case "b":
                                vkAlphabet = VirtualKeyCode.VK_B;
                                break;
                            case "c":
                                vkAlphabet = VirtualKeyCode.VK_C;
                                break;
                            case "d":
                                vkAlphabet = VirtualKeyCode.VK_D;
                                break;
                            case "e":
                                vkAlphabet = VirtualKeyCode.VK_E;
                                break;
                            case "f":
                                vkAlphabet = VirtualKeyCode.VK_F;
                                break;

                            case "g":
                                vkAlphabet = VirtualKeyCode.VK_G;
                                break;
                            case "h":
                                vkAlphabet = VirtualKeyCode.VK_H;
                                break;
                            case "i":
                                vkAlphabet = VirtualKeyCode.VK_I;
                                break;
                            case "j":
                                vkAlphabet = VirtualKeyCode.VK_J;
                                break;
                            case "k":
                                vkAlphabet = VirtualKeyCode.VK_K;
                                break;

                            case "l":
                                vkAlphabet = VirtualKeyCode.VK_L;
                                break;
                            case "m":
                                vkAlphabet = VirtualKeyCode.VK_M;
                                break;
                            case "n":
                                vkAlphabet = VirtualKeyCode.VK_N;
                                break;
                            case "o":
                                vkAlphabet = VirtualKeyCode.VK_O;
                                break;
                            case "p":
                                vkAlphabet = VirtualKeyCode.VK_P;
                                break;

                            case "q":
                                vkAlphabet = VirtualKeyCode.VK_Q;
                                break;
                            case "r":
                                vkAlphabet = VirtualKeyCode.VK_R;
                                break;
                            case "s":
                                vkAlphabet = VirtualKeyCode.VK_S;
                                break;
                            case "t":
                                vkAlphabet = VirtualKeyCode.VK_T;
                                break;

                            case "u":
                                vkAlphabet = VirtualKeyCode.VK_U;
                                break;
                            case "v":
                                vkAlphabet = VirtualKeyCode.VK_V;
                                break;
                            case "w":
                                vkAlphabet = VirtualKeyCode.VK_W;
                                break;
                            case "x":
                                vkAlphabet = VirtualKeyCode.VK_X;
                                break;
                            case "y":
                                vkAlphabet = VirtualKeyCode.VK_Y;
                                break;
                            case "z":
                                vkAlphabet = VirtualKeyCode.VK_Z;
                                break;
                            case "0":
                                vkAlphabet = VirtualKeyCode.VK_0;
                                break;
                            case "1":
                                vkAlphabet = VirtualKeyCode.VK_1;
                                break;
                            case "2":
                                vkAlphabet = VirtualKeyCode.VK_2;
                                break;
                            case "3":
                                vkAlphabet = VirtualKeyCode.VK_3;
                                break;
                            case "4":
                                vkAlphabet = VirtualKeyCode.VK_4;
                                break;
                            case "5":
                                vkAlphabet = VirtualKeyCode.VK_5;
                                break;
                            case "6":
                                vkAlphabet = VirtualKeyCode.VK_6;
                                break;
                            case "7":
                                vkAlphabet = VirtualKeyCode.VK_7;
                                break;
                            case "8":
                                vkAlphabet = VirtualKeyCode.VK_8;
                                break;
                            case "9":
                                vkAlphabet = VirtualKeyCode.VK_9;
                                break;
                            default:
                                //return;

                                break;
                        }

                    }

                    if ((int)Sk != 0)
                    {
                        switch (Sk)
                        {
                            case HardKey.ENTER:
                                vkSkKey = VirtualKeyCode.RETURN;

                                break;
                            case HardKey.DELETE:
                                vkSkKey = VirtualKeyCode.DELETE;

                                break;
                            case HardKey.ADD:
                                vkSkKey = VirtualKeyCode.ADD;

                                break;
                            case HardKey.CAPS:
                                vkSkKey = VirtualKeyCode.CAPITAL;

                                break;
                            case HardKey.DIVIDE:
                                vkSkKey = VirtualKeyCode.DIVIDE;

                                break;
                            case HardKey.ECS:
                                vkSkKey = VirtualKeyCode.ESCAPE;
                                break;
                            case HardKey.MULTIPLY:
                                vkSkKey = VirtualKeyCode.MULTIPLY;
                                break;

                            //case HardKey.PAGEUP:
                            //    vkSkKey = VirtualKeyCode.;
                            //    break;
                            //case HardKey.PAGEDOWN:
                            //    vkSkKey = VirtualKeyCode.;
                            //    break;
                            case HardKey.PRINTSCREEN:
                                vkSkKey = VirtualKeyCode.SNAPSHOT;
                                break;
                            case HardKey.HOME:
                                vkSkKey = VirtualKeyCode.HOME;
                                break;
                            case HardKey.LEFTARROW:
                                vkSkKey = VirtualKeyCode.LEFT;
                                break;
                            case HardKey.DOWNARROW:
                                vkSkKey = VirtualKeyCode.DOWN;
                                break;
                            case HardKey.TAB:
                                vkSkKey = VirtualKeyCode.TAB;
                                break;
                            case HardKey.BACKSPACE:
                                vkSkKey = VirtualKeyCode.BACK;
                                break;
                            //case HardKey.BREAK:
                            //    vkSkKey = VirtualKeyCode.br;
                            //    break;
                            case HardKey.SUBTRACT:
                                vkSkKey = VirtualKeyCode.SUBTRACT;
                                break;
                            case HardKey.RIGHTARROW:
                                vkSkKey = VirtualKeyCode.RIGHT;
                                break;
                            case HardKey.F1:
                                vkSkKey = VirtualKeyCode.F1;
                                break;
                            case HardKey.F2:
                                vkSkKey = VirtualKeyCode.F2;
                                break;
                            case HardKey.F3:
                                vkSkKey = VirtualKeyCode.F3;
                                break;
                            case HardKey.F4:
                                vkSkKey = VirtualKeyCode.F4;
                                break;
                            case HardKey.F5:
                                vkSkKey = VirtualKeyCode.F5;
                                break;
                            case HardKey.F6:
                                vkSkKey = VirtualKeyCode.F6;
                                break;
                            case HardKey.F7:
                                vkSkKey = VirtualKeyCode.F7;
                                break;
                            case HardKey.F8:
                                vkSkKey = VirtualKeyCode.F8;
                                break;
                            case HardKey.F9:
                                vkSkKey = VirtualKeyCode.F9;
                                break;
                            case HardKey.F10:
                                vkSkKey = VirtualKeyCode.F10;
                                break;
                            case HardKey.F11:
                                vkSkKey = VirtualKeyCode.F11;
                                break;
                            case HardKey.F12:
                                vkSkKey = VirtualKeyCode.F12;
                                break;
                            //case HardKey.RIGHTMOUSE:
                            //    vkSkKey = VirtualKeyCode.;
                            //    break;
                            //case HardKey.DoubleClick:
                            //    vkSkKey = VirtualKeyCode.;
                            //    break;

                            //case HardKey.LEFTMOUSE:
                            //    vkSkKey = VirtualKeyCode.;
                            //    break;

                            default:



                                break;
                        }
                    }

                    bool alphabool = (wins != null && wins != "");
                    bool shiftbool = Shift;
                    bool ctrlbool = Ctrl;
                    bool altbool = Alt;
                    bool skbool = (int)Sk != 0;
                    int count = 0;
                    int i = 0;
                    bool[] InputArgsboolArray = new bool[] { alphabool, skbool, ctrlbool, altbool, shiftbool };
                    string[] InputTypes = new string[5] { "alpha", "sk", "ctrl", "alt", "shift" };
                    VirtualKeyCode[] Parameters = new VirtualKeyCode[5] { vkAlphabet, vkSkKey, vkControlKey, vkAltKey, vkShiftKey };
                    string[] TrueInputs = new string[5];
                    VirtualKeyCode[] TrueInputsParam = new VirtualKeyCode[5];
                    foreach (bool val in InputArgsboolArray)
                    {
                        if (val == true)
                        {
                            TrueInputs[count] = InputTypes[i];
                            TrueInputsParam[count] = Parameters[i];
                            count++;
                        }
                        i++;

                    }



                    if (count == 1)       // case with 2 params
                    {
                        switch (TrueInputs[0])
                        {
                            case "alpha":

                                simu.Keyboard.ModifiedKeyStroke(VirtualKeyCode.LWIN, vkAlphabet);
                                break;
                            case "shift":
                                simu.Keyboard.ModifiedKeyStroke(VirtualKeyCode.LWIN, vkShiftKey);
                                break;
                            case "ctrl":
                                simu.Keyboard.ModifiedKeyStroke(VirtualKeyCode.LWIN, vkControlKey);
                                break;
                            case "alt":
                                simu.Keyboard.ModifiedKeyStroke(VirtualKeyCode.LWIN, vkAltKey);
                                break;
                            case "sk":
                                simu.Keyboard.ModifiedKeyStroke(VirtualKeyCode.LWIN, vkSkKey);
                                break;
                            default:
                                return;


                        }
                    }
                    else if (count == 2)    //case with 3 params
                    {
                        simu.Keyboard.ModifiedKeyStroke(new[] { VirtualKeyCode.LWIN, TrueInputsParam[1] }, TrueInputsParam[0]);
                    }
                    else
                    {

                        return;
                    }












                    //switch (wins)
                    //{
                    //    case "a":
                    //        simu.Keyboard.ModifiedKeyStroke(VirtualKeyCode.LWIN, VirtualKeyCode.VK_A);
                    //        break;



                    //    case "b":
                    //        simu.Keyboard.ModifiedKeyStroke(VirtualKeyCode.LWIN, VirtualKeyCode.VK_B);
                    //        break;
                    //    case "c":
                    //        simu.Keyboard.ModifiedKeyStroke(VirtualKeyCode.LWIN, VirtualKeyCode.VK_C);
                    //        break;
                    //    case "d":
                    //        simu.Keyboard.ModifiedKeyStroke(VirtualKeyCode.LWIN, VirtualKeyCode.VK_D);
                    //        break;
                    //    case "e":
                    //        simu.Keyboard.ModifiedKeyStroke(VirtualKeyCode.LWIN, VirtualKeyCode.VK_E);
                    //        break;
                    //    case "f":
                    //        simu.Keyboard.ModifiedKeyStroke(VirtualKeyCode.LWIN, VirtualKeyCode.VK_F);
                    //        break;
                    //    case "g":
                    //        simu.Keyboard.ModifiedKeyStroke(VirtualKeyCode.LWIN, VirtualKeyCode.VK_G);
                    //        break;
                    //    case "h":
                    //        simu.Keyboard.ModifiedKeyStroke(VirtualKeyCode.LWIN, VirtualKeyCode.VK_H);
                    //        break;
                    //    case "i":
                    //        simu.Keyboard.ModifiedKeyStroke(VirtualKeyCode.LWIN, VirtualKeyCode.VK_I);
                    //        break;
                    //    case "j":
                    //        simu.Keyboard.ModifiedKeyStroke(VirtualKeyCode.LWIN, VirtualKeyCode.VK_J);
                    //        break;
                    //    case "k":
                    //        simu.Keyboard.ModifiedKeyStroke(VirtualKeyCode.LWIN, VirtualKeyCode.VK_K);
                    //        break;
                    //    case "l":
                    //        simu.Keyboard.ModifiedKeyStroke(VirtualKeyCode.LWIN, VirtualKeyCode.VK_L);
                    //        break;
                    //    case "m":
                    //        simu.Keyboard.ModifiedKeyStroke(VirtualKeyCode.LWIN, VirtualKeyCode.VK_M);
                    //        break;
                    //    case "n":
                    //        simu.Keyboard.ModifiedKeyStroke(VirtualKeyCode.LWIN, VirtualKeyCode.VK_N);
                    //        break;
                    //    case "o":
                    //        simu.Keyboard.ModifiedKeyStroke(VirtualKeyCode.LWIN, VirtualKeyCode.VK_O);
                    //        break;
                    //    case "p":
                    //        simu.Keyboard.ModifiedKeyStroke(VirtualKeyCode.LWIN, VirtualKeyCode.VK_P);
                    //        break;
                    //    case "q":
                    //        simu.Keyboard.ModifiedKeyStroke(VirtualKeyCode.LWIN, VirtualKeyCode.VK_Q);
                    //        break;
                    //    case "r":
                    //        simu.Keyboard.ModifiedKeyStroke(VirtualKeyCode.LWIN, VirtualKeyCode.VK_R);
                    //        break;
                    //    case "s":
                    //        simu.Keyboard.ModifiedKeyStroke(VirtualKeyCode.LWIN, VirtualKeyCode.VK_S);
                    //        break;
                    //    case "t":
                    //        simu.Keyboard.ModifiedKeyStroke(VirtualKeyCode.LWIN, VirtualKeyCode.VK_T);
                    //        break;
                    //    case "u":
                    //        simu.Keyboard.ModifiedKeyStroke(VirtualKeyCode.LWIN, VirtualKeyCode.VK_U);
                    //        break;
                    //    case "v":
                    //        simu.Keyboard.ModifiedKeyStroke(VirtualKeyCode.LWIN, VirtualKeyCode.VK_V);
                    //        break;
                    //    case "w":
                    //        simu.Keyboard.ModifiedKeyStroke(VirtualKeyCode.LWIN, VirtualKeyCode.VK_W);
                    //        break;
                    //    case "x":
                    //        simu.Keyboard.ModifiedKeyStroke(VirtualKeyCode.LWIN, VirtualKeyCode.VK_X);
                    //        break;
                    //    case "y":
                    //        simu.Keyboard.ModifiedKeyStroke(VirtualKeyCode.LWIN, VirtualKeyCode.VK_Y);
                    //        break;
                    //    case "z":
                    //        simu.Keyboard.ModifiedKeyStroke(VirtualKeyCode.LWIN, VirtualKeyCode.VK_Z);
                    //        break;
                    //    default: return;
                    //        //break;
                    //}




                    return;
                }


                if (Ctrl == true & Alt == true)
                {
                    s = "^%";
                    goto done1;
                }
                if (fp != null & Ctrl == true & Shift == true)
                {

                    System.IO.Directory.CreateDirectory(@fp);

                    return;
                }
                if (Win == true)
                {
                    s = "^{ESC}";
                    stringBuilder.Append(s);
                }

                if (Ctrl == true)
                {
                    s = "^";
                    stringBuilder.Append(s);
                }

                if (Alt == true)
                {
                    s = "%";
                    stringBuilder.Append(s);
                }

                if (Shift == true)
                {
                    s = "+";
                    stringBuilder.Append(s);
                }
            //if (Win == true)
            //{
            //    s = "+";
            //}
            done1:

                switch (Sk)
                {
                    case HardKey.ENTER:
                        temp2 = "{ENTER}";

                        break;
                    case HardKey.DELETE:
                        temp2 = "{DEL}";

                        break;
                    case HardKey.ADD:
                        temp2 = "{ADD}";

                        break;
                    case HardKey.CAPS:
                        temp2 = "{CAPSLOCK}";

                        break;
                    case HardKey.DIVIDE:
                        temp2 = "{DIVIDE}";

                        break;
                    case HardKey.ECS:
                        temp2 = "{ESC}";
                        break;
                    case HardKey.MULTIPLY:
                        temp2 = "{MULTIPLY}";
                        break;
                    case HardKey.NULL:
                        temp2 = "";
                        break;
                    case HardKey.PAGEUP:
                        temp2 = "{PGUP}";
                        break;
                    case HardKey.PAGEDOWN:
                        temp2 = "{PGDN}";
                        break;
                    case HardKey.PRINTSCREEN:
                        temp2 = "{PRTSC}";
                        break;
                    case HardKey.HOME:
                        temp2 = "{HOME}";
                        break;
                    case HardKey.LEFTARROW:
                        temp2 = "{LEFT}";
                        break;
                    case HardKey.DOWNARROW:
                        temp2 = "{DOWN}";
                        break;
                    case HardKey.TAB:
                        temp2 = "{TAB}";
                        break;
                    case HardKey.BACKSPACE:
                        temp2 = "{BS}";
                        break;
                    case HardKey.BREAK:
                        temp2 = "{BREAK}";
                        break;
                    case HardKey.SUBTRACT:
                        temp2 = "{SUBTRACT}";
                        break;
                    case HardKey.RIGHTARROW:
                        temp2 = "{RIGHT}";
                        break;
                    case HardKey.F1:
                        temp2 = "{F1}";
                        break;
                    case HardKey.F2:
                        temp2 = "{F2}";
                        break;
                    case HardKey.F3:
                        temp2 = "{F3}";
                        break;
                    case HardKey.F4:
                        temp2 = "{F4}";
                        break;
                    case HardKey.F5:
                        temp2 = "{F5}";
                        break;
                    case HardKey.F6:
                        temp2 = "{F6}";
                        break;
                    case HardKey.F7:
                        temp2 = "{F7}";
                        break;
                    case HardKey.F8:
                        temp2 = "{F8}";
                        break;
                    case HardKey.F9:
                        temp2 = "{F9}";
                        break;
                    case HardKey.F10:
                        temp2 = "{F10}";
                        break;
                    case HardKey.F11:
                        temp2 = "{F11}";
                        break;
                    case HardKey.F12:
                        temp2 = "{F12}";
                        break;
                    case HardKey.RIGHTMOUSE:
                        temp2 = "+{F10}";
                        break;
                    case HardKey.DoubleClick:
                        temp2 = "DC";
                        break;

                    case HardKey.LEFTMOUSE:
                        temp2 = "LM";
                        break;

                    default:


                        temp2 = "";
                        break;
                }

                stringBuilder.Append(temp2);

                if (EmptyField == true)
                {

                    IntPtr hwnd = GetForegroundWindow();
                    uint pid;
                    GetWindowThreadProcessId(hwnd, out pid);
                    Process p = Process.GetProcessById((int)pid);
                    SendKeys.SendWait("^(A){DEL}");

                }

                if (temp2 == "DC")
                {

                    int X = Cursor.Position.X;
                    int Y = Cursor.Position.Y;
                    Logger.Log.Logger.LogData("X is \n" + X + "Y is " + Y, LogLevel.Info);
                    mouse_event(MOUSEEVENTF_LEFTDOWN, X, Y, 0, 0);
                    mouse_event(MOUSEEVENTF_LEFTUP, X, Y, 0, 0);
                    Thread.Sleep(10);
                    mouse_event(MOUSEEVENTF_LEFTDOWN, X, Y, 0, 0);
                    mouse_event(MOUSEEVENTF_LEFTUP, X, Y, 0, 0);
                    return;

                }


                if (temp2 == "LM")
                {

                    int X = Cursor.Position.X;
                    int Y = Cursor.Position.Y;
                    Logger.Log.Logger.LogData("X is \n" + X + "Y is " + Y, LogLevel.Info);
                    mouse_event(MOUSEEVENTF_LEFTDOWN, X, Y, 0, 0);
                    return;
                }

                stringBuilder.Append(alpha);

                if ((DelayAfter.Get(context)) > 0)
                {

                    Thread.Sleep(DelayAfter.Get(context));
                }

                //finalkey = s + temp2 + alpha;
                finalkey = stringBuilder.ToString();
                // Logger.Log.Logger.LogData("Control is " + finalkey, LogLevel.Info);

                if (finalkey == "^%{DEL}")
                {
                    Process p = new Process();
                    p.StartInfo.FileName = "taskmgr";
                    p.StartInfo.CreateNoWindow = true;
                    p.Start();
                }
                else
                {
                    IntPtr hwnd = GetForegroundWindow();
                    uint pid;
                    GetWindowThreadProcessId(hwnd, out pid);
                    Process p = Process.GetProcessById((int)pid);
                    p.WaitForInputIdle();
                    Logger.Log.Logger.LogData("Current process is " + p.ProcessName, LogLevel.Info);
                    SendKeys.SendWait(finalkey);





                }

            }
            catch (Exception e)
            {
                Log.Logger.LogData("Exception is" + e.Message, LogLevel.Error);
            }
        }


    }
}

