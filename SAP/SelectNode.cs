using System;
using System.Activities;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Converters;
using CommonLibrary;
using Logger;
using sapfewse;

namespace Bot.Activity.SAP
{
    [Designer(typeof(ActivityDesigner1))]
   public class SelectNode : BaseNativeActivity
    {
        [DisplayName("Attach by ApplicationID")]
        public InArgument<string> ApplicationIDToAttach { get; set; }

        [DisplayName("Id")]
        [Description("Id to find Table control")]
        public InArgument<string> ControlId { get; set; }

        [DisplayName("Node Key")]
        [Description("SAP GUI Tree Node")]
        public InArgument<string> NodeKey { get; set; }

        //[DisplayName("Item")]
        //[Description("Name of Item")]
        //public InArgument<string> Item { get; set; }

        [DisplayName("Select")]
        [Description("To Select the node")]
        public bool Checkbox { get; set; }

        public SelectNode() { 
        
        }

        protected override void Execute(NativeActivityContext context)
        {
            GuiSession sapsession = null;
            string Nodekey = NodeKey.Get(context);
            SAPEntity entity = null;
          //  string item = Item.Get(context);
            string strAppIdToAttach = ApplicationIDToAttach.Get(context);
            string controlId = ControlId.Get(context);
            try
            {
                if (SelectHelper.CurrentRuntimeApplicationHelper.RuntimeApplicationObjects.ContainsKey(strAppIdToAttach)) { 
                   
                   
                    entity = (SAPEntity)SelectHelper.CurrentRuntimeApplicationHelper.RuntimeApplicationObjects[strAppIdToAttach];
                  
                    sapsession = entity.SapSession;
                 

                    GuiTree guiTree = (GuiTree)sapsession.FindById(controlId);
                                        if (guiTree == null) {
                        throw new NullReferenceException();
                    }

                    if(Checkbox)
                    guiTree.SelectNode(Nodekey);
                    Logger.Log.Logger.LogData("Control Is Selected :" + controlId, LogLevel.Info);

                }
                
            }
            catch (Exception ex)
            {
                Logger.Log.Logger.LogData("Error " + ex.Message, LogLevel.Error);

            }


        }
    }
}
