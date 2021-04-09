using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Bot.Activity.ActivityLibrary.Activities
{
    public partial class EditForm : Form
    {
        public EditForm()
        {
            InitializeComponent();
            comboBoxDataType.DataSource = Enum.GetNames(typeof(System.TypeCode));
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            try
            {




                DataTableCreateForm dataTableCreateForm = new DataTableCreateForm();
                string editColName = txtColumnName.Text;
                int index = Convert.ToInt32(Index.Text);



                string maxl = txtMaxLength.Text;
                int maxlength = 0;
                if (maxl.Equals(""))
                {
                    maxlength = 0;
                }
                else
                {
                    maxlength = Int32.Parse(maxl);
                }
                string defauktvalue = txtDefaultValue.Text;
                bool unique = chkUnique.Checked;
                bool allowNull = chkAllowNull.Checked;
                bool AutoIncrement = chkAutoIncrement.Checked;

                if (editColName != "")
                {
                    dataTableCreateForm.Edit(index, editColName);
                }
                if (comboBoxDataType.SelectedIndex != 0)
                {
                    string dataType = (string)comboBoxDataType.SelectedItem;
                    dataTableCreateForm.EditDatatype(index, dataType);
                }
                if (defauktvalue != null)
                {
                    dataTableCreateForm.EditDefaultvalue(index, defauktvalue);

                }
                if (maxlength != 0)
                {
                    dataTableCreateForm.Editmaxlength(index, maxlength);
                }
                if (allowNull != false)
                {
                    dataTableCreateForm.EditallowNull(index, allowNull);
                }
                if (unique != false)
                {
                    dataTableCreateForm.Editunique(index, unique);
                }
                if (AutoIncrement != false)
                {
                    dataTableCreateForm.EditAutoIncrement(index, AutoIncrement);
                }


                this.Close();

            }
            catch (Exception)
            {

                throw;
            }

        }

        private void btnCancel_Click(object sender, EventArgs e)
        {

        }
    }
}
