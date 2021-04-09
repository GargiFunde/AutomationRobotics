using System;
using System.Collections.Generic;
using System.Data;
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



namespace BOTDesigner.Views
{
    /// <summary>
    /// Interaction logic for LoginWindow.xaml
    /// </summary>
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
        }

        private void BtnSubmit_Click(object sender, RoutedEventArgs e)
        {
            string strUserName = string.Empty;
            string[] strUserDomain = null;
            string UserName = string.Empty;
            string PassWord = string.Empty;
            string Domain = string.Empty;
            string TenantName = string.Empty;
            string GroupName = string.Empty;
            DataTable result = null;

            try {

                strUserName = txtUserName.Text;



                if (strUserName.Contains("/"))
                {
                    strUserDomain = strUserName.Split('/');
                }
                else if (strUserName.Contains("\\"))
                {
                    strUserDomain = strUserName.Split('\\');
                }

                Domain = strUserDomain[0];
                UserName = strUserDomain[1];
                PassWord = txtPassWord.Password;
                TenantName = txtTenantName.Text;
                GroupName = txtGroupName.Text;

                string encodePwd = string.Empty;
                byte[] encode = new byte[PassWord.Length];
                encode = Encoding.UTF8.GetBytes(PassWord);
                encodePwd = Convert.ToBase64String(encode);
                PassWord = encodePwd;




                Logger.ServiceReference1.BOTServiceClient ser = new Logger.ServiceReference1.BOTServiceClient();
                result = ser.LoginUser(Domain, UserName, PassWord, TenantName, GroupName);


                if (result != null)
                {
                  //  MessageBox.Show("Welcome "+UserName+"!");
               

                     var mainWindow = new MainWindow();
                    // this.MainWindow = mainWindow;


                     mainWindow.Show();
                    this.Close();

                }
                else
                {
                    MessageBox.Show("Login failed. Please provide the right credentials!");
                    
                   
                }

            }
            catch (Exception)
            {

                MessageBox.Show("Login failed. Please provide the right credentials!");
                
            }
        }
    }
}
