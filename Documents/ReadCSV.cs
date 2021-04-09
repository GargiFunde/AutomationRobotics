using Microsoft.VisualBasic.FileIO;
using System;
using System.Activities;
using System.ComponentModel;
using System.Data;
using System.Drawing;

namespace Bot.Activity.CSV
{
    [ToolboxBitmap("Resources/CSV_Read.png")]
    [Designer(typeof(CSV_Read_ActivityDesigner))]
    public class CSV_Read : BaseNativeActivity
    {
        //[DisplayName("File Path")]
        //[Description("Set file path")]
        
        [RequiredArgument]
        [Category("File Path")]
        [DisplayName("CSV File Path*")]
        [Description("Source file for processing")]
        [EditorAttribute(typeof(System.Windows.Forms.Design.FileNameEditor), typeof(System.Drawing.Design.UITypeEditor))]
        public InArgument<string> FilePath { get; set; }

        [RequiredArgument]
        [Category("Input Parameters")]
        [DisplayName("Default Table Name*")]
        [Description("Set default table name")]
        public InArgument<string> DefaultTableName { get; set; }


        [Category("Input Parameters")]
        [DisplayName("De-Limiter")]
        [Description("Set Delimiter")]
        public InArgument<string> Delimiter { get; set; }

        [RequiredArgument]
        [Category("Output Parameters")]
        [DisplayName("Get Value*")]
        [Description("Get Control Value")]
        public OutArgument<DataTable> ResultTable { get; set; }


        protected override void Execute(NativeActivityContext context)
        {
            try
            {
                string strFilePath = FilePath.Get(context);
                string strDelimiter = Delimiter.Get(context);
                string strDefaultTableName = DefaultTableName.Get(context);
                DataTable MyResultTable = GetDataTabletFromCSVFile(strFilePath, strDefaultTableName, strDelimiter);
                if (MyResultTable != null)
                {
                    ResultTable.Set(context, MyResultTable);
                }
            }
            catch (Exception ex)
            {
                Logger.Log.Logger.LogData(ex.Message + " in activity CSV_Read", Logger.LogLevel.Error);
                if (!ContinueOnError)
                {
                    context.Abort();
                }
            }
        }
        public System.Data.DataTable GetDataTabletFromCSVFile(string csv_file_path, string defaultTableName, string tableDelim)
        {
            System.Data.DataTable csvData = new System.Data.DataTable(defaultTableName);
           
            using (TextFieldParser csvReader = new TextFieldParser(csv_file_path))
            {
                if (tableDelim == ",")
                {
                    csvReader.HasFieldsEnclosedInQuotes = true;
                    csvReader.SetDelimiters(new string[] { tableDelim });
                }
                else
                {
                    csvReader.SetDelimiters(new string[] { tableDelim });
                    csvReader.HasFieldsEnclosedInQuotes = true;
                }
             
                string[] colFields = csvReader.ReadFields();
                foreach (string column in colFields)
                {
                    DataColumn datecolumn = new DataColumn(column);
                    datecolumn.AllowDBNull = true;
                    csvData.Columns.Add(datecolumn);
                }

                while (!csvReader.EndOfData)
                {
                    string[] fieldData = csvReader.ReadFields();
                    //Making empty value as null
                    for (int i = 0; i < fieldData.Length; i++)
                    {
                        if (fieldData[i] == string.Empty)
                        {
                            fieldData[i] = string.Empty; //fieldData[i] = null
                        }
                        //Skip rows that have any csv header information or blank rows in them
                        if (fieldData[0].Contains("Disclaimer") || string.IsNullOrEmpty(fieldData[0]))
                        {
                            continue;
                        }
                    }
                    csvData.Rows.Add(fieldData);
                }
            }
            return csvData;
        }
    }
}
