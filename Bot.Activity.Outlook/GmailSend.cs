//using System;
//using System.Activities;
//using System.Collections.Generic;
//using System.ComponentModel;
//using System.Linq;
//using System.Net;
//using System.Net.Mail;
//using System.Text;
//using System.Threading.Tasks;

//namespace Bot.Activity.Outlook
//{
//    public class GmailSend : BaseNativeActivity
//    {
//        [RequiredArgument]
//        [Category("Input")]
//        [DisplayName("from")]
//        [Description("Enter E-mail ID to whom you wish to send E-mail")]
//        public InArgument<string> from { get; set; }

//        [RequiredArgument]
//        [Category("Input")]
//        [DisplayName("host")]
//        [Description("Enter host")]
//        public InArgument<string> host { get; set; }

//        [RequiredArgument]
//        [Category("Input")]
//        [DisplayName("User Name")]
//        [Description("Enter User Name")]
//        public InArgument<string> userName { get; set; }

//        [RequiredArgument]
//        [Category("Input")]
//        [DisplayName("Pass-Word")]
//        [Description("Enter Pass word")]
//        public InArgument<string> password { get; set; }

//        [RequiredArgument]
//        [Category("Input")]
//        [DisplayName("To")]
//        [Description("Enter E-Mail Id of user you wish to send E-mail")]
//        public InArgument<string> to { get; set; }
//        [Category("Input")]
//        [DisplayName("Subject")]
//        [Description("Enter Subject")]
//        public InArgument<string> subject { get; set; }
//        [Category("Input")]
//        [DisplayName("Body")]
//        [Description("Enter Body")]
//        public InArgument<string> body { get; set; }
//        [Category("Input")]
//        [DisplayName("Result")]
//        [Description("Enter Result")]
//        public OutArgument<string> result { get; set; }
//        [Category("Input")]
//        [DisplayName("Attachment")]
//        [Description("Attach All Attachments")]
//        public InArgument<string> Attachment { get; set; }
//        protected override void Execute(NativeActivityContext context)
//        {
//            //string attachment 


//            var mailMessage = new System.Net.Mail.MailMessage();
//            mailMessage.To.Add(to.Get(context).ToString());
//            mailMessage.Subject = subject.Get(context).ToString();
//            mailMessage.Body = body.Get(context);
//            mailMessage.From =
//            new System.Net.Mail.MailAddress(from.Get(context));

//            var use = new MailAddress(userName.Get(context));
//            var pass = password.Get(context);
//            var toadd = new MailAddress(to.Get(context).ToString());

//            System.Net.Mail.SmtpClient smtp = new System.Net.Mail.SmtpClient {
//                Host = host.Get(context),
//                Port = 587,
//                EnableSsl = true,
//                DeliveryMethod = System.Net.Mail.SmtpDeliveryMethod.Network,
//                UseDefaultCredentials = false,
//                Credentials = new NetworkCredential(use.Address, pass)
//            };

//            using (var message = new MailMessage(use, toadd)
//            {
//                Subject = mailMessage.Subject,
//                Body = mailMessage.Body
//            })
//                smtp.Send(mailMessage);
//            result.Set(context, "Sent Email Successfully!");
//        }
//    }
//}
using System;
using System.Activities;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MimeKit;
using MailKit;
using MailKit.Search;
using MailKit.Security;
using MailKit.Net.Imap;
using System.Net.Mime;

namespace Bot.Activity.IMAP
{
    class ReceiveMail : NativeActivity
    {
        public InArgument<string> Server { get; set; }
        public InArgument<int> Port { get; set; }

        public InArgument<string> Email { get; set; }
        public InArgument<string> Password { get; set; }

        public SecureSocketOptions Option { get; set; }

        public bool OnlyUnreadMessage { get; set; }
        public InArgument<int> Top { get; set; }
        public InArgument<string> BySubject { get; set; }

        public OutArgument<List<MimeMessage>> Output { get; set; }


        /*Please View the soap Activity Code  The is method called Override cachemetadata(NativeActivity  Context){}
          in that method result argument is addd to cahe data do it same for the Output. In Case this code does not work.
             */

        protected override void Execute(NativeActivityContext context)
        {
            try
            {
                string server = Server.Get(context);
                int port = Port.Get(context);
                string email = Email.Get(context);
                string password = Password.Get(context);
                SecureSocketOptions op = Option;

                int top = Top.Get(context);
                string bySubject = BySubject.Get(context).Trim();
                List<MimeMessage> messages = new List<MimeMessage>();

                using (var client = new ImapClient())
                {

                    client.Connect(server, port, op);
                    client.Authenticate(email, password);
                    client.Inbox.Open(FolderAccess.ReadOnly);


                    var uids = GetUIDS(bySubject, client);
                    int count = 0;
                    foreach (var uid in uids)
                    {
                        if (count != 0)
                        {
                            if (count < top)
                            {
                                var msg = client.Inbox.GetMessage(uid);
                                //add collection
                                messages.Add(msg);
                                ++count;
                            }
                            else
                            {
                                break;
                            }
                        }
                        else
                        {
                            var msg = client.Inbox.GetMessage(uid);
                            messages.Add(msg);
                        }

                    }

                    Output.Set(context, messages);//assigning collection to out argument 
                }
            }
            catch (Exception)
            {

                throw;
            }



        }



        private IList<UniqueId> GetUIDS(string bySubject, ImapClient client)
        {

            if (OnlyUnreadMessage)
            {
                if (bySubject != null)
                {
                    var uids1 = client.Inbox.Search(SearchQuery.NotSeen.And(SearchQuery.SubjectContains(bySubject)));
                    return uids1;
                }
                else
                {
                    var uids2 = client.Inbox.Search(SearchQuery.NotSeen);
                    return uids2;
                }
            }
            else
            {
                if (bySubject != null)
                {
                    var uids3 = client.Inbox.Search(SearchQuery.All.And(SearchQuery.SubjectContains(bySubject)));
                    return uids3;
                }
                else
                {
                    var uids4 = client.Inbox.Search(SearchQuery.All);
                    return uids4;
                }
            }


        }
    }
}

