using System;
using System.Activities;
using System.ComponentModel;
using Tesseract;
using System.Drawing;
using Logger;

namespace Bot.Activity.GoogleOCR
{
    [ToolboxBitmap("Resources/SingleImageToText.png")]
    [Designer(typeof(SingleImageToText_ActivityDesigner))]
    public class SingleImageToText : BaseNativeActivity
    {
        [Category("Input Parameters")]
        [DisplayName("Image Path")]
        [Description("Enter Image Path")]
        [RequiredArgument]
        public InArgument<string> ImagePath { get; set; }

        [Category("Output Parameters")]
        [DisplayName("Text(Variable Name)")]
        [Description("Enter Variable Name")]
        [RequiredArgument]
        public OutArgument<string> Text { get; set; }

        protected override void Execute(NativeActivityContext context)
        {
            try
            {
                string filepath = ImagePath.Get(context);
                Bitmap bitmap = new Bitmap(filepath);

                var ocrtext = string.Empty;
                using (var engine = new TesseractEngine(@".\tessdata", "eng", EngineMode.Default))
                {
                    using (var img = PixConverter.ToPix(bitmap))
                    {
                        using (var page = engine.Process(img))
                        {
                            ocrtext = page.GetText();
                        }
                    }
                }
                Text.Set(context, ocrtext);
            }
            catch (Exception ex)
            {
                if (ex.Message.Equals("Parameter is not valid."))
                {
                    Log.Logger.LogData("File Path Issue: Could Not Find Image", LogLevel.Error);
                }
                else
                {
                    Log.Logger.LogData(ex.Message + " in activity SingleImageToText", LogLevel.Error);
                }

                if (!ContinueOnError)
                {
                    context.Abort();
                }
            }
        }
    }
}
