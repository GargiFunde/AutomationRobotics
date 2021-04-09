// <copyright file=Program company=E2E BOTS>
// Copyright (c) 2018 All Rights Reserved
// </copyright>
// <author>Prabodhini Funde</author>
// <date> 03-10-2018 16:02:58</date>
// <summary></summary>


using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RobotConsoleLocal
{
    class Program
    {
        static void Main(string[] args)
        {

            string ConsoleMode = ConfigurationManager.AppSettings["ConsoleMode"];
            if(ConsoleMode.ToLower().Trim() == "ui")
            {
                MainWindow mw = new MainWindow();
                mw.Show();
            }
            else
            {
                Console.Read();
            }
        }
    }
}
