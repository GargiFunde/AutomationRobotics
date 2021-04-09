using System.Activities;
using System.ComponentModel;
using System;
using Logger;
using Microsoft.Office.Interop.Outlook;
using System.Drawing;

namespace Bot.Activity.Outlook
{
    [ToolboxBitmap("Resources/Mail_Send.png")]
    [Designer(typeof(Mail_Send_ActivityDesigner))]
    public class Mail_Send : BaseNativeActivity
    {
        [RequiredArgument]
        [Category("Input")]
        [DisplayName("From")]
        [Description("Enter your mail Credentials")]
        public InArgument<string> From { get; set; }

        [Category("Input")]
        [DisplayName("To")]
        [Description("Enter to whom you wish to send E-Mail")]
        public InArgument<string> To { get; set; }

        [Category("Input")]
        [DisplayName("cc")]
        [Description("Enter whom you wish to cc")]
        public InArgument<string> Cc { get; set; }

        [Category("Input")]
        [DisplayName("Bcc")]
        [Description("Enter whom you wish to Bcc")]
        public InArgument<string> Bcc { get; set; }

        [Category("Input")]
        [DisplayName("Subject")]
        [Description("Enter Subject for E-Mail")]
        public InArgument<string> Subject { get; set; }

        [Category("Input")]
        [DisplayName("Body")]
        [Description("Enter Body for E-Mail")]
        public InArgument<string> Body { get; set; }

        [Category("Input")]
        [DisplayName("Attachment")]
        [Description("Attach All Attachments")]
        public InArgument<string> Attachment { get; set; }

        [Category("Input")]
        [DisplayName("IsHtmlBody")]
        [Description("IsHtmlBody in Boolean")]
        public InArgument<bool> IsHtmlBody { get; set; }

        [Category("Input")]
        [DisplayName("IsDraft")]
        [Description("Wheather you wish to draft in Boolean")]
        public InArgument<bool> IsDraft { get; set; }

        protected override void Execute(NativeActivityContext context)
        {
            try
            {
                string from = string.Empty;
                string to = string.Empty;
                string cc = string.Empty;
                string bcc = string.Empty;
                string subject = string.Empty;
                string body = string.Empty;
                string attachment = string.Empty;
                bool isHtmlBody = false;
                bool isdraft = false;

                if (From != null)
                {
                    from = From.Get(context);
                }
                if (To != null)
                {
                    to = To.Get(context);
                }
                if (Cc != null)
                {
                    cc = Cc.Get(context);
                }

                if (Bcc != null)
                {
                    bcc = Bcc.Get(context);
                }

                if (Subject != null)
                {
                    subject = Subject.Get(context);
                }
                if (Body != null)
                {
                    body = Body.Get(context);
                }
                if (Attachment != null)
                {
                    attachment = Attachment.Get(context);
                }
                if (IsHtmlBody != null)
                {
                    isHtmlBody = IsHtmlBody.Get(context);
                }
                if (IsDraft != null)
                {
                    isdraft = IsDraft.Get(context);
                }
                //  dynamic app = Activator.CreateInstance(Type.GetTypeFromProgID("Outlook.Application"));

                dynamic app;
                try
                {
                    app = Activator.CreateInstance(Type.GetTypeFromProgID("Outlook.Application"));
                }
                catch (System.Exception ex1)
                {
                    app = new Application();
                }
         
                var mailItem = app.CreateItem(0 /*  OlItemType.olMailItem */);

                if (from.ToString().Length > 0)
                {
                    var fromAccount = app.Session.Accounts.Item(from.ToString());
                    if (fromAccount == null)
                    {
                       //throw new Exception("Cannot find " + from + " account.");
                        throw new System.Exception("Cannot find " + from + " account.");
                    }
                    mailItem.SendUsingAccount = fromAccount;
                }

                mailItem.To = to;
                mailItem.CC = cc;
                mailItem.BCC = bcc;
                mailItem.Subject = subject;
                if (isHtmlBody)
                {
                    mailItem.HTMLBody = body;
                }
                else
                {
                    mailItem.Body = body;
                }
                if ((attachment != null) && (attachment.ToString().Length > 0))
                {
                    mailItem.Attachments.Add(attachment.ToString(), 1 /* OlAttachmentType.olByValue */);
                }
                if (isdraft)
                {
                    mailItem.Display();
                }
                else
                {
                    mailItem.Send();
                }
     
            }
            catch (System.Exception ex)
            {
                Log.Logger.LogData("Activity Mail Exception: "+ex.Message , LogLevel.Error);
                if (!ContinueOnError)
                {
                    context.Abort();
                }
            }
        }
    }
}
