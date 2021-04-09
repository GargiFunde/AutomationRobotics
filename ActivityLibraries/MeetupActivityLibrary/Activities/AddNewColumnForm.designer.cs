namespace Bot.Activity.ActivityLibrary.Activities
{
    partial class AddNewColumnForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        public System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        public void InitializeComponent()
        {
            this.lblColumnName = new System.Windows.Forms.Label();
            this.txtColumnName = new System.Windows.Forms.TextBox();
            this.lblDataType = new System.Windows.Forms.Label();
            this.comboBoxDataType = new System.Windows.Forms.ComboBox();
            this.btnOk = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.lblUnique = new System.Windows.Forms.Label();
            this.chkUnique = new System.Windows.Forms.CheckBox();
            this.lblDefaultValue = new System.Windows.Forms.Label();
            this.txtDefaultValue = new System.Windows.Forms.TextBox();
            this.lblMaxLength = new System.Windows.Forms.Label();
            this.txtMaxLength = new System.Windows.Forms.TextBox();
            this.lblAllowNull = new System.Windows.Forms.Label();
            this.lblAutoIncrement = new System.Windows.Forms.Label();
            this.chkAllowNull = new System.Windows.Forms.CheckBox();
            this.chkAutoIncrement = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // lblColumnName
            // 
            this.lblColumnName.AutoSize = true;
            this.lblColumnName.Location = new System.Drawing.Point(84, 27);
            this.lblColumnName.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblColumnName.Name = "lblColumnName";
            this.lblColumnName.Size = new System.Drawing.Size(73, 13);
            this.lblColumnName.TabIndex = 0;
            this.lblColumnName.Text = "Column Name";
            // 
            // txtColumnName
            // 
            this.txtColumnName.Location = new System.Drawing.Point(197, 27);
            this.txtColumnName.Margin = new System.Windows.Forms.Padding(2);
            this.txtColumnName.Name = "txtColumnName";
            this.txtColumnName.Size = new System.Drawing.Size(215, 20);
            this.txtColumnName.TabIndex = 1;
            // 
            // lblDataType
            // 
            this.lblDataType.AutoSize = true;
            this.lblDataType.Location = new System.Drawing.Point(84, 60);
            this.lblDataType.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblDataType.Name = "lblDataType";
            this.lblDataType.Size = new System.Drawing.Size(57, 13);
            this.lblDataType.TabIndex = 2;
            this.lblDataType.Text = "Data Type";
            // 
            // comboBoxDataType
            // 
            this.comboBoxDataType.FormattingEnabled = true;
            this.comboBoxDataType.Location = new System.Drawing.Point(197, 60);
            this.comboBoxDataType.Margin = new System.Windows.Forms.Padding(2);
            this.comboBoxDataType.Name = "comboBoxDataType";
            this.comboBoxDataType.Size = new System.Drawing.Size(215, 21);
            this.comboBoxDataType.TabIndex = 3;
            // 
            // btnOk
            // 
            this.btnOk.Location = new System.Drawing.Point(148, 254);
            this.btnOk.Margin = new System.Windows.Forms.Padding(2);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(71, 22);
            this.btnOk.TabIndex = 4;
            this.btnOk.Text = "Ok";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.Ok_BtnClick);
            // 
            // btnCancel
            // 
            this.btnCancel.Location = new System.Drawing.Point(257, 254);
            this.btnCancel.Margin = new System.Windows.Forms.Padding(2);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(59, 22);
            this.btnCancel.TabIndex = 5;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.Cancel_BtnClick);
            // 
            // lblUnique
            // 
            this.lblUnique.AutoSize = true;
            this.lblUnique.Location = new System.Drawing.Point(84, 152);
            this.lblUnique.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblUnique.Name = "lblUnique";
            this.lblUnique.Size = new System.Drawing.Size(41, 13);
            this.lblUnique.TabIndex = 6;
            this.lblUnique.Text = "Unique";
            // 
            // chkUnique
            // 
            this.chkUnique.AutoSize = true;
            this.chkUnique.Location = new System.Drawing.Point(197, 152);
            this.chkUnique.Margin = new System.Windows.Forms.Padding(2);
            this.chkUnique.Name = "chkUnique";
            this.chkUnique.Size = new System.Drawing.Size(15, 14);
            this.chkUnique.TabIndex = 7;
            this.chkUnique.UseVisualStyleBackColor = true;
            // 
            // lblDefaultValue
            // 
            this.lblDefaultValue.AutoSize = true;
            this.lblDefaultValue.Location = new System.Drawing.Point(84, 92);
            this.lblDefaultValue.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblDefaultValue.Name = "lblDefaultValue";
            this.lblDefaultValue.Size = new System.Drawing.Size(71, 13);
            this.lblDefaultValue.TabIndex = 8;
            this.lblDefaultValue.Text = "Default Value";
            // 
            // txtDefaultValue
            // 
            this.txtDefaultValue.Location = new System.Drawing.Point(197, 92);
            this.txtDefaultValue.Margin = new System.Windows.Forms.Padding(2);
            this.txtDefaultValue.Name = "txtDefaultValue";
            this.txtDefaultValue.Size = new System.Drawing.Size(215, 20);
            this.txtDefaultValue.TabIndex = 9;
            // 
            // lblMaxLength
            // 
            this.lblMaxLength.AutoSize = true;
            this.lblMaxLength.Location = new System.Drawing.Point(84, 120);
            this.lblMaxLength.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblMaxLength.Name = "lblMaxLength";
            this.lblMaxLength.Size = new System.Drawing.Size(63, 13);
            this.lblMaxLength.TabIndex = 10;
            this.lblMaxLength.Text = "Max Length";
            // 
            // txtMaxLength
            // 
            this.txtMaxLength.Location = new System.Drawing.Point(197, 120);
            this.txtMaxLength.Margin = new System.Windows.Forms.Padding(2);
            this.txtMaxLength.Name = "txtMaxLength";
            this.txtMaxLength.Size = new System.Drawing.Size(215, 20);
            this.txtMaxLength.TabIndex = 11;
            // 
            // lblAllowNull
            // 
            this.lblAllowNull.AutoSize = true;
            this.lblAllowNull.Location = new System.Drawing.Point(84, 183);
            this.lblAllowNull.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblAllowNull.Name = "lblAllowNull";
            this.lblAllowNull.Size = new System.Drawing.Size(53, 13);
            this.lblAllowNull.TabIndex = 12;
            this.lblAllowNull.Text = "Allow Null";
            // 
            // lblAutoIncrement
            // 
            this.lblAutoIncrement.AutoSize = true;
            this.lblAutoIncrement.Location = new System.Drawing.Point(84, 211);
            this.lblAutoIncrement.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.lblAutoIncrement.Name = "lblAutoIncrement";
            this.lblAutoIncrement.Size = new System.Drawing.Size(79, 13);
            this.lblAutoIncrement.TabIndex = 13;
            this.lblAutoIncrement.Text = "Auto Increment";
            // 
            // chkAllowNull
            // 
            this.chkAllowNull.AutoSize = true;
            this.chkAllowNull.Location = new System.Drawing.Point(197, 184);
            this.chkAllowNull.Margin = new System.Windows.Forms.Padding(2);
            this.chkAllowNull.Name = "chkAllowNull";
            this.chkAllowNull.Size = new System.Drawing.Size(15, 14);
            this.chkAllowNull.TabIndex = 14;
            this.chkAllowNull.UseVisualStyleBackColor = true;
            // 
            // chkAutoIncrement
            // 
            this.chkAutoIncrement.AutoSize = true;
            this.chkAutoIncrement.Location = new System.Drawing.Point(197, 211);
            this.chkAutoIncrement.Margin = new System.Windows.Forms.Padding(2);
            this.chkAutoIncrement.Name = "chkAutoIncrement";
            this.chkAutoIncrement.Size = new System.Drawing.Size(15, 14);
            this.chkAutoIncrement.TabIndex = 15;
            this.chkAutoIncrement.UseVisualStyleBackColor = true;
            // 
            // AddNewColumnForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(481, 292);
            this.Controls.Add(this.chkAutoIncrement);
            this.Controls.Add(this.chkAllowNull);
            this.Controls.Add(this.lblAutoIncrement);
            this.Controls.Add(this.lblAllowNull);
            this.Controls.Add(this.txtMaxLength);
            this.Controls.Add(this.lblMaxLength);
            this.Controls.Add(this.txtDefaultValue);
            this.Controls.Add(this.lblDefaultValue);
            this.Controls.Add(this.chkUnique);
            this.Controls.Add(this.lblUnique);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.comboBoxDataType);
            this.Controls.Add(this.lblDataType);
            this.Controls.Add(this.txtColumnName);
            this.Controls.Add(this.lblColumnName);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "AddNewColumnForm";
            this.Text = "Add New Column";
            this.Load += new System.EventHandler(this.AddNewColumnForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public System.Windows.Forms.Label lblColumnName;
        public System.Windows.Forms.TextBox txtColumnName;
        public System.Windows.Forms.Label lblDataType;
        public System.Windows.Forms.ComboBox comboBoxDataType;
        public System.Windows.Forms.Button btnOk;
        public System.Windows.Forms.Button btnCancel;
        public System.Windows.Forms.Label lblUnique;
        public System.Windows.Forms.CheckBox chkUnique;
        public System.Windows.Forms.Label lblDefaultValue;
        public System.Windows.Forms.TextBox txtDefaultValue;
        public System.Windows.Forms.Label lblMaxLength;
        public System.Windows.Forms.TextBox txtMaxLength;
        public System.Windows.Forms.Label lblAllowNull;
        public System.Windows.Forms.Label lblAutoIncrement;
        public System.Windows.Forms.CheckBox chkAllowNull;
        public System.Windows.Forms.CheckBox chkAutoIncrement;
    }
}