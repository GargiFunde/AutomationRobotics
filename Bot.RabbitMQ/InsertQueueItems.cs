using Bot.RabbitMQ.Properties;
using System;
using System.Activities;
using System.Activities.Presentation.PropertyEditing;
using System.Activities.Validation;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Logger;
using System.Drawing;

namespace Bot.RabbitMQ
{
    [Designer(typeof(ActivityDesigner1))]
    [ToolboxBitmap("Resources/InsertQueueItems.png")]
    public class InsertQueueItems : NativeActivity
    {

        #region Input
        [Category("Input Parameter")]
        [Description("Enter Queue Name")]
        [DisplayName("Queue Name")]
        [RequiredArgument]
        public InArgument<string> QueueName { get; set; }


        [Category("Input Parameter")]
        [Description("Select Message Priority")]
        [DisplayName("Message Priority")]
        [RequiredArgument]
        [DefaultValue(QueuePriority.Normal)]
        public QueuePriority MessagPriority { get; set; }

        [Category("Input")]
        [DisplayName("Parameters")]
        [Editor(typeof(ArgumentDictionaryEditor), typeof(PropertyValueEditor))]
        [Browsable(true)]
        [RequiredArgument]
        public Dictionary<string, InArgument> ChildArguments { get; set; }

        [Category("Input parameter")]
        [Description("Enter the Tenant Name")]
        [DisplayName("Tenant Name")]
        [RequiredArgument]
        public InArgument<string> TenantName { get; set; }

        [Category("Input parameter")]
        [Description("The Group Name")]
        [DisplayName("Group Name")]
        [RequiredArgument]
        public InArgument<string> GroupName { get; set; }

        #endregion


        
        public  string strMode = string.Empty;
      // public int reloadXaml = 0;
       
        public InsertQueueItems() {

            this.ChildArguments = new Dictionary<string, InArgument>();
            this.MessagPriority = QueuePriority.Normal;
            strMode = ConfigurationManager.AppSettings["Mode"];
            if (strMode.ToLower() == "designer")
            {
              int  reloadXaml = 1;
            }

        }

     
        protected override void CacheMetadata(NativeActivityMetadata metadata)
        {

            //Argument argumentr = Result;
            //RuntimeArgument runtimeArgumentr = new RuntimeArgument("Result", argumentr.ArgumentType, argumentr.Direction);
            //metadata.Bind(argumentr, runtimeArgumentr);
            //metadata.AddArgument(runtimeArgumentr);
          //  ContinueOnError = true; 
            

            foreach (string argumentKey in this.ChildArguments.Keys) {
                Argument argument = this.ChildArguments[argumentKey];
                RuntimeArgument runtimeArgument = new RuntimeArgument(argumentKey, argument.ArgumentType, argument.Direction);
                metadata.Bind(argument, runtimeArgument);
                metadata.AddArgument(runtimeArgument);
            }

            base.CacheMetadata(metadata);
        }
        protected override void Execute(NativeActivityContext context)
        {
            try
            {
                string iRoutingKey = QueueName.Get(context);
                string iQueueName = QueueName.Get(context);
                int iMessagePriority = Convert.ToInt32(MessagPriority);
                string iTenantName = TenantName.Get(context);
                string iGroupName = GroupName.Get(context);


                Dictionary<string, object> childArguments = new Dictionary<string, object>();

                foreach (string index in this.ChildArguments.Keys.AsEnumerable<string>())
                {

                    childArguments[index] = ChildArguments[index].Get((ActivityContext)context);

                }


                Logger.ServiceReference1.BOTServiceClient obj = new Logger.ServiceReference1.BOTServiceClient();
                obj.AutomationRequestPriority(iQueueName,
                                              iRoutingKey,
                                              childArguments,
                                              iMessagePriority,
                                              iTenantName,
                                              iGroupName);
              
             
            }
            catch (Exception ex)
            {

                Logger.Log.Logger.LogData("The Execution Error" + ex.Message, Logger.LogLevel.Error);

            }

        }

        
        private DynamicActivity LoadDynamicActivityFromCache() {


            return null;
        }



        /// <summary>
        /// Validates the arguments in ChildArguments property against the arguments of specified dynamicActivity instance by adding a validation error
        /// to supplied metadata if the argument is wrong type, direction or does not exist.
        /// </summary>
        /// <param name="metadata">The metadata.</param>
        /// <param name="dynamicActivity">The dynamic activity.</param>
        private void Validate(NativeActivityMetadata metadata, DynamicActivity dynamicActivity)
        {
            foreach (string argumentKey in this.ChildArguments.Keys)
            {
                if (dynamicActivity.Properties.Contains(argumentKey))
                {
                    DynamicActivityProperty dynamicActivityProperty = dynamicActivity.Properties[argumentKey];
                    Argument arg = this.ChildArguments[argumentKey];
                    if (dynamicActivityProperty.Type.GetGenericTypeDefinition() == typeof(InArgument<>) && arg.Direction != ArgumentDirection.In)
                    {
                        metadata.AddValidationError(new ValidationError(string.Format(Resources.InvalidInArgumentDirectionValidationErrorText, argumentKey)));
                    }
                    else if (dynamicActivityProperty.Type.GetGenericTypeDefinition() == typeof(OutArgument<>) && arg.Direction != ArgumentDirection.Out)
                    {
                        metadata.AddValidationError(new ValidationError(string.Format(Resources.InvalidOutArgumentDirectionValidationErrorText, argumentKey)));
                    }
                    else if (dynamicActivityProperty.Type.GetGenericTypeDefinition() == typeof(InOutArgument<>) && arg.Direction != ArgumentDirection.InOut)
                    {
                        metadata.AddValidationError(new ValidationError(string.Format(Resources.InvalidInOutArgumentDirectionValidationErrorText, argumentKey)));
                    }

                    if (dynamicActivityProperty.Type.GetGenericArguments()[0] != arg.ArgumentType)
                    {
                        metadata.AddValidationError(new ValidationError(string.Format(Resources.InvalidIArgumentTypeValidationErrorText, argumentKey, dynamicActivityProperty.Type.GetGenericArguments()[0])));
                    }
                }
                else
                {
                    metadata.AddValidationError(new ValidationError(string.Format(Resources.InvalidIArgumentValidationErrorText, argumentKey)));
                }
            }
        }


        public enum QueuePriority 
        {
            Low = 1,
            Normal = 2,
            High = 3
        }
    }
}
