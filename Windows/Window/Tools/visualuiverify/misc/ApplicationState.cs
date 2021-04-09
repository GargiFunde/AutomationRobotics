
// (c) Copyright Microsoft Corporation.
// This source is subject to the Microsoft Permissive License.
// See http://www.microsoft.com/opensource/licenses.mspx#Ms-PL.
// All other rights reserved.

using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.Serialization;
using VisualUIAVerify.Features;

namespace VisualUIAVerify.Misc
{
    [Serializable()]   
   public class ApplicationState
    {
        public bool RemoveClientSideProvider = false;

        public string HighLight = ElementHighlighterFactory.BoundingRectangle;

        public bool ModeAlwaysOnTop = false;
        public bool ModeHoverMode = false;
        public bool ModeFocusTracking = false;

    }
}
