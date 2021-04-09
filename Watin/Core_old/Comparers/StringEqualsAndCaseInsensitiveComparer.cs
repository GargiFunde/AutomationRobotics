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
	/// Class that supports a simple matching of two strings. For a match, the given strings
	/// should be equal (this includes the casing of the strings).
	/// You can also use <seealso cref="StringComparer(string, bool)"/>.
	/// </summary>
	public class StringEqualsAndCaseInsensitiveComparer : StringComparer
	{
		public StringEqualsAndCaseInsensitiveComparer(string value) : base(value, true) {}
	}
}