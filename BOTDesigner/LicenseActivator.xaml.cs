using System;
using System.Windows;
using System.Management;
using System.IO;
using System.Reflection;
using Portable.Licensing;
using System.Windows.Navigation;
using System.Diagnostics;
using Logger;

namespace BOTDesigner
{
    /// <summary>
    /// Interaction logic for LicenseActivator.xaml
    /// </summary>
    public partial class LicenseActivator : Window
    {
        public LicenseActivator()
        {
            InitializeComponent();
            string motherboardsrno = GetMotherBoardID();
            if((motherboardsrno != null)&&(motherboardsrno != string.Empty))
            {
                txtkey.Text = motherboardsrno.Trim();
            }
           
        }

        private void Hyperlink_RequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            Process.Start(new ProcessStartInfo(e.Uri.AbsoluteUri));
            e.Handled = true;
        }
        public string GetMotherBoardID()
        {
            string mbInfo = String.Empty;
            try
            {
                ManagementScope scope = new ManagementScope("\\\\" + Environment.MachineName + "\\root\\cimv2");
                scope.Connect();
                ManagementObject wmiClass = new ManagementObject(scope, new ManagementPath("Win32_BaseBoard.Tag=\"Base Board\""), new ObjectGetOptions());

                foreach (PropertyData propData in wmiClass.Properties)
                {
                    if (propData.Name == "SerialNumber")
                        mbInfo = Convert.ToString(propData.Value);
                }
            }
            catch (Exception ex)
            {
                Log.Logger.LogData(ex.Message, LogLevel.Error);
            }
            return mbInfo;
        }

        private License ulicense;
        private void btnDirectoryExplorer_Click(object sender, RoutedEventArgs e)
        {
            //using (var ofdlicense = new System.Windows.Forms.FolderBrowserDialog())
            //{
            //    if (ofdlicense.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            //    {
            //        txtLocation.Text = ofdlicense.SelectedPath;
            //        // folderDialog.SelectedPath -- your result
            //    }
            //}

            //  ListBoxValidation.Items.Clear();
            Stream myStream = null;
            try
            {
               
                var ofdlicense = new System.Windows.Forms.OpenFileDialog();
                ////Dim LicStreamReader As StreamReader
               

            ////OfDLicense.InitialDirectory = "c:\"
            ofdlicense.Filter = "License (*.lic)|*.lic";
            ofdlicense.FilterIndex = 1;
            ofdlicense.InitialDirectory = System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            if (ofdlicense.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                txtLocation.Text = ofdlicense.SafeFileName;
                
                myStream = ofdlicense.OpenFile();
                if (myStream.Length <= 0)
                {
                    return;
                }
                
                if(File.Exists("lic.lic"))
                {
                        try
                        {
                            File.Delete("lic.lic");
                        }
                        catch(Exception)
                        {
                             Logger.Log.Logger.LogData("License File (lic.lic) is used by other process", LogLevel.Error);
                        }
                }
                File.Copy(ofdlicense.FileName, "lic.lic");
                    this.ulicense = License.Load(myStream);
                    //this.TextBoxCustomerRo.Text = this.ulicense.Customer.Name;
                    //this.TextBoxEMailRo.Text = this.ulicense.Customer.Email;
                    //this.TextBoxUsersRo.Text = this.ulicense.Quantity.ToString(CultureInfo.InvariantCulture);

                    //this.TextBoxAttributeNameRo.Text = "Software";
                    //this.TextBoxAttributeValueRo.Text = this.ulicense.AdditionalAttributes.Get(this.TextBoxAttributeNameRo.Text);
                    //this.CheckBoxSalesRo.IsChecked = this.ulicense.ProductFeatures.Get("Sales") == "True";
                    //this.CheckBoxBillingRo.IsChecked = this.ulicense.ProductFeatures.Get("Billing") == "True";
                    //this.TextBoxLicenseType.Text = this.ulicense.Type.ToString();
                    string publickey = txtpublickey.Text;

                    string path = Environment.CurrentDirectory + "//PublicKey.txt";
                    if(File.Exists(path))
                    {
                        try
                        { 
                        File.Delete(path);
                        }
                        catch (Exception)
                        {
                            Logger.Log.Logger.LogData("Public key File (PublicKey.txt) is used by other process", LogLevel.Error);
                        }
                    }

                    File.AppendAllLines(path, new[] { publickey });
                    LicenseHelper lf = new LicenseHelper();
                    var str = lf.ValidateLicense(this.ulicense, publickey);
                    lblvalidity.Content = str;
                    if (str == "Activated: License is Valid")
                    {
                        DialogResult = true;
                    }
                    else
                    {
                        DialogResult = false;
                    }
                    
                }
            }
            catch (Exception ex)
            {
                DialogResult = false;
                Log.Logger.LogData ("Cannot read file from disk. Original error: " + ex.Message , LogLevel.Error);
            }
            finally
            {
                //// Check this again, since we need to make sure we didn't throw an exception on open.
                if (myStream != null)
                {
                    myStream.Close();
                }
            }
        }
        
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
