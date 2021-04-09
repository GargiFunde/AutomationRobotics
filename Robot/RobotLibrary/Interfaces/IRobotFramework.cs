// <copyright file=IRobotFramework company=E2E BOTS>
// Copyright (c) 2018 All Rights Reserved
// </copyright>
// <author>Saurabh Munde</author>
// <date> 03-10-2018 16:02:51</date>
// <summary></summary>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using System.Runtime.Serialization;
using System.ServiceModel;

namespace RobotLibrary.Interfaces
{
    [ServiceContract]
    public interface IRobotFramework
    {
        [OperationContract]
        int PreLaunchAllApplications();
        [OperationContract]
        int LaunchCurrentGroupApplications();
        [OperationContract]
        int PostLaunchAllApplications();
        [OperationContract]
        int PreAutomate();
        //[OperationContract]
        //int Automate(RequestInput rqInput);
        [OperationContract]
        int PostAutomate();
        [OperationContract]
        void ShutDownCurrentApplications();

    }
}
