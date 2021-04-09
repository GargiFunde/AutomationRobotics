// <copyright file=Form1 company=E2E BOTS>
// Copyright (c) 2018 All Rights Reserved
// </copyright>
// <author>Prabodhini Funde</author>
// <date> 03-10-2018 16:02:55</date>
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

namespace EncryptionDecryption
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            SecurityEncryptHelper.SetPassword = "123456789";
            textBox2.Text = SecurityEncryptHelper.GetSecurityEncryptHelper().EncryptText(textBox1.Text);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            SecurityEncryptHelper.SetPassword = "123456789";
            textBox1.Text = SecurityEncryptHelper.GetSecurityEncryptHelper().DecryptText(textBox2.Text);
        }
    }
}
