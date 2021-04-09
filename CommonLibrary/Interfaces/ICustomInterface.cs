// <copyright file=ICustomInterface company=E2E BOTS>
// Copyright (c) 2018 All Rights Reserved
// </copyright>
// <author>Prabodhini Funde</author>
// <date> 03-10-2018 16:02:54</date>
// <summary></summary>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonLibrary.Interfaces
{
    public interface ICustomPluginInterface : IDisposable
    {
        //  string AssemblyName { get; set; }
        // void LoadAttachedApplication(object application);  
        //void Initialize();
        void StartScraping(string ApplicationId);
        void StopScraping(string ApplicationId);

        void CloseApplication(string ApplicationId);
    }
}
