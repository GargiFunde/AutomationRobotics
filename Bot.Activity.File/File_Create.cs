using System;
using System.Activities;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Office.Interop.PowerPoint;
using Microsoft.Office.Core;
using Microsoft.Office.Interop.Excel;
using System.IO;
using Application = Microsoft.Office.Interop.PowerPoint.Application;
using Logger;


namespace Bot.Activity.Files
{
    [ToolboxBitmap("Resources/FileCreate.png")]
    [Designer(typeof(File_Create_ActivityDesigner))]
    public class File_Create : BaseNativeActivity
    {
        [Category("Input Paramaters")]
        [DisplayName("File Path")]
        [Description("Set File Path")]
        [RequiredArgument]
        public InArgument<string> FilePath { get; set; }

        [Category("Common Parameters")]
        [DisplayName("Is Override")]
        public bool IsOverride { get; set; }

        [Category("Input Paramaters")]
        [DisplayName("File Extensions")]
        [DefaultValue("txt")]
        public FileExtension ExtensionType { get; set; }
        public File_Create()
        {
            IsOverride = false;
        }
        protected override void Execute(NativeActivityContext context)
        {
            try
            {
                string filepathWithoutExtension = FilePath.Get(context);
                string filepath = string.Empty;

                if (filepathWithoutExtension.Contains("."))
                {
                    string[] splitArry = filepathWithoutExtension.Split('.');
                    filepath = splitArry[0];
                }
                else
                {
                    filepath = filepathWithoutExtension;
                }
                filepath = string.Concat(filepath, ".");
                filepath = String.Concat(filepath, ExtensionType);

                if (File.Exists(filepath))
                {
                    if (true == IsOverride)
                    {
                        createFile(filepath);
                    }
                    else
                    {
                        Log.Logger.LogData("The file " + filepath + " is already exists in activity File_Create", LogLevel.Error);
                    }
                }
                else
                {
                    createFile(filepath);
                }

            }
            catch (Exception ex)
            {
                Log.Logger.LogData(ex.Message + " in activity File_Create", LogLevel.Error);
                if (!ContinueOnError) { context.Abort(); }
            }

        }

        public void createFile(string filepath)
        {

            if (filepath.EndsWith(".xlsx"))
            {
                Microsoft.Office.Interop.Excel.Application ExcelApp = new Microsoft.Office.Interop.Excel.Application();
                Workbook ExcelWorkBook = null;
                ExcelApp.Visible = false;
                ExcelApp.DisplayAlerts = false;

                ExcelWorkBook = ExcelApp.Workbooks.Add(XlWBATemplate.xlWBATWorksheet);
                ExcelWorkBook.SaveAs(filepath);
                ExcelWorkBook.Close();
                ExcelApp.Quit();
            }
            else if (filepath.EndsWith(".pptx"))
            {
                Application pptApplication = new Application();
                // Create the Presentation File
                Presentation pptPresentation = pptApplication.Presentations.Add(MsoTriState.msoTrue);
                Microsoft.Office.Interop.PowerPoint.CustomLayout customLayout = pptPresentation.SlideMaster.CustomLayouts[Microsoft.Office.Interop.PowerPoint.PpSlideLayout.ppLayoutText];

                Microsoft.Office.Interop.PowerPoint.Slides slides = pptPresentation.Slides;
                Microsoft.Office.Interop.PowerPoint.Slide slide = slides.AddSlide(1, customLayout);
                pptPresentation.SaveAs(filepath, Microsoft.Office.Interop.PowerPoint.PpSaveAsFileType.ppSaveAsDefault, MsoTriState.msoTrue);
                pptPresentation.Close();
                pptApplication.Quit();
               
            }
            else
            {
                File.Create(filepath).Close();
            }
        }
        public enum FileExtension
        {
            txt,
            pptx,
            xlsx,
            docx
        }
    }
}
