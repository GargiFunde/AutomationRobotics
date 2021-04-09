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
    [ToolboxBitmap("Resources/PowerpointReplaceImage.png")]
    [Designer(typeof(ActivityDesignerForPowerpoint_ReplaceImage))]
    public class Powerpoint_ReplaceImage : BaseNativeActivity
    {
        [Category("Input Paramaters")]
        [DisplayName("File Path")]
        [Description("Set File Path")]
        [RequiredArgument]
        public InArgument<string> FilePath { get; set; }

        [Category("Input Paramaters")]
        [DisplayName("Slide No")]
        [Description("eg.1")]
        public InArgument<Int32> SlideNo { get; set; }

        [Category("Input Paramaters")]
        [DisplayName("Image Name")]
        [Description("Set Image Name")]
        [RequiredArgument]
        public InArgument<string> ImageName { get; set; }

        [Category("Input Paramaters")]
        [DisplayName("New Image Path")]
        [Description("Set New Image Path")]
        [RequiredArgument]
        public InArgument<string> NewImagePath { get; set; }

        [Category("Output Paramaters")]
        [DisplayName("Result")]
        [Description("Get Result")]
        public OutArgument<bool> Result { get; set; }
        protected override void Execute(NativeActivityContext context)
        {
            try
            {
                string filePath = FilePath.Get(context);
                int slideNo = SlideNo.Get(context);
                string imageName = ImageName.Get(context);
                string newImagePath = NewImagePath.Get(context);
                bool result = false;

                if (File.Exists(filePath))
                {
                    if (File.Exists(newImagePath))
                    {
                        float t;
                        float l;
                        float h;
                        float w;

                        Application app = new Application();
                        app.Visible = MsoTriState.msoTrue;
                       
                        Presentations oPresSet = app.Presentations;
                        Presentation oPres = oPresSet.Open(filePath);
                        if (0 == slideNo)
                        {
                            slideNo = 1;
                        }
                        Slide src = oPres.Slides[slideNo];
                        Microsoft.Office.Interop.PowerPoint.Shape shpOrg;
                        foreach (Microsoft.Office.Interop.PowerPoint.Shape shp in src.Shapes)
                        {
                            if (shp.Type == Microsoft.Office.Core.MsoShapeType.msoPicture)
                            {
                                if (shp.Name.Contains(imageName))
                                {
                                    {
                                        var withBlock = shp;
                                        t = withBlock.Top;
                                        l = withBlock.Left;
                                        h = withBlock.Height;
                                        w = withBlock.Width;
                                    }
                                    shp.Delete();
                                    shpOrg= src.Shapes.AddPicture(newImagePath, MsoTriState.msoFalse, MsoTriState.msoCTrue, l, t, w, h);
                                    shpOrg.Name = imageName;
                                    result = true;
                                    oPres.Save();
                                }
                            }
                        }
                      
                        oPres.Save();
                        Result.Set(context, result);
                        oPres.Close();
                        releaseCOM(oPres);
                        Dispose(app);
                        GC.Collect();
                    }
                    else
                    {
                        Result.Set(context, result);
                        Log.Logger.LogData("Image does not exist:\"" + filePath + "\" in activity Powerpoint_ReplaceImage", LogLevel.Error);
                    }
                   
                }
                else
                {
                    Result.Set(context, result);
                    Log.Logger.LogData("File does not exist:\"" + filePath + "\" in activity Powerpoint_ReplaceImage", LogLevel.Error);
                }
            }
            catch (Exception ex)
            {
                Result.Set(context, false);
                Log.Logger.LogData(ex.Message + " in activity Powerpoint_ReplaceImage", LogLevel.Error);
            }
            finally
            {
                if (!ContinueOnError)
                {
                    context.Abort();
                }
            }
        }

        private static void releaseCOM(object o)
        {
            try
            {
                Marshal.FinalReleaseComObject(o);
            }
            catch { }
            finally
            {
                o = null;
            }
        }

        public void Dispose(Application app)
        {
            try
            {
                app.Quit();
                Marshal.FinalReleaseComObject(app);
            }
            catch (Exception ex)
            {
                Log.Logger.LogData(ex.Message + " in activity Powerpoint_ReplaceImage", LogLevel.Error);
            }

            app = null;
            GC.Collect();
            GC.WaitForPendingFinalizers();
            GC.Collect();
            GC.WaitForPendingFinalizers();
        }
    }
}
