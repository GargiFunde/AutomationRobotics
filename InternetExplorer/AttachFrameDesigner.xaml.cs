// <copyright file=AttachApplicationActivitiesDesigner.xaml company=E2E BOTS>
// Copyright (c) 2018 All Rights Reserved
// </copyright>
// <author>Prabodhini Funde</author>
// <date> 03-10-2018 16:19:14</date>
// <summary></summary>

using CommonLibrary;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using Logger;

namespace Bot.Activity.InternetExplorer
{
    /// <summary>
    /// Interaction logic for AttachApplicationActivitiesDesigner.xaml
    /// </summary>
    public partial class AttachFrameDesigner
    {
              
        public AttachFrameDesigner()
        {
            InitializeComponent();            
        }

        private void ActivityDesigner_Loaded(object sender, RoutedEventArgs e)
        {
            // MarginButton = new Thickness(0,-29, 10, 0);
            AttachFrame owa = (AttachFrame)this.ModelItem.GetCurrentValue();
            if (owa.Activities == null)
            {
                owa.Activities = new List<System.Activities.Activity>();
            }
        }
    
    }
}
