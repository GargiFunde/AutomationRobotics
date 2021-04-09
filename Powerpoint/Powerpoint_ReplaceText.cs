using System;
using System.Activities;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using Logger;
using Microsoft.Office.Core;
using Microsoft.Office.Interop.PowerPoint;

namespace Powerpoint
{
    [ToolboxBitmap("Resources/PowerpointReplaceText.png")]
    [Designer(typeof(ActivityDesignerForPowerpoint_ReplaceText))]
    public class Powerpoint_ReplaceText : BaseNativeActivity
    {
        [Category("Input Paramaters")]
        [DisplayName("File Path")]
        [Description("Set File Path")]
        [RequiredArgument]
        public InArgument<string> FilePath { get; set; }

        [Category("Input Paramaters")]
        [DisplayName("Shape Name")]
        [Description("Set Shape Name")]
        [RequiredArgument]
        public InArgument<string> ShapeName { get; set; }

        [Category("Input Paramaters")]
        [DisplayName("Replace Text")]
        [Description("Set Replace Text")]
        [RequiredArgument]
        public InArgument<string> ReplaceText { get; set; }

        [Category("Input Paramaters")]
        [DisplayName("Slide No")]
        [Description("eg.1")]
        public InArgument<Int32> SlideNo { get; set; }
        protected override void Execute(NativeActivityContext context)
        {
            try
            {
                string filepath = FilePath.Get(context);
                string shapeName = this.ShapeName.Get(context);
                string replaceText = this.ReplaceText.Get(context);
                int slideNo = this.SlideNo.Get(context);

                if (File.Exists(filepath))
                {
                    if (0 == slideNo)
                    {
                        slideNo = 1;
                    }

                    Application app = new Application();
                    Presentation pres = app.Presentations.Open(filepath, MsoTriState.msoFalse, MsoTriState.msoFalse, MsoTriState.msoFalse);

                    pres.Slides[slideNo].Shapes[shapeName].TextFrame.TextRange.Text = replaceText;

                    pres.Save();
                    pres.Close();
                    Marshal.FinalReleaseComObject(pres);
                    app.Quit();
                }
                else
                {
                    Log.Logger.LogData("File does not exist:\"" + filepath + "\" in activity Powerpoint_ReplaceText", LogLevel.Error);
                }
            }
            catch (Exception ex)
            {
                Log.Logger.LogData(ex.Message + " in activity Powerpoint_ReplaceText", LogLevel.Error);
            }
            finally
            {
                if (!ContinueOnError)
                {
                    context.Abort();
                }
            }

        }
    }
}
