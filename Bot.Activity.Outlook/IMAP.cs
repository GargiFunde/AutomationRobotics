using System;
using System.Activities;
using System.Collections.Generic;
using MimeKit;
using MailKit;
using MailKit.Search;
using MailKit.Security;
using MailKit.Net.Imap;
using System.Drawing;
using System.ComponentModel;

namespace Bot.Activity.Outlook
{
    [ToolboxBitmap("Resources/IMAP.png")]
    [Designer(typeof(IMAP_ActivityDesigner))]
    public class IMAP : BaseNativeActivity
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
        catch (Exception ex)
        {
                Logger.Log.Logger.LogData("Exception in IMAP Activity: "+ex.Message,Logger.LogLevel.Error);
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
