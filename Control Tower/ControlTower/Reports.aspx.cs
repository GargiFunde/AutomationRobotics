///-----------------------------------------------------------------
///   Namespace:      -
///   Class:                DemoMasterPage2_AddGroup
///   Description:       To capture all Active Logs.
///   Author:         B.Piyush                    Date: 2020-04-30 13:04:46.267
///   Notes:          <Notes>
///   Revision History:
///   Team:   E2E Team
///   Name:           Date:        Description:
///-----------------------------------------------------------------


using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Windows.Forms;
using System.Configuration;
using System.Data.SqlClient;
using System.Web.UI.DataVisualization.Charting;
using System.Drawing;

public partial class pages_DemoMasterPage2_SanketCharts : System.Web.UI.Page
{

    public string[] Labelsweekly { get; set; }
    public string Legendweekly { get; set; }
    public int[] Dataweekly { get; set; }
    public string TypeWeekly { get; set; }

    public string[] Labelsdaily { get; set; }
    public string[] Legenddaily { get; set; }
    public int[] Datadaily { get; set; }
    public string TypeDaily { get; set; }

    public string[] LabelsMonthly { get; set; }
    public string LegendMonthly { get; set; }
    public int[] DataMonthly { get; set; }
    public string TypeMonthly { get; set; }
    protected void Page_Load(object sender, EventArgs e)
    {
        //Page.ClientScript.RegisterStartupScript(this.GetType(), "alert", "DisplayWeeklyChart();", true);
        //Page.ClientScript.RegisterStartupScript(this.GetType(), "alert", "DisplayDailyChart();", true);
        if (!this.IsPostBack)
        { 
            ddlStatusMonthlyBind();
            ddlStatusBind();
            PopulateWeeklyChartJs();
            PopulateDailyDChartJs();
            PopulateMonthlyChartJs();
        }

    }

    protected override void InitializeCulture()
    {
        UICulture = Request.UserLanguages[0];
        base.InitializeCulture();
    }
    protected void Page_PreInit(object sender, EventArgs e)
    {

        if (Session["theme"] == null)
        {
            Theme = "default";  //for the first time Session["theme"] does not contain any value.
        }
        else
        {

            Theme = Session["theme"].ToString(); //with in the dropdown we set name of theme in session variable, this is set when user selects his choice of theme from dropdownlist.

        }

    }
    public void PopulateWeeklyChartJs()
    {

        
        string status = ddlStatusWeekly.SelectedValue.ToString();     
        DataTable dt = getChartData(status);
        DataView dv = new DataView(dt);
        DataTable dt1 = dv.ToTable(true, "sdate");
        string[] arrray = dt1.Rows.OfType<DataRow>().Select(k => k[0].ToString()).ToArray();        
        string type = ddlWeeklyChart.SelectedValue.ToString();

        Labelsweekly = arrray;
        Legendweekly = "Number of Processes";
        Dataweekly = dt.AsEnumerable().Select(r => r.Field<int>("cntStatus")).ToArray();
        TypeWeekly = type;
       
   

        //ClientScript.RegisterStartupScript(this.GetType(), "Onload", " assign();", true);

    }


    public void PopulateDailyDChartJs()
    {    
        DataTable dt = getDoughnutChartData();
        DataView dv = new DataView(dt);
        DataTable dt1 = dv.ToTable(true, "sdate");
        string[] arrray = dt1.Rows.OfType<DataRow>().Select(k => k[0].ToString()).ToArray();       
        string type = ddlDailyChart.SelectedValue.ToString();
        Labelsdaily = dt.AsEnumerable().Select(r => r.Field<string>("Status")).ToArray();
        Legenddaily = dt.AsEnumerable().Select(r => r.Field<string>("Status")).ToArray();
        Datadaily = dt.AsEnumerable().Select(r => r.Field<int>("cntStatus")).ToArray();
        TypeDaily = type;

    }

    public void PopulateMonthlyChartJs()
    {
        
        string status = ddlStatusMonthly.SelectedValue.ToString();        
        DataTable dt = getMonthlyChartData(status);        
        DataView dv = new DataView(dt);
        DataTable dt1 = dv.ToTable(true, "sdate");
        string[] arrray = dt1.Rows.OfType<DataRow>().Select(k => k[0].ToString()).ToArray();
        string type = ddlMonthlyChart.SelectedValue.ToString();
        LabelsMonthly = arrray;
        LegendMonthly = "Number of Processes";
        DataMonthly = dt.AsEnumerable().Select(r => r.Field<int>("cntStatus")).ToArray();
        TypeMonthly = type;
        
    }

    
   
    public void ddlStatusMonthlyBind()
    {
        ddlStatusMonthly.Items.Clear();
        DataTable dt = GetddlData();
        ddlStatusMonthly.DataSource = dt;
        ddlStatusMonthly.DataTextField = "Status";
        ddlStatusMonthly.DataValueField = "Status";
        ddlStatusMonthly.DataBind();
    }
    public void ddlStatusBind()
    {
        ddlStatusWeekly.Items.Clear();
        DataTable dt = GetddlData();
        ddlStatusWeekly.DataSource = dt;
        ddlStatusWeekly.DataTextField = "Status";
        ddlStatusWeekly.DataValueField = "Status";
        ddlStatusWeekly.DataBind();
    }
  

    private static DataTable getChartData(string status)
    {

        ServiceInterface serviceInterface = new ServiceInterface();
        DataTable dt = serviceInterface.getChartData(status);
        return dt;
    }
    
    private static DataTable getMonthlyChartData(string status)
    {

        ServiceInterface serviceInterface = new ServiceInterface();
        DataTable dt = serviceInterface.getMonthlyChartData(status);
        return dt;
    }

    private static DataTable getDoughnutChartData()
    {

        ServiceInterface serviceInterface = new ServiceInterface();
        DataTable dt = serviceInterface.getDoughnutChartData();
        return dt;
    }

    private static DataTable GetddlData()
    {
        ServiceInterface serviceInterface = new ServiceInterface();
        DataTable dt = serviceInterface.GetddlData_createschedulestatus();
        return dt;
    }


 


    private static DataTable GetData()
    {

        ServiceInterface serviceInterface = new ServiceInterface();
        DataTable dt = serviceInterface.GetData_createschedulestatus();
        return dt;
    }

        



    protected void ddlStatusMonthly_SelectedIndexChanged(object sender, EventArgs e)
    {

        PopulateMonthlyChartJs();
        PopulateWeeklyChartJs();

    }
    protected void ddlWeeklyChart_SelectedIndexChanged(object sender, EventArgs e)
    {

       PopulateWeeklyChartJs();

    }

    protected void ddlStatusWeekly_SelectedIndexChanged(object sender, EventArgs e)
    {

       
       
        PopulateWeeklyChartJs();

    }

    protected void ddlDailyChart_SelectedIndexChanged(object sender, EventArgs e)
    {

       
        PopulateDailyDChartJs();
       

    }

    protected void ddlMonthlyChart_SelectedIndexChanged(object sender, EventArgs e)
    {
       
        PopulateMonthlyChartJs();
        PopulateWeeklyChartJs();
    }




    //public void PopulateMonthlyChart()
    //{
    //    string status = ddlStatus.SelectedValue.ToString();
    //    Chart1.Visible = ddlStatus.SelectedValue != "";


    //    DataTable dt = getChartData(status);
    //    Chart1.DataSource = dt;
    //    //Chart1.Series[0].ChartType = (SeriesChartType)int.Parse(ddlChart.SelectedItem.Value);
    //    Chart1.Legends[0].Enabled = true;
    //    Chart1.Series[0].XValueMember = "sdate";
    //    Chart1.Series[0].YValueMembers = "cntStatus";
    //    Chart1.DataBind();
    //}



    //public void PopulateDoughnutChart()
    //{

    //    DataTable ChartData = getDoughnutChartData();

    //    //storing total rows count to loop on each Record  
    //    string[] XPointMember = new string[ChartData.Rows.Count];
    //    int[] YPointMember = new int[ChartData.Rows.Count];

    //    for (int count = 0; count < ChartData.Rows.Count; count++)
    //    {
    //        //storing Values for X axis  
    //        XPointMember[count] = ChartData.Rows[count]["Status"].ToString();
    //        //storing values for Y Axis  
    //        YPointMember[count] = Convert.ToInt32(ChartData.Rows[count]["cntStatus"]);

    //    }
    //    //binding chart control  
    //    Chart2.Series[0].Points.DataBindXY(XPointMember, YPointMember);

    //    //Setting width of line  
    //    Chart2.Series[0].BorderWidth = 10;
    //    //setting Chart type   
    //    Chart2.Series[0].ChartType = SeriesChartType.Doughnut;


    //    foreach (Series charts in Chart2.Series)
    //    {
    //        foreach (DataPoint point in charts.Points)
    //        {
    //            switch (point.AxisLabel)
    //            {
    //                case "Q1": point.Color = Color.YellowGreen; break;
    //                case "Q2": point.Color = Color.Yellow; break;
    //                case "Q3": point.Color = Color.SpringGreen; break;
    //            }
    //            point.Label = string.Format(point.AxisLabel);

    //        }
    //    }


    //    //Enabled 3D  
    //    Chart2.ChartAreas["ChartArea2"].Area3DStyle.Enable3D = true;  



    //}


    //private void GetData()
    //{
    //    ServiceInterface serviceInterface = new ServiceInterface();
    //    DataTable dt = serviceInterface.GetData_createschedulestatus();

    //    DataTableReader reader = new DataTableReader(dt);
    //    Series series = Chart1.Series["Series1"];



    //    while (reader.Read())
    //    {


    //        string status = reader["Status"].ToString();
    //        string cntstatus = reader["cntstatus"].ToString();            
    //        series.Points.AddXY(status,cntstatus);

    //    }
    //}


    //protected void ddlChart_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //    Chart1.Series[0].ChartType = (SeriesChartType)int.Parse(ddlChart.SelectedItem.Value);
    //    // Chart1.Series["Series1"].ChartType = (SeriesChartType)Enum.Parse(typeof(SeriesChartType), ddlChart.SelectedValue);
    //}




    //protected void ddlChart1_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //    PopulateDailyDChartJs();
    //    //PopulateMonthlyChart();
    //    // PopulateDoughnutChart();
    //    PopulateWeeklyChartJs();
    //   // PopulateMonthlyChartJs();

    //}

    //private void getChartTypes()
    //{
    //    foreach (int chartType in Enum.GetValues(typeof(SeriesChartType)))
    //    {
    //        ListItem li = new ListItem(Enum.GetName(typeof(SeriesChartType), chartType), chartType.ToString());
    //        ddlChart.Items.Add(li);
    //    }

    //}

    //protected void ddlChart_SelectedIndexChanged(object sender, EventArgs e)
    //{

    //    PopulateWeeklyChartJs();
    //}

    //protected void ddlStatus_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //   // PopulateDailyDChartJs();      
    //    PopulateWeeklyChartJs();
    //   //PopulateMonthlyChartJs();
    //}






    protected void Button1_Click(object sender, EventArgs e)
    {
     string sDataForROI1Graph = DateTime.Now.ToString();

    }
}