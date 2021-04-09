using System;
using System.Runtime.InteropServices;
using Microsoft.Office.Interop.Outlook;
using System.Activities;
using System.ComponentModel;
using System.IO;
using Logger;

using System.Net.Mail;
using OpenPop.Pop3;
using OpenPop.Mime;
using Pop3Client = OpenPop.Pop3.Pop3Client;
using System.Reflection;
using System.Drawing;

namespace Bot.Activity.Outlook
{
    [ToolboxBitmap("Resources/Mail_Receive.png")]
    [Designer(typeof(Mail_Receive_ActivityDesigner))]
    public class Mail_Receive: BaseNativeActivity
    {
        [Category("Input")]
        public InArgument<string> Account { get; set; }
        [Category("Input")]
        public InArgument<string> Password { get; set; }
        [Category("Options")]
        public InArgument<int> EmailNumber { get; set; }
        [Category("Options")]
        public InArgument<string> AttachmentDownloadPath { get; set; }
        [Category("Filter")]
        public Boolean UnRead { get; set; }

        [Category("Filter")]
        public InArgument<string> Subject { get; set; }
        [Category("Output")]

        public OutArgument<Items> Result { get; set; }
        [Category("Output")]

        public OutArgument<String> EmailSubject { get; set; }
        [Category("Output")]

        public OutArgument<String> To { get; set; }
        [Category("Output")]

        public OutArgument<String> Body { get; set; }
        [Category("Output")]

        public OutArgument<String> From { get; set; }

        public Mail_Receive()
        {
            UnRead = true;
        }

        Application outlookApplication = null;
        NameSpace outlookNamespace = null;
        MAPIFolder inboxFolder = null;
        Items mailItems1 = null;
        Items mailItems2 = null;
        string filePath = null;
        int count = 0;

        protected override void Execute(NativeActivityContext context)
        {
            Boolean check = login(Account.Get(context), Password.Get(context));
            string destinationDirectory = @AttachmentDownloadPath.Get(context);
            string Query = Subject.Get(context);


            if (EmailNumber.Get(context) != 0)
            {
                count = EmailNumber.Get(context);
            }


            try
            {
                outlookApplication = new Application();
                outlookNamespace = outlookApplication.GetNamespace("MAPI");
                inboxFolder = outlookNamespace.GetDefaultFolder(OlDefaultFolders.olFolderInbox);

                if (UnRead == false)
                {                                                                       //read read mails

                    mailItems1 = inboxFolder.Items;
                    if (EmailNumber.Get(context) == 0)
                    {
                        count = mailItems1.Count;
                    }

                }
                else
                {                                                                       //read unread mails here
                    mailItems1 = inboxFolder.Items.Restrict("[UnRead] = true");
                    if (EmailNumber.Get(context) == 0)
                    {
                        count = mailItems1.Count;
                    }
                }

                if (Query != null)
                {                                                                          //if subject is mentioned

                    mailItems1 = inboxFolder.Items.Restrict("[Subject] = '" + Query + "'");
                    Log.Logger.LogData("\n count is ## " + mailItems1.Count, LogLevel.Info);
                    if (EmailNumber.Get(context) == 0)
                    {
                        count = mailItems1.Count;
                    }
                }

                if (destinationDirectory != null)
                {
                    if (mailItems1[count].Attachments.Count > 0)                                    //Check for Attachments
                    {
                        for (int i = 1; i <= mailItems1[count].Attachments.Count; i++)
                        {
                            filePath = Path.Combine(destinationDirectory, mailItems1[count].Attachments[i].FileName);
                            mailItems1[count].Attachments[i].SaveAsFile(filePath);
                        }
                        Log.Logger.LogData("\n Attachment downloaded to path  " + filePath, LogLevel.Info);

                    }
                }


                Result.Set(context, mailItems1);
                EmailSubject.Set(context, mailItems1[count].Subject.ToString());
                To.Set(context, mailItems1[count].To.ToString());
                Body.Set(context, mailItems1[count].Body.ToString());
                From.Set(context, mailItems1[count].SenderEmailAddress.ToString());


                //Log.Logger.LogData("\n## Message ##  from " + mailItems1[count].SenderEmailAddress, LogLevel.Info);
                //Log.Logger.LogData("\n## Message ## To " + mailItems1[count].To, LogLevel.Info);
                //Log.Logger.LogData("\n## Message ## Subject  " + mailItems1[count].Subject, LogLevel.Info);
                //Log.Logger.LogData("\n## Message ## Body   " + mailItems1[count].Body, LogLevel.Info);
                Marshal.ReleaseComObject(mailItems1);

            }
            //Error handler.
            catch (System.Exception e)
            {
                Log.Logger.LogData("{0} Exception caught: " + e, LogLevel.Error);
            }
            finally
            {
                ReleaseComObject(mailItems1);
                ReleaseComObject(mailItems2);
                ReleaseComObject(inboxFolder);
                ReleaseComObject(outlookNamespace);
                ReleaseComObject(outlookApplication);
            }
        }
        private static void ReleaseComObject(object obj)
        {
            if (obj != null)
            {
                Marshal.ReleaseComObject(obj);
                obj = null;
            }
        }

        public static Boolean login(String username, string password)
        {
            Pop3Client pop = new Pop3Client();
            //Set host, username, password etc. for the client  

            //Connect the server  
            pop.Connect("outlook.office365.com", 995, true);
            pop.Authenticate(username, password);
            return true;
        }
    }
}
