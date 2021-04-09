using Logger.ServiceReference1;
using Newtonsoft.Json;
using System;
using System.Activities;
using System.ComponentModel;
using System.Drawing;
//using Logger.ServiceReference1;


namespace Bot.RabbitMQ
{
    [Designer(typeof(GetQueueItems1))]
    [ToolboxBitmap("Resources/GetQueueItems.png")]
    public class GetQueueItems : BaseNativeActivity
    {

        [Category("Input Parameter")]
        [Description("Enter Queue Name")]
        [DisplayName("Queue Name")]
        [RequiredArgument]
        public InArgument<string> QueueName { get; set; }

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

       
        [Category("Output parameter")]
        [Description("Result Of Dictionary")]
        [DisplayName("Result")]
        public OutArgument<QueueValue> Result { get; set; }

        protected override void Execute(NativeActivityContext context)
        {
            try
            {
                BOTServiceClient obj = new BOTServiceClient();
                string iQueueName = QueueName.Get(context);
                string iTenantName = TenantName.Get(context);
                string iGroupName = GroupName.Get(context);
                var queueItems = obj.GetQueueItem(iGroupName,iTenantName,iQueueName);
                QueueValue queueObject = JsonConvert.DeserializeObject<QueueValue>(queueItems);
               
                Result.Set(context, queueObject);
            }
            catch (Exception ex)
            {

                Logger.Log.Logger.LogData("The Execution Error" + ex.Message, Logger.LogLevel.Error);

            }
        }
    }

   
}
