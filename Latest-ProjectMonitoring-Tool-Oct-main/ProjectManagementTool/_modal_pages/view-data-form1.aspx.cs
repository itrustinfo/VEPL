
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
    public partial class view_data_form1 : System.Web.UI.Page
    {
        DBGetData getdata = new DBGetData();
        DataSet ds = new DataSet();
        string next = string.Empty;
        string prevstatus = string.Empty;
        string Version = "";
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

        //protected void GetDataList()
        //{
        //    DataSet ds = getdata.GetDataList1();

        //    GrdDataList.DataSource = ds;
        //    GrdDataList.DataBind();

        //}

        protected void GetDataList()
        {
            DataSet ds = getdata.ActualDocuments_SelectBy_ActualDocumentUID(new Guid(Request.QueryString["DocID"].ToString()));
            if (ds.Tables[0].Rows.Count > 0)
            {
                //txtDocumentName.Text = ds.Tables[0].Rows[0]["ActualDocument_Name"].ToString();
                lblDescription.Text = ds.Tables[0].Rows[0]["Description"].ToString();
                lblContractorno.Text = ds.Tables[0].Rows[0]["Ref_Number"].ToString();
                lblONTBRefNo.Text = ds.Tables[0].Rows[0]["ProjectRef_Number"].ToString();

                if (ds.Tables[0].Rows[0]["IncomingRec_Date"].ToString() != null && ds.Tables[0].Rows[0]["IncomingRec_Date"].ToString() != "")
                {
                    lblContractorDate.Text = Convert.ToDateTime(ds.Tables[0].Rows[0]["IncomingRec_Date"].ToString()).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
                    lblONTbdate.Text = DateTime.Now.ToString("dd/MM/yyyy");
                }
                lblDocNo.Text = ds.Tables[0].Rows[0]["FileRef_Number"].ToString();

                lblCurrentStatus.Text = ds.Tables[0].Rows[0]["ActualDocument_CurrentStatus"].ToString();

                ////
                //DataSet dslatestVersion = getdata.getLatest_DocumentVerisonSelect(new Guid(Request.QueryString["DocID"].ToString()));
                //if (dslatestVersion.Tables[0].Rows.Count > 0)
                //{
                //    lblRevision.Text = dslatestVersion.Tables[0].Rows[0]["Doc_Version"].ToString();

                //}
                //DataSet ds = getdata.GetDataList();

                //GrdDataList.DataSource = ds;
                //GrdDataList.DataBind();
                //get comment sheet for DTL
                DataSet dsdata = getdata.GetCRSSheetMainData(new Guid(Request.QueryString["DocID"]));
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

            Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>parent.location.href=parent.location.href;</script>");
        }

        protected void GrdDataList_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                
                if (e.Row.Cells[2].Text.Contains("Code C") || e.Row.Cells[2].Text.Contains("Code D"))
                {
                    DataSet dscomments = getdata.GetContractorCommentsforREsubmission(new Guid(Request.QueryString["DocID"]), (int.Parse(e.Row.Cells[0].Text) + 1));
                    if (dscomments.Tables[0].Rows.Count > 0)
                    {
                        e.Row.Cells[4].Text = dscomments.Tables[0].Rows[0]["Doc_Comments"].ToString();
                    }
                }
                if (Version != e.Row.Cells[5].Text)
                {
                    e.Row.Cells[0].Text = "Rev " + (int.Parse(e.Row.Cells[0].Text) - 1);
                    Version = e.Row.Cells[5].Text;
                    e.Row.Cells[5].Text = Version;
                }
                else
                {
                    e.Row.Cells[0].Text = "";
                }
            }
        }
    }
}