using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Windows.Forms;
using System.Xml;

namespace FrontOfficeBotWebApp
{
    public partial class DashboardUI : System.Web.UI.Page
    {
        const String BOOK_TABLE = "Notes";

        DataSet dSet = null;
        String xmlFilename = null;
       public string RobotIP = string.Empty;
       public string RobotDomain = string.Empty;
       public string RobotServer = string.Empty;
       public string RobotMachine = string.Empty;
       public string RobotPassword = string.Empty;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
              
                ReadUserDetails();
                PopulateNotes();
                Page.ClientScript.RegisterStartupScript(this.GetType(), "CallMyFunction", "openvm()", true);
            }
            GetRobotDetails();
        }

        //protected override void OnLoadComplete(EventArgs e)
        //{
        //    base.OnLoadComplete(e);
        //    OpenNewWindow(this, "https://google.com", "key");
        //}

        protected void GetRobotDetails()
        {
            // instantiate XmlDocument and load XML from file
            XmlDocument doc = new XmlDocument();
            //    doc.Load(@"C:\CloudBot\Executor2LayeredArchitecture-master\executor2layeredarchitecture\FrontOfficeBotWebApp\UpdateDetails.xml");
            doc.Load(ConfigurationManager.AppSettings["RobotConfigLoc"]);
            // get a list of nodes - in this case, I'm selecting all <AID> nodes under
            // the <GroupAIDs> node - change to suit your needs
            XmlNodeList cNodes = doc.DocumentElement.ChildNodes;

            // loop through all AID nodes
            foreach (XmlNode cNode in cNodes)
            {
                //adding logic for employee profile details

                if (cNode.Name == "ip")
                {
                    RobotIP = cNode.InnerText;
                }
                else if (cNode.Name == "server")
                {
                    RobotServer = cNode.InnerText;
                }
                else if (cNode.Name == "domain")
                {
                  RobotDomain = cNode.InnerText;
                }
                else if (cNode.Name == "machine")
                {
                    RobotMachine = cNode.InnerText;
                }
                else if (cNode.Name == "password")
                {
                   RobotPassword = cNode.InnerText;
                }

            }
        }

        private void SaveNotes()
        {
            string Feedback = Request.Form["Feedback"];
            string Complaint = Request.Form["Complaint"];

            //  string strNotesFilePath = @"C:\CloudBot\Executor2LayeredArchitecture-master\executor2layeredarchitecture\FrontOfficeBotWebApp\Notes.xml";
            string strNotesFilePath = ConfigurationManager.AppSettings["NotesLoc"];

            //string strCompaintFilePath = strFolderPath + "Complaint.xml";
            try
                {
                    if (string.IsNullOrEmpty(Feedback) && string.IsNullOrEmpty(Complaint))
                    {
                        MessageBox.Show("Please enter Feedback/Complaint.");
                    }
                    else
                    {
                        if (File.Exists(strNotesFilePath))
                        {
                            XmlDocument doc = new XmlDocument();
                            doc.Load(strNotesFilePath);

                            XmlNode xn = doc.DocumentElement;

                            XmlElement elemNotes = doc.CreateElement("Notes");

                            XmlElement elemFD = doc.CreateElement("Feedback");
                            if (!string.IsNullOrEmpty(Feedback))
                                elemFD.InnerText = Feedback;

                            elemNotes.AppendChild(elemFD);

                            XmlElement elemC = doc.CreateElement("Complaint");
                            if (!string.IsNullOrEmpty(Complaint))
                                elemC.InnerText = Complaint;

                            XmlElement elemD = doc.CreateElement("LoggedDate");
                            elemD.InnerText = System.DateTime.Now.ToString("ddd MM yyyy hh:mm");
                            elemNotes.AppendChild(elemD);

                            elemNotes.AppendChild(elemC);

                            xn.AppendChild(elemNotes);

                            doc.Save(strNotesFilePath);
                        }
                        else
                        {
                            using (XmlWriter writer = XmlWriter.Create(strNotesFilePath))
                            {
                                writer.WriteStartElement("Details");
                                writer.WriteStartElement("Notes");
                                writer.WriteElementString("Feedback", Feedback);
                                writer.WriteElementString("Complaint", Complaint);
                                writer.WriteElementString("LoggedDate", System.DateTime.Now.ToString("ddd MM yyyy hh:mm"));
                                writer.WriteEndElement();
                                writer.WriteEndElement();
                                writer.Flush();
                            }
                        }
                    }

                   
                }
                catch (Exception ex)
                {
                    
                }
            }
        

        private void PopulateNotes()
        {


            try
            {
                // get fully qualified path to the "books" xml document located
                // in the xml directory
                //  xmlFilename = Server.MapPath("xml") + "\\books.xml";
              //  xmlFilename = @"C:\CloudBot\Executor2LayeredArchitecture-master\executor2layeredarchitecture\FrontOfficeBotWebApp\Notes.xml";
                xmlFilename = ConfigurationManager.AppSettings["NotesLoc"];
                // create a dataset and load the books xml document into it
                dSet = new DataSet();
                dSet.ReadXml(xmlFilename);

                // bind the dataset to the datagrid
                dgBooks.DataSource = dSet.Tables[BOOK_TABLE];
                dgBooks.DataBind();
            }  // try
            catch (Exception ex)
            { 
            
            
            }
            finally
            {
                // cleanup
                if (dSet != null)
                {
                    dSet.Dispose();
                }
            }  //

        }

        private void ReadUserDetails()
        {
          

            // instantiate XmlDocument and load XML from file
            XmlDocument doc = new XmlDocument();
            //    doc.Load(@"C:\CloudBot\Executor2LayeredArchitecture-master\executor2layeredarchitecture\FrontOfficeBotWebApp\UpdateDetails.xml");
            doc.Load(ConfigurationManager.AppSettings["UpdateDetailsLoc"]);
            // get a list of nodes - in this case, I'm selecting all <AID> nodes under
            // the <GroupAIDs> node - change to suit your needs
            XmlNodeList cNodes = doc.DocumentElement.ChildNodes;

            // loop through all AID nodes
            foreach (XmlNode cNode in cNodes)
            {
                //adding logic for employee profile details

                if (cNode.Name == "EmpName")
                {
                    lblempName.Text = cNode.InnerText;
                }
                else if (cNode.Name == "DASID")
                {
                    lblempid.Text = cNode.InnerText;
                }
                else if (cNode.Name == "Country")
                {
                    lblCountry.Text = cNode.InnerText;
                }
                else if (cNode.Name == "EmplType")
                {
                    lblEmployeeType.Text = cNode.InnerText;
                }
                else if (cNode.Name == "Manager")
                {
                    lblManager.Text = cNode.InnerText;
                }
                else if (cNode.Name == "InnoWiseMainOffice")
                {
                    lblOffice.Text = cNode.InnerText;
                }
                else if (cNode.Name == "OrganizationalUnit")
                {
                    lblOrganisationalunit.Text = cNode.InnerText;
                }
                else if (cNode.Name == "SubDivision")
                {
                    lblSubDivision.Text = cNode.InnerText;
                }
                else if (cNode.Name == "Address")
                {
                    lblAddress.Text = cNode.InnerText;
                }
                //end


                else if (cNode.Name == "FirstName")
                {
                      Fname.Text = cNode.InnerText;
                }
                else if (cNode.Name == "LastName")
                {
                    Lname.Text = cNode.InnerText;
                }
                else if (cNode.Name == "MobileNo")
                {
                   MobileNo.Text = cNode.InnerText;
                }
                else if (cNode.Name == "OfficeRoomNo")
                {
                    OfficeNo.Text = cNode.InnerText;
                }
                else if (cNode.Name == "Interest")
                {
                    Interest.Text = cNode.InnerText;
                }
               
            }

 

        }


        protected void EditDetails_Click(object sender, EventArgs e)
        {
           
            //string Fn = Request.Form["Fname"];
            //string Ln = Request.Form["Lname"];
            //string MobNo = Request.Form["MobileNo"];
            //string OfficeNo = Request.Form["OfficeNo"];
            //string Interest = Request.Form["Interest"];

            // instantiate XmlDocument and load XML from file
            XmlDocument doc = new XmlDocument();
            // doc.Load(@"C:\CloudBot\Executor2LayeredArchitecture-master\executor2layeredarchitecture\FrontOfficeBotWebApp\UpdateDetails.xml");
            doc.Load(ConfigurationManager.AppSettings["UpdateDetailsLoc"]);
             // get a list of nodes - in this case, I'm selecting all <AID> nodes under
             // the <GroupAIDs> node - change to suit your needs
             XmlNodeList cNodes = doc.DocumentElement.ChildNodes;
         
            // loop through all AID nodes
            foreach (XmlNode cNode in cNodes)
            {
                //for first name and last name in profile
                if (cNode.Name == "EmpName")
                {
                    cNode.InnerText = Fname.Text+" "+Lname.Text;
                }
             

                //end


                else if (cNode.Name == "FirstName")
                {
                    cNode.InnerText = Fname.Text;
                }
                else if (cNode.Name == "LastName")
                {
                    cNode.InnerText = Lname.Text;
                }
                else if (cNode.Name == "MobileNo")
                {
                    cNode.InnerText = MobileNo.Text;
                }
                else if (cNode.Name == "OfficeRoomNo")
                {
                    cNode.InnerText = OfficeNo.Text;
                }
                else if (cNode.Name == "Interest")
                {
                    cNode.InnerText = Interest.Text;
                }
                //// grab the "id" attribute
                //XmlAttribute idAttribute = aNode.Attributes["id"];

                //// check if that attribute even exists...
                //if (idAttribute != null)
                //{
                //    // if yes - read its current value
                //    string currentValue = idAttribute.Value;

                //    // here, you can now decide what to do - for demo purposes,
                //    // I just set the ID value to a fixed value if it was empty before
                //    if (string.IsNullOrEmpty(currentValue))
                //    {
                //        idAttribute.Value = "515";
                //    }
                //}
            }

            // save the XmlDocument back to disk
            // doc.Save(@"C:\CloudBot\Executor2LayeredArchitecture-master\executor2layeredarchitecture\FrontOfficeBotWebApp\UpdateDetails.xml");
            doc.Save(ConfigurationManager.AppSettings["UpdateDetailsLoc"]);

        }

        protected void SubmitFeedback_Click(object sender, EventArgs e)
        {
            SaveNotes();
            PopulateNotes();
        }

        //public static void OpenNewWindow(System.Web.UI.Page page, string fullUrl, string key)
        //{
        //    string script = "window.open('" + fullUrl + "', '" + key + "', 'status=1,location=1,menubar=1,resizable=1,toolbar=1,scrollbars=1,titlebar=1');";
        //    page.ClientScript.RegisterClientScriptBlock(page.GetType(), key, script, true);
        //}

    }
}