﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace WatiN.Core.Properties {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "16.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class Resources {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Resources() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("WatiN.Core.Properties.Resources", typeof(Resources).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Collection is read-only.
        /// </summary>
        internal static string BaseComponentCollection_CollectionIsReadonly {
            get {
                return ResourceManager.GetString("BaseComponentCollection_CollectionIsReadonly", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Collection does not support searching by equality..
        /// </summary>
        internal static string BaseComponentCollection_DoesNotSupportSearchingByEquality {
            get {
                return ResourceManager.GetString("BaseComponentCollection_DoesNotSupportSearchingByEquality", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The control has already been initialized..
        /// </summary>
        internal static string Control_HasAlreadyBeenInitialized {
            get {
                return ResourceManager.GetString("Control_HasAlreadyBeenInitialized", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Closing IE instance.
        /// </summary>
        internal static string IE_Dispose {
            get {
                return ResourceManager.GetString("IE_Dispose", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The page type is expected to be a subclass of Page..
        /// </summary>
        internal static string PageMetadata_PageTypeIsExpectedToBeASubclassOfPage {
            get {
                return ResourceManager.GetString("PageMetadata_PageTypeIsExpectedToBeASubclassOfPage", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to A match operation has been aborted because it appeared to be re-entrant.  The exception occurred in an instance of &apos;{0}&apos; with constraint: {1}..
        /// </summary>
        internal static string ReEntryException_MessageFormat {
            get {
                return ResourceManager.GetString("ReEntryException_MessageFormat", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to (function(){var a=/((?:\((?:\([^()]+\)|[^()]+)+\)|\[(?:\[[^\[\]]*\]|[&apos;&quot;][^&apos;&quot;]*[&apos;&quot;]|[^\[\]&apos;&quot;]+)+\]|\\.|[^ &gt;+~,(\[\\]+)+|[&gt;+~])(\s*,\s*)?((?:.|\r|\n)*)/g,b=0,c=Object.prototype.toString,d=!1,e=!0;[0,0].sort(function(){e=!1;return 0});var f=function(b,d,e,i){e=e||[],d=d||document;var j=d;if(d.nodeType!==1&amp;&amp;d.nodeType!==9)return[];if(!b||typeof b!==&quot;string&quot;)return e;var l,m,n,o,p,r,s,t,u=!0,v=f.isXML(d),w=[],x=b;do{a.exec(&quot;&quot;),l=a.exec(x);if(l){x=l[3],w.push(l[1]);if(l[2]){o=l[3];break}}}while(l);if(w.length&gt;1&amp;&amp; [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string sizzle {
            get {
                return ResourceManager.GetString("sizzle", resourceCulture);
            }
        }
    }
}
