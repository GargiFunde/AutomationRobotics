///<summary>
///<para>Activity: Get complete Data using X-Path for a Table with all Page Content(Eg: Page 1,Page 2,...,Page n)</para>
///</summary>

using System;
using System.Collections.Generic;
using System.Activities;
using System.ComponentModel;
using CommonLibrary;
using System.Data;
using OpenQA.Selenium;
using System.Drawing;

namespace Bot.Activity.Web
{
    [Designer(typeof(Web_GetTable_ActivityDesigner))]
    [ToolboxBitmap("Resources/WebGetTable.png")]
    public class Web_GetTable : BaseNativeActivity
    {

        [Category("Input Paramaters")]
        [RequiredArgument]
        [DisplayName("Application ID To Attach")]
        public InArgument<string> ApplicationIDToAttach { get; set; }

        [Category("Input Paramaters")]
        [DisplayName("Table Id")]
        public InArgument<string> TableId { get; set; }

        [Category("Input Paramaters")]
        [DisplayName("Table Name")]
        public InArgument<string> TableName { get; set; }

        [Category("Input Paramaters")]
        [DisplayName("Table Class")]
        public InArgument<string> TableClass { get; set; }

        [Category("Input Paramaters")]
        [DisplayName("Table CSS Selector")]
        public InArgument<string> TableCSSSelector { get; set; }

        [Category("Input Paramaters")]
        [Description("Table element to Row XPath")]
        [DisplayName("Table XPath")]
        public InArgument<string> TableXPath { get; set; }

        [Category("Input Paramaters")]
        [DisplayName("Table Row XPath")]
        public InArgument<string> RowXPath { get; set; }

        [Category("Input Paramaters")]
        [Description("Next Button/Arrow XPath")]
        [DisplayName("Next Button/Arrow XPath")]
        public InArgument<string> ArrowXPath { get; set; }

        [Category("Input Paramaters")]
        [Description("Next Button/Arrow CSS Selector")]
        [DisplayName("Next Button/Arrow CSS Selector")]
        public InArgument<string> ArrowCSSSelector { get; set; }

        [Category("Input Paramaters")]
        [DisplayName("Next Button/Arrow Link Text")]
        [Description("Next Button/Arrow Link Text")]
        public InArgument<string> ArrowLinkText { get; set; }

        //[Description("required if there is inner element e.g. td/span")]
        //[DisplayName("TD To Cell XPath")]
        //public InArgument<string> CellXPath { get; set; }

        // [Category("Input Paramaters")]
        //public InArgument<string> SiblingInfo { get; set; }

        //[Category("Input Paramaters")]
        //public InArgument<string> ParentInfo { get; set; }

        //[RequiredArgument]
        //public InArgument<string> TableBody { get; set; }

        //[Category("Input Paramaters")]
        //[DisplayName("Time Out In Second")]
        //public InArgument<int> TimeOutInSecond { get; set; }

        [Category("Output Paramaters")]
        [DisplayName("Data Table")]
        public OutArgument<DataTable> DataTable { get; set; }
        public bool IsHeader { get; set; }
        protected override void Execute(NativeActivityContext context)
        {
            DataTable dt = new DataTable();
            string AppId = ApplicationIDToAttach.Get(context);
            string arrowXPath = ArrowXPath.Get(context);
            string arrowCSSSelector = ArrowCSSSelector.Get(context);
            string arrowLinkText = ArrowLinkText.Get(context);
            try
            {
                if (AppId != string.Empty)
                {
                    if (SelectHelper.CurrentRuntimeApplicationHelper.RuntimeApplicationObjects.ContainsKey(AppId))
                    {

                        CommonWebBrowser commonWebBrowser_Current = (CommonWebBrowser)SelectHelper.CurrentRuntimeApplicationHelper.RuntimeApplicationObjects[AppId];

                        //String siblingInfo = SiblingInfo.Get(context);
                        //string parentInfo = ParentInfo.Get(context);
                        string tableXPath = TableXPath.Get(context);
                        IWebElement arrowElement = null;
                        bool arrowElementexists = false;

                        //********** Arrow xpath is given 
                        if (!string.IsNullOrEmpty(arrowXPath))
                        {
                            dt = AddDataInDataTable(context, dt, AppId);

                            string cssClass = commonWebBrowser_Current.driver.FindElement(By.XPath(arrowXPath)).GetAttribute("Class");

                            while (commonWebBrowser_Current.driver.FindElement(By.XPath(arrowXPath)) != null)
                            {

                                string cssClass2 = commonWebBrowser_Current.driver.FindElement(By.XPath(arrowXPath)).GetAttribute("Class");
                                if (cssClass == cssClass2)
                                {
                                    commonWebBrowser_Current.driver.FindElement(By.XPath(arrowXPath)).Click();
                                    dt = AddDataInDataTable(context, dt, AppId);
                                }
                                else
                                {
                                    break;
                                }

                            }
                        }

                        //********** Arrow properties is not given 
                        if (string.IsNullOrEmpty(arrowCSSSelector) && string.IsNullOrEmpty(arrowLinkText) && string.IsNullOrEmpty(arrowXPath))
                        {
                            dt = AddDataInDataTable(context, dt, AppId);
                        }




                        //********** Arrow CSS Selector is given 
                        if (!string.IsNullOrEmpty(arrowCSSSelector))
                        {
                            dt = AddDataInDataTable(context, dt, AppId);

                            string cssClass = commonWebBrowser_Current.driver.FindElement(By.CssSelector(arrowCSSSelector)).GetAttribute("Class");

                            while (commonWebBrowser_Current.driver.FindElement(By.CssSelector(arrowCSSSelector)) != null)
                            {

                                string cssClass2 = commonWebBrowser_Current.driver.FindElement(By.CssSelector(arrowCSSSelector)).GetAttribute("Class");
                                if (cssClass == cssClass2)
                                {
                                    commonWebBrowser_Current.driver.FindElement(By.CssSelector(arrowCSSSelector)).Click();
                                    dt = AddDataInDataTable(context, dt, AppId);
                                }
                                else
                                {
                                    break;
                                }

                            }

                        }

                        //if ((siblingInfo != string.Empty) && (siblingInfo != null) && (siblingInfo.Trim().Length > 0))
                        //{
                        //    // RemoteWebElement previoussiblingelement = (RemoteWebElement)IEWATIN.driver.FindElementByClassName(sPreviousSiblingClassName);
                        //   IWebElement siblingElement= commonWebBrowser_Current.driver.FindElement(By.Id(siblingInfo));

                        //    //var currentelement5= siblingElement.FindElement(By.XPath("following-sibling::a"));
                        //    string ax = siblingInfo + "//following-sibling::a";
                        //    var currentelement6 = siblingElement.FindElement(By.XPath(ax));
                        //   // var currentelement6 = siblingElement.FindElement(By.XPath("descendant-sibling::a"));
                        //    var currentelement22 = siblingElement.FindElement(By.XPath("following-sibling"));
                        //    var currentelement23 = siblingElement.FindElement(By.XPath("following-siblings"));
                        //    var currentelement7 = siblingElement.FindElement(By.XPath("descendant::a"));
                        //    var currentelement9 = siblingElement.FindElements(By.XPath("descendant::a"));

                        //    var currentelement8 = siblingElement.FindElements(By.XPath("descendant-sibling::a"));
                        //    var currentelement10 = siblingElement.FindElements(By.XPath("following-sibling::a"));


                        //    var currentelement1 = siblingElement.FindElement(By.XPath("preceding-sibling::*[1]"));
                        //    var currentelement2 = siblingElement.FindElement(By.XPath("following-sibling::*[1]"));
                        //    var currentelement3 = siblingElement.FindElements(By.XPath("following-sibling::*[1]"));

                        //    var currentelement = siblingElement.FindElements(By.XPath("preceding-sibling::*[1]"));
                        //    var currentelement4 = siblingElement.FindElements(By.XPath("following::*[1]"));
                        //    string strName = string.Empty;


                        //}





                        //******** Arrow link text is given
                        if (!string.IsNullOrEmpty(arrowLinkText))
                        {
                            dt = AddDataInDataTable(context, dt, AppId);

                            while (commonWebBrowser_Current.driver.FindElement(By.LinkText(arrowLinkText)) != null)
                            {
                                commonWebBrowser_Current.driver.FindElement(By.LinkText(arrowLinkText)).Click();
                                dt = AddDataInDataTable(context, dt, AppId);
                            }
                        }

                    }

                    DataTable.Set(context, dt);

                }
            }
            catch (Exception ex)
            {
                if (ex.HResult == -2146233088)
                {
                    DataTable.Set(context, dt);
                }
                else
                {
                    Logger.Log.Logger.LogData(ex.Message + " in Web activity GetTable", Logger.LogLevel.Error);

                    if (!ContinueOnError)
                    {
                        context.Abort();
                    }
                }




            }
        }

        public DataTable AddDataInDataTable(NativeActivityContext context, DataTable dt, string AppId)
        {

            //  string AppId = ApplicationIDToAttach.Get(context);
            string tableId = TableId.Get(context);
            string tableName = TableName.Get(context);
            string tableClass = TableClass.Get(context);
            string sCSSSelector = TableCSSSelector.Get(context);
            string sXPath = TableXPath.Get(context);
            string sRowXPath = RowXPath.Get(context);
            // string sCellXPath = CellXPath.Get(context);
            string sCellXPath = null;


            //string tableBody = TableBody.Get(context);
            int timeInSec = TimeOutInSecond.Get(context);
            try
            {
                if (AppId != string.Empty) //scraping time
                {
                    if (SelectHelper.CurrentRuntimeApplicationHelper.RuntimeApplicationObjects.ContainsKey(AppId))
                    {
                        IWebElement table = null;
                        // WatiN.Core.IE IEWATIN = (WatiN.Core.IE)SelectHelper.CurrentRuntimeApplicationHelper.RuntimeApplicationObjects[AppId];
                        CommonWebBrowser commonWebBrowser_Current = (CommonWebBrowser)SelectHelper.CurrentRuntimeApplicationHelper.RuntimeApplicationObjects[AppId];
                        // IEWATIN.GoTo(searchUrl)
                        //TableBody tableBody = IEWATIN.Table(tableId).TableBodies[0];


                        if (!string.IsNullOrEmpty(sCSSSelector))
                        {
                            if (sCSSSelector.Contains(" "))
                            {
                                sCSSSelector = "." + sCSSSelector.Replace(" ", ".");
                            }
                            table = commonWebBrowser_Current.driver.FindElement(By.CssSelector(sCSSSelector));
                        }
                        else if (!string.IsNullOrEmpty(tableId))
                        {
                            if (tableId.Contains(" "))
                            {
                                tableId = "." + tableId.Replace(" ", ".");
                                table = commonWebBrowser_Current.driver.FindElement(By.CssSelector(tableId));
                            }
                            else { table = commonWebBrowser_Current.driver.FindElement(By.Id(tableId)); }

                        }
                        else if (!string.IsNullOrEmpty(tableName))
                        {
                            if (tableName.Contains(" "))
                            {
                                tableName = "." + tableName.Replace(" ", ".");
                                table = commonWebBrowser_Current.driver.FindElement(By.CssSelector(tableName));
                            }
                            else { table = commonWebBrowser_Current.driver.FindElement(By.Name(tableName)); }

                        }
                        else if (!string.IsNullOrEmpty(tableClass))
                        {
                            if (tableClass.Contains(" "))
                            {
                                tableClass = "." + tableClass.Replace(" ", ".");
                                table = commonWebBrowser_Current.driver.FindElement(By.CssSelector(tableClass));
                            }
                            else
                            {
                                table = commonWebBrowser_Current.driver.FindElement(By.ClassName(tableClass));
                            }
                        }
                        else if (!string.IsNullOrEmpty(sXPath))
                        {
                            table = commonWebBrowser_Current.driver.FindElement(By.XPath(sXPath));
                        }


                        List<IWebElement> allRows = null;
                        if (string.IsNullOrEmpty(sRowXPath))
                        {
                            allRows = new List<IWebElement>(table.FindElements(By.TagName("tr")));
                        }
                        else
                        {
                            allRows = new List<IWebElement>(table.FindElements(By.XPath(sRowXPath)));
                        }
                        if ((allRows == null) || (allRows.Count == 0))
                        {
                            Logger.Log.Logger.LogData("No rows found in table", Logger.LogLevel.Info);
                        }




                        // IEWATIN.WaitForComplete(5000);
                        if (dt.Columns.Count == 0)
                        {
                            if (IsHeader == true)
                            {
                                List<IWebElement> cells = null;

                                cells = new List<IWebElement>(table.FindElements(By.TagName("th")));
                                if ((cells == null) || (cells.Count == 0))
                                {
                                    cells = new List<IWebElement>(table.FindElements(By.TagName("thead")));
                                }
                                if ((cells == null) || (cells.Count == 0))
                                {
                                    var tr1 = allRows[0];
                                    if (string.IsNullOrEmpty(sCellXPath))
                                    {
                                        cells = new List<IWebElement>(tr1.FindElements(By.TagName("td")));
                                    }
                                    else
                                    {
                                        cells = new List<IWebElement>(tr1.FindElements(By.TagName(sCellXPath)));
                                    }

                                }
                                foreach (IWebElement item in cells)
                                {
                                    if (item.Text != null)
                                    {
                                        try
                                        {
                                            dt.Columns.Add(item.Text);
                                        }
                                        catch (Exception ex)
                                        {
                                            Console.WriteLine(ex.Message);
                                            dt.Columns.Add(item.Text + "_Repeated");
                                        }
                                    }
                                    else
                                    {
                                        dt.Columns.Add();
                                    }
                                }
                            }
                            else
                            {
                                List<IWebElement> cells = null;
                                var tr1 = allRows[0];
                                if (string.IsNullOrEmpty(sCellXPath))
                                {
                                    cells = new List<IWebElement>(tr1.FindElements(By.TagName("td")));
                                }
                                else
                                {
                                    cells = new List<IWebElement>(tr1.FindElements(By.TagName(sCellXPath)));
                                }
                                if ((cells == null) || (cells.Count == 0))
                                {
                                    cells = new List<IWebElement>(table.FindElements(By.TagName("th")));
                                }
                                if ((cells == null) || (cells.Count == 0))
                                {
                                    cells = new List<IWebElement>(table.FindElements(By.TagName("thead")));
                                }
                                foreach (IWebElement cell in cells)
                                {
                                    if (cell != null)
                                    {
                                        dt.Columns.Add();
                                    }
                                }
                            }
                        }

                        foreach (IWebElement Row in allRows)
                        {
                            int iDr = 0;
                            DataRow dr = dt.NewRow();
                            List<IWebElement> cells = null;
                            if (string.IsNullOrEmpty(sCellXPath))
                            {
                                cells = new List<IWebElement>(Row.FindElements(By.TagName("td")));
                            }
                            else
                            {
                                cells = new List<IWebElement>(Row.FindElements(By.TagName(sCellXPath)));
                            }
                            if (cells.Count == 0)
                                continue;

                            if (cells != null)
                            {
                                foreach (IWebElement cel in cells)
                                {

                                    dr[iDr] = cel.Text;
                                    //dr[iDr] = cel.GetAttribute("textContent");
                                    iDr = iDr + 1;
                                }
                            }
                            dt.Rows.Add(dr);
                        }
                        //RuntimeApplicationHelper.Instance.RuntimeApplicationObjects[AppId] = IEWATIN;
                        // TableResult.Set(context, dt);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Log.Logger.LogData(ex.Message + " in Web activity GetTable", Logger.LogLevel.Error);

                if (!ContinueOnError)
                {
                    context.Abort();
                }
            }

            return dt;
        }


    }
}
