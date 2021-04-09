// <copyright file=ProcessExtensions company=E2E BOTS>
// Copyright (c) 2018 All Rights Reserved
// </copyright>
// <author>Saurabh Munde</author>
// <date> 03-10-2018 16:03:06</date>
// <summary></summary>


using System;
using System.Runtime.InteropServices;
using static WindowsApi.WindowsApi; //added for CreateSessionAsUserW in the WindowsApi project

namespace AutomationAgent
{
    public static class ProcessExtensions
    {
        #region Win32 Constants

        private const int CREATE_UNICODE_ENVIRONMENT = 0x00000400;
        private const int CREATE_NO_WINDOW = 0x08000000;

        private const int CREATE_NEW_CONSOLE = 0x00000010;

        private const uint INVALID_SESSION_ID = 0xFFFFFFFF;
        private static readonly IntPtr WTS_CURRENT_SERVER_HANDLE = IntPtr.Zero;

        #endregion

        #region DllImports

        //[DllImport("advapi32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        //private static extern bool CreateProcessWithLogonW(
        //      String userName,
        //      String domain,
        //      String password,
        //      UInt32 logonFlags,
        //      String applicationName,
        //      String commandLine,
        //      UInt32 creationFlags,
        //      UInt32 environment,
        //      String currentDirectory,
        //      ref StartupInfo startupInfo,
        //      out ProcessInformation processInformation);


        [DllImport("advapi32.dll", EntryPoint = "CreateProcessAsUser", SetLastError = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
        private static extern bool CreateProcessAsUser(
            IntPtr hToken,
            String lpApplicationName,
            String lpCommandLine,
            IntPtr lpProcessAttributes,
            IntPtr lpThreadAttributes,
            bool bInheritHandle,
            uint dwCreationFlags,
            IntPtr lpEnvironment,
            String lpCurrentDirectory,
            ref STARTUPINFO lpStartupInfo,
            out PROCESS_INFORMATION lpProcessInformation);

        [DllImport("advapi32.dll", EntryPoint = "DuplicateTokenEx")]
        private static extern bool DuplicateTokenEx(
            IntPtr ExistingTokenHandle,
            uint dwDesiredAccess,
            IntPtr lpThreadAttributes,
            int TokenType,
            int ImpersonationLevel,
            ref IntPtr DuplicateTokenHandle);

        [DllImport("userenv.dll", SetLastError = true)]
        private static extern bool CreateEnvironmentBlock(ref IntPtr lpEnvironment, IntPtr hToken, bool bInherit);

        [DllImport("userenv.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool DestroyEnvironmentBlock(IntPtr lpEnvironment);

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool CloseHandle(IntPtr hSnapshot);

        [DllImport("kernel32.dll")]
        private static extern uint WTSGetActiveConsoleSessionId();

        [DllImport("Wtsapi32.dll")]
        private static extern uint WTSQueryUserToken(uint SessionId, ref IntPtr phToken);

        [DllImport("wtsapi32.dll", SetLastError = true)]
        private static extern int WTSEnumerateSessions(
            IntPtr hServer,
            int Reserved,
            int Version,
            ref IntPtr ppSessionInfo,
            ref int pCount);

        #endregion

        #region Win32 Structs

        private enum SW
        {
            SW_HIDE = 0,
            SW_SHOWNORMAL = 1,
            SW_NORMAL = 1,
            SW_SHOWMINIMIZED = 2,
            SW_SHOWMAXIMIZED = 3,
            SW_MAXIMIZE = 3,
            SW_SHOWNOACTIVATE = 4,
            SW_SHOW = 5,
            SW_MINIMIZE = 6,
            SW_SHOWMINNOACTIVE = 7,
            SW_SHOWNA = 8,
            SW_RESTORE = 9,
            SW_SHOWDEFAULT = 10,
            SW_MAX = 10
        }

        private enum WTS_CONNECTSTATE_CLASS
        {
            WTSActive,
            WTSConnected,
            WTSConnectQuery,
            WTSShadow,
            WTSDisconnected,
            WTSIdle,
            WTSListen,
            WTSReset,
            WTSDown,
            WTSInit
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct PROCESS_INFORMATION
        {
            public IntPtr hProcess;
            public IntPtr hThread;
            public uint dwProcessId;
            public uint dwThreadId;
        }

        private enum SECURITY_IMPERSONATION_LEVEL
        {
            SecurityAnonymous = 0,
            SecurityIdentification = 1,
            SecurityImpersonation = 2,
            SecurityDelegation = 3,
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct STARTUPINFO
        {
            public int cb;
            public String lpReserved;
            public String lpDesktop;
            public String lpTitle;
            public uint dwX;
            public uint dwY;
            public uint dwXSize;
            public uint dwYSize;
            public uint dwXCountChars;
            public uint dwYCountChars;
            public uint dwFillAttribute;
            public uint dwFlags;
            public short wShowWindow;
            public short cbReserved2;
            public IntPtr lpReserved2;
            public IntPtr hStdInput;
            public IntPtr hStdOutput;
            public IntPtr hStdError;
        }

        private enum TOKEN_TYPE
        {
            TokenPrimary = 1,
            TokenImpersonation = 2
        }

        [StructLayout(LayoutKind.Sequential)]
        private struct WTS_SESSION_INFO
        {
            public readonly UInt32 SessionID;

            [MarshalAs(UnmanagedType.LPStr)]
            public readonly String pWinStationName;

            public readonly WTS_CONNECTSTATE_CLASS State;
        }

        //[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
        //public struct StartupInfo
        //{
        //    public int cb;
        //    public String reserved;
        //    public String desktop;
        //    public String title;
        //    public int x;
        //    public int y;
        //    public int xSize;
        //    public int ySize;
        //    public int xCountChars;
        //    public int yCountChars;
        //    public int fillAttribute;
        //    public int flags;
        //    public UInt16 showWindow;
        //    public UInt16 reserved2;
        //    public byte reserved3;
        //    public IntPtr stdInput;
        //    public IntPtr stdOutput;
        //    public IntPtr stdError;
        //}

        //internal struct ProcessInformation
        //{
        //    public IntPtr process;
        //    public IntPtr thread;
        //    public int processId;
        //    public int threadId;
        //}
        #endregion

        // Gets the user token from the currently active session
        private static bool GetSessionUserToken(ref IntPtr phUserToken)
        {
            var bResult = false;
            var hImpersonationToken = IntPtr.Zero;
            var activeSessionId = INVALID_SESSION_ID;
            var pSessionInfo = IntPtr.Zero;
            var sessionCount = 0;

            // Get a handle to the user access token for the current active session.
            if (WTSEnumerateSessions(WTS_CURRENT_SERVER_HANDLE, 0, 1, ref pSessionInfo, ref sessionCount) != 0)
            {
                var arrayElementSize = Marshal.SizeOf(typeof(WTS_SESSION_INFO));
                var current = pSessionInfo;

                for (var i = 0; i < sessionCount; i++)
                {
                    var si = (WTS_SESSION_INFO)Marshal.PtrToStructure((IntPtr)current, typeof(WTS_SESSION_INFO));
                    current += arrayElementSize;

                    if (si.State == WTS_CONNECTSTATE_CLASS.WTSActive)
                    {
                        activeSessionId = si.SessionID;
                    }
                }
            }

            // If enumerating did not work, fall back to the old method
            if (activeSessionId == INVALID_SESSION_ID)
            {
                activeSessionId = WTSGetActiveConsoleSessionId();
            }

            if (WTSQueryUserToken(activeSessionId, ref hImpersonationToken) != 0)
            {
                // Convert the impersonation token to a primary token
                bResult = DuplicateTokenEx(hImpersonationToken, 0, IntPtr.Zero,
                    (int)SECURITY_IMPERSONATION_LEVEL.SecurityImpersonation, (int)TOKEN_TYPE.TokenPrimary,
                    ref phUserToken);

                CloseHandle(hImpersonationToken);
            }

            return bResult;
        }

        public static bool StartProcessAsCurrentUser(string appPath, string cmdLine = null, string workDir = null, bool visible = true)
        {
            System.IO.File.AppendAllText(@"C:\Piyush\DebugService.txt", "Process Extensions.cs=> In StartProcessAsCurrentUser. " + "\n");
            var hUserToken = IntPtr.Zero;
            var startInfo = new STARTUPINFO();
            var procInfo = new PROCESS_INFORMATION();
            var pEnv = IntPtr.Zero;
            int iResultOfCreateProcessAsUser;

            startInfo.cb = Marshal.SizeOf(typeof(STARTUPINFO));

            try
            {
                if (!GetSessionUserToken(ref hUserToken))
                {
                    throw new Exception("StartProcessAsCurrentUser: GetSessionUserToken failed.");
                }
                
                uint dwCreationFlags = CREATE_UNICODE_ENVIRONMENT | (uint)(visible ? CREATE_NEW_CONSOLE : CREATE_NO_WINDOW);
                startInfo.wShowWindow = (short)(visible ? SW.SW_SHOW : SW.SW_HIDE);
                startInfo.lpDesktop = "winsta0\\default";
             
                if (!CreateEnvironmentBlock(ref pEnv, hUserToken, false))
                {
                    throw new Exception("StartProcessAsCurrentUser: CreateEnvironmentBlock failed.");
                }

                if (!CreateProcessAsUser(hUserToken,
                    appPath, // Application Name
                    cmdLine, // Command Line
                    IntPtr.Zero,
                    IntPtr.Zero,
                    false,
                    dwCreationFlags,
                    pEnv,
                    workDir, // Working directory
                    ref startInfo,
                    out procInfo))
                {
                    iResultOfCreateProcessAsUser = Marshal.GetLastWin32Error();
                    throw new Exception("StartProcessAsCurrentUser: CreateProcessAsUser failed.  Error Code -" + iResultOfCreateProcessAsUser);
                }

                iResultOfCreateProcessAsUser = Marshal.GetLastWin32Error();
            }
            finally
            {
                CloseHandle(hUserToken);
                if (pEnv != IntPtr.Zero)
                {
                    DestroyEnvironmentBlock(pEnv);
                }
                CloseHandle(procInfo.hThread);
                CloseHandle(procInfo.hProcess);
            }

            return true;
        }


        public static void StartProcessAsRobotUser(string appPath, string botid, string pwd, string RunnerPath)
        {

            System.IO.File.AppendAllText(@"C:\LockLogs.txt", "\n" + appPath + "\n" + botid + "\n" + pwd + "\n" + RunnerPath + "\n" + Environment.MachineName + "\n");
            //TcpClient client = (TcpClient)obj;
            //var stream = client.GetStream();
            //string imei = String.Empty;
            //string data = null;
            //Byte[] bytes = new Byte[256];
            //int i;
            try
            {
                // while ((i = stream.Read(bytes, 0, bytes.Length)) != 0)
                //{
                //string hex = BitConverter.ToString(bytes);
                //data = Encoding.ASCII.GetString(bytes, 0, i);
                //Console.WriteLine("{1}: Received: {0}", data, Thread.CurrentThread.ManagedThreadId);
                //System.IO.File.AppendAllText(@"C:\CBLogs.txt",data+"\n");
                //var inp = data.Split(System.Environment.NewLine, );
                 
                //string username = inp[0];
                //string password = inp[1];
                //string folderName = inp[2];
                //string fileName = inp[3];
                //string inputargs = inp[4];
                var str = "error";

                string domain = Environment.MachineName;
                domain = domain.ToLower();
                botid = botid.ToLower();

                //    IntPtr SessionID = WindowsAPI.ListUsers("e2eqa02", "e2e02");
                IntPtr SessionID = WindowsAPI.ListUsers(domain, botid);
                Int32 sessionID = (Int32)SessionID;
                try
                {
                    //inputargs = inputargs.Replace('"','%');
                    // inputargs = inputargs.Replace(' ', '&');
                    //System.IO.File.AppendAllText(@"C:\CBLogs.txt", "\n\n\n\n"+ inputargs + "\n\n\n\n");
                    //var commandPath = options.Runner;
                    //   var commandPath = @"C:\Work\BotDesignerMaster\Build\netcoreapp3.1\RunnerTest.exe";

                    var commandPath = RunnerPath;
                    //System.IO.File.AppendAllText(@"C:\CBLogs.txt", "E2e exec-"+options.E2EExecutor + "\n");
                    //string commandArgs = " " + (char)34 + options.E2EExecutor + (char)34 + " " +(char)34 + username + (char)34+ " " + (char)34 + password + (char)34+
                    // " " + (char)34+options.Domain+ (char)34+ " " + (char)34 + Path.Join(options.DefaultProjectFolder,folderName,fileName) + (char)34 + " " + (char)34 + inputargs + (char)34;
                    // string commandArgs = string.Empty;
                    string commandArgs = " " + (char)34 + appPath + (char)34 + " " + (char)34 + botid + (char)34 + " " + (char)34 + pwd + (char)34 +
                    " " + (char)34 + domain + (char)34;// + " " + (char)34;+ Path.Join(options.DefaultProjectFolder,folderName,fileName) + (char)34 + " " + (char)34 + inputargs + (char)34;
                    System.IO.File.AppendAllText(@"C:\LockLogs.txt", commandArgs + "\n");
                    var commandArgsPtr = Marshal.StringToHGlobalAuto(commandArgs);
                    IntPtr userTokenHandle = IntPtr.Zero;
                    //ProcessInformation procInfo;
                    ProcessInformation procInfo;

                    StartupInfo startInfo = new StartupInfo();

                    WindowsApi.WindowsApi.WTSQueryUserToken((UInt32)SessionID, ref (userTokenHandle));
                    //  Console.WriteLine(String.Format("OnStart CreateProcessAsUser {0}", commandPath));
                    bool result = CreateProcessAsUserW(
                   userTokenHandle, commandPath, commandArgsPtr,
                   IntPtr.Zero, IntPtr.Zero, false, 0, IntPtr.Zero, null, ref (startInfo), out (procInfo));
                    str = "Process Started";
                    SessionID = IntPtr.Zero;
                    if (result == false)
                        str = "error creating process";
                }
                catch (Exception e)
                {
                    // System.IO.File.AppendAllText(@"C:\CBLogs.txt", e + "\n");
                    //Console.WriteLine(String.Format("runbot Error : sessionID {0} project : {1} Error: {2}", sessionID, Path.Join(options.DefaultProjectFolder, folderName, fileName), e.Message + Environment.NewLine + e.Source + Environment.NewLine + e.StackTrace));
                }
                //Byte[] reply = System.Text.Encoding.ASCII.GetBytes(str);
                //stream.Write(reply, 0, reply.Length);
                //Console.WriteLine("{1}: Sent: {0}", str, Thread.CurrentThread.ManagedThreadId);


                // }
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception: {0}", e.ToString());
            }
            finally
            {
                // client.Close();
            }
        }

    }
}
