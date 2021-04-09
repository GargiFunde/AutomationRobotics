// <copyright file=OpenWebApplication1.xaml company=E2E BOTS>
// Copyright (c) 2018 All Rights Reserved
// </copyright>
// <author>Prabodhini Funde</author>
// <date> 03-10-2018 16:02:57</date>
// <summary></summary>

using CommonLibrary;
using CommonLibrary.Interfaces;
using System;
using System.Activities.Presentation;
using System.Collections.Generic;
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
using System.Windows.Shapes;

namespace Bot.Activity.InternetExplorer
{
    /// <summary>
    /// Interaction logic for OpenWebApplication1.xaml
    /// </summary>
    public partial class OpenWebApplication1 
    {
        public OpenWebApplication1()
        {
            InitializeComponent();
        }
        private void btnLaunch_Click(object sender, RoutedEventArgs e)
        {
            
            
        }
        private void Designer_Loaded(object sender, RoutedEventArgs e)
        {
            //string ApplicationId = string.Empty;
            //OpenWebApplication owa = (OpenWebApplication)this.ModelItem.GetCurrentValue();
            //if (owa.IEWATIN == null)
            //{
            //    ApplicationId = owa.ApplicationID.Expression.ToString();
            //    WatiN.Core.IE.Settings.MakeNewIeInstanceVisible = false;
            //    if (!SelectHelper.CurrentRuntimeApplicationHelper.RuntimeApplicationObjects.ContainsKey(ApplicationId))
            //    {
            //        owa.taskA = Task.Factory.StartNew(() => {
            //            owa.IEWATIN = new WatiN.Core.IE();
                        
            //            if (owa.SearchUrl != null)
            //            {
            //                owa.IEWATIN.GoTo(new Uri(owa.SearchUrl.Expression.ToString()));
            //                owa.IEWATIN.SearchUrl = owa.SearchUrl.Expression.ToString();
            //                if (!SelectHelper.CurrentRuntimeApplicationHelper.RuntimeApplicationObjects.ContainsKey(owa.ApplicationID.Expression.ToString()))
            //                {
            //                    SelectHelper.CurrentRuntimeApplicationHelper.RuntimeApplicationObjects.Add(owa.ApplicationID.Expression.ToString(), owa.IEWATIN);
            //                }
            //            }
            //        });
            //    }
            //}
        }

    }
}
