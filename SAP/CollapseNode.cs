using CommonLibrary;
using Logger;
using sapfewse;
using System;
using System.Activities;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bot.Activity.SAP
{
    [Designer(typeof(ActivityDesigner1))]
  public  class CollapseNode : BaseNativeActivity
    {
        [DisplayName("Attach by ApplicationID")]
        public InArgument<string> ApplicationIDToAttach { get; set; }

        [DisplayName("Id")]
        [Description("Id to find Table control")]
        public InArgument<string> ControlId { get; set; }

        [DisplayName("Node Key")]
        [Description("SAP GUI Tree Node")]
        public InArgument<string> NodeKey { get; set; }

        //Test

        public CollapseNode() { 
        
        }


        protected override void Execute(NativeActivityContext context)
        {
            GuiSession sapsession = null;
            string Nodekey = NodeKey.Get(context);
          
            string strAppIdToAttach = ApplicationIDToAttach.Get(context);
            string controlId = ControlId.Get(context);
            SAPEntity entity = null;
            try
            {
                if (SelectHelper.CurrentRuntimeApplicationHelper.RuntimeApplicationObjects.ContainsKey(strAppIdToAttach))
                {

                    entity = (SAPEntity)SelectHelper.CurrentRuntimeApplicationHelper.RuntimeApplicationObjects[strAppIdToAttach];
                    sapsession = entity.SapSession;
                    GuiTree guiTree = (GuiTree)sapsession.FindById(controlId);
                    if (guiTree == null)
                    {
                        throw new NullReferenceException();
                    }

                    if (Nodekey == null ) throw new NullReferenceException();
                    guiTree.CollapseNode(Nodekey);
                 
                    Logger.Log.Logger.LogData("Node collapsed :" + controlId, LogLevel.Info);

                }
            }
            catch (Exception ex)
            {
                Logger.Log.Logger.LogData("Error " + ex.Message, LogLevel.Error);

            }
        }
    }
}
