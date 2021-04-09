using Logger;
using System;
using System.Activities;
using System.ComponentModel;
using System.Drawing;
using System.IO;

// Ionic.Zip package , Autor:Dino Chiesa ,https://www.nuget.org/packages/Ionic.Zip/1.9.1.8
using IZ = Ionic.Zip;

/* This Project is for only Internal Purpose not be used with the Product*/

namespace Zip
{
    [ToolboxBitmap("Resources/File_ConvertToZip.png")]
    [Designer(typeof(File_ConvertToZip_ActivityDesigner))]
    public class File_ConvertToZip : BaseNativeActivity
    {
        [Category("Input Paramaters")]
        [DisplayName("File Path")]
        [Description("Set File Path")]
        [RequiredArgument]
        public InArgument<string[]> FilePath { get; set; }

        [Category("Input Paramaters")]
        [DisplayName("Destination Folder Path")]
        [Description("Set Destination Folder Path")]
        [RequiredArgument]
        public InArgument<string> DestinationFolderPath { get; set; }

        [Category("Input Paramaters")]
        [DisplayName("Zip File Name")]
        [Description("Set Zip File Name")]
        [RequiredArgument]
        public InArgument<string> ZipFileName { get; set; }

        [Category("Input Paramaters")]
        [DisplayName("Password")]
        [Description("Set Password")]
        [RequiredArgument]
        public InArgument<string> Password { get; set; }

        [Category("Output Paramaters")]
        [DisplayName("Result")]
        [Description("Get Result")]
        public OutArgument<bool> Result { get; set; }

        [Category("Input Paramaters")]
        public bool DirectoryStruct { get; set; }
        public File_ConvertToZip()
        {
            DirectoryStruct = true;
        }
        protected override void Execute(NativeActivityContext context)
        {
            try
            {
                string[] inputFilePaths = FilePath.Get(context);
                string destinationFolderPath = DestinationFolderPath.Get(context);
                string zipFileName = ZipFileName.Get(context);
                string password = Password.Get(context);

                if (Directory.Exists(destinationFolderPath))
                {
                    zipFileName = string.Concat(zipFileName, ".zip");
                    string zipFile = Path.Combine(destinationFolderPath, zipFileName);

                    using (var zip = new IZ.ZipFile(System.Text.Encoding.Default))
                    {
                        zip.AlternateEncodingUsage = IZ.ZipOption.AsNecessary;
                        zip.Password = password;

                        foreach (string path in inputFilePaths)
                        {
                            string fileName = Path.GetFileName(path);
                            if (Directory.Exists(path))
                            {
                                if (DirectoryStruct)
                                {
                                    zip.UpdateDirectory(path, fileName);
                                }
                                else
                                {
                                    zip.UpdateDirectory(path, "");
                                }

                            }

                            if (File.Exists(path))
                            {
                                zip.UpdateFile(path, "");
                            }
                            Result.Set(context, true);
                        }
                        zip.Save(zipFile);
                    }
                }
                else
                {
                    Log.Logger.LogData("Directory \"" + destinationFolderPath + "\" does not exist in activity File_ConvertToZip", LogLevel.Error);
                    if (!ContinueOnError) { context.Abort(); }
                }
            }
            catch (Exception ex)
            {
                Result.Set(context, false);
                Log.Logger.LogData(ex.Message + " in activity File_ConvertToZip", LogLevel.Error);
                if (!ContinueOnError) { context.Abort(); }
            }
        }
    }
}
