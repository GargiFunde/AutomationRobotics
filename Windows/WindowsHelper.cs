using Logger;
using System;
using System.Activities;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Automation;
using TestStack.White.UIItems.WPFUIItems;
using TestStack.White;
using TestStack.White.UIItems;
using TestStack.White.UIItems.Custom;
using TestStack.White.UIItems.Finders;
using TestStack.White.UIItems.WindowItems;

namespace Bot.Activity.Windows
{
    [ControlTypeMapping(CustomUIItemType.Pane)]
    public class WindowsHelper : CustomUIItem
    {
        public static TestStack.White.UIItems.WindowItems.Window GetApplicationObject(string sWindowTitle, NativeActivityContext context)
        {
            Match m = null;
            try
            {
               List<TestStack.White.UIItems.WindowItems.Window> windows = Desktop.Instance.Windows();

                foreach (TestStack.White.UIItems.WindowItems.Window item in windows)
                {
                    if((item != null )&& (!string.IsNullOrEmpty(item.Title)))
                    {
                        if (string.Equals(item.Title, sWindowTitle))
                        {
                            return item;
                        }
                        else
                        {
                            try
                            {
                                //string pattern = string.Empty;
                                //pattern = string.Concat("^.*", sWindowTitle,".*$");
                                //Regex rgx = new Regex(pattern);
                                m = Regex.Match(item.Title, sWindowTitle);
                                //m = Regex.Match(item.Title, pattern);
                            }
                            catch (Exception) { }
                        }
                    }
                    if (m != null && m.Success)
                    {
                        return item;
                    }
                    try
                    {
                        TestStack.White.UIItems.WindowItems.Window itemModal = ChkInnerWindows(item, sWindowTitle);
                        if (itemModal != null)
                        {
                            return itemModal;
                        }
                    }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
                    catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
                    {
                    }
                }


            }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
            catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
            {
                Log.Logger.LogData("Error in Regex in windows title string", LogLevel.Error);
                context.Abort();
            }
            return null;
        }

        public static TestStack.White.UIItems.WindowItems.Window ChkInnerWindows(TestStack.White.UIItems.WindowItems.Window item, string sWindowTitle)
    {
        Match mIn = null;
        TestStack.White.UIItems.WindowItems.Window itemInn1 = null;
       List <TestStack.White.UIItems.WindowItems.Window> windowsInn = item.ModalWindows();
        for (int i = 0; i < windowsInn.Count; i++)
        {
            TestStack.White.UIItems.WindowItems.Window itemInn = windowsInn[i];

            if (!string.IsNullOrEmpty(itemInn.Title))
            {
                    try
                    {
                        mIn = Regex.Match(itemInn.Title, sWindowTitle);
                    }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
                    catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
                    {

                    }
            }
            
            if (mIn != null && mIn.Success)
            {
                return itemInn;
            }
            itemInn1 = ChkInnerWindows(itemInn, sWindowTitle);
            
        }
            return null;
    }


        public void Visit(TestStack.White.UIItems.IUIItem item)
        {
            if (item is TestStack.White.UIItems.Custom.CustomUIItem)
            {
                // Process custom controls
                TestStack.White.UIItems.Custom.CustomUIItem customControl = item as CustomUIItem;

                // Retrieve all the child controls
                //IUIItem[] items = customControl.AsContainer().GetMultiple(White.Core.UIItems.Finders.SearchCriteria.All);
                IUIItem[] items = customControl.GetMultiple(TestStack.White.UIItems.Finders.SearchCriteria.All);


                // visit all the children
                foreach (var t in items)
                {
                    Visit(t);
                }
        }
            else
            {
                // Process normal controls
        }
        }
        public static object TakeActionOnControlByID(TestStack.White.UIItems.WindowItems.Window window, string strID, string ctrl, bool IsEventField, bool Activate, bool ClickBeforeValueSet, bool ContinueOnError, string setValue, ref bool UseNextPriority)
        {
            UseNextPriority = false;


            if ((ctrl.Trim() == "ControlType.Button"))
            {
                try
                {
                    //TestStack.White.
                    TestStack.White.UIItems.Button button = window.Get<TestStack.White.UIItems.Button>(SearchCriteria.ByAutomationId(strID));
                    if (Activate)
                    {
                        try
                        {
                            window.WaitWhileBusy();
                            button.Focus();
                            window.WaitWhileBusy();
                            Log.Logger.LogData("Button control activated:" + strID, LogLevel.Info);
                        }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
                        catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
                        {
                            Log.Logger.LogData("Error while setting focus on Button by Id" + strID, LogLevel.Warning);
                        }
                        Log.Logger.LogData("Button control activated:" + strID, LogLevel.Info);
                    }
                    if (ClickBeforeValueSet)
                    {
                        try
                        {
                            window.WaitWhileBusy();
                            button.RaiseClickEvent();
                            window.WaitWhileBusy();
                            Log.Logger.LogData("Button control clicked:" + strID, LogLevel.Info);
                        }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
                        catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
                        {
                            Log.Logger.LogData("Error while click before value set on Button by Id" + strID, LogLevel.Warning);
                        }
                    }
                    if (IsEventField)
                    {
                        window.WaitWhileBusy();
                        button.RaiseClickEvent();
                        window.WaitWhileBusy();
                        Log.Logger.LogData("Span control clicked:" + strID, LogLevel.Info);
                    }
                    Log.Logger.LogData("Span control value get:" + strID, LogLevel.Info);
                    return button.ToString();
                }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
                catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
                { UseNextPriority = true; }
                return string.Empty;
            }
            else if ((ctrl.Trim() == "ControlType.Edit"))
            {
                //Piyush
                IUIItem[] items = window.GetMultiple(SearchCriteria.All);
                foreach (IUIItem item in items)
                {
                    if (item is TestStack.White.UIItems.Custom.CustomUIItem)
                    {
                        // Process custom controls
                        TestStack.White.UIItems.Custom.CustomUIItem customControl = item as CustomUIItem;

                        // Retrieve all the child controls
                        //IUIItem[] items = customControl.AsContainer().GetMultiple(White.Core.UIItems.Finders.SearchCriteria.All);
                        IUIItem[] items4 = customControl.GetMultiple(TestStack.White.UIItems.Finders.SearchCriteria.All);


                        // visit all the children
                        foreach (var t in items4)
                        {
                            // Visit(t);
                        }
                    }

                    //TestStack.White.UIItems.IUIItem[] uc = window.GetMultiple(SearchCriteria.All);
                    //List<IUIItem> paneList = new List<IUIItem>();
                    //foreach (IUIItem item in uc)
                    //{
                    //    if(item.GetType().ToString().ToLower().Contains("pane"))
                    //    {
                    //        paneList.Add(item);
                    //        CustomUIItem customControl = item as CustomUIItem;
                    //        customControl. Container.Get<TextBox>("year").Text = dateTime.Year.ToString();
                    //        IUIItem[] items = customControl. GetMultiple(SearchCriteria.All);



                    //        // visit all the children
                    //        foreach (var t in items)
                    //        {
                    //            //visit(t);
                    //        }
                    //        //visit(item);
                    //    }
                    //    if (item.Id == strID)
                    //    {

                    //    }
                    //}

                    //TestStack.White.UIItems.UIItem lastpane = (TestStack.White.UIItems.UIItem)uc[5];

                    //TestStack.White.UIItems.UIItem txt = window.Get < TestStack.White.UIItems.UiItem> (SearchCriteria.ByAutomationId("pane"));

                    //TestStack.White.UIItems.TextBox txt = lastpane.Get<TestStack.White.UIItems.TextBox>(SearchCriteria.ByAutomationId(strID));
                    try
                    {
                        if (Activate)
                        {
                            //                        try
                            //                        {
                            //                            window.WaitWhileBusy();
                            //                            txt.Focus();
                            //                            window.WaitWhileBusy();
                            //                            Log.Logger.LogData("Edit control activated:" + strID, LogLevel.Info);
                            //                        }
                            //#pragma warning disable CS0168 // The variable 'ex' is declared but never used
                            //                        catch (Exception ex)
                            //#pragma warning restore CS0168 // The variable 'ex' is declared but never used
                            //                        {
                            //                            Log.Logger.LogData("Error while setting focus on Edit by Id: " + strID, LogLevel.Warning);
                            //                        }
                            Log.Logger.LogData("Control activated:" + strID, LogLevel.Info);
                           
                        }
                        if (ClickBeforeValueSet)
                        {
                            try
                            {
                                window.WaitWhileBusy();
                                //txt.RaiseClickEvent();
                                window.WaitWhileBusy();
                            }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
                            catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
                            {
                                Log.Logger.LogData("Error while click before value set on Edit by Id: " + strID, LogLevel.Warning);
                            }
                        }
                        if (!string.IsNullOrEmpty(setValue))
                        {
                            window.WaitWhileBusy();
                            //txt.SetValue(setValue);
                            window.WaitWhileBusy();
                            Log.Logger.LogData("Edit control value set:" + strID, LogLevel.Info);
                        }
                        if (IsEventField)
                        {
                            window.WaitWhileBusy();
                            //txt.RaiseClickEvent();
                            window.WaitWhileBusy();
                            Log.Logger.LogData("Span control clicked:" + strID, LogLevel.Info);
                        }
                        Log.Logger.LogData("Edit control value get:" + strID, LogLevel.Info);
                        return "";

                    }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
                    catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
                    { UseNextPriority = true; }
                    return string.Empty;
                }
                return string.Empty;
            }
            else if ((ctrl.Trim() == "ControlType.Text"))
            {
                try
                {
                    TestStack.White.UIItems.Label label = window.Get<TestStack.White.UIItems.Label>(SearchCriteria.ByAutomationId(strID));
                    try
                    {
                        if (Activate)
                        {
                            try
                            {
                                window.WaitWhileBusy();
                                label.Focus();
                                window.WaitWhileBusy();
                                Log.Logger.LogData("Text control activated:" + strID, LogLevel.Info);
                            }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
                            catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
                            {
                                Log.Logger.LogData("Error while setting focus on Text: " + strID, LogLevel.Warning);
                            }
                            Log.Logger.LogData("Text control activated:" + strID, LogLevel.Info);

                        }
                        if (ClickBeforeValueSet)
                        {
                            try
                            {
                                window.WaitWhileBusy();
                                label.RaiseClickEvent();
                                window.WaitWhileBusy();
                            }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
                            catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
                            {
                                Log.Logger.LogData("Error while click before value set on Text by Id" + strID, LogLevel.Warning);
                            }
                        }
                        if (!string.IsNullOrEmpty(setValue))
                        {
                            //label.Text = setValue;
                            Log.Logger.LogData("Control value can not be set for label:" + strID, LogLevel.Warning);
                        }
                        if (IsEventField)
                        {
                            window.WaitWhileBusy();
                            label.RaiseClickEvent();
                            window.WaitWhileBusy();
                            Log.Logger.LogData("Span control clicked:" + strID, LogLevel.Info);
                        }
                        Log.Logger.LogData("Text control value get:" + strID, LogLevel.Info);
                        return label.Text;
                    }
                    catch (Exception ex)
                    {
                        Log.Logger.LogData(ex.Message, LogLevel.Warning);
                        UseNextPriority = true;
                    }
                }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
                catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
                { UseNextPriority = true; }
                return string.Empty;
            }
            else if ((ctrl.Trim() == "ControlType.CheckBox"))
            {
                try
                {
                    TestStack.White.UIItems.CheckBox chk = window.Get<TestStack.White.UIItems.CheckBox>(SearchCriteria.ByAutomationId(strID));
                    window.WaitWhileBusy();
                    if (Activate)
                    {
                        try
                        {
                            window.WaitWhileBusy();
                            chk.Focus();
                            window.WaitWhileBusy();
                            Log.Logger.LogData("CheckBox control activated:" + strID, LogLevel.Info);
                        }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
                        catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
                        {
                            Log.Logger.LogData("Error while setting focus on CheckBox by Id" + strID, LogLevel.Warning);
                        }
                        Log.Logger.LogData("Control activated:" + strID, LogLevel.Info);

                    }
                    if (ClickBeforeValueSet)
                    {
                        try
                        {
                            window.WaitWhileBusy();
                            chk.RaiseClickEvent();
                            window.WaitWhileBusy();
                        }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
                        catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
                        {
                            Log.Logger.LogData("Error while click before value set on CheckBox by Id " + strID, LogLevel.Warning);
                        }
                    }
                    if (!string.IsNullOrEmpty(setValue))
                    {
                        if ((setValue.ToLower().Trim() == "true") || (setValue.ToLower().Trim() == "select") || (setValue.ToLower().Trim() == "1"))
                        {
                            chk.Checked = true;
                        }
                        else
                        {
                            chk.Checked = false;
                        }
                    }
                    if (IsEventField)
                    {
                        chk.RaiseClickEvent();
                        Log.Logger.LogData("CheckBox control clicked:" + strID, LogLevel.Info);
                    }
                    window.WaitWhileBusy();
                    return chk.Checked.ToString();
                }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
                catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
                { UseNextPriority = true; }
                return string.Empty;
            }
            else if (ctrl.Trim() == "ControlType.RadioButton")
            {
                try
                {
                    TestStack.White.UIItems.RadioButton radioButton = window.Get<TestStack.White.UIItems.RadioButton>(SearchCriteria.ByAutomationId(strID));
                    window.WaitWhileBusy();
                    if (Activate)
                    {
                        try
                        {
                            radioButton.Focus();
                            Log.Logger.LogData("RadioButton control activated:" + strID, LogLevel.Info);
                        }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
                        catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
                        {
                            Log.Logger.LogData("Error while setting focus on RadioButton by Id" + strID, LogLevel.Warning);
                        }
                        Log.Logger.LogData("Control activated:" + strID, LogLevel.Info);

                    }
                    if (ClickBeforeValueSet)
                    {
                        try
                        {
                            radioButton.RaiseClickEvent();
                        }
                        catch (Exception)
                        {
                            Log.Logger.LogData("Error while click before value set on RadioButton by Id: " + strID, LogLevel.Warning);
                        }
                    }
                    if (!string.IsNullOrEmpty(setValue))
                    {
                        if ((setValue.ToLower().Trim() == "true") || (setValue.ToLower().Trim() == "select") || (setValue.ToLower().Trim() == "1"))
                        {
                            radioButton.Select();
                        }
                        else
                        {
                            radioButton.Select();
                        }
                    }
                    if (IsEventField)
                    {
                        radioButton.RaiseClickEvent();
                        Log.Logger.LogData("RadioButton control clicked:" + strID, LogLevel.Info);
                    }
                    window.WaitWhileBusy();
                    return radioButton.IsSelected.ToString();
                }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
                catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
                { UseNextPriority = true; }
                return string.Empty;

            }
            else if (ctrl.Trim() == "ControlType.ComboBox")
            {
                try
                {
                    TestStack.White.UIItems.ListBoxItems.ComboBox comboBox = window.Get<TestStack.White.UIItems.ListBoxItems.ComboBox>(SearchCriteria.ByAutomationId(strID));
                    window.WaitWhileBusy();
                    if (Activate)
                    {
                        try
                        {
                            comboBox.Focus();
                            Log.Logger.LogData("Combobox Control activated:" + strID, LogLevel.Info);
                        }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
                        catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
                        {
                            Log.Logger.LogData("Error while setting focus on ComboBox by Id:" + strID, LogLevel.Warning);
                        }
                    }
                    if (ClickBeforeValueSet)
                    {
                        try
                        {
                            comboBox.RaiseClickEvent();
                        }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
                        catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
                        {
                            Log.Logger.LogData("Error while Click before combobox value set on Id: " + strID, LogLevel.Warning);
                        }
                    }
                    if (!string.IsNullOrEmpty(setValue))
                    {
                        comboBox.Select(setValue);
                        Log.Logger.LogData("combobox value selected:" + strID, LogLevel.Info);
                    }
                    if (IsEventField)
                    {
                        comboBox.RaiseClickEvent();
                        Log.Logger.LogData("combobox control clicked:" + strID, LogLevel.Info);
                    }
                    window.WaitWhileBusy();
                    return comboBox.SelectedItemText;
                }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
                catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
                {
                    Log.Logger.LogData("Error in ComboBox set value by id:" + strID, LogLevel.Warning);
                    UseNextPriority = true;
                }
                return string.Empty;

            }
            else if ((ctrl.Trim() == " ControlType.Table"))
            {
                try
                {
                    TestStack.White.UIItems.TableItems.Table table = window.Get<TestStack.White.UIItems.TableItems.Table>(SearchCriteria.ByAutomationId(strID));

                    window.WaitWhileBusy();
                    if (Activate)
                    {
                        try
                        {
                            table.Focus();
                            Log.Logger.LogData("Table control activated:" + strID, LogLevel.Info);
                        }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
                        catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
                        {
                            Log.Logger.LogData("Error while setting focus on Table by Id: " + strID, LogLevel.Warning);
                        }
                    }
                    if (ClickBeforeValueSet)
                    {
                        try
                        {
                            table.RaiseClickEvent();
                        }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
                        catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
                        {
                            Log.Logger.LogData("Error while Click before Table value set on  by Id: " + strID, LogLevel.Warning);
                        }
                    }
                    if (!string.IsNullOrEmpty(setValue))
                    {
                        table.SetValue(setValue); //Need to send table object
                        Log.Logger.LogData("Table value selected:" + strID, LogLevel.Info);
                    }
                    if (IsEventField)
                    {
                        table.RaiseClickEvent();
                        Log.Logger.LogData("Table control clicked:" + strID, LogLevel.Info);
                    }
                    window.WaitWhileBusy();
                    return table;
                }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
                catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
                {
                    Log.Logger.LogData("Error in table set value by id:" + strID, LogLevel.Warning);
                    UseNextPriority = true;
                }
                return string.Empty;
            }
            else if ((ctrl.Trim() == "ControlType.Image"))
            {
                try
                {
                    TestStack.White.UIItems.Image image = window.Get<TestStack.White.UIItems.Image>(SearchCriteria.ByAutomationId(strID));
                    window.WaitWhileBusy();
                    if (Activate)
                    {
                        try
                        {
                            image.Focus();
                            Log.Logger.LogData("Image Control activated:" + strID, LogLevel.Info);
                        }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
                        catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
                        {
                            Log.Logger.LogData("Error while setting focus on image:" + strID, LogLevel.Warning);
                        }
                    }
                    if (ClickBeforeValueSet)
                    {
                        try
                        {
                            image.RaiseClickEvent();
                        }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
                        catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
                        {
                            Log.Logger.LogData("Error while Click Before image Value Set on Id: " + strID, LogLevel.Warning);
                        }
                    }
                    if (!string.IsNullOrEmpty(setValue))
                    {
                        image.SetValue(setValue); //Need to send table object
                        Log.Logger.LogData("Image value selected:" + strID, LogLevel.Info);
                    }
                    if (IsEventField)
                    {
                        image.RaiseClickEvent();
                        Log.Logger.LogData("Image control clicked:" + strID, LogLevel.Info);
                    }
                    window.WaitWhileBusy();
                    return image;
                }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
                catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
                {
                    Log.Logger.LogData("Error in image set value by id:" + strID, LogLevel.Warning);
                    UseNextPriority = true;
                }
                return string.Empty;
            }
            else if (ctrl.Trim() == "ControlType.ListItem")
            {
                try
                {
                    TestStack.White.UIItems.ListBoxItems.ListItem listItem = window.Get<TestStack.White.UIItems.ListBoxItems.ListItem>(SearchCriteria.ByAutomationId(strID));
                    window.WaitWhileBusy();
                    if (Activate)
                    {
                        try
                        {
                            listItem.Focus();
                            Log.Logger.LogData("ListItem Control activated:" + strID, LogLevel.Info);
                        }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
                        catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
                        {
                            Log.Logger.LogData("Error while setting focus on ListItem:" + strID, LogLevel.Warning);
                        }
                    }
                    if (ClickBeforeValueSet)
                    {
                        try
                        {
                            listItem.RaiseClickEvent();
                        }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
                        catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
                        {
                            Log.Logger.LogData("Error while click before ListItem value set on Id: " + strID, LogLevel.Warning);
                        }
                    }
                    if (!string.IsNullOrEmpty(setValue))
                    {
                        listItem.SetValue(setValue); //Need to send table object
                        Log.Logger.LogData("ListItem value selected:" + strID, LogLevel.Info);
                    }
                    if (IsEventField)
                    {
                        listItem.RaiseClickEvent();
                        Log.Logger.LogData("ListItem control clicked:" + strID, LogLevel.Info);
                    }
                    window.WaitWhileBusy();
                    return listItem.Text;
                }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
                catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
                { UseNextPriority = true; }
                return string.Empty;

            }
            else if ((ctrl.Trim() == "ControlType.List"))
            {
                try
                {
                    TestStack.White.UIItems.ListBoxItems.ListBox comboBox = window.Get<TestStack.White.UIItems.ListBoxItems.ListBox>(SearchCriteria.ByAutomationId(strID));
                    window.WaitWhileBusy();
                    if (Activate)
                    {
                        try
                        {
                            comboBox.Focus();
                            Log.Logger.LogData("List control activated:" + strID, LogLevel.Info);
                        }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
                        catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
                        {
                            Log.Logger.LogData("Error while setting focus on List by Id:" + strID, LogLevel.Warning);
                        }
                    }
                    if (ClickBeforeValueSet)
                    {
                        try
                        {
                            comboBox.RaiseClickEvent();
                        }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
                        catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
                        {
                            Log.Logger.LogData("Error while Click before List value set on Id:  " + strID, LogLevel.Warning);
                        }
                    }
                    if (!string.IsNullOrEmpty(setValue))
                    {
                        comboBox.SetValue(setValue);
                        Log.Logger.LogData("List value selected:" + strID, LogLevel.Info);
                    }
                    if (IsEventField)
                    {
                        comboBox.RaiseClickEvent();
                        Log.Logger.LogData("List control clicked:" + strID, LogLevel.Info);
                    }
                    window.WaitWhileBusy();
                    return comboBox.SelectedItemText;
                }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
                catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
                { UseNextPriority = true; }
                return string.Empty;
            }
            else if (ctrl.Trim() == "ControlType.Menu")
            {
                try
                {
                    TestStack.White.UIItems.MenuItems.Menu menuItem = window.Get<TestStack.White.UIItems.MenuItems.Menu>(SearchCriteria.ByAutomationId(strID));

                    window.WaitWhileBusy();
                    if (Activate)
                    {
                        try
                        {
                            menuItem.Focus();
                            Log.Logger.LogData("List control activated:" + strID, LogLevel.Info);
                        }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
                        catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
                        {
                            Log.Logger.LogData("Error while setting focus on Menu by Id: " + strID, LogLevel.Warning);
                        }
                    }
                    if (ClickBeforeValueSet)
                    {
                        try
                        {
                            menuItem.RaiseClickEvent();
                        }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
                        catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
                        {
                            Log.Logger.LogData("Error while click before Menu value set on Id: " + strID, LogLevel.Warning);
                        }
                    }
                    if (!string.IsNullOrEmpty(setValue))
                    {
                        menuItem.SetValue(setValue);
                        Log.Logger.LogData("Menu value selected:" + strID, LogLevel.Info);
                    }
                    if (IsEventField)
                    {
                        menuItem.RaiseClickEvent();
                        Log.Logger.LogData("Menu control clicked:" + strID, LogLevel.Info);
                    }
                    window.WaitWhileBusy();
                    return menuItem.ToString();
                }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
                catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
                { UseNextPriority = true; }
                return string.Empty;

            }
            else if ((ctrl.Trim() == "ControlType.MenuBar"))
            {
                try
                {
                    TestStack.White.UIItems.WindowStripControls.MenuBar menuBar = window.Get<TestStack.White.UIItems.WindowStripControls.MenuBar>(SearchCriteria.ByAutomationId(strID));

                    window.WaitWhileBusy();
                    if (Activate)
                    {
                        try
                        {
                            menuBar.Focus();
                            Log.Logger.LogData("MenuBar Control activated:" + strID, LogLevel.Info);
                        }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
                        catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
                        {
                            Log.Logger.LogData("Error while setting focus on MenuBar by Id: " + strID, LogLevel.Warning);
                        }
                    }
                    if (ClickBeforeValueSet)
                    {
                        try
                        {
                            menuBar.RaiseClickEvent();
                        }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
                        catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
                        {
                            Log.Logger.LogData("Error while Click before MenuBar value set on Id: " + strID, LogLevel.Warning);
                        }
                    }
                    if (!string.IsNullOrEmpty(setValue))
                    {
                        menuBar.SetValue(setValue);
                        Log.Logger.LogData("MenuBar value selected:" + strID, LogLevel.Info);
                    }
                    if (IsEventField)
                    {
                        menuBar.RaiseClickEvent();
                        Log.Logger.LogData("MenuBar control clicked:" + strID, LogLevel.Info);
                    }
                    window.WaitWhileBusy();
                    return menuBar.ToString();
                }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
                catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
                { UseNextPriority = true; }
                return string.Empty;
            }
            else if (ctrl.Trim() == "ControlType.MenuItem")
            {
                try
                {
                    TestStack.White.UIItems.MenuItems.Menu menuItem = window.Get<TestStack.White.UIItems.MenuItems.Menu>(SearchCriteria.ByAutomationId(strID));

                    window.WaitWhileBusy();
                    if (Activate)
                    {
                        try
                        {
                            menuItem.Focus();
                            Log.Logger.LogData("MenuItem control activated:" + strID, LogLevel.Info);
                        }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
                        catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
                        {
                            Log.Logger.LogData("Error while setting focus on MenuItem by Id:" + strID, LogLevel.Warning);
                        }
                    }
                    if (ClickBeforeValueSet)
                    {
                        try
                        {
                            menuItem.RaiseClickEvent();
                        }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
                        catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
                        {
                            Log.Logger.LogData("Error while Click before MenuItem value set on Id: " + strID, LogLevel.Warning);
                        }
                    }
                    if (!string.IsNullOrEmpty(setValue))
                    {
                        menuItem.SetValue(setValue);
                        Log.Logger.LogData("MenuItem value selected:" + strID, LogLevel.Info);
                    }
                    if (IsEventField)
                    {
                        menuItem.RaiseClickEvent();
                        Log.Logger.LogData("MenuItem control clicked:" + strID, LogLevel.Info);
                    }
                    window.WaitWhileBusy();
                    return menuItem.ToString();
                }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
                catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
                { UseNextPriority = true; }
                return string.Empty;

            }
            else if ((ctrl.Trim() == "ControlType.Calendar") || (ctrl.Trim() == "ControlType.Pane"))
            {
                try
                {
                    TestStack.White.UIItems.DateTimePicker sDate = window.Get<TestStack.White.UIItems.DateTimePicker>(SearchCriteria.ByAutomationId(strID));
                    window.WaitWhileBusy();
                    if (sDate != null)
                    {
                        if (Activate)
                        {
                            try
                            {
                                sDate.Focus();
                                Log.Logger.LogData("Calendar control activated:" + strID, LogLevel.Info);
                            }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
                            catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
                            {
                                Log.Logger.LogData("Error while setting focus on Calendar by Id:" + strID, LogLevel.Warning);
                            }
                        }
                        if (ClickBeforeValueSet)
                        {
                            try
                            {
                                sDate.RaiseClickEvent();
                            }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
                            catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
                            {
                                Log.Logger.LogData("Error while click before Calendar value set on Id: " + strID, LogLevel.Warning);
                            }
                        }
                        if (!string.IsNullOrEmpty(setValue))
                        {
                            System.DateTime dateTime = Convert.ToDateTime(setValue);
                            sDate.SetDate(dateTime, TestStack.White.UIItems.DateFormat.MonthDayYear);
                            Log.Logger.LogData("Calendar value selected:" + strID, LogLevel.Info);
                        }
                        if (IsEventField)
                        {
                            sDate.RaiseClickEvent();
                            Log.Logger.LogData("Calendar control clicked:" + strID, LogLevel.Info);
                        }
                        window.WaitWhileBusy();
                        return sDate.Date.ToString();//ToLongDateString();
                    }
                    else
                    {
                        TestStack.White.UIItems.PropertyGridItems.PropertyGrid propertyGrid = window.Get<TestStack.White.UIItems.PropertyGridItems.PropertyGrid>(SearchCriteria.ByAutomationId(strID));
                        window.WaitWhileBusy();
                        if (propertyGrid != null)
                        {

                            if (Activate)
                            {
                                try
                                {
                                    propertyGrid.RaiseClickEvent();
                                    Log.Logger.LogData("PropertyGrid control activated:" + strID, LogLevel.Info);
                                }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
                                catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
                                {
                                    Log.Logger.LogData("Error while setting focus on PropertyGrid by Id: " + strID, LogLevel.Warning);
                                }
                            }
                            if (ClickBeforeValueSet)
                            {
                                try
                                {
                                    propertyGrid.RaiseClickEvent();
                                }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
                                catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
                                {
                                    Log.Logger.LogData("Error while Click before PropertyGrid value set on Id: " + strID, LogLevel.Warning);
                                }
                            }
                            if (!string.IsNullOrEmpty(setValue))
                            {
                                try
                                {
                                    propertyGrid.SetValue(setValue);
                                    Log.Logger.LogData("PropertyGrid value selected:" + strID, LogLevel.Info);
                                }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
                                catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
                                {
                                    Log.Logger.LogData("Error in propertyGrid set value by Id:" + strID, LogLevel.Info);
                                }
                            }
                            if (IsEventField)
                            {
                                propertyGrid.RaiseClickEvent();
                                Log.Logger.LogData("PropertyGrid control clicked:" + strID, LogLevel.Info);
                            }
                            window.WaitWhileBusy();
                            return propertyGrid;
                        }
                    }
                    window.WaitWhileBusy();
                }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
                catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
                { UseNextPriority = true; }
                return string.Empty;

            }
            else if (ctrl.Trim() == "ControlType.Hyperlink")
            {
                try
                {
                    TestStack.White.UIItems.Hyperlink hlink = window.Get<TestStack.White.UIItems.Hyperlink>(SearchCriteria.ByAutomationId(strID));
                    window.WaitWhileBusy();
                    if (Activate)
                    {
                        try
                        {
                            hlink.RaiseClickEvent();
                            Log.Logger.LogData("Hyperlink Control activated:" + strID, LogLevel.Info);
                        }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
                        catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
                        {
                            Log.Logger.LogData("Error while setting focus on Hyperlink by Id:" + strID, LogLevel.Warning);
                        }
                    }
                    if (ClickBeforeValueSet)
                    {
                        try
                        {
                            hlink.RaiseClickEvent();
                        }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
                        catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
                        {
                            Log.Logger.LogData("Error while Click before Hyperlink value set on Id: " + strID, LogLevel.Warning);
                        }
                    }
                    if (!string.IsNullOrEmpty(setValue))
                    {
                        try
                        {
                            hlink.SetValue(setValue);
                            Log.Logger.LogData("Hyperlink value selected:" + strID, LogLevel.Info);
                        }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
                        catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
                        {
                            Log.Logger.LogData("Error in Hyperlink set value by Id:" + strID, LogLevel.Info);
                        }
                    }
                    if (IsEventField)
                    {
                        hlink.RaiseClickEvent();
                        Log.Logger.LogData("Hyperlink control clicked:" + strID, LogLevel.Info);
                    }
                    window.WaitWhileBusy();
                    return hlink.ToString();
                }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
                catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
                { UseNextPriority = true; }
                return string.Empty;

            }
            else if ((ctrl.Trim() == " ControlType.Tab"))
            {
                try
                {
                    TestStack.White.UIItems.TabItems.Tab tab = window.Get<TestStack.White.UIItems.TabItems.Tab>(SearchCriteria.ByAutomationId(strID));
                    window.WaitWhileBusy();
                    if (Activate)
                    {
                        try
                        {
                            tab.RaiseClickEvent();
                            Log.Logger.LogData("Tab control activated:" + strID, LogLevel.Info);
                        }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
                        catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
                        {
                            Log.Logger.LogData("Error while setting focus on Tab by Id: " + strID, LogLevel.Warning);
                        }
                    }
                    if (ClickBeforeValueSet)
                    {
                        try
                        {
                            tab.RaiseClickEvent();
                        }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
                        catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
                        {
                            Log.Logger.LogData("Error while Click before Tab value set on Id: " + strID, LogLevel.Warning);
                        }
                    }
                    if (!string.IsNullOrEmpty(setValue))
                    {
                        try
                        {
                            tab.SetValue(setValue);
                            Log.Logger.LogData("Tab value selected:" + strID, LogLevel.Info);
                        }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
                        catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
                        {
                            Log.Logger.LogData("Error in Tab set value by Id:" + strID, LogLevel.Info);
                        }
                    }
                    if (IsEventField)
                    {
                        tab.RaiseClickEvent();
                        Log.Logger.LogData("Tab control clicked:" + strID, LogLevel.Info);
                    }
                    window.WaitWhileBusy();
                    return tab.ToString();
                }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
                catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
                { UseNextPriority = true; }
                return string.Empty;
            }
            else if (ctrl.Trim() == "ControlType.TabItem")
            {
                try
                {
                    TestStack.White.UIItems.TabItems.TabPage tabPage = window.Get<TestStack.White.UIItems.TabItems.TabPage>(SearchCriteria.ByAutomationId(strID));
                    window.WaitWhileBusy();
                    if (Activate)
                    {
                        try
                        {
                            tabPage.RaiseClickEvent();
                            Log.Logger.LogData("TabItem control activated:" + strID, LogLevel.Info);
                        }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
                        catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
                        {
                            Log.Logger.LogData("Error while setting focus on TabItem by Id: " + strID, LogLevel.Warning);
                        }
                    }
                    if (ClickBeforeValueSet)
                    {
                        try
                        {
                            tabPage.RaiseClickEvent();
                        }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
                        catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
                        {
                            Log.Logger.LogData("Error while click before TabItem value set on Id: " + strID, LogLevel.Warning);
                        }
                    }
                    if (!string.IsNullOrEmpty(setValue))
                    {
                        try
                        {
                            tabPage.SetValue(setValue);
                            Log.Logger.LogData("TabItem value selected:" + strID, LogLevel.Info);
                        }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
                        catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
                        {
                            Log.Logger.LogData("Error in TabItem set value by Id:" + strID, LogLevel.Info);
                        }
                    }
                    if (IsEventField)
                    {
                        tabPage.RaiseClickEvent();
                        Log.Logger.LogData("TabItem control clicked:" + strID, LogLevel.Info);
                    }
                    window.WaitWhileBusy();
                    return tabPage.ToString();
                }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
                catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
                { UseNextPriority = true; }
                return string.Empty;

            }
            else if ((ctrl.Trim() == "ControlType.DataGrid"))
            {
                try
                {
                    TestStack.White.UIItems.ListView dataGrid = window.Get<TestStack.White.UIItems.ListView>(SearchCriteria.ByAutomationId(strID));

                    window.WaitWhileBusy();
                    if (Activate)
                    {
                        try
                        {
                            dataGrid.RaiseClickEvent();
                            Log.Logger.LogData("DataGrid control activated:" + strID, LogLevel.Info);
                        }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
                        catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
                        {
                            Log.Logger.LogData("Error while setting focus on DataGrid by Id: " + strID, LogLevel.Warning);
                        }
                    }
                    if (ClickBeforeValueSet)
                    {
                        try
                        {
                            dataGrid.RaiseClickEvent();
                        }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
                        catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
                        {
                            Log.Logger.LogData("Error while Click before List value set on Id: " + strID, LogLevel.Warning);
                        }
                    }
                    if (!string.IsNullOrEmpty(setValue))
                    {
                        try
                        {
                            dataGrid.SetValue(setValue);
                            Log.Logger.LogData("List value selected:" + strID, LogLevel.Info);
                        }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
                        catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
                        {
                            Log.Logger.LogData("Error in propertyGrid set value by Id:" + strID, LogLevel.Info);
                        }
                    }
                    if (IsEventField)
                    {
                        dataGrid.RaiseClickEvent();
                        Log.Logger.LogData("List control clicked:" + strID, LogLevel.Info);
                    }
                    window.WaitWhileBusy();
                    return dataGrid;
                }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
                catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
                { UseNextPriority = true; }
                return string.Empty;
            }
            else if (ctrl.Trim() == "ControlType.DataItem")
            {
                try
                {
                    TestStack.White.UIItems.ListViewRow dataGridrow = window.Get<TestStack.White.UIItems.ListViewRow>(SearchCriteria.ByAutomationId(strID));

                    window.WaitWhileBusy();
                    if (Activate)
                    {
                        try
                        {
                            dataGridrow.RaiseClickEvent();
                            Log.Logger.LogData("List control activated:" + strID, LogLevel.Info);
                        }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
                        catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
                        {
                            Log.Logger.LogData("Error while setting focus on List by Id:" + strID, LogLevel.Warning);
                        }
                    }
                    if (ClickBeforeValueSet)
                    {
                        try
                        {
                            dataGridrow.RaiseClickEvent();
                        }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
                        catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
                        {
                            Log.Logger.LogData("Error while Click before List value set on Id: " + strID, LogLevel.Warning);
                        }
                    }
                    if (!string.IsNullOrEmpty(setValue))
                    {
                        try
                        {
                            dataGridrow.SetValue(setValue);
                            Log.Logger.LogData("List value selected:" + strID, LogLevel.Info);
                        }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
                        catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
                        {
                            Log.Logger.LogData("Error in propertyGrid set value by Id:" + strID, LogLevel.Info);
                        }
                    }
                    if (IsEventField)
                    {
                        dataGridrow.RaiseClickEvent();
                        Log.Logger.LogData("List control clicked:" + strID, LogLevel.Info);
                    }
                    window.WaitWhileBusy();
                    return dataGridrow;
                }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
                catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
                { UseNextPriority = true; }
                return string.Empty;

            }
            else if ((ctrl.Trim() == " ControlType.TreeItem") || (ctrl.Trim() == "ControlType.Tree"))
            {
                try
                {
                    TestStack.White.UIItems.TreeItems.TreeNode nodeOne = window.Get<TestStack.White.UIItems.TreeItems.TreeNode>(SearchCriteria.ByAutomationId(strID));
                    //nodeOne.Select();
                    //nodeOne.Expand();
                    window.WaitWhileBusy();
                    if (Activate)
                    {
                        try
                        {
                            nodeOne.RaiseClickEvent();
                            Log.Logger.LogData("TreeNode control activated:" + strID, LogLevel.Info);
                        }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
                        catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
                        {
                            Log.Logger.LogData("Error while setting focus on TreeNode by Id: " + strID, LogLevel.Warning);
                        }
                    }
                    if (ClickBeforeValueSet)
                    {
                        try
                        {
                            nodeOne.RaiseClickEvent();
                        }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
                        catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
                        {
                            Log.Logger.LogData("Error while Click before TreeNode value set on Id: " + strID, LogLevel.Warning);
                        }
                    }
                    if (!string.IsNullOrEmpty(setValue))
                    {
                        try
                        {
                            nodeOne.Select();
                            Log.Logger.LogData("TreeNode value selected:" + strID, LogLevel.Info);
                        }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
                        catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
                        {
                            Log.Logger.LogData("Error in TreeNode set value by Id:" + strID, LogLevel.Info);
                        }
                    }
                    if (IsEventField)
                    {
                        nodeOne.RaiseClickEvent();
                        Log.Logger.LogData("TreeNode control clicked:" + strID, LogLevel.Info);
                    }
                    window.WaitWhileBusy();
                    return nodeOne.Text;
                }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
                catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
                { UseNextPriority = true; }
                return string.Empty;
            }
            else if ((ctrl.Trim() == " ControlType.ProgressBar"))
            {
                try
                {
                    TestStack.White.UIItems.ProgressBar progressBar = window.Get<TestStack.White.UIItems.ProgressBar>(SearchCriteria.ByAutomationId(strID));
                    progressBar.RaiseClickEvent();
                    do
                    {
                        Thread.Sleep(5000);
                    } while (progressBar.Value != progressBar.Maximum);

                }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
                catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
                { UseNextPriority = true; }
                return string.Empty;
            }
            else if (ctrl.Trim() == "ControlType.Slider")
            {
                try
                {
                    TestStack.White.UIItems.Slider sldrOne = window.Get<TestStack.White.UIItems.Slider>(SearchCriteria.ByAutomationId(strID));

                    sldrOne.LargeIncrementButton.RaiseClickEvent();
                    //   sldrOne.LargeDecrementButton.RaiseClickEvent();
                }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
                catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
                { UseNextPriority = true; }
                return string.Empty;

            }
            else if ((ctrl.Trim() == "ControlType.Spinner"))
            {
                try
                {
                    TestStack.White.UIItems.Spinner spinner = window.Get<TestStack.White.UIItems.Spinner>(SearchCriteria.ByAutomationId(strID));
                    window.WaitWhileBusy();
                    if (Activate)
                    {
                        try
                        {
                            spinner.RaiseClickEvent();
                            Log.Logger.LogData("Spinner control activated:" + strID, LogLevel.Info);
                        }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
                        catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
                        {
                            Log.Logger.LogData("Error while setting focus on Spinner by Id: " + strID, LogLevel.Warning);
                        }
                    }
                    if (ClickBeforeValueSet)
                    {
                        try
                        {
                            spinner.RaiseClickEvent();
                        }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
                        catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
                        {
                            Log.Logger.LogData("Error while Click before Spinner value set on Id: " + strID, LogLevel.Warning);
                        }
                    }
                    if (!string.IsNullOrEmpty(setValue))
                    {
                        try
                        {
                            spinner.SetValue(setValue);
                            Log.Logger.LogData("Spinner value selected:" + strID, LogLevel.Info);
                        }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
                        catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
                        {
                            Log.Logger.LogData("Error in Spinner set value by Id:" + strID, LogLevel.Info);
                        }
                    }
                    if (IsEventField)
                    {
                        spinner.RaiseClickEvent();
                        Log.Logger.LogData("Spinner control clicked:" + strID, LogLevel.Info);
                    }
                    window.WaitWhileBusy();
                    return spinner.Value.ToString();
                }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
                catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
                { UseNextPriority = true; }
                return string.Empty;
            }
            else if (ctrl.Trim() == "ControlType.ToolBar")
            {
                try
                {
                    TestStack.White.UIItems.WindowStripControls.ToolStrip toolStrip = window.Get<TestStack.White.UIItems.WindowStripControls.ToolStrip>(SearchCriteria.ByAutomationId(strID));
                    window.WaitWhileBusy();
                    if (Activate)
                    {
                        try
                        {
                            toolStrip.RaiseClickEvent();
                            Log.Logger.LogData("ToolBar control activated:" + strID, LogLevel.Info);
                        }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
                        catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
                        {
                            Log.Logger.LogData("Error while setting focus on ToolBar by Id: " + strID, LogLevel.Warning);
                        }
                    }
                    if (ClickBeforeValueSet)
                    {
                        try
                        {
                            toolStrip.RaiseClickEvent();
                        }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
                        catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
                        {
                            Log.Logger.LogData("Error while Click before ToolBar value set on Id: " + strID, LogLevel.Warning);
                        }
                    }
                    if (!string.IsNullOrEmpty(setValue))
                    {
                        try
                        {
                            toolStrip.SetValue(setValue);
                            Log.Logger.LogData("ToolBar value selected:" + strID, LogLevel.Info);
                        }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
                        catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
                        {
                            Log.Logger.LogData("Error in ToolBar set value by Id:" + strID, LogLevel.Info);
                        }
                    }
                    if (IsEventField)
                    {
                        toolStrip.RaiseClickEvent();
                        Log.Logger.LogData("ToolBar control clicked:" + strID, LogLevel.Info);
                    }
                    window.WaitWhileBusy();
                    return toolStrip.ToString();
                }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
                catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
                { UseNextPriority = true; }
                return string.Empty;

            }
            else if ((ctrl.Trim() == "ControlType.ToolTip"))
            {
                try
                {
                    TestStack.White.UIItems.ToolTip toolTip = window.Get<TestStack.White.UIItems.ToolTip>(SearchCriteria.ByAutomationId(strID));
                    window.WaitWhileBusy();
                    if (Activate)
                    {
                        try
                        {
                            toolTip.RaiseClickEvent();
                            Log.Logger.LogData("ToolTip control activated:" + strID, LogLevel.Info);
                        }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
                        catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
                        {
                            Log.Logger.LogData("Error while setting focus on ToolTip by Id:" + strID, LogLevel.Warning);
                        }
                    }
                    if (ClickBeforeValueSet)
                    {
                        try
                        {
                            toolTip.RaiseClickEvent();
                        }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
                        catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
                        {
                            Log.Logger.LogData("Error while Click before ToolTip value set on Id: " + strID, LogLevel.Warning);
                        }
                    }
                    if (!string.IsNullOrEmpty(setValue))
                    {
                        try
                        {
                            toolTip.SetValue(setValue);
                            Log.Logger.LogData("ToolTip value selected:" + strID, LogLevel.Info);
                        }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
                        catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
                        {
                            Log.Logger.LogData("Error in ToolTip set value by Id:" + strID, LogLevel.Info);
                        }
                    }
                    if (IsEventField)
                    {
                        toolTip.RaiseClickEvent();
                        Log.Logger.LogData("ToolTip control clicked:" + strID, LogLevel.Info);
                    }
                    window.WaitWhileBusy();
                    return toolTip.Text;

                }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
                catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
                { UseNextPriority = true; }
                return string.Empty;
            }
            else if ((ctrl.Trim() == "ControlType.Group"))
            {
                try
                {
                    TestStack.White.UIItems.GroupBox groupBox = window.Get<TestStack.White.UIItems.GroupBox>(SearchCriteria.ByAutomationId(strID));

                    window.WaitWhileBusy();
                    if (Activate)
                    {
                        try
                        {
                            groupBox.RaiseClickEvent();
                            Log.Logger.LogData("Group control activated:" + strID, LogLevel.Info);
                        }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
                        catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
                        {
                            Log.Logger.LogData("Error while setting focus on Group by Id:" + strID, LogLevel.Warning);
                        }
                    }
                    if (ClickBeforeValueSet)
                    {
                        try
                        {
                            groupBox.RaiseClickEvent();
                        }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
                        catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
                        {
                            Log.Logger.LogData("Error while Click before Group value set on Id: " + strID, LogLevel.Warning);
                        }
                    }
                    if (!string.IsNullOrEmpty(setValue))
                    {
                        try
                        {
                            groupBox.SetValue(setValue);
                            Log.Logger.LogData("Group value selected:" + strID, LogLevel.Info);
                        }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
                        catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
                        {
                            Log.Logger.LogData("Error in Group set value by Id:" + strID, LogLevel.Info);
                        }
                    }
                    if (IsEventField)
                    {
                        groupBox.RaiseClickEvent();
                        Log.Logger.LogData("Group control clicked:" + strID, LogLevel.Info);
                    }
                    window.WaitWhileBusy();
                    return groupBox.ToString();
                }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
                catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
                { UseNextPriority = true; }
                return string.Empty;
            }
            else if (ctrl.Trim() == "ControlType.Thumb")
            {
                try
                {
                    TestStack.White.UIItems.Thumb thumb = window.Get<TestStack.White.UIItems.Thumb>(SearchCriteria.ByAutomationId(strID));

                    window.WaitWhileBusy();
                    if (Activate)
                    {
                        try
                        {
                            thumb.RaiseClickEvent();
                            Log.Logger.LogData("Thumb control activated:" + strID, LogLevel.Info);
                        }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
                        catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
                        {
                            Log.Logger.LogData("Error while setting focus on Thumb by Id:" + strID, LogLevel.Warning);
                        }
                    }
                    if (ClickBeforeValueSet)
                    {
                        try
                        {
                            thumb.RaiseClickEvent();
                        }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
                        catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
                        {
                            Log.Logger.LogData("Error while Click before Thumb value set on Id: " + strID, LogLevel.Warning);
                        }
                    }
                    if (!string.IsNullOrEmpty(setValue))
                    {
                        try
                        {
                            thumb.SetValue(setValue);
                            Log.Logger.LogData("Thumb value selected:" + strID, LogLevel.Info);
                        }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
                        catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
                        {
                            Log.Logger.LogData("Error in Thumb set value by Id:" + strID, LogLevel.Info);
                        }
                    }
                    if (IsEventField)
                    {
                        thumb.RaiseClickEvent();
                        Log.Logger.LogData("Thumb control clicked:" + strID, LogLevel.Info);
                    }
                    window.WaitWhileBusy();
                    return thumb.ToString();
                }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
                catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
                { UseNextPriority = true; }
                return string.Empty;

            }
            else if (ctrl.Trim() == "ControlType.TitleBar")
            {
                try
                {
                    TestStack.White.UIItems.WindowItems.TitleBar titleBar = window.Get<TestStack.White.UIItems.WindowItems.TitleBar>(SearchCriteria.ByAutomationId(strID));

                    window.WaitWhileBusy();
                    if (Activate)
                    {
                        try
                        {
                            titleBar.RaiseClickEvent();
                            Log.Logger.LogData("TitleBar control activated:" + strID, LogLevel.Info);
                        }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
                        catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
                        {
                            Log.Logger.LogData("Error while setting focus on TitleBar by Id:" + strID, LogLevel.Warning);
                        }
                    }
                    if (ClickBeforeValueSet)
                    {
                        try
                        {
                            titleBar.RaiseClickEvent();
                        }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
                        catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
                        {
                            Log.Logger.LogData("Error while Click before TitleBar value set on Id: " + strID, LogLevel.Warning);
                        }
                    }
                    if (!string.IsNullOrEmpty(setValue))
                    {
                        try
                        {
                            titleBar.SetValue(setValue);
                            Log.Logger.LogData("TitleBar value selected:" + strID, LogLevel.Info);
                        }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
                        catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
                        {
                            Log.Logger.LogData("Error in TitleBar set value by Id:" + strID, LogLevel.Info);
                        }
                    }
                    if (IsEventField)
                    {
                        titleBar.RaiseClickEvent();
                        Log.Logger.LogData("TitleBar control clicked:" + strID, LogLevel.Info);
                    }
                    window.WaitWhileBusy();
                    return titleBar.ToString();
                }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
                catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
                { UseNextPriority = true; }
                return string.Empty;

            }

            #region "Not Supported - do not delete - for future implementation"
            //else if (ctrl.Trim() == "ControlType.StatusBar")
            //{
            //    try
            //    {
            //        //                    TestStack.White.UIItems.Thumb thumb = window.Get<TestStack.White.UIItems.Thumb>(SearchCriteria.ByAutomationId(strID));
            //    }
            //    catch (Exception ex)
            //    { UseNextPriority = true; }
            //    return string.Empty;

            //}
            //else if ((ctrl.Trim() == "ControlType.ScrollBar"))
            //{
            //    try
            //    {
            //        //TestStack.White.UIItems.ProgressBar progressBar = window.Get<ScrollBar>(SearchCriteria.ByAutomationId(strID));
            //    }
            //    catch (Exception ex)
            //    { UseNextPriority = true; }
            //    return string.Empty;
            //}
            //else if (ctrl.Trim() == "ControlType.SplitButton")
            //{
            //    try
            //    {
            //         TestStack.White.UIItems.ListViewRow dataGridrow = window.Get<SplitButton>(SearchCriteria.ByAutomationId(strID));
            //    }
            //    catch (Exception ex)
            //    { UseNextPriority = true; }
            //    return string.Empty;

            //}
            //else if ((ctrl.Trim() == " ControlType.Document"))
            //{
            //    try
            //    {
            //        //TestStack.White.UIItems.Document document = window.Get<Document>(SearchCriteria.ByAutomationId(strID));
            //    }
            //    catch (Exception ex)
            //    { UseNextPriority = true; }
            //    return string.Empty;
            //}
            //else if ((ctrl.Trim() == "ControlType.Window"))
            //{
            //    try
            //    {

            //    }
            //    catch (Exception ex)
            //    { UseNextPriority = true; }
            //    return string.Empty;
            //}
            //else if ((ctrl.Trim() == "ControlType.Header"))
            //{
            //    try
            //    {

            //    }
            //    catch (Exception ex)
            //    { UseNextPriority = true; }
            //    return string.Empty;
            //}
            //else if (ctrl.Trim() == "ControlType.HeaderItem")
            //{
            //    try
            //    {
            //       // TestStack.White.UIItems.WindowItems.TitleBar titleBar = window.Get<HeaderItem>(SearchCriteria.ByAutomationId(strID));
            //    }
            //    catch (Exception ex)
            //    { UseNextPriority = true; }
            //    return string.Empty;

            //}
            //else if (ctrl.Trim() == "ControlType.Custom")
            //{
            //    try
            //    {
            //        //TestStack.White.UIItems.Custom groupBox = window.Get<TestStack.White.UIItems.Custom>(SearchCriteria.ByAutomationId(strID));
            //    }
            //    catch (Exception ex)
            //    { UseNextPriority = true; }
            //    return string.Empty;

            //}
            //else if ((ctrl.Trim() == "ControlType.Separator"))
            //{
            //    try
            //    {
            //        //TestStack.White.UIItems.WindowItems.TitleBar titleBar = window.Get<Separator>(SearchCriteria.ByAutomationId(strID));
            //        //titleBar.RaiseClickEvent();
            //    }
            //    catch (Exception ex)
            //    { UseNextPriority = true; }
            //    return string.Empty;
            //}
            #endregion

            else
            {
                return null;
            }

        }

        public static object TakeActionOnControlByName( Window window, string strName, string ctrl, bool IsEventField, bool Activate, bool ClickBeforeValueSet, bool ContinueOnError, string setValue, ref bool UseNextPriority)
        {
            UseNextPriority = false;
            if ((ctrl.Trim() == "ControlType.Button"))
            {
                try
                {
                    TestStack.White.UIItems.Button button = window.Get<TestStack.White.UIItems.Button>(SearchCriteria.ByText(strName));
                    if (Activate)
                    {
                        try
                        {
                            window.WaitWhileBusy();
                            button.Focus();
                            window.WaitWhileBusy();
                            Log.Logger.LogData("Button control activated:" + strName, LogLevel.Info);
                        }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
                        catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
                        {
                            Log.Logger.LogData("Error while setting focus on Button by Name: " + strName, LogLevel.Warning);
                        }
                        Log.Logger.LogData("Button control activated:" + strName, LogLevel.Info);
                    }
                    if (ClickBeforeValueSet)
                    {
                        try
                        {
                            window.WaitWhileBusy();
                            button.RaiseClickEvent();
                            window.WaitWhileBusy();
                            Log.Logger.LogData("Button control clicked:" + strName, LogLevel.Info);
                        }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
                        catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
                        {
                            Log.Logger.LogData("Error while click before value set on Button by Name: " + strName, LogLevel.Warning);
                        }
                    }
                    if (IsEventField)
                    {
                        window.WaitWhileBusy();
                        button.RaiseClickEvent();
                        window.WaitWhileBusy();
                        Log.Logger.LogData("Span control clicked:" + strName, LogLevel.Info);
                    }
                    Log.Logger.LogData("Span control value get:" + strName, LogLevel.Info);
                    return button.ToString();
                }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
                catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
                { UseNextPriority = true; }
                return string.Empty;
            }
            else if ((ctrl.Trim() == "ControlType.Edit"))
            {
                TestStack.White.UIItems.TextBox txt = window.Get<TestStack.White.UIItems.TextBox>(SearchCriteria.ByText(strName));
                try
                {
                    if (Activate)
                    {
                        try
                        {
                            window.WaitWhileBusy();
                            txt.Focus();
                            window.WaitWhileBusy();
                            Log.Logger.LogData("Edit control activated:" + strName, LogLevel.Info);
                        }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
                        catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
                        {
                            Log.Logger.LogData("Error while setting focus on Edit by Name: " + strName, LogLevel.Warning);
                        }
                        Log.Logger.LogData("Control activated:" + strName, LogLevel.Info);

                    }
                    if (ClickBeforeValueSet)
                    {
                        try
                        {
                            window.WaitWhileBusy();
                            txt.RaiseClickEvent();
                            window.WaitWhileBusy();
                        }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
                        catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
                        {
                            Log.Logger.LogData("Error while click before value set on Edit by Name: " + strName, LogLevel.Warning);
                        }
                    }
                    if (!string.IsNullOrEmpty(setValue))
                    {
                        window.WaitWhileBusy();
                        txt.Text = setValue;
                        window.WaitWhileBusy();
                        Log.Logger.LogData("Edit control value set:" + strName, LogLevel.Info);
                    }
                    if (IsEventField)
                    {
                        window.WaitWhileBusy();
                        txt.RaiseClickEvent();
                        window.WaitWhileBusy();
                        Log.Logger.LogData("Span control clicked:" + strName, LogLevel.Info);
                    }
                    Log.Logger.LogData("Edit control value get:" + strName, LogLevel.Info);
                    return txt.Text;

                }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
                catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
                { UseNextPriority = true; }
                return string.Empty;
            }
            else if ((ctrl.Trim() == "ControlType.Text"))
            {
                try
                {
                    TestStack.White.UIItems.Label label = window.Get<TestStack.White.UIItems.Label>(SearchCriteria.ByText(strName));
                    try
                    {
                        if (Activate)
                        {
                            try
                            {
                                window.WaitWhileBusy();
                                label.Focus();
                                window.WaitWhileBusy();
                                Log.Logger.LogData("Text control activated:" + strName, LogLevel.Info);
                            }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
                            catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
                            {
                                Log.Logger.LogData("Error while setting focus on Text: " + strName, LogLevel.Warning);
                            }
                            Log.Logger.LogData("Text control activated:" + strName, LogLevel.Info);

                        }
                        if (ClickBeforeValueSet)
                        {
                            try
                            {
                                window.WaitWhileBusy();
                                label.RaiseClickEvent();
                                window.WaitWhileBusy();
                            }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
                            catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
                            {
                                Log.Logger.LogData("Error while click before value set on Text by Name: " + strName, LogLevel.Warning);
                            }
                        }
                        if (!string.IsNullOrEmpty(setValue))
                        {
                            //label.vText = setValue;
                            Log.Logger.LogData("Control value can not be set for label:" + strName, LogLevel.Warning);
                        }
                        if (IsEventField)
                        {
                            window.WaitWhileBusy();
                            label.RaiseClickEvent();
                            window.WaitWhileBusy();
                            Log.Logger.LogData("Span control clicked:" + strName, LogLevel.Info);
                        }
                        Log.Logger.LogData("Text control value get:" + strName, LogLevel.Info);
                        return label.Text;
                    }
                    catch (Exception ex)
                    {
                        Log.Logger.LogData(ex.Message, LogLevel.Warning);
                        UseNextPriority = true;
                    }
                }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
                catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
                { UseNextPriority = true; }
                return string.Empty;
            }
            else if ((ctrl.Trim() == "ControlType.CheckBox"))
            {
                try
                {
                    TestStack.White.UIItems.CheckBox chk = window.Get<TestStack.White.UIItems.CheckBox>(SearchCriteria.ByText(strName));
                    window.WaitWhileBusy();
                    if (Activate)
                    {
                        try
                        {
                            window.WaitWhileBusy();
                            chk.Focus();
                            window.WaitWhileBusy();
                            Log.Logger.LogData("CheckBox control activated:" + strName, LogLevel.Info);
                        }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
                        catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
                        {
                            Log.Logger.LogData("Error while setting focus on CheckBox by Name: " + strName, LogLevel.Warning);
                        }
                        Log.Logger.LogData("Control activated:" + strName, LogLevel.Info);

                    }
                    if (ClickBeforeValueSet)
                    {
                        try
                        {
                            window.WaitWhileBusy();
                            chk.RaiseClickEvent();
                            window.WaitWhileBusy();
                        }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
                        catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
                        {
                            Log.Logger.LogData("Error while click before value set on CheckBox by Id " + strName, LogLevel.Warning);
                        }
                    }
                    if (!string.IsNullOrEmpty(setValue))
                    {
                        if ((setValue.ToLower().Trim() == "true") || (setValue.ToLower().Trim() == "select") || (setValue.ToLower().Trim() == "1"))
                        {
                            chk.Checked = true;
                        }
                        else
                        {
                            chk.Checked = false;
                        }
                    }
                    if (IsEventField)
                    {
                        chk.RaiseClickEvent();
                        Log.Logger.LogData("CheckBox control clicked:" + strName, LogLevel.Info);
                    }
                    window.WaitWhileBusy();
                    return chk.Checked.ToString();
                }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
                catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
                { UseNextPriority = true; }
                return string.Empty;
            }
            else if (ctrl.Trim() == "ControlType.RadioButton")
            {
                try
                {
                    TestStack.White.UIItems.RadioButton radioButton = window.Get<TestStack.White.UIItems.RadioButton>(SearchCriteria.ByText(strName));
                    window.WaitWhileBusy();
                    if (Activate)
                    {
                        try
                        {
                            radioButton.Focus();
                            Log.Logger.LogData("RadioButton control activated:" + strName, LogLevel.Info);
                        }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
                        catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
                        {
                            Log.Logger.LogData("Error while setting focus on RadioButton by Name: " + strName, LogLevel.Warning);
                        }
                        Log.Logger.LogData("Control activated:" + strName, LogLevel.Info);

                    }
                    if (ClickBeforeValueSet)
                    {
                        try
                        {
                            radioButton.RaiseClickEvent();
                        }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
                        catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
                        {
                            Log.Logger.LogData("Error while click before value set on RadioButton by Name: " + strName, LogLevel.Warning);
                        }
                    }
                    if (!string.IsNullOrEmpty(setValue))
                    {
                        if ((setValue.ToLower().Trim() == "true") || (setValue.ToLower().Trim() == "select") || (setValue.ToLower().Trim() == "1"))
                        {
                            radioButton.Select();
                        }
                        else
                        {
                            radioButton.Select();
                        }
                    }
                    if (IsEventField)
                    {
                        radioButton.RaiseClickEvent();
                        Log.Logger.LogData("RadioButton control clicked:" + strName, LogLevel.Info);
                    }
                    window.WaitWhileBusy();
                    return radioButton.IsSelected.ToString();
                }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
                catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
                { UseNextPriority = true; }
                return string.Empty;

            }
            else if (ctrl.Trim() == "ControlType.ComboBox")
            {
                try
                {
                    TestStack.White.UIItems.ListBoxItems.ComboBox comboBox = window.Get<TestStack.White.UIItems.ListBoxItems.ComboBox>(SearchCriteria.ByText(strName));
                    window.WaitWhileBusy();
                    if (Activate)
                    {
                        try
                        {
                            comboBox.Focus();
                            Log.Logger.LogData("Combobox Control activated:" + strName, LogLevel.Info);
                        }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
                        catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
                        {
                            Log.Logger.LogData("Error while setting focus on ComboBox by Name:" + strName, LogLevel.Warning);
                        }
                    }
                    if (ClickBeforeValueSet)
                    {
                        try
                        {
                            comboBox.RaiseClickEvent();
                        }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
                        catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
                        {
                            Log.Logger.LogData("Error while Click before combobox value set on Name: " + strName, LogLevel.Warning);
                        }
                    }
                    if (!string.IsNullOrEmpty(setValue))
                    {
                        comboBox.Select(setValue);
                        Log.Logger.LogData("combobox value selected:" + strName, LogLevel.Info);
                    }
                    if (IsEventField)
                    {
                        comboBox.RaiseClickEvent();
                        Log.Logger.LogData("combobox control clicked:" + strName, LogLevel.Info);
                    }
                    window.WaitWhileBusy();
                    return comboBox.SelectedItemText;
                }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
                catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
                {
                    Log.Logger.LogData("Error in ComboBox set value by Name:" + strName, LogLevel.Warning);
                    UseNextPriority = true;
                }
                return string.Empty;

            }
            else if ((ctrl.Trim() == " ControlType.Table"))
            {
                try
                {
                    TestStack.White.UIItems.TableItems.Table table = window.Get<TestStack.White.UIItems.TableItems.Table>(SearchCriteria.ByText(strName));

                    window.WaitWhileBusy();
                    if (Activate)
                    {
                        try
                        {
                            table.Focus();
                            Log.Logger.LogData("Table control activated:" + strName, LogLevel.Info);
                        }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
                        catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
                        {
                            Log.Logger.LogData("Error while setting focus on Table by Name: " + strName, LogLevel.Warning);
                        }
                    }
                    if (ClickBeforeValueSet)
                    {
                        try
                        {
                            table.RaiseClickEvent();
                        }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
                        catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
                        {
                            Log.Logger.LogData("Error while Click before Table value set on  by Name: " + strName, LogLevel.Warning);
                        }
                    }
                    if (!string.IsNullOrEmpty(setValue))
                    {
                        table.SetValue(setValue); //Need to send table object
                        Log.Logger.LogData("Table value selected:" + strName, LogLevel.Info);
                    }
                    if (IsEventField)
                    {
                        table.RaiseClickEvent();
                        Log.Logger.LogData("Table control clicked:" + strName, LogLevel.Info);
                    }
                    window.WaitWhileBusy();
                    return table;
                }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
                catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
                {
                    Log.Logger.LogData("Error in table set value by Name:" + strName, LogLevel.Warning);
                    UseNextPriority = true;
                }
                return string.Empty;
            }

            else if ((ctrl.Trim() == "ControlType.Image"))
            {
                try
                {
                    TestStack.White.UIItems.Image image = window.Get<TestStack.White.UIItems.Image>(SearchCriteria.ByText(strName));
                    window.WaitWhileBusy();
                    if (Activate)
                    {
                        try
                        {
                            image.Focus();
                            Log.Logger.LogData("Image Control activated:" + strName, LogLevel.Info);
                        }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
                        catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
                        {
                            Log.Logger.LogData("Error while setting focus on image:" + strName, LogLevel.Warning);
                        }
                    }
                    if (ClickBeforeValueSet)
                    {
                        try
                        {
                            image.RaiseClickEvent();
                        }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
                        catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
                        {
                            Log.Logger.LogData("Error while Click Before image Value set on Name: " + strName, LogLevel.Warning);
                        }
                    }
                    if (!string.IsNullOrEmpty(setValue))
                    {
                        image.SetValue(setValue); //Need to send table object
                        Log.Logger.LogData("Image value selected:" + strName, LogLevel.Info);
                    }
                    if (IsEventField)
                    {
                        image.RaiseClickEvent();
                        Log.Logger.LogData("Image control clicked:" + strName, LogLevel.Info);
                    }
                    window.WaitWhileBusy();
                    return image;
                }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
                catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
                {
                    Log.Logger.LogData("Error in image set value by Name:" + strName, LogLevel.Warning);
                    UseNextPriority = true;
                }
                return string.Empty;
            }
            else if (ctrl.Trim() == "ControlType.ListItem")
            {
                try
                {
                    TestStack.White.UIItems.ListBoxItems.ListItem listItem = window.Get<TestStack.White.UIItems.ListBoxItems.ListItem>(SearchCriteria.ByText(strName));
                    window.WaitWhileBusy();
                    if (Activate)
                    {
                        try
                        {
                            listItem.Focus();
                            Log.Logger.LogData("ListItem Control activated:" + strName, LogLevel.Info);
                        }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
                        catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
                        {
                            Log.Logger.LogData("Error while setting focus on ListItem:" + strName, LogLevel.Warning);
                        }
                    }
                    if (ClickBeforeValueSet)
                    {
                        try
                        {
                            listItem.RaiseClickEvent();
                        }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
                        catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
                        {
                            Log.Logger.LogData("Error while click before ListItem value set on Name: " + strName, LogLevel.Warning);
                        }
                    }
                    if (!string.IsNullOrEmpty(setValue))
                    {
                        listItem.SetValue(setValue); //Need to send table object
                        Log.Logger.LogData("ListItem value selected:" + strName, LogLevel.Info);
                    }
                    if (IsEventField)
                    {
                        listItem.RaiseClickEvent();
                        Log.Logger.LogData("ListItem control clicked:" + strName, LogLevel.Info);
                    }
                    window.WaitWhileBusy();
                    return listItem.Text;
                }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
                catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
                { UseNextPriority = true; }
                return string.Empty;

            }

            else if ((ctrl.Trim() == "ControlType.List"))
            {
                try
                {
                    TestStack.White.UIItems.ListBoxItems.ListBox comboBox = window.Get<TestStack.White.UIItems.ListBoxItems.ListBox>(SearchCriteria.ByText(strName));
                    window.WaitWhileBusy();
                    if (Activate)
                    {
                        try
                        {
                            comboBox.Focus();
                            Log.Logger.LogData("List control activated:" + strName, LogLevel.Info);
                        }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
                        catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
                        {
                            Log.Logger.LogData("Error while setting focus on List by Name:" + strName, LogLevel.Warning);
                        }
                    }
                    if (ClickBeforeValueSet)
                    {
                        try
                        {
                            comboBox.RaiseClickEvent();
                        }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
                        catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
                        {
                            Log.Logger.LogData("Error while Click before List value set on Id:  " + strName, LogLevel.Warning);
                        }
                    }
                    if (!string.IsNullOrEmpty(setValue))
                    {
                        comboBox.SetValue(setValue);
                        Log.Logger.LogData("List value selected:" + strName, LogLevel.Info);
                    }
                    if (IsEventField)
                    {
                        comboBox.RaiseClickEvent();
                        Log.Logger.LogData("List control clicked:" + strName, LogLevel.Info);
                    }
                    window.WaitWhileBusy();
                    return comboBox.SelectedItemText;
                }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
                catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
                { UseNextPriority = true; }
                return string.Empty;
            }
            else if (ctrl.Trim() == "ControlType.Menu")
            {
                try
                {
                    TestStack.White.UIItems.MenuItems.Menu menuItem = window.Get<TestStack.White.UIItems.MenuItems.Menu>(SearchCriteria.ByText(strName));

                    window.WaitWhileBusy();
                    if (Activate)
                    {
                        try
                        {
                            menuItem.Focus();
                            Log.Logger.LogData("List control activated:" + strName, LogLevel.Info);
                        }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
                        catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
                        {
                            Log.Logger.LogData("Error while setting focus on Menu by Name: " + strName, LogLevel.Warning);
                        }
                    }
                    if (ClickBeforeValueSet)
                    {
                        try
                        {
                            menuItem.RaiseClickEvent();
                        }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
                        catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
                        {
                            Log.Logger.LogData("Error while click before Menu value set on Name: " + strName, LogLevel.Warning);
                        }
                    }
                    if (!string.IsNullOrEmpty(setValue))
                    {
                        menuItem.SetValue(setValue);
                        Log.Logger.LogData("Menu value selected:" + strName, LogLevel.Info);
                    }
                    if (IsEventField)
                    {
                        menuItem.RaiseClickEvent();
                        Log.Logger.LogData("Menu control clicked:" + strName, LogLevel.Info);
                    }
                    window.WaitWhileBusy();
                    return menuItem.ToString();
                }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
                catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
                { UseNextPriority = true; }
                return string.Empty;

            }

            else if ((ctrl.Trim() == "ControlType.MenuBar"))
            {
                try
                {
                    TestStack.White.UIItems.WindowStripControls.MenuBar menuBar = window.Get<TestStack.White.UIItems.WindowStripControls.MenuBar>(SearchCriteria.ByText(strName));

                    window.WaitWhileBusy();
                    if (Activate)
                    {
                        try
                        {
                            menuBar.Focus();
                            Log.Logger.LogData("MenuBar Control activated:" + strName, LogLevel.Info);
                        }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
                        catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
                        {
                            Log.Logger.LogData("Error while setting focus on MenuBar by Name: " + strName, LogLevel.Warning);
                        }
                    }
                    if (ClickBeforeValueSet)
                    {
                        try
                        {
                            menuBar.RaiseClickEvent();
                        }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
                        catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
                        {
                            Log.Logger.LogData("Error while Click before MenuBar value set on Name: " + strName, LogLevel.Warning);
                        }
                    }
                    if (!string.IsNullOrEmpty(setValue))
                    {
                        menuBar.SetValue(setValue);
                        Log.Logger.LogData("MenuBar value selected:" + strName, LogLevel.Info);
                    }
                    if (IsEventField)
                    {
                        menuBar.RaiseClickEvent();
                        Log.Logger.LogData("MenuBar control clicked:" + strName, LogLevel.Info);
                    }
                    window.WaitWhileBusy();
                    return menuBar.ToString();
                }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
                catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
                { UseNextPriority = true; }
                return string.Empty;
            }
            else if (ctrl.Trim() == "ControlType.MenuItem")
            {
                try
                {
                    TestStack.White.UIItems.MenuItems.Menu menuItem = window.Get<TestStack.White.UIItems.MenuItems.Menu>(SearchCriteria.ByText(strName));

                    window.WaitWhileBusy();
                    if (Activate)
                    {
                        try
                        {
                            menuItem.Focus();
                            Log.Logger.LogData("MenuItem control activated:" + strName, LogLevel.Info);
                        }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
                        catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
                        {
                            Log.Logger.LogData("Error while setting focus on MenuItem by Name:" + strName, LogLevel.Warning);
                        }
                    }
                    if (ClickBeforeValueSet)
                    {
                        try
                        {
                            menuItem.RaiseClickEvent();
                        }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
                        catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
                        {
                            Log.Logger.LogData("Error while Click before MenuItem value set on Name: " + strName, LogLevel.Warning);
                        }
                    }
                    if (!string.IsNullOrEmpty(setValue))
                    {
                        menuItem.SetValue(setValue);
                        Log.Logger.LogData("MenuItem value selected:" + strName, LogLevel.Info);
                    }
                    if (IsEventField)
                    {
                        menuItem.Click();// RaiseClickEvent();
                        Log.Logger.LogData("MenuItem control clicked:" + strName, LogLevel.Info);
                    }
                    window.WaitWhileBusy();
                    return menuItem.ToString();
                }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
                catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
                { UseNextPriority = true; }
                return string.Empty;

            }
            else if ((ctrl.Trim() == "ControlType.Calendar") || (ctrl.Trim() == "ControlType.Pane"))
            {
                try
                {
                    TestStack.White.UIItems.DateTimePicker sDate = window.Get<TestStack.White.UIItems.DateTimePicker>(SearchCriteria.ByText(strName));
                    window.WaitWhileBusy();
                    if (sDate != null)
                    {
                        if (Activate)
                        {
                            try
                            {
                                sDate.Focus();
                                Log.Logger.LogData("Calendar control activated:" + strName, LogLevel.Info);
                            }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
                            catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
                            {
                                Log.Logger.LogData("Error while setting focus on Calendar by Name:" + strName, LogLevel.Warning);
                            }
                        }
                        if (ClickBeforeValueSet)
                        {
                            try
                            {
                                sDate.RaiseClickEvent();
                            }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
                            catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
                            {
                                Log.Logger.LogData("Error while click before Calendar value set on Name: " + strName, LogLevel.Warning);
                            }
                        }
                        if (!string.IsNullOrEmpty(setValue))
                        {
                            System.DateTime dateTime = Convert.ToDateTime(setValue);
                            sDate.SetDate(dateTime, TestStack.White.UIItems.DateFormat.MonthDayYear);
                            Log.Logger.LogData("Calendar value selected:" + strName, LogLevel.Info);
                        }
                        if (IsEventField)
                        {
                            sDate.RaiseClickEvent();
                            Log.Logger.LogData("Calendar control clicked:" + strName, LogLevel.Info);
                        }
                        window.WaitWhileBusy();
                        return sDate.Date.ToString();// ToLongDateString();
                    }
                    else
                    {
                        TestStack.White.UIItems.PropertyGridItems.PropertyGrid propertyGrid = window.Get<TestStack.White.UIItems.PropertyGridItems.PropertyGrid>(SearchCriteria.ByText(strName));
                        window.WaitWhileBusy();
                        if (propertyGrid != null)
                        {

                            if (Activate)
                            {
                                try
                                {
                                    propertyGrid.RaiseClickEvent();
                                    Log.Logger.LogData("PropertyGrid control activated:" + strName, LogLevel.Info);
                                }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
                                catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
                                {
                                    Log.Logger.LogData("Error while setting focus on PropertyGrid by Name: " + strName, LogLevel.Warning);
                                }
                            }
                            if (ClickBeforeValueSet)
                            {
                                try
                                {
                                    propertyGrid.RaiseClickEvent();
                                }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
                                catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
                                {
                                    Log.Logger.LogData("Error while Click before PropertyGrid value set on Name: " + strName, LogLevel.Warning);
                                }
                            }
                            if (!string.IsNullOrEmpty(setValue))
                            {
                                try
                                {
                                    propertyGrid.SetValue(setValue);
                                    Log.Logger.LogData("PropertyGrid value selected:" + strName, LogLevel.Info);
                                }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
                                catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
                                {
                                    Log.Logger.LogData("Error in propertyGrid set value by Name:" + strName, LogLevel.Info);
                                }
                            }
                            if (IsEventField)
                            {
                                propertyGrid.RaiseClickEvent();
                                Log.Logger.LogData("PropertyGrid control clicked:" + strName, LogLevel.Info);
                            }
                            window.WaitWhileBusy();
                            return propertyGrid;
                        }
                    }
                    window.WaitWhileBusy();
                }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
                catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
                { UseNextPriority = true; }
                return string.Empty;

            }

            else if (ctrl.Trim() == "ControlType.Hyperlink")
            {
                try
                {
                    TestStack.White.UIItems.Hyperlink hlink = window.Get<TestStack.White.UIItems.Hyperlink>(SearchCriteria.ByText(strName));
                    window.WaitWhileBusy();
                    if (Activate)
                    {
                        try
                        {
                            hlink.RaiseClickEvent();
                            Log.Logger.LogData("Hyperlink Control activated:" + strName, LogLevel.Info);
                        }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
                        catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
                        {
                            Log.Logger.LogData("Error while setting focus on Hyperlink by Name:" + strName, LogLevel.Warning);
                        }
                    }
                    if (ClickBeforeValueSet)
                    {
                        try
                        {
                            hlink.RaiseClickEvent();
                        }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
                        catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
                        {
                            Log.Logger.LogData("Error while Click before Hyperlink value set on Name: " + strName, LogLevel.Warning);
                        }
                    }
                    if (!string.IsNullOrEmpty(setValue))
                    {
                        try
                        {
                            hlink.SetValue(setValue);
                            Log.Logger.LogData("Hyperlink value selected:" + strName, LogLevel.Info);
                        }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
                        catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
                        {
                            Log.Logger.LogData("Error in Hyperlink set value by Name:" + strName, LogLevel.Info);
                        }
                    }
                    if (IsEventField)
                    {
                        hlink.RaiseClickEvent();
                        Log.Logger.LogData("Hyperlink control clicked:" + strName, LogLevel.Info);
                    }
                    window.WaitWhileBusy();
                    return hlink.ToString();
                }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
                catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
                { UseNextPriority = true; }
                return string.Empty;

            }

            else if ((ctrl.Trim() == " ControlType.Tab"))
            {
                try
                {
                    TestStack.White.UIItems.TabItems.Tab tab = window.Get<TestStack.White.UIItems.TabItems.Tab>(SearchCriteria.ByText(strName));
                    window.WaitWhileBusy();
                    if (Activate)
                    {
                        try
                        {
                            tab.RaiseClickEvent();
                            Log.Logger.LogData("Tab control activated:" + strName, LogLevel.Info);
                        }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
                        catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
                        {
                            Log.Logger.LogData("Error while setting focus on Tab by Name: " + strName, LogLevel.Warning);
                        }
                    }
                    if (ClickBeforeValueSet)
                    {
                        try
                        {
                            tab.RaiseClickEvent();
                        }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
                        catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
                        {
                            Log.Logger.LogData("Error while Click before Tab value set on Name: " + strName, LogLevel.Warning);
                        }
                    }
                    if (!string.IsNullOrEmpty(setValue))
                    {
                        try
                        {
                            tab.SetValue(setValue);
                            Log.Logger.LogData("Tab value selected:" + strName, LogLevel.Info);
                        }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
                        catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
                        {
                            Log.Logger.LogData("Error in Tab set value by Name:" + strName, LogLevel.Info);
                        }
                    }
                    if (IsEventField)
                    {
                        tab.RaiseClickEvent();
                        Log.Logger.LogData("Tab control clicked:" + strName, LogLevel.Info);
                    }
                    window.WaitWhileBusy();
                    return tab.ToString();
                }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
                catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
                { UseNextPriority = true; }
                return string.Empty;
            }
            else if (ctrl.Trim() == "ControlType.TabItem")
            {
                try
                {
                    TestStack.White.UIItems.TabItems.TabPage tabPage = window.Get<TestStack.White.UIItems.TabItems.TabPage>(SearchCriteria.ByText(strName));
                    window.WaitWhileBusy();
                    if (Activate)
                    {
                        try
                        {
                            tabPage.RaiseClickEvent();
                            Log.Logger.LogData("TabItem control activated:" + strName, LogLevel.Info);
                        }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
                        catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
                        {
                            Log.Logger.LogData("Error while setting focus on TabItem by Name: " + strName, LogLevel.Warning);
                        }
                    }
                    if (ClickBeforeValueSet)
                    {
                        try
                        {
                            tabPage.RaiseClickEvent();
                        }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
                        catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
                        {
                            Log.Logger.LogData("Error while click before TabItem value set on Name: " + strName, LogLevel.Warning);
                        }
                    }
                    if (!string.IsNullOrEmpty(setValue))
                    {
                        try
                        {
                            tabPage.SetValue(setValue);
                            Log.Logger.LogData("TabItem value selected:" + strName, LogLevel.Info);
                        }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
                        catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
                        {
                            Log.Logger.LogData("Error in TabItem set value by Name:" + strName, LogLevel.Info);
                        }
                    }
                    if (IsEventField)
                    {
                        tabPage.RaiseClickEvent();
                        Log.Logger.LogData("TabItem control clicked:" + strName, LogLevel.Info);
                    }
                    window.WaitWhileBusy();
                    return tabPage.ToString();
                }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
                catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
                { UseNextPriority = true; }
                return string.Empty;

            }
            else if ((ctrl.Trim() == "ControlType.DataGrid"))
            {
                try
                {
                    TestStack.White.UIItems.ListView dataGrid = window.Get<TestStack.White.UIItems.ListView>(SearchCriteria.ByText(strName));

                    window.WaitWhileBusy();
                    if (Activate)
                    {
                        try
                        {
                            dataGrid.RaiseClickEvent();
                            Log.Logger.LogData("DataGrid control activated:" + strName, LogLevel.Info);
                        }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
                        catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
                        {
                            Log.Logger.LogData("Error while setting focus on DataGrid by Name: " + strName, LogLevel.Warning);
                        }
                    }
                    if (ClickBeforeValueSet)
                    {
                        try
                        {
                            dataGrid.RaiseClickEvent();
                        }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
                        catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
                        {
                            Log.Logger.LogData("Error while Click before List value set on Name: " + strName, LogLevel.Warning);
                        }
                    }
                    if (!string.IsNullOrEmpty(setValue))
                    {
                        try
                        {
                            dataGrid.SetValue(setValue);
                            Log.Logger.LogData("List value selected:" + strName, LogLevel.Info);
                        }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
                        catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
                        {
                            Log.Logger.LogData("Error in propertyGrid set value by Name:" + strName, LogLevel.Info);
                        }
                    }
                    if (IsEventField)
                    {
                        dataGrid.RaiseClickEvent();
                        Log.Logger.LogData("List control clicked:" + strName, LogLevel.Info);
                    }
                    window.WaitWhileBusy();
                    return dataGrid;
                }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
                catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
                { UseNextPriority = true; }
                return string.Empty;
            }
            else if (ctrl.Trim() == "ControlType.DataItem")
            {
                try
                {
                    TestStack.White.UIItems.ListViewRow dataGridrow = window.Get<TestStack.White.UIItems.ListViewRow>(SearchCriteria.ByText(strName));

                    window.WaitWhileBusy();
                    if (Activate)
                    {
                        try
                        {
                            dataGridrow.RaiseClickEvent();
                            Log.Logger.LogData("List control activated:" + strName, LogLevel.Info);
                        }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
                        catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
                        {
                            Log.Logger.LogData("Error while setting focus on List by Name:" + strName, LogLevel.Warning);
                        }
                    }
                    if (ClickBeforeValueSet)
                    {
                        try
                        {
                            dataGridrow.RaiseClickEvent();
                        }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
                        catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
                        {
                            Log.Logger.LogData("Error while Click before List value set on Name: " + strName, LogLevel.Warning);
                        }
                    }
                    if (!string.IsNullOrEmpty(setValue))
                    {
                        try
                        {
                            dataGridrow.SetValue(setValue);
                            Log.Logger.LogData("List value selected:" + strName, LogLevel.Info);
                        }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
                        catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
                        {
                            Log.Logger.LogData("Error in propertyGrid set value by Name:" + strName, LogLevel.Info);
                        }
                    }
                    if (IsEventField)
                    {
                        dataGridrow.RaiseClickEvent();
                        Log.Logger.LogData("List control clicked:" + strName, LogLevel.Info);
                    }
                    window.WaitWhileBusy();
                    return dataGridrow;
                }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
                catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
                { UseNextPriority = true; }
                return string.Empty;

            }
            else if ((ctrl.Trim() == " ControlType.TreeItem") || (ctrl.Trim() == "ControlType.Tree"))
            {
                try
                {
                    TestStack.White.UIItems.TreeItems.TreeNode nodeOne = window.Get<TestStack.White.UIItems.TreeItems.TreeNode>(SearchCriteria.ByText(strName));
                    //nodeOne.Select();
                    //nodeOne.Expand();
                    window.WaitWhileBusy();
                    if (Activate)
                    {
                        try
                        {
                            nodeOne.RaiseClickEvent();
                            Log.Logger.LogData("TreeNode control activated:" + strName, LogLevel.Info);
                        }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
                        catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
                        {
                            Log.Logger.LogData("Error while setting focus on TreeNode by Name: " + strName, LogLevel.Warning);
                        }
                    }
                    if (ClickBeforeValueSet)
                    {
                        try
                        {
                            nodeOne.RaiseClickEvent();
                        }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
                        catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
                        {
                            Log.Logger.LogData("Error while Click before TreeNode value set on Name: " + strName, LogLevel.Warning);
                        }
                    }
                    if (!string.IsNullOrEmpty(setValue))
                    {
                        try
                        {
                            nodeOne.Select();
                            Log.Logger.LogData("TreeNode value selected:" + strName, LogLevel.Info);
                        }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
                        catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
                        {
                            Log.Logger.LogData("Error in TreeNode set value by Name:" + strName, LogLevel.Info);
                        }
                    }
                    if (IsEventField)
                    {
                        nodeOne.RaiseClickEvent();
                        Log.Logger.LogData("TreeNode control clicked:" + strName, LogLevel.Info);
                    }
                    window.WaitWhileBusy();
                    return nodeOne.Text;
                }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
                catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
                { UseNextPriority = true; }
                return string.Empty;
            }
            else if ((ctrl.Trim() == " ControlType.ProgressBar"))
            {
                try
                {
                    TestStack.White.UIItems.ProgressBar progressBar = window.Get<TestStack.White.UIItems.ProgressBar>(SearchCriteria.ByText(strName));
                    progressBar.RaiseClickEvent();
                    do
                    {
                        Thread.Sleep(5000);
                    } while (progressBar.Value != progressBar.Maximum);

                }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
                catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
                { UseNextPriority = true; }
                return string.Empty;
            }

            else if (ctrl.Trim() == "ControlType.Slider")
            {
                try
                {
                    TestStack.White.UIItems.Slider sldrOne = window.Get<TestStack.White.UIItems.Slider>(SearchCriteria.ByText(strName));

                    sldrOne.LargeIncrementButton.RaiseClickEvent();
                    //   sldrOne.LargeDecrementButton.RaiseClickEvent();
                }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
                catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
                { UseNextPriority = true; }
                return string.Empty;

            }

            else if ((ctrl.Trim() == "ControlType.Spinner"))
            {
                try
                {
                    TestStack.White.UIItems.Spinner spinner = window.Get<TestStack.White.UIItems.Spinner>(SearchCriteria.ByText(strName));
                    window.WaitWhileBusy();
                    if (Activate)
                    {
                        try
                        {
                            spinner.RaiseClickEvent();
                            Log.Logger.LogData("Spinner control activated:" + strName, LogLevel.Info);
                        }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
                        catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
                        {
                            Log.Logger.LogData("Error while setting focus on Spinner by Name: " + strName, LogLevel.Warning);
                        }
                    }
                    if (ClickBeforeValueSet)
                    {
                        try
                        {
                            spinner.RaiseClickEvent();
                        }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
                        catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
                        {
                            Log.Logger.LogData("Error while Click before Spinner value set on Name: " + strName, LogLevel.Warning);
                        }
                    }
                    if (!string.IsNullOrEmpty(setValue))
                    {
                        try
                        {
                            spinner.SetValue(setValue);
                            Log.Logger.LogData("Spinner value selected:" + strName, LogLevel.Info);
                        }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
                        catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
                        {
                            Log.Logger.LogData("Error in Spinner set value by Name:" + strName, LogLevel.Info);
                        }
                    }
                    if (IsEventField)
                    {
                        spinner.RaiseClickEvent();
                        Log.Logger.LogData("Spinner control clicked:" + strName, LogLevel.Info);
                    }
                    window.WaitWhileBusy();
                    return spinner.Value.ToString();
                }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
                catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
                { UseNextPriority = true; }
                return string.Empty;
            }

            else if (ctrl.Trim() == "ControlType.ToolBar")
            {
                try
                {
                    TestStack.White.UIItems.WindowStripControls.ToolStrip toolStrip = window.Get<TestStack.White.UIItems.WindowStripControls.ToolStrip>(SearchCriteria.ByText(strName));
                    window.WaitWhileBusy();
                    if (Activate)
                    {
                        try
                        {
                            toolStrip.RaiseClickEvent();
                            Log.Logger.LogData("ToolBar control activated:" + strName, LogLevel.Info);
                        }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
                        catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
                        {
                            Log.Logger.LogData("Error while setting focus on ToolBar by Name: " + strName, LogLevel.Warning);
                        }
                    }
                    if (ClickBeforeValueSet)
                    {
                        try
                        {
                            toolStrip.RaiseClickEvent();
                        }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
                        catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
                        {
                            Log.Logger.LogData("Error while Click before ToolBar value set on Name: " + strName, LogLevel.Warning);
                        }
                    }
                    if (!string.IsNullOrEmpty(setValue))
                    {
                        try
                        {
                            toolStrip.SetValue(setValue);
                            Log.Logger.LogData("ToolBar value selected:" + strName, LogLevel.Info);
                        }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
                        catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
                        {
                            Log.Logger.LogData("Error in ToolBar set value by Name:" + strName, LogLevel.Info);
                        }
                    }
                    if (IsEventField)
                    {
                        toolStrip.RaiseClickEvent();
                        Log.Logger.LogData("ToolBar control clicked:" + strName, LogLevel.Info);
                    }
                    window.WaitWhileBusy();
                    return toolStrip.ToString();
                }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
                catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
                { UseNextPriority = true; }
                return string.Empty;

            }

            else if ((ctrl.Trim() == "ControlType.ToolTip"))
            {
                try
                {
                    TestStack.White.UIItems.ToolTip toolTip = window.Get<TestStack.White.UIItems.ToolTip>(SearchCriteria.ByText(strName));
                    window.WaitWhileBusy();
                    if (Activate)
                    {
                        try
                        {
                            toolTip.RaiseClickEvent();
                            Log.Logger.LogData("ToolTip control activated:" + strName, LogLevel.Info);
                        }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
                        catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
                        {
                            Log.Logger.LogData("Error while setting focus on ToolTip by Name:" + strName, LogLevel.Warning);
                        }
                    }
                    if (ClickBeforeValueSet)
                    {
                        try
                        {
                            toolTip.RaiseClickEvent();
                        }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
                        catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
                        {
                            Log.Logger.LogData("Error while Click before ToolTip value set on Name: " + strName, LogLevel.Warning);
                        }
                    }
                    if (!string.IsNullOrEmpty(setValue))
                    {
                        try
                        {
                            toolTip.SetValue(setValue);
                            Log.Logger.LogData("ToolTip value selected:" + strName, LogLevel.Info);
                        }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
                        catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
                        {
                            Log.Logger.LogData("Error in ToolTip set value by Name:" + strName, LogLevel.Info);
                        }
                    }
                    if (IsEventField)
                    {
                        toolTip.RaiseClickEvent();
                        Log.Logger.LogData("ToolTip control clicked:" + strName, LogLevel.Info);
                    }
                    window.WaitWhileBusy();
                    return toolTip.Text;

                }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
                catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
                { UseNextPriority = true; }
                return string.Empty;
            }
            else if ((ctrl.Trim() == "ControlType.Group"))
            {
                try
                {
                    TestStack.White.UIItems.GroupBox groupBox = window.Get<TestStack.White.UIItems.GroupBox>(SearchCriteria.ByText(strName));

                    window.WaitWhileBusy();
                    if (Activate)
                    {
                        try
                        {
                            groupBox.RaiseClickEvent();
                            Log.Logger.LogData("Group control activated:" + strName, LogLevel.Info);
                        }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
                        catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
                        {
                            Log.Logger.LogData("Error while setting focus on Group by Name:" + strName, LogLevel.Warning);
                        }
                    }
                    if (ClickBeforeValueSet)
                    {
                        try
                        {
                            groupBox.RaiseClickEvent();
                        }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
                        catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
                        {
                            Log.Logger.LogData("Error while Click before Group value set on Name: " + strName, LogLevel.Warning);
                        }
                    }
                    if (!string.IsNullOrEmpty(setValue))
                    {
                        try
                        {
                            groupBox.SetValue(setValue);
                            Log.Logger.LogData("Group value selected:" + strName, LogLevel.Info);
                        }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
                        catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
                        {
                            Log.Logger.LogData("Error in Group set value by Name:" + strName, LogLevel.Info);
                        }
                    }
                    if (IsEventField)
                    {
                        groupBox.RaiseClickEvent();
                        Log.Logger.LogData("Group control clicked:" + strName, LogLevel.Info);
                    }
                    window.WaitWhileBusy();
                    return groupBox.ToString();
                }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
                catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
                { UseNextPriority = true; }
                return string.Empty;
            }
            else if (ctrl.Trim() == "ControlType.Thumb")
            {
                try
                {
                    TestStack.White.UIItems.Thumb thumb = window.Get<TestStack.White.UIItems.Thumb>(SearchCriteria.ByText(strName));

                    window.WaitWhileBusy();
                    if (Activate)
                    {
                        try
                        {
                            thumb.RaiseClickEvent();
                            Log.Logger.LogData("Thumb control activated:" + strName, LogLevel.Info);
                        }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
                        catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
                        {
                            Log.Logger.LogData("Error while setting focus on Thumb by Name:" + strName, LogLevel.Warning);
                        }
                    }
                    if (ClickBeforeValueSet)
                    {
                        try
                        {
                            thumb.RaiseClickEvent();
                        }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
                        catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
                        {
                            Log.Logger.LogData("Error while Click before Thumb value set on Name: " + strName, LogLevel.Warning);
                        }
                    }
                    if (!string.IsNullOrEmpty(setValue))
                    {
                        try
                        {
                            thumb.SetValue(setValue);
                            Log.Logger.LogData("Thumb value selected:" + strName, LogLevel.Info);
                        }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
                        catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
                        {
                            Log.Logger.LogData("Error in Thumb set value by Name:" + strName, LogLevel.Info);
                        }
                    }
                    if (IsEventField)
                    {
                        thumb.RaiseClickEvent();
                        Log.Logger.LogData("Thumb control clicked:" + strName, LogLevel.Info);
                    }
                    window.WaitWhileBusy();
                    return thumb.ToString();
                }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
                catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
                { UseNextPriority = true; }
                return string.Empty;

            }
            else if (ctrl.Trim() == "ControlType.TitleBar")
            {
                try
                {
                    TestStack.White.UIItems.WindowItems.TitleBar titleBar = window.Get<TestStack.White.UIItems.WindowItems.TitleBar>(SearchCriteria.ByText(strName));

                    window.WaitWhileBusy();
                    if (Activate)
                    {
                        try
                        {
                            titleBar.RaiseClickEvent();
                            Log.Logger.LogData("TitleBar control activated:" + strName, LogLevel.Info);
                        }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
                        catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
                        {
                            Log.Logger.LogData("Error while setting focus on TitleBar by Name:" + strName, LogLevel.Warning);
                        }
                    }
                    if (ClickBeforeValueSet)
                    {
                        try
                        {
                            titleBar.RaiseClickEvent();
                        }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
                        catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
                        {
                            Log.Logger.LogData("Error while Click before TitleBar value set on Name: " + strName, LogLevel.Warning);
                        }
                    }
                    if (!string.IsNullOrEmpty(setValue))
                    {
                        try
                        {
                            titleBar.SetValue(setValue);
                            Log.Logger.LogData("TitleBar value selected:" + strName, LogLevel.Info);
                        }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
                        catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
                        {
                            Log.Logger.LogData("Error in TitleBar set value by Name:" + strName, LogLevel.Info);
                        }
                    }
                    if (IsEventField)
                    {
                        titleBar.RaiseClickEvent();
                        Log.Logger.LogData("TitleBar control clicked:" + strName, LogLevel.Info);
                    }
                    window.WaitWhileBusy();
                    return titleBar.ToString();
                }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
                catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
                { UseNextPriority = true; }
                return string.Empty;

            }

            #region "Not Supported - do not delete - for future implementation"
            //else if (ctrl.Trim() == "ControlType.StatusBar")
            //{
            //    try
            //    {
            //        //                    TestStack.White.UIItems.Thumb thumb = window.Get<TestStack.White.UIItems.Thumb>(SearchCriteria.ByText(strName));
            //    }
            //    catch (Exception ex)
            //    { UseNextPriority = true; }
            //    return string.Empty;

            //}
            //else if ((ctrl.Trim() == "ControlType.ScrollBar"))
            //{
            //    try
            //    {
            //        //TestStack.White.UIItems.ProgressBar progressBar = window.Get<ScrollBar>(SearchCriteria.ByText(strName));
            //    }
            //    catch (Exception ex)
            //    { UseNextPriority = true; }
            //    return string.Empty;
            //}
            //else if (ctrl.Trim() == "ControlType.SplitButton")
            //{
            //    try
            //    {
            //         TestStack.White.UIItems.ListViewRow dataGridrow = window.Get<SplitButton>(SearchCriteria.ByText(strName));
            //    }
            //    catch (Exception ex)
            //    { UseNextPriority = true; }
            //    return string.Empty;

            //}
            //else if ((ctrl.Trim() == " ControlType.Document"))
            //{
            //    try
            //    {
            //        //TestStack.White.UIItems.Document document = window.Get<Document>(SearchCriteria.ByText(strName));
            //    }
            //    catch (Exception ex)
            //    { UseNextPriority = true; }
            //    return string.Empty;
            //}
            //else if ((ctrl.Trim() == "ControlType.Window"))
            //{
            //    try
            //    {

            //    }
            //    catch (Exception ex)
            //    { UseNextPriority = true; }
            //    return string.Empty;
            //}
            //else if ((ctrl.Trim() == "ControlType.Header"))
            //{
            //    try
            //    {

            //    }
            //    catch (Exception ex)
            //    { UseNextPriority = true; }
            //    return string.Empty;
            //}
            //else if (ctrl.Trim() == "ControlType.HeaderItem")
            //{
            //    try
            //    {
            //       // TestStack.White.UIItems.WindowItems.TitleBar titleBar = window.Get<HeaderItem>(SearchCriteria.ByText(strName));
            //    }
            //    catch (Exception ex)
            //    { UseNextPriority = true; }
            //    return string.Empty;

            //}
            //else if (ctrl.Trim() == "ControlType.Custom")
            //{
            //    try
            //    {
            //        //TestStack.White.UIItems.Custom groupBox = window.Get<TestStack.White.UIItems.Custom>(SearchCriteria.ByText(strName));
            //    }
            //    catch (Exception ex)
            //    { UseNextPriority = true; }
            //    return string.Empty;

            //}
            //else if ((ctrl.Trim() == "ControlType.Separator"))
            //{
            //    try
            //    {
            //        //TestStack.White.UIItems.WindowItems.TitleBar titleBar = window.Get<Separator>(SearchCriteria.ByText(strName));
            //        //titleBar.RaiseClickEvent();
            //    }
            //    catch (Exception ex)
            //    { UseNextPriority = true; }
            //    return string.Empty;
            //}
            #endregion

            else
            {
                return null;
            }
        }

        public static object TakeActionOnControlByClassName(TestStack.White.UIItems.WindowItems.Window window, string strClassName, string ctrl, bool IsEventField, bool Activate, bool ClickBeforeValueSet, bool ContinueOnError, string setValue, ref bool UseNextPriority)
        {
            return null;
        }
        public static object TakeActionOnControl(TestStack.White.UIItems.WindowItems.Window window, TestStack.White.UIItems.UIItem item, string ctrl, bool IsEventField, bool Activate, bool ClickBeforeValueSet, bool ContinueOnError, string setValue, ref bool UseNextPriority)
        {
            //PropertyCondition cond1 =new PropertyCondition(AutomationElement.ControlTypeProperty,ControlType.Table);
            //PropertyCondition cond2 = new PropertyCondition(AutomationElement.IsTablePatternAvailableProperty,true);
            //AndCondition tableCondition = new AndCondition(cond1, cond2);

            //AutomationElement targetTableElement = window.FindFirst(TreeScope.Descendants, tableCondition);

            // If targetTableElement is null then a suitable table control 
            // was not found.
            //return targetTableElement;

       //     TestStack.White. CustomUIItem customControl = item as CustomUIItem;
       //     IUIItem[] items = customControl.GetMultiple(SearchCriteria.All);

       //     if ((ctrl.Trim().ToLower().Contains("item")))
       //     {
       //         TestStack.White.UIItems.UIItem tableitem = (TestStack.White.UIItems.UIItem)(item);
       //         if (Activate)
       //         {
       //             try
       //             {
       //                 window.WaitWhileBusy();
       //                 tableitem.Focus();
       //                 window.WaitWhileBusy();
       //             }
       //             catch (Exception ex)
       //             {
       //                 Log.Logger.LogData("Exception: ITEM not Found: " + ex.Message, LogLevel.Error);
       //             }
       //         }
       //         if (ClickBeforeValueSet)
       //         {
       //             try
       //             {
       //                 window.WaitWhileBusy();
       //                 tableitem.RaiseClickEvent();
       //                 window.WaitWhileBusy();
       //             }
       //             catch (Exception ex)
       //             {
       //                 Log.Logger.LogData("Exception: ITEM Click Before not Found: " + ex.Message, LogLevel.Error);
       //             }
       //         }
       //         if (IsEventField)
       //         {
       //             window.WaitWhileBusy();
       //             tableitem.RaiseClickEvent();
       //             window.WaitWhileBusy();
       //             //Log.Logger.LogData("Span control clicked:" + strName, LogLevel.Info);
       //         }
       //         return tableitem.ToString();
       //     }
       //     //    catch (Exception ex)
       //     //{ UseNextPriority = true; }
       //     //return string.Empty;
       //// }

                //UseNextPriority = false;
            if ((ctrl.Trim().ToLower().Contains("button")))
            {
                try
                {
                    TestStack.White.UIItems.Button button = (TestStack.White.UIItems.Button)(item);
                    if (Activate)
                    {
                        try
                        {
                            window.WaitWhileBusy();
                            button.Focus();
                            window.WaitWhileBusy();
                            //Log.Logger.LogData("Button control activated:" + strName, LogLevel.Info);
                        }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
                        catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
                        {
                            //Log.Logger.LogData("Error while setting focus on Button by Name: " + strName, LogLevel.Warning);
                        }
                        //Log.Logger.LogData("Button control activated:" + strName, LogLevel.Info);
                    }
                    if (ClickBeforeValueSet)
                    {
                        try
                        {
                            window.WaitWhileBusy();
                            button.RaiseClickEvent();
                            window.WaitWhileBusy();
                            //Log.Logger.LogData("Button control clicked:" + strName, LogLevel.Info);
                        }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
                        catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
                        {
                            //Log.Logger.LogData("Error while click before value set on Button by Name: " + strName, LogLevel.Warning);
                        }
                    }
                    if (IsEventField)
                    {
                        window.WaitWhileBusy();
                        button.RaiseClickEvent();
                        window.WaitWhileBusy();
                        //Log.Logger.LogData("Span control clicked:" + strName, LogLevel.Info);
                    }
                    return button.ToString();
                }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
                catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
                { UseNextPriority = true; }
                return string.Empty;
            }
            else if ((ctrl.Trim().ToLower().Contains("edit")))
            {
                TestStack.White.UIItems.TextBox txt = (TestStack.White.UIItems.TextBox)(item);
                try
                {
                    if (Activate)
                    {
                        try
                        {
                            window.WaitWhileBusy();
                            txt.Focus();
                            window.WaitWhileBusy();
                        }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
                        catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
                        {
                            Log.Logger.LogData("Error while Editing COntent from take action on control." , LogLevel.Error);
                        }


                    }
                    if (ClickBeforeValueSet)
                    {
                        try
                        {
                            window.WaitWhileBusy();
                            txt.RaiseClickEvent();
                            window.WaitWhileBusy();
                        }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
                        catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
                        {
                            //Log.Logger.LogData("Error while click before value set on Edit by Name: " , LogLevel.Warning);
                        }
                    }
                    if (!string.IsNullOrEmpty(setValue))
                    {
                        window.WaitWhileBusy();
                        txt.Text = setValue;
                        window.WaitWhileBusy();

                    }
                    if (IsEventField)
                    {
                        window.WaitWhileBusy();
                        txt.RaiseClickEvent();
                        window.WaitWhileBusy();

                    }
                    //Log.Logger.LogData("Edit control value get:" + strName, LogLevel.Info);
                    return txt.Text;

                }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
                catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
                { UseNextPriority = true; }
                return string.Empty;
            }
            else if ((ctrl.Trim().ToLower().Contains("text")))
            {
                try
                {
                    TestStack.White.UIItems.Label label = (TestStack.White.UIItems.Label)(item);
                    try
                    {
                        if (Activate)
                        {
                            try
                            {
                                window.WaitWhileBusy();
                                label.Focus();
                                window.WaitWhileBusy();
                                //Log.Logger.LogData("Text control activated:" + strName, LogLevel.Info);
                            }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
                            catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
                            {
                                //Log.Logger.LogData("Error while setting focus on Text: " + strName, LogLevel.Warning);
                            }
                            //Log.Logger.LogData("Text control activated:" + strName, LogLevel.Info);

                        }
                        if (ClickBeforeValueSet)
                        {
                            try
                            {
                                window.WaitWhileBusy();
                                label.RaiseClickEvent();
                                window.WaitWhileBusy();
                            }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
                            catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
                            {
                                //Log.Logger.LogData("Error while click before value set on Text by Name: " + strName, LogLevel.Warning);
                            }
                        }
                        if (!string.IsNullOrEmpty(setValue))
                        {
                            //label.vText = setValue;
                            //Log.Logger.LogData("Control value can not be set for label:" + strName, LogLevel.Warning);
                        }
                        if (IsEventField)
                        {
                            window.WaitWhileBusy();
                            label.RaiseClickEvent();
                            window.WaitWhileBusy();
                            //Log.Logger.LogData("Span control clicked:" + strName, LogLevel.Info);
                        }
                        //Log.Logger.LogData("Text control value get:" + strName, LogLevel.Info);
                        return label.Text;
                    }
                    catch (Exception ex)
                    {
                        Log.Logger.LogData(ex.Message, LogLevel.Warning);
                        UseNextPriority = true;
                    }
                }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
                catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
                { UseNextPriority = true; }
                return string.Empty;
            }
            else if ((ctrl.Trim().ToLower().Contains("checkbox")))
            {
                try
                {
                    TestStack.White.UIItems.CheckBox chk = (TestStack.White.UIItems.CheckBox)(item);
                    window.WaitWhileBusy();
                    if (Activate)
                    {
                        try
                        {
                            window.WaitWhileBusy();
                            chk.Focus();
                            window.WaitWhileBusy();
                            //Log.Logger.LogData("CheckBox control activated:" + strName, LogLevel.Info);
                        }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
                        catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
                        {
                            //Log.Logger.LogData("Error while setting focus on CheckBox by Name: " + strName, LogLevel.Warning);
                        }
                        //Log.Logger.LogData("Control activated:" + strName, LogLevel.Info);

                    }
                    if (ClickBeforeValueSet)
                    {
                        try
                        {
                            window.WaitWhileBusy();
                            chk.RaiseClickEvent();
                            window.WaitWhileBusy();
                        }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
                        catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
                        {
                            //Log.Logger.LogData("Error while click before value set on CheckBox by Id " + strName, LogLevel.Warning);
                        }
                    }
                    if (!string.IsNullOrEmpty(setValue))
                    {
                        if ((setValue.ToLower().Trim() == "true") || (setValue.ToLower().Trim() == "select") || (setValue.ToLower().Trim() == "1"))
                        {
                            chk.Checked = true;
                        }
                        else
                        {
                            chk.Checked = false;
                        }
                    }
                    if (IsEventField)
                    {
                        chk.RaiseClickEvent();
                        //Log.Logger.LogData("CheckBox control clicked:" + strName, LogLevel.Info);
                    }
                    window.WaitWhileBusy();
                    return chk.Checked.ToString();
                }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
                catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
                { UseNextPriority = true; }
                return string.Empty;
            }
            else if (ctrl.Trim().ToLower().Contains("radiobutton"))
            {
                try
                {
                    TestStack.White.UIItems.RadioButton radioButton = (TestStack.White.UIItems.RadioButton)(item);
                    window.WaitWhileBusy();
                    if (Activate)
                    {
                        try
                        {
                            radioButton.Focus();
                            //Log.Logger.LogData("RadioButton control activated:" + strName, LogLevel.Info);
                        }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
                        catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
                        {
                            //Log.Logger.LogData("Error while setting focus on RadioButton by Name: " + strName, LogLevel.Warning);
                        }
                        //Log.Logger.LogData("Control activated:" + strName, LogLevel.Info);

                    }
                    if (ClickBeforeValueSet)
                    {
                        try
                        {
                            radioButton.RaiseClickEvent();
                        }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
                        catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
                        {
                            //Log.Logger.LogData("Error while click before value set on RadioButton by Name: " + strName, LogLevel.Warning);
                        }
                    }
                    if (!string.IsNullOrEmpty(setValue))
                    {
                        if ((setValue.ToLower().Trim() == "true") || (setValue.ToLower().Trim() == "select") || (setValue.ToLower().Trim() == "1"))
                        {
                            radioButton.Select();
                        }
                        else
                        {
                            radioButton.Select();
                        }
                    }
                    if (IsEventField)
                    {
                        radioButton.RaiseClickEvent();
                        //Log.Logger.LogData("RadioButton control clicked:" + strName, LogLevel.Info);
                    }
                    window.WaitWhileBusy();
                    return radioButton.IsSelected.ToString();
                }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
                catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
                { UseNextPriority = true; }
                return string.Empty;

            }
            else if (ctrl.Trim().ToLower().Contains("combobox"))
            {
                try
                {
                    TestStack.White.UIItems.ListBoxItems.ComboBox comboBox = (TestStack.White.UIItems.ListBoxItems.ComboBox)(item);
                    window.WaitWhileBusy();
                    if (Activate)
                    {
                        try
                        {
                            comboBox.Focus();
                            //Log.Logger.LogData("Combobox Control activated:" + strName, LogLevel.Info);
                        }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
                        catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
                        {
                            //Log.Logger.LogData("Error while setting focus on ComboBox by Name:" + strName, LogLevel.Warning);
                        }
                    }
                    if (ClickBeforeValueSet)
                    {
                        try
                        {
                            comboBox.RaiseClickEvent();
                        }
                        catch (Exception ex)
                        {
                            Log.Logger.LogData("Error while ClickBeforeValueSet : " + ex.Message, LogLevel.Error);
                        }
                    }
                    //if (!string.IsNullOrEmpty(setValue))
                    //{
                    //    comboBox.Select(setValue);
                    //}
                    if (IsEventField)
                    {
                        //comboBox.Click();

                        comboBox.RaiseClickEvent();
                    }
                    window.WaitWhileBusy();
                    if (!string.IsNullOrEmpty(setValue))
                    {
                        comboBox.Select(setValue);
                    }
                    return comboBox.SelectedItemText;
                }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
                catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
                {
                    //Log.Logger.LogData("Error in ComboBox set value by Name:" + strName, LogLevel.Warning);
                    UseNextPriority = true;
                }
                return string.Empty;

            }
            else if ((ctrl.Trim().ToLower().Contains("table")))
            {
                try
                {
                    TestStack.White.UIItems.TableItems.Table table = (TestStack.White.UIItems.TableItems.Table)(item);

                    window.WaitWhileBusy();
                    if (Activate)
                    {
                        try
                        {
                            table.Focus();
                            //Log.Logger.LogData("Table control activated:" + strName, LogLevel.Info);
                        }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
                        catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
                        {
                            //Log.Logger.LogData("Error while setting focus on Table by Name: " + strName, LogLevel.Warning);
                        }
                    }
                    if (ClickBeforeValueSet)
                    {
                        try
                        {
                            table.RaiseClickEvent();
                        }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
                        catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
                        {
                            //Log.Logger.LogData("Error while Click before Table value set on  by Name: " + strName, LogLevel.Warning);
                        }
                    }
                    if (!string.IsNullOrEmpty(setValue))
                    {
                        table.SetValue(setValue); //Need to send table object
                        //Log.Logger.LogData("Table value selected:" + strName, LogLevel.Info);
                    }
                    if (IsEventField)
                    {
                        table.RaiseClickEvent();
                        //Log.Logger.LogData("Table control clicked:" + strName, LogLevel.Info);
                    }
                    window.WaitWhileBusy();
                    return table;
                }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
                catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
                {
                    //Log.Logger.LogData("Error in table set value by Name:" + strName, LogLevel.Warning);
                    UseNextPriority = true;
                }
                return string.Empty;
            }
            else if ((ctrl.Trim().ToLower().Contains("image")))
            {
                try
                {
                    TestStack.White.UIItems.Image image = (TestStack.White.UIItems.Image)(item);
                    window.WaitWhileBusy();
                    if (Activate)
                    {
                        try
                        {
                            image.Focus();
                            //Log.Logger.LogData("Image Control activated:" + strName, LogLevel.Info);
                        }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
                        catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
                        {
                            //Log.Logger.LogData("Error while setting focus on image:" + strName, LogLevel.Warning);
                        }
                    }
                    if (ClickBeforeValueSet)
                    {
                        try
                        {
                            image.RaiseClickEvent();
                        }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
                        catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
                        {
                            //Log.Logger.LogData("Error while Click Before image Value set on Name: " + strName, LogLevel.Warning);
                        }
                    }
                    if (!string.IsNullOrEmpty(setValue))
                    {
                        image.SetValue(setValue); //Need to send table object
                        //Log.Logger.LogData("Image value selected:" + strName, LogLevel.Info);
                    }
                    if (IsEventField)
                    {
                        image.RaiseClickEvent();
                        //Log.Logger.LogData("Image control clicked:" + strName, LogLevel.Info);
                    }
                    window.WaitWhileBusy();
                    return image;
                }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
                catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
                {
                    //Log.Logger.LogData("Error in image set value by Name:" + strName, LogLevel.Warning);
                    UseNextPriority = true;
                }
                return string.Empty;
            }
            else if (ctrl.Trim().ToLower().Contains("listitem"))
            {
                try
                {
                    TestStack.White.UIItems.ListBoxItems.ListItem listItem = (TestStack.White.UIItems.ListBoxItems.ListItem)(item);
                    window.WaitWhileBusy();
                    if (Activate)
                    {
                        try
                        {
                            listItem.Focus();
                            //Log.Logger.LogData("ListItem Control activated:" + strName, LogLevel.Info);
                        }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
                        catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
                        {
                            //Log.Logger.LogData("Error while setting focus on ListItem:" + strName, LogLevel.Warning);
                        }
                    }
                    if (ClickBeforeValueSet)
                    {
                        try
                        {
                            listItem.RaiseClickEvent();
                        }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
                        catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
                        {
                            //Log.Logger.LogData("Error while click before ListItem value set on Name: " + strName, LogLevel.Warning);
                        }
                    }
                    if (!string.IsNullOrEmpty(setValue))
                    {
                        listItem.SetValue(setValue); //Need to send table object
                        //Log.Logger.LogData("ListItem value selected:" + strName, LogLevel.Info);
                    }
                    if (IsEventField)
                    {
                        listItem.RaiseClickEvent();
                        //Log.Logger.LogData("ListItem control clicked:" + strName, LogLevel.Info);
                    }
                    window.WaitWhileBusy();
                    return listItem.Text;
                }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
                catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
                { UseNextPriority = true; }
                return string.Empty;

            }
            else if ((ctrl.Trim().ToLower().Contains("list")))
            {
                try
                {
                    TestStack.White.UIItems.ListBoxItems.ListBox comboBox = (TestStack.White.UIItems.ListBoxItems.ListBox)(item);
                    window.WaitWhileBusy();
                    if (Activate)
                    {
                        try
                        {
                            comboBox.Focus();
                            //Log.Logger.LogData("List control activated:" + strName, LogLevel.Info);
                        }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
                        catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
                        {
                            //Log.Logger.LogData("Error while setting focus on List by Name:" + strName, LogLevel.Warning);
                        }
                    }
                    if (ClickBeforeValueSet)
                    {
                        try
                        {
                            comboBox.RaiseClickEvent();
                        }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
                        catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
                        {
                            //Log.Logger.LogData("Error while Click before List value set on Id:  " + strName, LogLevel.Warning);
                        }
                    }
                    if (!string.IsNullOrEmpty(setValue))
                    {
                        comboBox.SetValue(setValue);
                        //Log.Logger.LogData("List value selected:" + strName, LogLevel.Info);
                    }
                    if (IsEventField)
                    {
                        comboBox.RaiseClickEvent();
                        //Log.Logger.LogData("List control clicked:" + strName, LogLevel.Info);
                    }
                    window.WaitWhileBusy();
                    return comboBox.SelectedItemText;
                }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
                catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
                { UseNextPriority = true; }
                return string.Empty;
            }
            else if ((ctrl.Trim().ToLower().Contains("menubar")))
            {
                try
                {
                    TestStack.White.UIItems.WindowStripControls.MenuBar menuBar = (TestStack.White.UIItems.WindowStripControls.MenuBar)(item);

                    window.WaitWhileBusy();
                    if (Activate)
                    {
                        try
                        {
                            menuBar.Focus();
                            //Log.Logger.LogData("MenuBar Control activated:" + strName, LogLevel.Info);
                        }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
                        catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
                        {
                            //Log.Logger.LogData("Error while setting focus on MenuBar by Name: " + strName, LogLevel.Warning);
                        }
                    }
                    if (ClickBeforeValueSet)
                    {
                        try
                        {
                            menuBar.RaiseClickEvent();
                        }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
                        catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
                        {
                            //Log.Logger.LogData("Error while Click before MenuBar value set on Name: " + strName, LogLevel.Warning);
                        }
                    }
                    if (!string.IsNullOrEmpty(setValue))
                    {
                        menuBar.SetValue(setValue);
                        //Log.Logger.LogData("MenuBar value selected:" + strName, LogLevel.Info);
                    }
                    if (IsEventField)
                    {
                        menuBar.RaiseClickEvent();
                        //Log.Logger.LogData("MenuBar control clicked:" + strName, LogLevel.Info);
                    }
                    window.WaitWhileBusy();
                    return menuBar.ToString();
                }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
                catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
                { UseNextPriority = true; }
                return string.Empty;
            }
            else if (ctrl.Trim().ToLower().Contains("menuitem"))
            {
                try
                {
                    TestStack.White.UIItems.MenuItems.Menu menuItem = (TestStack.White.UIItems.MenuItems.Menu)(item);

                    window.WaitWhileBusy();
                    if (Activate)
                    {
                        try
                        {
                            menuItem.Focus();
                            //Log.Logger.LogData("MenuItem control activated:" + strName, LogLevel.Info);
                        }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
                        catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
                        {
                            //Log.Logger.LogData("Error while setting focus on MenuItem by Name:" + strName, LogLevel.Warning);
                        }
                    }
                    if (ClickBeforeValueSet)
                    {
                        try
                        {
                            menuItem.RaiseClickEvent();
                        }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
                        catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
                        {
                            //Log.Logger.LogData("Error while Click before MenuItem value set on Name: " + strName, LogLevel.Warning);
                        }
                    }
                    if (!string.IsNullOrEmpty(setValue))
                    {
                        menuItem.SetValue(setValue);
                        //Log.Logger.LogData("MenuItem value selected:" + strName, LogLevel.Info);
                    }
                    if (IsEventField)
                    {
                        menuItem.Click();// RaiseClickEvent();
                        //Log.Logger.LogData("MenuItem control clicked:" + strName, LogLevel.Info);
                    }
                    window.WaitWhileBusy();
                    return menuItem.ToString();
                }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
                catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
                { UseNextPriority = true; }
                return string.Empty;

            }
            else if (ctrl.Trim().ToLower().Contains("menu"))
            {
                try
                {
                    TestStack.White.UIItems.MenuItems.Menu menuItem = (TestStack.White.UIItems.MenuItems.Menu)(item);

                    window.WaitWhileBusy();
                    if (Activate)
                    {
                        try
                        {
                            menuItem.Focus();
                            //Log.Logger.LogData("List control activated:" + strName, LogLevel.Info);
                        }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
                        catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
                        {
                            //Log.Logger.LogData("Error while setting focus on Menu by Name: " + strName, LogLevel.Warning);
                        }
                    }
                    if (ClickBeforeValueSet)
                    {
                        try
                        {
                            menuItem.RaiseClickEvent();
                        }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
                        catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
                        {
                            //Log.Logger.LogData("Error while click before Menu value set on Name: " + strName, LogLevel.Warning);
                        }
                    }
                    if (!string.IsNullOrEmpty(setValue))
                    {
                        menuItem.SetValue(setValue);
                        //Log.Logger.LogData("Menu value selected:" + strName, LogLevel.Info);
                    }
                    if (IsEventField)
                    {
                        menuItem.RaiseClickEvent();
                        //Log.Logger.LogData("Menu control clicked:" + strName, LogLevel.Info);
                    }
                    window.WaitWhileBusy();
                    return menuItem.ToString();
                }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
                catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
                { UseNextPriority = true; }
                return string.Empty;

            }
            else if ((ctrl.Trim().ToLower().Contains("calendar")) || (ctrl.Trim().ToLower().Contains("pane")))
            {
                try
                {
                    TestStack.White.UIItems.DateTimePicker sDate = (TestStack.White.UIItems.DateTimePicker)(item);
                    window.WaitWhileBusy();
                    if (sDate != null)
                    {
                        if (Activate)
                        {
                            try
                            {
                                sDate.Focus();
                                //Log.Logger.LogData("Calendar control activated:" + strName, LogLevel.Info);
                            }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
                            catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
                            {
                                //Log.Logger.LogData("Error while setting focus on Calendar by Name:" + strName, LogLevel.Warning);
                            }
                        }
                        if (ClickBeforeValueSet)
                        {
                            try
                            {
                                sDate.RaiseClickEvent();
                            }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
                            catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
                            {
                                //Log.Logger.LogData("Error while click before Calendar value set on Name: " + strName, LogLevel.Warning);
                            }
                        }
                        if (!string.IsNullOrEmpty(setValue))
                        {
                            System.DateTime dateTime = Convert.ToDateTime(setValue);
                            sDate.SetDate(dateTime, TestStack.White.UIItems.DateFormat.MonthDayYear);
                            //Log.Logger.LogData("Calendar value selected:" + strName, LogLevel.Info);
                        }
                        if (IsEventField)
                        {
                            sDate.RaiseClickEvent();
                            //Log.Logger.LogData("Calendar control clicked:" + strName, LogLevel.Info);
                        }
                        window.WaitWhileBusy();
                        return sDate.Date.ToString();//ToLongDateString();
                    }
                    else
                    {
                        TestStack.White.UIItems.PropertyGridItems.PropertyGrid propertyGrid = (TestStack.White.UIItems.PropertyGridItems.PropertyGrid)(item);
                        window.WaitWhileBusy();
                        if (propertyGrid != null)
                        {

                            if (Activate)
                            {
                                try
                                {
                                    propertyGrid.RaiseClickEvent();
                                    //Log.Logger.LogData("PropertyGrid control activated:" + strName, LogLevel.Info);
                                }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
                                catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
                                {
                                    //Log.Logger.LogData("Error while setting focus on PropertyGrid by Name: " + strName, LogLevel.Warning);
                                }
                            }
                            if (ClickBeforeValueSet)
                            {
                                try
                                {
                                    propertyGrid.RaiseClickEvent();
                                }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
                                catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
                                {
                                    //Log.Logger.LogData("Error while Click before PropertyGrid value set on Name: " + strName, LogLevel.Warning);
                                }
                            }
                            if (!string.IsNullOrEmpty(setValue))
                            {
                                try
                                {
                                    propertyGrid.SetValue(setValue);
                                    //Log.Logger.LogData("PropertyGrid value selected:" + strName, LogLevel.Info);
                                }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
                                catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
                                {
                                    //Log.Logger.LogData("Error in propertyGrid set value by Name:" + strName, LogLevel.Info);
                                }
                            }
                            if (IsEventField)
                            {
                                propertyGrid.RaiseClickEvent();
                                //Log.Logger.LogData("PropertyGrid control clicked:" + strName, LogLevel.Info);
                            }
                            window.WaitWhileBusy();
                            return propertyGrid;
                        }
                    }
                    window.WaitWhileBusy();
                }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
                catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
                { UseNextPriority = true; }
                return string.Empty;

            }
            else if (ctrl.Trim().ToLower().Contains("hyperlink"))
            {
                try
                {
                    TestStack.White.UIItems.Hyperlink hlink = (TestStack.White.UIItems.Hyperlink)(item);
                    window.WaitWhileBusy();
                    if (Activate)
                    {
                        try
                        {
                            hlink.RaiseClickEvent();
                            //Log.Logger.LogData("Hyperlink Control activated:" + strName, LogLevel.Info);
                        }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
                        catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
                        {
                            //Log.Logger.LogData("Error while setting focus on Hyperlink by Name:" + strName, LogLevel.Warning);
                        }
                    }
                    if (ClickBeforeValueSet)
                    {
                        try
                        {
                            hlink.RaiseClickEvent();
                        }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
                        catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
                        {
                            //Log.Logger.LogData("Error while Click before Hyperlink value set on Name: " + strName, LogLevel.Warning);
                        }
                    }
                    if (!string.IsNullOrEmpty(setValue))
                    {
                        try
                        {
                            hlink.SetValue(setValue);
                            //Log.Logger.LogData("Hyperlink value selected:" + strName, LogLevel.Info);
                        }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
                        catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
                        {
                            //Log.Logger.LogData("Error in Hyperlink set value by Name:" + strName, LogLevel.Info);
                        }
                    }
                    if (IsEventField)
                    {
                        hlink.RaiseClickEvent();
                        //Log.Logger.LogData("Hyperlink control clicked:" + strName, LogLevel.Info);
                    }
                    window.WaitWhileBusy();
                    return hlink.ToString();
                }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
                catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
                { UseNextPriority = true; }
                return string.Empty;

            }
            else if (ctrl.Trim().ToLower().Contains("tabitem"))
            {
                try
                {
                    TestStack.White.UIItems.TabItems.TabPage tabPage = (TestStack.White.UIItems.TabItems.TabPage)(item);
                    window.WaitWhileBusy();
                    if (Activate)
                    {
                        try
                        {
                            tabPage.RaiseClickEvent();
                            //Log.Logger.LogData("TabItem control activated:" + strName, LogLevel.Info);
                        }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
                        catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
                        {
                            //Log.Logger.LogData("Error while setting focus on TabItem by Name: " + strName, LogLevel.Warning);
                        }
                    }
                    if (ClickBeforeValueSet)
                    {
                        try
                        {
                            tabPage.RaiseClickEvent();
                        }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
                        catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
                        {
                            //Log.Logger.LogData("Error while click before TabItem value set on Name: " + strName, LogLevel.Warning);
                        }
                    }
                    if (!string.IsNullOrEmpty(setValue))
                    {
                        try
                        {
                            tabPage.SetValue(setValue);
                            //Log.Logger.LogData("TabItem value selected:" + strName, LogLevel.Info);
                        }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
                        catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
                        {
                            //Log.Logger.LogData("Error in TabItem set value by Name:" + strName, LogLevel.Info);
                        }
                    }
                    if (IsEventField)
                    {
                        tabPage.RaiseClickEvent();
                        //Log.Logger.LogData("TabItem control clicked:" + strName, LogLevel.Info);
                    }
                    window.WaitWhileBusy();
                    return tabPage.ToString();
                }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
                catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
                { UseNextPriority = true; }
                return string.Empty;

            }
            else if ((ctrl.Trim().ToLower().Contains("tab")))
            {
                try
                {
                    TestStack.White.UIItems.TabItems.Tab tab = (TestStack.White.UIItems.TabItems.Tab)(item);
                    window.WaitWhileBusy();
                    if (Activate)
                    {
                        try
                        {
                            tab.RaiseClickEvent();
                            //Log.Logger.LogData("Tab control activated:" + strName, LogLevel.Info);
                        }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
                        catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
                        {
                            //Log.Logger.LogData("Error while setting focus on Tab by Name: " + strName, LogLevel.Warning);
                        }
                    }
                    if (ClickBeforeValueSet)
                    {
                        try
                        {
                            tab.RaiseClickEvent();
                        }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
                        catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
                        {
                            //Log.Logger.LogData("Error while Click before Tab value set on Name: " + strName, LogLevel.Warning);
                        }
                    }
                    if (!string.IsNullOrEmpty(setValue))
                    {
                        try
                        {
                            tab.SetValue(setValue);
                            //Log.Logger.LogData("Tab value selected:" + strName, LogLevel.Info);
                        }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
                        catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
                        {
                            //Log.Logger.LogData("Error in Tab set value by Name:" + strName, LogLevel.Info);
                        }
                    }
                    if (IsEventField)
                    {
                        tab.RaiseClickEvent();
                        //Log.Logger.LogData("Tab control clicked:" + strName, LogLevel.Info);
                    }
                    window.WaitWhileBusy();
                    return tab.ToString();
                }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
                catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
                { UseNextPriority = true; }
                return string.Empty;
            }
            else if ((ctrl.Trim().ToLower().Contains("datagrid")))
            {
                try
                {
                    TestStack.White.UIItems.ListView dataGrid = (TestStack.White.UIItems.ListView)(item);

                    window.WaitWhileBusy();
                    if (Activate)
                    {
                        try
                        {
                            dataGrid.RaiseClickEvent();
                            //Log.Logger.LogData("DataGrid control activated:" + strName, LogLevel.Info);
                        }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
                        catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
                        {
                            //Log.Logger.LogData("Error while setting focus on DataGrid by Name: " + strName, LogLevel.Warning);
                        }
                    }
                    if (ClickBeforeValueSet)
                    {
                        try
                        {
                            dataGrid.RaiseClickEvent();
                        }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
                        catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
                        {
                            //Log.Logger.LogData("Error while Click before List value set on Name: " + strName, LogLevel.Warning);
                        }
                    }
                    if (!string.IsNullOrEmpty(setValue))
                    {
                        try
                        {
                            dataGrid.SetValue(setValue);
                            //Log.Logger.LogData("List value selected:" + strName, LogLevel.Info);
                        }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
                        catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
                        {
                            //Log.Logger.LogData("Error in propertyGrid set value by Name:" + strName, LogLevel.Info);
                        }
                    }
                    if (IsEventField)
                    {
                        dataGrid.RaiseClickEvent();
                        //Log.Logger.LogData("List control clicked:" + strName, LogLevel.Info);
                    }
                    window.WaitWhileBusy();
                    return dataGrid;
                }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
                catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
                { UseNextPriority = true; }
                return string.Empty;
            }
            else if (ctrl.Trim().ToLower().Contains("dataitem"))
            {
                try
                {
                    TestStack.White.UIItems.ListViewRow dataGridrow = (TestStack.White.UIItems.ListViewRow)(item);

                    window.WaitWhileBusy();
                    if (Activate)
                    {
                        try
                        {
                            dataGridrow.RaiseClickEvent();
                            //Log.Logger.LogData("List control activated:" + strName, LogLevel.Info);
                        }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
                        catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
                        {
                            //Log.Logger.LogData("Error while setting focus on List by Name:" + strName, LogLevel.Warning);
                        }
                    }
                    if (ClickBeforeValueSet)
                    {
                        try
                        {
                            dataGridrow.RaiseClickEvent();
                        }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
                        catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
                        {
                            //Log.Logger.LogData("Error while Click before List value set on Name: " + strName, LogLevel.Warning);
                        }
                    }
                    if (!string.IsNullOrEmpty(setValue))
                    {
                        try
                        {
                            dataGridrow.SetValue(setValue);
                            //Log.Logger.LogData("List value selected:" + strName, LogLevel.Info);
                        }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
                        catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
                        {
                            //Log.Logger.LogData("Error in propertyGrid set value by Name:" + strName, LogLevel.Info);
                        }
                    }
                    if (IsEventField)
                    {
                        dataGridrow.RaiseClickEvent();
                        //Log.Logger.LogData("List control clicked:" + strName, LogLevel.Info);
                    }
                    window.WaitWhileBusy();
                    return dataGridrow;
                }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
                catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
                { UseNextPriority = true; }
                return string.Empty;

            }
            else if ((ctrl.Trim().ToLower().Contains("TreeItem") || (ctrl.Trim().ToLower().Contains("Tree"))))
            //else if ((ctrl.Trim().ToLower().Contains(" ControlType.TreeItem") || (ctrl.Trim().ToLower().Contains("Tree")))
            {
                try
                {
                    TestStack.White.UIItems.TreeItems.TreeNode nodeOne = (TestStack.White.UIItems.TreeItems.TreeNode)(item);
                    //nodeOne.Select();
                    //nodeOne.Expand();
                    window.WaitWhileBusy();
                    if (Activate)
                    {
                        try
                        {
                            nodeOne.RaiseClickEvent();
                            //Log.Logger.LogData("TreeNode control activated:" + strName, LogLevel.Info);
                        }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
                        catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
                        {
                            //Log.Logger.LogData("Error while setting focus on TreeNode by Name: " + strName, LogLevel.Warning);
                        }
                    }
                    if (ClickBeforeValueSet)
                    {
                        try
                        {
                            nodeOne.RaiseClickEvent();
                        }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
                        catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
                        {
                            //Log.Logger.LogData("Error while Click before TreeNode value set on Name: " + strName, LogLevel.Warning);
                        }
                    }
                    if (!string.IsNullOrEmpty(setValue))
                    {
                        try
                        {
                            nodeOne.Select();
                            //Log.Logger.LogData("TreeNode value selected:" + strName, LogLevel.Info);
                        }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
                        catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
                        {
                            //Log.Logger.LogData("Error in TreeNode set value by Name:" + strName, LogLevel.Info);
                        }
                    }
                    if (IsEventField)
                    {
                        nodeOne.RaiseClickEvent();
                        //Log.Logger.LogData("TreeNode control clicked:" + strName, LogLevel.Info);
                    }
                    window.WaitWhileBusy();
                    return nodeOne.Text;
                }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
                catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
                {
                    UseNextPriority = true;
                }
                return string.Empty;
            }
            else if ((ctrl.Trim().ToLower().Contains(" ProgressBar")))
            {
                try
                {
                    TestStack.White.UIItems.ProgressBar progressBar = (TestStack.White.UIItems.ProgressBar)(item);
                    progressBar.RaiseClickEvent();
                    do
                    {
                        Thread.Sleep(5000);
                    } while (progressBar.Value != progressBar.Maximum);

                }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
                catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
                { UseNextPriority = true; }
                return string.Empty;
            }
            else if (ctrl.Trim().ToLower().Contains("Slider"))
            {
                try
                {
                    TestStack.White.UIItems.Slider sldrOne = (TestStack.White.UIItems.Slider)(item);

                    sldrOne.LargeIncrementButton.RaiseClickEvent();
                    //   sldrOne.LargeDecrementButton.RaiseClickEvent();
                }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
                catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
                { UseNextPriority = true; }
                return string.Empty;

            }
            else if ((ctrl.Trim().ToLower().Contains("Spinner")))
            {
                try
                {
                    TestStack.White.UIItems.Spinner spinner = (TestStack.White.UIItems.Spinner)(item);
                    window.WaitWhileBusy();
                    if (Activate)
                    {
                        try
                        {
                            spinner.RaiseClickEvent();
                            //Log.Logger.LogData("Spinner control activated:" + strName, LogLevel.Info);
                        }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
                        catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
                        {
                            //Log.Logger.LogData("Error while setting focus on Spinner by Name: " + strName, LogLevel.Warning);
                        }
                    }
                    if (ClickBeforeValueSet)
                    {
                        try
                        {
                            spinner.RaiseClickEvent();
                        }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
                        catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
                        {
                            //Log.Logger.LogData("Error while Click before Spinner value set on Name: " + strName, LogLevel.Warning);
                        }
                    }
                    if (!string.IsNullOrEmpty(setValue))
                    {
                        try
                        {
                            spinner.SetValue(setValue);
                            //Log.Logger.LogData("Spinner value selected:" + strName, LogLevel.Info);
                        }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
                        catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
                        {
                            //Log.Logger.LogData("Error in Spinner set value by Name:" + strName, LogLevel.Info);
                        }
                    }
                    if (IsEventField)
                    {
                        spinner.RaiseClickEvent();
                        //Log.Logger.LogData("Spinner control clicked:" + strName, LogLevel.Info);
                    }
                    window.WaitWhileBusy();
                    return spinner.Value.ToString();
                }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
                catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
                { UseNextPriority = true; }
                return string.Empty;
            }
            else if (ctrl.Trim().ToLower().Contains("ToolBar"))
            {
                try
                {
                    TestStack.White.UIItems.WindowStripControls.ToolStrip toolStrip = (TestStack.White.UIItems.WindowStripControls.ToolStrip)(item);
                    window.WaitWhileBusy();
                    if (Activate)
                    {
                        try
                        {
                            toolStrip.RaiseClickEvent();
                            //Log.Logger.LogData("ToolBar control activated:" + strName, LogLevel.Info);
                        }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
                        catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
                        {
                            //Log.Logger.LogData("Error while setting focus on ToolBar by Name: " + strName, LogLevel.Warning);
                        }
                    }
                    if (ClickBeforeValueSet)
                    {
                        try
                        {
                            toolStrip.RaiseClickEvent();
                        }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
                        catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
                        {
                            //Log.Logger.LogData("Error while Click before ToolBar value set on Name: " + strName, LogLevel.Warning);
                        }
                    }
                    if (!string.IsNullOrEmpty(setValue))
                    {
                        try
                        {
                            toolStrip.SetValue(setValue);
                            //Log.Logger.LogData("ToolBar value selected:" + strName, LogLevel.Info);
                        }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
                        catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
                        {
                            //Log.Logger.LogData("Error in ToolBar set value by Name:" + strName, LogLevel.Info);
                        }
                    }
                    if (IsEventField)
                    {
                        toolStrip.RaiseClickEvent();
                        //Log.Logger.LogData("ToolBar control clicked:" + strName, LogLevel.Info);
                    }
                    window.WaitWhileBusy();
                    return toolStrip.ToString();
                }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
                catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
                { UseNextPriority = true; }
                return string.Empty;

            }
            else if ((ctrl.Trim().ToLower().Contains("ToolTip")))
            {
                try
                {
                    TestStack.White.UIItems.ToolTip toolTip = (TestStack.White.UIItems.ToolTip)(item);
                    window.WaitWhileBusy();
                    if (Activate)
                    {
                        try
                        {
                            toolTip.RaiseClickEvent();
                            //Log.Logger.LogData("ToolTip control activated:" + strName, LogLevel.Info);
                        }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
                        catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
                        {
                            //Log.Logger.LogData("Error while setting focus on ToolTip by Name:" + strName, LogLevel.Warning);
                        }
                    }
                    if (ClickBeforeValueSet)
                    {
                        try
                        {
                            toolTip.RaiseClickEvent();
                        }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
                        catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
                        {
                            //Log.Logger.LogData("Error while Click before ToolTip value set on Name: " + strName, LogLevel.Warning);
                        }
                    }
                    if (!string.IsNullOrEmpty(setValue))
                    {
                        try
                        {
                            toolTip.SetValue(setValue);
                            //Log.Logger.LogData("ToolTip value selected:" + strName, LogLevel.Info);
                        }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
                        catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
                        {
                            //Log.Logger.LogData("Error in ToolTip set value by Name:" + strName, LogLevel.Info);
                        }
                    }
                    if (IsEventField)
                    {
                        toolTip.RaiseClickEvent();
                        //Log.Logger.LogData("ToolTip control clicked:" + strName, LogLevel.Info);
                    }
                    window.WaitWhileBusy();
                    return toolTip.Text;

                }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
                catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
                { UseNextPriority = true; }
                return string.Empty;
            }
            else if ((ctrl.Trim().ToLower().Contains("Group")))
            {
                try
                {
                    TestStack.White.UIItems.GroupBox groupBox = (TestStack.White.UIItems.GroupBox)(item);

                    window.WaitWhileBusy();
                    if (Activate)
                    {
                        try
                        {
                            groupBox.RaiseClickEvent();
                            //Log.Logger.LogData("Group control activated:" + strName, LogLevel.Info);
                        }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
                        catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
                        {
                            //Log.Logger.LogData("Error while setting focus on Group by Name:" + strName, LogLevel.Warning);
                        }
                    }
                    if (ClickBeforeValueSet)
                    {
                        try
                        {
                            groupBox.RaiseClickEvent();
                        }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
                        catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
                        {
                            //Log.Logger.LogData("Error while Click before Group value set on Name: " + strName, LogLevel.Warning);
                        }
                    }
                    if (!string.IsNullOrEmpty(setValue))
                    {
                        try
                        {
                            groupBox.SetValue(setValue);
                            //Log.Logger.LogData("Group value selected:" + strName, LogLevel.Info);
                        }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
                        catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
                        {
                            //Log.Logger.LogData("Error in Group set value by Name:" + strName, LogLevel.Info);
                        }
                    }
                    if (IsEventField)
                    {
                        groupBox.RaiseClickEvent();
                        //Log.Logger.LogData("Group control clicked:" + strName, LogLevel.Info);
                    }
                    window.WaitWhileBusy();
                    return groupBox.ToString();
                }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
                catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
                { UseNextPriority = true; }
                return string.Empty;
            }
            else if (ctrl.Trim().ToLower().Contains("Thumb"))
            {
                try
                {
                    TestStack.White.UIItems.Thumb thumb = (TestStack.White.UIItems.Thumb)(item);

                    window.WaitWhileBusy();
                    if (Activate)
                    {
                        try
                        {
                            thumb.RaiseClickEvent();
                            //Log.Logger.LogData("Thumb control activated:" + strName, LogLevel.Info);
                        }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
                        catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
                        {
                            //Log.Logger.LogData("Error while setting focus on Thumb by Name:" + strName, LogLevel.Warning);
                        }
                    }
                    if (ClickBeforeValueSet)
                    {
                        try
                        {
                            thumb.RaiseClickEvent();
                        }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
                        catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
                        {
                            //Log.Logger.LogData("Error while Click before Thumb value set on Name: " + strName, LogLevel.Warning);
                        }
                    }
                    if (!string.IsNullOrEmpty(setValue))
                    {
                        try
                        {
                            thumb.SetValue(setValue);
                            //Log.Logger.LogData("Thumb value selected:" + strName, LogLevel.Info);
                        }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
                        catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
                        {
                            //Log.Logger.LogData("Error in Thumb set value by Name:" + strName, LogLevel.Info);
                        }
                    }
                    if (IsEventField)
                    {
                        thumb.RaiseClickEvent();
                        //Log.Logger.LogData("Thumb control clicked:" + strName, LogLevel.Info);
                    }
                    window.WaitWhileBusy();
                    return thumb.ToString();
                }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
                catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
                { UseNextPriority = true; }
                return string.Empty;

            }
            else if (ctrl.Trim().ToLower().Contains("TitleBar"))
            {
                try
                {
                    TestStack.White.UIItems.WindowItems.TitleBar titleBar = (TestStack.White.UIItems.WindowItems.TitleBar)(item);

                    window.WaitWhileBusy();
                    if (Activate)
                    {
                        try
                        {
                            titleBar.RaiseClickEvent();
                            //Log.Logger.LogData("TitleBar control activated:" + strName, LogLevel.Info);
                        }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
                        catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
                        {
                            //Log.Logger.LogData("Error while setting focus on TitleBar by Name:" + strName, LogLevel.Warning);
                        }
                    }
                    if (ClickBeforeValueSet)
                    {
                        try
                        {
                            titleBar.RaiseClickEvent();
                        }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
                        catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
                        {
                            //Log.Logger.LogData("Error while Click before TitleBar value set on Name: " + strName, LogLevel.Warning);
                        }
                    }
                    if (!string.IsNullOrEmpty(setValue))
                    {
                        try
                        {
                            titleBar.SetValue(setValue);
                            //Log.Logger.LogData("TitleBar value selected:" + strName, LogLevel.Info);
                        }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
                        catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
                        {
                            //Log.Logger.LogData("Error in TitleBar set value by Name:" + strName, LogLevel.Info);
                        }
                    }
                    if (IsEventField)
                    {
                        titleBar.RaiseClickEvent();
                        //Log.Logger.LogData("TitleBar control clicked:" + strName, LogLevel.Info);
                    }
                    window.WaitWhileBusy();
                    return titleBar.ToString();
                }
#pragma warning disable CS0168 // The variable 'ex' is declared but never used
                catch (Exception ex)
#pragma warning restore CS0168 // The variable 'ex' is declared but never used
                {
                    UseNextPriority = true;
                    return string.Empty;
                }

            }

            //else if ((ctrl.Trim().ToLower().Contains("item")))
            //{
            //    TestStack.White.UIItems.UIItem itemControl = (TestStack.White.UIItems.UIItem)(item); 
            //    try
            //    {
            //        if (Activate)
            //        {
            //            try
            //            {
            //                window.WaitWhileBusy();
            //                itemControl.Focus();
            //                window.WaitWhileBusy();
            //            }
            //            catch (Exception ex)
            //            {
            //                Log.Logger.LogData("Error while Editing COntent from take action on control." , LogLevel.Error);
            //            }


            //        }
            //        if (ClickBeforeValueSet)
            //        {
            //            try
            //            {
            //                window.WaitWhileBusy();
            //                itemControl.RaiseClickEvent();
            //                window.WaitWhileBusy();
            //            }
            //            catch (Exception ex)
            //            {
            //                //Log.Logger.LogData("Error while click before value set on Edit by Name: " , LogLevel.Warning);
            //            }
            //        }
            //        if (!string.IsNullOrEmpty(setValue))
            //        {
            //            window.WaitWhileBusy();
            //            itemControl.SetValue(setValue);//= setValue;
            //            window.WaitWhileBusy();

            //        }
            //        if (IsEventField)
            //        {
            //            window.WaitWhileBusy();
            //            itemControl.RaiseClickEvent();
            //            window.WaitWhileBusy();

            //        }
            //        //Log.Logger.LogData("Edit control value get:" + strName, LogLevel.Info);
            //        //return itemControl.Text;

            //    }
            //    catch (Exception ex)
            //    { UseNextPriority = true; }
            //    return string.Empty;
            //}
            return string.Empty;
        }

            #region "Not Supported - do not delete - for future implementation"
            //else if (ctrl.Trim().ToLower().Contains("StatusBar")
            //{
            //    try
            //    {
            //        //                    TestStack.White.UIItems.Thumb thumb =(TestStack.White.UIItems.Thumb)(item);
            //    }
            //    catch (Exception ex)
            //    { UseNextPriority = true; }
            //    return string.Empty;

            //}
            //else if ((ctrl.Trim().ToLower().Contains("ScrollBar"))
            //{
            //    try
            //    {
            //        //TestStack.White.UIItems.ProgressBar progressBar = window.Get<ScrollBar)(item);
            //    }
            //    catch (Exception ex)
            //    { UseNextPriority = true; }
            //    return string.Empty;
            //}
            //else if (ctrl.Trim().ToLower().Contains("SplitButton")
            //{
            //    try
            //    {
            //         TestStack.White.UIItems.ListViewRow dataGridrow = window.Get<SplitButton)(item);
            //    }
            //    catch (Exception ex)
            //    { UseNextPriority = true; }
            //    return string.Empty;

            //}
            //else if ((ctrl.Trim().ToLower().Contains(" ControlType.Document"))
            //{
            //    try
            //    {
            //        //TestStack.White.UIItems.Document document = window.Get<Document)(item);
            //    }
            //    catch (Exception ex)
            //    { UseNextPriority = true; }
            //    return string.Empty;
            //}
            //else if ((ctrl.Trim().ToLower().Contains("Window"))
            //{
            //    try
            //    {

            //    }
            //    catch (Exception ex)
            //    { UseNextPriority = true; }
            //    return string.Empty;
            //}
            //else if ((ctrl.Trim().ToLower().Contains("Header"))
            //{
            //    try
            //    {

            //    }
            //    catch (Exception ex)
            //    { UseNextPriority = true; }
            //    return string.Empty;
            //}
            //else if (ctrl.Trim().ToLower().Contains("HeaderItem")
            //{
            //    try
            //    {
            //       // TestStack.White.UIItems.WindowItems.TitleBar titleBar = window.Get<HeaderItem)(item);
            //    }
            //    catch (Exception ex)
            //    { UseNextPriority = true; }
            //    return string.Empty;

            //}
            //else if (ctrl.Trim().ToLower().Contains("Custom")
            //{
            //    try
            //    {
            //        //TestStack.White.UIItems.Custom groupBox =(TestStack.White.UIItems.Custom)(item);
            //    }
            //    catch (Exception ex)
            //    { UseNextPriority = true; }
            //    return string.Empty;

            //}
            //else if ((ctrl.Trim().ToLower().Contains("Separator"))
            //{
            //    try
            //    {
            //        //TestStack.White.UIItems.WindowItems.TitleBar titleBar = window.Get<Separator)(item);
            //        //titleBar.RaiseClickEvent();
            //    }
            //    catch (Exception ex)
            //    { UseNextPriority = true; }
            //    return string.Empty;
            //}
            #endregion

            //else
            //{
            //    return null;
            //}
        //}
    }
}
