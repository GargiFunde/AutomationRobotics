// <copyright file=CaptureImage1.xaml company=E2E BOTS>
// Copyright (c) 2018 All Rights Reserved
// </copyright>
// <author>Prabodhini Funde</author>
// <date> 03-10-2018 16:19:14</date>
// <summary></summary>

using CommonLibrary;
//using RehostedWorkflowDesigner.ImageCapture;
using System;
using System.Activities;
using System.Activities.Statements;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Workflow.Activities;

namespace Bot.Activity.ActivityLibrary.Activities
{
    /// <summary>
    /// Interaction logic for AttachApplicationActivitiesDesigner.xaml
    /// </summary>
    public partial class CaptureImage1
    {
        public double X { get; set; }
        public double Y { get; set; }
        double height = 0;
        double width = 0;
        Window window = null;
        public CaptureImage1()
        {
            InitializeComponent();
            SnippingTool.AreaSelected -= SnippingTool_AreaSelected;
            SnippingTool.AreaSelected += SnippingTool_AreaSelected;
            EventManager.RegisterClassHandler(typeof(Button), MouseDownEvent, new RoutedEventHandler(OnMouseDown));


        }
        Image_Capture owa = null;
        private void ActivityDesigner_Loaded(object sender, RoutedEventArgs e)
        {
            window = Window.GetWindow(this);
            //height = window.Height;
            //width = window.Width;
            owa = (Image_Capture)this.ModelItem.GetCurrentValue();
            if ((owa.ImagePath != null) && (!string.IsNullOrEmpty(owa.ImagePath.Expression.ToString())))
            {
                img.Source = new BitmapImage(new Uri(owa.ImagePath.Expression.ToString(), UriKind.RelativeOrAbsolute));
            }
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {

            // Find the window that contains the control


            // Minimize
           
            window.WindowState = WindowState.Minimized;
            height = window.Height;
            width = window.Width;
            window.Height = 0;
            window.Width = 0;
            Thread.Sleep(1000);
            //  owa = (Image_Capture)this.ModelItem.GetCurrentValue();
            string _imageId = Guid.NewGuid().ToString();
            owa.ImageId = _imageId;
            Bitmap printscreen = new Bitmap(System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width, System.Windows.Forms.Screen.PrimaryScreen.Bounds.Height, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            Graphics graphics = Graphics.FromImage(printscreen);
            graphics.CopyFromScreen(0, 0, 0, 0, printscreen.Size);
            SnippingTool.ImageBMP = printscreen;
            try
            {
                using (MemoryStream s = new MemoryStream())
                {   
                    //save graphic variable into memory
                    printscreen.Save(s, ImageFormat.Bmp);
                    SnippingTool sn = new SnippingTool(System.Drawing.Image.FromStream(s), 0, 0, printscreen.Size.Width, printscreen.Size.Height);
                    //set the picture box with temporary stream
                    SnippingTool.UniqueId = _imageId;
                    SnippingTool.Snip();
                }
            }
            catch (Exception)
            {

            }
            //window.Height = height;
            //window.Width =width;
            //window.WindowState = WindowState.Normal;
        }
        private void OnMouseDown(object sender, RoutedEventArgs e)
        {
            var element = sender as ContentControl;
            if (element != null)
            {
                ShowLocation(element);
            }
        }
        private void ShowLocation(ContentControl element)
        {
            var location = element.PointToScreen(new System.Windows.Point(0, 0));
            X = location.X;
            Y = location.Y;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            // Find the window that contains the control

            // Minimize
            height = window.Height;
            width = window.Width;
            window.WindowState = WindowState.Minimized;
           

            window.Height = 0;
            window.Width = 0;
            Thread.Sleep(1000);
            Bitmap bmp = null;
          //Image_Capture owa = null;
           // owa = (Image_Capture)this.ModelItem.GetCurrentValue();
            if (owa.Type == "Design Time")
            {
               
                string processDirectory = SelectHelper.ProjectLocation +"\\Images";
                if (!Directory.Exists(processDirectory))
                {
                    Directory.CreateDirectory(processDirectory);
                }
                string ScreenPath = processDirectory + "\\" + owa.ImageId + ".png";

                if ((ScreenPath != null) && (ScreenPath != string.Empty))
                {
                    bmp = new Bitmap(ScreenPath);
                    
                }
            }
            else
            {
                int x =Convert.ToInt32(owa.Left.Expression.ToString());
                int y = Convert.ToInt32(owa.Top.Expression.ToString());
                int width1 = Convert.ToInt32(owa.Width.Expression.ToString());
                int height1 = Convert.ToInt32(owa.Height.Expression.ToString());

                System.Drawing.Rectangle rectangle = new System.Drawing.Rectangle(x, y, width1, height1);
                bmp = RunTime_GetBitmapFromScreenRect(rectangle);

            }
            if (bmp != null)
            {
                Form2 form2 = new Form2();
                form2.ControlImage = bmp;

                form2.Width = bmp.Width + 20;
                form2.Height = bmp.Height + 50;
                int width = (int)X - form2.Width - 50;
                int Top = (int)Y;
                if ((X - form2.Width - 50) > 0)
                    form2.Location = new System.Drawing.Point(width, Top);
                else
                    form2.Location = new System.Drawing.Point(250, 150);

                form2.ShowDialog();
            }
            window.Height = height;
            window.Width = width;
            window.WindowState = WindowState.Normal;
        }
        private Bitmap RunTime_GetBitmapFromScreenRect(System.Drawing.Rectangle r)
        {
            var srcBitmap = new Bitmap(r.Width, r.Height, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            Graphics.FromImage(srcBitmap).CopyFromScreen(r.Location, System.Drawing.Point.Empty, srcBitmap.Size, CopyPixelOperation.SourceCopy);
            return srcBitmap;
        }
        private void SnippingTool_AreaSelected(object sender, EventArgs e)
        {
            //Image_Capture owa = null;
            if (owa == null)
            {
                owa = (Image_Capture)this.ModelItem.GetCurrentValue();
            }
            if (SnippingTool.UniqueId == owa.ImageId)
            {
                string processDirectory = SelectHelper.ProjectLocation +  "\\Images";
                if (!Directory.Exists(processDirectory))
                {
                    Directory.CreateDirectory(processDirectory);
                }
                string ScreenPath = processDirectory + "\\" + owa.ImageId + ".png";
                var bmp = SnippingTool.Image;
                owa.Width = SnippingTool.Image.Width;
                owa.Height = SnippingTool.Image.Height;
                owa.Top = SnippingTool.Top;
                owa.Left = SnippingTool.Left;
                bmp.Save(ScreenPath, ImageFormat.Bmp);
                img.Source = new BitmapImage(new Uri(ScreenPath, UriKind.RelativeOrAbsolute));
                owa.ImagePath = ScreenPath;
                
            }
            window.Height = height;
            window.Width = width;
            if(window.WindowState == WindowState.Minimized)
            window.WindowState = WindowState.Normal;
           
        }

      
    }
}
