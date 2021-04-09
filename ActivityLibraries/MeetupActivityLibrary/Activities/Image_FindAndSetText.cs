using CommonLibrary;
using System;
using System.Activities;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;

namespace Bot.Activity.ActivityLibrary.Activities
{
    [Designer(typeof(Image_FindeAndSetText1))]
    [ToolboxBitmap("Resources/ImageFindAndSetText.png")]
    public class Image_FindAndSetText : BaseNativeActivity
    {

        [Browsable(false)]
        public InArgument<string> ImagePath { get; set; }

        [RequiredArgument]
        public InArgument<string> SetText { get; set; }

        [Browsable(true)]
        private double accuracy = 0.2;
        [RequiredArgument]
        [TypeConverter(typeof(FormatDoubleConverter1))]
        [Description("0 to 1")]
        public double Accuracy { get { return accuracy; } set { accuracy = value; } }

        public OutArgument<bool> Result { get; set; }

        [Browsable(false)]
        public string ImageId { get; set; }

        public Image_FindAndSetText()
        {
            ImagePath = new InArgument<string>();
        }
        protected override void Execute(NativeActivityContext context)
        {
            try
            {
                //ThreadInvoker.Instance.RunByUiThread(() =>
                //{
                string ScreenPath = ImagePath.Get(context);
                if ((ScreenPath != null) && (ScreenPath != string.Empty))
                {
                    if (File.Exists(ScreenPath))
                    {
                        string sValue = SetText.Get(context);
                        GetSetClick getSetClick = new GetSetClick();
                        ImageRecognition imgRecognition = new ImageRecognition();
                        getSetClick = GetSetClick.Set;
                        bool result = imgRecognition.GetSetClickImage(ScreenPath, getSetClick, sValue, 10000, accuracy);
                        Result.Set(context, result);
                    }
                }
                //});
            }
            catch (Exception ex)
            {
                Logger.Log.Logger.LogData(ex.Message + " in activity Image_FindAndSetText", Logger.LogLevel.Error);
                if (!ContinueOnError)
                {
                    context.Abort();
                }
            }

        }
    }
    public class FormatDoubleConverter1 : DoubleConverter
    {
        public override Boolean GetStandardValuesSupported(ITypeDescriptorContext context) { return true; }
        public override Boolean GetStandardValuesExclusive(ITypeDescriptorContext context) { return true; }
        public override TypeConverter.StandardValuesCollection GetStandardValues(ITypeDescriptorContext context)
        {
            List<Double> list = new List<Double>();
            list.Add(0);
            list.Add(0.1);
            list.Add(0.2);
            list.Add(0.3);
            list.Add(0.4);
            list.Add(0.5);
            list.Add(0.6);
            list.Add(0.7);
            list.Add(0.8);
            list.Add(0.9);
            return new StandardValuesCollection(list);
        }
    }
}