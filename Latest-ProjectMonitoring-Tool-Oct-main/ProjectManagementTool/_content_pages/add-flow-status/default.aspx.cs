using ProjectManager.DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ProjectManagementTool._content_pages.add_flow_status
{
    public partial class _default : System.Web.UI.Page
    {
        DBGetData getdt = new DBGetData();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["Username"] == null)
            {
                Response.Redirect("~/Login.aspx");
            }
            if (!IsPostBack)
            {

                DataTable ds = getdt.getAllUserFlows();

                //DDlFlow.Items.Insert(0, new ListItem("Select", ""));

                
                DDlFlow.DataSource = ds;
                DDlFlow.DataValueField = "FlowMasterUID";
                DDlFlow.DataTextField = "Flow_Name";
                DDlFlow.DataBind();
                DDlFlow.Items.Insert(0, "--Select--");
            }
            
            
        }





        protected void DDlFlow_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (DDlFlow.SelectedValue != "--Select--")
            {
                GrdFlowStatus.Visible = true;




                /*
                DataSet ds = getdt.getAllStatusForFlow(new Guid(DDlFlow.SelectedValue));
                GrdFlowStatus.DataSource = ds;
                GrdFlowStatus.DataBind();
                */
                bindGrid(new Guid(DDlFlow.SelectedValue));


            }
            else
            {
                GrdFlowStatus.Visible = false;
            }
        }

        protected void btnadd_Click(object sender, EventArgs e)
        {
            string Status = TXTFlowStatus.Text.ToString();
            Guid FlowStatusUID = Guid.NewGuid();

           bool ret = getdt.insertUserStatusForFlow(FlowStatusUID, new Guid(DDlFlow.SelectedValue), Status);
            if (ret)
            {
                Response.Write("<script>alert('Status inserted Successfully');</script>");
            }
            TXTFlowStatus.Text = String.Empty;

            /*DataSet dset = getdt.getAllStatusForFlow(new Guid(DDlFlow.SelectedValue));
            GrdFlowStatus.DataSource = dset;
            GrdFlowStatus.DataBind();
            */
            bindGrid(new Guid(DDlFlow.SelectedValue));
        }

        protected void GrdFlowStatus_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GrdFlowStatus.PageIndex = e.NewPageIndex;
            GrdFlowStatus.DataBind();
        }

        protected void bindGrid(Guid FlowUID)
        {
            DataTable dset = getdt.getAllStatusForFlow(FlowUID);
            GrdFlowStatus.DataSource = dset;
            GrdFlowStatus.DataBind();
        }


        
         protected void GrdFlowStatus_RowCommand(object sender, GridViewCommandEventArgs e)
         {


            string UID = e.CommandArgument.ToString();
               if (e.CommandName == "Delete")
                {
                            

                // Delete the data with the specified ID from the data source
                // (code to interact with your data source goes here)

                int cnt = getdt.flowStatus_Delete(new Guid(UID), new Guid(Session["UserUID"].ToString()));
                if (cnt > 0)
                {
                    Response.Write("<script>alert('Status Deleted!');</script>");
                }


                // Rebind the GridView to refresh the displayed data
                bindGrid(new Guid(DDlFlow.SelectedValue));
            }
 

         }
         

        protected void GrdFlowStatus_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
           
        }

        
    }
}