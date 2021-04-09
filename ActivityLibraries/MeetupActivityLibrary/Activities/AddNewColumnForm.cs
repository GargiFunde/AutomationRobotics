using System;
using dt=System.Data;
using System.Windows.Forms;
using Logger;
using System.Collections.Generic;
using System.Linq;

namespace Bot.Activity.ActivityLibrary.Activities
{
    public partial class AddNewColumnForm : Form
    {
      
        public static string SetValueForColumnName = "";
        public AddNewColumnForm()
        {
            InitializeComponent();


            comboBoxDataType.DataSource = Enum.GetNames(typeof(System.TypeCode));

        }

        public List<Type> GetTypes()
        {
            return typeof(Type).Assembly.GetTypes().Where(t => t.IsPrimitive).ToList();
            
        }

      
       

        public dt.DataTable dtFromAddNewForm(dt.DataTable dt)
        {
            return dt;
        }

        public void Ok_BtnClick(object sender, EventArgs e)
        {
            try
            {
                DataTableCreateForm frm2 = new DataTableCreateForm(); 
                string columnName = txtColumnName.Text;
                string defauktvalue = txtDefaultValue.Text;
                string maxl = txtMaxLength.Text;
                int maxlength = 0;
               

               
                
                bool unique = chkUnique.Checked;
                bool allowNull = chkAllowNull.Checked;
                bool AutoIncrement = chkAutoIncrement.Checked;


                //Type a = (Type)comboBoxDataType.SelectedItem;
                string dataType =(string)comboBoxDataType.SelectedItem;
                //Type dataType = (Type)comboBoxDataType.SelectedItem;
                System.Data.DataColumn newColumn = null;
                if (defauktvalue.Equals(""))
                {
                    defauktvalue = null;
                }
                else
                {

                    //newColumn.DefaultValue = defauktvalue;   testing...
                }



                if (comboBoxDataType.SelectedIndex == 0 )
                {
                    Logger.Log.Logger.LogData("Data Type is not selected", LogLevel.Error);
                    
                }
                else
                {
                    if (dataType.Equals("string"))
                    {
                        if (maxl.Equals(""))
                        {
                            maxlength = 100000000;
                        }
                        else
                        {
                            maxlength = Int32.Parse(maxl);
                        }
                        newColumn.MaxLength = maxlength;
                        newColumn.DefaultValue = defauktvalue;
                    }
                lblColumnName.Text = AddNewColumnForm.SetValueForColumnName;
                newColumn = new System.Data.DataColumn(columnName);
                newColumn.DataType = System.Type.GetType("System."+ dataType);
                    newColumn.DefaultValue = defauktvalue; //testing..
                    newColumn.Unique = unique;
                newColumn.AllowDBNull = allowNull;
                newColumn.AutoIncrement = AutoIncrement;
                //dtable = frm2.fromDataCreatedForm();
                //DataTableCreateForm.dtable.Columns.
              
                }

                DataTableCreateForm.dtable.Columns.Add(newColumn);
                DataTableCreateForm.dtable.AcceptChanges();
                dtFromAddNewForm(DataTableCreateForm.dtable);
                

                frm2.Show();
                this.Close();

            }
            catch (Exception ex)
            {
                Logger.Log.Logger.LogData("Error in Import:" + ex.Message, Logger.LogLevel.Error);
            }


        }

        public void Cancel_BtnClick(object sender, EventArgs e)
        {
            this.Close();
        }

        private void AddNewColumnForm_Load(object sender, EventArgs e)
        {

        }
    }
}
