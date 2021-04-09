
using Logger;
using sapfewse;
using saprotwr.net;
using System;

namespace Bot.Activity.SAP
{
    internal class SAPHelper
    {

        //Test

        public SAPHelper(CSapROTWrapper sapROTWrapper)
        {

        }


        public static string TakeActionOnControlByID(GuiSession session, string strID, string ctrl, bool IsEventField, bool Activate, bool ClickBeforeValueSet, bool ContinueOnError, string setValue, string key, bool IsDoubleClick, ref bool UseNextPriority)
        {
            try
            {
                UseNextPriority = false;
                //IE ie = IE.AttachToIE(Find.ByUrl(ienew.Url));
                if (string.IsNullOrEmpty(setValue))
                {
                    Log.Logger.LogData("'Set Value' is not set for ID: " + strID, LogLevel.Info);
                }

                if (ctrl.ToLower().Trim() == "guibutton")
                {
                    try
                    {
                        GuiButton button = ((GuiButton)session.FindById(strID));
                        //Assert.IsTrue(sp.Exists, "Could not Find: " + strID);
                        if (Activate)
                        {
                            try
                            {
                                button.SetFocus();
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
                                button.Press();
                                Logger.Log.Logger.LogData("Control clicked:" + strID, LogLevel.Info);
                            }
                            catch (Exception ex)
                            {
                                Logger.Log.Logger.LogData("Error while before click " + strID, LogLevel.Warning);
                            }
                        }
                        if (IsEventField)
                        {
                            button.Press();
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
                            return button.Text;
                        }
                    }
                    catch (Exception ex)
                    { UseNextPriority = true; }

                    return string.Empty;
                }
                else if ((ctrl.ToLower().Trim() == "guictextfield"))
                {
                    try
                    {
                        GuiCTextField txt = ((GuiCTextField)session.FindById(strID));
                        if (Activate)
                        {
                            try
                            {
                                txt.SetFocus();
                                Logger.Log.Logger.LogData("Control activated:" + strID, LogLevel.Info);
                            }
                            catch (Exception ex)
                            {
                                Logger.Log.Logger.LogData("Error while setting focus on " + strID, LogLevel.Warning);
                            }

                            //   IEWATIN.WaitForComplete(5000);
                        }
                        if (ClickBeforeValueSet)
                        {
                            txt.Text = "";
                            Logger.Log.Logger.LogData("Control clicked:" + strID, LogLevel.Info);
                            //   IEWATIN.WaitForComplete(5000);
                        }
                        if ((setValue != null) && (setValue != string.Empty))
                        {
                            txt.Text = setValue;
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


                    return string.Empty;
                }
                else if ((ctrl.ToLower().Trim() == "guitextfield"))
                {
                    try
                    {
                        GuiTextField txt = ((GuiTextField)session.FindById(strID));
                        if (Activate)
                        {
                            try
                            {
                                txt.SetFocus();
                                Logger.Log.Logger.LogData("Control activated:" + strID, LogLevel.Info);
                            }
                            catch (Exception ex)
                            {
                                Logger.Log.Logger.LogData("Error while setting focus on " + strID, LogLevel.Warning);
                            }

                            //   IEWATIN.WaitForComplete(5000);
                        }
                        if (ClickBeforeValueSet)
                        {
                            txt.Text = "";
                            Logger.Log.Logger.LogData("Control clicked:" + strID, LogLevel.Info);
                            //   IEWATIN.WaitForComplete(5000);
                        }
                        if ((setValue != null) && (setValue != string.Empty))
                        {
                            txt.Text = setValue;
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


                    return string.Empty;
                }
                else if (ctrl.ToLower().Trim() == "guitree")
                {
                    try
                    {
                        GuiTree guiTree = (GuiTree)session.FindById(strID);


                        if ((setValue != null) && (setValue != string.Empty))
                        {
                            guiTree.SelectedNode = setValue;
                            Logger.Log.Logger.LogData("Control value set:" + strID, LogLevel.Info);
                        }
                        //if (IsEventField)
                        //{
                        //    guiTree.DoubleClickNode(setValue);
                        //    Logger.Log.Logger.LogData("GuiStatusbar control clicked:" + strID, LogLevel.Info);
                        //}
                        if (IsEventField)
                        {
                            guiTree.SelectNode(key);
                            Logger.Log.Logger.LogData("Control is selected:" + strID, LogLevel.Info);
                        }

                        if ((key != null) && (key != string.Empty))
                        {

                            if (IsDoubleClick)
                            {

                                guiTree.DoubleClickNode(key);
                                Logger.Log.Logger.LogData("GUI Tree control clicked:" + strID, LogLevel.Info);
                            }



                        }


                    }
                    catch (Exception ex)
                    {
                        Log.Logger.LogData(ex.Message, LogLevel.Warning);
                        UseNextPriority = true;
                    }
                }
                else if ((ctrl.ToLower().Trim() == "guiokcodefield"))
                {
                    try
                    {
                        GuiOkCodeField txt = ((GuiOkCodeField)session.FindById(strID));
                        if (Activate)
                        {
                            try
                            {
                                txt.SetFocus();
                                Logger.Log.Logger.LogData("Control activated:" + strID, LogLevel.Info);
                            }
                            catch (Exception ex)
                            {
                                Logger.Log.Logger.LogData("Error while setting focus on " + strID, LogLevel.Warning);
                            }

                            //   IEWATIN.WaitForComplete(5000);
                        }
                        if (ClickBeforeValueSet)
                        {
                            txt.Text = "";
                            Logger.Log.Logger.LogData("Control clicked:" + strID, LogLevel.Info);
                            //   IEWATIN.WaitForComplete(5000);
                        }
                        if ((setValue != null) && (setValue != string.Empty))
                        {
                            txt.Text = setValue;
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


                    return string.Empty;
                }
                else if ((ctrl.ToLower().Trim() == "guitextedit"))
                {
                    try
                    {
                        GuiTextedit txt = ((GuiTextedit)session.FindById(strID));
                        if (Activate)
                        {
                            try
                            {
                                txt.SetFocus();
                                Logger.Log.Logger.LogData("Control activated:" + strID, LogLevel.Info);
                            }
                            catch (Exception ex)
                            {
                                Logger.Log.Logger.LogData("Error while setting focus on " + strID, LogLevel.Warning);
                            }

                            //   IEWATIN.WaitForComplete(5000);
                        }
                        if (ClickBeforeValueSet)
                        {
                            txt.Text = "";
                            Logger.Log.Logger.LogData("Control clicked:" + strID, LogLevel.Info);
                            //   IEWATIN.WaitForComplete(5000);
                        }
                        if ((setValue != null) && (setValue != string.Empty))
                        {
                            txt.Text = setValue;
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


                    return string.Empty;
                }
                else if (ctrl.ToLower().Trim() == "guiradiobutton")
                {
                    try
                    {
                        GuiRadioButton radio = ((GuiRadioButton)session.FindById(strID));
                        if (Activate)
                        {
                            try
                            {
                                radio.SetFocus();
                                Logger.Log.Logger.LogData("Control activated:" + strID, LogLevel.Info);
                            }
                            catch (Exception ex)
                            {
                                Logger.Log.Logger.LogData("Error while setting focus on " + strID, LogLevel.Warning);
                            }

                            //   IEWATIN.WaitForComplete(5000);
                        }
                        if (ClickBeforeValueSet)
                        {
                            try
                            {
                                radio.Select();
                                Logger.Log.Logger.LogData("Control clicked:" + strID, LogLevel.Info);
                            }
                            catch (Exception ex)
                            {
                                Logger.Log.Logger.LogData("Error while before click " + strID, LogLevel.Warning);
                            }
                        }
                        if (IsEventField)
                        {
                            radio.Select();
                            Logger.Log.Logger.LogData("Link Control clicked:" + strID, LogLevel.Info);
                        }
                        else
                        {
                            Logger.Log.Logger.LogData("Link control value get:" + strID, LogLevel.Info);
                            return radio.Text;
                        }
                    }
                    catch (Exception ex)
                    {
                        Log.Logger.LogData(ex.Message, LogLevel.Warning);
                        UseNextPriority = true;
                    }

                    return string.Empty;
                }

                else if (ctrl.ToLower().Trim() == "guicombobox")
                {
                    try
                    {
                        GuiComboBox comboBox = ((GuiComboBox)session.FindById(strID));
                        if (Activate)
                        {
                            try
                            {
                                comboBox.SetFocus();
                                Logger.Log.Logger.LogData("Control activated:" + strID, LogLevel.Info);
                            }
                            catch (Exception ex)
                            {
                                Logger.Log.Logger.LogData("Error while setting focus on " + strID, LogLevel.Warning);
                            }

                            //   IEWATIN.WaitForComplete(5000);
                        }
                        if (ClickBeforeValueSet)
                        {
                            try
                            {
                                comboBox.SetFocus();
                                Logger.Log.Logger.LogData("Control clicked:" + strID, LogLevel.Info);
                            }
                            catch (Exception ex)
                            {
                                Logger.Log.Logger.LogData("Error while before click " + strID, LogLevel.Warning);
                            }
                        }
                        if (IsEventField)
                        {

                            Logger.Log.Logger.LogData("Link Control clicked:" + strID, LogLevel.Info);
                        }
                        else
                        {
                            Logger.Log.Logger.LogData("Link control value get:" + strID, LogLevel.Info);
                            return comboBox.Text;
                        }
                    }
                    catch (Exception ex)
                    {
                        Log.Logger.LogData(ex.Message, LogLevel.Warning);
                        UseNextPriority = true;
                    }

                    return string.Empty;
                }
                else if (ctrl.ToLower().Trim() == "guitab")
                {
                    try
                    {
                        GuiTab guiTab = (GuiTab)session.FindById(strID);
                        if (Activate)
                        {
                            try
                            {
                                guiTab.SetFocus();
                                Logger.Log.Logger.LogData("Control activated:" + strID, LogLevel.Info);
                            }
                            catch (Exception ex)
                            {
                                Logger.Log.Logger.LogData("Error while setting focus on " + strID, LogLevel.Warning);
                            }

                            //   IEWATIN.WaitForComplete(5000);
                        }
                        if (ClickBeforeValueSet)
                        {
                            try
                            {
                                guiTab.SetFocus();
                                Logger.Log.Logger.LogData("Control clicked:" + strID, LogLevel.Info);
                            }
                            catch (Exception ex)
                            {
                                Logger.Log.Logger.LogData("Error while before click " + strID, LogLevel.Warning);
                            }
                        }
                        if (IsEventField)
                        {

                            Logger.Log.Logger.LogData("GuiTab Control clicked:" + strID, LogLevel.Info);
                        }
                        else
                        {
                            Logger.Log.Logger.LogData("GuiTab Control value get:" + strID, LogLevel.Info);
                            return guiTab.Text;
                        }
                    }
                    catch (Exception ex)
                    {
                        Log.Logger.LogData(ex.Message, LogLevel.Warning);
                        UseNextPriority = true;
                    }

                    return string.Empty;
                }
                else if (ctrl.ToLower().Trim() == "guistatusbar")
                {
                    try
                    {
                        GuiStatusbar guiStatusbar = (GuiStatusbar)session.FindById(strID);
                        if (ClickBeforeValueSet)
                        {
                            guiStatusbar.DoubleClick();
                            Logger.Log.Logger.LogData("Control clicked:" + strID, LogLevel.Info);
                            //   IEWATIN.WaitForComplete(5000);
                        }
                        if (IsEventField)
                        {
                            guiStatusbar.DoubleClick();
                            Logger.Log.Logger.LogData("GuiStatusbar control clicked:" + strID, LogLevel.Info);
                        }
                        else
                        {
                            Logger.Log.Logger.LogData("GuiStatusbar control value get:" + strID, LogLevel.Info);
                            return guiStatusbar.Text;
                        }
                    }
                    catch (Exception ex)
                    {
                        Log.Logger.LogData(ex.Message, LogLevel.Warning);
                        UseNextPriority = true;
                    }

                    return string.Empty;
                }
                else if (ctrl.ToLower().Trim() == "guipasswordfield")
                {
                    try
                    {
                        GuiPasswordField guiPasswordField = ((GuiPasswordField)session.FindById(strID));
                        if (Activate)
                        {
                            try
                            {
                                guiPasswordField.SetFocus();
                                Logger.Log.Logger.LogData("Control activated:" + strID, LogLevel.Info);
                            }
                            catch (Exception ex)
                            {
                                Logger.Log.Logger.LogData("Error while setting focus on " + strID, LogLevel.Warning);
                            }

                            //   IEWATIN.WaitForComplete(5000);
                        }
                        if (ClickBeforeValueSet)
                        {
                            try
                            {
                                guiPasswordField.SetFocus();
                                Logger.Log.Logger.LogData("Control clicked:" + strID, LogLevel.Info);
                            }
                            catch (Exception ex)
                            {
                                Logger.Log.Logger.LogData("Error while before click " + strID, LogLevel.Warning);
                            }
                        }
                        if ((setValue != null) && (setValue != string.Empty))
                        {
                            guiPasswordField.Text = setValue;
                            Logger.Log.Logger.LogData("Control value set:" + strID, LogLevel.Info);
                        }
                        else
                        {
                            Logger.Log.Logger.LogData("Control value get:" + strID, LogLevel.Info);
                            return guiPasswordField.Text;
                        }
                    }
                    catch (Exception ex)
                    {
                        Log.Logger.LogData(ex.Message, LogLevel.Warning);
                        UseNextPriority = true;
                    }
                }

                //    return string.Empty;
                //}
                //else if (ctrl.ToLower().Trim() == "tablecell")
                //{
                //    try
                //    {
                //        TableCell tCell = ie.TableCell(Find.ById(strID));
                //        //Assert.IsTrue(tCell.Exists, "Could not Find: " + strID);
                //        if (ClickBeforeValueSet)
                //        {
                //            tCell.Click();
                //            Logger.Log.Logger.LogData("Control clicked:" + strID, LogLevel.Info);
                //            //   IEWATIN.WaitForComplete(5000);
                //        }
                //        if (IsEventField)
                //        {
                //            tCell.Click();
                //            Logger.Log.Logger.LogData("Table cell control clicked:" + strID, LogLevel.Info);
                //        }
                //        else
                //        {
                //            Logger.Log.Logger.LogData("Table cell control value get:" + strID, LogLevel.Info);
                //            return tCell.Text;
                //        }
                //    }
                //    catch (Exception ex)
                //    {
                //        Log.Logger.LogData(ex.Message, LogLevel.Warning);
                //        UseNextPriority = true;
                //    }

                //    return string.Empty;
                //}
                //else if (ctrl.ToLower().Trim() == "table")
                //{
                //    try
                //    {
                //        Table tbl = ie.Table(Find.ById(strID));
                //        if (ClickBeforeValueSet)
                //        {
                //            tbl.Click();
                //            Logger.Log.Logger.LogData("Control clicked:" + strID, LogLevel.Info);
                //            //   IEWATIN.WaitForComplete(5000);
                //        }
                //        if (IsEventField)
                //        {
                //            tbl.Click();
                //            Logger.Log.Logger.LogData("Table control clicked:" + strID, LogLevel.Info);
                //        }
                //        else
                //        {
                //            Logger.Log.Logger.LogData("Table control value get:" + strID, LogLevel.Info);
                //            return tbl.Text;
                //        }
                //    }
                //    catch (Exception ex)
                //    {
                //        Log.Logger.LogData(ex.Message, LogLevel.Warning);
                //        UseNextPriority = true;
                //    }

                //    return string.Empty;
                //}
                //else if (ctrl.ToLower().Trim() == "tablerow")
                //{
                //    try
                //    {
                //        TableRow row = ie.TableRow(Find.ById(strID));
                //        if (ClickBeforeValueSet)
                //        {
                //            row.Click();
                //            Logger.Log.Logger.LogData("Control clicked:" + strID, LogLevel.Info);
                //            //   IEWATIN.WaitForComplete(5000);
                //        }
                //        if (IsEventField)
                //        {
                //            row.Click();
                //            Logger.Log.Logger.LogData("Table row control clicked:" + strID, LogLevel.Info);
                //        }
                //        else
                //        {
                //            Logger.Log.Logger.LogData("Table row control value get:" + strID, LogLevel.Info);
                //            return row.Text;
                //        }
                //    }
                //    catch (Exception ex)
                //    {
                //        Log.Logger.LogData(ex.Message, LogLevel.Warning);
                //        UseNextPriority = true;
                //    }

                //    return string.Empty;
                //}
                //else if (ctrl.ToLower().Trim() == "checkbox")
                //{
                //    try
                //    {
                //        CheckBox chk = ie.CheckBox(Find.ById(strID));
                //        if (ClickBeforeValueSet)
                //        {
                //            chk.Click();
                //            Logger.Log.Logger.LogData("Control clicked:" + strID, LogLevel.Info);
                //            //   IEWATIN.WaitForComplete(5000);
                //        }
                //        if (IsEventField)
                //        {
                //            chk.Click();
                //            Logger.Log.Logger.LogData("Checkbox control clicked:" + strID, LogLevel.Info);
                //        }
                //        else
                //        {
                //            Logger.Log.Logger.LogData("Checkbox control value get:" + strID, LogLevel.Info);
                //            return chk.Text;
                //        }
                //    }
                //    catch (Exception ex)
                //    {
                //        Log.Logger.LogData(ex.Message, LogLevel.Warning);
                //        UseNextPriority = true;
                //    }

                //    return string.Empty;
                //}

            }
            catch (Exception ex)
            {
                Log.Logger.LogData(ex.Message, LogLevel.Error);
            }
            return string.Empty;
        }



    }
}
