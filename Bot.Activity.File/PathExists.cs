using Logger;
using System;
using System.Activities;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bot.Activity.Files
{
    [Designer(typeof(PathExist_ActivityDesigner))]
    [ToolboxBitmap("Resources/PathExists.png")]
    public class PathExist : BaseNativeActivity
    {
        [Category("Input")]
        [DisplayName("Path")]
        [Description("File path to check")]
        [RequiredArgument]
        public InArgument<string> path { get; set; }

        [Category("Output")]
        [DisplayName("Exist")]
        [Description("File path to check")]
        [RequiredArgument]
        public OutArgument<Boolean> Exist { get; set; }

        [Category("Input")]
        [DisplayName("Filetype")]
        [Description("")]
        [RequiredArgument]
        public FileType Ft { get; set; }

        public enum FileType
        {
            File = 0, Folder = 1
        }


        protected override void Execute(NativeActivityContext context)
        {

            string Res = @path.Get(context);


            try
            {
                if (Res != null)
                {


                    switch (Ft)
                    {
                        case FileType.File:
                            Exist.Set(context, File.Exists(Res) ? true : false);
                            break;

                        case FileType.Folder:
                            Exist.Set(context, Directory.Exists(Res) ? true : false);
                            break;

                    }



                }
            }

            catch (NullReferenceException e)
            {
                Logger.Log.Logger.LogData("NullReferenceException  " + e.Message, LogLevel.Error);
            }
            catch (Exception e)
            {
                Logger.Log.Logger.LogData("Exception  " + e.Message, LogLevel.Error);
            }



        }
    }
}
