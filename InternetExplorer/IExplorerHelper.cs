// <copyright file=IExplorerHelper company=E2E BOTS>
// Copyright (c) 2018 All Rights Reserved
// </copyright>
// <author>Prabodhini Funde</author>
// <date> 03-10-2018 16:02:57</date>
// <summary></summary>

using System;
using mshtml;
using System.Text;
using WatiN.Core;
using Logger;

namespace Bot.Activity.InternetExplorer
{
    public class IExplorerHelper
    {

      //  public static bool UseNextPriority = false;
        public static IHTMLElement FindControlByName(string name, mshtml.HTMLDocument document)
        {
            mshtml.IHTMLElement element = null;
            try
            {
                element = document.getElementById(name);

                if (element == null)
                {
                    if (document.frames.length > 0)
                    {
                        element = SearchFrameElements(document, name);
                    }
                }
                //below code is to find menu elements
                if (element == null)
                {
                    IHTMLElementCollection anchorElementsWithourID = document.getElementsByTagName("A");
                    foreach (IHTMLElement elementsWithourID in anchorElementsWithourID)
                    {
                        if (elementsWithourID.outerText == name)
                            element = elementsWithourID;
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Logger.LogData(ex.Message, LogLevel.Error);
            }
            return element;
        }

        public static IHTMLElement SearchFrameElements(mshtml.HTMLDocument document, string name)
        {
            IHTMLElement element = null;
            try
            {
                FramesCollection frameList = document.frames;

                for (int i = 0; i < document.frames.length; i++)
                {
                    IHTMLElement frameWindow = frameList.item(i);
                    mshtml.HTMLDocument frameDoc = frameWindow.document;
                    element = frameDoc.getElementById(name);
                    if (element != null)
                    {
                        break;
                    }
                    if (element == null)//for menu
                    {
                        IHTMLElementCollection anchorElementsWithourID = document.getElementsByTagName("A");
                        foreach (IHTMLElement elementsWithourID in anchorElementsWithourID)
                        {
                            if (elementsWithourID.outerText == name)
                                element = elementsWithourID;
                            break;
                        }
                    }
                    if (element != null)
                    {
                        break;
                    }
                    if (frameDoc.frames.length > 0)
                    {
                        element = SearchFrameElements(frameDoc, name);
                        if (element != null)
                        {
                            break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Logger.LogData(ex.Message, LogLevel.Error);
            }
            return element;
        }

        public static IHTMLElement SelectHtmlElementByXPath(string xPath, IHTMLElement htmlElement)
        {
            try
            {
                string currentNodeTagName = string.Empty;
                int indexOfElement = 0;
                xPath = xPath.Replace("/HTML[1]/BODY[1]", "");
                if (xPath == "/")
                    return htmlElement;
                currentNodeTagName = xPath.Substring(1, xPath.Length - 1);
                //code added to get exact count of table elements
                int endindex = (xPath.IndexOf(']') - xPath.IndexOf('[')) - 1;
                indexOfElement = Convert.ToInt32(currentNodeTagName.Substring(xPath.IndexOf('['), endindex));
                currentNodeTagName = currentNodeTagName.Substring(0, xPath.IndexOf('[') - 1).ToLower();

                int i = 1;
                foreach (IHTMLElement child in htmlElement.children)
                {
                    if (child.tagName.ToLower() == "frameset")
                    {
                        return SelectHtmlElementByXPath(xPath.Substring(xPath.IndexOf('/', 1)), child);

                    }
                    else if (child.tagName.ToLower() == "iframe" || child.tagName.ToLower() == "frame")
                    {
                        IHTMLElementCollection frameList = htmlElement.all.tags("frame");
                        if (frameList.length >= indexOfElement)
                        {
                            System.Windows.Forms.HtmlDocument frameDoc = frameList.item(indexOfElement - 1).Document;
                            return SelectHtmlElementByXPath(xPath.Substring(xPath.IndexOf('/', 1)), child.document.getElementsByTagName(currentNodeTagName)[0]);
                        }
                    }
                    if (child.tagName.ToLower() == currentNodeTagName.ToLower())
                    {
                        if (i == indexOfElement)
                        {
                            bool flg = false;

                            if (child.tagName.ToLower() == "td")
                                return SelectHtmlElementByXPath(xPath.Substring(xPath.IndexOf('/', 1)), child);

                            foreach (var tab in child.children)
                            {
                                flg = true;
                                //if (xPath.IndexOf('/', 1) != -1)
                                break;
                                // else
                                //     return child;
                            }
                            if (flg != true)
                            {
                                return child;
                            }
                            else
                            {
                                return SelectHtmlElementByXPath(xPath.Substring(xPath.IndexOf('/', 1)), child);
                            }
                        }
                        else
                            i++;
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Logger.LogData(ex.Message, LogLevel.Error);
            }
            return null;
        }

        public static string GetXPath(dynamic element, ref bool UseNextPriority)
        {
            UseNextPriority = false;
            System.Text.StringBuilder xPath = new StringBuilder();
            int index = 1;
            try
            {
                xPath.Remove(0, xPath.Length);
                while (element != null)
                {
                    index = GetIndexByXPath(element);
                    xPath = xPath.Append(string.Format("{0}[{1}]/", element.tagName, index));
                    if (element.parentElement == null)
                    {
                        if (element.parentNode != null && (element.parentNode.frames.parent).length > 0)
                        {
                            element = element.parentNode.frames.frameElement;
                        }
                        else
                        {
                            break;
                        }
                    }
                    else
                    {
                        element = element.parentElement;
                    }
                }
            }
            catch (Exception ex)
            {
                UseNextPriority = true;
                Log.Logger.LogData("Error in generating xpath: " + ex.Message, LogLevel.Error);
                //Logging.LogException(5, "IDE.WebApplicationUserControl.WebApplicationUserControl", System.Reflection.MethodBase.GetCurrentMethod().Name, ex.Message, ex, logStackTrace: true,
                //logCategory: logCategory, instanceId: Logging.InstanceID, userId: Logging.WindowsUserIdentity, ipAddress: Logging.LocalIpAddress);
            }
            finally
            {
                //Logging.LogMessage(Logging.LogLevel.Debug, 10, "IDE.WebApplicationUserControl.WebApplicationUserControl", System.Reflection.MethodBase.GetCurrentMethod().Name, "Exiting method",
                //logCategory: logCategory, instanceId: Logging.InstanceID, userId: Logging.WindowsUserIdentity, ipAddress: Logging.LocalIpAddress);
            }
            return Reverse(Convert.ToString(xPath));
        }

        /// <summary>
        /// Get the parent elements and iterate through it's children and get the position of the homogeneous element.
        /// /// </summary>
        /// <param name="element"></param>
        /// <returns> index of the element </returns>
        private static int GetIndexByXPath(dynamic element)
        {
            int index = 1;
            try
            {
                string etagName = element.tagName;
                if (etagName == "tD")
                {
                    index = element.cellIndex + 1;
                }
                else if (etagName == "tR")
                {
                    if (element.parentElement.tagName == "tBODY" || element.parentElement.tagName == "tHEAD" || element.parentElement.tagName == "tFOOT")
                    {
                        index = element.sectionRowIndex + 1;
                    }
                    else
                    {
                        index = element.rowIndex + 1;
                    }
                }
                else
                {
                    if (element.parentElement != null && element.parentElement.canHaveChildren)
                    {
                        int j = 1;
                        foreach (IHTMLElement item in element.parentElement.children)
                        {
                            if (element.tagName.ToLower() == item.tagName.ToLower())
                            {
                                if (element == item)
                                {
                                    index = j;
                                    break;
                                }
                                j++;
                            }
                        }
                    }
                    else
                    {
                        index = 1;
                    }
                }
                return index;
            }
            catch (Exception ex)
            {
                Log.Logger.LogData(ex.Message, LogLevel.Error);
            }
            finally
            {

            }
            return index;
        }

        private static string Reverse(string s)
        {
            string xPath = string.Empty;
            try
            {
                char seprator = '/';
                string[] charArray = s.Split(seprator);
                Array.Reverse(charArray);
                
                foreach (string charAry in charArray)
                {
                    xPath = xPath + charAry + "/";
                }
            }
            catch (Exception ex)
            {
                Log.Logger.LogData(ex.Message, LogLevel.Error);
            }
            finally
            {

            }
            return xPath;
        }

        #region "watin"

        public static string TakeActionOnControlInBrowserByID(IE ie, string strID, string ctrl,bool IsEventField, bool Activate, bool ClickBeforeValueSet, bool ContinueOnError, string setValue, ref bool UseNextPriority, string strWaitUntilContainText, int  waitMaxTimeWhileLoop)
        {

            try
            {
                UseNextPriority = false;
                //IE ie = IE.AttachToIE(Find.ByUrl(ienew.Url));
                if (string.IsNullOrEmpty(setValue))
                {
                    Log.Logger.LogData("'Set Value' is not set for ID: " + strID, LogLevel.Info);
                }

                if ((ctrl.ToLower().Trim() == "span") || (ctrl.ToLower().Trim() == "td"))
                {
                    try
                    {
                        Span sp = ie.Span(Find.ById(strID));
                        //Assert.IsTrue(sp.Exists, "Could not Find: " + strID);
                        if (Activate)
                        {
                            try
                            {
                                sp.Focus();
                                Logger.Log.Logger.LogData("Control activated:" + strID, LogLevel.Info);
                            }
                            catch (Exception ex)
                            {
                                Logger.Log.Logger.LogData("Error while setting focus on " + strID, LogLevel.Warning);
                            }
                            Logger.Log.Logger.LogData("Control activated:" + strID, LogLevel.Info);
                            //   IEWATIN.WaitForComplete(5000);
                        }
                        if (ClickBeforeValueSet)
                        {
                            try
                            {
                                sp.Click();
                                Logger.Log.Logger.LogData("Control clicked:" + strID, LogLevel.Info);
                            }
                            catch (Exception ex)
                            {
                                Logger.Log.Logger.LogData("Error while before click " + strID, LogLevel.Warning);
                            }
                        }
                        if (IsEventField)
                        {
                            sp.Click();
                            Logger.Log.Logger.LogData("Control clicked:" + strID, LogLevel.Info);
                            //   IEWATIN.WaitForComplete(5000);
                        }
                        else if ((setValue != null) && (setValue != string.Empty))
                        {
                            //Not Implemented
                            Log.Logger.LogData("Not able to set data for ControlTypeSpan or td", LogLevel.Warning);
                        }
                        else
                        {
                            return sp.Text;
                        }
                    }
                    catch (Exception ex)
                    { UseNextPriority = true; }

                    return string.Empty;
                }
                else if ((ctrl.ToLower().Trim() == "textfield") || (ctrl.ToLower().Trim() == "input") || (ctrl.ToLower().Trim() == "button"))
                {

                    if (IsEventField)
                    {
                        try
                        {
                            Button btn = ie.Button(Find.ById(strID));
                            btn.Click();
                            Logger.Log.Logger.LogData("Control clicked:" + strID, LogLevel.Info);
                            ie.WaitForComplete(waitMaxTimeWhileLoop);

                            return string.Empty;

                        }
                        catch (Exception ex)
                        {
                            Log.Logger.LogData(ex.Message, LogLevel.Warning);
                            UseNextPriority = true;
                        }
                    }
                    else
                    {
                        try
                        {
                            TextField txt = ie.TextField(Find.ById(strID));
                            if (Activate)
                            {
                                try
                                {
                                    txt.Focus();
                                    Logger.Log.Logger.LogData("Control activated:" + strID, LogLevel.Info);
                                }
                                catch (Exception ex)
                                {
                                    Logger.Log.Logger.LogData("Error while setting focus on " + strID, LogLevel.Warning);
                                }

                                //   IEWATIN.WaitForComplete(waitMaxTimeWhileLoop);
                            }
                            if (ClickBeforeValueSet)
                            {
                                txt.Click();
                                Logger.Log.Logger.LogData("Control clicked:" + strID, LogLevel.Info);
                                //   IEWATIN.WaitForComplete(waitMaxTimeWhileLoop);
                            }
                            if ((setValue != null) && (setValue != string.Empty))
                            {
                                txt.Value = setValue;
                                Logger.Log.Logger.LogData("Control value set:" + strID, LogLevel.Info);
                            }
                            else
                            {
                                Logger.Log.Logger.LogData("Control value get:" + strID, LogLevel.Info);
                                return txt.Text;
                            }
                        }
                        catch (Exception ex)
                        {
                            Log.Logger.LogData(ex.Message, LogLevel.Error);
                            UseNextPriority = true;
                        }
                    }

                    return string.Empty;
                }
                else if (ctrl.ToLower().Trim() == "link")
                {
                    try
                    {
                        Link lnk = ie.Link(Find.ById(strID));
                        if (ClickBeforeValueSet)
                        {
                            try
                            {
                                lnk.Click();
                                Logger.Log.Logger.LogData("Control clicked:" + strID, LogLevel.Info);
                            }
                            catch (Exception ex)
                            {
                                Logger.Log.Logger.LogData("Error while before click " + strID, LogLevel.Warning);
                            }
                        }
                        if (IsEventField)
                        {
                            lnk.Click();
                            Logger.Log.Logger.LogData("Link Control clicked:" + strID, LogLevel.Info);
                        }
                        else
                        {
                            Logger.Log.Logger.LogData("Link control value get:" + strID, LogLevel.Info);
                            return lnk.Text;
                        }
                    }
                    catch (Exception ex)
                    {
                        Log.Logger.LogData(ex.Message, LogLevel.Warning);
                        UseNextPriority = true;
                    }

                    return string.Empty;
                }

                else if (ctrl.ToLower().Trim() == "p")
                {
                    try
                    {
                        Para iPara = ie.Para(Find.ById(strID));
                        if (ClickBeforeValueSet)
                        {
                            try
                            {
                                iPara.Click();
                                Logger.Log.Logger.LogData("Control clicked:" + strID, LogLevel.Info);
                            }
                            catch (Exception ex)
                            {
                                Logger.Log.Logger.LogData("Error while control click " + strID, LogLevel.Error);
                            }
                            //   IEWATIN.WaitForComplete(5000);
                        }
                        if (IsEventField)
                        {
                            iPara.Click();
                            Logger.Log.Logger.LogData("Control clicked: Class Name=" + strID, LogLevel.Info);
                        }
                        else
                        {
                            Logger.Log.Logger.LogData("Control value get:" + strID, LogLevel.Info);
                            return iPara.Text;
                        }
                    }
                    catch (Exception ex)
                    {
                        Log.Logger.LogData(ex.Message, LogLevel.Warning);
                        UseNextPriority = true;
                    }

                    return string.Empty;
                }
                else if (ctrl.ToLower().Trim() == "label")
                {
                    try
                    {
                        Label label = ie.Label(Find.ById(strID));
                        if (ClickBeforeValueSet)
                        {
                            try
                            {
                                label.Click();
                                Logger.Log.Logger.LogData("Control clicked:" + strID, LogLevel.Info);
                            }
                            catch (Exception ex)
                            {
                                Logger.Log.Logger.LogData("Error while before click " + strID, LogLevel.Warning);
                            }
                        }
                        if (IsEventField)
                        {
                            label.Click();
                            Logger.Log.Logger.LogData("Label Control clicked:" + strID, LogLevel.Info);
                        }
                        else
                        {
                            Logger.Log.Logger.LogData("Label control value get:" + strID, LogLevel.Info);
                            return label.Text;
                        }
                    }
                    catch (Exception ex)
                    {
                        Log.Logger.LogData(ex.Message, LogLevel.Warning);
                        UseNextPriority = true;
                    }

                    return string.Empty;
                }
                else if (ctrl.ToLower().Trim() == "frame")
                {
                    try
                    {
                        Frame iFrame = ie.Frame(Find.ById(strID));

                        Logger.Log.Logger.LogData("Frame control value get:" + strID, LogLevel.Info);
                        return iFrame.Text;
                    }
                    catch (Exception ex)
                    {
                        Log.Logger.LogData(ex.Message, LogLevel.Warning);
                        UseNextPriority = true;
                    }

                    return string.Empty;
                }
                else if (ctrl.ToLower().Trim() == "img")
                {
                    try
                    {
                        Image img = ie.Image(Find.ById(strID));
                        if (ClickBeforeValueSet)
                        {
                            img.Click();
                            Logger.Log.Logger.LogData("Control clicked:" + strID, LogLevel.Info);
                            //   IEWATIN.WaitForComplete(5000);
                        }
                        if (IsEventField)
                        {
                            img.Click();
                            Logger.Log.Logger.LogData("Image control clicked:" + strID, LogLevel.Info);
                        }
                        else
                        {
                            Logger.Log.Logger.LogData("Image control value get:" + strID, LogLevel.Info);
                            return img.Text;
                        }
                    }
                    catch (Exception ex)
                    {
                        Log.Logger.LogData(ex.Message, LogLevel.Warning);
                        UseNextPriority = true;
                    }

                    return string.Empty;
                }
                else if (ctrl.ToLower().Trim() == "tablecell")
                {
                    try
                    {
                        TableCell tCell = ie.TableCell(Find.ById(strID));
                        //Assert.IsTrue(tCell.Exists, "Could not Find: " + strID);
                        if (ClickBeforeValueSet)
                        {
                            tCell.Click();
                            Logger.Log.Logger.LogData("Control clicked:" + strID, LogLevel.Info);
                            //   IEWATIN.WaitForComplete(5000);
                        }
                        if (IsEventField)
                        {
                            tCell.Click();
                            Logger.Log.Logger.LogData("Table cell control clicked:" + strID, LogLevel.Info);
                        }
                        else
                        {
                            Logger.Log.Logger.LogData("Table cell control value get:" + strID, LogLevel.Info);
                            return tCell.Text;
                        }
                    }
                    catch (Exception ex)
                    {
                        Log.Logger.LogData(ex.Message, LogLevel.Warning);
                        UseNextPriority = true;
                    }

                    return string.Empty;
                }
                else if (ctrl.ToLower().Trim() == "table")
                {
                    try
                    {
                        Table tbl = ie.Table(Find.ById(strID));
                        if (ClickBeforeValueSet)
                        {
                            tbl.Click();
                            Logger.Log.Logger.LogData("Control clicked:" + strID, LogLevel.Info);
                            //   IEWATIN.WaitForComplete(5000);
                        }
                        if (IsEventField)
                        {
                            tbl.Click();
                            Logger.Log.Logger.LogData("Table control clicked:" + strID, LogLevel.Info);
                        }
                        else
                        {
                            Logger.Log.Logger.LogData("Table control value get:" + strID, LogLevel.Info);
                            return tbl.Text;
                        }
                    }
                    catch (Exception ex)
                    {
                        Log.Logger.LogData(ex.Message, LogLevel.Warning);
                        UseNextPriority = true;
                    }

                    return string.Empty;
                }
                else if (ctrl.ToLower().Trim() == "tablerow")
                {
                    try
                    {
                        TableRow row = ie.TableRow(Find.ById(strID));
                        if (ClickBeforeValueSet)
                        {
                            row.Click();
                            Logger.Log.Logger.LogData("Control clicked:" + strID, LogLevel.Info);
                            //   IEWATIN.WaitForComplete(5000);
                        }
                        if (IsEventField)
                        {
                            row.Click();
                            Logger.Log.Logger.LogData("Table row control clicked:" + strID, LogLevel.Info);
                        }
                        else
                        {
                            Logger.Log.Logger.LogData("Table row control value get:" + strID, LogLevel.Info);
                            return row.Text;
                        }
                    }
                    catch (Exception ex)
                    {
                        Log.Logger.LogData(ex.Message, LogLevel.Warning);
                        UseNextPriority = true;
                    }

                    return string.Empty;
                }
                else if (ctrl.ToLower().Trim() == "checkbox")
                {
                    try
                    {
                        CheckBox chk = ie.CheckBox(Find.ById(strID));
                        if (ClickBeforeValueSet)
                        {
                            chk.Click();
                            Logger.Log.Logger.LogData("Control clicked:" + strID, LogLevel.Info);
                            //   IEWATIN.WaitForComplete(5000);
                        }
                        if (IsEventField)
                        {
                            chk.Click();
                            Logger.Log.Logger.LogData("Checkbox control clicked:" + strID, LogLevel.Info);
                        }
                        else
                        {
                            Logger.Log.Logger.LogData("Checkbox control value get:" + strID, LogLevel.Info);
                            return chk.Text;
                        }
                    }
                    catch (Exception ex)
                    {
                        Log.Logger.LogData(ex.Message, LogLevel.Warning);
                        UseNextPriority = true;
                    }

                    return string.Empty;
                }

                else if ((ctrl.ToLower().Trim() == "selectlist") || (ctrl.ToLower().Trim() == "select"))
                {
                    try
                    {
                        SelectList sList = ie.SelectList(Find.ById(strID));
                        if (ClickBeforeValueSet)
                        {
                            sList.Click();
                            Logger.Log.Logger.LogData("Control clicked:" + strID, LogLevel.Info);
                            //   IEWATIN.WaitForComplete(5000);
                        }
                        if (IsEventField)
                        {
                            sList.Click();
                            Logger.Log.Logger.LogData("Selectlist control clicked:" + strID, LogLevel.Info);
                        }
                        else if ((setValue != null) && (setValue != string.Empty))
                        {

                            sList.Select(setValue);
                        }
                        else
                        {
                            Logger.Log.Logger.LogData("selectlist control value get:" + strID, LogLevel.Info);
                            return sList.Text;
                        }
                    }
                    catch (Exception ex)
                    {
                        Log.Logger.LogData(ex.Message, LogLevel.Warning);
                        UseNextPriority = true;
                    }

                    return string.Empty;
                }
                else if (ctrl.ToLower().Trim() == "div")
                {
                    try
                    {
                        Div division = ie.Div(Find.ById(strID));
                        if (ClickBeforeValueSet)
                        {
                            division.Click();
                            Logger.Log.Logger.LogData("Control clicked:" + strID, LogLevel.Info);
                            //   IEWATIN.WaitForComplete(5000);
                        }
                        if (IsEventField)
                        {
                            division.Click();
                            Logger.Log.Logger.LogData("Division control clicked:" + strID, LogLevel.Info);
                        }
                        else
                        {
                            Logger.Log.Logger.LogData("Division control value get:" + strID, LogLevel.Info);
                            return division.Text;
                        }
                    }
                    catch (Exception ex)
                    {
                        Log.Logger.LogData(ex.Message, LogLevel.Warning);
                        UseNextPriority = true;
                    }

                    return string.Empty;
                }
                else if (ctrl.ToLower().Trim() == "tablerow")
                {
                    try
                    {
                        TableRow tRow = ie.TableRow(Find.ById(strID));
                        if (ClickBeforeValueSet)
                        {
                            tRow.Click();
                            Logger.Log.Logger.LogData("Control clicked:" + strID, LogLevel.Info);
                            //   IEWATIN.WaitForComplete(5000);
                        }
                        if (IsEventField)
                        {
                            tRow.Click();
                            Logger.Log.Logger.LogData("Table row control clicked:" + strID, LogLevel.Info);
                        }
                        Logger.Log.Logger.LogData("Table row control value get:" + strID, LogLevel.Info);
                        return tRow.Text;
                    }
                    catch (Exception ex)
                    {
                        Log.Logger.LogData(ex.Message, LogLevel.Warning);
                        UseNextPriority = true;
                    }

                    return string.Empty;
                }
                else if (ctrl.ToLower().Trim() == "fileupload")
                {
                    try
                    {
                        FileUpload fileUpload = ie.FileUpload(Find.ById(strID));
                        if (ClickBeforeValueSet)
                        {
                            fileUpload.Click();
                            Logger.Log.Logger.LogData("Control clicked:" + strID, LogLevel.Info);
                            //   IEWATIN.WaitForComplete(5000);
                        }
                        if (IsEventField)
                        {
                            fileUpload.Click();
                            Logger.Log.Logger.LogData("Fileupload control clicked:" + strID, LogLevel.Info);
                        }
                        else
                        {
                            Logger.Log.Logger.LogData("Fileupload control value get:" + strID, LogLevel.Info);
                            return fileUpload.Text;
                        }
                    }
                    catch (Exception ex)
                    {
                        Log.Logger.LogData(ex.Message, LogLevel.Warning);
                        UseNextPriority = true;
                    }

                    return string.Empty;
                }
                else
                {
                    return null;
                }
            } catch (Exception ex)
            {
                Log.Logger.LogData(ex.Message, LogLevel.Error);
            }
            return string.Empty;
        }

        public static string TakeActionOnControlInBrowserByName(IE ie, string strName, string ctrl, bool IsEventField, bool Activate, bool ClickBeforeValueSet, bool ContinueOnError, string setValue, ref bool UseNextPriority, string strWaitUntilContainText, int waitMaxTimeWhileLoop)
        {
            try
            {
                UseNextPriority = false;
                //IE ie = IE.AttachToIE(Find.ByUrl(ienew.Url));
                if (string.IsNullOrEmpty(setValue))
                {
                    Log.Logger.LogData("'Set Value' is not set for Control Name: " + strName, LogLevel.Info);
                }
                if ((ctrl.ToLower().Trim() == "span") || (ctrl.ToLower().Trim() == "td"))
                {
                    try
                    {
                        Span sp = ie.Span(Find.ByName(strName));
                        //Assert.IsTrue(sp.Exists, "Could not Find: " + strName);
                        if (Activate)
                        {
                            try
                            {
                                sp.Focus();
                                Logger.Log.Logger.LogData("Control activated:" + strName, LogLevel.Info);
                            }
                            catch (Exception ex)
                            {
                                Logger.Log.Logger.LogData("Error while setting focus on " + strName, LogLevel.Warning);
                            }
                            Logger.Log.Logger.LogData("Control activated:" + strName, LogLevel.Info);
                            //   IEWATIN.WaitForComplete(waitMaxTimeWhileLoop);
                        }
                        if (ClickBeforeValueSet)
                        {
                            sp.Click();
                            Logger.Log.Logger.LogData("Control clicked:" + strName, LogLevel.Info);
                            //   IEWATIN.WaitForComplete(waitMaxTimeWhileLoop);
                        }
                        if (IsEventField)
                        {
                            sp.Click();
                            Logger.Log.Logger.LogData("Span control clicked:" + strName, LogLevel.Info);
                            ie.WaitForComplete(waitMaxTimeWhileLoop);
                        }
                        Logger.Log.Logger.LogData("Span control value get:" + strName, LogLevel.Info);
                        return sp.Text;
                    }
                    catch (Exception ex)
                    {
                        Log.Logger.LogData(ex.Message, LogLevel.Warning);
                        UseNextPriority = true;
                    }

                    return string.Empty;
                }
                else if ((ctrl.ToLower().Trim() == "textfield") || (ctrl.ToLower().Trim() == "input") || (ctrl.ToLower().Trim() == "button"))
                {
                    if (IsEventField)
                    {
                        try
                        {
                            //ie.WaitForComplete(5000);
                            Button btn = ie.Button(Find.ByName(strName));
                            btn.Click();
                            Logger.Log.Logger.LogData("Textfield control clicked:" + strName, LogLevel.Info);
                            ie.WaitForComplete(waitMaxTimeWhileLoop);
                            return string.Empty;

                        }
                        catch (Exception ex)
                        {
                            Log.Logger.LogData(ex.Message, LogLevel.Warning);
                            UseNextPriority = true;
                        }
                    }
                    else
                    {
                        try
                        {
                            TextField txt = ie.TextField(Find.ByName(strName));
                            if (Activate)
                            {
                                try
                                {
                                    txt.Focus();
                                    Logger.Log.Logger.LogData("Control activated:" + strName, LogLevel.Info);
                                }
                                catch (Exception ex)
                                {
                                    Logger.Log.Logger.LogData("Error while setting focus on " + strName, LogLevel.Warning);
                                }
                                Logger.Log.Logger.LogData("Control activated:" + strName, LogLevel.Info);
                                //   IEWATIN.WaitForComplete(5000);
                            }
                            if (ClickBeforeValueSet)
                            {
                                txt.Click();
                            }

                            if (!string.IsNullOrEmpty(setValue))
                            {
                                txt.Value = setValue;
                                Logger.Log.Logger.LogData("Control value set:" + strName, LogLevel.Info);
                            }
                            Logger.Log.Logger.LogData("Control value get:" + strName, LogLevel.Info);
                            return txt.Text;
                        }
                        catch (Exception ex)
                        {
                            Log.Logger.LogData(ex.Message, LogLevel.Warning);
                            UseNextPriority = true;
                        }
                    }
                    return string.Empty;
                }
                else if (ctrl.ToLower().Trim() == "link")
                {
                    try
                    {
                        Link lnk = ie.Link(Find.ByName(strName));
                        if (ClickBeforeValueSet)
                        {
                            lnk.Click();
                            Logger.Log.Logger.LogData("Control clicked:" + strName, LogLevel.Info);
                            //   IEWATIN.WaitForComplete(5000);
                        }
                        if (IsEventField)
                        {
                            lnk.Click();
                            Logger.Log.Logger.LogData("Control clicked:" + strName, LogLevel.Info);
                        }
                        Logger.Log.Logger.LogData("Control value get:" + strName, LogLevel.Info);
                        return lnk.Text;
                    }
                    catch (Exception ex)
                    {
                        Log.Logger.LogData(ex.Message, LogLevel.Warning);
                        UseNextPriority = true;
                    }

                    return string.Empty;
                }

                else if (ctrl.ToLower().Trim() == "p")
                {
                    try
                    {
                        Para iPara = ie.Para(Find.ByName(strName));
                        if (ClickBeforeValueSet)
                        {
                            try
                            {
                                iPara.Click();
                                Logger.Log.Logger.LogData("Control clicked:" + strName, LogLevel.Info);
                            }
                            catch (Exception ex)
                            {
                                Logger.Log.Logger.LogData("Error while control click " + strName, LogLevel.Error);
                            }
                            //   IEWATIN.WaitForComplete(5000);
                        }
                        if (IsEventField)
                        {
                            iPara.Click();
                            Logger.Log.Logger.LogData("Control clicked: Class Name=" + strName, LogLevel.Info);
                        }
                        else
                        {
                            Logger.Log.Logger.LogData("Control value get:" + strName, LogLevel.Info);
                            return iPara.Text;
                        }
                    }
                    catch (Exception ex)
                    {
                        Log.Logger.LogData(ex.Message, LogLevel.Warning);
                        UseNextPriority = true;
                    }

                    return string.Empty;
                }
                else if (ctrl.ToLower().Trim() == "frame")
                {
                    try
                    {
                        Frame iFrame = ie.Frame(Find.ByName(strName));
                        Logger.Log.Logger.LogData("Control value get:" + strName, LogLevel.Info);
                        return iFrame.Text;
                    }
                    catch (Exception ex)
                    {
                        Log.Logger.LogData(ex.Message, LogLevel.Warning);
                        UseNextPriority = true;
                    }

                    return string.Empty;
                }
                else if (ctrl.ToLower().Trim() == "label")
                {
                    try
                    {
                        Label label = ie.Label(Find.ById(strName));
                        if (ClickBeforeValueSet)
                        {
                            try
                            {
                                label.Click();
                                Logger.Log.Logger.LogData("Control clicked:" + strName, LogLevel.Info);
                            }
                            catch (Exception ex)
                            {
                                Logger.Log.Logger.LogData("Error while before click " + strName, LogLevel.Warning);
                            }
                        }
                        if (IsEventField)
                        {
                            label.Click();
                            Logger.Log.Logger.LogData("Label Control clicked:" + strName, LogLevel.Info);
                        }
                        else
                        {
                            Logger.Log.Logger.LogData("Label control value get:" + strName, LogLevel.Info);
                            return label.Text;
                        }
                    }
                    catch (Exception ex)
                    {
                        Log.Logger.LogData(ex.Message, LogLevel.Warning);
                        UseNextPriority = true;
                    }

                    return string.Empty;
                }
                else if (ctrl.ToLower().Trim() == "img")
                {
                    try
                    {
                        Image img = ie.Image(Find.ByName(strName));
                        if (ClickBeforeValueSet)
                        {
                            img.Click();
                            Logger.Log.Logger.LogData("Control clicked:" + strName, LogLevel.Info);
                            //   IEWATIN.WaitForComplete(5000);
                        }
                        if (IsEventField)
                        {
                            img.Click();
                            Logger.Log.Logger.LogData("Control clicked:" + strName, LogLevel.Info);
                        }
                        Logger.Log.Logger.LogData("Control value get:" + strName, LogLevel.Info);
                        return img.Text;
                    }
                    catch (Exception ex)
                    {
                        Log.Logger.LogData(ex.Message, LogLevel.Warning);
                        UseNextPriority = true;
                    }

                    return string.Empty;
                }
                else if (ctrl.ToLower().Trim() == "tablecell")
                {
                    TableCell tCell = ie.TableCell(Find.ByName(strName));
                    //Assert.IsTrue(tCell.Exists, "Could not Find: " + strName);
                    if (ClickBeforeValueSet)
                    {
                        tCell.Click();
                        Logger.Log.Logger.LogData("Control clicked:" + strName, LogLevel.Info);
                        //   IEWATIN.WaitForComplete(5000);
                    }
                    if (IsEventField)
                    {
                        tCell.Click();
                        Logger.Log.Logger.LogData("Control clicked:" + strName, LogLevel.Info);
                    }
                    Logger.Log.Logger.LogData("Control value get:" + strName, LogLevel.Info);
                    return tCell.Text;
                }
                else if (ctrl.ToLower().Trim() == "table")
                {
                    Table tbl = ie.Table(Find.ByName(strName));
                    if (ClickBeforeValueSet)
                    {
                        tbl.Click();
                        Logger.Log.Logger.LogData("Control clicked:" + strName, LogLevel.Info);
                        //   IEWATIN.WaitForComplete(5000);
                    }
                    if (IsEventField)
                    {
                        tbl.Click();
                        Logger.Log.Logger.LogData("Control clicked:" + strName, LogLevel.Info);
                    }
                    Logger.Log.Logger.LogData("Control value get:" + strName, LogLevel.Info);
                    return tbl.Text;
                }
                else if (ctrl.ToLower().Trim() == "tablerow")
                {
                    try
                    {
                        TableRow row = ie.TableRow(Find.ByName(strName));
                        if (ClickBeforeValueSet)
                        {
                            row.Click();
                            Logger.Log.Logger.LogData("Control clicked:" + strName, LogLevel.Info);
                            //   IEWATIN.WaitForComplete(5000);
                        }
                        if (IsEventField)
                        {
                            row.Click();
                            Logger.Log.Logger.LogData("Control clicked:" + strName, LogLevel.Info);
                        }
                        Logger.Log.Logger.LogData("Control value get:" + strName, LogLevel.Info);
                        return row.Text;
                    }
                    catch (Exception ex)
                    {
                        Log.Logger.LogData(ex.Message, LogLevel.Warning);
                        UseNextPriority = true;
                    }

                    return string.Empty;
                }
                else if (ctrl.ToLower().Trim() == "checkbox")
                {
                    try
                    {
                        CheckBox chk = ie.CheckBox(Find.ByName(strName));
                        if (ClickBeforeValueSet)
                        {
                            chk.Click();
                            Logger.Log.Logger.LogData("Control clicked:" + strName, LogLevel.Info);
                            //   IEWATIN.WaitForComplete(5000);
                        }
                        if (IsEventField)
                        {
                            chk.Click();
                            Logger.Log.Logger.LogData("Control clicked:" + strName, LogLevel.Info);
                        }
                        Logger.Log.Logger.LogData("Control value get:" + strName, LogLevel.Info);
                        return chk.Text;
                    }
                    catch (Exception ex)
                    {
                        Log.Logger.LogData(ex.Message, LogLevel.Warning);
                        UseNextPriority = true;
                    }
                    return string.Empty;
                }
                else if ((ctrl.ToLower().Trim() == "selectlist") || (ctrl.ToLower().Trim() == "select"))
                {
                    try
                    {
                        SelectList sList = ie.SelectList(Find.ByName(strName));
                        if (ClickBeforeValueSet)
                        {
                            sList.Click();
                            Logger.Log.Logger.LogData("Control clicked:" + strName, LogLevel.Info);
                            //   IEWATIN.WaitForComplete(5000);
                        }
                        if (IsEventField)
                        {
                            sList.Click();
                            Logger.Log.Logger.LogData("Control clicked:" + strName, LogLevel.Info);
                        }
                        else if ((setValue != null) && (setValue != string.Empty))
                        {

                            sList.Select(setValue);
                        }
                        Logger.Log.Logger.LogData("Control value get:" + strName, LogLevel.Info);
                        return sList.Text;
                    }
                    catch (Exception ex)
                    {
                        Log.Logger.LogData(ex.Message, LogLevel.Error);
                        UseNextPriority = true;
                    }
                    return string.Empty;
                }
                else if (ctrl.ToLower().Trim() == "div")
                {
                    try
                    {
                        Div division = ie.Div(Find.ByName(strName));
                        if (ClickBeforeValueSet)
                        {
                            division.Click();
                            Logger.Log.Logger.LogData("Control clicked:" + strName, LogLevel.Info);
                            //   IEWATIN.WaitForComplete(5000);
                        }
                        if (IsEventField)
                        {
                            division.Click();
                            Logger.Log.Logger.LogData("Control clicked:" + strName, LogLevel.Info);
                        }
                        Logger.Log.Logger.LogData("Control value get:" + strName, LogLevel.Info);
                        return division.Text;
                    }
                    catch (Exception ex)
                    {
                        Log.Logger.LogData(ex.Message, LogLevel.Error);
                        UseNextPriority = true;
                    }

                    return string.Empty;
                }
                else if (ctrl.ToLower().Trim() == "tablerow")
                {
                    try
                    {
                        TableRow tRow = ie.TableRow(Find.ByName(strName));
                        if (ClickBeforeValueSet)
                        {
                            tRow.Click();
                            Logger.Log.Logger.LogData("Control clicked:" + strName, LogLevel.Info);
                            //   IEWATIN.WaitForComplete(5000);
                        }
                        if (IsEventField)
                        {
                            tRow.Click();
                            Logger.Log.Logger.LogData("Control clicked:" + strName, LogLevel.Info);
                        }
                        return tRow.Text;
                    }
                    catch (Exception ex)
                    {
                        Log.Logger.LogData(ex.Message, LogLevel.Warning);
                        UseNextPriority = true;
                    }

                    return string.Empty;
                }
                else if (ctrl.ToLower().Trim() == "fileupload")
                {
                    try
                    {
                        FileUpload fileUpload = ie.FileUpload(Find.ByName(strName));
                        if (ClickBeforeValueSet)
                        {
                            fileUpload.Click();
                            Logger.Log.Logger.LogData("Control clicked:" + strName, LogLevel.Info);
                            //   IEWATIN.WaitForComplete(5000);
                        }
                        if (IsEventField)
                        {
                            fileUpload.Click();
                            Logger.Log.Logger.LogData("Control clicked:" + strName, LogLevel.Info);
                        }
                        Logger.Log.Logger.LogData("Control value get:" + strName, LogLevel.Info);
                        return fileUpload.Text;
                    }
                    catch (Exception ex)
                    {
                        Log.Logger.LogData(ex.Message, LogLevel.Warning);
                        UseNextPriority = true;
                    }

                    return string.Empty;
                }
                else
                {
                    return string.Empty;
                }
            } catch (Exception ex)
            {
                Log.Logger.LogData(ex.Message, LogLevel.Error);
            }
            return string.Empty;
        }

        public static string TakeActionOnControlInBrowserByClassName(IE ie, string strClassName, string ctrl, bool IsEventField, bool Activate, bool ClickBeforeValueSet, bool ContinueOnError, string setValue, ref bool UseNextPriority, string strWaitUntilContainText, int waitMaxTimeWhileLoop)
        {
            try
            {
                UseNextPriority = false;
                if (string.IsNullOrEmpty(setValue))
                {
                    Log.Logger.LogData("'Set Value' is not set for Control Class Name: " + strClassName, LogLevel.Info);
                }
                if ((ctrl.ToLower().Trim() == "span") || (ctrl.ToLower().Trim() == "td"))
                {
                    try
                    {
                        Span sp = ie.Span(Find.ByClass(strClassName));
                        if (Activate)
                        {
                            try
                            {
                                sp.Focus();
                                Logger.Log.Logger.LogData("Control activated:" + strClassName, LogLevel.Info);
                            }
                            catch (Exception ex)
                            {
                                Logger.Log.Logger.LogData("Error while setting focus on " + strClassName, LogLevel.Error);
                            }
                            Logger.Log.Logger.LogData("Control activated:" + strClassName, LogLevel.Info);
                            //   IEWATIN.WaitForComplete(5000);
                        }
                        if (ClickBeforeValueSet)
                        {
                            try
                            {
                                sp.Click();
                                Logger.Log.Logger.LogData("Control clicked:" + strClassName, LogLevel.Info);
                            }
                            catch (Exception ex)
                            {
                                Logger.Log.Logger.LogData("Error while control click " + strClassName, LogLevel.Error);
                            }
                            //   IEWATIN.WaitForComplete(5000);
                        }
                        //Assert.IsTrue(sp.Exists, "Could not Find: " + strClassName);
                        if (IsEventField)
                        {
                            sp.Click();
                            Logger.Log.Logger.LogData("Control clicked:" + strClassName, LogLevel.Info);
                            ie.WaitForComplete(5000);
                        }
                        else
                        {
                            Logger.Log.Logger.LogData("Control value get:" + strClassName, LogLevel.Info);
                            return sp.Text;
                        }
                    }
                    catch (Exception ex)
                    {
                        Log.Logger.LogData(ex.Message, LogLevel.Warning);
                        UseNextPriority = true;
                    }

                    return string.Empty;
                }
                else if ((ctrl.ToLower().Trim() == "textfield") || (ctrl.ToLower().Trim() == "input") || (ctrl.ToLower().Trim() == "button"))
                {
                    if (IsEventField)
                    {
                        try
                        {
                            Button btn = ie.Button(Find.ByClass(strClassName));
                            btn.Click();
                            Logger.Log.Logger.LogData("Control clicked: Class Name=" + strClassName, LogLevel.Info);
                            ie.WaitForComplete(waitMaxTimeWhileLoop);
                            return string.Empty;

                        }
                        catch (Exception ex)
                        {
                            Log.Logger.LogData(ex.Message, LogLevel.Warning);
                            UseNextPriority = true;
                        }
                    }
                    else
                    {
                        try
                        {
                            TextField txt = ie.TextField(Find.ByClass(strClassName));
                            if (Activate)
                            {
                                try
                                {
                                    txt.Focus();
                                    Logger.Log.Logger.LogData("Control activated:" + strClassName, LogLevel.Info);
                                }
                                catch (Exception ex)
                                {
                                    Logger.Log.Logger.LogData("Error while setting focus on " + strClassName, LogLevel.Error);
                                }
                                Logger.Log.Logger.LogData("Control activated:" + strClassName, LogLevel.Info);
                                //   IEWATIN.WaitForComplete(5000);
                            }
                            if (ClickBeforeValueSet)
                            {
                                try
                                {
                                    txt.Click();
                                }
                                catch (Exception ex)
                                {
                                    Logger.Log.Logger.LogData("Error while control click " + strClassName, LogLevel.Error);
                                }
                            }
                            if ((setValue != null) && (setValue != string.Empty))
                            {
                                txt.Value = setValue;
                                Logger.Log.Logger.LogData("Control value set:" + strClassName, LogLevel.Info);
                            }
                            else
                            {
                                Logger.Log.Logger.LogData("Control value get:" + strClassName, LogLevel.Info);
                                return txt.Text;
                            }
                        }
                        catch (Exception ex)
                        {
                            Log.Logger.LogData(ex.Message, LogLevel.Warning);
                            UseNextPriority = true;
                        }
                    }
                    return string.Empty;
                }
                else if (ctrl.ToLower().Trim() == "link")
                {
                    try
                    {
                        Link lnk = ie.Link(Find.ByClass(strClassName));
                        if (ClickBeforeValueSet)
                        {
                            try
                            {
                                lnk.Click();
                                Logger.Log.Logger.LogData("Control clicked:" + strClassName, LogLevel.Info);
                            }
                            catch (Exception ex)
                            {
                                Logger.Log.Logger.LogData("Error while control click " + strClassName, LogLevel.Error);
                            }
                            //   IEWATIN.WaitForComplete(5000);
                        }
                        if (IsEventField)
                        {
                            lnk.Click();
                            Logger.Log.Logger.LogData("Control clicked: Class Name=" + strClassName, LogLevel.Info);
                        }
                        else
                        {
                            Logger.Log.Logger.LogData("Control value get:" + strClassName, LogLevel.Info);
                            return lnk.Text;
                        }
                    }
                    catch (Exception ex)
                    {
                        Log.Logger.LogData(ex.Message, LogLevel.Warning);
                        UseNextPriority = true;
                    }

                    return string.Empty;
                }

                else if (ctrl.ToLower().Trim() == "p")
                {
                    try
                    {
                        Para iPara = ie.Para(Find.ByClass(strClassName));
                        if (ClickBeforeValueSet)
                        {
                            try
                            {
                                iPara.Click();
                                Logger.Log.Logger.LogData("Control clicked:" + strClassName, LogLevel.Info);
                            }
                            catch (Exception ex)
                            {
                                Logger.Log.Logger.LogData("Error while control click " + strClassName, LogLevel.Error);
                            }
                            //   IEWATIN.WaitForComplete(5000);
                        }
                        if (IsEventField)
                        {
                            iPara.Click();
                            Logger.Log.Logger.LogData("Control clicked: Class Name=" + strClassName, LogLevel.Info);
                        }
                        else
                        {
                            Logger.Log.Logger.LogData("Control value get:" + strClassName, LogLevel.Info);
                            return iPara.Text;
                        }
                    }
                    catch (Exception ex)
                    {
                        Log.Logger.LogData(ex.Message, LogLevel.Warning);
                        UseNextPriority = true;
                    }

                    return string.Empty;
                }
                else if (ctrl.ToLower().Trim() == "frame")
                {
                    try
                    {
                        Frame iFrame = ie.Frame(Find.ByClass(strClassName));
                        Logger.Log.Logger.LogData("Control value get:" + strClassName, LogLevel.Info);
                        return iFrame.Text;
                    }
                    catch (Exception ex)
                    {
                        Log.Logger.LogData(ex.Message, LogLevel.Warning);
                        UseNextPriority = true;
                    }

                    return string.Empty;
                }
                else if (ctrl.ToLower().Trim() == "label")
                {
                    try
                    {
                        Label label = ie.Label(Find.ById(strClassName));
                        if (ClickBeforeValueSet)
                        {
                            try
                            {
                                label.Click();
                                Logger.Log.Logger.LogData("Control clicked:" + strClassName, LogLevel.Info);
                            }
                            catch (Exception ex)
                            {
                                Logger.Log.Logger.LogData("Error while before click " + strClassName, LogLevel.Warning);
                            }
                        }
                        if (IsEventField)
                        {
                            label.Click();
                            Logger.Log.Logger.LogData("Label Control clicked:" + strClassName, LogLevel.Info);
                        }
                        else
                        {
                            Logger.Log.Logger.LogData("Label control value get:" + strClassName, LogLevel.Info);
                            return label.Text;
                        }
                    }
                    catch (Exception ex)
                    {
                        Log.Logger.LogData(ex.Message, LogLevel.Warning);
                        UseNextPriority = true;
                    }

                    return string.Empty;
                }
                else if (ctrl.ToLower().Trim() == "img")
                {
                    try
                    {
                        Image img = ie.Image(Find.ByClass(strClassName));
                        if (ClickBeforeValueSet)
                        {
                            img.Click();
                            Logger.Log.Logger.LogData("Control clicked:" + strClassName, LogLevel.Info);
                            //   IEWATIN.WaitForComplete(5000);
                        }
                        if ((IsEventField) || (ClickBeforeValueSet))
                        {
                            img.Click();
                            Logger.Log.Logger.LogData("Control clicked: Class Name=" + strClassName, LogLevel.Info);
                        }
                        else
                        {
                            Logger.Log.Logger.LogData("Control value get:" + strClassName, LogLevel.Info);
                            return img.Text;
                        }
                    }
                    catch (Exception ex)
                    {
                        Log.Logger.LogData(ex.Message, LogLevel.Warning);
                        UseNextPriority = true;
                    }

                    return string.Empty;
                }
                else if (ctrl.ToLower().Trim() == "tablecell")
                {
                    TableCell tCell = ie.TableCell(Find.ByClass(strClassName));
                    //Assert.IsTrue(tCell.Exists, "Could not Find: " + strClassName);
                    if (ClickBeforeValueSet)
                    {
                        tCell.Click();
                        Logger.Log.Logger.LogData("Control clicked:" + strClassName, LogLevel.Info);
                        //   IEWATIN.WaitForComplete(5000);
                    }
                    if (IsEventField)
                    {
                        tCell.Click();
                        Logger.Log.Logger.LogData("Control clicked: Class Name=" + strClassName, LogLevel.Info);
                    }
                    else
                    {
                        Logger.Log.Logger.LogData("Control value get:" + strClassName, LogLevel.Info);
                        return tCell.Text;
                    }
                }
                else if (ctrl.ToLower().Trim() == "table")
                {
                    try
                    {
                        Table tbl = ie.Table(Find.ByClass(strClassName));
                        if (ClickBeforeValueSet)
                        {
                            tbl.Click();
                            Logger.Log.Logger.LogData("Control clicked:" + strClassName, LogLevel.Info);
                            //   IEWATIN.WaitForComplete(5000);
                        }
                        if (IsEventField)
                        {
                            tbl.Click();
                            Logger.Log.Logger.LogData("Control clicked: Class Name=" + strClassName, LogLevel.Info);
                        }
                        else
                        {
                            Logger.Log.Logger.LogData("Control value get:" + strClassName, LogLevel.Info);
                            return tbl.Text;
                        }
                    }
                    catch (Exception ex)
                    {
                        Log.Logger.LogData(ex.Message, LogLevel.Warning);
                        UseNextPriority = true;
                    }

                    return string.Empty;
                }
                else if (ctrl.ToLower().Trim() == "tablerow")
                {
                    try
                    {
                        TableRow row = ie.TableRow(Find.ByClass(strClassName));
                        if (ClickBeforeValueSet)
                        {
                            row.Click();
                            Logger.Log.Logger.LogData("Control clicked:" + strClassName, LogLevel.Info);
                            //   IEWATIN.WaitForComplete(5000);
                        }
                        if (IsEventField)
                        {
                            row.Click();
                            Logger.Log.Logger.LogData("Control clicked: Class Name=" + strClassName, LogLevel.Info);
                        }
                        else
                        {
                            Logger.Log.Logger.LogData("Control value get:" + strClassName, LogLevel.Info);
                            return row.Text;
                        }
                    }
                    catch (Exception ex)
                    {
                        Log.Logger.LogData(ex.Message, LogLevel.Warning);
                        UseNextPriority = true;
                    }

                    return string.Empty;
                }
                else if (ctrl.ToLower().Trim() == "checkbox")
                {
                    CheckBox chk = ie.CheckBox(Find.ByClass(strClassName));
                    if (ClickBeforeValueSet)
                    {
                        chk.Click();
                        Logger.Log.Logger.LogData("Control clicked:" + strClassName, LogLevel.Info);
                        //   IEWATIN.WaitForComplete(5000);
                    }
                    if (IsEventField)
                    {
                        chk.Click();
                        Logger.Log.Logger.LogData("Control clicked: Class Name=" + strClassName, LogLevel.Info);
                    }
                    else
                    {
                        Logger.Log.Logger.LogData("Control value get:" + strClassName, LogLevel.Info);
                        return chk.Text;
                    }
                }
                else if ((ctrl.ToLower().Trim() == "selectlist") || (ctrl.ToLower().Trim() == "select"))
                {
                    try
                    {
                        SelectList sList = ie.SelectList(Find.ByClass(strClassName));
                        if (ClickBeforeValueSet)
                        {
                            sList.Click();
                            Logger.Log.Logger.LogData("Control clicked:" + strClassName, LogLevel.Info);
                            //   IEWATIN.WaitForComplete(5000);
                        }
                        if (IsEventField)
                        {
                            sList.Click();
                            Logger.Log.Logger.LogData("Control clicked: Class Name=" + strClassName, LogLevel.Info);
                        }
                        else if ((setValue != null) && (setValue != string.Empty))
                        {

                            sList.Select(setValue);
                        }
                        else
                        {
                            Logger.Log.Logger.LogData("Control value get:" + strClassName, LogLevel.Info);
                            return sList.Text;
                        }
                    }
                    catch (Exception ex)
                    {
                        Log.Logger.LogData(ex.Message, LogLevel.Warning);
                        UseNextPriority = true;
                    }

                    return string.Empty;
                }
                else if (ctrl.ToLower().Trim() == "div")
                {
                    try
                    {
                        Div division = ie.Div(Find.ByClass(strClassName));
                        if (ClickBeforeValueSet)
                        {
                            division.Click();
                            Logger.Log.Logger.LogData("Control clicked:" + strClassName, LogLevel.Info);
                            //   IEWATIN.WaitForComplete(5000);
                        }
                        if (IsEventField)
                        {
                            division.Click();
                            Logger.Log.Logger.LogData("Control clicked: Class Name=" + strClassName, LogLevel.Info);
                        }
                        else
                        {
                            Logger.Log.Logger.LogData("Control value get:" + strClassName, LogLevel.Info);
                            return division.Text;
                        }
                    }
                    catch (Exception ex)
                    {
                        Log.Logger.LogData(ex.Message, LogLevel.Warning);
                        UseNextPriority = true;
                    }

                    return string.Empty;
                }
                else if (ctrl.ToLower().Trim() == "tablerow")
                {
                    try
                    {
                        TableRow tRow = ie.TableRow(Find.ByClass(strClassName));
                        if (ClickBeforeValueSet)
                        {
                            tRow.Click();
                            Logger.Log.Logger.LogData("Control clicked:" + strClassName, LogLevel.Info);
                            //   IEWATIN.WaitForComplete(5000);
                        }
                        if (IsEventField)
                        {
                            tRow.Click();
                            Logger.Log.Logger.LogData("Control clicked: Class Name=" + strClassName, LogLevel.Info);
                        }
                        else
                        {
                            Logger.Log.Logger.LogData("Control value get:" + strClassName, LogLevel.Info);
                            return tRow.Text;
                        }
                    }
                    catch (Exception ex)
                    {
                        Log.Logger.LogData(ex.Message, LogLevel.Warning);
                        UseNextPriority = true;
                    }

                    return string.Empty;
                }
                else if (ctrl.ToLower().Trim() == "fileupload")
                {
                    try
                    {
                        FileUpload fileUpload = ie.FileUpload(Find.ByClass(strClassName));
                        if (ClickBeforeValueSet)
                        {
                            fileUpload.Click();
                            Logger.Log.Logger.LogData("Control clicked:" + strClassName, LogLevel.Info);
                            //   IEWATIN.WaitForComplete(5000);
                        }
                        if (IsEventField)
                        {
                            fileUpload.Click();
                            Logger.Log.Logger.LogData("Control clicked: Class Name=" + strClassName, LogLevel.Info);
                        }
                        else
                        {
                            Logger.Log.Logger.LogData("Control value get:" + strClassName, LogLevel.Info);
                            return fileUpload.Text;
                        }
                    }
                    catch (Exception ex)
                    {
                        Log.Logger.LogData(ex.Message, LogLevel.Warning);
                        UseNextPriority = true;
                    }

                    return string.Empty;
                }
                else
                {
                    return string.Empty;
                }
                return string.Empty;
            } catch (Exception ex)
            {
                Log.Logger.LogData(ex.Message, LogLevel.Error);
            }
            return string.Empty;
        }

        public static string TakeActionOnControlInBrowserByCustomAttribute(IE ie, string strCustomAttribute, string ctrl, bool IsEventField, bool Activate, bool ClickBeforeValueSet, bool ContinueOnError, string setValue, ref bool UseNextPriority, string strWaitUntilContainText, int waitMaxTimeWhileLoop) 
        {
            try
            {
                if (string.IsNullOrEmpty(strCustomAttribute))
                {
                    UseNextPriority = true;
                    return "";
                }
                Element ele = null;

                string[] strCustomAttributeValues = strCustomAttribute.Split(';');
                if (strCustomAttributeValues.Length > 0)
                {
                    UseNextPriority = false;
                    if (string.IsNullOrEmpty(setValue))
                    {
                        Log.Logger.LogData("'Set Value' is not set for Custom Attribute: " + strCustomAttribute, LogLevel.Info);
                    }
                    if ((ctrl.ToLower().Trim() == "span") || (ctrl.ToLower().Trim() == "td"))
                    {
                        try
                        {
                                if (ctrl.ToLower().Trim() == "span")
                                {
                                    ele = FindElementByCustomAttribute("span", ie, strCustomAttributeValues);
                                }
                                else if (ctrl.ToLower().Trim() == "td")
                                {
                                    ele = FindElementByCustomAttribute("td", ie,strCustomAttributeValues);
                                }
                                if (ele == null)
                                {
                                    return null;
                                }
                            
                            //  Span sp = ie.Span(Find.By(strCustomAttribute, strToFind));
                            //Assert.IsTrue(sp.Exists, "Could not Find: " + strCustomAttribute);
                            if (ClickBeforeValueSet)
                            {
                                ele.Click();
                                Logger.Log.Logger.LogData("Control clicked:" + strCustomAttribute, LogLevel.Info);
                                //   IEWATIN.WaitForComplete(5000);
                            }
                            if (IsEventField)
                            {
                                ele.Click();
                                Logger.Log.Logger.LogData("Control clicked: Custom Attribute=" + strCustomAttribute, LogLevel.Info);
                                //   IEWATIN.WaitForComplete(5000);
                            }
                            else
                            {
                                Logger.Log.Logger.LogData("Control value get:" + strCustomAttribute, LogLevel.Info);
                                return ele.Text;
                            }
                        }
                        catch (Exception ex)
                        {
                            Log.Logger.LogData(ex.Message, LogLevel.Warning);
                            UseNextPriority = true;
                        }

                        return string.Empty;
                    }
                    else if (ctrl.ToLower().Trim() == "div")
                    {
                        try
                        {
                            Div division = (Div)FindElementByCustomAttribute("div", ie, strCustomAttributeValues);
                            // Div division = ie.Div(Find.By(strCustomAttribute, strToFind));
                            if (ClickBeforeValueSet)
                            {
                                division.Click();
                                Logger.Log.Logger.LogData("Control clicked:" + strCustomAttribute, LogLevel.Info);
                                //   IEWATIN.WaitForComplete(5000);
                            }
                            if (IsEventField)
                            {
                                division.Click();
                                Logger.Log.Logger.LogData("Control clicked: Custom Attribute=" + strCustomAttribute, LogLevel.Info);
                            }
                            return division.Text;
                        }
                        catch (Exception ex)
                        {
                            Log.Logger.LogData(ex.Message, LogLevel.Warning);
                            UseNextPriority = true;
                        }

                        return string.Empty;
                    }
                    else if ((ctrl.ToLower().Trim() == "input") || (ctrl.ToLower().Trim() == "button"))
                    {
                        if (IsEventField)
                        {
                            try
                            {
                                Element btn = null;

                                    if (ctrl.ToLower().Trim() == "input")
                                    {
                                        btn = FindElementByCustomAttribute("input", ie, strCustomAttributeValues);
                                    }
                                    else if (ctrl.ToLower().Trim() == "button")
                                    {
                                        btn = FindElementByCustomAttribute("button", ie, strCustomAttributeValues);
                                    }

                                    if (btn == null)
                                    {
                                        return null;
                                    }
                                
                                //  Button btn = ie.Button(Find.By(strCustomAttribute, strToFind));
                                btn.Click();
                                Logger.Log.Logger.LogData("Control clicked: Custom Attribute=" + strCustomAttribute, LogLevel.Info);
                                ie.WaitForComplete(5000);
                                return string.Empty;

                            }
                            catch (Exception ex)
                            {
                                Log.Logger.LogData(ex.Message, LogLevel.Warning);
                                UseNextPriority = true;
                            }
                        }
                        //try
                        //{
                        //    TextField txt = ie.TextField(Find.By(strCustomAttribute, strToFind));
                        //    if (ClickBeforeValueSet)
                        //    {
                        //        txt.Click();
                        //    }

                        //    if ((setValue != null) && (setValue != string.Empty))
                        //    {
                        //        txt.Value = setValue;
                        //        Logger.Log.Logger.LogData("Control value set:" + strCustomAttribute, LogLevel.Info);
                        //    }
                        //    else
                        //    {
                        //        Logger.Log.Logger.LogData("Control value get:" + strCustomAttribute, LogLevel.Info);
                        //        return txt.Text;
                        //    }
                        //}
                        //catch (Exception ex)
                        //{ UseNextPriority = true; }

                        return string.Empty;
                    }
                    else if (ctrl.ToLower().Trim() == "textfield")
                    {
                        try
                        {
                            TextField txt = null;
                            txt =(TextField) FindElementByCustomAttribute("textfield", ie, strCustomAttributeValues);

                            if (txt == null)
                            {
                                return null;
                            }
                            // TextField txt = ie.TextField(Find.By(strCustomAttribute, strToFind));
                            if (ClickBeforeValueSet)
                            {
                                txt.Click();
                                Logger.Log.Logger.LogData("Control clicked:" + strCustomAttribute, LogLevel.Info);
                                //   IEWATIN.WaitForComplete(5000);
                            }
                            if (IsEventField)
                            {
                                txt.Click();
                                Logger.Log.Logger.LogData("Control clicked: Custom Attribute=" + strCustomAttribute, LogLevel.Info);
                            }
                            else if ((setValue != null) && (setValue != string.Empty))
                            {
                                txt.Value = setValue;
                                Logger.Log.Logger.LogData("Control value set:" + strCustomAttribute, LogLevel.Info);
                            }
                            else
                            {
                                Logger.Log.Logger.LogData("Control value get:" + strCustomAttribute, LogLevel.Info);
                                return txt.Text;
                            }
                        }
                        catch (Exception ex)
                        {
                            Log.Logger.LogData(ex.Message, LogLevel.Warning);
                            UseNextPriority = true;
                        }

                        return string.Empty;
                    }
                    else if (ctrl.ToLower().Trim() == "link")
                    {
                        Link lnk = (Link)FindElementByCustomAttribute("link", ie, strCustomAttributeValues);
                       // Link lnk = ie.Link(Find.By(strCustomAttribute, strToFind));
                        if (ClickBeforeValueSet)
                        {
                            lnk.Click();
                            Logger.Log.Logger.LogData("Control clicked:" + strCustomAttribute, LogLevel.Info);
                            //   IEWATIN.WaitForComplete(5000);
                        }
                        if (IsEventField)
                        {
                            lnk.Click();
                            Logger.Log.Logger.LogData("Control clicked: Custom Attribute=" + strCustomAttribute, LogLevel.Info);
                        }
                        else
                        {
                            Logger.Log.Logger.LogData("Control value get:" + strCustomAttribute, LogLevel.Info);
                            return lnk.Text;
                        }
                    }

                    else if (ctrl.ToLower().Trim() == "a")
                    {
                        try
                        {
                            HTMLAnchorElement anchor = (HTMLAnchorElement)FindElementByCustomAttribute("a", ie, strCustomAttributeValues);
                            // Para iPara = ie.Para(Find.By(strCustomAttribute, strToFind));
                            if (ClickBeforeValueSet)
                            {
                                try
                                {
                                    anchor.click();
                                    Logger.Log.Logger.LogData("Control clicked:" + strCustomAttribute, LogLevel.Info);
                                }
                                catch (Exception ex)
                                {
                                    Logger.Log.Logger.LogData("Error while control click " + strCustomAttribute, LogLevel.Error);
                                }
                                //   IEWATIN.WaitForComplete(5000);
                            }
                            if (IsEventField)
                            {
                                anchor.click();
                                Logger.Log.Logger.LogData("Control clicked: Class Name=" + strCustomAttribute, LogLevel.Info);
                            }
                            else
                            {
                                Logger.Log.Logger.LogData("Control value get:" + strCustomAttribute, LogLevel.Info);
                                return anchor.innerText;
                            }
                        }
                        catch (Exception ex)
                        {
                            Log.Logger.LogData(ex.Message, LogLevel.Warning);
                            UseNextPriority = true;
                        }

                        return string.Empty;
                    }

                    else if (ctrl.ToLower().Trim() == "p")
                    {
                        try
                        {
                            Para iPara = (Para)FindElementByCustomAttribute("p", ie, strCustomAttributeValues);
                           // Para iPara = ie.Para(Find.By(strCustomAttribute, strToFind));
                            if (ClickBeforeValueSet)
                            {
                                try
                                {
                                    iPara.Click();
                                    Logger.Log.Logger.LogData("Control clicked:" + strCustomAttribute, LogLevel.Info);
                                }
                                catch (Exception ex)
                                {
                                    Logger.Log.Logger.LogData("Error while control click " + strCustomAttribute, LogLevel.Error);
                                }
                                //   IEWATIN.WaitForComplete(5000);
                            }
                            if (IsEventField)
                            {
                                iPara.Click();
                                Logger.Log.Logger.LogData("Control clicked: Class Name=" + strCustomAttribute, LogLevel.Info);
                            }
                            else
                            {
                                Logger.Log.Logger.LogData("Control value get:" + strCustomAttribute, LogLevel.Info);
                                return iPara.Text;
                            }
                        }
                        catch (Exception ex)
                        {
                            Log.Logger.LogData(ex.Message, LogLevel.Warning);
                            UseNextPriority = true;
                        }

                        return string.Empty;
                    }
                    else if (ctrl.ToLower().Trim() == "frame")
                    {
                        try
                        {
                            //Frame iFrame = (Frame)FindElementByCustomAttribute("frame", ie, strCustomAttributeValues);
                           Frame iFrame = ie.Frame(Find.By(strCustomAttributeValues[0], strCustomAttributeValues[1]));
                            Logger.Log.Logger.LogData("Control value get:" + strCustomAttribute, LogLevel.Info);
                            return iFrame.Text;
                        }
                        catch (Exception ex)
                        {
                            Log.Logger.LogData(ex.Message, LogLevel.Error);
                            UseNextPriority = true;
                        }

                        return string.Empty;
                    }
                    else if (ctrl.ToLower().Trim() == "img")
                    {
                        try
                        {
                            Image img = (Image)FindElementByCustomAttribute("img", ie, strCustomAttributeValues);
                          //  Image img = ie.Image(Find.By(strCustomAttribute, strToFind));
                            if (ClickBeforeValueSet)
                            {
                                img.Click();
                                Logger.Log.Logger.LogData("Control clicked:" + strCustomAttribute, LogLevel.Info);
                                //   IEWATIN.WaitForComplete(5000);
                            }
                            if (IsEventField)
                            {
                                img.Click();
                                Logger.Log.Logger.LogData("Control clicked: Custom Attribute=" + strCustomAttribute, LogLevel.Info);
                            }
                            else
                            {
                                Logger.Log.Logger.LogData("Control value get:" + strCustomAttribute, LogLevel.Info);
                                return img.Text;
                            }
                        }
                        catch (Exception ex)
                        {
                            Log.Logger.LogData(ex.Message, LogLevel.Warning);
                            UseNextPriority = true;
                        }

                        return string.Empty;
                    }
                    else if (ctrl.ToLower().Trim() == "tablecell")
                    {
                        try
                        {
                            TableCell tCell =(TableCell) FindElementByCustomAttribute("tablecell", ie, strCustomAttributeValues);
                            // TableCell tCell = ie.TableCell(Find.By(strCustomAttribute, strToFind));
                            //Assert.IsTrue(tCell.Exists, "Could not Find: " + strCustomAttribute);
                            if (ClickBeforeValueSet)
                            {
                                tCell.Click();
                                Logger.Log.Logger.LogData("Control clicked:" + strCustomAttribute, LogLevel.Info);
                                //   IEWATIN.WaitForComplete(5000);
                            }
                            if (IsEventField)
                            {
                                tCell.Click();
                                Logger.Log.Logger.LogData("Control clicked: Custom Attribute=" + strCustomAttribute, LogLevel.Info);
                            }
                            else
                            {
                                Logger.Log.Logger.LogData("Control value get:" + strCustomAttribute, LogLevel.Info);
                                return tCell.Text;
                            }
                        }
                        catch (Exception ex)
                        {
                            Log.Logger.LogData(ex.Message, LogLevel.Warning);
                            UseNextPriority = true;
                        }

                        return string.Empty;
                    }
                    else if (ctrl.ToLower().Trim() == "table")
                    {
                        try
                        {
                            Table tbl = (Table)FindElementByCustomAttribute("table", ie, strCustomAttributeValues);
                            // Table tbl = ie.Table(Find.By(strCustomAttribute, strToFind));
                            if (ClickBeforeValueSet)
                            {
                                tbl.Click();
                                Logger.Log.Logger.LogData("Control clicked:" + strCustomAttribute, LogLevel.Info);
                                //   IEWATIN.WaitForComplete(5000);
                            }
                            if (IsEventField)
                            {
                                tbl.Click();
                                Logger.Log.Logger.LogData("Control clicked: Custom Attribute=" + strCustomAttribute, LogLevel.Info);
                            }
                            else
                            {
                                Logger.Log.Logger.LogData("Control value get:" + strCustomAttribute, LogLevel.Info);
                                return tbl.Text;
                            }
                        }
                        catch (Exception ex)
                        {
                            Log.Logger.LogData(ex.Message, LogLevel.Warning);
                            UseNextPriority = true;
                        }

                        return string.Empty;
                    }
                    else if (ctrl.ToLower().Trim() == "tablerow")
                    {
                        try
                        {
                            TableRow row = (TableRow)FindElementByCustomAttribute("tablerow", ie, strCustomAttributeValues);
                            //TableRow row = ie.TableRow(Find.By(strCustomAttribute, strToFind));
                            if (ClickBeforeValueSet)
                            {
                                row.Click();
                                Logger.Log.Logger.LogData("Control clicked:" + strCustomAttribute, LogLevel.Info);
                                //   IEWATIN.WaitForComplete(5000);
                            }
                            if (IsEventField)
                            {
                                row.Click();
                                Logger.Log.Logger.LogData("Control clicked: Custom Attribute=" + strCustomAttribute, LogLevel.Info);
                            }
                            else
                            {
                                Logger.Log.Logger.LogData("Control value get:" + strCustomAttribute, LogLevel.Info);
                                return row.Text;
                            }
                        }
                        catch (Exception ex)
                        {
                            Log.Logger.LogData(ex.Message, LogLevel.Warning);
                            UseNextPriority = true;
                        }

                        return string.Empty;
                    }
                    else if (ctrl.ToLower().Trim() == "checkbox")
                    {
                        try
                        {
                            CheckBox chk = (CheckBox)FindElementByCustomAttribute("checkbox", ie, strCustomAttributeValues);
                            // CheckBox chk = ie.CheckBox(Find.By(strCustomAttribute, strToFind));
                            if (ClickBeforeValueSet)
                            {
                                chk.Click();
                                Logger.Log.Logger.LogData("Control clicked:" + strCustomAttribute, LogLevel.Info);
                                //   IEWATIN.WaitForComplete(5000);
                            }
                            if (IsEventField)
                            {
                                chk.Click();
                                Logger.Log.Logger.LogData("Control clicked: Custom Attribute=" + strCustomAttribute, LogLevel.Info);
                            }
                            else
                            {
                                Logger.Log.Logger.LogData("Control value get:" + strCustomAttribute, LogLevel.Info);
                                return chk.Text;
                            }
                        }
                        catch (Exception ex)
                        {
                            Log.Logger.LogData(ex.Message, LogLevel.Warning);
                            UseNextPriority = true;
                        }

                        return string.Empty;
                    }

                    else if ((ctrl.ToLower().Trim() == "selectlist") || (ctrl.ToLower().Trim() == "select"))
                    {
                        try
                        {
                            SelectList sList = null;
                            if (ctrl.ToLower().Trim() == "selectlist")
                            {
                                sList = (SelectList)FindElementByCustomAttribute("selectlist", ie, strCustomAttributeValues);
                            }
                            else if (ctrl.ToLower().Trim() == "select")
                            {
                                sList = (SelectList)FindElementByCustomAttribute("select", ie, strCustomAttributeValues);
                            }

                           // SelectList sList = ie.SelectList(Find.By(strCustomAttribute, strToFind));
                            if (ClickBeforeValueSet)
                            {
                                sList.Click();
                                Logger.Log.Logger.LogData("Control clicked:" + strCustomAttribute, LogLevel.Info);
                                //   IEWATIN.WaitForComplete(5000);
                            }
                            if (IsEventField)
                            {
                                sList.Click();
                                Logger.Log.Logger.LogData("Control clicked: Custom Attribute=" + strCustomAttribute, LogLevel.Info);
                            }
                            else if ((setValue != null) && (setValue != string.Empty))
                            {
                                sList.Select(setValue);
                                Logger.Log.Logger.LogData("Control value select:" + strCustomAttribute, LogLevel.Info);
                            }
                            else
                            {
                                Logger.Log.Logger.LogData("Control value get:" + strCustomAttribute, LogLevel.Info);
                                return sList.Text;
                            }
                        }
                        catch (Exception ex)
                        {
                            Log.Logger.LogData(ex.Message, LogLevel.Warning);
                            UseNextPriority = true;
                        }

                        return string.Empty;
                    }
                   
                    //else if (ctrl.ToLower().Trim() == "tablerow")
                    //{
                    //    try
                    //    {
                    //        TableRow tRow = ie.TableRow(Find.By(strCustomAttribute, strToFind));
                    //        if (ClickBeforeValueSet)
                    //        {
                    //            tRow.Click();
                    //            Logger.Log.Logger.LogData("Control clicked:" + strCustomAttribute, LogLevel.Info);
                    //            //   IEWATIN.WaitForComplete(5000);
                    //        }
                    //        if (IsEventField)
                    //        {
                    //            tRow.Click();
                    //            Logger.Log.Logger.LogData("Control clicked: Custom Attribute=" + strCustomAttribute, LogLevel.Info);
                    //        }
                    //        else
                    //        {
                    //            Logger.Log.Logger.LogData("Control value select:" + strCustomAttribute, LogLevel.Info);
                    //            return tRow.Text;
                    //        }
                    //    }
                    //    catch (Exception ex)
                    //    {
                    //        Log.Logger.LogData(ex.Message, LogLevel.Warning);
                    //        UseNextPriority = true;
                    //    }

                    //    return string.Empty;
                    //}
                    else if (ctrl.ToLower().Trim() == "fileupload")
                    {
                        try
                        {
                            FileUpload fileUpload = (FileUpload )FindElementByCustomAttribute("fileupload", ie, strCustomAttributeValues);
                          //  FileUpload fileUpload = ie.FileUpload(Find.By(strCustomAttribute, strToFind));
                            if (ClickBeforeValueSet)
                            {
                                fileUpload.Click();
                                Logger.Log.Logger.LogData("Control clicked:" + strCustomAttribute, LogLevel.Info);
                                //   IEWATIN.WaitForComplete(5000);
                            }
                            if (IsEventField)
                            {
                                fileUpload.Click();
                                Logger.Log.Logger.LogData("Control clicked: Custom Attribute=" + strCustomAttribute, LogLevel.Info);
                            }
                            else
                            {
                                Logger.Log.Logger.LogData("Control value select:" + strCustomAttribute, LogLevel.Info);
                                return fileUpload.Text;
                            }

                        }
                        catch (Exception ex)
                        {
                            Log.Logger.LogData(ex.Message, LogLevel.Warning);
                            UseNextPriority = true;
                        }

                        return string.Empty;
                    }
                    else
                    {
                        return string.Empty;
                    }
                    return string.Empty;
                }
                } catch (Exception ex)
                            
            {
                Log.Logger.LogData(ex.Message, LogLevel.Error);
            }
            return string.Empty;
        }

       static   Element FindElementByCustomAttribute(string strTag, IE ie, string[] strCustomAttributeValues)
        {
            ElementCollection ec = ie.ElementsWithTag(strTag);
            Element ele = null;
            foreach (string item in strCustomAttributeValues)
            {
                string[] strAttributeValues = item.Split('=');
                if (strAttributeValues.Length > 1)
                {
                    string strAttributeName = strAttributeValues[0].Trim();
                    string strAttributeValue = strAttributeValues[1].Trim();

                    if (!string.IsNullOrEmpty(strAttributeName) && !string.IsNullOrEmpty(strAttributeValue))
                    {
                        if (strAttributeName.ToLower().Equals("class"))
                        {
                            ec = ec.Filter(Find.ByClass(strAttributeValue));
                        }
                        else if (strAttributeName.ToLower().Equals("id"))
                        {
                            ec = ec.Filter(Find.ById(strAttributeValue));
                        }
                        else if (strAttributeName.ToLower().Equals("name"))
                        {
                            ec = ec.Filter(Find.ByName(strAttributeValue));
                        }
                        else
                        {
                            ec = ec.Filter(Find.By(strAttributeName, strAttributeValue));
                        }

                        if (ec.Count > 1)
                        {
                            ele = ec[0];
                            continue;
                        }
                        else
                        {
                            ele = ec[0];
                            return ele;
                        }

                    }
                    else
                    {
                        Logger.Log.Logger.LogData("Attribute Name Or Value should not be Empty.", Logger.LogLevel.Info);
                    }
                }

            }
            Logger.Log.Logger.LogData("Element not found", Logger.LogLevel.Info);
            return null;
        }

        public static string TakeActionOnControlInBrowser(Element element, string ctrl, bool IsEventField,bool Activate, bool ClickBeforeValueSet, bool ContinueOnError, string setValue, ref bool UseNextPriority, string strWaitUntilContainText, int waitMaxTimeWhileLoop)
        {
            try
            {
                UseNextPriority = false;
                //IE ie = IE.AttachToIE(Find.ByUrl(ienew.Url));
                if ((ctrl.ToLower().Trim() == "span") || (ctrl.ToLower().Trim() == "td"))
                {
                    try
                    {
                        Span sp = (Span)element;
                        //Assert.IsTrue(sp.Exists, "Could not Find: " + strName);
                        if (ClickBeforeValueSet)
                        {
                            sp.Click();
                        }
                        if (IsEventField)
                        {
                            sp.Click();
                            Logger.Log.Logger.LogData("Span control clicked:", LogLevel.Info);
                        }
                        else
                        {
                            Logger.Log.Logger.LogData("Span control value get:", LogLevel.Info);
                            return sp.Text;
                        }
                    }
                    catch (Exception ex)
                    {
                        Log.Logger.LogData(ex.Message, LogLevel.Warning);
                        UseNextPriority = true;
                    }

                    return string.Empty;
                }
                else if ((ctrl.ToLower().Trim() == "textfield") || (ctrl.ToLower().Trim() == "input") || (ctrl.ToLower().Trim() == "button"))
                {
                    if (IsEventField)
                    {
                        try
                        {
                            Button btn = (Button)element;
                            btn.Click();
                            Logger.Log.Logger.LogData("Textfield control clicked:", LogLevel.Info);
                            return string.Empty;

                        }
                        catch (Exception ex)
                        {
                            Log.Logger.LogData(ex.Message, LogLevel.Warning);
                            UseNextPriority = true;
                        }
                    }
                    else
                    {
                        try
                        {
                            TextField txt = (TextField)element;
                            if (ClickBeforeValueSet)
                            {
                                txt.Click();
                            }
                            if ((setValue != null) && (setValue != string.Empty))
                            {
                                txt.Value = setValue;
                                Logger.Log.Logger.LogData("Control value set:", LogLevel.Info);
                            }
                            else
                            {
                                Logger.Log.Logger.LogData("Control value get:", LogLevel.Info);
                                return txt.Text;
                            }
                        }
                        catch (Exception ex)
                        {
                            Log.Logger.LogData(ex.Message, LogLevel.Warning);
                            UseNextPriority = true;
                        }
                    }
                    return string.Empty;
                }
                else if (ctrl.ToLower().Trim() == "link")
                {
                    try
                    {
                        Link lnk = (Link)element;
                        if (ClickBeforeValueSet)
                        {
                            lnk.Click();
                            Logger.Log.Logger.LogData("Control clicked:", LogLevel.Info);
                            //   IEWATIN.WaitForComplete(5000);
                        }
                        if (IsEventField)
                        {
                            lnk.Click();
                            Logger.Log.Logger.LogData("Control clicked:", LogLevel.Info);
                        }
                        else
                        {
                            Logger.Log.Logger.LogData("Control value get:", LogLevel.Info);
                            return lnk.Text;
                        }
                    }
                    catch (Exception ex)
                    {
                        Log.Logger.LogData(ex.Message, LogLevel.Warning);
                        UseNextPriority = true;
                    }

                    return string.Empty;
                }
                //else if (ctrl.ToLower().Trim() == "frame")
                //{
                //    try
                //    {
                //        Frame iFrame = element;
                //        Logger.Log.Logger.LogData("Control value get:" , LogLevel.Info);
                //        return iFrame.Text;
                //    }
                //    catch (Exception ex)
                //    {
                //        Log.Logger.LogData(ex.Message, LogLevel.Error);
                //        UseNextPriority = true;
                //    }

                //    return string.Empty;
                //}
                else if (ctrl.ToLower().Trim() == "img")
                {
                    try
                    {
                        Image img = (Image)element;
                        if (ClickBeforeValueSet)
                        {
                            img.Click();
                            Logger.Log.Logger.LogData("Control clicked:", LogLevel.Info);
                            //   IEWATIN.WaitForComplete(5000);
                        }
                        if (IsEventField)
                        {
                            img.Click();
                            Logger.Log.Logger.LogData("Control clicked:", LogLevel.Info);
                        }
                        else
                        {
                            Logger.Log.Logger.LogData("Control value get:", LogLevel.Info);
                            return img.Text;
                        }
                    }
                    catch (Exception ex)
                    {
                        Log.Logger.LogData(ex.Message, LogLevel.Warning);
                        UseNextPriority = true;
                    }

                    return string.Empty;
                }
                else if (ctrl.ToLower().Trim() == "tablecell")
                {
                    TableCell tCell = (TableCell)element;
                    //Assert.IsTrue(tCell.Exists, "Could not Find: " + strName);
                    if (ClickBeforeValueSet)
                    {
                        tCell.Click();
                        Logger.Log.Logger.LogData("Control clicked:", LogLevel.Info);
                        //   IEWATIN.WaitForComplete(5000);
                    }
                    if (IsEventField)
                    {
                        tCell.Click();
                        Logger.Log.Logger.LogData("Control clicked:", LogLevel.Info);
                    }
                    else
                    {
                        Logger.Log.Logger.LogData("Control value get:", LogLevel.Info);
                        return tCell.Text;
                    }
                }
                else if (ctrl.ToLower().Trim() == "table")
                {
                    Table tbl = (Table)element;
                    if (ClickBeforeValueSet)
                    {
                        tbl.Click();
                        Logger.Log.Logger.LogData("Control clicked:", LogLevel.Info);
                        //   IEWATIN.WaitForComplete(5000);
                    }
                    if (IsEventField)
                    {
                        tbl.Click();
                        Logger.Log.Logger.LogData("Control clicked:", LogLevel.Info);
                    }
                    else
                    {
                        Logger.Log.Logger.LogData("Control value get:", LogLevel.Info);
                        return tbl.Text;
                    }
                }
                else if (ctrl.ToLower().Trim() == "tablerow")
                {
                    try
                    {
                        TableRow row = (TableRow)element;
                        if (ClickBeforeValueSet)
                        {
                            row.Click();
                            Logger.Log.Logger.LogData("Control clicked:", LogLevel.Info);
                            //   IEWATIN.WaitForComplete(5000);
                        }
                        if (IsEventField)
                        {
                            row.Click();
                            Logger.Log.Logger.LogData("Control clicked:", LogLevel.Info);
                        }
                        else
                        {
                            Logger.Log.Logger.LogData("Control value get:", LogLevel.Info);
                            return row.Text;
                        }
                    }
                    catch (Exception ex)
                    {
                        Log.Logger.LogData(ex.Message, LogLevel.Warning);
                        UseNextPriority = true;
                    }

                    return string.Empty;
                }
                else if (ctrl.ToLower().Trim() == "checkbox")
                {
                    try
                    {
                        CheckBox chk = (CheckBox)element;
                        if (ClickBeforeValueSet)
                        {
                            chk.Click();
                            Logger.Log.Logger.LogData("Control clicked:", LogLevel.Info);
                            //   IEWATIN.WaitForComplete(5000);
                        }
                        if (IsEventField)
                        {
                            chk.Click();
                            Logger.Log.Logger.LogData("Control clicked:", LogLevel.Info);
                        }
                        else
                        {
                            Logger.Log.Logger.LogData("Control value get:", LogLevel.Info);
                            return chk.Text;
                        }
                    }
                    catch (Exception ex)
                    {
                        Log.Logger.LogData(ex.Message, LogLevel.Warning);
                        UseNextPriority = true;
                    }
                    return string.Empty;
                }
                else if ((ctrl.ToLower().Trim() == "selectlist") || (ctrl.ToLower().Trim() == "select"))
                {
                    try
                    {
                        SelectList sList = (SelectList)element;
                        if (ClickBeforeValueSet)
                        {
                            sList.Click();
                            Logger.Log.Logger.LogData("Control clicked:", LogLevel.Info);
                            //   IEWATIN.WaitForComplete(5000);
                        }
                        if (IsEventField)
                        {
                            sList.Click();
                            Logger.Log.Logger.LogData("Control clicked:", LogLevel.Info);
                        }
                        //else if ((setValue != null) && (setValue != string.Empty))
                        else if ((setValue != null) && (setValue != string.Empty))
                        {

                            sList.Select(setValue);
                        }
                        else
                        {
                            Logger.Log.Logger.LogData("Control value get:", LogLevel.Info);
                            return sList.Text;
                        }
                    }
                    catch (Exception ex)
                    {
                        Log.Logger.LogData(ex.Message, LogLevel.Warning);
                        UseNextPriority = true;
                    }
                    return string.Empty;
                }
                else if (ctrl.ToLower().Trim() == "div")
                {
                    try
                    {
                        Div division = (Div)element;
                        if (ClickBeforeValueSet)
                        {
                            division.Click();
                            Logger.Log.Logger.LogData("Control clicked:", LogLevel.Info);
                            //   IEWATIN.WaitForComplete(5000);
                        }
                        if (IsEventField)
                        {
                            division.Click();
                            Logger.Log.Logger.LogData("Control clicked:", LogLevel.Info);
                        }
                        else
                        {
                            Logger.Log.Logger.LogData("Control value get:", LogLevel.Info);
                            return division.Text;
                        }
                    }
                    catch (Exception ex)
                    {
                        Log.Logger.LogData(ex.Message, LogLevel.Warning);
                        UseNextPriority = true;
                    }

                    return string.Empty;
                }

                else if (ctrl.ToLower().Trim() == "fileupload")
                {
                    try
                    {
                        FileUpload fileUpload = (FileUpload)element;
                        if (ClickBeforeValueSet)
                        {
                            fileUpload.Click();
                            Logger.Log.Logger.LogData("Control clicked:", LogLevel.Info);
                            //   IEWATIN.WaitForComplete(5000);
                        }
                        if (IsEventField)
                        {
                            fileUpload.Click();
                            Logger.Log.Logger.LogData("Control clicked:", LogLevel.Info);
                        }
                        else
                        {
                            Logger.Log.Logger.LogData("Control value get:", LogLevel.Info);
                            return fileUpload.Text;
                        }
                    }
                    catch (Exception ex)
                    {
                        Log.Logger.LogData(ex.Message, LogLevel.Warning);
                        UseNextPriority = true;
                    }

                    return string.Empty;
                }
                else
                {
                    return string.Empty;
                }
                return string.Empty;
            } catch (Exception ex)
            {
                Log.Logger.LogData(ex.Message, LogLevel.Error);
            }
            return string.Empty;
        }

        public static object FindControlInTableByID(Table tbl, string strID, string ctrl)
        {
            try
            {
                if (ctrl.ToLower().Trim() == "span")
                {
                    Span sp = tbl.Span(Find.ById(strID));
                    //Assert.IsTrue(sp.Exists, "Could not Find: " + strID);
                    return sp;
                }
                else if (ctrl.ToLower().Trim() == "link")
                {
                    Link lnk = tbl.Link(Find.ById(strID));
                    //Assert.IsTrue(lnk.Exists, "Could not Find: " + strID);
                    return lnk;
                }
                else if (ctrl.ToLower().Trim() == "img")
                {
                    Image img = tbl.Image(Find.BySrc(strID));
                    //Assert.IsTrue(img.Exists, "Could not Find: " + strID);
                    return img;
                }
                else if (ctrl.ToLower().Trim() == "tablecell")
                {
                    TableCell tCell = tbl.TableCell(Find.ById(strID));
                    //Assert.IsTrue(tCell.Exists, "Could not Find: " + strID);
                    return tCell;
                }
                else if (ctrl.ToLower().Trim() == "table")
                {
                    Table nestedTbl = tbl.Table(Find.ById(strID));
                    //Assert.IsTrue(nestedTbl.Exists, "Could not Find: " + strID);
                    return nestedTbl;
                }
                else if (ctrl.ToLower().Trim() == "checkbox")
                {
                    CheckBox chk = tbl.CheckBox(Find.ById(strID));
                    //Assert.IsTrue(chk.Exists, "Could not Find: " + strID);
                    return chk;
                }
                else if (ctrl.ToLower().Trim() == "button")
                {
                    Button btn = tbl.Button(Find.ById(strID));
                    //Assert.IsTrue(btn.Exists, "Could not Find: " + strID);
                    return btn;
                }
                else if (ctrl.ToLower().Trim() == "textfield")
                {
                    TextField txt = tbl.TextField(Find.ById(strID));
                    //Assert.IsTrue(txt.Exists, "Could not Find: " + strID);
                    return txt;
                }
                else if (ctrl.ToLower().Trim() == "div")
                {
                    Div division = tbl.Div(Find.ById(strID));
                    //Assert.IsTrue(division.Exists, "Could not Find: " + strID);
                    return division;
                }
                else if (ctrl.ToLower().Trim() == "tablerow")
                {
                    TableRow tRow = tbl.TableRow(Find.ById(strID));
                    //Assert.IsTrue(tRow.Exists, "Could not Find: " + strID);
                    return tRow;
                }
                else
                {
                    return null;
                }
            } catch (Exception ex)
            {
                Log.Logger.LogData(ex.Message, LogLevel.Error);
            }
            return string.Empty;
            
        }

        public static object FindControlInTableByCustom(Table tbl, string strCustomAttribute, string strToFind, string ctrl)
        {
            try
            {
                if (ctrl.ToLower().Trim() == "span")
                {
                    Span sp = tbl.Span(Find.By(strCustomAttribute, strToFind));
                    //Assert.IsTrue(sp.Exists, "Could not Find: " + strToFind);
                    return sp;
                }
                else if (ctrl.ToLower().Trim() == "link")
                {
                    Link lnk = tbl.Link(Find.By(strCustomAttribute, strToFind));
                    //Assert.IsTrue(lnk.Exists, "Could not Find: " + strToFind);
                    return lnk;
                }
                else if (ctrl.ToLower().Trim() == "img")
                {
                    Image img;

                    if (strCustomAttribute == "src")
                    {
                        img = tbl.Image(Find.BySrc(strToFind));
                    }
                    else
                    {
                        img = tbl.Image(Find.By(strCustomAttribute, strToFind));
                    }
                    //Assert.IsTrue(img.Exists, "Could not Find: " + strToFind);
                    return img;
                }
                else if (ctrl.ToLower().Trim() == "tablecell")
                {
                    TableCell tCell = tbl.TableCell(Find.By(strCustomAttribute, strToFind));
                    //Assert.IsTrue(tCell.Exists, "Could not Find: " + strToFind);
                    return tCell;
                }
                else if (ctrl.ToLower().Trim() == "table")
                {
                    Table nestedTbl = tbl.Table(Find.By(strCustomAttribute, strToFind));
                    //Assert.IsTrue(nestedTbl.Exists, "Could not Find: " + strToFind);
                    return nestedTbl;
                }
                else if (ctrl.ToLower().Trim() == "tablerow")
                {
                    TableRow row = tbl.TableRow(Find.By(strCustomAttribute, strToFind));
                    //Assert.IsTrue(row.Exists, "Could not Find: " + strToFind);
                    return row;
                }
                else if (ctrl.ToLower().Trim() == "checkbox")
                {
                    CheckBox chk = tbl.CheckBox(Find.By(strCustomAttribute, strToFind));
                    //Assert.IsTrue(chk.Exists, "Could not Find: " + strToFind);
                    return chk;
                }
                else if (ctrl.ToLower().Trim() == "button")
                {
                    Button btn = tbl.Button(Find.By(strCustomAttribute, strToFind));
                    //Assert.IsTrue(btn.Exists, "Could not Find: " + strToFind);
                    return btn;
                }
                else if (ctrl.ToLower().Trim() == "textfield")
                {
                    TextField txt = tbl.TextField(Find.By(strCustomAttribute, strToFind));
                    //Assert.IsTrue(txt.Exists, "Could not Find: " + strToFind);
                    return txt;
                }
                else if ((ctrl.ToLower().Trim() == "selectlist") || (ctrl.ToLower().Trim() == "select"))
                {
                    SelectList sList = tbl.SelectList(Find.By(strCustomAttribute, strToFind));
                    //Assert.IsTrue(sList.Exists, "Could not Find: " + strToFind);
                    return sList;
                }
                else if (ctrl.ToLower().Trim() == "div")
                {
                    Div div = tbl.Div(Find.By(strCustomAttribute, strToFind));
                    //Assert.IsTrue(div.Exists, "Could not Find: " + strToFind);
                    return div;
                }
                else
                {
                    return null;
                }
            }catch (Exception ex)
            {
                Log.Logger.LogData(ex.Message, LogLevel.Error);
            }
            return string.Empty;
        }

        public static object FindControlInFrameByID(Frame frame, string strID, string ctrl)
        {
            try
            {
                if (ctrl.ToLower().Trim() == "span")
                {
                    Span sp = frame.Span(Find.ById(strID));
                    //Assert.IsTrue(sp.Exists, "Could not Find: " + strID);
                    return sp;
                }
                else if (ctrl.ToLower().Trim() == "link")
                {
                    Link lnk = frame.Link(Find.ById(strID));
                    //Assert.IsTrue(lnk.Exists, "Could not Find: " + strID);
                    return lnk;
                }
                else if (ctrl.ToLower().Trim() == "img")
                {
                    Image img = frame.Image(Find.BySrc(strID));
                    //Assert.IsTrue(img.Exists, "Could not Find: " + strID);
                    return img;
                }
                else if (ctrl.ToLower().Trim() == "tablecell")
                {
                    TableCell tCell = frame.TableCell(Find.ById(strID));
                    //Assert.IsTrue(tCell.Exists, "Could not Find: " + strID);
                    return tCell;
                }
                else if (ctrl.ToLower().Trim() == "table")
                {
                    Table nestedTbl = frame.Table(Find.ById(strID));
                    //Assert.IsTrue(nestedTbl.Exists, "Could not Find: " + strID);
                    return nestedTbl;
                }
                else if (ctrl.ToLower().Trim() == "checkbox")
                {
                    CheckBox chk = frame.CheckBox(Find.ById(strID));
                    //Assert.IsTrue(chk.Exists, "Could not Find: " + strID);
                    return chk;
                }
                else if (ctrl.ToLower().Trim() == "button")
                {
                    Button btn = frame.Button(Find.ById(strID));
                    //Assert.IsTrue(btn.Exists, "Could not Find: " + strID);
                    return btn;
                }
                else if (ctrl.ToLower().Trim() == "textfield")
                {
                    TextField txt = frame.TextField(Find.ById(strID));
                    //Assert.IsTrue(txt.Exists, "Could not Find: " + strID);
                    return txt;
                }
                else if (ctrl.ToLower().Trim() == "div")
                {
                    Div division = frame.Div(Find.ById(strID));
                    //Assert.IsTrue(division.Exists, "Could not Find: " + strID);
                    return division;
                }
                else if (ctrl.ToLower().Trim() == "tablerow")
                {
                    TableRow tRow = frame.TableRow(Find.ById(strID));
                    //Assert.IsTrue(tRow.Exists, "Could not Find: " + strID);
                    return tRow;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                Log.Logger.LogData(ex.Message, LogLevel.Error);
            }
            return string.Empty;
        }

      
        # endregion
    }

    //public class Enumerators
    //{
    //    public enum ControlType
    //    {
    //        Span,
    //        Link,
    //        Table,
    //        TableCell,
    //        TableRow,
    //        Frame,
    //        Image,
    //        TextField,
    //        CheckBox,
    //        Button,
    //        SelectList,
    //        Div,
    //        RadioButton,
    //        FileUpload
    //    }
    //}
}
