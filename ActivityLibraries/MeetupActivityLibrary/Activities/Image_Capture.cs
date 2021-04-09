using CommonLibrary;
using System;
using System.Activities;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace Bot.Activity.ActivityLibrary.Activities
{
    [ToolboxBitmap("Resources/ImageCapture.png")]
    [Designer(typeof(Image_Capture_ActivityDesigner))]
    public class Image_Capture : BaseNativeActivity
    {
        [Category("Input")]
        [DisplayName("Left")]
        [Description("Enter Left Value")]
        public InArgument<int> Left { get; set; }

        [Category("Input")]
        [DisplayName("Top")]
        [Description("Enter Top Value")]
        public InArgument<int> Top { get; set; }

        [Category("Input")]
        [DisplayName("Height")]
        [Description("Enter Height Value")]
        public InArgument<int> Height { get; set; }

        [Category("Input")]
        [DisplayName("Width")]
        [Description("Enter Width Value")]
        public InArgument<int> Width { get; set; }

        [Browsable(false)]
        [Category("Input")]
        [DisplayName("ImagePath")]
        [Description("Enter Image Path")]
        public InArgument<string> ImagePath { get; set; }

        private String _formatString = null;
        [RequiredArgument]
        [TypeConverter(typeof(FormatStringConverter))]
        public String Type { get { return _formatString; } set { _formatString = value; } }

       // [RequiredArgument]
        public OutArgument<Bitmap> ResultBitmap { get; set; }

        [Browsable(false)]
        public string ImageId { get; set; }

        protected override void Execute(NativeActivityContext context)
        {
            try
            {
                int x = context.GetValue(Left);
                int y = context.GetValue(Top);
                int width = context.GetValue(Width);
                int height = context.GetValue(Height);
                Bitmap bitmap = null;
                if (Type == "Design Time")
                {
                    bitmap = DesignTime();
                }
                else
                {
                    Rectangle rectangle = new Rectangle(x, y, width, height);
                    bitmap = RunTime_GetBitmapFromScreenRect(rectangle);

                }
                if (bitmap == null)
                {
                    Logger.Log.Logger.LogData("Image not found", Logger.LogLevel.Error);
                }
                else
                {
                    ResultBitmap.Set(context, bitmap);
                }
                // we are using static method because by default the regular expression engine caches the 15 most recently used static regular expressions            

            }
            catch (Exception ex)
            {
                Logger.Log.Logger.LogData(ex.Message + " in activity Image_Capture", Logger.LogLevel.Error);
                if (!ContinueOnError)
                {
                    context.Abort();
                }
            }
        }
        private Bitmap DesignTime()
        {
           
            string processDirectory = Environment.CurrentDirectory + "\\" + SelectHelper.CurrentProcessName + "\\Images";
            if (!Directory.Exists(processDirectory))
            {
                Directory.CreateDirectory(processDirectory);
            }
            string ScreenPath = processDirectory + "\\" + ImageId + ".png";

            if ((ScreenPath != null) && (ScreenPath != string.Empty))
            {
                Bitmap bmp = new Bitmap(ScreenPath);
                return bmp;
            }
            return null;
        }

        private  Bitmap RunTime_GetBitmapFromScreenRect(Rectangle r)
        {
            var srcBitmap = new Bitmap(r.Width, r.Height, PixelFormat.Format32bppArgb);
            Graphics.FromImage(srcBitmap).CopyFromScreen(r.Location, Point.Empty, srcBitmap.Size, CopyPixelOperation.SourceCopy);
            return srcBitmap;
        }
    }
    public class FormatStringConverter : StringConverter
    {
        public override Boolean GetStandardValuesSupported(ITypeDescriptorContext context) { return true; }
        public override Boolean GetStandardValuesExclusive(ITypeDescriptorContext context) { return true; }
        public override TypeConverter.StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
        {
            List<String> list = new List<String>();
            list.Add("Design Time");
            list.Add("Run Time");
            return new StandardValuesCollection(list);
        }
    }
}
