using Logger;
using System;
using System.Activities;
using System.ComponentModel;
using System.Drawing;
using System.Net.Mail;
using System.Net.Mime;

namespace Bot.Activity.SMTP
{
    [Designer(typeof(SMTP_MailSend_ActivityDesigner))]
    [ToolboxBitmap("Resources/SMTPMailSend.png")]
    public class SMTP_MailSend : BaseNativeActivity
    {
        [Category("Input Paramaters")]
        [DisplayName("From")]
        [Description("The email address of sender")]
        [RequiredArgument]
        public InArgument<string> From { get; set; }

        [Category("Input Paramaters")]
        [DisplayName("Host")]
        [Description("Set Host eg.smtp-mail.outlook.com")]
        [RequiredArgument]
        public InArgument<string> Host { get; set; }

        [Category("Input Paramaters")]
        [DisplayName("UserName")]
        [Description("User name of sender")]
        [RequiredArgument]
        public InArgument<string> UserName { get; set; }

        [Category("Input Paramaters")]
        [DisplayName("Password")]
        [Description("The Password of the email account used to send message")]
        [RequiredArgument]
        public InArgument<string> Password { get; set; }

        [Category("Input Paramaters")]
        [DisplayName("To")]
        [Description("The main recipients of the email message")]
        [RequiredArgument]
        public InArgument<string> To { get; set; }

        [Category("Input Paramaters")]
        [DisplayName("Subject")]
        [Description("The subject of the email message")]
        [RequiredArgument]
        public InArgument<string> Subject { get; set; }

        [Category("Input Paramaters")]
        [DisplayName("Body")]
        [Description("The body of the email message")]
        public InArgument<string> Body { get; set; }

        [Category("Input Paramaters")]
        [DisplayName("Cc")]
        [Description("The secondary recipients of the email message")]
        public InArgument<string> Cc { get; set; }

        [Category("Input Paramaters")]
        [DisplayName("Bcc")]
        [Description("The hidden recipients of the email message")]
        public InArgument<string> Bcc { get; set; }

        [Category("Input Paramaters")]
        [DisplayName("Attachment")]
        [Description("Set Attachment")]
        public InArgument<string> Attachment { get; set; }

        [Category("Input Paramaters")]
        [DisplayName("Port")]
        [Description("Set Port No.")]
        public InArgument<int> Port { get; set; }

        [Category("Input Paramaters")]
        public bool IsHtmlBody { get; set; }

        public SMTP_MailSend()
        {
            this.DisplayName = "SMTP Mail Send";
            ContinueOnError = true;
            this.Port = 587;
        }

        protected override void Execute(NativeActivityContext context)
        {
            try
            {
                string bccStr = Bcc.Get(context);
                string tostr = To.Get(context);
                string subjectStr = Subject.Get(context);
                string CcStr = Cc.Get(context);
                string bodyStr = Body.Get(context);
                string fromStr = From.Get(context);
                string hostStr = Host.Get(context);
                int portStr = Port.Get(context);
                string attachment = Attachment.Get(context);
              
                var mailMessage = new MailMessage();
                mailMessage.To.Add(tostr);
                mailMessage.Subject = subjectStr;

                if (!string.IsNullOrEmpty(CcStr))
                {
                    mailMessage.CC.Add(CcStr);
                }

                if (!string.IsNullOrEmpty(bccStr))
                {
                    mailMessage.Bcc.Add(bccStr);
                }

                if (!string.IsNullOrEmpty(attachment))
                {
                    // Create  the file attachment for this e-mail message.
                    Attachment data = new Attachment(attachment, MediaTypeNames.Application.Octet);
                    // Add time stamp information for the file.
                    ContentDisposition disposition = data.ContentDisposition;
                    disposition.CreationDate = System.IO.File.GetCreationTime(attachment);
                    disposition.ModificationDate = System.IO.File.GetLastWriteTime(attachment);
                    disposition.ReadDate = System.IO.File.GetLastAccessTime(attachment);
                    // Add the file attachment to this e-mail message.
                    mailMessage.Attachments.Add(data);
                }

                if (true == IsHtmlBody)
                {
                    mailMessage.IsBodyHtml = true;
                }

                if (!string.IsNullOrEmpty(bodyStr))
                {
                    mailMessage.Body = bodyStr;
                }
                

                mailMessage.From = new MailAddress(fromStr);
                SmtpClient smtp = new SmtpClient();
                smtp.Host = hostStr;
                smtp.Port = portStr;

                smtp.Credentials = new System.Net.NetworkCredential(UserName.Get(context), Password.Get(context));
                smtp.EnableSsl = true;
                smtp.Send(mailMessage);
            }
            catch (Exception ex)
            {
                Log.Logger.LogData(ex.Message + " in activity SMTP Mail Send", LogLevel.Error);
                if (!ContinueOnError) { context.Abort(); }
            }

        }

    }
}
