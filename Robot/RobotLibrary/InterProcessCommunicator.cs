// <copyright file=InterProcessCommunicator company=E2E Robotics>
// Copyright (c) 2018 All Rights Reserved
// </copyright>
// <author>Saurabh Mundhe</author>
// <date> 03-10-2018 16:02:51</date>
// <summary></summary>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Interop;
using System.Runtime.InteropServices;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

namespace RobotLibrary
{
   public class InterProcessCommunicator
    {
       private IntPtr processhandle;

       public object ProcessIdentifier
       {
           get
           {
               return processhandle;
           }
           set
           {
               processhandle = (IntPtr)value;
               if( processhandle != IntPtr.Zero)
               {
                   System.Windows.Interop.HwndSource source = HwndSource.FromHwnd(processhandle);
               }
               else
               {
                   //No process found
               }
           }
       }
       public InterProcessCommunicator()
       {
       }
       public InterProcessCommunicator(Window window)
       {
           if(window ==null)
           {
               //no window details provided
           }
           HwndSource source = HwndSource.FromHwnd(new WindowInteropHelper(window).Handle);
           source.AddHook(WndProc);
           processhandle = source.Handle;
       }
       public event EventHandler<CommunicatorMessageEventArgs> communicationMessageReceived;

       private IntPtr WndProc(IntPtr _hwnd, int _msg, IntPtr _wParam, IntPtr _lParam, ref bool _handle)
       {
           switch (_msg)
           {
               // program receives WM_COPYDATA Message from target app
               case Win32.WM_COPYDATA:
                   if (_msg == Win32.WM_COPYDATA)
                   {
                       // get the data
                       Win32.CopyDataStruct cds = (Win32.CopyDataStruct)Marshal.PtrToStructure(_lParam, typeof(Win32.CopyDataStruct));
                       if (cds.cbData > 0)
                       {
                           if (communicationMessageReceived != null)
                           {
                               communicationMessageReceived(this, new CommunicatorMessageEventArgs()
                               {
                                   Message = Marshal.PtrToStringAuto(cds.lpData),
                                   SenderProcessIdentifier = _hwnd
                               });
                           }
                       }
                   }
                   break;
           }
           return IntPtr.Zero;
       } 
       public int SendMessage(object receivingProcessIdentifier, string message)
       {
           int result = 0;
           try
           {
               if (string.IsNullOrEmpty(message))
               {
                   //Message is empty;
               }
               if (((IntPtr)receivingProcessIdentifier).ToInt32() > 0)
               {
                   BinaryFormatter binary = new BinaryFormatter();
                   MemoryStream mStream = new MemoryStream();
                   binary.Serialize(mStream, message);
                   int dataSize = Convert.ToInt32(mStream.Length);
                   mStream.Flush();
                   Win32.CopyDataStruct cds;
                   cds.cbData = 10000;
                   cds.dwData = IntPtr.Zero;
                   cds.lpData = Marshal.StringToHGlobalAuto(message);
                   result = Win32.SendMessage(((IntPtr)receivingProcessIdentifier).ToInt32(), Win32.WM_COPYDATA, IntPtr.Zero, ref cds);
                   Marshal.FreeHGlobal(cds.lpData);
               }
               else
               {
                   //nvalid process handle
               }
           }catch(Exception ex)
           {
               //Log
           }
           return result;
       }
    }
}
