using Logger;
using System;
using System.Activities;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using WordToPDF;

namespace Word
{
    [ToolboxBitmap("Resources/wordtopdf.png")]
    [Designer(typeof(Word_To_PDF_ActivityDesigner))]
    public class Word_To_PDF : BaseNativeActivity
    {
        [Category("Input Paramaters")]
        [DisplayName("File Path")]
        [Description("Set File Path")]
        [RequiredArgument]
        public InArgument<string> FilePath { get; set; }

        [Category("Input Paramaters")]
        [DisplayName("Output Folder Path")]
        [Description("Set Folder Path")]
        //[RequiredArgument]
        public InArgument<string> OutputFilePath { get; set; }

        protected override void Execute(NativeActivityContext context)
        {
            try
            {
                Word2Pdf objWorPdf = new Word2Pdf();

                string filepathtoconvert = FilePath.Get(context);
                string OutputFilePathafterConvert = OutputFilePath.Get(context);


                string strFileName = Path.GetFileName(filepathtoconvert);


                object FromLocation = filepathtoconvert;
                string FileExtension = Path.GetExtension(strFileName);
                string ChangeExtension = strFileName.Replace(FileExtension, ".pdf");
                object ToLocation = null;
                if (FileExtension == ".doc" || FileExtension == ".docx")
                {
                    if (OutputFilePathafterConvert != null)
                    {
                        ToLocation = OutputFilePathafterConvert + "\\" + ChangeExtension;
                    }
                    else
                    {
                        string SaveInSameFolder = Path.GetDirectoryName(filepathtoconvert);
                        ToLocation = SaveInSameFolder + "\\" + ChangeExtension;
                    }

                    objWorPdf.InputLocation = FromLocation;
                    objWorPdf.OutputLocation = ToLocation;
                    objWorPdf.Word2PdfCOnversion();
                }
            }
            catch (Exception ex)
            {
                Log.Logger.LogData("Check a File Path and File name in activity Word_To_PDF", LogLevel.Info);

            }

        }
    }
}
