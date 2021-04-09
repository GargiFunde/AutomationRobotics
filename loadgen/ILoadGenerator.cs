// <copyright file=ILoadGenerator company=E2E BOTS>
// Copyright (c) 2018 All Rights Reserved
// </copyright>
// <author>Prabodhini Funde</author>
// <date> 03-10-2018 16:03:08</date>
// <summary></summary>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataConnectorToGenerateRequest
{
    interface ILoadGenerator
    {
        void ReadData();

        PublishSubscribe publishSubscribe { get; set; }
        //void ReadClassData(); // Commented By Piyush
        void ReadClassData(string a ,string b ,string c );



        // void PrepareJsonAndAutomation(Dictionary<string, string> dict, string[] values);
        //  string ConsumeService(string path, string paramJson);

    }
}
