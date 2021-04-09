// <copyright file=RuntimeApplicationHelper company=E2E BOTS>
// Copyright (c) 2018 All Rights Reserved
// </copyright>
// <author>Prabodhini Funde</author>
// <date> 03-10-2018 16:02:53</date>
// <summary></summary>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonLibrary
{
   
       
    public sealed class RuntimeApplicationHelper
    {
        //private static RuntimeApplicationHelper instance = null;
        //private static readonly object padlock = new object();

        //public string WorkflowName;
        public Dictionary<string,object> RuntimeApplicationObjects { get; set; }

        public RuntimeApplicationHelper()
        {
            RuntimeApplicationObjects = new Dictionary<string, object>();
        }
        //public static RuntimeApplicationHelper Instance
        //{
        //    get
        //    {
        //        if (instance == null)
        //        {
        //            lock (padlock)
        //            {
        //                if (instance == null)
        //                {
        //                    instance = new RuntimeApplicationHelper();
        //                }
        //            }
        //        }
        //        return instance;
        //    }
        //}
    }
}
