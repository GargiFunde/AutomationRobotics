using Microsoft.VisualBasic.FileIO;
using System;
using System.Activities;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Bot.Activity.CSV
{
    [ToolboxBitmap(typeof(CSV_Read), "ReadCSV.png")]
    public class CSV_Read : BaseNativeActivity
    {
        //[DisplayName("File Path")]
        //[Description("Set file path")]

        [Category("File")]
        [Description("Source file for processing")]
        [EditorAttribute(typeof(System.Windows.Forms.Design.FileNameEditor), typeof(System.Drawing.Design.UITypeEditor))]
        public InArgument<string> FilePath { get; set; }
        
        [DisplayName("Default Table Name")]
        [Description("Set default table name")]
        public InArgument<string> DefaultTableName { get; set; }

        [DisplayName("Delimiter")]
        [Description("Set Delimiter")]
        public InArgument<string> Delimiter { get; set; }

        [DisplayName("Get Value")]
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
                csvReader.SetDelimiters(new string[] { tableDelim });
                csvReader.HasFieldsEnclosedInQuotes = true;
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
