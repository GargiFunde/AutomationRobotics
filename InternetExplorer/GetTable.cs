using System;
using System.Activities;
using System.ComponentModel;
using CommonLibrary;
using WatiN.Core;
using System.Data;
using System.Drawing;

namespace Bot.Activity.InternetExplorer
{
    [ToolboxBitmap("Resources/IE_GetTable.png")]
    [Designer(typeof(ActivityDesignerForGetTable))]
    public class IE_GetTable : BaseNativeActivity
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

        [Category("Output")]
        [DisplayName("Table Result")]
        [System.ComponentModel.Description("Enter DataTable Variable")]
        public OutArgument<DataTable> TableResult { get; set; }

        public bool IsHeader { get; set; }

        protected override void Execute(NativeActivityContext context)
        {
            Table tableResult = null; 
            string AppId = ApplicationIDToAttach.Get(context);
            string tableId = TableId.Get(context);
            string tableName = TableName.Get(context);
            string tableClass = TableClass.Get(context);

            string tableAttributeName = TableAttributeName.Get(context);
            string tableAttributeValue = TableAttributeValue.Get(context);

            //string tableBody = TableBody.Get(context);
            int timeInSec = TimeOutInSecond.Get(context);
            try
            {
                if (AppId != string.Empty) //scraping time
                {
                    if (SelectHelper.CurrentRuntimeApplicationHelper.RuntimeApplicationObjects.ContainsKey(AppId))
                    {

                        WatiN.Core.IE IEWATIN = (WatiN.Core.IE)SelectHelper.CurrentRuntimeApplicationHelper.RuntimeApplicationObjects[AppId];
                        // IEWATIN.GoTo(searchUrl)
                        //TableBody tableBody = IEWATIN.Table(tableId).TableBodies[0];
                        if ((!string.IsNullOrEmpty(tableAttributeName)) && (!string.IsNullOrEmpty(tableAttributeValue)))
                        {
                            tableResult = IEWATIN.Table(Find.By(tableAttributeName, tableAttributeValue));
                        }
                        else if (!string.IsNullOrEmpty(tableId))
                        {
                            tableResult = IEWATIN.Table(Find.ById(tableId));
                        }
                        else if (!string.IsNullOrEmpty(tableName))
                        {
                            tableResult = IEWATIN.Table(Find.ByName(tableName));
                        }
                        else if (!string.IsNullOrEmpty(tableClass))
                        {
                            tableResult = IEWATIN.Table(Find.ByClass(tableClass));
                        }
                        
                        if(tableResult ==null)
                        {
                            Logger.Log.Logger.LogData("table not found " + " in activity GetTable", Logger.LogLevel.Error);
                            TableResult.Set(context, false);
                            if (!ContinueOnError)
                            {
                                context.Abort();
                            }
                            return;
                        }
                        var resultBody = tableResult.TableBodies[0];
                        // IEWATIN.WaitForComplete(5000);
                        DataTable dt = new DataTable();
                        if (IsHeader == true)
                        {
                            var gridTableRows = tableResult.TableRows[0];
                            foreach (Element item in gridTableRows.Elements)
                            {
                                if ((item.TagName == "TH")||(item.TagName == "TD"))
                                {
                                    if (item.Text != null)
                                    {

                                        try
                                        {
                                            dt.Columns.Add(item.Text);
                                        }
                                        catch (Exception ex)
                                        {
                                            dt.Columns.Add(item.Text + "_Repeated");
                                        }

                                    }
                                    else
                                    {
                                        dt.Columns.Add();
                                    }
                                }
                            }
                        }
                        else
                        {
                            TableRow tr1 = resultBody.OwnTableRows[0];
                            foreach (var item in tr1.Elements)
                            {
                                var cell = item as TableCell;
                                if (cell != null)
                                {
                                     dt.Columns.Add();
                                }
                               
                            }
                        }
                       
                        foreach (TableRow tr in resultBody.OwnTableRows)
                        {

                            int iDr = 0;
                            DataRow dr = dt.NewRow();                      
                        
                            foreach (var item in tr.Elements)
                            {

                                var cell = item as TableCell;
                                if(cell !=null)
                                    {
                                        dr[iDr] = item.Text;
                                        iDr = iDr + 1;
                                }
                               
                            }
                            dt.Rows.Add(dr);
                        }
                        //RuntimeApplicationHelper.Instance.RuntimeApplicationObjects[AppId] = IEWATIN;
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


    }
}
