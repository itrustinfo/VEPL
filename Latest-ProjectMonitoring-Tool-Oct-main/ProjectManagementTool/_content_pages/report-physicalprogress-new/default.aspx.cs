using ProjectManager.DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.Web.Configuration;
using System.IO;
using System.Text;
using ProjectManagementTool.Models;
using System.Reflection;

namespace ProjectManagementTool._content_pages.report_physicalprogress_new
{
    public partial class _default : System.Web.UI.Page
    {
        DBGetData getdt = new DBGetData();
        TaskUpdate gettk = new TaskUpdate();
        double PreviousTarget = 0;
        double PreviousActual = 0;
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
                    BindProject();
                    ReportFormat.Visible = false;
                    MonthlyPhysicalProgress.Visible = false;
                    QuarterPhysicalProgress.Visible = false;
                    HalfYearPhysicalProgress.Visible = false;
                    ByHalfYear.Visible = false;
                    ByMonth.Visible = false;
                    ByWeek.Visible = false;
                    ByQuarteMonth.Visible = false;
                    Byfortingiht.Visible = false;
                    WeeklyProgressReport.Visible = false;
                    AcrossMonthsProgressReport.Visible = false;
                    FortiProgressReport.Visible = false;
                    ByActivity.Visible = false;
                    ActivityProgressReport.Visible = false;
                    AllMilestoneGraph.Visible = false;
                }
            }
        }

        private void BindProject()
        {
            DataTable ds = new DataTable();
            if (Session["TypeOfUser"].ToString() == "U" || Session["TypeOfUser"].ToString() == "MD" || Session["TypeOfUser"].ToString() == "VP")
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

            DDlProject.Items.Insert(0, new System.Web.UI.WebControls.ListItem("-- Select --", ""));
            DDLWorkPackage.Items.Insert(0, new System.Web.UI.WebControls.ListItem("-- Select --", ""));

        }

        protected void DDlProject_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (DDlProject.SelectedValue != "")
            {
                DataSet ds = new DataSet();
                if (Session["TypeOfUser"].ToString() == "U" || Session["TypeOfUser"].ToString() == "MD" || Session["TypeOfUser"].ToString() == "VP")
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
                if (ds.Tables[0].Rows.Count > 0)
                {
                    DDLWorkPackage.DataTextField = "Name";
                    DDLWorkPackage.DataValueField = "WorkPackageUID";
                    DDLWorkPackage.DataSource = ds;
                    DDLWorkPackage.DataBind();

                    DDLWorkPackage_SelectedIndexChanged(sender, e);
                }
            }
        }

        protected void DDLWorkPackage_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (DDLWorkPackage.SelectedValue != "")
            {
                //DataSet ds = getdt.GetConstructionProgramme_Tasks(new Guid(DDLWorkPackage.SelectedValue));
                //GrdMonthPhysicalProgress.DataSource = ds;
                //GrdMonthPhysicalProgress.DataBind();
                //BindMonths(DDLWorkPackage.SelectedValue);
                ReportFormat.Visible = true;
                Bind_Year(DDLWorkPackage.SelectedValue);
                ByMonth.Visible = false;
                ByWeek.Visible = false;
                ByActivity.Visible = false;
                AcrossMonthsProgressReport.Visible = false;
                ActivityProgressReport.Visible = false;
                WeeklyProgressReport.Visible = false;
                MonthlyPhysicalProgress.Visible = false;
                QuarterPhysicalProgress.Visible = false;
                HalfYearPhysicalProgress.Visible = false;
                RBLReportFor.ClearSelection();
                RBLReportFor.Visible = false;
                BindMilestones();
                AllMilestoneGraph.Visible = true;

            }
        }

        private void Bind_Year(string WorkpackageUID)
        {
            DataSet ds = getdt.GetWorkPackages_By_WorkPackageUID(new Guid(WorkpackageUID));
            if (ds.Tables[0].Rows.Count > 0)
            {
                if (ds.Tables[0].Rows[0]["StartDate"].ToString() != "" && ds.Tables[0].Rows[0]["PlannedEndDate"].ToString() != "")
                {
                    try
                    {
                        DDLYear.Items.Clear();
                        ddlQYear.Items.Clear();
                        ddlHalfYear.Items.Clear();
                        DDLYear.Items.Add(new System.Web.UI.WebControls.ListItem("--Select Year--", ""));
                        ddlQYear.Items.Add(new System.Web.UI.WebControls.ListItem("--Select Year--", ""));
                        ddlHalfYear.Items.Add(new System.Web.UI.WebControls.ListItem("--Select Year--", ""));
                        DDLMonth.Items.Add(new System.Web.UI.WebControls.ListItem("--Select Month--", ""));

                        int StartYear = Convert.ToDateTime(ds.Tables[0].Rows[0]["StartDate"].ToString()).Year;
                        int EndDateYear = Convert.ToDateTime(ds.Tables[0].Rows[0]["PlannedEndDate"].ToString()).Year;
                        for (int i = StartYear; i <= EndDateYear; i++)
                        {
                            DDLYear.Items.Add(new System.Web.UI.WebControls.ListItem(i.ToString(), i.ToString()));
                            ddlQYear.Items.Add(new System.Web.UI.WebControls.ListItem(i.ToString(), i.ToString()));
                            ddlHalfYear.Items.Add(new System.Web.UI.WebControls.ListItem(i.ToString(), i.ToString()));
                        }
                    }
                    catch (Exception ex)
                    {
                        Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>alert('Error : " + ex.Message + "');</script>");
                    }
                }
            }

        }
        //private void BindMonths(string WorkpackageUID)
        //{
        //    DataSet ds = getdt.GetWorkPackages_By_WorkPackageUID(new Guid(WorkpackageUID));
        //    if (ds.Tables[0].Rows.Count > 0)
        //    {
        //        DateTime StartDate = Convert.ToDateTime(ds.Tables[0].Rows[0]["StartDate"].ToString());
        //        DateTime EndDate = Convert.ToDateTime(ds.Tables[0].Rows[0]["PlannedEndDate"].ToString());
        //        int i = 0;
        //        foreach (DateTime day in EachCalendarDay(StartDate, EndDate))
        //        {
        //            DDLMonth.Items.Insert(i, day.ToString("MMM yyyy"));
        //            i += 1;
        //        }
        //    }
        //}

        //public IEnumerable<DateTime> EachCalendarDay(DateTime startDate, DateTime endDate)
        //{
        //    for (var date = startDate.Date; date.Date <= endDate.Date; date = date.AddDays(1)) yield
        //    return date;
        //}

        protected void RBLReportFor_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (RBLReportFor.SelectedValue == "By Week")
            {
                Byfortingiht.Visible = false;
                ByMonth.Visible = false;
                ByWeek.Visible = true;
                ByActivity.Visible = false;
                ByQuarteMonth.Visible = false;
                AcrossMonthsProgressReport.Visible = false;
                ActivityProgressReport.Visible = false;
                MonthlyPhysicalProgress.Visible = false;
                QuarterPhysicalProgress.Visible = false;
                WeeklyProgressReport.Visible = false;
                FortiProgressReport.Visible = false;
                Byfortingiht.Visible = false;
                ByHalfYear.Visible = false;
                HalfYearPhysicalProgress.Visible = false;
                AllMilestoneGraph.Visible = false;

                DateTime start = DateTime.Today;// Adjust to your start date

                DataSet ds = getdt.GetWorkPackages_By_WorkPackageUID(new Guid(DDLWorkPackage.SelectedValue));
                DDLWeekYear.Items.Clear();
                if (ds.Tables[0].Rows.Count > 0)
                {
                    if (ds.Tables[0].Rows[0]["StartDate"].ToString() != "" && ds.Tables[0].Rows[0]["PlannedEndDate"].ToString() != "")
                    {
                        DDLWeekYear.Items.Add(new System.Web.UI.WebControls.ListItem("--Select Year--", ""));
                        DDLWeekMonth.Items.Add(new System.Web.UI.WebControls.ListItem("--Select Month--", ""));
                        int StartYear = Convert.ToDateTime(ds.Tables[0].Rows[0]["StartDate"].ToString()).Year;
                        int EndDateYear = Convert.ToDateTime(ds.Tables[0].Rows[0]["PlannedEndDate"].ToString()).Year;
                        for (int i = StartYear; i <= EndDateYear; i++)
                        {
                            DDLWeekYear.Items.Add(new System.Web.UI.WebControls.ListItem(i.ToString(), i.ToString()));
                        }
                    }

                }
            }
            else if (RBLReportFor.SelectedValue == "By Fortnightly")
            {
                ByMonth.Visible = false;
                ByWeek.Visible = false;
                Byfortingiht.Visible = true;
                ByActivity.Visible = false;
                ByQuarteMonth.Visible = false;
                AcrossMonthsProgressReport.Visible = false;
                ActivityProgressReport.Visible = false;
                MonthlyPhysicalProgress.Visible = false;
                WeeklyProgressReport.Visible = false;
                QuarterPhysicalProgress.Visible = false;
                FortiProgressReport.Visible = false;
                ByHalfYear.Visible = false;
                HalfYearPhysicalProgress.Visible = false;
                AllMilestoneGraph.Visible = false;

                DateTime start = DateTime.Today;// Adjust to your start date

                DataSet ds = getdt.GetWorkPackages_By_WorkPackageUID(new Guid(DDLWorkPackage.SelectedValue));
                if (ds.Tables[0].Rows.Count > 0)
                {
                    if (ds.Tables[0].Rows[0]["StartDate"].ToString() != "" && ds.Tables[0].Rows[0]["PlannedEndDate"].ToString() != "")
                    {
                        DDLFortYear.Items.Add(new System.Web.UI.WebControls.ListItem("--Select Year--", ""));
                        DDLFortiMonth.Items.Add(new System.Web.UI.WebControls.ListItem("--Select Month--", ""));
                        int StartYear = Convert.ToDateTime(ds.Tables[0].Rows[0]["StartDate"].ToString()).Year;
                        int EndDateYear = Convert.ToDateTime(ds.Tables[0].Rows[0]["PlannedEndDate"].ToString()).Year;
                        for (int i = StartYear; i <= EndDateYear; i++)
                        {
                            DDLFortYear.Items.Add(new System.Web.UI.WebControls.ListItem(i.ToString(), i.ToString()));
                        }
                    }

                }
            }
            else if (RBLReportFor.SelectedValue == "Across Months")
            {
                Byfortingiht.Visible = false;
                ByMonth.Visible = false;
                ByWeek.Visible = false;
                ByActivity.Visible = false;
                ByQuarteMonth.Visible = false;
                Byfortingiht.Visible = false;
                AcrossMonthsProgressReport.Visible = true;
                ActivityProgressReport.Visible = false;
                WeeklyProgressReport.Visible = false;
                MonthlyPhysicalProgress.Visible = false;
                QuarterPhysicalProgress.Visible = false;
                FortiProgressReport.Visible = false;
                ByHalfYear.Visible = false;
                HalfYearPhysicalProgress.Visible = false;
                AllMilestoneGraph.Visible = false;

                BindAcrossMontsPhysicalProgress(DDLWorkPackage.SelectedValue);
            }
            else if (RBLReportFor.SelectedValue == "By Activity")
            {
                Byfortingiht.Visible = false;
                DDLActivity.Items.Clear();
                ByMonth.Visible = false;
                ByWeek.Visible = false;
                ByQuarteMonth.Visible = false;
                ByActivity.Visible = true;
                Byfortingiht.Visible = false;
                AcrossMonthsProgressReport.Visible = false;
                ActivityProgressReport.Visible = false;
                WeeklyProgressReport.Visible = false;
                MonthlyPhysicalProgress.Visible = false;
                QuarterPhysicalProgress.Visible = false;
                FortiProgressReport.Visible = false;
                ByHalfYear.Visible = false;
                HalfYearPhysicalProgress.Visible = false;
                AllMilestoneGraph.Visible = false;

                DataTable ds = getdt.GetConstructionProgramme_Tasks(new Guid(DDLWorkPackage.SelectedValue)).Tables[0].AsEnumerable()
                     .OrderBy(r => r.Field<string>("Name"))
                     .CopyToDataTable();

                foreach (DataRow dr in ds.Rows)
                {
                    System.Web.UI.WebControls.ListItem lst = new System.Web.UI.WebControls.ListItem(dr["Name"].ToString() + " (" + getTaskHeirarchy(new Guid(dr["TaskUID"].ToString())) + ")", dr["TaskUID"].ToString());
                    DDLActivity.Items.Add(lst);
                }

                DDLActivity.Items.Insert(0, new System.Web.UI.WebControls.ListItem("-- Select Activity--", ""));
            }
            else if (RBLReportFor.SelectedValue == "By Quarter")
            {
                Byfortingiht.Visible = false;
                ByMonth.Visible = false;
                ByQuarteMonth.Visible = true;
                ByWeek.Visible = false;
                ByActivity.Visible = false;
                Byfortingiht.Visible = false;
                AcrossMonthsProgressReport.Visible = false;
                ActivityProgressReport.Visible = false;
                WeeklyProgressReport.Visible = false;
                MonthlyPhysicalProgress.Visible = false;
                QuarterPhysicalProgress.Visible = false;
                FortiProgressReport.Visible = false;
                ByHalfYear.Visible = false;
                HalfYearPhysicalProgress.Visible = false;
                AllMilestoneGraph.Visible = false;
            }
            else if (RBLReportFor.SelectedValue == "By HalfYear")
            {
                Byfortingiht.Visible = false;
                ByMonth.Visible = false;
                ByQuarteMonth.Visible = false;
                ByWeek.Visible = false;
                ByActivity.Visible = false;
                Byfortingiht.Visible = false;
                AcrossMonthsProgressReport.Visible = false;
                ActivityProgressReport.Visible = false;
                WeeklyProgressReport.Visible = false;
                MonthlyPhysicalProgress.Visible = false;
                QuarterPhysicalProgress.Visible = false;
                FortiProgressReport.Visible = false;
                ByHalfYear.Visible = true;
                HalfYearPhysicalProgress.Visible = false;
                AllMilestoneGraph.Visible = false;
            }
            else
            {
                Byfortingiht.Visible = false;
                ByMonth.Visible = true;
                ByQuarteMonth.Visible = false;
                ByWeek.Visible = false;
                ByActivity.Visible = false;
                Byfortingiht.Visible = false;
                AcrossMonthsProgressReport.Visible = false;
                ActivityProgressReport.Visible = false;
                WeeklyProgressReport.Visible = false;
                MonthlyPhysicalProgress.Visible = false;
                QuarterPhysicalProgress.Visible = false;
                FortiProgressReport.Visible = false;
                ByHalfYear.Visible = false;
                HalfYearPhysicalProgress.Visible = false;
                AllMilestoneGraph.Visible = false;
            }
        }

        protected void BntSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                if (DDLYear.SelectedValue != "" && DDLMonth.SelectedValue != "")
                {
                    if (DDLMilestones.Enabled)
                    {
                        if (DDLMilestones.SelectedIndex > 0)
                        {
                            MileStoneGraph();
                        }
                        else
                        {
                            AllMileStoneGraph();
                        }
                    }
                    else
                    {
                        BindHalfYearProjectProgressReport();
                    }
                }
                else
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>alert('Please select Month or Year');</script>");
                }
            }
            catch (Exception ex)
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>alert('There is a problem with this feature. Please contact system admin.');</script>");
            }
        }

        protected void BindMonthlyProjectProgressReport()
        {
            ViewState["Export"] = "1";
            MonthlyProgressReportName.InnerHtml = "Report Name : Physical Progress Achievements in the month " + DDLMonth.SelectedItem.Text + " " + DDLYear.SelectedValue;
            MonthlyProgressProjectName.InnerHtml = "Project Name : " + DDlProject.SelectedItem.Text + " (" + DDLWorkPackage.SelectedItem.Text + ")";
            DataSet ds = getdt.GetConstructionProgramme_Tasks(new Guid(DDLWorkPackage.SelectedValue));
            GrdMonthPhysicalProgress.DataSource = ds;
            GrdMonthPhysicalProgress.DataBind();

            MonthlyPhysicalProgress.Visible = true;
            if (ds.Tables[0].Rows.Count > 0)
            {
                btnMonthlyProgressExportPDF.Visible = false; // must be true
                btnMonthlyProgressPrint.Visible = false; // must be true
                btnMonthlyProgressExporttoExcel.Visible = true;
            }
        }

        protected void GrdMonthPhysicalProgress_DataBound(object sender, EventArgs e)
        {
            if (ViewState["Export"].ToString() == "1")
            {
                GridViewRow row = new GridViewRow(0, 0, DataControlRowType.Header, DataControlRowState.Normal);
                TableHeaderCell cell = new TableHeaderCell();
                cell.Text = "";
                cell.ColumnSpan = 1;
                row.Controls.Add(cell);

                cell = new TableHeaderCell();
                cell.Text = "";
                cell.ColumnSpan = 1;
                row.Controls.Add(cell);

                cell = new TableHeaderCell();
                cell.Text = "";
                cell.ColumnSpan = 1;
                row.Controls.Add(cell);

                cell = new TableHeaderCell();
                cell.Text = "";
                cell.ColumnSpan = 1;
                row.Controls.Add(cell);

                cell = new TableHeaderCell();
                cell.Text = "";
                cell.ColumnSpan = 1;
                row.Controls.Add(cell);

                cell = new TableHeaderCell();
                cell.Text = "";
                cell.ColumnSpan = 1;
                row.Controls.Add(cell);

                cell = new TableHeaderCell();
                cell.Text = "";
                cell.ColumnSpan = 1;
                row.Controls.Add(cell);

                cell = new TableHeaderCell();
                cell.Text = "";
                cell.ColumnSpan = 1;
                row.Controls.Add(cell);

                cell = new TableHeaderCell();
                cell.Text = "";
                cell.ColumnSpan = 1;
                row.Controls.Add(cell);

                cell = new TableHeaderCell();
                cell.Text = "";
                cell.ColumnSpan = 1;
                row.Controls.Add(cell);

                cell = new TableHeaderCell();
                cell.ColumnSpan = 1;
                cell.Text = "Up to previous Month";
                //cell.HorizontalAlign
                row.Controls.Add(cell);

                cell = new TableHeaderCell();
                cell.ColumnSpan = 3;
                cell.Text = "For this Month";
                row.Controls.Add(cell);

                cell = new TableHeaderCell();
                cell.ColumnSpan = 3;
                cell.Text = "Cumulative for the Project";
                row.Controls.Add(cell);

                //row.BackColor = ColorTranslator.FromHtml("#3AC0F2");
                GrdMonthPhysicalProgress.HeaderRow.Parent.Controls.AddAt(0, row);
            }
        }

        protected void GrdMonthPhysicalProgress_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                string TaskUID = GrdMonthPhysicalProgress.DataKeys[e.Row.RowIndex].Values[0].ToString();

                string sDate1 = "";
                DateTime CDate1 = DateTime.Now;

                sDate1 = "01/" + DDLMonth.SelectedValue + "/" + DDLYear.SelectedValue;
                sDate1 = getdt.ConvertDateFormat(sDate1);
                CDate1 = Convert.ToDateTime(sDate1);

                if (e.Row.Cells[2 + 6].Text != "")
                {

                    if (e.Row.Cells[2 + 6].Text.ToUpper() == "METERS")
                    {
                        e.Row.Cells[3 + 6].Text = e.Row.Cells[3 + 6].Text != "" ? Convert.ToDouble(e.Row.Cells[3 + 6].Text).ToString("N2") : "";
                    }
                    else if (e.Row.Cells[2 + 6].Text.ToUpper() == "NUMBERS")
                    {
                        e.Row.Cells[3 + 6].Text = e.Row.Cells[3 + 6].Text;
                    }
                    else if (e.Row.Cells[2 + 6].Text.ToUpper() == "RMT")
                    {
                        e.Row.Cells[3 + 6].Text = e.Row.Cells[3 + 6].Text != "" ? Convert.ToDouble(e.Row.Cells[3 + 6].Text).ToString("N2") : "";
                    }
                    else if (e.Row.Cells[2 + 6].Text.ToUpper() == "TONS")
                    {
                        e.Row.Cells[3 + 6].Text = e.Row.Cells[3 + 6].Text != "" ? Convert.ToDouble(e.Row.Cells[3 + 6].Text).ToString("N2") : "";
                    }
                    else if (e.Row.Cells[2 + 6].Text.ToUpper() == "KM")
                    {
                        e.Row.Cells[3 + 6].Text = e.Row.Cells[3 + 6].Text != "" ? Convert.ToDecimal(e.Row.Cells[3 + 6].Text).ToString("N2") : "";
                    }
                    else if (e.Row.Cells[2 + 6].Text.ToUpper() == "LOT")
                    {
                        e.Row.Cells[3 + 6].Text = e.Row.Cells[3 + 6].Text != "" ? Convert.ToDouble(e.Row.Cells[3 + 6].Text).ToString("N2") : "";
                    }
                    else if (e.Row.Cells[2 + 6].Text.ToUpper() == "HOUR")
                    {
                        e.Row.Cells[3 + 6].Text = e.Row.Cells[3 + 6].Text != "" ? Convert.ToDouble(e.Row.Cells[3 + 6].Text).ToString("N2") : "";
                    }
                    else
                    {
                        e.Row.Cells[3 + 6].Text = e.Row.Cells[3 + 6].Text != "" ? Convert.ToDouble(e.Row.Cells[3 + 6].Text).ToString("N2") : "";
                    }
                }
                DataSet ds = getdt.GetPhysicalProgress_ForMonth_by_TaskUID(new Guid(TaskUID), CDate1);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    e.Row.Cells[4 + 6].Text = ds.Tables[0].Rows[0]["PrevAchieved"].ToString() != "" ? decimal.Round(Convert.ToDecimal(ds.Tables[0].Rows[0]["PrevAchieved"].ToString()), 2).ToString() : "";
                    e.Row.Cells[5 + 6].Text = ds.Tables[0].Rows[0]["PlannedThisMonth"].ToString() != "" ? decimal.Round(Convert.ToDecimal(ds.Tables[0].Rows[0]["PlannedThisMonth"].ToString()), 2).ToString() : "";
                    e.Row.Cells[6 + 6].Text = ds.Tables[0].Rows[0]["AchievedThisMonth"].ToString() != "" ? decimal.Round(Convert.ToDecimal(ds.Tables[0].Rows[0]["AchievedThisMonth"].ToString()), 2).ToString() : "";
                    e.Row.Cells[7 + 6].Text = ds.Tables[0].Rows[0]["AchievedPercentage"].ToString() != "" ? decimal.Round(Convert.ToDecimal(ds.Tables[0].Rows[0]["AchievedPercentage"].ToString()), 2).ToString() : "";
                    e.Row.Cells[8 + 6].Text = ds.Tables[0].Rows[0]["CumulativePlanned"].ToString() != "" ? decimal.Round(Convert.ToDecimal(ds.Tables[0].Rows[0]["CumulativePlanned"].ToString()), 2).ToString() : "";
                    e.Row.Cells[9 + 6].Text = ds.Tables[0].Rows[0]["CumulativeAchieved"].ToString() != "" ? decimal.Round(Convert.ToDecimal(ds.Tables[0].Rows[0]["CumulativeAchieved"].ToString()), 2).ToString() : "";
                    e.Row.Cells[10 + 6].Text = ds.Tables[0].Rows[0]["Balance"].ToString() != "" ? decimal.Round(Convert.ToDecimal(ds.Tables[0].Rows[0]["Balance"].ToString()), 2).ToString() : "";
                    e.Row.Cells[11 + 6].Text = ds.Tables[0].Rows[0]["NextMonthPlan"].ToString() != "" ? decimal.Round(Convert.ToDecimal(ds.Tables[0].Rows[0]["NextMonthPlan"].ToString()), 2).ToString() : "";
                    e.Row.Cells[12 + 6].Text = ds.Tables[0].Rows[0]["OverAllCompletion"].ToString() != "" ? decimal.Round(Convert.ToDecimal(ds.Tables[0].Rows[0]["OverAllCompletion"].ToString()), 2).ToString() : "";
                }

                if (e.Row.Cells[19].Text == "6")
                {
                    e.Row.Cells[2].Text = "";
                    e.Row.Cells[3].Text = "";
                    e.Row.Cells[4].Text = "";
                    e.Row.Cells[5].Text = "";
                    e.Row.Cells[6].Text = "";
                    e.Row.Cells[7].Text = "";
                }

                if (e.Row.Cells[19].Text == "7")
                {
                    e.Row.Cells[1].Text = "";
                    e.Row.Cells[3].Text = "";
                    e.Row.Cells[4].Text = "";
                    e.Row.Cells[5].Text = "";
                    e.Row.Cells[6].Text = "";
                    e.Row.Cells[7].Text = "";
                }

                if (e.Row.Cells[19].Text == "8")
                {
                    e.Row.Cells[1].Text = "";
                    e.Row.Cells[2].Text = "";
                    e.Row.Cells[4].Text = "";
                    e.Row.Cells[5].Text = "";
                    e.Row.Cells[6].Text = "";
                    e.Row.Cells[7].Text = "";
                }

                if (e.Row.Cells[19].Text == "9")
                {
                    e.Row.Cells[1].Text = "";
                    e.Row.Cells[2].Text = "";
                    e.Row.Cells[3].Text = "";
                    e.Row.Cells[5].Text = "";
                    e.Row.Cells[6].Text = "";
                    e.Row.Cells[7].Text = "";
                }

                if (e.Row.Cells[19].Text == "10")
                {
                    e.Row.Cells[1].Text = "";
                    e.Row.Cells[2].Text = "";
                    e.Row.Cells[3].Text = "";
                    e.Row.Cells[4].Text = "";
                    e.Row.Cells[6].Text = "";
                    e.Row.Cells[7].Text = "";
                }

                if (e.Row.Cells[19].Text == "11")
                {
                    e.Row.Cells[1].Text = "";
                    e.Row.Cells[2].Text = "";
                    e.Row.Cells[3].Text = "";
                    e.Row.Cells[4].Text = "";
                    e.Row.Cells[5].Text = "";
                    e.Row.Cells[7].Text = "";
                }

                if (e.Row.Cells[19].Text == "12")
                {
                    e.Row.Cells[1].Text = "";
                    e.Row.Cells[2].Text = "";
                    e.Row.Cells[3].Text = "";
                    e.Row.Cells[4].Text = "";
                    e.Row.Cells[5].Text = "";
                    e.Row.Cells[6].Text = "";
                }

            }

        }

        protected void btnWeekSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                if (DDLWeek.SelectedValue != "")
                {
                    if (DDLMilestones.Enabled)
                    {
                        if (DDLMilestones.SelectedIndex > 0)
                        {
                            MileStoneGraph();
                        }
                        else
                        {
                            AllMileStoneGraph();
                        }

                    }
                    else
                    {
                        BindWeeklyProgressReport();
                    }

                }
                else
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>alert('Please select Week');</script>");
                }
            }
            catch (Exception ex)
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>alert('There is a problem with this feature. Please contact system admin.');</script>");
            }
        }

        protected void btnFortiSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                if (DDLFortWeek.SelectedValue != "")
                {
                    if (DDLMilestones.Enabled)
                    {
                        if (DDLMilestones.SelectedIndex > 0)
                        {
                            MileStoneGraph();
                        }
                        else
                        {
                            AllMileStoneGraph();
                        }
                    }
                    else
                    {
                        BindFortiProgressReport();
                    }

                }
                else
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>alert('Please select Week');</script>");
                }
            }
            catch (Exception ex)
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>alert('There is a problem with this feature. Please contact system admin.');</script>");
            }
        }

        protected void BindWeeklyProgressReport()
        {
            ViewState["Export"] = "1";
            //LblWeeklyHeading.Text = "Weekly work Progress Status as on " + DDLWeek.SelectedValue.Split('-')[1];
            WeeklyReportNameHeading.InnerHtml = "Report Name : Weekly work Progress Status as on " + DDLWeek.SelectedValue.Split('-')[1];
            WeeklyProjectName.InnerHtml = "Project Name : " + DDlProject.SelectedItem.Text + " (" + DDLWorkPackage.SelectedItem.Text + ")";
            DataSet ds = getdt.GetConstructionProgramme_Tasks(new Guid(DDLWorkPackage.SelectedValue));
            GrdWeeklyprogress.DataSource = ds;
            GrdWeeklyprogress.DataBind();
            MonthlyPhysicalProgress.Visible = false;
            WeeklyProgressReport.Visible = true;

            if (ds.Tables[0].Rows.Count > 0)
            {

                btnExportReportPDF.Visible = false;
                btnPrintPDF.Visible = false;
                btnExportReportExcel.Visible = true;
            }
        }

        protected void GrdWeeklyprogress_DataBound(object sender, EventArgs e)
        {
            if (ViewState["Export"].ToString() == "1")
            {
                GridViewRow row = new GridViewRow(0, 0, DataControlRowType.Header, DataControlRowState.Normal);

                TableHeaderCell cell = new TableHeaderCell();
                cell.Text = "";
                cell.ColumnSpan = 1;
                row.Controls.Add(cell);

                cell = new TableHeaderCell();
                cell.Text = "";
                cell.ColumnSpan = 1;
                row.Controls.Add(cell);

                cell = new TableHeaderCell();
                cell.Text = "";
                cell.ColumnSpan = 1;
                row.Controls.Add(cell);

                cell = new TableHeaderCell();
                cell.Text = "";
                cell.ColumnSpan = 1;
                row.Controls.Add(cell);

                cell = new TableHeaderCell();
                cell.Text = "";
                cell.ColumnSpan = 1;
                row.Controls.Add(cell);

                cell = new TableHeaderCell();
                cell.Text = "";
                cell.ColumnSpan = 1;
                row.Controls.Add(cell);

                cell = new TableHeaderCell();
                cell.Text = "";
                cell.ColumnSpan = 1;
                row.Controls.Add(cell);

                cell = new TableHeaderCell();
                cell.Text = "";
                cell.ColumnSpan = 1;
                row.Controls.Add(cell);

                cell = new TableHeaderCell();
                cell.Text = "";
                cell.ColumnSpan = 1;
                row.Controls.Add(cell);

                cell = new TableHeaderCell();
                cell.ColumnSpan = 2;
                cell.Text = "Target as on " + DDLWeek.SelectedItem.Text.Split('-')[1].Trim();
                //cell.HorizontalAlign
                row.Controls.Add(cell);

                cell = new TableHeaderCell();
                cell.ColumnSpan = 2;
                cell.Text = "Achieved as on " + DDLWeek.SelectedItem.Text.Split('-')[1].Trim();
                row.Controls.Add(cell);

                //row.BackColor = ColorTranslator.FromHtml("#3AC0F2");
                GrdWeeklyprogress.HeaderRow.Parent.Controls.AddAt(0, row);
            }
        }

        protected void GrdWeeklyprogress_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                string TaskUID = GrdWeeklyprogress.DataKeys[e.Row.RowIndex].Values[0].ToString();

                string sDate1 = "", sDate2 = "";
                DateTime CDate1 = DateTime.MinValue, CDate2 = DateTime.MinValue;

                sDate1 = DDLWeek.SelectedValue.Split('-')[0].Trim();
                sDate1 = getdt.ConvertDateFormat(sDate1);
                CDate1 = Convert.ToDateTime(sDate1);

                sDate2 = DDLWeek.SelectedValue.Split('-')[1].Trim();
                sDate2 = getdt.ConvertDateFormat(sDate2);
                CDate2 = Convert.ToDateTime(sDate2);

                DataSet ds = getdt.GetPhysicalProgress_ForWeek__by_TaskUID(new Guid(TaskUID), CDate1, CDate2);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    e.Row.Cells[3 + 6].Text = ds.Tables[0].Rows[0]["CumulativePlanned"].ToString() != "" ? decimal.Round(Convert.ToDecimal(ds.Tables[0].Rows[0]["CumulativePlanned"].ToString()), 2).ToString() : "";
                    e.Row.Cells[4 + 6].Text = ds.Tables[0].Rows[0]["PlanFortheWeek"].ToString() != "" ? decimal.Round(Convert.ToDecimal(ds.Tables[0].Rows[0]["PlanFortheWeek"].ToString()), 2).ToString() : "";
                    e.Row.Cells[5 + 6].Text = ds.Tables[0].Rows[0]["CumulativeAchieved"].ToString() != "" ? decimal.Round(Convert.ToDecimal(ds.Tables[0].Rows[0]["CumulativeAchieved"].ToString()), 2).ToString() : "";
                    e.Row.Cells[6 + 6].Text = ds.Tables[0].Rows[0]["AchievedFortheWeek"].ToString() != "" ? decimal.Round(Convert.ToDecimal(ds.Tables[0].Rows[0]["AchievedFortheWeek"].ToString()), 2).ToString() : "";
                    e.Row.Cells[7 + 6].Text = ds.Tables[0].Rows[0]["ProgressPercentage"].ToString() != "" ? decimal.Round(Convert.ToDecimal(ds.Tables[0].Rows[0]["ProgressPercentage"].ToString()), 2).ToString() : "";
                }

                if (e.Row.Cells[14].Text == "6")
                {
                    e.Row.Cells[2].Text = "";
                    e.Row.Cells[3].Text = "";
                    e.Row.Cells[4].Text = "";
                    e.Row.Cells[5].Text = "";
                    e.Row.Cells[6].Text = "";
                    e.Row.Cells[7].Text = "";
                }

                if (e.Row.Cells[14].Text == "7")
                {
                    e.Row.Cells[1].Text = "";
                    e.Row.Cells[3].Text = "";
                    e.Row.Cells[4].Text = "";
                    e.Row.Cells[5].Text = "";
                    e.Row.Cells[6].Text = "";
                    e.Row.Cells[7].Text = "";
                }

                if (e.Row.Cells[14].Text == "8")
                {
                    e.Row.Cells[1].Text = "";
                    e.Row.Cells[2].Text = "";
                    e.Row.Cells[4].Text = "";
                    e.Row.Cells[5].Text = "";
                    e.Row.Cells[6].Text = "";
                    e.Row.Cells[7].Text = "";
                }

                if (e.Row.Cells[14].Text == "9")
                {
                    e.Row.Cells[1].Text = "";
                    e.Row.Cells[2].Text = "";
                    e.Row.Cells[3].Text = "";
                    e.Row.Cells[5].Text = "";
                    e.Row.Cells[6].Text = "";
                    e.Row.Cells[7].Text = "";
                }

                if (e.Row.Cells[14].Text == "10")
                {
                    e.Row.Cells[1].Text = "";
                    e.Row.Cells[2].Text = "";
                    e.Row.Cells[3].Text = "";
                    e.Row.Cells[4].Text = "";
                    e.Row.Cells[6].Text = "";
                    e.Row.Cells[7].Text = "";
                }

                if (e.Row.Cells[14].Text == "11")
                {
                    e.Row.Cells[1].Text = "";
                    e.Row.Cells[2].Text = "";
                    e.Row.Cells[3].Text = "";
                    e.Row.Cells[4].Text = "";
                    e.Row.Cells[5].Text = "";
                    e.Row.Cells[7].Text = "";
                }

                if (e.Row.Cells[14].Text == "12")
                {
                    e.Row.Cells[1].Text = "";
                    e.Row.Cells[2].Text = "";
                    e.Row.Cells[3].Text = "";
                    e.Row.Cells[4].Text = "";
                    e.Row.Cells[5].Text = "";
                    e.Row.Cells[6].Text = "";
                }

            }
        }

        protected void BindAcrossMontsPhysicalProgress(string WorkpackageUID)
        {
            AcrossMonthReportName.InnerHtml = "Report Name : Physical Progress Monitoring Sheet";
            AcrossMonthProjectName.InnerHtml = "Project Name : " + DDlProject.SelectedItem.Text + " (" + DDLWorkPackage.SelectedItem.Text + ")";

            DataSet ds = getdt.GetConstructionProgramme_Tasks(new Guid(WorkpackageUID));
            if (ds.Tables[0].Rows.Count > 0)
            {
                btnAcrossMonthsExportExcel.Visible = true;
                string sHTMLString = "<div style='width:100%; float:left; height:500px; overflow-y:auto;'><table class='table table-bordered'>";
                string DescriptionofWork = "<thead style='position:sticky; top:0; background:LightGray;'><tr><th></th>";
                string sUnit = "<tr><td><b>Unit</td></td>";
                string BillOfQuantity = "<tr><td><b>Bill of Quantities</td></td>";
                string Rev_Scope = "<tr><td><b>Surveyed Quantities</td></td>";
                string ActivityData = "";
                string sActivityData = "";
                string Target = "";
                string Achieved = "";
                string PercentageProgress = "";
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    DescriptionofWork += "<th style='text-align:center;'><b>" + (i + 1) + "." + ds.Tables[0].Rows[i]["Name"].ToString() + "</b></th>";
                    sUnit += "<td style='text-align:center;'>" + ds.Tables[0].Rows[i]["UnitforProgress"].ToString() + "</td>";
                    BillOfQuantity += "<td style='text-align:center;'>" + ds.Tables[0].Rows[i]["UnitQuantity"].ToString() + "</td>";
                    Rev_Scope += "<td style='text-align:center;'>" + ds.Tables[0].Rows[i]["UnitQuantity"].ToString() + "</td>";
                }
                DataSet dsMonths = getdt.GetConstructionProgramme_Months_by_WorkpackageUID(new Guid(WorkpackageUID), DateTime.Now);
                for (int j = 0; j < dsMonths.Tables[0].Rows.Count; j++)
                {
                    ActivityData = "<tr style='background-color:LightGray;'><td><b>" + Convert.ToDateTime(dsMonths.Tables[0].Rows[j]["StartDate"].ToString()).ToString("MMM yyyy") + "</b></td>";
                    Target = "<tr><td>Target</td>";
                    Achieved = "<tr><td>Achieved</td>";
                    PercentageProgress = "<tr><td>% age of Progress</td>";

                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        try
                        {
                            DataSet cdate = getdt.GetConstructionProgramme_MonthData_by_TaskUID(new Guid(ds.Tables[0].Rows[i]["TaskUID"].ToString()), Convert.ToDateTime(dsMonths.Tables[0].Rows[j]["StartDate"].ToString()));
                            if (cdate.Tables[0].Rows.Count > 0)
                            {
                                decimal TargetValue = 0;
                                decimal AchievedValue = 0;
                                decimal ProgressPercentage = 0;

                                TargetValue = cdate.Tables[0].Rows[0]["Schedule_Value"].ToString() != "" ? Convert.ToDecimal(cdate.Tables[0].Rows[0]["Schedule_Value"].ToString()) : 0;
                                AchievedValue = cdate.Tables[0].Rows[0]["Achieved_Value"].ToString() != "" ? Convert.ToDecimal(cdate.Tables[0].Rows[0]["Achieved_Value"].ToString()) : 0;
                                ProgressPercentage = cdate.Tables[0].Rows[0]["ProgressPercentage"].ToString() != "" ? Convert.ToDecimal(cdate.Tables[0].Rows[0]["ProgressPercentage"].ToString()) : 0;
                                ActivityData += "<td></td>";
                                Target += "<td style='text-align:center;'>" + decimal.Round(TargetValue, 2) + "</td>";
                                Achieved += "<td style='text-align:center;'>" + decimal.Round(AchievedValue, 2) + "</td>";
                                PercentageProgress += "<td style='text-align:center;'>" + decimal.Round(ProgressPercentage, 2) + "</td>";
                            }
                            else
                            {
                                ActivityData += "<td></td>";
                                Target += "<td>-</td>";
                                Achieved += "<td>-</td>";
                                PercentageProgress += "<td>-</td>";
                            }
                        }
                        catch (Exception ex)
                        {

                        }
                    }
                    ActivityData += "</tr>";
                    Target += "</tr>";
                    Achieved += "</tr>";
                    PercentageProgress += "</tr>";
                    sActivityData += ActivityData + Target + Achieved + PercentageProgress;
                }

                DescriptionofWork += "</tr></thead>";
                sUnit += "</tr>";
                BillOfQuantity += "</tr>";
                Rev_Scope += "</tr>";
                sHTMLString += DescriptionofWork + sUnit + BillOfQuantity + Rev_Scope + sActivityData + "</table></div>";

                DivAcrossMonths.InnerHtml = sHTMLString;
            }
            else
            {
                DivAcrossMonths.InnerHtml = "No Data Found..";
            }
        }

        protected void DDLActivity_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (DDLActivity.SelectedValue != "")
            {
                ActivityProgressReport.Visible = true;
                //LblActivityPhysicalProgress.Text = "Physical Progress for the Activity : " + DDLActivity.SelectedItem.Text;

                ActivityProgressReportName.InnerHtml = "Report Name : Physical Progress Monitoring for the Activity '" + DDLActivity.SelectedItem.Text + "'";
                ActivityProgressProjectName.InnerHtml = "Project Name : " + DDlProject.SelectedItem.Text + " (" + DDLWorkPackage.SelectedItem.Text + ")";

                decimal PrevCumulativePlan = 0;
                decimal PrevCumulativeActual = 0;
                DataSet ds = getdt.GetTaskSchedule_By_TaskUID(new Guid(DDLActivity.SelectedValue));
                if (ds.Tables[0].Rows.Count > 0)
                {
                    btnActivityProgressPrint.Visible = true;
                    bool ShowTable = false;
                    string tablemonths = "<td style=\"width:200px\">&nbsp;</td>";
                    string tmonthlyplan = "<td style=\"padding:3px\">Monthly Plan</td>";
                    string tmonthlyactual = "<td style=\"padding:3px\">Monthly Actual</td>";
                    string tcumulativeplan = "<td style=\"padding:3px\">Cumulative Plan</td>";
                    string tcumulativeactual = "<td style=\"padding:3px\">Cumulative Actual</td>";

                    System.Text.StringBuilder strScript = new System.Text.StringBuilder();
                    strScript.Append(@"<script type='text/javascript'>
                    google.charts.load('current', { 'packages':['corechart']
                    });
                      if (typeof google.charts.visualization == 'undefined') {
                        google.charts.setOnLoadCallback(drawVisualization);
                    }
                    else {
                        drawVisualization();
                    }
                    function drawVisualization()
                    {
                        // Some raw data (not necessarily accurate)
                        var data = google.visualization.arrayToDataTable([
                          ['Month', 'Plan', 'Actual', 'Cumulative Plan', 'Cumulative Actual'],");
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        if (Convert.ToDateTime(ds.Tables[0].Rows[i]["EndDate"].ToString()) <= DateTime.Now)
                        {
                            ShowTable = true;
                            string Plan = ds.Tables[0].Rows[i]["Schedule_Value"].ToString() != "" ? ds.Tables[0].Rows[i]["Schedule_Value"].ToString() : "0";
                            string Actual = ds.Tables[0].Rows[i]["Achieved_Value"].ToString() != "" ? ds.Tables[0].Rows[i]["Achieved_Value"].ToString() : "0";
                            if (Plan != "" && Actual != "")
                            {
                                if (i == 0)
                                {
                                    PrevCumulativePlan = Convert.ToDecimal(Plan);
                                    PrevCumulativeActual = Convert.ToDecimal(Actual);
                                    strScript.Append("['" + Convert.ToDateTime(ds.Tables[0].Rows[i]["EndDate"].ToString()).ToString("MMM-yy") + "'," + Plan + "," + Actual + "," + Plan + "," + Actual + "],");
                                }
                                else
                                {
                                    strScript.Append("['" + Convert.ToDateTime(ds.Tables[0].Rows[i]["EndDate"].ToString()).ToString("MMM-yy") + "'," + Plan + "," + Actual + "," + (Convert.ToDecimal(Plan) + PrevCumulativePlan) + "," + (Convert.ToDecimal(Actual) + PrevCumulativeActual) + "],");
                                    //e.Row.Cells[3].Text = (Convert.ToDouble(TargetValue) + PreviousTarget).ToString();
                                    PrevCumulativePlan = (Convert.ToDecimal(Plan) + PrevCumulativePlan);
                                    //e.Row.Cells[4].Text = (Convert.ToDouble(AchievedValue) + PreviousActual).ToString();
                                    PrevCumulativeActual = (Convert.ToDecimal(Actual) + PrevCumulativeActual);
                                }

                                tablemonths += "<td style=\"padding:3px\">" + Convert.ToDateTime(ds.Tables[0].Rows[i]["EndDate"].ToString()).ToString("MMM-yy") + "</td>";
                                tmonthlyplan += "<td style=\"padding:3px\">" + decimal.Round(Convert.ToDecimal(Plan), 2) + "</td>";
                                tmonthlyactual += "<td style=\"padding:3px\">" + decimal.Round(Convert.ToDecimal(Actual), 2) + "</td>";

                                tcumulativeplan += "<td style=\"padding:3px\">" + decimal.Round(PrevCumulativePlan, 2) + "</td>";
                                tcumulativeactual += "<td style=\"padding:3px\">" + decimal.Round(PrevCumulativeActual, 2) + "</td>";
                            }


                        }
                    }
                    strScript.Remove(strScript.Length - 1, 1);
                    strScript.Append("]);");
                    strScript.Append(@"var options = {
                        legend: { position: 'top' },
                        seriesType: 'bars',
                        series: { 2: { type: 'line',targetAxisIndex: 1 },3: { type: 'line',targetAxisIndex: 1 } },
                        hAxis: { title: 'Month',titleTextStyle: {
                        bold:'true',
                      }},
                      vAxes: {                        
          
                        0: {title: 'Monthly Plan',titleTextStyle: {
                    bold:'true',
                  }},
                        1: {title: 'Cumulative Plan',titleTextStyle: {
                    bold:'true',
                  }}
                      }
                };

                var chart = new google.visualization.ComboChart(document.getElementById('chart_div'));
                chart.draw(data, options);
                  }
                </script>");
                    if (ShowTable)
                    {
                        ltScript_Progress.Text = strScript.ToString();

                        DivActivityProgressTabular.InnerHtml = "<table border=\"1\" style=\"text-align:center;font-size:12px; width:100%; color:black; padding-left:10px;\">" +
                                     "<tr> " + tablemonths + "</tr>" +
                                      "<tr> " + tmonthlyplan + "</tr>" +
                                       "<tr> " + tmonthlyactual + "</tr>" +
                                        "<tr> " + tcumulativeplan + "</tr>" +
                                         "<tr> " + tcumulativeactual + "</tr>" +
                                             "</table>";
                    }
                    else
                    {
                        ltScript_Progress.Text = "<h3>No data</h3>";
                        DivActivityProgressTabular.InnerHtml = "<h3>No data</h3>";
                    }

                }
                else
                {
                    ltScript_Progress.Text = "<h3>No data</h3>";
                }
            }
        }

        protected void btnExportReportPDF_Click(object sender, EventArgs e)
        {
            ViewState["Export"] = "1";
            BindWeeklyProgressReport();
            ExporttoPDF(GrdWeeklyprogress, 2, "No");
        }

        protected void btnPrintPDF_Click(object sender, EventArgs e)
        {
            ViewState["Export"] = "2";
            BindWeeklyProgressReport();
            ExporttoPDF(GrdWeeklyprogress, 2, "Yes");
        }

        protected void btnFortExportReportPDF_Click(object sender, EventArgs e)
        {
            ViewState["Export"] = "2";
            BindWeeklyProgressReport();
            ExporttoPDF(grdFortiProgressReport, 2, "No");
        }

        protected void btnFortExportReportPrint_Click(object sender, EventArgs e)
        {
            ViewState["Export"] = "2";
            BindWeeklyProgressReport();
            ExporttoPDF(grdFortiProgressReport, 2, "Yes");
        }
        private void ExporttoPDF(GridView gd, int type, string isPrint)
        {
            try
            {

                DateTime CDate1 = DateTime.Now;
                GridView gdRp = new GridView();
                gdRp = gd;

                int noOfColumns = 0, noOfRows = 0;
                DataTable tbl = null;

                if (RBLReportFor.SelectedValue == "By Week" || RBLReportFor.SelectedValue == "By Fortnightly")
                {
                    gdRp.Columns[0].HeaderText = "Sl.No";
                    gdRp.Columns[1].HeaderText = "Description of Work";
                    gdRp.Columns[2].HeaderText = "Description of Work";
                    gdRp.Columns[3].HeaderText = "Description of Work";
                    gdRp.Columns[4].HeaderText = "Description of Work";
                    gdRp.Columns[5].HeaderText = "Description of Work";
                    gdRp.Columns[6].HeaderText = "Description of Work";
                    gdRp.Columns[7].HeaderText = "Description of Work";
                    gdRp.Columns[8].HeaderText = "UOM";
                    gdRp.Columns[9].HeaderText = "Cumulative Target as on " + DDLWeek.SelectedValue.Split('-')[1].Trim();
                    gdRp.Columns[10].HeaderText = "Target for the Week";
                    gdRp.Columns[11].HeaderText = "Cumulative Achieved as on " + DDLWeek.SelectedValue.Split('-')[1].Trim();
                    gdRp.Columns[12].HeaderText = "Achieved for the Week";
                    gdRp.Columns[13].HeaderText = "% of Progress Cumulative";
                }
                else if (RBLReportFor.SelectedValue == "By Month")
                {
                    int index = 0;
                    if (DDLMonth.SelectedIndex != 0)
                    {
                        index = (DDLMonth.SelectedIndex) - 1;
                    }

                    gdRp.Columns[0].HeaderText = "S.No";
                    gdRp.Columns[1].HeaderText = "Description of Work";
                    gdRp.Columns[2].HeaderText = "Description of Work";
                    gdRp.Columns[3].HeaderText = "Description of Work";
                    gdRp.Columns[4].HeaderText = "Description of Work";
                    gdRp.Columns[5].HeaderText = "Description of Work";
                    gdRp.Columns[6].HeaderText = "Description of Work";
                    gdRp.Columns[7].HeaderText = "Description of Work";
                    gdRp.Columns[8].HeaderText = "Description of Work";
                    gdRp.Columns[9].HeaderText = "UOM";
                    gdRp.Columns[10].HeaderText = "Scope as per BOQ";
                    gdRp.Columns[11].HeaderText = "Achieved up to " + DDLMonth.Items[index].Text + "_" + DDLYear.SelectedValue;
                    gdRp.Columns[12].HeaderText = "Planned for " + DDLMonth.SelectedItem.Text + "_" + DDLYear.SelectedValue + "";
                    gdRp.Columns[13].HeaderText = "Achieved for " + DDLMonth.SelectedItem.Text + "_" + DDLYear.SelectedValue + "";
                    gdRp.Columns[14].HeaderText = "Achieved % " + DDLMonth.SelectedItem.Text + "_" + DDLYear.SelectedValue + "";

                    gdRp.Columns[15].HeaderText = "Cum. Planned";
                    gdRp.Columns[16].HeaderText = "Cum. Achieved";
                    gdRp.Columns[17].HeaderText = "Balance";
                    gdRp.Columns[18].HeaderText = "Plan for the Next Month";
                    gdRp.Columns[19].HeaderText = "Overall % of Completion";
                }
                else if (RBLReportFor.SelectedValue == "By Quarter")
                {
                    int index = 0;
                    if (ddlQuarter.SelectedIndex != 0)
                    {
                        index = (ddlQuarter.SelectedIndex) - 1;
                    }

                    gdRp.Columns[0].HeaderText = "S.No";
                    gdRp.Columns[1].HeaderText = "Description of Work";
                    gdRp.Columns[2].HeaderText = "UOM";
                    gdRp.Columns[3].HeaderText = "Scope as per BOQ";
                    gdRp.Columns[4].HeaderText = "Achieved up to " + ddlQuarter.SelectedValue.ToString().Split('-')[1];
                    gdRp.Columns[5].HeaderText = "Planned for " + ddlQuarter.SelectedValue.ToString().Split('-')[1];
                    gdRp.Columns[6].HeaderText = "Achieved for " + ddlQuarter.SelectedValue.ToString().Split('-')[1];
                    gdRp.Columns[7].HeaderText = "Achieved % " + ddlQuarter.SelectedValue.ToString().Split('-')[1];

                    gdRp.Columns[8].HeaderText = "Cum. Planned";
                    gdRp.Columns[9].HeaderText = "Cum. Achieved";
                    gdRp.Columns[10].HeaderText = "Balance";
                    gdRp.Columns[11].HeaderText = "Plan for the Next Month";
                    gdRp.Columns[12].HeaderText = "Overall % of Completion";
                }
                else if (RBLReportFor.SelectedValue == "By HalfYear")
                {
                    //int index = 0;
                    //if (ddlQuarter.SelectedIndex != 0)
                    //{
                    //    index = (ddlQuarter.SelectedIndex) - 1;
                    //}

                    gdRp.Columns[0].HeaderText = "S.No";
                    gdRp.Columns[1].HeaderText = "Description of Work";
                    gdRp.Columns[2].HeaderText = "UOM";
                    gdRp.Columns[3].HeaderText = "Scope as per BOQ";
                    gdRp.Columns[4].HeaderText = "Achieved up to " + ddlHalfyearperiod.SelectedValue.ToString().Split('-')[1];
                    gdRp.Columns[5].HeaderText = "Planned for " + ddlHalfyearperiod.SelectedValue.ToString().Split('-')[1];
                    gdRp.Columns[6].HeaderText = "Achieved for " + ddlHalfyearperiod.SelectedValue.ToString().Split('-')[1];
                    gdRp.Columns[7].HeaderText = "Achieved % " + ddlHalfyearperiod.SelectedValue.ToString().Split('-')[1];

                    gdRp.Columns[8].HeaderText = "Cum. Planned";
                    gdRp.Columns[9].HeaderText = "Cum. Achieved";
                    gdRp.Columns[10].HeaderText = "Balance";
                    gdRp.Columns[11].HeaderText = "Plan for the Next Month";
                    gdRp.Columns[12].HeaderText = "Overall % of Completion";
                }
                if (gdRp.AutoGenerateColumns)
                {
                    tbl = gdRp.DataSource as DataTable; // Gets the DataSource of the GridView Control.
                    noOfColumns = tbl.Columns.Count;
                    noOfRows = tbl.Rows.Count;
                }
                else
                {
                    noOfColumns = gdRp.Columns.Count;
                    noOfRows = gdRp.Rows.Count;
                }

                float HeaderTextSize = 9;
                float ReportNameSize = 9;
                float ReportTextSize = 9;
                float ApplicationNameSize = 13;
                string ProjectName = DDlProject.SelectedItem.ToString();

                //if (ProjectName.Length > 30)
                //{
                //    ProjectName = ProjectName.Substring(0, 29) + "..";
                //}

                // Creates a PDF document

                Document document = null;
                //if (LandScape == true)
                //{
                // Sets the document to A4 size and rotates it so that the     orientation of the page is Landscape.
                document = new Document(PageSize.A4.Rotate(), 0, 0, 0, 0);

                //}
                //else
                //{
                //    document = new Document(PageSize.A4, 0, 0, 15, 5);
                //}

                // Creates a PdfPTable with column count of the table equal to no of columns of the gridview or gridview datasource.
                iTextSharp.text.pdf.PdfPTable mainTable = new iTextSharp.text.pdf.PdfPTable(noOfColumns);

                // Sets the first 4 rows of the table as the header rows which will be repeated in all the pages.
                mainTable.HeaderRows = 4;

                // Creates a PdfPTable with 2 columns to hold the header in the exported PDF.
                iTextSharp.text.pdf.PdfPTable headerTable = new iTextSharp.text.pdf.PdfPTable(1);

                // Creates a phrase to hold the application name at the left hand side of the header.
                Phrase phApplicationName = new Phrase();
                string ExportFileName = "";
                if (RBLReportFor.SelectedValue == "By Week")
                {
                    phApplicationName = new Phrase(Environment.NewLine + WebConfigurationManager.AppSettings["Domain"] + " Report Name: Weekly work Progress Status as on  " + DDLWeek.SelectedValue.Split('-')[1].Trim(), FontFactory.GetFont("Arial", ApplicationNameSize, iTextSharp.text.Font.BOLD));
                    ExportFileName = "Report_Weekly_work_Progress_Status_" + DateTime.Now.Ticks + ".pdf";

                    mainTable.SetWidths(new float[] { 6, 32, 12, 10, 10, 10, 10, 10 });
                }
                else if (RBLReportFor.SelectedValue == "By Fortnightly")
                {
                    phApplicationName = new Phrase(Environment.NewLine + WebConfigurationManager.AppSettings["Domain"] + " Report Name: Fortnight work Progress Status as on  " + DDLFortWeek.SelectedValue.Split('-')[1].Trim(), FontFactory.GetFont("Arial", ApplicationNameSize, iTextSharp.text.Font.BOLD));
                    ExportFileName = "Report_Weekly_work_Progress_Status_" + DateTime.Now.Ticks + ".pdf";

                    mainTable.SetWidths(new float[] { 6, 32, 12, 10, 10, 10, 10, 10 });
                }
                else if (RBLReportFor.SelectedValue == "By Month")
                {
                    phApplicationName = new Phrase(Environment.NewLine + WebConfigurationManager.AppSettings["Domain"] + " Report Name: Physical Progress Achievements in the month " + DDLMonth.SelectedItem.Text + " " + DDLYear.SelectedValue, FontFactory.GetFont("Arial", ApplicationNameSize, iTextSharp.text.Font.BOLD));
                    ExportFileName = "Report_Physical_Progress_Achievements_in_the_month_" + DDLMonth.SelectedItem.Text + "-" + DDLYear.SelectedValue + "_" + DateTime.Now.Ticks + ".pdf";
                    mainTable.SetWidths(new float[] { 4, 12, 12, 12, 12, 12, 12, 12, 12, 10, 8, 8, 7, 7, 7, 7, 7, 6, 8, 8 });
                }
                else if (RBLReportFor.SelectedValue == "By Quarter")
                {
                    phApplicationName = new Phrase(Environment.NewLine + WebConfigurationManager.AppSettings["Domain"] + " Report Name: Physical Progress Achievements in the Quarter " + ddlQuarter.SelectedItem.Text, FontFactory.GetFont("Arial", ApplicationNameSize, iTextSharp.text.Font.BOLD));
                    ExportFileName = "Report_Physical_Progress_Achievements_in_the_month_" + ddlQuarter.SelectedItem.Text + "_" + DateTime.Now.Ticks + ".pdf";
                    mainTable.SetWidths(new float[] { 4, 12, 10, 8, 8, 7, 7, 7, 7, 7, 6, 8, 8 });
                }
                else if (RBLReportFor.SelectedValue == "By HalfYear")
                {
                    phApplicationName = new Phrase(Environment.NewLine + WebConfigurationManager.AppSettings["Domain"] + " Report Name: Physical Progress Achievements in the Half Year " + ddlHalfyearperiod.SelectedItem.Text, FontFactory.GetFont("Arial", ApplicationNameSize, iTextSharp.text.Font.BOLD));
                    ExportFileName = "Report_Physical_Progress_Achievements_in_the_halfyear_" + ddlHalfyearperiod.SelectedItem.Text + "_" + DateTime.Now.Ticks + ".pdf";
                    mainTable.SetWidths(new float[] { 4, 12, 10, 8, 8, 7, 7, 7, 7, 7, 6, 8, 8 });
                }
                else if (RBLReportFor.SelectedValue == "Across Months")
                {
                    phApplicationName = new Phrase(Environment.NewLine + WebConfigurationManager.AppSettings["Domain"] + " Report Name: Physical Progress Monitoring Sheet", FontFactory.GetFont("Arial", ApplicationNameSize, iTextSharp.text.Font.BOLD));
                    ExportFileName = "Report_Physical_Progress_Monitoring_Sheet_" + DateTime.Now.Ticks + ".pdf";
                }
                else
                {
                    phApplicationName = new Phrase(Environment.NewLine + WebConfigurationManager.AppSettings["Domain"] + " Report Name: Physical Progress Monitoring for the Activity " + DDLActivity.SelectedItem.Text, FontFactory.GetFont("Arial", ApplicationNameSize, iTextSharp.text.Font.BOLD));
                    ExportFileName = "Report_Physical_Progress_Monitoring_for_the_Activity_" + DDLActivity.SelectedItem.Text + "_" + DateTime.Now.Ticks + ".pdf";
                }


                // Creates a PdfPCell which accepts a phrase as a parameter.
                PdfPCell clApplicationName = new PdfPCell(phApplicationName);
                // Sets the border of the cell to zero.
                clApplicationName.Border = PdfPCell.NO_BORDER;
                // Sets the Horizontal Alignment of the PdfPCell to left.
                clApplicationName.HorizontalAlignment = Element.ALIGN_CENTER;

                // Creates a phrase to show the current date at the right hand side of the header.
                //Phrase phDate = new Phrase(DateTime.Now.Date.ToString("dd/MM/yyyy"), FontFactory.GetFont("Arial", ApplicationNameSize, iTextSharp.text.Font.NORMAL));

                //// Creates a PdfPCell which accepts the date phrase as a parameter.
                //PdfPCell clDate = new PdfPCell(phDate);
                //// Sets the Horizontal Alignment of the PdfPCell to right.
                //clDate.HorizontalAlignment = Element.ALIGN_RIGHT;
                //// Sets the border of the cell to zero.
                //clDate.Border = PdfPCell.NO_BORDER;

                // Adds the cell which holds the application name to the headerTable.
                headerTable.AddCell(clApplicationName);
                // Adds the cell which holds the date to the headerTable.
                //  headerTable.AddCell(clDate);
                // Sets the border of the headerTable to zero.
                headerTable.DefaultCell.Border = PdfPCell.NO_BORDER;

                // Creates a PdfPCell that accepts the headerTable as a parameter and then adds that cell to the main PdfPTable.
                PdfPCell cellHeader = new PdfPCell(headerTable);
                cellHeader.Border = PdfPCell.NO_BORDER;
                // Sets the column span of the header cell to noOfColumns.
                cellHeader.Colspan = noOfColumns;
                // Adds the above header cell to the table.
                mainTable.AddCell(cellHeader);

                // Creates a phrase which holds the file name.
                Phrase phHeader = new Phrase("Project Name : " + ProjectName + " (" + DDLWorkPackage.SelectedItem.Text + ")");
                PdfPCell clHeader = new PdfPCell(phHeader);
                clHeader.Colspan = noOfColumns;
                clHeader.Border = PdfPCell.NO_BORDER;
                clHeader.HorizontalAlignment = Element.ALIGN_CENTER;
                mainTable.AddCell(clHeader);



                // Creates a phrase for a new line.
                Phrase phSpace = new Phrase("\n");
                PdfPCell clSpace = new PdfPCell(phSpace);
                clSpace.Border = PdfPCell.NO_BORDER;
                clSpace.Colspan = noOfColumns;
                mainTable.AddCell(clSpace);

                // Sets the gridview column names as table headers.
                for (int i = 0; i < noOfColumns; i++)
                {
                    Phrase ph = null;

                    if (gdRp.AutoGenerateColumns)
                    {
                        ph = new Phrase(tbl.Columns[i].ColumnName.Replace("<br/>", Environment.NewLine), FontFactory.GetFont("Arial", HeaderTextSize, iTextSharp.text.Font.BOLD));
                    }
                    else
                    {
                        ph = new Phrase(gdRp.Columns[i].HeaderText.Replace("<br/>", Environment.NewLine), FontFactory.GetFont("Arial", HeaderTextSize, iTextSharp.text.Font.BOLD));
                    }
                    PdfPCell cl = new PdfPCell(ph);
                    if (i == 1)
                    {
                        cl.HorizontalAlignment = Element.ALIGN_LEFT;
                        mainTable.AddCell(cl);
                    }
                    else
                    {
                        cl.HorizontalAlignment = Element.ALIGN_CENTER;
                        mainTable.AddCell(cl);
                    }

                }

                // Reads the gridview rows and adds them to the mainTable
                for (int rowNo = 0; rowNo <= noOfRows; rowNo++)
                {
                    if (rowNo != noOfRows)
                    {
                        for (int columnNo = 0; columnNo < noOfColumns; columnNo++)
                        {
                            if (gdRp.AutoGenerateColumns)
                            {
                                string s = gdRp.Rows[rowNo].Cells[columnNo].Text.Trim();
                                Phrase ph = new Phrase(s, FontFactory.GetFont("Arial", ReportTextSize, iTextSharp.text.Font.NORMAL));
                                PdfPCell cl = new PdfPCell(ph);
                                cl.HorizontalAlignment = Element.ALIGN_CENTER;
                                mainTable.AddCell(cl);
                            }
                            else
                            {
                                if (columnNo == 0)
                                {
                                    if (gdRp.Columns[columnNo] is TemplateField)
                                    {
                                        DataBoundLiteralControl lc = gdRp.Rows[rowNo].Cells[columnNo].Controls[0] as DataBoundLiteralControl;
                                        string s = lc.Text.Trim();
                                        Phrase ph = new Phrase(s, FontFactory.GetFont("Arial", ReportTextSize, iTextSharp.text.Font.NORMAL));
                                        PdfPCell cl = new PdfPCell(ph);
                                        cl.HorizontalAlignment = Element.ALIGN_CENTER;
                                        mainTable.AddCell(cl);
                                    }
                                    else
                                    {
                                        string s = gdRp.Rows[rowNo].Cells[columnNo].Text.Trim();
                                        s = s.Replace("&nbsp;", "");
                                        Phrase ph = new Phrase(s, FontFactory.GetFont("Arial", ReportTextSize, iTextSharp.text.Font.NORMAL));
                                        PdfPCell cl = new PdfPCell(ph);
                                        cl.HorizontalAlignment = Element.ALIGN_CENTER;
                                        mainTable.AddCell(cl);
                                    }
                                }
                                else if (columnNo == 1)
                                {
                                    if (gdRp.Columns[columnNo] is TemplateField)
                                    {
                                        DataBoundLiteralControl lc = gdRp.Rows[rowNo].Cells[columnNo].Controls[0] as DataBoundLiteralControl;
                                        string s = lc.Text.Trim();
                                        Phrase ph = new Phrase(s, FontFactory.GetFont("Arial", ReportTextSize, iTextSharp.text.Font.NORMAL));
                                        PdfPCell cl = new PdfPCell(ph);
                                        cl.HorizontalAlignment = Element.ALIGN_LEFT;
                                        mainTable.AddCell(cl);
                                    }
                                    else
                                    {
                                        string s = gdRp.Rows[rowNo].Cells[columnNo].Text.Trim();
                                        s = s.Replace("&nbsp;", "");
                                        Phrase ph = new Phrase(s, FontFactory.GetFont("Arial", ReportTextSize, iTextSharp.text.Font.NORMAL));
                                        PdfPCell cl = new PdfPCell(ph);
                                        cl.HorizontalAlignment = Element.ALIGN_LEFT;
                                        mainTable.AddCell(cl);
                                    }
                                }
                                else if (columnNo == 2 || columnNo == 3 || columnNo == 4 || columnNo == 5 || columnNo == 6 || columnNo == 7 || columnNo == 8)
                                {
                                    if (gdRp.Columns[columnNo] is TemplateField)
                                    {
                                        if (gdRp.Rows[rowNo].Cells[columnNo].Controls[0] != null)
                                        {
                                            DataBoundLiteralControl lc = gdRp.Rows[rowNo].Cells[columnNo].Controls[0] as DataBoundLiteralControl;
                                            string s = lc.Text.Trim();
                                            Phrase ph = new Phrase(s, FontFactory.GetFont("Arial", ReportTextSize, iTextSharp.text.Font.NORMAL));
                                            PdfPCell cl = new PdfPCell(ph);
                                            cl.HorizontalAlignment = Element.ALIGN_CENTER;
                                            mainTable.AddCell(cl);
                                        }
                                    }
                                    else
                                    {
                                        string s = gdRp.Rows[rowNo].Cells[columnNo].Text.Trim();
                                        s = s.Replace("&nbsp;", "");
                                        Phrase ph = new Phrase(s, FontFactory.GetFont("Arial", ReportTextSize, iTextSharp.text.Font.NORMAL));
                                        PdfPCell cl = new PdfPCell(ph);
                                        cl.HorizontalAlignment = Element.ALIGN_CENTER;
                                        mainTable.AddCell(cl);
                                    }
                                }
                                else if (columnNo == 9 || columnNo == 10 || columnNo == 11 || columnNo == 12 || columnNo == 13)
                                {
                                    if (gdRp.Columns[columnNo] is TemplateField)
                                    {
                                        DataBoundLiteralControl lc = gdRp.Rows[rowNo].Cells[columnNo].Controls[0] as DataBoundLiteralControl;
                                        string s = lc.Text.Trim();
                                        Phrase ph = new Phrase(s, FontFactory.GetFont("Arial", ReportTextSize, iTextSharp.text.Font.NORMAL));
                                        PdfPCell cl = new PdfPCell(ph);
                                        cl.HorizontalAlignment = Element.ALIGN_RIGHT;
                                        mainTable.AddCell(cl);
                                    }
                                    else
                                    {
                                        string s = gdRp.Rows[rowNo].Cells[columnNo].Text.Trim();
                                        s = s.Replace("&nbsp;", "");
                                        Phrase ph = new Phrase(s, FontFactory.GetFont("Arial", ReportTextSize, iTextSharp.text.Font.NORMAL));
                                        PdfPCell cl = new PdfPCell(ph);
                                        cl.HorizontalAlignment = Element.ALIGN_RIGHT;
                                        mainTable.AddCell(cl);
                                    }
                                }
                                else
                                {
                                    if (gdRp.Columns[columnNo] is TemplateField)
                                    {
                                        DataBoundLiteralControl lc = gdRp.Rows[rowNo].Cells[columnNo].Controls[0] as DataBoundLiteralControl;
                                        string s = lc.Text.Trim();
                                        Phrase ph = new Phrase(s, FontFactory.GetFont("Arial", ReportTextSize, iTextSharp.text.Font.NORMAL));
                                        PdfPCell cl = new PdfPCell(ph);
                                        cl.HorizontalAlignment = Element.ALIGN_RIGHT;
                                        mainTable.AddCell(cl);
                                    }
                                    else
                                    {
                                        string s = gdRp.Rows[rowNo].Cells[columnNo].Text.Trim();
                                        s = s.Replace("&nbsp;", "");
                                        Phrase ph = new Phrase(s, FontFactory.GetFont("Arial", ReportTextSize, iTextSharp.text.Font.NORMAL));
                                        PdfPCell cl = new PdfPCell(ph);
                                        cl.HorizontalAlignment = Element.ALIGN_RIGHT;
                                        mainTable.AddCell(cl);
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        if (type == 1 || type == 6)
                        {
                            for (int columnNo = 0; columnNo < noOfColumns; columnNo++)
                            {
                                string s = "Grand Total";
                                if (columnNo == 1)
                                {
                                    Phrase ph = new Phrase(s, FontFactory.GetFont("Arial", ReportTextSize, iTextSharp.text.Font.BOLD));
                                    PdfPCell cl = new PdfPCell(ph);
                                    cl.HorizontalAlignment = Element.ALIGN_CENTER;
                                    mainTable.AddCell(cl);
                                }
                                else if (columnNo == 2)
                                {
                                    s = ViewState["grandtotalcount"].ToString();// Session["AwardedValue"].ToString();
                                    Phrase ph = new Phrase(s, FontFactory.GetFont("Arial", ReportTextSize, iTextSharp.text.Font.BOLD));
                                    PdfPCell cl = new PdfPCell(ph);
                                    cl.HorizontalAlignment = Element.ALIGN_CENTER;
                                    mainTable.AddCell(cl);
                                }
                                else if (columnNo == 3)
                                {
                                    s = ViewState["grandtotaldwnldcount"].ToString();// Session["ExpenditureOverallTotal"].ToString();
                                    Phrase ph = new Phrase(s, FontFactory.GetFont("Arial", ReportTextSize, iTextSharp.text.Font.BOLD));
                                    PdfPCell cl = new PdfPCell(ph);
                                    cl.HorizontalAlignment = Element.ALIGN_CENTER;
                                    mainTable.AddCell(cl);
                                }
                                else if (columnNo == 4)
                                {
                                    s = ViewState["grandtotalviewcount"].ToString();// Session["TargetedOverallTotal"].ToString() + "%";
                                    Phrase ph = new Phrase(s, FontFactory.GetFont("Arial", ReportTextSize, iTextSharp.text.Font.BOLD));
                                    PdfPCell cl = new PdfPCell(ph);
                                    cl.HorizontalAlignment = Element.ALIGN_CENTER;
                                    mainTable.AddCell(cl);
                                }
                                else if (columnNo == 5)
                                {
                                    s = ViewState["GrandTotalDocumentLinkSent"].ToString();// Session["AchievedOverallTotal"].ToString() + "%";
                                    Phrase ph = new Phrase(s, FontFactory.GetFont("Arial", ReportTextSize, iTextSharp.text.Font.BOLD));
                                    PdfPCell cl = new PdfPCell(ph);
                                    cl.HorizontalAlignment = Element.ALIGN_CENTER;
                                    mainTable.AddCell(cl);
                                }
                                else
                                {
                                    Phrase ph = new Phrase("", FontFactory.GetFont("Arial", ReportTextSize, iTextSharp.text.Font.BOLD));
                                    PdfPCell cl = new PdfPCell(ph);
                                    cl.HorizontalAlignment = Element.ALIGN_CENTER;
                                    mainTable.AddCell(cl);
                                }
                            }
                        }

                    }

                    // Tells the mainTable to complete the row even if any cell is left incomplete.
                    mainTable.CompleteRow();
                }

                // Gets the instance of the document created and writes it to the output stream of the Response object.
                if (isPrint == "Yes")
                {
                    PdfWriter.GetInstance(document, new FileStream(Server.MapPath("~") + "/RegExcel/printpdf.pdf", FileMode.Create));
                }
                else
                {
                    PdfWriter.GetInstance(document, Response.OutputStream);
                }
                //PdfWriter.GetInstance(document, new FileStream(Server.MapPath("~") + "mypdf.pdf", FileMode.Create));
                // Creates a footer for the PDF document.
                int len = 174;
                System.Text.StringBuilder time = new System.Text.StringBuilder();
                time.Append(DateTime.Now.ToString("hh:mm tt"));
                time.Append("".PadLeft(len, ' ').Replace(" ", " "));

                iTextSharp.text.Font foot = new iTextSharp.text.Font();
                foot.Size = 10;
                HeaderFooter pdfFooter = new HeaderFooter(new Phrase("Date : " + DateTime.Now.ToString("dd/MM/yyyy") + " Time " + time + "  Page: ", foot), new Phrase(Environment.NewLine + " ", foot));
                pdfFooter.Alignment = Element.ALIGN_CENTER;
                document.Footer = pdfFooter;
                document.Open();
                // Adds the mainTable to the document.
                document.Add(mainTable);
                // Closes the document.
                document.Close();
                if (isPrint == "Yes")
                {
                    Session["Print"] = true;
                    Response.Write("<script>window.open ('/PDFPRint.aspx','_blank');</script>");
                }
                else
                {
                    Response.ContentType = "application/pdf";
                    Response.AddHeader("content-disposition", "attachment;filename=" + ExportFileName + "");
                    Response.End();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected void btnMonthlyProgressExportPDF_Click(object sender, EventArgs e)
        {
            ViewState["Export"] = "1";
            BindMonthlyProjectProgressReport();
            ExporttoPDF(GrdMonthPhysicalProgress, 2, "No");
        }

        protected void btnMonthlyProgressPrint_Click(object sender, EventArgs e)
        {
            ViewState["Export"] = "2";
            BindMonthlyProjectProgressReport();
            ExporttoPDF(GrdMonthPhysicalProgress, 2, "Yes");
        }

        public override void VerifyRenderingInServerForm(Control control)
        {
            //required to avoid the runtime error "  
            //Control 'GridView1' of type 'GridView' must be placed inside a form tag with runat=server."  
        }

        protected void btnAcrossMonthsExportExcel_Click(object sender, EventArgs e)
        {
            try
            {
                StringBuilder StrExport = new StringBuilder();

                StringWriter stw = new StringWriter();
                HtmlTextWriter htextw = new HtmlTextWriter(stw);
                htextw.AddStyleAttribute("font-size", "9pt");
                htextw.AddStyleAttribute("color", "Black");



                DivAcrossMonths.RenderControl(htextw); //Name of the Panel

                //var sb1 = new StringBuilder();
                //GridDiv.RenderControl(new HtmlTextWriter(new StringWriter(sb1)));

                string s = htextw.InnerWriter.ToString();

                string HTMLstring = "<html><body>" +
                    "<div style='width:100%; margin:auto;'><div style='width:100%; float:left; line-height:25px; font-size:12pt;' align='center'>" +
                    "<asp:Label ID='Lbl1' runat='server' Font-Bold='true'>" + WebConfigurationManager.AppSettings["Domain"] + " Report Name: Physical Progress Monitoring Sheet</asp:Label><br />" +
                    "<asp:Label ID='Lbl4' runat='server'>Project Name : " + DDlProject.SelectedItem.Text + " (" + DDLWorkPackage.SelectedItem.Text + ")</asp:Label><br />" +
                    "</div> <div style='width:100%; float:left; height:10px;'>&nbsp;&nbsp;&nbsp;</div>" +
                    "<div style='width:100%;font-size:11pt float:left;' align='left'>" +
                    s +
                    "</div>" +
                    "</div></body></html>";

                string strFile = "Report_Report_Physical_Progress_Monitoring_Sheet_" + DateTime.Now.Ticks + ".xls";
                string strcontentType = "application/excel";
                Response.ClearContent();
                Response.ClearHeaders();
                Response.BufferOutput = true;
                Response.ContentType = strcontentType;
                Response.AddHeader("Content-Disposition", "attachment; filename=" + strFile);
                Response.Write(HTMLstring);
                Response.Flush();
                Response.Close();
                Response.End();

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected void btnExportReportExcel_Click(object sender, EventArgs e)
        {
            try
            {
                ViewState["Export"] = "2";
                BindWeeklyProgressReport();

                StringBuilder StrExport = new StringBuilder();

                StringWriter stw = new StringWriter();
                HtmlTextWriter htextw = new HtmlTextWriter(stw);
                htextw.AddStyleAttribute("font-size", "9pt");
                htextw.AddStyleAttribute("color", "Black");



                GrdWeeklyprogress.RenderControl(htextw); //Name of the Panel

                //var sb1 = new StringBuilder();
                //GridDiv.RenderControl(new HtmlTextWriter(new StringWriter(sb1)));

                string s = htextw.InnerWriter.ToString();

                string HTMLstring = "<html><body>" +
                    "<div style='width:100%; margin:auto;'><div style='width:100%; float:left; line-height:25px; font-size:12pt;' align='center'>" +
                    "<asp:Label ID='Lbl1' runat='server' Font-Bold='true'>" + WebConfigurationManager.AppSettings["Domain"] + " Report Name: Weekly work Progress Status as on  " + DDLWeek.SelectedValue.Split('-')[1].Trim() + "</asp:Label><br />" +
                    "<asp:Label ID='Lbl4' runat='server'>Project Name : " + DDlProject.SelectedItem.Text + " (" + DDLWorkPackage.SelectedItem.Text + ")</asp:Label><br />" +
                    "</div> <div style='width:100%; float:left; height:10px;'>&nbsp;&nbsp;&nbsp;</div>" +
                    "<div style='width:100%;font-size:11pt float:left;' align='left'>" +
                    s +
                    "</div>" +
                    "</div></body></html>";

                string strFile = "Report_Weekly_work_Progress_Status_" + DateTime.Now.Ticks + ".xls";
                string strcontentType = "application/excel";
                Response.ClearContent();
                Response.ClearHeaders();
                Response.BufferOutput = true;
                Response.ContentType = strcontentType;
                Response.AddHeader("Content-Disposition", "attachment; filename=" + strFile);
                Response.Write(HTMLstring);
                Response.Flush();
                Response.Close();
                Response.End();

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected void btnMonthlyProgressExporttoExcel_Click(object sender, EventArgs e)
        {
            ViewState["Export"] = "2";
            BindMonthlyProjectProgressReport();

            StringBuilder StrExport = new StringBuilder();

            StringWriter stw = new StringWriter();
            HtmlTextWriter htextw = new HtmlTextWriter(stw);
            htextw.AddStyleAttribute("font-size", "9pt");
            htextw.AddStyleAttribute("color", "Black");



            GrdMonthPhysicalProgress.RenderControl(htextw); //Name of the Panel

            //var sb1 = new StringBuilder();
            //GridDiv.RenderControl(new HtmlTextWriter(new StringWriter(sb1)));

            string s = htextw.InnerWriter.ToString();

            string HTMLstring = "<html><body>" +
                "<div style='width:100%; margin:auto;'><div style='width:100%; float:left; line-height:25px; font-size:12pt;' align='center'>" +
                "<asp:Label ID='Lbl1' runat='server' Font-Bold='true'>" + WebConfigurationManager.AppSettings["Domain"] + " Report Name: Physical Progress Achievements in the month " + DDLMonth.SelectedItem.Text + " " + DDLYear.SelectedValue + "</asp:Label><br />" +
                "<asp:Label ID='Lbl4' runat='server'>Project Name : " + DDlProject.SelectedItem.Text + " (" + DDLWorkPackage.SelectedItem.Text + ")</asp:Label><br />" +
                "</div> <div style='width:100%; float:left; height:10px;'>&nbsp;&nbsp;&nbsp;</div>" +
                "<div style='width:100%;font-size:11pt float:left;' align='left'>" +
                s +
                "</div>" +
                "</div></body></html>";

            string strFile = "Report_Physical_Progress_Achievements_in_the_month_" + DDLMonth.SelectedItem.Text + "-" + DDLYear.SelectedValue + "_" + DateTime.Now.Ticks + ".xls";
            string strcontentType = "application/excel";
            Response.ClearContent();
            Response.ClearHeaders();
            Response.BufferOutput = true;
            Response.ContentType = strcontentType;
            Response.AddHeader("Content-Disposition", "attachment; filename=" + strFile);
            Response.Write(HTMLstring);
            Response.Flush();
            Response.Close();
            Response.End();
        }

        private string getTaskHeirarchy(Guid TaskUID)
        {
            string TaskList = string.Empty;
            string ParenttaskUID = getdt.GetParentTaskUID_by_TaskUID(TaskUID);
            while (!string.IsNullOrWhiteSpace(ParenttaskUID))
            {
                TaskList += getdt.getTaskNameby_TaskUID(new Guid(ParenttaskUID)) + "->";
                ParenttaskUID = getdt.GetParentTaskUID_by_TaskUID(new Guid(ParenttaskUID));
            }
            if (!string.IsNullOrEmpty(TaskList))
            {
                TaskList = TaskList.Substring(0, TaskList.Length - 2);
            }
            return TaskList;
        }

        protected void DDLWeekMonth_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (DDLWeekMonth.SelectedIndex == 0)
            {
                DDLWeek.Items.Clear();
                DDLWeek.Items.Add(new System.Web.UI.WebControls.ListItem("-- Select Week --", ""));
            }
            else
            {
                DataSet ds = getdt.GetWorkPackages_By_WorkPackageUID(new Guid(DDLWorkPackage.SelectedValue));
                if (ds.Tables[0].Rows.Count > 0)
                {
                    DateTime startDate = DateTime.Now;
                    if (Convert.ToDateTime(ds.Tables[0].Rows[0]["StartDate"].ToString()).Year.ToString() == DDLWeekYear.SelectedValue.ToString() && Convert.ToDateTime(ds.Tables[0].Rows[0]["StartDate"].ToString()).Month.ToString() == DDLWeekMonth.SelectedValue.ToString())
                    {
                        startDate = Convert.ToDateTime(ds.Tables[0].Rows[0]["StartDate"].ToString());
                    }
                    else
                    {
                        startDate = new DateTime(Convert.ToInt32(DDLWeekYear.SelectedValue.ToString()), Convert.ToInt32(DDLWeekMonth.SelectedValue.ToString()), 1);
                    }
                    int noOfDays = DateTime.DaysInMonth(startDate.Year, startDate.Month);
                    DDLWeek.Items.Clear();
                    DDLWeek.Items.Add(new System.Web.UI.WebControls.ListItem("-- Select Week --", ""));
                    for (int x = 0; x < (noOfDays / 7) + 1; x++)
                    {
                        DDLWeek.Items.Add(new System.Web.UI.WebControls.ListItem(startDate.ToString("dd/MM/yyyy") + " - " + startDate.AddDays(6).ToString("dd/MM/yyyy"), startDate.ToString("dd/MM/yyyy") + " - " + startDate.AddDays(6).ToString("dd/MM/yyyy")));
                        startDate = startDate.AddDays(7);
                    }
                }
            }
        }

        protected void DDLWeekYear_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (DDLWeekYear.SelectedIndex == 0)
            {
                DDLWeekMonth.Items.Clear();
                DDLWeekMonth.Items.Add(new System.Web.UI.WebControls.ListItem("-- Select Month --", ""));
                DDLWeek.Items.Clear();
                DDLWeek.Items.Add(new System.Web.UI.WebControls.ListItem("-- Select Week --", ""));

            }
            else
            {
                DataSet ds = getdt.GetWorkPackages_By_WorkPackageUID(new Guid(DDLWorkPackage.SelectedValue));
                if (ds.Tables[0].Rows.Count > 0)
                {
                    DateTime startDate = DateTime.Now;
                    if (Convert.ToDateTime(ds.Tables[0].Rows[0]["StartDate"].ToString()).Year.ToString() == DDLWeekYear.SelectedValue.ToString())
                    {
                        startDate = Convert.ToDateTime(ds.Tables[0].Rows[0]["StartDate"].ToString());
                    }
                    else
                    {
                        startDate = new DateTime(Convert.ToInt32(DDLWeekYear.SelectedValue.ToString()), 1, 1);
                    }

                    DDLWeekMonth.Items.Clear();
                    DDLWeekMonth.Items.Add(new System.Web.UI.WebControls.ListItem("-- Select Month --", ""));

                    while (startDate.Year == Convert.ToInt32(DDLWeekYear.SelectedValue.ToString()) && startDate <= Convert.ToDateTime(ds.Tables[0].Rows[0]["PlannedEndDate"].ToString()))
                    {
                        DDLWeekMonth.Items.Add(new System.Web.UI.WebControls.ListItem(startDate.ToString("MMM"), startDate.ToString("MM")));
                        startDate = startDate.AddMonths(1);

                    }
                }
            }
        }

        protected void DDLFotMonth_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (DDLFortiMonth.SelectedIndex == 0)
            {
                DDLFortWeek.Items.Clear();
                DDLFortWeek.Items.Add(new System.Web.UI.WebControls.ListItem("-- Select Week --", ""));
            }
            else
            {
                DataSet ds = getdt.GetWorkPackages_By_WorkPackageUID(new Guid(DDLWorkPackage.SelectedValue));
                if (ds.Tables[0].Rows.Count > 0)
                {
                    DateTime startDate = DateTime.Now;
                    if (Convert.ToDateTime(ds.Tables[0].Rows[0]["StartDate"].ToString()).Year.ToString() == DDLFortYear.SelectedValue.ToString() && Convert.ToDateTime(ds.Tables[0].Rows[0]["StartDate"].ToString()).Month.ToString() == DDLFortiMonth.SelectedValue.ToString())
                    {
                        startDate = Convert.ToDateTime(ds.Tables[0].Rows[0]["StartDate"].ToString());
                    }
                    else
                    {
                        startDate = new DateTime(Convert.ToInt32(DDLFortYear.SelectedValue.ToString()), Convert.ToInt32(DDLFortiMonth.SelectedValue.ToString()), 1);
                    }
                    int noOfDays = DateTime.DaysInMonth(startDate.Year, startDate.Month);
                    DDLFortWeek.Items.Clear();
                    DDLFortWeek.Items.Add(new System.Web.UI.WebControls.ListItem("-- Select Week --", ""));
                    for (int x = 0; x < 2; x++)
                    {
                        DDLFortWeek.Items.Add(new System.Web.UI.WebControls.ListItem(startDate.ToString("dd/MM/yyyy") + " - " + startDate.AddDays(14).ToString("dd/MM/yyyy"), startDate.ToString("dd/MM/yyyy") + " - " + startDate.AddDays(14).ToString("dd/MM/yyyy")));
                        startDate = startDate.AddDays(15);
                    }
                }
            }
        }

        protected void DDLFortYear_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (DDLFortYear.SelectedIndex == 0)
            {
                DDLFortiMonth.Items.Clear();
                DDLFortiMonth.Items.Add(new System.Web.UI.WebControls.ListItem("-- Select Month --", ""));
                DDLFortWeek.Items.Clear();
                DDLFortWeek.Items.Add(new System.Web.UI.WebControls.ListItem("-- Select Week --", ""));

            }
            else
            {
                DataSet ds = getdt.GetWorkPackages_By_WorkPackageUID(new Guid(DDLWorkPackage.SelectedValue));
                if (ds.Tables[0].Rows.Count > 0)
                {
                    DateTime startDate = DateTime.Now;
                    if (Convert.ToDateTime(ds.Tables[0].Rows[0]["StartDate"].ToString()).Year.ToString() == DDLFortYear.SelectedValue.ToString())
                    {
                        startDate = Convert.ToDateTime(ds.Tables[0].Rows[0]["StartDate"].ToString());
                    }
                    else
                    {
                        startDate = new DateTime(Convert.ToInt32(DDLFortYear.SelectedValue.ToString()), 1, 1);
                    }

                    DDLFortiMonth.Items.Clear();
                    DDLFortiMonth.Items.Add(new System.Web.UI.WebControls.ListItem("-- Select Month --", ""));

                    while (startDate.Year == Convert.ToInt32(DDLFortYear.SelectedValue.ToString()) && startDate <= Convert.ToDateTime(ds.Tables[0].Rows[0]["PlannedEndDate"].ToString()))
                    {
                        DDLFortiMonth.Items.Add(new System.Web.UI.WebControls.ListItem(startDate.ToString("MMM"), startDate.ToString("MM")));
                        startDate = startDate.AddMonths(1);

                    }
                }
            }
        }

        protected void BindFortiProgressReport()
        {
            ViewState["Export"] = "1";
            //LblWeeklyHeading.Text = "Weekly work Progress Status as on " + DDLWeek.SelectedValue.Split('-')[1];
            FortnightlyReportNameHeading.InnerHtml = "Report Name : Fortnight work Progress Status as on " + DDLFortWeek.SelectedValue.Split('-')[1];
            FortnightlyProjectName.InnerHtml = "Project Name : " + DDlProject.SelectedItem.Text + " (" + DDLWorkPackage.SelectedItem.Text + ")";
            DataSet ds = getdt.GetConstructionProgramme_Tasks(new Guid(DDLWorkPackage.SelectedValue));
            grdFortiProgressReport.DataSource = ds;
            grdFortiProgressReport.DataBind();

            FortiProgressReport.Visible = true;
            if (ds.Tables[0].Rows.Count > 0)
            {

                btnFortExportReportPDF.Visible = false;
                btnFortExportReportPrint.Visible = false;
                btnFortExportReportExcel.Visible = true;
            }
        }

        protected void GrdFortyprogress_DataBound(object sender, EventArgs e)
        {
            if (ViewState["Export"].ToString() == "1")
            {
                GridViewRow row = new GridViewRow(0, 0, DataControlRowType.Header, DataControlRowState.Normal);

                TableHeaderCell cell = new TableHeaderCell();
                cell.Text = "";
                cell.ColumnSpan = 1;
                row.Controls.Add(cell);

                cell = new TableHeaderCell();
                cell.Text = "";
                cell.ColumnSpan = 1;
                row.Controls.Add(cell);

                cell = new TableHeaderCell();
                cell.Text = "";
                cell.ColumnSpan = 1;
                row.Controls.Add(cell);

                cell = new TableHeaderCell();
                cell.Text = "";
                cell.ColumnSpan = 1;
                row.Controls.Add(cell);

                cell = new TableHeaderCell();
                cell.Text = "";
                cell.ColumnSpan = 1;
                row.Controls.Add(cell);

                cell = new TableHeaderCell();
                cell.Text = "";
                cell.ColumnSpan = 1;
                row.Controls.Add(cell);

                cell = new TableHeaderCell();
                cell.Text = "";
                cell.ColumnSpan = 1;
                row.Controls.Add(cell);

                cell = new TableHeaderCell();
                cell.Text = "";
                cell.ColumnSpan = 1;
                row.Controls.Add(cell);

                cell = new TableHeaderCell();
                cell.Text = "";
                cell.ColumnSpan = 1;
                row.Controls.Add(cell);

                cell = new TableHeaderCell();
                cell.Text = "";
                cell.ColumnSpan = 1;
                row.Controls.Add(cell);


                cell = new TableHeaderCell();
                cell.ColumnSpan = 2;
                cell.Text = "Target as on " + DDLFortWeek.SelectedValue.Split('-')[1].Trim();
                //cell.HorizontalAlign
                row.Controls.Add(cell);

                cell = new TableHeaderCell();
                cell.ColumnSpan = 2;
                cell.Text = "Achieved as on " + DDLFortWeek.SelectedValue.Split('-')[1].Trim();
                row.Controls.Add(cell);

                //row.BackColor = ColorTranslator.FromHtml("#3AC0F2");
                grdFortiProgressReport.HeaderRow.Parent.Controls.AddAt(0, row);
            }
        }

        protected void GrdFortprogress_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                string TaskUID = grdFortiProgressReport.DataKeys[e.Row.RowIndex].Values[0].ToString();

                string sDate1 = "", sDate2 = "";
                DateTime CDate1 = DateTime.MinValue, CDate2 = DateTime.MinValue;

                sDate1 = DDLFortWeek.SelectedValue.Split('-')[0].Trim();
                sDate1 = getdt.ConvertDateFormat(sDate1);
                CDate1 = Convert.ToDateTime(sDate1);

                sDate2 = DDLFortWeek.SelectedValue.Split('-')[1].Trim();
                sDate2 = getdt.ConvertDateFormat(sDate2);
                CDate2 = Convert.ToDateTime(sDate2);

                DataSet ds = getdt.GetPhysicalProgress_ForWeek__by_TaskUID(new Guid(TaskUID), CDate1, CDate2);

                if (ds.Tables[0].Rows.Count > 0)
                {
                    e.Row.Cells[3 + 6].Text = ds.Tables[0].Rows[0]["CumulativePlanned"].ToString() != "" ? decimal.Round(Convert.ToDecimal(ds.Tables[0].Rows[0]["CumulativePlanned"].ToString()), 2).ToString() : "";
                    e.Row.Cells[4 + 6].Text = ds.Tables[0].Rows[0]["PlanFortheWeek"].ToString() != "" ? decimal.Round(Convert.ToDecimal(ds.Tables[0].Rows[0]["PlanFortheWeek"].ToString()), 2).ToString() : "";
                    e.Row.Cells[5 + 6].Text = ds.Tables[0].Rows[0]["CumulativeAchieved"].ToString() != "" ? decimal.Round(Convert.ToDecimal(ds.Tables[0].Rows[0]["CumulativeAchieved"].ToString()), 2).ToString() : "";
                    e.Row.Cells[6 + 6].Text = ds.Tables[0].Rows[0]["AchievedFortheWeek"].ToString() != "" ? decimal.Round(Convert.ToDecimal(ds.Tables[0].Rows[0]["AchievedFortheWeek"].ToString()), 2).ToString() : "";
                    e.Row.Cells[7 + 6].Text = ds.Tables[0].Rows[0]["ProgressPercentage"].ToString() != "" ? decimal.Round(Convert.ToDecimal(ds.Tables[0].Rows[0]["ProgressPercentage"].ToString()), 2).ToString() : "";
                }

                if (e.Row.Cells[14].Text == "6")
                {
                    e.Row.Cells[2].Text = "";
                    e.Row.Cells[3].Text = "";
                    e.Row.Cells[4].Text = "";
                    e.Row.Cells[5].Text = "";
                    e.Row.Cells[6].Text = "";
                    e.Row.Cells[7].Text = "";
                }

                if (e.Row.Cells[14].Text == "7")
                {
                    e.Row.Cells[1].Text = "";
                    e.Row.Cells[3].Text = "";
                    e.Row.Cells[4].Text = "";
                    e.Row.Cells[5].Text = "";
                    e.Row.Cells[6].Text = "";
                    e.Row.Cells[7].Text = "";
                }

                if (e.Row.Cells[14].Text == "8")
                {
                    e.Row.Cells[1].Text = "";
                    e.Row.Cells[2].Text = "";
                    e.Row.Cells[4].Text = "";
                    e.Row.Cells[5].Text = "";
                    e.Row.Cells[6].Text = "";
                    e.Row.Cells[7].Text = "";
                }

                if (e.Row.Cells[14].Text == "9")
                {
                    e.Row.Cells[1].Text = "";
                    e.Row.Cells[2].Text = "";
                    e.Row.Cells[3].Text = "";
                    e.Row.Cells[5].Text = "";
                    e.Row.Cells[6].Text = "";
                    e.Row.Cells[7].Text = "";
                }

                if (e.Row.Cells[14].Text == "10")
                {
                    e.Row.Cells[1].Text = "";
                    e.Row.Cells[2].Text = "";
                    e.Row.Cells[3].Text = "";
                    e.Row.Cells[4].Text = "";
                    e.Row.Cells[6].Text = "";
                    e.Row.Cells[7].Text = "";
                }

                if (e.Row.Cells[14].Text == "11")
                {
                    e.Row.Cells[1].Text = "";
                    e.Row.Cells[2].Text = "";
                    e.Row.Cells[3].Text = "";
                    e.Row.Cells[4].Text = "";
                    e.Row.Cells[5].Text = "";
                    e.Row.Cells[7].Text = "";
                }

                if (e.Row.Cells[14].Text == "12")
                {
                    e.Row.Cells[1].Text = "";
                    e.Row.Cells[2].Text = "";
                    e.Row.Cells[3].Text = "";
                    e.Row.Cells[4].Text = "";
                    e.Row.Cells[5].Text = "";
                    e.Row.Cells[6].Text = "";
                }

            }
        }
        protected void btnFortExportReportExcel_Click(object sender, EventArgs e)
        {
            try
            {
                ViewState["Export"] = "2";
                BindFortiProgressReport();

                StringBuilder StrExport = new StringBuilder();

                StringWriter stw = new StringWriter();
                HtmlTextWriter htextw = new HtmlTextWriter(stw);
                htextw.AddStyleAttribute("font-size", "9pt");
                htextw.AddStyleAttribute("color", "Black");



                grdFortiProgressReport.RenderControl(htextw); //Name of the Panel

                //var sb1 = new StringBuilder();
                //GridDiv.RenderControl(new HtmlTextWriter(new StringWriter(sb1)));

                string s = htextw.InnerWriter.ToString();

                string HTMLstring = "<html><body>" +
                    "<div style='width:100%; margin:auto;'><div style='width:100%; float:left; line-height:25px; font-size:12pt;' align='center'>" +
                    "<asp:Label ID='Lbl1' runat='server' Font-Bold='true'>" + WebConfigurationManager.AppSettings["Domain"] + " Report Name: Weekly work Progress Status as on  " + DDLFortWeek.SelectedValue.Split('-')[1].Trim() + "</asp:Label><br />" +
                    "<asp:Label ID='Lbl4' runat='server'>Project Name : " + DDlProject.SelectedItem.Text + " (" + DDLWorkPackage.SelectedItem.Text + ")</asp:Label><br />" +
                    "</div> <div style='width:100%; float:left; height:10px;'>&nbsp;&nbsp;&nbsp;</div>" +
                    "<div style='width:100%;font-size:11pt float:left;' align='left'>" +
                    s +
                    "</div>" +
                    "</div></body></html>";

                string strFile = "Report_Forti_work_Progress_Status_" + DateTime.Now.Ticks + ".xls";
                string strcontentType = "application/excel";
                Response.ClearContent();
                Response.ClearHeaders();
                Response.BufferOutput = true;
                Response.ContentType = strcontentType;
                Response.AddHeader("Content-Disposition", "attachment; filename=" + strFile);
                Response.Write(HTMLstring);
                Response.Flush();
                Response.Close();
                Response.End();

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected void BtnQuarterSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                if (ddlQYear.SelectedValue != "" && ddlQuarter.SelectedValue != "")
                {
                    if (DDLMilestones.Enabled)
                    {
                        if (DDLMilestones.SelectedIndex > 0)
                        {
                            MileStoneGraph();
                        }
                        else
                        {
                            AllMileStoneGraph();
                        }
                    }
                    else
                    {
                        BindQuarterProjectProgressReport();
                    }

                }
                else
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>alert('Please select Month or Year');</script>");
                }
            }
            catch (Exception ex)
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>alert('There is a problem with this feature. Please contact system admin.');</script>");
            }
        }

        protected void BindQuarterProjectProgressReport()
        {
            ViewState["Export"] = "1";
            QuarterlyProgressReportName.InnerHtml = "Report Name : Physical Progress Achievements in the Quarter " + DDLMonth.SelectedItem.Text + " " + DDLYear.SelectedValue;
            QuarterlyProjectName.InnerHtml = "Project Name : " + DDlProject.SelectedItem.Text + " (" + DDLWorkPackage.SelectedItem.Text + ")";
            DataSet ds = getdt.GetConstructionProgramme_Tasks(new Guid(DDLWorkPackage.SelectedValue));
            GrdQuarterPhysicalProgress.DataSource = ds;
            GrdQuarterPhysicalProgress.DataBind();

            QuarterPhysicalProgress.Visible = true;
            if (ds.Tables[0].Rows.Count > 0)
            {
                btnQuarterProgressExportPDF.Visible = false;
                btnQuarterProgressPrint.Visible = false;
                btnQuarterProgressExporttoExcel.Visible = true;
            }
        }
        protected void GrdQuarterPhysicalProgress_DataBound(object sender, EventArgs e)
        {
            if (ViewState["Export"].ToString() == "1")
            {
                GridViewRow row = new GridViewRow(0, 0, DataControlRowType.Header, DataControlRowState.Normal);
                TableHeaderCell cell = new TableHeaderCell();
                cell.Text = "";
                cell.ColumnSpan = 1;
                row.Controls.Add(cell);

                cell = new TableHeaderCell();
                cell.Text = "";
                cell.ColumnSpan = 1;
                row.Controls.Add(cell);

                cell = new TableHeaderCell();
                cell.Text = "";
                cell.ColumnSpan = 1;
                row.Controls.Add(cell);

                cell = new TableHeaderCell();
                cell.Text = "";
                cell.ColumnSpan = 1;
                row.Controls.Add(cell);

                cell = new TableHeaderCell();
                cell.Text = "";
                cell.ColumnSpan = 1;
                row.Controls.Add(cell);

                cell = new TableHeaderCell();
                cell.Text = "";
                cell.ColumnSpan = 1;
                row.Controls.Add(cell);

                cell = new TableHeaderCell();
                cell.Text = "";
                cell.ColumnSpan = 1;
                row.Controls.Add(cell);

                cell = new TableHeaderCell();
                cell.Text = "";
                cell.ColumnSpan = 1;
                row.Controls.Add(cell);

                cell = new TableHeaderCell();
                cell.Text = "";
                cell.ColumnSpan = 1;
                row.Controls.Add(cell);

                cell = new TableHeaderCell();
                cell.Text = "";
                cell.ColumnSpan = 1;
                row.Controls.Add(cell);

                cell = new TableHeaderCell();
                cell.ColumnSpan = 1;
                cell.Text = "Up to previous Month";
                //cell.HorizontalAlign
                row.Controls.Add(cell);

                cell = new TableHeaderCell();
                cell.ColumnSpan = 3;
                cell.Text = "For this Month";
                row.Controls.Add(cell);

                cell = new TableHeaderCell();
                cell.ColumnSpan = 3;
                cell.Text = "Cumulative for the Project";
                row.Controls.Add(cell);

                //row.BackColor = ColorTranslator.FromHtml("#3AC0F2");
                GrdQuarterPhysicalProgress.HeaderRow.Parent.Controls.AddAt(0, row);
            }
        }

        protected void GrdQuarterPhysicalProgress_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                string TaskUID = GrdQuarterPhysicalProgress.DataKeys[e.Row.RowIndex].Values[0].ToString();

                string sDate1 = "", sDate2 = "";
                DateTime CDate1 = DateTime.MinValue, CDate2 = DateTime.MinValue;

                sDate1 = ddlQuarter.SelectedValue.Split('-')[0].Trim();
                sDate1 = getdt.ConvertDateFormat(sDate1);
                CDate1 = Convert.ToDateTime(sDate1);

                if (e.Row.Cells[2 + 6].Text != "")
                {

                    if (e.Row.Cells[2 + 6].Text.ToUpper() == "METERS")
                    {
                        e.Row.Cells[3 + 6].Text = e.Row.Cells[3 + 6].Text != "" ? Convert.ToDouble(e.Row.Cells[3 + 6].Text).ToString("N2") : "";
                    }
                    else if (e.Row.Cells[2 + 6].Text.ToUpper() == "NUMBERS")
                    {
                        e.Row.Cells[3 + 6].Text = e.Row.Cells[3 + 6].Text;
                    }
                    else if (e.Row.Cells[2 + 6].Text.ToUpper() == "RMT")
                    {
                        e.Row.Cells[3 + 6].Text = e.Row.Cells[3 + 6].Text != "" ? Convert.ToDouble(e.Row.Cells[3 + 6].Text).ToString("N2") : "";
                    }
                    else if (e.Row.Cells[2 + 6].Text.ToUpper() == "TONS")
                    {
                        e.Row.Cells[3 + 6].Text = e.Row.Cells[3 + 6].Text != "" ? Convert.ToDouble(e.Row.Cells[3 + 6].Text).ToString("N2") : "";
                    }
                    else if (e.Row.Cells[2 + 6].Text.ToUpper() == "KM")
                    {
                        e.Row.Cells[3 + 6].Text = e.Row.Cells[3 + 6].Text != "" ? Convert.ToDecimal(e.Row.Cells[3 + 6].Text).ToString("N2") : "";
                    }
                    else if (e.Row.Cells[2 + 6].Text.ToUpper() == "LOT")
                    {
                        e.Row.Cells[3 + 6].Text = e.Row.Cells[3 + 6].Text != "" ? Convert.ToDouble(e.Row.Cells[3 + 6].Text).ToString("N2") : "";
                    }
                    else if (e.Row.Cells[2 + 6].Text.ToUpper() == "HOUR")
                    {
                        e.Row.Cells[3 + 6].Text = e.Row.Cells[3 + 6].Text != "" ? Convert.ToDouble(e.Row.Cells[3 + 6].Text).ToString("N2") : "";
                    }
                    else
                    {
                        e.Row.Cells[3 + 6].Text = e.Row.Cells[3 + 6].Text != "" ? Convert.ToDouble(e.Row.Cells[3 + 6].Text).ToString("N2") : "";
                    }
                }
                sDate2 = ddlQuarter.SelectedValue.Split('-')[1].Trim();
                sDate2 = getdt.ConvertDateFormat(sDate2);
                CDate2 = Convert.ToDateTime(sDate2);
                DataSet ds = getdt.GetPhysicalProgress_ForMonth_by_TaskUID(new Guid(TaskUID), CDate1, CDate2);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    e.Row.Cells[4 + 6].Text = ds.Tables[0].Rows[0]["PrevAchieved"].ToString() != "" ? decimal.Round(Convert.ToDecimal(ds.Tables[0].Rows[0]["PrevAchieved"].ToString()), 2).ToString() : "";
                    e.Row.Cells[5 + 6].Text = ds.Tables[0].Rows[0]["PlannedThisMonth"].ToString() != "" ? decimal.Round(Convert.ToDecimal(ds.Tables[0].Rows[0]["PlannedThisMonth"].ToString()), 2).ToString() : "";
                    e.Row.Cells[6 + 6].Text = ds.Tables[0].Rows[0]["AchievedThisMonth"].ToString() != "" ? decimal.Round(Convert.ToDecimal(ds.Tables[0].Rows[0]["AchievedThisMonth"].ToString()), 2).ToString() : "";
                    e.Row.Cells[7 + 6].Text = ds.Tables[0].Rows[0]["AchievedPercentage"].ToString() != "" ? decimal.Round(Convert.ToDecimal(ds.Tables[0].Rows[0]["AchievedPercentage"].ToString()), 2).ToString() : "";
                    e.Row.Cells[8 + 6].Text = ds.Tables[0].Rows[0]["CumulativePlanned"].ToString() != "" ? decimal.Round(Convert.ToDecimal(ds.Tables[0].Rows[0]["CumulativePlanned"].ToString()), 2).ToString() : "";
                    e.Row.Cells[9 + 6].Text = ds.Tables[0].Rows[0]["CumulativeAchieved"].ToString() != "" ? decimal.Round(Convert.ToDecimal(ds.Tables[0].Rows[0]["CumulativeAchieved"].ToString()), 2).ToString() : "";
                    e.Row.Cells[10 + 6].Text = ds.Tables[0].Rows[0]["Balance"].ToString() != "" ? decimal.Round(Convert.ToDecimal(ds.Tables[0].Rows[0]["Balance"].ToString()), 2).ToString() : "";
                    e.Row.Cells[11 + 6].Text = ds.Tables[0].Rows[0]["NextMonthPlan"].ToString() != "" ? decimal.Round(Convert.ToDecimal(ds.Tables[0].Rows[0]["NextMonthPlan"].ToString()), 2).ToString() : "";
                    e.Row.Cells[12 + 6].Text = ds.Tables[0].Rows[0]["OverAllCompletion"].ToString() != "" ? decimal.Round(Convert.ToDecimal(ds.Tables[0].Rows[0]["OverAllCompletion"].ToString()), 2).ToString() : "";
                }

                if (e.Row.Cells[19].Text == "6")
                {
                    e.Row.Cells[2].Text = "";
                    e.Row.Cells[3].Text = "";
                    e.Row.Cells[4].Text = "";
                    e.Row.Cells[5].Text = "";
                    e.Row.Cells[6].Text = "";
                    e.Row.Cells[7].Text = "";
                }

                if (e.Row.Cells[19].Text == "7")
                {
                    e.Row.Cells[1].Text = "";
                    e.Row.Cells[3].Text = "";
                    e.Row.Cells[4].Text = "";
                    e.Row.Cells[5].Text = "";
                    e.Row.Cells[6].Text = "";
                    e.Row.Cells[7].Text = "";
                }

                if (e.Row.Cells[19].Text == "8")
                {
                    e.Row.Cells[1].Text = "";
                    e.Row.Cells[2].Text = "";
                    e.Row.Cells[4].Text = "";
                    e.Row.Cells[5].Text = "";
                    e.Row.Cells[6].Text = "";
                    e.Row.Cells[7].Text = "";
                }

                if (e.Row.Cells[19].Text == "9")
                {
                    e.Row.Cells[1].Text = "";
                    e.Row.Cells[2].Text = "";
                    e.Row.Cells[3].Text = "";
                    e.Row.Cells[5].Text = "";
                    e.Row.Cells[6].Text = "";
                    e.Row.Cells[7].Text = "";
                }

                if (e.Row.Cells[19].Text == "10")
                {
                    e.Row.Cells[1].Text = "";
                    e.Row.Cells[2].Text = "";
                    e.Row.Cells[3].Text = "";
                    e.Row.Cells[4].Text = "";
                    e.Row.Cells[6].Text = "";
                    e.Row.Cells[7].Text = "";
                }

                if (e.Row.Cells[19].Text == "11")
                {
                    e.Row.Cells[1].Text = "";
                    e.Row.Cells[2].Text = "";
                    e.Row.Cells[3].Text = "";
                    e.Row.Cells[4].Text = "";
                    e.Row.Cells[5].Text = "";
                    e.Row.Cells[7].Text = "";
                }

                if (e.Row.Cells[19].Text == "12")
                {
                    e.Row.Cells[1].Text = "";
                    e.Row.Cells[2].Text = "";
                    e.Row.Cells[3].Text = "";
                    e.Row.Cells[4].Text = "";
                    e.Row.Cells[5].Text = "";
                    e.Row.Cells[6].Text = "";
                }


            }
        }
        protected void btnQuarterProgressExporttoExcel_Click(object sender, EventArgs e)
        {
            try
            {
                ViewState["Export"] = "2";
                BindQuarterProjectProgressReport();

                StringBuilder StrExport = new StringBuilder();

                StringWriter stw = new StringWriter();
                HtmlTextWriter htextw = new HtmlTextWriter(stw);
                htextw.AddStyleAttribute("font-size", "9pt");
                htextw.AddStyleAttribute("color", "Black");



                GrdQuarterPhysicalProgress.RenderControl(htextw); //Name of the Panel

                //var sb1 = new StringBuilder();
                //GridDiv.RenderControl(new HtmlTextWriter(new StringWriter(sb1)));

                string s = htextw.InnerWriter.ToString();

                string HTMLstring = "<html><body>" +
                    "<div style='width:100%; margin:auto;'><div style='width:100%; float:left; line-height:25px; font-size:12pt;' align='center'>" +
                    "<asp:Label ID='Lbl1' runat='server' Font-Bold='true'>" + WebConfigurationManager.AppSettings["Domain"] + " Report Name: Weekly work Progress Status as on  " + ddlQuarter.SelectedValue.Split('-')[1].Trim() + "</asp:Label><br />" +
                    "<asp:Label ID='Lbl4' runat='server'>Project Name : " + DDlProject.SelectedItem.Text + " (" + DDLWorkPackage.SelectedItem.Text + ")</asp:Label><br />" +
                    "</div> <div style='width:100%; float:left; height:10px;'>&nbsp;&nbsp;&nbsp;</div>" +
                    "<div style='width:100%;font-size:11pt float:left;' align='left'>" +
                    s +
                    "</div>" +
                    "</div></body></html>";

                string strFile = "Report_Quarter_work_Progress_Status_" + DateTime.Now.Ticks + ".xls";
                string strcontentType = "application/excel";
                Response.ClearContent();
                Response.ClearHeaders();
                Response.BufferOutput = true;
                Response.ContentType = strcontentType;
                Response.AddHeader("Content-Disposition", "attachment; filename=" + strFile);
                Response.Write(HTMLstring);
                Response.Flush();
                Response.Close();
                Response.End();

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        protected void btnQuarterProgressExportPDF_Click(object sender, EventArgs e)
        {
            ViewState["Export"] = "2";
            BindMonthlyProjectProgressReport();
            ExporttoPDF(GrdQuarterPhysicalProgress, 2, "No");
        }

        protected void btnQuarterProgressPrint_Click(object sender, EventArgs e)
        {
            ViewState["Export"] = "2";
            BindQuarterProjectProgressReport();
            ExporttoPDF(GrdQuarterPhysicalProgress, 2, "Yes");
        }
        protected void DDLQuarterYear_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlQYear.SelectedIndex == 0)
            {
                ddlQuarter.Items.Clear();
                ddlQuarter.Items.Add(new System.Web.UI.WebControls.ListItem("-- Select Month --", ""));

            }
            else
            {
                DataSet ds = getdt.GetWorkPackages_By_WorkPackageUID(new Guid(DDLWorkPackage.SelectedValue));
                if (ds.Tables[0].Rows.Count > 0)
                {
                    DateTime startDate = DateTime.Now;
                    if (Convert.ToDateTime(ds.Tables[0].Rows[0]["StartDate"].ToString()).Year.ToString() == ddlQYear.SelectedValue.ToString())
                    {
                        startDate = Convert.ToDateTime(ds.Tables[0].Rows[0]["StartDate"].ToString());
                    }
                    else
                    {
                        startDate = new DateTime(Convert.ToInt32(ddlQYear.SelectedValue.ToString()), 1, 1);
                    }

                    ddlQuarter.Items.Clear();
                    ddlQuarter.Items.Add(new System.Web.UI.WebControls.ListItem("-- Select Quarter --", ""));

                    while (startDate.Year == Convert.ToInt32(ddlQYear.SelectedValue.ToString()) && startDate <= Convert.ToDateTime(ds.Tables[0].Rows[0]["PlannedEndDate"].ToString()))
                    {
                        int monthcnt = startDate.Month;
                        int cnt = 1;
                        while (monthcnt % 3 != 0)
                        {
                            monthcnt = monthcnt + 1;
                            cnt = cnt + 1;
                        }
                        ddlQuarter.Items.Add(new System.Web.UI.WebControls.ListItem(startDate.ToString("dd/MM/yyyy") + " - " + startDate.AddMonths(cnt).AddDays(-startDate.Day).ToString("dd/MM/yyyy"), startDate.ToString("dd/MM/yyyy") + " - " + startDate.AddMonths(cnt).AddDays(-startDate.Day).ToString("dd/MM/yyyy")));
                        startDate = startDate.AddMonths(cnt).AddDays(-startDate.Day + 1);

                    }
                }
            }
        }

        protected void DDLHalfYear_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ddlHalfYear.SelectedIndex == 0)
            {
                ddlHalfyearperiod.Items.Clear();
                ddlHalfyearperiod.Items.Add(new System.Web.UI.WebControls.ListItem("-- Select Month --", ""));

            }
            else
            {
                DataSet ds = getdt.GetWorkPackages_By_WorkPackageUID(new Guid(DDLWorkPackage.SelectedValue));
                if (ds.Tables[0].Rows.Count > 0)
                {
                    DateTime startDate = DateTime.Now;
                    if (Convert.ToDateTime(ds.Tables[0].Rows[0]["StartDate"].ToString()).Year.ToString() == ddlHalfYear.SelectedValue.ToString())
                    {
                        startDate = Convert.ToDateTime(ds.Tables[0].Rows[0]["StartDate"].ToString());
                    }
                    else
                    {
                        startDate = new DateTime(Convert.ToInt32(ddlHalfYear.SelectedValue.ToString()), 1, 1);
                    }

                    ddlHalfyearperiod.Items.Clear();
                    ddlHalfyearperiod.Items.Add(new System.Web.UI.WebControls.ListItem("-- Select Half Year --", ""));

                    while (startDate.Year == Convert.ToInt32(ddlHalfYear.SelectedValue.ToString()) && startDate <= Convert.ToDateTime(ds.Tables[0].Rows[0]["PlannedEndDate"].ToString()))
                    {
                        int monthcnt = startDate.Month;
                        int cnt = 1;
                        while (monthcnt % 6 != 0)
                        {
                            monthcnt = monthcnt + 1;
                            cnt = cnt + 1;
                        }
                        ddlHalfyearperiod.Items.Add(new System.Web.UI.WebControls.ListItem(startDate.ToString("dd/MM/yyyy") + " - " + startDate.AddMonths(cnt).AddDays(-startDate.Day).ToString("dd/MM/yyyy"), startDate.ToString("dd/MM/yyyy") + " - " + startDate.AddMonths(cnt).AddDays(-startDate.Day).ToString("dd/MM/yyyy")));
                        startDate = startDate.AddMonths(cnt).AddDays(-startDate.Day + 1);

                    }
                }
            }
        }
        protected void BtnHalfYearSubmit_Click(object sender, EventArgs e)
        {
            try
            {
                if (ddlHalfYear.SelectedValue != "" && ddlHalfyearperiod.SelectedValue != "")
                {
                    if (DDLMilestones.Enabled)
                    {
                        if (DDLMilestones.SelectedIndex > 0)
                        {
                            MileStoneGraph();
                        }
                        else
                        {
                            AllMileStoneGraph();
                        }
                    }
                    else
                    {
                        BindHalfYearProjectProgressReport();
                    }

                }
                else
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>alert('Please select Month or Year');</script>");
                }
            }
            catch (Exception ex)
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>alert('There is a problem with this feature. Please contact system admin.');</script>");
            }
        }

        protected void BindHalfYearProjectProgressReport()
        {
            ViewState["Export"] = "1";
            HalfYearlyReportHeading.InnerHtml = "Report Name : Physical Progress Achievements in the Half year " + ddlHalfyearperiod.SelectedItem.Text;
            HalfYearlyProjectName.InnerHtml = "Project Name : " + DDlProject.SelectedItem.Text + " (" + DDLWorkPackage.SelectedItem.Text + ")";
            DataSet ds = getdt.GetConstructionProgramme_Tasks(new Guid(DDLWorkPackage.SelectedValue));
            GrdHalfYearPhysicalProgress.DataSource = ds;
            GrdHalfYearPhysicalProgress.DataBind();

            HalfYearPhysicalProgress.Visible = true;
            if (ds.Tables[0].Rows.Count > 0)
            {
                btnHalfYearProgressExportPDF.Visible = false;
                btnHalfYearProgressPrint.Visible = false;
                btnHalfYearProgressExporttoExcel.Visible = true;
            }
        }
        protected void GrdHalfYearPhysicalProgress_DataBound(object sender, EventArgs e)
        {
            if (ViewState["Export"].ToString() == "1")
            {
                GridViewRow row = new GridViewRow(0, 0, DataControlRowType.Header, DataControlRowState.Normal);
                TableHeaderCell cell = new TableHeaderCell();
                cell.Text = "";
                cell.ColumnSpan = 1;
                row.Controls.Add(cell);

                cell = new TableHeaderCell();
                cell.Text = "";
                cell.ColumnSpan = 1;
                row.Controls.Add(cell);

                cell = new TableHeaderCell();
                cell.Text = "";
                cell.ColumnSpan = 1;
                row.Controls.Add(cell);

                cell = new TableHeaderCell();
                cell.Text = "";
                cell.ColumnSpan = 1;
                row.Controls.Add(cell);

                cell = new TableHeaderCell();
                cell.Text = "";
                cell.ColumnSpan = 1;
                row.Controls.Add(cell);

                cell = new TableHeaderCell();
                cell.Text = "";
                cell.ColumnSpan = 1;
                row.Controls.Add(cell);

                cell = new TableHeaderCell();
                cell.Text = "";
                cell.ColumnSpan = 1;
                row.Controls.Add(cell);

                cell = new TableHeaderCell();
                cell.Text = "";
                cell.ColumnSpan = 1;
                row.Controls.Add(cell);

                cell = new TableHeaderCell();
                cell.Text = "";
                cell.ColumnSpan = 1;
                row.Controls.Add(cell);

                cell = new TableHeaderCell();
                cell.Text = "";
                cell.ColumnSpan = 1;
                row.Controls.Add(cell);

                cell = new TableHeaderCell();
                cell.ColumnSpan = 1;
                cell.Text = "Up to previous Month";
                //cell.HorizontalAlign
                row.Controls.Add(cell);

                cell = new TableHeaderCell();
                cell.ColumnSpan = 3;
                cell.Text = "For this Month";
                row.Controls.Add(cell);

                cell = new TableHeaderCell();
                cell.ColumnSpan = 3;
                cell.Text = "Cumulative for the Project";
                row.Controls.Add(cell);

                //row.BackColor = ColorTranslator.FromHtml("#3AC0F2");
                GrdHalfYearPhysicalProgress.HeaderRow.Parent.Controls.AddAt(0, row);
            }
        }

        protected void GrdHalfYearPhysicalProgress_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                string TaskUID = GrdHalfYearPhysicalProgress.DataKeys[e.Row.RowIndex].Values[0].ToString();

                string sDate1 = "", sDate2 = "";
                DateTime CDate1 = DateTime.MinValue, CDate2 = DateTime.MinValue;

                sDate1 = ddlHalfyearperiod.SelectedValue.Split('-')[0].Trim();
                sDate1 = getdt.ConvertDateFormat(sDate1);
                CDate1 = Convert.ToDateTime(sDate1);

                if (e.Row.Cells[2 + 6].Text != "")
                {

                    if (e.Row.Cells[2 + 6].Text.ToUpper() == "METERS")
                    {
                        e.Row.Cells[3 + 6].Text = e.Row.Cells[3 + 6].Text != "" ? Convert.ToDouble(e.Row.Cells[3 + 6].Text).ToString("N2") : "";
                    }
                    else if (e.Row.Cells[2 + 6].Text.ToUpper() == "NUMBERS")
                    {
                        e.Row.Cells[3 + 6].Text = e.Row.Cells[3 + 6].Text;
                    }
                    else if (e.Row.Cells[2 + 6].Text.ToUpper() == "RMT")
                    {
                        e.Row.Cells[3 + 6].Text = e.Row.Cells[3 + 6].Text != "" ? Convert.ToDouble(e.Row.Cells[3 + 6].Text).ToString("N2") : "";
                    }
                    else if (e.Row.Cells[2 + 6].Text.ToUpper() == "TONS")
                    {
                        e.Row.Cells[3 + 6].Text = e.Row.Cells[3 + 6].Text != "" ? Convert.ToDouble(e.Row.Cells[3 + 6].Text).ToString("N2") : "";
                    }
                    else if (e.Row.Cells[2 + 6].Text.ToUpper() == "KM")
                    {
                        e.Row.Cells[3 + 6].Text = e.Row.Cells[3 + 6].Text != "" ? Convert.ToDecimal(e.Row.Cells[3 + 6].Text).ToString("N2") : "";
                    }
                    else if (e.Row.Cells[2 + 6].Text.ToUpper() == "LOT")
                    {
                        e.Row.Cells[3 + 6].Text = e.Row.Cells[3 + 6].Text != "" ? Convert.ToDouble(e.Row.Cells[3 + 6].Text).ToString("N2") : "";
                    }
                    else if (e.Row.Cells[2 + 6].Text.ToUpper() == "HOUR")
                    {
                        e.Row.Cells[3 + 6].Text = e.Row.Cells[3 + 6].Text != "" ? Convert.ToDouble(e.Row.Cells[3 + 6].Text).ToString("N2") : "";
                    }
                    else
                    {
                        e.Row.Cells[3 + 6].Text = e.Row.Cells[3 + 6].Text != "" ? Convert.ToDouble(e.Row.Cells[3 + 6].Text).ToString("N2") : "";
                    }
                }
                sDate2 = ddlHalfyearperiod.SelectedValue.Split('-')[1].Trim();
                sDate2 = getdt.ConvertDateFormat(sDate2);
                CDate2 = Convert.ToDateTime(sDate2);
                DataSet ds = getdt.GetPhysicalProgress_ForMonth_by_TaskUID(new Guid(TaskUID), CDate1, CDate2);
                if (ds.Tables[0].Rows.Count > 0)
                {
                    e.Row.Cells[4 + 6].Text = ds.Tables[0].Rows[0]["PrevAchieved"].ToString() != "" ? decimal.Round(Convert.ToDecimal(ds.Tables[0].Rows[0]["PrevAchieved"].ToString()), 2).ToString() : "";
                    e.Row.Cells[5 + 6].Text = ds.Tables[0].Rows[0]["PlannedThisMonth"].ToString() != "" ? decimal.Round(Convert.ToDecimal(ds.Tables[0].Rows[0]["PlannedThisMonth"].ToString()), 2).ToString() : "";
                    e.Row.Cells[6 + 6].Text = ds.Tables[0].Rows[0]["AchievedThisMonth"].ToString() != "" ? decimal.Round(Convert.ToDecimal(ds.Tables[0].Rows[0]["AchievedThisMonth"].ToString()), 2).ToString() : "";
                    e.Row.Cells[7 + 6].Text = ds.Tables[0].Rows[0]["AchievedPercentage"].ToString() != "" ? decimal.Round(Convert.ToDecimal(ds.Tables[0].Rows[0]["AchievedPercentage"].ToString()), 2).ToString() : "";
                    e.Row.Cells[8 + 6].Text = ds.Tables[0].Rows[0]["CumulativePlanned"].ToString() != "" ? decimal.Round(Convert.ToDecimal(ds.Tables[0].Rows[0]["CumulativePlanned"].ToString()), 2).ToString() : "";
                    e.Row.Cells[9 + 6].Text = ds.Tables[0].Rows[0]["CumulativeAchieved"].ToString() != "" ? decimal.Round(Convert.ToDecimal(ds.Tables[0].Rows[0]["CumulativeAchieved"].ToString()), 2).ToString() : "";
                    e.Row.Cells[10 + 6].Text = ds.Tables[0].Rows[0]["Balance"].ToString() != "" ? decimal.Round(Convert.ToDecimal(ds.Tables[0].Rows[0]["Balance"].ToString()), 2).ToString() : "";
                    e.Row.Cells[11 + 6].Text = ds.Tables[0].Rows[0]["NextMonthPlan"].ToString() != "" ? decimal.Round(Convert.ToDecimal(ds.Tables[0].Rows[0]["NextMonthPlan"].ToString()), 2).ToString() : "";
                    e.Row.Cells[12 + 6].Text = ds.Tables[0].Rows[0]["OverAllCompletion"].ToString() != "" ? decimal.Round(Convert.ToDecimal(ds.Tables[0].Rows[0]["OverAllCompletion"].ToString()), 2).ToString() : "";
                }

                if (e.Row.Cells[19].Text == "6")
                {
                    e.Row.Cells[2].Text = "";
                    e.Row.Cells[3].Text = "";
                    e.Row.Cells[4].Text = "";
                    e.Row.Cells[5].Text = "";
                    e.Row.Cells[6].Text = "";
                    e.Row.Cells[7].Text = "";
                }

                if (e.Row.Cells[19].Text == "7")
                {
                    e.Row.Cells[1].Text = "";
                    e.Row.Cells[3].Text = "";
                    e.Row.Cells[4].Text = "";
                    e.Row.Cells[5].Text = "";
                    e.Row.Cells[6].Text = "";
                    e.Row.Cells[7].Text = "";
                }

                if (e.Row.Cells[19].Text == "8")
                {
                    e.Row.Cells[1].Text = "";
                    e.Row.Cells[2].Text = "";
                    e.Row.Cells[4].Text = "";
                    e.Row.Cells[5].Text = "";
                    e.Row.Cells[6].Text = "";
                    e.Row.Cells[7].Text = "";
                }

                if (e.Row.Cells[19].Text == "9")
                {
                    e.Row.Cells[1].Text = "";
                    e.Row.Cells[2].Text = "";
                    e.Row.Cells[3].Text = "";
                    e.Row.Cells[5].Text = "";
                    e.Row.Cells[6].Text = "";
                    e.Row.Cells[7].Text = "";
                }

                if (e.Row.Cells[19].Text == "10")
                {
                    e.Row.Cells[1].Text = "";
                    e.Row.Cells[2].Text = "";
                    e.Row.Cells[3].Text = "";
                    e.Row.Cells[4].Text = "";
                    e.Row.Cells[6].Text = "";
                    e.Row.Cells[7].Text = "";
                }

                if (e.Row.Cells[19].Text == "11")
                {
                    e.Row.Cells[1].Text = "";
                    e.Row.Cells[2].Text = "";
                    e.Row.Cells[3].Text = "";
                    e.Row.Cells[4].Text = "";
                    e.Row.Cells[5].Text = "";
                    e.Row.Cells[7].Text = "";
                }

                if (e.Row.Cells[19].Text == "12")
                {
                    e.Row.Cells[1].Text = "";
                    e.Row.Cells[2].Text = "";
                    e.Row.Cells[3].Text = "";
                    e.Row.Cells[4].Text = "";
                    e.Row.Cells[5].Text = "";
                    e.Row.Cells[6].Text = "";
                }
            }
        }
        protected void btnHalfYearProgressExporttoExcel_Click(object sender, EventArgs e)
        {
            try
            {
                ViewState["Export"] = "2";
                BindHalfYearProjectProgressReport();

                StringBuilder StrExport = new StringBuilder();

                StringWriter stw = new StringWriter();
                HtmlTextWriter htextw = new HtmlTextWriter(stw);
                htextw.AddStyleAttribute("font-size", "9pt");
                htextw.AddStyleAttribute("color", "Black");



                GrdHalfYearPhysicalProgress.RenderControl(htextw); //Name of the Panel

                //var sb1 = new StringBuilder();
                //GridDiv.RenderControl(new HtmlTextWriter(new StringWriter(sb1)));

                string s = htextw.InnerWriter.ToString();

                string HTMLstring = "<html><body>" +
                    "<div style='width:100%; margin:auto;'><div style='width:100%; float:left; line-height:25px; font-size:12pt;' align='center'>" +
                    "<asp:Label ID='Lbl1' runat='server' Font-Bold='true'>" + WebConfigurationManager.AppSettings["Domain"] + " Report Name: Weekly work Progress Status as on  " + ddlHalfyearperiod.SelectedValue.Split('-')[1].Trim() + "</asp:Label><br />" +
                    "<asp:Label ID='Lbl4' runat='server'>Project Name : " + DDlProject.SelectedItem.Text + " (" + DDLWorkPackage.SelectedItem.Text + ")</asp:Label><br />" +
                    "</div> <div style='width:100%; float:left; height:10px;'>&nbsp;&nbsp;&nbsp;</div>" +
                    "<div style='width:100%;font-size:11pt float:left;' align='left'>" +
                    s +
                    "</div>" +
                    "</div></body></html>";

                string strFile = "Report_HalfYear_work_Progress_Status_" + DateTime.Now.Ticks + ".xls";
                string strcontentType = "application/excel";
                Response.ClearContent();
                Response.ClearHeaders();
                Response.BufferOutput = true;
                Response.ContentType = strcontentType;
                Response.AddHeader("Content-Disposition", "attachment; filename=" + strFile);
                Response.Write(HTMLstring);
                Response.Flush();
                Response.Close();
                Response.End();

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        protected void btnHalfYearProgressExportPDF_Click(object sender, EventArgs e)
        {
            ViewState["Export"] = "2";
            BindHalfYearProjectProgressReport();
            ExporttoPDF(GrdHalfYearPhysicalProgress, 2, "No");
        }

        protected void btnHalfYearProgressPrint_Click(object sender, EventArgs e)
        {
            ViewState["Export"] = "2";
            BindHalfYearProjectProgressReport();
            ExporttoPDF(GrdHalfYearPhysicalProgress, 2, "Yes");
        }

        protected void RBLType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (RBLType.SelectedIndex == 1)
            {
                DDLMilestones.Enabled = true;
                BindMilestones();
                RBLReportFor.Visible = false;
                MonthlyPhysicalProgress.Visible = false;
                QuarterPhysicalProgress.Visible = false;
                HalfYearPhysicalProgress.Visible = false;
                ByHalfYear.Visible = false;
                ByMonth.Visible = false;
                ByWeek.Visible = false;
                ByQuarteMonth.Visible = false;
                Byfortingiht.Visible = false;
                WeeklyProgressReport.Visible = false;
                AcrossMonthsProgressReport.Visible = false;
                FortiProgressReport.Visible = false;
                ByActivity.Visible = false;
                ActivityProgressReport.Visible = false;
                AllMilestoneGraph.Visible = true;
                RBLReportFor.Items[0].Selected = false;
                RBLReportFor.Items[1].Selected = false;
                RBLReportFor.Items[2].Selected = false;
                RBLReportFor.Items[3].Selected = false;
                RBLReportFor.Items[4].Selected = false;
                RBLReportFor.Items[5].Selected = false;
                RBLReportFor.Items[6].Selected = false;

            }
            else
            {
                DDLMilestones.Items.Clear();
                DDLMilestones.Enabled = false;
                RBLReportFor.Visible = true;
                MonthlyPhysicalProgress.Visible = false;
                QuarterPhysicalProgress.Visible = false;
                HalfYearPhysicalProgress.Visible = false;
                ByHalfYear.Visible = false;
                ByMonth.Visible = false;
                ByWeek.Visible = false;
                ByQuarteMonth.Visible = false;
                Byfortingiht.Visible = false;
                WeeklyProgressReport.Visible = false;
                AcrossMonthsProgressReport.Visible = false;
                FortiProgressReport.Visible = false;
                ByActivity.Visible = false;
                ActivityProgressReport.Visible = false;
                AllMilestoneGraph.Visible = false;
                RBLReportFor.Items[0].Selected = false;
                RBLReportFor.Items[1].Selected = false;
                RBLReportFor.Items[2].Selected = false;
                RBLReportFor.Items[3].Selected = false;
                RBLReportFor.Items[4].Selected = false;
                RBLReportFor.Items[5].Selected = false;
                RBLReportFor.Items[6].Selected = false;
            }
        }

        protected void BindMilestones()
        {
            DDLMilestones.Items.Clear();

            DataSet ds = getdt.GetAllMileStones(new Guid(DDLWorkPackage.SelectedValue));

            //List <Milestone> Milestones = new List<Milestone>()
            //{
            //    new Milestone() { MilestoneId="Milestone A", Name="Mobilization" },
            //    new Milestone() { MilestoneId="Milestone B", Name="Mobilize Construction Equipment" },
            //    new Milestone() { MilestoneId="Milestone C", Name="Submission of design and drawings" },
            //    new Milestone() { MilestoneId="Milestone D", Name="Place orders for plant and equipments" },
            //    new Milestone() { MilestoneId="Milestone E", Name="Civil Works" },
            //    new Milestone() { MilestoneId="Milestone F", Name="Receipt of Plant and Equipments" },
            //    new Milestone() { MilestoneId="Milestone G", Name="Trial run and commissioning" },
            //    new Milestone() { MilestoneId="Milestone H", Name="Power Supply Systems" },
            //    new Milestone() { MilestoneId="Milestone I", Name="Plant road construction" },
            //    new Milestone() { MilestoneId="Milestone J", Name="Painting Landscape" },
            //    new Milestone() { MilestoneId="Milestone K", Name="Demolition of Existing Structures" }
            //};

            DDLMilestones.DataValueField = "MilestoneUID";
            DDLMilestones.DataTextField = "MilestoneName";
            DDLMilestones.DataSource = ds;
            DDLMilestones.DataBind();

            DDLMilestones.Items.Insert(0, "All");
        }

        protected void AllMileStoneGraph()
        {

            ActivityProgressReport.Visible = true;
            //LblActivityPhysicalProgress.Text = "Physical Progress for the Activity : " + DDLActivity.SelectedItem.Text;

            ActivityProgressReportName.InnerHtml = "Report Name : Physical Progress Monitoring for the Activity '" + DDLMilestones.SelectedItem.Text + "'";
            ActivityProgressProjectName.InnerHtml = "Project Name : " + DDlProject.SelectedItem.Text + " (" + DDLWorkPackage.SelectedItem.Text + ")";

            decimal PrevCumulativePlan = 0;
            decimal PrevCumulativeActual = 0;

            DataSet ds = getdt.GetAllMilestoneGraphValues();

            List<string> MonthDays = new List<string>() { "31", "28", "31", "30", "31", "30", "31", "31", "30", "31", "30", "31" };

            List<WorkSchedule> schedule_list = CreateWorkTable();

            // DataTable ds = null;

            string fromDateString = "";
            string toDateString = "";
            int fromMonth = 0;
            int toMonth = 0;

            DateTime fromDate = new DateTime();
            DateTime toDate = new DateTime();

            //switch (RBLReportFor.SelectedValue)
            //{

            //    case "By Week":
            //        fromDateString = DDLWeek.SelectedValue.Split('-')[0];
            //        string day = fromDateString.Split('/')[0];
            //        string month = fromDateString.Split('/')[1];
            //        string year = fromDateString.Split('/')[2];

            //        fromDateString = month + "/" + day + "/" + year;
            //        fromDate = Convert.ToDateTime(fromDateString);

            //        toDateString = DDLWeek.SelectedValue.Split('-')[1];

            //        day = toDateString.Split('/')[0];
            //        month = toDateString.Split('/')[1];
            //        year = toDateString.Split('/')[2];

            //        toDateString = month + "/" + day + "/" + year;

            //        toDate = Convert.ToDateTime(toDateString);

            //        break;
            //    case "By Fortnightly":

            //        fromDateString = DDLFortWeek.SelectedValue.Split('-')[0];
            //        string day1 = fromDateString.Split('/')[0];
            //        string month1 = fromDateString.Split('/')[1];
            //        string year1 = fromDateString.Split('/')[2];

            //        fromDateString = month1 + "/" + day1 + "/" + year1;
            //        fromDate = Convert.ToDateTime(fromDateString);

            //        toDateString = DDLFortWeek.SelectedValue.Split('-')[1];

            //        day1 = toDateString.Split('/')[0];
            //        month1 = toDateString.Split('/')[1];
            //        year1 = toDateString.Split('/')[2];

            //        toDateString = month1 + "/" + day1 + "/" + year1;

            //        toDate = Convert.ToDateTime(toDateString);
            //        break;
            //    case "By Month":
            //        fromDateString = DDLMonth.SelectedValue + "/" + "01" + "/" + DDLYear.SelectedValue;
            //        fromDate = Convert.ToDateTime(fromDateString);

            //        toDateString = DDLMonth.SelectedValue + "/" + MonthDays[DDLMonth.SelectedIndex] + "/" + DDLYear.SelectedValue;
            //        toDate = Convert.ToDateTime(toDateString);
            //        break;
            //    case "By Quarter":
            //        fromDateString = ddlQuarter.SelectedValue.Split('-')[0];
            //        string day2 = fromDateString.Split('/')[0];
            //        string month2 = fromDateString.Split('/')[1];
            //        string year2 = fromDateString.Split('/')[2];

            //        fromDateString = month2 + "/" + day2 + "/" + year2;
            //        fromDate = Convert.ToDateTime(fromDateString);

            //        fromMonth = fromDate.Month;

            //        toDateString = ddlQuarter.SelectedValue.Split('-')[1];

            //        day2 = toDateString.Split('/')[0];
            //        month2 = toDateString.Split('/')[1];
            //        year2 = toDateString.Split('/')[2];

            //        toDateString = month2 + "/" + day2 + "/" + year2;

            //        toDate = Convert.ToDateTime(toDateString);

            //        toMonth = toDate.Month;
            //        break;
            //    case "By HalfYear":
            //        fromDateString = ddlHalfyearperiod.SelectedValue.Split('-')[0];
            //        string day3 = fromDateString.Split('/')[0];
            //        string month3 = fromDateString.Split('/')[1];
            //        string year3 = fromDateString.Split('/')[2];

            //        fromDateString = month3 + "/" + day3 + "/" + year3;
            //        fromDate = Convert.ToDateTime(fromDateString);

            //        fromMonth = fromDate.Month;

            //        toDateString = ddlHalfyearperiod.SelectedValue.Split('-')[1];

            //        day3 = toDateString.Split('/')[0];
            //        month3 = toDateString.Split('/')[1];
            //        year3 = toDateString.Split('/')[2];

            //        toDateString = month3 + "/" + day3 + "/" + year3;

            //        toDate = Convert.ToDateTime(toDateString);
            //        toMonth = toDate.Month;

            //        break;
            //}

            //if (RBLReportFor.SelectedIndex == 3 || RBLReportFor.SelectedIndex == 4)
            //{
            //    var milestones = schedule_list.OrderBy(a => a.WorkDate).GroupBy(a => new { a.WorkDate.Year, a.WorkDate.Month }).Select(a => new MilestoneGraph { Year = a.Key.Year.ToString(), Month = a.Key.Month.ToString(), PlannedValue = a.Sum(b => b.PlannedValue), AchievedValue = a.Sum(b => b.AchievedValue) }).ToList();
            //    ds = ToDataTable(milestones);
            //}
            //else
            //{
            //    var milestones = schedule_list.OrderBy(a => a.WorkDate).GroupBy(a => new { a.WorkDate, a.MileStoneId, a.MileStoneName }).Select(a => new MilestoneGraph { WorkDate = a.Key.WorkDate, MileStoneId = a.Key.MileStoneName, PlannedValue = a.Sum(b => b.PlannedValue), AchievedValue = a.Sum(b => b.AchievedValue) }).ToList();
            //    ds = ToDataTable(milestones);
            //}

            // var milestones = schedule_list.OrderBy(a => a.MileStoneId).GroupBy(a => new { a.MileStoneId, a.MileStoneName }).Select(a => new MilestoneGraph {  MileStoneId = a.Key.MileStoneName.ToString(), PlannedValue = a.Sum(b => b.PlannedValue), AchievedValue = a.Sum(b => b.AchievedValue) }).ToList();
            // ds = ToDataTable(milestones);



            //fromDateString = DDLMonth.SelectedValue + "/" + "01" + "/" + DDLYear.SelectedValue;
            //fromDate = Convert.ToDateTime(fromDateString);

            //toDateString = DDLMonth.SelectedValue + "/" + MonthDays[DDLMonth.SelectedIndex] + "/" + DDLYear.SelectedValue;
            //toDate = Convert.ToDateTime(toDateString);



            if (ds.Tables[0].Rows.Count > 0)
            {
                btnActivityProgressPrint.Visible = true;
                bool ShowTable = false;
                string tablemonths = "<td style=\"width:200px\">&nbsp;</td>";
                string tmonthlyplan = "<td style=\"padding:3px\">Milestone Plan</td>";
                string tmonthlyactual = "<td style=\"padding:3px\">Milestone Actual</td>";
                string tcumulativeplan = "<td style=\"padding:3px\">Cumulative Plan</td>";
                string tcumulativeactual = "<td style=\"padding:3px\">Cumulative Actual</td>";

                System.Text.StringBuilder strScript = new System.Text.StringBuilder();
                strScript.Append(@"<script type='text/javascript'>
                    google.charts.load('current', { 'packages':['corechart']
                    });
                      if (typeof google.charts.visualization == 'undefined') {
                        google.charts.setOnLoadCallback(drawVisualization);
                    }
                    else {
                        drawVisualization();
                    }
                    function drawVisualization()
                    {
                        // Some raw data (not necessarily accurate)
                        var data = google.visualization.arrayToDataTable([
                          ['Milestone', 'Plan', 'Actual', 'Cumulative Plan', 'Cumulative Actual'],");
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    ShowTable = true;
                    string Plan = ds.Tables[0].Rows[i]["PlannedValue"].ToString() != "" ? ds.Tables[0].Rows[i]["PlannedValue"].ToString() : "0";
                    string Actual = ds.Tables[0].Rows[i]["AchievedValue"].ToString() != "" ? ds.Tables[0].Rows[i]["AchievedValue"].ToString() : "0";
                    if (Plan != "" && Actual != "")
                    {
                        if (i == 0)
                        {
                            PrevCumulativePlan = Convert.ToDecimal(Plan);
                            PrevCumulativeActual = Convert.ToDecimal(Actual);
                            // strScript.Append("['" + Convert.ToDateTime(ds.Rows[i]["WorkDate"].ToString()).ToString("dd-MMM-yy") + "'," + Plan + "," + Actual + "," + Plan + "," + Actual + "],");
                            strScript.Append("['" + ds.Tables[0].Rows[i]["MilestoneName"].ToString() + "'," + Plan + "," + Actual + "," + Plan + "," + Actual + "],");

                        }
                        else
                        {
                            // strScript.Append("['" + Convert.ToDateTime(ds.Rows[i]["WorkDate"].ToString()).ToString("dd-MMM-yy") + "'," + Plan + "," + Actual + "," + (Convert.ToDecimal(Plan) + PrevCumulativePlan) + "," + (Convert.ToDecimal(Actual) + PrevCumulativeActual) + "],");

                            strScript.Append("['" + ds.Tables[0].Rows[i]["MilestoneName"].ToString() + "'," + Plan + "," + Actual + "," + (Convert.ToDecimal(Plan) + PrevCumulativePlan) + "," + (Convert.ToDecimal(Actual) + PrevCumulativeActual) + "],");

                            //e.Row.Cells[3].Text = (Convert.ToDouble(TargetValue) + PreviousTarget).ToString();


                            PrevCumulativePlan = (Convert.ToDecimal(Plan) + PrevCumulativePlan);
                            //e.Row.Cells[4].Text = (Convert.ToDouble(AchievedValue) + PreviousActual).ToString();
                            PrevCumulativeActual = (Convert.ToDecimal(Actual) + PrevCumulativeActual);
                        }

                        // tablemonths += "<td style=\"padding:3px\">" + Convert.ToDateTime(ds.Rows[i]["WorkDate"].ToString()).ToString("dd-MMM-yy") + "</td>";

                        tablemonths += "<td style=\"padding:3px\">" + ds.Tables[0].Rows[i]["MileStoneName"].ToString() + "</td>";


                        tmonthlyplan += "<td style=\"padding:3px\">" + decimal.Round(Convert.ToDecimal(Plan), 2) + "</td>";
                        tmonthlyactual += "<td style=\"padding:3px\">" + decimal.Round(Convert.ToDecimal(Actual), 2) + "</td>";

                        tcumulativeplan += "<td style=\"padding:3px\">" + decimal.Round(PrevCumulativePlan, 2) + "</td>";
                        tcumulativeactual += "<td style=\"padding:3px\">" + decimal.Round(PrevCumulativeActual, 2) + "</td>";
                    }
                }
                strScript.Remove(strScript.Length - 1, 1);
                strScript.Append("]);");
                strScript.Append(@"var options = {
                        legend: { position: 'top' },
                        seriesType: 'bars',
                        series: { 2: { type: 'line',targetAxisIndex: 1 },3: { type: 'line',targetAxisIndex: 1 } },
                        hAxis: { title: 'Milestone',titleTextStyle: {
                        bold:'true',
                      }},
                      vAxes: {                        
          
                        0: {title: 'Milestone Plan',titleTextStyle: {
                    bold:'true',
                  }},
                        1: {title: 'Cumulative Plan',titleTextStyle: {
                    bold:'true',
                  }}
                      }
                };

                var chart = new google.visualization.ComboChart(document.getElementById('chart_div'));
                chart.draw(data, options);
                  }
                </script>");
                if (ShowTable)
                {
                    ltScript_Progress.Text = strScript.ToString();

                    DivActivityProgressTabular.InnerHtml = "<table border=\"1\" style=\"text-align:center;font-size:12px; width:100%; color:black; padding-left:10px;\">" +
                                 "<tr> " + tablemonths + "</tr>" +
                                  "<tr> " + tmonthlyplan + "</tr>" +
                                   "<tr> " + tmonthlyactual + "</tr>" +
                                    "<tr> " + tcumulativeplan + "</tr>" +
                                     "<tr> " + tcumulativeactual + "</tr>" +
                                         "</table>";
                }
                else
                {
                    ltScript_Progress.Text = "<h3>No data</h3>";
                    DivActivityProgressTabular.InnerHtml = "<h3>No data</h3>";
                }

            }
            else
            {
                ltScript_Progress.Text = "<h3>No data</h3>";
            }
        }

        protected void MileStoneGraph()
        {

            ActivityProgressReport.Visible = true;
            //LblActivityPhysicalProgress.Text = "Physical Progress for the Activity : " + DDLActivity.SelectedItem.Text;

            ActivityProgressReportName.InnerHtml = "Report Name : Physical Progress Monitoring for the Activity '" + DDLMilestones.SelectedItem.Text + "'";
            ActivityProgressProjectName.InnerHtml = "Project Name : " + DDlProject.SelectedItem.Text + " (" + DDLWorkPackage.SelectedItem.Text + ")";

            decimal PrevCumulativePlan = 0;
            decimal PrevCumulativeActual = 0;
            // DataSet ds = getdt.GetTaskSchedule_By_TaskUID(new Guid(DDLMilestones.SelectedValue));

            List<string> MonthDays = new List<string>() { "31", "28", "31", "30", "31", "30", "31", "31", "30", "31", "30", "31" };

            List<string> YearMonths = new List<string>() { "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec" };

            //  List<WorkSchedule> schedule_list = CreateWorkTable();

            DataSet ds = null;

            if (RBLReportFor.SelectedIndex == 3 || RBLReportFor.SelectedIndex == 4)
            {
                ds = getdt.GetMilestoneGraphValuesByMonth(DDLMilestones.SelectedValue);
            }
            else
            {
                ds = getdt.GetMilestoneGraphValues(DDLMilestones.SelectedValue);
            }

            string fromDateString = "";
            string toDateString = "";
            int fromMonth = 0;
            int toMonth = 0;

            DateTime fromDate = new DateTime();
            DateTime toDate = new DateTime();

            switch (RBLReportFor.SelectedValue)
            {

                case "By Week":
                    fromDateString = DDLWeek.SelectedValue.Split('-')[0];
                    string day = fromDateString.Split('/')[0];
                    string month = fromDateString.Split('/')[1];
                    string year = fromDateString.Split('/')[2];

                    fromDateString = month + "/" + day + "/" + year;
                    fromDate = Convert.ToDateTime(fromDateString);

                    toDateString = DDLWeek.SelectedValue.Split('-')[1];

                    day = toDateString.Split('/')[0];
                    month = toDateString.Split('/')[1];
                    year = toDateString.Split('/')[2];

                    toDateString = month + "/" + day + "/" + year;

                    toDate = Convert.ToDateTime(toDateString);

                    break;
                case "By Fortnightly":

                    fromDateString = DDLFortWeek.SelectedValue.Split('-')[0];
                    string day1 = fromDateString.Split('/')[0];
                    string month1 = fromDateString.Split('/')[1];
                    string year1 = fromDateString.Split('/')[2];

                    fromDateString = month1 + "/" + day1 + "/" + year1;
                    fromDate = Convert.ToDateTime(fromDateString);

                    toDateString = DDLFortWeek.SelectedValue.Split('-')[1];

                    day1 = toDateString.Split('/')[0];
                    month1 = toDateString.Split('/')[1];
                    year1 = toDateString.Split('/')[2];

                    toDateString = month1 + "/" + day1 + "/" + year1;

                    toDate = Convert.ToDateTime(toDateString);
                    break;
                case "By Month":
                    fromDateString = DDLMonth.SelectedValue + "/" + "01" + "/" + DDLYear.SelectedValue;
                    fromDate = Convert.ToDateTime(fromDateString);

                    toDateString = DDLMonth.SelectedValue + "/" + MonthDays[DDLMonth.SelectedIndex] + "/" + DDLYear.SelectedValue;
                    toDate = Convert.ToDateTime(toDateString);
                    break;
                case "By Quarter":
                    fromDateString = ddlQuarter.SelectedValue.Split('-')[0];
                    string day2 = fromDateString.Split('/')[0];
                    string month2 = fromDateString.Split('/')[1];
                    string year2 = fromDateString.Split('/')[2];

                    fromDateString = month2 + "/" + day2 + "/" + year2;
                    fromDate = Convert.ToDateTime(fromDateString);

                    fromMonth = fromDate.Month;

                    toDateString = ddlQuarter.SelectedValue.Split('-')[1];

                    day2 = toDateString.Split('/')[0];
                    month2 = toDateString.Split('/')[1];
                    year2 = toDateString.Split('/')[2];

                    toDateString = month2 + "/" + day2 + "/" + year2;

                    toDate = Convert.ToDateTime(toDateString);

                    toMonth = toDate.Month;
                    break;
                case "By HalfYear":
                    fromDateString = ddlHalfyearperiod.SelectedValue.Split('-')[0];
                    string day3 = fromDateString.Split('/')[0];
                    string month3 = fromDateString.Split('/')[1];
                    string year3 = fromDateString.Split('/')[2];

                    fromDateString = month3 + "/" + day3 + "/" + year3;
                    fromDate = Convert.ToDateTime(fromDateString);

                    fromMonth = fromDate.Month;

                    toDateString = ddlHalfyearperiod.SelectedValue.Split('-')[1];

                    day3 = toDateString.Split('/')[0];
                    month3 = toDateString.Split('/')[1];
                    year3 = toDateString.Split('/')[2];

                    toDateString = month3 + "/" + day3 + "/" + year3;

                    toDate = Convert.ToDateTime(toDateString);
                    toMonth = toDate.Month;

                    break;
            }

            //if (RBLReportFor.SelectedIndex == 3 || RBLReportFor.SelectedIndex == 4)
            //{
            //    var milestones = schedule_list.OrderBy(a => a.WorkDate).GroupBy(a => new {a.WorkDate.Year , a.WorkDate.Month }).Select(a => new MilestoneGraph {  Year = a.Key.Year.ToString(), Month = a.Key.Month.ToString(), PlannedValue = a.Sum(b => b.PlannedValue), AchievedValue = a.Sum(b => b.AchievedValue) }).ToList();
            //    ds = ToDataTable(milestones);
            //}
            //else
            //{
            //    var milestones = schedule_list.Where(a=>a.MileStoneId == DDLMilestones.SelectedValue).OrderBy(a => a.WorkDate).GroupBy(a => new {a.WorkDate, a.MileStoneId, a.MileStoneName }).Select(a => new MilestoneGraph { WorkDate= a.Key.WorkDate , MileStoneId = a.Key.MileStoneName, PlannedValue = a.Sum(b => b.PlannedValue), AchievedValue = a.Sum(b => b.AchievedValue) }).ToList();

            //    for(DateTime i = fromDate; i<=toDate; i=i.AddDays(1) )
            //    {
            //        if (milestones.ToList().Where(a=>a.WorkDate == i).Count() == 0)
            //        {
            //            milestones.Add(new MilestoneGraph() { MileStoneId = "Milestone", PlannedValue = 0, AchievedValue = 0, WorkDate = i, Month = i.Month.ToString(), Year = i.Year.ToString() });
            //        }
            //    }

            //    milestones = milestones.OrderBy(a => a.WorkDate).ToList();

            //    ds = ToDataTable(milestones);
            //}



            //fromDateString = DDLMonth.SelectedValue + "/" + "01" + "/" + DDLYear.SelectedValue;
            //fromDate = Convert.ToDateTime(fromDateString);

            //toDateString = DDLMonth.SelectedValue + "/" + MonthDays[DDLMonth.SelectedIndex] + "/" + DDLYear.SelectedValue;
            //toDate = Convert.ToDateTime(toDateString);





            if (ds.Tables[0].Rows.Count > 0)
            {
                btnActivityProgressPrint.Visible = true;
                bool ShowTable = false;
                string tablemonths = "<td style=\"width:200px\">&nbsp;</td>";
                string tmonthlyplan = "<td style=\"padding:3px\">Milestone Plan</td>";
                string tmonthlyactual = "<td style=\"padding:3px\">Milestone Actual</td>";
                string tcumulativeplan = "<td style=\"padding:3px\">Cumulative Plan</td>";
                string tcumulativeactual = "<td style=\"padding:3px\">Cumulative Actual</td>";

                System.Text.StringBuilder strScript = new System.Text.StringBuilder();
                strScript.Append(@"<script type='text/javascript'>
                    google.charts.load('current', { 'packages':['corechart']
                    });
                      if (typeof google.charts.visualization == 'undefined') {
                        google.charts.setOnLoadCallback(drawVisualization);
                    }
                    else {
                        drawVisualization();
                    }
                    function drawVisualization()
                    {
                        // Some raw data (not necessarily accurate)
                        var data = google.visualization.arrayToDataTable([
                          ['Milestone', 'Plan', 'Actual', 'Cumulative Plan', 'Cumulative Actual'],");
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    if (RBLReportFor.SelectedIndex == 3 || RBLReportFor.SelectedIndex == 4)
                    {
                        if (Convert.ToInt32(ds.Tables[0].Rows[i]["Year"].ToString()) >= fromDate.Year & Convert.ToInt32(ds.Tables[0].Rows[i]["Month"].ToString()) >= fromDate.Month & Convert.ToInt32(ds.Tables[0].Rows[i]["Year"].ToString()) <= toDate.Year & Convert.ToInt32(ds.Tables[0].Rows[i]["Month"].ToString()) <= toDate.Month)
                        {
                            ShowTable = true;
                            string Plan = ds.Tables[0].Rows[i]["PlannedValue"].ToString() != "" ? ds.Tables[0].Rows[i]["PlannedValue"].ToString() : "0";
                            string Actual = ds.Tables[0].Rows[i]["AchievedValue"].ToString() != "" ? ds.Tables[0].Rows[i]["AchievedValue"].ToString() : "0";
                            if (Plan != "" && Actual != "")
                            {
                                if (i == 0)
                                {
                                    PrevCumulativePlan = Convert.ToDecimal(Plan);
                                    PrevCumulativeActual = Convert.ToDecimal(Actual);
                                    // strScript.Append("['" + Convert.ToDateTime(ds.Rows[i]["WorkDate"].ToString()).ToString("dd-MMM-yy") + "'," + Plan + "," + Actual + "," + Plan + "," + Actual + "],");
                                    strScript.Append("['" + YearMonths[Convert.ToInt32(ds.Tables[0].Rows[i]["Month"].ToString()) - 1] + "'," + Plan + "," + Actual + "," + Plan + "," + Actual + "],");

                                }
                                else
                                {
                                    // strScript.Append("['" + Convert.ToDateTime(ds.Rows[i]["WorkDate"].ToString()).ToString("dd-MMM-yy") + "'," + Plan + "," + Actual + "," + (Convert.ToDecimal(Plan) + PrevCumulativePlan) + "," + (Convert.ToDecimal(Actual) + PrevCumulativeActual) + "],");

                                    strScript.Append("['" + YearMonths[Convert.ToInt32(ds.Tables[0].Rows[i]["Month"].ToString()) - 1] + "'," + Plan + "," + Actual + "," + (Convert.ToDecimal(Plan) + PrevCumulativePlan) + "," + (Convert.ToDecimal(Actual) + PrevCumulativeActual) + "],");

                                    //e.Row.Cells[3].Text = (Convert.ToDouble(TargetValue) + PreviousTarget).ToString();


                                    PrevCumulativePlan = (Convert.ToDecimal(Plan) + PrevCumulativePlan);
                                    //e.Row.Cells[4].Text = (Convert.ToDouble(AchievedValue) + PreviousActual).ToString();
                                    PrevCumulativeActual = (Convert.ToDecimal(Actual) + PrevCumulativeActual);
                                }

                                // tablemonths += "<td style=\"padding:3px\">" + Convert.ToDateTime(ds.Rows[i]["WorkDate"].ToString()).ToString("dd-MMM-yy") + "</td>";

                                tablemonths += "<td style=\"padding:3px\">" + ds.Tables[0].Rows[i]["Month"].ToString() + "</td>";


                                tmonthlyplan += "<td style=\"padding:3px\">" + decimal.Round(Convert.ToDecimal(Plan), 2) + "</td>";
                                tmonthlyactual += "<td style=\"padding:3px\">" + decimal.Round(Convert.ToDecimal(Actual), 2) + "</td>";

                                tcumulativeplan += "<td style=\"padding:3px\">" + decimal.Round(PrevCumulativePlan, 2) + "</td>";
                                tcumulativeactual += "<td style=\"padding:3px\">" + decimal.Round(PrevCumulativeActual, 2) + "</td>";
                            }
                        }
                    }
                    else
                    {
                        if (Convert.ToDateTime(ds.Tables[0].Rows[i]["WorkDate"].ToString()) >= fromDate & Convert.ToDateTime(ds.Tables[0].Rows[i]["WorkDate"].ToString()) <= toDate)
                        {
                            ShowTable = true;
                            string Plan = ds.Tables[0].Rows[i]["PlannedValue"].ToString() != "" ? ds.Tables[0].Rows[i]["PlannedValue"].ToString() : "0";
                            string Actual = ds.Tables[0].Rows[i]["AchievedValue"].ToString() != "" ? ds.Tables[0].Rows[i]["AchievedValue"].ToString() : "0";
                            if (Plan != "" && Actual != "")
                            {
                                if (i == 0)
                                {
                                    PrevCumulativePlan = Convert.ToDecimal(Plan);
                                    PrevCumulativeActual = Convert.ToDecimal(Actual);
                                    // strScript.Append("['" + Convert.ToDateTime(ds.Rows[i]["WorkDate"].ToString()).ToString("dd-MMM-yy") + "'," + Plan + "," + Actual + "," + Plan + "," + Actual + "],");
                                    strScript.Append("['" + Convert.ToDateTime(ds.Tables[0].Rows[i]["WorkDate"].ToString()).ToString("dd-MMM-yy") + "'," + Plan + "," + Actual + "," + Plan + "," + Actual + "],");

                                }
                                else
                                {
                                    // strScript.Append("['" + Convert.ToDateTime(ds.Rows[i]["WorkDate"].ToString()).ToString("dd-MMM-yy") + "'," + Plan + "," + Actual + "," + (Convert.ToDecimal(Plan) + PrevCumulativePlan) + "," + (Convert.ToDecimal(Actual) + PrevCumulativeActual) + "],");

                                    strScript.Append("['" + Convert.ToDateTime(ds.Tables[0].Rows[i]["WorkDate"].ToString()).ToString("dd-MMM-yy") + "'," + Plan + "," + Actual + "," + (Convert.ToDecimal(Plan) + PrevCumulativePlan) + "," + (Convert.ToDecimal(Actual) + PrevCumulativeActual) + "],");

                                    //e.Row.Cells[3].Text = (Convert.ToDouble(TargetValue) + PreviousTarget).ToString();


                                    PrevCumulativePlan = (Convert.ToDecimal(Plan) + PrevCumulativePlan);
                                    //e.Row.Cells[4].Text = (Convert.ToDouble(AchievedValue) + PreviousActual).ToString();
                                    PrevCumulativeActual = (Convert.ToDecimal(Actual) + PrevCumulativeActual);
                                }

                                // tablemonths += "<td style=\"padding:3px\">" + Convert.ToDateTime(ds.Rows[i]["WorkDate"].ToString()).ToString("dd-MMM-yy") + "</td>";

                                tablemonths += "<td style=\"padding:3px\">" + Convert.ToDateTime(ds.Tables[0].Rows[i]["WorkDate"].ToString()).ToString("dd-MMM-yy") + "</td>";


                                tmonthlyplan += "<td style=\"padding:3px\">" + decimal.Round(Convert.ToDecimal(Plan), 2) + "</td>";
                                tmonthlyactual += "<td style=\"padding:3px\">" + decimal.Round(Convert.ToDecimal(Actual), 2) + "</td>";

                                tcumulativeplan += "<td style=\"padding:3px\">" + decimal.Round(PrevCumulativePlan, 2) + "</td>";
                                tcumulativeactual += "<td style=\"padding:3px\">" + decimal.Round(PrevCumulativeActual, 2) + "</td>";
                            }


                        }
                    }


                }
                strScript.Remove(strScript.Length - 1, 1);
                strScript.Append("]);");
                strScript.Append(@"var options = {
                        legend: { position: 'top' },
                        seriesType: 'bars',
                        series: { 2: { type: 'line',targetAxisIndex: 1 },3: { type: 'line',targetAxisIndex: 1 } },
                        hAxis: { title: 'Milestone',titleTextStyle: {
                        bold:'true',
                      }},
                      vAxes: {                        
          
                        0: {title: 'Milestone Plan',titleTextStyle: {
                    bold:'true',
                  }},
                        1: {title: 'Cumulative Plan',titleTextStyle: {
                    bold:'true',
                  }}
                      }
                };

                var chart = new google.visualization.ComboChart(document.getElementById('chart_div'));
                chart.draw(data, options);
                  }
                </script>");
                if (ShowTable)
                {
                    ltScript_Progress.Text = strScript.ToString();

                    DivActivityProgressTabular.InnerHtml = "<table border=\"1\" style=\"text-align:center;font-size:12px; width:100%; color:black; padding-left:10px;\">" +
                                 "<tr> " + tablemonths + "</tr>" +
                                  "<tr> " + tmonthlyplan + "</tr>" +
                                   "<tr> " + tmonthlyactual + "</tr>" +
                                    "<tr> " + tcumulativeplan + "</tr>" +
                                     "<tr> " + tcumulativeactual + "</tr>" +
                                         "</table>";
                }
                else
                {
                    ltScript_Progress.Text = "<h3>No data</h3>";
                    DivActivityProgressTabular.InnerHtml = "<h3>No data</h3>";
                }

            }
            else
            {
                ltScript_Progress.Text = "<h3>No data</h3>";
            }
        }


        protected void ProgressGraph()
        {
            ActivityProgressReport.Visible = true;
            //LblActivityPhysicalProgress.Text = "Physical Progress for the Activity : " + DDLActivity.SelectedItem.Text;

            ActivityProgressReportName.InnerHtml = "Report Name : Physical Progress Monitoring for the Activity '" + DDLMilestones.SelectedItem.Text + "'";
            ActivityProgressProjectName.InnerHtml = "Project Name : " + DDlProject.SelectedItem.Text + " (" + DDLWorkPackage.SelectedItem.Text + ")";

            decimal PrevCumulativePlan = 0;
            decimal PrevCumulativeActual = 0;
            // DataSet ds = getdt.GetTaskSchedule_By_TaskUID(new Guid(DDLActivity.SelectedValue));

            List<string> MonthDays = new List<string>() { "31", "28", "31", "30", "31", "30", "31", "31", "30", "31", "30", "31" };

            List<WorkSchedule> schedule_list = CreateWorkTable();

            DataTable ds = null;

            //if (RBLReportFor.SelectedValue == "By Quarter")
            //{
            //    var milestones = schedule_list.OrderBy(a => a.WorkDate).GroupBy(a => new { a.WorkDate.Month }).Select(a => new Milestone {  MileStoneId = a.Key.Month.ToString(), PlannedValue = a.Sum(b => b.PlannedValue), AchievedValue = a.Sum(b => b.AchievedValue) }).ToList();

            //}

            schedule_list = schedule_list.Where(a => a.MileStoneId == DDLMilestones.SelectedValue).ToList();

            ds = ToDataTable(schedule_list);

            string fromDateString = "";
            string toDateString = "";


            DateTime fromDate = new DateTime();
            DateTime toDate = new DateTime();

            switch (RBLReportFor.SelectedValue)
            {

                case "By Week":
                    fromDateString = DDLWeek.SelectedValue.Split('-')[0];
                    string day = fromDateString.Split('/')[0];
                    string month = fromDateString.Split('/')[1];
                    string year = fromDateString.Split('/')[2];

                    fromDateString = month + "/" + day + "/" + year;
                    fromDate = Convert.ToDateTime(fromDateString);

                    toDateString = DDLWeek.SelectedValue.Split('-')[1];

                    day = toDateString.Split('/')[0];
                    month = toDateString.Split('/')[1];
                    year = toDateString.Split('/')[2];

                    toDateString = month + "/" + day + "/" + year;

                    toDate = Convert.ToDateTime(toDateString);

                    break;
                case "By Fortnightly":

                    fromDateString = DDLFortWeek.SelectedValue.Split('-')[0];
                    string day1 = fromDateString.Split('/')[0];
                    string month1 = fromDateString.Split('/')[1];
                    string year1 = fromDateString.Split('/')[2];

                    fromDateString = month1 + "/" + day1 + "/" + year1;
                    fromDate = Convert.ToDateTime(fromDateString);

                    toDateString = DDLFortWeek.SelectedValue.Split('-')[1];

                    day1 = toDateString.Split('/')[0];
                    month1 = toDateString.Split('/')[1];
                    year1 = toDateString.Split('/')[2];

                    toDateString = month1 + "/" + day1 + "/" + year1;

                    toDate = Convert.ToDateTime(toDateString);
                    break;
                case "By Month":
                    fromDateString = DDLMonth.SelectedValue + "/" + "01" + "/" + DDLYear.SelectedValue;
                    fromDate = Convert.ToDateTime(fromDateString);

                    toDateString = DDLMonth.SelectedValue + "/" + MonthDays[DDLMonth.SelectedIndex] + "/" + DDLYear.SelectedValue;
                    toDate = Convert.ToDateTime(toDateString);
                    break;
                case "By Quarter":
                    fromDateString = ddlQuarter.SelectedValue.Split('-')[0];
                    string day2 = fromDateString.Split('/')[0];
                    string month2 = fromDateString.Split('/')[1];
                    string year2 = fromDateString.Split('/')[2];

                    fromDateString = month2 + "/" + day2 + "/" + year2;
                    fromDate = Convert.ToDateTime(fromDateString);

                    toDateString = ddlQuarter.SelectedValue.Split('-')[1];

                    day2 = toDateString.Split('/')[0];
                    month2 = toDateString.Split('/')[1];
                    year2 = toDateString.Split('/')[2];

                    toDateString = month2 + "/" + day2 + "/" + year2;

                    toDate = Convert.ToDateTime(toDateString);
                    break;
                case "By HalfYear":
                    fromDateString = ddlHalfyearperiod.SelectedValue.Split('-')[0];
                    string day3 = fromDateString.Split('/')[0];
                    string month3 = fromDateString.Split('/')[1];
                    string year3 = fromDateString.Split('/')[2];

                    fromDateString = month3 + "/" + day3 + "/" + year3;
                    fromDate = Convert.ToDateTime(fromDateString);

                    toDateString = ddlHalfyearperiod.SelectedValue.Split('-')[1];

                    day3 = toDateString.Split('/')[0];
                    month3 = toDateString.Split('/')[1];
                    year3 = toDateString.Split('/')[2];

                    toDateString = month3 + "/" + day3 + "/" + year3;

                    toDate = Convert.ToDateTime(toDateString);
                    break;
            }

            if (ds.Rows.Count > 0)
            {
                btnActivityProgressPrint.Visible = true;
                bool ShowTable = false;
                string tablemonths = "<td style=\"width:200px\">&nbsp;</td>";
                string tmonthlyplan = "<td style=\"padding:3px\">Monthly Plan</td>";
                string tmonthlyactual = "<td style=\"padding:3px\">Monthly Actual</td>";
                string tcumulativeplan = "<td style=\"padding:3px\">Cumulative Plan</td>";
                string tcumulativeactual = "<td style=\"padding:3px\">Cumulative Actual</td>";

                System.Text.StringBuilder strScript = new System.Text.StringBuilder();
                strScript.Append(@"<script type='text/javascript'>
                    google.charts.load('current', { 'packages':['corechart']
                    });
                      if (typeof google.charts.visualization == 'undefined') {
                        google.charts.setOnLoadCallback(drawVisualization);
                    }
                    else {
                        drawVisualization();
                    }
                    function drawVisualization()
                    {
                        // Some raw data (not necessarily accurate)
                        var data = google.visualization.arrayToDataTable([
                          ['Month', 'Plan', 'Actual', 'Cumulative Plan', 'Cumulative Actual'],");
                for (int i = 0; i < ds.Rows.Count; i++)
                {
                    if (Convert.ToDateTime(ds.Rows[i]["WorkDate"].ToString()) >= fromDate & Convert.ToDateTime(ds.Rows[i]["WorkDate"].ToString()) <= toDate)
                    {
                        ShowTable = true;
                        string Plan = ds.Rows[i]["PlannedValue"].ToString() != "" ? ds.Rows[i]["PlannedValue"].ToString() : "0";
                        string Actual = ds.Rows[i]["AchievedValue"].ToString() != "" ? ds.Rows[i]["AchievedValue"].ToString() : "0";
                        if (Plan != "" && Actual != "")
                        {
                            if (i == 0)
                            {
                                PrevCumulativePlan = Convert.ToDecimal(Plan);
                                PrevCumulativeActual = Convert.ToDecimal(Actual);
                                strScript.Append("['" + Convert.ToDateTime(ds.Rows[i]["WorkDate"].ToString()).ToString("dd-MMM-yy") + "'," + Plan + "," + Actual + "," + Plan + "," + Actual + "],");
                            }
                            else
                            {
                                strScript.Append("['" + Convert.ToDateTime(ds.Rows[i]["WorkDate"].ToString()).ToString("dd-MMM-yy") + "'," + Plan + "," + Actual + "," + (Convert.ToDecimal(Plan) + PrevCumulativePlan) + "," + (Convert.ToDecimal(Actual) + PrevCumulativeActual) + "],");
                                //e.Row.Cells[3].Text = (Convert.ToDouble(TargetValue) + PreviousTarget).ToString();
                                PrevCumulativePlan = (Convert.ToDecimal(Plan) + PrevCumulativePlan);
                                //e.Row.Cells[4].Text = (Convert.ToDouble(AchievedValue) + PreviousActual).ToString();
                                PrevCumulativeActual = (Convert.ToDecimal(Actual) + PrevCumulativeActual);
                            }

                            tablemonths += "<td style=\"padding:3px\">" + Convert.ToDateTime(ds.Rows[i]["WorkDate"].ToString()).ToString("dd-MMM-yy") + "</td>";
                            tmonthlyplan += "<td style=\"padding:3px\">" + decimal.Round(Convert.ToDecimal(Plan), 2) + "</td>";
                            tmonthlyactual += "<td style=\"padding:3px\">" + decimal.Round(Convert.ToDecimal(Actual), 2) + "</td>";

                            tcumulativeplan += "<td style=\"padding:3px\">" + decimal.Round(PrevCumulativePlan, 2) + "</td>";
                            tcumulativeactual += "<td style=\"padding:3px\">" + decimal.Round(PrevCumulativeActual, 2) + "</td>";
                        }


                    }
                }
                strScript.Remove(strScript.Length - 1, 1);
                strScript.Append("]);");
                strScript.Append(@"var options = {
                        legend: { position: 'top' },
                        seriesType: 'bars',
                        series: { 2: { type: 'line',targetAxisIndex: 1 },3: { type: 'line',targetAxisIndex: 1 } },
                        hAxis: { title: 'Month',titleTextStyle: {
                        bold:'true',
                      }},
                      vAxes: {                        
          
                        0: {title: 'Monthly Plan',titleTextStyle: {
                    bold:'true',
                  }},
                        1: {title: 'Cumulative Plan',titleTextStyle: {
                    bold:'true',
                  }}
                      }
                };

                var chart = new google.visualization.ComboChart(document.getElementById('chart_div'));
                chart.draw(data, options);
                  }
                </script>");
                if (ShowTable)
                {
                    ltScript_Progress.Text = strScript.ToString();

                    DivActivityProgressTabular.InnerHtml = "<table border=\"1\" style=\"text-align:center;font-size:12px; width:100%; color:black; padding-left:10px;\">" +
                                 "<tr> " + tablemonths + "</tr>" +
                                  "<tr> " + tmonthlyplan + "</tr>" +
                                   "<tr> " + tmonthlyactual + "</tr>" +
                                    "<tr> " + tcumulativeplan + "</tr>" +
                                     "<tr> " + tcumulativeactual + "</tr>" +
                                         "</table>";
                }
                else
                {
                    ltScript_Progress.Text = "<h3>No data</h3>";
                    DivActivityProgressTabular.InnerHtml = "<h3>No data</h3>";
                }

            }
            else
            {
                ltScript_Progress.Text = "<h3>No data</h3>";
            }
        }

        protected List<WorkSchedule> CreateWorkTable()
        {
            List<WorkSchedule> WorkScheduleList = new List<WorkSchedule>()
            {
                new WorkSchedule(){ MileStoneId ="Milestone A", MileStoneName="Mobilization", TaskId="AA", StartDate=Convert.ToDateTime("01/01/2023") , EndDate = Convert.ToDateTime("12/31/2023") ,  WorkDate= Convert.ToDateTime("04/01/2023"), PlannedValue=20, AchievedValue=9},
                new WorkSchedule(){ MileStoneId ="Milestone A", MileStoneName="Mobilization", TaskId="AB", StartDate=Convert.ToDateTime("01/01/2023") , EndDate = Convert.ToDateTime("12/31/2023") ,  WorkDate= Convert.ToDateTime("04/02/2023"), PlannedValue=30, AchievedValue=10},
                new WorkSchedule(){ MileStoneId ="Milestone A", MileStoneName="Mobilization", TaskId="AC", StartDate=Convert.ToDateTime("01/01/2023") , EndDate = Convert.ToDateTime("12/31/2023") ,  WorkDate= Convert.ToDateTime("04/03/2023"), PlannedValue=10, AchievedValue=9},
                new WorkSchedule(){ MileStoneId ="Milestone A", MileStoneName="Mobilization", TaskId="AD", StartDate=Convert.ToDateTime("01/01/2023") , EndDate = Convert.ToDateTime("12/31/2023") ,  WorkDate= Convert.ToDateTime("04/04/2023"), PlannedValue=30, AchievedValue=10},
                new WorkSchedule(){ MileStoneId ="Milestone A", MileStoneName="Mobilization", TaskId="AE", StartDate=Convert.ToDateTime("01/01/2023") , EndDate = Convert.ToDateTime("12/31/2023") ,  WorkDate= Convert.ToDateTime("04/05/2023"), PlannedValue=10, AchievedValue=11},
                new WorkSchedule(){ MileStoneId ="Milestone B", MileStoneName="Mobilize Construction Equipment", TaskId="BA", StartDate=Convert.ToDateTime("01/01/2023") , EndDate = Convert.ToDateTime("12/31/2023") ,  WorkDate= Convert.ToDateTime("04/06/2023"), PlannedValue=30, AchievedValue=15},
                new WorkSchedule(){ MileStoneId ="Milestone B", MileStoneName="Mobilize Construction Equipment", TaskId="BB", StartDate=Convert.ToDateTime("01/01/2023") , EndDate = Convert.ToDateTime("12/31/2023") ,  WorkDate= Convert.ToDateTime("04/07/2023"), PlannedValue=10, AchievedValue=18},
                new WorkSchedule(){ MileStoneId ="Milestone B", MileStoneName="Mobilize Construction Equipment", TaskId="BC", StartDate=Convert.ToDateTime("01/01/2023") , EndDate = Convert.ToDateTime("12/31/2023") ,  WorkDate= Convert.ToDateTime("04/08/2023"), PlannedValue=20, AchievedValue=9},
                new WorkSchedule(){ MileStoneId ="Milestone B", MileStoneName="Mobilize Construction Equipment", TaskId="BD", StartDate=Convert.ToDateTime("01/01/2023") , EndDate = Convert.ToDateTime("12/31/2023") ,  WorkDate= Convert.ToDateTime("04/09/2023"), PlannedValue=20, AchievedValue=8},
                new WorkSchedule(){ MileStoneId ="Milestone B", MileStoneName="Mobilize Construction Equipment", TaskId="BE", StartDate=Convert.ToDateTime("01/01/2023") , EndDate = Convert.ToDateTime("12/31/2023") ,  WorkDate= Convert.ToDateTime("04/10/2023"), PlannedValue=20, AchievedValue=9},
                new WorkSchedule(){ MileStoneId ="Milestone C", MileStoneName="Submission of design and drawings", TaskId="CA", StartDate=Convert.ToDateTime("01/01/2023") , EndDate = Convert.ToDateTime("12/31/2023") ,  WorkDate= Convert.ToDateTime("04/11/2023"), PlannedValue=20, AchievedValue=9},
                new WorkSchedule(){ MileStoneId ="Milestone C", MileStoneName="Submission of design and drawings", TaskId="CB", StartDate=Convert.ToDateTime("01/01/2023") , EndDate = Convert.ToDateTime("12/31/2023") ,  WorkDate= Convert.ToDateTime("04/12/2023"), PlannedValue=15, AchievedValue=12},
                new WorkSchedule(){ MileStoneId ="Milestone C", MileStoneName="Submission of design and drawings", TaskId="CC", StartDate=Convert.ToDateTime("01/01/2023") , EndDate = Convert.ToDateTime("12/31/2023") ,  WorkDate= Convert.ToDateTime("04/13/2023"), PlannedValue=25, AchievedValue=13},
                new WorkSchedule(){ MileStoneId ="Milestone C", MileStoneName="Submission of design and drawings", TaskId="CD", StartDate=Convert.ToDateTime("01/01/2023") , EndDate = Convert.ToDateTime("12/31/2023") ,  WorkDate= Convert.ToDateTime("04/14/2023"), PlannedValue=20, AchievedValue=9},
                new WorkSchedule(){ MileStoneId ="Milestone C", MileStoneName="Submission of design and drawings", TaskId="CE", StartDate=Convert.ToDateTime("01/01/2023") , EndDate = Convert.ToDateTime("12/31/2023") ,  WorkDate= Convert.ToDateTime("04/15/2023"), PlannedValue=25, AchievedValue=10},
                new WorkSchedule(){ MileStoneId ="Milestone D", MileStoneName="Place orders for plant and equipments", TaskId="AD", StartDate=Convert.ToDateTime("01/01/2023") , EndDate = Convert.ToDateTime("12/31/2023") ,  WorkDate= Convert.ToDateTime("04/16/2023"), PlannedValue=30, AchievedValue=9},
                new WorkSchedule(){ MileStoneId ="Milestone D", MileStoneName="Place orders for plant and equipments", TaskId="AD", StartDate=Convert.ToDateTime("01/01/2023") , EndDate = Convert.ToDateTime("12/31/2023") ,  WorkDate= Convert.ToDateTime("04/17/2023"), PlannedValue=20, AchievedValue=5},
                new WorkSchedule(){ MileStoneId ="Milestone D", MileStoneName="Place orders for plant and equipments", TaskId="AD", StartDate=Convert.ToDateTime("01/01/2023") , EndDate = Convert.ToDateTime("12/31/2023") ,  WorkDate= Convert.ToDateTime("04/18/2023"), PlannedValue=10, AchievedValue=10},
                new WorkSchedule(){ MileStoneId ="Milestone D", MileStoneName="Place orders for plant and equipments", TaskId="AD", StartDate=Convert.ToDateTime("01/01/2023") , EndDate = Convert.ToDateTime("12/31/2023") ,  WorkDate= Convert.ToDateTime("04/19/2023"), PlannedValue=20, AchievedValue=9},
                new WorkSchedule(){ MileStoneId ="Milestone D", MileStoneName="Place orders for plant and equipments", TaskId="AD", StartDate=Convert.ToDateTime("01/01/2023") , EndDate = Convert.ToDateTime("12/31/2023") ,  WorkDate= Convert.ToDateTime("04/20/2023"), PlannedValue=20, AchievedValue=9},
                new WorkSchedule(){ MileStoneId ="Milestone E", MileStoneName="Civil Works", TaskId="AE", StartDate=Convert.ToDateTime("01/01/2023") , EndDate = Convert.ToDateTime("12/31/2023") ,  WorkDate= Convert.ToDateTime("04/21/2023"), PlannedValue=20, AchievedValue=9},
                new WorkSchedule(){ MileStoneId ="Milestone E", MileStoneName="Civil Works", TaskId="AE", StartDate=Convert.ToDateTime("01/01/2023") , EndDate = Convert.ToDateTime("12/31/2023") ,  WorkDate= Convert.ToDateTime("04/22/2023"), PlannedValue=30, AchievedValue=8},
                new WorkSchedule(){ MileStoneId ="Milestone E", MileStoneName="Civil Works", TaskId="AE", StartDate=Convert.ToDateTime("01/01/2023") , EndDate = Convert.ToDateTime("12/31/2023") ,  WorkDate= Convert.ToDateTime("04/23/2023"), PlannedValue=10, AchievedValue=12},
                new WorkSchedule(){ MileStoneId ="Milestone E", MileStoneName="Civil Works", TaskId="AE", StartDate=Convert.ToDateTime("01/01/2023") , EndDate = Convert.ToDateTime("12/31/2023") ,  WorkDate= Convert.ToDateTime("04/24/2023"), PlannedValue=20, AchievedValue=8},
                new WorkSchedule(){ MileStoneId ="Milestone E", MileStoneName="Civil Works", TaskId="AE", StartDate=Convert.ToDateTime("01/01/2023") , EndDate = Convert.ToDateTime("12/31/2023") ,  WorkDate= Convert.ToDateTime("04/25/2023"), PlannedValue=20, AchievedValue=9},
                new WorkSchedule(){ MileStoneId ="Milestone F", MileStoneName="Receipt of Plant and Equipments", TaskId="AA", StartDate=Convert.ToDateTime("01/01/2023") , EndDate = Convert.ToDateTime("12/31/2023") ,  WorkDate= Convert.ToDateTime("05/01/2023"), PlannedValue=10, AchievedValue=9},
                new WorkSchedule(){ MileStoneId ="Milestone F", MileStoneName="Receipt of Plant and Equipments", TaskId="AA", StartDate=Convert.ToDateTime("01/01/2023") , EndDate = Convert.ToDateTime("12/31/2023") ,  WorkDate= Convert.ToDateTime("05/02/2023"), PlannedValue=20, AchievedValue=10},
                new WorkSchedule(){ MileStoneId ="Milestone F", MileStoneName="Receipt of Plant and Equipments", TaskId="AA", StartDate=Convert.ToDateTime("01/01/2023") , EndDate = Convert.ToDateTime("12/31/2023") ,  WorkDate= Convert.ToDateTime("05/03/2023"), PlannedValue=25, AchievedValue=9},
                new WorkSchedule(){ MileStoneId ="Milestone F", MileStoneName="Receipt of Plant and Equipments", TaskId="AA", StartDate=Convert.ToDateTime("01/01/2023") , EndDate = Convert.ToDateTime("12/31/2023") ,  WorkDate= Convert.ToDateTime("05/04/2023"), PlannedValue=15, AchievedValue=10},
                new WorkSchedule(){ MileStoneId ="Milestone F", MileStoneName="Receipt of Plant and Equipments", TaskId="AA", StartDate=Convert.ToDateTime("01/01/2023") , EndDate = Convert.ToDateTime("12/31/2023") ,  WorkDate= Convert.ToDateTime("05/05/2023"), PlannedValue=30, AchievedValue=11},
                new WorkSchedule(){ MileStoneId ="Milestone G", MileStoneName="Trial run and commissioning", TaskId="AB", StartDate=Convert.ToDateTime("01/01/2023") , EndDate = Convert.ToDateTime("12/31/2023") ,  WorkDate= Convert.ToDateTime("05/06/2023"), PlannedValue=20, AchievedValue=15},
                new WorkSchedule(){ MileStoneId ="Milestone G", MileStoneName="Trial run and commissioning", TaskId="AB", StartDate=Convert.ToDateTime("01/01/2023") , EndDate = Convert.ToDateTime("12/31/2023") ,  WorkDate= Convert.ToDateTime("05/07/2023"), PlannedValue=10, AchievedValue=18},
                new WorkSchedule(){ MileStoneId ="Milestone G", MileStoneName="Trial run and commissioning", TaskId="AB", StartDate=Convert.ToDateTime("01/01/2023") , EndDate = Convert.ToDateTime("12/31/2023") ,  WorkDate= Convert.ToDateTime("05/08/2023"), PlannedValue=20, AchievedValue=9},
                new WorkSchedule(){ MileStoneId ="Milestone G", MileStoneName="Trial run and commissioning", TaskId="AB", StartDate=Convert.ToDateTime("01/01/2023") , EndDate = Convert.ToDateTime("12/31/2023") ,  WorkDate= Convert.ToDateTime("05/09/2023"), PlannedValue=25, AchievedValue=8},
                new WorkSchedule(){ MileStoneId ="Milestone G", MileStoneName="Trial run and commissioning", TaskId="AB", StartDate=Convert.ToDateTime("01/01/2023") , EndDate = Convert.ToDateTime("12/31/2023") ,  WorkDate= Convert.ToDateTime("05/10/2023"), PlannedValue=25, AchievedValue=9},
                new WorkSchedule(){ MileStoneId ="Milestone H", MileStoneName="Power Supply Systems", TaskId="AC", StartDate=Convert.ToDateTime("01/01/2023") , EndDate = Convert.ToDateTime("12/31/2023") ,  WorkDate= Convert.ToDateTime("05/11/2023"), PlannedValue=20, AchievedValue=9},
                new WorkSchedule(){ MileStoneId ="Milestone H", MileStoneName="Power Supply Systems", TaskId="AC", StartDate=Convert.ToDateTime("01/01/2023") , EndDate = Convert.ToDateTime("12/31/2023") ,  WorkDate= Convert.ToDateTime("05/12/2023"), PlannedValue=15, AchievedValue=12},
                new WorkSchedule(){ MileStoneId ="Milestone H", MileStoneName="Power Supply Systems", TaskId="AC", StartDate=Convert.ToDateTime("01/01/2023") , EndDate = Convert.ToDateTime("12/31/2023") ,  WorkDate= Convert.ToDateTime("05/12/2023"), PlannedValue=25, AchievedValue=12},
                new WorkSchedule(){ MileStoneId ="Milestone H", MileStoneName="Power Supply Systems", TaskId="AC", StartDate=Convert.ToDateTime("01/01/2023") , EndDate = Convert.ToDateTime("12/31/2023") ,  WorkDate= Convert.ToDateTime("05/12/2023"), PlannedValue=20, AchievedValue=12},
                new WorkSchedule(){ MileStoneId ="Milestone H", MileStoneName="Power Supply Systems", TaskId="AC", StartDate=Convert.ToDateTime("01/01/2023") , EndDate = Convert.ToDateTime("12/31/2023") ,  WorkDate= Convert.ToDateTime("05/13/2023"), PlannedValue=20, AchievedValue=13},
                new WorkSchedule(){ MileStoneId ="Milestone I", MileStoneName="Plant road construction", TaskId="AC", StartDate=Convert.ToDateTime("01/01/2023") , EndDate = Convert.ToDateTime("12/31/2023") ,  WorkDate= Convert.ToDateTime("05/15/2023"), PlannedValue=10, AchievedValue=10},
                new WorkSchedule(){ MileStoneId ="Milestone I", MileStoneName="Plant road construction", TaskId="AD", StartDate=Convert.ToDateTime("01/01/2023") , EndDate = Convert.ToDateTime("12/31/2023") ,  WorkDate= Convert.ToDateTime("05/16/2023"), PlannedValue=20, AchievedValue=9},
                new WorkSchedule(){ MileStoneId ="Milestone I", MileStoneName="Plant road construction", TaskId="AD", StartDate=Convert.ToDateTime("01/01/2023") , EndDate = Convert.ToDateTime("12/31/2023") ,  WorkDate= Convert.ToDateTime("05/17/2023"), PlannedValue=30, AchievedValue=5},
                new WorkSchedule(){ MileStoneId ="Milestone I", MileStoneName="Plant road construction", TaskId="AD", StartDate=Convert.ToDateTime("01/01/2023") , EndDate = Convert.ToDateTime("12/31/2023") ,  WorkDate= Convert.ToDateTime("05/18/2023"), PlannedValue=15, AchievedValue=10},
                new WorkSchedule(){ MileStoneId ="Milestone I", MileStoneName="Plant road construction", TaskId="AD", StartDate=Convert.ToDateTime("01/01/2023") , EndDate = Convert.ToDateTime("12/31/2023") ,  WorkDate= Convert.ToDateTime("05/19/2023"), PlannedValue=25, AchievedValue=9},
                new WorkSchedule(){ MileStoneId ="Milestone J", MileStoneName="Painting,Landscape", TaskId="AE", StartDate=Convert.ToDateTime("01/01/2023") , EndDate = Convert.ToDateTime("12/31/2023") ,  WorkDate= Convert.ToDateTime("05/21/2023"), PlannedValue=15, AchievedValue=9},
                new WorkSchedule(){ MileStoneId ="Milestone J", MileStoneName="Painting,Landscape", TaskId="AE", StartDate=Convert.ToDateTime("01/01/2023") , EndDate = Convert.ToDateTime("12/31/2023") ,  WorkDate= Convert.ToDateTime("05/22/2023"), PlannedValue=20, AchievedValue=8},
                new WorkSchedule(){ MileStoneId ="Milestone J", MileStoneName="Painting,Landscape", TaskId="AE", StartDate=Convert.ToDateTime("01/01/2023") , EndDate = Convert.ToDateTime("12/31/2023") ,  WorkDate= Convert.ToDateTime("05/23/2023"), PlannedValue=25, AchievedValue=12},
                new WorkSchedule(){ MileStoneId ="Milestone J", MileStoneName="Painting,Landscape", TaskId="AE", StartDate=Convert.ToDateTime("01/01/2023") , EndDate = Convert.ToDateTime("12/31/2023") ,  WorkDate= Convert.ToDateTime("05/24/2023"), PlannedValue=15, AchievedValue=8},
                new WorkSchedule(){ MileStoneId ="Milestone J", MileStoneName="Painting,Landscape", TaskId="AE", StartDate=Convert.ToDateTime("01/01/2023") , EndDate = Convert.ToDateTime("12/31/2023") ,  WorkDate= Convert.ToDateTime("05/25/2023"), PlannedValue=25, AchievedValue=9},
                new WorkSchedule(){ MileStoneId ="Milestone K", MileStoneName="Demolition of Existing Structures", TaskId="AE", StartDate=Convert.ToDateTime("01/01/2023") , EndDate = Convert.ToDateTime("12/31/2023") ,  WorkDate= Convert.ToDateTime("05/21/2023"), PlannedValue=15, AchievedValue=9},
                new WorkSchedule(){ MileStoneId ="Milestone K", MileStoneName="Demolition of Existing Structures", TaskId="AE", StartDate=Convert.ToDateTime("01/01/2023") , EndDate = Convert.ToDateTime("12/31/2023") ,  WorkDate= Convert.ToDateTime("05/22/2023"), PlannedValue=25, AchievedValue=8},
                new WorkSchedule(){ MileStoneId ="Milestone K", MileStoneName="Demolition of Existing Structures", TaskId="AE", StartDate=Convert.ToDateTime("01/01/2023") , EndDate = Convert.ToDateTime("12/31/2023") ,  WorkDate= Convert.ToDateTime("05/23/2023"), PlannedValue=30, AchievedValue=12},
                new WorkSchedule(){ MileStoneId ="Milestone K", MileStoneName="Demolition of Existing Structures", TaskId="AE", StartDate=Convert.ToDateTime("01/01/2023") , EndDate = Convert.ToDateTime("12/31/2023") ,  WorkDate= Convert.ToDateTime("05/24/2023"), PlannedValue=20, AchievedValue=8},
                new WorkSchedule(){ MileStoneId ="Milestone K", MileStoneName="Demolition of Existing Structures", TaskId="AE", StartDate=Convert.ToDateTime("01/01/2023") , EndDate = Convert.ToDateTime("12/31/2023") ,  WorkDate= Convert.ToDateTime("05/25/2023"), PlannedValue=10, AchievedValue=9},

            };

            return WorkScheduleList;
        }

        protected static DataTable ToDataTable<T>(List<T> items)
        {
            DataTable dataTable = new DataTable(typeof(T).Name);

            //Get all the properties
            PropertyInfo[] Props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (PropertyInfo prop in Props)
            {
                //Defining type of data column gives proper data table 
                var type = (prop.PropertyType.IsGenericType && prop.PropertyType.GetGenericTypeDefinition() == typeof(Nullable<>) ? Nullable.GetUnderlyingType(prop.PropertyType) : prop.PropertyType);
                //Setting column names as Property names
                dataTable.Columns.Add(prop.Name, type);
            }
            foreach (T item in items)
            {
                var values = new object[Props.Length];
                for (int i = 0; i < Props.Length; i++)
                {
                    //inserting property values to datatable rows
                    values[i] = Props[i].GetValue(item, null);
                }
                dataTable.Rows.Add(values);
            }
            //put a breakpoint here and check datatable
            return dataTable;
        }

        protected void DDLMileStones_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (DDLMilestones.SelectedIndex > 0)
            {
                RBLReportFor.Visible = true;
                MonthlyPhysicalProgress.Visible = false;
                QuarterPhysicalProgress.Visible = false;
                HalfYearPhysicalProgress.Visible = false;
                ByHalfYear.Visible = false;
                ByMonth.Visible = false;
                ByWeek.Visible = false;
                ByQuarteMonth.Visible = false;
                Byfortingiht.Visible = false;
                WeeklyProgressReport.Visible = false;
                AcrossMonthsProgressReport.Visible = false;
                FortiProgressReport.Visible = false;
                ByActivity.Visible = false;
                ActivityProgressReport.Visible = false;
                AllMilestoneGraph.Visible = false;
                RBLReportFor.Items[0].Selected = false;
                RBLReportFor.Items[1].Selected = false;
                RBLReportFor.Items[2].Selected = false;
                RBLReportFor.Items[3].Selected = false;
                RBLReportFor.Items[4].Selected = false;
                RBLReportFor.Items[5].Selected = false;
                RBLReportFor.Items[6].Selected = false;
            }
            else
            {
                MonthlyPhysicalProgress.Visible = false;
                QuarterPhysicalProgress.Visible = false;
                HalfYearPhysicalProgress.Visible = false;
                ByHalfYear.Visible = false;
                ByMonth.Visible = false;
                ByWeek.Visible = false;
                ByQuarteMonth.Visible = false;
                Byfortingiht.Visible = false;
                WeeklyProgressReport.Visible = false;
                AcrossMonthsProgressReport.Visible = false;
                FortiProgressReport.Visible = false;
                ByActivity.Visible = false;
                ActivityProgressReport.Visible = false;
                AllMilestoneGraph.Visible = true;
                RBLReportFor.Visible = false;
            }

        }

        protected void btnGraphSubmit_Click(object sender, EventArgs e)
        {
            if (DDLMilestones.Enabled)
            {
                if (DDLMilestones.SelectedIndex == 0 )
                    AllMileStoneGraph();
            }
        }

    }
}