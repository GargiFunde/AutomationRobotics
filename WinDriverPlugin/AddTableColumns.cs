using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Activities;
using System.ComponentModel;
using CommonLibrary;
using Logger;
using System.Data;
using OpenQA.Selenium.Appium.Windows;
using OpenQA.Selenium.Appium;
using System.Collections.ObjectModel;
using OpenQA.Selenium;
using System.Drawing;

namespace Bot.Activity.WinDriverPlugin
{
    [ToolboxBitmap("Resources/WinDriverAddTableColumns.png")]
    [Designer(typeof(WinDriver_AddTableColumns_ActivityDesigner))]
    public class WinDriver_AddTableColumns : BaseNativeActivity
    {
        [RequiredArgument]
        public InArgument<string> ApplicationIDToAttach { get; set; }
        public InArgument<string> TableColumnXPath { get; set; }
        public InArgument<int> TimeOutInSecond { get; set; }
        public OutArgument<DataTable> Result { get; set; }

        // public bool IsHeader { get; set; }

        protected override void Execute(NativeActivityContext context)
        {
            string AppId = ApplicationIDToAttach.Get(context);
            string tableColumnXPath = TableColumnXPath.Get(context);
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
                                catch (Exception)
                                {
                                    dt.Columns.Add(item.Text + "_Repeated");
                                }
                            }
                            else
                            {
                                dt.Columns.Add();
                            }
                        }
                        Result.Set(context, dt);
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
        private static void FillInvisibleRows(WindowsElement row)
        {
            row.Click();
            row.SendKeys(Keys.ArrowDown);

        }
        private static void AddRows(WindowsInstance windowObj, int Col_count, ref DataTable dt, string tableRowXPath, int iCurrentRowCount)
        {
           
        }
    }
}
