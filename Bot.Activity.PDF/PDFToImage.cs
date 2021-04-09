using System;
using System.Activities;
using Spire.Pdf;
using System.Drawing;
using System.Drawing.Imaging;
using System.ComponentModel;

namespace Bot.Activity.PDF
{
    [ToolboxBitmap("Resources/PDFToImage.png")]
    [Designer(typeof(ActivityDesignerForPDFToImage))]
    public class PDFToImage : BaseNativeActivity
    {
        [RequiredArgument]
        public InArgument<string> FilePath { get; set; }
        [RequiredArgument]
        public InArgument<string> OutputPath { get; set; }
        [RequiredArgument]
        public FormatType Format { get; set; }
        
        [RequiredArgument]
        public InArgument<string> PDFName { get; set; }


        protected override void Execute(NativeActivityContext context)
        {
            string filePath = FilePath.Get(context).Trim();
            string outputFilePath = OutputPath.Get(context).Trim();
            // string format = "." + Format.Get(context);
            string PdfName = PDFName.Get(context);

            try
            {
                ImageFormat imageFormat = ImageFormat.Png;
                string extension = null;
                switch (Format)
                {
                    case FormatType.jpeg:
                        imageFormat = ImageFormat.Jpeg;
                        extension = ".jpeg";
                        break;
                    case FormatType.png:
                        imageFormat = ImageFormat.Png;
                        extension = ".png";
                        break;
                    default:
                        imageFormat = ImageFormat.Png;
                        extension = ".png";
                        break;
                }


                if (String.IsNullOrEmpty(filePath))
                {


                }
                else
                {
                    PdfDocument document = new PdfDocument();
                    document.LoadFromFile(filePath);
                    int total = document.Pages.Count;
                    string pdfsave = string.Empty;
                    for (int i = 0; i < total; ++i)
                    {
                        pdfsave = outputFilePath + PdfName + "-" + i + extension;
                        pdfsave.Trim();
                        Image image = document.SaveAsImage(i);

                        image.Save(pdfsave, imageFormat);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Log.Logger.LogData("Exception while converting PDF: " + ex.Message, Logger.LogLevel.Error);

            }
        }

        public enum FormatType
        {
            jpeg,
            png,
        }
    }
}
