// <copyright file=WindowsHelper company=E2E Robotics>
// Copyright (c) 2018 All Rights Reserved
// </copyright>
// <author>Saurabh Mundhe</author>
// <date> 03-10-2018 16:19:13</date>
// <summary></summary>

//using CommonLibrary;
//using Logger;
//using System;
//using System.Collections.Generic;
//using System.Diagnostics;
//using System.Linq;
//using System.Text;
//using System.Threading;
//using System.Threading.Tasks;
//using White.Core;
//using System.Windows.Automation;
//using White.Core.UIItems.Finders;
//using White.Core.UIItems;

//namespace Core.ActivityLibrary
//{
//    public class WindowsHelper
//    {
//        public static White.Core.UIItems.WindowItems.Window GetApplicationObject(string sWindowTitle)
//        {

//            List<White.Core.UIItems.WindowItems.Window> windows = Desktop.Instance.Windows();
//            foreach (White.Core.UIItems.WindowItems.Window item in windows)
//            {
//                if (String.Compare(item.Title, sWindowTitle) == 0)
//                {
//                    return item;
//                }
//            }
//            return null;

//        }

//        public static object TakeActionOnControlByID(White.Core.UIItems.WindowItems.Window window, string strID, string ctrl, bool IsEventField, bool Activate, bool ClickBeforeValueSet, bool ContinueOnError, string setValue, ref bool UseNextPriority)
//        {
//            UseNextPriority = false;


//            if ((ctrl.Trim() == "ControlType.Button"))
//            {
//                try
//                {
//                    White.Core.UIItems.Button button = window.Get<White.Core.UIItems.Button>(SearchCriteria.ByAutomationId(strID));
//                    if (Activate)
//                    {
//                        try
//                        {
//                            window.WaitWhileBusy();
//                            button.Focus();
//                            window.WaitWhileBusy();
//                            Logger.Log.Logger.LogData("Button control activated:" + strID, LogLevel.Info);
//                        }
//                        catch (Exception ex)
//                        {
//                            Logger.Log.Logger.LogData("Error while setting focus on Button by Id" + strID, LogLevel.Warning);
//                        }
//                        Logger.Log.Logger.LogData("Button control activated:" + strID, LogLevel.Info);
//                    }
//                    if (ClickBeforeValueSet)
//                    {
//                        try
//                        {
//                            window.WaitWhileBusy();
//                            button.RaiseClickEvent();
//                            window.WaitWhileBusy();
//                            Logger.Log.Logger.LogData("Button control clicked:" + strID, LogLevel.Info);
//                        }
//                        catch (Exception ex)
//                        {
//                            Logger.Log.Logger.LogData("Error while click before value set on Button by Id" + strID, LogLevel.Warning);
//                        }
//                    }
//                    if (IsEventField)
//                    {
//                        window.WaitWhileBusy();
//                        button.RaiseClickEvent();
//                        window.WaitWhileBusy();
//                        Logger.Log.Logger.LogData("Span control clicked:" + strID, LogLevel.Info);
//                    }
//                    Logger.Log.Logger.LogData("Span control value get:" + strID, LogLevel.Info);
//                    return button.ToString();
//                }
//                catch (Exception ex)
//                { UseNextPriority = true; }
//                return string.Empty;
//            }
//            else if ((ctrl.Trim() == "ControlType.Edit"))
//            {
//                White.Core.UIItems.TextBox txt = window.Get<White.Core.UIItems.TextBox>(SearchCriteria.ByAutomationId(strID));
//                try
//                {
//                    if (Activate)
//                    {
//                        try
//                        {
//                            window.WaitWhileBusy();
//                            txt.Focus();
//                            window.WaitWhileBusy();
//                            Logger.Log.Logger.LogData("Edit control activated:" + strID, LogLevel.Info);
//                        }
//                        catch (Exception ex)
//                        {
//                            Logger.Log.Logger.LogData("Error while setting focus on Edit by Id: " + strID, LogLevel.Warning);
//                        }
//                        Logger.Log.Logger.LogData("Control activated:" + strID, LogLevel.Info);

//                    }
//                    if (ClickBeforeValueSet)
//                    {
//                        try
//                        {
//                            window.WaitWhileBusy();
//                            txt.RaiseClickEvent();
//                            window.WaitWhileBusy();
//                        }
//                        catch (Exception ex)
//                        {
//                            Logger.Log.Logger.LogData("Error while click before value set on Edit by Id: " + strID, LogLevel.Warning);
//                        }
//                    }
//                    if (!string.IsNullOrEmpty(setValue))
//                    {
//                        window.WaitWhileBusy();
//                        txt.Text = setValue;
//                        window.WaitWhileBusy();
//                        Logger.Log.Logger.LogData("Edit control value set:" + strID, LogLevel.Info);
//                    }
//                    Logger.Log.Logger.LogData("Edit control value get:" + strID, LogLevel.Info);
//                    return txt.Text;

//                }
//                catch (Exception ex)
//                { UseNextPriority = true; }
//                return string.Empty;
//            }
//            else if ((ctrl.Trim() == "ControlType.Text"))
//            {
//                try
//                {
//                    White.Core.UIItems.Label label = window.Get<White.Core.UIItems.Label>(SearchCriteria.ByAutomationId(strID));
//                    try
//                    {
//                        if (Activate)
//                        {
//                            try
//                            {
//                                window.WaitWhileBusy();
//                                label.Focus();
//                                window.WaitWhileBusy();
//                                Logger.Log.Logger.LogData("Text control activated:" + strID, LogLevel.Info);
//                            }
//                            catch (Exception ex)
//                            {
//                                Logger.Log.Logger.LogData("Error while setting focus on Text: " + strID, LogLevel.Warning);
//                            }
//                            Logger.Log.Logger.LogData("Text control activated:" + strID, LogLevel.Info);

//                        }
//                        if (ClickBeforeValueSet)
//                        {
//                            try
//                            {
//                                window.WaitWhileBusy();
//                                label.RaiseClickEvent();
//                                window.WaitWhileBusy();
//                            }
//                            catch (Exception ex)
//                            {
//                                Logger.Log.Logger.LogData("Error while click before value set on Text by Id" + strID, LogLevel.Warning);
//                            }
//                        }
//                        if (!string.IsNullOrEmpty(setValue))
//                        {
//                            //label.Text = setValue;
//                            Logger.Log.Logger.LogData("Control value can not be set for label:" + strID, LogLevel.Warning);
//                        }
//                        Logger.Log.Logger.LogData("Text control value get:" + strID, LogLevel.Info);
//                        return label.Text;
//                    }
//                    catch (Exception ex)
//                    {
//                        Log.Logger.LogData(ex.Message, LogLevel.Warning);
//                        UseNextPriority = true;
//                    }
//                }
//                catch (Exception ex)
//                { UseNextPriority = true; }
//                return string.Empty;
//            }
//            else if ((ctrl.Trim() == "ControlType.CheckBox"))
//            {
//                try
//                {
//                    White.Core.UIItems.CheckBox chk = window.Get<White.Core.UIItems.CheckBox>(SearchCriteria.ByAutomationId(strID));
//                    window.WaitWhileBusy();
//                    if (Activate)
//                    {
//                        try
//                        {
//                            window.WaitWhileBusy();
//                            chk.Focus();
//                            window.WaitWhileBusy();
//                            Logger.Log.Logger.LogData("CheckBox control activated:" + strID, LogLevel.Info);
//                        }
//                        catch (Exception ex)
//                        {
//                            Logger.Log.Logger.LogData("Error while setting focus on CheckBox by Id" + strID, LogLevel.Warning);
//                        }
//                        Logger.Log.Logger.LogData("Control activated:" + strID, LogLevel.Info);

//                    }
//                    if (ClickBeforeValueSet)
//                    {
//                        try
//                        {
//                            window.WaitWhileBusy();
//                            chk.RaiseClickEvent();
//                            window.WaitWhileBusy();
//                        }
//                        catch (Exception ex)
//                        {
//                            Logger.Log.Logger.LogData("Error while click before value set on CheckBox by Id " + strID, LogLevel.Warning);
//                        }
//                    }
//                    if (!string.IsNullOrEmpty(setValue))
//                    {
//                        if ((setValue.ToLower().Trim() == "true") || (setValue.ToLower().Trim() == "select") || (setValue.ToLower().Trim() == "1"))
//                        {
//                            chk.Checked = true;
//                        }
//                        else
//                        {
//                            chk.Checked = false;
//                        }
//                    }
//                    if (IsEventField)
//                    {
//                        chk.RaiseClickEvent();
//                        Logger.Log.Logger.LogData("CheckBox control clicked:" + strID, LogLevel.Info);
//                    }
//                    window.WaitWhileBusy();
//                    return chk.Checked.ToString();
//                }
//                catch (Exception ex)
//                { UseNextPriority = true; }
//                return string.Empty;
//            }
//            else if (ctrl.Trim() == "ControlType.RadioButton")
//            {
//                try
//                {
//                    White.Core.UIItems.RadioButton radioButton = window.Get<White.Core.UIItems.RadioButton>(SearchCriteria.ByAutomationId(strID));
//                    window.WaitWhileBusy();
//                    if (Activate)
//                    {
//                        try
//                        {
//                            radioButton.Focus();
//                            Logger.Log.Logger.LogData("RadioButton control activated:" + strID, LogLevel.Info);
//                        }
//                        catch (Exception ex)
//                        {
//                            Logger.Log.Logger.LogData("Error while setting focus on RadioButton by Id" + strID, LogLevel.Warning);
//                        }
//                        Logger.Log.Logger.LogData("Control activated:" + strID, LogLevel.Info);

//                    }
//                    if (ClickBeforeValueSet)
//                    {
//                        try
//                        {
//                            radioButton.RaiseClickEvent();
//                        }
//                        catch (Exception ex)
//                        {
//                            Logger.Log.Logger.LogData("Error while click before value set on RadioButton by Id: " + strID, LogLevel.Warning);
//                        }
//                    }
//                    if (!string.IsNullOrEmpty(setValue))
//                    {
//                        if ((setValue.ToLower().Trim() == "true") || (setValue.ToLower().Trim() == "select") || (setValue.ToLower().Trim() == "1"))
//                        {
//                            radioButton.Select();
//                        }
//                        else
//                        {
//                            radioButton.Select();
//                        }
//                    }
//                    if (IsEventField)
//                    {
//                        radioButton.RaiseClickEvent();
//                        Logger.Log.Logger.LogData("RadioButton control clicked:" + strID, LogLevel.Info);
//                    }
//                    window.WaitWhileBusy();
//                    return radioButton.IsSelected.ToString();
//                }
//                catch (Exception ex)
//                { UseNextPriority = true; }
//                return string.Empty;

//            }
//            else if (ctrl.Trim() == "ControlType.ComboBox")
//            {
//                try
//                {
//                    White.Core.UIItems.ListBoxItems.ComboBox comboBox = window.Get<White.Core.UIItems.ListBoxItems.ComboBox>(SearchCriteria.ByAutomationId(strID));
//                    window.WaitWhileBusy();
//                    if (Activate)
//                    {
//                        try
//                        {
//                            comboBox.Focus();
//                            Logger.Log.Logger.LogData("Combobox Control activated:" + strID, LogLevel.Info);
//                        }
//                        catch (Exception ex)
//                        {
//                            Logger.Log.Logger.LogData("Error while setting focus on ComboBox by Id:" + strID, LogLevel.Warning);
//                        }
//                    }
//                    if (ClickBeforeValueSet)
//                    {
//                        try
//                        {
//                            comboBox.RaiseClickEvent();
//                        }
//                        catch (Exception ex)
//                        {
//                            Logger.Log.Logger.LogData("Error while Click before combobox value set on Id: " + strID, LogLevel.Warning);
//                        }
//                    }
//                    if (!string.IsNullOrEmpty(setValue))
//                    {
//                        comboBox.Select(setValue);
//                        Logger.Log.Logger.LogData("combobox value selected:" + strID, LogLevel.Info);
//                    }
//                    if (IsEventField)
//                    {
//                        comboBox.RaiseClickEvent();
//                        Logger.Log.Logger.LogData("combobox control clicked:" + strID, LogLevel.Info);
//                    }
//                    window.WaitWhileBusy();
//                    return comboBox.SelectedItemText;
//                }
//                catch (Exception ex)
//                {
//                    Logger.Log.Logger.LogData("Error in ComboBox set value by id:" + strID, LogLevel.Warning);
//                    UseNextPriority = true;
//                }
//                return string.Empty;

//            }
//            else if ((ctrl.Trim() == " ControlType.Table"))
//            {
//                try
//                {
//                    White.Core.UIItems.TableItems.Table table = window.Get<White.Core.UIItems.TableItems.Table>(SearchCriteria.ByAutomationId(strID));

//                    window.WaitWhileBusy();
//                    if (Activate)
//                    {
//                        try
//                        {
//                            table.Focus();
//                            Logger.Log.Logger.LogData("Table control activated:" + strID, LogLevel.Info);
//                        }
//                        catch (Exception ex)
//                        {
//                            Logger.Log.Logger.LogData("Error while setting focus on Table by Id: " + strID, LogLevel.Warning);
//                        }
//                    }
//                    if (ClickBeforeValueSet)
//                    {
//                        try
//                        {
//                            table.RaiseClickEvent();
//                        }
//                        catch (Exception ex)
//                        {
//                            Logger.Log.Logger.LogData("Error while Click before Table value set on  by Id: " + strID, LogLevel.Warning);
//                        }
//                    }
//                    if (!string.IsNullOrEmpty(setValue))
//                    {
//                        table.SetValue(setValue); //Need to send table object
//                        Logger.Log.Logger.LogData("Table value selected:" + strID, LogLevel.Info);
//                    }
//                    if (IsEventField)
//                    {
//                        table.RaiseClickEvent();
//                        Logger.Log.Logger.LogData("Table control clicked:" + strID, LogLevel.Info);
//                    }
//                    window.WaitWhileBusy();
//                    return table;
//                }
//                catch (Exception ex)
//                {
//                    Logger.Log.Logger.LogData("Error in table set value by id:" + strID, LogLevel.Warning);
//                    UseNextPriority = true;
//                }
//                return string.Empty;
//            }

//            else if ((ctrl.Trim() == "ControlType.Image"))
//            {
//                try
//                {
//                    White.Core.UIItems.Image image = window.Get<White.Core.UIItems.Image>(SearchCriteria.ByAutomationId(strID));
//                    window.WaitWhileBusy();
//                    if (Activate)
//                    {
//                        try
//                        {
//                            image.Focus();
//                            Logger.Log.Logger.LogData("Image Control activated:" + strID, LogLevel.Info);
//                        }
//                        catch (Exception ex)
//                        {
//                            Logger.Log.Logger.LogData("Error while setting focus on image:" + strID, LogLevel.Warning);
//                        }
//                    }
//                    if (ClickBeforeValueSet)
//                    {
//                        try
//                        {
//                            image.RaiseClickEvent();
//                        }
//                        catch (Exception ex)
//                        {
//                            Logger.Log.Logger.LogData("Error while Click Before image Value Set on Id: " + strID, LogLevel.Warning);
//                        }
//                    }
//                    if (!string.IsNullOrEmpty(setValue))
//                    {
//                        image.SetValue(setValue); //Need to send table object
//                        Logger.Log.Logger.LogData("Image value selected:" + strID, LogLevel.Info);
//                    }
//                    if (IsEventField)
//                    {
//                        image.RaiseClickEvent();
//                        Logger.Log.Logger.LogData("Image control clicked:" + strID, LogLevel.Info);
//                    }
//                    window.WaitWhileBusy();
//                    return image;
//                }
//                catch (Exception ex)
//                {
//                    Logger.Log.Logger.LogData("Error in image set value by id:" + strID, LogLevel.Warning);
//                    UseNextPriority = true;
//                }
//                return string.Empty;
//            }
//            else if (ctrl.Trim() == "ControlType.ListItem")
//            {
//                try
//                {
//                    White.Core.UIItems.ListBoxItems.ListItem listItem = window.Get<White.Core.UIItems.ListBoxItems.ListItem>(SearchCriteria.ByAutomationId(strID));
//                    window.WaitWhileBusy();
//                    if (Activate)
//                    {
//                        try
//                        {
//                            listItem.Focus();
//                            Logger.Log.Logger.LogData("ListItem Control activated:" + strID, LogLevel.Info);
//                        }
//                        catch (Exception ex)
//                        {
//                            Logger.Log.Logger.LogData("Error while setting focus on ListItem:" + strID, LogLevel.Warning);
//                        }
//                    }
//                    if (ClickBeforeValueSet)
//                    {
//                        try
//                        {
//                            listItem.RaiseClickEvent();
//                        }
//                        catch (Exception ex)
//                        {
//                            Logger.Log.Logger.LogData("Error while click before ListItem value set on Id: " + strID, LogLevel.Warning);
//                        }
//                    }
//                    if (!string.IsNullOrEmpty(setValue))
//                    {
//                        listItem.SetValue(setValue); //Need to send table object
//                        Logger.Log.Logger.LogData("ListItem value selected:" + strID, LogLevel.Info);
//                    }
//                    if (IsEventField)
//                    {
//                        listItem.RaiseClickEvent();
//                        Logger.Log.Logger.LogData("ListItem control clicked:" + strID, LogLevel.Info);
//                    }
//                    window.WaitWhileBusy();
//                    return listItem.Text;
//                }
//                catch (Exception ex)
//                { UseNextPriority = true; }
//                return string.Empty;

//            }

//            else if ((ctrl.Trim() == "ControlType.List"))
//            {
//                try
//                {
//                    White.Core.UIItems.ListBoxItems.ListBox comboBox = window.Get<White.Core.UIItems.ListBoxItems.ListBox>(SearchCriteria.ByAutomationId(strID));
//                    window.WaitWhileBusy();
//                    if (Activate)
//                    {
//                        try
//                        {
//                            comboBox.Focus();
//                            Logger.Log.Logger.LogData("List control activated:" + strID, LogLevel.Info);
//                        }
//                        catch (Exception ex)
//                        {
//                            Logger.Log.Logger.LogData("Error while setting focus on List by Id:" + strID, LogLevel.Warning);
//                        }
//                    }
//                    if (ClickBeforeValueSet)
//                    {
//                        try
//                        {
//                            comboBox.RaiseClickEvent();
//                        }
//                        catch (Exception ex)
//                        {
//                            Logger.Log.Logger.LogData("Error while Click before List value set on Id:  " + strID, LogLevel.Warning);
//                        }
//                    }
//                    if (!string.IsNullOrEmpty(setValue))
//                    {
//                        comboBox.SetValue(setValue);
//                        Logger.Log.Logger.LogData("List value selected:" + strID, LogLevel.Info);
//                    }
//                    if (IsEventField)
//                    {
//                        comboBox.RaiseClickEvent();
//                        Logger.Log.Logger.LogData("List control clicked:" + strID, LogLevel.Info);
//                    }
//                    window.WaitWhileBusy();
//                    return comboBox.SelectedItemText;
//                }
//                catch (Exception ex)
//                { UseNextPriority = true; }
//                return string.Empty;
//            }
//            else if (ctrl.Trim() == "ControlType.Menu")
//            {
//                try
//                {
//                    White.Core.UIItems.MenuItems.Menu menuItem = window.Get<White.Core.UIItems.MenuItems.Menu>(SearchCriteria.ByAutomationId(strID));

//                    window.WaitWhileBusy();
//                    if (Activate)
//                    {
//                        try
//                        {
//                            menuItem.Focus();
//                            Logger.Log.Logger.LogData("List control activated:" + strID, LogLevel.Info);
//                        }
//                        catch (Exception ex)
//                        {
//                            Logger.Log.Logger.LogData("Error while setting focus on Menu by Id: " + strID, LogLevel.Warning);
//                        }
//                    }
//                    if (ClickBeforeValueSet)
//                    {
//                        try
//                        {
//                            menuItem.RaiseClickEvent();
//                        }
//                        catch (Exception ex)
//                        {
//                            Logger.Log.Logger.LogData("Error while click before Menu value set on Id: " + strID, LogLevel.Warning);
//                        }
//                    }
//                    if (!string.IsNullOrEmpty(setValue))
//                    {
//                        menuItem.SetValue(setValue);
//                        Logger.Log.Logger.LogData("Menu value selected:" + strID, LogLevel.Info);
//                    }
//                    if (IsEventField)
//                    {
//                        menuItem.RaiseClickEvent();
//                        Logger.Log.Logger.LogData("Menu control clicked:" + strID, LogLevel.Info);
//                    }
//                    window.WaitWhileBusy();
//                    return menuItem.ToString();
//                }
//                catch (Exception ex)
//                { UseNextPriority = true; }
//                return string.Empty;

//            }

//            else if ((ctrl.Trim() == "ControlType.MenuBar"))
//            {
//                try
//                {
//                    White.Core.UIItems.WindowStripControls.MenuBar menuBar = window.Get<White.Core.UIItems.WindowStripControls.MenuBar>(SearchCriteria.ByAutomationId(strID));

//                    window.WaitWhileBusy();
//                    if (Activate)
//                    {
//                        try
//                        {
//                            menuBar.Focus();
//                            Logger.Log.Logger.LogData("MenuBar Control activated:" + strID, LogLevel.Info);
//                        }
//                        catch (Exception ex)
//                        {
//                            Logger.Log.Logger.LogData("Error while setting focus on MenuBar by Id: " + strID, LogLevel.Warning);
//                        }
//                    }
//                    if (ClickBeforeValueSet)
//                    {
//                        try
//                        {
//                            menuBar.RaiseClickEvent();
//                        }
//                        catch (Exception ex)
//                        {
//                            Logger.Log.Logger.LogData("Error while Click before MenuBar value set on Id: " + strID, LogLevel.Warning);
//                        }
//                    }
//                    if (!string.IsNullOrEmpty(setValue))
//                    {
//                        menuBar.SetValue(setValue);
//                        Logger.Log.Logger.LogData("MenuBar value selected:" + strID, LogLevel.Info);
//                    }
//                    if (IsEventField)
//                    {
//                        menuBar.RaiseClickEvent();
//                        Logger.Log.Logger.LogData("MenuBar control clicked:" + strID, LogLevel.Info);
//                    }
//                    window.WaitWhileBusy();
//                    return menuBar.ToString();
//                }
//                catch (Exception ex)
//                { UseNextPriority = true; }
//                return string.Empty;
//            }
//            else if (ctrl.Trim() == "ControlType.MenuItem")
//            {
//                try
//                {
//                    White.Core.UIItems.MenuItems.Menu menuItem = window.Get<White.Core.UIItems.MenuItems.Menu>(SearchCriteria.ByAutomationId(strID));

//                    window.WaitWhileBusy();
//                    if (Activate)
//                    {
//                        try
//                        {
//                            menuItem.Focus();
//                            Logger.Log.Logger.LogData("MenuItem control activated:" + strID, LogLevel.Info);
//                        }
//                        catch (Exception ex)
//                        {
//                            Logger.Log.Logger.LogData("Error while setting focus on MenuItem by Id:" + strID, LogLevel.Warning);
//                        }
//                    }
//                    if (ClickBeforeValueSet)
//                    {
//                        try
//                        {
//                            menuItem.RaiseClickEvent();
//                        }
//                        catch (Exception ex)
//                        {
//                            Logger.Log.Logger.LogData("Error while Click before MenuItem value set on Id: " + strID, LogLevel.Warning);
//                        }
//                    }
//                    if (!string.IsNullOrEmpty(setValue))
//                    {
//                        menuItem.SetValue(setValue);
//                        Logger.Log.Logger.LogData("MenuItem value selected:" + strID, LogLevel.Info);
//                    }
//                    if (IsEventField)
//                    {
//                        menuItem.RaiseClickEvent();
//                        Logger.Log.Logger.LogData("MenuItem control clicked:" + strID, LogLevel.Info);
//                    }
//                    window.WaitWhileBusy();
//                    return menuItem.ToString();
//                }
//                catch (Exception ex)
//                { UseNextPriority = true; }
//                return string.Empty;

//            }
//            else if ((ctrl.Trim() == "ControlType.Calendar") || (ctrl.Trim() == "ControlType.Pane"))
//            {
//                try
//                {
//                    White.Core.UIItems.DateTimePicker sDate = window.Get<White.Core.UIItems.DateTimePicker>(SearchCriteria.ByAutomationId(strID));
//                    window.WaitWhileBusy();
//                    if (sDate != null)
//                    {
//                        if (Activate)
//                        {
//                            try
//                            {
//                                sDate.Focus();
//                                Logger.Log.Logger.LogData("Calendar control activated:" + strID, LogLevel.Info);
//                            }
//                            catch (Exception ex)
//                            {
//                                Logger.Log.Logger.LogData("Error while setting focus on Calendar by Id:" + strID, LogLevel.Warning);
//                            }
//                        }
//                        if (ClickBeforeValueSet)
//                        {
//                            try
//                            {
//                                sDate.RaiseClickEvent();
//                            }
//                            catch (Exception ex)
//                            {
//                                Logger.Log.Logger.LogData("Error while click before Calendar value set on Id: " + strID, LogLevel.Warning);
//                            }
//                        }
//                        if (!string.IsNullOrEmpty(setValue))
//                        {
//                            System.DateTime dateTime = Convert.ToDateTime(setValue);
//                            sDate.SetDate(dateTime, DateFormat.MonthDayYear);
//                            Logger.Log.Logger.LogData("Calendar value selected:" + strID, LogLevel.Info);
//                        }
//                        if (IsEventField)
//                        {
//                            sDate.RaiseClickEvent();
//                            Logger.Log.Logger.LogData("Calendar control clicked:" + strID, LogLevel.Info);
//                        }
//                        window.WaitWhileBusy();
//                        return sDate.Date.ToLongDateString();
//                    }
//                    else
//                    {
//                        White.Core.UIItems.PropertyGridItems.PropertyGrid propertyGrid = window.Get<White.Core.UIItems.PropertyGridItems.PropertyGrid>(SearchCriteria.ByAutomationId(strID));
//                        window.WaitWhileBusy();
//                        if (propertyGrid != null)
//                        {

//                            if (Activate)
//                            {
//                                try
//                                {
//                                    propertyGrid.RaiseClickEvent();
//                                    Logger.Log.Logger.LogData("PropertyGrid control activated:" + strID, LogLevel.Info);
//                                }
//                                catch (Exception ex)
//                                {
//                                    Logger.Log.Logger.LogData("Error while setting focus on PropertyGrid by Id: " + strID, LogLevel.Warning);
//                                }
//                            }
//                            if (ClickBeforeValueSet)
//                            {
//                                try
//                                {
//                                    propertyGrid.RaiseClickEvent();
//                                }
//                                catch (Exception ex)
//                                {
//                                    Logger.Log.Logger.LogData("Error while Click before PropertyGrid value set on Id: " + strID, LogLevel.Warning);
//                                }
//                            }
//                            if (!string.IsNullOrEmpty(setValue))
//                            {
//                                try
//                                {
//                                    propertyGrid.SetValue(setValue);
//                                    Logger.Log.Logger.LogData("PropertyGrid value selected:" + strID, LogLevel.Info);
//                                }
//                                catch (Exception ex)
//                                {
//                                    Logger.Log.Logger.LogData("Error in propertyGrid set value by Id:" + strID, LogLevel.Info);
//                                }
//                            }
//                            if (IsEventField)
//                            {
//                                propertyGrid.RaiseClickEvent();
//                                Logger.Log.Logger.LogData("PropertyGrid control clicked:" + strID, LogLevel.Info);
//                            }
//                            window.WaitWhileBusy();
//                            return propertyGrid;
//                        }
//                    }
//                    window.WaitWhileBusy();
//                }
//                catch (Exception ex)
//                { UseNextPriority = true; }
//                return string.Empty;

//            }

//            else if (ctrl.Trim() == "ControlType.Hyperlink")
//            {
//                try
//                {
//                    White.Core.UIItems.Hyperlink hlink = window.Get<White.Core.UIItems.Hyperlink>(SearchCriteria.ByAutomationId(strID));
//                    window.WaitWhileBusy();
//                    if (Activate)
//                    {
//                        try
//                        {
//                            hlink.RaiseClickEvent();
//                            Logger.Log.Logger.LogData("Hyperlink Control activated:" + strID, LogLevel.Info);
//                        }
//                        catch (Exception ex)
//                        {
//                            Logger.Log.Logger.LogData("Error while setting focus on Hyperlink by Id:" + strID, LogLevel.Warning);
//                        }
//                    }
//                    if (ClickBeforeValueSet)
//                    {
//                        try
//                        {
//                            hlink.RaiseClickEvent();
//                        }
//                        catch (Exception ex)
//                        {
//                            Logger.Log.Logger.LogData("Error while Click before Hyperlink value set on Id: " + strID, LogLevel.Warning);
//                        }
//                    }
//                    if (!string.IsNullOrEmpty(setValue))
//                    {
//                        try
//                        {
//                            hlink.SetValue(setValue);
//                            Logger.Log.Logger.LogData("Hyperlink value selected:" + strID, LogLevel.Info);
//                        }
//                        catch (Exception ex)
//                        {
//                            Logger.Log.Logger.LogData("Error in Hyperlink set value by Id:" + strID, LogLevel.Info);
//                        }
//                    }
//                    if (IsEventField)
//                    {
//                        hlink.RaiseClickEvent();
//                        Logger.Log.Logger.LogData("Hyperlink control clicked:" + strID, LogLevel.Info);
//                    }
//                    window.WaitWhileBusy();
//                    return hlink.ToString();
//                }
//                catch (Exception ex)
//                { UseNextPriority = true; }
//                return string.Empty;

//            }

//            else if ((ctrl.Trim() == " ControlType.Tab"))
//            {
//                try
//                {
//                    White.Core.UIItems.TabItems.Tab tab = window.Get<White.Core.UIItems.TabItems.Tab>(SearchCriteria.ByAutomationId(strID));
//                    window.WaitWhileBusy();
//                    if (Activate)
//                    {
//                        try
//                        {
//                            tab.RaiseClickEvent();
//                            Logger.Log.Logger.LogData("Tab control activated:" + strID, LogLevel.Info);
//                        }
//                        catch (Exception ex)
//                        {
//                            Logger.Log.Logger.LogData("Error while setting focus on Tab by Id: " + strID, LogLevel.Warning);
//                        }
//                    }
//                    if (ClickBeforeValueSet)
//                    {
//                        try
//                        {
//                            tab.RaiseClickEvent();
//                        }
//                        catch (Exception ex)
//                        {
//                            Logger.Log.Logger.LogData("Error while Click before Tab value set on Id: " + strID, LogLevel.Warning);
//                        }
//                    }
//                    if (!string.IsNullOrEmpty(setValue))
//                    {
//                        try
//                        {
//                            tab.SetValue(setValue);
//                            Logger.Log.Logger.LogData("Tab value selected:" + strID, LogLevel.Info);
//                        }
//                        catch (Exception ex)
//                        {
//                            Logger.Log.Logger.LogData("Error in Tab set value by Id:" + strID, LogLevel.Info);
//                        }
//                    }
//                    if (IsEventField)
//                    {
//                        tab.RaiseClickEvent();
//                        Logger.Log.Logger.LogData("Tab control clicked:" + strID, LogLevel.Info);
//                    }
//                    window.WaitWhileBusy();
//                    return tab.ToString();
//                }
//                catch (Exception ex)
//                { UseNextPriority = true; }
//                return string.Empty;
//            }
//            else if (ctrl.Trim() == "ControlType.TabItem")
//            {
//                try
//                {
//                    White.Core.UIItems.TabItems.TabPage tabPage = window.Get<White.Core.UIItems.TabItems.TabPage>(SearchCriteria.ByAutomationId(strID));
//                    window.WaitWhileBusy();
//                    if (Activate)
//                    {
//                        try
//                        {
//                            tabPage.RaiseClickEvent();
//                            Logger.Log.Logger.LogData("TabItem control activated:" + strID, LogLevel.Info);
//                        }
//                        catch (Exception ex)
//                        {
//                            Logger.Log.Logger.LogData("Error while setting focus on TabItem by Id: " + strID, LogLevel.Warning);
//                        }
//                    }
//                    if (ClickBeforeValueSet)
//                    {
//                        try
//                        {
//                            tabPage.RaiseClickEvent();
//                        }
//                        catch (Exception ex)
//                        {
//                            Logger.Log.Logger.LogData("Error while click before TabItem value set on Id: " + strID, LogLevel.Warning);
//                        }
//                    }
//                    if (!string.IsNullOrEmpty(setValue))
//                    {
//                        try
//                        {
//                            tabPage.SetValue(setValue);
//                            Logger.Log.Logger.LogData("TabItem value selected:" + strID, LogLevel.Info);
//                        }
//                        catch (Exception ex)
//                        {
//                            Logger.Log.Logger.LogData("Error in TabItem set value by Id:" + strID, LogLevel.Info);
//                        }
//                    }
//                    if (IsEventField)
//                    {
//                        tabPage.RaiseClickEvent();
//                        Logger.Log.Logger.LogData("TabItem control clicked:" + strID, LogLevel.Info);
//                    }
//                    window.WaitWhileBusy();
//                    return tabPage.ToString();
//                }
//                catch (Exception ex)
//                { UseNextPriority = true; }
//                return string.Empty;

//            }
//            else if ((ctrl.Trim() == "ControlType.DataGrid"))
//            {
//                try
//                {
//                    White.Core.UIItems.ListView dataGrid = window.Get<White.Core.UIItems.ListView>(SearchCriteria.ByAutomationId(strID));

//                    window.WaitWhileBusy();
//                    if (Activate)
//                    {
//                        try
//                        {
//                            dataGrid.RaiseClickEvent();
//                            Logger.Log.Logger.LogData("DataGrid control activated:" + strID, LogLevel.Info);
//                        }
//                        catch (Exception ex)
//                        {
//                            Logger.Log.Logger.LogData("Error while setting focus on DataGrid by Id: " + strID, LogLevel.Warning);
//                        }
//                    }
//                    if (ClickBeforeValueSet)
//                    {
//                        try
//                        {
//                            dataGrid.RaiseClickEvent();
//                        }
//                        catch (Exception ex)
//                        {
//                            Logger.Log.Logger.LogData("Error while Click before List value set on Id: " + strID, LogLevel.Warning);
//                        }
//                    }
//                    if (!string.IsNullOrEmpty(setValue))
//                    {
//                        try
//                        {
//                            dataGrid.SetValue(setValue);
//                            Logger.Log.Logger.LogData("List value selected:" + strID, LogLevel.Info);
//                        }
//                        catch (Exception ex)
//                        {
//                            Logger.Log.Logger.LogData("Error in propertyGrid set value by Id:" + strID, LogLevel.Info);
//                        }
//                    }
//                    if (IsEventField)
//                    {
//                        dataGrid.RaiseClickEvent();
//                        Logger.Log.Logger.LogData("List control clicked:" + strID, LogLevel.Info);
//                    }
//                    window.WaitWhileBusy();
//                    return dataGrid;
//                }
//                catch (Exception ex)
//                { UseNextPriority = true; }
//                return string.Empty;
//            }
//            else if (ctrl.Trim() == "ControlType.DataItem")
//            {
//                try
//                {
//                    White.Core.UIItems.ListViewRow dataGridrow = window.Get<White.Core.UIItems.ListViewRow>(SearchCriteria.ByAutomationId(strID));

//                    window.WaitWhileBusy();
//                    if (Activate)
//                    {
//                        try
//                        {
//                            dataGridrow.RaiseClickEvent();
//                            Logger.Log.Logger.LogData("List control activated:" + strID, LogLevel.Info);
//                        }
//                        catch (Exception ex)
//                        {
//                            Logger.Log.Logger.LogData("Error while setting focus on List by Id:" + strID, LogLevel.Warning);
//                        }
//                    }
//                    if (ClickBeforeValueSet)
//                    {
//                        try
//                        {
//                            dataGridrow.RaiseClickEvent();
//                        }
//                        catch (Exception ex)
//                        {
//                            Logger.Log.Logger.LogData("Error while Click before List value set on Id: " + strID, LogLevel.Warning);
//                        }
//                    }
//                    if (!string.IsNullOrEmpty(setValue))
//                    {
//                        try
//                        {
//                            dataGridrow.SetValue(setValue);
//                            Logger.Log.Logger.LogData("List value selected:" + strID, LogLevel.Info);
//                        }
//                        catch (Exception ex)
//                        {
//                            Logger.Log.Logger.LogData("Error in propertyGrid set value by Id:" + strID, LogLevel.Info);
//                        }
//                    }
//                    if (IsEventField)
//                    {
//                        dataGridrow.RaiseClickEvent();
//                        Logger.Log.Logger.LogData("List control clicked:" + strID, LogLevel.Info);
//                    }
//                    window.WaitWhileBusy();
//                    return dataGridrow;
//                }
//                catch (Exception ex)
//                { UseNextPriority = true; }
//                return string.Empty;

//            }
//            else if ((ctrl.Trim() == " ControlType.TreeItem") || (ctrl.Trim() == "ControlType.Tree"))
//            {
//                try
//                {
//                    White.Core.UIItems.TreeItems.TreeNode nodeOne = window.Get<White.Core.UIItems.TreeItems.TreeNode>(SearchCriteria.ByAutomationId(strID));
//                    //nodeOne.Select();
//                    //nodeOne.Expand();
//                    window.WaitWhileBusy();
//                    if (Activate)
//                    {
//                        try
//                        {
//                            nodeOne.RaiseClickEvent();
//                            Logger.Log.Logger.LogData("TreeNode control activated:" + strID, LogLevel.Info);
//                        }
//                        catch (Exception ex)
//                        {
//                            Logger.Log.Logger.LogData("Error while setting focus on TreeNode by Id: " + strID, LogLevel.Warning);
//                        }
//                    }
//                    if (ClickBeforeValueSet)
//                    {
//                        try
//                        {
//                            nodeOne.RaiseClickEvent();
//                        }
//                        catch (Exception ex)
//                        {
//                            Logger.Log.Logger.LogData("Error while Click before TreeNode value set on Id: " + strID, LogLevel.Warning);
//                        }
//                    }
//                    if (!string.IsNullOrEmpty(setValue))
//                    {
//                        try
//                        {
//                            nodeOne.Select();
//                            Logger.Log.Logger.LogData("TreeNode value selected:" + strID, LogLevel.Info);
//                        }
//                        catch (Exception ex)
//                        {
//                            Logger.Log.Logger.LogData("Error in TreeNode set value by Id:" + strID, LogLevel.Info);
//                        }
//                    }
//                    if (IsEventField)
//                    {
//                        nodeOne.RaiseClickEvent();
//                        Logger.Log.Logger.LogData("TreeNode control clicked:" + strID, LogLevel.Info);
//                    }
//                    window.WaitWhileBusy();
//                    return nodeOne.Text;
//                }
//                catch (Exception ex)
//                { UseNextPriority = true; }
//                return string.Empty;
//            }
//            else if ((ctrl.Trim() == " ControlType.ProgressBar"))
//            {
//                try
//                {
//                    White.Core.UIItems.ProgressBar progressBar = window.Get<White.Core.UIItems.ProgressBar>(SearchCriteria.ByAutomationId(strID));
//                    progressBar.RaiseClickEvent();
//                    do
//                    {
//                        Thread.Sleep(5000);
//                    } while (progressBar.Value != progressBar.Maximum);

//                }
//                catch (Exception ex)
//                { UseNextPriority = true; }
//                return string.Empty;
//            }

//            else if (ctrl.Trim() == "ControlType.Slider")
//            {
//                try
//                {
//                    White.Core.UIItems.Slider sldrOne = window.Get<White.Core.UIItems.Slider>(SearchCriteria.ByAutomationId(strID));

//                    sldrOne.LargeIncrementButton.RaiseClickEvent();
//                    //   sldrOne.LargeDecrementButton.RaiseClickEvent();
//                }
//                catch (Exception ex)
//                { UseNextPriority = true; }
//                return string.Empty;

//            }

//            else if ((ctrl.Trim() == "ControlType.Spinner"))
//            {
//                try
//                {
//                    White.Core.UIItems.Spinner spinner = window.Get<White.Core.UIItems.Spinner>(SearchCriteria.ByAutomationId(strID));
//                    window.WaitWhileBusy();
//                    if (Activate)
//                    {
//                        try
//                        {
//                            spinner.RaiseClickEvent();
//                            Logger.Log.Logger.LogData("Spinner control activated:" + strID, LogLevel.Info);
//                        }
//                        catch (Exception ex)
//                        {
//                            Logger.Log.Logger.LogData("Error while setting focus on Spinner by Id: " + strID, LogLevel.Warning);
//                        }
//                    }
//                    if (ClickBeforeValueSet)
//                    {
//                        try
//                        {
//                            spinner.RaiseClickEvent();
//                        }
//                        catch (Exception ex)
//                        {
//                            Logger.Log.Logger.LogData("Error while Click before Spinner value set on Id: " + strID, LogLevel.Warning);
//                        }
//                    }
//                    if (!string.IsNullOrEmpty(setValue))
//                    {
//                        try
//                        {
//                            spinner.SetValue(setValue);
//                            Logger.Log.Logger.LogData("Spinner value selected:" + strID, LogLevel.Info);
//                        }
//                        catch (Exception ex)
//                        {
//                            Logger.Log.Logger.LogData("Error in Spinner set value by Id:" + strID, LogLevel.Info);
//                        }
//                    }
//                    if (IsEventField)
//                    {
//                        spinner.RaiseClickEvent();
//                        Logger.Log.Logger.LogData("Spinner control clicked:" + strID, LogLevel.Info);
//                    }
//                    window.WaitWhileBusy();
//                    return spinner.Value.ToString();
//                }
//                catch (Exception ex)
//                { UseNextPriority = true; }
//                return string.Empty;
//            }

//            else if (ctrl.Trim() == "ControlType.ToolBar")
//            {
//                try
//                {
//                    White.Core.UIItems.WindowStripControls.ToolStrip toolStrip = window.Get<White.Core.UIItems.WindowStripControls.ToolStrip>(SearchCriteria.ByAutomationId(strID));
//                    window.WaitWhileBusy();
//                    if (Activate)
//                    {
//                        try
//                        {
//                            toolStrip.RaiseClickEvent();
//                            Logger.Log.Logger.LogData("ToolBar control activated:" + strID, LogLevel.Info);
//                        }
//                        catch (Exception ex)
//                        {
//                            Logger.Log.Logger.LogData("Error while setting focus on ToolBar by Id: " + strID, LogLevel.Warning);
//                        }
//                    }
//                    if (ClickBeforeValueSet)
//                    {
//                        try
//                        {
//                            toolStrip.RaiseClickEvent();
//                        }
//                        catch (Exception ex)
//                        {
//                            Logger.Log.Logger.LogData("Error while Click before ToolBar value set on Id: " + strID, LogLevel.Warning);
//                        }
//                    }
//                    if (!string.IsNullOrEmpty(setValue))
//                    {
//                        try
//                        {
//                            toolStrip.SetValue(setValue);
//                            Logger.Log.Logger.LogData("ToolBar value selected:" + strID, LogLevel.Info);
//                        }
//                        catch (Exception ex)
//                        {
//                            Logger.Log.Logger.LogData("Error in ToolBar set value by Id:" + strID, LogLevel.Info);
//                        }
//                    }
//                    if (IsEventField)
//                    {
//                        toolStrip.RaiseClickEvent();
//                        Logger.Log.Logger.LogData("ToolBar control clicked:" + strID, LogLevel.Info);
//                    }
//                    window.WaitWhileBusy();
//                    return toolStrip.ToString();
//                }
//                catch (Exception ex)
//                { UseNextPriority = true; }
//                return string.Empty;

//            }

//            else if ((ctrl.Trim() == "ControlType.ToolTip"))
//            {
//                try
//                {
//                    White.Core.UIItems.ToolTip toolTip = window.Get<White.Core.UIItems.ToolTip>(SearchCriteria.ByAutomationId(strID));
//                    window.WaitWhileBusy();
//                    if (Activate)
//                    {
//                        try
//                        {
//                            toolTip.RaiseClickEvent();
//                            Logger.Log.Logger.LogData("ToolTip control activated:" + strID, LogLevel.Info);
//                        }
//                        catch (Exception ex)
//                        {
//                            Logger.Log.Logger.LogData("Error while setting focus on ToolTip by Id:" + strID, LogLevel.Warning);
//                        }
//                    }
//                    if (ClickBeforeValueSet)
//                    {
//                        try
//                        {
//                            toolTip.RaiseClickEvent();
//                        }
//                        catch (Exception ex)
//                        {
//                            Logger.Log.Logger.LogData("Error while Click before ToolTip value set on Id: " + strID, LogLevel.Warning);
//                        }
//                    }
//                    if (!string.IsNullOrEmpty(setValue))
//                    {
//                        try
//                        {
//                            toolTip.SetValue(setValue);
//                            Logger.Log.Logger.LogData("ToolTip value selected:" + strID, LogLevel.Info);
//                        }
//                        catch (Exception ex)
//                        {
//                            Logger.Log.Logger.LogData("Error in ToolTip set value by Id:" + strID, LogLevel.Info);
//                        }
//                    }
//                    if (IsEventField)
//                    {
//                        toolTip.RaiseClickEvent();
//                        Logger.Log.Logger.LogData("ToolTip control clicked:" + strID, LogLevel.Info);
//                    }
//                    window.WaitWhileBusy();
//                    return toolTip.Text;

//                }
//                catch (Exception ex)
//                { UseNextPriority = true; }
//                return string.Empty;
//            }
//            else if ((ctrl.Trim() == "ControlType.Group"))
//            {
//                try
//                {
//                    White.Core.UIItems.GroupBox groupBox = window.Get<White.Core.UIItems.GroupBox>(SearchCriteria.ByAutomationId(strID));

//                    window.WaitWhileBusy();
//                    if (Activate)
//                    {
//                        try
//                        {
//                            groupBox.RaiseClickEvent();
//                            Logger.Log.Logger.LogData("Group control activated:" + strID, LogLevel.Info);
//                        }
//                        catch (Exception ex)
//                        {
//                            Logger.Log.Logger.LogData("Error while setting focus on Group by Id:" + strID, LogLevel.Warning);
//                        }
//                    }
//                    if (ClickBeforeValueSet)
//                    {
//                        try
//                        {
//                            groupBox.RaiseClickEvent();
//                        }
//                        catch (Exception ex)
//                        {
//                            Logger.Log.Logger.LogData("Error while Click before Group value set on Id: " + strID, LogLevel.Warning);
//                        }
//                    }
//                    if (!string.IsNullOrEmpty(setValue))
//                    {
//                        try
//                        {
//                            groupBox.SetValue(setValue);
//                            Logger.Log.Logger.LogData("Group value selected:" + strID, LogLevel.Info);
//                        }
//                        catch (Exception ex)
//                        {
//                            Logger.Log.Logger.LogData("Error in Group set value by Id:" + strID, LogLevel.Info);
//                        }
//                    }
//                    if (IsEventField)
//                    {
//                        groupBox.RaiseClickEvent();
//                        Logger.Log.Logger.LogData("Group control clicked:" + strID, LogLevel.Info);
//                    }
//                    window.WaitWhileBusy();
//                    return groupBox.ToString();
//                }
//                catch (Exception ex)
//                { UseNextPriority = true; }
//                return string.Empty;
//            }
//            else if (ctrl.Trim() == "ControlType.Thumb")
//            {
//                try
//                {
//                    White.Core.UIItems.Thumb thumb = window.Get<White.Core.UIItems.Thumb>(SearchCriteria.ByAutomationId(strID));

//                    window.WaitWhileBusy();
//                    if (Activate)
//                    {
//                        try
//                        {
//                            thumb.RaiseClickEvent();
//                            Logger.Log.Logger.LogData("Thumb control activated:" + strID, LogLevel.Info);
//                        }
//                        catch (Exception ex)
//                        {
//                            Logger.Log.Logger.LogData("Error while setting focus on Thumb by Id:" + strID, LogLevel.Warning);
//                        }
//                    }
//                    if (ClickBeforeValueSet)
//                    {
//                        try
//                        {
//                            thumb.RaiseClickEvent();
//                        }
//                        catch (Exception ex)
//                        {
//                            Logger.Log.Logger.LogData("Error while Click before Thumb value set on Id: " + strID, LogLevel.Warning);
//                        }
//                    }
//                    if (!string.IsNullOrEmpty(setValue))
//                    {
//                        try
//                        {
//                            thumb.SetValue(setValue);
//                            Logger.Log.Logger.LogData("Thumb value selected:" + strID, LogLevel.Info);
//                        }
//                        catch (Exception ex)
//                        {
//                            Logger.Log.Logger.LogData("Error in Thumb set value by Id:" + strID, LogLevel.Info);
//                        }
//                    }
//                    if (IsEventField)
//                    {
//                        thumb.RaiseClickEvent();
//                        Logger.Log.Logger.LogData("Thumb control clicked:" + strID, LogLevel.Info);
//                    }
//                    window.WaitWhileBusy();
//                    return thumb.ToString();
//                }
//                catch (Exception ex)
//                { UseNextPriority = true; }
//                return string.Empty;

//            }
//            else if (ctrl.Trim() == "ControlType.TitleBar")
//            {
//                try
//                {
//                    White.Core.UIItems.WindowItems.TitleBar titleBar = window.Get<White.Core.UIItems.WindowItems.TitleBar>(SearchCriteria.ByAutomationId(strID));

//                    window.WaitWhileBusy();
//                    if (Activate)
//                    {
//                        try
//                        {
//                            titleBar.RaiseClickEvent();
//                            Logger.Log.Logger.LogData("TitleBar control activated:" + strID, LogLevel.Info);
//                        }
//                        catch (Exception ex)
//                        {
//                            Logger.Log.Logger.LogData("Error while setting focus on TitleBar by Id:" + strID, LogLevel.Warning);
//                        }
//                    }
//                    if (ClickBeforeValueSet)
//                    {
//                        try
//                        {
//                            titleBar.RaiseClickEvent();
//                        }
//                        catch (Exception ex)
//                        {
//                            Logger.Log.Logger.LogData("Error while Click before TitleBar value set on Id: " + strID, LogLevel.Warning);
//                        }
//                    }
//                    if (!string.IsNullOrEmpty(setValue))
//                    {
//                        try
//                        {
//                            titleBar.SetValue(setValue);
//                            Logger.Log.Logger.LogData("TitleBar value selected:" + strID, LogLevel.Info);
//                        }
//                        catch (Exception ex)
//                        {
//                            Logger.Log.Logger.LogData("Error in TitleBar set value by Id:" + strID, LogLevel.Info);
//                        }
//                    }
//                    if (IsEventField)
//                    {
//                        titleBar.RaiseClickEvent();
//                        Logger.Log.Logger.LogData("TitleBar control clicked:" + strID, LogLevel.Info);
//                    }
//                    window.WaitWhileBusy();
//                    return titleBar.ToString();
//                }
//                catch (Exception ex)
//                { UseNextPriority = true; }
//                return string.Empty;

//            }

//            #region "Not Supported - do not delete - for future implementation"
//            //else if (ctrl.Trim() == "ControlType.StatusBar")
//            //{
//            //    try
//            //    {
//            //        //                    White.Core.UIItems.Thumb thumb = window.Get<White.Core.UIItems.Thumb>(SearchCriteria.ByAutomationId(strID));
//            //    }
//            //    catch (Exception ex)
//            //    { UseNextPriority = true; }
//            //    return string.Empty;

//            //}
//            //else if ((ctrl.Trim() == "ControlType.ScrollBar"))
//            //{
//            //    try
//            //    {
//            //        //White.Core.UIItems.ProgressBar progressBar = window.Get<ScrollBar>(SearchCriteria.ByAutomationId(strID));
//            //    }
//            //    catch (Exception ex)
//            //    { UseNextPriority = true; }
//            //    return string.Empty;
//            //}
//            //else if (ctrl.Trim() == "ControlType.SplitButton")
//            //{
//            //    try
//            //    {
//            //         White.Core.UIItems.ListViewRow dataGridrow = window.Get<SplitButton>(SearchCriteria.ByAutomationId(strID));
//            //    }
//            //    catch (Exception ex)
//            //    { UseNextPriority = true; }
//            //    return string.Empty;

//            //}
//            //else if ((ctrl.Trim() == " ControlType.Document"))
//            //{
//            //    try
//            //    {
//            //        //White.Core.UIItems.Document document = window.Get<Document>(SearchCriteria.ByAutomationId(strID));
//            //    }
//            //    catch (Exception ex)
//            //    { UseNextPriority = true; }
//            //    return string.Empty;
//            //}
//            //else if ((ctrl.Trim() == "ControlType.Window"))
//            //{
//            //    try
//            //    {

//            //    }
//            //    catch (Exception ex)
//            //    { UseNextPriority = true; }
//            //    return string.Empty;
//            //}
//            //else if ((ctrl.Trim() == "ControlType.Header"))
//            //{
//            //    try
//            //    {

//            //    }
//            //    catch (Exception ex)
//            //    { UseNextPriority = true; }
//            //    return string.Empty;
//            //}
//            //else if (ctrl.Trim() == "ControlType.HeaderItem")
//            //{
//            //    try
//            //    {
//            //       // White.Core.UIItems.WindowItems.TitleBar titleBar = window.Get<HeaderItem>(SearchCriteria.ByAutomationId(strID));
//            //    }
//            //    catch (Exception ex)
//            //    { UseNextPriority = true; }
//            //    return string.Empty;

//            //}
//            //else if (ctrl.Trim() == "ControlType.Custom")
//            //{
//            //    try
//            //    {
//            //        //White.Core.UIItems.Custom groupBox = window.Get<White.Core.UIItems.Custom>(SearchCriteria.ByAutomationId(strID));
//            //    }
//            //    catch (Exception ex)
//            //    { UseNextPriority = true; }
//            //    return string.Empty;

//            //}
//            //else if ((ctrl.Trim() == "ControlType.Separator"))
//            //{
//            //    try
//            //    {
//            //        //White.Core.UIItems.WindowItems.TitleBar titleBar = window.Get<Separator>(SearchCriteria.ByAutomationId(strID));
//            //        //titleBar.RaiseClickEvent();
//            //    }
//            //    catch (Exception ex)
//            //    { UseNextPriority = true; }
//            //    return string.Empty;
//            //}
//            #endregion

//            else
//            {
//                return null;
//            }

//        }

//        public static object TakeActionOnControlByName(White.Core.UIItems.WindowItems.Window window, string strName, string ctrl, bool IsEventField, bool Activate, bool ClickBeforeValueSet, bool ContinueOnError, string setValue, ref bool UseNextPriority)
//        {
//            UseNextPriority = false;
//            if ((ctrl.Trim() == "ControlType.Button"))
//            {
//                try
//                {
//                    White.Core.UIItems.Button button = window.Get<White.Core.UIItems.Button>(SearchCriteria.ByText(strName));
//                    if (Activate)
//                    {
//                        try
//                        {
//                            window.WaitWhileBusy();
//                            button.Focus();
//                            window.WaitWhileBusy();
//                            Logger.Log.Logger.LogData("Button control activated:" + strName, LogLevel.Info);
//                        }
//                        catch (Exception ex)
//                        {
//                            Logger.Log.Logger.LogData("Error while setting focus on Button by Name: " + strName, LogLevel.Warning);
//                        }
//                        Logger.Log.Logger.LogData("Button control activated:" + strName, LogLevel.Info);
//                    }
//                    if (ClickBeforeValueSet)
//                    {
//                        try
//                        {
//                            window.WaitWhileBusy();
//                            button.RaiseClickEvent();
//                            window.WaitWhileBusy();
//                            Logger.Log.Logger.LogData("Button control clicked:" + strName, LogLevel.Info);
//                        }
//                        catch (Exception ex)
//                        {
//                            Logger.Log.Logger.LogData("Error while click before value set on Button by Name: " + strName, LogLevel.Warning);
//                        }
//                    }
//                    if (IsEventField)
//                    {
//                        window.WaitWhileBusy();
//                        button.RaiseClickEvent();
//                        window.WaitWhileBusy();
//                        Logger.Log.Logger.LogData("Span control clicked:" + strName, LogLevel.Info);
//                    }
//                    Logger.Log.Logger.LogData("Span control value get:" + strName, LogLevel.Info);
//                    return button.ToString();
//                }
//                catch (Exception ex)
//                { UseNextPriority = true; }
//                return string.Empty;
//            }
//            else if ((ctrl.Trim() == "ControlType.Edit"))
//            {
//                White.Core.UIItems.TextBox txt = window.Get<White.Core.UIItems.TextBox>(SearchCriteria.ByText(strName));
//                try
//                {
//                    if (Activate)
//                    {
//                        try
//                        {
//                            window.WaitWhileBusy();
//                            txt.Focus();
//                            window.WaitWhileBusy();
//                            Logger.Log.Logger.LogData("Edit control activated:" + strName, LogLevel.Info);
//                        }
//                        catch (Exception ex)
//                        {
//                            Logger.Log.Logger.LogData("Error while setting focus on Edit by Name: " + strName, LogLevel.Warning);
//                        }
//                        Logger.Log.Logger.LogData("Control activated:" + strName, LogLevel.Info);

//                    }
//                    if (ClickBeforeValueSet)
//                    {
//                        try
//                        {
//                            window.WaitWhileBusy();
//                            txt.RaiseClickEvent();
//                            window.WaitWhileBusy();
//                        }
//                        catch (Exception ex)
//                        {
//                            Logger.Log.Logger.LogData("Error while click before value set on Edit by Name: " + strName, LogLevel.Warning);
//                        }
//                    }
//                    if (!string.IsNullOrEmpty(setValue))
//                    {
//                        window.WaitWhileBusy();
//                        txt.Text = setValue;
//                        window.WaitWhileBusy();
//                        Logger.Log.Logger.LogData("Edit control value set:" + strName, LogLevel.Info);
//                    }
//                    Logger.Log.Logger.LogData("Edit control value get:" + strName, LogLevel.Info);
//                    return txt.Text;

//                }
//                catch (Exception ex)
//                { UseNextPriority = true; }
//                return string.Empty;
//            }
//            else if ((ctrl.Trim() == "ControlType.Text"))
//            {
//                try
//                {
//                    White.Core.UIItems.Label label = window.Get<White.Core.UIItems.Label>(SearchCriteria.ByText(strName));
//                    try
//                    {
//                        if (Activate)
//                        {
//                            try
//                            {
//                                window.WaitWhileBusy();
//                                label.Focus();
//                                window.WaitWhileBusy();
//                                Logger.Log.Logger.LogData("Text control activated:" + strName, LogLevel.Info);
//                            }
//                            catch (Exception ex)
//                            {
//                                Logger.Log.Logger.LogData("Error while setting focus on Text: " + strName, LogLevel.Warning);
//                            }
//                            Logger.Log.Logger.LogData("Text control activated:" + strName, LogLevel.Info);

//                        }
//                        if (ClickBeforeValueSet)
//                        {
//                            try
//                            {
//                                window.WaitWhileBusy();
//                                label.RaiseClickEvent();
//                                window.WaitWhileBusy();
//                            }
//                            catch (Exception ex)
//                            {
//                                Logger.Log.Logger.LogData("Error while click before value set on Text by Name: " + strName, LogLevel.Warning);
//                            }
//                        }
//                        if (!string.IsNullOrEmpty(setValue))
//                        {
//                            //label.Text = setValue;
//                            Logger.Log.Logger.LogData("Control value can not be set for label:" + strName, LogLevel.Warning);
//                        }
//                        Logger.Log.Logger.LogData("Text control value get:" + strName, LogLevel.Info);
//                        return label.Text;
//                    }
//                    catch (Exception ex)
//                    {
//                        Log.Logger.LogData(ex.Message, LogLevel.Warning);
//                        UseNextPriority = true;
//                    }
//                }
//                catch (Exception ex)
//                { UseNextPriority = true; }
//                return string.Empty;
//            }
//            else if ((ctrl.Trim() == "ControlType.CheckBox"))
//            {
//                try
//                {
//                    White.Core.UIItems.CheckBox chk = window.Get<White.Core.UIItems.CheckBox>(SearchCriteria.ByText(strName));
//                    window.WaitWhileBusy();
//                    if (Activate)
//                    {
//                        try
//                        {
//                            window.WaitWhileBusy();
//                            chk.Focus();
//                            window.WaitWhileBusy();
//                            Logger.Log.Logger.LogData("CheckBox control activated:" + strName, LogLevel.Info);
//                        }
//                        catch (Exception ex)
//                        {
//                            Logger.Log.Logger.LogData("Error while setting focus on CheckBox by Name: " + strName, LogLevel.Warning);
//                        }
//                        Logger.Log.Logger.LogData("Control activated:" + strName, LogLevel.Info);

//                    }
//                    if (ClickBeforeValueSet)
//                    {
//                        try
//                        {
//                            window.WaitWhileBusy();
//                            chk.RaiseClickEvent();
//                            window.WaitWhileBusy();
//                        }
//                        catch (Exception ex)
//                        {
//                            Logger.Log.Logger.LogData("Error while click before value set on CheckBox by Id " + strName, LogLevel.Warning);
//                        }
//                    }
//                    if (!string.IsNullOrEmpty(setValue))
//                    {
//                        if ((setValue.ToLower().Trim() == "true") || (setValue.ToLower().Trim() == "select") || (setValue.ToLower().Trim() == "1"))
//                        {
//                            chk.Checked = true;
//                        }
//                        else
//                        {
//                            chk.Checked = false;
//                        }
//                    }
//                    if (IsEventField)
//                    {
//                        chk.RaiseClickEvent();
//                        Logger.Log.Logger.LogData("CheckBox control clicked:" + strName, LogLevel.Info);
//                    }
//                    window.WaitWhileBusy();
//                    return chk.Checked.ToString();
//                }
//                catch (Exception ex)
//                { UseNextPriority = true; }
//                return string.Empty;
//            }
//            else if (ctrl.Trim() == "ControlType.RadioButton")
//            {
//                try
//                {
//                    White.Core.UIItems.RadioButton radioButton = window.Get<White.Core.UIItems.RadioButton>(SearchCriteria.ByText(strName));
//                    window.WaitWhileBusy();
//                    if (Activate)
//                    {
//                        try
//                        {
//                            radioButton.Focus();
//                            Logger.Log.Logger.LogData("RadioButton control activated:" + strName, LogLevel.Info);
//                        }
//                        catch (Exception ex)
//                        {
//                            Logger.Log.Logger.LogData("Error while setting focus on RadioButton by Name: " + strName, LogLevel.Warning);
//                        }
//                        Logger.Log.Logger.LogData("Control activated:" + strName, LogLevel.Info);

//                    }
//                    if (ClickBeforeValueSet)
//                    {
//                        try
//                        {
//                            radioButton.RaiseClickEvent();
//                        }
//                        catch (Exception ex)
//                        {
//                            Logger.Log.Logger.LogData("Error while click before value set on RadioButton by Name: " + strName, LogLevel.Warning);
//                        }
//                    }
//                    if (!string.IsNullOrEmpty(setValue))
//                    {
//                        if ((setValue.ToLower().Trim() == "true") || (setValue.ToLower().Trim() == "select") || (setValue.ToLower().Trim() == "1"))
//                        {
//                            radioButton.Select();
//                        }
//                        else
//                        {
//                            radioButton.Select();
//                        }
//                    }
//                    if (IsEventField)
//                    {
//                        radioButton.RaiseClickEvent();
//                        Logger.Log.Logger.LogData("RadioButton control clicked:" + strName, LogLevel.Info);
//                    }
//                    window.WaitWhileBusy();
//                    return radioButton.IsSelected.ToString();
//                }
//                catch (Exception ex)
//                { UseNextPriority = true; }
//                return string.Empty;

//            }
//            else if (ctrl.Trim() == "ControlType.ComboBox")
//            {
//                try
//                {
//                    White.Core.UIItems.ListBoxItems.ComboBox comboBox = window.Get<White.Core.UIItems.ListBoxItems.ComboBox>(SearchCriteria.ByText(strName));
//                    window.WaitWhileBusy();
//                    if (Activate)
//                    {
//                        try
//                        {
//                            comboBox.Focus();
//                            Logger.Log.Logger.LogData("Combobox Control activated:" + strName, LogLevel.Info);
//                        }
//                        catch (Exception ex)
//                        {
//                            Logger.Log.Logger.LogData("Error while setting focus on ComboBox by Name:" + strName, LogLevel.Warning);
//                        }
//                    }
//                    if (ClickBeforeValueSet)
//                    {
//                        try
//                        {
//                            comboBox.RaiseClickEvent();
//                        }
//                        catch (Exception ex)
//                        {
//                            Logger.Log.Logger.LogData("Error while Click before combobox value set on Name: " + strName, LogLevel.Warning);
//                        }
//                    }
//                    if (!string.IsNullOrEmpty(setValue))
//                    {
//                        comboBox.Select(setValue);
//                        Logger.Log.Logger.LogData("combobox value selected:" + strName, LogLevel.Info);
//                    }
//                    if (IsEventField)
//                    {
//                        comboBox.RaiseClickEvent();
//                        Logger.Log.Logger.LogData("combobox control clicked:" + strName, LogLevel.Info);
//                    }
//                    window.WaitWhileBusy();
//                    return comboBox.SelectedItemText;
//                }
//                catch (Exception ex)
//                {
//                    Logger.Log.Logger.LogData("Error in ComboBox set value by Name:" + strName, LogLevel.Warning);
//                    UseNextPriority = true;
//                }
//                return string.Empty;

//            }
//            else if ((ctrl.Trim() == " ControlType.Table"))
//            {
//                try
//                {
//                    White.Core.UIItems.TableItems.Table table = window.Get<White.Core.UIItems.TableItems.Table>(SearchCriteria.ByText(strName));

//                    window.WaitWhileBusy();
//                    if (Activate)
//                    {
//                        try
//                        {
//                            table.Focus();
//                            Logger.Log.Logger.LogData("Table control activated:" + strName, LogLevel.Info);
//                        }
//                        catch (Exception ex)
//                        {
//                            Logger.Log.Logger.LogData("Error while setting focus on Table by Name: " + strName, LogLevel.Warning);
//                        }
//                    }
//                    if (ClickBeforeValueSet)
//                    {
//                        try
//                        {
//                            table.RaiseClickEvent();
//                        }
//                        catch (Exception ex)
//                        {
//                            Logger.Log.Logger.LogData("Error while Click before Table value set on  by Name: " + strName, LogLevel.Warning);
//                        }
//                    }
//                    if (!string.IsNullOrEmpty(setValue))
//                    {
//                        table.SetValue(setValue); //Need to send table object
//                        Logger.Log.Logger.LogData("Table value selected:" + strName, LogLevel.Info);
//                    }
//                    if (IsEventField)
//                    {
//                        table.RaiseClickEvent();
//                        Logger.Log.Logger.LogData("Table control clicked:" + strName, LogLevel.Info);
//                    }
//                    window.WaitWhileBusy();
//                    return table;
//                }
//                catch (Exception ex)
//                {
//                    Logger.Log.Logger.LogData("Error in table set value by Name:" + strName, LogLevel.Warning);
//                    UseNextPriority = true;
//                }
//                return string.Empty;
//            }

//            else if ((ctrl.Trim() == "ControlType.Image"))
//            {
//                try
//                {
//                    White.Core.UIItems.Image image = window.Get<White.Core.UIItems.Image>(SearchCriteria.ByText(strName));
//                    window.WaitWhileBusy();
//                    if (Activate)
//                    {
//                        try
//                        {
//                            image.Focus();
//                            Logger.Log.Logger.LogData("Image Control activated:" + strName, LogLevel.Info);
//                        }
//                        catch (Exception ex)
//                        {
//                            Logger.Log.Logger.LogData("Error while setting focus on image:" + strName, LogLevel.Warning);
//                        }
//                    }
//                    if (ClickBeforeValueSet)
//                    {
//                        try
//                        {
//                            image.RaiseClickEvent();
//                        }
//                        catch (Exception ex)
//                        {
//                            Logger.Log.Logger.LogData("Error while Click Before image Value set on Name: " + strName, LogLevel.Warning);
//                        }
//                    }
//                    if (!string.IsNullOrEmpty(setValue))
//                    {
//                        image.SetValue(setValue); //Need to send table object
//                        Logger.Log.Logger.LogData("Image value selected:" + strName, LogLevel.Info);
//                    }
//                    if (IsEventField)
//                    {
//                        image.RaiseClickEvent();
//                        Logger.Log.Logger.LogData("Image control clicked:" + strName, LogLevel.Info);
//                    }
//                    window.WaitWhileBusy();
//                    return image;
//                }
//                catch (Exception ex)
//                {
//                    Logger.Log.Logger.LogData("Error in image set value by Name:" + strName, LogLevel.Warning);
//                    UseNextPriority = true;
//                }
//                return string.Empty;
//            }
//            else if (ctrl.Trim() == "ControlType.ListItem")
//            {
//                try
//                {
//                    White.Core.UIItems.ListBoxItems.ListItem listItem = window.Get<White.Core.UIItems.ListBoxItems.ListItem>(SearchCriteria.ByText(strName));
//                    window.WaitWhileBusy();
//                    if (Activate)
//                    {
//                        try
//                        {
//                            listItem.Focus();
//                            Logger.Log.Logger.LogData("ListItem Control activated:" + strName, LogLevel.Info);
//                        }
//                        catch (Exception ex)
//                        {
//                            Logger.Log.Logger.LogData("Error while setting focus on ListItem:" + strName, LogLevel.Warning);
//                        }
//                    }
//                    if (ClickBeforeValueSet)
//                    {
//                        try
//                        {
//                            listItem.RaiseClickEvent();
//                        }
//                        catch (Exception ex)
//                        {
//                            Logger.Log.Logger.LogData("Error while click before ListItem value set on Name: " + strName, LogLevel.Warning);
//                        }
//                    }
//                    if (!string.IsNullOrEmpty(setValue))
//                    {
//                        listItem.SetValue(setValue); //Need to send table object
//                        Logger.Log.Logger.LogData("ListItem value selected:" + strName, LogLevel.Info);
//                    }
//                    if (IsEventField)
//                    {
//                        listItem.RaiseClickEvent();
//                        Logger.Log.Logger.LogData("ListItem control clicked:" + strName, LogLevel.Info);
//                    }
//                    window.WaitWhileBusy();
//                    return listItem.Text;
//                }
//                catch (Exception ex)
//                { UseNextPriority = true; }
//                return string.Empty;

//            }

//            else if ((ctrl.Trim() == "ControlType.List"))
//            {
//                try
//                {
//                    White.Core.UIItems.ListBoxItems.ListBox comboBox = window.Get<White.Core.UIItems.ListBoxItems.ListBox>(SearchCriteria.ByText(strName));
//                    window.WaitWhileBusy();
//                    if (Activate)
//                    {
//                        try
//                        {
//                            comboBox.Focus();
//                            Logger.Log.Logger.LogData("List control activated:" + strName, LogLevel.Info);
//                        }
//                        catch (Exception ex)
//                        {
//                            Logger.Log.Logger.LogData("Error while setting focus on List by Name:" + strName, LogLevel.Warning);
//                        }
//                    }
//                    if (ClickBeforeValueSet)
//                    {
//                        try
//                        {
//                            comboBox.RaiseClickEvent();
//                        }
//                        catch (Exception ex)
//                        {
//                            Logger.Log.Logger.LogData("Error while Click before List value set on Id:  " + strName, LogLevel.Warning);
//                        }
//                    }
//                    if (!string.IsNullOrEmpty(setValue))
//                    {
//                        comboBox.SetValue(setValue);
//                        Logger.Log.Logger.LogData("List value selected:" + strName, LogLevel.Info);
//                    }
//                    if (IsEventField)
//                    {
//                        comboBox.RaiseClickEvent();
//                        Logger.Log.Logger.LogData("List control clicked:" + strName, LogLevel.Info);
//                    }
//                    window.WaitWhileBusy();
//                    return comboBox.SelectedItemText;
//                }
//                catch (Exception ex)
//                { UseNextPriority = true; }
//                return string.Empty;
//            }
//            else if (ctrl.Trim() == "ControlType.Menu")
//            {
//                try
//                {
//                    White.Core.UIItems.MenuItems.Menu menuItem = window.Get<White.Core.UIItems.MenuItems.Menu>(SearchCriteria.ByText(strName));

//                    window.WaitWhileBusy();
//                    if (Activate)
//                    {
//                        try
//                        {
//                            menuItem.Focus();
//                            Logger.Log.Logger.LogData("List control activated:" + strName, LogLevel.Info);
//                        }
//                        catch (Exception ex)
//                        {
//                            Logger.Log.Logger.LogData("Error while setting focus on Menu by Name: " + strName, LogLevel.Warning);
//                        }
//                    }
//                    if (ClickBeforeValueSet)
//                    {
//                        try
//                        {
//                            menuItem.RaiseClickEvent();
//                        }
//                        catch (Exception ex)
//                        {
//                            Logger.Log.Logger.LogData("Error while click before Menu value set on Name: " + strName, LogLevel.Warning);
//                        }
//                    }
//                    if (!string.IsNullOrEmpty(setValue))
//                    {
//                        menuItem.SetValue(setValue);
//                        Logger.Log.Logger.LogData("Menu value selected:" + strName, LogLevel.Info);
//                    }
//                    if (IsEventField)
//                    {
//                        menuItem.RaiseClickEvent();
//                        Logger.Log.Logger.LogData("Menu control clicked:" + strName, LogLevel.Info);
//                    }
//                    window.WaitWhileBusy();
//                    return menuItem.ToString();
//                }
//                catch (Exception ex)
//                { UseNextPriority = true; }
//                return string.Empty;

//            }

//            else if ((ctrl.Trim() == "ControlType.MenuBar"))
//            {
//                try
//                {
//                    White.Core.UIItems.WindowStripControls.MenuBar menuBar = window.Get<White.Core.UIItems.WindowStripControls.MenuBar>(SearchCriteria.ByText(strName));

//                    window.WaitWhileBusy();
//                    if (Activate)
//                    {
//                        try
//                        {
//                            menuBar.Focus();
//                            Logger.Log.Logger.LogData("MenuBar Control activated:" + strName, LogLevel.Info);
//                        }
//                        catch (Exception ex)
//                        {
//                            Logger.Log.Logger.LogData("Error while setting focus on MenuBar by Name: " + strName, LogLevel.Warning);
//                        }
//                    }
//                    if (ClickBeforeValueSet)
//                    {
//                        try
//                        {
//                            menuBar.RaiseClickEvent();
//                        }
//                        catch (Exception ex)
//                        {
//                            Logger.Log.Logger.LogData("Error while Click before MenuBar value set on Name: " + strName, LogLevel.Warning);
//                        }
//                    }
//                    if (!string.IsNullOrEmpty(setValue))
//                    {
//                        menuBar.SetValue(setValue);
//                        Logger.Log.Logger.LogData("MenuBar value selected:" + strName, LogLevel.Info);
//                    }
//                    if (IsEventField)
//                    {
//                        menuBar.RaiseClickEvent();
//                        Logger.Log.Logger.LogData("MenuBar control clicked:" + strName, LogLevel.Info);
//                    }
//                    window.WaitWhileBusy();
//                    return menuBar.ToString();
//                }
//                catch (Exception ex)
//                { UseNextPriority = true; }
//                return string.Empty;
//            }
//            else if (ctrl.Trim() == "ControlType.MenuItem")
//            {
//                try
//                {
//                    White.Core.UIItems.MenuItems.Menu menuItem = window.Get<White.Core.UIItems.MenuItems.Menu>(SearchCriteria.ByText(strName));

//                    window.WaitWhileBusy();
//                    if (Activate)
//                    {
//                        try
//                        {
//                            menuItem.Focus();
//                            Logger.Log.Logger.LogData("MenuItem control activated:" + strName, LogLevel.Info);
//                        }
//                        catch (Exception ex)
//                        {
//                            Logger.Log.Logger.LogData("Error while setting focus on MenuItem by Name:" + strName, LogLevel.Warning);
//                        }
//                    }
//                    if (ClickBeforeValueSet)
//                    {
//                        try
//                        {
//                            menuItem.RaiseClickEvent();
//                        }
//                        catch (Exception ex)
//                        {
//                            Logger.Log.Logger.LogData("Error while Click before MenuItem value set on Name: " + strName, LogLevel.Warning);
//                        }
//                    }
//                    if (!string.IsNullOrEmpty(setValue))
//                    {
//                        menuItem.SetValue(setValue);
//                        Logger.Log.Logger.LogData("MenuItem value selected:" + strName, LogLevel.Info);
//                    }
//                    if (IsEventField)
//                    {
//                        menuItem.RaiseClickEvent();
//                        Logger.Log.Logger.LogData("MenuItem control clicked:" + strName, LogLevel.Info);
//                    }
//                    window.WaitWhileBusy();
//                    return menuItem.ToString();
//                }
//                catch (Exception ex)
//                { UseNextPriority = true; }
//                return string.Empty;

//            }
//            else if ((ctrl.Trim() == "ControlType.Calendar") || (ctrl.Trim() == "ControlType.Pane"))
//            {
//                try
//                {
//                    White.Core.UIItems.DateTimePicker sDate = window.Get<White.Core.UIItems.DateTimePicker>(SearchCriteria.ByText(strName));
//                    window.WaitWhileBusy();
//                    if (sDate != null)
//                    {
//                        if (Activate)
//                        {
//                            try
//                            {
//                                sDate.Focus();
//                                Logger.Log.Logger.LogData("Calendar control activated:" + strName, LogLevel.Info);
//                            }
//                            catch (Exception ex)
//                            {
//                                Logger.Log.Logger.LogData("Error while setting focus on Calendar by Name:" + strName, LogLevel.Warning);
//                            }
//                        }
//                        if (ClickBeforeValueSet)
//                        {
//                            try
//                            {
//                                sDate.RaiseClickEvent();
//                            }
//                            catch (Exception ex)
//                            {
//                                Logger.Log.Logger.LogData("Error while click before Calendar value set on Name: " + strName, LogLevel.Warning);
//                            }
//                        }
//                        if (!string.IsNullOrEmpty(setValue))
//                        {
//                            System.DateTime dateTime = Convert.ToDateTime(setValue);
//                            sDate.SetDate(dateTime, DateFormat.MonthDayYear);
//                            Logger.Log.Logger.LogData("Calendar value selected:" + strName, LogLevel.Info);
//                        }
//                        if (IsEventField)
//                        {
//                            sDate.RaiseClickEvent();
//                            Logger.Log.Logger.LogData("Calendar control clicked:" + strName, LogLevel.Info);
//                        }
//                        window.WaitWhileBusy();
//                        return sDate.Date.ToLongDateString();
//                    }
//                    else
//                    {
//                        White.Core.UIItems.PropertyGridItems.PropertyGrid propertyGrid = window.Get<White.Core.UIItems.PropertyGridItems.PropertyGrid>(SearchCriteria.ByText(strName));
//                        window.WaitWhileBusy();
//                        if (propertyGrid != null)
//                        {

//                            if (Activate)
//                            {
//                                try
//                                {
//                                    propertyGrid.RaiseClickEvent();
//                                    Logger.Log.Logger.LogData("PropertyGrid control activated:" + strName, LogLevel.Info);
//                                }
//                                catch (Exception ex)
//                                {
//                                    Logger.Log.Logger.LogData("Error while setting focus on PropertyGrid by Name: " + strName, LogLevel.Warning);
//                                }
//                            }
//                            if (ClickBeforeValueSet)
//                            {
//                                try
//                                {
//                                    propertyGrid.RaiseClickEvent();
//                                }
//                                catch (Exception ex)
//                                {
//                                    Logger.Log.Logger.LogData("Error while Click before PropertyGrid value set on Name: " + strName, LogLevel.Warning);
//                                }
//                            }
//                            if (!string.IsNullOrEmpty(setValue))
//                            {
//                                try
//                                {
//                                    propertyGrid.SetValue(setValue);
//                                    Logger.Log.Logger.LogData("PropertyGrid value selected:" + strName, LogLevel.Info);
//                                }
//                                catch (Exception ex)
//                                {
//                                    Logger.Log.Logger.LogData("Error in propertyGrid set value by Name:" + strName, LogLevel.Info);
//                                }
//                            }
//                            if (IsEventField)
//                            {
//                                propertyGrid.RaiseClickEvent();
//                                Logger.Log.Logger.LogData("PropertyGrid control clicked:" + strName, LogLevel.Info);
//                            }
//                            window.WaitWhileBusy();
//                            return propertyGrid;
//                        }
//                    }
//                    window.WaitWhileBusy();
//                }
//                catch (Exception ex)
//                { UseNextPriority = true; }
//                return string.Empty;

//            }

//            else if (ctrl.Trim() == "ControlType.Hyperlink")
//            {
//                try
//                {
//                    White.Core.UIItems.Hyperlink hlink = window.Get<White.Core.UIItems.Hyperlink>(SearchCriteria.ByText(strName));
//                    window.WaitWhileBusy();
//                    if (Activate)
//                    {
//                        try
//                        {
//                            hlink.RaiseClickEvent();
//                            Logger.Log.Logger.LogData("Hyperlink Control activated:" + strName, LogLevel.Info);
//                        }
//                        catch (Exception ex)
//                        {
//                            Logger.Log.Logger.LogData("Error while setting focus on Hyperlink by Name:" + strName, LogLevel.Warning);
//                        }
//                    }
//                    if (ClickBeforeValueSet)
//                    {
//                        try
//                        {
//                            hlink.RaiseClickEvent();
//                        }
//                        catch (Exception ex)
//                        {
//                            Logger.Log.Logger.LogData("Error while Click before Hyperlink value set on Name: " + strName, LogLevel.Warning);
//                        }
//                    }
//                    if (!string.IsNullOrEmpty(setValue))
//                    {
//                        try
//                        {
//                            hlink.SetValue(setValue);
//                            Logger.Log.Logger.LogData("Hyperlink value selected:" + strName, LogLevel.Info);
//                        }
//                        catch (Exception ex)
//                        {
//                            Logger.Log.Logger.LogData("Error in Hyperlink set value by Name:" + strName, LogLevel.Info);
//                        }
//                    }
//                    if (IsEventField)
//                    {
//                        hlink.RaiseClickEvent();
//                        Logger.Log.Logger.LogData("Hyperlink control clicked:" + strName, LogLevel.Info);
//                    }
//                    window.WaitWhileBusy();
//                    return hlink.ToString();
//                }
//                catch (Exception ex)
//                { UseNextPriority = true; }
//                return string.Empty;

//            }

//            else if ((ctrl.Trim() == " ControlType.Tab"))
//            {
//                try
//                {
//                    White.Core.UIItems.TabItems.Tab tab = window.Get<White.Core.UIItems.TabItems.Tab>(SearchCriteria.ByText(strName));
//                    window.WaitWhileBusy();
//                    if (Activate)
//                    {
//                        try
//                        {
//                            tab.RaiseClickEvent();
//                            Logger.Log.Logger.LogData("Tab control activated:" + strName, LogLevel.Info);
//                        }
//                        catch (Exception ex)
//                        {
//                            Logger.Log.Logger.LogData("Error while setting focus on Tab by Name: " + strName, LogLevel.Warning);
//                        }
//                    }
//                    if (ClickBeforeValueSet)
//                    {
//                        try
//                        {
//                            tab.RaiseClickEvent();
//                        }
//                        catch (Exception ex)
//                        {
//                            Logger.Log.Logger.LogData("Error while Click before Tab value set on Name: " + strName, LogLevel.Warning);
//                        }
//                    }
//                    if (!string.IsNullOrEmpty(setValue))
//                    {
//                        try
//                        {
//                            tab.SetValue(setValue);
//                            Logger.Log.Logger.LogData("Tab value selected:" + strName, LogLevel.Info);
//                        }
//                        catch (Exception ex)
//                        {
//                            Logger.Log.Logger.LogData("Error in Tab set value by Name:" + strName, LogLevel.Info);
//                        }
//                    }
//                    if (IsEventField)
//                    {
//                        tab.RaiseClickEvent();
//                        Logger.Log.Logger.LogData("Tab control clicked:" + strName, LogLevel.Info);
//                    }
//                    window.WaitWhileBusy();
//                    return tab.ToString();
//                }
//                catch (Exception ex)
//                { UseNextPriority = true; }
//                return string.Empty;
//            }
//            else if (ctrl.Trim() == "ControlType.TabItem")
//            {
//                try
//                {
//                    White.Core.UIItems.TabItems.TabPage tabPage = window.Get<White.Core.UIItems.TabItems.TabPage>(SearchCriteria.ByText(strName));
//                    window.WaitWhileBusy();
//                    if (Activate)
//                    {
//                        try
//                        {
//                            tabPage.RaiseClickEvent();
//                            Logger.Log.Logger.LogData("TabItem control activated:" + strName, LogLevel.Info);
//                        }
//                        catch (Exception ex)
//                        {
//                            Logger.Log.Logger.LogData("Error while setting focus on TabItem by Name: " + strName, LogLevel.Warning);
//                        }
//                    }
//                    if (ClickBeforeValueSet)
//                    {
//                        try
//                        {
//                            tabPage.RaiseClickEvent();
//                        }
//                        catch (Exception ex)
//                        {
//                            Logger.Log.Logger.LogData("Error while click before TabItem value set on Name: " + strName, LogLevel.Warning);
//                        }
//                    }
//                    if (!string.IsNullOrEmpty(setValue))
//                    {
//                        try
//                        {
//                            tabPage.SetValue(setValue);
//                            Logger.Log.Logger.LogData("TabItem value selected:" + strName, LogLevel.Info);
//                        }
//                        catch (Exception ex)
//                        {
//                            Logger.Log.Logger.LogData("Error in TabItem set value by Name:" + strName, LogLevel.Info);
//                        }
//                    }
//                    if (IsEventField)
//                    {
//                        tabPage.RaiseClickEvent();
//                        Logger.Log.Logger.LogData("TabItem control clicked:" + strName, LogLevel.Info);
//                    }
//                    window.WaitWhileBusy();
//                    return tabPage.ToString();
//                }
//                catch (Exception ex)
//                { UseNextPriority = true; }
//                return string.Empty;

//            }
//            else if ((ctrl.Trim() == "ControlType.DataGrid"))
//            {
//                try
//                {
//                    White.Core.UIItems.ListView dataGrid = window.Get<White.Core.UIItems.ListView>(SearchCriteria.ByText(strName));

//                    window.WaitWhileBusy();
//                    if (Activate)
//                    {
//                        try
//                        {
//                            dataGrid.RaiseClickEvent();
//                            Logger.Log.Logger.LogData("DataGrid control activated:" + strName, LogLevel.Info);
//                        }
//                        catch (Exception ex)
//                        {
//                            Logger.Log.Logger.LogData("Error while setting focus on DataGrid by Name: " + strName, LogLevel.Warning);
//                        }
//                    }
//                    if (ClickBeforeValueSet)
//                    {
//                        try
//                        {
//                            dataGrid.RaiseClickEvent();
//                        }
//                        catch (Exception ex)
//                        {
//                            Logger.Log.Logger.LogData("Error while Click before List value set on Name: " + strName, LogLevel.Warning);
//                        }
//                    }
//                    if (!string.IsNullOrEmpty(setValue))
//                    {
//                        try
//                        {
//                            dataGrid.SetValue(setValue);
//                            Logger.Log.Logger.LogData("List value selected:" + strName, LogLevel.Info);
//                        }
//                        catch (Exception ex)
//                        {
//                            Logger.Log.Logger.LogData("Error in propertyGrid set value by Name:" + strName, LogLevel.Info);
//                        }
//                    }
//                    if (IsEventField)
//                    {
//                        dataGrid.RaiseClickEvent();
//                        Logger.Log.Logger.LogData("List control clicked:" + strName, LogLevel.Info);
//                    }
//                    window.WaitWhileBusy();
//                    return dataGrid;
//                }
//                catch (Exception ex)
//                { UseNextPriority = true; }
//                return string.Empty;
//            }
//            else if (ctrl.Trim() == "ControlType.DataItem")
//            {
//                try
//                {
//                    White.Core.UIItems.ListViewRow dataGridrow = window.Get<White.Core.UIItems.ListViewRow>(SearchCriteria.ByText(strName));

//                    window.WaitWhileBusy();
//                    if (Activate)
//                    {
//                        try
//                        {
//                            dataGridrow.RaiseClickEvent();
//                            Logger.Log.Logger.LogData("List control activated:" + strName, LogLevel.Info);
//                        }
//                        catch (Exception ex)
//                        {
//                            Logger.Log.Logger.LogData("Error while setting focus on List by Name:" + strName, LogLevel.Warning);
//                        }
//                    }
//                    if (ClickBeforeValueSet)
//                    {
//                        try
//                        {
//                            dataGridrow.RaiseClickEvent();
//                        }
//                        catch (Exception ex)
//                        {
//                            Logger.Log.Logger.LogData("Error while Click before List value set on Name: " + strName, LogLevel.Warning);
//                        }
//                    }
//                    if (!string.IsNullOrEmpty(setValue))
//                    {
//                        try
//                        {
//                            dataGridrow.SetValue(setValue);
//                            Logger.Log.Logger.LogData("List value selected:" + strName, LogLevel.Info);
//                        }
//                        catch (Exception ex)
//                        {
//                            Logger.Log.Logger.LogData("Error in propertyGrid set value by Name:" + strName, LogLevel.Info);
//                        }
//                    }
//                    if (IsEventField)
//                    {
//                        dataGridrow.RaiseClickEvent();
//                        Logger.Log.Logger.LogData("List control clicked:" + strName, LogLevel.Info);
//                    }
//                    window.WaitWhileBusy();
//                    return dataGridrow;
//                }
//                catch (Exception ex)
//                { UseNextPriority = true; }
//                return string.Empty;

//            }
//            else if ((ctrl.Trim() == " ControlType.TreeItem") || (ctrl.Trim() == "ControlType.Tree"))
//            {
//                try
//                {
//                    White.Core.UIItems.TreeItems.TreeNode nodeOne = window.Get<White.Core.UIItems.TreeItems.TreeNode>(SearchCriteria.ByText(strName));
//                    //nodeOne.Select();
//                    //nodeOne.Expand();
//                    window.WaitWhileBusy();
//                    if (Activate)
//                    {
//                        try
//                        {
//                            nodeOne.RaiseClickEvent();
//                            Logger.Log.Logger.LogData("TreeNode control activated:" + strName, LogLevel.Info);
//                        }
//                        catch (Exception ex)
//                        {
//                            Logger.Log.Logger.LogData("Error while setting focus on TreeNode by Name: " + strName, LogLevel.Warning);
//                        }
//                    }
//                    if (ClickBeforeValueSet)
//                    {
//                        try
//                        {
//                            nodeOne.RaiseClickEvent();
//                        }
//                        catch (Exception ex)
//                        {
//                            Logger.Log.Logger.LogData("Error while Click before TreeNode value set on Name: " + strName, LogLevel.Warning);
//                        }
//                    }
//                    if (!string.IsNullOrEmpty(setValue))
//                    {
//                        try
//                        {
//                            nodeOne.Select();
//                            Logger.Log.Logger.LogData("TreeNode value selected:" + strName, LogLevel.Info);
//                        }
//                        catch (Exception ex)
//                        {
//                            Logger.Log.Logger.LogData("Error in TreeNode set value by Name:" + strName, LogLevel.Info);
//                        }
//                    }
//                    if (IsEventField)
//                    {
//                        nodeOne.RaiseClickEvent();
//                        Logger.Log.Logger.LogData("TreeNode control clicked:" + strName, LogLevel.Info);
//                    }
//                    window.WaitWhileBusy();
//                    return nodeOne.Text;
//                }
//                catch (Exception ex)
//                { UseNextPriority = true; }
//                return string.Empty;
//            }
//            else if ((ctrl.Trim() == " ControlType.ProgressBar"))
//            {
//                try
//                {
//                    White.Core.UIItems.ProgressBar progressBar = window.Get<White.Core.UIItems.ProgressBar>(SearchCriteria.ByText(strName));
//                    progressBar.RaiseClickEvent();
//                    do
//                    {
//                        Thread.Sleep(5000);
//                    } while (progressBar.Value != progressBar.Maximum);

//                }
//                catch (Exception ex)
//                { UseNextPriority = true; }
//                return string.Empty;
//            }

//            else if (ctrl.Trim() == "ControlType.Slider")
//            {
//                try
//                {
//                    White.Core.UIItems.Slider sldrOne = window.Get<White.Core.UIItems.Slider>(SearchCriteria.ByText(strName));

//                    sldrOne.LargeIncrementButton.RaiseClickEvent();
//                    //   sldrOne.LargeDecrementButton.RaiseClickEvent();
//                }
//                catch (Exception ex)
//                { UseNextPriority = true; }
//                return string.Empty;

//            }

//            else if ((ctrl.Trim() == "ControlType.Spinner"))
//            {
//                try
//                {
//                    White.Core.UIItems.Spinner spinner = window.Get<White.Core.UIItems.Spinner>(SearchCriteria.ByText(strName));
//                    window.WaitWhileBusy();
//                    if (Activate)
//                    {
//                        try
//                        {
//                            spinner.RaiseClickEvent();
//                            Logger.Log.Logger.LogData("Spinner control activated:" + strName, LogLevel.Info);
//                        }
//                        catch (Exception ex)
//                        {
//                            Logger.Log.Logger.LogData("Error while setting focus on Spinner by Name: " + strName, LogLevel.Warning);
//                        }
//                    }
//                    if (ClickBeforeValueSet)
//                    {
//                        try
//                        {
//                            spinner.RaiseClickEvent();
//                        }
//                        catch (Exception ex)
//                        {
//                            Logger.Log.Logger.LogData("Error while Click before Spinner value set on Name: " + strName, LogLevel.Warning);
//                        }
//                    }
//                    if (!string.IsNullOrEmpty(setValue))
//                    {
//                        try
//                        {
//                            spinner.SetValue(setValue);
//                            Logger.Log.Logger.LogData("Spinner value selected:" + strName, LogLevel.Info);
//                        }
//                        catch (Exception ex)
//                        {
//                            Logger.Log.Logger.LogData("Error in Spinner set value by Name:" + strName, LogLevel.Info);
//                        }
//                    }
//                    if (IsEventField)
//                    {
//                        spinner.RaiseClickEvent();
//                        Logger.Log.Logger.LogData("Spinner control clicked:" + strName, LogLevel.Info);
//                    }
//                    window.WaitWhileBusy();
//                    return spinner.Value.ToString();
//                }
//                catch (Exception ex)
//                { UseNextPriority = true; }
//                return string.Empty;
//            }

//            else if (ctrl.Trim() == "ControlType.ToolBar")
//            {
//                try
//                {
//                    White.Core.UIItems.WindowStripControls.ToolStrip toolStrip = window.Get<White.Core.UIItems.WindowStripControls.ToolStrip>(SearchCriteria.ByText(strName));
//                    window.WaitWhileBusy();
//                    if (Activate)
//                    {
//                        try
//                        {
//                            toolStrip.RaiseClickEvent();
//                            Logger.Log.Logger.LogData("ToolBar control activated:" + strName, LogLevel.Info);
//                        }
//                        catch (Exception ex)
//                        {
//                            Logger.Log.Logger.LogData("Error while setting focus on ToolBar by Name: " + strName, LogLevel.Warning);
//                        }
//                    }
//                    if (ClickBeforeValueSet)
//                    {
//                        try
//                        {
//                            toolStrip.RaiseClickEvent();
//                        }
//                        catch (Exception ex)
//                        {
//                            Logger.Log.Logger.LogData("Error while Click before ToolBar value set on Name: " + strName, LogLevel.Warning);
//                        }
//                    }
//                    if (!string.IsNullOrEmpty(setValue))
//                    {
//                        try
//                        {
//                            toolStrip.SetValue(setValue);
//                            Logger.Log.Logger.LogData("ToolBar value selected:" + strName, LogLevel.Info);
//                        }
//                        catch (Exception ex)
//                        {
//                            Logger.Log.Logger.LogData("Error in ToolBar set value by Name:" + strName, LogLevel.Info);
//                        }
//                    }
//                    if (IsEventField)
//                    {
//                        toolStrip.RaiseClickEvent();
//                        Logger.Log.Logger.LogData("ToolBar control clicked:" + strName, LogLevel.Info);
//                    }
//                    window.WaitWhileBusy();
//                    return toolStrip.ToString();
//                }
//                catch (Exception ex)
//                { UseNextPriority = true; }
//                return string.Empty;

//            }

//            else if ((ctrl.Trim() == "ControlType.ToolTip"))
//            {
//                try
//                {
//                    White.Core.UIItems.ToolTip toolTip = window.Get<White.Core.UIItems.ToolTip>(SearchCriteria.ByText(strName));
//                    window.WaitWhileBusy();
//                    if (Activate)
//                    {
//                        try
//                        {
//                            toolTip.RaiseClickEvent();
//                            Logger.Log.Logger.LogData("ToolTip control activated:" + strName, LogLevel.Info);
//                        }
//                        catch (Exception ex)
//                        {
//                            Logger.Log.Logger.LogData("Error while setting focus on ToolTip by Name:" + strName, LogLevel.Warning);
//                        }
//                    }
//                    if (ClickBeforeValueSet)
//                    {
//                        try
//                        {
//                            toolTip.RaiseClickEvent();
//                        }
//                        catch (Exception ex)
//                        {
//                            Logger.Log.Logger.LogData("Error while Click before ToolTip value set on Name: " + strName, LogLevel.Warning);
//                        }
//                    }
//                    if (!string.IsNullOrEmpty(setValue))
//                    {
//                        try
//                        {
//                            toolTip.SetValue(setValue);
//                            Logger.Log.Logger.LogData("ToolTip value selected:" + strName, LogLevel.Info);
//                        }
//                        catch (Exception ex)
//                        {
//                            Logger.Log.Logger.LogData("Error in ToolTip set value by Name:" + strName, LogLevel.Info);
//                        }
//                    }
//                    if (IsEventField)
//                    {
//                        toolTip.RaiseClickEvent();
//                        Logger.Log.Logger.LogData("ToolTip control clicked:" + strName, LogLevel.Info);
//                    }
//                    window.WaitWhileBusy();
//                    return toolTip.Text;

//                }
//                catch (Exception ex)
//                { UseNextPriority = true; }
//                return string.Empty;
//            }
//            else if ((ctrl.Trim() == "ControlType.Group"))
//            {
//                try
//                {
//                    White.Core.UIItems.GroupBox groupBox = window.Get<White.Core.UIItems.GroupBox>(SearchCriteria.ByText(strName));

//                    window.WaitWhileBusy();
//                    if (Activate)
//                    {
//                        try
//                        {
//                            groupBox.RaiseClickEvent();
//                            Logger.Log.Logger.LogData("Group control activated:" + strName, LogLevel.Info);
//                        }
//                        catch (Exception ex)
//                        {
//                            Logger.Log.Logger.LogData("Error while setting focus on Group by Name:" + strName, LogLevel.Warning);
//                        }
//                    }
//                    if (ClickBeforeValueSet)
//                    {
//                        try
//                        {
//                            groupBox.RaiseClickEvent();
//                        }
//                        catch (Exception ex)
//                        {
//                            Logger.Log.Logger.LogData("Error while Click before Group value set on Name: " + strName, LogLevel.Warning);
//                        }
//                    }
//                    if (!string.IsNullOrEmpty(setValue))
//                    {
//                        try
//                        {
//                            groupBox.SetValue(setValue);
//                            Logger.Log.Logger.LogData("Group value selected:" + strName, LogLevel.Info);
//                        }
//                        catch (Exception ex)
//                        {
//                            Logger.Log.Logger.LogData("Error in Group set value by Name:" + strName, LogLevel.Info);
//                        }
//                    }
//                    if (IsEventField)
//                    {
//                        groupBox.RaiseClickEvent();
//                        Logger.Log.Logger.LogData("Group control clicked:" + strName, LogLevel.Info);
//                    }
//                    window.WaitWhileBusy();
//                    return groupBox.ToString();
//                }
//                catch (Exception ex)
//                { UseNextPriority = true; }
//                return string.Empty;
//            }
//            else if (ctrl.Trim() == "ControlType.Thumb")
//            {
//                try
//                {
//                    White.Core.UIItems.Thumb thumb = window.Get<White.Core.UIItems.Thumb>(SearchCriteria.ByText(strName));

//                    window.WaitWhileBusy();
//                    if (Activate)
//                    {
//                        try
//                        {
//                            thumb.RaiseClickEvent();
//                            Logger.Log.Logger.LogData("Thumb control activated:" + strName, LogLevel.Info);
//                        }
//                        catch (Exception ex)
//                        {
//                            Logger.Log.Logger.LogData("Error while setting focus on Thumb by Name:" + strName, LogLevel.Warning);
//                        }
//                    }
//                    if (ClickBeforeValueSet)
//                    {
//                        try
//                        {
//                            thumb.RaiseClickEvent();
//                        }
//                        catch (Exception ex)
//                        {
//                            Logger.Log.Logger.LogData("Error while Click before Thumb value set on Name: " + strName, LogLevel.Warning);
//                        }
//                    }
//                    if (!string.IsNullOrEmpty(setValue))
//                    {
//                        try
//                        {
//                            thumb.SetValue(setValue);
//                            Logger.Log.Logger.LogData("Thumb value selected:" + strName, LogLevel.Info);
//                        }
//                        catch (Exception ex)
//                        {
//                            Logger.Log.Logger.LogData("Error in Thumb set value by Name:" + strName, LogLevel.Info);
//                        }
//                    }
//                    if (IsEventField)
//                    {
//                        thumb.RaiseClickEvent();
//                        Logger.Log.Logger.LogData("Thumb control clicked:" + strName, LogLevel.Info);
//                    }
//                    window.WaitWhileBusy();
//                    return thumb.ToString();
//                }
//                catch (Exception ex)
//                { UseNextPriority = true; }
//                return string.Empty;

//            }
//            else if (ctrl.Trim() == "ControlType.TitleBar")
//            {
//                try
//                {
//                    White.Core.UIItems.WindowItems.TitleBar titleBar = window.Get<White.Core.UIItems.WindowItems.TitleBar>(SearchCriteria.ByText(strName));

//                    window.WaitWhileBusy();
//                    if (Activate)
//                    {
//                        try
//                        {
//                            titleBar.RaiseClickEvent();
//                            Logger.Log.Logger.LogData("TitleBar control activated:" + strName, LogLevel.Info);
//                        }
//                        catch (Exception ex)
//                        {
//                            Logger.Log.Logger.LogData("Error while setting focus on TitleBar by Name:" + strName, LogLevel.Warning);
//                        }
//                    }
//                    if (ClickBeforeValueSet)
//                    {
//                        try
//                        {
//                            titleBar.RaiseClickEvent();
//                        }
//                        catch (Exception ex)
//                        {
//                            Logger.Log.Logger.LogData("Error while Click before TitleBar value set on Name: " + strName, LogLevel.Warning);
//                        }
//                    }
//                    if (!string.IsNullOrEmpty(setValue))
//                    {
//                        try
//                        {
//                            titleBar.SetValue(setValue);
//                            Logger.Log.Logger.LogData("TitleBar value selected:" + strName, LogLevel.Info);
//                        }
//                        catch (Exception ex)
//                        {
//                            Logger.Log.Logger.LogData("Error in TitleBar set value by Name:" + strName, LogLevel.Info);
//                        }
//                    }
//                    if (IsEventField)
//                    {
//                        titleBar.RaiseClickEvent();
//                        Logger.Log.Logger.LogData("TitleBar control clicked:" + strName, LogLevel.Info);
//                    }
//                    window.WaitWhileBusy();
//                    return titleBar.ToString();
//                }
//                catch (Exception ex)
//                { UseNextPriority = true; }
//                return string.Empty;

//            }

//            #region "Not Supported - do not delete - for future implementation"
//            //else if (ctrl.Trim() == "ControlType.StatusBar")
//            //{
//            //    try
//            //    {
//            //        //                    White.Core.UIItems.Thumb thumb = window.Get<White.Core.UIItems.Thumb>(SearchCriteria.ByText(strName));
//            //    }
//            //    catch (Exception ex)
//            //    { UseNextPriority = true; }
//            //    return string.Empty;

//            //}
//            //else if ((ctrl.Trim() == "ControlType.ScrollBar"))
//            //{
//            //    try
//            //    {
//            //        //White.Core.UIItems.ProgressBar progressBar = window.Get<ScrollBar>(SearchCriteria.ByText(strName));
//            //    }
//            //    catch (Exception ex)
//            //    { UseNextPriority = true; }
//            //    return string.Empty;
//            //}
//            //else if (ctrl.Trim() == "ControlType.SplitButton")
//            //{
//            //    try
//            //    {
//            //         White.Core.UIItems.ListViewRow dataGridrow = window.Get<SplitButton>(SearchCriteria.ByText(strName));
//            //    }
//            //    catch (Exception ex)
//            //    { UseNextPriority = true; }
//            //    return string.Empty;

//            //}
//            //else if ((ctrl.Trim() == " ControlType.Document"))
//            //{
//            //    try
//            //    {
//            //        //White.Core.UIItems.Document document = window.Get<Document>(SearchCriteria.ByText(strName));
//            //    }
//            //    catch (Exception ex)
//            //    { UseNextPriority = true; }
//            //    return string.Empty;
//            //}
//            //else if ((ctrl.Trim() == "ControlType.Window"))
//            //{
//            //    try
//            //    {

//            //    }
//            //    catch (Exception ex)
//            //    { UseNextPriority = true; }
//            //    return string.Empty;
//            //}
//            //else if ((ctrl.Trim() == "ControlType.Header"))
//            //{
//            //    try
//            //    {

//            //    }
//            //    catch (Exception ex)
//            //    { UseNextPriority = true; }
//            //    return string.Empty;
//            //}
//            //else if (ctrl.Trim() == "ControlType.HeaderItem")
//            //{
//            //    try
//            //    {
//            //       // White.Core.UIItems.WindowItems.TitleBar titleBar = window.Get<HeaderItem>(SearchCriteria.ByText(strName));
//            //    }
//            //    catch (Exception ex)
//            //    { UseNextPriority = true; }
//            //    return string.Empty;

//            //}
//            //else if (ctrl.Trim() == "ControlType.Custom")
//            //{
//            //    try
//            //    {
//            //        //White.Core.UIItems.Custom groupBox = window.Get<White.Core.UIItems.Custom>(SearchCriteria.ByText(strName));
//            //    }
//            //    catch (Exception ex)
//            //    { UseNextPriority = true; }
//            //    return string.Empty;

//            //}
//            //else if ((ctrl.Trim() == "ControlType.Separator"))
//            //{
//            //    try
//            //    {
//            //        //White.Core.UIItems.WindowItems.TitleBar titleBar = window.Get<Separator>(SearchCriteria.ByText(strName));
//            //        //titleBar.RaiseClickEvent();
//            //    }
//            //    catch (Exception ex)
//            //    { UseNextPriority = true; }
//            //    return string.Empty;
//            //}
//            #endregion

//            else
//            {
//                return null;
//            }
//        }

//        public static object TakeActionOnControlByClassName(White.Core.UIItems.WindowItems.Window window, string strClassName, string ctrl, bool IsEventField, bool Activate, bool ClickBeforeValueSet, bool ContinueOnError, string setValue, ref bool UseNextPriority)
//        {
//            return null;
//        }
//    }
//}
