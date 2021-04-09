using CommonLibrary;
using FirstFloor.ModernUI.Presentation;
using Logger;
using Microsoft.Win32;
using MRULib.MRU.ViewModels;
using System;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Navigation;

namespace BOTDesigner.Views
{
    /// <summary>
    /// Interaction logic for DashBoard.xaml
    /// </summary>
    public partial class DashBoard : UserControl
    {
        public event NewProjectEventHandler NewProjectDiagram;                                                                //Event
        public delegate void NewProjectEventHandler(object sender, NewProjectEventArgs e);                      //Delegate
        public event OpenProjectEventHandler OpenProject;                                                                         //Event
        public delegate void OpenProjectEventHandler(object sender, NewProjectEventArgs e);                     //Delegate
        public string ProjectName { get; set; }
        public string Location { get; set; }
        DataTable dtRecent = new DataTable();
        public DashBoard()
        {
            InitializeComponent();
            ReadRecenFilesData();
            dtRecent.TableName = "RecentProjects";
            dtRecent.Columns.Add("ProjectPath");
        }


        protected virtual void OnNewProjectDiagram(NewProjectEventArgs e)
        {
            NewProjectEventHandler handler = NewProjectDiagram;
            if (handler != null)
            {
                handler(this, e);
            }
        }
        private void btnOpen_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var dialogOpen = new OpenFileDialog();
                dialogOpen.Title = "Open Workflow";
                //dialogOpen.Filter = "Project|*.aut|Workflows (.xaml)|*.xaml";
                dialogOpen.Filter = "Project|*.aut";

                if (dialogOpen.ShowDialog() == true)
                {
                    // using (var file = new StreamReader(dialogOpen.FileName, true))
                    //{
                    NewProjectEventArgs ne = new NewProjectEventArgs();
                    ne.Path = dialogOpen.FileName;
                    ne.projectType = ProjectType.openproject;
                    ne.projectFullName = dialogOpen.FileName;
                    OpenProject(sender, ne);
                    // }
                }
            }
            catch (Exception ex)
            {
                Log.Logger.LogData(ex.Message, LogLevel.Error);
            }
        }
        private void btnOpenExplorer_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                NewProjectEventArgs ne = new NewProjectEventArgs();
                ne.projectType = ProjectType.openexplorer;
                OpenProject(sender, ne);
            }
            catch (Exception ex)
            {
                Log.Logger.LogData(ex.Message, LogLevel.Error);
            }
        }
       

        private MRUListViewModel mMruList = null;
        public MRUListViewModel MruList
        {
            get
            {
                return mMruList;
            }
        }
        private void ReadRecenFilesData()
        {
            try
            {
                mMruList = new MRUListViewModel()
                {
                    // Invoke this callback method when a file link is clicked
                    LoadFileCommandDelegate = LoadFile                                                        //Load File Delegate called from here.
                };

                DataTable dt = new DataTable();
                string userfolder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
                userfolder = userfolder + Path.DirectorySeparatorChar + "RecentProjects.xml";
                if (File.Exists(userfolder))
                {
                    dt.ReadXml(userfolder);
                    if (dt.Rows.Count > 0)
                    {
                        // int icount = 1;
                        foreach (DataRow item in dt.Rows)
                        {
                            MruList.AddMRUEntry(item[0].ToString());
                        }
                        DataContext = this;
                        AppearanceManager.Current.ThemeSource = new Uri("/BOTDesigner;component/Pages/Settings/ModernUI.Light.xaml", UriKind.RelativeOrAbsolute);
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Logger.LogData(ex.Message, LogLevel.Error);
            }
        }
        /// <summary>
        /// Is invoked when a file click or Load File is invoked from the MRU control.
        /// </summary>
        /// <param name="filePathName"></param>
        public void LoadFile(string filePathName)
        {
            try
            {
                NewProjectEventArgs ne = new NewProjectEventArgs();
                ne.Path = filePathName;
                ne.projectType = ProjectType.openproject;
                OpenProject(null, ne);
                //MessageBox.Show("Demo Opening File: " + filePathName);
            }
            catch (Exception ex)
            {
                Log.Logger.LogData(ex.Message, LogLevel.Error);
            }
        }
      

        public void AddToRecentTable(string PathFileName)
        {
            try
            {
                DataRow dr = dtRecent.NewRow();
                dr[0] = PathFileName;

                //Start - to remove old duplicate entries
                if (dtRecent.Rows.Count > 0)
                {
                    for (int i = dtRecent.Rows.Count - 1; i >= 0; i--)
                    {
                        DataRow drOld = dtRecent.Rows[i];
                        if (drOld[0].ToString() == PathFileName)
                        {
                            dr.Delete();
                            MruList.RemoveEntry(PathFileName);
                        }
                    }
                }
                //End - to remove old duplicate entries
                dtRecent.Rows.Add(dr);
                
                MruList.AddMRUEntry(PathFileName);
                               
                DataContext = this;
            }
            catch (Exception ex)
            {
                Log.Logger.LogData(ex.Message, LogLevel.Error);
            }
        }
        public void CreateRecentTableFile()
        {
            try
            {

                //foreach (MRUEntryViewModel item in MruList.ListOfMRUEntries)
                //{
                //    DataRow dr = dtRecent.NewRow();
                //    dr[0] = item.PathFileName;
                //    dtRecent.Rows.Add(dr);
                //}
                DataTable dt = dtRecent.Clone();
                int iCount = 0;

                for (int i = MruList.ListOfMRUEntries.Count - 1; i >= 0; i--)
                {
                    iCount = iCount + 1;
                    DataRow dr = dt.NewRow();
                    dr[0] = MruList.ListOfMRUEntries[i].PathFileName;
                    dt.Rows.Add(dr);
                    if (iCount > 10)
                    {
                        dt.Rows.RemoveAt(0);
                    }
                }
                for (int i = 0; i < dtRecent.Rows.Count; i++)
                {

                    iCount = iCount + 1;
                    DataRow dr = dt.NewRow();
                    dr[0] = dtRecent.Rows[i][0];
                    dt.Rows.Add(dr);
                    if (iCount > 10)
                    {
                        dt.Rows.RemoveAt(0);
                    }
                }
                string userfolder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
                dt.WriteXml(userfolder + Path.DirectorySeparatorChar + "RecentProjects.xml", XmlWriteMode.WriteSchema);
            }
            catch (Exception ex)
            {
                Log.Logger.LogData(ex.Message, LogLevel.Error);
            }
        }
        private void btnBlankSeq_Click(object sender, RoutedEventArgs e)
        {
            CreateProject(sender, ProjectType.sequence);
        }
        private void btnBlankWF_Click(object sender, RoutedEventArgs e)
        {
            CreateProject(sender, ProjectType.workflow);
        }
        private void btnBlankSM_Click(object sender, RoutedEventArgs e)
        {
            CreateProject(sender, ProjectType.statemachine);
        }
        private void CreateProject(object sender, ProjectType projectType)
        {
            try
            {
                NewProject newProject = new NewProject("New Project");
                newProject.ShowDialog();
                if (newProject.DialogResult == true)
                {
                    ProjectName = newProject.ProjectName;
                    Location = newProject.Location;

                    Location = Location + Path.DirectorySeparatorChar + ProjectName;
                    if(!Directory.Exists(Location))
                    {
                        Directory.CreateDirectory(Location);
                    }

                    string ProjectFullName = Location + Path.DirectorySeparatorChar + ProjectName + ".aut";
                    File.Create(ProjectFullName);

                    if (!Directory.Exists(Location + Path.DirectorySeparatorChar + "Images"))
                    {
                        Directory.CreateDirectory(Location + Path.DirectorySeparatorChar + "Images");
                    }

                    NewProjectEventArgs ne = new NewProjectEventArgs();
                    ne.Path = Location;
                    ne.projectType = projectType;
                    ne.projectFullName = ProjectFullName;
                    NewProjectDiagram(sender, ne);
                }
            }
            catch (Exception ex)
            {
                Log.Logger.LogData(ex.Message, LogLevel.Error);
            }
        }
        private void btnMain_Click(object sender, RoutedEventArgs e)
        {

        }
        private void Hyperlink_RequestNavigate(object sender, RequestNavigateEventArgs e)
        {
            try
            {
                Process.Start(new ProcessStartInfo(e.Uri.AbsoluteUri));
                e.Handled = true;
            }
            catch (Exception ex)
            {
                Log.Logger.LogData(ex.Message, LogLevel.Error);
            }
        }
        private void btnFirstAutomation_Click(object sender, RoutedEventArgs e)
        {
            string firstAutomationDoc = ConfigurationManager.AppSettings["firstAutomationDoc"];
            Hyperlink hyperlink = (Hyperlink)sender;
            hyperlink.NavigateUri = new Uri("file:///" + Path.Combine(SelectHelper.CurrentExecutablepath, firstAutomationDoc));
        }
        private void btnWebAutomation_Click(object sender, RoutedEventArgs e)
        {
            string webAutomationDoc = ConfigurationManager.AppSettings["webAutomationDoc"];
            Hyperlink hyperlink = (Hyperlink)sender;
            hyperlink.NavigateUri = new Uri("file:///" + Path.Combine(SelectHelper.CurrentExecutablepath, webAutomationDoc));
        }
        private void btnSAPAutomation_Click(object sender, RoutedEventArgs e)
        {
            string sapAutomationDoc = ConfigurationManager.AppSettings["sapAutomationDoc"];
            Hyperlink hyperlink = (Hyperlink)sender;
            hyperlink.NavigateUri = new Uri("file:///" + Path.Combine(SelectHelper.CurrentExecutablepath, sapAutomationDoc));
        }
        private void btnMainframeAutomation_Click(object sender, RoutedEventArgs e)
        {
            string sapAutomationDoc = ConfigurationManager.AppSettings["mainframeAutomationDoc"];
            Hyperlink hyperlink = (Hyperlink)sender;
            hyperlink.NavigateUri = new Uri("file:///" + Path.Combine(SelectHelper.CurrentExecutablepath, sapAutomationDoc));
        }

        private void btnBestPractices_Click(object sender, RoutedEventArgs e)
        {
            string bestpracticesDoc = ConfigurationManager.AppSettings["bestpracticesDoc"];
            Hyperlink hyperlink = (Hyperlink)sender;
            hyperlink.NavigateUri = new Uri("file:///" + Path.Combine(SelectHelper.CurrentExecutablepath, bestpracticesDoc));
        }
        private void btnExcelAutomation_Click(object sender, RoutedEventArgs e)
        {
            string excelAutomationDoc = ConfigurationManager.AppSettings["excelAutomationDoc"];
            Hyperlink hyperlink = (Hyperlink)sender;
            hyperlink.NavigateUri = new Uri("file:///" + Path.Combine(SelectHelper.CurrentExecutablepath, excelAutomationDoc));
        }
        private void btnWindowsAutomation_Click(object sender, RoutedEventArgs e)
        {
            string windowsAutomationDoc = ConfigurationManager.AppSettings["windowsAutomationDoc"];
            Hyperlink hyperlink = (Hyperlink)sender;
            hyperlink.NavigateUri = new Uri("file:///" + Path.Combine(SelectHelper.CurrentExecutablepath, windowsAutomationDoc));
        }
        private void btnCustomActivity_Click(object sender, RoutedEventArgs e)
        {
            string custActivityDoc = ConfigurationManager.AppSettings["customactivityDoc"];
            Hyperlink hyperlink = (Hyperlink)sender;
            hyperlink.NavigateUri = new Uri("file:///" + Path.Combine(SelectHelper.CurrentExecutablepath, custActivityDoc));
        }
        private void btnCustomProperty_Click(object sender, RoutedEventArgs e)
        {
            string custPropAutomationDoc = ConfigurationManager.AppSettings["custompropertyDoc"];
            Hyperlink hyperlink = (Hyperlink)sender;
            hyperlink.NavigateUri = new Uri("file:///" + Path.Combine(SelectHelper.CurrentExecutablepath, custPropAutomationDoc));
        }

    }
    public class NewProjectEventArgs : EventArgs
    {
        public string Path { get; set; }
        public ProjectType projectType;
        public string projectFullName;
    }

    public enum ProjectType
    {
        sequence = 1,
        workflow = 2,
        statemachine = 3,
        openproject=4,
        openexplorer =5

    }
}
