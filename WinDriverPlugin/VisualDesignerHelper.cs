// <copyright file=VisualDesignerHelper company=E2E BOTS>
// Copyright (c) 2018 All Rights Reserved
// </copyright>
// <author>Prabodhini Funde</author>
// <date> 03-10-2018 16:15:03</date>
// <summary></summary>


using CommonLibrary;
using Gma.System.MouseKeyHook;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Automation;
using System.Windows.Forms;
//using VisualUIAVerify;
using VisualUIAVerify;
using VisualUIAVerify.Controls;
using VisualUIAVerify.Features;
using VisualUIAVerify.Misc;

namespace Bot.Activity.WinDriverPlugin
{
    public class VisualDesignerHelper
    {
        private ElementHighlighter _highlighter;
        bool StartFocus { get; set; }
        ApplicationState _applicationState = new ApplicationState();
        private VisualUIAVerify.Controls.AutomationElementTreeControl _automationElementTree;
        private IKeyboardMouseEvents m_GlobalHook;
        WindowsPropertyCreator windowsPropertyCreator = null;
        [DllImport("user32.dll")]
        static extern IntPtr GetForegroundWindow();

        [DllImport("user32.dll")]
        static extern int GetWindowText(IntPtr hWnd, StringBuilder text, int count);

        private string GetActiveWindowTitle()
        {
            const int nChars = 256;
            StringBuilder Buff = new StringBuilder(nChars);
            IntPtr handle = GetForegroundWindow();

            if (GetWindowText(handle, Buff, nChars) > 0)
            {
                return Buff.ToString();
            }
            return null;
        }
        public VisualDesignerHelper()
        {
            windowsPropertyCreator = new WindowsPropertyCreator();
            m_GlobalHook = Hook.GlobalEvents();
            m_GlobalHook.MouseDoubleClick -= M_GlobalHook_MouseDoubleClick;
            m_GlobalHook.MouseDoubleClick += M_GlobalHook_MouseDoubleClick;
        }
        public void InitializeVisualDesigner(VisualUIAVerify.Controls.AutomationElementTreeControl automationElementTree)
        {
            _automationElementTree = automationElementTree;
            StartScraping();
        }
        private void M_GlobalHook_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            SelectHelper.ScrapingWindowTitle = GetActiveWindowTitle();

            if (!SelectHelper.ScrapingWindowTitle.Contains("Automation Studio"))
            { 
                if ((SelectHelper.CurrentScrapMode == ScrapMode.Windows) && (SelectHelper.StartSimulation == true))
                {
                    SelectHelper.StartSimulation = false;
                                       
                    AutomationElement automationElement = (AutomationElement)SelectHelper.CurrentScrapControl;
                   
                    if (automationElement != null)
                    {
                        windowsPropertyCreator.CreateWindowsPropertyObject(automationElement);
                    }
                    SelectHelper.StartSimulation = true;
                }
        
            }
        }
       
        public void Unsubscribe()
        {
            m_GlobalHook.MouseDownExt -= M_GlobalHook_MouseDoubleClick;
           // m_GlobalHook.KeyPress -= GlobalHookKeyPress;
            //It is recommened to dispose it 
            m_GlobalHook.Dispose();
        }


        private void StartScraping()
        {
            this._applicationState.ModeFocusTracking = false; // focusTrackingToolStripMenuItem1.Checked;
            _automationElementTree.StartFocusTracing();
           
            switch (this._applicationState.HighLight)
            {
                case ElementHighlighterFactory.None:
                    {
                        break;
                    }
                default:
                    {

                        _highlighter = ElementHighlighterFactory.CreateHighlighterById(this._applicationState.HighLight, this._automationElementTree);

                        _highlighter.StartHighlighting();

                        break;
                    }
            }
         }

        public void StartHighlighting()
        {
            //ThreadInvoker.Instance.RunByUiThread(() =>
            //{
            if (this._highlighter != null)
            {
                this._highlighter.StartHighlighting();

            }
            //});
        }
        public void StopHighlighting()
        {
            _automationElementTree.StopFocusTracing();
            if (this._highlighter != null)
            {
                this._highlighter.Dispose();
                this._highlighter = null;
            }
        }
        // Set up pInvoke for UiaRegisterProviderCallback
        [DllImport("UIAutomationCore.dll", CharSet = CharSet.Unicode)]
        private static extern void UiaRegisterProviderCallback(IntPtr callback);

        /// <summary>
        /// Unload client side provider.  This will default to the MSAA Proxy.
        /// </summary>
        public void UnloadClientSideProviders()
        {
            // First, do something to ensure the proxy loading call has been made
            AutomationElement root = AutomationElement.RootElement;

            // Register a Null callback, this tells UI Automation to use the new proxies in Core
            UiaRegisterProviderCallback(IntPtr.Zero);

            VerifyWeHaveUIAutomation();

            //this.UnmanagedProxiesToolStripMenuItem.Enabled = false;
            //Text = string.Format("{0} : {1}", BaseTitle, "No Client Side Provider");

            AutomationElementTreeNode node = _automationElementTree.RootNode;
            if (node != null)
            {
                _automationElementTree.RefreshNode(node);
            }
        }

        /// <summary>
        /// Quick test to determine if we have a valid UIAutomationCore dynamic library.
        /// </summary>
        public static void VerifyWeHaveUIAutomation()
        {
            try
            {
                AutomationElement root = AutomationElement.RootElement;
            }
            catch (ArgumentException)
            {
                MessageBox.Show("Exception has occured that indicates you are trying to turn off 'Client Side Provider'.  You may not have the most recent version of UIAutomationCore.dll installed on your system.  Please contact your accessibility contact to find out how to obtain a newer version of UIAutomationCoredll.  Visual UIVerify / UIAutomationCore are now unstable.  Please restart Visual UIVerify to use the default Client Side Provider (Windows Vista/.NET Framework 3.0).");
            }
            catch (Exception error)
            {
                while (null != error.InnerException)
                {
                    error = error.InnerException;
                }
                MessageBox.Show(error.Message + error.GetType().ToString());
                throw error;
            }
        }
    }
}
