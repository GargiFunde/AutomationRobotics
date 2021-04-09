using Microsoft.VisualBasic.FileIO;
using System;
using System.Activities;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Design;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Bot.Activity.CSV
{
    [ToolboxBitmap(typeof(CSV_Read), "WriteCSV.png")]
    [Designer(typeof(WriteCSV1))]
    public class CSV_Write: BaseNativeActivity
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

        [DisplayName("Table to save")]
        [Description("Table to save")]
        public InArgument<DataTable> TableToSave { get; set; }

        [DisplayName("Result")]
        [Description("result")]
        public OutArgument<bool> Result { get; set; }
        protected override void Execute(NativeActivityContext context)
        {
            try
            {
                string strFilePath = FilePath.Get(context);
                string strDelimiter = Delimiter.Get(context);
                DataTable dtDataTable = TableToSave.Get(context);
                bool result = ExpoetToCSV(dtDataTable, strFilePath, strDelimiter);
                Result.Set(context, result);
                //if (MyResultTable != null)
                //{
                //    TableToSave.Set(context, MyResultTable);
                //}
            }
            catch (Exception ex)
            {
                Logger.Log.Logger.LogData(ex.Message + " in activity CSV_Write", Logger.LogLevel.Error);
                if (!ContinueOnError)
                {
                    context.Abort();
                }
            }
        }
        public bool ExpoetToCSV(DataTable dtDataTable, string strFilePath, string strDelimiter = ",")
        {
            try
            {
                StreamWriter sw = new StreamWriter(strFilePath, false);
                //headers   
                for (int i = 0; i < dtDataTable.Columns.Count; i++)
                {
                    sw.Write(dtDataTable.Columns[i].ToString().Trim());
                    if (i < dtDataTable.Columns.Count - 1)
                    {
                        sw.Write("" + strDelimiter + "");
                    }
                }
                sw.Write(sw.NewLine);
                foreach (DataRow dr in dtDataTable.Rows)
                {
                    for (int i = 0; i < dtDataTable.Columns.Count; i++)
                    {
                        if (!Convert.IsDBNull(dr[i]))
                        {
                            string value = dr[i].ToString().Trim();
                            if (value.Contains(','))
                            {
                                value = String.Format("\"{0}\"", value);
                                sw.Write(value);
                            }
                            else
                            {
                                sw.Write(dr[i].ToString().Trim());
                            }
                        }
                        if (i < dtDataTable.Columns.Count - 1)
                        {
                            sw.Write("" + strDelimiter + "");
                        }
                    }
                    sw.Write(sw.NewLine);
                }
                sw.Close();
            }catch(Exception ex)
            {
                return false;
            }
            return true;
        }

    }

   
}
