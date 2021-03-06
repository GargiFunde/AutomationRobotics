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

using System;
using System.Collections;
using WatiN.Core.Constraints;
using WatiN.Core.Interfaces;

namespace WatiN.Core
{
	/// <summary>
	/// A typed collection of <see cref="Image" /> instances within a <see cref="Document"/> or <see cref="Element"/>.
	/// </summary>
#if NET11
	public class ImageCollection : BaseElementCollection
#else
    public class ImageCollection : BaseElementCollection<Image>
#endif
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="ImageCollection"/> class.
		/// Mainly used by WatiN internally.
		/// </summary>
		/// <param name="domContainer">The DOM container.</param>
		/// <param name="finder">The finder.</param>
		public ImageCollection(DomContainer domContainer, INativeElementFinder finder) : base(domContainer, finder, new CreateElementInstance(Image.New)) {}

		/// <summary>
		/// Initializes a new instance of the <see cref="ImageCollection"/> class.
		/// Mainly used by WatiN internally.
		/// </summary>
		/// <param name="domContainer">The DOM container.</param>
		/// <param name="elements">The elements.</param>
		public ImageCollection(DomContainer domContainer, ArrayList elements) : base(domContainer, elements, new CreateElementInstance(Image.New)) {}

		/// <summary>
		/// Gets the <see cref="Image"/> at the specified index.
		/// </summary>
		/// <value></value>
		public Image this[int index]
		{
			get { return (Image) ElementsTyped(index); }
		}

		public ImageCollection Filter(BaseConstraint findBy)
		{
			return new ImageCollection(domContainer, DoFilter(findBy));
		}

#if !NET11
        public ImageCollection Filter(Predicate<Image> predicate)
        {
            return new ImageCollection(domContainer, DoFilter(Find.ByElement(predicate)));
        }
#endif

	}
}