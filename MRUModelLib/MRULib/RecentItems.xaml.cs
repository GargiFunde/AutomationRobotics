using System;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MRULib
{
    /// <summary>
    /// Interaction logic for RecentItems.xaml
    /// </summary>
    public partial class RecentItems : UserControl
    {
        /// <summary>
        /// Using a DependencyProperty as the backing store for LoadFileCommandHeader.
        /// </summary>
        public static readonly DependencyProperty LoadFileCommandHeaderProperty =
            DependencyProperty.Register("LoadFileCommandHeader",
                             typeof(string),
                             typeof(RecentItems),
                             new PropertyMetadata("Load File"));

        public RecentItems()
        {
            InitializeComponent();
        }

        public string LoadFileCommandHeader
        {
            get { return (string)GetValue(LoadFileCommandHeaderProperty); }
            set { SetValue(LoadFileCommandHeaderProperty, value); }
        }
    }
}
