// <copyright file=SaveFields.xaml company=E2E BOTS>
// Copyright (c) 2018 All Rights Reserved
// </copyright>
// <author>Prabodhini Funde</author>
// <date> 03-10-2018 16:19:13</date>
// <summary></summary>

using CommonLibrary;
using System;
using System.Activities;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Windows;
using System.Windows.Controls;

namespace Bot.Activity.ActivityLibrary
{
    /// <summary>
    /// Interaction logic for SaveFields.xaml
    /// </summary>
    public partial class SaveFields : UserControl
    {
        public double X { get; set; }
        public double Y { get; set; }

        public SaveFields()
        {
            InitializeComponent();
            SnippingTool.AreaSelected -= SnippingTool_AreaSelected;
            SnippingTool.AreaSelected += SnippingTool_AreaSelected;
            EventManager.RegisterClassHandler(typeof(Button), MouseDownEvent, new RoutedEventHandler(OnMouseDown));
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
            try
            {
                var location = element.PointToScreen(new System.Windows.Point(0, 0));
                X = location.X;
                Y = location.Y;
            }catch(Exception)
            {

            }
        }
        public void RemoveButton_Click(object Sender, EventArgs evt)
        {
            try
            {
                System.Activities.Activity ae = (System.Activities.Activity)lblUniqueControlld.DataContext;
                SaveFieldEventArgs saveFieldEventArgs = new SaveFieldEventArgs();
                saveFieldEventArgs.activity = ae;
                saveFieldEventArgs.iSaveFieldOperation = 2; //2 = remove
                SelectHelper.OnSaveFieldOperations(saveFieldEventArgs);
            } catch(Exception)
            {

            }
        }

        public void ShowPropertyDetails_Click(object Sender, EventArgs evt)
        {

            System.Activities.Activity ae = (System.Activities.Activity)lblUniqueControlld.DataContext;
            SaveFieldEventArgs saveFieldEventArgs = new SaveFieldEventArgs();
            saveFieldEventArgs.activity = ae;
            saveFieldEventArgs.iSaveFieldOperation = 1; //1 = details
            SelectHelper.OnSaveFieldOperations(saveFieldEventArgs);
        }

        public void CaptureFieldImage_Click(object Sender, EventArgs evt)
        {
            Bitmap printscreen = new Bitmap(System.Windows.Forms.Screen.PrimaryScreen.Bounds.Width, System.Windows.Forms.Screen.PrimaryScreen.Bounds.Height, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            Graphics graphics = Graphics.FromImage(printscreen);
            graphics.CopyFromScreen(0, 0, 0, 0, printscreen.Size);
            SnippingTool.ImageBMP = printscreen;
            using (MemoryStream s = new MemoryStream())
            {
                //save graphic variable into memory
                printscreen.Save(s, ImageFormat.Bmp);
                SnippingTool sn = new SnippingTool(System.Drawing.Image.FromStream(s), 0, 0, printscreen.Size.Width, printscreen.Size.Height);
                //set the picture box with temporary stream
                SnippingTool.UniqueId = lblUniqueControlld.Content.ToString();
                SnippingTool.Snip();
            }
        }
        public void RetrieveFieldImage_Click(object Sender, EventArgs evt)
        {
            string ScreenPath = string.Empty;
            if ((lblImagePath.Content != null )&&(lblImagePath.Content is System.Activities.InArgument<string>))
            {
                System.Activities.InArgument<string> strval = (System.Activities.InArgument<string>) lblImagePath.Content;
                if(strval.Expression != null)
                {
                    ScreenPath = SelectHelper.ProjectLocation + "\\Images\\" + strval.Expression.ToString();
                    if ((ScreenPath != null) && (ScreenPath != string.Empty))
                    {
                        if (File.Exists(ScreenPath))
                        {
                            Bitmap bmp = new Bitmap(ScreenPath);
                            Form2 form2 = new Form2();
                            form2.Text = "";
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
                    }
                }
            }        
        }
        private void SnippingTool_AreaSelected(object sender, EventArgs e)
        {
            if (SnippingTool.UniqueId == lblUniqueControlld.Content.ToString())
            {
                if (!string.IsNullOrEmpty(SelectHelper.ProjectLocation))
                {
                    string ScreenPath = SelectHelper.ProjectLocation + "\\Images\\" + lblUniqueControlld.Content.ToString() + ".png";
                    var bmp = SnippingTool.Image;
                    bmp.Save(ScreenPath, ImageFormat.Bmp);
                    lblImagePath.Content = lblUniqueControlld.Content.ToString() + ".png"; //Path should be dynamic based on deployed folder location and it should not static
                }
            }
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}
