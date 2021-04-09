// <copyright file=PuttyEngine company=E2E BOTS>
// Copyright (c) 2018 All Rights Reserved
// </copyright>
// <author>Prabodhini Funde</author>
// <date> 03-10-2018 16:02:45</date>
// <summary></summary>

using System;
using System.Runtime.InteropServices;

namespace Bot.Activity.Linux
{
    public class PuttyEngine
    {
        [DllImport("user32.dll", SetLastError = true)]
        static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint lpdwProcessId);
        [DllImport("user32.dll", EntryPoint = "FindWindow", SetLastError = true)]
        static extern IntPtr FindWindowByCaption(IntPtr ZeroOnly, string lpWindowName);
        [DllImport("ExtraPuTTY.dll", EntryPoint = "Connexion")]
        static extern int OpenConnection(string TargetName,
                                            ref UInt32 ConnectionId,
                                            string Login,
                                            string Password,
                                            byte ShowTerminal,
                                            Int32 Protocol,
                                            UInt32 PortNumber,
                                            Int32 Report,
                                            CallbackRcvData Callback,
                                            UInt32 SpecSettings);

        [DllImport("ExtraPuTTY.dll", EntryPoint = "SendRcvData")]
        static extern int SendData(UInt32 ConnectionId,
                                    string Data,
                                    string Title,
                                    string Comments,
                                    Int32 TimeCapture,
                                    char[] Buf,
                                    Int32 MaxSizeData,
                                    UInt32 settings);

        [DllImport("ExtraPuTTY.dll", EntryPoint = "SendRcvData")]
        static extern int SendData(UInt32 ConnectionId,
                                    string Data,
                                    string Title,
                                    string Comments,
                                    Int32 TimeCapture,
                                    ref IntPtr Buf,
                                    Int32 MaxSizeData,
                                    UInt32 settings);
        // int SendRcvData(unsigned long ConnectionId,char *Command,char *Title,char *Comments,long TimeCapture,char **DataRcv,long MaxSizeofData,unsigned long settings);
        // Settings Bit fields of settings (2^0 : CRLF (0 send,1 not send),2^1 : Virtual key codes (0 no virtual key codes in command,1 yes)...reserved)
        // See FAQ page for a description of all virtual keys codes.  
        [DllImport("kernal32.dll", SetLastError = true)]
        static extern bool SetDllDirectory(string lpPathName);

        [DllImport("ExtraPuTTY.dll", EntryPoint = "CloseConnexion")]
        public static extern int CloseSession(UInt32 ConnectionId);

        public UInt32 myConnectionId;

        public delegate int CallbackRcvData(UInt32 ConnectionId, IntPtr Data, Int32 size, Int32 Status);


        public PuttyEngine()
        {

        }
        ~PuttyEngine()
        {
            if (this.myConnectionId != null)
            {
                CloseSession(myConnectionId);
            }
        }

        public void CreateDataConnection(string puttyDllDirPath, string target, string login, string password, Int32 protocol, UInt32 portCOM, CallbackRcvData callback)
        {
            int iresult = 0;
            SetDllDirectory(puttyDllDirPath);// (@"D:\DLL\Putty\");
            iresult = OpenConnection(target, ref myConnectionId, login, password, 0, protocol, portCOM, 0, callback, 0);

            if (iresult != 0)
            {
                throw new System.ArgumentException("Parameter can not be null", "original");
            }
        }

        public void SendData(string data, string title, string comments, Int32 timecapture, ref IntPtr buf, Int32 maxsizedata, UInt32 settings)
        {
            int iresult = 0;

            iresult = SendData(myConnectionId, data, title, comments, timecapture, ref buf, maxsizedata, settings);

            if (iresult != 0)
            {
                // throw new System.ArgumentException("Parameter can not be null", "original");
            }
        }
        public void SendData(string data, string title, string comments, Int32 timecapture, ref char[] buf, Int32 maxsizedata, UInt32 settings)
        {
            int iresult = 0;

            iresult = SendData(myConnectionId, data, title, comments, timecapture, buf, maxsizedata, settings);

            if (iresult != 0)
            {
                // throw new System.ArgumentException("Parameter can not be null", "original");
            }
        }

    }
}
