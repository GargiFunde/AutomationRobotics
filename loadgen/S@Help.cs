// <copyright file=S@Help company=E2E BOTS>
// Copyright (c) 2018 All Rights Reserved
// </copyright>
// <author>Prabodhini Funde</author>
// <date> 03-10-2018 16:03:08</date>
// <summary></summary>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataConnectorToGenerateRequest
{
    class S_Help
    {
//        Connecting to an IMAP server using SSL

//using System;
//using S22.Imap;

//namespace Test {
//    class Program {
//        static void Main(string[] args)
//        {
//            using (ImapClient Client = new ImapClient("imap.gmail.com", 993,
//             "username", "password", AuthMethod.Login, true))
//            {
//                Console.WriteLine("We are connected!");
//            }
//        }
//    }
//}
//Download unseen mail messages

//using System;
//using S22.Imap;

//namespace Test {
//    class Program {
//        static void Main(string[] args)
//        {
//            using (ImapClient Client = new ImapClient("imap.gmail.com", 993,
//             "username", "password", AuthMethod.Login, true))
//            {
//                IEnumerable<uint> uids = Client.Search( SearchCondition.Unseen() );
//                IEnumerable<MailMessage> messages = Client.GetMessages(uids);
//            }
//        }
//    }
//}
//Search for messages

//using System;
//using S22.Imap;

//namespace Test {
//    class Program {
//        static void Main(string[] args)
//        {
//            using (ImapClient Client = new ImapClient("imap.gmail.com", 993,
//             "username", "password", AuthMethod.Login, true))
//            {
//                // Find messages that were sent from abc@def.com and have
//                // the string "Hello World" in their subject line.
//                IEnumerable<uint> uids = Client.Search(
//                    SearchCondition.From("abc@def.com").And(
//                    SearchCondition.Subject("Hello World"))
//                );
//            }
//        }
//    }
//}
//Figure out the amount of free space left for the inbox

//This is not supported by all IMAP servers and some may just return 0

//using System;
//using S22.Imap;

//namespace Test {
//    class Program {
//        static void Main(string[] args)
//        {
//            using (ImapClient Client = new ImapClient("imap.gmail.com", 993,
//             "username", "password", AuthMethod.Login, true))
//            {
//                MailboxStatus status = Client.GetStatus();

//                Console.WriteLine(status.FreeStorage + " Bytes left");
//                Console.WriteLine(status.UsedStorage + " Bytes used");
//            }
//        }
//    }
//}
//Receive IMAP IDLE notifications

//using System;
//using S22.Imap;

//namespace Test {
//    class Program {
//        static void Main(string[] args)
//        {
//            using (ImapClient Client = new ImapClient("imap.gmail.com", 993,
//             "username", "password", AuthMethod.Login, true))
//            {
//                // Should ensure IDLE is actually supported by the server
//                if(Client.Supports("IDLE") == false) {
//                    Console.WriteLine("Server does not support IMAP IDLE");
//                    return;
//                }

//                // We want to be informed when new messages arrive
//                Client.NewMessage += new EventHandler<IdleMessageEventArgs>(OnNewMessage);

//                // Put calling thread to sleep. This is just so the example program does
//                // not immediately exit.
//                System.Threading.Thread.Sleep(60000);
//            }
//        }

//        static void OnNewMessage(object sender, IdleMessageEventArgs e)
//        {
//            Console.WriteLine('A new message arrived. Message has UID: ' +
//                e.MessageUID);

//            // Fetch the new message's headers and print the subject line
//            MailMessage m = e.Client.GetMessage( e.MessageUID, FetchOptions.HeadersOnly );

//            Console.WriteLine("New message's subject: " + m.Subject);
//        }
//    }
//}
//Download mail headers only instead of the entire mail message

//using System;
//using S22.Imap;

//namespace Test {
//    class Program {
//        static void Main(string[] args)
//        {
//            using (ImapClient Client = new ImapClient("imap.gmail.com", 993,
//             "username", "password", AuthMethod.Login, true))
//            {
//                // This returns *ALL* messages in the inbox.
//                IEnumerable<uint> uids = Client.Search( SearchCondition.All() );

//                // If we're only interested in the subject line or envelope
//                // information, just downloading the mail headers is alot
//                // cheaper and alot faster.
//                IEnumerable<MailMessage> messages = Client.GetMessages(uids. FetchOptions.HeadersOnly);
//            }
//        }
//    }
//}
//Download attachments only if they are smaller than 2 Megabytes

//using System;
//using S22.Imap;

//namespace Test {
//    class Program {
//        static void Main(string[] args)
//        {
//            using (ImapClient Client = new ImapClient("imap.gmail.com", 993,
//             "username", "password", AuthMethod.Login, true))
//            {
//                // This returns all messages sent since August 23rd 2012.
//                IEnumerable<uint> uids = Client.Search(
//                    SearchCondition.SentSince( new DateTime(2012, 8, 23) )
//                );

//                // The expression will be evaluated for every MIME part
//                // of every mail message in the uids collection.
//                IEnumerable<MailMessage> messages = Client.GetMessages(uids,
//                    (Bodypart part) => {
//                     // We're only interested in attachments.
//                     if(part.Disposition.Type == ContentDispositionType.Attachment)
//                     {
//                        Int64 TwoMegabytes = (1024 * 1024 * 2);
//                        if(part.Size > TwoMegabytes)
//                        {
//                            // Don't download this attachment
//                            return false;
//                        }
//                     }

//                     // Fetch MIME part and include it in the returned MailMessage instance.
//                     return true;
//                    }
//                );
//            }
//        }
//    }
//}
//Download attachments only if they are zip archives, otherwise skip them

//using System;
//using S22.Imap;

//namespace Test {
//    class Program {
//        static void Main(string[] args)
//        {
//            using (ImapClient Client = new ImapClient("imap.gmail.com", 993,
//             "username", "password", AuthMethod.Login, true))
//            {
//                // This returns all messages sent since August 23rd 2012.
//                IEnumerable<uint> uids = Client.Search(
//                    SearchCondition.SentSince( new DateTime(2012, 8, 23) )
//                );

//                // The expression will be evaluated for every MIME part
//                // of every mail message in the uids collection.
//                IEnumerable<MailMessage> messages = Client.GetMessages(uids,
//                    (Bodypart part) => {
//                        // We're only interested in attachments.
//                        if(part.Disposition.Type == ContentDispositionType.Attachment)
//                        {
//                            // Zip files have a content-type of application/zip.
//                            if(part.Type == ContentType.Application &&
//                               part.Subtype.toLower() == "zip")
//                            {
//                                return true;
//                            }
//                            else
//                            {
//                                // Skip this attachment, it's not a zip archive.
//                                return false;
//                            }
//                        }

//                        // Fetch MIME part and include it in the returned MailMessage instance.
//                        return true;
//                    }
//                );
//            }
//        }
//    }
//}
//Store mail message on an IMAP server

//using System;
//using System.Net.Mail;
//using S22.Imap;

//namespace Test {
//    class Program {
//        static void Main(string[] args)
//        {
//            using (ImapClient Client = new ImapClient("imap.gmail.com", 993,
//             "username", "password", AuthMethod.Login, true))
//            {
//                MailMessage m = CreateSimpleMailMessage();

//                uint uid = Client.StoreMessage(m);
//                Console.WriteLine("The UID of the stored mail message is " + uid);
//            }
//        }

//        // This creates a simple mail message with a text/plain body and a PNG image
//        // as a file attachment.
//        // Consult the MSDN website for details on the System.Net.Mail.Mailmessage class
//        static MailMessage CreateSimpleMailMessage() {
//            MailMessage message = new MailMessage();

//            message.From = new MailAddress("someone@someplace.com");
//            message.To.Add("john.doe@someplace.com");

//            message.Subject = "This is just a test!";
//            message.Body = "This is the text/plain body. An additional HTML body " +
//                    "can optionally be attached as an alternate view";

//            // Add the attachment
//            Attachment attachment = new Attachment("some_image.png", "image/png");
//            attachment.Name = "my_attached_image.png";
//            message.Attachments.Add(attachment);

//            return message;
//        }
//    }
//}
    }
}
