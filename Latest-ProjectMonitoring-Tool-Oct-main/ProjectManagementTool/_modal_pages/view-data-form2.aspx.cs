
using ProjectManagementTool.DAL;
using ProjectManager.DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace ProjectManagementTool._modal_pages
{
    public partial class view_data_form2 : System.Web.UI.Page
    {
        DBGetData getdata = new DBGetData();
        DataSet ds = new DataSet();
        string next = string.Empty;
        string prevstatus = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["Username"] == null)
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>parent.location.href=parent.location.href;</script>");
            }
            else
            {
                if (!Page.IsPostBack)
                {
                    GetDataList();
                }
            }
        }

        protected void GetDataList()
        {
            DataSet ds = getdata.ActualDocuments_SelectBy_ActualDocumentUID(new Guid(Request.QueryString["DocID"].ToString()));
            if (ds.Tables[0].Rows.Count > 0)
            {
                //txtDocumentName.Text = ds.Tables[0].Rows[0]["ActualDocument_Name"].ToString();
                lblDocumentDescription.Text = ds.Tables[0].Rows[0]["Description"].ToString();
                lblContractorno.Text = ds.Tables[0].Rows[0]["Ref_Number"].ToString();
                lblDocRefno.Text = ds.Tables[0].Rows[0]["ProjectRef_Number"].ToString();

                if (ds.Tables[0].Rows[0]["IncomingRec_Date"].ToString() != null && ds.Tables[0].Rows[0]["IncomingRec_Date"].ToString() != "")
                {
                    lblContractorDate.Text = Convert.ToDateTime(ds.Tables[0].Rows[0]["IncomingRec_Date"].ToString()).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
                    lblONTbdate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                }
                lblDocNo.Text = ds.Tables[0].Rows[0]["FileRef_Number"].ToString();
                lblMainDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                //lblCurrentStatus.Text = ds.Tables[0].Rows[0]["ActualDocument_CurrentStatus"].ToString();

                //
                int revison = 0;
                DataSet dslatestVersion = getdata.getLatest_DocumentVerisonSelect(new Guid(Request.QueryString["DocID"].ToString()));
                if(dslatestVersion.Tables[0].Rows.Count > 0)
                {
                    if ((int.Parse(dslatestVersion.Tables[0].Rows[0]["Doc_Version"].ToString()) - 1) == 0)
                    {
                        lblRevision.Text = "Rev-A";
                    }
                    else if ((int.Parse(dslatestVersion.Tables[0].Rows[0]["Doc_Version"].ToString()) - 1) == 1)
                    {
                        lblRevision.Text = "Rev-B";

                    }
                    else if ((int.Parse(dslatestVersion.Tables[0].Rows[0]["Doc_Version"].ToString()) - 1) == 2)
                    {
                        lblRevision.Text = "Rev-C";

                    }
                    revison = (int.Parse(dslatestVersion.Tables[0].Rows[0]["Doc_Version"].ToString()));
                }
                //DataSet ds = getdata.GetDataList();

                //GrdDataList.DataSource = ds;
                //GrdDataList.DataBind();
                //get comment sheet for DTL
                DataSet dsdata = getdata.GetCommentSheetForDTL_Complete(new Guid(Request.QueryString["DocID"]), revison);
                GrdDataList.DataSource = dsdata;
              
                GrdDataList.DataBind();

            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {

            //foreach(GridViewRow  item in GrdDataList.Rows)
            //{
            //    TextBox txtBox = item.FindControl("TextBox1") as TextBox;

            //    getdata.UpdateComment(Convert.ToInt32(item.Cells[0].Text), txtBox.Text);
            //}

           // Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>parent.location.href=parent.location.href;</script>");
        }

        protected void GrdDataList_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                e.Row.Cells[2].Text = "Comments on " + lblRevision.Text;

            }
                if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (!string.IsNullOrEmpty(e.Row.Cells[1].Text) && e.Row.Cells[1].Text != "&nbsp;")
                {
                    DataSet dsUser = getdata.getUserDetails(new Guid(e.Row.Cells[1].Text));
                    if (dsUser.Tables[0].Rows.Count > 0)
                    {
                        e.Row.Cells[1].Text = dsUser.Tables[0].Rows[0]["UserName"].ToString();
                    }
                }
            }
        }
    }
}