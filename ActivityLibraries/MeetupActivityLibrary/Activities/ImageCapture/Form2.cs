// <copyright file=Form2 company=E2E BOTS>
// Copyright (c) 2018 All Rights Reserved
// </copyright>
// <author>Prabodhini Funde</author>
// <date> 03-10-2018 16:19:14</date>
// <summary></summary>

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Bot.Activity.ActivityLibrary
{
    public partial class Form2 : Form
    {
       // public Bitmap ControlImage;
       // public Bitmap ControlImage { get; set; }

        public Bitmap ControlImage
        {
            //get
            //{
            //    return id;
            //}
            set
            {
                pictureBox1.Image = value;
            }
        }
        public Form2()
        {
            InitializeComponent();
        }
        private void Form2_Load(object sender, EventArgs e)
        {
            //pictureBox1.Image = ControlImage;
        }

        private void Form2_FormClosed(object sender, FormClosedEventArgs e)
        {
           // ControlImage.Dispose();
        }
    }
}
