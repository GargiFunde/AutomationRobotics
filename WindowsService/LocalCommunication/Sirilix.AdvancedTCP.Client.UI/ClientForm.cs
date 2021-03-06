// <copyright file=ClientForm company=E2E Robotics>
// Copyright (c) 2018 All Rights Reserved
// </copyright>
// <author>Saurabh Mundhe</author>
// <date> 03-10-2018 16:03:06</date>
// <summary></summary>

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TCP.Client.UI
{
    public partial class ClientForm : Form
    {
        private TCP.Client.Client client;

        public ClientForm()
        {
            client = new Client();

            InitializeComponent();
            RegisterEvents();
        }

        private void RegisterEvents()
        {
            //this
           // btnConnect.Click += btnConnect_Click;
         //   btnLogin.Click += btnLogin_Click;
          //  btnStartSession.Click += btnStartSession_Click;
          //  btnRemoteDesktop.Click += btnRemoteDesktop_Click;
            btnSendMessage.Click += btnSendMessage_Click;
           // btnUploadFile.Click += btnUploadFile_Click;
            btnDisconnect.Click += btnDisconnect_Click;
            //btnCalc.Click += btnCalc_Click;
           // btnEndSession.Click += btnEndSession_Click;
            this.FormClosing += ClientForm_FormClosing;

            //client
            //client.SessionRequest += client_SessionRequest;
            client.TextMessageReceived += client_TextMessageReceived;
            //client.FileUploadRequest += client_FileUploadRequest;
            //client.FileUploadProgress += client_FileUploadProgress;
            client.ClientDisconnected += client_ClientDisconnected;
            //client.SessionClientDisconnected += client_SessionClientDisconnected;
            //client.GenericRequestReceived += client_GenericRequestReceived;
            //client.SessionEndedByTheRemoteClient += client_SessionEndedByTheRemoteClient;
        }

        //void btnEndSession_Click(object sender, EventArgs e)
        //{
        //    client.EndCurrentSession((senderClient, response) =>
        //    {

        //        InvokeUI(() =>
        //        {
        //            if (!response.HasError)
        //            {
        //                btnEndSession.Enabled = false;
        //                btnStartSession.Enabled = true;
        //                btnCalc.Enabled = false;
        //                btnRemoteDesktop.Enabled = false;
        //                btnSendMessage.Enabled = false;
        //                btnUploadFile.Enabled = false;
        //            }
        //            else
        //            {
        //                Status(response.Exception.ToString());
        //            }
        //        });
        //    });
        //}

        //void client_SessionEndedByTheRemoteClient(Client client)
        //{
        //    InvokeUI(() =>
        //    {
        //        MessageBox.Show(this, "Sesion ended by the remote client.", this.Text);
        //      //  btnEndSession.Enabled = false;
        //      //  btnStartSession.Enabled = true;
        //      //  btnCalc.Enabled = false;
        //      //  btnRemoteDesktop.Enabled = false;
        //        btnSendMessage.Enabled = false;
        //      //  btnUploadFile.Enabled = false;
        //    });
        //}

        //void client_GenericRequestReceived(Client client, Shared.Messages.GenericRequest msg)
        //{
        //    if (msg.GetType() == typeof(MessagesExtensions.CalcMessageRequest))
        //    {
        //        MessagesExtensions.CalcMessageRequest request = msg as MessagesExtensions.CalcMessageRequest;

        //        MessagesExtensions.CalcMessageResponse response = new MessagesExtensions.CalcMessageResponse(request);
        //        response.Result = request.A + request.B;
        //        client.SendGenericResponse(response);
        //    }
        //}

        //private void btnCalc_Click(object sender, EventArgs e)
        //{
        //    MessagesExtensions.CalcMessageRequest request = new MessagesExtensions.CalcMessageRequest();
        //    request.A = 10;
        //    request.B = 5;

        //    client.SendGenericRequest<MessagesExtensions.CalcMessageResponseDelegate>(request, (clientSender, response) =>
        //    {

        //        InvokeUI(() =>
        //        {

        //            MessageBox.Show(this, response.Result.ToString(), this.Text);

        //        });

        //    });
        //}

        void ClientForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (client.Status == Shared.Enums.StatusEnum.Connected)
            {
                client.Disconnect();
            }
        }

        //void client_SessionClientDisconnected(Client obj)
        //{
        //    InvokeUI(() =>
        //    {
        //        btnSendMessage.Enabled = false;
        //        //btnEndSession.Enabled = false;
        //        //btnRemoteDesktop.Enabled = false;
        //       // btnCalc.Enabled = false;
        //        //btnUploadFile.Enabled = false;
        //    });

        //    Status("The remote session client was disconnected!");
        //}

        void client_ClientDisconnected(Client obj)
        {
            InvokeUI(() =>
            {
                btnDisconnect.Enabled = false;
                btnLogin.Enabled = false;
               // btnSendMessage.Enabled = false;
               // btnRemoteDesktop.Enabled = false;
               // btnCalc.Enabled = false;
               // btnStartSession.Enabled = false;
               // btnUploadFile.Enabled = false;
                btnConnect.Enabled = true;
              //  btnEndSession.Enabled = false;
            });

            Status("The client was disconnected!");
        }

        void btnDisconnect_Click(object sender, EventArgs e)
        {
            client.Disconnect();
        }

        //void client_FileUploadProgress(Client client, EventArguments.FileUploadProgressEventArguments args)
        //{
        //    Status("Downloading " + args.FileName + " To " + args.DestinationPath + ", " + ((args.CurrentPosition * 100) / args.TotalBytes) + "%...");

        //    if (args.CurrentPosition >= args.TotalBytes)
        //    {
        //        Status("Downloading " + args.FileName + " Completed!");
        //    }
        //}

        //void client_FileUploadRequest(Client client, EventArguments.FileUploadRequestEventArguments args)
        //{
        //    InvokeUI(() =>
        //    {
        //        if (MessageBox.Show(this, "File upload request, " + args.Request.FileName + ", " + args.Request.TotalBytes.ToString() + ". Confirm?", this.Text, MessageBoxButtons.YesNo) == System.Windows.Forms.DialogResult.Yes)
        //        {
        //            SaveFileDialog dlg = new SaveFileDialog();
        //            dlg.Title = this.Text + " Save files as";
        //            if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
        //            {
        //                args.Confirm(dlg.FileName);
        //            }
        //            else
        //            {
        //                args.Refuse();
        //            }
        //        }
        //        else
        //        {
        //            args.Refuse();
        //        }
        //    });
        //}

        //void btnUploadFile_Click(object sender, EventArgs e)
        //{
        //    OpenFileDialog dlg = new OpenFileDialog();
        //    dlg.Title = this.Text + " select file to upload";
        //    if (dlg.ShowDialog() != System.Windows.Forms.DialogResult.OK) return;

        //    client.UploadFile(dlg.FileName, (clientSender, response) =>
        //    {
        //        Status("Uploading " + response.FileName + ", " + ((response.CurrentPosition * 100) / response.TotalBytes) + "%...");

        //        if (response.CurrentPosition >= response.TotalBytes)
        //        {
        //            Status("Uploading " + response.FileName + " Completed!");
        //        }
        //    });
        //}

        void client_TextMessageReceived(Client sender, string message)
        {
            Status("Message received: " + message);
        }

    
        void btnLogin_Click(object sender, EventArgs e)
        {
            DialogInput dlg = new DialogInput("Please enter your user name:");
            if (dlg.ShowDialog() != System.Windows.Forms.DialogResult.OK) return;

            client.Login(dlg.Result, (senderClient, args) =>
            {

                if (args.IsValid)
                {
                    Status("User Validated!");
                    this.InvokeUI(() =>
                    {
                        this.Text = "Client - " + dlg.Result;
                        //btnStartSession.Enabled = true;
                        btnLogin.Enabled = false;
                    });
                }

                if (args.HasError)
                {
                    Status(args.Exception.ToString());
                }

            });
        }

        void btnConnect_Click(object sender, EventArgs e)
        {
            client.Connect("localhost", 8888);
            client.Login("Ajit", (senderClient, args) =>
            {

                if (args.IsValid)
                {
                    Status("User Validated!");
                    this.InvokeUI(() =>
                    {
                        this.Text = "Client - Ajit";
                        //btnStartSession.Enabled = true;
                        btnLogin.Enabled = false;
                        btnSendMessage.Enabled = true;
                    });
                }

                if (args.HasError)
                {
                    Status(args.Exception.ToString());
                }

            });
            btnLogin.Enabled = true;
            btnDisconnect.Enabled = true;
            btnConnect.Enabled = false;
            btnSendMessage.Enabled = true;
        }

        private void Status(String str)
        {
            InvokeUI(() => { lbStatus.Text = str; });
        }

        private void InvokeUI(Action action)
        {
            this.Invoke(action);
        }

        private void btnSendMessage_Click(object sender, EventArgs e)
        {
            DialogInput dlg = new DialogInput("Enter text message:");
            if (dlg.ShowDialog() != System.Windows.Forms.DialogResult.OK) return;
            client.SendTextMessage(dlg.Result);
        }
    }
}
