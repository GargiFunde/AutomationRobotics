using System.Windows;

namespace HotKeys
{
    // Interaction logic for ActivityDesigner1.xaml
    public partial class ActivityDesigner1
    {
       HotKey sn = new HotKey();
        public ActivityDesigner1()
        {
            InitializeComponent();
            this.DataContext = sn;
        }
        //public ActivityDesigner1()
        //{
        //    InitializeComponent();
        //}
        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {


        }
    }
}
