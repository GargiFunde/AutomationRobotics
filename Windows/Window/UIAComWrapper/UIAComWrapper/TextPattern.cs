
// (c) Copyright Microsoft Corporation.
// This source is subject to the Microsoft Permissive License.
// See http://www.microsoft.com/opensource/licenses.mspx#Ms-PL.
// All other rights reserved.

using System;
using System.Collections;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Windows.Automation.Text;
using UIAComWrapperInternal;

namespace System.Windows.Automation
{
    public class TextPattern : BasePattern
    {
        
        private UIAutomationClient.IUIAutomationTextPattern _pattern;
        public static readonly AutomationPattern Pattern = TextPatternIdentifiers.Pattern;
        public static readonly AutomationTextAttribute AnimationStyleAttribute = TextPatternIdentifiers.AnimationStyleAttribute;
        public static readonly AutomationTextAttribute BackgroundColorAttribute = TextPatternIdentifiers.BackgroundColorAttribute;
        public static readonly AutomationTextAttribute BulletStyleAttribute = TextPatternIdentifiers.BulletStyleAttribute;
        public static readonly AutomationTextAttribute CapStyleAttribute = TextPatternIdentifiers.CapStyleAttribute;
        public static readonly AutomationTextAttribute CultureAttribute = TextPatternIdentifiers.CultureAttribute;
        public static readonly AutomationTextAttribute FontNameAttribute = TextPatternIdentifiers.FontNameAttribute;
        public static readonly AutomationTextAttribute FontSizeAttribute = TextPatternIdentifiers.FontSizeAttribute;
        public static readonly AutomationTextAttribute FontWeightAttribute = TextPatternIdentifiers.FontWeightAttribute;
        public static readonly AutomationTextAttribute ForegroundColorAttribute = TextPatternIdentifiers.ForegroundColorAttribute;
        public static readonly AutomationTextAttribute HorizontalTextAlignmentAttribute = TextPatternIdentifiers.HorizontalTextAlignmentAttribute;
        public static readonly AutomationTextAttribute IndentationFirstLineAttribute = TextPatternIdentifiers.IndentationFirstLineAttribute;
        public static readonly AutomationTextAttribute IndentationLeadingAttribute = TextPatternIdentifiers.IndentationLeadingAttribute;
        public static readonly AutomationTextAttribute IndentationTrailingAttribute = TextPatternIdentifiers.IndentationTrailingAttribute;
        public static readonly AutomationTextAttribute IsHiddenAttribute = TextPatternIdentifiers.IsHiddenAttribute;
        public static readonly AutomationTextAttribute IsItalicAttribute = TextPatternIdentifiers.IsItalicAttribute;
        public static readonly AutomationTextAttribute IsReadOnlyAttribute = TextPatternIdentifiers.IsReadOnlyAttribute;
        public static readonly AutomationTextAttribute IsSubscriptAttribute = TextPatternIdentifiers.IsSubscriptAttribute;
        public static readonly AutomationTextAttribute IsSuperscriptAttribute = TextPatternIdentifiers.IsSuperscriptAttribute;
        public static readonly AutomationTextAttribute MarginBottomAttribute = TextPatternIdentifiers.MarginBottomAttribute;
        public static readonly AutomationTextAttribute MarginLeadingAttribute = TextPatternIdentifiers.MarginLeadingAttribute;
        public static readonly AutomationTextAttribute MarginTopAttribute = TextPatternIdentifiers.MarginTopAttribute;
        public static readonly AutomationTextAttribute MarginTrailingAttribute = TextPatternIdentifiers.MarginTrailingAttribute;
        public static readonly object MixedAttributeValue = TextPatternIdentifiers.MixedAttributeValue;
        public static readonly AutomationTextAttribute OutlineStylesAttribute = TextPatternIdentifiers.OutlineStylesAttribute;
        public static readonly AutomationTextAttribute OverlineColorAttribute = TextPatternIdentifiers.OverlineColorAttribute;
        public static readonly AutomationTextAttribute OverlineStyleAttribute = TextPatternIdentifiers.OverlineStyleAttribute;
        public static readonly AutomationTextAttribute StrikethroughColorAttribute = TextPatternIdentifiers.StrikethroughColorAttribute;
        public static readonly AutomationTextAttribute StrikethroughStyleAttribute = TextPatternIdentifiers.StrikethroughStyleAttribute;
        public static readonly AutomationTextAttribute TabsAttribute = TextPatternIdentifiers.TabsAttribute;
        public static readonly AutomationEvent TextChangedEvent = TextPatternIdentifiers.TextChangedEvent;
        public static readonly AutomationTextAttribute TextFlowDirectionsAttribute = TextPatternIdentifiers.TextFlowDirectionsAttribute;
        public static readonly AutomationEvent TextSelectionChangedEvent = TextPatternIdentifiers.TextSelectionChangedEvent;
        public static readonly AutomationTextAttribute UnderlineColorAttribute = TextPatternIdentifiers.UnderlineColorAttribute;
        public static readonly AutomationTextAttribute UnderlineStyleAttribute = TextPatternIdentifiers.UnderlineStyleAttribute;

        
        private TextPattern(AutomationElement el, UIAutomationClient.IUIAutomationTextPattern pattern, bool cached)
            : base(el, cached)
        {
            Debug.Assert(pattern != null);
            this._pattern = pattern;
        }

        internal static object Wrap(AutomationElement el, object pattern, bool cached)
        {
            return (pattern == null) ? null : new TextPattern(el, (UIAutomationClient.IUIAutomationTextPattern)pattern, cached);
        }

        public TextPatternRange[] GetSelection()
        {
            try
            {
                return TextPatternRange.Wrap(this._pattern.GetSelection(), this);
            }
            catch (System.Runtime.InteropServices.COMException e)
            {
                Exception newEx; if (Utility.ConvertException(e, out newEx)) { throw newEx; } else { throw; }
            }
        }

        public TextPatternRange[] GetVisibleRanges()
        {
            try
            {
                return TextPatternRange.Wrap(this._pattern.GetVisibleRanges(), this);
            }
            catch (System.Runtime.InteropServices.COMException e)
            {
                Exception newEx; if (Utility.ConvertException(e, out newEx)) { throw newEx; } else { throw; }
            }
        }

        public TextPatternRange RangeFromChild(AutomationElement childElement)
        {
            Utility.ValidateArgumentNonNull(childElement, "childElement");
            try
            {
                return TextPatternRange.Wrap(this._pattern.RangeFromChild(childElement.NativeElement), this);
            }
            catch (System.Runtime.InteropServices.COMException e)
            {
                Exception newEx; if (Utility.ConvertException(e, out newEx)) { throw newEx; } else { throw; }
            }
        }

        public TextPatternRange RangeFromPoint(Point screenLocation)
        {
            try
            {
                return TextPatternRange.Wrap(this._pattern.RangeFromPoint(Utility.PointManagedToNative(screenLocation)), this);
            }
            catch (System.Runtime.InteropServices.COMException e)
            {
                Exception newEx; if (Utility.ConvertException(e, out newEx)) { throw newEx; } else { throw; }
            }
        }

        public TextPatternRange DocumentRange
        {
            get
            {
                try
                {
                    return TextPatternRange.Wrap(this._pattern.DocumentRange, this);
                }
                catch (System.Runtime.InteropServices.COMException e)
                {
                    Exception newEx; if (Utility.ConvertException(e, out newEx)) { throw newEx; } else { throw; }
                }
            }
        }

        public SupportedTextSelection SupportedTextSelection
        {
            get
            {
                try
                {
                    return (SupportedTextSelection)this._pattern.SupportedTextSelection;
                }
                catch (System.Runtime.InteropServices.COMException e)
                {
                    Exception newEx; if (Utility.ConvertException(e, out newEx)) { throw newEx; } else { throw; }
                }
            }
        }
    }
}
