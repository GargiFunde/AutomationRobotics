
// (c) Copyright Microsoft Corporation.
// This source is subject to the Microsoft Permissive License.
// See http://www.microsoft.com/opensource/licenses.mspx#Ms-PL.
// All other rights reserved.

#define CODE_ANALYSIS  // Required for FxCop suppression attributes

using System;
using System.CodeDom;
using System.Collections;
using Drawing = System.Drawing;
using System.Globalization;
using System.Threading;
using System.Windows.Automation;
using System.Windows.Automation.Text;
using System.Windows;
using System.Diagnostics.CodeAnalysis;
using MS.Win32;

namespace InternalHelper.Tests.Patterns
{
    using InternalHelper;
    using InternalHelper.Tests;
    using InternalHelper.Enumerations;
    using Microsoft.Test.UIAutomation;
    using Microsoft.Test.UIAutomation.Core;
    using Microsoft.Test.UIAutomation.TestManager;
    using Microsoft.Test.UIAutomation.Interfaces;

    #region TextWrapper Class

    /// -----------------------------------------------------------------------
    /// <summary></summary>
    /// -----------------------------------------------------------------------
    public class TextWrapper : PatternObject
    {
        #region Member Variables
        ///---------------------------------------------------------------------------
        /// <summary></summary>
        ///---------------------------------------------------------------------------
        internal TextPattern _pattern;       // Reference to TextPattern for this automationelement
        #endregion

        #region constructor

        //---------------------------------------------------------------------------
        /// <summary></summary>
        //---------------------------------------------------------------------------
        [SuppressMessage("Microsoft.Globalization", "CA1303:DoNotPassLiteralsAsLocalizedParameters", MessageId = "System.ArgumentException.#ctor(System.String)")]
        internal TextWrapper(AutomationElement element, string testSuite, TestPriorities priority, TypeOfControl typeOfControl, TypeOfPattern typeOfPattern, string dirResults, bool testEvents, IApplicationCommands commands)
            :
            base(element, testSuite, priority, typeOfControl, typeOfPattern, dirResults, testEvents, commands)
        {
            if (m_le == null)
                throw new ArgumentException("m_le cannot be null");

            _pattern = (TextPattern)m_le.GetCurrentPattern(TextPattern.Pattern);
            _frameworkId = ((string)m_le.GetCurrentPropertyValue(AutomationElement.FrameworkIdProperty)).ToLower(CultureInfo.InvariantCulture);
            _testPriority = TestPriorities.BuildVerificationTest; // default value

            // Determine if tests currently running on Windows Vista
            NativeMethods.OSVERSIONINFOEX ver = new NativeMethods.OSVERSIONINFOEX();
            UnsafeNativeMethods.GetVersionEx(ver);
            if (ver.majorVersion >= 6) // This should account for Windows Vista + Service Packs
                _windowsVista = true;   // It may also occur for Vienna, but the expectation is things
            else                        // could change so much post-Vista, that TextPattern tests will
                _windowsVista = false;  // have to be revisited anyway ((i.e. we will likely be moving to un-managed client)

            Comment("Operating System Version = " + ver.majorVersion + "." + ver.minorVersion + "." + ver.buildNumber);
        }

        #endregion constructor

        #region Win32Proxy is Developer Preview in Windows Vista

        static internal string _frameworkId;    // Provider/proxy name is...???
        static internal bool _windowsVista;   // Is this Windows Vista (or earlier)???
        static internal TestPriorities _testPriority;   // What is priority of the current test?
        static internal string _problemDescription; // What is possible explanation of faiure?

        /// ---------------------------------------------------------------------------
        /// <summary>
        /// Over-rides the TestObject.ThrowMe()
        /// </summary>
        /// ---------------------------------------------------------------------------
        static internal new void ThrowMe(CheckType checkType, string format, params object[] args)
        {
            // Add enhanced fialure reporting
            if (!String.IsNullOrEmpty(_problemDescription))
            {
                Comment("~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~");
                
                // Report base error message
                if (checkType == CheckType.Verification)
                {
                    Comment("This test variation has failed. Possible cause:");
                }
                else
                {
                    Comment("This test variation has failed due to incorrect configuration. Specific reason for the failure:");
                }
                Comment("");
                Comment(format, args);
                Comment("");

                // Do we have a known failure for this test?                
                if ((_problemDescription != null) && (_problemDescription.Length > 0))
                {
                    Comment("FYI: The following bug has been demonstrated to directly cause this test to fail (although it may not have on this failure):");
                    Comment("");
                    Comment(_problemDescription);
                    Comment("");
                }

                // logs that information in the output to facilitate ease of analysis for these known failures.
                //      FrameworkID             = win32 (i.e. Win32 Proxy)
                //      OS Version              = 6.0   (i.e. Windows Vista--only  Any OS Version > 6.0 is not Vista (Veinna, etc.)) 
                //      UIA Verify Test Priority  > 0     (i.e. Not a BVT test)
                if ((checkType == CheckType.Verification)
                 && (_windowsVista == true)
                 && ((_frameworkId == "win32") || (_frameworkId == "winform"))
                 && (_testPriority != TestPriorities.Pri0))
                {
                    Comment("In addition to the above, there are a number of ther issues that could contribute to this test failing.");
                    Comment("We have known failures for Win32 Proxy running on Windows Vista. (It *IS* \"Developer Preview\" for Vista)");
                    Comment("");
                    Comment("Move()                  functionality 'root' bug(s):  1768226 (Edit) 1768223 (RichEdit)");
                    Comment("MoveEndpintByUnit()     functionality 'root' bug(s):  1364606 (RichEdit)");
                    Comment("ScrollIntoView()        functionality 'root' bug(s):  1777200 (Edit)  1777203 (RichEdit)");
                    Comment("ExpandToEnclosingUnit() functionality 'root' bug(s):  1663830 (Edit)  1663901 (RichEdit)");
                    Comment("");
                    Comment("Win32 RichEdit Move/MoveEndpointByUnit/Scroll functionality incorrectly uses the RichEdit Text Object Model(): 1134056");
                    Comment("");
                    Comment("GetText(-1)             misbehaving:  1348727 Random assertions that cause timeout failures");
                    Comment("GetText(-1)             misbehaving:  1456324 Random raising of InvalidOperationException");
                    Comment("GetBoundingRectangles() misbehaving:  1370948");
                    Comment("Move()                  misbehaving:  1369775, 1345450, 1370082, 1369423");
                    Comment("GetVisibleRange()       misbehaving:  1346630");
                    Comment("");
                    Comment("Almost all P1/P2/P3 failures in UIA Verify for Win32 can be traced back to these core bugs.");
                    // Therefore...
                    checkType = CheckType.KnownProductIssue;
                }

                Comment("~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~");
                Comment("");
            }

            TestObject.ThrowMe(checkType, format, args);
        }

        /// -------------------------------------------------------------------
        /// <summary>
        /// Set local static avriable for _testPriority for current test/variation
        /// </summary>
        /// -------------------------------------------------------------------
        new internal void HeaderComment(TestCaseAttribute testCaseAttribute)
        {
            _testPriority = testCaseAttribute.Priority;
            _problemDescription = testCaseAttribute.ProblemDescription;

            base.HeaderComment(testCaseAttribute);
        }

        #endregion Win32Proxy is Developer Preview in Windows Vista

        #region TextPattern Test Enums
        #region AttributeType
        ///---------------------------------------------------------------------------
        ///<summary> Enum for test case variations</summary>
        ///---------------------------------------------------------------------------
        public enum AttributeType
        {
            /// <summary></summary>
            SupportedAttributes,                // Supported attributes on the control
            /// <summary></summary>
            UnsupportedAttributes,              // Attributes not supported by the control
            /// <summary></summary>
            Null,                               // Null value for attribute(s)
            /// <summary></summary>
            EnumAttributes,                     // Those attributes whose data type is an enum
        };
        ///---------------------------------------------------------------------------
        /// <summary>Parses values for enum</summary>
        ///---------------------------------------------------------------------------
        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        [SuppressMessage("Microsoft.Globalization", "CA1303:DoNotPassLiteralsAsLocalizedParameters", MessageId = "System.ArgumentException.#ctor(System.String)")]
        static public string Parse(AttributeType value)
        {
            switch (value)
            {
                case AttributeType.SupportedAttributes: return "Supported attributes on the control";
                case AttributeType.UnsupportedAttributes: return "Attributes not supported by the control";
                case AttributeType.Null: return "Null value for attribute(s)";
                case AttributeType.EnumAttributes: return "Those attributes whose data type is an enum";
                default:
                    throw new ArgumentException("Parse() has no support for " + ParseType(value));
            }
        }
        ///---------------------------------------------------------------------------
        /// <summary>Parses values for enum</summary>
        ///---------------------------------------------------------------------------
        static public string ParseType(AttributeType value)
        {
            return ParseType(value.GetType().ToString(), value.ToString());
        }
        #endregion AttributeType
        #region AutoElementType
        ///---------------------------------------------------------------------------
        /// <summary>Enum for test case variations</summary>
        ///---------------------------------------------------------------------------
        public enum AutoElementType // Type of auotmation element to use
        {
            /// <summary></summary>
            DifferentFromPattern,                  //different than autoElement that contains this pattern
            /// <summary></summary>
            Null,                                  //equal to null 
            /// <summary></summary>
            SameAsPattern,                         //that contains this pattern
            /// <summary></summary>
            UseChildren                            //from TextPattern.Children proeprty
        };
        ///---------------------------------------------------------------------------
        /// <summary>Parses values for enum</summary>
        ///---------------------------------------------------------------------------
        [SuppressMessage("Microsoft.Globalization", "CA1303:DoNotPassLiteralsAsLocalizedParameters", MessageId = "System.ArgumentException.#ctor(System.String)")]
        static public string Parse(AutoElementType value)
        {
            switch (value)
            {
                case AutoElementType.SameAsPattern: return "that contains this pattern";
                case AutoElementType.DifferentFromPattern: return "different than autoElement that contains this pattern";
                case AutoElementType.Null: return "equal to null ";
                case AutoElementType.UseChildren: return "from TextPattern.Children proeprty";
                default:
                    throw new ArgumentException("Parse() has no support for " + ParseType(value));
            }
        }
        ///---------------------------------------------------------------------------
        /// <summary>Parses values for enum</summary>
        ///---------------------------------------------------------------------------
        static public string ParseType(AutoElementType value)
        {
            return ParseType(value.GetType().ToString(), value.ToString());
        }
        #endregion AutoElementType
        #region BoundingRectangleLocation
        ///---------------------------------------------------------------------------
        /// <summary>Enum for test case variations</summary>
        ///---------------------------------------------------------------------------
        [FlagsAttribute]
        [SuppressMessage("Microsoft.Naming", "CA1714:FlagsEnumsShouldHavePluralNames")]
        public enum BoundingRectangleLocation // Location of point relative to bounding rectangle
        {
            /// <summary></summary>
            None = 0,             // Unused value
            /// <summary></summary>
            InsideBottomRight = 1,   //Inside Bottom Right       
            /// <summary></summary>
            InsideTopLeft = 2,   //Inside Top Left           
            /// <summary></summary>
            Middle = 4,   //Middle                  
            /// <summary></summary>
            OutsideAutomationElement = 8,   //Outside Automation Element
            /// <summary></summary>
            OutsideBottomRight = 16,  //Outside Bottom Right      
            /// <summary></summary>
            OutsideTopLeft = 32,  //Outside Top Left
            /// <summary></summary>
            FirstChar = 64,  //Within first character in document
            /// <summary></summary>
            FirstCharInRange = 128, //Within first character in range
            /// <summary></summary>
            LastCharInRange = 256  //Within last character in range            
        };
        ///---------------------------------------------------------------------------
        /// <summary>Parses values for enum</summary> 
        ///---------------------------------------------------------------------------
        [SuppressMessage("Microsoft.Globalization", "CA1303:DoNotPassLiteralsAsLocalizedParameters", MessageId = "System.ArgumentException.#ctor(System.String)")]
        static public string Parse(BoundingRectangleLocation value)
        {
            switch (value)
            {
                case BoundingRectangleLocation.InsideTopLeft: return "Inside Top Left";
                case BoundingRectangleLocation.Middle: return "Middle";
                case BoundingRectangleLocation.InsideBottomRight: return "Inside Bottom Right";
                case BoundingRectangleLocation.OutsideTopLeft: return "Outside Top Left";
                case BoundingRectangleLocation.OutsideBottomRight: return "Outside Bottom Right";
                case BoundingRectangleLocation.OutsideAutomationElement: return "Outside Automation Element";
                case BoundingRectangleLocation.FirstChar: return "Within first character";
                case BoundingRectangleLocation.FirstCharInRange: return "Within first character in range";
                case BoundingRectangleLocation.LastCharInRange: return "Within last character in range";
                default:
                    throw new ArgumentException("Parse() has no support for " + ParseType(value));
            }
        }
        /// ---------------------------------------------------------------------------
        /// <summary>Parses values for enum</summary>
        /// ---------------------------------------------------------------------------
        static public string ParseType(BoundingRectangleLocation value)
        {
            return ParseType(value.GetType().ToString(), value.ToString());
        }
        #endregion BoundingRectangleLocation
        #region Count
        /// ---------------------------------------------------------------------------
        /// <summary>Enum for test case variations</summary>
        /// ---------------------------------------------------------------------------
        public enum Count
        {
            /// <summary></summary>
            MinInt,                                 // count = Int32.MinValue
            /// <summary></summary>
            NegativeNPlusOne,                       // count = ((# of text units in range) + 1) * -1
            /// <summary></summary>
            NegativeN,                              // count = (# of text units in range) * -1
            /// <summary></summary>
            NegativeNMinusOne,                      // count = ((# of text units in range) - 1) * -1
            /// <summary></summary>
            NegativeHalfN,                          // count = -(half # of text units in range)
            /// <summary></summary>
            NegativeOne,                            // count = -1 
            /// <summary></summary>
            Zero,                                   // count = 0
            /// <summary></summary>
            One,                                    // count = One
            /// <summary></summary>
            HalfN,                                  // count = half # of text units in range
            /// <summary></summary>
            NMinusOne,                              // count = # of text units in range
            /// <summary></summary>
            N,                                      // count = # of text units in range
            /// <summary></summary>
            NPlusOne,                               // count = (# of text units in range) + 1
            /// <summary></summary>
            MaxInt,                                 // count = Int32.MaxValue
        };
        /// ---------------------------------------------------------------------------
        /// <summary>Parses values for enum</summary> 
        /// ---------------------------------------------------------------------------
        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        [SuppressMessage("Microsoft.Globalization", "CA1303:DoNotPassLiteralsAsLocalizedParameters", MessageId = "System.ArgumentException.#ctor(System.String)")]
        static public string Parse(Count value)
        {
            switch (value)
            {
                case Count.MinInt: return "Int32.MinValue";
                case Count.NegativeNPlusOne: return "((# of text units in range) + 1) * -1";
                case Count.NegativeN: return "(# of text units in range) * -1";
                case Count.NegativeNMinusOne: return "((# of text units in range) - 1) * -1";
                case Count.NegativeHalfN: return "-(half # of text units in range)";
                case Count.NegativeOne: return "-1";
                case Count.Zero: return "0";
                case Count.One: return "One";
                case Count.HalfN: return "half # of text units in range";
                case Count.NMinusOne: return "(# of text units in range) - 1";
                case Count.N: return "# of text units in range";
                case Count.NPlusOne: return "(# of text units in range) + 1";
                case Count.MaxInt: return "Int32.MaxValue";
                default:
                    throw new ArgumentException("Parse() has no support for " + ParseType(value));
            }
        }
        /// ---------------------------------------------------------------------------
        /// <summary>Parses values for enum</summary>
        /// ---------------------------------------------------------------------------
        static public string ParseType(Count value)
        {
            return ParseType(value.GetType().ToString(), value.ToString());
        }
        #endregion Count
        #region FindResults
        /// ---------------------------------------------------------------------------
        /// <summary>Enum for test case variations</summary>
        /// ---------------------------------------------------------------------------
        [FlagsAttribute]
        public enum FindResults
        {
            /// <summary></summary>
            None = 0,             // Unused value
            /// <summary></summary>
            EmptyRange = 1,                     // EMPTY range
            /// <summary></summary>
            Exception = 2,                      // raises exception
            /// <summary></summary>
            MatchesFirst = 4,                   // matches 1st instance of dup'd text
            /// <summary></summary>
            MatchesLast = 8,                    // matches last block of dup'd text
            /// <summary></summary>
            MatchingRange = 16,                 // matching range (case sensitive)
            /// <summary></summary>
            MatchingRangeCaseLess = 32,         // matching range (case insensitive)
            /// <summary></summary>
            NoHiddenText = 64,                  // has no hidden text
            /// <summary></summary>
            Null = 128,                         // NULL
            /// <summary></summary>
            MatchingFirstCaseLess = 36,         // Matches first range and is caseless
            /// <summary></summary>
            MatchingLastCaseLess = 40          // Matches last range and is caseless
        };
        /// ---------------------------------------------------------------------------
        /// <summary>Parses values for enum</summary>
        /// ---------------------------------------------------------------------------
        [SuppressMessage("Microsoft.Globalization", "CA1303:DoNotPassLiteralsAsLocalizedParameters", MessageId = "System.ArgumentException.#ctor(System.String)")]
        static public string Parse(FindResults value, Type type)
        {
            switch (value)
            {
                case FindResults.MatchingRange: return "verify result MATCHING range (case sensitive)";
                case FindResults.MatchingRangeCaseLess: return "matching range (case insensitive)";
                case FindResults.MatchingFirstCaseLess: return "Matches first range and is caseless";
                case FindResults.MatchingLastCaseLess: return "Matches last range and is caseless";
                case FindResults.EmptyRange: return "verify result EMPTY range";
                case FindResults.Null: return "verify result NULL";
                case FindResults.Exception:
                    if (type == null)
                        throw new ArgumentException("Can't haveFindResults.Exception with null value for Type");
                    return ParseEx1(type);
                case FindResults.NoHiddenText: return "verify result has NO hidden text";
                case FindResults.MatchesFirst: return "verify result MATCHES 1st instance of dup'd text";
                case FindResults.MatchesLast: return "verify result MATCHES LAST instance of dup'd text";
                default:
                    throw new ArgumentException("Parse() has no support for " + ParseType(value));
            }
        }
        /// ---------------------------------------------------------------------------
        /// <summary>Parses values for enum</summary>
        /// ---------------------------------------------------------------------------
        static public string ParseType(FindResults value)
        {
            return ParseType(value.GetType().ToString(), value.ToString());
        }
        #endregion FindResults
        #region GetBoundRectResult
        /// ---------------------------------------------------------------------------
        /// <summary>Enum for test case variations</summary>
        /// ---------------------------------------------------------------------------
        public enum GetBoundRectResult
        {
            /// <summary></summary>
            EmptyArray,                      // empty array of bounding rectangles
            /// <summary></summary>
            SubsetOfAutoElementBoundRect        // boundRect(s) are a subset of autoElement boundRect
        };
        /// ---------------------------------------------------------------------------
        /// <summary>Parses values for enum</summary>
        /// ---------------------------------------------------------------------------
        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        [SuppressMessage("Microsoft.Globalization", "CA1303:DoNotPassLiteralsAsLocalizedParameters", MessageId = "System.ArgumentException.#ctor(System.String)")]
        static public string Parse(GetBoundRectResult value)
        {
            switch (value)
            {
                case GetBoundRectResult.SubsetOfAutoElementBoundRect: return "boundRect(s) are a subset of autoElement boundRect";
                case GetBoundRectResult.EmptyArray: return "empty array of bounding rectangles";
                default:
                    throw new ArgumentException("Parse() has no support for " + ParseType(value));
            }
        }
        /// ---------------------------------------------------------------------------
        /// <summary>Parses values for enum</summary>
        /// ---------------------------------------------------------------------------
        static public string ParseType(GetBoundRectResult value)
        {
            return ParseType(value.GetType().ToString(), value.ToString());
        }
        #endregion GetBoundRectResult
        #region GetResult
        /// ---------------------------------------------------------------------------
        /// <summary>Enum for test case variations</summary>
        /// ---------------------------------------------------------------------------
        public enum GetResult          // Used for both GetText and GetAttributeValue
        {
            /// <summary></summary>
            CallingRangeLength,          // matches length of calling range
            /// <summary></summary>
            DocumentRange,               // matches entire TextPattern document
            /// <summary></summary>
            Empty,                       // EMPTY range
            /// <summary></summary>
            Exception,                   // raises exception
            /// <summary></summary>
            MatchingAttributeValue,      // Matching attribute value(s)
            /// <summary></summary>
            Null,                        // verify result returns null
            /// <summary></summary>
            NotSupported                 // verify result returns AutomationElement.NotSupported
        };
        /// ---------------------------------------------------------------------------
        /// <summary>Parses values for enum</summary>
        /// ---------------------------------------------------------------------------
        [SuppressMessage("Microsoft.Globalization", "CA1303:DoNotPassLiteralsAsLocalizedParameters", MessageId = "System.ArgumentException.#ctor(System.String)")]
        static public string Parse(GetResult value, Type type)
        {
            switch (value)
            {
                case GetResult.DocumentRange: return "verify result matches ENTIRE TextPattern document";
                case GetResult.CallingRangeLength: return "verify result matches LENGTH of calling range";
                case GetResult.Empty: return "verify result is EMPTY range";
                case GetResult.Exception:
                    if (type == null)
                        throw new ArgumentException("Can't have GetResult.Exception with null Type y");
                    return ParseEx1(type);
                case GetResult.MatchingAttributeValue: return "verify result MATCHES expected attribute values";
                case GetResult.Null: return "verify result returns NULL";
                case GetResult.NotSupported: return "verify result returns AutomationElement.NotSupported";
                default:
                    throw new ArgumentException("Parse() has no support for " + ParseType(value));
            }
        }
        /// ---------------------------------------------------------------------------
        /// <summary>Parses values for enum</summary>
        /// ---------------------------------------------------------------------------
        static public string ParseType(GetResult value)
        {
            return ParseType(value.GetType().ToString(), value.ToString());
        }
        #endregion GetResult
        #region MaxLength
        /// ---------------------------------------------------------------------------
        /// <summary>Enum for test case variations</summary>
        /// ---------------------------------------------------------------------------
        public enum MaxLength
        {
            /// <summary></summary>
            All,                                      // -1 (all) for calling range
            /// <summary></summary>
            Length,                                   // actual length of calling range
            /// <summary></summary>
            MaxInt,                                   // max Integer value
            /// <summary></summary>
            MinusTwo,                                 // -2 (error) for text length)
            /// <summary></summary>
            NegativeMaxInt,                           // negative max Integer value
            /// <summary></summary>
            One,                                      // 1 (single character) for text length
            /// <summary></summary>
            RandomOutsideValidSize,                   // RANDOM size > actual size
            /// <summary></summary>
            RandomWithinValidSize,                    // RANDOM size <= actual size
            /// <summary></summary>
            Zero                                      // 0 for text length
        };
        /// ---------------------------------------------------------------------------
        /// <summary>Parses values for enum</summary> 
        /// ---------------------------------------------------------------------------
        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        [SuppressMessage("Microsoft.Globalization", "CA1303:DoNotPassLiteralsAsLocalizedParameters", MessageId = "System.ArgumentException.#ctor(System.String)")]
        static public string Parse(MaxLength value)
        {
            switch (value)
            {
                case MaxLength.All: return "-1 (all) for calling range";
                case MaxLength.One: return "1 (single character) for text length";
                case MaxLength.Zero: return "0 for text length";
                case MaxLength.MinusTwo: return "-2 (error) for text length)";
                case MaxLength.Length: return "actual length of calling range";
                case MaxLength.RandomWithinValidSize: return "RANDOM size <= actual size";
                case MaxLength.RandomOutsideValidSize: return "RANDOM size > actual size";
                case MaxLength.MaxInt: return "max Integer value";
                case MaxLength.NegativeMaxInt: return "negative max Integer value";
                default:
                    throw new ArgumentException("Parse() has no support for " + ParseType(value));
            }
        }
        /// ---------------------------------------------------------------------------
        /// <summary>Parses values for enum</summary>
        /// ---------------------------------------------------------------------------
        static public string ParseType(MaxLength value)
        {
            return ParseType(value.GetType().ToString(), value.ToString());
        }
        #endregion MaxLength
        #region MoveEPResults
        /// ---------------------------------------------------------------------------
        /// <summary>Enum for test case variations</summary>
        /// ---------------------------------------------------------------------------
        [FlagsAttribute]
        public enum MoveEPResults
        {
            /// <summary></summary>
            None = 0,             // Unused value
            /// <summary></summary>
            EmptyRange = 1,    // verify result is EMPTY range
            /// <summary></summary>
            NonemptyRange = 2,    // verify result is NON-empty range
            /// <summary></summary>
            Exception = 4     // fails
        };
        /// ---------------------------------------------------------------------------
        /// <summary>Parses values for enum</summary>
        /// ---------------------------------------------------------------------------
        [SuppressMessage("Microsoft.Globalization", "CA1303:DoNotPassLiteralsAsLocalizedParameters", MessageId = "System.ArgumentException.#ctor(System.String)")]
        static public string Parse(MoveEPResults value, Type type)
        {
            switch (value)
            {
                case MoveEPResults.EmptyRange: return "verify result is EMPTY range";
                case MoveEPResults.NonemptyRange: return "verify result is NON-empty range";
                case MoveEPResults.Exception:
                    if (type == null)
                        throw new ArgumentException("Can't have MoveEPResults.Exception with null Type y");
                    return ParseEx1(type);
                default:
                    throw new ArgumentException("Parse() has no support for " + ParseType(value));
            }
        }
        /// ---------------------------------------------------------------------------
        /// <summary>Parses values for enum</summary>
        /// ---------------------------------------------------------------------------
        static public string ParseType(MoveEPResults value)
        {
            return ParseType(value.GetType().ToString(), value.ToString());
        }
        #endregion MoveEPResults
        #region RangeCompare
        /// ---------------------------------------------------------------------------
        /// <summary>Enum for test case variations</summary>
        /// ---------------------------------------------------------------------------
        [FlagsAttribute]
        [SuppressMessage("Microsoft.Naming", "CA1714:FlagsEnumsShouldHavePluralNames")]
        public enum RangeCompare // Enum for type of range comparison to perform
        {
            /// <summary></summary>
            None = 0,             // Unused value
            /// <summary></summary>
            Contained = 1,  // Target Range within Calling range
            /// <summary></summary>
            Empty = 2,  // Target Range EMPTY
            /// <summary></summary>
            Equal = 4,  // Target Range Equal to Calling Range
            /// <summary></summary>
            WithinDocRange = 8,  // Target Range within DocumentRange
            /// <summary></summary>
            Exception = 16, // Exception is generated, no target range
        }
        /// ---------------------------------------------------------------------------
        /// <summary>Parses values for enum</summary>
        /// ---------------------------------------------------------------------------
        [SuppressMessage("Microsoft.Globalization", "CA1303:DoNotPassLiteralsAsLocalizedParameters", MessageId = "System.ArgumentException.#ctor(System.String)")]
        static public string Parse(RangeCompare value)
        {
            switch (value)
            {
                case RangeCompare.Equal: return "Target Range Equal to Calling Range";
                case RangeCompare.Empty: return "Target Range EMPTY";
                case RangeCompare.Contained: return "Target Range within Calling range";
                case RangeCompare.WithinDocRange: return "Target Range is within DocumentRange";
                case RangeCompare.Exception: return "Exception generated, no target range";
                default:
                    throw new ArgumentException("Parse() has no support for " + ParseType(value));
            }
        }
        /// ---------------------------------------------------------------------------
        /// <summary>Parses values for enum</summary>
        /// ---------------------------------------------------------------------------
        static public string ParseType(RangeCompare value)
        {
            return ParseType(value.GetType().ToString(), value.ToString());
        }
        #endregion RangeCompare
        #region RangeLocation
        /// ---------------------------------------------------------------------------
        /// <summary>Enum for test case variations</summary>
        /// ---------------------------------------------------------------------------
        public enum RangeLocation  // Enum for determining where an empty range will be created
        {
            /// <summary></summary>
            End,                                  // End
            /// <summary></summary>
            Middle,                               // Middle
            /// <summary></summary>
            Start                                 // Start
        };
        /// ---------------------------------------------------------------------------
        /// <summary>Parses values for enum</summary>
        /// ---------------------------------------------------------------------------
        [SuppressMessage("Microsoft.Globalization", "CA1303:DoNotPassLiteralsAsLocalizedParameters", MessageId = "System.ArgumentException.#ctor(System.String)")]
        static public string Parse(RangeLocation value)
        {
            switch (value)
            {
                case RangeLocation.Start: return "Start";
                case RangeLocation.Middle: return "Middle";
                case RangeLocation.End: return "End";
                default:
                    throw new ArgumentException("Parse() has no support for " + ParseType(value));
            }
        }
        /// ---------------------------------------------------------------------------
        /// <summary>Parses values for enum</summary>
        /// ---------------------------------------------------------------------------
        static public string ParseType(RangeLocation value)
        {
            return ParseType(value.GetType().ToString(), value.ToString());
        }
        #endregion RangeLocation
        #region SampleText
        ///---------------------------------------------------------------------------
        /// <summary></summary>
        ///---------------------------------------------------------------------------
        public enum SampleText            // Size/type of sample text to use in test
        {
            /// <summary></summary>
            Complex1,                                // Set text to complex multi-line text with long line of text at end
            /// <summary></summary>
            Complex2,                                // Set text to complex multi-line text with long line of text at start
            /// <summary></summary>
            ComplexHidden,                           // Set text to complex multi-line text with big block of hidden text
            /// <summary></summary>
            DuplicateBlocksOfText,                   // Set text to have duplicate blocks of text
            /// <summary></summary>
            EasyText,                                // Set text to be 'easy' text
            /// <summary></summary>
            Empty,                                   // Set text to EMPTY string
            /// <summary></summary>
            NotApplicable,                           // Value of text does not matter for test
            /// <summary></summary>
            NotEmpty,                                // Verify text is NOT empty (or SET text if it is empty)
            /// <summary></summary>
            Null,                                    // set text to be NULL
            /// <summary></summary>
            Num123,                                  // Set text to 123456789
            /// <summary></summary>
            Num123CR,                                // Set text to 1\\n2\\n3\\n...\\n9
            /// <summary></summary>
            OneCRTwoCRThreeCR,                    // Set text to One\\nTwo\\nThree\\n\\nNine
            /// <summary></summary>
            OneTwoThree,                          // Set text to OneTwoThreeNine
            /// <summary></summary>
            Random256,                               // Set text to be RANDOM 256 characters
            /// <summary></summary>
            RandomTextAnySize,                       // Set text to be RANDOM sample text
            /// <summary></summary>
            MultipleLines,                         // Set text to span MULTIPLE LINES
            /// <summary></summary>
            TextIsOneChar,                           // Set text to be ONE character
            /// <summary></summary>
            String1,                                 // Set text to be 'String1'
            /// <summary></summary>
            WordsAndSpaces,                          // Range of 3x words each followed 2 spaces
            /// <summary></summary>
            WordsAndPunctuation,                     // Range of 3x words each followed by 2 punctuation marks
            /// <summary></summary>
            ExtraLarge,                              // Text that has lots of lines and is wider than our controls
            /// <summary></summary> 
            Random64,                                // 64 chracters of random text
            /// <summary></summary> 
            Random64CR,                              // Multi-line 64 characters of random text
            /// <summary></summary> 
            Custom,                                  // CUstom text string
            /// <summary></summary>
            Unused,                                  // "Default" value for SampleText
        };
        /// ---------------------------------------------------------------------------
        /// <summary>Parses values for enum</summary>
        /// ---------------------------------------------------------------------------
        [SuppressMessage("Microsoft.Globalization", "CA1303:DoNotPassLiteralsAsLocalizedParameters", MessageId = "System.ArgumentException.#ctor(System.String)")]
        static public string Parse(SampleText value)
        {
            switch (value)
            {
                case SampleText.NotApplicable: return "Value of text does not matter for test";
                case SampleText.NotEmpty: return "Verify text is NOT empty (or SET text if it is empty)";
                case SampleText.Empty: return "Set text to EMPTY string";
                case SampleText.String1: return "Set text to 'String1'";
                case SampleText.TextIsOneChar: return "Set text to be ONE character";
                case SampleText.MultipleLines: return "Set text to span MULTIPLE LINES";
                case SampleText.EasyText: return "Set text to be 'easy' text";
                case SampleText.Random256: return "Set text to be RANDOM 256 characters";
                case SampleText.RandomTextAnySize: return "Set text to be RANDOM sample text";
                case SampleText.DuplicateBlocksOfText: return "Set text to have duplicate blocks of text";
                case SampleText.Complex1: return "Set text to complex multi-line text with long line of text at end";
                case SampleText.Complex2: return "Set text to complex multi-line text with long line of text at start";
                case SampleText.ComplexHidden: return "Set text to complex multi-line text with big block of hidden text";
                case SampleText.Num123: return "Set text to 123456789";
                case SampleText.Num123CR: return "Set text to 1<LF>2<LF>3<LF>...<LF>9";
                case SampleText.OneTwoThree: return "Set text to OneTwoThree...Nine";
                case SampleText.OneCRTwoCRThreeCR: return "Set text to One\\nTwo\\nThree\\n...\\nNine";
                case SampleText.Null: return "set text to be NULL";
                case SampleText.WordsAndSpaces: return "Range of 3x words each followed 2 spaces";
                case SampleText.WordsAndPunctuation: return "Range of 3x words each followed by 2 punctuation marks";
                case SampleText.ExtraLarge: return "Text that has lots of lines and is wider than our controls";
                case SampleText.Random64: return "Text that is 64 characters in length";
                case SampleText.Random64CR: return "Text that is 64 characters in length and has multiple lines";
                case SampleText.Custom: return "Custom text string";
                case SampleText.Unused: return "\"Default\" value for SampleText";
                default:
                    throw new ArgumentException("Parse() has no support for " + ParseType(value));
            }
        }
        /// ---------------------------------------------------------------------------
        /// <summary>Parses values for enum</summary>
        /// ---------------------------------------------------------------------------
        static public string ParseType(SampleText value)
        {
            return ParseType(value.GetType().ToString(), value.ToString());
        }
        #endregion
        #region ScrollLocation
        /// ---------------------------------------------------------------------------
        /// <summary>Enum for test case variations</summary>
        /// ---------------------------------------------------------------------------
        public enum ScrollLocation        // Location to scroll to
        {
            /// <summary></summary>
            LeftTop,            // Left-Top of viewport
            /// <summary></summary>
            RightTop,           // Right-Top of viewport
            /// <summary></summary>
            LeftCenter,         // Left-Center of viewport
            /// <summary></summary>
            Center,             // Middle (horiz & vert) of viewport
            /// <summary></summary>
            RightCenter,        // Right-Middle of viewport
            /// <summary></summary>
            LeftBottom,         // Left-Bottom of viewport
            /// <summary></summary>
            RightBottom,        // Right-Bottom of viewport
            /// <summary></summary>
            NotApplicable       // not applicable, viewport location is not relevant
        };
        /// ---------------------------------------------------------------------------
        /// <summary>Parses values for enum</summary>
        /// ---------------------------------------------------------------------------
        [SuppressMessage("Microsoft.Globalization", "CA1303:DoNotPassLiteralsAsLocalizedParameters", MessageId = "System.ArgumentException.#ctor(System.String)")]
        static public string Parse(ScrollLocation value)
        {
            switch (value)
            {
                case ScrollLocation.LeftTop: return "LEFT-TOP of viewport";
                case ScrollLocation.RightTop: return "RIGHT-TOP of viewport";
                case ScrollLocation.LeftCenter: return "LEFT-CENTER of viewport";
                case ScrollLocation.Center: return "MIDDLE (horiz & vert) of viewport";
                case ScrollLocation.RightCenter: return "RIGHT-MIDDLE of viewport";
                case ScrollLocation.LeftBottom: return "LEFT-BOTTOM of viewport";
                case ScrollLocation.RightBottom: return "RIGHT-BOTTOM of viewport";
                case ScrollLocation.NotApplicable: return "NOT APPLICABLE: viewport location is not relevant";
                default:
                    throw new ArgumentException("Parse() has no support for " + ParseType(value));
            }
        }
        /// ---------------------------------------------------------------------------
        /// <summary>Parses values for enum</summary>
        /// ---------------------------------------------------------------------------
        static public string ParseType(ScrollLocation value)
        {
            return ParseType(value.GetType().ToString(), value.ToString());
        }
        #endregion ScorllLocation
        #region SearchText
        /// ---------------------------------------------------------------------------
        /// <summary>Enum for test case variations</summary>
        /// ---------------------------------------------------------------------------
        public enum SearchText
        {
            /// <summary></summary>
            Empty,                                   // EMPTY Text
            /// <summary></summary>
            MatchesAll,                              // matches ALL Text
            /// <summary></summary>
            MatchesFirstBlock,                       // matches 1ST instance of repeating block of text
            /// <summary></summary>
            MatchesLastBlock,                        // matches LAST instance of repeating block of text
            /// <summary></summary>
            MatchesSubRangeEnd,                      // matches sub range at END of doc
            /// <summary></summary>
            MatchesSubRangeFirst,                    // matches sub range at START of doc
            /// <summary></summary>
            MatchesSubRangeMiddle,                   // matches sub range in MIDDLE of doc
            /// <summary></summary>
            MismatchedCaseAll,                       // matches ALL Text, w/MISMATCHED case
            /// <summary></summary>
            MismatchedCaseFirstBlock,                // matches 1ST instance of repeating block of text, w/MISMATCHED case
            /// <summary></summary>
            MismatchedCaseLastBlock,                 // matches LAST instance of repeating block of text, w/MISMATCHED case
            /// <summary></summary>
            MismatchedCaseSubRangeEnd,               // matches sub range at END of doc, w/MISMATCHED case
            /// <summary></summary>
            MismatchedCaseSubRangeFirst,             // matches sub range at START of doc, w/MISMATCHED case
            /// <summary></summary>
            MismatchedCaseSubRangeMiddle,            // matches sub range in MIDDLE of doc, w/MISMATCHED case
            /// <summary></summary>
            Null,                                    // NULL
            /// <summary></summary>
            Random256                                // RANDOM 256 characters
        };
        /// ---------------------------------------------------------------------------
        /// <summary>Parses values for enum</summary>
        /// ---------------------------------------------------------------------------
        [SuppressMessage("Microsoft.Globalization", "CA1303:DoNotPassLiteralsAsLocalizedParameters", MessageId = "System.ArgumentException.#ctor(System.String)")]
        static public string Parse(SearchText value)
        {
            switch (value)
            {
                case SearchText.Empty: return "EMPTY Text";
                case SearchText.MatchesAll: return "matches ALL Text";
                case SearchText.MatchesSubRangeFirst: return "matches sub range at START of doc";
                case SearchText.MatchesSubRangeMiddle: return "matches sub range in MIDDLE of doc";
                case SearchText.MatchesSubRangeEnd: return "matches sub range at END of doc";
                case SearchText.MismatchedCaseAll: return "matches ALL Text, w/MISMATCHED case";
                case SearchText.MismatchedCaseSubRangeFirst: return "matches sub range at START of doc, w/MISMATCHED case";
                case SearchText.MismatchedCaseSubRangeMiddle: return "matches sub range in MIDDLE of doc, w/MISMATCHED case";
                case SearchText.MismatchedCaseSubRangeEnd: return "matches sub range at END of doc, w/MISMATCHED case";
                case SearchText.Random256: return "RANDOM 256 characters";
                case SearchText.Null: return "NULL";
                case SearchText.MatchesFirstBlock: return "matches 1ST instance of repeating block of text";
                case SearchText.MatchesLastBlock: return "matches LAST instance of repeating block of text";
                case SearchText.MismatchedCaseFirstBlock: return "matches 1ST instance of repeating block of text, w/MISMATCHED case";
                case SearchText.MismatchedCaseLastBlock: return "matches LAST instance of repeating block of text, w/MISMATCHED case";
                default:
                    throw new ArgumentException("Parse() has no support for " + ParseType(value));
            }
        }
        /// ---------------------------------------------------------------------------
        /// <summary>Parses values for enum</summary>
        /// ---------------------------------------------------------------------------
        static public string ParseType(SearchText value)
        {
            return ParseType(value.GetType().ToString(), value.ToString());
        }
        #endregion SearchText
        #region TargetRangeType
        /// ---------------------------------------------------------------------------
        /// <summary>Enum for test case variations</summary>
        /// ---------------------------------------------------------------------------
        public enum TargetRangeType        // Makeup of the target range to create
        {
            /// <summary></summary>
            DifferentTextPattern,               // range from a different TextPattern
            /// <summary></summary>
            DocumentRange,                      // entire document
            /// <summary></summary>
            EmptyEnd,                           // EMPTY range at END of document
            /// <summary></summary>
            EmptyMiddle,                        // EMPTY range in MIDDLE of document
            /// <summary></summary>
            EmptyStart,                         // EMPTY range at START of document
            /// <summary></summary>
            FirstCharacter,                     // first character in document
            /// <summary></summary>
            HiddenFirst,                        // 1st instance of hidden text within document
            /// <summary></summary>
            HiddenLast,                         // Last instance of hidden text within document
            /// <summary></summary>
            LastCharacter,                      // last character in document
            /// <summary></summary>
            MiddleCharacter,                    // MIDDLE character in document
            /// <summary></summary>
            Null,                               // null value
            /// <summary></summary>
            RandomEmptyEnd,                     // EMPTY range at random location near END of document
            /// <summary></summary>
            RandomEmptyStart,                   // EMPTY range at random location near START of document
            /// <summary></summary>
            RandomEnd,                          // RANDOM range at END of document
            /// <summary></summary>
            RandomEndsEnd,                      // range that ENDs at END of calling range
            /// <summary></summary>
            RandomEndsStart,                    // range that ENDs at START of calling range
            /// <summary></summary>
            RandomMiddle,                       // RANDOM range in MIDDLE of document
            /// <summary></summary>
            RandomStart,                        // RANDOM range at START of document
            /// <summary></summary>
            RandomStartsEnd,                    // range that STARTs at END of calling range
            /// <summary></summary>
            RandomStartStart,                   // range that STARTs at START of calling range
            /// <summary></summary>
            SameAsCaller,                       // same as calling range
            /// <summary></summary>
            TwoCharsAdjacent,                   // two adjacent characters
            /// <summary></summary>
            TwoCharsSplitAcrossLine,            // two characters split across a line break
            /// <summary></summary>
            Clone,                              // clone of calling range
            /// <summary></summary>
            FirstFormat,                        // First "format" range in document
            /// <summary></summary>
            FirstWord,                          // First "word" range in document
            /// <summary></summary>
            FirstLine,                          // First "line" range in document
            /// <summary></summary>
            FirstParagraph,                     // First "paragraph" range in document
            /// <summary></summary>
            FirstPage,                          // First "page" range in document
            /// <summary></summary>
            MiddleSpaces,                       // In middle of spaces between 1st and 2nd word
            /// <summary></summary>
            MiddlePunctuation,                  // In middle of punctuation between 1st and 2nd word
            /// <summary></summary>
            VisibleRange,                       // Equal to visible range of control
        };
        /// ---------------------------------------------------------------------------
        /// <summary>Parses values for enum</summary>
        /// ---------------------------------------------------------------------------
        [SuppressMessage("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [SuppressMessage("Microsoft.Globalization", "CA1303:DoNotPassLiteralsAsLocalizedParameters", MessageId = "System.ArgumentException.#ctor(System.String)")]
        static public string Parse(TargetRangeType value)
        {
            switch (value)
            {
                case TargetRangeType.DocumentRange: return "entire document";
                case TargetRangeType.EmptyStart: return "EMPTY range at START of document";
                case TargetRangeType.EmptyMiddle: return "EMPTY range in MIDDLE of document";
                case TargetRangeType.EmptyEnd: return "EMPTY range at END of document";
                case TargetRangeType.RandomEmptyStart: return "EMPTY range at random location near START of document";
                case TargetRangeType.RandomEmptyEnd: return "EMPTY range at random location near END of document";
                case TargetRangeType.RandomStart: return "RANDOM range at START of document";
                case TargetRangeType.RandomMiddle: return "RANDOM range in MIDDLE of document";
                case TargetRangeType.RandomEnd: return "RANDOM range at END of document";
                case TargetRangeType.TwoCharsAdjacent: return "two adjacent characters";
                case TargetRangeType.TwoCharsSplitAcrossLine: return "two characters split across a line break";
                case TargetRangeType.RandomEndsStart: return "range that ENDs at START of calling range";
                case TargetRangeType.RandomStartStart: return "range that STARTs at START of calling range";
                case TargetRangeType.RandomEndsEnd: return "range that ENDs at END of calling range";
                case TargetRangeType.RandomStartsEnd: return "range that STARTs at END of calling range";
                case TargetRangeType.Null: return "null value";
                case TargetRangeType.DifferentTextPattern: return "range from a different TextPattern";
                case TargetRangeType.SameAsCaller: return "same as calling range";
                case TargetRangeType.FirstCharacter: return "first character in document";
                case TargetRangeType.MiddleCharacter: return "MIDDLE character in document";
                case TargetRangeType.LastCharacter: return "last character in document";
                case TargetRangeType.HiddenFirst: return "1st instance of hidden text within document";
                case TargetRangeType.HiddenLast: return "Last instance of hidden text within document";
                case TargetRangeType.Clone: return "clone of calling range";
                case TargetRangeType.FirstFormat: return "First format range in document";
                case TargetRangeType.FirstWord: return "First word range in document";
                case TargetRangeType.FirstLine: return "First line range in document";
                case TargetRangeType.FirstParagraph: return "First paragraph range in document";
                case TargetRangeType.FirstPage: return "First page range in document";
                case TargetRangeType.MiddleSpaces: return "In middle of spaces between 1st and 2nd word";
                case TargetRangeType.MiddlePunctuation: return "In middle of punctuation between 1st and 2nd word";
                case TargetRangeType.VisibleRange: return "Equal to visible range of control";
                default:
                    throw new ArgumentException("Parse() has no support for " + ParseType(value));
            }
        }
        /// ---------------------------------------------------------------------------
        /// <summary>Parses values for enum</summary>
        /// ---------------------------------------------------------------------------
        static public string ParseType(TargetRangeType value)
        {
            return ParseType(value.GetType().ToString(), value.ToString());
        }
        #endregion
        #region TextUnit
        /// ---------------------------------------------------------------------------
        /// <summary>Parses values for enum</summary>
        /// ---------------------------------------------------------------------------
        [SuppressMessage("Microsoft.Globalization", "CA1303:DoNotPassLiteralsAsLocalizedParameters", MessageId = "System.ArgumentException.#ctor(System.String)")]
        static public string Parse(TextUnit value)
        {
            switch (value)
            {
                case TextUnit.Character: return "Character";
                case TextUnit.Format: return "Format";
                case TextUnit.Word: return "Word";
                case TextUnit.Line: return "Line";
                case TextUnit.Paragraph: return "Paragraph";
                case TextUnit.Page: return "Page";
                case TextUnit.Document: return "Document";
                default:
                    throw new ArgumentException("Parse() has no support for " + ParseType(value));
            }
        }
        /// ---------------------------------------------------------------------------
        /// <summary>Parses values for enum</summary>
        /// ---------------------------------------------------------------------------
        static public string ParseType(TextUnit value)
        {
            return ParseType(value.GetType().ToString(), value.ToString());
        }
        #endregion TextUnit
        #region TypeValue
        /// ---------------------------------------------------------------------------
        /// <summary>Enum for test case variations</summary>
        /// ---------------------------------------------------------------------------
        public enum TypeValue
        {
            /// <summary></summary>
            Null,                                     // NULL
            /// <summary></summary>
            MatchesTypeAndValue,                         // CORRECT type and value
            /// <summary></summary>
            WrongTypeAndValue,                           // INCORRECT type and value
            /// <summary></summary>
            WrongType,                                // INCORRECT type, CORRECT value
            /// <summary></summary>
            WrongValue,                               // CORRECT type, INCORRECT value
            /// <summary></summary>
            WrongEnumValue,                           // Incorrect enum value
        }
        /// ---------------------------------------------------------------------------
        /// <summary>Parses values for enum</summary>
        /// ---------------------------------------------------------------------------
        [SuppressMessage("Microsoft.Globalization", "CA1303:DoNotPassLiteralsAsLocalizedParameters", MessageId = "System.ArgumentException.#ctor(System.String)")]
        static public string Parse(TypeValue value)
        {
            switch (value)
            {
                case TypeValue.MatchesTypeAndValue: return "CORRECT type and value";
                case TypeValue.WrongType: return "INCORRECT type, CORRECT value";
                case TypeValue.WrongValue: return "CORRECT type, INCORRECT value";
                case TypeValue.WrongTypeAndValue: return "INCORRECT type and value";
                case TypeValue.Null: return "NULL";
                case TypeValue.WrongEnumValue: return "Incorrect enum value";
                default:
                    throw new ArgumentException("Parse() has no support for " + ParseType(value));
            }
        }
        /// ---------------------------------------------------------------------------
        /// <summary>Parses values for enum</summary>
        /// ---------------------------------------------------------------------------
        static public string ParseType(TypeValue value)
        {
            return ParseType(value.GetType().ToString(), value.ToString());
        }
        #endregion TypeValue
        #region ValueComparison
        /// ---------------------------------------------------------------------------
        /// <summary>Parses values for enum</summary>
        /// ---------------------------------------------------------------------------
        public enum ValueComparison
        {
            /// <summary></summary>
            Equals,                             // Equals
            /// <summary></summary>
            GreaterThan,                        // Greater Than
            /// <summary></summary>
            LessThan,                           // Less than
        };
        /// ---------------------------------------------------------------------------
        /// <summary>Parses values for enum</summary>
        /// ---------------------------------------------------------------------------
        [SuppressMessage("Microsoft.Globalization", "CA1303:DoNotPassLiteralsAsLocalizedParameters", MessageId = "System.ArgumentException.#ctor(System.String)")]
        static public string Parse(ValueComparison value)
        {
            switch (value)
            {
                case ValueComparison.LessThan: return "Less than";
                case ValueComparison.Equals: return "Equals";
                case ValueComparison.GreaterThan: return "Greater Than";
                default:
                    throw new ArgumentException("Parse() has no support for " + ParseType(value));
            }
        }
        /// ---------------------------------------------------------------------------
        /// <summary>Parses values for enum</summary>
        /// ---------------------------------------------------------------------------
        static public string ParseType(ValueComparison value)
        {
            return ParseType(value.GetType().ToString(), value.ToString());
        }
        #endregion ValueComparison
        #region ParseType
        /// ---------------------------------------------------------------------------
        /// <summary>Parses values for enum</summary>
        /// ---------------------------------------------------------------------------
        [SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods")]
        static public string ParseType(string type, string typeValue)
        {
            int i = 19;
            string s;

            Library.ValidateArgumentNonNull(type, "can't have null type string");
            Library.ValidateArgumentNonNull(typeValue, "can't have null typeValue string");

            if ((i = type.LastIndexOf("+")) > 0)
                s = type.Substring(i + 1, type.Length - i - 1) + "." + typeValue;  // exclude the "+" in the type name
            else if ((i = type.LastIndexOf(".")) > 0)
                s = type.Substring(i + 1, type.Length - i - 1) + "." + typeValue;  // exclude the "/" in the type name
            else
                s = type;

            return s;
        }
        #endregion ParseType

        #endregion TestPattern Test Enums

        #region UIA Verify Enum Support

        /// ---------------------------------------------------------------------------
        /// <summary></summary>
        /// ---------------------------------------------------------------------------
        static public string ParseType(TestStatus value)
        {
            return ParseType(value.GetType().ToString(), value.ToString());
        }
        /// ---------------------------------------------------------------------------
        /// <summary></summary>
        /// ---------------------------------------------------------------------------
        static public string ParseType(TestCaseType value)
        {
            return ParseType(value.GetType().ToString(), value.ToString());
        }
        /// ---------------------------------------------------------------------------
        /// <summary></summary>
        /// ---------------------------------------------------------------------------
        static public string ParseType(TestPriorities value)
        {
            return ParseType(value.GetType().ToString(), value.ToString());
        }
        /// ---------------------------------------------------------------------------
        /// <summary></summary>
        /// ---------------------------------------------------------------------------
        static public string ParseType(TextPatternRangeEndpoint value)
        {
            return ParseType(value.GetType().ToString(), value.ToString());
        }
        #region ParseEx#
        /// ---------------------------------------------------------------------------
        /// <summary>Parses values for enum</summary>
        /// ---------------------------------------------------------------------------
        static public string ParseEx1(Type ex)
        {
            if (ex == null)
                return " without errors";

            string s = ex.ToString();
            s = " RAISES " + s + " exception";

            return s;
        }
        /// ---------------------------------------------------------------------------
        /// <summary>Parses values for enum</summary>
        /// ---------------------------------------------------------------------------
        static public string ParseEx2(Type ex)
        {
            if (ex == null)
                return "";

            string exception = ex.ToString();
            string s;
            int idx = 1 + exception.LastIndexOf(".");
            s = exception.Substring(idx, exception.Length - idx);
            if (exception == "System.ArgumentNullException")
                s = "            " + s + " ex = new " + s + "(\"testCase\"); //  Unused, just need an instance for GetType() below\n";
            else
                s = "            " + s + " ex = new " + s + "(\"Unused, just need an instance for GetType() below\");\n";

            return s;
            //             Define argument to be passed to 
            //            return ( ex == null ? "" : "            " + ex.ToString() + " ex = new " + ex.ToString() + "();\n" );
        }
        /// ---------------------------------------------------------------------------
        /// <summary>Parses values for enum</summary>
        /// ---------------------------------------------------------------------------
        static public string ParseEx3(Type ex)
        {
            //             Argument for method argument list
            return (ex == null ? "null" : "ex.GetType()");
        }
        #endregion ParseEx#
        #endregion UIA Verify Enum Support

        #region TextPattern

        #region Properties

        //---------------------------------------------------------------------------
        // Wrapper for TextPattern.DocumentRange Property
        //---------------------------------------------------------------------------
        internal TextPatternRange Pattern_DocumentRange(CheckType checkType)
        {
            string call = "TextPattern.DocumentRange";
            TextPatternRange returnedRange = null;

            Comment("---Getting value of " + call);

            returnedRange = _pattern.DocumentRange;

            if (returnedRange == null)
                ThrowMe(checkType, call + " should not give a null TextPatternRange");

            return returnedRange;
        }

        //---------------------------------------------------------------------------
        // Wrapper for TextPattern.SupportedTextSelection Property
        //---------------------------------------------------------------------------
        internal void Pattern_SupportedTextSelection(ref SupportedTextSelection supportedTextSelection)
        {
            string call = "TextPattern.SupportedTextSelection";
            Comment("---Getting value of " + call);

            supportedTextSelection = _pattern.SupportedTextSelection;
        }

        #endregion Properties

        #region Methods

        //---------------------------------------------------------------------------
        // Wrapper for TextPattern.RangeFromPoint Method
        //---------------------------------------------------------------------------
        internal void Pattern_RangeFromPoint(TextPattern textPattern, ref TextPatternRange returnedRange, Point screenLocation, Type expectedException, CheckType checkType)
        {
            string call = "TextPattern.RangeFromPoint(" + screenLocation + ")";
            Comment("---Calling " + call);

            try
            {
                returnedRange = textPattern.RangeFromPoint(screenLocation);
            }
            catch (Exception actualException)
            {
                if (Library.IsCriticalException(actualException))
                    throw;

                TestException(expectedException, actualException, call, checkType);
                return;
            }

            if (returnedRange == null)
                throw new ArgumentNullException(call + " should not give a null TextPatternRange");

            TestNoExceptionQuiet(expectedException, call, checkType);
        }

        //---------------------------------------------------------------------------
        // Wrapper for TextPattern.GetSelection Method
        //---------------------------------------------------------------------------
        internal void Pattern_GetSelection(TextPattern textPattern, ref TextPatternRange[] returnedRanges, Type expectedException, CheckType checkType)
        {
            string call = "TextPattern.GetSelection()";
            Comment("---Calling " + call);

            try
            {
                returnedRanges = textPattern.GetSelection();
            }
            catch (Exception actualException)
            {
                if (Library.IsCriticalException(actualException))
                    throw;

                TestException(expectedException, actualException, call, checkType);
                return;
            }

            if (returnedRanges == null)
                ThrowMe(checkType, call + " should not give a null TextPatternRange array");

            Comment(call + " returned an array of TextPatternRange size " + returnedRanges.Length);

            TestNoExceptionQuiet(expectedException, call, checkType);
        }

        //---------------------------------------------------------------------------
        // Wrapper for TextPattern.RangeFromChild Method
        //---------------------------------------------------------------------------
        internal void Pattern_RangeFromChild(TextPattern textPattern, ref TextPatternRange returnedRange, AutomationElement childElem, Type expectedException, CheckType checkType)
        {
            string call = "TextPattern.RangeFromChild(" + childElem + ")";
            Comment("---Calling " + call);

            try
            {
                returnedRange = textPattern.RangeFromChild(childElem);
            }
            catch (Exception actualException)
            {
                if (Library.IsCriticalException(actualException))
                    throw;

                if( (actualException is ArgumentException) || (actualException is InvalidOperationException))
                {
                    // Win32 Edit Controls don't support children. InvalidOperationException here is expected
                    if ((TextLibrary.typeOfProvider == "win32") || (TextLibrary.typeOfProvider == "winform"))
                    {
                        if ((childElem != null) && (TextLibrary.IsRichEdit(childElem) == false))
                        {
                            // Yes, this is a hard-coded CheckType. This is by-design/a good thing
                            ThrowMe(CheckType.IncorrectElementConfiguration, "Win32 Edit controls do not support children");
                        }
                    }
                }

                TestException(expectedException, actualException, call, checkType);
                return;
            }

            if (returnedRange == null)
                ThrowMe(checkType, call + " should not give a null TextPatternRange");

            TestNoExceptionQuiet(expectedException, call, checkType);
        }

        //---------------------------------------------------------------------------
        // Wrapper for TextPattern.GetVisibleRanges Method
        //---------------------------------------------------------------------------
        internal void Pattern_GetVisibleRanges(TextPattern textPattern, ref TextPatternRange[] returnedRanges, Type expectedException, CheckType checkType)
        {
            string call = "TextPattern.GetVisibleRanges()";
            Comment("---Calling " + call);

            try
            {
                returnedRanges = textPattern.GetVisibleRanges();
            }
            catch (Exception actualException)
            {
                if (Library.IsCriticalException(actualException))
                    throw;

                TestException(expectedException, actualException, call, checkType);
                return;
            }

            if (returnedRanges == null)
                ThrowMe(checkType, call + " should not give a null TextPatternRange");

            TestNoExceptionQuiet(expectedException, call, checkType);
        }

        #endregion Methods

        #endregion TextPattern

        #region TextPatternRange

        #region Properties

        //---------------------------------------------------------------------------
        // Wrapper for TextPatternRange.TextPattern Property
        //---------------------------------------------------------------------------
        static internal void Range_TextPattern(TextPatternRange callingRange, ref TextPattern pattern)
        {
            string call = "TextPatternRange.TextPattern";
            Comment("---Getting value of " + call);

            if (callingRange == null)
                throw new ArgumentNullException(call + " requires non-NULL TextPatterncallingRange");

            pattern = callingRange.TextPattern;
        }

        //---------------------------------------------------------------------------
        // Wrapper for TextPatternRange.Children Property
        //---------------------------------------------------------------------------
        static internal void Range_GetChildren(TextPatternRange callingRange, ref AutomationElement[] children,
                                CheckType checkType)
        {
            string call = "TextPatternRange.Children";

            if (callingRange == null)
                throw new ArgumentNullException(call + " requires non-NULL TextPatterncallingRange");

            children = callingRange.GetChildren();

            if (children == null)
                ThrowMe(checkType, "TextPatternRange.Children should not return NULL ");

            Comment("Property " + call + " returned array size " + children.Length);

        }

        #endregion Properties

        #region Methods

        //---------------------------------------------------------------------------
        // Wrapper for TextPatternRange.Clone Method
        //---------------------------------------------------------------------------
        internal void Range_Clone(TextPatternRange callingRange, ref TextPatternRange clonedRange, Type expectedException, CheckType checkType)
        {
            string call = "TextPatternRange.Clone()";
            Comment("---Calling " + call);

            if (callingRange == null)
                throw new ArgumentNullException(call + " requires non-NULL TextPatterncallingRange");

            try
            {
                clonedRange = callingRange.Clone();
            }
            catch (Exception actualException)
            {
                if (Library.IsCriticalException(actualException))
                    throw;

                TestException(expectedException, actualException, call, checkType);
                return;
            }

            if (clonedRange == null)
                ThrowMe(checkType, call + " should not give a null TextPatternRange");

            TestNoExceptionQuiet(expectedException, call, checkType);
        }

        //---------------------------------------------------------------------------
        // Wrapper for TextPatternRange.Compare Method
        //---------------------------------------------------------------------------
        internal void Range_Compare(TextPatternRange callingRange, TextPatternRange argumentRange, ref bool isEqual, Type expectedException, CheckType checkType)
        {
            string call = "TextPatternRange.Compare(" + argumentRange + ")";

            if (callingRange == null)
                throw new ArgumentNullException(call + " requires non-NULL TextPatterncallingRange");

            try
            {
                isEqual = callingRange.Compare(argumentRange);
                Comment("---Called " + call + ", returned = " + isEqual);

            }
            catch (Exception actualException)
            {
                if (Library.IsCriticalException(actualException))
                    throw;

                Comment("---Calling " + call);

                TestException(expectedException, actualException, call, checkType);
                return;
            }
            TestNoExceptionQuiet(expectedException, call, checkType);
        }

        //---------------------------------------------------------------------------
        // Wrapper for TextPatternRange.CompareEndpoints Method
        //---------------------------------------------------------------------------
        internal void Range_CompareEndpoints(TextPatternRange callingRange, TextPatternRangeEndpoint endPoint, TextPatternRange argumentRange, TextPatternRangeEndpoint targetEndPoint, ref int result, Type expectedException, CheckType checkType)
        {
            string call = "TextPatternRange.CompareEndpoints(" + endPoint + ", " + argumentRange + ", " + targetEndPoint + ")";

            if (callingRange == null)
                throw new ArgumentNullException(call + " requires non-NULL TextPatterncallingRange");

            try
            {
                result = callingRange.CompareEndpoints(endPoint, argumentRange, targetEndPoint);
                Comment("---Called " + call + " returning " + result);
            }
            catch (Exception actualException)
            {
                if (Library.IsCriticalException(actualException))
                    throw;

                Comment("---Calling " + call); // No results, so let user know we made the call

                TestException(expectedException, actualException, call, checkType);
                return;
            }
            TestNoExceptionQuiet(expectedException, call, checkType);
        }

        //---------------------------------------------------------------------------
        // Wrapper for TextPatternRange.Select Method
        //---------------------------------------------------------------------------
        internal void Range_Select(TextPatternRange callingRange, Type expectedException, CheckType checkType)
        {
            string call = "TextPatternRange.Select()";
            Comment("---Calling " + call);

            if (callingRange == null)
                throw new ArgumentNullException(call + " requires non-NULL TextPatterncallingRange");

            try
            {
                callingRange.Select();
            }
            catch (Exception actualException)
            {
                if (Library.IsCriticalException(actualException))
                    throw;

                TestException(expectedException, actualException, call, checkType);
                return;
            }
            TestNoExceptionQuiet(expectedException, call, checkType);
        }

        //---------------------------------------------------------------------------
        // Wrapper for TextPatternRange.FindAttribute Method
        //---------------------------------------------------------------------------
        internal void Range_FindAttribute(TextPatternRange callingRange, ref TextPatternRange targetRange, AutomationTextAttribute attrib, object val, bool backward, Type expectedException, CheckType checkType)
        {
            string call = "";
            if (attrib != null)
                call = "TextPatternRange.FindAttribute(" + Helpers.GetProgrammaticName(attrib) + ", " + (val == null ? "NULL" : val.ToString()) + ", " + backward + ")";
            else
                call = "TextPatternRange.FindAttribute(NULL," + (val == null ? "NULL" : val.ToString()) + ", " + backward + ")";
            Comment("---Calling " + call);

            if (callingRange == null)
                throw new ArgumentNullException(call + " requires non-NULL TextPatterncallingRange");

            try
            {
                targetRange = callingRange.FindAttribute(attrib, val, backward);
            }
            catch (Exception actualException)
            {
                if (Library.IsCriticalException(actualException))
                    throw;

                TestException(expectedException, actualException, call, checkType);
                return;
            }
            TestNoExceptionQuiet(expectedException, call, checkType);
        }

        //---------------------------------------------------------------------------
        // Wrapper for TextPatternRange.FindText Method
        //---------------------------------------------------------------------------
        internal void Range_FindText(TextPatternRange callingRange, ref TextPatternRange targetRange, string text, bool backward, bool ignoreCase, Type expectedException, CheckType checkType)
        {
            string call = "TextPatternRange.FindText(" + text + ", " + backward + ", " + ignoreCase + ")";
            Comment("---Calling " + call);

            if (callingRange == null)
                throw new ArgumentNullException(call + " requires non-NULL TextPatterncallingRange");

            try
            {
                targetRange = callingRange.FindText(text, backward, ignoreCase);
            }
            catch (Exception actualException)
            {
                if (Library.IsCriticalException(actualException))
                    throw;

                TestException(expectedException, actualException, call, checkType);
                return;
            }
            TestNoExceptionQuiet(expectedException, call, checkType);
        }

        //---------------------------------------------------------------------------
        // Wrapper for TextPatternRange.GetAttributeValue Method
        //---------------------------------------------------------------------------
        internal void Range_GetAttributeValue(TextPatternRange callingRange, AutomationTextAttribute attrib, ref object val, Type expectedException, CheckType checkType)
        {
            string call = "";
            if (attrib != null)
                call = "TextPatternRange.GetAttributeValue(" + Helpers.GetProgrammaticName(attrib) + ")";
            else
                call = "TextPatternRange.GetAttributeValue(NULL)";
            Comment("---Calling " + call);

            if (callingRange == null)
                throw new ArgumentNullException(call + " requires non-NULL TextPatterncallingRange");

            try
            {
                val = callingRange.GetAttributeValue(attrib);

                if (val.Equals(AutomationElement.NotSupported))
                {
                    string msg = Helpers.GetProgrammaticName(attrib) + " not supported by control";
                    Comment(msg);
                }
            }
            catch (Exception actualException)
            {
                if (Library.IsCriticalException(actualException))
                    throw;

                TestException(expectedException, actualException, call, checkType);
                return;
            }
            TestNoExceptionQuiet(expectedException, call, checkType);
        }

        //---------------------------------------------------------------------------
        // Wrapper for TextPatternRange.GetBoundingRectangles Method
        //---------------------------------------------------------------------------
        internal void Range_GetBoundingRectangles(TextPatternRange callingRange, ref Rect[] boundRects, Type expectedException, CheckType checkType)
        {
            string call = "TextPatternRange.GetBoundingRectangles()";

            if (callingRange == null)
                throw new ArgumentNullException(call + " requires non-NULL TextPatterncallingRange");

            try
            {
                boundRects = callingRange.GetBoundingRectangles();
                Comment("---Called " + call + ", returning array size = " + boundRects.Length);
            }
            catch (Exception actualException)
            {
                if (Library.IsCriticalException(actualException))
                    throw;

                Comment("---Calling " + call); // Exception was raised, so log it here...

                TestException(expectedException, actualException, call, checkType);
                return;
            }
            TestNoExceptionQuiet(expectedException, call, checkType);
        }

        //---------------------------------------------------------------------------
        // Wrapper for TextPatternRange.GetText Method
        //---------------------------------------------------------------------------
        internal void Range_GetText(TextPatternRange callingRange, ref string text, int maxLength, Type expectedException, CheckType checkType)
        {
            string call = "TextPatternRange.GetText(" + maxLength + ")";
            Comment("---Calling " + call);

            if (callingRange == null)
                throw new ArgumentNullException(call + " requires non-NULL TextPatterncallingRange");

            try
            {
                text = callingRange.GetText(maxLength);
            }
            catch (Exception actualException)
            {
                if (Library.IsCriticalException(actualException))
                    throw;

                //                     // Can't use BugIssues.PS1456324 here becasue 
                //                     // coneivably MOST of the 300 TextPattern tests could hit this
                //if (maxLength == -1) // Can't use TS_FilterOnException because of this special case
                //    ThrowMe(CheckType.KnownProductIssue, "Bug hit 1456324");

                TestException(expectedException, actualException, call, checkType);
                return;
            }
            TestNoExceptionQuiet(expectedException, call, checkType);
        }


        //---------------------------------------------------------------------------
        // Wrapper for TextPatternRange.GetText Method 
        // that "eats" InavlidOperationException's when raised
        // THis is so we can get all the text of a control without 1456324 causing
        // unexpected side-effects of the test infrastructure / tests whose scenarios 
        // may not obviously define them as having 1456324 as a "known issue"
        //---------------------------------------------------------------------------
        internal string Range_GetText(TextPatternRange callingRange)
        {
            string text = "";
            string call = "TextPatternRange.GetText(-1)";
            Comment("---Calling " + call);

            if (callingRange == null)
                throw new ArgumentNullException(call + " requires non-NULL TextPatterncallingRange");

            try
            {
                text = callingRange.GetText(-1);
            }
            catch (InvalidOperationException)
            {
                Comment("SOFT ERROR:  GetText(-1) raised InvalidOperationException. This is a known bug.");
                Comment("             1456324: BVT BLOCKER: GetText(-1) throws InvalidOperationException");
                ThrowMe(CheckType.IncorrectElementConfiguration, "Unable to continue with test due to known bug/side-effect");
            }
            return text;
        }

        //---------------------------------------------------------------------------
        // Wrapper for TextPatternRange.Move Method
        //---------------------------------------------------------------------------
        internal void Range_Move(TextPatternRange callingRange, TextUnit unit, int moveCount, ref int actualCount, Type expectedException, CheckType checkType)
        {
            string call = "TextPatternRange.Move(" + unit + ", " + moveCount + ")";

            if (callingRange == null)
                throw new ArgumentNullException(call + " requires non-NULL TextPatterncallingRange");

            try
            {
                actualCount = callingRange.Move(unit, moveCount);
                Comment("---Called " + call + ", returned = " + actualCount);

            }
            catch (Exception actualException)
            {
                if (Library.IsCriticalException(actualException))
                    throw;

                Comment("---Calling " + call); // we don't have a count, but still tell user we called

                TestException(expectedException, actualException, call, checkType);
                return;
            }
            TestNoExceptionQuiet(expectedException, call, checkType);
        }

        //---------------------------------------------------------------------------
        // Wrapper for TextPatternRange.MoveEndpointByUnit Method
        //---------------------------------------------------------------------------
        internal void Range_MoveEndpointByUnit(TextPatternRange callingRange, TextPatternRangeEndpoint endPoint, TextUnit unit, int count, ref int actualCount, Type expectedException, CheckType checkType)
        {
            string call = "TextPatternRange.MoveEndpointByUnit(" + endPoint + ", " + unit + ", " + count + ")";

            if (callingRange == null)
                throw new ArgumentNullException(call + " requires non-NULL TextPatterncallingRange");

            try
            {
                actualCount = callingRange.MoveEndpointByUnit(endPoint, unit, count);
                Comment("---Called " + call + ", returned = " + actualCount);
            }
            catch (Exception actualException)
            {
                if (Library.IsCriticalException(actualException))
                    throw;

                Comment("---Calling " + call); // we don't have a count, but still tell user we called

                TestException(expectedException, actualException, call, checkType);
                return;
            }
            TestNoExceptionQuiet(expectedException, call, checkType);
        }

        //---------------------------------------------------------------------------
        // Wrapper for TextPatternRange.MoveEndpointByRange Method
        //---------------------------------------------------------------------------
        internal void Range_MoveEndpointByRange(TextPatternRange callingRange, TextPatternRangeEndpoint endPoint, TextPatternRange argumentRange, TextPatternRangeEndpoint targetEndPoint, Type expectedException, CheckType checkType)
        {
            string call = "TextPatternRange.MoveEndpointByRange(" + endPoint + ", " + argumentRange + ", " + targetEndPoint + ")";
            Comment("---Calling " + call);

            if (callingRange == null)
                throw new ArgumentNullException(call + " requires non-NULL TextPatterncallingRange");

            try
            {
                callingRange.MoveEndpointByRange(endPoint, argumentRange, targetEndPoint);
            }
            catch (Exception actualException)
            {
                if (Library.IsCriticalException(actualException))
                    throw;

                TestException(expectedException, actualException, call, checkType);
                return;
            }
            TestNoExceptionQuiet(expectedException, call, checkType);
        }

        //---------------------------------------------------------------------------
        // Wrapper for TextPatternRange.ExpandToEnclosingUnit Method
        //---------------------------------------------------------------------------
        internal void Range_ExpandToEnclosingUnit(TextPatternRange callingRange, TextUnit unit, Type expectedException, CheckType checkType)
        {
            string call = "TextPatternRange.ExpandToEnclosingUnit(" + unit + ")";
            Comment("---Calling " + call);

            if (callingRange == null)
                throw new ArgumentNullException(call + " requires non-NULL TextPatterncallingRange");

            try
            {
                callingRange.ExpandToEnclosingUnit(unit);
            }
            catch (Exception actualException)
            {
                if (Library.IsCriticalException(actualException))
                    throw;

                TestException(expectedException, actualException, call, checkType);
                return;
            }
            TestNoExceptionQuiet(expectedException, call, checkType);
        }

        //---------------------------------------------------------------------------
        // Wrapper for TextPatternRange.ScrollIntoView Method
        //---------------------------------------------------------------------------
        internal void Range_ScrollIntoView(TextPatternRange callingRange, bool alignToTop, Type expectedException, CheckType checkType)
        {
            string call = "TextPatternRange.ScrollIntoView(" + alignToTop + ")";
            Comment("---Calling " + call);

            if (callingRange == null)
                throw new ArgumentNullException(call + " requires non-NULL TextPatterncallingRange");

            try
            {
                callingRange.ScrollIntoView(alignToTop);
            }
            catch (Exception actualException)
            {
                if (Library.IsCriticalException(actualException))
                    throw;

                TestException(expectedException, actualException, call, checkType);
                return;
            }
            TestNoExceptionQuiet(expectedException, call, checkType);
        }

        //---------------------------------------------------------------------------
        // Wrapper for TextPatternRange.GetEnclosingElement Method
        //---------------------------------------------------------------------------
        internal void Range_GetEnclosingElement(TextPatternRange callingRange, ref AutomationElement parent, Type expectedException, CheckType checkType)
        {
            string call = "TextPatternRange.GetEnclosingElement()";
            Comment("---Calling " + call);

            if (callingRange == null)
                throw new ArgumentNullException(call + " requires non-NULL TextPatterncallingRange");

            try
            {
                parent = callingRange.GetEnclosingElement();
            }
            catch (Exception actualException)
            {
                if (Library.IsCriticalException(actualException))
                    throw;

                TestException(expectedException, actualException, call, checkType);
                return;
            }
            TestNoExceptionQuiet(expectedException, call, checkType);
        }

        //---------------------------------------------------------------------------
        // Wrapper for TextPatternRange.AddToSelection Method
        //---------------------------------------------------------------------------
        internal void Range_AddToSelection(TextPatternRange callingRange, Type expectedException, CheckType checkType)
        {
            string call = "TextPatternRange.AddToSelection()";
            Comment("---Calling " + call);

            if (callingRange == null)
                throw new ArgumentNullException(call + " requires non-NULL TextPatterncallingRange");

            try
            {
                callingRange.AddToSelection();
            }
            catch (Exception actualException)
            {
                if (Library.IsCriticalException(actualException))
                    throw;

                // Bug 1746355: Throw "NotSupported" from TextPattern methods that...um...aren't supported
                if ((actualException is NotSupportedException) && (_frameworkId == "wpf"))
                {
                    NotSupportedException nse = new NotSupportedException();

                    TestException(nse.GetType(), actualException, call, checkType);
                }
                else
                    TestException(expectedException, actualException, call, checkType);
                return;
            }
            TestNoExceptionQuiet(expectedException, call, checkType);
        }

        //---------------------------------------------------------------------------
        // Wrapper for TextPatternRange.RemoveFromSelection Method
        //---------------------------------------------------------------------------
        internal void Range_RemoveFromSelection(TextPatternRange callingRange, Type expectedException, CheckType checkType)
        {
            string call = "TextPatternRange.RemoveFromSelection()";
            Comment("---Calling " + call);

            if (callingRange == null)
                throw new ArgumentNullException(call + " requires non-NULL TextPatterncallingRange");

            try
            {
                callingRange.RemoveFromSelection();
            }
            catch (Exception actualException)
            {
                if (Library.IsCriticalException(actualException))
                    throw;

                // Bug 1746355: Throw "NotSupported" from TextPattern methods that...um...aren't supported
                if ((actualException is NotSupportedException) && (_frameworkId == "wpf"))
                {
                    NotSupportedException nse = new NotSupportedException();

                    TestException(nse.GetType(), actualException, call, checkType);
                }
                else
                    TestException(expectedException, actualException, call, checkType);
                return;
            }
            TestNoExceptionQuiet(expectedException, call, checkType);
        }

        #endregion Methods

        #endregion TextPatternRange
    }

    #endregion TextWrapper Class
}

namespace Microsoft.Test.UIAutomation.Tests.Patterns
{
    using InternalHelper;
    using InternalHelper.Tests;
    using InternalHelper.Tests.Patterns;
    using InternalHelper.Enumerations;
    using Microsoft.Test.UIAutomation;
    using Microsoft.Test.UIAutomation.Core;
    using Microsoft.Test.UIAutomation.TestManager;
    using Microsoft.Test.UIAutomation.Interfaces;

    #region TextTests class

    ///---------------------------------------------------------------------------
    /// <summary></summary>
    ///---------------------------------------------------------------------------
    public sealed class TextTests : TextTestsHelper
    {
        #region Member Variables

        const string THIS = "TextTests";

        ///---------------------------------------------------------------------------
        /// <summary></summary>
        ///---------------------------------------------------------------------------
        public const string TestSuite = NAMESPACE + "." + THIS;

        ///---------------------------------------------------------------------------
        /// <summary>Defines which UIAutomation Pattern this tests</summary>
        ///---------------------------------------------------------------------------
        public static readonly string TestWhichPattern = Automation.PatternName(TextPattern.Pattern);

        #endregion Member Variables

        #region TextTests Constructor

        ///---------------------------------------------------------------------------
        /// <summary></summary>
        ///---------------------------------------------------------------------------
        [SuppressMessage("Microsoft.Globalization", "CA1303:DoNotPassLiteralsAsLocalizedParameters", MessageId = "System.ArgumentException.#ctor(System.String)")]
        public TextTests(AutomationElement element, TestPriorities priority, string dirResults, bool testEvents, TypeOfControl typeOfControl, IApplicationCommands commands)
            : base(element, TestSuite, priority, typeOfControl, TypeOfPattern.Text, dirResults, testEvents, commands)
        {
            if (element == null)
                throw new ArgumentException("element cannot be null");

            _pattern = (TextPattern)element.GetCurrentPattern(TextPattern.Pattern);
        }

        #endregion TextTests Constructor

        #region TestCase

        #region TextPattern

        #region Properties

        #region 1.1 TextPattern.DocumentRange Property

        ///<summary>TextPattern.DocumentRange Property.T.1.1</summary>
        [TestCaseAttribute("TextPattern.DocumentRange Property.T.1.1",
            TestSummary = "Verify TextPattern.DocumentRange is non-null for any TextPattern",
            Priority = TestPriorities.Pri0,
            Status = TestStatus.Works,
            Author = "Microsoft Corp.",
            TestCaseType = TestCaseType.Generic,
            Description = new string[]
            {
                "Pre-Condition: Set text to 'String1'",
                "Verfiy TextPattern.DocumentRange is non-NULL"
            })]
        public void TestDocumentRangeProperty11(TestCaseAttribute testCase)
        {
            HeaderComment(testCase);

            DocumentRangeHelper(SampleText.String1);
        }

        #endregion 1.1 TextPattern.DocumentRange Property

        #region 1.2 TextPattern.SupportedTextSelection Property

        ///<summary>TextPattern.SupportedTextSelection Property.T.1.2.1</summary>
        [TestCaseAttribute("TextPattern.SupportedTextSelection Property.T.1.2.1",
            TestSummary = "Verify SupportedTextSelection is true for controls that we expect to be true",
            Priority = TestPriorities.Pri0,
            Status = TestStatus.Works,
            Author = "Microsoft Corp.",
            TestCaseType = TestCaseType.Generic,
            Description = new string[]
            {
                "Pre-Condition: Verify text is expected value: Value of text does not matter for test",
                "Get SupportedTextSelection property",
                "Validate actual result against known controls"
            })]
        public void TestSupportedTextSelectionProperty121(TestCaseAttribute testCase)
        {
            HeaderComment(testCase);

            SupportedTextSelectionHelper(SampleText.NotApplicable);
        }

        #endregion 1.2 TextPattern.SupportedTextSelection Property

        #endregion Properties

        #region Methods

        #region 2.1 TextPattern.RangeFromPoint Method

        ///<summary>TextPattern.RangeFromPoint Method.T.2.01.01</summary>
        [TestCaseAttribute("TextPattern.RangeFromPoint Method.T.2.01.01",
            TestSummary = "Verify that range returned is within visbile bounding rectangles of control",
            Priority = TestPriorities.Pri0,
            Status = TestStatus.Works,
            Author = "Microsoft Corp.",
            TestCaseType = TestCaseType.Generic,
            Description = new string[]
            {
                "Pre-Condition: Verify text is expected value: Set text to 'String1'",
                "Pre-Condition: Create empty calling range @ Start of TextPattern.DocumentRange",
                "Pre-Condition: Expand End endPoint by 1 character(s)",
                "Pre-Condition: Scroll range into view",
                "Pre-Condition: Call GetBoundingRectangles() on that range",
                "Pre-Condition: Create a point Inside Top Left of bounding rect",
                "Call RangeFromPoint on point without errors",
                "Verify that range returned is Target Range within Calling range"
            })]
        public void TestRangeFromPointMethod20101(TestCaseAttribute testCase)
        {
            HeaderComment(testCase);


            RangeFromPointHelper(SampleText.String1, 1, TextPatternRangeEndpoint.End, RangeLocation.Start, BoundingRectangleLocation.InsideTopLeft, RangeCompare.Contained, null);
        }

        ///<summary>TextPattern.RangeFromPoint Method.T.2.01.02</summary>
        [TestCaseAttribute("TextPattern.RangeFromPoint Method.T.2.01.02",
            TestSummary = "Verify that range returned is within visbile bounding rectangles of control",
            Priority = TestPriorities.Pri1,
            Status = TestStatus.Works,
            Author = "Microsoft Corp.",
            TestCaseType = TestCaseType.Generic,
            Description = new string[]
            {
                "Pre-Condition: Verify text is expected value: Verify text is NOT empty (or SET text if it is empty)",
                "Pre-Condition: Create empty calling range @ Start of TextPattern.DocumentRange",
                "Pre-Condition: Expand End endPoint by 1 character(s)",
                "Pre-Condition: Scroll range into view",
                "Pre-Condition: Call GetBoundingRectangles() on that range",
                "Pre-Condition: Create a point Inside Top Left of bounding rect",
                "Call RangeFromPoint on point without errors",
                "Verify that range returned is Target Range within Calling range"
            })]
        public void TestRangeFromPointMethod20102(TestCaseAttribute testCase)
        {
            HeaderComment(testCase);


            RangeFromPointHelper(SampleText.NotEmpty, 1, TextPatternRangeEndpoint.End, RangeLocation.Start, BoundingRectangleLocation.InsideTopLeft, RangeCompare.Contained, null);
        }

        ///<summary>TextPattern.RangeFromPoint Method.T.2.01.04</summary>
        [TestCaseAttribute("TextPattern.RangeFromPoint Method.T.2.01.04",
            TestSummary = "Verify that range returned is within visbile bounding rectangles of control",
            Priority = TestPriorities.Pri1,
            Status = TestStatus.Blocked, //ScrollIntoView locks up
            ProblemDescription = "Bug(s): 1346630--CtrlTestEdit(1003)",
            Author = "Microsoft Corp.",
            TestCaseType = TestCaseType.Generic,
            Description = new string[]
            {
                "Pre-Condition: Verify text is expected value: Verify text is NOT empty (or SET text if it is empty)",
                "Pre-Condition: Create empty calling range @ End of TextPattern.DocumentRange",
                "Pre-Condition: Expand Start endPoint by -1 character(s)",
                "Pre-Condition: Scroll range into view",
                "Pre-Condition: Call GetBoundingRectangles() on that range",
                "Pre-Condition: Create a point Inside Bottom Right of bounding rect",
                "Call RangeFromPoint on point without errors",
                "Verify that range returned is Target Range within Calling range"
            })]
        public void TestRangeFromPointMethod20104(TestCaseAttribute testCase)
        {
            HeaderComment(testCase);


            RangeFromPointHelper(SampleText.NotEmpty, -1, TextPatternRangeEndpoint.Start, RangeLocation.End, BoundingRectangleLocation.InsideBottomRight, RangeCompare.Contained, null);
        }

        ///<summary>TextPattern.RangeFromPoint Method.T.2.01.09</summary>
        [TestCaseAttribute("TextPattern.RangeFromPoint Method.T.2.01.09",
            TestSummary = "Verify that range with two adjacent characters is within visbile bounding rectangles of control",
            Priority = TestPriorities.Pri2,
            Status = TestStatus.Works,
            Author = "Microsoft Corp.",
            TestCaseType = TestCaseType.Generic,
            Description = new string[]
            {
                "Pre-Condition: Control must supports multi-line = false",
                "Pre-Condition: Verify text is expected value: Set text to OneTwoThree...Nine",
                "Pre-Condition: Create range equal to: two adjacent characters",
                "Pre-Condition: Scroll range into view",
                "Pre-Condition: Call GetBoundingRectangles() on that range",
                "Pre-Condition: Create a point Inside Top Left of bounding rect",
                "Call RangeFromPoint on point without errors",
                "Verify that range returned is Target Range within Calling range"
            })]
        public void TestRangeFromPointMethod20109(TestCaseAttribute testCase)
        {
            HeaderComment(testCase);

            RangeFromPointHelper2(SampleText.OneTwoThree, false, TargetRangeType.TwoCharsAdjacent, BoundingRectangleLocation.InsideTopLeft, RangeCompare.Contained);
        }

        ///<summary>TextPattern.RangeFromPoint Method.T.2.01.10</summary>
        [TestCaseAttribute("TextPattern.RangeFromPoint Method.T.2.01.10",
            TestSummary = "Verify that range with two adjacent characters is within visbile bounding rectangles of control",
            Priority = TestPriorities.Pri2,
            Status = TestStatus.Works,
            Author = "Microsoft Corp.",
            TestCaseType = TestCaseType.Generic,
            Description = new string[]
            {
                "Pre-Condition: Control must supports multi-line = false",
                "Pre-Condition: Verify text is expected value: Set text to OneTwoThree...Nine",
                "Pre-Condition: Create range equal to: two adjacent characters",
                "Pre-Condition: Scroll range into view",
                "Pre-Condition: Call GetBoundingRectangles() on that range",
                "Pre-Condition: Create a point Within first character of bounding rect",
                "Call RangeFromPoint on point without errors",
                "Verify that range returned is Target Range is within DocumentRange"
            })]
        public void TestRangeFromPointMethod20110(TestCaseAttribute testCase)
        {
            HeaderComment(testCase);

            RangeFromPointHelper2(SampleText.OneTwoThree, false, TargetRangeType.TwoCharsAdjacent, BoundingRectangleLocation.FirstChar, RangeCompare.WithinDocRange);
        }

        ///<summary>TextPattern.RangeFromPoint Method.T.2.01.11</summary>
        [TestCaseAttribute("TextPattern.RangeFromPoint Method.T.2.01.11",
            TestSummary = "Verify that range with two adjacent characters is within visbile bounding rectangles of control",
            Priority = TestPriorities.Pri2,
            Status = TestStatus.Works,
            Author = "Microsoft Corp.",
            TestCaseType = TestCaseType.Generic,
            Description = new string[]
            {
                "Pre-Condition: Control must supports multi-line = true",
                "Pre-Condition: Verify text is expected value: Set text to One\nTwo\nThree\n...\nNine",
                "Pre-Condition: Create range equal to: two characters split across a line break",
                "Pre-Condition: Scroll range into view",
                "Pre-Condition: Call GetBoundingRectangles() on that range",
                "Pre-Condition: Create a point Within first character in range of bounding rect",
                "Call RangeFromPoint on point without errors",
                "Verify that range returned is Target Range is within DocumentRange"
            })]
        public void TestRangeFromPointMethod20111(TestCaseAttribute testCase)
        {
            HeaderComment(testCase);

            RangeFromPointHelper2(SampleText.OneCRTwoCRThreeCR, true, TargetRangeType.TwoCharsSplitAcrossLine, BoundingRectangleLocation.FirstCharInRange, RangeCompare.WithinDocRange);
        }

        ///<summary>TextPattern.RangeFromPoint Method.T.2.01.12</summary>
        [TestCaseAttribute("TextPattern.RangeFromPoint Method.T.2.01.12",
            TestSummary = "Verify that range with two adjacent characters is within visbile bounding rectangles of control",
            Priority = TestPriorities.Pri2,
            Status = TestStatus.Works,
            Author = "Microsoft Corp.",
            TestCaseType = TestCaseType.Generic,
            Description = new string[]
            {
                "Pre-Condition: Control must supports multi-line = true",
                "Pre-Condition: Verify text is expected value: Set text to One\nTwo\nThree\n...\nNine",
                "Pre-Condition: Create range equal to: two characters split across a line break",
                "Pre-Condition: Scroll range into view",
                "Pre-Condition: Call GetBoundingRectangles() on that range",
                "Pre-Condition: Create a point Within last character in range of bounding rect",
                "Call RangeFromPoint on point without errors",
                "Verify that range returned is Target Range within Calling range"
            })]
        public void TestRangeFromPointMethod20112(TestCaseAttribute testCase)
        {
            HeaderComment(testCase);

            RangeFromPointHelper2(SampleText.OneCRTwoCRThreeCR, true, TargetRangeType.TwoCharsSplitAcrossLine, BoundingRectangleLocation.LastCharInRange, RangeCompare.Contained);
        }

        #endregion 2.1 TextPattern.RangeFromPoint Metho d

        #region 2.2 TextPattern.GetSelection Method

        ///<summary>TextPattern.GetSelection Method.T.2.02.01</summary>
        [TestCaseAttribute("TextPattern.GetSelection Method.T.2.02.01",
            TestSummary = "Performs GetSelection() on control with all text selected",
            Priority = TestPriorities.Pri0,
            Status = TestStatus.Works,
            Author = "Microsoft Corp.",
            TestCaseType = TestCaseType.Generic,
            Description = new string[]
            {
                "Pre-Condition: Verify SupportedTextSelection=only single selection of text is supported",
                "Pre-Condition: Verify text is expected value: Set text to 'String1'",
                "Pre-Condition: Create calling range=entire document",
                "Call Select()",
                "Call GetSelection() without errors, returning array size 1",
                "Compare selected range and calling range, comparison should match"
            })]
        public void TestGetSelectionMethod20201(TestCaseAttribute testCase)
        {
            HeaderComment(testCase);


            GetSelectionHelper(SampleText.String1, SupportedTextSelection.Single, TargetRangeType.DocumentRange, true, 1, null);
        }

        ///<summary>TextPattern.GetSelection Method.T.2.02.02</summary>
        [TestCaseAttribute("TextPattern.GetSelection Method.T.2.02.02",
            TestSummary = "Performs GetSelection() on control with all text selected",
            Priority = TestPriorities.Pri1,
            Status = TestStatus.Works,
            Author = "Microsoft Corp.",
            TestCaseType = TestCaseType.Generic,
            Description = new string[]
            {
                "Pre-Condition: Verify SupportedTextSelection=only single selection of text is supported",
                "Pre-Condition: Verify text is expected value: Set text to be RANDOM 256 characters",
                "Pre-Condition: Create calling range=entire document",
                "Call Select()",
                "Call GetSelection() without errors, returning array size 1",
                "Compare selected range and calling range, comparison should match"
            })]
        public void TestGetSelectionMethod20202(TestCaseAttribute testCase)
        {
            HeaderComment(testCase);


            GetSelectionHelper(SampleText.Random256, SupportedTextSelection.Single, TargetRangeType.DocumentRange, true, 1, null);
        }

        ///<summary>TextPattern.GetSelection Method.T.2.02.03</summary>
        [TestCaseAttribute("TextPattern.GetSelection Method.T.2.02.03",
            TestSummary = "Performs GetSelection() on control with all text selected",
            Priority = TestPriorities.Pri1,
            Status = TestStatus.Works,
            Author = "Microsoft Corp.",
            TestCaseType = TestCaseType.Generic,
            Description = new string[]
            {
                "Pre-Condition: Verify SupportedTextSelection=only single selection of text is supported",
                "Pre-Condition: Verify text is expected value: Set text to be RANDOM 256 characters",
                "Pre-Condition: Create calling range=RANDOM range in MIDDLE of document",
                "Call Select()",
                "Call GetSelection() without errors, returning array size 1",
                "Compare selected range and calling range, comparison should match"
            })]
        public void TestGetSelectionMethod20203(TestCaseAttribute testCase)
        {
            HeaderComment(testCase);


            GetSelectionHelper(SampleText.Random256, SupportedTextSelection.Single, TargetRangeType.RandomMiddle, true, 1, null);
        }

        ///<summary>TextPattern.GetSelection Method.T.2.02.04</summary>
        [TestCaseAttribute("TextPattern.GetSelection Method.T.2.02.04",
            TestSummary = "Performs GetSelection() on control with all text selected",
            Priority = TestPriorities.Pri1,
            Status = TestStatus.Works,
            Author = "Microsoft Corp.",
            TestCaseType = TestCaseType.Generic,
            Description = new string[]
            {
                "Pre-Condition: Verify SupportedTextSelection=only single selection of text is supported",
                "Pre-Condition: Verify text is expected value: Set text to be RANDOM 256 characters",
                "Pre-Condition: Create calling range=EMPTY range at START of document",
                "Call Select()",
                "Call GetSelection() without errors, returning array size 1",
                "Compare selected range and calling range, comparison should match"
            })]
        public void TestGetSelectionMethod20204(TestCaseAttribute testCase)
        {
            HeaderComment(testCase);


            GetSelectionHelper(SampleText.Random256, SupportedTextSelection.Single, TargetRangeType.EmptyStart, true, 1, null);
        }

        ///<summary>TextPattern.GetSelection Method.T.2.02.05</summary>
        [TestCaseAttribute("TextPattern.GetSelection Method.T.2.02.05",
            TestSummary = "Performs GetSelection() on control with all text selected",
            Priority = TestPriorities.Pri1,
            Status = TestStatus.Works,
            Author = "Microsoft Corp.",
            TestCaseType = TestCaseType.Generic,
            Description = new string[]
            {
                "Pre-Condition: Verify SupportedTextSelection=only single selection of text is supported",
                "Pre-Condition: Verify text is expected value: Set text to EMPTY string",
                "Pre-Condition: Create calling range=EMPTY range at START of document",
                "Call Select()",
                "Call GetSelection() without errors, returning array size 1",
                "Compare selected range and calling range, comparison should match"
            })]
        public void TestGetSelectionMethod20205(TestCaseAttribute testCase)
        {
            HeaderComment(testCase);


            GetSelectionHelper(SampleText.Empty, SupportedTextSelection.Single, TargetRangeType.EmptyStart, true, 1, null);
        }

        ///<summary>TextPattern.GetSelection Method.T.2.02.06</summary>
        [TestCaseAttribute("TextPattern.GetSelection Method.T.2.02.06",
            TestSummary = "Performs GetSelection() on control with all text selected",
            Priority = TestPriorities.Pri1,
            Status = TestStatus.Works,
            Author = "Microsoft Corp.",
            TestCaseType = TestCaseType.Generic,
            Description = new string[]
            {
                "Pre-Condition: Verify SupportedTextSelection=multiple selections of text is supported",
                "Pre-Condition: Verify text is expected value: Set text to be RANDOM 256 characters",
                "Pre-Condition: Create calling range=entire document",
                "Call Select()",
                "Call GetSelection() without errors, returning array size 1",
                "Compare selected range and calling range, comparison should match"
            })]
        public void TestGetSelectionMethod20206(TestCaseAttribute testCase)
        {
            HeaderComment(testCase);


            GetSelectionHelper(SampleText.Random256, SupportedTextSelection.Multiple, TargetRangeType.DocumentRange, true, 1, null);
        }

        ///<summary>TextPattern.GetSelection Method.T.2.02.07</summary>
        [TestCaseAttribute("TextPattern.GetSelection Method.T.2.02.07",
            TestSummary = "Performs GetSelection() on control with all text selected",
            Priority = TestPriorities.Pri1,
            Status = TestStatus.Works,
            Author = "Microsoft Corp.",
            TestCaseType = TestCaseType.Generic,
            Description = new string[]
            {
                "Pre-Condition: Verify SupportedTextSelection=multiple selections of text is supported",
                "Pre-Condition: Verify text is expected value: Set text to be RANDOM 256 characters",
                "Pre-Condition: Create calling range=RANDOM range in MIDDLE of document",
                "Call Select()",
                "Call GetSelection() without errors, returning array size 1",
                "Compare selected range and calling range, comparison should match"
            })]
        public void TestGetSelectionMethod20207(TestCaseAttribute testCase)
        {
            HeaderComment(testCase);


            GetSelectionHelper(SampleText.Random256, SupportedTextSelection.Multiple, TargetRangeType.RandomMiddle, true, 1, null);
        }

        ///<summary>TextPattern.GetSelection Method.T.2.02.08</summary>
        [TestCaseAttribute("TextPattern.GetSelection Method.T.2.02.08",
            TestSummary = "Performs GetSelection() on control with all text selected",
            Priority = TestPriorities.Pri1,
            Status = TestStatus.Works,
            Author = "Microsoft Corp.",
            TestCaseType = TestCaseType.Generic,
            Description = new string[]
            {
                "Pre-Condition: Verify SupportedTextSelection=multiple selections of text is supported",
                "Pre-Condition: Verify text is expected value: Set text to be RANDOM 256 characters",
                "Pre-Condition: Create calling range=EMPTY range at START of document",
                "Call Select()",
                "Call GetSelection() without errors, returning array size 1",
                "Compare selected range and calling range, comparison should match"
            })]
        public void TestGetSelectionMethod20208(TestCaseAttribute testCase)
        {
            HeaderComment(testCase);


            GetSelectionHelper(SampleText.Random256, SupportedTextSelection.Multiple, TargetRangeType.EmptyStart, true, 1, null);
        }

        ///<summary>TextPattern.GetSelection Method.T.2.02.09</summary>
        [TestCaseAttribute("TextPattern.GetSelection Method.T.2.02.09",
            TestSummary = "Performs GetSelection() on control with all text selected",
            Priority = TestPriorities.Pri1,
            Status = TestStatus.Works,
            Author = "Microsoft Corp.",
            TestCaseType = TestCaseType.Generic,
            Description = new string[]
            {
                "Pre-Condition: Verify SupportedTextSelection=multiple selections of text is supported",
                "Pre-Condition: Verify text is expected value: Set text to EMPTY string",
                "Pre-Condition: Create calling range=EMPTY range at START of document",
                "Call Select()",
                "Call GetSelection() without errors, returning array size 1",
                "Compare selected range and calling range, comparison should match"
            })]
        public void TestGetSelectionMethod20209(TestCaseAttribute testCase)
        {
            HeaderComment(testCase);


            GetSelectionHelper(SampleText.Empty, SupportedTextSelection.Multiple, TargetRangeType.EmptyStart, true, 1, null);
        }

        #endregion 2.2 TextPattern.GetSelection Method

        #region 2.3 TextPattern.RangeFromChild Method

        ///<summary>TextPattern.RangeFromChild Method.T.2.03.01</summary>
        [TestCaseAttribute("TextPattern.RangeFromChild Method.T.2.03.01",
            TestSummary = "Call RangeFromChild for given automation elements",
            Priority = TestPriorities.Pri1,
            Status = TestStatus.Works,
            Author = "Microsoft Corp.",
            TestCaseType = TestCaseType.Generic,
            Description = new string[]
            {
                "Pre-Condition: Verify text is expected value: Set text to 'String1'",
                "Pre-Condition: Verify range children property is non-null",
                "Pre-Condition: Acquire automation element(s) from TextPattern.Children proeprty",
                "Iterate through each child, calling RangeFromChild without errors"
            })]
        [SuppressMessage("Microsoft.Globalization", "CA1303:DoNotPassLiteralsAsLocalizedParameters")]
        public void TestRangeFromChildMethod20301(TestCaseAttribute testCase)
        {
            HeaderComment(testCase);


            RangeFromChildHelper(SampleText.String1, true, AutoElementType.UseChildren, null);
        }

        ///<summary>TextPattern.RangeFromChild Method.T.2.03.02</summary>
        [TestCaseAttribute("TextPattern.RangeFromChild Method.T.2.03.02",
            TestSummary = "Call RangeFromChild for given automation elements",
            Priority = TestPriorities.Pri1,
            Status = TestStatus.Works,
            Author = "Microsoft Corp.",
            TestCaseType = TestCaseType.Generic,
            Description = new string[]
            {
                "Pre-Condition: Verify text is expected value: Set text to 'String1'",
                "Pre-Condition: Verify range children property is doesn't matter",
                "Pre-Condition: Acquire automation element(s) that contains this pattern",
                "Iterate through each child, calling RangeFromChild RAISES System.InvalidOperationException exception"
            })]
        [SuppressMessage("Microsoft.Globalization", "CA1303:DoNotPassLiteralsAsLocalizedParameters")]
        public void TestRangeFromChildMethod20302(TestCaseAttribute testCase)
        {
            HeaderComment(testCase);

            InvalidOperationException ex = new InvalidOperationException("Unused, just need an instance for GetType() below");

            RangeFromChildHelper(SampleText.String1, false, AutoElementType.SameAsPattern, ex.GetType());
        }

        ///<summary>TextPattern.RangeFromChild Method.T.2.03.04</summary>
        [TestCaseAttribute("TextPattern.RangeFromChild Method.T.2.03.04",
            TestSummary = "Call RangeFromChild for given automation elements",
            Priority = TestPriorities.Pri1,
            Status = TestStatus.Works,
            Author = "Microsoft Corp.",
            TestCaseType = TestCaseType.Generic,
            Description = new string[]
            {
                "Pre-Condition: Verify text is expected value: Set text to 'String1'",
                "Pre-Condition: Verify range children property is doesn't matter",
                "Pre-Condition: Acquire automation element(s) equal to null ",
                "Iterate through each child, calling RangeFromChild RAISES System.ArgumentNullException exception"
            })]
        [SuppressMessage("Microsoft.Globalization", "CA1303:DoNotPassLiteralsAsLocalizedParameters")]
        public void TestRangeFromChildMethod20304(TestCaseAttribute testCase)
        {
            HeaderComment(testCase);

            ArgumentNullException ex = new ArgumentNullException("testCase"); //  Unused, just need an instance for GetType() below

            RangeFromChildHelper(SampleText.String1, false, AutoElementType.Null, ex.GetType());
        }

        #endregion 2.3 TextPattern.RangeFromChild Method

        #region 2.4 TextPattern.GetVisibleRanges Method

        ///<summary>TextPattern.GetVisibleRange Method.T.2.04.01</summary>
        [TestCaseAttribute("TextPattern.GetVisibleRanges Method.T.2.04.01",
            TestSummary = "GetVisibleRanges on a single/multi-line control ",
            Priority = TestPriorities.Pri0,
            Status = TestStatus.Works,
            ProblemDescription = "Bug(s): 1456324--(Jpn) Edit*",
            Author = "Microsoft Corp.",
            TestCaseType = TestCaseType.Generic,
            Description = new string[]
            {
                "Pre-Condition: Control must supports multi-line = false",
                "Pre-Condition: Control text selection = only single selection of text is supported",
                "Pre-Condition: Verify text is expected value: Set text to 'String1'",
                "Call GetVisibleRanges on the control with no errors, returning array size 1",
                "Call GetText() on FIRST visible range (post V1, this should be on all ranges)",
                "Verify that text range matches what was set in the control"
            })]
        public void TestGetVisibleRangesMethod20401(TestCaseAttribute testCase)
        {
            HeaderComment(testCase);

            GetVisibleRangesHelper(SampleText.String1, false, SupportedTextSelection.Single, 1);
        }

        ///<summary>TextPattern.GetVisibleRange Method.T.2.04.02</summary>
        [TestCaseAttribute("TextPattern.GetVisibleRanges Method.T.2.04.02",
            TestSummary = "GetVisibleRanges on a single/multi-line control ",
            Priority = TestPriorities.Pri1,
            Status = TestStatus.Works,
            ProblemDescription = "Bug(s): 1456324--(Jpn) Edit*",
            Author = "Microsoft Corp.",
            TestCaseType = TestCaseType.Generic,
            Description = new string[]
            {
                "Pre-Condition: Control must supports multi-line = false",
                "Pre-Condition: Control text selection = only single selection of text is supported",
                "Pre-Condition: Verify text is expected value: Set text to be RANDOM 256 characters",
                "Call GetVisibleRanges on the control with no errors, returning array size 1",
                "Call GetText() on FIRST visible range (post V1, this should be on all ranges)",
                "Verify that text range matches what was set in the control"
            })]
        public void TestGetVisibleRangesMethod20402(TestCaseAttribute testCase)
        {
            HeaderComment(testCase);

            GetVisibleRangesHelper(SampleText.Random256, false, SupportedTextSelection.Single, 1);
        }

        ///<summary>TextPattern.GetVisibleRange Method.T.2.04.03</summary>
        [TestCaseAttribute("TextPattern.GetVisibleRanges Method.T.2.04.03",
            TestSummary = "GetVisibleRanges on a single/multi-line control ",
            Priority = TestPriorities.Pri1,
            Status = TestStatus.Works,
            ProblemDescription = "Bug(s): 1456324--(Jpn) Edit*",
            Author = "Microsoft Corp.",
            TestCaseType = TestCaseType.Generic,
            Description = new string[]
            {
                "Pre-Condition: Control must supports multi-line = true",
                "Pre-Condition: Control text selection = only single selection of text is supported",
                "Pre-Condition: Verify text is expected value: Set text to span MULTIPLE LINES",
                "Call GetVisibleRanges on the control with no errors, returning array size 1",
                "Call GetText() on FIRST visible range (post V1, this should be on all ranges)",
                "Verify that text range matches what was set in the control"
            })]
        public void TestGetVisibleRangesMethod20403(TestCaseAttribute testCase)
        {
            HeaderComment(testCase);

            GetVisibleRangesHelper(SampleText.MultipleLines, true, SupportedTextSelection.Single, 1);
        }

        ///<summary>TextPattern.GetVisibleRange Method.T.2.04.04</summary>
        [TestCaseAttribute("TextPattern.GetVisibleRanges Method.T.2.04.04",
            TestSummary = "GetVisibleRanges on a single/multi-line control ",
            Priority = TestPriorities.Pri1,
            Status = TestStatus.Works,
            ProblemDescription = "Bug(s): 1456324--(Jpn) Edit*",
            Author = "Microsoft Corp.",
            TestCaseType = TestCaseType.Generic,
            Description = new string[]
            {
                "Pre-Condition: Control must supports multi-line = false",
                "Pre-Condition: Control text selection = only single selection of text is supported",
                "Pre-Condition: Verify text is expected value: Set text to EMPTY string",
                "Call GetVisibleRanges on the control with no errors, returning array size 0",
                "Call GetText() on FIRST visible range (post V1, this should be on all ranges)",
                "Verify that text range matches what was set in the control"
            })]
        public void TestGetVisibleRangesMethod20404(TestCaseAttribute testCase)
        {
            HeaderComment(testCase);

            GetVisibleRangesHelper(SampleText.Empty, false, SupportedTextSelection.Single, 0);
        }

        ///<summary>TextPattern.GetVisibleRange Method.T.2.04.05</summary>
        [TestCaseAttribute("TextPattern.GetVisibleRanges Method.T.2.04.05",
            TestSummary = "GetVisibleRanges on a single/multi-line control ",
            Priority = TestPriorities.Pri1,
            Status = TestStatus.Works,
            ProblemDescription = "Bug(s): 1456324--(Jpn) Edit*",
            Author = "Microsoft Corp.",
            TestCaseType = TestCaseType.Generic,
            Description = new string[]
            {
                "Pre-Condition: Control must supports multi-line = false",
                "Pre-Condition: Control text selection = multiple selections of text is supported",
                "Pre-Condition: Verify text is expected value: Set text to 'String1'",
                "Call GetVisibleRanges on the control with no errors, returning array size 1",
                "Call GetText() on FIRST visible range (post V1, this should be on all ranges)",
                "Verify that text range matches what was set in the control"
            })]
        public void TestGetVisibleRangesMethod20405(TestCaseAttribute testCase)
        {
            HeaderComment(testCase);

            GetVisibleRangesHelper(SampleText.String1, false, SupportedTextSelection.Multiple, 1);
        }

        ///<summary>TextPattern.GetVisibleRange Method.T.2.04.06</summary>
        [TestCaseAttribute("TextPattern.GetVisibleRanges Method.T.2.04.06",
            TestSummary = "GetVisibleRanges on a single/multi-line control ",
            Priority = TestPriorities.Pri1,
            Status = TestStatus.Works,
            ProblemDescription = "Bug(s): 1456324--(Jpn) Edit*",
            Author = "Microsoft Corp.",
            TestCaseType = TestCaseType.Generic,
            Description = new string[]
            {
                "Pre-Condition: Control must supports multi-line = false",
                "Pre-Condition: Control text selection = multiple selections of text is supported",
                "Pre-Condition: Verify text is expected value: Set text to be RANDOM 256 characters",
                "Call GetVisibleRanges on the control with no errors, returning array size 1",
                "Call GetText() on FIRST visible range (post V1, this should be on all ranges)",
                "Verify that text range matches what was set in the control"
            })]
        public void TestGetVisibleRangesMethod20406(TestCaseAttribute testCase)
        {
            HeaderComment(testCase);

            GetVisibleRangesHelper(SampleText.Random256, false, SupportedTextSelection.Multiple, 1);
        }

        ///<summary>TextPattern.GetVisibleRange Method.T.2.04.07</summary>
        [TestCaseAttribute("TextPattern.GetVisibleRanges Method.T.2.04.07",
            TestSummary = "GetVisibleRanges on a single/multi-line control ",
            Priority = TestPriorities.Pri1,
            Status = TestStatus.Works,
            ProblemDescription = "Bug(s): 1456324--(Jpn) Edit*",
            Author = "Microsoft Corp.",
            TestCaseType = TestCaseType.Generic,
            Description = new string[]
            {
                "Pre-Condition: Control must supports multi-line = true",
                "Pre-Condition: Control text selection = multiple selections of text is supported",
                "Pre-Condition: Verify text is expected value: Set text to span MULTIPLE LINES",
                "Call GetVisibleRanges on the control with no errors, returning array size 1",
                "Call GetText() on FIRST visible range (post V1, this should be on all ranges)",
                "Verify that text range matches what was set in the control"
            })]
        public void TestGetVisibleRangesMethod20407(TestCaseAttribute testCase)
        {
            HeaderComment(testCase);

            GetVisibleRangesHelper(SampleText.MultipleLines, true, SupportedTextSelection.Multiple, 1);
        }

        ///<summary>TextPattern.GetVisibleRange Method.T.2.04.08</summary>
        [TestCaseAttribute("TextPattern.GetVisibleRanges Method.T.2.04.08",
            TestSummary = "GetVisibleRanges on a single/multi-line control ",
            Priority = TestPriorities.Pri1,
            Status = TestStatus.Works,
            ProblemDescription = "Bug(s): 1456324--(Jpn) Edit*",
            Author = "Microsoft Corp.",
            TestCaseType = TestCaseType.Generic,
            Description = new string[]
            {
                "Pre-Condition: Control must supports multi-line = false",
                "Pre-Condition: Control text selection = multiple selections of text is supported",
                "Pre-Condition: Verify text is expected value: Set text to EMPTY string",
                "Call GetVisibleRanges on the control with no errors, returning array size 0",
                "Call GetText() on FIRST visible range (post V1, this should be on all ranges)",
                "Verify that text range matches what was set in the control"
            })]
        public void TestGetVisibleRangesMethod20408(TestCaseAttribute testCase)
        {
            HeaderComment(testCase);

            GetVisibleRangesHelper(SampleText.Empty, false, SupportedTextSelection.Multiple, 0);
        }

        ///<summary>TextPattern.GetVisibleRange Method.T.2.04.09</summary>
        [TestCaseAttribute("TextPattern.GetVisibleRanges Method.T.2.04.09",
            TestSummary = "GetVisibleRanges on a single/multi-line control ",
            Priority = TestPriorities.Pri1,
            Status = TestStatus.Works,
            ProblemDescription = "Bug(s): 1456324--(Jpn) Edit*",
            Author = "Microsoft Corp.",
            TestCaseType = TestCaseType.Generic,
            Description = new string[]
            {
                "Pre-Condition: Control must supports multi-line = false",
                "Pre-Condition: Control text selection = text selection is not supported",
                "Pre-Condition: Verify text is expected value: Set text to 'String1'",
                "Call GetVisibleRanges on the control with no errors, returning array size 0",
                "Call GetText() on FIRST visible range (post V1, this should be on all ranges)",
                "Verify that text range matches what was set in the control"
            })]
        public void TestGetVisibleRangesMethod20409(TestCaseAttribute testCase)
        {
            HeaderComment(testCase);

            GetVisibleRangesHelper(SampleText.String1, false, SupportedTextSelection.None, 0);
        }

        #endregion 2.4 TextPattern.GetVisibleRange Method

        #endregion Methods

        #region Events

        #region 3.1 TextSelectionChanged Event

        ///<summary>TextPattern.TextSelectionChanged Event.T.3.1.01</summary>
        [TestCaseAttribute("TextPattern.TextSelectionChanged Event.T.3.1.01",
            TestSummary = "Validate if TextSelectionChanged event fires",
            Priority = TestPriorities.Pri0,
            Status = TestStatus.Works,
            Author = "Microsoft Corp.",
            TestCaseType = TestCaseType.Events,
            EventTested = "TextPattern.TextSelectionChanged",
            Description = new string[]
            {
                "Pre-Condition: Verify SupportedTextSelection!=SupportedTextSelection.None",
                "Pre-Condition: Verify text is expected value: Set text to 'String1'",
                "Pre-Condition: Create range equal to: MIDDLE character in document",
                "Set listener for TextPattern.SelectionChangedEvent",
                "Select all text in the document",
                "Wait for event",
                "Verify that TestSelectionChanged event fires"
            })]
        public void TestSelectionChangedEvent3101(TestCaseAttribute testCase)
        {
            HeaderComment(testCase);

            TextSelectionChangedEventHelper(SampleText.String1, TargetRangeType.MiddleCharacter);
        }

        ///<summary>TextPattern.TextSelectionChanged Event.T.3.1.02</summary>
        [TestCaseAttribute("TextPattern.TextSelectionChanged Event.T.3.1.02",
            TestSummary = "Validate if TextSelectionChanged event fires",
            Priority = TestPriorities.Pri1,
            Status = TestStatus.Works,
            Author = "Microsoft Corp.",
            TestCaseType = TestCaseType.Events,
            EventTested = "TextPattern.TextSelectionChanged",
            Description = new string[]
            {
                "Pre-Condition: Verify SupportedTextSelection!=SupportedTextSelection.None",
                "Pre-Condition: Verify text is expected value: Set text to be RANDOM sample text",
                "Pre-Condition: Create range equal to: MIDDLE character in document",
                "Set listener for TextPattern.SelectionChangedEvent",
                "Select all text in the document",
                "Wait for event",
                "Verify that TestSelectionChanged event fires"
            })]
        public void TestSelectionChangedEvent3102(TestCaseAttribute testCase)
        {
            HeaderComment(testCase);

            TextSelectionChangedEventHelper(SampleText.RandomTextAnySize, TargetRangeType.MiddleCharacter);
        }

        ///<summary>TextPattern.TextSelectionChanged Event.T.3.1.03</summary>
        [TestCaseAttribute("TextPattern.TextSelectionChanged Event.T.3.1.03",
            TestSummary = "Use keyboard input to validate if TextSelectionChanged event fires",
            Priority = TestPriorities.Pri1,
            Status = TestStatus.Works,
            Author = "Microsoft Corp.",
            TestCaseType = TestCaseType.Events,
            EventTested = "TextPattern.TextSelectionChanged",
            Description = new string[]
            {
                "Pre-Condition: Verify SupportedTextSelection!=SupportedTextSelection.None",
                "Pre-Condition: Verify text is expected value: Set text to be RANDOM sample text",
                "Pre-Condition: SetFocus to control containing TextPattern",
                "Set listener for TextPattern.SelectionChangedEvent",
                "Select one character using keyboard input moving FORWARDS",
                "Wait for event",
                "Verify that TestSelectionChanged event fires"
            })]
        public void TestSelectionChangedEvent3103(TestCaseAttribute testCase)
        {
            HeaderComment(testCase);

            TextSelectionChangedEventHelper2(SampleText.RandomTextAnySize, false);
        }

        ///<summary>TextPattern.TextSelectionChanged Event.T.3.1.04</summary>
        [TestCaseAttribute("TextPattern.TextSelectionChanged Event.T.3.1.04",
            TestSummary = "Use keyboard input to validate if TextSelectionChanged event fires",
            Priority = TestPriorities.Pri1,
            Status = TestStatus.Works,
            Author = "Microsoft Corp.",
            TestCaseType = TestCaseType.Events,
            EventTested = "TextPattern.TextSelectionChanged",
            Description = new string[]
            {
                "Pre-Condition: Verify SupportedTextSelection!=SupportedTextSelection.None",
                "Pre-Condition: Verify text is expected value: Set text to be RANDOM sample text",
                "Pre-Condition: SetFocus to control containing TextPattern",
                "Set listener for TextPattern.SelectionChangedEvent",
                "Select one character using keyboard input moving BACKWARDS",
                "Wait for event",
                "Verify that TestSelectionChanged event fires"
            })]
        public void TestSelectionChangedEvent3104(TestCaseAttribute testCase)
        {
            HeaderComment(testCase);

            TextSelectionChangedEventHelper2(SampleText.RandomTextAnySize, true);
        }

        #endregion 3.1 TextSelectionChanged Event

        #endregion Events

        #endregion TextPattern

        #region TextPatternRange

        #region Properties

        #region 4.1 TextPatternRange.TextPattern Property

        ///<summary>TextPatternRange.TextPattern Property.T.4.01.01</summary>
        [TestCaseAttribute("TextPatternRange.TextPattern Property.T.4.01.01",
            TestSummary = "Verfies TextPattern.DocumentRange.TextPattern = TextPattern with any text",
            Priority = TestPriorities.Pri0,
            Status = TestStatus.Works,
            Author = "Microsoft Corp.",
            TestCaseType = TestCaseType.Generic,
            Description = new string[]
            {
                "Pre-Condition: Verify text is expected value: Set text to 'String1'",
                "Get TextPattern property",
                "ReferenceEquals comparison of pattern and TextPattern property"
            })]
        public void TestTextPatternProperty40101(TestCaseAttribute testCase)
        {
            HeaderComment(testCase);

            TextPatternHelper(SampleText.String1);
        }

        ///<summary>TextPatternRange.TextPattern Property.T.4.01.02</summary>
        [TestCaseAttribute("TextPatternRange.TextPattern Property.T.4.01.02",
            TestSummary = "Verfies TextPattern.DocumentRange.TextPattern = TextPattern with any text",
            Priority = TestPriorities.Pri1,
            Status = TestStatus.Works,
            Author = "Microsoft Corp.",
            TestCaseType = TestCaseType.Generic,
            Description = new string[]
            {
                "Pre-Condition: Verify text is expected value: Verify text is NOT empty (or SET text if it is empty)",
                "Get TextPattern property",
                "ReferenceEquals comparison of pattern and TextPattern property"
            })]
        public void TestTextPatternProperty40102(TestCaseAttribute testCase)
        {
            HeaderComment(testCase);

            TextPatternHelper(SampleText.NotEmpty);
        }

        ///<summary>TextPatternRange.TextPattern Property.T.4.01.03</summary>
        [TestCaseAttribute("TextPatternRange.TextPattern Property.T.4.01.03",
            TestSummary = "Verfies TextPattern.DocumentRange.TextPattern = TextPattern with any text",
            Priority = TestPriorities.Pri1,
            Status = TestStatus.Works,
            Author = "Microsoft Corp.",
            TestCaseType = TestCaseType.Generic,
            Description = new string[]
            {
                "Pre-Condition: Verify text is expected value: Set text to EMPTY string",
                "Get TextPattern property",
                "ReferenceEquals comparison of pattern and TextPattern property"
            })]
        public void TestTextPatternProperty40103(TestCaseAttribute testCase)
        {
            HeaderComment(testCase);

            TextPatternHelper(SampleText.Empty);
        }

        #endregion 4.1 TextPatternRange.TextPattern Property

        #region 4.2 TextPatternRange.Children Property

        ///<summary>TextPatternRange.Children Property.T.4.02.01</summary>
        [TestCaseAttribute("TextPatternRange.Children Property.T.4.02.01",
            TestSummary = "Determines if there are any children for the automation element",
            Priority = TestPriorities.Pri0,
            Status = TestStatus.Works,
            Author = "Microsoft Corp.",
            TestCaseType = TestCaseType.Generic,
            Description = new string[]
            {
                "Pre-Condition: Verify text is expected value: Set text to 'String1'",
                "Pre-Condition: (Children Property is non-null) == false ",
                "Enumerate children (if they exist)"
            })]
        public void TestChildrenProperty40201(TestCaseAttribute testCase)
        {
            HeaderComment(testCase);

            ChildrenHelper(SampleText.String1, false);
        }

        ///<summary>TextPatternRange.Children Property.T.4.02.02</summary>
        [TestCaseAttribute("TextPatternRange.Children Property.T.4.02.02",
            TestSummary = "Determines if there are any children for the automation element",
            Priority = TestPriorities.Pri1,
            Status = TestStatus.Works,
            Author = "Microsoft Corp.",
            TestCaseType = TestCaseType.Generic,
            Description = new string[]
            {
                "Pre-Condition: Verify text is expected value: Verify text is NOT empty (or SET text if it is empty)",
                "Pre-Condition: (Children Property is non-null) == false ",
                "Enumerate children (if they exist)"
            })]
        public void TestChildrenProperty40202(TestCaseAttribute testCase)
        {
            HeaderComment(testCase);

            ChildrenHelper(SampleText.NotEmpty, false);
        }

        ///<summary>TextPatternRange.Children Property.T.4.02.03</summary>
        [TestCaseAttribute("TextPatternRange.Children Property.T.4.02.03",
            TestSummary = "Determines if there are any children for the automation element",
            Priority = TestPriorities.Pri1,
            Status = TestStatus.Works,
            Author = "Microsoft Corp.",
            TestCaseType = TestCaseType.Generic,
            Description = new string[]
            {
                "Pre-Condition: Verify text is expected value: Set text to EMPTY string",
                "Pre-Condition: (Children Property is non-null) == false ",
                "Enumerate children (if they exist)"
            })]
        public void TestChildrenProperty40203(TestCaseAttribute testCase)
        {
            HeaderComment(testCase);

            ChildrenHelper(SampleText.Empty, false);
        }

        ///<summary>TextPatternRange.Children Property.T.4.02.04</summary>
        [TestCaseAttribute("TextPatternRange.Children Property.T.4.02.04",
            TestSummary = "Determines if there are any children for the automation element",
            Priority = TestPriorities.Pri2,
            Status = TestStatus.Works,
            Author = "Microsoft Corp.",
            TestCaseType = TestCaseType.Generic,
            Description = new string[]
            {
                "Pre-Condition: Verify text is expected value: Verify text is NOT empty (or SET text if it is empty)",
                "Pre-Condition: (Children Property is non-null) == true ",
                "Enumerate children (if they exist)"
            })]
        public void TestChildrenProperty40204(TestCaseAttribute testCase)
        {
            HeaderComment(testCase);

            ChildrenHelper(SampleText.NotEmpty, true);
        }

        #endregion 4.2 TextPatternRange.Children Property

        #endregion Properties

        #region Methods

        #region 5.1 TextPatternRange.Clone Method

        ///<summary>TextPatternRange.Clone Method.T.5.01.01</summary>
        [TestCaseAttribute("TextPatternRange.Clone Method.T.5.01.01",
            TestSummary = "Clones an existing TextPatternRange with text",
            Priority = TestPriorities.Pri0,
            Status = TestStatus.Works,
            Author = "Microsoft Corp.",
            TestCaseType = TestCaseType.Generic,
            Description = new string[]
            {
                "Pre-Condition: Verify text is expected value: Set text to 'String1'",
                "Pre-Condition: Create range = entire document",
                "Pre-Condition: Children Property is empty array",
                "Clone the range",
                "Verify identical ranges by calling Compare()",
                "If children, Verify each child element is identical"
            })]
        public void TestCloneMethod50101(TestCaseAttribute testCase)
        {
            HeaderComment(testCase);

            CloneHelper(SampleText.String1, TargetRangeType.DocumentRange, false);
        }

        ///<summary>TextPatternRange.Clone Method.T.5.01.02</summary>
        [TestCaseAttribute("TextPatternRange.Clone Method.T.5.01.02",
            TestSummary = "Clones an existing TextPatternRange with text",
            Priority = TestPriorities.Pri1,
            Status = TestStatus.Works,
            Author = "Microsoft Corp.",
            TestCaseType = TestCaseType.Generic,
            Description = new string[]
            {
                "Pre-Condition: Verify text is expected value: Verify text is NOT empty (or SET text if it is empty)",
                "Pre-Condition: Create range = entire document",
                "Pre-Condition: Children Property is empty array",
                "Clone the range",
                "Verify identical ranges by calling Compare()",
                "If children, Verify each child element is identical"
            })]
        public void TestCloneMethod50102(TestCaseAttribute testCase)
        {
            HeaderComment(testCase);

            CloneHelper(SampleText.NotEmpty, TargetRangeType.DocumentRange, false);
        }

        ///<summary>TextPatternRange.Clone Method.T.5.01.03</summary>
        [TestCaseAttribute("TextPatternRange.Clone Method.T.5.01.03",
            TestSummary = "Clones an existing TextPatternRange with text",
            Priority = TestPriorities.Pri1,
            Status = TestStatus.Works,
            Author = "Microsoft Corp.",
            TestCaseType = TestCaseType.Generic,
            Description = new string[]
            {
                "Pre-Condition: Verify text is expected value: Set text to EMPTY string",
                "Pre-Condition: Create range = entire document",
                "Pre-Condition: Children Property is empty array",
                "Clone the range",
                "Verify identical ranges by calling Compare()",
                "If children, Verify each child element is identical"
            })]
        public void TestCloneMethod50103(TestCaseAttribute testCase)
        {
            HeaderComment(testCase);

            CloneHelper(SampleText.Empty, TargetRangeType.DocumentRange, false);
        }

        ///<summary>TextPatternRange.Clone Method.T.5.01.04</summary>
        [TestCaseAttribute("TextPatternRange.Clone Method.T.5.01.04",
            TestSummary = "Clones an existing TextPatternRange with text",
            Priority = TestPriorities.Pri1,
            Status = TestStatus.Works,
            Author = "Microsoft Corp.",
            TestCaseType = TestCaseType.Generic,
            Description = new string[]
            {
                "Pre-Condition: Verify text is expected value: Verify text is NOT empty (or SET text if it is empty)",
                "Pre-Condition: Create range = EMPTY range at START of document",
                "Pre-Condition: Children Property is empty array",
                "Clone the range",
                "Verify identical ranges by calling Compare()",
                "If children, Verify each child element is identical"
            })]
        public void TestCloneMethod50104(TestCaseAttribute testCase)
        {
            HeaderComment(testCase);

            CloneHelper(SampleText.NotEmpty, TargetRangeType.EmptyStart, false);
        }

        ///<summary>TextPatternRange.Clone Method.T.5.01.05</summary>
        [TestCaseAttribute("TextPatternRange.Clone Method.T.5.01.05",
            TestSummary = "Clones an existing TextPatternRange with text",
            Priority = TestPriorities.Pri1,
            Status = TestStatus.Works,
            Author = "Microsoft Corp.",
            TestCaseType = TestCaseType.Generic,
            Description = new string[]
            {
                "Pre-Condition: Verify text is expected value: Verify text is NOT empty (or SET text if it is empty)",
                "Pre-Condition: Create range = EMPTY range in MIDDLE of document",
                "Pre-Condition: Children Property is empty array",
                "Clone the range",
                "Verify identical ranges by calling Compare()",
                "If children, Verify each child element is identical"
            })]
        public void TestCloneMethod50105(TestCaseAttribute testCase)
        {
            HeaderComment(testCase);

            CloneHelper(SampleText.NotEmpty, TargetRangeType.EmptyMiddle, false);
        }

        ///<summary>TextPatternRange.Clone Method.T.5.01.06</summary>
        [TestCaseAttribute("TextPatternRange.Clone Method.T.5.01.06",
            TestSummary = "Clones an existing TextPatternRange with text",
            Priority = TestPriorities.Pri1,
            Status = TestStatus.Works,
            Author = "Microsoft Corp.",
            TestCaseType = TestCaseType.Generic,
            Description = new string[]
            {
                "Pre-Condition: Verify text is expected value: Verify text is NOT empty (or SET text if it is empty)",
                "Pre-Condition: Create range = RANDOM range at END of document",
                "Pre-Condition: Children Property is empty array",
                "Clone the range",
                "Verify identical ranges by calling Compare()",
                "If children, Verify each child element is identical"
            })]
        public void TestCloneMethod50106(TestCaseAttribute testCase)
        {
            HeaderComment(testCase);

            CloneHelper(SampleText.NotEmpty, TargetRangeType.RandomEnd, false);
        }

        ///<summary>TextPatternRange.Clone Method.T.5.01.07</summary>
        [TestCaseAttribute("TextPatternRange.Clone Method.T.5.01.07",
            TestSummary = "Clones an existing TextPatternRange with text",
            Priority = TestPriorities.Pri1,
            Status = TestStatus.Works,
            Author = "Microsoft Corp.",
            TestCaseType = TestCaseType.Generic,
            Description = new string[]
            {
                "Pre-Condition: Verify text is expected value: Verify text is NOT empty (or SET text if it is empty)",
                "Pre-Condition: Create range = RANDOM range in MIDDLE of document",
                "Pre-Condition: Children Property is empty array",
                "Clone the range",
                "Verify identical ranges by calling Compare()",
                "If children, Verify each child element is identical"
            })]
        public void TestCloneMethod50107(TestCaseAttribute testCase)
        {
            HeaderComment(testCase);

            CloneHelper(SampleText.NotEmpty, TargetRangeType.RandomMiddle, false);
        }

        ///<summary>TextPatternRange.Clone Method.T.5.01.08</summary>
        [TestCaseAttribute("TextPatternRange.Clone Method.T.5.01.08",
            TestSummary = "Clones an existing TextPatternRange with text",
            Priority = TestPriorities.Pri2,
            Status = TestStatus.Works,
            Author = "Microsoft Corp.",
            TestCaseType = TestCaseType.Generic,
            Description = new string[]
            {
                "Pre-Condition: Verify text is expected value: Verify text is NOT empty (or SET text if it is empty)",
                "Pre-Condition: Create range = entire document",
                "Pre-Condition: Children Property is non-empty array",
                "Clone the range",
                "Verify identical ranges by calling Compare()",
                "If children, Verify each child element is identical"
            })]
        public void TestCloneMethod50108(TestCaseAttribute testCase)
        {
            HeaderComment(testCase);

            CloneHelper(SampleText.NotEmpty, TargetRangeType.DocumentRange, true);
        }

        ///<summary>TextPatternRange.Clone Method.T.5.01.09</summary>
        [TestCaseAttribute("TextPatternRange.Clone Method.T.5.01.09",
            TestSummary = "Clones an existing TextPatternRange with text",
            Priority = TestPriorities.Pri2,
            Status = TestStatus.Works,
            Author = "Microsoft Corp.",
            TestCaseType = TestCaseType.Generic,
            Description = new string[]
            {
                "Pre-Condition: Verify text is expected value: Verify text is NOT empty (or SET text if it is empty)",
                "Pre-Condition: Create range = EMPTY range at START of document",
                "Pre-Condition: Children Property is non-empty array",
                "Clone the range",
                "Verify identical ranges by calling Compare()",
                "If children, Verify each child element is identical"
            })]
        public void TestCloneMethod50109(TestCaseAttribute testCase)
        {
            HeaderComment(testCase);

            CloneHelper(SampleText.NotEmpty, TargetRangeType.EmptyStart, true);
        }

        ///<summary>TextPatternRange.Clone Method.T.5.01.10</summary>
        [TestCaseAttribute("TextPatternRange.Clone Method.T.5.01.10",
            TestSummary = "Clones an existing TextPatternRange with text",
            Priority = TestPriorities.Pri2,
            Status = TestStatus.Works,
            Author = "Microsoft Corp.",
            TestCaseType = TestCaseType.Generic,
            Description = new string[]
            {
                "Pre-Condition: Verify text is expected value: Verify text is NOT empty (or SET text if it is empty)",
                "Pre-Condition: Create range = EMPTY range in MIDDLE of document",
                "Pre-Condition: Children Property is non-empty array",
                "Clone the range",
                "Verify identical ranges by calling Compare()",
                "If children, Verify each child element is identical"
            })]
        public void TestCloneMethod50110(TestCaseAttribute testCase)
        {
            HeaderComment(testCase);

            CloneHelper(SampleText.NotEmpty, TargetRangeType.EmptyMiddle, true);
        }

        ///<summary>TextPatternRange.Clone Method.T.5.01.11</summary>
        [TestCaseAttribute("TextPatternRange.Clone Method.T.5.01.11",
            TestSummary = "Clones an existing TextPatternRange with text",
            Priority = TestPriorities.Pri2,
            Status = TestStatus.Works,
            Author = "Microsoft Corp.",
            TestCaseType = TestCaseType.Generic,
            Description = new string[]
            {
                "Pre-Condition: Verify text is expected value: Verify text is NOT empty (or SET text if it is empty)",
                "Pre-Condition: Create range = RANDOM range at END of document",
                "Pre-Condition: Children Property is non-empty array",
                "Clone the range",
                "Verify identical ranges by calling Compare()",
                "If children, Verify each child element is identical"
            })]
        public void TestCloneMethod50111(TestCaseAttribute testCase)
        {
            HeaderComment(testCase);

            CloneHelper(SampleText.NotEmpty, TargetRangeType.RandomEnd, true);
        }

        ///<summary>TextPatternRange.Clone Method.T.5.01.12</summary>
        [TestCaseAttribute("TextPatternRange.Clone Method.T.5.01.12",
            TestSummary = "Clones an existing TextPatternRange with text",
            Priority = TestPriorities.Pri2,
            Status = TestStatus.Works,
            Author = "Microsoft Corp.",
            TestCaseType = TestCaseType.Generic,
            Description = new string[]
            {
                "Pre-Condition: Verify text is expected value: Verify text is NOT empty (or SET text if it is empty)",
                "Pre-Condition: Create range = RANDOM range in MIDDLE of document",
                "Pre-Condition: Children Property is non-empty array",
                "Clone the range",
                "Verify identical ranges by calling Compare()",
                "If children, Verify each child element is identical"
            })]
        public void TestCloneMethod50112(TestCaseAttribute testCase)
        {
            HeaderComment(testCase);

            CloneHelper(SampleText.NotEmpty, TargetRangeType.RandomMiddle, true);
        }

        #endregion 5.1 TextPatternRange.Clone Method

        #region 5.2 TextPatternRange.Compare Method

        ///<summary>TextPatternRange.Compare Method.T.5.02.01</summary>
        [TestCaseAttribute("TextPatternRange.Compare Method.T.5.02.01",
            TestSummary = "Performs a comparison on the entire TextPattern",
            Priority = TestPriorities.Pri0,
            Status = TestStatus.Works,
            Author = "Microsoft Corp.",
            TestCaseType = TestCaseType.Generic,
            Description = new string[]
            {
                "Pre-Condition: Verify text is expected value: Set text to 'String1'",
                "Pre-Condition: Create calling range as RANDOM range at START of document",
                "Pre-Condition: Argument range is clone of calling range",
                "Compare the ranges without errors",
                "Calling Range: Get Attribute dictionary",
                "Calling Range: Get Enclosing element",
                "Calling Range: Get children",
                "Verify matches Start/End Points",
                "Verify matches text",
                "Verify matches Attribute dictionary",
                "Verify matches Get Enclosing element",
                "Verify matches children"
            })]
        [SuppressMessage("Microsoft.Globalization", "CA1303:DoNotPassLiteralsAsLocalizedParameters")]
        public void TestCompareMethod50201(TestCaseAttribute testCase)
        {
            HeaderComment(testCase);


            CompareHelper(SampleText.String1, TargetRangeType.RandomStart, TargetRangeType.Clone, true, null);
        }

        ///<summary>TextPatternRange.Compare Method.T.5.02.02</summary>
        [TestCaseAttribute("TextPatternRange.Compare Method.T.5.02.02",
            TestSummary = "Performs a comparison on the entire TextPattern",
            Priority = TestPriorities.Pri1,
            Status = TestStatus.Works,
            Author = "Microsoft Corp.",
            TestCaseType = TestCaseType.Generic,
            Description = new string[]
            {
                "Pre-Condition: Verify text is expected value: Set text to be RANDOM 256 characters",
                "Pre-Condition: Create calling range as RANDOM range at START of document",
                "Pre-Condition: Argument range is clone of calling range",
                "Compare the ranges without errors",
                "Calling Range: Get Attribute dictionary",
                "Calling Range: Get Enclosing element",
                "Calling Range: Get children",
                "Verify matches Start/End Points",
                "Verify matches text",
                "Verify matches Attribute dictionary",
                "Verify matches Get Enclosing element",
                "Verify matches children"
            })]
        [SuppressMessage("Microsoft.Globalization", "CA1303:DoNotPassLiteralsAsLocalizedParameters")]
        public void TestCompareMethod50202(TestCaseAttribute testCase)
        {
            HeaderComment(testCase);


            CompareHelper(SampleText.Random256, TargetRangeType.RandomStart, TargetRangeType.Clone, true, null);
        }

        ///<summary>TextPatternRange.Compare Method.T.5.02.03</summary>
        [TestCaseAttribute("TextPatternRange.Compare Method.T.5.02.03",
            TestSummary = "Performs a comparison on the entire TextPattern",
            Priority = TestPriorities.Pri1,
            Status = TestStatus.Works,
            Author = "Microsoft Corp.",
            TestCaseType = TestCaseType.Generic,
            Description = new string[]
            {
                "Pre-Condition: Verify text is expected value: Set text to be RANDOM 256 characters",
                "Pre-Condition: Create calling range as RANDOM range at START of document",
                "Pre-Condition: Argument range is EMPTY range at START of document",
                "Compare the ranges without errors",
                "Calling Range: Get Attribute dictionary",
                "Calling Range: Get Enclosing element",
                "Calling Range: Get children",
                "Verify non-matching Start/End Points",
                "Verify non-matching text",
                "Verify non-matching Attribute dictionary",
                "Verify non-matching Get Enclosing element",
                "Verify non-matching children"
            })]
        [SuppressMessage("Microsoft.Globalization", "CA1303:DoNotPassLiteralsAsLocalizedParameters")]
        public void TestCompareMethod50203(TestCaseAttribute testCase)
        {
            HeaderComment(testCase);


            CompareHelper(SampleText.Random256, TargetRangeType.RandomStart, TargetRangeType.EmptyStart, false, null);
        }

        ///<summary>TextPatternRange.Compare Method.T.5.02.04</summary>
        [TestCaseAttribute("TextPatternRange.Compare Method.T.5.02.04",
            TestSummary = "Performs a comparison on the entire TextPattern",
            Priority = TestPriorities.Pri1,
            Status = TestStatus.Works,
            Author = "Microsoft Corp.",
            TestCaseType = TestCaseType.Generic,
            Description = new string[]
            {
                "Pre-Condition: Verify text is expected value: Set text to be RANDOM 256 characters",
                "Pre-Condition: Create calling range as RANDOM range at START of document",
                "Pre-Condition: Argument range is null value",
                "Compare the ranges RAISES System.ArgumentNullException exception",
                "Calling Range: Get Attribute dictionary",
                "Calling Range: Get Enclosing element",
                "Calling Range: Get children",
                "Verify non-matching Start/End Points",
                "Verify non-matching text",
                "Verify non-matching Attribute dictionary",
                "Verify non-matching Get Enclosing element",
                "Verify non-matching children"
            })]
        [SuppressMessage("Microsoft.Globalization", "CA1303:DoNotPassLiteralsAsLocalizedParameters")]
        public void TestCompareMethod50204(TestCaseAttribute testCase)
        {
            HeaderComment(testCase);

            ArgumentNullException ex = new ArgumentNullException("testCase"); //  Unused, just need an instance for GetType() below

            CompareHelper(SampleText.Random256, TargetRangeType.RandomStart, TargetRangeType.Null, false, ex.GetType());
        }

        ///<summary>TextPatternRange.Compare Method.T.5.02.06</summary>
        [TestCaseAttribute("TextPatternRange.Compare Method.T.5.02.06",
            TestSummary = "Performs a comparison on the entire TextPattern",
            Priority = TestPriorities.Pri1,
            Status = TestStatus.Works,
            Author = "Microsoft Corp.",
            TestCaseType = TestCaseType.Generic,
            Description = new string[]
            {
                "Pre-Condition: Verify text is expected value: Set text to span MULTIPLE LINES",
                "Pre-Condition: Create calling range as EMPTY range at START of document",
                "Pre-Condition: Argument range is clone of calling range",
                "Compare the ranges without errors",
                "Calling Range: Get Attribute dictionary",
                "Calling Range: Get Enclosing element",
                "Calling Range: Get children",
                "Verify matches Start/End Points",
                "Verify matches text",
                "Verify matches Attribute dictionary",
                "Verify matches Get Enclosing element",
                "Verify matches children"
            })]
        [SuppressMessage("Microsoft.Globalization", "CA1303:DoNotPassLiteralsAsLocalizedParameters")]
        public void TestCompareMethod50206(TestCaseAttribute testCase)
        {
            HeaderComment(testCase);


            CompareHelper(SampleText.MultipleLines, TargetRangeType.EmptyStart, TargetRangeType.Clone, true, null);
        }

        ///<summary>TextPatternRange.Compare Method.T.5.02.07</summary>
        [TestCaseAttribute("TextPatternRange.Compare Method.T.5.02.07",
            TestSummary = "Performs a comparison on the entire TextPattern",
            Priority = TestPriorities.Pri1,
            Status = TestStatus.Works,
            Author = "Microsoft Corp.",
            TestCaseType = TestCaseType.Generic,
            Description = new string[]
            {
                "Pre-Condition: Verify text is expected value: Set text to span MULTIPLE LINES",
                "Pre-Condition: Create calling range as EMPTY range at START of document",
                "Pre-Condition: Argument range is EMPTY range in MIDDLE of document",
                "Compare the ranges without errors",
                "Calling Range: Get Attribute dictionary",
                "Calling Range: Get Enclosing element",
                "Calling Range: Get children",
                "Verify non-matching Start/End Points",
                "Verify non-matching text",
                "Verify non-matching Attribute dictionary",
                "Verify non-matching Get Enclosing element",
                "Verify non-matching children"
            })]
        [SuppressMessage("Microsoft.Globalization", "CA1303:DoNotPassLiteralsAsLocalizedParameters")]
        public void TestCompareMethod50207(TestCaseAttribute testCase)
        {
            HeaderComment(testCase);


            CompareHelper(SampleText.MultipleLines, TargetRangeType.EmptyStart, TargetRangeType.EmptyMiddle, false, null);
        }

        ///<summary>TextPatternRange.Compare Method.T.5.02.08</summary>
        [TestCaseAttribute("TextPatternRange.Compare Method.T.5.02.08",
            TestSummary = "Performs a comparison on the entire TextPattern",
            Priority = TestPriorities.Pri2,
            Status = TestStatus.Works,
            Author = "Microsoft Corp.",
            TestCaseType = TestCaseType.Generic,
            Description = new string[]
            {
                "Pre-Condition: Verify text is expected value: Set text to EMPTY string",
                "Pre-Condition: Create calling range as EMPTY range at START of document",
                "Pre-Condition: Argument range is clone of calling range",
                "Compare the ranges without errors",
                "Calling Range: Get Attribute dictionary",
                "Calling Range: Get Enclosing element",
                "Calling Range: Get children",
                "Verify matches Start/End Points",
                "Verify matches text",
                "Verify matches Attribute dictionary",
                "Verify matches Get Enclosing element",
                "Verify matches children"
            })]
        [SuppressMessage("Microsoft.Globalization", "CA1303:DoNotPassLiteralsAsLocalizedParameters")]
        public void TestCompareMethod50208(TestCaseAttribute testCase)
        {
            HeaderComment(testCase);


            CompareHelper(SampleText.Empty, TargetRangeType.EmptyStart, TargetRangeType.Clone, true, null);
        }

        ///<summary>TextPatternRange.Compare Method.T.5.02.09</summary>
        [TestCaseAttribute("TextPatternRange.Compare Method.T.5.02.09",
            TestSummary = "Performs a comparison on the entire TextPattern",
            Priority = TestPriorities.Pri2,
            Status = TestStatus.Works,
            Author = "Microsoft Corp.",
            TestCaseType = TestCaseType.Generic,
            Description = new string[]
            {
                "Pre-Condition: Verify text is expected value: Set text to EMPTY string",
                "Pre-Condition: Create calling range as EMPTY range at START of document",
                "Pre-Condition: Argument range is EMPTY range at START of document",
                "Compare the ranges without errors",
                "Calling Range: Get Attribute dictionary",
                "Calling Range: Get Enclosing element",
                "Calling Range: Get children",
                "Verify matches Start/End Points",
                "Verify matches text",
                "Verify matches Attribute dictionary",
                "Verify matches Get Enclosing element",
                "Verify matches children"
            })]
        [SuppressMessage("Microsoft.Globalization", "CA1303:DoNotPassLiteralsAsLocalizedParameters")]
        public void TestCompareMethod50209(TestCaseAttribute testCase)
        {
            HeaderComment(testCase);


            CompareHelper(SampleText.Empty, TargetRangeType.EmptyStart, TargetRangeType.EmptyStart, true, null);
        }

        ///<summary>TextPatternRange.Compare Method.T.5.02.10</summary>
        [TestCaseAttribute("TextPatternRange.Compare Method.T.5.02.10",
            TestSummary = "Performs a comparison on the entire TextPattern",
            Priority = TestPriorities.Pri2,
            Status = TestStatus.Works,
            Author = "Microsoft Corp.",
            TestCaseType = TestCaseType.Generic,
            Description = new string[]
            {
                "Pre-Condition: Verify text is expected value: Set text to span MULTIPLE LINES",
                "Pre-Condition: Create calling range as RANDOM range at START of document",
                "Pre-Condition: Argument range is clone of calling range",
                "Compare the ranges without errors",
                "Calling Range: Get Attribute dictionary",
                "Calling Range: Get Enclosing element",
                "Calling Range: Get children",
                "Verify matches Start/End Points",
                "Verify matches text",
                "Verify matches Attribute dictionary",
                "Verify matches Get Enclosing element",
                "Verify matches children"
            })]
        [SuppressMessage("Microsoft.Globalization", "CA1303:DoNotPassLiteralsAsLocalizedParameters")]
        public void TestCompareMethod50210(TestCaseAttribute testCase)
        {
            HeaderComment(testCase);


            CompareHelper(SampleText.MultipleLines, TargetRangeType.RandomStart, TargetRangeType.Clone, true, null);
        }

        #endregion 5.2 TextPatternRange.Compare Method

        #region 5.3 TextPatternRange.CompareEndpoints Method

        ///<summary>TextPatternRange.CompareEndpoints Method.T.5.03.01</summary>
        [TestCaseAttribute("TextPatternRange.CompareEndpoints Method.T.5.03.01",
            TestSummary = "Comparing the endpoints of the TextPattern entire range against themselves",
            Priority = TestPriorities.Pri0,
            Status = TestStatus.Works,
            Author = "Microsoft Corp.",
            TestCaseType = TestCaseType.Generic,
            Description = new string[]
            {
                "Pre-Condition: Verify text is expected value: Set text to 'String1'",
                "Pre-Condition: Create calling range equal to entire document",
                "Pre-Condition: Create argument range equal to entire document",
                "Verify CompareEndPoints(start,start) Equals zero, without errors",
                "Verify CompareEndPoints(start,  end) Less than zero, without errors",
                "Verify CompareEndPoints(  end,start) Greater Than zero, without errors",
                "Verify CompareEndPoints(  end,  end) Equals zero, without errors"
            })]
        [SuppressMessage("Microsoft.Globalization", "CA1303:DoNotPassLiteralsAsLocalizedParameters")]
        public void TestCompareEndpointsMethod50301(TestCaseAttribute testCase)
        {
            HeaderComment(testCase);


            CompareEndpointsHelper(SampleText.String1, TargetRangeType.DocumentRange, TargetRangeType.DocumentRange,
                   ValueComparison.Equals, ValueComparison.LessThan, ValueComparison.GreaterThan, ValueComparison.Equals, null);
        }

        ///<summary>TextPatternRange.CompareEndpoints Method.T.5.03.02</summary>
        [TestCaseAttribute("TextPatternRange.CompareEndpoints Method.T.5.03.02",
            TestSummary = "Comparing the endpoints of the TextPattern entire range against themselves",
            Priority = TestPriorities.Pri1,
            Status = TestStatus.Works,
            Author = "Microsoft Corp.",
            TestCaseType = TestCaseType.Generic,
            Description = new string[]
            {
                "Pre-Condition: Verify text is expected value: Set text to be RANDOM 256 characters",
                "Pre-Condition: Create calling range equal to entire document",
                "Pre-Condition: Create argument range equal to entire document",
                "Verify CompareEndPoints(start,start) Equals zero, without errors",
                "Verify CompareEndPoints(start,  end) Less than zero, without errors",
                "Verify CompareEndPoints(  end,start) Greater Than zero, without errors",
                "Verify CompareEndPoints(  end,  end) Equals zero, without errors"
            })]
        [SuppressMessage("Microsoft.Globalization", "CA1303:DoNotPassLiteralsAsLocalizedParameters")]
        public void TestCompareEndpointsMethod50302(TestCaseAttribute testCase)
        {
            HeaderComment(testCase);


            CompareEndpointsHelper(SampleText.Random256, TargetRangeType.DocumentRange, TargetRangeType.DocumentRange,
                   ValueComparison.Equals, ValueComparison.LessThan, ValueComparison.GreaterThan, ValueComparison.Equals, null);
        }

        ///<summary>TextPatternRange.CompareEndpoints Method.T.5.03.03</summary>
        [TestCaseAttribute("TextPatternRange.CompareEndpoints Method.T.5.03.03",
            TestSummary = "Comparing the endpoints of the TextPattern entire range against themselves",
            Priority = TestPriorities.Pri1,
            Status = TestStatus.Works,
            Author = "Microsoft Corp.",
            TestCaseType = TestCaseType.Generic,
            Description = new string[]
            {
                "Pre-Condition: Verify text is expected value: Set text to be RANDOM 256 characters",
                "Pre-Condition: Create calling range equal to entire document",
                "Pre-Condition: Create argument range equal to EMPTY range at START of document",
                "Verify CompareEndPoints(start,start) Equals zero, without errors",
                "Verify CompareEndPoints(start,  end) Equals zero, without errors",
                "Verify CompareEndPoints(  end,start) Greater Than zero, without errors",
                "Verify CompareEndPoints(  end,  end) Greater Than zero, without errors"
            })]
        [SuppressMessage("Microsoft.Globalization", "CA1303:DoNotPassLiteralsAsLocalizedParameters")]
        public void TestCompareEndpointsMethod50303(TestCaseAttribute testCase)
        {
            HeaderComment(testCase);


            CompareEndpointsHelper(SampleText.Random256, TargetRangeType.DocumentRange, TargetRangeType.EmptyStart,
                   ValueComparison.Equals, ValueComparison.Equals, ValueComparison.GreaterThan, ValueComparison.GreaterThan, null);
        }

        ///<summary>TextPatternRange.CompareEndpoints Method.T.5.03.04</summary>
        [TestCaseAttribute("TextPatternRange.CompareEndpoints Method.T.5.03.04",
            TestSummary = "Comparing the endpoints of the TextPattern entire range against themselves",
            Priority = TestPriorities.Pri1,
            Status = TestStatus.Works,
            Author = "Microsoft Corp.",
            TestCaseType = TestCaseType.Generic,
            Description = new string[]
            {
                "Pre-Condition: Verify text is expected value: Set text to be RANDOM 256 characters",
                "Pre-Condition: Create calling range equal to entire document",
                "Pre-Condition: Create argument range equal to EMPTY range in MIDDLE of document",
                "Verify CompareEndPoints(start,start) Less than zero, without errors",
                "Verify CompareEndPoints(start,  end) Less than zero, without errors",
                "Verify CompareEndPoints(  end,start) Greater Than zero, without errors",
                "Verify CompareEndPoints(  end,  end) Greater Than zero, without errors"
            })]
        [SuppressMessage("Microsoft.Globalization", "CA1303:DoNotPassLiteralsAsLocalizedParameters")]
        public void TestCompareEndpointsMethod50304(TestCaseAttribute testCase)
        {
            HeaderComment(testCase);


            CompareEndpointsHelper(SampleText.Random256, TargetRangeType.DocumentRange, TargetRangeType.EmptyMiddle,
                   ValueComparison.LessThan, ValueComparison.LessThan, ValueComparison.GreaterThan, ValueComparison.GreaterThan, null);
        }

        ///<summary>TextPatternRange.CompareEndpoints Method.T.5.03.05</summary>
        [TestCaseAttribute("TextPatternRange.CompareEndpoints Method.T.5.03.05",
            TestSummary = "Comparing the endpoints of the TextPattern entire range against themselves",
            Priority = TestPriorities.Pri1,
            Status = TestStatus.Works,
            Author = "Microsoft Corp.",
            TestCaseType = TestCaseType.Generic,
            Description = new string[]
            {
                "Pre-Condition: Verify text is expected value: Set text to be RANDOM 256 characters",
                "Pre-Condition: Create calling range equal to entire document",
                "Pre-Condition: Create argument range equal to EMPTY range at END of document",
                "Verify CompareEndPoints(start,start) Less than zero, without errors",
                "Verify CompareEndPoints(start,  end) Less than zero, without errors",
                "Verify CompareEndPoints(  end,start) Equals zero, without errors",
                "Verify CompareEndPoints(  end,  end) Equals zero, without errors"
            })]
        [SuppressMessage("Microsoft.Globalization", "CA1303:DoNotPassLiteralsAsLocalizedParameters")]
        public void TestCompareEndpointsMethod50305(TestCaseAttribute testCase)
        {
            HeaderComment(testCase);


            CompareEndpointsHelper(SampleText.Random256, TargetRangeType.DocumentRange, TargetRangeType.EmptyEnd,
                   ValueComparison.LessThan, ValueComparison.LessThan, ValueComparison.Equals, ValueComparison.Equals, null);
        }

        ///<summary>TextPatternRange.CompareEndpoints Method.T.5.03.06</summary>
        [TestCaseAttribute("TextPatternRange.CompareEndpoints Method.T.5.03.06",
            TestSummary = "Comparing the endpoints of the TextPattern entire range against themselves",
            Priority = TestPriorities.Pri1,
            Status = TestStatus.Works,
            Author = "Microsoft Corp.",
            TestCaseType = TestCaseType.Generic,
            Description = new string[]
            {
                "Pre-Condition: Verify text is expected value: Set text to be RANDOM 256 characters",
                "Pre-Condition: Create calling range equal to entire document",
                "Pre-Condition: Create argument range equal to RANDOM range at START of document",
                "Verify CompareEndPoints(start,start) Equals zero, without errors",
                "Verify CompareEndPoints(start,  end) Less than zero, without errors",
                "Verify CompareEndPoints(  end,start) Greater Than zero, without errors",
                "Verify CompareEndPoints(  end,  end) Greater Than zero, without errors"
            })]
        [SuppressMessage("Microsoft.Globalization", "CA1303:DoNotPassLiteralsAsLocalizedParameters")]
        public void TestCompareEndpointsMethod50306(TestCaseAttribute testCase)
        {
            HeaderComment(testCase);


            CompareEndpointsHelper(SampleText.Random256, TargetRangeType.DocumentRange, TargetRangeType.RandomStart,
                   ValueComparison.Equals, ValueComparison.LessThan, ValueComparison.GreaterThan, ValueComparison.GreaterThan, null);
        }

        ///<summary>TextPatternRange.CompareEndpoints Method.T.5.03.07</summary>
        [TestCaseAttribute("TextPatternRange.CompareEndpoints Method.T.5.03.07",
            TestSummary = "Comparing the endpoints of the TextPattern entire range against themselves",
            Priority = TestPriorities.Pri1,
            Status = TestStatus.Works,
            Author = "Microsoft Corp.",
            TestCaseType = TestCaseType.Generic,
            Description = new string[]
            {
                "Pre-Condition: Verify text is expected value: Set text to be RANDOM 256 characters",
                "Pre-Condition: Create calling range equal to entire document",
                "Pre-Condition: Create argument range equal to RANDOM range in MIDDLE of document",
                "Verify CompareEndPoints(start,start) Less than zero, without errors",
                "Verify CompareEndPoints(start,  end) Less than zero, without errors",
                "Verify CompareEndPoints(  end,start) Greater Than zero, without errors",
                "Verify CompareEndPoints(  end,  end) Greater Than zero, without errors"
            })]
        [SuppressMessage("Microsoft.Globalization", "CA1303:DoNotPassLiteralsAsLocalizedParameters")]
        public void TestCompareEndpointsMethod50307(TestCaseAttribute testCase)
        {
            HeaderComment(testCase);


            CompareEndpointsHelper(SampleText.Random256, TargetRangeType.DocumentRange, TargetRangeType.RandomMiddle,
                   ValueComparison.LessThan, ValueComparison.LessThan, ValueComparison.GreaterThan, ValueComparison.GreaterThan, null);
        }

        ///<summary>TextPatternRange.CompareEndpoints Method.T.5.03.08</summary>
        [TestCaseAttribute("TextPatternRange.CompareEndpoints Method.T.5.03.08",
            TestSummary = "Comparing the endpoints of the TextPattern entire range against themselves",
            Priority = TestPriorities.Pri1,
            Status = TestStatus.Works,
            Author = "Microsoft Corp.",
            TestCaseType = TestCaseType.Generic,
            Description = new string[]
            {
                "Pre-Condition: Verify text is expected value: Set text to be RANDOM 256 characters",
                "Pre-Condition: Create calling range equal to entire document",
                "Pre-Condition: Create argument range equal to RANDOM range at END of document",
                "Verify CompareEndPoints(start,start) Less than zero, without errors",
                "Verify CompareEndPoints(start,  end) Less than zero, without errors",
                "Verify CompareEndPoints(  end,start) Greater Than zero, without errors",
                "Verify CompareEndPoints(  end,  end) Equals zero, without errors"
            })]
        [SuppressMessage("Microsoft.Globalization", "CA1303:DoNotPassLiteralsAsLocalizedParameters")]
        public void TestCompareEndpointsMethod50308(TestCaseAttribute testCase)
        {
            HeaderComment(testCase);


            CompareEndpointsHelper(SampleText.Random256, TargetRangeType.DocumentRange, TargetRangeType.RandomEnd,
                   ValueComparison.LessThan, ValueComparison.LessThan, ValueComparison.GreaterThan, ValueComparison.Equals, null);
        }

        ///<summary>TextPatternRange.CompareEndpoints Method.T.5.03.09</summary>
        [TestCaseAttribute("TextPatternRange.CompareEndpoints Method.T.5.03.09",
            TestSummary = "Comparing the endpoints of the TextPattern entire range against themselves",
            Priority = TestPriorities.Pri1,
            Status = TestStatus.Works,
            Author = "Microsoft Corp.",
            TestCaseType = TestCaseType.Generic,
            Description = new string[]
            {
                "Pre-Condition: Verify text is expected value: Set text to EMPTY string",
                "Pre-Condition: Create calling range equal to entire document",
                "Pre-Condition: Create argument range equal to EMPTY range at START of document",
                "Verify CompareEndPoints(start,start) Equals zero, without errors",
                "Verify CompareEndPoints(start,  end) Equals zero, without errors",
                "Verify CompareEndPoints(  end,start) Equals zero, without errors",
                "Verify CompareEndPoints(  end,  end) Equals zero, without errors"
            })]
        [SuppressMessage("Microsoft.Globalization", "CA1303:DoNotPassLiteralsAsLocalizedParameters")]
        public void TestCompareEndpointsMethod50309(TestCaseAttribute testCase)
        {
            HeaderComment(testCase);


            CompareEndpointsHelper(SampleText.Empty, TargetRangeType.DocumentRange, TargetRangeType.EmptyStart,
                   ValueComparison.Equals, ValueComparison.Equals, ValueComparison.Equals, ValueComparison.Equals, null);
        }

        ///<summary>TextPatternRange.CompareEndpoints Method.T.5.03.10</summary>
        [TestCaseAttribute("TextPatternRange.CompareEndpoints Method.T.5.03.10",
            TestSummary = "Comparing the endpoints of the TextPattern entire range against themselves",
            Priority = TestPriorities.Pri2,
            Status = TestStatus.Works,
            Author = "Microsoft Corp.",
            TestCaseType = TestCaseType.Generic,
            Description = new string[]
            {
                "Pre-Condition: Verify text is expected value: Set text to be RANDOM 256 characters",
                "Pre-Condition: Create calling range equal to entire document",
                "Pre-Condition: Create argument range equal to clone of calling range",
                "Verify CompareEndPoints(start,start) Equals zero, without errors",
                "Verify CompareEndPoints(start,  end) Less than zero, without errors",
                "Verify CompareEndPoints(  end,start) Greater Than zero, without errors",
                "Verify CompareEndPoints(  end,  end) Equals zero, without errors"
            })]
        [SuppressMessage("Microsoft.Globalization", "CA1303:DoNotPassLiteralsAsLocalizedParameters")]
        public void TestCompareEndpointsMethod50310(TestCaseAttribute testCase)
        {
            HeaderComment(testCase);


            CompareEndpointsHelper(SampleText.Random256, TargetRangeType.DocumentRange, TargetRangeType.Clone,
                   ValueComparison.Equals, ValueComparison.LessThan, ValueComparison.GreaterThan, ValueComparison.Equals, null);
        }

        ///<summary>TextPatternRange.CompareEndpoints Method.T.5.03.11</summary>
        [TestCaseAttribute("TextPatternRange.CompareEndpoints Method.T.5.03.11",
            TestSummary = "Comparing the endpoints of the TextPattern entire range against themselves",
            Priority = TestPriorities.Pri2,
            Status = TestStatus.Works,
            Author = "Microsoft Corp.",
            TestCaseType = TestCaseType.Generic,
            Description = new string[]
            {
                "Pre-Condition: Verify text is expected value: Set text to EMPTY string",
                "Pre-Condition: Create calling range equal to entire document",
                "Pre-Condition: Create argument range equal to clone of calling range",
                "Verify CompareEndPoints(start,start) Equals zero, without errors",
                "Verify CompareEndPoints(start,  end) Equals zero, without errors",
                "Verify CompareEndPoints(  end,start) Equals zero, without errors",
                "Verify CompareEndPoints(  end,  end) Equals zero, without errors"
            })]
        [SuppressMessage("Microsoft.Globalization", "CA1303:DoNotPassLiteralsAsLocalizedParameters")]
        public void TestCompareEndpointsMethod50311(TestCaseAttribute testCase)
        {
            HeaderComment(testCase);


            CompareEndpointsHelper(SampleText.Empty, TargetRangeType.DocumentRange, TargetRangeType.Clone,
                   ValueComparison.Equals, ValueComparison.Equals, ValueComparison.Equals, ValueComparison.Equals, null);
        }

        ///<summary>TextPatternRange.CompareEndpoints Method.T.5.03.12</summary>
        [TestCaseAttribute("TextPatternRange.CompareEndpoints Method.T.5.03.12",
            TestSummary = "Comparing the endpoints of the TextPattern entire range against themselves",
            Priority = TestPriorities.Pri2,
            Status = TestStatus.Works,
            Author = "Microsoft Corp.",
            TestCaseType = TestCaseType.Generic,
            Description = new string[]
            {
                "Pre-Condition: Verify text is expected value: Set text to be RANDOM 256 characters",
                "Pre-Condition: Create calling range equal to RANDOM range in MIDDLE of document",
                "Pre-Condition: Create argument range equal to range that STARTs at END of calling range",
                "Verify CompareEndPoints(start,start) Less than zero, without errors",
                "Verify CompareEndPoints(start,  end) Less than zero, without errors",
                "Verify CompareEndPoints(  end,start) Equals zero, without errors",
                "Verify CompareEndPoints(  end,  end) Less than zero, without errors"
            })]
        [SuppressMessage("Microsoft.Globalization", "CA1303:DoNotPassLiteralsAsLocalizedParameters")]
        public void TestCompareEndpointsMethod50312(TestCaseAttribute testCase)
        {
            HeaderComment(testCase);


            CompareEndpointsHelper(SampleText.Random256, TargetRangeType.RandomMiddle, TargetRangeType.RandomStartsEnd,
                   ValueComparison.LessThan, ValueComparison.LessThan, ValueComparison.Equals, ValueComparison.LessThan, null);
        }

        ///<summary>TextPatternRange.CompareEndpoints Method.T.5.03.13</summary>
        [TestCaseAttribute("TextPatternRange.CompareEndpoints Method.T.5.03.13",
            TestSummary = "Comparing the endpoints of the TextPattern entire range against themselves",
            Priority = TestPriorities.Pri2,
            Status = TestStatus.Works,
            Author = "Microsoft Corp.",
            TestCaseType = TestCaseType.Generic,
            Description = new string[]
            {
                "Pre-Condition: Verify text is expected value: Set text to be RANDOM 256 characters",
                "Pre-Condition: Create calling range equal to RANDOM range in MIDDLE of document",
                "Pre-Condition: Create argument range equal to range that STARTs at START of calling range",
                "Verify CompareEndPoints(start,start) Equals zero, without errors",
                "Verify CompareEndPoints(start,  end) Less than zero, without errors",
                "Verify CompareEndPoints(  end,start) Greater Than zero, without errors",
                "Verify CompareEndPoints(  end,  end) Less than zero, without errors"
            })]
        [SuppressMessage("Microsoft.Globalization", "CA1303:DoNotPassLiteralsAsLocalizedParameters")]
        public void TestCompareEndpointsMethod50313(TestCaseAttribute testCase)
        {
            HeaderComment(testCase);


            CompareEndpointsHelper(SampleText.Random256, TargetRangeType.RandomMiddle, TargetRangeType.RandomStartStart,
                   ValueComparison.Equals, ValueComparison.LessThan, ValueComparison.GreaterThan, ValueComparison.LessThan, null);
        }

        ///<summary>TextPatternRange.CompareEndpoints Method.T.5.03.14</summary>
        [TestCaseAttribute("TextPatternRange.CompareEndpoints Method.T.5.03.14",
            TestSummary = "Comparing the endpoints of the TextPattern entire range against themselves",
            Priority = TestPriorities.Pri2,
            Status = TestStatus.Works,
            Author = "Microsoft Corp.",
            TestCaseType = TestCaseType.Generic,
            Description = new string[]
            {
                "Pre-Condition: Verify text is expected value: Set text to be RANDOM 256 characters",
                "Pre-Condition: Create calling range equal to RANDOM range in MIDDLE of document",
                "Pre-Condition: Create argument range equal to range that ENDs at END of calling range",
                "Verify CompareEndPoints(start,start) Greater Than zero, without errors",
                "Verify CompareEndPoints(start,  end) Less than zero, without errors",
                "Verify CompareEndPoints(  end,start) Greater Than zero, without errors",
                "Verify CompareEndPoints(  end,  end) Equals zero, without errors"
            })]
        [SuppressMessage("Microsoft.Globalization", "CA1303:DoNotPassLiteralsAsLocalizedParameters")]
        public void TestCompareEndpointsMethod50314(TestCaseAttribute testCase)
        {
            HeaderComment(testCase);


            CompareEndpointsHelper(SampleText.Random256, TargetRangeType.RandomMiddle, TargetRangeType.RandomEndsEnd,
                   ValueComparison.GreaterThan, ValueComparison.LessThan, ValueComparison.GreaterThan, ValueComparison.Equals, null);
        }

        ///<summary>TextPatternRange.CompareEndpoints Method.T.5.03.16</summary>
        [TestCaseAttribute("TextPatternRange.CompareEndpoints Method.T.5.03.16",
            TestSummary = "Comparing the endpoints of the TextPattern entire range against themselves",
            Priority = TestPriorities.Pri2,
            Status = TestStatus.Works,
            Author = "Microsoft Corp.",
            TestCaseType = TestCaseType.Generic,
            Description = new string[]
            {
                "Pre-Condition: Verify text is expected value: Set text to be RANDOM 256 characters",
                "Pre-Condition: Create calling range equal to entire document",
                "Pre-Condition: Create argument range equal to range from a different TextPattern",
                "Verify CompareEndPoints(start,start) Equals zero, RAISES System.ArgumentException exception",
                "Verify CompareEndPoints(start,  end) Equals zero, RAISES System.ArgumentException exception",
                "Verify CompareEndPoints(  end,start) Equals zero, RAISES System.ArgumentException exception",
                "Verify CompareEndPoints(  end,  end) Equals zero, RAISES System.ArgumentException exception"
            })]
        [SuppressMessage("Microsoft.Globalization", "CA1303:DoNotPassLiteralsAsLocalizedParameters")]
        public void TestCompareEndpointsMethod50316(TestCaseAttribute testCase)
        {
            HeaderComment(testCase);

            ArgumentException ex = new ArgumentException("Unused, just need an instance for GetType() below");

            CompareEndpointsHelper(SampleText.Random256, TargetRangeType.DocumentRange, TargetRangeType.DifferentTextPattern,
                   ValueComparison.Equals, ValueComparison.Equals, ValueComparison.Equals, ValueComparison.Equals, ex.GetType());
        }

        ///<summary>TextPatternRange.CompareEndpoints Method.T.5.03.17</summary>
        [TestCaseAttribute("TextPatternRange.CompareEndpoints Method.T.5.03.17",
            TestSummary = "Comparing the endpoints of the TextPattern entire range against themselves",
            Priority = TestPriorities.Pri2,
            Status = TestStatus.Works,
            Author = "Microsoft Corp.",
            TestCaseType = TestCaseType.Generic,
            Description = new string[]
            {
                "Pre-Condition: Verify text is expected value: Set text to EMPTY string",
                "Pre-Condition: Create calling range equal to entire document",
                "Pre-Condition: Create argument range equal to null value",
                "Verify CompareEndPoints(start,start) Equals zero, RAISES System.ArgumentNullException exception",
                "Verify CompareEndPoints(start,  end) Equals zero, RAISES System.ArgumentNullException exception",
                "Verify CompareEndPoints(  end,start) Equals zero, RAISES System.ArgumentNullException exception",
                "Verify CompareEndPoints(  end,  end) Equals zero, RAISES System.ArgumentNullException exception"
            })]
        [SuppressMessage("Microsoft.Globalization", "CA1303:DoNotPassLiteralsAsLocalizedParameters")]
        public void TestCompareEndpointsMethod50317(TestCaseAttribute testCase)
        {
            HeaderComment(testCase);

            ArgumentNullException ex = new ArgumentNullException("testCase"); //  Unused, just need an instance for GetType() below

            CompareEndpointsHelper(SampleText.Empty, TargetRangeType.DocumentRange, TargetRangeType.Null,
                   ValueComparison.Equals, ValueComparison.Equals, ValueComparison.Equals, ValueComparison.Equals, ex.GetType());
        }

        #endregion 5.3 TextPatternRange.CompareEndpoints Method

        #region 5.4 TextPatternRange.Select Method

        ///<summary>TextPatternRange.Select Method.T.5.04.01</summary>
        [TestCaseAttribute("TextPatternRange.Select Method.T.5.04.01",
            TestSummary = "Creating a range for the entire textpattern, selecting it and validating the selection",
            Priority = TestPriorities.Pri0,
            Status = TestStatus.Works,
            Author = "Microsoft Corp.",
            TestCaseType = TestCaseType.Generic,
            Description = new string[]
            {
                "Pre-Condition: SetFocus to control containing TextPattern",
                "Pre-Condition: Verify text is expected value: Set text to 'String1'",
                "Pre-Condition: Create calling range equal to entire document",
                "Select the range",
                "Get the text from the selection",
                "Verify the selection is as expected"
            })]
        public void TestSelectMethod50401(TestCaseAttribute testCase)
        {
            HeaderComment(testCase);

            SelectHelper(SampleText.String1, TargetRangeType.DocumentRange, null);
        }

        ///<summary>TextPatternRange.Select Method.T.5.04.02</summary>
        [TestCaseAttribute("TextPatternRange.Select Method.T.5.04.02",
            TestSummary = "Creating a range for the entire textpattern, selecting it and validating the selection",
            Priority = TestPriorities.Pri1,
            Status = TestStatus.Works,
            Author = "Microsoft Corp.",
            TestCaseType = TestCaseType.Generic,
            Description = new string[]
            {
                "Pre-Condition: SetFocus to control containing TextPattern",
                "Pre-Condition: Verify text is expected value: Set text to be RANDOM 256 characters",
                "Pre-Condition: Create calling range equal to entire document",
                "Select the range",
                "Get the text from the selection",
                "Verify the selection is as expected"
            })]
        public void TestSelectMethod50402(TestCaseAttribute testCase)
        {
            HeaderComment(testCase);

            SelectHelper(SampleText.Random256, TargetRangeType.DocumentRange, null);
        }

        ///<summary>TextPatternRange.Select Method.T.5.04.03</summary>
        [TestCaseAttribute("TextPatternRange.Select Method.T.5.04.03",
            TestSummary = "Creating a range for the entire textpattern, selecting it and validating the selection",
            Priority = TestPriorities.Pri1,
            Status = TestStatus.Works,
            Author = "Microsoft Corp.",
            TestCaseType = TestCaseType.Generic,
            Description = new string[]
            {
                "Pre-Condition: SetFocus to control containing TextPattern",
                "Pre-Condition: Verify text is expected value: Set text to be RANDOM 256 characters",
                "Pre-Condition: Create calling range equal to RANDOM range at START of document",
                "Select the range",
                "Get the text from the selection",
                "Verify the selection is as expected"
            })]
        public void TestSelectMethod50403(TestCaseAttribute testCase)
        {
            HeaderComment(testCase);

            SelectHelper(SampleText.Random256, TargetRangeType.RandomStart, null);
        }

        ///<summary>TextPatternRange.Select Method.T.5.04.04</summary>
        [TestCaseAttribute("TextPatternRange.Select Method.T.5.04.04",
            TestSummary = "Creating a range for the entire textpattern, selecting it and validating the selection",
            Priority = TestPriorities.Pri1,
            Status = TestStatus.Works,
            Author = "Microsoft Corp.",
            TestCaseType = TestCaseType.Generic,
            Description = new string[]
            {
                "Pre-Condition: SetFocus to control containing TextPattern",
                "Pre-Condition: Verify text is expected value: Set text to be RANDOM 256 characters",
                "Pre-Condition: Create calling range equal to RANDOM range in MIDDLE of document",
                "Select the range",
                "Get the text from the selection",
                "Verify the selection is as expected"
            })]
        public void TestSelectMethod50404(TestCaseAttribute testCase)
        {
            HeaderComment(testCase);

            SelectHelper(SampleText.Random256, TargetRangeType.RandomMiddle, null);
        }

        ///<summary>TextPatternRange.Select Method.T.5.04.05</summary>
        [TestCaseAttribute("TextPatternRange.Select Method.T.5.04.05",
            TestSummary = "Creating a range for the entire textpattern, selecting it and validating the selection",
            Priority = TestPriorities.Pri1,
            Status = TestStatus.Works,
            Author = "Microsoft Corp.",
            TestCaseType = TestCaseType.Generic,
            Description = new string[]
            {
                "Pre-Condition: SetFocus to control containing TextPattern",
                "Pre-Condition: Verify text is expected value: Set text to be RANDOM 256 characters",
                "Pre-Condition: Create calling range equal to RANDOM range at END of document",
                "Select the range",
                "Get the text from the selection",
                "Verify the selection is as expected"
            })]
        public void TestSelectMethod50405(TestCaseAttribute testCase)
        {
            HeaderComment(testCase);

            SelectHelper(SampleText.Random256, TargetRangeType.RandomEnd, null);
        }

        ///<summary>TextPatternRange.Select Method.T.5.04.06</summary>
        [TestCaseAttribute("TextPatternRange.Select Method.T.5.04.06",
            TestSummary = "Creating a range for the entire textpattern, selecting it and validating the selection",
            Priority = TestPriorities.Pri1,
            Status = TestStatus.Works,
            Author = "Microsoft Corp.",
            TestCaseType = TestCaseType.Generic,
            Description = new string[]
            {
                "Pre-Condition: SetFocus to control containing TextPattern",
                "Pre-Condition: Verify text is expected value: Set text to EMPTY string",
                "Pre-Condition: Create calling range equal to EMPTY range at START of document",
                "Select the range",
                "Get the text from the selection",
                "Verify the selection is as expected"
            })]
        public void TestSelectMethod50406(TestCaseAttribute testCase)
        {
            HeaderComment(testCase);

            SelectHelper(SampleText.Empty, TargetRangeType.EmptyStart, null);
        }

        #endregion 5.4 TextPatternRange.Select Method

        #region 5.5 TextPatternRange.FindAttribute Method

        ///<summary>TextPatternRange.FindAttribute Method.T.5.05.01</summary>
        [TestCaseAttribute("TextPatternRange.FindAttribute Method.T.5.05.01",
            TestSummary = "Tests FindAttribute with supported/unsupported attributes, search direction and correct/incorrect arguments",
            Priority = TestPriorities.Pri0,
            Status = TestStatus.Works,
            Author = "Microsoft Corp.",
            TestCaseType = TestCaseType.Generic,
            Description = new string[]
            {
                "Pre-Condition: Verify text is expected value: Set text to 'String1'",
                "Pre-Condition: Identify & use Supported attributes on the control",
                "Pre-Condition: Identify if document has consistent attribute values",
                "Pre-Condition: For each attribute, Val argument has CORRECT type and value",
                "Call FindAttribute(search FORWARDS from beginning) verify result MATCHING range (case sensitive)"
            })]
        [SuppressMessage("Microsoft.Globalization", "CA1303:DoNotPassLiteralsAsLocalizedParameters")]
        public void TestFindAttributeMethod50501(TestCaseAttribute testCase)
        {
            HeaderComment(testCase);


            FindAttributeHelper(SampleText.String1, TypeValue.MatchesTypeAndValue, AttributeType.SupportedAttributes, false, FindResults.MatchingRange, null);
        }

        ///<summary>TextPatternRange.FindAttribute Method.T.5.05.03</summary>
        [TestCaseAttribute("TextPatternRange.FindAttribute Method.T.5.05.03",
            TestSummary = "Tests FindAttribute with supported/unsupported attributes, search direction and correct/incorrect arguments",
            Priority = TestPriorities.Pri1,
            Status = TestStatus.Works,
            Author = "Microsoft Corp.",
            TestCaseType = TestCaseType.Generic,
            Description = new string[]
            {
                "Pre-Condition: Verify text is expected value: Set text to span MULTIPLE LINES",
                "Pre-Condition: Identify & use Supported attributes on the control",
                "Pre-Condition: Identify if document has consistent attribute values",
                "Pre-Condition: For each attribute, Val argument has CORRECT type and value",
                "Call FindAttribute(search BACKWARDS from end) verify result MATCHING range (case sensitive)"
            })]
        [SuppressMessage("Microsoft.Globalization", "CA1303:DoNotPassLiteralsAsLocalizedParameters")]
        public void TestFindAttributeMethod50503(TestCaseAttribute testCase)
        {
            HeaderComment(testCase);


            FindAttributeHelper(SampleText.MultipleLines, TypeValue.MatchesTypeAndValue, AttributeType.SupportedAttributes, true, FindResults.MatchingRange, null);
        }

        ///<summary>TextPatternRange.FindAttribute Method.T.5.05.05</summary>
        [TestCaseAttribute("TextPatternRange.FindAttribute Method.T.5.05.05",
            TestSummary = "Tests FindAttribute with supported/unsupported attributes, search direction and correct/incorrect arguments",
            Priority = TestPriorities.Pri1,
            Status = TestStatus.Works,
            Author = "Microsoft Corp.",
            TestCaseType = TestCaseType.Generic,
            Description = new string[]
            {
                "Pre-Condition: Verify text is expected value: Set text to span MULTIPLE LINES",
                "Pre-Condition: Identify & use Supported attributes on the control",
                "Pre-Condition: Identify if document has consistent attribute values",
                "Pre-Condition: For each attribute, Val argument has INCORRECT type, CORRECT value",
                "Call FindAttribute(search FORWARDS from beginning)  RAISES System.ArgumentException exception"
            })]
        [SuppressMessage("Microsoft.Globalization", "CA1303:DoNotPassLiteralsAsLocalizedParameters")]
        public void TestFindAttributeMethod50505(TestCaseAttribute testCase)
        {
            HeaderComment(testCase);

            ArgumentException ex = new ArgumentException("Unused, just need an instance for GetType() below");

            FindAttributeHelper(SampleText.MultipleLines, TypeValue.WrongType, AttributeType.SupportedAttributes, false, FindResults.Exception, ex.GetType());
        }

        ///<summary>TextPatternRange.FindAttribute Method.T.5.05.06</summary>
        [TestCaseAttribute("TextPatternRange.FindAttribute Method.T.5.05.06",
            TestSummary = "Tests FindAttribute with supported/unsupported attributes, search direction and correct/incorrect arguments",
            Priority = TestPriorities.Pri1,
            Status = TestStatus.Works,
            Author = "Microsoft Corp.",
            TestCaseType = TestCaseType.Generic,
            Description = new string[]
            {
                "Pre-Condition: Verify text is expected value: Set text to span MULTIPLE LINES",
                "Pre-Condition: Identify & use Supported attributes on the control",
                "Pre-Condition: Identify if document has consistent attribute values",
                "Pre-Condition: For each attribute, Val argument has CORRECT type, INCORRECT value",
                "Call FindAttribute(search FORWARDS from beginning) verify result NULL"
            })]
        [SuppressMessage("Microsoft.Globalization", "CA1303:DoNotPassLiteralsAsLocalizedParameters")]
        public void TestFindAttributeMethod50506(TestCaseAttribute testCase)
        {
            HeaderComment(testCase);


            FindAttributeHelper(SampleText.MultipleLines, TypeValue.WrongValue, AttributeType.SupportedAttributes, false, FindResults.Null, null);
        }

        ///<summary>TextPatternRange.FindAttribute Method.T.5.05.07</summary>
        [TestCaseAttribute("TextPatternRange.FindAttribute Method.T.5.05.07",
            TestSummary = "Tests FindAttribute with supported/unsupported attributes, search direction and correct/incorrect arguments",
            Priority = TestPriorities.Pri1,
            Status = TestStatus.Works,
            Author = "Microsoft Corp.",
            TestCaseType = TestCaseType.Generic,
            Description = new string[]
            {
                "Pre-Condition: Verify text is expected value: Set text to span MULTIPLE LINES",
                "Pre-Condition: Identify & use Supported attributes on the control",
                "Pre-Condition: Identify if document has consistent attribute values",
                "Pre-Condition: For each attribute, Val argument has INCORRECT type and value",
                "Call FindAttribute(search FORWARDS from beginning)  RAISES System.ArgumentException exception"
            })]
        [SuppressMessage("Microsoft.Globalization", "CA1303:DoNotPassLiteralsAsLocalizedParameters")]
        public void TestFindAttributeMethod50507(TestCaseAttribute testCase)
        {
            HeaderComment(testCase);

            ArgumentException ex = new ArgumentException("Unused, just need an instance for GetType() below");

            FindAttributeHelper(SampleText.MultipleLines, TypeValue.WrongTypeAndValue, AttributeType.SupportedAttributes, false, FindResults.Exception, ex.GetType());
        }

        ///<summary>TextPatternRange.FindAttribute Method.T.5.05.11</summary>
        [TestCaseAttribute("TextPatternRange.FindAttribute Method.T.5.05.11",
            TestSummary = "Tests FindAttribute with supported/unsupported attributes, search direction and correct/incorrect arguments",
            Priority = TestPriorities.Pri2,
            Status = TestStatus.Works,
            Author = "Microsoft Corp.",
            TestCaseType = TestCaseType.Generic,
            Description = new string[]
            {
                "Pre-Condition: Verify text is expected value: Set text to EMPTY string",
                "Pre-Condition: Identify & use Supported attributes on the control",
                "Pre-Condition: Identify if document has consistent attribute values",
                "Pre-Condition: For each attribute, Val argument has INCORRECT type, CORRECT value",
                "Call FindAttribute(search FORWARDS from beginning)  RAISES System.ArgumentException exception"
            })]
        [SuppressMessage("Microsoft.Globalization", "CA1303:DoNotPassLiteralsAsLocalizedParameters")]
        public void TestFindAttributeMethod50511(TestCaseAttribute testCase)
        {
            HeaderComment(testCase);

            ArgumentException ex = new ArgumentException("Unused, just need an instance for GetType() below");

            FindAttributeHelper(SampleText.Empty, TypeValue.WrongType, AttributeType.SupportedAttributes, false, FindResults.Exception, ex.GetType());
        }

        ///<summary>TextPatternRange.FindAttribute Method.T.5.05.12</summary>
        [TestCaseAttribute("TextPatternRange.FindAttribute Method.T.5.05.12",
            TestSummary = "Tests FindAttribute with supported/unsupported attributes, search direction and correct/incorrect arguments",
            Priority = TestPriorities.Pri2,
            Status = TestStatus.Works,
            Author = "Microsoft Corp.",
            TestCaseType = TestCaseType.Generic,
            Description = new string[]
            {
                "Pre-Condition: Verify text is expected value: Set text to EMPTY string",
                "Pre-Condition: Identify & use Supported attributes on the control",
                "Pre-Condition: Identify if document has consistent attribute values",
                "Pre-Condition: For each attribute, Val argument has CORRECT type, INCORRECT value",
                "Call FindAttribute(search FORWARDS from beginning) verify result NULL"
            })]
        [SuppressMessage("Microsoft.Globalization", "CA1303:DoNotPassLiteralsAsLocalizedParameters")]
        public void TestFindAttributeMethod50512(TestCaseAttribute testCase)
        {
            HeaderComment(testCase);


            FindAttributeHelper(SampleText.Empty, TypeValue.WrongValue, AttributeType.SupportedAttributes, false, FindResults.Null, null);
        }

        ///<summary>TextPatternRange.FindAttribute Method.T.5.05.13</summary>
        [TestCaseAttribute("TextPatternRange.FindAttribute Method.T.5.05.13",
            TestSummary = "Tests FindAttribute with supported/unsupported attributes, search direction and correct/incorrect arguments",
            Priority = TestPriorities.Pri2,
            Status = TestStatus.Works,
            Author = "Microsoft Corp.",
            TestCaseType = TestCaseType.Generic,
            Description = new string[]
            {
                "Pre-Condition: Verify text is expected value: Set text to EMPTY string",
                "Pre-Condition: Identify & use Supported attributes on the control",
                "Pre-Condition: Identify if document has consistent attribute values",
                "Pre-Condition: For each attribute, Val argument has INCORRECT type and value",
                "Call FindAttribute(search FORWARDS from beginning)  RAISES System.ArgumentException exception"
            })]
        [SuppressMessage("Microsoft.Globalization", "CA1303:DoNotPassLiteralsAsLocalizedParameters")]
        public void TestFindAttributeMethod50513(TestCaseAttribute testCase)
        {
            HeaderComment(testCase);

            ArgumentException ex = new ArgumentException("Unused, just need an instance for GetType() below");

            FindAttributeHelper(SampleText.Empty, TypeValue.WrongTypeAndValue, AttributeType.SupportedAttributes, false, FindResults.Exception, ex.GetType());
        }

        ///<summary>TextPatternRange.FindAttribute Method.T.5.05.15</summary>
        [TestCaseAttribute("TextPatternRange.FindAttribute Method.T.5.05.15",
            TestSummary = "Pass in NULL for attribute argument, raising ArgumentNullException",
            Priority = TestPriorities.Pri2,
            Status = TestStatus.Works,
            Author = "Microsoft Corp.",
            TestCaseType = TestCaseType.Generic,
            Description = new string[]
            {
                "Pre-Condition: Verify text is expected value: Set text to be RANDOM 256 characters",
                "Call FindAttribute(null,<correct type & value>,false) RAISES System.ArgumentNullException exception"
            })]
        [SuppressMessage("Microsoft.Globalization", "CA1303:DoNotPassLiteralsAsLocalizedParameters")]
        public void TestFindAttributeMethod50515(TestCaseAttribute testCase)
        {
            HeaderComment(testCase);

            ArgumentNullException ex = new ArgumentNullException("testCase"); //  Unused, just need an instance for GetType() below

            FindAttributeHelper2(SampleText.Random256, ex.GetType());
        }

        ///<summary>TextPatternRange.FindAttribute Method.T.5.05.16</summary>
        [TestCaseAttribute("TextPatternRange.FindAttribute Method.T.5.05.16",
            TestSummary = "Pass in NULL for value argument, raising ArgumentNullException",
            Priority = TestPriorities.Pri2,
            Status = TestStatus.Works,
            Author = "Microsoft Corp.",
            TestCaseType = TestCaseType.Generic,
            Description = new string[]
            {
                "Pre-Condition: Verify text is expected value: Set text to be RANDOM 256 characters",
                "Call FindAttribute(<faux random Attribute>,null,false) RAISES System.ArgumentNullException exception"
            })]
        [SuppressMessage("Microsoft.Globalization", "CA1303:DoNotPassLiteralsAsLocalizedParameters")]
        public void TestFindAttributeMethod50516(TestCaseAttribute testCase)
        {
            HeaderComment(testCase);

            ArgumentNullException ex = new ArgumentNullException("testCase"); //  Unused, just need an instance for GetType() below

            FindAttributeHelper3(SampleText.Random256, ex.GetType());
        }

        #endregion 5.5 TextPatternRange.FindAttribute Method

        #region 5.6 TextPatternRange.FindText Method

        ///<summary>TextPatternRange.FindText Method.T.5.06.01</summary>
        [TestCaseAttribute("TextPatternRange.FindText Method.T.5.06.01",
            TestSummary = "Call FindText() with variation of backward/forward & case sensistive/in-senstive",
            Priority = TestPriorities.Pri0,
            Status = TestStatus.Works,
            Author = "Microsoft Corp.",
            TestCaseType = TestCaseType.Generic,
            Description = new string[]
            {
                "Pre-Condition: Verify text is expected value: Set text to 'String1'",
                "Pre-Condition: Text to search for is matches ALL Text",
                "CallFindText(forwards, case-insensitive)verify result MATCHING range (case sensitive)",
                "CallFindText(forwards, case-sensitive)  verify result MATCHING range (case sensitive)",
                "CallFindText(backwards,case-insensitive)verify result MATCHING range (case sensitive)",
                "CallFindText(backwards,case-sensitive)  verify result MATCHING range (case sensitive)"
            })]
        [SuppressMessage("Microsoft.Globalization", "CA1303:DoNotPassLiteralsAsLocalizedParameters")]
        public void TestFindTextMethod50601(TestCaseAttribute testCase)
        {
            HeaderComment(testCase);


            FindTextHelper(SampleText.String1, SearchText.MatchesAll,
                   FindResults.MatchingRange, FindResults.MatchingRange, FindResults.MatchingRange, FindResults.MatchingRange, null);
        }

        ///<summary>TextPatternRange.FindText Method.T.5.06.02</summary>
        [TestCaseAttribute("TextPatternRange.FindText Method.T.5.06.02",
            TestSummary = "Call FindText() with variation of backward/forward & case sensistive/in-senstive",
            Priority = TestPriorities.Pri1,
            Status = TestStatus.Works,
            Author = "Microsoft Corp.",
            TestCaseType = TestCaseType.Generic,
            Description = new string[]
            {
                "Pre-Condition: Verify text is expected value: Set text to be RANDOM 256 characters",
                "Pre-Condition: Text to search for is matches ALL Text",
                "CallFindText(forwards, case-insensitive)verify result MATCHING range (case sensitive)",
                "CallFindText(forwards, case-sensitive)  verify result MATCHING range (case sensitive)",
                "CallFindText(backwards,case-insensitive)verify result MATCHING range (case sensitive)",
                "CallFindText(backwards,case-sensitive)  verify result MATCHING range (case sensitive)"
            })]
        [SuppressMessage("Microsoft.Globalization", "CA1303:DoNotPassLiteralsAsLocalizedParameters")]
        public void TestFindTextMethod50602(TestCaseAttribute testCase)
        {
            HeaderComment(testCase);


            FindTextHelper(SampleText.Random256, SearchText.MatchesAll,
                   FindResults.MatchingRange, FindResults.MatchingRange, FindResults.MatchingRange, FindResults.MatchingRange, null);
        }

        ///<summary>TextPatternRange.FindText Method.T.5.06.03</summary>
        [TestCaseAttribute("TextPatternRange.FindText Method.T.5.06.03",
            TestSummary = "Call FindText() with variation of backward/forward & case sensistive/in-senstive",
            Priority = TestPriorities.Pri1,
            Status = TestStatus.Works,
            Author = "Microsoft Corp.",
            TestCaseType = TestCaseType.Generic,
            Description = new string[]
            {
                "Pre-Condition: Verify text is expected value: Set text to be RANDOM 256 characters",
                "Pre-Condition: Text to search for is matches sub range at START of doc",
                "CallFindText(forwards, case-insensitive)verify result MATCHING range (case sensitive)",
                "CallFindText(forwards, case-sensitive)  verify result MATCHING range (case sensitive)",
                "CallFindText(backwards,case-insensitive)verify result MATCHING range (case sensitive)",
                "CallFindText(backwards,case-sensitive)  verify result MATCHING range (case sensitive)"
            })]
        [SuppressMessage("Microsoft.Globalization", "CA1303:DoNotPassLiteralsAsLocalizedParameters")]
        public void TestFindTextMethod50603(TestCaseAttribute testCase)
        {
            HeaderComment(testCase);


            FindTextHelper(SampleText.Random256, SearchText.MatchesSubRangeFirst,
                   FindResults.MatchingRange, FindResults.MatchingRange, FindResults.MatchingRange, FindResults.MatchingRange, null);
        }

        ///<summary>TextPatternRange.FindText Method.T.5.06.04</summary>
        [TestCaseAttribute("TextPatternRange.FindText Method.T.5.06.04",
            TestSummary = "Call FindText() with variation of backward/forward & case sensistive/in-senstive",
            Priority = TestPriorities.Pri1,
            Status = TestStatus.Works,
            Author = "Microsoft Corp.",
            TestCaseType = TestCaseType.Generic,
            Description = new string[]
            {
                "Pre-Condition: Verify text is expected value: Set text to be RANDOM 256 characters",
                "Pre-Condition: Text to search for is matches sub range in MIDDLE of doc",
                "CallFindText(forwards, case-insensitive)verify result MATCHING range (case sensitive)",
                "CallFindText(forwards, case-sensitive)  verify result MATCHING range (case sensitive)",
                "CallFindText(backwards,case-insensitive)verify result MATCHING range (case sensitive)",
                "CallFindText(backwards,case-sensitive)  verify result MATCHING range (case sensitive)"
            })]
        [SuppressMessage("Microsoft.Globalization", "CA1303:DoNotPassLiteralsAsLocalizedParameters")]
        public void TestFindTextMethod50604(TestCaseAttribute testCase)
        {
            HeaderComment(testCase);


            FindTextHelper(SampleText.Random256, SearchText.MatchesSubRangeMiddle,
                   FindResults.MatchingRange, FindResults.MatchingRange, FindResults.MatchingRange, FindResults.MatchingRange, null);
        }

        ///<summary>TextPatternRange.FindText Method.T.5.06.05</summary>
        [TestCaseAttribute("TextPatternRange.FindText Method.T.5.06.05",
            TestSummary = "Call FindText() with variation of backward/forward & case sensistive/in-senstive",
            Priority = TestPriorities.Pri1,
            Status = TestStatus.Works,
            Author = "Microsoft Corp.",
            TestCaseType = TestCaseType.Generic,
            Description = new string[]
            {
                "Pre-Condition: Verify text is expected value: Set text to be RANDOM 256 characters",
                "Pre-Condition: Text to search for is matches sub range at END of doc",
                "CallFindText(forwards, case-insensitive)verify result MATCHING range (case sensitive)",
                "CallFindText(forwards, case-sensitive)  verify result MATCHING range (case sensitive)",
                "CallFindText(backwards,case-insensitive)verify result MATCHING range (case sensitive)",
                "CallFindText(backwards,case-sensitive)  verify result MATCHING range (case sensitive)"
            })]
        [SuppressMessage("Microsoft.Globalization", "CA1303:DoNotPassLiteralsAsLocalizedParameters")]
        public void TestFindTextMethod50605(TestCaseAttribute testCase)
        {
            HeaderComment(testCase);


            FindTextHelper(SampleText.Random256, SearchText.MatchesSubRangeEnd,
                   FindResults.MatchingRange, FindResults.MatchingRange, FindResults.MatchingRange, FindResults.MatchingRange, null);
        }

        ///<summary>TextPatternRange.FindText Method.T.5.06.06</summary>
        [TestCaseAttribute("TextPatternRange.FindText Method.T.5.06.06",
            TestSummary = "Call FindText() with variation of backward/forward & case sensistive/in-senstive",
            Priority = TestPriorities.Pri1,
            Status = TestStatus.Works,
            Author = "Microsoft Corp.",
            TestCaseType = TestCaseType.Generic,
            Description = new string[]
            {
                "Pre-Condition: Verify text is expected value: Set text to be RANDOM 256 characters",
                "Pre-Condition: Text to search for is matches ALL Text, w/MISMATCHED case",
                "CallFindText(forwards, case-insensitive)verify result NULL",
                "CallFindText(forwards, case-sensitive)  matching range (case insensitive)",
                "CallFindText(backwards,case-insensitive)verify result NULL",
                "CallFindText(backwards,case-sensitive)  matching range (case insensitive)"
            })]
        [SuppressMessage("Microsoft.Globalization", "CA1303:DoNotPassLiteralsAsLocalizedParameters")]
        public void TestFindTextMethod50606(TestCaseAttribute testCase)
        {
            HeaderComment(testCase);


            FindTextHelper(SampleText.Random256, SearchText.MismatchedCaseAll,
                   FindResults.Null, FindResults.MatchingRangeCaseLess, FindResults.Null, FindResults.MatchingRangeCaseLess, null);
        }

        ///<summary>TextPatternRange.FindText Method.T.5.06.07</summary>
        [TestCaseAttribute("TextPatternRange.FindText Method.T.5.06.07",
            TestSummary = "Call FindText() with variation of backward/forward & case sensistive/in-senstive",
            Priority = TestPriorities.Pri1,
            Status = TestStatus.Works,
            Author = "Microsoft Corp.",
            TestCaseType = TestCaseType.Generic,
            Description = new string[]
            {
                "Pre-Condition: Verify text is expected value: Set text to be RANDOM 256 characters",
                "Pre-Condition: Text to search for is matches sub range at START of doc, w/MISMATCHED case",
                "CallFindText(forwards, case-insensitive)verify result NULL",
                "CallFindText(forwards, case-sensitive)  matching range (case insensitive)",
                "CallFindText(backwards,case-insensitive)verify result NULL",
                "CallFindText(backwards,case-sensitive)  matching range (case insensitive)"
            })]
        [SuppressMessage("Microsoft.Globalization", "CA1303:DoNotPassLiteralsAsLocalizedParameters")]
        public void TestFindTextMethod50607(TestCaseAttribute testCase)
        {
            HeaderComment(testCase);


            FindTextHelper(SampleText.Random256, SearchText.MismatchedCaseSubRangeFirst,
                   FindResults.Null, FindResults.MatchingRangeCaseLess, FindResults.Null, FindResults.MatchingRangeCaseLess, null);
        }

        ///<summary>TextPatternRange.FindText Method.T.5.06.08</summary>
        [TestCaseAttribute("TextPatternRange.FindText Method.T.5.06.08",
            TestSummary = "Call FindText() with variation of backward/forward & case sensistive/in-senstive",
            Priority = TestPriorities.Pri1,
            Status = TestStatus.Works,
            Author = "Microsoft Corp.",
            TestCaseType = TestCaseType.Generic,
            Description = new string[]
            {
                "Pre-Condition: Verify text is expected value: Set text to be RANDOM 256 characters",
                "Pre-Condition: Text to search for is matches sub range in MIDDLE of doc, w/MISMATCHED case",
                "CallFindText(forwards, case-insensitive)verify result NULL",
                "CallFindText(forwards, case-sensitive)  matching range (case insensitive)",
                "CallFindText(backwards,case-insensitive)verify result NULL",
                "CallFindText(backwards,case-sensitive)  matching range (case insensitive)"
            })]
        [SuppressMessage("Microsoft.Globalization", "CA1303:DoNotPassLiteralsAsLocalizedParameters")]
        public void TestFindTextMethod50608(TestCaseAttribute testCase)
        {
            HeaderComment(testCase);


            FindTextHelper(SampleText.Random256, SearchText.MismatchedCaseSubRangeMiddle,
                   FindResults.Null, FindResults.MatchingRangeCaseLess, FindResults.Null, FindResults.MatchingRangeCaseLess, null);
        }

        ///<summary>TextPatternRange.FindText Method.T.5.06.09</summary>
        [TestCaseAttribute("TextPatternRange.FindText Method.T.5.06.09",
            TestSummary = "Call FindText() with variation of backward/forward & case sensistive/in-senstive",
            Priority = TestPriorities.Pri1,
            Status = TestStatus.Works,
            Author = "Microsoft Corp.",
            TestCaseType = TestCaseType.Generic,
            Description = new string[]
            {
                "Pre-Condition: Verify text is expected value: Set text to be RANDOM 256 characters",
                "Pre-Condition: Text to search for is matches sub range at END of doc, w/MISMATCHED case",
                "CallFindText(forwards, case-insensitive)verify result NULL",
                "CallFindText(forwards, case-sensitive)  matching range (case insensitive)",
                "CallFindText(backwards,case-insensitive)verify result NULL",
                "CallFindText(backwards,case-sensitive)  matching range (case insensitive)"
            })]
        [SuppressMessage("Microsoft.Globalization", "CA1303:DoNotPassLiteralsAsLocalizedParameters")]
        public void TestFindTextMethod50609(TestCaseAttribute testCase)
        {
            HeaderComment(testCase);


            FindTextHelper(SampleText.Random256, SearchText.MismatchedCaseSubRangeEnd,
                   FindResults.Null, FindResults.MatchingRangeCaseLess, FindResults.Null, FindResults.MatchingRangeCaseLess, null);
        }

        ///<summary>TextPatternRange.FindText Method.T.5.06.10</summary>
        [TestCaseAttribute("TextPatternRange.FindText Method.T.5.06.10",
            TestSummary = "Call FindText() with variation of backward/forward & case sensistive/in-senstive",
            Priority = TestPriorities.Pri1,
            Status = TestStatus.Works,
            Author = "Microsoft Corp.",
            TestCaseType = TestCaseType.Generic,
            Description = new string[]
            {
                "Pre-Condition: Verify text is expected value: Set text to have duplicate blocks of text",
                "Pre-Condition: Text to search for is matches 1ST instance of repeating block of text",
                "CallFindText(forwards, case-insensitive)verify result MATCHES 1st instance of dup'd text",
                "CallFindText(forwards, case-sensitive)  verify result MATCHES 1st instance of dup'd text",
                "CallFindText(backwards,case-insensitive)verify result MATCHES LAST instance of dup'd text",
                "CallFindText(backwards,case-sensitive)  verify result MATCHES LAST instance of dup'd text"
            })]
        [SuppressMessage("Microsoft.Globalization", "CA1303:DoNotPassLiteralsAsLocalizedParameters")]
        public void TestFindTextMethod50610(TestCaseAttribute testCase)
        {
            HeaderComment(testCase);


            FindTextHelper(SampleText.DuplicateBlocksOfText, SearchText.MatchesFirstBlock,
                   FindResults.MatchesFirst, FindResults.MatchesFirst, FindResults.MatchesLast, FindResults.MatchesLast, null);
        }

        ///<summary>TextPatternRange.FindText Method.T.5.06.11</summary>
        [TestCaseAttribute("TextPatternRange.FindText Method.T.5.06.11",
            TestSummary = "Call FindText() with variation of backward/forward & case sensistive/in-senstive",
            Priority = TestPriorities.Pri1,
            Status = TestStatus.Works,
            Author = "Microsoft Corp.",
            TestCaseType = TestCaseType.Generic,
            Description = new string[]
            {
                "Pre-Condition: Verify text is expected value: Set text to have duplicate blocks of text",
                "Pre-Condition: Text to search for is matches 1ST instance of repeating block of text, w/MISMATCHED case",
                "CallFindText(forwards, case-insensitive)verify result NULL",
                "CallFindText(forwards, case-sensitive)  Matches first range and is caseless",
                "CallFindText(backwards,case-insensitive)verify result NULL",
                "CallFindText(backwards,case-sensitive)  Matches last range and is caseless"
            })]
        [SuppressMessage("Microsoft.Globalization", "CA1303:DoNotPassLiteralsAsLocalizedParameters")]
        public void TestFindTextMethod50611(TestCaseAttribute testCase)
        {
            HeaderComment(testCase);


            FindTextHelper(SampleText.DuplicateBlocksOfText, SearchText.MismatchedCaseFirstBlock,
                   FindResults.Null, FindResults.MatchingFirstCaseLess, FindResults.Null, FindResults.MatchingLastCaseLess, null);
        }

        ///<summary>TextPatternRange.FindText Method.T.5.06.12</summary>
        [TestCaseAttribute("TextPatternRange.FindText Method.T.5.06.12",
            TestSummary = "Call FindText() with variation of backward/forward & case sensistive/in-senstive",
            Priority = TestPriorities.Pri1,
            Status = TestStatus.Works,
            Author = "Microsoft Corp.",
            TestCaseType = TestCaseType.Generic,
            Description = new string[]
            {
                "Pre-Condition: Verify text is expected value: Set text to have duplicate blocks of text",
                "Pre-Condition: Text to search for is matches LAST instance of repeating block of text",
                "CallFindText(forwards, case-insensitive)verify result MATCHES 1st instance of dup'd text",
                "CallFindText(forwards, case-sensitive)  verify result MATCHES 1st instance of dup'd text",
                "CallFindText(backwards,case-insensitive)verify result MATCHES LAST instance of dup'd text",
                "CallFindText(backwards,case-sensitive)  verify result MATCHES LAST instance of dup'd text"
            })]
        [SuppressMessage("Microsoft.Globalization", "CA1303:DoNotPassLiteralsAsLocalizedParameters")]
        public void TestFindTextMethod50612(TestCaseAttribute testCase)
        {
            HeaderComment(testCase);


            FindTextHelper(SampleText.DuplicateBlocksOfText, SearchText.MatchesLastBlock,
                   FindResults.MatchesFirst, FindResults.MatchesFirst, FindResults.MatchesLast, FindResults.MatchesLast, null);
        }

        ///<summary>TextPatternRange.FindText Method.T.5.06.13</summary>
        [TestCaseAttribute("TextPatternRange.FindText Method.T.5.06.13",
            TestSummary = "Call FindText() with variation of backward/forward & case sensistive/in-senstive",
            Priority = TestPriorities.Pri1,
            Status = TestStatus.Works,
            Author = "Microsoft Corp.",
            TestCaseType = TestCaseType.Generic,
            Description = new string[]
            {
                "Pre-Condition: Verify text is expected value: Set text to have duplicate blocks of text",
                "Pre-Condition: Text to search for is matches LAST instance of repeating block of text, w/MISMATCHED case",
                "CallFindText(forwards, case-insensitive)verify result NULL",
                "CallFindText(forwards, case-sensitive)  verify result MATCHES 1st instance of dup'd text",
                "CallFindText(backwards,case-insensitive)verify result NULL",
                "CallFindText(backwards,case-sensitive)  verify result MATCHES LAST instance of dup'd text"
            })]
        [SuppressMessage("Microsoft.Globalization", "CA1303:DoNotPassLiteralsAsLocalizedParameters")]
        public void TestFindTextMethod50613(TestCaseAttribute testCase)
        {
            HeaderComment(testCase);


            FindTextHelper(SampleText.DuplicateBlocksOfText, SearchText.MismatchedCaseLastBlock,
                   FindResults.Null, FindResults.MatchesFirst, FindResults.Null, FindResults.MatchesLast, null);
        }

        ///<summary>TextPatternRange.FindText Method.T.5.06.14</summary>
        [TestCaseAttribute("TextPatternRange.FindText Method.T.5.06.14",
            TestSummary = "Call FindText() with variation of backward/forward & case sensistive/in-senstive",
            Priority = TestPriorities.Pri2,
            Status = TestStatus.Works,
            Author = "Microsoft Corp.",
            TestCaseType = TestCaseType.Generic,
            Description = new string[]
            {
                "Pre-Condition: Verify text is expected value: Set text to be RANDOM 256 characters",
                "Pre-Condition: Text to search for is NULL",
                "CallFindText(forwards, case-insensitive) RAISES System.ArgumentNullException exception",
                "CallFindText(forwards, case-sensitive)   RAISES System.ArgumentNullException exception",
                "CallFindText(backwards,case-insensitive) RAISES System.ArgumentNullException exception",
                "CallFindText(backwards,case-sensitive)   RAISES System.ArgumentNullException exception"
            })]
        [SuppressMessage("Microsoft.Globalization", "CA1303:DoNotPassLiteralsAsLocalizedParameters")]
        public void TestFindTextMethod50614(TestCaseAttribute testCase)
        {
            HeaderComment(testCase);

            ArgumentNullException ex = new ArgumentNullException("testCase"); //  Unused, just need an instance for GetType() below

            FindTextHelper(SampleText.Random256, SearchText.Null,
                   FindResults.Exception, FindResults.Exception, FindResults.Exception, FindResults.Exception, ex.GetType());
        }

        ///<summary>TextPatternRange.FindText Method.T.5.06.15</summary>
        [TestCaseAttribute("TextPatternRange.FindText Method.T.5.06.15",
            TestSummary = "Call FindText() with variation of backward/forward & case sensistive/in-senstive",
            Priority = TestPriorities.Pri2,
            Status = TestStatus.Works,
            Author = "Microsoft Corp.",
            TestCaseType = TestCaseType.Generic,
            Description = new string[]
            {
                "Pre-Condition: Verify text is expected value: Set text to be RANDOM 256 characters",
                "Pre-Condition: Text to search for is EMPTY Text",
                "CallFindText(forwards, case-insensitive) RAISES System.ArgumentException exception",
                "CallFindText(forwards, case-sensitive)   RAISES System.ArgumentException exception",
                "CallFindText(backwards,case-insensitive) RAISES System.ArgumentException exception",
                "CallFindText(backwards,case-sensitive)   RAISES System.ArgumentException exception"
            })]
        [SuppressMessage("Microsoft.Globalization", "CA1303:DoNotPassLiteralsAsLocalizedParameters")]
        public void TestFindTextMethod50615(TestCaseAttribute testCase)
        {
            HeaderComment(testCase);

            ArgumentException ex = new ArgumentException("Unused, just need an instance for GetType() below");

            FindTextHelper(SampleText.Random256, SearchText.Empty,
                   FindResults.Exception, FindResults.Exception, FindResults.Exception, FindResults.Exception, ex.GetType());
        }

        #endregion 5.6 TextPatternRange.FindText Method

        #region 5.7 TextPatternRange.GetAttributeValue Method

        ///<summary>TextPatternRange.GetAttributeValue Method.T.5.07.01</summary>
        [TestCaseAttribute("TextPatternRange.GetAttributeValue Method.T.5.07.01",
            TestSummary = "Call GetAttributeValue for randomly selected attribute on empty text",
            Priority = TestPriorities.Pri0,
            Status = TestStatus.Works,
            Author = "Microsoft Corp.",
            TestCaseType = TestCaseType.Generic,
            Description = new string[]
            {
                "Pre-Condition: Verify text is expected value: Set text to 'String1'",
                "Pre-Condition: Create range equal to entire document",
                "Pre-Condition: Identify & use Supported attributes on the control",
                "For each attribute identified, GetAttributeValue() verify result MATCHES expected attribute values"
            })]
        public void TestGetAttributeValueMethod50701(TestCaseAttribute testCase)
        {
            HeaderComment(testCase);


            GetAttributeValueHelper(SampleText.String1, TargetRangeType.DocumentRange, AttributeType.SupportedAttributes, GetResult.MatchingAttributeValue, null);
        }

        ///<summary>TextPatternRange.GetAttributeValue Method.T.5.07.02</summary>
        [TestCaseAttribute("TextPatternRange.GetAttributeValue Method.T.5.07.02",
            TestSummary = "Call GetAttributeValue for randomly selected attribute on empty text",
            Priority = TestPriorities.Pri1,
            Status = TestStatus.Works,
            Author = "Microsoft Corp.",
            TestCaseType = TestCaseType.Generic,
            Description = new string[]
            {
                "Pre-Condition: Verify text is expected value: Verify text is NOT empty (or SET text if it is empty)",
                "Pre-Condition: Create range equal to entire document",
                "Pre-Condition: Identify & use Supported attributes on the control",
                "For each attribute identified, GetAttributeValue() verify result MATCHES expected attribute values"
            })]
        public void TestGetAttributeValueMethod50702(TestCaseAttribute testCase)
        {
            HeaderComment(testCase);


            GetAttributeValueHelper(SampleText.NotEmpty, TargetRangeType.DocumentRange, AttributeType.SupportedAttributes, GetResult.MatchingAttributeValue, null);
        }

        ///<summary>TextPatternRange.GetAttributeValue Method.T.5.07.03</summary>
        [TestCaseAttribute("TextPatternRange.GetAttributeValue Method.T.5.07.03",
            TestSummary = "Call GetAttributeValue for randomly selected attribute on empty text",
            Priority = TestPriorities.Pri1,
            Status = TestStatus.Works,
            Author = "Microsoft Corp.",
            TestCaseType = TestCaseType.Generic,
            Description = new string[]
            {
                "Pre-Condition: Verify text is expected value: Verify text is NOT empty (or SET text if it is empty)",
                "Pre-Condition: Create range equal to EMPTY range at START of document",
                "Pre-Condition: Identify & use Supported attributes on the control",
                "For each attribute identified, GetAttributeValue() verify result MATCHES expected attribute values"
            })]
        public void TestGetAttributeValueMethod50703(TestCaseAttribute testCase)
        {
            HeaderComment(testCase);


            GetAttributeValueHelper(SampleText.NotEmpty, TargetRangeType.EmptyStart, AttributeType.SupportedAttributes, GetResult.MatchingAttributeValue, null);
        }

        ///<summary>TextPatternRange.GetAttributeValue Method.T.5.07.04</summary>
        [TestCaseAttribute("TextPatternRange.GetAttributeValue Method.T.5.07.04",
            TestSummary = "Call GetAttributeValue for randomly selected attribute on empty text",
            Priority = TestPriorities.Pri1,
            Status = TestStatus.Works,
            Author = "Microsoft Corp.",
            TestCaseType = TestCaseType.Generic,
            Description = new string[]
            {
                "Pre-Condition: Verify text is expected value: Verify text is NOT empty (or SET text if it is empty)",
                "Pre-Condition: Create range equal to EMPTY range in MIDDLE of document",
                "Pre-Condition: Identify & use Supported attributes on the control",
                "For each attribute identified, GetAttributeValue() verify result MATCHES expected attribute values"
            })]
        public void TestGetAttributeValueMethod50704(TestCaseAttribute testCase)
        {
            HeaderComment(testCase);


            GetAttributeValueHelper(SampleText.NotEmpty, TargetRangeType.EmptyMiddle, AttributeType.SupportedAttributes, GetResult.MatchingAttributeValue, null);
        }

        ///<summary>TextPatternRange.GetAttributeValue Method.T.5.07.05</summary>
        [TestCaseAttribute("TextPatternRange.GetAttributeValue Method.T.5.07.05",
            TestSummary = "Call GetAttributeValue for randomly selected attribute on empty text",
            Priority = TestPriorities.Pri1,
            Status = TestStatus.Works,
            Author = "Microsoft Corp.",
            TestCaseType = TestCaseType.Generic,
            Description = new string[]
            {
                "Pre-Condition: Verify text is expected value: Verify text is NOT empty (or SET text if it is empty)",
                "Pre-Condition: Create range equal to EMPTY range at END of document",
                "Pre-Condition: Identify & use Supported attributes on the control",
                "For each attribute identified, GetAttributeValue() verify result MATCHES expected attribute values"
            })]
        public void TestGetAttributeValueMethod50705(TestCaseAttribute testCase)
        {
            HeaderComment(testCase);


            GetAttributeValueHelper(SampleText.NotEmpty, TargetRangeType.EmptyEnd, AttributeType.SupportedAttributes, GetResult.MatchingAttributeValue, null);
        }

        ///<summary>TextPatternRange.GetAttributeValue Method.T.5.07.06</summary>
        [TestCaseAttribute("TextPatternRange.GetAttributeValue Method.T.5.07.06",
            TestSummary = "Call GetAttributeValue for randomly selected attribute on empty text",
            Priority = TestPriorities.Pri1,
            Status = TestStatus.Works,
            Author = "Microsoft Corp.",
            TestCaseType = TestCaseType.Generic,
            Description = new string[]
            {
                "Pre-Condition: Verify text is expected value: Set text to EMPTY string",
                "Pre-Condition: Create range equal to EMPTY range at START of document",
                "Pre-Condition: Identify & use Supported attributes on the control",
                "For each attribute identified, GetAttributeValue() verify result MATCHES expected attribute values"
            })]
        public void TestGetAttributeValueMethod50706(TestCaseAttribute testCase)
        {
            HeaderComment(testCase);


            GetAttributeValueHelper(SampleText.Empty, TargetRangeType.EmptyStart, AttributeType.SupportedAttributes, GetResult.MatchingAttributeValue, null);
        }

        ///<summary>TextPatternRange.GetAttributeValue Method.T.5.07.07</summary>
        [TestCaseAttribute("TextPatternRange.GetAttributeValue Method.T.5.07.07",
            TestSummary = "Call GetAttributeValue for randomly selected attribute on empty text",
            Priority = TestPriorities.Pri1,
            Status = TestStatus.Works,
            Author = "Microsoft Corp.",
            TestCaseType = TestCaseType.Generic,
            Description = new string[]
            {
                "Pre-Condition: Verify text is expected value: Verify text is NOT empty (or SET text if it is empty)",
                "Pre-Condition: Create range equal to entire document",
                "Pre-Condition: Identify & use Null value for attribute(s)",
                "For each attribute identified, GetAttributeValue()  RAISES System.ArgumentNullException exception"
            })]
        public void TestGetAttributeValueMethod50707(TestCaseAttribute testCase)
        {
            HeaderComment(testCase);

            ArgumentNullException ex = new ArgumentNullException("testCase"); //  Unused, just need an instance for GetType() below

            GetAttributeValueHelper(SampleText.NotEmpty, TargetRangeType.DocumentRange, AttributeType.Null, GetResult.Exception, ex.GetType());
        }

        #endregion 5.7 TextPatternRange.GetAttributeValue Method

        #region 5.8 TextPatternRange.GetBoundingRectangles Method

        ///<summary>TextPatternRange.GetBoundingRectangles Method.T.5.08.01</summary>
        [TestCaseAttribute("TextPatternRange.GetBoundingRectangles Method.T.5.08.01",
            TestSummary = "Verify bounding rectangle(s) for text",
            Priority = TestPriorities.Pri0,
            Status = TestStatus.Works,
            Author = "Microsoft Corp.",
            TestCaseType = TestCaseType.Generic,
            Description = new string[]
            {
                "Pre-Condition: Control must supports multi-line = false",
                "Pre-Condition: Verify text is expected value: Set text to 'String1'",
                "Pre-Condition: Create calling range equal to entire document",
                "Pre-Condition: Scroll viewport to NOT APPLICABLE: viewport location is not relevant ",
                "Pre-Condition: Get Automation Element Bounding Rectangle",
                "Call GetBoundingRectangles()",
                "Valdiate Bounding Rectangles boundRect(s) are a subset of autoElement boundRect"
            })]
        public void TestGetBoundingRectanglesMethod50801(TestCaseAttribute testCase)
        {
            HeaderComment(testCase);

            GetBoundingRectanglesHelper(SampleText.String1, TargetRangeType.DocumentRange, false, ScrollLocation.NotApplicable, GetBoundRectResult.SubsetOfAutoElementBoundRect);
        }

        ///<summary>TextPatternRange.GetBoundingRectangles Method.T.5.08.02</summary>
        [TestCaseAttribute("TextPatternRange.GetBoundingRectangles Method.T.5.08.02",
            TestSummary = "Verify bounding rectangle(s) for text",
            Priority = TestPriorities.Pri1,
            Status = TestStatus.Works,
            Author = "Microsoft Corp.",
            TestCaseType = TestCaseType.Generic,
            Description = new string[]
            {
                "Pre-Condition: Control must supports multi-line = false",
                "Pre-Condition: Verify text is expected value: Set text to be ONE character",
                "Pre-Condition: Create calling range equal to entire document",
                "Pre-Condition: Scroll viewport to NOT APPLICABLE: viewport location is not relevant ",
                "Pre-Condition: Get Automation Element Bounding Rectangle",
                "Call GetBoundingRectangles()",
                "Valdiate Bounding Rectangles boundRect(s) are a subset of autoElement boundRect"
            })]
        public void TestGetBoundingRectanglesMethod50802(TestCaseAttribute testCase)
        {
            HeaderComment(testCase);

            GetBoundingRectanglesHelper(SampleText.TextIsOneChar, TargetRangeType.DocumentRange, false, ScrollLocation.NotApplicable, GetBoundRectResult.SubsetOfAutoElementBoundRect);
        }

        ///<summary>TextPatternRange.GetBoundingRectangles Method.T.5.08.03</summary>
        [TestCaseAttribute("TextPatternRange.GetBoundingRectangles Method.T.5.08.03",
            TestSummary = "Verify bounding rectangle(s) for text",
            Priority = TestPriorities.Pri1,
            Status = TestStatus.Works,
            Author = "Microsoft Corp.",
            TestCaseType = TestCaseType.Generic,
            Description = new string[]
            {
                "Pre-Condition: Control must supports multi-line = false",
                "Pre-Condition: Verify text is expected value: Set text to be RANDOM 256 characters",
                "Pre-Condition: Create calling range equal to entire document",
                "Pre-Condition: Scroll viewport to NOT APPLICABLE: viewport location is not relevant ",
                "Pre-Condition: Get Automation Element Bounding Rectangle",
                "Call GetBoundingRectangles()",
                "Valdiate Bounding Rectangles boundRect(s) are a subset of autoElement boundRect"
            })]
        public void TestGetBoundingRectanglesMethod50803(TestCaseAttribute testCase)
        {
            HeaderComment(testCase);

            GetBoundingRectanglesHelper(SampleText.Random256, TargetRangeType.DocumentRange, false, ScrollLocation.NotApplicable, GetBoundRectResult.SubsetOfAutoElementBoundRect);
        }

        ///<summary>TextPatternRange.GetBoundingRectangles Method.T.5.08.06</summary>
        [TestCaseAttribute("TextPatternRange.GetBoundingRectangles Method.T.5.08.06",
            TestSummary = "Verify bounding rectangle(s) for text",
            Priority = TestPriorities.Pri1,
            Status = TestStatus.Works,
            Author = "Microsoft Corp.",
            TestCaseType = TestCaseType.Generic,
            Description = new string[]
            {
                "Pre-Condition: Control must supports multi-line = false",
                "Pre-Condition: Verify text is expected value: Set text to EMPTY string",
                "Pre-Condition: Create calling range equal to entire document",
                "Pre-Condition: Scroll viewport to NOT APPLICABLE: viewport location is not relevant ",
                "Pre-Condition: Get Automation Element Bounding Rectangle",
                "Call GetBoundingRectangles()",
                "Valdiate Bounding Rectangles empty array of bounding rectangles"
            })]
        public void TestGetBoundingRectanglesMethod50806(TestCaseAttribute testCase)
        {
            HeaderComment(testCase);

            GetBoundingRectanglesHelper(SampleText.Empty, TargetRangeType.DocumentRange, false, ScrollLocation.NotApplicable, GetBoundRectResult.EmptyArray);
        }

        ///<summary>TextPatternRange.GetBoundingRectangles Method.T.5.08.07</summary>
        [TestCaseAttribute("TextPatternRange.GetBoundingRectangles Method.T.5.08.07",
            TestSummary = "Verify bounding rectangle(s) for text",
            Priority = TestPriorities.Pri1,
            Status = TestStatus.Works,
            Author = "Microsoft Corp.",
            TestCaseType = TestCaseType.Generic,
            Description = new string[]
            {
                "Pre-Condition: Control must supports multi-line = true",
                "Pre-Condition: Verify text is expected value: Set text to EMPTY string",
                "Pre-Condition: Create calling range equal to entire document",
                "Pre-Condition: Scroll viewport to NOT APPLICABLE: viewport location is not relevant ",
                "Pre-Condition: Get Automation Element Bounding Rectangle",
                "Call GetBoundingRectangles()",
                "Valdiate Bounding Rectangles empty array of bounding rectangles"
            })]
        public void TestGetBoundingRectanglesMethod50807(TestCaseAttribute testCase)
        {
            HeaderComment(testCase);

            GetBoundingRectanglesHelper(SampleText.Empty, TargetRangeType.DocumentRange, true, ScrollLocation.NotApplicable, GetBoundRectResult.EmptyArray);
        }

        ///<summary>TextPatternRange.GetBoundingRectangles Method.T.5.08.08</summary>
        [TestCaseAttribute("TextPatternRange.GetBoundingRectangles Method.T.5.08.08",
            TestSummary = "Verify bounding rectangle(s) for text",
            Priority = TestPriorities.Pri1,
            Status = TestStatus.Works,
            Author = "Microsoft Corp.",
            TestCaseType = TestCaseType.Generic,
            Description = new string[]
            {
                "Pre-Condition: Control must supports multi-line = false",
                "Pre-Condition: Verify text is expected value: Set text to be RANDOM 256 characters",
                "Pre-Condition: Create calling range equal to entire document",
                "Pre-Condition: Scroll viewport to LEFT-TOP of viewport ",
                "Pre-Condition: Get Automation Element Bounding Rectangle",
                "Call GetBoundingRectangles()",
                "Valdiate Bounding Rectangles boundRect(s) are a subset of autoElement boundRect"
            })]
        public void TestGetBoundingRectanglesMethod50808(TestCaseAttribute testCase)
        {
            HeaderComment(testCase);

            GetBoundingRectanglesHelper(SampleText.Random256, TargetRangeType.DocumentRange, false, ScrollLocation.LeftTop, GetBoundRectResult.SubsetOfAutoElementBoundRect);
        }

        ///<summary>TextPatternRange.GetBoundingRectangles Method.T.5.08.09</summary>
        [TestCaseAttribute("TextPatternRange.GetBoundingRectangles Method.T.5.08.09",
            TestSummary = "Verify bounding rectangle(s) for text",
            Priority = TestPriorities.Pri1,
            Status = TestStatus.Works,
            ProblemDescription = "Bug(s): 1370948--CtrlTest(1096)",
            Author = "Microsoft Corp.",
            TestCaseType = TestCaseType.Generic,
            Description = new string[]
            {
                "Pre-Condition: Control must supports multi-line = true",
                "Pre-Condition: Verify text is expected value: Set text to One\nTwo\nThree\n...\nNine",
                "Pre-Condition: Create calling range equal to entire document",
                "Pre-Condition: Scroll viewport to LEFT-TOP of viewport ",
                "Pre-Condition: Get Automation Element Bounding Rectangle",
                "Call GetBoundingRectangles()",
                "Valdiate Bounding Rectangles boundRect(s) are a subset of autoElement boundRect"
            })]
        public void TestGetBoundingRectanglesMethod50809(TestCaseAttribute testCase)
        {
            HeaderComment(testCase);

            GetBoundingRectanglesHelper(SampleText.OneCRTwoCRThreeCR, TargetRangeType.DocumentRange, true, ScrollLocation.LeftTop, GetBoundRectResult.SubsetOfAutoElementBoundRect);
        }

        ///<summary>TextPatternRange.GetBoundingRectangles Method.T.5.08.11</summary>
        [TestCaseAttribute("TextPatternRange.GetBoundingRectangles Method.T.5.08.11",
            TestSummary = "Verify bounding rectangle(s) for text",
            Priority = TestPriorities.Pri1,
            Status = TestStatus.Works,
            Author = "Microsoft Corp.",
            TestCaseType = TestCaseType.Generic,
            Description = new string[]
            {
                "Pre-Condition: Control must supports multi-line = false",
                "Pre-Condition: Verify text is expected value: Set text to be RANDOM 256 characters",
                "Pre-Condition: Create calling range equal to entire document",
                "Pre-Condition: Scroll viewport to RIGHT-TOP of viewport ",
                "Pre-Condition: Get Automation Element Bounding Rectangle",
                "Call GetBoundingRectangles()",
                "Valdiate Bounding Rectangles boundRect(s) are a subset of autoElement boundRect"
            })]
        public void TestGetBoundingRectanglesMethod50811(TestCaseAttribute testCase)
        {
            HeaderComment(testCase);

            GetBoundingRectanglesHelper(SampleText.Random256, TargetRangeType.DocumentRange, false, ScrollLocation.RightTop, GetBoundRectResult.SubsetOfAutoElementBoundRect);
        }

        ///<summary>TextPatternRange.GetBoundingRectangles Method.T.5.08.12</summary>
        [TestCaseAttribute("TextPatternRange.GetBoundingRectangles Method.T.5.08.12",
            TestSummary = "Verify bounding rectangle(s) for text",
            Priority = TestPriorities.Pri1,
            Status = TestStatus.Works,
            Author = "Microsoft Corp.",
            TestCaseType = TestCaseType.Generic,
            Description = new string[]
            {
                "Pre-Condition: Control must supports multi-line = true",
                "Pre-Condition: Verify text is expected value: Set text to One\nTwo\nThree\n...\nNine",
                "Pre-Condition: Create calling range equal to entire document",
                "Pre-Condition: Scroll viewport to RIGHT-TOP of viewport ",
                "Pre-Condition: Get Automation Element Bounding Rectangle",
                "Call GetBoundingRectangles()",
                "Valdiate Bounding Rectangles boundRect(s) are a subset of autoElement boundRect"
            })]
        public void TestGetBoundingRectanglesMethod50812(TestCaseAttribute testCase)
        {
            HeaderComment(testCase);

            GetBoundingRectanglesHelper(SampleText.OneCRTwoCRThreeCR, TargetRangeType.DocumentRange, true, ScrollLocation.RightTop, GetBoundRectResult.SubsetOfAutoElementBoundRect);
        }

        ///<summary>TextPatternRange.GetBoundingRectangles Method.T.5.08.14</summary>
        [TestCaseAttribute("TextPatternRange.GetBoundingRectangles Method.T.5.08.14",
            TestSummary = "Verify bounding rectangle(s) for text",
            Priority = TestPriorities.Pri1,
            Status = TestStatus.Works,
            Author = "Microsoft Corp.",
            TestCaseType = TestCaseType.Generic,
            Description = new string[]
            {
                "Pre-Condition: Control must supports multi-line = false",
                "Pre-Condition: Verify text is expected value: Set text to be RANDOM 256 characters",
                "Pre-Condition: Create calling range equal to entire document",
                "Pre-Condition: Scroll viewport to LEFT-CENTER of viewport ",
                "Pre-Condition: Get Automation Element Bounding Rectangle",
                "Call GetBoundingRectangles()",
                "Valdiate Bounding Rectangles boundRect(s) are a subset of autoElement boundRect"
            })]
        public void TestGetBoundingRectanglesMethod50814(TestCaseAttribute testCase)
        {
            HeaderComment(testCase);

            GetBoundingRectanglesHelper(SampleText.Random256, TargetRangeType.DocumentRange, false, ScrollLocation.LeftCenter, GetBoundRectResult.SubsetOfAutoElementBoundRect);
        }

        ///<summary>TextPatternRange.GetBoundingRectangles Method.T.5.08.15</summary>
        [TestCaseAttribute("TextPatternRange.GetBoundingRectangles Method.T.5.08.15",
            TestSummary = "Verify bounding rectangle(s) for text",
            Priority = TestPriorities.Pri1,
            Status = TestStatus.Works,
            Author = "Microsoft Corp.",
            TestCaseType = TestCaseType.Generic,
            Description = new string[]
            {
                "Pre-Condition: Control must supports multi-line = true",
                "Pre-Condition: Verify text is expected value: Set text to One\nTwo\nThree\n...\nNine",
                "Pre-Condition: Create calling range equal to entire document",
                "Pre-Condition: Scroll viewport to LEFT-CENTER of viewport ",
                "Pre-Condition: Get Automation Element Bounding Rectangle",
                "Call GetBoundingRectangles()",
                "Valdiate Bounding Rectangles boundRect(s) are a subset of autoElement boundRect"
            })]
        public void TestGetBoundingRectanglesMethod50815(TestCaseAttribute testCase)
        {
            HeaderComment(testCase);

            GetBoundingRectanglesHelper(SampleText.OneCRTwoCRThreeCR, TargetRangeType.DocumentRange, true, ScrollLocation.LeftCenter, GetBoundRectResult.SubsetOfAutoElementBoundRect);
        }

        ///<summary>TextPatternRange.GetBoundingRectangles Method.T.5.08.16</summary>
        [TestCaseAttribute("TextPatternRange.GetBoundingRectangles Method.T.5.08.16",
            TestSummary = "Verify bounding rectangle(s) for text",
            Priority = TestPriorities.Pri1,
            Status = TestStatus.Works,
            Author = "Microsoft Corp.",
            TestCaseType = TestCaseType.Generic,
            Description = new string[]
            {
                "Pre-Condition: Control must supports multi-line = true",
                "Pre-Condition: Verify text is expected value: Set text to span MULTIPLE LINES",
                "Pre-Condition: Create calling range equal to entire document",
                "Pre-Condition: Scroll viewport to LEFT-CENTER of viewport ",
                "Pre-Condition: Get Automation Element Bounding Rectangle",
                "Call GetBoundingRectangles()",
                "Valdiate Bounding Rectangles boundRect(s) are a subset of autoElement boundRect"
            })]
        public void TestGetBoundingRectanglesMethod50816(TestCaseAttribute testCase)
        {
            HeaderComment(testCase);

            GetBoundingRectanglesHelper(SampleText.MultipleLines, TargetRangeType.DocumentRange, true, ScrollLocation.LeftCenter, GetBoundRectResult.SubsetOfAutoElementBoundRect);
        }

        ///<summary>TextPatternRange.GetBoundingRectangles Method.T.5.08.17</summary>
        [TestCaseAttribute("TextPatternRange.GetBoundingRectangles Method.T.5.08.17",
            TestSummary = "Verify bounding rectangle(s) for text",
            Priority = TestPriorities.Pri1,
            Status = TestStatus.Works,
            Author = "Microsoft Corp.",
            TestCaseType = TestCaseType.Generic,
            Description = new string[]
            {
                "Pre-Condition: Control must supports multi-line = false",
                "Pre-Condition: Verify text is expected value: Set text to be RANDOM 256 characters",
                "Pre-Condition: Create calling range equal to entire document",
                "Pre-Condition: Scroll viewport to MIDDLE (horiz & vert) of viewport ",
                "Pre-Condition: Get Automation Element Bounding Rectangle",
                "Call GetBoundingRectangles()",
                "Valdiate Bounding Rectangles boundRect(s) are a subset of autoElement boundRect"
            })]
        public void TestGetBoundingRectanglesMethod50817(TestCaseAttribute testCase)
        {
            HeaderComment(testCase);

            GetBoundingRectanglesHelper(SampleText.Random256, TargetRangeType.DocumentRange, false, ScrollLocation.Center, GetBoundRectResult.SubsetOfAutoElementBoundRect);
        }

        ///<summary>TextPatternRange.GetBoundingRectangles Method.T.5.08.18</summary>
        [TestCaseAttribute("TextPatternRange.GetBoundingRectangles Method.T.5.08.18",
            TestSummary = "Verify bounding rectangle(s) for text",
            Priority = TestPriorities.Pri1,
            Status = TestStatus.Works,
            Author = "Microsoft Corp.",
            TestCaseType = TestCaseType.Generic,
            Description = new string[]
            {
                "Pre-Condition: Control must supports multi-line = true",
                "Pre-Condition: Verify text is expected value: Set text to One\nTwo\nThree\n...\nNine",
                "Pre-Condition: Create calling range equal to entire document",
                "Pre-Condition: Scroll viewport to MIDDLE (horiz & vert) of viewport ",
                "Pre-Condition: Get Automation Element Bounding Rectangle",
                "Call GetBoundingRectangles()",
                "Valdiate Bounding Rectangles boundRect(s) are a subset of autoElement boundRect"
            })]
        public void TestGetBoundingRectanglesMethod50818(TestCaseAttribute testCase)
        {
            HeaderComment(testCase);

            GetBoundingRectanglesHelper(SampleText.OneCRTwoCRThreeCR, TargetRangeType.DocumentRange, true, ScrollLocation.Center, GetBoundRectResult.SubsetOfAutoElementBoundRect);
        }

        ///<summary>TextPatternRange.GetBoundingRectangles Method.T.5.08.19</summary>
        [TestCaseAttribute("TextPatternRange.GetBoundingRectangles Method.T.5.08.19",
            TestSummary = "Verify bounding rectangle(s) for text",
            Priority = TestPriorities.Pri1,
            Status = TestStatus.Works,
            Author = "Microsoft Corp.",
            TestCaseType = TestCaseType.Generic,
            Description = new string[]
            {
                "Pre-Condition: Control must supports multi-line = true",
                "Pre-Condition: Verify text is expected value: Set text to span MULTIPLE LINES",
                "Pre-Condition: Create calling range equal to entire document",
                "Pre-Condition: Scroll viewport to MIDDLE (horiz & vert) of viewport ",
                "Pre-Condition: Get Automation Element Bounding Rectangle",
                "Call GetBoundingRectangles()",
                "Valdiate Bounding Rectangles boundRect(s) are a subset of autoElement boundRect"
            })]
        public void TestGetBoundingRectanglesMethod50819(TestCaseAttribute testCase)
        {
            HeaderComment(testCase);

            GetBoundingRectanglesHelper(SampleText.MultipleLines, TargetRangeType.DocumentRange, true, ScrollLocation.Center, GetBoundRectResult.SubsetOfAutoElementBoundRect);
        }

        ///<summary>TextPatternRange.GetBoundingRectangles Method.T.5.08.20</summary>
        [TestCaseAttribute("TextPatternRange.GetBoundingRectangles Method.T.5.08.20",
            TestSummary = "Verify bounding rectangle(s) for text",
            Priority = TestPriorities.Pri1,
            Status = TestStatus.Works,
            Author = "Microsoft Corp.",
            TestCaseType = TestCaseType.Generic,
            Description = new string[]
            {
                "Pre-Condition: Control must supports multi-line = false",
                "Pre-Condition: Verify text is expected value: Set text to be RANDOM 256 characters",
                "Pre-Condition: Create calling range equal to entire document",
                "Pre-Condition: Scroll viewport to RIGHT-MIDDLE of viewport ",
                "Pre-Condition: Get Automation Element Bounding Rectangle",
                "Call GetBoundingRectangles()",
                "Valdiate Bounding Rectangles boundRect(s) are a subset of autoElement boundRect"
            })]
        public void TestGetBoundingRectanglesMethod50820(TestCaseAttribute testCase)
        {
            HeaderComment(testCase);

            GetBoundingRectanglesHelper(SampleText.Random256, TargetRangeType.DocumentRange, false, ScrollLocation.RightCenter, GetBoundRectResult.SubsetOfAutoElementBoundRect);
        }

        ///<summary>TextPatternRange.GetBoundingRectangles Method.T.5.08.21</summary>
        [TestCaseAttribute("TextPatternRange.GetBoundingRectangles Method.T.5.08.21",
            TestSummary = "Verify bounding rectangle(s) for text",
            Priority = TestPriorities.Pri1,
            Status = TestStatus.Works,
            Author = "Microsoft Corp.",
            TestCaseType = TestCaseType.Generic,
            Description = new string[]
            {
                "Pre-Condition: Control must supports multi-line = true",
                "Pre-Condition: Verify text is expected value: Set text to One\nTwo\nThree\n...\nNine",
                "Pre-Condition: Create calling range equal to entire document",
                "Pre-Condition: Scroll viewport to RIGHT-MIDDLE of viewport ",
                "Pre-Condition: Get Automation Element Bounding Rectangle",
                "Call GetBoundingRectangles()",
                "Valdiate Bounding Rectangles boundRect(s) are a subset of autoElement boundRect"
            })]
        public void TestGetBoundingRectanglesMethod50821(TestCaseAttribute testCase)
        {
            HeaderComment(testCase);

            GetBoundingRectanglesHelper(SampleText.OneCRTwoCRThreeCR, TargetRangeType.DocumentRange, true, ScrollLocation.RightCenter, GetBoundRectResult.SubsetOfAutoElementBoundRect);
        }

        ///<summary>TextPatternRange.GetBoundingRectangles Method.T.5.08.22</summary>
        [TestCaseAttribute("TextPatternRange.GetBoundingRectangles Method.T.5.08.22",
            TestSummary = "Verify bounding rectangle(s) for text",
            Priority = TestPriorities.Pri1,
            Status = TestStatus.Works,
            Author = "Microsoft Corp.",
            TestCaseType = TestCaseType.Generic,
            Description = new string[]
            {
                "Pre-Condition: Control must supports multi-line = true",
                "Pre-Condition: Verify text is expected value: Set text to span MULTIPLE LINES",
                "Pre-Condition: Create calling range equal to entire document",
                "Pre-Condition: Scroll viewport to RIGHT-MIDDLE of viewport ",
                "Pre-Condition: Get Automation Element Bounding Rectangle",
                "Call GetBoundingRectangles()",
                "Valdiate Bounding Rectangles boundRect(s) are a subset of autoElement boundRect"
            })]
        public void TestGetBoundingRectanglesMethod50822(TestCaseAttribute testCase)
        {
            HeaderComment(testCase);

            GetBoundingRectanglesHelper(SampleText.MultipleLines, TargetRangeType.DocumentRange, true, ScrollLocation.RightCenter, GetBoundRectResult.SubsetOfAutoElementBoundRect);
        }

        ///<summary>TextPatternRange.GetBoundingRectangles Method.T.5.08.23</summary>
        [TestCaseAttribute("TextPatternRange.GetBoundingRectangles Method.T.5.08.23",
            TestSummary = "Verify bounding rectangle(s) for text",
            Priority = TestPriorities.Pri1,
            Status = TestStatus.Works,
            Author = "Microsoft Corp.",
            TestCaseType = TestCaseType.Generic,
            Description = new string[]
            {
                "Pre-Condition: Control must supports multi-line = false",
                "Pre-Condition: Verify text is expected value: Set text to be RANDOM 256 characters",
                "Pre-Condition: Create calling range equal to entire document",
                "Pre-Condition: Scroll viewport to LEFT-BOTTOM of viewport ",
                "Pre-Condition: Get Automation Element Bounding Rectangle",
                "Call GetBoundingRectangles()",
                "Valdiate Bounding Rectangles boundRect(s) are a subset of autoElement boundRect"
            })]
        public void TestGetBoundingRectanglesMethod50823(TestCaseAttribute testCase)
        {
            HeaderComment(testCase);

            GetBoundingRectanglesHelper(SampleText.Random256, TargetRangeType.DocumentRange, false, ScrollLocation.LeftBottom, GetBoundRectResult.SubsetOfAutoElementBoundRect);
        }

        ///<summary>TextPatternRange.GetBoundingRectangles Method.T.5.08.24</summary>
        [TestCaseAttribute("TextPatternRange.GetBoundingRectangles Method.T.5.08.24",
            TestSummary = "Verify bounding rectangle(s) for text",
            Priority = TestPriorities.Pri1,
            Status = TestStatus.Works,
            Author = "Microsoft Corp.",
            TestCaseType = TestCaseType.Generic,
            Description = new string[]
            {
                "Pre-Condition: Control must supports multi-line = true",
                "Pre-Condition: Verify text is expected value: Set text to One\nTwo\nThree\n...\nNine",
                "Pre-Condition: Create calling range equal to entire document",
                "Pre-Condition: Scroll viewport to LEFT-BOTTOM of viewport ",
                "Pre-Condition: Get Automation Element Bounding Rectangle",
                "Call GetBoundingRectangles()",
                "Valdiate Bounding Rectangles boundRect(s) are a subset of autoElement boundRect"
            })]
        public void TestGetBoundingRectanglesMethod50824(TestCaseAttribute testCase)
        {
            HeaderComment(testCase);

            GetBoundingRectanglesHelper(SampleText.OneCRTwoCRThreeCR, TargetRangeType.DocumentRange, true, ScrollLocation.LeftBottom, GetBoundRectResult.SubsetOfAutoElementBoundRect);
        }

        ///<summary>TextPatternRange.GetBoundingRectangles Method.T.5.08.25</summary>
        [TestCaseAttribute("TextPatternRange.GetBoundingRectangles Method.T.5.08.25",
            TestSummary = "Verify bounding rectangle(s) for text",
            Priority = TestPriorities.Pri1,
            Status = TestStatus.Works,
            Author = "Microsoft Corp.",
            TestCaseType = TestCaseType.Generic,
            Description = new string[]
            {
                "Pre-Condition: Control must supports multi-line = true",
                "Pre-Condition: Verify text is expected value: Set text to span MULTIPLE LINES",
                "Pre-Condition: Create calling range equal to entire document",
                "Pre-Condition: Scroll viewport to LEFT-BOTTOM of viewport ",
                "Pre-Condition: Get Automation Element Bounding Rectangle",
                "Call GetBoundingRectangles()",
                "Valdiate Bounding Rectangles boundRect(s) are a subset of autoElement boundRect"
            })]
        public void TestGetBoundingRectanglesMethod50825(TestCaseAttribute testCase)
        {
            HeaderComment(testCase);

            GetBoundingRectanglesHelper(SampleText.MultipleLines, TargetRangeType.DocumentRange, true, ScrollLocation.LeftBottom, GetBoundRectResult.SubsetOfAutoElementBoundRect);
        }

        ///<summary>TextPatternRange.GetBoundingRectangles Method.T.5.08.26</summary>
        [TestCaseAttribute("TextPatternRange.GetBoundingRectangles Method.T.5.08.26",
            TestSummary = "Verify bounding rectangle(s) for text",
            Priority = TestPriorities.Pri1,
            Status = TestStatus.Works,
            Author = "Microsoft Corp.",
            TestCaseType = TestCaseType.Generic,
            Description = new string[]
            {
                "Pre-Condition: Control must supports multi-line = false",
                "Pre-Condition: Verify text is expected value: Set text to be RANDOM 256 characters",
                "Pre-Condition: Create calling range equal to entire document",
                "Pre-Condition: Scroll viewport to RIGHT-BOTTOM of viewport ",
                "Pre-Condition: Get Automation Element Bounding Rectangle",
                "Call GetBoundingRectangles()",
                "Valdiate Bounding Rectangles boundRect(s) are a subset of autoElement boundRect"
            })]
        public void TestGetBoundingRectanglesMethod50826(TestCaseAttribute testCase)
        {
            HeaderComment(testCase);

            GetBoundingRectanglesHelper(SampleText.Random256, TargetRangeType.DocumentRange, false, ScrollLocation.RightBottom, GetBoundRectResult.SubsetOfAutoElementBoundRect);
        }

        ///<summary>TextPatternRange.GetBoundingRectangles Method.T.5.08.27</summary>
        [TestCaseAttribute("TextPatternRange.GetBoundingRectangles Method.T.5.08.27",
            TestSummary = "Verify bounding rectangle(s) for text",
            Priority = TestPriorities.Pri1,
            Status = TestStatus.Works,
            Author = "Microsoft Corp.",
            TestCaseType = TestCaseType.Generic,
            Description = new string[]
            {
                "Pre-Condition: Control must supports multi-line = true",
                "Pre-Condition: Verify text is expected value: Set text to One\nTwo\nThree\n...\nNine",
                "Pre-Condition: Create calling range equal to entire document",
                "Pre-Condition: Scroll viewport to RIGHT-BOTTOM of viewport ",
                "Pre-Condition: Get Automation Element Bounding Rectangle",
                "Call GetBoundingRectangles()",
                "Valdiate Bounding Rectangles boundRect(s) are a subset of autoElement boundRect"
            })]
        public void TestGetBoundingRectanglesMethod50827(TestCaseAttribute testCase)
        {
            HeaderComment(testCase);

            GetBoundingRectanglesHelper(SampleText.OneCRTwoCRThreeCR, TargetRangeType.DocumentRange, true, ScrollLocation.RightBottom, GetBoundRectResult.SubsetOfAutoElementBoundRect);
        }

        ///<summary>TextPatternRange.GetBoundingRectangles Method.T.5.08.28</summary>
        [TestCaseAttribute("TextPatternRange.GetBoundingRectangles Method.T.5.08.28",
            TestSummary = "Verify bounding rectangle(s) for text",
            Priority = TestPriorities.Pri1,
            Status = TestStatus.Works,
            Author = "Microsoft Corp.",
            TestCaseType = TestCaseType.Generic,
            Description = new string[]
            {
                "Pre-Condition: Control must supports multi-line = true",
                "Pre-Condition: Verify text is expected value: Set text to span MULTIPLE LINES",
                "Pre-Condition: Create calling range equal to entire document",
                "Pre-Condition: Scroll viewport to RIGHT-BOTTOM of viewport ",
                "Pre-Condition: Get Automation Element Bounding Rectangle",
                "Call GetBoundingRectangles()",
                "Valdiate Bounding Rectangles boundRect(s) are a subset of autoElement boundRect"
            })]
        public void TestGetBoundingRectanglesMethod50828(TestCaseAttribute testCase)
        {
            HeaderComment(testCase);

            GetBoundingRectanglesHelper(SampleText.MultipleLines, TargetRangeType.DocumentRange, true, ScrollLocation.RightBottom, GetBoundRectResult.SubsetOfAutoElementBoundRect);
        }

        ///<summary>TextPatternRange.GetBoundingRectangles Method.T.5.08.30</summary>
        [TestCaseAttribute("TextPatternRange.GetBoundingRectangles Method.T.5.08.30",
            TestSummary = "Verify bounding rectangle(s) for text",
            Priority = TestPriorities.Pri1,
            Status = TestStatus.Works,
            Author = "Microsoft Corp.",
            TestCaseType = TestCaseType.Generic,
            Description = new string[]
            {
                "Pre-Condition: Control must supports multi-line = true",
                "Pre-Condition: Verify text is expected value: Set text to complex multi-line text with long line of text at start",
                "Pre-Condition: Create calling range equal to entire document",
                "Pre-Condition: Scroll viewport to RIGHT-BOTTOM of viewport ",
                "Pre-Condition: Get Automation Element Bounding Rectangle",
                "Call GetBoundingRectangles()",
                "Valdiate Bounding Rectangles empty array of bounding rectangles"
            })]
        public void TestGetBoundingRectanglesMethod50830(TestCaseAttribute testCase)
        {
            HeaderComment(testCase);

            GetBoundingRectanglesHelper(SampleText.Complex2, TargetRangeType.DocumentRange, true, ScrollLocation.RightBottom, GetBoundRectResult.EmptyArray);
        }

        #endregion 5.8 TextPatternRange.GetBoundingRectangles Method

        #region 5.9 TextPatternRange.GetText Method

        ///<summary>TextPatternRange.GetText Method.T.5.09.01</summary>
        [TestCaseAttribute("TextPatternRange.GetText Method.T.5.09.01",
            TestSummary = "Calls GetText on range",
            Priority = TestPriorities.Pri0,
            Status = TestStatus.Works,
            Author = "Microsoft Corp.",
            TestCaseType = TestCaseType.Generic,
            Description = new string[]
            {
                "Pre-Condition: Verify text is expected value: Set text to 'String1'",
                "Pre-Condition: Create range equal to entire document",
                "Pre-Condition: Determine length of text to get = actual length of calling range",
                "Call GetText(actual length of calling range) without errors",
                "Verify text is verify result matches ENTIRE TextPattern document"
            })]
        [SuppressMessage("Microsoft.Usage", "CA2208:InstantiateArgumentExceptionsCorrectly")]
        public void TestGetTextMethod50901(TestCaseAttribute testCase)
        {
            HeaderComment(testCase);


            GetTextHelper(SampleText.String1, TargetRangeType.DocumentRange, MaxLength.Length, GetResult.DocumentRange, null);
        }

        ///<summary>TextPatternRange.GetText Method.T.5.09.02</summary>
        [TestCaseAttribute("TextPatternRange.GetText Method.T.5.09.02",
            TestSummary = "Calls GetText on range",
            Priority = TestPriorities.Pri1,
            Status = TestStatus.Works,
            Author = "Microsoft Corp.",
            TestCaseType = TestCaseType.Generic,
            Description = new string[]
            {
                "Pre-Condition: Verify text is expected value: Verify text is NOT empty (or SET text if it is empty)",
                "Pre-Condition: Create range equal to entire document",
                "Pre-Condition: Determine length of text to get = actual length of calling range",
                "Call GetText(actual length of calling range) without errors",
                "Verify text is verify result matches ENTIRE TextPattern document"
            })]
        [SuppressMessage("Microsoft.Usage", "CA2208:InstantiateArgumentExceptionsCorrectly")]
        public void TestGetTextMethod50902(TestCaseAttribute testCase)
        {
            HeaderComment(testCase);


            GetTextHelper(SampleText.NotEmpty, TargetRangeType.DocumentRange, MaxLength.Length, GetResult.DocumentRange, null);
        }

        ///<summary>TextPatternRange.GetText Method.T.5.09.03</summary>
        [TestCaseAttribute("TextPatternRange.GetText Method.T.5.09.03",
            TestSummary = "Calls GetText on range",
            Priority = TestPriorities.Pri1,
            Status = TestStatus.Works,
            Author = "Microsoft Corp.",
            TestCaseType = TestCaseType.Generic,
            Description = new string[]
            {
                "Pre-Condition: Verify text is expected value: Verify text is NOT empty (or SET text if it is empty)",
                "Pre-Condition: Create range equal to RANDOM range at START of document",
                "Pre-Condition: Determine length of text to get = RANDOM size <= actual size",
                "Call GetText(RANDOM size <= actual size) without errors",
                "Verify text is verify result matches LENGTH of calling range"
            })]
        [SuppressMessage("Microsoft.Usage", "CA2208:InstantiateArgumentExceptionsCorrectly")]
        public void TestGetTextMethod50903(TestCaseAttribute testCase)
        {
            HeaderComment(testCase);


            GetTextHelper(SampleText.NotEmpty, TargetRangeType.RandomStart, MaxLength.RandomWithinValidSize, GetResult.CallingRangeLength, null);
        }

        ///<summary>TextPatternRange.GetText Method.T.5.09.04</summary>
        [TestCaseAttribute("TextPatternRange.GetText Method.T.5.09.04",
            TestSummary = "Calls GetText on range",
            Priority = TestPriorities.Pri1,
            Status = TestStatus.Works,
            Author = "Microsoft Corp.",
            TestCaseType = TestCaseType.Generic,
            Description = new string[]
            {
                "Pre-Condition: Verify text is expected value: Verify text is NOT empty (or SET text if it is empty)",
                "Pre-Condition: Create range equal to RANDOM range in MIDDLE of document",
                "Pre-Condition: Determine length of text to get = RANDOM size <= actual size",
                "Call GetText(RANDOM size <= actual size) without errors",
                "Verify text is verify result matches LENGTH of calling range"
            })]
        [SuppressMessage("Microsoft.Usage", "CA2208:InstantiateArgumentExceptionsCorrectly")]
        public void TestGetTextMethod50904(TestCaseAttribute testCase)
        {
            HeaderComment(testCase);


            GetTextHelper(SampleText.NotEmpty, TargetRangeType.RandomMiddle, MaxLength.RandomWithinValidSize, GetResult.CallingRangeLength, null);
        }

        ///<summary>TextPatternRange.GetText Method.T.5.09.05</summary>
        [TestCaseAttribute("TextPatternRange.GetText Method.T.5.09.05",
            TestSummary = "Calls GetText on range",
            Priority = TestPriorities.Pri1,
            Status = TestStatus.Works,
            ProblemDescription = "Bug(s): 1348727--(richedit*)",
            Author = "Microsoft Corp.",
            TestCaseType = TestCaseType.Generic,
            Description = new string[]
            {
                "Pre-Condition: Verify text is expected value: Verify text is NOT empty (or SET text if it is empty)",
                "Pre-Condition: Create range equal to RANDOM range at END of document",
                "Pre-Condition: Determine length of text to get = RANDOM size <= actual size",
                "Call GetText(RANDOM size <= actual size) without errors",
                "Verify text is verify result matches LENGTH of calling range"
            })]
        [SuppressMessage("Microsoft.Usage", "CA2208:InstantiateArgumentExceptionsCorrectly")]
        public void TestGetTextMethod50905(TestCaseAttribute testCase)
        {
            HeaderComment(testCase);


            GetTextHelper(SampleText.NotEmpty, TargetRangeType.RandomEnd, MaxLength.RandomWithinValidSize, GetResult.CallingRangeLength, null);
        }

        ///<summary>TextPatternRange.GetText Method.T.5.09.06</summary>
        [TestCaseAttribute("TextPatternRange.GetText Method.T.5.09.06",
            TestSummary = "Calls GetText on range",
            Priority = TestPriorities.Pri1,
            Status = TestStatus.Works,
            Author = "Microsoft Corp.",
            TestCaseType = TestCaseType.Generic,
            Description = new string[]
            {
                "Pre-Condition: Verify text is expected value: Verify text is NOT empty (or SET text if it is empty)",
                "Pre-Condition: Create range equal to entire document",
                "Pre-Condition: Determine length of text to get = RANDOM size <= actual size",
                "Call GetText(RANDOM size <= actual size) without errors",
                "Verify text is verify result matches LENGTH of calling range"
            })]
        [SuppressMessage("Microsoft.Usage", "CA2208:InstantiateArgumentExceptionsCorrectly")]
        public void TestGetTextMethod50906(TestCaseAttribute testCase)
        {
            HeaderComment(testCase);


            GetTextHelper(SampleText.NotEmpty, TargetRangeType.DocumentRange, MaxLength.RandomWithinValidSize, GetResult.CallingRangeLength, null);
        }

        ///<summary>TextPatternRange.GetText Method.T.5.09.07</summary>
        [TestCaseAttribute("TextPatternRange.GetText Method.T.5.09.07",
            TestSummary = "Calls GetText on range",
            Priority = TestPriorities.Pri1,
            Status = TestStatus.Works,
            Author = "Microsoft Corp.",
            TestCaseType = TestCaseType.Generic,
            Description = new string[]
            {
                "Pre-Condition: Verify text is expected value: Verify text is NOT empty (or SET text if it is empty)",
                "Pre-Condition: Create range equal to entire document",
                "Pre-Condition: Determine length of text to get = -1 (all) for calling range",
                "Call GetText(-1 (all) for calling range) without errors",
                "Verify text is verify result matches ENTIRE TextPattern document"
            })]
        [SuppressMessage("Microsoft.Usage", "CA2208:InstantiateArgumentExceptionsCorrectly")]
        public void TestGetTextMethod50907(TestCaseAttribute testCase)
        {
            HeaderComment(testCase);


            GetTextHelper(SampleText.NotEmpty, TargetRangeType.DocumentRange, MaxLength.All, GetResult.DocumentRange, null);
        }

        ///<summary>TextPatternRange.GetText Method.T.5.09.08</summary>
        [TestCaseAttribute("TextPatternRange.GetText Method.T.5.09.08",
            TestSummary = "Calls GetText on range",
            Priority = TestPriorities.Pri1,
            Status = TestStatus.Works,
            Author = "Microsoft Corp.",
            TestCaseType = TestCaseType.Generic,
            Description = new string[]
            {
                "Pre-Condition: Verify text is expected value: Verify text is NOT empty (or SET text if it is empty)",
                "Pre-Condition: Create range equal to entire document",
                "Pre-Condition: Determine length of text to get = 0 for text length",
                "Call GetText(0 for text length) without errors",
                "Verify text is verify result is EMPTY range"
            })]
        [SuppressMessage("Microsoft.Usage", "CA2208:InstantiateArgumentExceptionsCorrectly")]
        public void TestGetTextMethod50908(TestCaseAttribute testCase)
        {
            HeaderComment(testCase);


            GetTextHelper(SampleText.NotEmpty, TargetRangeType.DocumentRange, MaxLength.Zero, GetResult.Empty, null);
        }

        ///<summary>TextPatternRange.GetText Method.T.5.09.09</summary>
        [TestCaseAttribute("TextPatternRange.GetText Method.T.5.09.09",
            TestSummary = "Calls GetText on range",
            Priority = TestPriorities.Pri1,
            Status = TestStatus.Works,
            Author = "Microsoft Corp.",
            TestCaseType = TestCaseType.Generic,
            Description = new string[]
            {
                "Pre-Condition: Verify text is expected value: Verify text is NOT empty (or SET text if it is empty)",
                "Pre-Condition: Create range equal to entire document",
                "Pre-Condition: Determine length of text to get = -2 (error) for text length)",
                "Call GetText(-2 (error) for text length)) RAISES System.ArgumentOutOfRangeException exception",
                "Verify text is  RAISES System.ArgumentOutOfRangeException exception"
            })]
        [SuppressMessage("Microsoft.Usage", "CA2208:InstantiateArgumentExceptionsCorrectly")]
        public void TestGetTextMethod50909(TestCaseAttribute testCase)
        {
            HeaderComment(testCase);

            ArgumentOutOfRangeException ex = new ArgumentOutOfRangeException("Unused, just need an instance for GetType() below");

            GetTextHelper(SampleText.NotEmpty, TargetRangeType.DocumentRange, MaxLength.MinusTwo, GetResult.Exception, ex.GetType());
        }

        ///<summary>TextPatternRange.GetText Method.T.5.09.10</summary>
        [TestCaseAttribute("TextPatternRange.GetText Method.T.5.09.10",
            TestSummary = "Calls GetText on range",
            Priority = TestPriorities.Pri2,
            Status = TestStatus.Works,
            Author = "Microsoft Corp.",
            TestCaseType = TestCaseType.Generic,
            Description = new string[]
            {
                "Pre-Condition: Verify text is expected value: Verify text is NOT empty (or SET text if it is empty)",
                "Pre-Condition: Create range equal to entire document",
                "Pre-Condition: Determine length of text to get = max Integer value",
                "Call GetText(max Integer value) without errors",
                "Verify text is verify result matches ENTIRE TextPattern document"
            })]
        [SuppressMessage("Microsoft.Usage", "CA2208:InstantiateArgumentExceptionsCorrectly")]
        public void TestGetTextMethod50910(TestCaseAttribute testCase)
        {
            HeaderComment(testCase);


            GetTextHelper(SampleText.NotEmpty, TargetRangeType.DocumentRange, MaxLength.MaxInt, GetResult.DocumentRange, null);
        }

        ///<summary>TextPatternRange.GetText Method.T.5.09.11</summary>
        [TestCaseAttribute("TextPatternRange.GetText Method.T.5.09.11",
            TestSummary = "Calls GetText on range",
            Priority = TestPriorities.Pri2,
            Status = TestStatus.Works,
            Author = "Microsoft Corp.",
            TestCaseType = TestCaseType.Generic,
            Description = new string[]
            {
                "Pre-Condition: Verify text is expected value: Verify text is NOT empty (or SET text if it is empty)",
                "Pre-Condition: Create range equal to entire document",
                "Pre-Condition: Determine length of text to get = negative max Integer value",
                "Call GetText(negative max Integer value) RAISES System.ArgumentOutOfRangeException exception",
                "Verify text is  RAISES System.ArgumentOutOfRangeException exception"
            })]
        [SuppressMessage("Microsoft.Usage", "CA2208:InstantiateArgumentExceptionsCorrectly")]
        public void TestGetTextMethod50911(TestCaseAttribute testCase)
        {
            HeaderComment(testCase);

            ArgumentOutOfRangeException ex = new ArgumentOutOfRangeException("Unused, just need an instance for GetType() below");

            GetTextHelper(SampleText.NotEmpty, TargetRangeType.DocumentRange, MaxLength.NegativeMaxInt, GetResult.Exception, ex.GetType());
        }

        ///<summary>TextPatternRange.GetText Method.T.5.09.12</summary>
        [TestCaseAttribute("TextPatternRange.GetText Method.T.5.09.12",
            TestSummary = "Calls GetText on range",
            Priority = TestPriorities.Pri2,
            Status = TestStatus.Works,
            Author = "Microsoft Corp.",
            TestCaseType = TestCaseType.Generic,
            Description = new string[]
            {
                "Pre-Condition: Verify text is expected value: Set text to EMPTY string",
                "Pre-Condition: Create range equal to EMPTY range at START of document",
                "Pre-Condition: Determine length of text to get = -1 (all) for calling range",
                "Call GetText(-1 (all) for calling range) without errors",
                "Verify text is verify result is EMPTY range"
            })]
        [SuppressMessage("Microsoft.Usage", "CA2208:InstantiateArgumentExceptionsCorrectly")]
        public void TestGetTextMethod50912(TestCaseAttribute testCase)
        {
            HeaderComment(testCase);


            GetTextHelper(SampleText.Empty, TargetRangeType.EmptyStart, MaxLength.All, GetResult.Empty, null);
        }

        ///<summary>TextPatternRange.GetText Method.T.5.09.13</summary>
        [TestCaseAttribute("TextPatternRange.GetText Method.T.5.09.13",
            TestSummary = "Calls GetText on range",
            Priority = TestPriorities.Pri2,
            Status = TestStatus.Works,
            Author = "Microsoft Corp.",
            TestCaseType = TestCaseType.Generic,
            Description = new string[]
            {
                "Pre-Condition: Verify text is expected value: Set text to EMPTY string",
                "Pre-Condition: Create range equal to EMPTY range at START of document",
                "Pre-Condition: Determine length of text to get = 0 for text length",
                "Call GetText(0 for text length) without errors",
                "Verify text is verify result is EMPTY range"
            })]
        [SuppressMessage("Microsoft.Usage", "CA2208:InstantiateArgumentExceptionsCorrectly")]
        public void TestGetTextMethod50913(TestCaseAttribute testCase)
        {
            HeaderComment(testCase);


            GetTextHelper(SampleText.Empty, TargetRangeType.EmptyStart, MaxLength.Zero, GetResult.Empty, null);
        }

        ///<summary>TextPatternRange.GetText Method.T.5.09.14</summary>
        [TestCaseAttribute("TextPatternRange.GetText Method.T.5.09.14",
            TestSummary = "Calls GetText on range",
            Priority = TestPriorities.Pri2,
            Status = TestStatus.Works,
            Author = "Microsoft Corp.",
            TestCaseType = TestCaseType.Generic,
            Description = new string[]
            {
                "Pre-Condition: Verify text is expected value: Verify text is NOT empty (or SET text if it is empty)",
                "Pre-Condition: Create range equal to EMPTY range at START of document",
                "Pre-Condition: Determine length of text to get = 1 (single character) for text length",
                "Call GetText(1 (single character) for text length) without errors",
                "Verify text is verify result is EMPTY range"
            })]
        [SuppressMessage("Microsoft.Usage", "CA2208:InstantiateArgumentExceptionsCorrectly")]
        public void TestGetTextMethod50914(TestCaseAttribute testCase)
        {
            HeaderComment(testCase);


            GetTextHelper(SampleText.NotEmpty, TargetRangeType.EmptyStart, MaxLength.One, GetResult.Empty, null);
        }

        ///<summary>TextPatternRange.GetText Method.T.5.09.15</summary>
        [TestCaseAttribute("TextPatternRange.GetText Method.T.5.09.15",
            TestSummary = "Calls GetText on range",
            Priority = TestPriorities.Pri2,
            Status = TestStatus.Works,
            Author = "Microsoft Corp.",
            TestCaseType = TestCaseType.Generic,
            Description = new string[]
            {
                "Pre-Condition: Verify text is expected value: Verify text is NOT empty (or SET text if it is empty)",
                "Pre-Condition: Create range equal to EMPTY range at START of document",
                "Pre-Condition: Determine length of text to get = -1 (all) for calling range",
                "Call GetText(-1 (all) for calling range) without errors",
                "Verify text is verify result is EMPTY range"
            })]
        [SuppressMessage("Microsoft.Usage", "CA2208:InstantiateArgumentExceptionsCorrectly")]
        public void TestGetTextMethod50915(TestCaseAttribute testCase)
        {
            HeaderComment(testCase);


            GetTextHelper(SampleText.NotEmpty, TargetRangeType.EmptyStart, MaxLength.All, GetResult.Empty, null);
        }

        ///<summary>TextPatternRange.GetText Method.T.5.09.16</summary>
        [TestCaseAttribute("TextPatternRange.GetText Method.T.5.09.16",
            TestSummary = "Calls GetText on range",
            Priority = TestPriorities.Pri2,
            Status = TestStatus.Works,
            Author = "Microsoft Corp.",
            TestCaseType = TestCaseType.Generic,
            Description = new string[]
            {
                "Pre-Condition: Verify text is expected value: Verify text is NOT empty (or SET text if it is empty)",
                "Pre-Condition: Create range equal to EMPTY range at START of document",
                "Pre-Condition: Determine length of text to get = negative max Integer value",
                "Call GetText(negative max Integer value) RAISES System.ArgumentOutOfRangeException exception",
                "Verify text is  RAISES System.ArgumentOutOfRangeException exception"
            })]
        [SuppressMessage("Microsoft.Usage", "CA2208:InstantiateArgumentExceptionsCorrectly")]
        public void TestGetTextMethod50916(TestCaseAttribute testCase)
        {
            HeaderComment(testCase);

            ArgumentOutOfRangeException ex = new ArgumentOutOfRangeException("Unused, just need an instance for GetType() below");

            GetTextHelper(SampleText.NotEmpty, TargetRangeType.EmptyStart, MaxLength.NegativeMaxInt, GetResult.Exception, ex.GetType());
        }

        ///<summary>TextPatternRange.GetText Method.T.5.09.17</summary>
        [TestCaseAttribute("TextPatternRange.GetText Method.T.5.09.17",
            TestSummary = "Calls GetText on range",
            Priority = TestPriorities.Pri2,
            Status = TestStatus.Works,
            Author = "Microsoft Corp.",
            TestCaseType = TestCaseType.Generic,
            Description = new string[]
            {
                "Pre-Condition: Verify text is expected value: Verify text is NOT empty (or SET text if it is empty)",
                "Pre-Condition: Create range equal to EMPTY range at START of document",
                "Pre-Condition: Determine length of text to get = max Integer value",
                "Call GetText(max Integer value) without errors",
                "Verify text is verify result is EMPTY range"
            })]
        [SuppressMessage("Microsoft.Usage", "CA2208:InstantiateArgumentExceptionsCorrectly")]
        public void TestGetTextMethod50917(TestCaseAttribute testCase)
        {
            HeaderComment(testCase);


            GetTextHelper(SampleText.NotEmpty, TargetRangeType.EmptyStart, MaxLength.MaxInt, GetResult.Empty, null);
        }

        #endregion 5.9 TextPatternRange.GetText Method

        #region 5.10 TextPatternRange.Move Method

        ///<summary>TextPatternRange.Move Method.T.5.10.01</summary>
        [TestCaseAttribute("TextPatternRange.Move Method.T.5.10.01",
            TestSummary = "Moves a range once for each TextUnit",
            Priority = TestPriorities.Pri1,
            Status = TestStatus.Works,
            ProblemDescription = "Bug(s): 1345450--Edit",
            Author = "Microsoft Corp.",
            TestCaseType = TestCaseType.Generic,
            Description = new string[]
            {
                "Pre-Condition: Verify text is expected value: Set text to 'String1'",
                "Pre-Condition: Acquire range for entire document",
                "Pre-Condition: Create calling range equal to entire document",
                "Pre-Condition: Determine supported TextUnits for this control",
                "Pre-Condition: Determine Count for each TextUnit in document",
                "Call Move(One) for each TextUnit, validating result is 0"
            })]
        public void TestMoveMethod51001(TestCaseAttribute testCase)
        {
            HeaderComment(testCase);

            MoveHelper(SampleText.String1, TargetRangeType.DocumentRange, Count.One, Count.Zero);
        }

        ///<summary>TextPatternRange.Move Method.T.5.10.02</summary>
        [TestCaseAttribute("TextPatternRange.Move Method.T.5.10.02",
            TestSummary = "Moves a range once for each TextUnit",
            Priority = TestPriorities.Pri1,
            Status = TestStatus.Works,
            ProblemDescription = "Bug(s): 1369775--RichEdit TextUnit.*, no Format",
            Author = "Microsoft Corp.",
            TestCaseType = TestCaseType.Generic,
            Description = new string[]
            {
                "Pre-Condition: Verify text is expected value: Set text to span MULTIPLE LINES",
                "Pre-Condition: Acquire range for entire document",
                "Pre-Condition: Create calling range equal to entire document",
                "Pre-Condition: Determine supported TextUnits for this control",
                "Pre-Condition: Determine Count for each TextUnit in document",
                "Call Move(Int32.MinValue) for each TextUnit, validating result is 0"
            })]
        public void TestMoveMethod51002(TestCaseAttribute testCase)
        {
            HeaderComment(testCase);

            MoveHelper(SampleText.MultipleLines, TargetRangeType.DocumentRange, Count.MinInt, Count.Zero);
        }

        ///<summary>TextPatternRange.Move Method.T.5.10.03</summary>
        [TestCaseAttribute("TextPatternRange.Move Method.T.5.10.03",
            TestSummary = "Moves a range once for each TextUnit",
            Priority = TestPriorities.Pri1,
            Status = TestStatus.Works,
            Author = "Microsoft Corp.",
            TestCaseType = TestCaseType.Generic,
            Description = new string[]
            {
                "Pre-Condition: Verify text is expected value: Set text to span MULTIPLE LINES",
                "Pre-Condition: Acquire range for entire document",
                "Pre-Condition: Create calling range equal to EMPTY range at START of document",
                "Pre-Condition: Determine supported TextUnits for this control",
                "Pre-Condition: Determine Count for each TextUnit in document",
                "Call Move(Int32.MinValue) for each TextUnit, validating result is 0"
            })]
        public void TestMoveMethod51003(TestCaseAttribute testCase)
        {
            HeaderComment(testCase);

            MoveHelper(SampleText.MultipleLines, TargetRangeType.EmptyStart, Count.MinInt, Count.Zero);
        }

        ///<summary>TextPatternRange.Move Method.T.5.10.05</summary>
        [TestCaseAttribute("TextPatternRange.Move Method.T.5.10.05",
            TestSummary = "Moves a range once for each TextUnit",
            Priority = TestPriorities.Pri1,
            Status = TestStatus.Works,
            ProblemDescription = "Bug(s): 1369775--RichEdit TextUnit.*, no Format",
            Author = "Microsoft Corp.",
            TestCaseType = TestCaseType.Generic,
            Description = new string[]
            {
                "Pre-Condition: Verify text is expected value: Set text to span MULTIPLE LINES",
                "Pre-Condition: Acquire range for entire document",
                "Pre-Condition: Create calling range equal to entire document",
                "Pre-Condition: Determine supported TextUnits for this control",
                "Pre-Condition: Determine Count for each TextUnit in document",
                "Call Move(((# of text units in range) + 1) * -1) for each TextUnit, validating result is 0"
            })]
        public void TestMoveMethod51005(TestCaseAttribute testCase)
        {
            HeaderComment(testCase);

            MoveHelper(SampleText.MultipleLines, TargetRangeType.DocumentRange, Count.NegativeNPlusOne, Count.Zero);
        }

        ///<summary>TextPatternRange.Move Method.T.5.10.06</summary>
        [TestCaseAttribute("TextPatternRange.Move Method.T.5.10.06",
            TestSummary = "Moves a range once for each TextUnit",
            Priority = TestPriorities.Pri1,
            Status = TestStatus.Works,
            Author = "Microsoft Corp.",
            TestCaseType = TestCaseType.Generic,
            Description = new string[]
            {
                "Pre-Condition: Verify text is expected value: Set text to span MULTIPLE LINES",
                "Pre-Condition: Acquire range for entire document",
                "Pre-Condition: Create calling range equal to EMPTY range at START of document",
                "Pre-Condition: Determine supported TextUnits for this control",
                "Pre-Condition: Determine Count for each TextUnit in document",
                "Call Move(((# of text units in range) + 1) * -1) for each TextUnit, validating result is 0"
            })]
        public void TestMoveMethod51006(TestCaseAttribute testCase)
        {
            HeaderComment(testCase);

            MoveHelper(SampleText.MultipleLines, TargetRangeType.EmptyStart, Count.NegativeNPlusOne, Count.Zero);
        }

        ///<summary>TextPatternRange.Move Method.T.5.10.08</summary>
        [TestCaseAttribute("TextPatternRange.Move Method.T.5.10.08",
            TestSummary = "Moves a range once for each TextUnit",
            Priority = TestPriorities.Pri1,
            Status = TestStatus.Works,
            ProblemDescription = "Bug(s): 1369775--RichEdit TextUnit.*, no Format",
            Author = "Microsoft Corp.",
            TestCaseType = TestCaseType.Generic,
            Description = new string[]
            {
                "Pre-Condition: Verify text is expected value: Set text to span MULTIPLE LINES",
                "Pre-Condition: Acquire range for entire document",
                "Pre-Condition: Create calling range equal to entire document",
                "Pre-Condition: Determine supported TextUnits for this control",
                "Pre-Condition: Determine Count for each TextUnit in document",
                "Call Move((# of text units in range) * -1) for each TextUnit, validating result is 0"
            })]
        public void TestMoveMethod51008(TestCaseAttribute testCase)
        {
            HeaderComment(testCase);

            MoveHelper(SampleText.MultipleLines, TargetRangeType.DocumentRange, Count.NegativeN, Count.Zero);
        }

        ///<summary>TextPatternRange.Move Method.T.5.10.09</summary>
        [TestCaseAttribute("TextPatternRange.Move Method.T.5.10.09",
            TestSummary = "Moves a range once for each TextUnit",
            Priority = TestPriorities.Pri1,
            Status = TestStatus.Works,
            Author = "Microsoft Corp.",
            TestCaseType = TestCaseType.Generic,
            Description = new string[]
            {
                "Pre-Condition: Verify text is expected value: Set text to span MULTIPLE LINES",
                "Pre-Condition: Acquire range for entire document",
                "Pre-Condition: Create calling range equal to EMPTY range at START of document",
                "Pre-Condition: Determine supported TextUnits for this control",
                "Pre-Condition: Determine Count for each TextUnit in document",
                "Call Move((# of text units in range) * -1) for each TextUnit, validating result is 0"
            })]
        public void TestMoveMethod51009(TestCaseAttribute testCase)
        {
            HeaderComment(testCase);

            MoveHelper(SampleText.MultipleLines, TargetRangeType.EmptyStart, Count.NegativeN, Count.Zero);
        }

        ///<summary>TextPatternRange.Move Method.T.5.10.11</summary>
        [TestCaseAttribute("TextPatternRange.Move Method.T.5.10.11",
            TestSummary = "Moves a range once for each TextUnit",
            Priority = TestPriorities.Pri1,
            Status = TestStatus.Works,
            ProblemDescription = "Bug(s): 1369775--RichEdit TextUnit.*, no Format",
            Author = "Microsoft Corp.",
            TestCaseType = TestCaseType.Generic,
            Description = new string[]
            {
                "Pre-Condition: Verify text is expected value: Set text to span MULTIPLE LINES",
                "Pre-Condition: Acquire range for entire document",
                "Pre-Condition: Create calling range equal to entire document",
                "Pre-Condition: Determine supported TextUnits for this control",
                "Pre-Condition: Determine Count for each TextUnit in document",
                "Call Move(((# of text units in range) - 1) * -1) for each TextUnit, validating result is 0"
            })]
        public void TestMoveMethod51011(TestCaseAttribute testCase)
        {
            HeaderComment(testCase);

            MoveHelper(SampleText.MultipleLines, TargetRangeType.DocumentRange, Count.NegativeNMinusOne, Count.Zero);
        }

        ///<summary>TextPatternRange.Move Method.T.5.10.12</summary>
        [TestCaseAttribute("TextPatternRange.Move Method.T.5.10.12",
            TestSummary = "Moves a range once for each TextUnit",
            Priority = TestPriorities.Pri1,
            Status = TestStatus.Works,
            Author = "Microsoft Corp.",
            TestCaseType = TestCaseType.Generic,
            Description = new string[]
            {
                "Pre-Condition: Verify text is expected value: Set text to span MULTIPLE LINES",
                "Pre-Condition: Acquire range for entire document",
                "Pre-Condition: Create calling range equal to EMPTY range at START of document",
                "Pre-Condition: Determine supported TextUnits for this control",
                "Pre-Condition: Determine Count for each TextUnit in document",
                "Call Move(((# of text units in range) - 1) * -1) for each TextUnit, validating result is 0"
            })]
        public void TestMoveMethod51012(TestCaseAttribute testCase)
        {
            HeaderComment(testCase);

            MoveHelper(SampleText.MultipleLines, TargetRangeType.EmptyStart, Count.NegativeNMinusOne, Count.Zero);
        }

        ///<summary>TextPatternRange.Move Method.T.5.10.13</summary>
        [TestCaseAttribute("TextPatternRange.Move Method.T.5.10.13",
            TestSummary = "Moves a range once for each TextUnit",
            Priority = TestPriorities.Pri1,
            Status = TestStatus.Works,
            Author = "Microsoft Corp.",
            TestCaseType = TestCaseType.Generic,
            Description = new string[]
            {
                "Pre-Condition: Verify text is expected value: Set text to span MULTIPLE LINES",
                "Pre-Condition: Acquire range for entire document",
                "Pre-Condition: Create calling range equal to EMPTY range at END of document",
                "Pre-Condition: Determine supported TextUnits for this control",
                "Pre-Condition: Determine Count for each TextUnit in document",
                "Call Move(((# of text units in range) - 1) * -1) for each TextUnit, validating result is ((# of text units in range) - 1) * -1"
            })]
        public void TestMoveMethod51013(TestCaseAttribute testCase)
        {
            HeaderComment(testCase);

            MoveHelper(SampleText.MultipleLines, TargetRangeType.EmptyEnd, Count.NegativeNMinusOne, Count.NegativeNMinusOne);
        }

        ///<summary>TextPatternRange.Move Method.T.5.10.14</summary>
        [TestCaseAttribute("TextPatternRange.Move Method.T.5.10.14",
            TestSummary = "Moves a range once for each TextUnit",
            Priority = TestPriorities.Pri1,
            Status = TestStatus.Works,
            ProblemDescription = "Bug(s): 1369775--RichEdit TextUnit.*, no Format",
            Author = "Microsoft Corp.",
            TestCaseType = TestCaseType.Generic,
            Description = new string[]
            {
                "Pre-Condition: Verify text is expected value: Set text to span MULTIPLE LINES",
                "Pre-Condition: Acquire range for entire document",
                "Pre-Condition: Create calling range equal to entire document",
                "Pre-Condition: Determine supported TextUnits for this control",
                "Pre-Condition: Determine Count for each TextUnit in document",
                "Call Move(-(half # of text units in range)) for each TextUnit, validating result is 0"
            })]
        public void TestMoveMethod51014(TestCaseAttribute testCase)
        {
            HeaderComment(testCase);

            MoveHelper(SampleText.MultipleLines, TargetRangeType.DocumentRange, Count.NegativeHalfN, Count.Zero);
        }

        ///<summary>TextPatternRange.Move Method.T.5.10.15</summary>
        [TestCaseAttribute("TextPatternRange.Move Method.T.5.10.15",
            TestSummary = "Moves a range once for each TextUnit",
            Priority = TestPriorities.Pri1,
            Status = TestStatus.Works,
            Author = "Microsoft Corp.",
            TestCaseType = TestCaseType.Generic,
            Description = new string[]
            {
                "Pre-Condition: Verify text is expected value: Set text to span MULTIPLE LINES",
                "Pre-Condition: Acquire range for entire document",
                "Pre-Condition: Create calling range equal to EMPTY range at START of document",
                "Pre-Condition: Determine supported TextUnits for this control",
                "Pre-Condition: Determine Count for each TextUnit in document",
                "Call Move(-(half # of text units in range)) for each TextUnit, validating result is 0"
            })]
        public void TestMoveMethod51015(TestCaseAttribute testCase)
        {
            HeaderComment(testCase);

            MoveHelper(SampleText.MultipleLines, TargetRangeType.EmptyStart, Count.NegativeHalfN, Count.Zero);
        }

        ///<summary>TextPatternRange.Move Method.T.5.10.16</summary>
        [TestCaseAttribute("TextPatternRange.Move Method.T.5.10.16",
            TestSummary = "Moves a range once for each TextUnit",
            Priority = TestPriorities.Pri1,
            Status = TestStatus.Works,
            Author = "Microsoft Corp.",
            TestCaseType = TestCaseType.Generic,
            Description = new string[]
            {
                "Pre-Condition: Verify text is expected value: Set text to span MULTIPLE LINES",
                "Pre-Condition: Acquire range for entire document",
                "Pre-Condition: Create calling range equal to EMPTY range at END of document",
                "Pre-Condition: Determine supported TextUnits for this control",
                "Pre-Condition: Determine Count for each TextUnit in document",
                "Call Move(-(half # of text units in range)) for each TextUnit, validating result is -(half # of text units in range)"
            })]
        public void TestMoveMethod51016(TestCaseAttribute testCase)
        {
            HeaderComment(testCase);

            MoveHelper(SampleText.MultipleLines, TargetRangeType.EmptyEnd, Count.NegativeHalfN, Count.NegativeHalfN);
        }

        ///<summary>TextPatternRange.Move Method.T.5.10.17</summary>
        [TestCaseAttribute("TextPatternRange.Move Method.T.5.10.17",
            TestSummary = "Moves a range once for each TextUnit",
            Priority = TestPriorities.Pri1,
            Status = TestStatus.Works,
            ProblemDescription = "Bug(s): 1369775--RichEdit TextUnit.*, no Format",
            Author = "Microsoft Corp.",
            TestCaseType = TestCaseType.Generic,
            Description = new string[]
            {
                "Pre-Condition: Verify text is expected value: Set text to span MULTIPLE LINES",
                "Pre-Condition: Acquire range for entire document",
                "Pre-Condition: Create calling range equal to entire document",
                "Pre-Condition: Determine supported TextUnits for this control",
                "Pre-Condition: Determine Count for each TextUnit in document",
                "Call Move(-1) for each TextUnit, validating result is 0"
            })]
        public void TestMoveMethod51017(TestCaseAttribute testCase)
        {
            HeaderComment(testCase);

            MoveHelper(SampleText.MultipleLines, TargetRangeType.DocumentRange, Count.NegativeOne, Count.Zero);
        }

        ///<summary>TextPatternRange.Move Method.T.5.10.18</summary>
        [TestCaseAttribute("TextPatternRange.Move Method.T.5.10.18",
            TestSummary = "Moves a range once for each TextUnit",
            Priority = TestPriorities.Pri1,
            Status = TestStatus.Works,
            Author = "Microsoft Corp.",
            TestCaseType = TestCaseType.Generic,
            Description = new string[]
            {
                "Pre-Condition: Verify text is expected value: Set text to span MULTIPLE LINES",
                "Pre-Condition: Acquire range for entire document",
                "Pre-Condition: Create calling range equal to EMPTY range at START of document",
                "Pre-Condition: Determine supported TextUnits for this control",
                "Pre-Condition: Determine Count for each TextUnit in document",
                "Call Move(-1) for each TextUnit, validating result is 0"
            })]
        public void TestMoveMethod51018(TestCaseAttribute testCase)
        {
            HeaderComment(testCase);

            MoveHelper(SampleText.MultipleLines, TargetRangeType.EmptyStart, Count.NegativeOne, Count.Zero);
        }

        ///<summary>TextPatternRange.Move Method.T.5.10.49</summary>
        [TestCaseAttribute("TextPatternRange.Move Method.T.5.10.49",
            TestSummary = "Moves a empty range N+1 times for each TextUnit",
            Priority = TestPriorities.Pri2,
            Status = TestStatus.Works,
            ProblemDescription = "Bug(s): 1370082--CtrlTest(1037/1039/1041)",
            Author = "Microsoft Corp.",
            TestCaseType = TestCaseType.Generic,
            Description = new string[]
            {
                "Pre-Condition: Verify text is expected value: Set text to 1<LF>2<LF>3<LF>...<LF>9",
                "Pre-Condition: Create calling range equal to EMPTY range at END of document",
                "Pre-Condition: Determine supported TextUnits for this control",
                "Pre-Condition: Determine Count for each TextUnit in document",
                "Call Move(-1) (N+1) times where N=# characters in doc, verify result = -1",
                "Call Move(-1) (N+1) times where N=# formats in doc, verify result = -1",
                "Call Move(-1) (N+1) times where N=# words in doc, verify result = -1",
                "Call Move(-1) (N+1) times where N=# lines in doc, verify result = -1",
                "Call Move(-1) (N+1) times where N=# paragraphs in doc, verify result = -1",
                "Call Move(-1) (N+1) times where N=# pages in doc, verify result = -1",
                "Call Move(-1) (N+1) times where N=# document in doc, verify result = -1"
            })]
        public void TestMoveMethod51049(TestCaseAttribute testCase)
        {
            HeaderComment(testCase);

            MoveHelper2(SampleText.Num123CR, TargetRangeType.EmptyEnd, -1, -1);
        }

        #endregion 5.10 TextPatternRange.Move Method

        #region 5.11 TextPatternRange.MoveEndpointByUnit Method

        ///<summary>TextPatternRange.MoveEndpointByUnit Method.T.5.11.01</summary>
        [TestCaseAttribute("TextPatternRange.MoveEndpointByUnit Method.T.5.11.01",
            TestSummary = "Move both start/end points by 1/-1 for each TextUnit enum",
            Priority = TestPriorities.Pri0,
            Status = TestStatus.Works,
            Author = "Microsoft Corp.",
            TestCaseType = TestCaseType.Generic,
            Description = new string[]
            {
                "Pre-Condition: Verify text is expected value: Set text to 'String1'",
                "Pre-Condition: Create calling range equal to entire document",
                "Pre-Condition: Determine supported TextUnits for this control",
                "Verify MoveEndpointByUnit(start,TextUnit.*, 1) returns 1 for all TextUnits",
                "Verify MoveEndpointByUnit(start,TextUnit.*,-1) returns 0 for all TextUnits",
                "Verify MoveEndpointByUnit(end,  TextUnit.*, 1) returns 0 for all TextUnits",
                "Verify MoveEndpointByUnit(end,  TextUnit.*,-1) returns -1 for all TextUnits"
            })]
        public void TestMoveEndpointMethodUnit51101(TestCaseAttribute testCase)
        {
            HeaderComment(testCase);

            MoveEndpointByUnitHelper1(SampleText.String1, TargetRangeType.DocumentRange, 1, 0, 0, -1);
        }

        ///<summary>TextPatternRange.MoveEndpointByUnit Method.T.5.11.02</summary>
        [TestCaseAttribute("TextPatternRange.MoveEndpointByUnit Method.T.5.11.02",
            TestSummary = "Move both start/end points by 1/-1 for each TextUnit enum",
            Priority = TestPriorities.Pri1,
            Status = TestStatus.Works,
            Author = "Microsoft Corp.",
            TestCaseType = TestCaseType.Generic,
            Description = new string[]
            {
                "Pre-Condition: Verify text is expected value: Set text to be RANDOM 256 characters",
                "Pre-Condition: Create calling range equal to entire document",
                "Pre-Condition: Determine supported TextUnits for this control",
                "Verify MoveEndpointByUnit(start,TextUnit.*, 1) returns 1 for all TextUnits",
                "Verify MoveEndpointByUnit(start,TextUnit.*,-1) returns 0 for all TextUnits",
                "Verify MoveEndpointByUnit(end,  TextUnit.*, 1) returns 0 for all TextUnits",
                "Verify MoveEndpointByUnit(end,  TextUnit.*,-1) returns -1 for all TextUnits"
            })]
        public void TestMoveEndpointMethodUnit51102(TestCaseAttribute testCase)
        {
            HeaderComment(testCase);

            MoveEndpointByUnitHelper1(SampleText.Random256, TargetRangeType.DocumentRange, 1, 0, 0, -1);
        }

        ///<summary>TextPatternRange.MoveEndpointByUnit Method.T.5.11.03</summary>
        [TestCaseAttribute("TextPatternRange.MoveEndpointByUnit Method.T.5.11.03",
            TestSummary = "Move both start/end points by 1/-1 for each TextUnit enum",
            Priority = TestPriorities.Pri1,
            Status = TestStatus.Works,
            Author = "Microsoft Corp.",
            TestCaseType = TestCaseType.Generic,
            Description = new string[]
            {
                "Pre-Condition: Verify text is expected value: Set text to be RANDOM 256 characters",
                "Pre-Condition: Create calling range equal to EMPTY range at START of document",
                "Pre-Condition: Determine supported TextUnits for this control",
                "Verify MoveEndpointByUnit(start,TextUnit.*, 1) returns 1 for all TextUnits",
                "Verify MoveEndpointByUnit(start,TextUnit.*,-1) returns 0 for all TextUnits",
                "Verify MoveEndpointByUnit(end,  TextUnit.*, 1) returns 1 for all TextUnits",
                "Verify MoveEndpointByUnit(end,  TextUnit.*,-1) returns 0 for all TextUnits"
            })]
        public void TestMoveEndpointMethodUnit51103(TestCaseAttribute testCase)
        {
            HeaderComment(testCase);

            MoveEndpointByUnitHelper1(SampleText.Random256, TargetRangeType.EmptyStart, 1, 0, 1, 0);
        }

        ///<summary>TextPatternRange.MoveEndpointByUnit Method.T.5.11.04</summary>
        [TestCaseAttribute("TextPatternRange.MoveEndpointByUnit Method.T.5.11.04",
            TestSummary = "Move both start/end points by 1/-1 for each TextUnit enum",
            Priority = TestPriorities.Pri1,
            Status = TestStatus.Works,
            Author = "Microsoft Corp.",
            TestCaseType = TestCaseType.Generic,
            Description = new string[]
            {
                "Pre-Condition: Verify text is expected value: Set text to be RANDOM 256 characters",
                "Pre-Condition: Create calling range equal to EMPTY range in MIDDLE of document",
                "Pre-Condition: Determine supported TextUnits for this control",
                "Verify MoveEndpointByUnit(start,TextUnit.*, 1) returns 1 for all TextUnits",
                "Verify MoveEndpointByUnit(start,TextUnit.*,-1) returns -1 for all TextUnits",
                "Verify MoveEndpointByUnit(end,  TextUnit.*, 1) returns 1 for all TextUnits",
                "Verify MoveEndpointByUnit(end,  TextUnit.*,-1) returns -1 for all TextUnits"
            })]
        public void TestMoveEndpointMethodUnit51104(TestCaseAttribute testCase)
        {
            HeaderComment(testCase);

            MoveEndpointByUnitHelper1(SampleText.Random256, TargetRangeType.EmptyMiddle, 1, -1, 1, -1);
        }

        ///<summary>TextPatternRange.MoveEndpointByUnit Method.T.5.11.05</summary>
        [TestCaseAttribute("TextPatternRange.MoveEndpointByUnit Method.T.5.11.05",
            TestSummary = "Move both start/end points by 1/-1 for each TextUnit enum",
            Priority = TestPriorities.Pri1,
            Status = TestStatus.Works,
            ProblemDescription = "Bug(s): 1364606",
            Author = "Microsoft Corp.",
            TestCaseType = TestCaseType.Generic,
            Description = new string[]
            {
                "Pre-Condition: Verify text is expected value: Set text to be RANDOM 256 characters",
                "Pre-Condition: Create calling range equal to EMPTY range at END of document",
                "Pre-Condition: Determine supported TextUnits for this control",
                "Verify MoveEndpointByUnit(start,TextUnit.*, 1) returns 0 for all TextUnits",
                "Verify MoveEndpointByUnit(start,TextUnit.*,-1) returns -1 for all TextUnits",
                "Verify MoveEndpointByUnit(end,  TextUnit.*, 1) returns 0 for all TextUnits",
                "Verify MoveEndpointByUnit(end,  TextUnit.*,-1) returns -1 for all TextUnits"
            })]
        public void TestMoveEndpointMethodUnit51105(TestCaseAttribute testCase)
        {
            HeaderComment(testCase);

            MoveEndpointByUnitHelper1(SampleText.Random256, TargetRangeType.EmptyEnd, 0, -1, 0, -1);
        }

        ///<summary>TextPatternRange.MoveEndpointByUnit Method.T.5.11.06</summary>
        [TestCaseAttribute("TextPatternRange.MoveEndpointByUnit Method.T.5.11.06",
            TestSummary = "Move both start/end points by 1/-1 for each TextUnit enum",
            Priority = TestPriorities.Pri1,
            Status = TestStatus.Works,
            Author = "Microsoft Corp.",
            TestCaseType = TestCaseType.Generic,
            Description = new string[]
            {
                "Pre-Condition: Verify text is expected value: Set text to be RANDOM 256 characters",
                "Pre-Condition: Create calling range equal to RANDOM range at START of document",
                "Pre-Condition: Determine supported TextUnits for this control",
                "Verify MoveEndpointByUnit(start,TextUnit.*, 1) returns 1 for all TextUnits",
                "Verify MoveEndpointByUnit(start,TextUnit.*,-1) returns 0 for all TextUnits",
                "Verify MoveEndpointByUnit(end,  TextUnit.*, 1) returns 1 for all TextUnits",
                "Verify MoveEndpointByUnit(end,  TextUnit.*,-1) returns -1 for all TextUnits"
            })]
        public void TestMoveEndpointMethodUnit51106(TestCaseAttribute testCase)
        {
            HeaderComment(testCase);

            MoveEndpointByUnitHelper1(SampleText.Random256, TargetRangeType.RandomStart, 1, 0, 1, -1);
        }

        ///<summary>TextPatternRange.MoveEndpointByUnit Method.T.5.11.07</summary>
        [TestCaseAttribute("TextPatternRange.MoveEndpointByUnit Method.T.5.11.07",
            TestSummary = "Move both start/end points by 1/-1 for each TextUnit enum",
            Priority = TestPriorities.Pri1,
            Status = TestStatus.Works,
            Author = "Microsoft Corp.",
            TestCaseType = TestCaseType.Generic,
            Description = new string[]
            {
                "Pre-Condition: Verify text is expected value: Set text to be RANDOM 256 characters",
                "Pre-Condition: Create calling range equal to RANDOM range in MIDDLE of document",
                "Pre-Condition: Determine supported TextUnits for this control",
                "Verify MoveEndpointByUnit(start,TextUnit.*, 1) returns 1 for all TextUnits",
                "Verify MoveEndpointByUnit(start,TextUnit.*,-1) returns -1 for all TextUnits",
                "Verify MoveEndpointByUnit(end,  TextUnit.*, 1) returns 1 for all TextUnits",
                "Verify MoveEndpointByUnit(end,  TextUnit.*,-1) returns -1 for all TextUnits"
            })]
        public void TestMoveEndpointMethodUnit51107(TestCaseAttribute testCase)
        {
            HeaderComment(testCase);

            MoveEndpointByUnitHelper1(SampleText.Random256, TargetRangeType.RandomMiddle, 1, -1, 1, -1);
        }

        ///<summary>TextPatternRange.MoveEndpointByUnit Method.T.5.11.08</summary>
        [TestCaseAttribute("TextPatternRange.MoveEndpointByUnit Method.T.5.11.08",
            TestSummary = "Move both start/end points by 1/-1 for each TextUnit enum",
            Priority = TestPriorities.Pri1,
            Status = TestStatus.Works,
            Author = "Microsoft Corp.",
            TestCaseType = TestCaseType.Generic,
            Description = new string[]
            {
                "Pre-Condition: Verify text is expected value: Set text to be RANDOM 256 characters",
                "Pre-Condition: Create calling range equal to RANDOM range at END of document",
                "Pre-Condition: Determine supported TextUnits for this control",
                "Verify MoveEndpointByUnit(start,TextUnit.*, 1) returns 1 for all TextUnits",
                "Verify MoveEndpointByUnit(start,TextUnit.*,-1) returns -1 for all TextUnits",
                "Verify MoveEndpointByUnit(end,  TextUnit.*, 1) returns 0 for all TextUnits",
                "Verify MoveEndpointByUnit(end,  TextUnit.*,-1) returns -1 for all TextUnits"
            })]
        public void TestMoveEndpointMethodUnit51108(TestCaseAttribute testCase)
        {
            HeaderComment(testCase);

            MoveEndpointByUnitHelper1(SampleText.Random256, TargetRangeType.RandomEnd, 1, -1, 0, -1);
        }

        ///<summary>TextPatternRange.MoveEndpointByUnit Method.T.5.11.09</summary>
        [TestCaseAttribute("TextPatternRange.MoveEndpointByUnit Method.T.5.11.09",
            TestSummary = "Move both start/end points by 1/-1 for each TextUnit enum",
            Priority = TestPriorities.Pri1,
            Status = TestStatus.Works,
            ProblemDescription = "Bug(s): 1364606",
            Author = "Microsoft Corp.",
            TestCaseType = TestCaseType.Generic,
            Description = new string[]
            {
                "Pre-Condition: Verify text is expected value: Set text to EMPTY string",
                "Pre-Condition: Create calling range equal to EMPTY range at START of document",
                "Pre-Condition: Determine supported TextUnits for this control",
                "Verify MoveEndpointByUnit(start,TextUnit.*, 1) returns 0 for all TextUnits",
                "Verify MoveEndpointByUnit(start,TextUnit.*,-1) returns 0 for all TextUnits",
                "Verify MoveEndpointByUnit(end,  TextUnit.*, 1) returns 0 for all TextUnits",
                "Verify MoveEndpointByUnit(end,  TextUnit.*,-1) returns 0 for all TextUnits"
            })]
        public void TestMoveEndpointMethodUnit51109(TestCaseAttribute testCase)
        {
            HeaderComment(testCase);

            MoveEndpointByUnitHelper1(SampleText.Empty, TargetRangeType.EmptyStart, 0, 0, 0, 0);
        }

        ///<summary>TextPatternRange.MoveEndpointByUnit Method.T.5.11.10</summary>
        [TestCaseAttribute("TextPatternRange.MoveEndpointByUnit Method.T.5.11.10",
            TestSummary = "Move both start/end points by 1/-1 for each TextUnit enum",
            Priority = TestPriorities.Pri1,
            Status = TestStatus.Works,
            Author = "Microsoft Corp.",
            TestCaseType = TestCaseType.Generic,
            Description = new string[]
            {
                "Pre-Condition: Verify text is expected value: Set text to span MULTIPLE LINES",
                "Pre-Condition: Create calling range equal to entire document",
                "Pre-Condition: Determine supported TextUnits for this control",
                "Verify MoveEndpointByUnit(start,TextUnit.*, 1) returns 1 for all TextUnits",
                "Verify MoveEndpointByUnit(start,TextUnit.*,-1) returns 0 for all TextUnits",
                "Verify MoveEndpointByUnit(end,  TextUnit.*, 1) returns 0 for all TextUnits",
                "Verify MoveEndpointByUnit(end,  TextUnit.*,-1) returns -1 for all TextUnits"
            })]
        public void TestMoveEndpointMethodUnit51110(TestCaseAttribute testCase)
        {
            HeaderComment(testCase);

            MoveEndpointByUnitHelper1(SampleText.MultipleLines, TargetRangeType.DocumentRange, 1, 0, 0, -1);
        }

        ///<summary>TextPatternRange.MoveEndpointByUnit Method.T.5.11.11</summary>
        [TestCaseAttribute("TextPatternRange.MoveEndpointByUnit Method.T.5.11.11",
            TestSummary = "Move both start/end points by 1/-1 for each TextUnit enum",
            Priority = TestPriorities.Pri1,
            Status = TestStatus.Works,
            Author = "Microsoft Corp.",
            TestCaseType = TestCaseType.Generic,
            Description = new string[]
            {
                "Pre-Condition: Verify text is expected value: Set text to span MULTIPLE LINES",
                "Pre-Condition: Create calling range equal to EMPTY range at START of document",
                "Pre-Condition: Determine supported TextUnits for this control",
                "Verify MoveEndpointByUnit(start,TextUnit.*, 1) returns 1 for all TextUnits",
                "Verify MoveEndpointByUnit(start,TextUnit.*,-1) returns 0 for all TextUnits",
                "Verify MoveEndpointByUnit(end,  TextUnit.*, 1) returns 1 for all TextUnits",
                "Verify MoveEndpointByUnit(end,  TextUnit.*,-1) returns 0 for all TextUnits"
            })]
        public void TestMoveEndpointMethodUnit51111(TestCaseAttribute testCase)
        {
            HeaderComment(testCase);

            MoveEndpointByUnitHelper1(SampleText.MultipleLines, TargetRangeType.EmptyStart, 1, 0, 1, 0);
        }

        ///<summary>TextPatternRange.MoveEndpointByUnit Method.T.5.11.12</summary>
        [TestCaseAttribute("TextPatternRange.MoveEndpointByUnit Method.T.5.11.12",
            TestSummary = "Move both start/end points by 1/-1 for each TextUnit enum",
            Priority = TestPriorities.Pri1,
            Status = TestStatus.Works,
            Author = "Microsoft Corp.",
            TestCaseType = TestCaseType.Generic,
            Description = new string[]
            {
                "Pre-Condition: Verify text is expected value: Set text to span MULTIPLE LINES",
                "Pre-Condition: Create calling range equal to EMPTY range in MIDDLE of document",
                "Pre-Condition: Determine supported TextUnits for this control",
                "Verify MoveEndpointByUnit(start,TextUnit.*, 1) returns 1 for all TextUnits",
                "Verify MoveEndpointByUnit(start,TextUnit.*,-1) returns -1 for all TextUnits",
                "Verify MoveEndpointByUnit(end,  TextUnit.*, 1) returns 1 for all TextUnits",
                "Verify MoveEndpointByUnit(end,  TextUnit.*,-1) returns -1 for all TextUnits"
            })]
        public void TestMoveEndpointMethodUnit51112(TestCaseAttribute testCase)
        {
            HeaderComment(testCase);

            MoveEndpointByUnitHelper1(SampleText.MultipleLines, TargetRangeType.EmptyMiddle, 1, -1, 1, -1);
        }

        ///<summary>TextPatternRange.MoveEndpointByUnit Method.T.5.11.13</summary>
        [TestCaseAttribute("TextPatternRange.MoveEndpointByUnit Method.T.5.11.13",
            TestSummary = "Move both start/end points by 1/-1 for each TextUnit enum",
            Priority = TestPriorities.Pri1,
            Status = TestStatus.Works,
            ProblemDescription = "Bug(s): 1364606",
            Author = "Microsoft Corp.",
            TestCaseType = TestCaseType.Generic,
            Description = new string[]
            {
                "Pre-Condition: Verify text is expected value: Set text to span MULTIPLE LINES",
                "Pre-Condition: Create calling range equal to EMPTY range at END of document",
                "Pre-Condition: Determine supported TextUnits for this control",
                "Verify MoveEndpointByUnit(start,TextUnit.*, 1) returns 0 for all TextUnits",
                "Verify MoveEndpointByUnit(start,TextUnit.*,-1) returns -1 for all TextUnits",
                "Verify MoveEndpointByUnit(end,  TextUnit.*, 1) returns 0 for all TextUnits",
                "Verify MoveEndpointByUnit(end,  TextUnit.*,-1) returns -1 for all TextUnits"
            })]
        public void TestMoveEndpointMethodUnit51113(TestCaseAttribute testCase)
        {
            HeaderComment(testCase);

            MoveEndpointByUnitHelper1(SampleText.MultipleLines, TargetRangeType.EmptyEnd, 0, -1, 0, -1);
        }

        ///<summary>TextPatternRange.MoveEndpointByUnit Method.T.5.11.14</summary>
        [TestCaseAttribute("TextPatternRange.MoveEndpointByUnit Method.T.5.11.14",
            TestSummary = "Move both start/end points by 1/-1 for each TextUnit enum",
            Priority = TestPriorities.Pri1,
            Status = TestStatus.Works,
            Author = "Microsoft Corp.",
            TestCaseType = TestCaseType.Generic,
            Description = new string[]
            {
                "Pre-Condition: Verify text is expected value: Set text to span MULTIPLE LINES",
                "Pre-Condition: Create calling range equal to RANDOM range at START of document",
                "Pre-Condition: Determine supported TextUnits for this control",
                "Verify MoveEndpointByUnit(start,TextUnit.*, 1) returns 1 for all TextUnits",
                "Verify MoveEndpointByUnit(start,TextUnit.*,-1) returns 0 for all TextUnits",
                "Verify MoveEndpointByUnit(end,  TextUnit.*, 1) returns 1 for all TextUnits",
                "Verify MoveEndpointByUnit(end,  TextUnit.*,-1) returns -1 for all TextUnits"
            })]
        public void TestMoveEndpointMethodUnit51114(TestCaseAttribute testCase)
        {
            HeaderComment(testCase);

            MoveEndpointByUnitHelper1(SampleText.MultipleLines, TargetRangeType.RandomStart, 1, 0, 1, -1);
        }

        ///<summary>TextPatternRange.MoveEndpointByUnit Method.T.5.11.15</summary>
        [TestCaseAttribute("TextPatternRange.MoveEndpointByUnit Method.T.5.11.15",
            TestSummary = "Move both start/end points by 1/-1 for each TextUnit enum",
            Priority = TestPriorities.Pri1,
            Status = TestStatus.Works,
            Author = "Microsoft Corp.",
            TestCaseType = TestCaseType.Generic,
            Description = new string[]
            {
                "Pre-Condition: Verify text is expected value: Set text to span MULTIPLE LINES",
                "Pre-Condition: Create calling range equal to RANDOM range in MIDDLE of document",
                "Pre-Condition: Determine supported TextUnits for this control",
                "Verify MoveEndpointByUnit(start,TextUnit.*, 1) returns 1 for all TextUnits",
                "Verify MoveEndpointByUnit(start,TextUnit.*,-1) returns -1 for all TextUnits",
                "Verify MoveEndpointByUnit(end,  TextUnit.*, 1) returns 1 for all TextUnits",
                "Verify MoveEndpointByUnit(end,  TextUnit.*,-1) returns -1 for all TextUnits"
            })]
        public void TestMoveEndpointMethodUnit51115(TestCaseAttribute testCase)
        {
            HeaderComment(testCase);

            MoveEndpointByUnitHelper1(SampleText.MultipleLines, TargetRangeType.RandomMiddle, 1, -1, 1, -1);
        }

        #endregion 5.11 TextPatternRange.MoveEndpointByUnit Method

        #region 5.12 TextPatternRange.MoveEndpointByRange Method

        ///<summary>TextPatternRange.MoveEndpointByRange Method.T.5.12.01</summary>
        [TestCaseAttribute("TextPatternRange.MoveEndpointByRange Method.T.5.12.01",
            TestSummary = "Move the endpoint of an empty range equal to the endpoint of DocumentRange",
            Priority = TestPriorities.Pri0,
            Status = TestStatus.Works,
            Author = "Microsoft Corp.",
            TestCaseType = TestCaseType.Generic,
            Description = new string[]
            {
                "Pre-Condition: Verify text is expected value: Set text to 'String1'",
                "Pre-Condition: Create calling range equal to EMPTY range at START of document",
                "Pre-Condition: Create target range equal to RANDOM range at START of document",
                "Call MoveEndpointByRange(Start,target range,Start) verify result is EMPTY range",
                "Call MoveEndpointByRange(Start,target range,End  ) verify result is EMPTY range",
                "Call MoveEndpointByRange(End,  target range,Start) verify result is EMPTY range",
                "Call MoveEndpointByRange(End,  target range,End  ) verify result is NON-empty range"
            })]
        [SuppressMessage("Microsoft.Globalization", "CA1303:DoNotPassLiteralsAsLocalizedParameters")]
        public void TestMoveEndpointMethodRange51201(TestCaseAttribute testCase)
        {
            HeaderComment(testCase);


            MoveEndpointByRangeHelper1(SampleText.String1, TargetRangeType.EmptyStart, TargetRangeType.RandomStart,
                   MoveEPResults.EmptyRange, MoveEPResults.EmptyRange, MoveEPResults.EmptyRange, MoveEPResults.NonemptyRange,
                   null);
        }

        ///<summary>TextPatternRange.MoveEndpointByRange Method.T.5.12.02</summary>
        [TestCaseAttribute("TextPatternRange.MoveEndpointByRange Method.T.5.12.02",
            TestSummary = "Move the endpoint of an empty range equal to the endpoint of DocumentRange",
            Priority = TestPriorities.Pri1,
            Status = TestStatus.Works,
            Author = "Microsoft Corp.",
            TestCaseType = TestCaseType.Generic,
            Description = new string[]
            {
                "Pre-Condition: Verify text is expected value: Set text to be RANDOM 256 characters",
                "Pre-Condition: Create calling range equal to EMPTY range at START of document",
                "Pre-Condition: Create target range equal to RANDOM range at START of document",
                "Call MoveEndpointByRange(Start,target range,Start) verify result is EMPTY range",
                "Call MoveEndpointByRange(Start,target range,End  ) verify result is EMPTY range",
                "Call MoveEndpointByRange(End,  target range,Start) verify result is EMPTY range",
                "Call MoveEndpointByRange(End,  target range,End  ) verify result is NON-empty range"
            })]
        [SuppressMessage("Microsoft.Globalization", "CA1303:DoNotPassLiteralsAsLocalizedParameters")]
        public void TestMoveEndpointMethodRange51202(TestCaseAttribute testCase)
        {
            HeaderComment(testCase);


            MoveEndpointByRangeHelper1(SampleText.Random256, TargetRangeType.EmptyStart, TargetRangeType.RandomStart,
                   MoveEPResults.EmptyRange, MoveEPResults.EmptyRange, MoveEPResults.EmptyRange, MoveEPResults.NonemptyRange,
                   null);
        }

        ///<summary>TextPatternRange.MoveEndpointByRange Method.T.5.12.03</summary>
        [TestCaseAttribute("TextPatternRange.MoveEndpointByRange Method.T.5.12.03",
            TestSummary = "Move the endpoint of an empty range equal to the endpoint of DocumentRange",
            Priority = TestPriorities.Pri1,
            Status = TestStatus.Works,
            Author = "Microsoft Corp.",
            TestCaseType = TestCaseType.Generic,
            Description = new string[]
            {
                "Pre-Condition: Verify text is expected value: Set text to be RANDOM 256 characters",
                "Pre-Condition: Create calling range equal to EMPTY range in MIDDLE of document",
                "Pre-Condition: Create target range equal to RANDOM range at START of document",
                "Call MoveEndpointByRange(Start,target range,Start) verify result is NON-empty range",
                "Call MoveEndpointByRange(Start,target range,End  ) verify result is NON-empty range",
                "Call MoveEndpointByRange(End,  target range,Start) verify result is EMPTY range",
                "Call MoveEndpointByRange(End,  target range,End  ) verify result is EMPTY range"
            })]
        [SuppressMessage("Microsoft.Globalization", "CA1303:DoNotPassLiteralsAsLocalizedParameters")]
        public void TestMoveEndpointMethodRange51203(TestCaseAttribute testCase)
        {
            HeaderComment(testCase);


            MoveEndpointByRangeHelper1(SampleText.Random256, TargetRangeType.EmptyMiddle, TargetRangeType.RandomStart,
                   MoveEPResults.NonemptyRange, MoveEPResults.NonemptyRange, MoveEPResults.EmptyRange, MoveEPResults.EmptyRange,
                   null);
        }

        ///<summary>TextPatternRange.MoveEndpointByRange Method.T.5.12.04</summary>
        [TestCaseAttribute("TextPatternRange.MoveEndpointByRange Method.T.5.12.04",
            TestSummary = "Move the endpoint of an empty range equal to the endpoint of DocumentRange",
            Priority = TestPriorities.Pri1,
            Status = TestStatus.Works,
            Author = "Microsoft Corp.",
            TestCaseType = TestCaseType.Generic,
            Description = new string[]
            {
                "Pre-Condition: Verify text is expected value: Set text to be RANDOM 256 characters",
                "Pre-Condition: Create calling range equal to EMPTY range at END of document",
                "Pre-Condition: Create target range equal to RANDOM range at START of document",
                "Call MoveEndpointByRange(Start,target range,Start) verify result is NON-empty range",
                "Call MoveEndpointByRange(Start,target range,End  ) verify result is NON-empty range",
                "Call MoveEndpointByRange(End,  target range,Start) verify result is EMPTY range",
                "Call MoveEndpointByRange(End,  target range,End  ) verify result is EMPTY range"
            })]
        [SuppressMessage("Microsoft.Globalization", "CA1303:DoNotPassLiteralsAsLocalizedParameters")]
        public void TestMoveEndpointMethodRange51204(TestCaseAttribute testCase)
        {
            HeaderComment(testCase);


            MoveEndpointByRangeHelper1(SampleText.Random256, TargetRangeType.EmptyEnd, TargetRangeType.RandomStart,
                   MoveEPResults.NonemptyRange, MoveEPResults.NonemptyRange, MoveEPResults.EmptyRange, MoveEPResults.EmptyRange,
                   null);
        }

        ///<summary>TextPatternRange.MoveEndpointByRange Method.T.5.12.05</summary>
        [TestCaseAttribute("TextPatternRange.MoveEndpointByRange Method.T.5.12.05",
            TestSummary = "Move the endpoint of an empty range equal to the endpoint of DocumentRange",
            Priority = TestPriorities.Pri1,
            Status = TestStatus.Works,
            ProblemDescription = "Bug(s): 1364606",
            Author = "Microsoft Corp.",
            TestCaseType = TestCaseType.Generic,
            Description = new string[]
            {
                "Pre-Condition: Verify text is expected value: Set text to be RANDOM 256 characters",
                "Pre-Condition: Create calling range equal to RANDOM range in MIDDLE of document",
                "Pre-Condition: Create target range equal to RANDOM range at START of document",
                "Call MoveEndpointByRange(Start,target range,Start) verify result is NON-empty range",
                "Call MoveEndpointByRange(Start,target range,End  ) verify result is NON-empty range",
                "Call MoveEndpointByRange(End,  target range,Start) verify result is EMPTY range",
                "Call MoveEndpointByRange(End,  target range,End  ) verify result is EMPTY range"
            })]
        [SuppressMessage("Microsoft.Globalization", "CA1303:DoNotPassLiteralsAsLocalizedParameters")]
        public void TestMoveEndpointMethodRange51205(TestCaseAttribute testCase)
        {
            HeaderComment(testCase);


            MoveEndpointByRangeHelper1(SampleText.Random256, TargetRangeType.RandomMiddle, TargetRangeType.RandomStart,
                   MoveEPResults.NonemptyRange, MoveEPResults.NonemptyRange, MoveEPResults.EmptyRange, MoveEPResults.EmptyRange,
                   null);
        }

        ///<summary>TextPatternRange.MoveEndpointByRange Method.T.5.12.06</summary>
        [TestCaseAttribute("TextPatternRange.MoveEndpointByRange Method.T.5.12.06",
            TestSummary = "Move the endpoint of an empty range equal to the endpoint of DocumentRange",
            Priority = TestPriorities.Pri1,
            Status = TestStatus.Works,
            Author = "Microsoft Corp.",
            TestCaseType = TestCaseType.Generic,
            Description = new string[]
            {
                "Pre-Condition: Verify text is expected value: Set text to be RANDOM 256 characters",
                "Pre-Condition: Create calling range equal to entire document",
                "Pre-Condition: Create target range equal to RANDOM range at START of document",
                "Call MoveEndpointByRange(Start,target range,Start) verify result is NON-empty range",
                "Call MoveEndpointByRange(Start,target range,End  ) verify result is NON-empty range",
                "Call MoveEndpointByRange(End,  target range,Start) verify result is EMPTY range",
                "Call MoveEndpointByRange(End,  target range,End  ) verify result is NON-empty range"
            })]
        [SuppressMessage("Microsoft.Globalization", "CA1303:DoNotPassLiteralsAsLocalizedParameters")]
        public void TestMoveEndpointMethodRange51206(TestCaseAttribute testCase)
        {
            HeaderComment(testCase);


            MoveEndpointByRangeHelper1(SampleText.Random256, TargetRangeType.DocumentRange, TargetRangeType.RandomStart,
                   MoveEPResults.NonemptyRange, MoveEPResults.NonemptyRange, MoveEPResults.EmptyRange, MoveEPResults.NonemptyRange,
                   null);
        }

        ///<summary>TextPatternRange.MoveEndpointByRange Method.T.5.12.07</summary>
        [TestCaseAttribute("TextPatternRange.MoveEndpointByRange Method.T.5.12.07",
            TestSummary = "Move the endpoint of an empty range equal to the endpoint of DocumentRange",
            Priority = TestPriorities.Pri1,
            Status = TestStatus.Works,
            Author = "Microsoft Corp.",
            TestCaseType = TestCaseType.Generic,
            Description = new string[]
            {
                "Pre-Condition: Verify text is expected value: Set text to be RANDOM 256 characters",
                "Pre-Condition: Create calling range equal to EMPTY range at START of document",
                "Pre-Condition: Create target range equal to RANDOM range at END of document",
                "Call MoveEndpointByRange(Start,target range,Start) verify result is EMPTY range",
                "Call MoveEndpointByRange(Start,target range,End  ) verify result is EMPTY range",
                "Call MoveEndpointByRange(End,  target range,Start) verify result is NON-empty range",
                "Call MoveEndpointByRange(End,  target range,End  ) verify result is NON-empty range"
            })]
        [SuppressMessage("Microsoft.Globalization", "CA1303:DoNotPassLiteralsAsLocalizedParameters")]
        public void TestMoveEndpointMethodRange51207(TestCaseAttribute testCase)
        {
            HeaderComment(testCase);


            MoveEndpointByRangeHelper1(SampleText.Random256, TargetRangeType.EmptyStart, TargetRangeType.RandomEnd,
                   MoveEPResults.EmptyRange, MoveEPResults.EmptyRange, MoveEPResults.NonemptyRange, MoveEPResults.NonemptyRange,
                   null);
        }

        ///<summary>TextPatternRange.MoveEndpointByRange Method.T.5.12.09</summary>
        [TestCaseAttribute("TextPatternRange.MoveEndpointByRange Method.T.5.12.09",
            TestSummary = "Move the endpoint of an empty range equal to the endpoint of DocumentRange",
            Priority = TestPriorities.Pri1,
            Status = TestStatus.Works,
            Author = "Microsoft Corp.",
            TestCaseType = TestCaseType.Generic,
            Description = new string[]
            {
                "Pre-Condition: Verify text is expected value: Set text to be RANDOM 256 characters",
                "Pre-Condition: Create calling range equal to EMPTY range at END of document",
                "Pre-Condition: Create target range equal to RANDOM range at END of document",
                "Call MoveEndpointByRange(Start,target range,Start) verify result is NON-empty range",
                "Call MoveEndpointByRange(Start,target range,End  ) verify result is EMPTY range",
                "Call MoveEndpointByRange(End,  target range,Start) verify result is EMPTY range",
                "Call MoveEndpointByRange(End,  target range,End  ) verify result is EMPTY range"
            })]
        [SuppressMessage("Microsoft.Globalization", "CA1303:DoNotPassLiteralsAsLocalizedParameters")]
        public void TestMoveEndpointMethodRange51209(TestCaseAttribute testCase)
        {
            HeaderComment(testCase);


            MoveEndpointByRangeHelper1(SampleText.Random256, TargetRangeType.EmptyEnd, TargetRangeType.RandomEnd,
                   MoveEPResults.NonemptyRange, MoveEPResults.EmptyRange, MoveEPResults.EmptyRange, MoveEPResults.EmptyRange,
                   null);
        }

        ///<summary>TextPatternRange.MoveEndpointByRange Method.T.5.12.10</summary>
        [TestCaseAttribute("TextPatternRange.MoveEndpointByRange Method.T.5.12.10",
            TestSummary = "Move the endpoint of an empty range equal to the endpoint of DocumentRange",
            Priority = TestPriorities.Pri1,
            Status = TestStatus.Works,
            Author = "Microsoft Corp.",
            TestCaseType = TestCaseType.Generic,
            Description = new string[]
            {
                "Pre-Condition: Verify text is expected value: Set text to be RANDOM 256 characters",
                "Pre-Condition: Create calling range equal to RANDOM range in MIDDLE of document",
                "Pre-Condition: Create target range equal to RANDOM range at END of document",
                "Call MoveEndpointByRange(Start,target range,Start) verify result is EMPTY range",
                "Call MoveEndpointByRange(Start,target range,End  ) verify result is EMPTY range",
                "Call MoveEndpointByRange(End,  target range,Start) verify result is NON-empty range",
                "Call MoveEndpointByRange(End,  target range,End  ) verify result is NON-empty range"
            })]
        [SuppressMessage("Microsoft.Globalization", "CA1303:DoNotPassLiteralsAsLocalizedParameters")]
        public void TestMoveEndpointMethodRange51210(TestCaseAttribute testCase)
        {
            HeaderComment(testCase);


            MoveEndpointByRangeHelper1(SampleText.Random256, TargetRangeType.RandomMiddle, TargetRangeType.RandomEnd,
                   MoveEPResults.EmptyRange, MoveEPResults.EmptyRange, MoveEPResults.NonemptyRange, MoveEPResults.NonemptyRange,
                   null);
        }

        ///<summary>TextPatternRange.MoveEndpointByRange Method.T.5.12.11</summary>
        [TestCaseAttribute("TextPatternRange.MoveEndpointByRange Method.T.5.12.11",
            TestSummary = "Move the endpoint of an empty range equal to the endpoint of DocumentRange",
            Priority = TestPriorities.Pri1,
            Status = TestStatus.Works,
            Author = "Microsoft Corp.",
            TestCaseType = TestCaseType.Generic,
            Description = new string[]
            {
                "Pre-Condition: Verify text is expected value: Set text to be RANDOM 256 characters",
                "Pre-Condition: Create calling range equal to entire document",
                "Pre-Condition: Create target range equal to RANDOM range at END of document",
                "Call MoveEndpointByRange(Start,target range,Start) verify result is NON-empty range",
                "Call MoveEndpointByRange(Start,target range,End  ) verify result is EMPTY range",
                "Call MoveEndpointByRange(End,  target range,Start) verify result is NON-empty range",
                "Call MoveEndpointByRange(End,  target range,End  ) verify result is NON-empty range"
            })]
        [SuppressMessage("Microsoft.Globalization", "CA1303:DoNotPassLiteralsAsLocalizedParameters")]
        public void TestMoveEndpointMethodRange51211(TestCaseAttribute testCase)
        {
            HeaderComment(testCase);


            MoveEndpointByRangeHelper1(SampleText.Random256, TargetRangeType.DocumentRange, TargetRangeType.RandomEnd,
                   MoveEPResults.NonemptyRange, MoveEPResults.EmptyRange, MoveEPResults.NonemptyRange, MoveEPResults.NonemptyRange,
                   null);
        }

        ///<summary>TextPatternRange.MoveEndpointByRange Method.T.5.12.12</summary>
        [TestCaseAttribute("TextPatternRange.MoveEndpointByRange Method.T.5.12.12",
            TestSummary = "Move the endpoint of an empty range equal to the endpoint of DocumentRange",
            Priority = TestPriorities.Pri1,
            Status = TestStatus.Works,
            Author = "Microsoft Corp.",
            TestCaseType = TestCaseType.Generic,
            Description = new string[]
            {
                "Pre-Condition: Verify text is expected value: Set text to be RANDOM 256 characters",
                "Pre-Condition: Create calling range equal to EMPTY range at START of document",
                "Pre-Condition: Create target range equal to EMPTY range at START of document",
                "Call MoveEndpointByRange(Start,target range,Start) verify result is EMPTY range",
                "Call MoveEndpointByRange(Start,target range,End  ) verify result is EMPTY range",
                "Call MoveEndpointByRange(End,  target range,Start) verify result is EMPTY range",
                "Call MoveEndpointByRange(End,  target range,End  ) verify result is EMPTY range"
            })]
        [SuppressMessage("Microsoft.Globalization", "CA1303:DoNotPassLiteralsAsLocalizedParameters")]
        public void TestMoveEndpointMethodRange51212(TestCaseAttribute testCase)
        {
            HeaderComment(testCase);


            MoveEndpointByRangeHelper1(SampleText.Random256, TargetRangeType.EmptyStart, TargetRangeType.EmptyStart,
                   MoveEPResults.EmptyRange, MoveEPResults.EmptyRange, MoveEPResults.EmptyRange, MoveEPResults.EmptyRange,
                   null);
        }

        ///<summary>TextPatternRange.MoveEndpointByRange Method.T.5.12.13</summary>
        [TestCaseAttribute("TextPatternRange.MoveEndpointByRange Method.T.5.12.13",
            TestSummary = "Move the endpoint of an empty range equal to the endpoint of DocumentRange",
            Priority = TestPriorities.Pri1,
            Status = TestStatus.Works,
            Author = "Microsoft Corp.",
            TestCaseType = TestCaseType.Generic,
            Description = new string[]
            {
                "Pre-Condition: Verify text is expected value: Set text to be RANDOM 256 characters",
                "Pre-Condition: Create calling range equal to entire document",
                "Pre-Condition: Create target range equal to same as calling range",
                "Call MoveEndpointByRange(Start,target range,Start) verify result is NON-empty range",
                "Call MoveEndpointByRange(Start,target range,End  ) verify result is EMPTY range",
                "Call MoveEndpointByRange(End,  target range,Start) verify result is EMPTY range",
                "Call MoveEndpointByRange(End,  target range,End  ) verify result is NON-empty range"
            })]
        [SuppressMessage("Microsoft.Globalization", "CA1303:DoNotPassLiteralsAsLocalizedParameters")]
        public void TestMoveEndpointMethodRange51213(TestCaseAttribute testCase)
        {
            HeaderComment(testCase);


            MoveEndpointByRangeHelper1(SampleText.Random256, TargetRangeType.DocumentRange, TargetRangeType.SameAsCaller,
                   MoveEPResults.NonemptyRange, MoveEPResults.EmptyRange, MoveEPResults.EmptyRange, MoveEPResults.NonemptyRange,
                   null);
        }

        ///<summary>TextPatternRange.MoveEndpointByRange Method.T.5.12.14</summary>
        [TestCaseAttribute("TextPatternRange.MoveEndpointByRange Method.T.5.12.14",
            TestSummary = "Move the endpoint of an empty range equal to the endpoint of DocumentRange",
            Priority = TestPriorities.Pri1,
            Status = TestStatus.Works,
            Author = "Microsoft Corp.",
            TestCaseType = TestCaseType.Generic,
            Description = new string[]
            {
                "Pre-Condition: Verify text is expected value: Set text to be RANDOM 256 characters",
                "Pre-Condition: Create calling range equal to entire document",
                "Pre-Condition: Create target range equal to null value",
                "Call MoveEndpointByRange(Start,target range,Start)  RAISES System.ArgumentNullException exception",
                "Call MoveEndpointByRange(Start,target range,End  )  RAISES System.ArgumentNullException exception",
                "Call MoveEndpointByRange(End,  target range,Start)  RAISES System.ArgumentNullException exception",
                "Call MoveEndpointByRange(End,  target range,End  )  RAISES System.ArgumentNullException exception"
            })]
        [SuppressMessage("Microsoft.Globalization", "CA1303:DoNotPassLiteralsAsLocalizedParameters")]
        public void TestMoveEndpointMethodRange51214(TestCaseAttribute testCase)
        {
            HeaderComment(testCase);

            ArgumentNullException ex = new ArgumentNullException("testCase"); //  Unused, just need an instance for GetType() below

            MoveEndpointByRangeHelper1(SampleText.Random256, TargetRangeType.DocumentRange, TargetRangeType.Null,
                   MoveEPResults.Exception, MoveEPResults.Exception, MoveEPResults.Exception, MoveEPResults.Exception,
                   ex.GetType());
        }

        ///<summary>TextPatternRange.MoveEndpointByRange Method.T.5.12.16</summary>
        [TestCaseAttribute("TextPatternRange.MoveEndpointByRange Method.T.5.12.16",
            TestSummary = "Move the endpoint of an empty range equal to the endpoint of DocumentRange",
            Priority = TestPriorities.Pri1,
            Status = TestStatus.Works,
            Author = "Microsoft Corp.",
            TestCaseType = TestCaseType.Generic,
            Description = new string[]
            {
                "Pre-Condition: Verify text is expected value: Set text to EMPTY string",
                "Pre-Condition: Create calling range equal to EMPTY range at START of document",
                "Pre-Condition: Create target range equal to EMPTY range at START of document",
                "Call MoveEndpointByRange(Start,target range,Start) verify result is EMPTY range",
                "Call MoveEndpointByRange(Start,target range,End  ) verify result is EMPTY range",
                "Call MoveEndpointByRange(End,  target range,Start) verify result is EMPTY range",
                "Call MoveEndpointByRange(End,  target range,End  ) verify result is EMPTY range"
            })]
        [SuppressMessage("Microsoft.Globalization", "CA1303:DoNotPassLiteralsAsLocalizedParameters")]
        public void TestMoveEndpointMethodRange51216(TestCaseAttribute testCase)
        {
            HeaderComment(testCase);


            MoveEndpointByRangeHelper1(SampleText.Empty, TargetRangeType.EmptyStart, TargetRangeType.EmptyStart,
                   MoveEPResults.EmptyRange, MoveEPResults.EmptyRange, MoveEPResults.EmptyRange, MoveEPResults.EmptyRange,
                   null);
        }

        ///<summary>TextPatternRange.MoveEndpointByRange Method.T.5.12.17</summary>
        [TestCaseAttribute("TextPatternRange.MoveEndpointByRange Method.T.5.12.17",
            TestSummary = "Move the endpoint of an empty range equal to the endpoint of DocumentRange",
            Priority = TestPriorities.Pri2,
            Status = TestStatus.Works,
            Author = "Microsoft Corp.",
            TestCaseType = TestCaseType.Generic,
            Description = new string[]
            {
                "Pre-Condition: Verify text is expected value: Set text to span MULTIPLE LINES",
                "Pre-Condition: Create calling range equal to EMPTY range at START of document",
                "Pre-Condition: Create target range equal to RANDOM range at START of document",
                "Call MoveEndpointByRange(Start,target range,Start) verify result is EMPTY range",
                "Call MoveEndpointByRange(Start,target range,End  ) verify result is EMPTY range",
                "Call MoveEndpointByRange(End,  target range,Start) verify result is EMPTY range",
                "Call MoveEndpointByRange(End,  target range,End  ) verify result is NON-empty range"
            })]
        [SuppressMessage("Microsoft.Globalization", "CA1303:DoNotPassLiteralsAsLocalizedParameters")]
        public void TestMoveEndpointMethodRange51217(TestCaseAttribute testCase)
        {
            HeaderComment(testCase);


            MoveEndpointByRangeHelper1(SampleText.MultipleLines, TargetRangeType.EmptyStart, TargetRangeType.RandomStart,
                   MoveEPResults.EmptyRange, MoveEPResults.EmptyRange, MoveEPResults.EmptyRange, MoveEPResults.NonemptyRange,
                   null);
        }

        ///<summary>TextPatternRange.MoveEndpointByRange Method.T.5.12.18</summary>
        [TestCaseAttribute("TextPatternRange.MoveEndpointByRange Method.T.5.12.18",
            TestSummary = "Move the endpoint of an empty range equal to the endpoint of DocumentRange",
            Priority = TestPriorities.Pri2,
            Status = TestStatus.Works,
            Author = "Microsoft Corp.",
            TestCaseType = TestCaseType.Generic,
            Description = new string[]
            {
                "Pre-Condition: Verify text is expected value: Set text to span MULTIPLE LINES",
                "Pre-Condition: Create calling range equal to EMPTY range in MIDDLE of document",
                "Pre-Condition: Create target range equal to RANDOM range at START of document",
                "Call MoveEndpointByRange(Start,target range,Start) verify result is NON-empty range",
                "Call MoveEndpointByRange(Start,target range,End  ) verify result is NON-empty range",
                "Call MoveEndpointByRange(End,  target range,Start) verify result is EMPTY range",
                "Call MoveEndpointByRange(End,  target range,End  ) verify result is EMPTY range"
            })]
        [SuppressMessage("Microsoft.Globalization", "CA1303:DoNotPassLiteralsAsLocalizedParameters")]
        public void TestMoveEndpointMethodRange51218(TestCaseAttribute testCase)
        {
            HeaderComment(testCase);


            MoveEndpointByRangeHelper1(SampleText.MultipleLines, TargetRangeType.EmptyMiddle, TargetRangeType.RandomStart,
                   MoveEPResults.NonemptyRange, MoveEPResults.NonemptyRange, MoveEPResults.EmptyRange, MoveEPResults.EmptyRange,
                   null);
        }

        ///<summary>TextPatternRange.MoveEndpointByRange Method.T.5.12.19</summary>
        [TestCaseAttribute("TextPatternRange.MoveEndpointByRange Method.T.5.12.19",
            TestSummary = "Move the endpoint of an empty range equal to the endpoint of DocumentRange",
            Priority = TestPriorities.Pri2,
            Status = TestStatus.Works,
            Author = "Microsoft Corp.",
            TestCaseType = TestCaseType.Generic,
            Description = new string[]
            {
                "Pre-Condition: Verify text is expected value: Set text to span MULTIPLE LINES",
                "Pre-Condition: Create calling range equal to EMPTY range at END of document",
                "Pre-Condition: Create target range equal to RANDOM range at START of document",
                "Call MoveEndpointByRange(Start,target range,Start) verify result is NON-empty range",
                "Call MoveEndpointByRange(Start,target range,End  ) verify result is NON-empty range",
                "Call MoveEndpointByRange(End,  target range,Start) verify result is EMPTY range",
                "Call MoveEndpointByRange(End,  target range,End  ) verify result is EMPTY range"
            })]
        [SuppressMessage("Microsoft.Globalization", "CA1303:DoNotPassLiteralsAsLocalizedParameters")]
        public void TestMoveEndpointMethodRange51219(TestCaseAttribute testCase)
        {
            HeaderComment(testCase);


            MoveEndpointByRangeHelper1(SampleText.MultipleLines, TargetRangeType.EmptyEnd, TargetRangeType.RandomStart,
                   MoveEPResults.NonemptyRange, MoveEPResults.NonemptyRange, MoveEPResults.EmptyRange, MoveEPResults.EmptyRange,
                   null);
        }

        ///<summary>TextPatternRange.MoveEndpointByRange Method.T.5.12.21</summary>
        [TestCaseAttribute("TextPatternRange.MoveEndpointByRange Method.T.5.12.21",
            TestSummary = "Move the endpoint of an empty range equal to the endpoint of DocumentRange",
            Priority = TestPriorities.Pri2,
            Status = TestStatus.Works,
            Author = "Microsoft Corp.",
            TestCaseType = TestCaseType.Generic,
            Description = new string[]
            {
                "Pre-Condition: Verify text is expected value: Set text to span MULTIPLE LINES",
                "Pre-Condition: Create calling range equal to entire document",
                "Pre-Condition: Create target range equal to RANDOM range at START of document",
                "Call MoveEndpointByRange(Start,target range,Start) verify result is NON-empty range",
                "Call MoveEndpointByRange(Start,target range,End  ) verify result is NON-empty range",
                "Call MoveEndpointByRange(End,  target range,Start) verify result is EMPTY range",
                "Call MoveEndpointByRange(End,  target range,End  ) verify result is NON-empty range"
            })]
        [SuppressMessage("Microsoft.Globalization", "CA1303:DoNotPassLiteralsAsLocalizedParameters")]
        public void TestMoveEndpointMethodRange51221(TestCaseAttribute testCase)
        {
            HeaderComment(testCase);


            MoveEndpointByRangeHelper1(SampleText.MultipleLines, TargetRangeType.DocumentRange, TargetRangeType.RandomStart,
                   MoveEPResults.NonemptyRange, MoveEPResults.NonemptyRange, MoveEPResults.EmptyRange, MoveEPResults.NonemptyRange,
                   null);
        }

        ///<summary>TextPatternRange.MoveEndpointByRange Method.T.5.12.23</summary>
        [TestCaseAttribute("TextPatternRange.MoveEndpointByRange Method.T.5.12.23",
            TestSummary = "Move the endpoint of an empty range equal to the endpoint of DocumentRange",
            Priority = TestPriorities.Pri2,
            Status = TestStatus.Works,
            Author = "Microsoft Corp.",
            TestCaseType = TestCaseType.Generic,
            Description = new string[]
            {
                "Pre-Condition: Verify text is expected value: Set text to span MULTIPLE LINES",
                "Pre-Condition: Create calling range equal to EMPTY range in MIDDLE of document",
                "Pre-Condition: Create target range equal to RANDOM range at END of document",
                "Call MoveEndpointByRange(Start,target range,Start) verify result is EMPTY range",
                "Call MoveEndpointByRange(Start,target range,End  ) verify result is EMPTY range",
                "Call MoveEndpointByRange(End,  target range,Start) verify result is NON-empty range",
                "Call MoveEndpointByRange(End,  target range,End  ) verify result is NON-empty range"
            })]
        [SuppressMessage("Microsoft.Globalization", "CA1303:DoNotPassLiteralsAsLocalizedParameters")]
        public void TestMoveEndpointMethodRange51223(TestCaseAttribute testCase)
        {
            HeaderComment(testCase);


            MoveEndpointByRangeHelper1(SampleText.MultipleLines, TargetRangeType.EmptyMiddle, TargetRangeType.RandomEnd,
                   MoveEPResults.EmptyRange, MoveEPResults.EmptyRange, MoveEPResults.NonemptyRange, MoveEPResults.NonemptyRange,
                   null);
        }

        ///<summary>TextPatternRange.MoveEndpointByRange Method.T.5.12.24</summary>
        [TestCaseAttribute("TextPatternRange.MoveEndpointByRange Method.T.5.12.24",
            TestSummary = "Move the endpoint of an empty range equal to the endpoint of DocumentRange",
            Priority = TestPriorities.Pri2,
            Status = TestStatus.Works,
            Author = "Microsoft Corp.",
            TestCaseType = TestCaseType.Generic,
            Description = new string[]
            {
                "Pre-Condition: Verify text is expected value: Set text to span MULTIPLE LINES",
                "Pre-Condition: Create calling range equal to EMPTY range at END of document",
                "Pre-Condition: Create target range equal to RANDOM range at END of document",
                "Call MoveEndpointByRange(Start,target range,Start) verify result is NON-empty range",
                "Call MoveEndpointByRange(Start,target range,End  ) verify result is EMPTY range",
                "Call MoveEndpointByRange(End,  target range,Start) verify result is EMPTY range",
                "Call MoveEndpointByRange(End,  target range,End  ) verify result is EMPTY range"
            })]
        [SuppressMessage("Microsoft.Globalization", "CA1303:DoNotPassLiteralsAsLocalizedParameters")]
        public void TestMoveEndpointMethodRange51224(TestCaseAttribute testCase)
        {
            HeaderComment(testCase);


            MoveEndpointByRangeHelper1(SampleText.MultipleLines, TargetRangeType.EmptyEnd, TargetRangeType.RandomEnd,
                   MoveEPResults.NonemptyRange, MoveEPResults.EmptyRange, MoveEPResults.EmptyRange, MoveEPResults.EmptyRange,
                   null);
        }

        ///<summary>TextPatternRange.MoveEndpointByRange Method.T.5.12.25</summary>
        [TestCaseAttribute("TextPatternRange.MoveEndpointByRange Method.T.5.12.25",
            TestSummary = "Move the endpoint of an empty range equal to the endpoint of DocumentRange",
            Priority = TestPriorities.Pri2,
            Status = TestStatus.Works,
            Author = "Microsoft Corp.",
            TestCaseType = TestCaseType.Generic,
            Description = new string[]
            {
                "Pre-Condition: Verify text is expected value: Set text to span MULTIPLE LINES",
                "Pre-Condition: Create calling range equal to RANDOM range in MIDDLE of document",
                "Pre-Condition: Create target range equal to RANDOM range at END of document",
                "Call MoveEndpointByRange(Start,target range,Start) verify result is EMPTY range",
                "Call MoveEndpointByRange(Start,target range,End  ) verify result is EMPTY range",
                "Call MoveEndpointByRange(End,  target range,Start) verify result is NON-empty range",
                "Call MoveEndpointByRange(End,  target range,End  ) verify result is NON-empty range"
            })]
        [SuppressMessage("Microsoft.Globalization", "CA1303:DoNotPassLiteralsAsLocalizedParameters")]
        public void TestMoveEndpointMethodRange51225(TestCaseAttribute testCase)
        {
            HeaderComment(testCase);


            MoveEndpointByRangeHelper1(SampleText.MultipleLines, TargetRangeType.RandomMiddle, TargetRangeType.RandomEnd,
                   MoveEPResults.EmptyRange, MoveEPResults.EmptyRange, MoveEPResults.NonemptyRange, MoveEPResults.NonemptyRange,
                   null);
        }

        ///<summary>TextPatternRange.MoveEndpointByRange Method.T.5.12.26</summary>
        [TestCaseAttribute("TextPatternRange.MoveEndpointByRange Method.T.5.12.26",
            TestSummary = "Move the endpoint of an empty range equal to the endpoint of DocumentRange",
            Priority = TestPriorities.Pri2,
            Status = TestStatus.Works,
            Author = "Microsoft Corp.",
            TestCaseType = TestCaseType.Generic,
            Description = new string[]
            {
                "Pre-Condition: Verify text is expected value: Set text to span MULTIPLE LINES",
                "Pre-Condition: Create calling range equal to entire document",
                "Pre-Condition: Create target range equal to RANDOM range at END of document",
                "Call MoveEndpointByRange(Start,target range,Start) verify result is NON-empty range",
                "Call MoveEndpointByRange(Start,target range,End  ) verify result is EMPTY range",
                "Call MoveEndpointByRange(End,  target range,Start) verify result is NON-empty range",
                "Call MoveEndpointByRange(End,  target range,End  ) verify result is NON-empty range"
            })]
        [SuppressMessage("Microsoft.Globalization", "CA1303:DoNotPassLiteralsAsLocalizedParameters")]
        public void TestMoveEndpointMethodRange51226(TestCaseAttribute testCase)
        {
            HeaderComment(testCase);


            MoveEndpointByRangeHelper1(SampleText.MultipleLines, TargetRangeType.DocumentRange, TargetRangeType.RandomEnd,
                   MoveEPResults.NonemptyRange, MoveEPResults.EmptyRange, MoveEPResults.NonemptyRange, MoveEPResults.NonemptyRange,
                   null);
        }

        ///<summary>TextPatternRange.MoveEndpointByRange Method.T.5.12.27</summary>
        [TestCaseAttribute("TextPatternRange.MoveEndpointByRange Method.T.5.12.27",
            TestSummary = "Move the endpoint of an empty range equal to the endpoint of DocumentRange",
            Priority = TestPriorities.Pri2,
            Status = TestStatus.Works,
            Author = "Microsoft Corp.",
            TestCaseType = TestCaseType.Generic,
            Description = new string[]
            {
                "Pre-Condition: Verify text is expected value: Set text to span MULTIPLE LINES",
                "Pre-Condition: Create calling range equal to EMPTY range at START of document",
                "Pre-Condition: Create target range equal to EMPTY range at START of document",
                "Call MoveEndpointByRange(Start,target range,Start) verify result is EMPTY range",
                "Call MoveEndpointByRange(Start,target range,End  ) verify result is EMPTY range",
                "Call MoveEndpointByRange(End,  target range,Start) verify result is EMPTY range",
                "Call MoveEndpointByRange(End,  target range,End  ) verify result is EMPTY range"
            })]
        [SuppressMessage("Microsoft.Globalization", "CA1303:DoNotPassLiteralsAsLocalizedParameters")]
        public void TestMoveEndpointMethodRange51227(TestCaseAttribute testCase)
        {
            HeaderComment(testCase);


            MoveEndpointByRangeHelper1(SampleText.MultipleLines, TargetRangeType.EmptyStart, TargetRangeType.EmptyStart,
                   MoveEPResults.EmptyRange, MoveEPResults.EmptyRange, MoveEPResults.EmptyRange, MoveEPResults.EmptyRange,
                   null);
        }

        ///<summary>TextPatternRange.MoveEndpointByRange Method.T.5.12.28</summary>
        [TestCaseAttribute("TextPatternRange.MoveEndpointByRange Method.T.5.12.28",
            TestSummary = "Move the endpoint of an empty range equal to the endpoint of DocumentRange",
            Priority = TestPriorities.Pri2,
            Status = TestStatus.Works,
            Author = "Microsoft Corp.",
            TestCaseType = TestCaseType.Generic,
            Description = new string[]
            {
                "Pre-Condition: Verify text is expected value: Set text to span MULTIPLE LINES",
                "Pre-Condition: Create calling range equal to entire document",
                "Pre-Condition: Create target range equal to same as calling range",
                "Call MoveEndpointByRange(Start,target range,Start) verify result is NON-empty range",
                "Call MoveEndpointByRange(Start,target range,End  ) verify result is EMPTY range",
                "Call MoveEndpointByRange(End,  target range,Start) verify result is EMPTY range",
                "Call MoveEndpointByRange(End,  target range,End  ) verify result is NON-empty range"
            })]
        [SuppressMessage("Microsoft.Globalization", "CA1303:DoNotPassLiteralsAsLocalizedParameters")]
        public void TestMoveEndpointMethodRange51228(TestCaseAttribute testCase)
        {
            HeaderComment(testCase);


            MoveEndpointByRangeHelper1(SampleText.MultipleLines, TargetRangeType.DocumentRange, TargetRangeType.SameAsCaller,
                   MoveEPResults.NonemptyRange, MoveEPResults.EmptyRange, MoveEPResults.EmptyRange, MoveEPResults.NonemptyRange,
                   null);
        }

        #endregion 5.12 TextPatternRange.MoveEndpointByRange Method

        #region 5.13 TextPatternRange.ExpandToEnclosingUnit Method

        ///<summary>TextPatternRange.ExpandToEnclosingUnit Method.T.5.13.01</summary>
        [TestCaseAttribute("TextPatternRange.ExpandToEnclosingUnit Method.T.5.13.01",
            TestSummary = "Expand a range by each TextUnit and verify the resulting range is correct",
            Priority = TestPriorities.Pri0,
            Status = TestStatus.Works,
            Author = "Microsoft Corp.",
            TestCaseType = TestCaseType.Generic,
            Description = new string[]
            {
                "Pre-Condition: Determine supported TextUnits for this control",
                "Pre-Condition: Verify text is expected value: Set text to 'String1'",
                "Pre-Condition: Create calling range equal to: EMPTY range at START of document",
                "Clone range and call ExpandToEnclosingUnit(Character) and verify results",
                "Clone range and call ExpandToEnclosingUnit(Format)    and verify results",
                "Clone range and call ExpandToEnclosingUnit(Word)      and verify results",
                "Clone range and call ExpandToEnclosingUnit(Line)      and verify results",
                "Clone range and call ExpandToEnclosingUnit(Paragraph) and verify results",
                "Clone range and call ExpandToEnclosingUnit(Page)      and verify results",
                "Clone range and call ExpandToEnclosingUnit(Document)  and verify results"
            })]
        public void TestExpandToEnclosingUnitMethod51301(TestCaseAttribute testCase)
        {
            HeaderComment(testCase);

            ExpandToEnclosingUnitHelper(SampleText.String1, TargetRangeType.EmptyStart);
        }

        ///<summary>TextPatternRange.ExpandToEnclosingUnit Method.T.5.13.02</summary>
        [TestCaseAttribute("TextPatternRange.ExpandToEnclosingUnit Method.T.5.13.02",
            TestSummary = "Expand a range by each TextUnit and verify the resulting range is correct",
            Priority = TestPriorities.Pri1,
            Status = TestStatus.Works,
            Author = "Microsoft Corp.",
            TestCaseType = TestCaseType.Generic,
            Description = new string[]
            {
                "Pre-Condition: Determine supported TextUnits for this control",
                "Pre-Condition: Verify text is expected value: Text that is 64 characters in length",
                "Pre-Condition: Create calling range equal to: EMPTY range at START of document",
                "Clone range and call ExpandToEnclosingUnit(Character) and verify results",
                "Clone range and call ExpandToEnclosingUnit(Format)    and verify results",
                "Clone range and call ExpandToEnclosingUnit(Word)      and verify results",
                "Clone range and call ExpandToEnclosingUnit(Line)      and verify results",
                "Clone range and call ExpandToEnclosingUnit(Paragraph) and verify results",
                "Clone range and call ExpandToEnclosingUnit(Page)      and verify results",
                "Clone range and call ExpandToEnclosingUnit(Document)  and verify results"
            })]
        public void TestExpandToEnclosingUnitMethod51302(TestCaseAttribute testCase)
        {
            HeaderComment(testCase);

            ExpandToEnclosingUnitHelper(SampleText.Random64, TargetRangeType.EmptyStart);
        }

        ///<summary>TextPatternRange.ExpandToEnclosingUnit Method.T.5.13.04</summary>
        [TestCaseAttribute("TextPatternRange.ExpandToEnclosingUnit Method.T.5.13.04",
            TestSummary = "Expand a range by each TextUnit and verify the resulting range is correct",
            Priority = TestPriorities.Pri1,
            Status = TestStatus.Works,
            Author = "Microsoft Corp.",
            TestCaseType = TestCaseType.Generic,
            Description = new string[]
            {
                "Pre-Condition: Determine supported TextUnits for this control",
                "Pre-Condition: Verify text is expected value: Text that is 64 characters in length",
                "Pre-Condition: Create calling range equal to: first character in document",
                "Clone range and call ExpandToEnclosingUnit(Character) and verify results",
                "Clone range and call ExpandToEnclosingUnit(Format)    and verify results",
                "Clone range and call ExpandToEnclosingUnit(Word)      and verify results",
                "Clone range and call ExpandToEnclosingUnit(Line)      and verify results",
                "Clone range and call ExpandToEnclosingUnit(Paragraph) and verify results",
                "Clone range and call ExpandToEnclosingUnit(Page)      and verify results",
                "Clone range and call ExpandToEnclosingUnit(Document)  and verify results"
            })]
        public void TestExpandToEnclosingUnitMethod51304(TestCaseAttribute testCase)
        {
            HeaderComment(testCase);

            ExpandToEnclosingUnitHelper(SampleText.Random64, TargetRangeType.FirstCharacter);
        }

        ///<summary>TextPatternRange.ExpandToEnclosingUnit Method.T.5.13.05</summary>
        [TestCaseAttribute("TextPatternRange.ExpandToEnclosingUnit Method.T.5.13.05",
            TestSummary = "Expand a range by each TextUnit and verify the resulting range is correct",
            Priority = TestPriorities.Pri1,
            Status = TestStatus.Works,
            Author = "Microsoft Corp.",
            TestCaseType = TestCaseType.Generic,
            Description = new string[]
            {
                "Pre-Condition: Determine supported TextUnits for this control",
                "Pre-Condition: Verify text is expected value: Text that is 64 characters in length",
                "Pre-Condition: Create calling range equal to: last character in document",
                "Clone range and call ExpandToEnclosingUnit(Character) and verify results",
                "Clone range and call ExpandToEnclosingUnit(Format)    and verify results",
                "Clone range and call ExpandToEnclosingUnit(Word)      and verify results",
                "Clone range and call ExpandToEnclosingUnit(Line)      and verify results",
                "Clone range and call ExpandToEnclosingUnit(Paragraph) and verify results",
                "Clone range and call ExpandToEnclosingUnit(Page)      and verify results",
                "Clone range and call ExpandToEnclosingUnit(Document)  and verify results"
            })]
        public void TestExpandToEnclosingUnitMethod51305(TestCaseAttribute testCase)
        {
            try
            {
                HeaderComment(testCase);

                ExpandToEnclosingUnitHelper(SampleText.Random64, TargetRangeType.LastCharacter);
            }
            catch (Exception e)
            {
                throw e; /* amd64fre does not include this function (1st called in assembly?) in the trace unless we do this */
            }
        }

        ///<summary>TextPatternRange.ExpandToEnclosingUnit Method.T.5.13.06</summary>
        [TestCaseAttribute("TextPatternRange.ExpandToEnclosingUnit Method.T.5.13.06",
            TestSummary = "Expand a range by each TextUnit and verify the resulting range is correct",
            Priority = TestPriorities.Pri2,
            Status = TestStatus.Works,
            Author = "Microsoft Corp.",
            TestCaseType = TestCaseType.Generic,
            Description = new string[]
            {
                "Pre-Condition: Determine supported TextUnits for this control",
                "Pre-Condition: Verify text is expected value: Set text to EMPTY string",
                "Pre-Condition: Create calling range equal to: EMPTY range at START of document",
                "Clone range and call ExpandToEnclosingUnit(Character) and verify results",
                "Clone range and call ExpandToEnclosingUnit(Format)    and verify results",
                "Clone range and call ExpandToEnclosingUnit(Word)      and verify results",
                "Clone range and call ExpandToEnclosingUnit(Line)      and verify results",
                "Clone range and call ExpandToEnclosingUnit(Paragraph) and verify results",
                "Clone range and call ExpandToEnclosingUnit(Page)      and verify results",
                "Clone range and call ExpandToEnclosingUnit(Document)  and verify results"
            })]
        public void TestExpandToEnclosingUnitMethod51306(TestCaseAttribute testCase)
        {
            HeaderComment(testCase);
            
            ExpandToEnclosingUnitHelper(SampleText.Empty, TargetRangeType.EmptyStart);
        }

        ///<summary>TextPatternRange.ExpandToEnclosingUnit Method.T.5.13.07</summary>
        [TestCaseAttribute("TextPatternRange.ExpandToEnclosingUnit Method.T.5.13.07",
            TestSummary = "Expand a range by each TextUnit and verify the resulting range is correct",
            Priority = TestPriorities.Pri2,
            Status = TestStatus.Works,
            Author = "Microsoft Corp.",
            TestCaseType = TestCaseType.Generic,
            Description = new string[]
            {
                "Pre-Condition: Determine supported TextUnits for this control",
                "Pre-Condition: Verify text is expected value: Text that is 64 characters in length and has multiple lines",
                "Pre-Condition: Create calling range equal to: EMPTY range at START of document",
                "Clone range and call ExpandToEnclosingUnit(Character) and verify results",
                "Clone range and call ExpandToEnclosingUnit(Format)    and verify results",
                "Clone range and call ExpandToEnclosingUnit(Word)      and verify results",
                "Clone range and call ExpandToEnclosingUnit(Line)      and verify results",
                "Clone range and call ExpandToEnclosingUnit(Paragraph) and verify results",
                "Clone range and call ExpandToEnclosingUnit(Page)      and verify results",
                "Clone range and call ExpandToEnclosingUnit(Document)  and verify results"
            })]
        public void TestExpandToEnclosingUnitMethod51307(TestCaseAttribute testCase)
        {
            HeaderComment(testCase);

            ExpandToEnclosingUnitHelper(SampleText.Random64CR, TargetRangeType.EmptyStart);
        }

        #endregion 5.13 TextPatternRange.ExpandToEnclosingUnit Method

        #region 5.14 TextPatternRange.ScrollIntoView Method

        ///<summary>TextPatternRange.ScrollIntoView Method.T.5.14.01</summary>
        [TestCaseAttribute("TextPatternRange.ScrollIntoView Method.T.5.14.01",
            TestSummary = "Calls ScrollIntoView for either top or bottom of viewport",
            Priority = TestPriorities.Pri1,
            Status = TestStatus.Works,
            ProblemDescription = "Bug(s): 1456324--(Any or JPN) Edit*",
            Author = "Microsoft Corp.",
            TestCaseType = TestCaseType.Generic,
            Description = new string[]
            {
                "Pre-Condition: Verify text is expected value: Set text to be 'easy' text",
                "Pre-Condition: Create range equal to: entire document",
                "Pre-Condition: Get text from calling range",
                "Pre-Condition: SetFocus to control containing TextPattern",
                "Call ScrollIntoView(align to top of viewport)",
                "Call GetVisibleRanges() on TextPattern",
                "Call GetText() on FIRST visible range (post V1, this should validate against each/all ranges)",
                "Verify that text sub-range is in visible range" })]
        public void TestScrollIntoViewMethod51401(TestCaseAttribute testCase)
        {
            HeaderComment(testCase);

            ScrollIntoViewHelper(SampleText.EasyText, TargetRangeType.DocumentRange, true);
        }

        ///<summary>TextPatternRange.ScrollIntoView Method.T.5.14.02</summary>
        [TestCaseAttribute("TextPatternRange.ScrollIntoView Method.T.5.14.02",
            TestSummary = "Calls ScrollIntoView for either top or bottom of viewport",
            Priority = TestPriorities.Pri1,
            Status = TestStatus.Works,
            ProblemDescription = "Bug(s): 1456324--(Any or Jpn) Edit*",
            Author = "Microsoft Corp.",
            TestCaseType = TestCaseType.Generic,
            Description = new string[]
            {
                "Pre-Condition: Verify text is expected value: Set text to be 'easy' text",
                "Pre-Condition: Create range equal to: entire document",
                "Pre-Condition: Get text from calling range",
                "Pre-Condition: SetFocus to control containing TextPattern",
                "Call ScrollIntoView(align to bottom of viewport)",
                "Call GetVisibleRanges() on TextPattern",
                "Call GetText() on FIRST visible range (post V1, this should validate against each/all ranges)",
                "Verify that text sub-range is in visible range" })]
        public void TestScrollIntoViewMethod51402(TestCaseAttribute testCase)
        {
            HeaderComment(testCase);

            ScrollIntoViewHelper(SampleText.EasyText, TargetRangeType.DocumentRange, false);
        }

        ///<summary>TextPatternRange.ScrollIntoView Method.T.5.14.03</summary>
        [TestCaseAttribute("TextPatternRange.ScrollIntoView Method.T.5.14.03",
            TestSummary = "Calls ScrollIntoView for either top or bottom of viewport",
            Priority = TestPriorities.Pri1,
            Status = TestStatus.Works,
            Author = "Microsoft Corp.",
            TestCaseType = TestCaseType.Generic,
            Description = new string[]
            {
                "Pre-Condition: Verify text is expected value: Set text to be RANDOM 256 characters",
                "Pre-Condition: Create range equal to: entire document",
                "Pre-Condition: Get text from calling range",
                "Pre-Condition: SetFocus to control containing TextPattern",
                "Call ScrollIntoView(align to top of viewport)",
                "Call GetVisibleRanges() on TextPattern",
                "Call GetText() on FIRST visible range (post V1, this should validate against each/all ranges)",
                "Verify that text sub-range is in visible range" })]
        public void TestScrollIntoViewMethod51403(TestCaseAttribute testCase)
        {
            HeaderComment(testCase);

            ScrollIntoViewHelper(SampleText.Random256, TargetRangeType.DocumentRange, true);
        }

        ///<summary>TextPatternRange.ScrollIntoView Method.T.5.14.04</summary>
        [TestCaseAttribute("TextPatternRange.ScrollIntoView Method.T.5.14.04",
            TestSummary = "Calls ScrollIntoView for either top or bottom of viewport",
            Priority = TestPriorities.Pri1,
            Status = TestStatus.Works,
            Author = "Microsoft Corp.",
            TestCaseType = TestCaseType.Generic,
            Description = new string[]
            {
                "Pre-Condition: Verify text is expected value: Set text to be RANDOM 256 characters",
                "Pre-Condition: Create range equal to: entire document",
                "Pre-Condition: Get text from calling range",
                "Pre-Condition: SetFocus to control containing TextPattern",
                "Call ScrollIntoView(align to bottom of viewport)",
                "Call GetVisibleRanges() on TextPattern",
                "Call GetText() on FIRST visible range (post V1, this should validate against each/all ranges)",
                "Verify that text sub-range is in visible range" })]
        public void TestScrollIntoViewMethod51404(TestCaseAttribute testCase)
        {
            HeaderComment(testCase);

            ScrollIntoViewHelper(SampleText.Random256, TargetRangeType.DocumentRange, false);
        }

        ///<summary>TextPatternRange.ScrollIntoView Method.T.5.14.05</summary>
        [TestCaseAttribute("TextPatternRange.ScrollIntoView Method.T.5.14.05",
            TestSummary = "Calls ScrollIntoView for either top or bottom of viewport",
            Priority = TestPriorities.Pri1,
            Status = TestStatus.Blocked, // ScrollIntoView Control 1000 locks up
            ProblemDescription = "Bug(s): 1346630--CtrlTestEdit(1003)",
            Author = "Microsoft Corp.",
            TestCaseType = TestCaseType.Generic,
            Description = new string[]
            {
                "Pre-Condition: Verify text is expected value: Set text to be RANDOM 256 characters",
                "Pre-Condition: Create range equal to: last character in document",
                "Pre-Condition: Get text from calling range",
                "Pre-Condition: SetFocus to control containing TextPattern",
                "Call ScrollIntoView(align to bottom of viewport)",
                "Call GetVisibleRanges() on TextPattern",
                "Call GetText() on FIRST visible range (post V1, this should validate against each/all ranges)",
                "Verify that text sub-range is in visible range" })]
        public void TestScrollIntoViewMethod51405(TestCaseAttribute testCase)
        {
            HeaderComment(testCase);

            ScrollIntoViewHelper(SampleText.Random256, TargetRangeType.LastCharacter, false);
        }

        #endregion 5.14 TextPatternRange.ScrollIntoView Method

        #region 5.15 TextPatternRange.GetEnclosingElement Method

        ///<summary>TextPatternRange.GetEnclosingElement Method.T.5.15.01</summary>
        [TestCaseAttribute("TextPatternRange.GetEnclosingElement Method.T.5.15.01",
            TestSummary = "Verify that GetEnclosingElment() returns AutoELem that contains TextPattern",
            Priority = TestPriorities.Pri0,
            Status = TestStatus.Works,
            Author = "Microsoft Corp.",
            TestCaseType = TestCaseType.Generic,
            Description = new string[]
            {
                "Pre-Condition: Verify text is expected value: Set text to 'String1'",
                "Pre-Condition: Confirm that there are NO children",
                "Call GetEnclosingElement()",
                "Perform value equality to validate instances have same identity",
                "Enumerate through children (if present), calling GetEnclosingElement on each"
            })]
        public void TestGetEnclosingElementMethod51501(TestCaseAttribute testCase)
        {
            HeaderComment(testCase);

            GetEnclosingElementHelper(SampleText.String1, false);
        }

        ///<summary>TextPatternRange.GetEnclosingElement Method.T.5.15.02</summary>
        [TestCaseAttribute("TextPatternRange.GetEnclosingElement Method.T.5.15.02",
            TestSummary = "Verify that GetEnclosingElment() returns AutoELem that contains TextPattern",
            Priority = TestPriorities.Pri1,
            Status = TestStatus.Works,
            Author = "Microsoft Corp.",
            TestCaseType = TestCaseType.Generic,
            Description = new string[]
            {
                "Pre-Condition: Verify text is expected value: Verify text is NOT empty (or SET text if it is empty)",
                "Pre-Condition: Confirm that there are NO children",
                "Call GetEnclosingElement()",
                "Perform value equality to validate instances have same identity",
                "Enumerate through children (if present), calling GetEnclosingElement on each"
            })]
        public void TestGetEnclosingElementMethod51502(TestCaseAttribute testCase)
        {
            HeaderComment(testCase);

            GetEnclosingElementHelper(SampleText.NotEmpty, false);
        }

        ///<summary>TextPatternRange.GetEnclosingElement Method.T.5.15.03</summary>
        [TestCaseAttribute("TextPatternRange.GetEnclosingElement Method.T.5.15.03",
            TestSummary = "Verify that GetEnclosingElment() returns AutoELem that contains TextPattern",
            Priority = TestPriorities.Pri1,
            Status = TestStatus.Works,
            Author = "Microsoft Corp.",
            TestCaseType = TestCaseType.Generic,
            Description = new string[]
            {
                "Pre-Condition: Verify text is expected value: Set text to EMPTY string",
                "Pre-Condition: Confirm that there are NO children",
                "Call GetEnclosingElement()",
                "Perform value equality to validate instances have same identity",
                "Enumerate through children (if present), calling GetEnclosingElement on each"
            })]
        public void TestGetEnclosingElementMethod51503(TestCaseAttribute testCase)
        {
            HeaderComment(testCase);

            GetEnclosingElementHelper(SampleText.Empty, false);
        }

        ///<summary>TextPatternRange.GetEnclosingElement Method.T.5.15.04</summary>
        [TestCaseAttribute("TextPatternRange.GetEnclosingElement Method.T.5.15.04",
            TestSummary = "Verify that GetEnclosingElment() returns AutoELem that contains TextPattern",
            Priority = TestPriorities.Pri1,
            Status = TestStatus.Works,
            Author = "Microsoft Corp.",
            TestCaseType = TestCaseType.Generic,
            Description = new string[]
            {
                "Pre-Condition: Verify text is expected value: Verify text is NOT empty (or SET text if it is empty)",
                "Pre-Condition: Confirm that there are NO children",
                "Call GetEnclosingElement()",
                "Perform value equality to validate instances have same identity",
                "Enumerate through children (if present), calling GetEnclosingElement on each"
            })]
        public void TestGetEnclosingElementMethod51504(TestCaseAttribute testCase)
        {
            HeaderComment(testCase);

            GetEnclosingElementHelper(SampleText.NotEmpty, false);
        }

        ///<summary>TextPatternRange.GetEnclosingElement Method.T.5.15.05</summary>
        [TestCaseAttribute("TextPatternRange.GetEnclosingElement Method.T.5.15.05",
            TestSummary = "Verify that GetEnclosingElment() returns AutoELem that contains TextPattern",
            Priority = TestPriorities.Pri1,
            Status = TestStatus.Works,
            Author = "Microsoft Corp.",
            TestCaseType = TestCaseType.Generic,
            Description = new string[]
            {
                "Pre-Condition: Verify text is expected value: Set text to EMPTY string",
                "Pre-Condition: Confirm that there are NO children",
                "Call GetEnclosingElement()",
                "Perform value equality to validate instances have same identity",
                "Enumerate through children (if present), calling GetEnclosingElement on each"
            })]
        public void TestGetEnclosingElementMethod51505(TestCaseAttribute testCase)
        {
            HeaderComment(testCase);

            GetEnclosingElementHelper(SampleText.Empty, false);
        }

        #endregion 5.15 TextPatternRange.GetEnclosingElement Method

        #region 5.16 TextPatternRange.AddToSelection Method

        ///<summary>TextPatternRange.AddToSelection Method.T.5.16.01</summary>
        [TestCaseAttribute("TextPatternRange.AddToSelection Method.T.5.16.01",
            TestSummary = "Call AddToSelection for given automation elements",
            Priority = TestPriorities.Pri0,
            Status = TestStatus.Works,
            Author = "Microsoft Corp.",
            TestCaseType = TestCaseType.Generic,
            Description = new string[]
            {
                "Call AddToSelection()  RAISES System.InvalidOperationException exception"
            })]
        [SuppressMessage("Microsoft.Globalization", "CA1303:DoNotPassLiteralsAsLocalizedParameters")]
        public void TestAddToSelectionMethod51601(TestCaseAttribute testCase)
        {
            HeaderComment(testCase);

            InvalidOperationException ex = new InvalidOperationException("Unused, just need an instance for GetType() below");

            AddToSelectionHelper(ex.GetType());
        }

        #endregion 5.16 TextPatternRange.AddToSelection Method


        #region 5.17 TextPatternRange.RemoveFromSelection Method

        ///<summary>TextPatternRange.RemoveFromSelection Method.T.5.17.01</summary>
        [TestCaseAttribute("TextPatternRange.RemoveFromSelection Method.T.5.17.01",
            TestSummary = "Call RemoveFromSelection for given automation elements",
            Priority = TestPriorities.Pri0,
            Status = TestStatus.Works,
            Author = "Microsoft Corp.",
            TestCaseType = TestCaseType.Generic,
            Description = new string[]
            {
                "Call RemoveFromSelection()  RAISES System.InvalidOperationException exception"
            })]
        [SuppressMessage("Microsoft.Globalization", "CA1303:DoNotPassLiteralsAsLocalizedParameters")]
        public void TestRemoveFromSelectionMethod51701(TestCaseAttribute testCase)
        {
            HeaderComment(testCase);

            InvalidOperationException ex = new InvalidOperationException("Unused, just need an instance for GetType() below");

            RemoveFromSelectionHelper(ex.GetType());
        }

        #endregion 5.17 TextPatternRange.RemoveFromSelection Method

        #endregion Methods

        #endregion TextPatternRange

        #region Regression Tests


        ///<summary>TextPatternRangeRegression.T.1192048</summary>
        [TestCaseAttribute("TextPatternRangeRegression.T.1192048",
            TestSummary = "Verify no regression of 1192048.",
            Priority = TestPriorities.Pri0,
            Status = TestStatus.Works,
            Author = "Microsoft Corp.",
            TestCaseType = TestCaseType.Generic,
            Description = new string[]
            {
                "Pre-Condition: Give info about bug",
                "Perform Regression Test",
            })]
        public void TextPatternRangeRegression1192048(TestCaseAttribute testCase)
        {
            HeaderComment(testCase);

            Bug1192048("1192048", "Win32Edit(CtrlTestEdit:1001)", "BVT BLOCKER: ScrollIntoView raises InvalidOperationException on Win32Edit control");
        }

        ///<summary>TextPatternRangeRegression.T.1191485</summary>
        [TestCaseAttribute("TextPatternRangeRegression.T.1191485",
            TestSummary = "Verify no regression of 1191485.",
            Priority = TestPriorities.Pri0,
            Status = TestStatus.Works,
            Author = "Microsoft Corp.",
            TestCaseType = TestCaseType.Generic,
            Description = new string[]
            {
                "Pre-Condition: Give info about bug",
                "Perform Regression Test",
            })]
        public void TextPatternRangeRegression1191485(TestCaseAttribute testCase)
        {
            HeaderComment(testCase);

            Bug1191485("1191485", "Any Win32(Plus others?)", "BVT BLOCKER: GetText(-1) throws ArgumentOutOfRangeException");
        }

        ///<summary>TextPatternRangeRegression.T.1186712</summary>
        [TestCaseAttribute("TextPatternRangeRegression.T.1186712",
            TestSummary = "Verify no regression of 1186712.",
            Priority = TestPriorities.Pri0,
            Status = TestStatus.Works,
            Author = "Microsoft Corp.",
            TestCaseType = TestCaseType.Generic,
            Description = new string[]
            {
                "Pre-Condition: Give info about bug",
                "Perform Regression Test",
            })]
        public void TextPatternRangeRegression1186712(TestCaseAttribute testCase)
        {
            HeaderComment(testCase);

            Bug1186712("1186712", "Win32Edit(CtrlTest:1000)", "HOT: MoveEndpointByUnit(Start, <<all textunits>>,1 ) returning 0, not 1, for non-empty text range on Win32 Edit");
        }

        ///<summary>TextPatternRangeRegression.T.1186687</summary>
        [TestCaseAttribute("TextPatternRangeRegression.T.1186687",
            TestSummary = "Verify no regression of 1186687.",
            Priority = TestPriorities.Pri0,
            Status = TestStatus.Works,
            Author = "Microsoft Corp.",
            TestCaseType = TestCaseType.Generic,
            Description = new string[]
            {
                "Pre-Condition: Give info about bug",
                "Perform Regression Test",
            })]
        public void TextPatternRangeRegression1186687(TestCaseAttribute testCase)
        {
            HeaderComment(testCase);

            Bug1186687("1186687", "Win32Edit(CtrlTest:1000)", "HOT: MoveEndpointByUnit(End, TextUnit.Leine, 1 ) returning 1, not 0, for non-empty text range on Win32 Edit");
        }

        ///<summary>TextPatternRangeRegression.T.1186653</summary>
        [TestCaseAttribute("TextPatternRangeRegression.T.1186653",
            TestSummary = "Verify no regression of 1186653.",
            Priority = TestPriorities.Pri0,
            Status = TestStatus.Works,
            Author = "Microsoft Corp.",
            TestCaseType = TestCaseType.Generic,
            Description = new string[]
            {
                "Pre-Condition: Give info about bug",
                "Perform Regression Test",
            })]
        public void TextPatternRangeRegression1186653(TestCaseAttribute testCase)
        {
            HeaderComment(testCase);

            Bug1186653("1186653", "Win32Edit(CtrlTest:1000)", "HOT: FindText returns range that is trimmed by one character on Win32 Edit Control");
        }

        ///<summary>TextPatternRangeRegression.T.1184263</summary>
        [TestCaseAttribute("TextPatternRangeRegression.T.1184263",
            TestSummary = "Verify no regression of 1184263.",
            Priority = TestPriorities.Pri0,
            Status = TestStatus.Works,
            Author = "Microsoft Corp.",
            TestCaseType = TestCaseType.Generic,
            Description = new string[]
            {
                "Pre-Condition: Give info about bug",
                "Perform Regression Test",
            })]
        public void TextPatternRangeRegression1184263(TestCaseAttribute testCase)
        {
            HeaderComment(testCase);

            Bug1184263("1184263", "Win32Edit(CtrlTest:1095)", "GetVisibleRanges raises assertion for multi-line edit controls withut AutoHScroll style after ScrollIntoView");
        }

        ///<summary>TextPatternRangeRegression.T.1106920</summary>
        [TestCaseAttribute("TextPatternRangeRegression.T.1106920",
            TestSummary = "Verify no regression of 1106920.",
            ProblemDescription = "Possible Regression of 1106920 (Incorrect Exception raised from FindText), originally filed on Win32Edit(CtrlTest:1000)",
            Priority = TestPriorities.Pri0,
            Status = TestStatus.Works,
            Author = "Microsoft Corp.",
            TestCaseType = TestCaseType.Generic,
            Description = new string[]
            {
                "Pre-Condition: Give info about bug",
                "Perform Regression Test",
            })]
        public void TextPatternRangeRegression1106920(TestCaseAttribute testCase)
        {
            HeaderComment(testCase);

            Bug1106920("1106920", "Win32Edit(CtrlTest:1000)", "Incorrect Exception raised from FindText");
        }

        ///<summary>TextPatternRangeRegression.T.1103948</summary>
        [TestCaseAttribute("TextPatternRangeRegression.T.1103948",
            TestSummary = "Verify no regression of 1103948.",
            ProblemDescription = "Possible Regression of 1103948 (HOT Bug: GetBoundingRectangles() raises exception for Edit controls with Multiline style set, but no scroll bar), originally filed on Win32Edit(CtrlTestEdit:1007)",
            Priority = TestPriorities.Pri0,
            Status = TestStatus.Works,
            Author = "Microsoft Corp.",
            TestCaseType = TestCaseType.Generic,
            Description = new string[]
            {
                "Pre-Condition: Give info about bug",
                "Perform Regression Test",
            })]
        public void TextPatternRangeRegression1103948(TestCaseAttribute testCase)
        {
            HeaderComment(testCase);

            Bug1103948("1103948", "Win32Edit(CtrlTestEdit:1007)", "HOT Bug: GetBoundingRectangles() raises exception for Edit controls with Multiline style set, but no scroll bar");
        }

        ///<summary>TextPatternRangeRegression.T.1081944</summary>
        [TestCaseAttribute("TextPatternRangeRegression.T.1081944",
            TestSummary = "Verify no regression of 1081944.",
            ProblemDescription = "Possible Regression of 1081944 (BVT Blocker: GetVisibleRanges() raises assertion on edit control when size of text is 1 character), originally filed on Win32Edit(CtrlTest:1000)",
            Priority = TestPriorities.Pri0,
            Status = TestStatus.Works,
            Author = "Microsoft Corp.",
            TestCaseType = TestCaseType.Generic,
            Description = new string[]
            {
                "Pre-Condition: Give info about bug",
                "Perform Regression Test",
            })]
        public void TextPatternRangeRegression1081944(TestCaseAttribute testCase)
        {
            HeaderComment(testCase);

            Bug1081944("1081944", "Win32Edit(CtrlTest:1000)", "BVT Blocker: GetVisibleRanges() raises assertion on edit control when size of text is 1 character");
        }

        ///<summary>TextPatternRangeRegression.T.1080760</summary>
        [TestCaseAttribute("TextPatternRangeRegression.T.1080760",
            TestSummary = "Verify no regression of 1080760.",
            ProblemDescription = "Possible Regression of 1080760 (ExpandToEnclosingUnit(TextUnit.Line) incorrectly returns an empty range on single-line Win32:Edit), originally filed on Win32Edit(CtrlTest:1000)",
            Priority = TestPriorities.Pri0,
            Status = TestStatus.Works,
            Author = "Microsoft Corp.",
            TestCaseType = TestCaseType.Generic,
            Description = new string[]
            {
                "Pre-Condition: Give info about bug",
                "Perform Regression Test",
            })]
        public void TextPatternRangeRegression1080760(TestCaseAttribute testCase)
        {
            HeaderComment(testCase);

            Bug1080760("1080760", "Win32Edit(CtrlTest:1000)", "ExpandToEnclosingUnit(TextUnit.Line) incorrectly returns an empty range on single-line Win32:Edit");
        }

        ///<summary>TextPatternRangeRegression.T.1080662</summary>
        [TestCaseAttribute("TextPatternRangeRegression.T.1080662",
            TestSummary = "Verify no regression of 1080662.",
            ProblemDescription = "Possible Regression of 1080662 (MoveEndpointByUnit(TextUnit.Line, MinInt) and Move(TextUnit.Line, MinInt) both return incorrect line count Win32:Edit/RichEdit), originally filed on Win32Edit(CtrlTest:1000)",
            Priority = TestPriorities.Pri0,
            Status = TestStatus.Works,
            Author = "Microsoft Corp.",
            TestCaseType = TestCaseType.Generic,
            Description = new string[]
            {
                "Pre-Condition: Give info about bug",
                "Perform Regression Test",
            })]
        public void TextPatternRangeRegression1080662(TestCaseAttribute testCase)
        {
            HeaderComment(testCase);

            Bug1080662("1080662", "Win32Edit(CtrlTest:1000)", "MoveEndpointByUnit(TextUnit.Line, MinInt) and Move(TextUnit.Line, MinInt) both return incorrect line count Win32:Edit/RichEdit");
        }

        ///<summary>TextPatternRangeRegression.T.1080418</summary>
        [TestCaseAttribute("TextPatternRangeRegression.T.1080418",
            TestSummary = "Verify no regression of 1080418.",
            ProblemDescription = "Possible Regression of 1080418 (MoveEndpointByUnit(TextUnit.Line, MaxInt) and Move(TextUnit.Line, MaxInt) both return 0 on Win32:Edit when moving forward), originally filed on Win32Edit(CtrlTest:1000)",
            Priority = TestPriorities.Pri0,
            Status = TestStatus.Works,
            Author = "Microsoft Corp.",
            TestCaseType = TestCaseType.Generic,
            Description = new string[]
            {
                "Pre-Condition: Give info about bug",
                "Perform Regression Test",
            })]
        public void TextPatternRangeRegression1080418(TestCaseAttribute testCase)
        {
            HeaderComment(testCase);

            Bug1080418("1080418", "Win32Edit(CtrlTest:1000)", "MoveEndpointByUnit(TextUnit.Line, MaxInt) and Move(TextUnit.Line, MaxInt) both return 0 on Win32:Edit when moving forward");
        }

        ///<summary>TextPatternRangeRegression.T.1055455</summary>
        [TestCaseAttribute("TextPatternRangeRegression.T.1055455",
            TestSummary = "Verify no regression of 1055455.",
            ProblemDescription = "Possible Regression of 1055455 (BVT Blocker: TextPatternRange.GetVisibleRanges returns text longer than actual visible range on Edit w/Horiz Scrollbar), originally filed on Win32Edit(CtrlTest:1096)",
            Priority = TestPriorities.Pri0,
            Status = TestStatus.Works,
            Author = "Microsoft Corp.",
            TestCaseType = TestCaseType.Generic,
            Description = new string[]
            {
                "Pre-Condition: Give info about bug",
                "Perform Regression Test",
            })]
        public void TextPatternRangeRegression1055455(TestCaseAttribute testCase)
        {
            HeaderComment(testCase);

            Bug1055455("1055455", "Win32Edit(CtrlTest:1096)", "BVT Blocker: TextPatternRange.GetVisibleRanges returns text longer than actual visible range on Edit w/Horiz Scrollbar");
        }

        ///<summary>TextPatternRangeRegression.T.1139305</summary>
        [TestCaseAttribute("TextPatternRangeRegression.T.1139305",
            TestSummary = "Verify no regression of 1139305.",
            ProblemDescription = "Possible Regression of 1139305 (TextPattern.GetVisibleRanges on empty document raises assertion, instead of returning degenerate range on Win32 Edit), originally filed on Win32Edit(CtrlTest:1000)",
            Priority = TestPriorities.Pri0,
            Status = TestStatus.Works,
            Author = "Microsoft Corp.",
            TestCaseType = TestCaseType.Generic,
            Description = new string[]
            {
                "Pre-Condition: Give info about bug",
                "Perform Regression Test",
            })]
        public void TextPatternRangeRegression1139305(TestCaseAttribute testCase)
        {
            HeaderComment(testCase);

            Bug1139305("1139305", "Win32Edit(CtrlTest:1000)", "TextPattern.GetVisibleRanges on empty document raises assertion, instead of returning degenerate range on Win32 Edit");
        }

        ///<summary>TextPatternRangeRegression.T.1040436</summary>
        [TestCaseAttribute("TextPatternRangeRegression.T.1040436",
            TestSummary = "Verify no regression of 1040436.",
            ProblemDescription = "Possible Regression of 1040436 (BVT Blocker: TextPatternRange--Expanding empty range via TextUnit.Line on text in a single-line Win32.Edit leaves an empty range), originally filed on Win32Edit(CtrlTest:1000)",
            Priority = TestPriorities.Pri0,
            Status = TestStatus.Works,
            Author = "Microsoft Corp.",
            TestCaseType = TestCaseType.Generic,
            Description = new string[]
            {
                "Pre-Condition: Give info about bug",
                "Perform Regression Test",
            })]
        public void TextPatternRangeRegression1040436(TestCaseAttribute testCase)
        {
            HeaderComment(testCase);

            Bug1040436("1040436", "Win32Edit(CtrlTest:1000)", "BVT Blocker: TextPatternRange--Expanding empty range via TextUnit.Line on text in a single-line Win32.Edit leaves an empty range");
        }

        ///<summary>TextPatternRangeRegression.T.1744558</summary>
        [TestCaseAttribute("TextPatternRangeRegression.T.1744558",
            TestSummary = "Verify no regression of 1744558.",
            ProblemDescription = "Possible Regression of 1744558 (TextPatternRange.Compare returns true even when ranges do not match), originally filed on Avalon/WPF FlowDocument",
            Priority = TestPriorities.Pri0,
            Status = TestStatus.Works,
            Author = "Microsoft Corp.",
            TestCaseType = TestCaseType.Generic,
            Description = new string[]
            {
                "Pre-Condition: Give info about bug",
                "Perform Regression Test",
            })]
        public void TextPatternRangeRegression1744558(TestCaseAttribute testCase)
        {
            HeaderComment(testCase);

            Bug1744558("1744558", "Avalon/WPF FlowDocument", "TextPatternRange.Compare returns true even when ranges do not match");
        }



        #endregion Regression Tests

        #endregion TestCase
    }
    #endregion TextTests class

}
