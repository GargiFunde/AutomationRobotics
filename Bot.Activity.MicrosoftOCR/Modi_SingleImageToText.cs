using Logger;
using MODI;
using System;
using System.Activities;
using System.ComponentModel;
using System.Drawing;

namespace Bot.Activity.MicrosoftOCR
{
    [Designer(typeof(ActivityDesigner1))]
    public class Modi_SingleImageToText : BaseNativeActivity
    {
        [RequiredArgument]
        public InArgument<string> ImagePath { get; set; }
        [RequiredArgument]
        public OutArgument<string> Text { get; set; }

        protected override void Execute(NativeActivityContext context)
        {
            try
            {
                string filepath = ImagePath.Get(context);
                Bitmap bitmap = new Bitmap(filepath);

                var ocrtext = string.Empty;
                //MODI.Document doc = new Document();
                MODI.Document doc = new MODI.Document();
                doc.Create(filepath);
                //doc.OCR(MODI.MiLANGUAGES.miLANG_ENGLISH, true, true);
                doc.OCR( MODI.MiLANGUAGES.miLANG_ENGLISH, true, true);
                MODI.Image imgObj = (MODI.Image)doc.Images[0];
                ocrtext = imgObj.Layout.Text.ToString();
                doc.Close();

                Text.Set(context, ocrtext);
            }
            catch (Exception ex)
            {
                Log.Logger.LogData(ex.Message + " in activity Modi_SingleImageToText", LogLevel.Error);
                if (!ContinueOnError)
                {
                    context.Abort();
                }
            }
        }
    }
}
