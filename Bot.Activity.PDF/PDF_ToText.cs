using System.Activities;
using System.ComponentModel;
using System;
using Logger;
using org.apache.pdfbox.pdmodel;
using org.apache.pdfbox.util;
using System.Drawing;

namespace Bot.Activity.PDF
{
    [ToolboxBitmap("Resources/PDFToText.png")]
    [Designer(typeof(ActivityDesignerForPDF_ToText))]
    public class PDF_ToText : BaseNativeActivity
    {
        [RequiredArgument]
        public InArgument<string>PDFFilePath { get; set; }
        public OutArgument<string> PDFFileText { get; set; }
        protected override void Execute(NativeActivityContext context)
        {
            try
            { 
                string pdfFilePath = PDFFilePath.Get(context);
                if (!String.IsNullOrEmpty (pdfFilePath))
                {
                   string output = parseUsingPDFBox(pdfFilePath);
                    if (!String.IsNullOrEmpty(pdfFilePath))
                    {
                        PDFFileText.Set(context, output);
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Logger.LogData(ex.Message + " in activity PDF_ToText", LogLevel.Error);
                if (!ContinueOnError)
                {
                    context.Abort();
                }
            }
        }
        private static string parseUsingPDFBox(string input)
        {
            PDDocument doc = null;

            try
            {
                doc = PDDocument.load(input);
                PDFTextStripper stripper = new PDFTextStripper();
                return stripper.getText(doc);
            }
            finally
            {
                if (doc != null)
                {
                    doc.close();
                }
            }
        }
    }
}
