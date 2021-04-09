// <copyright file=GenericRequest company=E2E BOTS>
// Copyright (c) 2018 All Rights Reserved
// </copyright>
// <author>Saurabh Munde</author>
// <date> 03-10-2018 16:03:07</date>
// <summary></summary>

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace TCP.Shared.Messages
{
    [Serializable]
    public class GenericRequest : RequestMessageBase
    {
        internal MemoryStream InnerMessage { get; set; }

        public GenericRequest()
        {
            InnerMessage = new MemoryStream();
        }

        public GenericRequest(RequestMessageBase request)
            : this()
        {
            BinaryFormatter f = new BinaryFormatter();
            f.Serialize(InnerMessage, request);
            InnerMessage.Position = 0;
        }

        public GenericRequest ExtractInnerMessage()
        {
            BinaryFormatter f = new BinaryFormatter();
            f.Binder = new AllowAllAssemblyVersionsDeserializationBinder();
            return f.Deserialize(InnerMessage) as GenericRequest;
        }
    }
}
