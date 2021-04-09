using Logger;
using System;
using System.Activities;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using DrawingImage = System.Drawing.Image;
using DrawingImaging = System.Drawing.Imaging;

namespace Bot.Activity.Image
{
    [ToolboxBitmap("Resources/ImageResize.png")]
    [Designer(typeof(Image_Resize_ActivityDesigner))]
    public class Image_Resize : BaseNativeActivity
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

        public Image_Resize()
        {
            this.DisplayName = "Image Resize";
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
                        Resize(sourcePath, width, height, destinationFolderPath, imageNameAfterResize);
                    }
                    else
                    {
                        Log.Logger.LogData("Image does not exist:\"" + sourcePath + "\" in activity Image_Resize", LogLevel.Error);
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
                            Resize(item, width, height, destinationFolderPath, imageNameAfterResize);
                        }
                    }
                    else
                    {
                        Log.Logger.LogData("Folder does not exist:\"" + sourcePath + "\" in activity Image_Resize", LogLevel.Error);
                        if (!ContinueOnError) { context.Abort(); }
                    }

                   
                }
              
            }
            catch (Exception ex)
            {
                Log.Logger.LogData(ex.Message + " in activity Image_Resize", LogLevel.Error);
                if (!ContinueOnError) { context.Abort(); }
            }
            
        }

        public static void Resize(string srcPath, int width, int height,string destinationFolderPath,string imageNameAfterResize)
        {
            string destinationImagePath = string.Empty;
            DrawingImage image = DrawingImage.FromFile(srcPath);
            Bitmap resultImage = Resize(image, width, height);

            if (false == Directory.Exists(destinationFolderPath))
            {
                Directory.CreateDirectory(destinationFolderPath);
            }
            if (string.IsNullOrEmpty(imageNameAfterResize))
            {
                destinationImagePath = Path.Combine(destinationFolderPath, Path.GetFileName(srcPath));
            }
            else
            {
               String newImageName= string.Concat(imageNameAfterResize, Path.GetExtension(srcPath));
                destinationImagePath = Path.Combine(destinationFolderPath, newImageName);
            }

            if (Path.GetExtension(srcPath).Equals(".jpg") || Path.GetExtension(srcPath).Equals(".JPG"))
            {
                resultImage.Save(destinationImagePath, DrawingImaging.ImageFormat.Jpeg);
            }
            if (Path.GetExtension(srcPath).Equals(".png") || Path.GetExtension(srcPath).Equals(".PNG"))
            {
                resultImage.Save(destinationImagePath, DrawingImaging.ImageFormat.Png);
            }
        }

        public static Bitmap Resize(DrawingImage image, int width, int height)
        {

            var destRect = new Rectangle(0, 0, width, height);
            var destImage = new Bitmap(width, height);

            destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

            using (var graphics = Graphics.FromImage(destImage))
            {
                graphics.CompositingMode = CompositingMode.SourceCopy;
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

                using (var wrapMode = new DrawingImaging.ImageAttributes())
                {
                    wrapMode.SetWrapMode(WrapMode.TileFlipXY);
                    graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
                }
            }

            return destImage;
        }

        public enum InputType
        {
            Image = 0,
            Folder = 1
        }
    }
}
