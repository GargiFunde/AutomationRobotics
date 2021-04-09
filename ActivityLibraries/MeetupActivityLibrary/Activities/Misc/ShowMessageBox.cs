using System;
//using System.Collections.Generic;
//using System.Text;
using System.Activities;
using System.Windows.Forms;
using System.ComponentModel;
using System.Drawing;
using Bot.Activity.ActivityLibrary.Activities.Misc;

namespace Bot.Activity.ActivityLibrary.Misc
{
    /// <summary>
    /// Custom Activity that displays in a MessageBox the Value of the InputData argument
    /// </summary>
    /// 
    [ToolboxBitmap("Resources/ShowMessageBox.png")]
    [Designer(typeof(ShowMessageBox_ActivityDesigner))]
    public sealed class ShowMessageBox : CodeActivity
    {
        #region Arguments

        [Category("Ouput Parameter")]
        [DisplayName("Text")]
        [Description("Text to be Displayed in MessageBox")]
        public InArgument<Object> InputData { get; set; }

        #endregion

        /// <summary>
        /// Constructor
        /// </summary>
        public ShowMessageBox():base()
        {
            InputData = new Object();
            this.DisplayName = "Message";
        }

        /// <summary>
        /// Execution Logic
        /// </summary>
        protected override void Execute(CodeActivityContext context)
        {
            if (this.InputData.Get(context) != null)
            {
                MessageBox.Show(this.InputData.Get(context).ToString());
            }
            else
            {
                MessageBox.Show("Value is null");
            }
        }
    }
}
