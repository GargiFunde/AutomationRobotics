using System;
using System.Collections.Generic;
using System.Activities;
using System.ComponentModel;
using Tesseract;
using System.Drawing;
using Logger;

namespace Bot.Activity.GoogleOCR
{
    [ToolboxBitmap("Resources/MultipageTifToText.png")]
    [Designer(typeof(MultipageTifToText_ActivityDesigner))]
    public class MultipageTifToText : BaseNativeActivity
    {
        [RequiredArgument]
        [Category("Input")]
        [DisplayName("Image Path")]
        [Description("Set Image Path")]
        public InArgument<string> ImagePath { get; set; }

        [RequiredArgument]
        [Category("Output")]
        [DisplayName("Text Value")]
        [Description("String Array Variable")]
        public OutArgument<string[]> TextValues { get; set; }

        protected override void Execute(NativeActivityContext context)
        {
            try
            {
                string filepath = ImagePath.Get(context);
                List<string> result = new List<string>();

                using (var engine = new TesseractEngine(@"./tessdata", "eng", EngineMode.Default))
                {
                    using (var pixA = PixArray.LoadMultiPageTiffFromFile(filepath))
                    {
                        int i = 1;
                        foreach (var pix in pixA)
                        {
                            using (var page = engine.Process(pix))
                            {
                                var text = page.GetText().Trim();

                                string expectedText = String.Format("Page {0}", i);
                                result.Add(text);
                            }
                            i++;
                        }
                    }
                }
                string[] strvalues = result.ToArray();
                if ((strvalues != null) && (strvalues.Length > 0))
                {

                    TextValues.Set(context, strvalues);
                }
            }
            catch (Exception ex)
            {
                Log.Logger.LogData(ex.Message + " in activity MultipageTifToText", LogLevel.Error);
                if (!ContinueOnError)
                {
                    context.Abort();
                }
            }
        }
    }
}
