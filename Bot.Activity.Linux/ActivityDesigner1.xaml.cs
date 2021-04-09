// <copyright file=ActivityDesigner1.xaml company=E2E BOTS>
// Copyright (c) 2018 All Rights Reserved
// </copyright>
// <author>Prabodhini Funde</author>
// <date> 03-10-2018 16:00:31</date>
// <summary></summary>


using System;
using System.Activities;
using System.Activities.Statements;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;


namespace Bot.Activity.Linux
{
    /// <summary>
    /// Interaction logic for AttachApplicationActivitiesDesigner.xaml
    /// </summary>
    public partial class ActivityDesigner1
    {
        public ActivityDesigner1()
        {
            InitializeComponent();
        }

        private void ActivityDesigner_Loaded(object sender, RoutedEventArgs e)
        {
            // MarginButton = new Thickness(0,-29, 10, 0);
            SendPuttyCommand owa = (SendPuttyCommand)this.ModelItem.GetCurrentValue();
            owa.Port = 22;
            owa.Protocol = 1;
            owa.PuttyDllDirPath = @"D:\DLL\Putty\"; //default sample value
        }
    }
}
