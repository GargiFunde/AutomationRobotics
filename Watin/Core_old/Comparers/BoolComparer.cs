#region WatiN Copyright (C) 2006-2009 Jeroen van Menen

//Copyright 2006-2009 Jeroen van Menen
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

namespace WatiN.Core.Comparers
{
	/// <summary>
	/// Class that supports comparing a <see cref="bool"/> instance with a string value.
	/// </summary>
	public class BoolComparer : StringEqualsAndCaseInsensitiveComparer
	{
        /// <summary>
        /// Initializes a new instance of the <see cref="BoolComparer"/> class.
        /// </summary>
        /// <param name="value">The given value will be used for the comparision(s) done by <see cref="BaseComparer.Compare(string)"/>.</param>
		public BoolComparer(bool value) : base(value.ToString()) {}
	}
}