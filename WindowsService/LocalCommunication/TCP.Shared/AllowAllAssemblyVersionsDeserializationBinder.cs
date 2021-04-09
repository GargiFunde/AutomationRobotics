// <copyright file=AllowAllAssemblyVersionsDeserializationBinder company=E2E BOTS>
// Copyright (c) 2018 All Rights Reserved
// </copyright>
// <author>Saurabh Munde</author>
// <date> 03-10-2018 16:03:07</date>
// <summary></summary>

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace TCP.Shared
{
public sealed class AllowAllAssemblyVersionsDeserializationBinder : System.Runtime.Serialization.SerializationBinder
{
    public override Type BindToType(string assemblyName, string typeName)
    {
        Type typeToDeserialize = null;

        String currentAssembly = Assembly.GetExecutingAssembly().FullName;

        // In this case we are always using the current assembly
        assemblyName = currentAssembly;

        // Get the type using the typeName and assemblyName
        typeToDeserialize = Type.GetType(String.Format("{0}, {1}",
            typeName, assemblyName));

        return typeToDeserialize;
    }
}
}
