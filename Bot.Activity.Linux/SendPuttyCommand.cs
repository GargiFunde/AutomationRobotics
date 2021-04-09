// <copyright file=SendPuttyCommand company=E2E BOTS>
// Copyright (c) 2018 All Rights Reserved
// </copyright>
// <author>Prabodhini Funde</author>
// <date> 03-10-2018 16:02:45</date>
// <summary></summary>

using Logger;
using System;
using System.Activities;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Bot.Activity.Linux
{
    [ToolboxBitmap("../BOTDesigner/Icons/E2E BOTS ICONS/putty.png")]
    [Designer(typeof(ActivityDesigner1))]
    public class SendPuttyCommand : BaseNativeActivity
    {

        [RequiredArgument]
        public InArgument<string> PuttyDllDirPath { get; set; }

        [RequiredArgument]
        public InArgument<string> UserName { get; set; }

        [RequiredArgument]
        public InArgument<string> Password { get; set; }

        [RequiredArgument]
        public InArgument<string> ComputerName { get; set; }

        [RequiredArgument]
        public InArgument<int> Protocol { get; set; }

        [RequiredArgument]
        public InArgument<uint> Port { get; set; }

        //[RequiredArgument]
        //public InArgument<int> ConnectionId { get; set; }

        [RequiredArgument]
        public InArgument<string> CommandData { get; set; }

        public OutArgument<string> Result { get; set; }
       
        public SendPuttyCommand()
        {
            //Port.Expression = 22;
            //Protocol.Expression = 1;
        }
        protected override void Execute(NativeActivityContext context)
        {
            try
            {
                UInt32 myConnectionId;
                string strPuttyDllDirPath = string.Empty;
                string strMachine = string.Empty;
                string strUser = string.Empty;
                string strPwd = string.Empty;
                string strCommandData = string.Empty; //"uname -r"
                uint iPort = Port.Get(context);
                int iProtocol = Protocol.Get(context);

                strPuttyDllDirPath = PuttyDllDirPath.Get(context);
                strUser = UserName.Get(context);
                strPwd = Password.Get(context);
                strCommandData = CommandData.Get(context);

                PuttyEngine puttyEngine = new PuttyEngine();

                myConnectionId = new UInt32();
                int size, status;
                char[] data = null;

                PuttyEngine.CallbackRcvData Callback = new PuttyEngine.CallbackRcvData(RcvData);
                //CallbackRcvData = new CallbackRcvData(myConnectionId, 

                puttyEngine.CreateDataConnection(strPuttyDllDirPath, strMachine,
                    strUser, // user
                    strPwd, // password
                    iProtocol,       // SSH Protocol
                    iPort,      // Port
                    Callback);         // bin_000 -> dont wait login prompt, dynamically starting putty log, ssh v1 otherwise ssh v2

                puttyEngine.SendData(strCommandData, "", "", 5000, ref data, 20000, 0);
                //  PuttyEngine.CloseSession(myConnectionId);
            }catch(Exception ex)
            {
                Log.Logger.LogData(ex.Message + " in activity SendPuttyCommand", LogLevel.Error);
                if (!ContinueOnError) { context.Abort(); }
            }
        }
        public int RcvData(UInt32 ConnectionId, IntPtr Data, Int32 size, Int32 Status)
        {
            if ((size > 0) && (Status == 0) && (Data != null))
            {
                string myString = Marshal.PtrToStringAnsi(Data);
                //Result.Expression = myString;
            }
            return 0;
        }
        
    }
}
