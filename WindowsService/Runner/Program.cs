using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Runner
{
    class Program
    {
        static void Main(string[] args)
        {
            string executor = args[0];
            string username = args[1];
            string password = args[2];
            string domain = args[3];
            // string filePath = args[4];
            // string inputargs = args[5];



            var procInfo = new ProcessStartInfo();


            procInfo.UserName = username;


            procInfo.Domain = domain;

            procInfo.Password = (new System.Net.NetworkCredential("", password)).SecurePassword;
            procInfo.UseShellExecute = false;
            procInfo.FileName = executor;
            //   procInfo.Arguments = filePath + " " + inputargs;

            //  Console.WriteLine(Directory.GetCurrentDirectory());
            procInfo.WorkingDirectory = Directory.GetCurrentDirectory();
            try
            {
                var process1 = Process.Start(procInfo);
            }
            catch (Exception e)
            {
                Console.WriteLine("AppRunner", String.Format("AppRunner: Error while opening applicaton details are : {0}  exception message is {1} stack is {2}", procInfo.Arguments, e.Message, e.StackTrace));
                throw;
            }

            Console.WriteLine("Over");
        }
    }
}
