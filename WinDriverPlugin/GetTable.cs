//using System;
//using System.Activities;
//using System.ComponentModel;
//using CommonLibrary;
//using System.Data;
//using OpenQA.Selenium.Appium.Windows;
//using System.Collections.ObjectModel;

//namespace Bot.Activity.WinDriverPlugin
//{
//    [Designer(typeof(WindowsControlProperties1))]
//    public class WinDriver_GetTable : BaseNativeActivity
//    {
//        WindowsElement tableResult = null;
//        string AppId = string.Empty;
//        string tableRowXPath = string.Empty;
//        string tableColumnXPath = string.Empty;
//        WindowsInstance windowObj = null;
//        int timeInSec = 0;

//        [RequiredArgument]
//        [Category("Input")]
//        [DisplayName("Application ID")]
//        [Description("Set Application ID To Attach")]
//        public InArgument<string> ApplicationIDToAttach { get; set; }

//        [Category("Input")]
//        [DisplayName("TableHeader-XPath")]
//        [Description("Set TableHeaderXPath")]
//        public InArgument<string> TableHeaderXPath { get; set; }

//        [Category("Input")]
//        [DisplayName("TableRow-XPath")]
//        [Description("Set TableRowXPath")]
//        public InArgument<string> TableRowXPath { get; set; }

//        [Category("Input")]
//        [DisplayName("Time Out In Second")]
//        [Description("Set Time Out In Second")]
//        public InArgument<int> TimeOutInSecond { get; set; }  //Not Yet Used

//        [Category("Output")]
//        [DisplayName("Table Result")]
//        [Description("Set Table Result")]
//        public OutArgument<DataTable> TableResult { get; set; }

//        // public bool IsHeader { get; set; }

//        WinDriver_GetTable()
//        {
//            tableResult = null;
//             AppId = string.Empty;
//             tableRowXPath = string.Empty;
//             tableColumnXPath = string.Empty;
//             windowObj = null;
//             timeInSec = 0;
//        }

//        protected override void Execute(NativeActivityContext context)
//        {
//            this.AppId = ApplicationIDToAttach.Get(context);
//            this.tableRowXPath = TableRowXPath.Get(context);
//            this.tableColumnXPath = TableHeaderXPath.Get(context);
//            this.timeInSec = TimeOutInSecond.Get(context);

//            try
//            {
//                if (String.IsNullOrEmpty(AppId)) //scraping time
//                {
//                    if (SelectHelper.CurrentRuntimeApplicationHelper.RuntimeApplicationObjects.ContainsKey(AppId))
//                    {
//                        windowObj = (WindowsInstance)SelectHelper.CurrentRuntimeApplicationHelper.RuntimeApplicationObjects[AppId];

//                        ReadOnlyCollection<WindowsElement> collectionColumn = windowObj._driver.FindElementsByXPath(tableColumnXPath);// ("//HeaderItem");
//                        ReadOnlyCollection<WindowsElement> collectionRow = windowObj._driver.FindElementsByXPath(tableRowXPath);// ("//DataItem");
//                        int Col_count = collectionColumn.Count;
//                        int Row_count = collectionRow.Count;

//                        DataTable dt = new DataTable();
//                        //if (IsHeader == true)
//                        //{
//                        foreach (WindowsElement item in collectionColumn)
//                        {
//                            if (item.Text != null)
//                            {
//                                try
//                                {
//                                    String Table_data = item.Text;
//                                    dt.Columns.Add(item.Text);
//                                }
//                                catch (Exception ex)
//                                {
//                                    dt.Columns.Add(item.Text + "_Repeated");
//                                }
//                            }
//                            else
//                            {
//                                dt.Columns.Add();
//                            }
//                        }
//                        //  }
//                        WindowsElement row = AddRows(collectionRow, Col_count, Row_count, dt);


//                        TableResult.Set(context, dt);
//                    }
//                }
//            }
//            catch (Exception ex)
//            {
//                Logger.Log.Logger.LogData(ex.Message + " in activity GetTable", Logger.LogLevel.Error);

//                if (!ContinueOnError)
//                {
//                    context.Abort();
//                }
//            }
//        }

//        private static WindowsElement AddRows(ReadOnlyCollection<WindowsElement> collectionRow, int Col_count, int Row_count, DataTable dt)
//        {
//            int iRowCountInternal = 0;
//            WindowsElement row = null;
//            for (int iRow = 0; iRow < Row_count; iRow = iRow + Col_count)
//            {
//                DataRow dr = dt.NewRow();
//                for (int i = 0; i < Col_count; i++)
//                {

//                    if (iRowCountInternal <= Row_count)
//                    {
//                        row = collectionRow[iRow];
//                        dr[i] = row.Text;
//                    }
//                    else
//                    {
//                        break;
//                    }

//                }
//                //row.Click();
//               // string sNmae = row.GetAttribute("Name");
//              ///  row.SendKeys(Keys.ArrowDown);
//                dt.Rows.Add(dr);

//            }

//            return row;
//        }
//    }
//}


using System;
using System.Activities;
using System.ComponentModel;
using CommonLibrary;
using System.Data;
using OpenQA.Selenium.Appium.Windows;
using System.Collections.ObjectModel;
using System.Drawing;

namespace Bot.Activity.WinDriverPlugin
{
    [ToolboxBitmap("Resources/WinDriverGetTable.png")]
    [Designer(typeof(WinDriver_GetTable_ActivityDesigner))]
    public class WinDriver_GetTable : BaseNativeActivity
    {
        [RequiredArgument]
        [Category("Input")]
        [DisplayName("Application ID")]
        [Description("Set Application ID To Attach")]
        public InArgument<string> ApplicationIDToAttach { get; set; }
        [Category("Input")]
        [DisplayName("TableHeader-XPath")]
        [Description("Set TableHeaderXPath")]
        public InArgument<string> TableHeaderXPath { get; set; }

        [Category("Input")]
        [DisplayName("TableRow-XPath")]
        [Description("Set TableRowXPath")]
        public InArgument<string> TableRowXPath { get; set; }

        [Category("Input")]
        [DisplayName("Time Out In Second")]
        [Description("Set Time Out In Second")]
        public InArgument<int> TimeOutInSecond { get; set; }  //Not Yet Used

        [Category("Output")]
        [DisplayName("Table Result")]
        [Description("Set Table Result")]
        public OutArgument<DataTable> TableResult { get; set; }


        protected override void Execute(NativeActivityContext context)
        {
            WindowsElement tableResult = null;
            string AppId = ApplicationIDToAttach.Get(context);
            string tableRowXPath = TableRowXPath.Get(context);
            string tableColumnXPath = TableHeaderXPath.Get(context);
            WindowsInstance windowObj = null;
            int timeInSec = TimeOutInSecond.Get(context);
            try
            {
                if (AppId != string.Empty) //scraping time
                {
                    if (SelectHelper.CurrentRuntimeApplicationHelper.RuntimeApplicationObjects.ContainsKey(AppId))
                    {
                        windowObj = (WindowsInstance)SelectHelper.CurrentRuntimeApplicationHelper.RuntimeApplicationObjects[AppId];

                        ReadOnlyCollection<WindowsElement> collectionColumn = windowObj._driver.FindElementsByXPath(tableColumnXPath);// ("//HeaderItem");
                        ReadOnlyCollection<WindowsElement> collectionRow = windowObj._driver.FindElementsByXPath(tableRowXPath);// ("//DataItem");
                        int Col_count = collectionColumn.Count;
                        int Row_count = collectionRow.Count;

                        DataTable dt = new DataTable();
                        foreach (WindowsElement item in collectionColumn)
                        {
                            if (item.Text != null)
                            {
                                try
                                {
                                    String Table_data = item.Text;
                                    dt.Columns.Add(item.Text);
                                }
                                catch (Exception )
                                {
                                    dt.Columns.Add(item.Text + "_Repeated");
                                }
                            }
                            else
                            {
                                dt.Columns.Add();
                            }
                        }
                        WindowsElement row = AddRows(collectionRow, Col_count, Row_count, dt);

                        TableResult.Set(context, dt);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Log.Logger.LogData(ex.Message + " in activity GetTable", Logger.LogLevel.Error);

                if (!ContinueOnError)
                {
                    context.Abort();
                }
            }
        }



        private static WindowsElement AddRows(ReadOnlyCollection<WindowsElement> collectionRow, int Col_count, int Row_count, DataTable dt)
        {
            int iRowCountInternal = 0;
            WindowsElement row = null;
            for (int iRow = 0; iRow < Row_count; iRow = iRow + Col_count)
            {
                DataRow dr = dt.NewRow();
                for (int i = 0; i < Col_count; i++)
                {
                    if (i <= Row_count)                        //Will be carried till rowcount == 9
                    {
                        row = collectionRow[iRowCountInternal];
                        dr[i] = row.Text;
                        iRowCountInternal++;
                    }
                    else if (i == Row_count)
                    {
                        dt.Rows.Add(dr);                       // If row count ==9 add 
                        i = 0;
                    }
                    //else
                    //{
                    //    break;
                    //}
                    //Comment By Piyush
                    //if (iRowCountInternal <= Row_count)
                    //{
                    //    //row = collectionRow[iRow];      //Comment By Piyush
                    //    row = collectionRow[i];
                    //    dr[i] = row.Text;
                    //}
                    //else
                    //{
                    //    break;
                    //}



                }
                //row.Click();
                // string sNmae = row.GetAttribute("Name");
                ///  row.SendKeys(Keys.ArrowDown);
                dt.Rows.Add(dr);

            }

            return row;
        }
    }
}
