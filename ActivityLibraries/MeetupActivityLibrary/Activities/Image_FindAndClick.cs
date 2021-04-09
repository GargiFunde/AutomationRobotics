//using CommonLibrary;
using CommonLibrary;
//using RehostedWorkflowDesigner.ImageCapture;
using System;
using System.Activities;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;

namespace Bot.Activity.ActivityLibrary.Activities
{
    [ToolboxBitmap("Resources/ImageFindAndClick.png")]
    [Designer(typeof(Image_FindeAndClick1))]
    public class Image_FindAndClick : BaseNativeActivity
    {

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

        public Image_FindAndClick()
        {
            ImagePath = new InArgument<string>();
        }
        protected override void Execute(NativeActivityContext context)
        {
            try
            {
                string sImagePath = ImagePath.Get(context);
                bool result = false;

                if (File.Exists(sImagePath))
                {
                    GetSetClick getSetClick = new GetSetClick();
                    ImageRecognition imgRecognition = new ImageRecognition();
                    getSetClick = GetSetClick.Click;
                    result = imgRecognition.GetSetClickImage(sImagePath, getSetClick, "", 10000, Accuracy);
                    Result.Set(context, result);
                }
                //});
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
    }
    public class FormatDoubleConverter : DoubleConverter
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