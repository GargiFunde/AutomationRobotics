// <copyright file=ActivityDesigner1.xaml company=E2E BOTS>
// Copyright (c) 2018 All Rights Reserved
// </copyright>
// <author>Prabodhini Funde</author>
// <date> 03-10-2018 16:00:31</date>
// <summary></summary>


using Microsoft.Win32;
using System;
using System.Activities;
using System.Activities.Statements;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
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


namespace Bot.Activity.Excel
{
    /// <summary>
    /// Interaction logic for AttachApplicationActivitiesDesigner.xaml
    /// </summary>
    public partial class ReadExcelDesigner
    {
        
        public ReadExcelDesigner()
        {
            InitializeComponent();
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
       
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Excel_ReadFile owa = null;
            ExcelData exceldata = null;
            owa = (Excel_ReadFile)this.ModelItem.GetCurrentValue();

            string workSheetName = owa.WorksheetName.Expression.ToString();
            string strFilePath = owa.FilePath.Expression.ToString();
            string workbookName = owa.FilePath.Expression.ToString();
            //string strFilePath = owa.FilePath;
            //string workbookName = owa.FilePath;
            bool bIsHeader = owa.IsHeader;
            owa.ReadExcelData(workSheetName, strFilePath, workbookName, bIsHeader);
                        
            if (owa.dt != null)
            {
                if (exceldata == null)
                {
                    exceldata = new ExcelData();
                }
               
                exceldata.SetData(owa.dt);
                exceldata.ShowDialog();
            }
                      
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Microsoft.Win32.OpenFileDialog fileDialog = new Microsoft.Win32.OpenFileDialog();
            if (fileDialog.ShowDialog() == true)
            {
                //this.ModelItem.Properties["FilePath"].SetValue(fileDialog.FileName);
                Excel_ReadFile owa = null;
                owa = (Excel_ReadFile)this.ModelItem.GetCurrentValue();
                owa.FilePath.Expression = fileDialog.FileName;
            }
        }
    }
}
