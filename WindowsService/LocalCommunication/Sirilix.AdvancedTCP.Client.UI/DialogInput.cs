// <copyright file=DialogInput company=E2E Robotics>
// Copyright (c) 2018 All Rights Reserved
// </copyright>
// <author>Saurabh Mundhe</author>
// <date> 03-10-2018 16:03:06</date>
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

namespace TCP.Client.UI
{
    public partial class DialogInput : Form
    {
        public String Result { get; set; }

        public DialogInput(String message)
        {
            InitializeComponent();
            lbMessage.Text = message;

            btnOK.Click += btnOK_Click;
            btnCancel.Click += btnCancel_Click;
        }

        void btnCancel_Click(object sender, EventArgs e)
        {
            DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Close();
        }

        void btnOK_Click(object sender, EventArgs e)
        {
            Result = txtResult.Text;
            DialogResult = System.Windows.Forms.DialogResult.OK;
            this.Close();
        }
    }
}
