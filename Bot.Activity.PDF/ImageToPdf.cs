///-----------------------------------------------------------------
///   Namespace:      Bot.Activity.PDF
///   Class:               ImageToPdf
///   Description:    <para>Convert Image To PDF </para>
///   Author:           E2E BOTS Team                   Date: 02 March,2020
///   Notes:            Convert Image To PDF 
///   Revision History: 2020.1.2.3
///   Name:           Date:        Description:
///-----------------------------------------------------------------


using Logger;
using PdfSharp.Drawing;
using PdfSharp.Pdf;
using System;
using System.Activities;
using System.ComponentModel;
using System.Drawing;
using System.IO;

namespace Bot.Activity.PDF
{
    [ToolboxBitmap("Resources/ImageToPdf.png")]
    [Designer(typeof(ActivityDesignerForImageToPdf))]
    public class ImageToPdf : BaseNativeActivity
    {
        [Category("Input Paramaters")]
        [DisplayName("File Path")]
        [Description("Set File Path with file name and extension of file")]
        [RequiredArgument]
        public InArgument<string> FilePath { get; set; }

        [Category("Input Paramaters")]
        [DisplayName("Output file Path")]
        [Description("Set Folder Path")]
        //[RequiredArgument]
        public InArgument<string> OutputFilePath { get; set; }

        protected override void Execute(NativeActivityContext context)
        {

            try
            {

                string imageFileName = FilePath.Get(context);

                /*"C:\\Work\\process\\NewWallpaper.png";*/
                string SetpdfFileName = OutputFilePath.Get(context);

                string pdfFileName = null;
                if (SetpdfFileName != null)
                {
                    string extension = Path.GetExtension(SetpdfFileName);

                    if (!extension.Equals(""))
                    {
                        pdfFileName = SetpdfFileName;
                    }
                    else
                    {
                        string pdfFilepath = SetpdfFileName;
                        pdfFileName = pdfFilepath + ".pdf";
                    }
                }
                else
                {

                    string pdfFileName1 = imageFileName;
                    //string extension = Path.GetExtension(pdfFileName);
                    pdfFileName = Path.ChangeExtension(pdfFileName1, ".pdf");

                    //pdfFileName.Replace(extension, ".pdf");

                }
                /*"C:\\Work\\process\\NewWallpape.pdf";*/
                int width = 600;


                using (var document = new PdfDocument())
                {
                    PdfPage page = document.AddPage();
                    using (XImage img = XImage.FromFile(imageFileName))
                    {
                        // Calculate new height to keep image ratio
                        var height = (int)(((double)width / (double)img.PixelWidth) * img.PixelHeight);

                        // Change PDF Page size to match image
                        page.Width = width;
                        page.Height = height;

                        XGraphics gfx = XGraphics.FromPdfPage(page);
                        gfx.DrawImage(img, 0, 0, width, height);
                    }
                    document.Save(pdfFileName);
                }

            }
            catch (Exception ex)
            {

                Log.Logger.LogData("Check a File Path and File name ", LogLevel.Error);
            }

        }
    }
}
