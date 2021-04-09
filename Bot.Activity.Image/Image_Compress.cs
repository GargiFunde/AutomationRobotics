using Logger;
using System;
using System.Activities;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using DrawingImage = System.Drawing.Image;
using DrawingImaging = System.Drawing.Imaging;

namespace Bot.Activity.Image
{
    [ToolboxBitmap("Resources/ImageResize.png")]
    [Designer(typeof(Image_Compress_ActivityDesigner))]
    public class Image_Compress : BaseNativeActivity
    {
        [Category("Input Paramaters")]
        [DisplayName("Source Path")]
        [Description("Input type is Image, then sets image path and Input type is Folder, then sets the folder path")]
        [RequiredArgument]
        public InArgument<string> SourcePath { get; set; }

        [Category("Input Paramaters")]
        [DisplayName("Dimensions Height")]
        [Description("Set Height")]
        [RequiredArgument]
        public InArgument<int> Height { get; set; }

        [Category("Input Paramaters")]
        [DisplayName("Dimensions Width")]
        [Description("Set Width")]
        [RequiredArgument]
        public InArgument<int> Width { get; set; }

        [Category("Input Paramaters")]
        [DisplayName("Destination Folder Path")]
        [Description("Set Destination Folder Path")]
        [RequiredArgument]
        public InArgument<String> DestinationFolderPath { get; set; }

        [Category("Input Paramaters")]
        [DisplayName("Image Name After Resize")]
        [Description("Set Image Name After Resize")]
        public InArgument<String> ImageNameAfterResize { get; set; }

        [Category("Input Paramaters")]
        [DisplayName("Input Type")]
        public InputType inputType { get; set; }

        public Image_Compress()
        {
            this.DisplayName = "Image Compress";
        }
        protected override void Execute(NativeActivityContext context)
        {

            try
            {
                string sourcePath = SourcePath.Get(context);
                int height = Height.Get(context);
                int width = Width.Get(context);
                string destinationFolderPath = DestinationFolderPath.Get(context);
                string imageNameAfterResize = ImageNameAfterResize.Get(context);

                if (inputType == InputType.Image)
                {
                    if (File.Exists(sourcePath))
                    {
                        CompressImage(sourcePath, width, height, destinationFolderPath, imageNameAfterResize);
                    }
                    else
                    {
                        Log.Logger.LogData("Image does not exist:\"" + sourcePath + "\" in activity Image_Compress", LogLevel.Error);
                        if (!ContinueOnError) { context.Abort(); }
                    }

                }

                if (inputType == InputType.Folder)
                {
                    if (Directory.Exists(sourcePath))
                    {
                        string[] filePaths = Directory.GetFiles(sourcePath);

                        foreach (var item in filePaths)
                        {
                            CompressImage(sourcePath, width, height, destinationFolderPath, imageNameAfterResize);
                        }
                    }
                    else
                    {
                        Log.Logger.LogData("Folder does not exist:\"" + sourcePath + "\" in activity Image_Compress", LogLevel.Error);
                        if (!ContinueOnError) { context.Abort(); }
                    }


                }

            }
            catch (Exception ex)
            {
                Log.Logger.LogData(ex.Message + " in activity Image_Compress", LogLevel.Error);
                if (!ContinueOnError) { context.Abort(); }
            }

        }

        public static void CompressImage(string inputPath, int width, int height, string destinationFolderPath,string imageNameAfterResize)
        {
         
            string destinationImagePath = string.Empty;

            if (false == Directory.Exists(destinationFolderPath))
            {
                Directory.CreateDirectory(destinationFolderPath);
            }
            if (string.IsNullOrEmpty(imageNameAfterResize))
            {
                destinationImagePath = Path.Combine(destinationFolderPath, Path.GetFileName(inputPath));
            }
            else
            {
                String newImageName = string.Concat(imageNameAfterResize, Path.GetExtension(inputPath));
                destinationImagePath = Path.Combine(destinationFolderPath, newImageName);
            }
            const int quality = 75;
            using (var image = new Bitmap(DrawingImage.FromFile(inputPath)))
            {
                //string outputPath = @"C:\Work\E2EBot\NewBirthdayBot\Input\Templates\Images\EmplyeeImage";

                //outputPath = Path.Combine(outputPath, Path.GetFileName(inputPath));

                //if (image.Width > image.Height)
                //{
                //    width = size;
                //    height = Convert.ToInt32(image.Height * size / (double)image.Width);
                //}
                //else
                //{
                //    width = Convert.ToInt32(image.Width * size / (double)image.Height);
                //    height = size;
                //}

                var resized = new Bitmap(width, height);
                using (var graphics = Graphics.FromImage(resized))
                {
                    graphics.CompositingQuality = CompositingQuality.HighSpeed;
                    graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                    graphics.CompositingMode = CompositingMode.SourceCopy;
                    graphics.DrawImage(image, 0, 0, width, height);
                    // using (var output = File.Open(outputPath, FileMode.Create))
                    using (var output = resized)
                    {
                        var qualityParamId = DrawingImaging.Encoder.Quality;
                        var encoderParameters = new DrawingImaging.EncoderParameters(1);
                        encoderParameters.Param[0] = new DrawingImaging.EncoderParameter(qualityParamId, quality);

                        if (Path.GetExtension(inputPath).Equals(".jpg") || Path.GetExtension(inputPath).Equals(".JPG") || Path.GetExtension(inputPath).Equals(".jpeg") || Path.GetExtension(inputPath).Equals(".JPEG"))
                        {
                            var codecImag = DrawingImaging.ImageCodecInfo.GetImageDecoders().FirstOrDefault(codec => codec.FormatID == DrawingImaging.ImageFormat.Jpeg.Guid);
                            resized.Save(destinationImagePath, DrawingImaging.ImageFormat.Jpeg);
                        }
                        if (Path.GetExtension(inputPath).Equals(".png") || Path.GetExtension(inputPath).Equals(".PNG"))
                        {
                            var codecImag = DrawingImaging.ImageCodecInfo.GetImageDecoders().FirstOrDefault(codec => codec.FormatID == DrawingImaging.ImageFormat.Png.Guid);
                            resized.Save(destinationImagePath, DrawingImaging.ImageFormat.Png);
                        }
                    }
                }
            }
        }

        public enum InputType
        {
            Image = 0,
            Folder = 1
        }
    }
}
