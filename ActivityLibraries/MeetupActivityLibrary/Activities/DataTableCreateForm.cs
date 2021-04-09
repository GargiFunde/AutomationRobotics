using System;
using System.Collections.Generic;
using System.ComponentModel;
using dt = System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Controls;
using Logger;
using Newtonsoft.Json;
using System.Data;
using System.IO;
using System.Xml;

namespace Bot.Activity.ActivityLibrary.Activities
{
    public partial class DataTableCreateForm : Form
    {

        public static dt.DataTable dtable = new dt.DataTable();
        public static string dtableser;
        public Type type { set; get; }
        public string name { get; set; }
        AddNewColumnForm addNewColumnForm = new AddNewColumnForm();
        int index;
        public DataTableCreateForm()
        {
            InitializeComponent();
            Addbutton();
        }

        public void Addbutton()
        {

            System.Windows.Forms.Button btn = new System.Windows.Forms.Button();
            btn.Location = new Point(0, 0);
            btn.Text = "+";
            btn.Width = 40;
            btn.BackColor = Color.White;
            dataGridView1.Controls.Add(btn);
           
                dataGridView1.DataSource = dtable;
           
           
            dataGridView1.BackgroundColor = Color.White;
            btn.Click += new EventHandler(this.BtnAddColumn_Click);
        }

        public DataGridView gvFromDataTableCreate()
        {
            return dataGridView1;
        }

        private void BtnAddColumn_Click(object sender, EventArgs e)
        {
            AddNewColumnForm addNewColumnForm = new AddNewColumnForm();
            addNewColumnForm.Show();
            this.Close();
        }

        public void Editmaxlength(int index, int maxlength)
        {
            dtable.Columns[index].MaxLength = maxlength;
            dtable.AcceptChanges();
        }

        public void EditAutoIncrement(int index, bool autoIncrement)
        {
            dtable.Columns[index].AutoIncrement = autoIncrement;
            dtable.AcceptChanges();
        }

        public void Editunique(int index, bool unique)
        {
            dtable.Columns[index].Unique = unique;
            dtable.AcceptChanges();
        }

        public void EditDefaultvalue(int index, string defauktvalue)
        {
             dtable.Columns[index].DefaultValue = defauktvalue;
            dtable.AcceptChanges();
        }

        public dt.DataTable fromDataCreatedForm()
        {
            
            return dtable;           
        }
        public string fromDataCreatedFormString()
        {
            //dtableser = JsonConvert.SerializeObject(dtable);
            //return dtableser;
            StringWriter writer = null;
            try {

              writer = new StringWriter();
                dtable.TableName = "MainTable";
                dtable.WriteXml(writer, XmlWriteMode.WriteSchema, false);
           
                string xmlstring = writer.GetStringBuilder().ToString();
                dtableser = xmlstring;
              //  XmlDocument doc = new XmlDocument();
                //    doc.LoadXml(xmlstring);
                //json = JsonConvert.SerializeXmlNode(doc);
            }
            catch (Exception e)
            {
                Logger.Log.Logger.LogData(e.Message + " in activity DataTableCreate", LogLevel.Error);
            }
            finally
            {
                writer.Dispose();
               
            }
            return dtableser;

        }

        public static void GetCreatedDataTable(string dtstring)
        {
            if (dtstring != null)
            {
                //DataTable temp2 = JsonConvert.DeserializeObject<DataTable>(dtstring);
                //dtable = temp2;
                StringReader reader = null;
                try
                {

                    reader = new StringReader(dtstring);
                    DataTable temp = new DataTable();
                    temp.ReadXml(reader);

                    dtable = temp;
                  
                
                }
                catch (Exception e)
                {
                    Logger.Log.Logger.LogData(e.Message + " in activity DataTableCreate", LogLevel.Error);
                }
                finally
                {
                    reader.Dispose();

                }
            }
            else
            {
                dtable = new DataTable();
            }
            //return temp2;
        }
        private void BtnOk_Click(object sender, EventArgs e)
        {
            //DataTableCreate datatb = new DataTableCreate();
            //datatb.TableInfo.Set(context, dtable); 
            //DataTableCreate datatb =  DataTableCreate.ReturnDataTableCreate();
            //SetTableDataStringFormat = dtable;
            //datatb.TableInfo.Expression
            //Binding ModelItem.Alt, Mode = TwoWay;
          //  DataTableCreate.SetProp(dtable);
            //dtable
            this.Close();
        }

        private void BtnCancel_Click(object sender, EventArgs e)
        {
            try
            {
                //dataGridView1.Rows.Clear();
                this.Close();
            }
            catch (Exception ex)
            {
                Logger.Log.Logger.LogData("Exception while Closing: " + ex.Message,  LogLevel.Error);
            }
        }

        // public System.Data.DataTable DT { get; set; }
       

        private void DataTableCreateForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            
        }
        private void lblColumnName_Click(object sender, EventArgs e)
        {
         

        }
        private void brnDelete_Click(object sender, EventArgs e)
        {
            Delete(index);
           
        }

        public void Delete(int index)
        {
            dtable.Columns.Remove(dtable.Columns[index]);
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            EditForm editForm = new EditForm();
            string colname = dtable.Columns[index].ColumnName;
            editForm.Index.Text = index.ToString();
            editForm.txtColumnName.Text = colname;
            editForm.Show();

        }

        public void Edit(int index,string editColName)
        {
            EditForm editForm = new EditForm();

            string colname = dtable.Columns[index].ColumnName;
            //string editcolname = editForm.txtColumnName.Text;
            dtable.Columns[colname].ColumnName = editColName;
            dtable.AcceptChanges();
            
        }

        public void EditDatatype(int index,string dataType)
        {
            
            dtable.Columns[index].DataType = System.Type.GetType("System." + dataType); ;
            dtable.AcceptChanges();

        }

        public void EditallowNull(int index, bool allowNull)
        {           
            dtable.Columns[index].AllowDBNull = allowNull;
            dtable.AcceptChanges();
        }
        
        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            index = e.ColumnIndex;           
        }

       
    }
}
