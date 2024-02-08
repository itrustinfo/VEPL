using ProjectManager.DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ProjectManagementTool._content_pages.assign_status_to_flowsteps
{
    public partial class _default : System.Web.UI.Page
    {
        DBGetData getData = new DBGetData();
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

                    //DataTable ds = getData.getAllUserFlows();

                    //DDlFlow.DataSource = ds;
                    //DDlFlow.DataValueField = "FlowMasterUID";
                    //DDlFlow.DataTextField = "Flow_Name";
                    //DDlFlow.DataBind();
                    //DDlFlow.Items.Insert(0, "--Select--");
                    //ViewState["Flow"] = ds;
                    BindFlow();
                }
            }
        }

        protected void DDlFlow_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (DDlFlow.SelectedValue != "--Select--")
            {
                DDLFlowStep.Visible = true;
                //DataTable dst = getData.getAllStepsForFlow(new Guid(DDlFlow.SelectedValue));



                //DDLFlowStep.DataSource = dst;
                //DDLFlowStep.DataTextField = "FlowStepName";
                //DDLFlowStep.DataBind();
                

                BindSteps();
                DDLFlowStep.Items.Insert(0, "--Select--");
            }
            else
            {
                DDLFlowStep.Visible = false;
            }


        }

        protected void DDLFlowStep_SelectedIndexChanged(object sender, EventArgs e)
        {

            if (DDLFlowStep.SelectedValue != "--Select--")
            {
                btnSubmit.Visible = true;
                chkFlowStatus.Visible = true;
                DataTable dsb = getData.getAllStatusForFlow(new Guid(DDlFlow.SelectedValue));

                chkFlowStatus.DataSource = dsb;
                chkFlowStatus.DataTextField = "Status";
                chkFlowStatus.DataValueField = "FlowStatusUID";
                chkFlowStatus.DataBind();
                LoadStatusSelected();
            }
            else
            {
                chkFlowStatus.Visible = false;
                btnSubmit.Visible = false;
            }

        }



        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            if (DDlFlow.SelectedValue == "--Select--")
            {
                Response.Write("<script>alert('Please select a Project');</script>");
                return;
            }
            else if (DDLFlowStep.SelectedValue == "--Select--")
            {
                Response.Write("<script>alert('Please select a Workpackage');</script>");
                return;
            }

            else if (chkFlowStatus.SelectedValue == "--Select--")
            {
                Response.Write("<script>alert('Please select a step');</script>");
                return;
            }


            List<ListItem> selectedItems = chkFlowStatus.Items.Cast<ListItem>()
                                    .Where(li => li.Selected)
                                    .ToList();

            if (selectedItems.Count == 0)
            {
                Response.Write("<script>alert('Please check atleast one Status');</script>");
                return;
            }

            var selectedStatusUID = selectedItems.Select(r => new Guid(r.Value)).ToList();
           // string Step_Name = DDLFlowStep.SelectedValue.ToString();


            DataTable dtAddedStatus = getData.FlowStepStatus_Select(new Guid(DDlFlow.SelectedValue), Convert.ToInt32(DDLFlowStep.SelectedValue));


            var toBeDeleted = dtAddedStatus.AsEnumerable().Where(r => !selectedStatusUID.Contains(r.Field<Guid>("FlowStepStatusUID")));
            DataTable dtToBeDeleted = new DataTable();
            if (toBeDeleted.FirstOrDefault() != null)
            {
                dtToBeDeleted = toBeDeleted.CopyToDataTable();
            }

            foreach (var eachSelectedItems in selectedItems)
            {
                string Status = eachSelectedItems.Text;
                int cnt = getData.FlowStepStatusMaster_InsertUpdate(new Guid(eachSelectedItems.Value), new Guid(DDlFlow.SelectedValue), Convert.ToInt32(DDLFlowStep.SelectedValue), Status);

            }
            if (dtToBeDeleted.Rows.Count > 0)
            {
                foreach (DataRow eachRow in dtToBeDeleted.Rows)
                {
                    int cnt = getData.FlowStepStatusMaster_Delete(eachRow.Field<Guid>("FlowStepStatusUID"));
                }
            }
            Response.Write("<script>alert('User flow saved successfully.');</script>");

            LoadStatusSelected();

        }


        private void LoadStatusSelected()
        {
            int steps = 0;
            int.TryParse(DDLFlowStep.SelectedValue, out steps);
            

            DataTable dtStatus = new DataTable();

            if (DDlFlow.SelectedValue != "--Select--" && DDLFlowStep.SelectedValue != "--Select--")
            {
                dtStatus = getData.FlowStepStatus_Select(new Guid(DDlFlow.SelectedValue), Convert.ToInt32(DDLFlowStep.SelectedValue));

            }

            if (dtStatus != null && dtStatus.Rows.Count > 0)
            {
                foreach (ListItem listItem in chkFlowStatus.Items)
                {
                    listItem.Selected = false;

                    if (dtStatus.AsEnumerable().Where(r => r.Field<Guid>("FlowStepStatusUID") == new Guid(listItem.Value)).FirstOrDefault() != null)
                    {
                        listItem.Selected = true;
                    }
                }
            }
        }

        void BindFlow()
        {
            DataTable ds = getData.GetDocumentFlow();
            if (ds != null && ds.Rows.Count > 0)
            {
                DDlFlow.DataTextField = "Flow_Name";
                DDlFlow.DataValueField = "FlowMasterUID";
                DDlFlow.DataSource = ds;
                DDlFlow.DataBind();
                DDlFlow.Items.Insert(0, "--Select--");
                ViewState["Flow"] = ds;
            }
        }

        private void BindSteps()
        {


            DDLFlowStep.Items.Clear();
            int totalSteps = 0;
            if (DDlFlow.SelectedValue != "--Select--")
            {
                DataTable dt = (DataTable)ViewState["Flow"];
                totalSteps = dt.AsEnumerable().Where(r => r.Field<Guid>("FlowMasterUID") == new Guid(DDlFlow.SelectedValue)).Select(r => r.Field<int>("Steps_Count")).FirstOrDefault();

                for (int counter = 1; counter <= totalSteps; counter++)
                {
                    string colName = "FlowStep" + counter.ToString() + "_DisplayName";
                    var val = dt.AsEnumerable().Where(r => r.Field<Guid>("FlowMasterUID") == new Guid(DDlFlow.SelectedValue))
                        .Select(r => r.Field<string>(colName)).FirstOrDefault();


                    DDLFlowStep.Items.Add(new ListItem { Value = counter.ToString(), Text = val });
                    
                }
            }



        }
    }
}