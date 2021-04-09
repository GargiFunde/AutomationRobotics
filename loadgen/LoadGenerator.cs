  using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Net;
    using System.Net.Mail;
    using System.Threading;
    using System.Web.Script.Serialization;
    using System.Windows.Forms;
    using System.Data;
    using System.Xml;
    using LoggingUtility;
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;
    using RabbitMQ.Client;
    using S22.Imap;
    using System.Text;
using CommonLibrary;
using Microsoft.VisualBasic;


namespace DataConnectorToGenerateRequest
{
    public class LoadGenerator : ILoadGenerator
    {
        System.Timers.Timer tmr_RequestGenerator = new System.Timers.Timer();
        //Outlook.NameSpace outlookNameSpace;
        //Outlook.MAPIFolder inbox;
        //Outlook.Items items;
        //Outlook.Application OutlookApplication;

        #region Variables
        Dictionary<string, string> dict = new Dictionary<string, string>();
        string[] values = new string[2];
        public string file_path;
        public string emailSubject;
        public string rest_api_url;
        public string req_exp_timeout;
        public string profile;
        public string process;
        public string app_id;
        public string source_id;
        public string selected;
        public string selectedFile;
        public string fileType;
        public string sourceModule = "RPAPOC.LoadGenerator";
        public string Log_file_Name;
        public string NoOfFailReq;
        public int requestcount = 0;
        public string userid = string.Empty;
        public uint cnt;
        public int count;
        public int queuecount = 0;
        public int queuethresholdcount = 300;
        public DateTime startTime;
        public DateTime endTime;
        #endregion

        #region Interface Methods

        public void ReadData()
        {
            try
            {
                startTime = DateTime.Now;
                read_app_param();
                DirectoryInfo di = new DirectoryInfo(file_path);
                FileInfo[] TXTFiles = di.GetFiles();// ("*.xlsx");

                if (TXTFiles.Length == 0)
                {
                    Console.WriteLine("file is not present");
                }
                else
                {

                    int min = 999;
                    //= new FileInfo();
                    foreach (FileInfo t in TXTFiles)
                    {
                        string fileName = System.IO.Path.GetFileName(t.Name);
                        string extension = System.IO.Path.GetExtension(fileName);

                        if (extension == ".xlsx")
                            fileType = "Excel";
                        else if (extension == ".xml")
                            fileType = "Xml";
                        else if (extension == ".pdf")
                            fileType = "PDF";
                        else if (extension == ".docx")
                            fileType = "Word";

                        selected = t.Name;
                    }
                    selectedFile = Path.Combine(file_path, selected);
                    System.Timers.Timer _requesttimeout = new System.Timers.Timer();
                    _requesttimeout.Interval = 10000;
                    _requesttimeout.Enabled = true;
                    _requesttimeout.Elapsed += _requesttimeout_Elapsed;

                    Console.WriteLine("Load Generator Started at : {0}", DateTime.Now.ToShortTimeString());

                    //Logger.LogMessage(LogLevel.Info, "ReadData:Started", "ReadData method Executing", Log_file_Name);
                    //IDictionary<string, string> dataDict;

                    switch (fileType)
                    {
                        case "Excel":
                         //   ReadExcelData();
                            break;
                        case "Class":
                           // ReadClassData();
                            break;
                        case "Xml":
                        //    ReadXml();
                            break;
                        case "PDF":
                          //  ReadWordorPDF();
                            break;
                        case "Word":
                          //  ReadWordorPDF();
                            break;
                        case "Email":
                            ReadEmail();
                            break;
                        default:
                            break;
                    }


                }
                Console.ReadKey();

            }
            catch (Exception ex)
            {
                //Logger.LogMessage(LogLevel.Error, "ReadData", ex.Message, Log_file_Name);
                Console.WriteLine(ex.Message);
                Console.ReadKey();
            }
        }

        //public void PrepareJsonAndAutomation(Dictionary<string, string> dict, string[] values)
        //{
        //    try
        //    {

        //        int Errormsg = 0;
        //        int fallout = 0;
        //        PerformAutomation performAutomation = new PerformAutomation(process, profile);

        //        performAutomation.AutomationInputDictionary = dict;

        //        var dict1 = new Dictionary<string, object>();
        //        performAutomation.UserName = userid;
        //        dict1.Add("PerformAutomation", performAutomation);
        //        var postString = new PostString(profile, app_id, source_id, values, Convert.ToInt32(req_exp_timeout));
        //        postString.payload = new JavaScriptSerializer().Serialize(dict1);

        //        var postStringJson = new JavaScriptSerializer().Serialize(postString);
        //        Logger.LogMessage(LogLevel.Info, "Main:IsValidJson", "Is JSON valid Checking", Log_file_Name);

        //        if (IsValidJson(postStringJson))
        //        {
        //            Console.WriteLine("Sending Request at : {0}", DateTime.Now.ToShortTimeString());
        //            Logger.LogMessage(LogLevel.Info, "Main:IsValidJson", "JSON valid", Log_file_Name);
        //            Logger.LogMessage(LogLevel.Info, "Main:Consume Service Method called", "Sending request to RabbitMQ", Log_file_Name);
        //            var resp = ConsumeService(rest_api_url, postStringJson);
        //            Logger.LogMessage(LogLevel.Info, "Main:Response Generated", "Response Generated", Log_file_Name);

        //            while (queuecount > queuethresholdcount)
        //            {
        //                //Thread.Sleep(60000);

        //            }
        //            if (resp.Contains("error_msg"))
        //            {
        //                //Thread.Sleep(10000);
        //                //_requesttimeout.Enabled = true;
        //                Errormsg = count - 1;
        //                if (count - Errormsg == 1)
        //                {
        //                    Errormsg = count;
        //                    fallout++;
        //                }
        //                else
        //                {
        //                    fallout = 0;
        //                }
        //            }
        //            else
        //            {
        //                fallout = 0;
        //                //break;
        //            }
        //            count++;
        //            Console.WriteLine(resp);
        //            if (fallout == 4) //(Convert.ToInt32(NoOfFailReq)))
        //            {
        //                endTime = DateTime.Now;
        //                //Logger.LogMessage(LogLevel.Info, "Main:Request Processed", "No of Responses:" + count, Log_file_Name);
        //                MailSend();

        //            }
        //        }
        //        else
        //        {
        //            Logger.LogMessage(LogLevel.Error, "Main:IsValidJson", "JSON not Valid", Log_file_Name);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine(ex.Message);
        //        Console.ReadKey();
        //    }
        //}

        //public string ConsumeService(string path, string paramJson)
        //{
        //    string response = string.Empty;
        //    try
        //    {
        //        Logger.LogMessage(LogLevel.Info, "In ConsumeService Method", "Started ConsumeService Method", Log_file_Name);

        //        using (WebClient client = new WebClient())
        //        {
        //            client.Credentials = new NetworkCredential(rmq_user, rmq_pass);
        //            client.Headers.Set("Content-Type", "application/json");
        //            response = client.UploadString(path, paramJson);
        //        }
        //        Logger.LogMessage(LogLevel.Info, "In ConsumeService Method", "Ended ConsumeService Method", Log_file_Name);

        //    }
        //    catch (WebException ex)
        //    {
        //        Console.WriteLine(ex.Message);
        //        Console.ReadKey();
        //        response = string.Format("Error : {0}", ex.Message);
        //        //Logging.LogMessageLogInfoMsg(sourceModule, System.Reflection.MethodInfo.GetCurrentMethod().Name, "Failed in Consume Service");
        //        Logger.LogMessage(LogLevel.Error, "ConsumeService:Catch", "Failed in Consume Service" + ex.Message, Log_file_Name);

        //    }
        //    return response;
        //}

        #endregion

        public PublishSubscribe publishSubscribe { get; set; }
        public LoadGenerator(PublishSubscribe publishSub)
        {
          //  GmailProcess();
           // int requestScheduleInterval = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["requestScheduleInterval"]);
          
            //tmr_RequestGenerator.Elapsed += tmr_RequestGenerator_Elapsed;
            //tmr_RequestGenerator.Enabled = true;
            //tmr_RequestGenerator.Start();
            //tmr_RequestGenerator.Interval = requestScheduleInterval * 1000;
            //publishSubscribe = publishSub;
            //publishSubscribe.AttachListener();
           
          //  Gmail1Process();
           
        }
        void tmr_RequestGenerator_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {

            Dictionary<string, object> obj = new Dictionary<string, object>();
            obj.Add("Input1","Input1");
           

            ServiceReference1.IBOTService AutomationRequest = new ServiceReference1.BOTServiceClient();

            RequestInput _requestInput = new RequestInput();

            _requestInput.TenantName = "";
            _requestInput.AutomationGroupName = "Group1";
            _requestInput.AutomationProcessName = "WebAutomation";
            List<string> lstSearchParam = new List<string>();
            lstSearchParam.Add("SearchField1");
            lstSearchParam.Add("TN000123");
            _requestInput.InputSearchParameters = lstSearchParam;
            _requestInput.RequestNumber = "100";
            _requestInput.RequestTimeoutSLAInSeconds = 100000;
            _requestInput.Input = obj;
            ServiceReference1.BOTServiceClient client = new ServiceReference1.BOTServiceClient();

            //Commenting for test
            //client.AutomationRequest(_requestInput);
            //GmailProcess();
        }
        #region Private Methods
        void _requesttimeout_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            var factory = new ConnectionFactory() { /*HostName = "172.17.150.195"*/ HostName="localhost", UserName = "se", Password = "se", Port = 5672 };
            try
            {
                using (var connection = factory.CreateConnection())
                {
                    using (var channel = connection.CreateModel())
                    {

                        BasicGetResult result = channel.BasicGet("ev.ae.se.q.automation.CreditMemo", false);
                        cnt = result != null ? result.MessageCount : 0;
                        queuecount = Convert.ToInt32(cnt);

                    }
                }
            }
            catch (Exception ex)
            {
                //LogMessage(ex.Message);
            }

        }

        //private bool IsValidJson(string strInput)
        //{
        //    try
        //    {
        //        Logger.LogMessage(LogLevel.Info, "In IsValidJson Method", "Started IsValidJson Method", Log_file_Name);

        //        strInput = strInput.Trim();
        //        if ((strInput.StartsWith("{") && strInput.EndsWith("}")) || //For object
        //            (strInput.StartsWith("[") && strInput.EndsWith("]"))) //For array
        //        {
        //            try
        //            {
        //                var obj = JToken.Parse(strInput);
        //                Logger.LogMessage(LogLevel.Info, "In IsValidJson Method", "Ended IsValidJson Method", Log_file_Name);

        //                return true;
        //            }
        //            catch (JsonReaderException jex)
        //            {
        //                //Exception in parsing json
        //                Console.WriteLine(jex.Message);
        //                Logger.LogMessage(LogLevel.Error, "In IsValidJson Method", "Started IsValidJson Method" + jex.Message, Log_file_Name);

        //                return false;

        //            }
        //            catch (Exception ex) //some other exception
        //            {
        //                Console.WriteLine(ex.ToString());
        //                Logger.LogMessage(LogLevel.Error, "In IsValidJson Method", "Started IsValidJson Method" + ex.Message, Log_file_Name);

        //                return false;
        //            }
        //        }
        //        else
        //        {
        //            Logger.LogMessage(LogLevel.Error, "In IsValidJson Method", "Started IsValidJson Method", Log_file_Name);

        //            return false;
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        //Logging.LogMessageLogInfoMsg(sourceModule, System.Reflection.MethodInfo.GetCurrentMethod().Name, "Failed in JSON Validation of request");
        //        Logger.LogMessage(LogLevel.Error, "IsValidJson:Catch", "Failed in Checking of Valid JSON" + ex.Message, Log_file_Name);

        //        return false;
        //    }
        //}

        /// <summary>
        /// Use this method to configure mails
        /// </summary>
        private void MailSend()
        {
            //MailMessage msg = null;
            {
                try
                {
                    //Logger.LogMessage(LogLevel.Info, "MailSendDaily:Started", "Execution of MailSendDaily method", Log_file_Name);
                    //msg = new MailMessage("rajiv_mishra03@infosys.com", "Rohit_Pawar@edgeverve.com");
                    //msg.Subject = string.Format("Load Generator Status", "{0}");

                    //string BodyText = string.Empty;
                    //int FileRecords = 0;
                    // string filePath = Path.Combine(Environment.CurrentDirectory, Log_file_Name + "_" + DateTime.Now.ToString("dd-MM-yyy") + ".log");
                    // if (File.Exists(filePath))
                    // {
                    //     System.Net.Mail.Attachment atchItem = new System.Net.Mail.Attachment(filePath);
                    //     FileRecords = File.ReadAllLines(filePath).Length;
                    //     msg.Attachments.Add(atchItem);
                    // }
                    // string str = string.Empty;
                    // BodyText = str;


                    // SmtpClient smtp = new System.Net.Mail.SmtpClient("blrkeccas.ad.infosys.com");
                    // smtp.Credentials = CredentialCache.DefaultNetworkCredentials;

                    // smtp.Send(msg);
                    int elapsedTime = Convert.ToInt32(endTime.Millisecond - startTime.Millisecond);
                    string countFileName = Path.Combine(Environment.CurrentDirectory, Log_file_Name + "_" + DateTime.Now.ToString("dd-MM-yyy") + ".txt");
                    if (!File.Exists(countFileName))
                    {
                        var fs = File.Create(countFileName);
                        fs.Close();
                    }
                    using (var sw = new StreamWriter(countFileName, true))
                    {
                        sw.WriteLine("StartTime" + "-" + startTime);
                        sw.WriteLine("EndTime" + "-" + endTime);
                        sw.WriteLine("ElapsedTime" + "-" + elapsedTime);
                        sw.WriteLine("No. of Responces" + "-" + Convert.ToString(count - 1));
                        sw.Flush();
                        sw.Close();
                    }
                    //Logger.LogMessage(LogLevel.Info, "MailSendDaily:Ended", "Execution of MailSendDaily method", Log_file_Name);
                }
                catch (Exception ex)
                {
                    //Logger.LogMessage(LogLevel.Error, "MailSendDaily", "Exception in MailSendDaily method", Log_file_Name);
                }
            }


        }

        private void read_app_param()
        {
           
            try
            {
                Log_file_Name = System.Configuration.ConfigurationManager.AppSettings["Log_file_Name"];
                emailSubject = System.Configuration.ConfigurationManager.AppSettings["emailSubject"];
                //Logger.LogMessage(LogLevel.Info, "In read_app_param Method", "Started read_app_param Method", Log_file_Name);
                file_path = System.Configuration.ConfigurationManager.AppSettings["file_path"];
                userid = System.Configuration.ConfigurationManager.AppSettings["userid"];
                rest_api_url = System.Configuration.ConfigurationManager.AppSettings["rest_api_url"];
                req_exp_timeout = System.Configuration.ConfigurationManager.AppSettings["req_exp_timeout"];
                profile = System.Configuration.ConfigurationManager.AppSettings["profile"];
                process = System.Configuration.ConfigurationManager.AppSettings["process"];
                app_id = System.Configuration.ConfigurationManager.AppSettings["app_id"];
                source_id = System.Configuration.ConfigurationManager.AppSettings["source_id"];
                //rmq_user = System.Configuration.ConfigurationManager.AppSettings["rmq_user"];
                //rmq_pass = System.Configuration.ConfigurationManager.AppSettings["rmq_pass"];
                NoOfFailReq = (System.Configuration.ConfigurationManager.AppSettings["NoOfFailReq"]);
                requestcount = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["requestcount"]);
                queuethresholdcount = Convert.ToInt32(System.Configuration.ConfigurationManager.AppSettings["queuethresholdcount"]);
                Console.WriteLine(Log_file_Name);
                //Logger.LogMessage(LogLevel.Info, "In read_app_param Method", "Ended read_app_param Method", Log_file_Name);

            }
            catch (Exception ex)
            {
                //Logger.LogMessage(LogLevel.Error, "read_app_param:Catch", "Failed in reading App.Config" + ex.Message, Log_file_Name);
                Console.WriteLine(ex.Message);
                Console.ReadKey();
                //Logging.LogMessageLogInfoMsg(sourceModule, System.Reflection.MethodInfo.GetCurrentMethod().Name, "Failed in Read Application Parameters in App.config ");
            }
        }

        /// <summary>
        ///  Add code for file watcher
        /// </summary>
        private void WatchFile()
        {
            FileSystemWatcher watcher = new FileSystemWatcher();
            watcher.Path = file_path;
            watcher.NotifyFilter = NotifyFilters.LastWrite;
            watcher.Filter = "*.*";
            watcher.Changed += new FileSystemEventHandler(OnChanged);
            watcher.EnableRaisingEvents = true;
        }

        //private void ReadXml()
        //{
        //    try
        //    {
        //        string[] values = new string[2];
        //        XmlReader xmlFile;
        //        xmlFile = XmlReader.Create(selectedFile, new XmlReaderSettings());
        //        DataSet ds = new DataSet();
        //        ds.ReadXml(xmlFile);
        //        int i, j = 0;
        //        for (i = 0; i <= ds.Tables[0].Rows.Count - 1; i++)
        //        {
        //            Dictionary<string, string> DictforExtraParam = new Dictionary<string, string>();
        //            Dictionary<string, string> dict = new Dictionary<string, string>();

        //            for (j = 0; j < ds.Tables[0].Columns.Count; j++)
        //            {
        //                values[j] = ds.Tables[0].Rows[i].ItemArray[j].ToString();
        //                dict.Add("Search.Data" + j + 1, values[j]);
        //            }

        //            PrepareJsonAndAutomation(dict, values);
        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        Logger.LogMessage(LogLevel.Error, "ReadXml", "Problem in reading file" + ex.Message, Log_file_Name);
        //        Console.WriteLine(ex.Message);
        //        Console.ReadKey();
        //    }
        //}
        //public void ReadClassData()
        
        public void ReadClassData(string InputTenantName,string InputProcessName,string InputGroupName)
        {
            try
            {
                Dictionary<string, object> obj = new Dictionary<string, object>();
                obj.Add("Input1", "Input1");
                ServiceReference1.BOTServiceClient AutomationRequest = new ServiceReference1.BOTServiceClient();
                RequestInput _requestInput = new RequestInput();

                //_requestInput.TenantName = "Self";
                //_requestInput.TenantName = "InnoWise";
                _requestInput.TenantName = InputTenantName;
                //_requestInput.AutomationGroupName = "Group2";
                //_requestInput.AutomationGroupName = "Default";
                //_requestInput.AutomationProcessName = "Piyush";
                _requestInput.AutomationGroupName = InputGroupName;
                _requestInput.AutomationProcessName = InputProcessName;
                List<string> lstSearchParam = new List<string>();
                lstSearchParam.Add("SearchField1");
                lstSearchParam.Add("Infosys Ltd.");
                _requestInput.InputSearchParameters = lstSearchParam;
                _requestInput.RequestNumber = "100";
                _requestInput.Input = obj;
                _requestInput.RequestTimeoutSLAInSeconds = 100000;
                ServiceReference1.BOTServiceClient client = new ServiceReference1.BOTServiceClient();
                client.AutomationRequest(_requestInput);

                //Commenting for test
                //client.AutomationRequest(_requestInput);
                //client.AutomationRequest(_requestInput);

                //Logger.Log.Logger.LogData("Queue Interface Error", Logger.LogLevel.Error);

                //_requestInput = new RequestInput();

                //_requestInput.TenantName = "Self";
                //_requestInput.AutomationGroupName = "Group1";
                //_requestInput.AutomationProcessName = "Web";

                //lstSearchParam = new List<string>();
                //lstSearchParam.Add("SearchField1");
                //lstSearchParam.Add("Wipro Ltd.");
                //_requestInput.InputSearchParameters = lstSearchParam;
                //_requestInput.RequestNumber = "100";
                //_requestInput.RequestTimeoutSLAInSeconds = 100000;

                //client.AutomationRequest(_requestInput);


                //_requestInput = new RequestInput();

                //_requestInput.TenantName = "Self";
                //_requestInput.AutomationGroupName = "Group1";
                //_requestInput.AutomationProcessName = "Web";
                //lstSearchParam = new List<string>();
                //lstSearchParam.Add("SearchField1");
                //lstSearchParam.Add("Tata Consultancy Services Ltd.");
                //_requestInput.InputSearchParameters = lstSearchParam;
                //_requestInput.RequestNumber = "100";
                //_requestInput.RequestTimeoutSLAInSeconds = 100000;
                //client.AutomationRequest(_requestInput);
                //_requestInput = new RequestInput();
                //_requestInput.TenantName = "Self";
                //_requestInput.AutomationGroupName = "Group1";
                //_requestInput.AutomationProcessName = "Web";
                //lstSearchParam = new List<string>();
                //lstSearchParam.Add("SearchField1");
                //lstSearchParam.Add("Tech Mahindra Ltd.");
                //_requestInput.InputSearchParameters = lstSearchParam;
                //_requestInput.RequestNumber = "100";
                //_requestInput.RequestTimeoutSLAInSeconds = 100000;
                //client.AutomationRequest(_requestInput);

            }
            catch (Exception ex)
            { }
        }
        //private void ReadExcelData()
        //{
        //    count = 0;

        //    Microsoft.Office.Interop.Excel.Application xlApp;
        //    Microsoft.Office.Interop.Excel.Workbook xlWorkBook;
        //    Microsoft.Office.Interop.Excel.Worksheet xlWorkSheet;
        //    Microsoft.Office.Interop.Excel.Range range;
        //    xlApp = new Microsoft.Office.Interop.Excel.Application();
        //    xlWorkBook = xlApp.Workbooks.Open(selectedFile, 0, true, 5, "", "", true, Microsoft.Office.Interop.Excel.XlPlatform.xlWindows, "\t", false, false, 0, true, 1, 0);
        //    xlWorkSheet = (Microsoft.Office.Interop.Excel.Worksheet)xlWorkBook.Worksheets.get_Item(1);

        //    string str;
        //    int rCnt = 0;
        //    int cCnt = 0;

        //    try
        //    {


        //        range = xlWorkSheet.UsedRange;

        //        for (rCnt = 2; rCnt <= range.Rows.Count; rCnt++)
        //        {
        //            if (requestcount != 0)
        //            {
        //                //if (count > requestcount)
        //                //    break;
        //            }
        //            //Change this code as per requirement
        //            values[0] = (string)(range.Cells[rCnt, 1] as Microsoft.Office.Interop.Excel.Range).Value2.ToString();
        //            //values[1] = (string)(range.Cells[rCnt, 2] as Microsoft.Office.Interop.Excel.Range).Value2.ToString();

        //            Dictionary<string, string> dict1 = new Dictionary<string, string>();
        //            dict1.Add("Search.Ban", values[0]);

        //            PrepareJsonAndAutomation(dict1, values);
        //            count++;

        //        }

        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine(ex.Message);
        //        Console.ReadKey();
        //        Logger.LogMessage(LogLevel.Error, "ReadExcelData", "Problem in reading file" + ex.Message, Log_file_Name);
        //    }



        //    Logger.LogMessage(LogLevel.Info, "Main:Request Processed", "No of Responses:" + count, Log_file_Name);


        //    xlWorkBook.Close(true, null, null);
        //    xlApp.Quit();
        //}

        private void OnChanged(object source, FileSystemEventArgs e)
        {
            ReadData();
        }

        private void ReadEmail()
        {
            try
            {
                //OutLook.Application oApp;
                //OutLook._NameSpace oNS;
                //OutLook.MAPIFolder oFolder;
                //OutLook._Explorer oExp;

                //oApp = new OutLook.Application();
                //oNS = (OutLook._NameSpace)oApp.GetNamespace("MAPI");
                //oFolder = oNS.GetDefaultFolder(OutLook.OlDefaultFolders.olFolderInbox);
                //oExp = oFolder.GetExplorer(false);
                //oNS.Logon(null, null, false, true);

                //Microsoft.Office.Interop.Outlook.MailItem mail = null;
                //OutLook.Items items = oFolder.Items;//.Restrict("[Unread] = true");
                //string emailBody = null;
                //string[] splitters = new string[] { "link: " };
                //for (int i = 1; i <= items.Count; i++)
                //{
                //    mail = items[i] as OutLook.MailItem;
                //    if (mail != null)
                //    {
                //        if (mail.Subject.Contains(emailSubject))
                //        {
                //            emailBody = mail.Body;
                //        }
                //    }
                //}
            }
            catch (Exception ex)
            {
                //throw;
            }
        }

        //private void ReadWordorPDF()
        //{
        //    count = 0;
        //    Document srcDoc;
        //    Microsoft.Office.Interop.Word.Application application = new Microsoft.Office.Interop.Word.Application();
        //    try
        //    {
        //        FileInfo fileInfo = new FileInfo(selectedFile);
        //        DataSet ds = new DataSet();
        //        object filename = fileInfo.FullName;
        //        object confirmConversion = false;
        //        object readOnly = true;
        //        object visible = false;
        //        object skipEncodingDialog = true;
        //        object missing = System.Reflection.Missing.Value;


        //        application.Visible = false;
        //        application.DisplayAlerts = WdAlertLevel.wdAlertsNone;
        //        srcDoc = application.Documents.Open(ref filename, ref confirmConversion, ref readOnly, ref missing,
        //           ref missing, ref missing, ref missing, ref missing,
        //           ref missing, ref missing, ref missing, ref visible,
        //           ref missing, ref missing, ref skipEncodingDialog, ref missing);
        //        String read = string.Empty;
        //        List<string> data = new List<string>();
        //        for (int i = 0; i < srcDoc.Paragraphs.Count; i++)
        //        {
        //            string temp = srcDoc.Paragraphs[i + 1].Range.Text.Trim();
        //            if (temp != string.Empty)
        //                values[i] = temp;
        //            dict.Add("Search.EmpID", values[0]);


        //            PrepareJsonAndAutomation(dict, values);
        //            count++;
        //        }

        //        srcDoc.Close();

        //    }
        //    catch (Exception ex)
        //    {
        //        Logger.LogMessage(LogLevel.Error, "ReadWordorPDF", "Problem in reading file" + ex.Message, Log_file_Name);
        //        Console.WriteLine(ex.Message);
        //        Console.ReadKey();
        //    }



        //    application.Quit();
        //    Logger.LogMessage(LogLevel.Info, "Main:Request Processed", "No of Responses:" + count, Log_file_Name);

        //}

        #endregion

      
        //public List<string> emailFrom;
        //public List<string> emailBody ;
        //public List<string> emailMessages;
        //public Int16 mailCount = 1;
        //private void GmailProcess()
        //{
        //     emailFrom = new List<string>();
        //     emailBody = new List<string>();
        //     emailMessages = new List<string>();

        //    System.Net.WebClient objClient = new System.Net.WebClient();
        //    XmlNodeList nodelist;
        //    XmlNode node;
        //    string response;
        //    XmlDocument xmlDoc = new XmlDocument();
        //    try
        //    {
        //        System.Net.NetworkCredential Credentials = new System.Net.NetworkCredential("e2erobotic", "dxlufhxgjtfnsymd");
        //        objClient.Credentials = Credentials;
        //        response = Encoding.UTF8.GetString(objClient.DownloadData("https://mail.google.com/mail/feed/atom"));
        //        response = response.Replace("<feed version=\"0.3\" xmlns=\"http://purl.org/atom/ns#\">", "<feed>");

        //        xmlDoc.LoadXml(response);
        //        node = xmlDoc.SelectSingleNode("/feed/fullcount");
        //        mailCount = Convert.ToInt16(node.InnerText);
        //        //Get the number of unread emails
        //        if (mailCount > 0)
        //        {
        //            nodelist = xmlDoc.SelectNodes("/feed/entry");
        //            node = xmlDoc.SelectSingleNode("title");
        //            foreach (XmlNode node1 in nodelist)
        //            {
        //                emailMessages.Add(node1.ChildNodes.Item(0).InnerText);
        //                if ((node1.ChildNodes.Item(0).InnerText.Contains("Action")))
        //                {
        //                    emailBody.Add(node1.ChildNodes.Item(1).InnerText);
        //                }
        //                emailFrom.Add(node1.ChildNodes.Item(6).ChildNodes[0].InnerText);
                        
        //            }
                    
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        //Interaction.MsgBox(ex.Message);
        //    }


        //}

        //ImapClient Client = new ImapClient("imap.gmail.com", 993,
        //    "e2erobotic", "dxlufhxgjtfnsymd", AuthMethod.Login, true);
        private void Gmail1Process()
        {   
            // Should ensure IDLE is actually supported by the server
            //if (Client.Supports("IDLE") == false)
            //{
            //    Console.WriteLine("Server does not support IMAP IDLE");
            //    return;
            //}
            //// We want to be informed when new messages arrive
            //Client.NewMessage += new EventHandler<IdleMessageEventArgs>(OnNewMessage);           
        }
         static void OnNewMessage(object sender, IdleMessageEventArgs e)
        {
            Console.WriteLine("A new message arrived. Message has UID: " +   e.MessageUID);

            // Fetch the new message's headers and print the subject line
            MailMessage m = e.Client.GetMessage( e.MessageUID, FetchOptions.Normal);
            Requestor req = new Requestor();
            req.From = m.From.ToString();
            req.To = m.To.ToString();
            req.Subject = m.Subject;
            FireAutomationRequest(m.Body.Trim(),req);
            Console.WriteLine("New message's subject: " + m.Subject);
        }

         public static Dictionary<string, Requestor> reqData = new Dictionary<string, Requestor>();
         public static void FireAutomationRequest(string strSearch, Requestor req)
        {
            ServiceReference1.IBOTService AutomationRequest = new ServiceReference1.BOTServiceClient();

            RequestInput _requestInput = new RequestInput();
            Dictionary<string, object> obj = new Dictionary<string, object>();
            obj.Add("Input1", "Input1");
            _requestInput.TenantName = "";
            _requestInput.AutomationGroupName = "Group1";
            _requestInput.AutomationProcessName = "Web";
            List<string> lstSearchParam = new List<string>();
            lstSearchParam.Add("SearchField1");
            lstSearchParam.Add(strSearch);
            _requestInput.InputSearchParameters = lstSearchParam;
            _requestInput.RequestNumber = Guid.NewGuid().ToString();
            reqData.Add(_requestInput.RequestNumber, req);
            _requestInput.RequestTimeoutSLAInSeconds = 100000;
            _requestInput.Input = obj;
            ServiceReference1.IBOTService client = new ServiceReference1.BOTServiceClient();

            //Commenting for test
            //client.AutomationRequest(_requestInput);
            //client.AutomationRequest(_requestInput);
        }

    }
    public class Requestor
    {
        public string From { get; set; }
        public string To { get; set; }
        public string Subject { get; set; }
    }
}

