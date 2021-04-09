using CommonLibrary;
using System;
using System.Activities;
using WatiN.Core;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Drawing;

namespace Bot.Activity.InternetExplorer
{
    [ToolboxBitmap("Resources/FindElementExists.png")]
    [Designer(typeof(ActivityDesignerForFindElementExists))]
    public class FindElementExists: BaseNativeActivity
    {
        [RequiredArgument]
        [Category("Input")]
        [DisplayName("Application ID To Attach")]
        [System.ComponentModel.Description("Enter Application ID To Attach")]
        public InArgument<string> ApplicationIDToAttach { get; set; }

        [Category("Input")]
        [DisplayName("ID")]
        [System.ComponentModel.Description("Enter ID")]
        public InArgument<string> ID { get; set; }

        [Category("Input")]
        [DisplayName("Class")]
        [System.ComponentModel.Description("Enter Class")]
        public InArgument<string> Class { get; set; }

        [Category("Input")]
        [DisplayName("Name")]
        [System.ComponentModel.Description("Enter Name")]
        public InArgument<string> Name { get; set; }

        [Category("Input")]
        [DisplayName("Custom Attribute")]
        [System.ComponentModel.Description("Enter Custom Attribute")]
        public InArgument<string> CustomAttribute { get; set; }

        [Category("Output")]
        [DisplayName("Output Value")]
        [System.ComponentModel.Description("Enter Output Variable(Boolean)")]
        public OutArgument<Boolean> Result { get; set; }

        protected override void Execute(NativeActivityContext context)
        {
            string AppId = string.Empty;
            string strID = string.Empty;
            string strClass = string.Empty;
            string strName = string.Empty;
            string strAttribute = string.Empty;
            bool boolResult = false;
            try
            {
                AppId = ApplicationIDToAttach.Get(context);
                strID = ID.Get(context);
                strClass = Class.Get(context);
                strName = Name.Get(context);
                strAttribute = CustomAttribute.Get(context);
                boolResult = false;

                if (AppId != string.Empty) //scraping time
                {
                    if (SelectHelper.CurrentRuntimeApplicationHelper.RuntimeApplicationObjects.ContainsKey(AppId))
                    {
                        WatiN.Core.IE IEWATIN = (WatiN.Core.IE)SelectHelper.CurrentRuntimeApplicationHelper.RuntimeApplicationObjects[AppId];

                        if (!string.IsNullOrEmpty(strID) && IEWATIN.Elements.Exists(Find.ById(strID)))
                        {
                            boolResult = true;
                        }
                        if (!string.IsNullOrEmpty(strClass) && IEWATIN.Elements.Exists(Find.ByClass(strClass)))
                        {
                            boolResult = true;
                        }
                        if (!string.IsNullOrEmpty(strName) &&  IEWATIN.Elements.Exists(Find.ByName(strName)))
                        {
                            boolResult = true;
                        }

                        if (!string.IsNullOrEmpty(strAttribute) )
                        {
                            string[] strAttributeValues = strAttribute.Split('=');
                            if (strAttributeValues.Length > 1)
                            {
                                string strAttributeName = strAttributeValues[0].Trim();
                                string strAttributeValue = strAttributeValues[1].Trim();

                                if (!string.IsNullOrEmpty(strAttributeName) && !string.IsNullOrEmpty(strAttributeValue))
                                {
                                    if (IEWATIN.Elements.Exists(Find.By(strAttributeName, strAttributeValue)))
                                    {
                                        boolResult = true;
                                    }
                                }
                                else
                                {
                                    Logger.Log.Logger.LogData("Attribute Name Or Value should not be Empty.",Logger.LogLevel.Info);
                                }
                            }
                        }

                        Result.Set(context, boolResult);


                    }
                }
                else
                {
                    Result.Set(context, false);
                }

            }
            catch (Exception ex)
            {
                Logger.Log.Logger.LogData("Exception in Find Element"+ex.Message , Logger.LogLevel.Error);

                if (!ContinueOnError)
                {
                    context.Abort();
                }
            }
        }
    }
}
