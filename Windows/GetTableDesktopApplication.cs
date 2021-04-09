//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
using CommonLibrary;
using System;
using System.Activities;
using System.ComponentModel;
using System.Data;
using System.Drawing;
//using System.ComponentModel;
//using CommonLibrary.Interfaces;
//using System.Data;
//using CommonLibrary;
//using White.Core.UIItems.Finders;
//using White.Core.UIItems.WPFUIItems;
//using System.Windows.Controls;
//using White.Core.UIItems.TableItems;
//using System.Collections.Generic;
//using TestStack.White.UIItems;
//using Xunit;
//using System.Windows.Automation;
//using System.Range;
using System.Linq;
using System.Windows.Automation;
using TestStack.White.AutomationElementSearch;
using TestStack.White.UIItems;
using TestStack.White.UIItems.Custom;
using TestStack.White.UIItems.Finders;
using TestStack.White.UIItems.TableItems;
using TestStack.White.UIItems.WindowItems;
using TestStack.White.UIItems.WPFUIItems;
//using White.Core.UIItems.Custom;
//using UIAComWrapper;

namespace Bot.Activity.Windows
{
    [ToolboxBitmap("Resources/GetTableDesktopApplication.png")]
    [Designer(typeof(GetTableDesktopApplication_ActivityDesigner))]

    public class GetTableDesktopApplication : NativeActivity
    {
        [RequiredArgument]
        [Category("Input")]
        [DisplayName("Application ID To Attach")]
        [System.ComponentModel.Description("Enter Application ID To Attach")]
        public InArgument<string> ApplicationIDToAttach { get; set; }

        [Category("Input")]
        [DisplayName("Table Id")]
        [System.ComponentModel.Description("Enter Table Id To Attach")]
        public InArgument<string> TableId { get; set; }

        [Category("Input")]
        [DisplayName("Table Name")]
        [System.ComponentModel.Description("Enter Table Name To Attach")]
        public InArgument<string> TableName { get; set; }

        [Category("Input")]
        [DisplayName("Table Class")]
        [System.ComponentModel.Description("Enter Table Class To Attach")]
        public InArgument<string> TableClass { get; set; }

        [Category("Input")]
        [DisplayName("Table Attribute Name")]
        [System.ComponentModel.Description("Enter Table Name To Attach")]
        public InArgument<string> TableAttributeName { get; set; }

        [Category("Input")]
        [DisplayName("Table Attribute Value")]
        [System.ComponentModel.Description("Enter Table Value To Attach")]
        public InArgument<string> TableAttributeValue { get; set; }

        //[RequiredArgument]
        //public InArgument<string> TableBody { get; set; }

        [Category("Input")]
        [DisplayName("Time Out In Second")]
        [System.ComponentModel.Description("Enter Time Out In Second")]
        public InArgument<int> TimeOutInSecond { get; set; }

        [DisplayName("Window Title")]
        //  [Description("XPath to find control")]
        public InArgument<string> WindowTitle { get; set; }

        [Category("Output")]
        [DisplayName("Table Result")]
        [System.ComponentModel.Description("Enter DataTable Variable")]
        public OutArgument<DataTable> TableResult { get; set; }

        public bool IsHeader { get; set; }

        protected override void Execute(NativeActivityContext context)
        {
            //Table tableResult = null;
            string AppId = ApplicationIDToAttach.Get(context);
            string tableId = TableId.Get(context);
            string tableName = TableName.Get(context);
            string tableClass = TableClass.Get(context);
            string sWindowTitle = WindowTitle.Get(context);
            string tableAttributeName = TableAttributeName.Get(context);
            string tableAttributeValue = TableAttributeValue.Get(context);

            //string tableBody = TableBody.Get(context);
            int timeInSec = TimeOutInSecond.Get(context);
            TestStack.White.UIItems.WindowItems.Window window = null;
                if (AppId != string.Empty) //scraping time
                {
                    if (SelectHelper.CurrentRuntimeApplicationHelper.RuntimeApplicationObjects.ContainsKey(AppId))
                    {

                        WindowsInstance windowsInstance = (WindowsInstance)SelectHelper.CurrentRuntimeApplicationHelper.RuntimeApplicationObjects[AppId];
                        // IEWATIN.GoTo(searchUrl)
                        //TableBody tableBody = IEWATIN.Table(tableId).TableBodies[0];
                        if (!string.IsNullOrEmpty(tableId))
                        {
                        try
                        {
                            try
                            {
                                window =  WindowsHelper.GetApplicationObject(sWindowTitle, context);


                                if(window == null)
                                {
                                    Logger.Log.Logger.LogData("Window object not found :", Logger.LogLevel.Error);
                                }
                            }
                            catch (Exception ex)
                            {
                                Logger.Log.Logger.LogData("Window Exception : " + ex.Message, Logger.LogLevel.Error);
                            }

                            //UIItem item = (UIItem)window.Get(SearchCriteria.ByAutomationId("item"));

                            //CustomUIItem customControl = item as CustomUIItem;
                            //IUIItem[] items = customControl.GetMultiple(SearchCriteria.All);

                            //ListView dataGrid = window.Get<ListView>(SearchCriteria.ByAutomationId(tableId));
                            //var row = dataGrid.Rows[1];
                            //row.Select();
                            //Table table1;

                            //Table table;
                            //table = window.Get<Table>("DataGrid");

                            IUIItem[] items = window.GetMultiple(SearchCriteria.All);
                            foreach (IUIItem item in items)
                            {
                               
                                //item.()AutomationElement.FindFirst();// GetCurrentPropertyValue(); //GetCurrentPropertyValue(); //AutomationElement(AutomationElementIdentifiers.ItemStatusProperty);
                                if (item is TestStack.White.UIItems.Custom.CustomUIItem)
                                {
                                    // Process custom controls
                                    TestStack.White.UIItems.Custom.CustomUIItem customControl = item as CustomUIItem;

                                    // Retrieve all the child controls
                                    //IUIItem[] items = customControl.AsContainer().GetMultiple(White.Core.UIItems.Finders.SearchCriteria.All);
                                    IUIItem[] items4 = customControl.GetMultiple(TestStack.White.UIItems.Finders.SearchCriteria.All);


                                    // visit all the children
                                    foreach (var t in items4)
                                    {
                                        // Visit(t);
                                    }
                                }
                            }
                                TestStack.White.UIItems.TableItems.Table table = window.Get<TestStack.White.UIItems.TableItems.Table>(tableId);

                            //White.Core.UIItems.TableItems.Table table = window.Get<White.Core.UIItems.TableItems.Table>(SearchCriteria.ByAutomationId(tableId));

                            //TableRows rows = table1.Rows;

                            //White.Core.UIItems.TableItems.Table table = window.Get<White.Core.UIItems.TableItems.Table>(tableId);

                            //Logger.Log.Logger.LogData("Dataable Count : "+table.Rows.Count(),Logger.LogLevel.Info);
                        }
                        catch (Exception ex)
                        {
                            Logger.Log.Logger.LogData("Exception "+ex.Message , Logger.LogLevel.Error);
                            try
                            {
                                //White.Core.UIItems.ListView table = window.Get<White.Core.UIItems.ListView>(SearchCriteria.ByAutomationId(tableId));
                                //Logger.Log.Logger.LogData("ListView Count : " + table.Rows.Count(), Logger.LogLevel.Info);
                            }
                            catch (Exception) { }
                            return;
                        }
                        // Get a TableItemPattern from the source of the event.
                    }
                    }
                }
            }
    }
}
