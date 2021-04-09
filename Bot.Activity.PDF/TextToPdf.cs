///-----------------------------------------------------------------
///   Namespace:      Bot.Activity.PDF
///   Class:               TextToPdf
///   Description:    <para>Convert Image To PDF </para>
///   Author:           E2E BOTS Team                   Date: 02 March,2020
///   Notes:            Convert Text To PDF 
///   Revision History: 2020.1.2.3
///   Name:           Date:        Description:
///-----------------------------------------------------------------


using System;
using System.Activities;
using System.ComponentModel;
using System.IO;
using Spire.Pdf;
using Spire.Pdf.Graphics;
using System.Drawing;
using Logger;

namespace Bot.Activity.PDF
{

    [ToolboxBitmap("Resources/TextToPdf.png")]
    [Designer(typeof(ActivityDesignerForTextToPdf))]
    public class TextToPdf : BaseNativeActivity
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
                string sourcepath = FilePath.Get(context);
                string destinationpdfFileName = OutputFilePath.Get(context);

                string text = File.ReadAllText(sourcepath);

                PdfDocument doc = new PdfDocument();

                //PdfSection section = doc.Sections.Add();

                PdfPageBase page = doc.Pages.Add();

                PdfFont font = new PdfFont(PdfFontFamily.Helvetica, 11);

                PdfStringFormat format = new PdfStringFormat();

                format.LineSpacing = 20f;

                PdfBrush brush = PdfBrushes.Black;

                PdfTextWidget textWidget = new PdfTextWidget(text, font, brush);

                float y = 0;

                PdfTextLayout textLayout = new PdfTextLayout();

                textLayout.Break = PdfLayoutBreakType.FitPage;

                textLayout.Layout = PdfLayoutType.Paginate;

                RectangleF bounds = new RectangleF(new PointF(0, y), page.Canvas.ClientSize);

                textWidget.StringFormat = format;

                textWidget.Draw(page, bounds, textLayout);



                if (destinationpdfFileName == null)
                {

                    string destpath = Path.ChangeExtension(sourcepath, ".pdf");
                    doc.SaveToFile(destpath, FileFormat.PDF);
                }
                else
                {
                    doc.SaveToFile(destinationpdfFileName, FileFormat.PDF);
                }

            }
            catch (Exception ex)
            {

                Log.Logger.LogData("Check a File Path and File name ", LogLevel.Error);
            }

        }
    }
}
