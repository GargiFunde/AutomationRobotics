using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AddCopyright
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //foreach (ListViewItem item in listView1.Items)
            //{
            //    string path = (string)item.Text;
            //    AddCopyright(path);
            //}
            AddCopyright(@"C:\Work\BOTDesignerMaster\ActivityLibraries\MeetupActivityLibrary");
        }


        public void AddCopyright(string path)
        {
            string[] files = GetFile(path);
            foreach (string file in files)
            {
                string tempFile = Path.GetFullPath(file) + ".tmp";

                using (StreamReader reader = new StreamReader(file))
                {
                    using (StreamWriter writer = new StreamWriter(tempFile))
                    {
                        writer.WriteLine(@"// <copyright file=" + Path.GetFileNameWithoutExtension(file) + @" company=E2E BOTS>
// Copyright (c) 2018 All Rights Reserved
// </copyright>
// <author>Prabodhini Funde</author>
// <date> " + DateTime.Now + @"</date>
// <summary></summary>
");

                        string line = string.Empty;
                        while ((line = reader.ReadLine()) != null)
                        {
                            writer.WriteLine(line);
                        }
                    }
                }
                File.Delete(file);
                File.Move(tempFile, file);
            }
        }

        public void DeleteCopyright(string path)
        {
           
            string[] files = GetFile(path);
            foreach (string file in files)
            {
                string tempFile = Path.GetFullPath(file) + ".tmp";

                using (StreamReader reader = new StreamReader(file))
                {
                    using (StreamWriter writer = new StreamWriter(tempFile))
                    {

                        int i = 0;
                        string line = string.Empty;
                        while ((line = reader.ReadLine()) != null)
                        {
                            i = i + 1;
                            if (i > 6)
                            {
                                writer.WriteLine(line);
                            }
                        }
                    }
                }
                File.Delete(file);
                File.Move(tempFile, file);
            }
        }
        public string[] GetFile(string path)
        {
            string[] files = null;
            string folderpath = path ; //@"C:\Work"
            string filter = "*.cs";

             return files = Directory.GetFiles(folderpath, filter, SearchOption.AllDirectories);
            //return files = Directory.GetFiles(folderpath, filter, SearchOption.TopDirectoryOnly);

        }

        private void button2_Click(object sender, EventArgs e)
        {
            //foreach (var item in listView1.Items)
            //{
            //    string path = (string)item;
            //   // DeleteCopyright(path);
            //}
            ////DeleteCopyright(@"C:\Work\BOTDesignerMaster\Windows");
        }
    }
}
