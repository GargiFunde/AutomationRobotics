using System;
using System.Activities;
using System.ComponentModel;
using Microsoft.Office.Interop.PowerPoint;
using Microsoft.Office.Core;
using System.IO;
using Logger;
using System.Runtime.InteropServices;
using System.Drawing;

namespace Powerpoint
{
    [ToolboxBitmap("Resources/PowerpointSaveAsJPG.png")]
    [Designer(typeof(ActivityDesignerForPowerpoint_SaveAsJPG))]
    public class Powerpoint_SaveAsJPG : BaseNativeActivity
    {
        [Category("Input Paramaters")]
        [DisplayName("File Path")]
        [Description("Set File Path")]
        [RequiredArgument]
        public InArgument<string> FilePath { get; set; }

        [Category("Input Paramaters")]
        [DisplayName("Destination Folder Path")]
        [Description("Set Destination Folder Path")]
        [RequiredArgument]
        public InArgument<string>  DestinationFolderPath { get; set; }

        protected override void Execute(NativeActivityContext context)
        {
            try
            {
                string filepath = FilePath.Get(context);
                string destinationFolderPath = DestinationFolderPath.Get(context);
                FileInfo objfile = new FileInfo(filepath);
                if (objfile.Extension.Equals(".pptx") || objfile.Extension.Equals(".ppt"))
                {
                    Application App = new Application();
                    Presentation pres = App.Presentations.Open(filepath, MsoTriState.msoTrue, MsoTriState.msoFalse, MsoTriState.msoFalse);
                    pres.SaveAs(destinationFolderPath, PpSaveAsFileType.ppSaveAsJPG, MsoTriState.msoFalse);
                    pres.Close();
                  
                    Marshal.FinalReleaseComObject(pres);
                    App.Quit();
                }

            }
            catch (Exception ex)
            {
                Log.Logger.LogData(ex.Message + " in activity Powerpoint_SaveAsJPG", LogLevel.Error);
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
