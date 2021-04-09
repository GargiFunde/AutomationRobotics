// <copyright file=DataTableForEach1.xaml company=E2E BOTS>
// Copyright (c) 2018 All Rights Reserved
// </copyright>
// <author>Prabodhini Funde</author>
// <date> 03-10-2018 16:19:14</date>
// <summary></summary>

using CommonLibrary;
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
using System.Workflow.Activities;

namespace Bot.Activity.ActivityLibrary.Activities
{
    /// <summary>
    /// Interaction logic for AttachApplicationActivitiesDesigner.xaml
    /// </summary>
    public partial class DataTableForEach1
    {
        public DataTableForEach1()
        {
            InitializeComponent();
        }

        private void ActivityDesigner_Loaded(object sender, RoutedEventArgs e)
        {
            DataTableForEach owa = (DataTableForEach)this.ModelItem.GetCurrentValue();
            if(owa.Activities == null)
            {
                owa.Activities = new List<System.Activities.Activity>();
            }
        }
    }
}
