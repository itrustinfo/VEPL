using ProjectManager.DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ProjectManagementTool._content_pages.assign_current_next_flowstepstatus
{
    public partial class _default : System.Web.UI.Page
    {
        DBGetData getdt = new DBGetData();
        TaskUpdate gettk = new TaskUpdate();
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

                    BindFlow();
                    BindUserType();
                }
            }
        }

        private void BindUserType()
        {
            DataSet ds = getdt.getAllUsers_RolesforFlowStatus();
            ChkUserType.DataTextField = "UserRole_Desc";
            ChkUserType.DataValueField = "UserRole_Name";
            ChkUserType.DataSource = ds;
            ChkUserType.DataBind();
        }


        void BindFlow()
        {
            DataTable ds = getdt.GetDocumentFlow();
            if (ds != null && ds.Rows.Count > 0)
            {
                DDLFlow.DataTextField = "Flow_Name";
                DDLFlow.DataValueField = "FlowMasterUID";
                DDLFlow.DataSource = ds;
                DDLFlow.DataBind();
                DDLFlow.Items.Insert(0, "--Select--");
                ViewState["Flow"] = ds;
            }
        }

        protected void DDlUserType_SelectedIndexChanged(object sender, EventArgs e)
        {


        }

        protected void DDLCurrentStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadUserTypeSelected();

        }
        protected void DDLNextStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadUserTypeSelected();
        }

        protected void DDLFlow_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(DDLFlow.SelectedValue!= "--Select--")
            {
                BindSteps();
                DDLFlowStep.Items.Insert(0, "--Select--");
                BindAllFlowStatus();
            }

        }
        protected void DDLFlowStep_SelectedIndexChanged(object sender, EventArgs e)
        {
            GrdCurrentNextStatus.Visible = true;
            BindGrid();
            BindAllFlowStatus();
            //update status DDL
            DataTable dtupdate = getdt.getAllUpdateStatusForFlowStep(new Guid(DDLFlow.SelectedValue), Convert.ToInt32(DDLFlowStep.SelectedValue));

            DDLNextStatus.DataSource = dtupdate;
            DDLNextStatus.DataTextField = "Status";
            DDLNextStatus.DataValueField = "FlowStepStatusUID";
            DDLNextStatus.DataBind();
            DDLNextStatus.Items.Insert(0, "--Select--");
            LoadUserTypeSelected();

        }




        protected void btnSubmit_Click(object sender, EventArgs e)
        {

            if (DDLFlow.SelectedValue == "--Select--")
            {
                Response.Write("<script>alert('Please select a Flow');</script>");
                return;
            }
            else if (DDLFlowStep.SelectedValue == "--Select--")
            {
                Response.Write("<script>alert('Please select a Flow Step');</script>");
                return;
            }
            else if (DDLCurrentStatus.SelectedValue == "--Select--")
            {
                Response.Write("<script>alert('Please select a Current Status');</script>");
                return;
            }
            else if (DDLNextStatus.SelectedValue == "--Select--")
            {
                Response.Write("<script>alert('Please select a Update Status');</script>");
                return;
            }
            else
            {
                List<ListItem> selectedItems = ChkUserType.Items.Cast<ListItem>()
                                   .Where(li => li.Selected)
                                   .ToList();
                if (selectedItems.Count == 0)
                {
                    Response.Write("<script>alert('Please check atleast one UserType');</script>");
                    return;
                }
                string Current_Status = DDLCurrentStatus.SelectedItem.Text;
                string Update_Status = DDLNextStatus.SelectedItem.Text;
                //var selectedUserTypeUID = selectedItems.Select(r => new Guid(r.Value)).ToList();
                var selectedUserType = selectedItems.Select(r => (r.Value)).ToList();

                // string Step_Name = DDLFlowStep.SelectedValue.ToString();


                DataTable dtAddedUserTypes = getdt.FlowUserType_Select(new Guid(DDLFlow.SelectedValue), Convert.ToInt32(DDLFlowStep.SelectedValue), Current_Status, Update_Status);


                var toBeDeleted = dtAddedUserTypes.AsEnumerable().Where(r => !selectedUserType.Contains(r.Field<string>("UserType")));
                DataTable dtToBeDeleted = new DataTable();
                if (toBeDeleted.FirstOrDefault() != null)
                {
                    dtToBeDeleted = toBeDeleted.CopyToDataTable();
                }
                foreach (var eachSelectedItems in selectedItems)
                {
                    //string UserType = eachSelectedItems.Text;
                    //DataTable dtUserType = getdt.getUserTypeByUID(new Guid (eachSelectedItems.Value));
                    //foreach (DataRow row in dtUserType.Rows) {
                    //    string UserType = row["UserRole_Name"].ToString();
                    int cnt = getdt.UserTypeStatusForFlows_InsertUpdate(new Guid(DDLFlow.SelectedValue), Convert.ToInt32(DDLFlowStep.SelectedValue),
                    Current_Status, Update_Status,  eachSelectedItems.Value);
                    //}

                }
                if (dtToBeDeleted.Rows.Count > 0)
                {
                    foreach (DataRow eachRow in dtToBeDeleted.Rows)
                    {
                        int cnt = getdt.FlowUserType_Delete(eachRow.Field<Guid>("UID"));
                    }
                }
            
            Response.Write("<script>alert('User flow saved successfully.');</script>");
             BindGrid();
            LoadUserTypeSelected();
        }
       }

        private void LoadUserTypeSelected()
        {
            int steps = 0;
            int.TryParse(DDLFlowStep.SelectedValue, out steps);

            string Current_Status = DDLCurrentStatus.SelectedItem.Text;
            string Update_Status = DDLNextStatus.SelectedItem.Text;
            DataTable dtUserType = new DataTable();

            if (DDLFlow.SelectedValue != "--Select--" && DDLFlowStep.SelectedValue != "--Select--")
            {
                dtUserType = getdt.FlowUserType_Select(new Guid(DDLFlow.SelectedValue), Convert.ToInt32(DDLFlowStep.SelectedValue), Current_Status, Update_Status);

            }

            //if (dtUserType != null && dtUserType.Rows.Count > 0)
            //{
                foreach (ListItem listItem in ChkUserType.Items)
                {
                    listItem.Selected = false;

                    //if (dtUserType.AsEnumerable().Where(r => r.Field<Guid>("UserRole_ID") == new Guid(listItem.Value)).FirstOrDefault() != null)
                    if (dtUserType.AsEnumerable().Where(r => r.Field<string>("UserType") ==  listItem.Value).FirstOrDefault() != null)

                    {
                        listItem.Selected = true;
                    }
                }
            //}
            
        }



        private void BindSteps()
        {


            DDLFlowStep.Items.Clear();
            int totalSteps = 0;
            if (DDLFlow.SelectedValue != "--Select--")
            {
                DataTable dt = (DataTable)ViewState["Flow"];
                totalSteps = dt.AsEnumerable().Where(r => r.Field<Guid>("FlowMasterUID") == new Guid(DDLFlow.SelectedValue)).Select(r => r.Field<int>("Steps_Count")).FirstOrDefault();
                //DataTable dtbl = getdt.getFlowStepsforUserType(new Guid(DDLFlow.SelectedValue),  DDlUserType.SelectedValue);

                for (int counter = 1; counter <= totalSteps; counter++)
                {
                    string colName = "FlowStep" + counter.ToString() + "_DisplayName";
                    var val = dt.AsEnumerable().Where(r => r.Field<Guid>("FlowMasterUID") == new Guid(DDLFlow.SelectedValue))
                        .Select(r => r.Field<string>(colName)).FirstOrDefault();
                    //int step_no = Convert.ToInt32(val);
                    //foreach (DataRow row in dtbl.Rows)
                    //{
                        //if(dtbl.Rows[0].)

                            DDLFlowStep.Items.Add(new ListItem { Value = counter.ToString(), Text = val });
                        
                    

                }

            }



        }

        private void BindAllFlowStatus()
        {
            DataTable dsb = new DataTable();
            if (DDLFlow.SelectedValue != "--Select--")
            {

                //current status DDL
                //DataTable dsb = getdt.getAllStatusForFlow(new Guid(DDLFlow.SelectedValue), Convert.ToInt32(DDLFlowStep.SelectedValue));
                int flowStepValue;
                if (int.TryParse(DDLFlowStep.SelectedValue, out flowStepValue))
                {
                    // When Flow step in greater than 1
                    if (flowStepValue > 1)
                    {
                         dsb = getdt.getAllCurrentStatusForFlowStep(new Guid(DDLFlow.SelectedValue), flowStepValue - 1);
                        
                        DDLCurrentStatus.DataSource = dsb;
                        DDLCurrentStatus.DataTextField = "Update_Status";
                        DDLCurrentStatus.DataValueField = "Update_Status";
                        DDLCurrentStatus.DataBind();
                        DDLCurrentStatus.Items.Insert(0, "--Select--");
                    }
                    // When Flow step is 1
                    else
                    {
                        
                        dsb = getdt.getAllCurrentStatusForFirstFlowStep(new Guid(DDLFlow.SelectedValue), Convert.ToInt32(DDLFlowStep.SelectedValue));
                        DDLCurrentStatus.DataSource = dsb;
                        DDLCurrentStatus.DataTextField = "Status";
                        DDLCurrentStatus.DataValueField = "FlowStepStatusUID";
                        DDLCurrentStatus.DataBind();
                        DDLCurrentStatus.Items.Insert(0, "--Select--");
                    }
                }
                else
                {
                    // Handle the case where the value cannot be converted to an integer
                    // You can log an error or handle it based on your application's logic
                }

            }
        }

        private void BindGrid()
        {
            DataTable dt = getdt.getExistingCurrentUpdatedStatus(new Guid(DDLFlow.SelectedValue), Convert.ToInt32(DDLFlowStep.SelectedValue));
            GrdCurrentNextStatus.DataSource = dt;
            GrdCurrentNextStatus.DataBind();

        }

    }

}