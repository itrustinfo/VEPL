using ProjectManager.DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ProjectManagementTool._content_pages.newform_submittal
{
    public partial class _default : System.Web.UI.Page
    {
        DBGetData getdata = new DBGetData();
        DataTable dt = new DataTable();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["Username"] == null)
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>parent.location.href=parent.location.href;</script>");
            }
            else
            {
                try
                {
                    DataSet ds = getdata.GetSubmittalFlowDetails();
             
                    GrdnewFormSubmittal.DataSource = ds;
                    GrdnewFormSubmittal.DataBind();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }
        protected void GrdnewFormSubmittal_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            GrdnewFormSubmittal.PageIndex = e.NewPageIndex;
            GrdnewFormSubmittal.DataBind();
        }
    }
}