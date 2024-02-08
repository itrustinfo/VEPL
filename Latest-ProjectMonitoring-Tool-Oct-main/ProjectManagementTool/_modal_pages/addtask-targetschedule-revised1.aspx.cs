using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ProjectManagementTool.Models;
using ProjectManager.DAL;

namespace ProjectManagementTool._modal_pages
{
    public partial class addtask_targetschedule_revised1 : System.Web.UI.Page
    {
        DBGetData getdata = new DBGetData();

        List<Week> DDLWeek = new List<Week>();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["Username"] == null)
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>parent.location.href=parent.location.href;</script>");
            }
            else
            {
                if (!IsPostBack)
                {
                    DataSet ds = getdata.GetTaskScheduleTypeByTaskUID(new Guid(Request.QueryString["TaskUID"]));

                    if (ds.Tables[0].Rows.Count >0)
                    {
                        string scheduleType = ds.Tables[0].Rows[0].ItemArray[0].ToString();

                        if (scheduleType == "Month")
                        {
                            BindTaskSchedule();
                            RBScheduleTye.Items[1].Enabled = false;

                            RBScheduleTye.SelectedIndex = 0;
                            RBScheduleType_Changed(RBScheduleTye, e);
                        }
                        else if (scheduleType == "Week")
                        {
                            BindWeeklyTaskSchedule();
                            RBScheduleTye.Items[0].Enabled = false;
                            RBScheduleTye.SelectedIndex = 1;
                            RBScheduleType_Changed(RBScheduleTye, e);
                        }
                        else
                        {
                            RBScheduleTye.Items[0].Enabled = false;
                            RBScheduleTye.Items[1].Enabled = false;

                            btnAddNew.Visible = false;
                            btnAddNew1.Visible = false;
                            btnCancel.Visible = false;
                            btnCancel1.Visible = false;
                            btnSaveNew.Visible = false;
                            btnSaveNew1.Visible = false;
                            btnUpdate.Visible = false;


                            ScriptManager.RegisterStartupScript(this, this.GetType(), "Warning", "alert('Set Schedule Type in Task and Do schedule !!');", true);
                            return;
                        }
                    }
                    
                }
            }
        }


        private void BindYear(DropDownList DDLYear)
        {
            DDLYear.Items.Clear();
            int year = DateTime.Now.Year - 5;
            DDLYear.Items.Add(new ListItem("--Select Year--", ""));
            for (int i = 1; i < 10; i++)
            {
                year = year + 1;
                DDLYear.Items.Add(new ListItem(year.ToString(), year.ToString()));
            }
        }


        protected void BindTaskSchedule()
        {
            DataSet ds1 = getdata.GetLatest_TaskScheduleVesrion_By_TaskUID(new Guid(Request.QueryString["TaskUID"]));

            if (ds1.Tables[0].Rows.Count > 0)
            {
                float version = float.Parse(ds1.Tables[0].Rows[0]["TaskScheduleVersion"].ToString());

                Session["version"] = version;
                HiddenAction.Value = "Update";

                GridView1.DataSource = null;
                GridView1.DataBind();
                DataSet ds = getdata.GetTaskSchedule_By_TaskUID_TaskScheduleVersion1(new Guid(Request.QueryString["TaskUID"]), version);
                GridView1.DataSource = ds;
                GridView1.DataBind();
            }
            else
            {
                HiddenAction.Value = "Add";
                Session["version"] = null;
            }
        }

        protected void OnRowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                //Find the DropDownList in the Row.
                DropDownList ddlMonths = (e.Row.FindControl("ddlMonths") as DropDownList);
                ddlMonths.CssClass = "form-control";

                //Add Default Item in the DropDownList.
                ddlMonths.Items.Insert(0, new ListItem("Please select", "00"));
                ddlMonths.Items.Insert(1, new ListItem("Jan", "01"));
                ddlMonths.Items.Insert(2, new ListItem("Feb", "02"));
                ddlMonths.Items.Insert(3, new ListItem("Mar", "03"));
                ddlMonths.Items.Insert(4, new ListItem("Apr", "04"));
                ddlMonths.Items.Insert(5, new ListItem("May", "05"));
                ddlMonths.Items.Insert(6, new ListItem("Jun", "06"));
                ddlMonths.Items.Insert(7, new ListItem("Jul", "07"));
                ddlMonths.Items.Insert(8, new ListItem("Aug", "08"));
                ddlMonths.Items.Insert(9, new ListItem("Sep", "09"));
                ddlMonths.Items.Insert(10, new ListItem("Oct", "10"));
                ddlMonths.Items.Insert(11, new ListItem("Nov", "11"));
                ddlMonths.Items.Insert(12, new ListItem("Dec", "12"));

                //Select the Country of Customer in DropDownList.
                string month = (e.Row.FindControl("lblMonth") as Label).Text;
                ddlMonths.Items.FindByText(month).Selected = true;


                DropDownList ddlYears = (e.Row.FindControl("ddlYears") as DropDownList);
                ddlYears.CssClass = "form-control";

                //Add Default Item in the DropDownList.
                ddlYears.Items.Insert(0, new ListItem("Please select", "0000"));
                ddlYears.Items.Insert(1, new ListItem("2020", "2020"));
                ddlYears.Items.Insert(2, new ListItem("2021", "2021"));
                ddlYears.Items.Insert(3, new ListItem("2022", "2022"));
                ddlYears.Items.Insert(4, new ListItem("2023", "2023"));
                ddlYears.Items.Insert(5, new ListItem("2024", "2024"));
                ddlYears.Items.Insert(6, new ListItem("2025", "2025"));
                ddlYears.Items.Insert(7, new ListItem("2026", "2026"));

                //Select the Country of Customer in DropDownList.
                string year = (e.Row.FindControl("lblYear") as Label).Text;
                ddlYears.Items.FindByText(year).Selected = true;

                TextBox txtPlanned = (e.Row.FindControl("txtScheduleValue") as TextBox);
                txtPlanned.CssClass = "form-control";


                string planned = (e.Row.FindControl("lblScheduleValue") as Label).Text;
                txtPlanned.Text = planned;
            }
        }

        protected void GrdView1_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            //Page.ClientScript.RegisterStartupScript(Page.GetType(), "Warning", "<script language='javascript'>alert('It can not be deleted as there exists data.');</script>");

        }

        protected void OnRowCommand(object sender, GridViewCommandEventArgs e)
        {
            string UID = e.CommandArgument.ToString();

            if (e.CommandName == "delete")
            {
                int cnt = getdata.TaskSchedule_Delete_by_TaskScheduleUID(new Guid(UID));

                if (cnt > 0)
                {
                    BindTaskSchedule();
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Warning", "alert('This cannot be deleted. contact system administrator');", true);

                }
            }
        }

        protected void btnAddNew_Click(object sender, EventArgs e)
        {
            if (Session["version"] == null)
            {
                Session["version"] = 0;
            }

            float version = float.Parse(Session["version"].ToString());

            if (version == 0)
            {
                bool result = getdata.InsertorUpdateTaskScheduleVesrion(Guid.NewGuid(), new Guid(Request.QueryString["TaskUID"]), "Month");
                if (result)
                {
                    version = 1;
                    Session["version"] = 1;
                }

            }

            DataSet ds = getdata.GetTaskSchedule_By_TaskUID_TaskScheduleVersion1(new Guid(Request.QueryString["TaskUID"]), version);

            ds.Tables[0].Rows.Add(Guid.NewGuid(), "Jan", "2020", 0);

            GridView1.DataSource = ds;
            GridView1.DataBind();

            HiddenAction.Value = "Save";
            btnAddNew.Enabled = false;
            btnCancel.Enabled = true;
            btnSaveNew.Enabled = true;
           // btnUpdate.Enabled = false;
        }



        protected void btnCancel_Click(object sender, EventArgs e)
        {
            BindTaskSchedule();

            btnAddNew.Enabled = true;
            btnCancel.Enabled = false;
            btnSaveNew.Enabled = false;
           // btnUpdate.Enabled = true;
        }

        protected void btnSaveNew_Click(object sender, EventArgs e)
        {
            try
            {

                GridViewRow NewRow = GridView1.Rows[GridView1.Rows.Count - 1];

                string ID = NewRow.Cells[0].Text;

                DropDownList ddlMonths = (NewRow.FindControl("ddlMonths") as DropDownList);

                int month = Convert.ToInt32(ddlMonths.SelectedValue);

                DropDownList ddlYears = (NewRow.FindControl("ddlYears") as DropDownList);

                int year = Convert.ToInt32(ddlYears.SelectedValue);

                TextBox txtSchedule = (NewRow.FindControl("txtScheduleValue") as TextBox);

                float ScheduledValue = Convert.ToSingle(txtSchedule.Text);

                if (getdata.CheckTaskScheduleQuanity(new Guid(Request.QueryString["TaskUID"]), ScheduledValue) == 0)
                {
                    DateTime startDate = new DateTime(year, month, 1);
                    DateTime endDate = startDate.AddDays(DateTime.DaysInMonth(startDate.Year, startDate.Month));

                    getdata.InsertorUpdateTaskSchedule(Guid.NewGuid(), new Guid(Request.QueryString["WorkUID"]), new Guid(Request.QueryString["TaskUID"]), startDate, endDate, ScheduledValue, "Month");

                    BindTaskSchedule();

                    btnAddNew.Enabled = true;
                    btnCancel.Enabled = false;
                    btnSaveNew.Enabled = false;
                }
                else
                {
                    //ScriptManager.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>alert('Total schedule quanity has exceeded the Total Quanity  allowed !');</script>");
                    ScriptManager.RegisterStartupScript(this, GetType(), "showalert", "alert('Total schedule quantity has exceeded the Total Quantity allowed !');", true);
                    return;
                }
            }
            catch (Exception ex)
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>alert('Error Code ATS-01 there is a problem with this feature. Please contact system admin. Description : " + ex.Message + "');</script>");
            }

        }

        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                // string taskuid = Request.QueryString["TaskUID"];

                if (RBScheduleTye.SelectedIndex == 1)
                {
                    if (GridView2.Rows.Count == 0)
                    {
                        Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>parent.location.href=parent.location.href;</script>");
                    }
                }

                string confirmValue = "";

                //added by zuber to check the schedule quanity should not exceed the total quantity
                //-------------------------------
                DataTable dttask = getdata.GetTaskDetails_TaskUID(Request.QueryString["TaskUID"].ToString());
                float TotalQuantity = 0;
                if (dttask.Rows.Count > 0)
                {
                    if (dttask.Rows[0]["UnitQuantity"] != DBNull.Value)
                    {
                        TotalQuantity = float.Parse(dttask.Rows[0]["UnitQuantity"].ToString());
                    }
                }

                float tScheduledValue = 0;

                if (RBScheduleTye.SelectedIndex == 0)
                {
                    foreach (GridViewRow grdrow in GridView1.Rows)
                    {
                        TextBox txtSchedule = (grdrow.FindControl("txtScheduleValue") as TextBox);
                        tScheduledValue = tScheduledValue + Convert.ToSingle(txtSchedule.Text);

                    }
                }
                else if (RBScheduleTye.SelectedIndex == 1){

                    foreach (GridViewRow grdrow in GridView2.Rows)
                    {
                        TextBox txtSchedule = (grdrow.FindControl("txtScheduleValue") as TextBox);
                        tScheduledValue = tScheduledValue + Convert.ToSingle(txtSchedule.Text);

                    }
                }
               
                if (tScheduledValue > TotalQuantity)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "showalert", "alert('Total schedule quantity has exceeded the Total Quantity allowed !');", true);
                    return;
                }
                //-------------------------------
                if (HiddenAction.Value == "Update")
                {
                    confirmValue = Request.Form["confirm_value"];

                    Boolean ver = false;

                    if (confirmValue == "Yes")
                    {
                        if (RBScheduleTye.SelectedIndex == 0)
                           ver = getdata.InsertorUpdateTaskScheduleVesrion(Guid.NewGuid(), new Guid(Request.QueryString["TaskUID"]), "Month");
                        else
                            ver = getdata.InsertorUpdateTaskScheduleVesrion(Guid.NewGuid(), new Guid(Request.QueryString["TaskUID"]), "Week");
                    }
                }

                DataSet ds1 = getdata.GetLatest_TaskScheduleVesrion_By_TaskUID(new Guid(Request.QueryString["TaskUID"]));

                if (ds1.Tables[0].Rows.Count > 0)
                {
                    float version = float.Parse(ds1.Tables[0].Rows[0]["TaskScheduleVersion"].ToString());
                }

                if (RBScheduleTye.SelectedIndex == 0)
                {
                    foreach (GridViewRow grdrow in GridView1.Rows)
                    {
                        string ID = grdrow.Cells[0].Text;

                        if (string.IsNullOrEmpty(ID))
                            ID = Guid.NewGuid().ToString();

                        DropDownList ddlMonths = (grdrow.FindControl("ddlMonths") as DropDownList);
                        int month = Convert.ToInt32(ddlMonths.SelectedValue);

                        DropDownList ddlYears = (grdrow.FindControl("ddlYears") as DropDownList);
                        int year = Convert.ToInt32(ddlYears.SelectedValue);

                        TextBox txtSchedule = (grdrow.FindControl("txtScheduleValue") as TextBox);
                        float ScheduledValue = Convert.ToSingle(txtSchedule.Text);

                        DateTime startDate = new DateTime(year, month, 1);
                        DateTime endDate = startDate.AddDays(DateTime.DaysInMonth(startDate.Year, startDate.Month));

                        if (confirmValue == "Yes")
                        {
                            getdata.InsertorUpdateTaskSchedule(Guid.NewGuid(), new Guid(Request.QueryString["WorkUID"]), new Guid(Request.QueryString["TaskUID"]), startDate, endDate, ScheduledValue, "Month");
                        }
                        else
                        {
                            getdata.InsertorUpdateTaskSchedule(new Guid(ID), new Guid(Request.QueryString["WorkUID"]), new Guid(Request.QueryString["TaskUID"]), startDate, endDate, ScheduledValue, "Month");
                        }
                    }
                }
                else if (RBScheduleTye.SelectedIndex == 1)
                {
                    foreach (GridViewRow grdrow in GridView2.Rows)
                    {
                        string ID = grdrow.Cells[0].Text;

                        if (string.IsNullOrEmpty(ID))
                            ID = Guid.NewGuid().ToString();

                        TextBox ddlStart = (grdrow.FindControl("txtDate1") as TextBox);

                        DateTime startDate = Convert.ToDateTime(getdata.ConvertDateFormat(ddlStart.Text));

                        TextBox ddlEnd = (grdrow.FindControl("txtDate2") as TextBox);

                        DateTime endDate = Convert.ToDateTime(getdata.ConvertDateFormat(ddlEnd.Text)); ;

                        TextBox txtSchedule = (grdrow.FindControl("txtScheduleValue") as TextBox);
                        float ScheduledValue = Convert.ToSingle(txtSchedule.Text);

                       
                        if (confirmValue == "Yes")
                        {
                            getdata.InsertorUpdateTaskSchedule(Guid.NewGuid(), new Guid(Request.QueryString["WorkUID"]), new Guid(Request.QueryString["TaskUID"]), startDate, endDate, ScheduledValue, "Week");
                        }
                        else
                        {
                            getdata.InsertorUpdateTaskSchedule(new Guid(ID), new Guid(Request.QueryString["WorkUID"]), new Guid(Request.QueryString["TaskUID"]), startDate, endDate, ScheduledValue, "Week");
                        }
                    }
                }

                

                Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>parent.location.href=parent.location.href;</script>");
            }
            catch (Exception ex)
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>alert('Error Code ATS-01 there is a problem with this feature. Please contact system admin. Description : " + ex.Message + "');</script>");
            }
        }

        protected void TxtScheduleValue_Changed(object sender, EventArgs e)
        {
            // ScriptManager.RegisterStartupScript(this.Page, this.GetType(), "script", "EnableSaveButton();", true);

        }

        protected void RBScheduleType_Changed(object sender, EventArgs e)
        {
            if (RBScheduleTye.SelectedIndex == 1)
            {
                DDLYear.Visible = true;
                DDLMonth.Visible = true;

                GridView1.Visible = false;
                btnAddNew.Visible = false;
                btnCancel.Visible = false;
                btnSaveNew.Visible = false;

                GridView2.Visible = true;

                btnAddNew1.Visible = false;
                btnCancel1.Visible = false;
                btnSaveNew1.Visible = false;

               // btnUpdate.Enabled = false;
            }
            else
            {
                DDLYear.Visible = false;
                DDLMonth.Visible = false;

                GridView1.Visible = true;
                btnAddNew.Visible = true;
                btnCancel.Visible = true;
                btnSaveNew.Visible = true;

                GridView2.Visible = false;
                btnAddNew1.Visible = false;
                btnSaveNew1.Visible = false;
                btnSaveNew1.Visible = false;

            }
        }

        protected void DDLMonth_selectedIndexChanged(object sender, EventArgs e)
        {
            if (DDLMonth.SelectedIndex > 0)
            {
                SetWeeks();
                BindWeeklyTaskSchedule();

                btnAddNew1.Visible = true;
                btnCancel1.Visible = true;
                btnSaveNew1.Visible = true;
                btnAddNew1.Enabled = true;

                if (GridView2.Rows.Count > 0)
                    HiddenAction.Value = "Update";
                else
                    HiddenAction.Value = "Add";

                //  btnUpdate.Enabled = true;

            }
                

        }

        protected void SetWeeks()
        {
            DataSet ds = getdata.GetWorkPackages_By_WorkPackageUID(new Guid(Request.QueryString["WorkUID"]));

            if (ds.Tables[0].Rows.Count > 0)
            {
                DateTime startDate = DateTime.Now;

                if (Convert.ToDateTime(ds.Tables[0].Rows[0]["StartDate"].ToString()).Year.ToString() == DDLYear.SelectedValue.ToString() && Convert.ToDateTime(ds.Tables[0].Rows[0]["StartDate"].ToString()).Month.ToString() == DDLMonth.SelectedValue.ToString())
                {
                    startDate = Convert.ToDateTime(ds.Tables[0].Rows[0]["StartDate"].ToString());
                }
                else
                {
                    startDate = new DateTime(Convert.ToInt32(DDLYear.SelectedValue.ToString()), DDLMonth.SelectedIndex, 1);
                }

                int noOfDays = DateTime.DaysInMonth(startDate.Year, startDate.Month);
                int month = startDate.Month;


                DDLWeek.Clear();
                // DDLWeek.Items.Add(new System.Web.UI.WebControls.ListItem("-- Select Week --", ""));

                int days_to_add = 6;
                DateTime lastday = new DateTime();

                for (int x = 0; x < (noOfDays / 7) + 1; x++)
                {
                    lastday = startDate.AddDays(days_to_add);

                    if (lastday.Month == month)
                    {
                        // DDLWeek.Items.Add(new ListItem(startDate.ToString("dd/MM/yyyy") + " - " + startDate.AddDays(days_to_add).ToString("dd/MM/yyyy"), startDate.ToString("dd/MM/yyyy") + " - " + startDate.AddDays(days_to_add).ToString("dd/MM/yyyy")));

                        DDLWeek.Add(new Week() { startFrom = startDate, endAt = startDate.AddDays(days_to_add) });
                    }
                    else
                    {
                        lastday = lastday.AddDays(-days_to_add);
                        days_to_add = noOfDays - lastday.Day;

                        if (days_to_add <=6)
                            DDLWeek.Add(new Week() { startFrom = startDate, endAt = startDate.AddDays(days_to_add) });
                    }

                    startDate = startDate.AddDays(7);

                }
            }
        }

        protected void BindWeeklyTaskSchedule()
        {
            DataSet ds1 = getdata.GetLatest_TaskScheduleVesrion_By_TaskUID(new Guid(Request.QueryString["TaskUID"]));

            if (ds1.Tables[0].Rows.Count > 0)
            {
                float version = float.Parse(ds1.Tables[0].Rows[0]["TaskScheduleVersion"].ToString());

                Session["version"] = version;
                HiddenAction.Value = "Update";

                GridView2.DataSource = null;
                GridView2.DataBind();
                DataSet ds = getdata.GetTaskSchedule_By_TaskUID_ByWeek_TaskScheduleVersion1(new Guid(Request.QueryString["TaskUID"]), DDLMonth.SelectedIndex, Convert.ToInt32(DDLYear.SelectedValue), version);
                                               
                GridView2.DataSource = ds;
                GridView2.DataBind();
                GridView2.Visible = true;
            }
            else
            {
                HiddenAction.Value = "Add";
            }
        }

        protected void GrdView2_OnRowCommand(object sender, GridViewCommandEventArgs e)
        {
            string UID = e.CommandArgument.ToString();

            if (e.CommandName == "delete")
            {
                int cnt = getdata.TaskSchedule_Delete_by_TaskScheduleUID(new Guid(UID));

                if (cnt > 0)
                {
                    BindWeeklyTaskSchedule();
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Warning", "alert('This cannot be deleted. contact system administrator');", true);

                }
            }
        }

        protected void GrdView2_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            //Page.ClientScript.RegisterStartupScript(Page.GetType(), "Warning", "<script language='javascript'>alert('It can not be deleted as there exists data.');</script>");

        }

        protected void GridView2_OnRowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {

                Label  lblDate1 = (e.Row.FindControl("lblDate1") as Label);

                TextBox txtDate1 = (e.Row.FindControl("txtDate1") as TextBox);
                txtDate1.CssClass = "form-control";

                DateTime tDate1 = Convert.ToDateTime(lblDate1.Text);

                txtDate1.Text = tDate1.Day.ToString() + "/" + tDate1.Month.ToString() + "/" + tDate1.Year.ToString();

                Label lblDate2 = (e.Row.FindControl("lblDate2") as Label);

                TextBox txtDate2 = (e.Row.FindControl("txtDate2") as TextBox);
                txtDate2.CssClass = "form-control";

                DateTime  tDate2  = Convert.ToDateTime(lblDate2.Text);

                txtDate2.Text = tDate2.Day.ToString() + "/" + tDate2.Month.ToString() + "/" + tDate2.Year.ToString();

                Label lblScheduleValue = (e.Row.FindControl("lblScheduleValue") as Label);

                TextBox txtScheduleValue = (e.Row.FindControl("txtScheduleValue") as TextBox);
                txtDate1.CssClass = "form-control";

                txtScheduleValue.Text = lblScheduleValue.Text;
            }
        }

        protected void btnAddNew1_Click(object sender, EventArgs e)
        {
            if (Session["version"] == null)
            {
                Session["version"] = 0;
            }

            float version = float.Parse(Session["version"].ToString());

            if (version == 0)
            {
                bool result = getdata.InsertorUpdateTaskScheduleVesrion(Guid.NewGuid(), new Guid(Request.QueryString["TaskUID"]), "Month");
                if (result)
                {
                    version = 1;
                    Session["version"] = 1;
                }

            }

            DataSet ds = getdata.GetTaskSchedule_By_TaskUID_ByWeek_TaskScheduleVersion1(new Guid(Request.QueryString["TaskUID"]), DDLMonth.SelectedIndex, Convert.ToInt32(DDLYear.SelectedValue), version);

            int ds_cnt = ds.Tables[0].Rows.Count;

            SetWeeks();

            if (ds_cnt < DDLWeek.Count)
            {
                ds.Tables[0].Rows.Add(Guid.NewGuid(), DDLWeek[ds_cnt].startFrom, DDLWeek[ds_cnt].endAt, 0);
                HiddenAction.Value = "Save";
                btnAddNew1.Enabled = false;
                btnCancel1.Enabled = true;
                btnSaveNew1.Enabled = true;
              //  btnUpdate.Enabled = false;
            }
                
            else
            {
                btnAddNew1.Enabled = false;
                btnCancel1.Enabled = false;
                btnSaveNew1.Enabled = false;
            }

            GridView2.DataSource = ds;
            GridView2.DataBind();
        }



        protected void btnCancel1_Click(object sender, EventArgs e)
        {
            BindWeeklyTaskSchedule();

            btnAddNew1.Enabled = true;
            btnCancel1.Enabled = false;
            btnSaveNew1.Enabled = false;
           // btnUpdate.Enabled = true;
        }

        protected void btnSaveNew1_Click(object sender, EventArgs e)
        {
            try
            {

                GridViewRow NewRow = GridView2.Rows[GridView2.Rows.Count - 1];

                string ID = NewRow.Cells[0].Text;

                TextBox  ddlStart = (NewRow.FindControl("txtDate1") as TextBox);

                DateTime  startDate = Convert.ToDateTime(getdata.ConvertDateFormat(ddlStart.Text));

                TextBox  ddlEnd = (NewRow.FindControl("txtDate2") as TextBox);

                DateTime endDate = Convert.ToDateTime(getdata.ConvertDateFormat(ddlEnd.Text)); ;

                TextBox txtSchedule = (NewRow.FindControl("txtScheduleValue") as TextBox);

                float ScheduledValue = Convert.ToSingle(txtSchedule.Text);

                if (getdata.CheckTaskScheduleQuanity(new Guid(Request.QueryString["TaskUID"]), ScheduledValue) == 0)
                {
                   // DateTime startDate = new DateTime(startDate.Year , month, 1);
                   // DateTime endDate = startDate.AddDays(DateTime.DaysInMonth(startDate.Year, startDate.Month));

                    getdata.InsertorUpdateTaskSchedule(Guid.NewGuid(), new Guid(Request.QueryString["WorkUID"]), new Guid(Request.QueryString["TaskUID"]), startDate, endDate, ScheduledValue, "Week");

                    BindWeeklyTaskSchedule();

                    btnAddNew1.Enabled = true;
                    btnCancel1.Enabled = false;
                    btnSaveNew1.Enabled = false;
                   // btnUpdate.Enabled = true;
                }
                else
                {
                    //ScriptManager.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>alert('Total schedule quanity has exceeded the Total Quanity  allowed !');</script>");
                    ScriptManager.RegisterStartupScript(this, GetType(), "showalert", "alert('Total schedule quantity has exceeded the Total Quantity allowed !');", true);
                    return;
                }
            }
            catch (Exception ex)
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>alert('Error Code ATS-01 there is a problem with this feature. Please contact system admin. Description : " + ex.Message + "');</script>");
            }

        }

       
    }
}