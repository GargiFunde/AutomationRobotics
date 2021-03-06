#region WatiN Copyright (C) 2006-2011 Jeroen van Menen

//Copyright 2006-2011 Jeroen van Menen
//
//   Licensed under the Apache License, Version 2.0 (the "License");
//   you may not use this file except in compliance with the License.
//   You may obtain a copy of the License at
//
//       http://www.apache.org/licenses/LICENSE-2.0
//
//   Unless required by applicable law or agreed to in writing, software
//   distributed under the License is distributed on an "AS IS" BASIS,
//   WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//   See the License for the specific language governing permissions and
//   limitations under the License.

#endregion Copyright

using System.Reflection;
using System.Runtime.InteropServices;
using System.Runtime.InteropServices.Expando;

namespace WatiN.Core.Native.InternetExplorer
{
    public class Expando
    {
        public object Object { get; private set; }
        public IExpando AsExpando { get; private set; }

        public Expando(object expando)
        {
            Object = expando;
            AsExpando = (IExpando) expando;
        }

        public T GetValue<T>(string propertyName)
        {
            return (T)GetValue(propertyName);
        }

        public object GetValue(string propertyName)
        {
            var property = AsExpando.GetProperty(propertyName, BindingFlags.Default);
            if (property != null)
            {
                try
                {
                    return property.GetValue(Object, null);
                }
                catch (COMException)
                {
                    return null;
                }
            }

            return null;
        }
    }
}