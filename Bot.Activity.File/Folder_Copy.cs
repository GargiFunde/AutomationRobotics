using System;
using System.Activities;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using Logger;

namespace Bot.Activity.Files
{
    [ToolboxBitmap("Resources/FolderCopy.png")]
    [Designer(typeof(Folder_Copy_ActivityDesigner))]
    public class Folder_Copy : BaseNativeActivity
    {
    
        [RequiredArgument]
        [Category("Input Parameters")]
        [DefaultValue("System.String")]
        [DisplayName("Source Folder Path")]
        [Description("Source Folder Path")]
        public InArgument<string> FolderPath { get; set; }
        [RequiredArgument]
        [Category("Input Parameters")]
        [DefaultValue("System.String")]
        [DisplayName("Destination Folder Path")]
        [Description("Destination Folder Path")]
        public InArgument<string> CopyPath { get; set; }

        [RequiredArgument]
        [Category("Common Parameters")]
        [DefaultValue("System.Bool")]
        [DisplayName("Is Override")]
        [Description("Overwrites Existing Folder")]
        public bool isOverride { get; set; }
        protected override void Execute(NativeActivityContext context)
        {
            try
            {
                string SourcePath = FolderPath.Get(context);
                string DestinationPath = CopyPath.Get(context);
                string DestFolder = Path.GetFileName(SourcePath);
                DestinationPath = DestinationPath + "\\" + DestFolder;


                if (Directory.Exists(SourcePath))
                {
                    if (isOverride == true && Directory.Exists(DestinationPath))
                    {
                        Directory.Delete(DestinationPath, true);
                    }
                    else if (isOverride == false && Directory.Exists(DestinationPath))
                    {
                        throw new Exception("File with the same name already exist - " + DestinationPath);
                    }

                    if (!Directory.Exists(DestinationPath))
                    {
                        Directory.CreateDirectory(DestinationPath);
                    }



                    //Now Create all of the directories
                    foreach (string dirPath in Directory.GetDirectories(SourcePath, "*",
                        SearchOption.AllDirectories))
                        Directory.CreateDirectory(dirPath.Replace(SourcePath, DestinationPath));

                    //Copy all the files & Replaces any files with the same name
                    foreach (string newPath in Directory.GetFiles(SourcePath, "*.*",
                        SearchOption.AllDirectories))
                        File.Copy(newPath, newPath.Replace(SourcePath, DestinationPath), true);

                }
                else if (!Directory.Exists(SourcePath))
                {

                    Log.Logger.LogData("Folder does not exist in path - " + SourcePath + " in activity Folder_Copy", LogLevel.Error);
                    if (!ContinueOnError) { context.Abort(); }
                }


            }
            catch (Exception ex)
            {
                Log.Logger.LogData(ex.Message + " in activity Folder_Copy", LogLevel.Error);
                if (!ContinueOnError) { context.Abort(); }
            }

        }
    }
}
