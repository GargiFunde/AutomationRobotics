using System;
using System.Activities;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Spire.Pdf;
using System.Drawing;
using System.Drawing.Imaging;
using System.ComponentModel;

namespace PDFToImage
{
    [Designer(typeof(ActivityDesigner1))]
   public class ConvertPdfToImage : BaseNativeActivity
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
                    for (int i = 0; i < total; ++i)
                    {
                        PdfName = outputFilePath+ PdfName + "-" + i+extension;
                        PdfName.Trim();
                        Image image = document.SaveAsImage(i);
                        
                        image.Save(PdfName, imageFormat);
                    }
                }
            }
            catch (Exception ex)
            {
               
                
            }
           
            
            
            }



 
        
    }



    public enum FormatType { 
    
        jpeg,
        png,
       
    
    
    } 
}
