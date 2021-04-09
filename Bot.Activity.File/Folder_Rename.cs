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
    [ToolboxBitmap("Resources/FolderRename.png")]
    [Designer(typeof(Folder_Rename_ActivityDesigner))]
    public class Folder_Rename : BaseNativeActivity
    {
        //[RequiredArgument]
        //[Category("Input Parameters")]
        //[DefaultValue("System.String")]
        //[DisplayName("Current Folder Name")]
        //[Description("The Folder To Be Renamed")]
        //public InArgument<string> FolderName { get; set; }
        [RequiredArgument]
        [Category("Input Parameters")]
        [DefaultValue("System.String")]
        [DisplayName("Folder Path")]
        [Description("Provide Folder Path")]
        public InArgument<string> FolderPath { get; set; }
        [RequiredArgument]
        [Category("Input Parameters")]
        [DefaultValue("System.String")]
        [DisplayName("New Folder Name")]
        [Description("New Name For The Folder")]
        public InArgument<string> NewFolderName { get; set; }
        [RequiredArgument]
        [Category("Common Parameters")]
        [DefaultValue("System.Bool")]
        [DisplayName("Is Override")]
        [Description("Overrides Existing Folder")]
        public bool isOverride { get; set; }
        protected override void Execute(NativeActivityContext context)
        {
            //throw new NotImplementedException();
            try
            {

                string path = FolderPath.Get(context);
                string newname = NewFolderName.Get(context);
                //string oldname = FolderName.Get(context);
                //int len = oldname.Length;

                // string filepath = Path.GetDirectoryName(path);

                if (Directory.Exists(path))
                {

                    // string newpath = path.Remove(path.Length - len, len);
                    string dirpath = Path.GetDirectoryName(path);
                    string newpath = dirpath + "\\" + newname;



                    if (isOverride == true)
                    {
                        if (Directory.Exists(newpath) && !(path.Equals(newpath)))
                        {

                            Directory.Delete(newpath, true);

                        }

                    }

                    Directory.Move(path, newpath);


                }
                else 
                {
                    Log.Logger.LogData("Folder does not exist in path - " + path + " in activity Folder_Rename", LogLevel.Error);
                    if (!ContinueOnError) { context.Abort(); }


                }






            }
            catch (Exception ex)
            {
                Log.Logger.LogData(ex.Message + " in activity Folder_Rename", LogLevel.Error);
                if (!ContinueOnError) { context.Abort(); }
            }
        }
    }
}
