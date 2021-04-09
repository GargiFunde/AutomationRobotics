// <copyright file=CommonTreeView.xaml company=E2E BOTS>
// Copyright (c) 2018 All Rights Reserved
// </copyright>
// <author>Prabodhini Funde</author>
// <date> 03-10-2018 16:02:53</date>
// <summary></summary>

using CommonLibrary;
using Logger;
using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace BOTDesigner.Views
{
    /// <summary>
    /// Interaction logic for CommonTreeView.xaml
    /// </summary>
    public partial class CommonTreeView : UserControl
    {
        public CommonTreeView()
        {
            InitializeComponent();
        }
        private object dummyNode = null;
        public string SelectedImagePath { get; set; }

        private string projectOrlibraryPath = string.Empty;
        public string ProjectOrLibraryPath
        {
            get
            {
                return this.projectOrlibraryPath;
            }

            set
            {
                if (value != this.projectOrlibraryPath)
                {
                    this.projectOrlibraryPath = value;
                    LoadTreeView();
                    // NotifyPropertyChanged("LibraryPath");
                }
            }
        }
        public void Expand()
        {
            foldersItem.Style = (Style)this.FindResource("TV_AllExpanded");

        }
        public void Collapse()
        {
            foldersItem.Style = (Style)this.FindResource("TV_AllCollapsed");
        }

        public void LoadTreeView()
        {
            try
            {
                foreach (string s in Directory.GetDirectories(ProjectOrLibraryPath))
                {
                    TreeViewItemExtended item = new TreeViewItemExtended();
                    item.FolderIcon = @"pack://application:,,,/Images/folder.png";
                    item.Header = s.Substring(s.LastIndexOf("\\") + 1);
                    item.Tag = s;
                    item.FontWeight = FontWeights.Normal;
                    item.Items.Add(dummyNode);
                    item.Expanded += new RoutedEventHandler(folder_Expanded);

                    foldersItem.Items.Add(item);
                }
                foreach (string s in Directory.GetFiles(ProjectOrLibraryPath))
                {
                    string extension = s.Substring(s.LastIndexOf("."));
                    if (extension.ToLower() == ".xaml")
                    {
                        TreeViewItemExtended item = new TreeViewItemExtended();
                        item.FolderIcon = @"pack://application:,,,/Images/file.png";
                        item.Header = s.Substring(s.LastIndexOf("\\") + 1);
                        item.Tag = s;
                        item.FontWeight = FontWeights.Normal;
                        //  item.Items.Add(dummyNode);

                        foldersItem.Items.Add(item);
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Logger.LogData(ex.Message, LogLevel.Error);
            }
        }
        public void AddingNewItem(string strDir, string strFileName, ItemCollection treeviewItem)
        {
            bool lastnode = true;

            foreach (TreeViewItemExtended s in treeviewItem)
            {
                if(s.Tag.ToString().Contains(strDir)) //if root folder
                {
                    if(Directory.Exists(s.Tag.ToString()))
                    {
                        if (s.Tag.ToString()==strDir)
                        {
                            AddItem(strFileName, s);
                            break;
                        }
                    }
                    else if(s.Tag.ToString().Contains(".xaml"))
                    {
                       string dirName =  Path.GetDirectoryName(s.Tag.ToString());
                        if (s.Tag.ToString() == strDir)
                        {
                            AddItem(strFileName, s);
                            break;
                        }
                    }
                }
                else if ((s.Tag != null) && (!(s.Tag.ToString().Contains(".xaml"))))
                {
                    AddingNewItem(strDir, strFileName, s);
                    lastnode = false;
                    
                }

            }
            if (lastnode)
            {
                TreeViewItemExtended item = new TreeViewItemExtended();
                item.FolderIcon = @"pack://application:,,,/Images/file.png";
                item.Header = strFileName;
                item.Tag = strDir;
                item.FontWeight = FontWeights.Normal;
                treeviewItem.Add(item);
                return;
            }
        }
        public void AddingNewItem(string strDir, string strFileName, TreeViewItemExtended treeItem)
        {

            foreach (TreeViewItemExtended s in treeItem.Items)
            {
                if ((s != null) && (s.Tag != null))
                {
                    if (strDir.Contains(s.Tag.ToString()))
                    {
                        if (strDir.ToLower() == s.Tag.ToString().ToLower())
                        {
                            AddItem(strFileName, s);
                            return;
                        }
                        else
                        {
                            AddingNewItem(strDir, strFileName, treeItem);
                        }
                    }
                }

            }
        }

        private static void AddItem(string strFileName, TreeViewItemExtended s)
        {
            TreeViewItemExtended item = new TreeViewItemExtended();
            item.FolderIcon = @"pack://application:,,,/Images/file.png";
            item.Header = strFileName;
            item.Tag = s;
            item.FontWeight = FontWeights.Normal;
            s.Items.Add(item);
        }

        void folder_Expanded(object sender, RoutedEventArgs e)
        {
            TreeViewItemExtended item = (TreeViewItemExtended)sender;
            if (item.Items.Count == 1 && item.Items[0] == dummyNode)
            {
                item.Items.Clear();
                try
                {
                    foreach (string s in Directory.GetDirectories(item.Tag.ToString()))
                    {
                        TreeViewItemExtended subitem = new TreeViewItemExtended();
                        item.FolderIcon =
                        subitem.FolderIcon = @"pack://application:,,,/Images/folder.png";
                        subitem.Header = s.Substring(s.LastIndexOf("\\") + 1);
                        subitem.Tag = s;
                        subitem.FontWeight = FontWeights.Normal;
                        subitem.Items.Add(dummyNode);
                        subitem.Expanded += new RoutedEventHandler(folder_Expanded);
                        item.Items.Add(subitem);
                    }
                    foreach (string s in Directory.GetFiles(item.Tag.ToString()))
                    {
                        string extension = s.Substring(s.LastIndexOf("."));
                        if (extension.ToLower() == ".xaml")
                        {
                            TreeViewItemExtended itemFile = new TreeViewItemExtended();
                            itemFile.Header = s.Substring(s.LastIndexOf("\\") + 1);
                            itemFile.FolderIcon = @"pack://application:,,,/Images/file.png";
                            itemFile.Tag = s;
                            itemFile.FontWeight = FontWeights.Normal;
                            //itemFile.Items.Add(dummyNode);

                            item.Items.Add(itemFile);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Log.Logger.LogData(ex.Message, LogLevel.Error);
                }
            }
        }
        //private void foldersItem_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        //{
        //    TreeView tree = (TreeView)sender;
        //    TreeViewItemExtended temp = ((TreeViewItemExtended)tree.SelectedItem);

        //    if (temp == null)
        //        return;
        //    SelectedImagePath = "";
        //    string temp1 = "";
        //    string temp2 = "";
        //    while (true)
        //    {
        //        temp1 = temp.Header.ToString();
        //        if (temp1.Contains(@"\"))
        //        {
        //            temp2 = "";
        //        }
        //        SelectedImagePath = temp1 + temp2 + SelectedImagePath;
        //        if (temp.Parent.GetType().Equals(typeof(TreeView)))
        //        {
        //            break;
        //        }
        //        temp = ((TreeViewItemExtended)temp.Parent);
        //        temp2 = @"\";
        //    }
        //    //show user selected path
        //    string FullNameWithPath = ProjectOrLibraryPath + "\\" + SelectedImagePath;
        //    //MessageBox.Show(ProjectOrLibraryPath + "\\" +  SelectedImagePath);
        //    OpenXamlFileEventArgs openXaml = new OpenXamlFileEventArgs();
        //    openXaml.XamlFileName = SelectedImagePath;
        //    openXaml.XamlFileNameWithPath = FullNameWithPath;
        //    SelectHelper.OnOpenXamlFile(openXaml);
        //    //CustomWfDesigner.NewInstanceCSharp(FullNameWithPath);
        //}

        
        private void StackPanel_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            Mouse.OverrideCursor = Cursors.Wait;
            try
            {
                TreeViewItemExtended temp = null;
                StackPanel sp = (StackPanel)sender;
                if (sp.Tag == null)
                    return;
                if (sp.Tag is TreeViewItemExtended)
                {
                    temp = (TreeViewItemExtended)sp.Tag;
                    if (temp == null)
                        return;
                    SelectedImagePath = "";
                    string temp1 = "";
                    string temp2 = "";
                    while (true)
                    {
                        temp1 = temp.Header.ToString();
                        if (temp1.Contains(@"\"))
                        {
                            temp2 = "";
                        }
                        SelectedImagePath = temp1 + temp2 + SelectedImagePath;
                        if (temp.Parent.GetType().Equals(typeof(TreeView)))
                        {
                            break;
                        }
                        temp = ((TreeViewItemExtended)temp.Parent);
                        temp2 = @"\";
                    }
                    //show user selected path
                    string FullNameWithPath = ProjectOrLibraryPath + "\\" + SelectedImagePath;
                    //MessageBox.Show(ProjectOrLibraryPath + "\\" +  SelectedImagePath);
                    OpenXamlFileEventArgs openXaml = new OpenXamlFileEventArgs();
                    openXaml.XamlFileName = SelectedImagePath;
                    openXaml.XamlFileNameWithPath = FullNameWithPath;
                    SelectHelper.OnOpenXamlFile(openXaml);
                }
            }
            catch (Exception ex)
            {
                Log.Logger.LogData(ex.Message, LogLevel.Error);
            }
            finally
            {
               Mouse.OverrideCursor = null;
            }

        }
        private void foldersItem_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            try
            {
                TreeView tree = (TreeView)sender;
                TreeViewItemExtended temp = ((TreeViewItemExtended)tree.SelectedItem);

                if (temp == null)
                    return;
                SelectedImagePath = "";
                string temp1 = "";
                string temp2 = "";
                while (true)
                {
                    temp1 = temp.Header.ToString();
                    if (temp1.Contains(@"\"))
                    {
                        temp2 = "";
                    }
                    SelectedImagePath = temp1 + temp2 + SelectedImagePath;
                    if (temp.Parent.GetType().Equals(typeof(TreeView)))
                    {
                        break;
                    }
                    temp = ((TreeViewItemExtended)temp.Parent);
                    temp2 = @"\";
                }
                //show user selected path
                string FullNameWithPath = ProjectOrLibraryPath + "\\" + SelectedImagePath;
                //MessageBox.Show(ProjectOrLibraryPath + "\\" +  SelectedImagePath);
                OpenXamlFileEventArgs openXaml = new OpenXamlFileEventArgs();
                openXaml.XamlFileName = SelectedImagePath;
                openXaml.XamlFileNameWithPath = FullNameWithPath;
                SelectHelper.OnOpenXamlFile(openXaml);
                //CustomWfDesigner.NewInstanceCSharp(FullNameWithPath);
            }
            catch (Exception ex)
            {
                Log.Logger.LogData(ex.Message, LogLevel.Error);
            }
        }
    }
}
