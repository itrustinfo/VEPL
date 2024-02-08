using ProjectManagementTool.DAL;
using ProjectManager.DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Configuration;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;
using ProjectManagementTool.Models;
using System.Globalization;

namespace ProjectManagementTool._content_pages.dashboard_new
{
    public partial class _default : System.Web.UI.Page
    {
        DBGetData getdt = new DBGetData();
        TaskUpdate gettk = new TaskUpdate();
        Invoice invoice = new Invoice();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["Username"] == null)
            {
                Response.Redirect("~/Login.aspx");
            }
            else
            {
                if (!IsPostBack)
                {
                    if (WebConfigurationManager.AppSettings["IsContractorPopUp"] == "Yes")
                    {
                        //if (Session["IsContractor"].ToString() == "Y" & Session["MsgShown"].ToString() == "N")
                        //{
                        //    ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "showModal();", true);
                        //    Session["MsgShown"] = "Y";
                        //}
                    }
                    if (Session["TypeOfUser"].ToString() == "DDE" && WebConfigurationManager.AppSettings["Domain"] == "ONTB" && Session["MsgGeneralDocs"].ToString() == "N")
                    {
                        //ScriptManager.RegisterStartupScript(this, this.GetType(), "Pop", "showModalGD();", true);
                        //Session["MsgGeneralDocs"] = "Y";
                    }

                    Session["ActivityUID"] = null;
                    BindProject();
                    SelectedProjectWorkpackage("Project");
                    DDlProject_SelectedIndexChanged(sender, e);

                    // added on 17/11/2020
                    DataSet dscheck = new DataSet();
                    dscheck = getdt.GetUsertypeFunctionality_Mapping(Session["TypeOfUser"].ToString());
                    // RdList.Items[1].Attributes.CssStyle.Add("display", "none");
                    //   rdSelect.Items[1].Attributes.CssStyle.Add("display", "none");
                    // rdSelect.Items[2].Attributes.CssStyle.Add("display", "none");
                    // RdList.Items[1].Enabled = false;
                    rdSelect.Items[1].Enabled = false;
                    rdSelect.Items[2].Enabled = false;
                   // divCamera.Visible = false;
                    if (dscheck.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow dr in dscheck.Tables[0].Rows)
                        {

                            if (dr["Code"].ToString() == "FS" || dr["Code"].ToString() == "FT" || dr["Code"].ToString() == "FZ") // VIEW FINANCIAL PROGRESS OF PROJECT-INDIVIDUAL REGIONS // ALL // INDIVIDUAL PROJECT //
                            {
                                // RdList.Items[1].Attributes.CssStyle.Add("display", "block");
                                //  rdSelect.Items[2].Attributes.CssStyle.Add("display", "block");
                                //  RdList.Items[1].Enabled = true;
                                rdSelect.Items[2].Enabled = true;

                            }
                            if (dr["Code"].ToString() == "FX" || Session["TypeOfUser"].ToString() == "U") //Project progress tracking
                            {

                                // rdSelect.Items[1].Attributes.CssStyle.Add("display", "block");
                                rdSelect.Items[1].Enabled = true;
                            }
                            if (Session["TypeOfUser"].ToString() != "U")
                            {
                                if (dr["Code"].ToString() == "DC") //Project progress tracking
                                {

                                    //divCamera.Visible = true;
                                }

                            }
                            else
                            {
                                //divCamera.Visible = true;
                            }

                        }
                    }
                    //added on 15/07/2022 for slahuddins new requirements
                    if (Session["TypeOfUser"].ToString() == "DDE")
                    {
                        rdSelect.Items[1].Enabled = false;
                        divAlerts.Visible = false;
                        divIssues.Visible = false;
                        divCostChart.Visible = false;
                        divPhotographs.Visible = false;
                    }
                    //
                    
                    //

                    if (Session["TypeOfUser"].ToString() == "NJSD")
                    {
                        UploadSitePhotograph.Visible = false;
                        rdSelect.Items[1].Enabled = true;
                    }
                }
            }
        }

        private void BindProject()
        {
            DataTable ds = new DataTable();
            if (Session["TypeOfUser"].ToString() == "U" || Session["TypeOfUser"].ToString() == "MD" || Session["TypeOfUser"].ToString() == "VP" || Session["TypeOfUser"].ToString() == "NJSD")
            {
                ds = gettk.GetProjects();
            }
            else if (Session["TypeOfUser"].ToString() == "PA")
            {
                ds = gettk.GetAssignedProjects_by_UserUID(new Guid(Session["UserUID"].ToString()));
            }
            else
            {
                ds = gettk.GetAssignedProjects_by_UserUID(new Guid(Session["UserUID"].ToString()));
            }

            DDlProject.DataTextField = "ProjectName";
            DDlProject.DataValueField = "ProjectUID";
            DDlProject.DataSource = ds;
            DDlProject.DataBind();
            //  DDlProject.Items.Insert(0, "--Select--");
            // DDLWorkPackage.Items.Insert(0, "--Select--");

        }

        private void SelectedProjectWorkpackage(string pType)
        {
            if (!IsPostBack)
            {
                if (Session["Project_Workpackage"] != null)
                {
                    string[] selectedValue = Session["Project_Workpackage"].ToString().Split('_');
                    if (selectedValue.Length > 1)
                    {
                        if (pType == "Project")
                        {
                            DDlProject.SelectedValue = selectedValue[0];
                        }
                        else
                        {
                            DDLWorkPackage.SelectedValue = selectedValue[1];
                        }
                    }
                    else
                    {
                        if (pType == "Project")
                        {
                            DDlProject.SelectedValue = Session["Project_Workpackage"].ToString();
                        }
                    }

                }
            }

        }

        protected void DDlProject_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (DDlProject.SelectedValue != "--Select--")
            {
                //chnage here
               
                //
                divdashboardimage.Visible = true;
             
                DataSet ds = new DataSet();
                //ds = getdt.GetWorkPackages_By_ProjectUID(new Guid(DDlProject.SelectedValue));
                if (Session["TypeOfUser"].ToString() == "U" || Session["TypeOfUser"].ToString() == "MD" || Session["TypeOfUser"].ToString() == "VP" || Session["TypeOfUser"].ToString() == "NJSD")
                {
                    ds = getdt.GetWorkPackages_By_ProjectUID(new Guid(DDlProject.SelectedValue));
                }
                else if (Session["TypeOfUser"].ToString() == "PA")
                {
                    ds = getdt.GetWorkPackages_ForUser_by_ProjectUID(new Guid(Session["UserUID"].ToString()), new Guid(DDlProject.SelectedValue));
                }
                else
                {
                    ds = getdt.GetWorkPackages_ForUser_by_ProjectUID(new Guid(Session["UserUID"].ToString()), new Guid(DDlProject.SelectedValue));
                }
                //ds = getdt.GetWorkPackages_By_ProjectUID(new Guid(DDlProject.SelectedValue));
                if (ds.Tables[0].Rows.Count > 0)
                {
                    DDLWorkPackage.DataTextField = "Name";
                    DDLWorkPackage.DataValueField = "WorkPackageUID";
                    DDLWorkPackage.DataSource = ds;
                    DDLWorkPackage.DataBind();
                    SelectedProjectWorkpackage("Workpackage");
                    //  DDLWorkPackage.Items.Insert(0, "--Select--");
                   // BindResourceMaster();

                    if (ViewState["vChart"] != null)
                    {
                        if (ViewState["vChart"].ToString() != "")
                        {
                            //Bind_DocumentsChart();
                            Bind_DocumentsChart6();
                           // RdList3.SelectedIndex = 0;
                            ViewState["vChart"] = 1;
                        }
                    }
                    else
                    if (Request.QueryString["Option"] != null)
                    {
                        if (Request.QueryString["selection"] == "1")
                        {
                            //Bind_DocumentsChart1();
                           // RdList3.SelectedIndex = 0;
                            ViewState["vChart"] = 2;
                        }
                        else
                        {
                            //if (Request.QueryString["back"] == "1")
                            //{
                            //    Bind_DocumentsChart4();
                            //    RdList3.SelectedIndex = 1;
                            //    ViewState["vChart"] = 1;
                            //}
                            //else
                            //{
                           // Bind_DocumentsChart5();
                           // RdList3.SelectedIndex = 1;
                            ViewState["vChart"] = 2;
                            //}
                        }
                    }
                    else
                    {
                        
                    }
                    // Bind_DocumentsChart();
                    BindAlerts("WorkPackage");
                    BindActivityPie_Chart("Work Package", DDLWorkPackage.SelectedValue);
                    //Bind_ResourceChart("Work Package", DDLWorkPackage.SelectedValue);
                    // Bind_ProgressChart("Work Package", DDLWorkPackage.SelectedValue);
                    Bind_CostChart("Work Package", DDLWorkPackage.SelectedValue);
                    if (rdSelect.SelectedValue == "1")
                    {
                        LoadGraph(); //Physical progress chart
                    }
                    else if (rdSelect.SelectedValue == "2")
                    {
                        LoadFinancialGraph();
                    }
                   // BindCamera(DDLWorkPackage.SelectedValue);
                    //
                    Bind_ResourceGraph(DDLWorkPackage.SelectedValue);
                    Bind_DocumentsChart6();
                    heading.InnerHtml = "Physical Progress Chart - " + DDlProject.SelectedItem.Text + " (" + DDLWorkPackage.SelectedItem.Text + ")";
                    headingF.InnerHtml = "Financial Progress Chart - " + DDlProject.SelectedItem.Text + " (" + DDLWorkPackage.SelectedItem.Text + ")";

                    DbSyncStatusCount(DDLWorkPackage.SelectedValue);
                  
                    //added on 23/03/2023
                  
                    Session["Project_Workpackage"] = DDlProject.SelectedValue + "_" + DDLWorkPackage.SelectedValue;
                    // added on 10/01/2022 for docs to act on for the user

                    if (Session["TypeOfUser"].ToString() != "U" && Session["TypeOfUser"].ToString() != "VP" && Session["TypeOfUser"].ToString() != "MD" && Session["TypeOfUser"].ToString() != "NJSD")
                    {
                        divUsersdocs.Visible = true;
                        //if (getUserDocsNo() == 0)
                        //{
                        //    Hluserdocs.HRef = "#";
                        //    Hluserdocs.InnerText = "no documents";
                        //}
                        //else
                        //{
                        //    Hluserdocs.InnerText = getUserDocsNo() + " documents";
                        //    Hluserdocs.HRef = "~/_content_pages/documents-contractor/?&type=Ontb&PrjUID=" + DDlProject.SelectedValue + "&UserUID=" + Session["UserUID"].ToString() + "&WkpgUID=" + DDLWorkPackage.SelectedValue;
                        //}
                    }
                    else
                    {
                        divUsersdocs.Visible = false;
                    }
                }
                else
                {
                    DDLWorkPackage.DataSource = null;
                    DDLWorkPackage.DataBind();
                    //ltScripts_piechart.Text = "<h4>No data</h4>";
                    //ltScript_Progress.Text = "<h4>No data</h4>";
                    //ltScript_Document.Text = "<h4>No data</h4>";
                    //ltScript_Resource.Text = "<h4>No data</h4>";
                    //ltScript_PhysicalProgress.Text = "<h4>No data</h4>";
                    //ltScript_FinProgress.Text = "<h4>No data</h4>";
                    //divtable.InnerHtml = "";
                    //btnPrint.Visible = false;
                }
                //}
            }
            else
            {
                //ltScripts_piechart.Text = "<h4>No data</h4>";
                //ltScript_Progress.Text = "<h4>No data</h4>";
                //ltScript_Document.Text = "<h4>No data</h4>";
                //ltScript_Resource.Text = "<h4>No data</h4>";
                //ltScript_PhysicalProgress.Text = "<h4>No data</h4>";
                //ltScript_FinProgress.Text = "<h4>No data</h4>";
                //divtable.InnerHtml = "";
                //btnPrint.Visible = false;
                DDLWorkPackage.Items.Clear();
                DDLWorkPackage.Items.Insert(0, "--Select--");
                divdashboardimage.Visible = false;
                divUsersdocs.Visible = false;
                
            }

           
           
            //show contractor data and project data
            if (DDLWorkPackage.SelectedValue != "--Select--")
            {
                DataSet dsc = getdt.GetWorkpackge_Contractor_Data_by_WorkpackageUID(new Guid(DDLWorkPackage.SelectedValue));
                if (dsc.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < dsc.Tables[0].Columns.Count; i++)
                    {
                        if (dsc.Tables[0].Columns[i].ToString() == "Contractor_Name")
                        {
                            lblContractor.Text = dsc.Tables[0].Rows[0]["Contractor_Name"].ToString();
                        }
                        //
                        else if (dsc.Tables[0].Columns[i].ToString() == "ProjectEndDate")
                        {
                            lblPEndDate.Text = dsc.Tables[0].Rows[0]["ProjectEndDate"].ToString() != "" ? Convert.ToDateTime(dsc.Tables[0].Rows[0]["ProjectEndDate"].ToString()).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture) : "-";
                        }
                        else if (dsc.Tables[0].Columns[i].ToString() == "StartDate")
                        {
                            lblPStartDate.Text = dsc.Tables[0].Rows[0]["StartDate"].ToString() != "" ? Convert.ToDateTime(dsc.Tables[0].Rows[0]["StartDate"].ToString()).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture) : "-";
                        }
                    }
                }
                //
                DataSet dsWorkpackage = getdt.GetWorkPackages_By_WorkPackageUID(new Guid(DDLWorkPackage.SelectedValue));
                //
                if (dsWorkpackage.Tables[0].Rows.Count > 0)
                {

                    if (dsWorkpackage.Tables[0].Rows[0]["ExtendedEndDate1"].ToString() != null && dsWorkpackage.Tables[0].Rows[0]["ExtendedEndDate1"].ToString() != "")
                    {
                        lblPEndDate.Text = Convert.ToDateTime(dsWorkpackage.Tables[0].Rows[0]["ExtendedEndDate1"]).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
                    }
                    if (dsWorkpackage.Tables[0].Rows[0]["ExtendedEndDate2"].ToString() != null && dsWorkpackage.Tables[0].Rows[0]["ExtendedEndDate2"].ToString() != "")
                    {
                        lblPEndDate.Text = Convert.ToDateTime(dsWorkpackage.Tables[0].Rows[0]["ExtendedEndDate2"]).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
                    }
                    if (dsWorkpackage.Tables[0].Rows[0]["ExtendedEndDate3"].ToString() != null && dsWorkpackage.Tables[0].Rows[0]["ExtendedEndDate3"].ToString() != "")
                    {
                        lblPEndDate.Text = Convert.ToDateTime(dsWorkpackage.Tables[0].Rows[0]["ExtendedEndDate3"]).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
                    }
                    if (dsWorkpackage.Tables[0].Rows[0]["ExtendedEndDate4"].ToString() != null && dsWorkpackage.Tables[0].Rows[0]["ExtendedEndDate4"].ToString() != "")
                    {
                        lblPEndDate.Text = Convert.ToDateTime(dsWorkpackage.Tables[0].Rows[0]["ExtendedEndDate4"]).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
                    }
                }
                lblProject.Text = DDlProject.SelectedItem.ToString();
            }
        }

        protected void DDLWorkPackage_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (DDLWorkPackage.SelectedValue != "--Select--")
            {
              
                //BindResourceMaster();
                //Bind_DocumentsChart();
                BindAlerts("WorkPackage");
                BindActivityPie_Chart("Work Package", DDLWorkPackage.SelectedValue);
                ////  Bind_ResourceChart("Work Package", DDLWorkPackage.SelectedValue);
                ////Bind_ProgressChart("Work Package", DDLWorkPackage.SelectedValue);
                Bind_CostChart("Work Package", DDLWorkPackage.SelectedValue);
                //BindCamera(DDLWorkPackage.SelectedValue);
                ////
                Bind_ResourceGraph(DDLWorkPackage.SelectedValue);
                ////added on 23/03/2023 for suez
                //if ((DDlProject.SelectedItem.ToString() == "CP-02" || DDlProject.SelectedItem.ToString() == "CP-03") && WebConfigurationManager.AppSettings["Domain"] == "Suez")
                //{
                //    BindDocumentSummary();
                //}
                ////
                heading.InnerHtml = "Physical Progress Chart - " + DDlProject.SelectedItem.Text + " (" + DDLWorkPackage.SelectedItem.Text + ")";
                headingF.InnerHtml = "Financial Progress Chart - " + DDlProject.SelectedItem.Text + " (" + DDLWorkPackage.SelectedItem.Text + ")";
                //if (rdSelect.SelectedValue == "1")
                //{
                //    LoadGraph(); //Physical progress chart
                //}
                //else if (rdSelect.SelectedValue == "2")
                //{
                //    LoadFinancialGraph();
                //}
                //Session["Project_Workpackage"] = DDlProject.SelectedValue + "_" + DDLWorkPackage.SelectedValue;
                ////----------------------
                //if (Session["TypeOfUser"].ToString() != "U" && Session["TypeOfUser"].ToString() != "VP" && Session["TypeOfUser"].ToString() != "MD" && Session["TypeOfUser"].ToString() != "NJSD")
                //{
                //    divUsersdocs.Visible = true;
                //    //if (getUserDocsNo() == 0)
                //    //{
                //    //    Hluserdocs.HRef = "#";
                //    //    Hluserdocs.InnerText = "no documents";
                //    //}
                //    //else
                //    //{
                //    //    Hluserdocs.InnerText = getUserDocsNo() + " documents";
                //    //    Hluserdocs.HRef = "~/_content_pages/documents-contractor/?&type=Ontb&PrjUID=" + DDlProject.SelectedValue + "&UserUID=" + Session["UserUID"].ToString() + "&WkpgUID=" + DDLWorkPackage.SelectedValue;
                //    //}
                //}
                //else
                //{
                //    divUsersdocs.Visible = false;
                //}
                //-----------------------

            }

           
            //show contractor data and project data
            DataSet ds = getdt.GetWorkpackge_Contractor_Data_by_WorkpackageUID(new Guid(DDLWorkPackage.SelectedValue));
            if (ds.Tables[0].Rows.Count > 0)
            {
                for (int i = 0; i < ds.Tables[0].Columns.Count; i++)
                {
                    if (ds.Tables[0].Columns[i].ToString() == "Contractor_Name")
                    {
                        lblContractor.Text = ds.Tables[0].Rows[0]["Contractor_Name"].ToString();
                    }
                }
            }
            lblProject.Text = DDlProject.SelectedItem.ToString();
        }

        private void Bind_CostChart(string ActivityType, string Activity_ID)
        {
            if (DDLWorkPackage.SelectedValue != "--Select--")
            {
                ltScript_Progress.Text = string.Empty;
                // DataSet ds = getdt.Get_WorkPackage_Budget(Session["UserUID"].ToString(), ActivityType, Activity_ID);

                DataSet ds = getdt.GetCostGraphData(Activity_ID); // changed by saji augustin dated 13 may 2022

                if (ds.Tables[0].Rows.Count > 0)
                {
                    StringBuilder strScript = new StringBuilder();
                    strScript.Append(@" <script type='text/javascript'>
                
                google.charts.load('current', { packages: ['corechart', 'bar'] });
                google.charts.setOnLoadCallback(drawBasic);

                function drawBasic() {

                var data = google.visualization.arrayToDataTable([
                     ['Element', 'Cost', { role: 'style' }, { role: 'annotation' }],");
                    string CurrencySymbol = "";
                    if (ds.Tables[0].Rows[0]["Currency"].ToString() == "&#x20B9;")
                    {
                        CurrencySymbol = "₹";
                    }
                    else if (ds.Tables[0].Rows[0]["Currency"].ToString() == "&#36;")
                    {
                        CurrencySymbol = "$";
                    }
                    else
                    {

                        CurrencySymbol = "¥";
                    }
                    
                        strScript.Append("['Actual', " + ds.Tables[0].Rows[0]["Actual"].ToString() + ", '#b66dff', '" + CurrencySymbol + ' ' + ds.Tables[0].Rows[0]["Actual"].ToString() + "'],['Planned', " + ds.Tables[0].Rows[0]["Planned"].ToString() + ", '#57c7d4', '" + CurrencySymbol + ' ' + ds.Tables[0].Rows[0]["Planned"].ToString() + "'],['Budget', " + ds.Tables[0].Rows[0]["Budget"].ToString() + ", '#5E50F9', '" + CurrencySymbol + ' ' + ds.Tables[0].Rows[0]["Budget"].ToString() + "']]);");
                  
                    
                    strScript.Append(@"var options = {
                    title : 'Cost in Crores of Rupees',

                    is3D: true, 
                    legend: { position: 'none' },
                    fontSize: 14,
                    chartArea: {
                        left: '10%',
                        top: '10%',
                        height: '75%',
                        width: '80%'
                    },
                    height: 200
                };

                var chart = new google.visualization.ColumnChart(
                  document.getElementById('chart_div'));
                 chart.draw(data, options);
                
            }</script>");
                    //ltScript_Cost.Text = strScript.ToString();
                    ltScript_Progress.Text = strScript.ToString();
                }
                else
                {
                    //ltScript_Cost.Text = "<h3>No data</h3>";
                    ltScript_Progress.Text = "<h3>No data</h3>";

                }
            }
        }

        private void BindAlerts(string By)
        {
            if (DDLWorkPackage.SelectedValue != "--Select--")
            {
                DataSet ds = new DataSet();
                if (By == "Project")
                {
                    ds = getdt.getAlerts_by_ProjectUID(new Guid(DDlProject.SelectedValue));
                }
                else if (By == "WorkPackage")
                {
                    ds = getdt.getAlerts_by_WorkPackageUID(new Guid(DDlProject.SelectedValue), new Guid(DDLWorkPackage.SelectedValue));
                }
                //else
                //{
                //    ds = getdt.getAlerts_by_TaskUID(new Guid(DDLTask.SelectedValue));
                //}
                if (ds.Tables[0].Rows.Count > 0)
                {
                    string s1;
                    s1 = "<table class='table table-borderless'>";

                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        //s1 += "<tr><td style='width:80%; color:#006699; font-size:larger;'>" + ds.Tables[0].Rows[i]["Alert_Text"].ToString() + "</td>" + "<td style='width:20%; text-align:right;'>" + Convert.ToDateTime(ds.Tables[0].Rows[i]["Alert_Date"].ToString()).ToString("dd/MM/yyyy") + "</td></tr>";
                        s1 += "<tr style='border-bottom:1px dotted Gray; margin-left:0px;'><td>" + ds.Tables[0].Rows[i]["Alert_Text"].ToString() + "</td></tr>";
                    }
                    s1 += "</table>";
                    lt1.Text = s1.ToString();
                }
                else
                {
                    lt1.Text = "<h4>No Alerts Found</h4>";
                }
            }
        }

        private void BindActivityPie_Chart(string ActivityType, string Activity_ID)
        {
            if (DDLWorkPackage.SelectedValue != "--Select--")
            {
                DataSet ds = getdt.Get_Open_Closed_Rejected_Issues_by_WorkPackageUID(new Guid(Activity_ID));
                if (ds.Tables[0].Rows.Count > 0)
                {
                    StringBuilder strScript = new StringBuilder();
                    strScript.Append(@"<script type='text/javascript'>        
                google.charts.load('current', { packages: ['corechart'] });
                google.charts.setOnLoadCallback(drawChart);

                 function drawChart() {
                     var data = google.visualization.arrayToDataTable([
                       ['Issues', 'Count'],");
                    strScript.Append("['Open ', " + ds.Tables[0].Rows[0]["OpenIssues"].ToString() + "], ['In-Progress ', " + ds.Tables[0].Rows[0]["InProgressIssues"].ToString() + "], ['Closed ', " + ds.Tables[0].Rows[0]["ClosedIssues"].ToString() + "], ['Rejected ', " + ds.Tables[0].Rows[0]["RejectedIssues"].ToString() + "]]);");
                    strScript.Append(@"var options = {
                         legend: { position: 'top' },
                                                        colors: ['#f96868', '#b66dff', '#57c7d4', '#3366CC'],
                                                        pieHole: 0.5,
                                                        pieSliceText: 'value',
                                                        chartArea: {
                                                            top: 35,
                                                            height: '100%',
                                                            width: '100%'
                                                        }
                     };
                        function selectHandler() {
                        var selection = chart.getSelection(); 
                        if (selection.length > 0) {
                        var selectedItem = chart.getSelection()[0];
                        if (selectedItem) {
                           window.open('/_content_pages/issues', '_self', true);
                         }
                        }
                     }
                     var chart = new google.visualization.PieChart(document.getElementById('piechart_3d'));
                      google.visualization.events.addListener(chart, 'select', selectHandler);
                     chart.draw(data, options);}
                    </script>");
                    ltScripts_piechart.Text = strScript.ToString();
                }
                else
                {
                    ltScripts_piechart.Text = "<h4>No data</h4>";
                }

                //DataSet ds = getdt.GetTask_Status_Count(Session["UserUID"].ToString(), ActivityType, Activity_ID);
                //if (ds.Tables[0].Rows.Count > 0)
                //{

                //    StringBuilder strScript = new StringBuilder();
                //    strScript.Append(@"<script type='text/javascript'>        
                //google.charts.load('current', { packages: ['corechart'] });
                //google.charts.setOnLoadCallback(drawChart);

                // function drawChart() {
                //     var data = google.visualization.arrayToDataTable([
                //       ['Task', 'Hours per Day'],");
                //    strScript.Append("['Not Started', " + ds.Tables[0].Rows[0]["Pending"].ToString() + "], ['Completed', " + ds.Tables[0].Rows[0]["Completed"].ToString() + "], ['In Progress', " + ds.Tables[0].Rows[0]["Inprogress"].ToString() + "]]);");
                //    strScript.Append(@"var options = {
                //         is3D: true,
                //         legend: { position: 'labeled', textStyle: { color: 'black', fontSize: 13 } },
                //         pieSliceText: 'value',
                //         pieSliceTextStyle: { bold: true, fontSize: 13 },
                //         chartArea: {                        
                //             height: '92%',
                //             width: '92%'
                //         }
                //     };
                //     var chart = new google.visualization.PieChart(document.getElementById('piechart_3d'));  
                //     chart.draw(data, options);}
                //    </script>");
                //    ltScripts_piechart.Text = strScript.ToString();


                //    //function selectHandler()
                //    //{
                //    //    var selectedItem = chart.getSelection()[0];
                //    //    if (selectedItem)
                //    //    {
                //    //        var topping = data.getValue(selectedItem.row, 0); ");
                //    //  strScript.Append("window.open('WorkPackages.aspx?TaskType=' + topping + '&ProjectUID=" + DDlProject.SelectedValue + "&WorkPackageUID=" + DDLWorkPackage.SelectedValue + "', '_self', true);");
                //    //        //alert('The user selected ' + topping);
                //    //        strScript.Append(@"}
                //    //}
                //    //google.visualization.events.addListener(chart, 'select', selectHandler);

                //    //}
                //    //else
                //    //{
                //    //    ltScripts_piechart.Text = " < h4>No data</h4>";
                //    //}
                //}
                //else
                //{
                //    ltScripts_piechart.Text = "<h4>No data</h4>";
                //}
            }

        }

        private void DbSyncStatusCount(string WorkpackageUID)
        {
            if (WebConfigurationManager.AppSettings["Dbsync"] == "Yes")
            {
                DataSet ds = getdt.GetDbsync_Status_Count_by_WorkPackageUID(new Guid(WorkpackageUID));
                if (ds.Tables[0].Rows.Count > 0)
                {
                    
                    lblReconDocsNo.InnerHtml = getdt.GetDashboardReconciliationDocs(new Guid(DDlProject.SelectedValue)).ToString();
                    lblContractorToNo.InnerHtml = getdt.GetDashboardContractotDocsSubmitted(new Guid(DDlProject.SelectedValue)).ToString();
                    
                    //lblONTBTo_No.Text = (int.Parse(lblONTBTo_No.Text) - int.Parse(lblContractorToNo.Text)) > 0  ? (int.Parse(lblONTBTo_No.Text) - int.Parse(lblContractorToNo.Text)).ToString() : "0";
                    lblRABills.InnerHtml = getdt.GetInvoiceDetails_by_WorkpackageUID(new Guid(WorkpackageUID)).Rows.Count.ToString();
                    lblInvoices.InnerHtml = invoice.GetInvoiceMaster_by_WorkpackageUID(new Guid(WorkpackageUID)).Tables[0].Rows.Count.ToString();
                    lblBankG.InnerHtml = getdt.GetBankGuarantee_by_Bank_WorkPackageUID(new Guid(WorkpackageUID)).Tables[0].Rows.Count.ToString();
                    lblInsurance.InnerHtml = getdt.GetInsuranceSelect_by_WorkPackageUID(new Guid(WorkpackageUID)).Tables[0].Rows.Count.ToString();
                    lblMeasurements.InnerHtml = getdt.GetTaskMeasurementBookForDashboard(new Guid(WorkpackageUID)).Tables[0].Rows.Count.ToString();
                    //
                    lblInstruction.InnerHtml = getdt.GetMOMandInstructionCount("2").ToString();
                    lblMOM.InnerHtml = getdt.GetMOMandInstructionCount("1").ToString();
                    lblArchived.InnerHtml = getdt.GetDashboardArchivedDocs(new Guid(DDlProject.SelectedValue)).ToString();
                    //


                    // make hyper links
                    hlRABills.HRef = "~/_content_pages/rabill-summary/?&PrjUID=" + DDlProject.SelectedValue;
                    hlInvoices.HRef = "~/_content_pages/invoice/?&PrjUID=" + DDlProject.SelectedValue;
                    hlBankGuarantee.HRef = "~/_content_pages/bank-guarantee/?&PrjUID=" + DDlProject.SelectedValue;
                    hlInsurance.HRef = "~/_content_pages/insurance/?&PrjUID=" + DDlProject.SelectedValue;
                    hlContractor.HRef = "~/_content_pages/documents-contractor/?&type=Contractor&PrjUID=" + DDlProject.SelectedValue;
                    hlReconciliationdocs.HRef = "~/_content_pages/documents-contractor/?&type=Recon&PrjUID=" + DDlProject.SelectedValue;
                   // hlONTB.HRef = "~/_content_pages/documents-contractor/?&type=Ontb&PrjUID=" + DDlProject.SelectedValue;
                    hlInstructionManual.HRef = "~/_content_pages/documents-ontb/?&fromdashboard=0";
                    hlMOM.HRef = "~/_content_pages/documents-ontb/?&fromdashboard=1";
                    hlArchived.HRef = "~/_content_pages/documents-contractor/?&type=Archived&PrjUID=" + DDlProject.SelectedValue;
                    hlMeasurement.HRef = "~/_content_pages/dashboard-measurment/?&WorkPackageUID=" + DDLWorkPackage.SelectedValue;
                    //
                    hlRABills.HRef = "~/_content_pages/rabill-summary/?&PrjUID=" + DDlProject.SelectedValue;


                    UploadSitePhotograph.HRef = "/_modal_pages/upload-sitephotograph.aspx?PrjUID=" + DDlProject.SelectedValue + "&WorkPackage=" + DDLWorkPackage.SelectedValue;
                    ViewSitePhotograph.HRef = "/_modal_pages/view-sitephotographs.aspx?PrjUID=" + DDlProject.SelectedValue + "&WorkPackage=" + DDLWorkPackage.SelectedValue;
                    //  A1.HRef = "~/_content_pages/document-correspondence?ProjectUID=" + DDlProject.SelectedValue + "&WorkPackageUID=" + DDLWorkPackage.SelectedValue;
                    // A2.HRef = "~/_content_pages/documents-contractor-replaced/?&type=Ontb&PrjUID=" + DDlProject.SelectedValue;
                    //
                    //A3.HRef = "/_content_pages/submittal_documents/default.aspx?ProjectUID=" + DDlProject.SelectedValue + "&WorkPackageUID=" + DDLWorkPackage.SelectedValue;

                    DataSet ds1 = getdt.GetDocumentsBySubmittal(new Guid(DDlProject.SelectedValue), new Guid(DDLWorkPackage.SelectedValue));

                    //if (ds1.Tables[0].Rows.Count == 0)
                    //    //A3.Visible = false;
                    //else
                    //    //A3.Visible = true;

                    //added on 27/01/2023
                   
                }
                else
                {
                   
                    //LblLastSyncedDate.Text = "NA";
                    //LblTotalSourceDocuments.Text = "NA";
                    //LblTotalDestinationDocuments.Text = "NA";
                }

            }
            else
            {
               
            }
        }

        private void LoadGraph()
        {
            try
            {
                //  DateTime t1 = DateTime.Now;

                if (DDLWorkPackage.SelectedValue != "--Select--")
                {
                    ltScript_PhysicalProgress.Text = string.Empty;

                    DataSet ds = getdt.GetGraphPhysicalProgressValues1(DDlProject.SelectedValue, DDLWorkPackage.SelectedValue);

                    Boolean IsRevisedPlan = getdt.IsRevisedPlan(new Guid(DDLWorkPackage.SelectedValue)) == "Y" ? true : false;

                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        StringBuilder strScript = new StringBuilder();

                        string tablemonths = "";
                        string tmonthlyplan = "";
                        string tmonthlyactual = "";
                        string tmonthlytest = "";
                        string tcumulativeplan = "";
                        string tcumulativeactual = "";
                        string tcumulativetest = "";

                        tablemonths = "<td>&nbsp;</td>";
                        tmonthlyplan = "<td>Monthly Plan</td>";
                        tmonthlyactual = "<td>Monthly Actual</td>";

                        if (IsRevisedPlan)
                            tmonthlytest = "<td>Monthly Revised Plan</td>";
                        tcumulativeplan = "<td>Cumulative Plan</td>";

                        tcumulativeactual = "<td>Cumulative Actual</td>";

                        if (IsRevisedPlan)
                            tcumulativetest = "<td>Cumulative Revised Plan</td>";

                        if (IsRevisedPlan)
                        {
                            strScript.Append(@" <script type='text/javascript'>
                
                google.charts.load('current', { packages: ['corechart', 'bar'] });
                google.charts.setOnLoadCallback(drawBasic);

                function drawBasic() {

                var data = google.visualization.arrayToDataTable([
          ['Month', 'Monthly Plan', { role: 'style' }, 'Monthly Actual', { role: 'style' }, 'Cumulative Plan', { role: 'style' }, 'Cumulative Actual', { role: 'style' }],");
                        }
                        else
                        {
                            strScript.Append(@" <script type='text/javascript'>
                
                google.charts.load('current', { packages: ['corechart', 'bar'] });
                google.charts.setOnLoadCallback(drawBasic);

                function drawBasic() {

                var data = google.visualization.arrayToDataTable([
          ['Month', 'Monthly Plan', { role: 'style' }, 'Monthly Actual', { role: 'style' }, 'Cumulative Plan', { role: 'style' }, 'Cumulative Actual', { role: 'style' }],");
                        }


                        int count = 1;

                        decimal planvalue = 0;
                        decimal actualvalue = 0;
                        decimal testvalue = 0;
                        decimal cumplanvalue = 0;
                        decimal cumactualvalue = 0;
                        decimal cumtestvalue = 0;

                        string[] graphValues = ds.Tables[0].Rows[0].ItemArray[0].ToString().Split(';');


                        foreach (string dr in graphValues)
                        {
                            //get the actual and planned values....

                            string[] fields = dr.Split(',');

                            if (fields.Length == 4)
                            {
                                planvalue = decimal.Parse(fields[1]);
                                cumplanvalue += planvalue;

                                actualvalue = decimal.Parse(fields[2]);
                                cumactualvalue += actualvalue;

                                testvalue = decimal.Parse(fields[3]);
                                cumtestvalue += testvalue;

                                if (count < graphValues.Length)
                                {
                                    if (IsRevisedPlan)
                                        strScript.Append("['" + Convert.ToDateTime(fields[0]).ToString("MMMyy") + "'," + planvalue + "," + actualvalue + "," + testvalue + "," + cumplanvalue + "," + cumactualvalue + "," + cumtestvalue + "],");
                                    else
                                        strScript.Append("['" + Convert.ToDateTime(fields[0]).ToString("MMMyy") + "'," + planvalue + ",'color: #57c7d4'," + actualvalue + ",'color: #f96868'," + cumplanvalue + ",'color: #b66dff'," + cumactualvalue + ",'color: #f2a654'],");
                                }
                                else
                                {
                                    if (IsRevisedPlan)
                                        strScript.Append("['" + Convert.ToDateTime(fields[0]).ToString("MMMyy") + "'," + planvalue + "," + actualvalue + "," + testvalue + "," + cumplanvalue + "," + cumactualvalue + "," + cumtestvalue + "]]);");
                                    else
                                        // strScript.Append("['" + Convert.ToDateTime(fields[0]).ToString("MMMyy") + "'," + planvalue + "," + actualvalue + "," + cumplanvalue + "," + cumactualvalue + "]]);");
                                        strScript.Append("['" + Convert.ToDateTime(fields[0]).ToString("MMMyy") + "'," + planvalue + ",'color: #57c7d4'," + actualvalue + ",'color: #f96868'," + cumplanvalue + ",'color: #b66dff'," + cumactualvalue + ",'color: #f2a654']]);");

                                }


                                tablemonths += "<td>" + Convert.ToDateTime(fields[0]).ToString("MMM-yy") + "</td>";
                                tmonthlyplan += "<td>" + decimal.Round(planvalue, 2) + "</td>";
                                tmonthlyactual += "<td>" + decimal.Round(actualvalue, 2) + "</td>";

                                if (IsRevisedPlan)
                                    tmonthlytest += "<td>" + decimal.Round(testvalue, 2) + "</td>";

                                tcumulativeplan += "<td>" + decimal.Round(cumplanvalue, 2) + "</td>";
                                tcumulativeactual += "<td>" + decimal.Round(cumactualvalue, 2) + "</td>";

                                if (IsRevisedPlan)
                                    tcumulativetest += "<td>" + decimal.Round(cumtestvalue, 2) + "</td>";
                            }
                            count++;
                        }

                        if (IsRevisedPlan)
                        {
                            strScript.Append(@"var options = {
                                                                    title: 'Plan vs Achieved Progress Curve',
                                                                    legend: {
                                                                        position: 'top',
                                                                        textStyle: { color: 'gray', fontName: 'Roboto' }
                                                                    },
                                                                    hAxis: {
                                                                        title: 'MONTH',
                                                                        titleTextStyle: {
                                                                            bold: 'true',
                                                                            italic: 'false',
                                                                            fontSize: 12,
                                                                            color: 'gray',
                                                                            fontName: 'Roboto',
                                                                        },
                                                                        textStyle: { color: 'gray', fontName: 'Roboto' }
                                                                    },
                                                                    seriesType: 'bars',
                                                                    //series: { 2: { type: 'line', targetAxisIndex: 1 }, 3: { type: 'line', targetAxisIndex: 1 }, 4: { type: 'line', targetAxisIndex: 1 } },
                                                                    series: {
                                                                        0: {
                                                                            color: '#57c7d4',
                                                                            visibleInLegend: true,
                                                                        },
                                                                        1: {
                                                                            color: '#f96868',
                                                                            visibleInLegend: true,
                                                                        },
                                                                        2: {
                                                                            type: 'line',
                                                                            targetAxisIndex: 1,
                                                                            color: '#b66dff',
                                                                            visibleInLegend: true,
                                                                        },
                                                                        3: {
                                                                            type: 'line',
                                                                            targetAxisIndex: 1,
                                                                            color: '#f2a654',
                                                                            visibleInLegend: true,
                                                                        }
                                                                    },
                                                                    vAxes: {
                                                                        // Adds titles to each axis.

                                                                        0: {
                                                                            title: 'Monthly Plan (%)',
                                                                            titleTextStyle: {
                                                                                bold: 'true',
                                                                                italic: 'false',
                                                                                fontSize: 12,
                                                                                color: 'gray',
                                                                                fontName: 'Roboto',
                                                                            },
                                                                            textStyle: { color: 'gray', fontName: 'Roboto' }
                                                                        },
                                                                        1: {
                                                                            title: 'Cumulative Plan (%)',
                                                                            titleTextStyle: {
                                                                                bold: 'true',
                                                                                italic: 'false',
                                                                                fontSize: 12,
                                                                                color: 'gray',
                                                                                fontName: 'Roboto',
                                                                            },
                                                                            textStyle: { color: 'gray', fontName: 'Roboto' }
                                                                        }
                                                                    }
                                                                };
                                                                var chart = new google.visualization.ComboChart(
                                                                    document.getElementById('chart_divProgress'));
                                                                chart.draw(data, options);
                
            }</script>");
                        }
                        else
                        {
                            strScript.Append(@"var options = {
                                                                    title: 'Plan vs Achieved Progress Curve',
                                                                    legend: {
                                                                        position: 'top',
                                                                        textStyle: { color: 'gray', fontName: 'Roboto' }
                                                                    },
                                                                    hAxis: {
                                                                        title: 'MONTH',
                                                                        titleTextStyle: {
                                                                            bold: 'true',
                                                                            italic: 'false',
                                                                            fontSize: 12,
                                                                            color: 'gray',
                                                                            fontName: 'Roboto',
                                                                        },
                                                                        textStyle: { color: 'gray', fontName: 'Roboto' }
                                                                    },
                                                                    seriesType: 'bars',
                                                                    //series: { 2: { type: 'line', targetAxisIndex: 1 }, 3: { type: 'line', targetAxisIndex: 1 }, 4: { type: 'line', targetAxisIndex: 1 } },
                                                                    series: {
                                                                        0: {
                                                                            color: '#57c7d4',
                                                                            visibleInLegend: true,
                                                                        },
                                                                        1: {
                                                                            color: '#f96868',
                                                                            visibleInLegend: true,
                                                                        },
                                                                        2: {
                                                                            type: 'line',
                                                                            targetAxisIndex: 1,
                                                                            color: '#b66dff',
                                                                            visibleInLegend: true,
                                                                        },
                                                                        3: {
                                                                            type: 'line',
                                                                            targetAxisIndex: 1,
                                                                            color: '#f2a654',
                                                                            visibleInLegend: true,
                                                                        }
                                                                    },
                                                                    vAxes: {
                                                                        // Adds titles to each axis.

                                                                        0: {
                                                                            title: 'Monthly Plan (%)',
                                                                            titleTextStyle: {
                                                                                bold: 'true',
                                                                                italic: 'false',
                                                                                fontSize: 12,
                                                                                color: 'gray',
                                                                                fontName: 'Roboto',
                                                                            },
                                                                            textStyle: { color: 'gray', fontName: 'Roboto' }
                                                                        },
                                                                        1: {
                                                                            title: 'Cumulative Plan (%)',
                                                                            titleTextStyle: {
                                                                                bold: 'true',
                                                                                italic: 'false',
                                                                                fontSize: 12,
                                                                                color: 'gray',
                                                                                fontName: 'Roboto',
                                                                            },
                                                                            textStyle: { color: 'gray', fontName: 'Roboto' }
                                                                        }
                                                                    }
                                                                };
                                                                var chart = new google.visualization.ComboChart(
                                                                    document.getElementById('chart_divProgress'));
                                                                chart.draw(data, options);
                
            }</script>");
                        }



                        //ltScript_Cost.Text = strScript.ToString();
                        ltScript_PhysicalProgress.Text = strScript.ToString();

                        if (IsRevisedPlan)
                        {
                            divtable.InnerHtml = "<table border=\"1\" style=\"text-align:center;font-size:11px;padding-left:0px;\">" +
                                                                      "<tr> " + tablemonths + "</tr>" +
                                                                       "<tr> " + tmonthlyplan + "</tr>" +
                                                                        "<tr> " + tmonthlyactual + "</tr>" +
                                                                        "<tr> " + tmonthlytest + "</tr>" +
                                                                         "<tr> " + tcumulativeplan + "</tr>" +
                                                                          "<tr> " + tcumulativeactual + "</tr>" +
                                                                          "<tr> " + tcumulativetest + "</tr>" +
                                                                              "</table>";
                        }
                        else
                        {
                            divtable.InnerHtml = "<table border=\"1\" style=\"text-align:center;font-size:11px;padding-left:0px;\">" +
                                                                     "<tr> " + tablemonths + "</tr>" +
                                                                      "<tr> " + tmonthlyplan + "</tr>" +
                                                                      "<tr> " + tmonthlyactual + "</tr>" +
                                                                      "<tr> " + tcumulativeplan + "</tr>" +
                                                                      "<tr> " + tcumulativeactual + "</tr>" +
                                                                      "</table>";
                        }

                        btnPrint.Visible = true;
                    }
                    else
                    {
                        ltScript_PhysicalProgress.Text = "<h3>No data</h3>";
                        divtable.InnerHtml = "";
                        btnPrint.Visible = false;
                    }
                }

                //  DateTime t2 = DateTime.Now;
                //  TimeSpan t3 = t2.Subtract(t1);

                //  WriteNewTimeTaken("Physical Progress Chart",DDlProject.SelectedItem.Text,t3);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected void rdSelect_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (rdSelect.SelectedIndex == 0)
            {
                divProgresschart.Visible = false;
                divFinProgressChart.Visible = false;
               
                divdashboardimage.Visible = true;
                
            }
            else if (rdSelect.SelectedIndex == 1)
            {
                divProgresschart.Visible = true;
                divFinProgressChart.Visible = false;
               
                divdashboardimage.Visible = true;
                divMainblocks.Visible = true;
                LoadGraph();
            }
            else if (rdSelect.SelectedIndex == 2)
            {
                divProgresschart.Visible = false;
                divFinProgressChart.Visible = true;
            
                divdashboardimage.Visible = true;
                divMainblocks.Visible = true;
                LoadFinancialGraph();
            }
           
        }

        private void LoadFinancialGraph()
        {
            try
            {
                if (DDLWorkPackage.SelectedValue != "--Select--")
                {
                    ltScript_FinProgress.Text = string.Empty;

                    DataSet ds = getdt.GetFinancialScheduleDatesforGraph(new Guid(DDLWorkPackage.SelectedValue));
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        StringBuilder strScript = new StringBuilder();
                        string tablemonths = "<td style=\"width:200px\">&nbsp;</td>";
                        string tmonthlyplan = "<td style=\"padding:3px\">Monthly Plan</td>";
                        string tmonthlyactual = "<td style=\"padding:3px\">Monthly Actual</td>";
                        string tcumulativeplan = "<td style=\"padding:3px\">Cumulative Plan</td>";
                        string tcumulativeactual = "<td style=\"padding:3px\">Cumulative Actual</td>";
                        strScript.Append(@" <script type='text/javascript'>
                
                google.charts.load('current', { packages: ['corechart', 'bar'] });
                google.charts.setOnLoadCallback(drawBasic);

                function drawBasic() {

                var data = google.visualization.arrayToDataTable([
          ['Month', 'Monthly Plan', { role: 'style' }, 'Monthly Actual', { role: 'style' }, 'Cumulative Plan', { role: 'style' }, 'Cumulative Actual', { role: 'style' }],");
                        int count = 1;
                        DataSet dsvalues = new DataSet();
                        decimal planvalue = 0;
                        decimal actualvalue = 0;
                        decimal cumplanvalue = 0;
                        decimal cumactualvalue = 0;
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            //get the actual and planned values....
                            //dsvalues.Clear();
                            //dsvalues = getdt.GetTaskScheduleValuesForGraph(new Guid(DDLWorkPackage.SelectedValue), Convert.ToDateTime(dr["StartDate"].ToString()), Convert.ToDateTime(dr["StartDate"].ToString()).AddMonths(1));
                            //if (dsvalues.Tables[0].Rows.Count > 0)
                            //{
                            //    planvalue = decimal.Parse(dsvalues.Tables[0].Rows[0]["TotalSchValue"].ToString());
                            //    actualvalue = decimal.Parse(dsvalues.Tables[0].Rows[0]["TotalAchValue"].ToString());
                            //    cumplanvalue += planvalue;
                            //    cumactualvalue += actualvalue;
                            //}
                            dsvalues = getdt.GetFinMonthsPaymentTotal(new Guid(dr["FinMileStoneMonthUID"].ToString()));
                            planvalue = decimal.Parse(dr["AllowedPayment"].ToString());
                            actualvalue = 0;
                            if (dsvalues.Tables[0].Rows.Count > 0)
                            {
                                if (dsvalues.Tables[0].Rows[0]["TotalAmnt"].ToString() != "&nbsp;" && dsvalues.Tables[0].Rows[0]["TotalAmnt"].ToString() != "")
                                {
                                    // e.Row.Cells[2].Text = (decimal.Parse(ds.Tables[0].Rows[0]["TotalAmnt"].ToString()) / 10000000).ToString("n2");
                                    actualvalue = (decimal.Parse(dsvalues.Tables[0].Rows[0]["TotalAmnt"].ToString()) / 10000000);
                                }
                            }
                            // comment this code..used only for demo since actual values are not available....1
                            //Random random = new Random();
                            //if (planvalue > 0)
                            //{
                            //    System.Threading.Thread.Sleep(1000);
                            //    actualvalue = planvalue - random.Next(2,5);
                            //}

                            //
                            cumplanvalue += planvalue;
                            cumactualvalue += actualvalue;
                            if (count < ds.Tables[0].Rows.Count)
                            {

                                strScript.Append("['" + dr["MonthYear"].ToString() + "'," + planvalue + ",'color: #57c7d4'," + actualvalue + ",'color: #f96868'," + cumplanvalue + ",'color: #b66dff'," + cumactualvalue + ",'color: #f2a654'],");
                            }
                            else
                            {
                                strScript.Append("['" + dr["MonthYear"].ToString() + "'," + planvalue + ",'color: #57c7d4'," + actualvalue + ",'color: #f96868'," + cumplanvalue + ",'color: #b66dff'," + cumactualvalue + ",'color: #f2a654']]);");
                            }
                            //
                            tablemonths += "<td style=\"padding:3px\">" + dr["MonthYear"].ToString() + "</td>";
                            tmonthlyplan += "<td style=\"padding:3px\">" + decimal.Round(planvalue, 2) + "</td>";
                            tmonthlyactual += "<td style=\"padding:3px\">" + decimal.Round(actualvalue, 2) + "</td>";

                            tcumulativeplan += "<td style=\"padding:3px\">" + decimal.Round(cumplanvalue, 2) + "</td>";
                            tcumulativeactual += "<td style=\"padding:3px\">" + decimal.Round(cumactualvalue, 2) + "</td>";


                            //
                            count++;
                        }

                        strScript.Append(@"var options = {
                                                                    title: 'Plan vs Achieved Progress Curve',
                                                                    legend: {
                                                                        position: 'top',
                                                                        textStyle: { color: 'gray', fontName: 'Roboto' }
                                                                    },
                                                                    hAxis: {
                                                                        title: 'MONTH',
                                                                        titleTextStyle: {
                                                                            bold: 'true',
                                                                            italic: 'false',
                                                                            fontSize: 12,
                                                                            color: 'gray',
                                                                            fontName: 'Roboto',
                                                                        },
                                                                        textStyle: { color: 'gray', fontName: 'Roboto' }
                                                                    },
                                                                    seriesType: 'bars',
                                                                    series: {
                                                                        0: {
                                                                            color: '#57c7d4',
                                                                            visibleInLegend: true,
                                                                        },
                                                                        1: {
                                                                            color: '#f96868',
                                                                            visibleInLegend: true,
                                                                        },
                                                                        2: {
                                                                            type: 'line',
                                                                            targetAxisIndex: 1,
                                                                            color: '#b66dff',
                                                                            visibleInLegend: true,
                                                                        },
                                                                        3: {
                                                                            type: 'line',
                                                                            targetAxisIndex: 1,
                                                                            color: '#f2a654',
                                                                            visibleInLegend: true,
                                                                        }
                                                                    },
                                                                    vAxes: {
                                                                        // Adds titles to each axis.

                                                                        0: {
                                                                            title: 'Monthly Plan (Crores.)',
                                                                            titleTextStyle: {
                                                                                bold: 'true',
                                                                                italic: 'false',
                                                                                fontSize: 12,
                                                                                color: 'gray',
                                                                                fontName: 'Roboto',
                                                                            },
                                                                            textStyle: { color: 'gray', fontName: 'Roboto' }
                                                                        },
                                                                        1: {
                                                                            title: 'Cumulative Plan (Crores.)',
                                                                            titleTextStyle: {
                                                                                bold: 'true',
                                                                                italic: 'false',
                                                                                fontSize: 12,
                                                                                color: 'gray',
                                                                                fontName: 'Roboto',
                                                                            },
                                                                            textStyle: { color: 'gray', fontName: 'Roboto' }
                                                                        }
                                                                    }
                                                                };
                                                                var chart = new google.visualization.ComboChart(
                                                                    document.getElementById('chart_divProgressFin'));
                                                                chart.draw(data, options);
                
            }</script>");
                        //ltScript_Cost.Text = strScript.ToString();
                        ltScript_FinProgress.Text = strScript.ToString();
                        divtableFin.InnerHtml = "<table border=\"1\" style=\"text-align:center;font-size:11px;padding-left:10px;\">" +
                                          "<tr> " + tablemonths + "</tr>" +
                                           "<tr> " + tmonthlyplan + "</tr>" +
                                            "<tr> " + tmonthlyactual + "</tr>" +
                                             "<tr> " + tcumulativeplan + "</tr>" +
                                              "<tr> " + tcumulativeactual + "</tr>" +
                                                  "</table>";
                        btnPrint.Visible = true;
                    }
                    else
                    {
                        ltScript_FinProgress.Text = "<h3>No data</h3>";
                        divtableFin.InnerHtml = "";
                        btnPrint.Visible = false;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private static int getUserDocsNo()// this is for getting no of docs user has to act on
        {
            DBGetData dbget = new DBGetData();
            int docscount = 0;
            string[] selectedValue = HttpContext.Current.Session["Project_Workpackage"].ToString().Split('_');

            DataSet ds = dbget.GetNextUserDocuments(new Guid(selectedValue[0]), new Guid(selectedValue[1]));
            DataSet dsNxtUser = new DataSet();
            HttpContext.Current.Session["docuids"] = string.Empty;
            // bool backtouser = false;
            foreach (DataRow drnext in ds.Tables[0].Rows)
            {
                // DataSet dsTop = dbget.getTop1_DocumentStatusSelect(new Guid(drnext["ActualDocumentUID"].ToString()));ActualDocuments.ActualDocument_CurrentStatus
                // dsTop.Tables[0].Rows[0]["ActivityType"].ToString()
                DataSet dsNext = dbget.GetNextStep_By_DocumentUID(new Guid(drnext["ActualDocumentUID"].ToString()), drnext["ActualDocument_CurrentStatus"].ToString());

                foreach (DataRow dr in dsNext.Tables[0].Rows)
                {
                    dsNxtUser = new DataSet();
                    dsNxtUser = dbget.GetNextUser_By_DocumentUID(new Guid(drnext["ActualDocumentUID"].ToString()), int.Parse(dr["ForFlow_Step"].ToString()));
                    if (dsNxtUser.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow druser in dsNxtUser.Tables[0].Rows)
                        {
                            if (HttpContext.Current.Session["UserUID"].ToString().ToUpper() == druser["Approver"].ToString().ToUpper())
                            {
                                //if (drnext["ActualDocument_CurrentStatus"].ToString() == "Accepted")
                                //{
                                //    if (dbget.checkUserAddedDocumentstatus(new Guid(drnext["ActualDocumentUID"].ToString()), new Guid(HttpContext.Current.Session["UserUID"].ToString()), drnext["ActualDocument_CurrentStatus"].ToString()) == 0)
                                //    {
                                //        docscount = docscount + 1;
                                //        //
                                //        HttpContext.Current.Session["docuids"] += drnext["ActualDocumentUID"].ToString() + ",";
                                //    }
                                //}
                                //else 
                                if (drnext["ActualDocument_CurrentStatus"].ToString().ToLower().Contains("back to pmc"))
                                {
                                    DataSet dsbackUser = dbget.GetBacktoPMCUsers(new Guid(drnext["ActualDocumentUID"].ToString()));
                                    foreach (DataRow druserb in dsbackUser.Tables[0].Rows)
                                    {
                                        if (druserb["PMCUser"].ToString().ToUpper() == HttpContext.Current.Session["UserUID"].ToString().ToUpper())
                                        {
                                            if (dbget.checkUserAddedDocumentstatus(new Guid(drnext["ActualDocumentUID"].ToString()), new Guid(HttpContext.Current.Session["UserUID"].ToString()), drnext["ActualDocument_CurrentStatus"].ToString()) == 0)
                                            {
                                                docscount = docscount + 1;
                                                //
                                                HttpContext.Current.Session["docuids"] += drnext["ActualDocumentUID"].ToString() + ",";
                                            }
                                        }
                                    }
                                }
                                else if (drnext["ActualDocument_CurrentStatus"].ToString().ToLower().Contains("sent to pmc"))
                                {
                                    DataSet dssentUser = dbget.GetSenttoPMCUsers(new Guid(drnext["ActualDocumentUID"].ToString()));
                                    foreach (DataRow druserb in dssentUser.Tables[0].Rows)
                                    {
                                        if (druserb["PMCUser"].ToString().ToUpper() == HttpContext.Current.Session["UserUID"].ToString().ToUpper())
                                        {
                                            if (dbget.checkUserAddedDocumentstatus(new Guid(drnext["ActualDocumentUID"].ToString()), new Guid(HttpContext.Current.Session["UserUID"].ToString()), drnext["ActualDocument_CurrentStatus"].ToString()) == 0)
                                            {
                                                docscount = docscount + 1;
                                                //
                                                HttpContext.Current.Session["docuids"] += drnext["ActualDocumentUID"].ToString() + ",";
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    docscount = docscount + 1;
                                    //
                                    HttpContext.Current.Session["docuids"] += drnext["ActualDocumentUID"].ToString() + ",";
                                }
                                goto afterloop;
                            }
                            else
                            {


                            }
                        }
                    }

                }

            afterloop:
                Console.WriteLine("/Done");
            }
            return docscount;
        }

        [WebMethod(EnableSession = true)]
        public static string GetDetails(string Id)
        {
            if (WebConfigurationManager.AppSettings["Domain"] != "Suez")
            {
                if (HttpContext.Current.Session["TypeOfUser"].ToString() != "U" && HttpContext.Current.Session["TypeOfUser"].ToString() != "NJSD")
                {
                    string[] selectedValue = HttpContext.Current.Session["Project_Workpackage"].ToString().Split('_');
                    return getUserDocsNo().ToString() + "$" + selectedValue[1];
                }
                else
                {

                    string[] selectedValue = HttpContext.Current.Session["Project_Workpackage"].ToString().Split('_');
                    return "0$" + selectedValue[1];
                }
            }
            else
            {
                string[] selectedValue = HttpContext.Current.Session["Project_Workpackage"].ToString().Split('_');
                return "0$" + selectedValue[1];
            }
        }

        private void Bind_ResourceGraph(string WorkpackageUID)
        {
            if (DDLWorkPackage.SelectedValue != "--Select--")
            {
                Guid FlowUID = new Guid("95AF4084-3F07-4184-8E3F-1CA3A916EE59");
                DataSet ds = getdt.getResourceChart_by_ProjectUID_WorkPackageUID_Flow(new Guid(DDlProject.SelectedValue), new Guid(DDLWorkPackage.SelectedValue), FlowUID);

                if (ds.Tables[0].Rows.Count > 0)
                {
                    // Create a StringBuilder to build your JavaScript code
                    StringBuilder strScript = new StringBuilder();

                    // Append the Google Chart JavaScript code to the StringBuilder
                   // strScript.AppendLine("<script type=\"text/javascript\" src=\"https://www.gstatic.com/charts/loader.js\"></script>");
                    strScript.AppendLine("<script type=\"text/javascript\">");
                    strScript.AppendLine("google.charts.load('current', {'packages':['corechart','bar']});");
                    strScript.AppendLine("google.charts.setOnLoadCallback(drawChart);");

                    strScript.AppendLine("function drawChart() {");
                    strScript.AppendLine("var data = google.visualization.arrayToDataTable([");
                    strScript.AppendLine("['Resources', 'Planned', 'Deployed'],");

                    // Iterate through the rows of the DataSet and add data to the JavaScript code
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        string resource = row[0].ToString();
                        decimal? planned = 0;
                        decimal? deployed = 0;
                        if (row[1] != DBNull.Value)
                        {
                            planned = Convert.ToDecimal(row[1]);
                        }

                        if (row[2] != DBNull.Value)
                        {
                            deployed = Convert.ToDecimal(row[2]);
                        }

                        // Append data for each resource
                        strScript.AppendLine($"['{resource}', {planned}, {deployed}],");
                    }

                    // Remove the trailing comma at the end of the data array
                    strScript.AppendLine("]);");

                    strScript.Append("var options = {");
                    strScript.Append("is3D: true,");
                    strScript.Append("legend: { position: 'top' },");
                    strScript.Append("colors: ['#b66dff', '#57c7d4'],");
                    strScript.Append("fontSize: 11,");
                    strScript.Append("bars: 'horizontal',");
                    strScript.Append("annotations: {");
                    strScript.Append("alwaysOutside: true,");
                    strScript.Append("textStyle: {");
                    strScript.Append("fontSize: 12,");
                    strScript.Append("color: 'black'");
                    strScript.Append("}");
                    strScript.Append("},");
                    strScript.Append("axes: {");
                    strScript.Append("x: {");
                    strScript.Append("0: { side: 'top', label: 'Planned & Deployed Values' }"); // Top x-axis.
                    strScript.Append("}");
                    strScript.Append("},");
                    strScript.Append("hAxis: {");
                    strScript.Append("minValue: 0");
                    strScript.Append("}");
                    strScript.Append("};");

                    strScript.AppendLine("var chart = new google.charts.Bar(document.getElementById('ResourceChart_Div'));");
                    strScript.AppendLine("chart.draw(data, google.charts.Bar.convertOptions(options));");
                    strScript.AppendLine("};");
                    strScript.AppendLine("</script>");

                    ltScript_ResourceGraph.Text = strScript.ToString();
                }
                else
                {
                    ltScript_ResourceGraph.Text = "<h4>No data</h4>";
                }
            }
        }

        private void Bind_DocumentsChart6()
        {
            if (DDLWorkPackage.SelectedValue != "--Select--")
            {
                Guid FlowUID = new Guid("95AF4084-3F07-4184-8E3F-1CA3A916EE59");
                DataSet ds = getdt.getDocumentCount_by_ProjectUID_WorkPackageUID_Flow(new Guid(DDlProject.SelectedValue), new Guid(DDLWorkPackage.SelectedValue), FlowUID);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    StringBuilder strScript = new StringBuilder();

                    // Define JavaScript variables for the counts and delays
                    strScript.AppendLine("<script type='text/javascript'>");
                    strScript.AppendLine("google.charts.load('current', { packages: ['corechart', 'bar'] });");
                    strScript.AppendLine("google.charts.setOnLoadCallback(drawBasic);");

                    // Generate JavaScript code to set variables from dataset values
                    strScript.AppendLine("function drawBasic() {");
                    strScript.AppendLine("var data = google.visualization.arrayToDataTable([");

                    // Define the headers for the data
                    strScript.AppendLine("['Document', 'Count', 'Delayed', { role: 'style' },{ role: 'style' }, { role: 'annotation' }, { role: 'annotation' }],");

                    // Loop through the dataset and add data rows
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        string documentName = row[0].ToString();
                        int count = Convert.ToInt32(row[1]);
                        int delay = Convert.ToInt32(row[2]);

                        // Add a data row for each document
                        strScript.AppendLine("['" + documentName + "', " + count + ", " + delay + ",'#57c7d4', '#f96868', '', ''],");

                    }

                    // Remove the trailing comma from the last row
                    strScript.Length -= 3;

                    strScript.AppendLine("]);");

                    // Rest of your JavaScript code
                    strScript.Append("var options = {");
                    strScript.Append("legend: { position: 'top' },");
                    strScript.Append("colors: ['#57c7d4', '#f96868'],");
                    strScript.Append("fontSize: 11,");
                    strScript.Append("bars: 'horizontal',");
                    strScript.AppendLine("  bar: { groupWidth: '100%' },"); // Adjust '50%' to the desired fixed width
                    strScript.AppendLine("  theme: {");
                    strScript.AppendLine("    axis: {");
                    strScript.AppendLine("      maxTextLines: 5"); // Increase the value to increase line spacing
                    strScript.AppendLine("    }");
                    strScript.AppendLine("  },");
                    strScript.Append("annotations: {");
                    strScript.Append("alwaysOutside: true,");
                    strScript.Append("textStyle: {");
                    strScript.Append("fontSize: 12,");
                    strScript.Append("color: 'black'");
                    strScript.Append("}");
                    strScript.Append("},");
                    strScript.Append("axes: {");
                    strScript.Append("x: {");
                    strScript.Append("0: { side: 'top', label: 'Percentage' }");
                    strScript.Append("}");
                    strScript.Append("},");
                    strScript.Append("hAxis: {");
                    strScript.Append("minValue: 0");
                    strScript.Append("}");
                    strScript.Append("};");


                    //Add count and delay to the 'annotation' columns with respective colors
                    strScript.AppendLine("for (var i = 0; i < data.getNumberOfRows(); i++) {");
                    strScript.AppendLine("    var count = data.getValue(i, 1);");
                    strScript.AppendLine("    var delay = data.getValue(i, 2);");
                    strScript.AppendLine("    data.setValue(i, 3, count.toString());");
                    strScript.AppendLine("    data.setValue(i, 4, delay.toString());");
                    strScript.AppendLine("    data.setValue(i, 5, count.toString());");
                    strScript.AppendLine("    data.setValue(i, 6, delay.toString());");
                    strScript.AppendLine("    data.setFormattedValue(i, 3, count.toString());");
                    strScript.AppendLine("    data.setFormattedValue(i, 4, delay.toString());");
                    strScript.AppendLine("    data.setFormattedValue(i, 5, count.toString());");
                    strScript.AppendLine("    data.setFormattedValue(i, 6, delay.toString());");
                    strScript.AppendLine("    data.setProperty(i, 3, 'style', 'color: #000080; font-weight: bold;');");
                    strScript.AppendLine("    data.setProperty(i, 4, 'style', 'color:#800000; font-weight: bold;');");
                    strScript.AppendLine("    data.setProperty(i, 5, 'style', 'color:#000080; font-weight: bold;');");
                    strScript.AppendLine("    data.setProperty(i, 6, 'style', 'color: #800000; font-weight: bold;');");
                    strScript.AppendLine("}");


                    strScript.AppendLine("function selectHandler() {");
                    strScript.AppendLine("    var selection = chart.getSelection();");
                    strScript.AppendLine("    if (selection.length > 0) {");
                    strScript.AppendLine("        colLabel = data.getColumnLabel(selection[0].column);");
                    strScript.AppendLine("        mydata = data.getValue(selection[0].row, 0);");
                    strScript.AppendLine("        if (colLabel === 'Count') {");
                    strScript.AppendLine("            window.location.href = '/_content_pages/document-chart-drilldown/default.aspx?" +
                    "ProjectUId=" + DDlProject.SelectedValue +
                    "&WorkPackageUID=" + DDLWorkPackage.SelectedValue +
                    "&FlowUID=" + FlowUID +
                    "&colLabel=' + colLabel + " +  // Concatenate JavaScript variable
                    "'&mydata=' + mydata;"); // Concatenate JavaScript variable
                    //strScript.AppendLine("        } else if (colLabel === 'Delayed') {");
                    //strScript.AppendLine("            window.location.href = '/_content_pages/dashboard/default.aspx';");
                    strScript.AppendLine("        }");
                    strScript.AppendLine("    }");
                    strScript.AppendLine("}");


                    // Rest of your JavaScript code
                    strScript.AppendLine("var chart = new google.visualization.BarChart(document.getElementById('DocChart_Div'));");
                    strScript.AppendLine("google.visualization.events.addListener(chart, 'select', selectHandler);");
                    strScript.AppendLine("chart.draw(data, options);");

                    strScript.AppendLine("}</script>");

                    ltScript_Document.Text = strScript.ToString();
                }
                else
                {
                    ltScript_Document.Text = "<h4>No data</h4>";
                }

                DataSet dataSet = getdt.getMasterListSubmissionFlow(new Guid(DDlProject.SelectedValue), new Guid(DDLWorkPackage.SelectedValue), FlowUID);
                if (dataSet.Tables[0].Rows.Count > 0)
                {
                    int totalDocuments = Convert.ToInt32(dataSet.Tables[0].Rows[0][0]);
                    int pendingCount = Convert.ToInt32(dataSet.Tables[0].Rows[0][1]);
                    int delayedCount = Convert.ToInt32(dataSet.Tables[0].Rows[0][2]);

                    lblTotalMasterListDocuments.Text = $"<span style='color: #f96868;'><b>Total Master List Documents : {totalDocuments}</b></span> &nbsp;&nbsp; " +
        $"<span style='color: #5E50F9;'><b>Pending : {pendingCount}</b></span> &nbsp;&nbsp; " +
        $"<span style='color: #57c7d4;'><b>Delayed : {delayedCount}</b></span>";
                }

            }
        }
    }
}