using Bot.Activity.Files;
using Logger;
using Microsoft.Office.Interop.Word;
using System;
using System.Activities;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using Application = Microsoft.Office.Interop.Word.Application;

namespace ReadText
{
    [Designer(typeof(ActivityDesignerforreadfile))]
    [ToolboxBitmap("Resources/ReadText.png")]
    public class Read_Text : BaseNativeActivity
    {
        [Category("Input Paramaters")]
        [DisplayName("File Path")]
        [Description("Set File Path")]
        [RequiredArgument]
        public InArgument<string> FilePath { get; set; }

        [Category("Input Paramaters")]
        [DisplayName("File Type")]
        [Description("File Type")]
        [RequiredArgument]
        public FileTypeEnum FileType { get; set; }

        [Category("Output Paramaters")]
        [DisplayName("Output")]
        [Description("Set output Variable")]
        public OutArgument<string> FileContent { get; set; }

        string doctext = null;

        protected override void Execute(NativeActivityContext context)
        {
            try
            {
                string fileName = FilePath.Get(context);
                string filetype = FileType.ToString();
                //string fileName = FilePath.ToString();
                FileInfo fi = new FileInfo(fileName);
                string extn = fi.Extension;

               
                    if (extn.Equals(".doc") || extn.Equals(".docx"))
                    {
                        string doctext = ReadDoctext(fileName);
                        FileContent.Set(context, doctext);

                    }
                    else
                    {
                        if (!fileName.Contains("."))
                        {
                            fileName = fileName + ".txt";
                        }
                        if (File.Exists(fileName))
                        {
                            string text = File.ReadAllText(fileName);

                            FileContent.Set(context, text);


                        }
                    }
               

               

            }
            catch (Exception ex)
            {

                Log.Logger.LogData(ex.Message , LogLevel.Error);
            }

            
          
        }

        public string ReadDoctext(string file)
        {
            try
            {
                Application word = new Application();
                word.ScreenUpdating = false;
                word.Visible = false;
               
                Document doc = new Document();
              
                object fileName = file;
                object missing = System.Type.Missing;

                doc = word.Documents.Open(ref fileName,
             ref missing, ref missing, ref missing, ref missing,
             ref missing, ref missing, ref missing, ref missing,
             ref missing, ref missing, ref missing, ref missing,
             ref missing, ref missing, ref missing);

                //doc = AC.Documents.Open(ref filename, ref missing, ref readOnly, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref isVisible, ref isVisible, ref missing, ref missing, ref missing);
                doctext = doc.Content.Text;
                doc.Close();

            }
            catch (Exception ex)
            {
                Log.Logger.LogData(ex.Message, LogLevel.Error);

            }
            return doctext;
            

        }
    }

  


    public enum FileTypeEnum
    {
        txt = 0,
        doc = 1,
        docx = 2,
        py = 3
       
        
    }

}
