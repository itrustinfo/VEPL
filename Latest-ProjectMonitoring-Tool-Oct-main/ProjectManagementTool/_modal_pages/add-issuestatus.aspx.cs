using ProjectManager.DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ProjectManagementTool._modal_pages
{
    public partial class add_issuestatus : System.Web.UI.Page
    {
        DBGetData getdata = new DBGetData();
        TaskUpdate TKUpdate = new TaskUpdate();
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
                    if (Request.QueryString["Issue_Uid"] != null)
                    {
                        IssueBind();
                    }
                    if (Request.QueryString["IssueRemarksUID"] != null)
                    {
                        IssueStatusDataBind();
                    }
                }
            }
        }

        private void IssueStatusDataBind()
        {
            DataSet ds = getdata.GetIssueStatus_by_IssueRemarksUID(new Guid(Request.QueryString["IssueRemarksUID"]));
            if (ds.Tables[0].Rows.Count > 0)
            {
                DDLStatus.SelectedValue = ds.Tables[0].Rows[0]["Issue_Status"].ToString();
                txtremarks.Text = ds.Tables[0].Rows[0]["Issue_Remarks"].ToString();
                ViewState["Document"] = ds.Tables[0].Rows[0]["Issue_Document"].ToString();
                //DDLStatus.Enabled = false;
            }
        }

        private void IssueBind()
        {
            DataSet ds = getdata.getIssuesList_by_UID(new Guid(Request.QueryString["Issue_Uid"]));
            if (ds.Tables[0].Rows.Count > 0)
            {
                if (ds.Tables[0].Rows[0]["Issue_Status"].ToString() == "Close")
                {
                    DDLStatus.SelectedValue = "Close";
                    DDLStatus.Enabled = false;
                    btnSubmit.Visible = false;
                }
                else
                {
                    DDLStatus.Enabled = true;
                    btnSubmit.Visible = true;
                }

                if (ds.Tables[0].Rows[0]["TaskUID"].ToString() == "00000000-0000-0000-0000-000000000000")
                {
                    HiddenActivity.Value = ds.Tables[0].Rows[0]["WorkPackagesUID"].ToString();
                }
                else
                {
                    HiddenActivity.Value = ds.Tables[0].Rows[0]["TaskUID"].ToString();
                }

                //added on 22/02/2023
                if(WebConfigurationManager.AppSettings["Domain"] == "LNT" || WebConfigurationManager.AppSettings["Domain"] =="Suez")
                {
                    DDLStatus.Items.Remove("Close");
                }
                //
                if(Session["IsContractor"].ToString() == "Y")
                {
                    DDLStatus.Enabled = true;
                    DDLStatus.Items.Remove("Rejected");
                }
                else //if (Session["EnggType"].ToString() == "AEE" || Session["EnggType"].ToString() == "AE" || Session["EnggType"].ToString() == "EE")
                {
                    DDLStatus.SelectedValue = "In-Progress";
                    DDLStatus.Items.Remove("Close");
                    //DDLStatus.Enabled = false;
                }
            }
        }
       

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            string DecryptPagePath = "";
            string files_path = "";

            var issue_uid = new Guid(Request.QueryString["Issue_Uid"]);

            var issue_remarks_uid = (Request.QueryString["IssueRemarksUID"] == null) ? Guid.NewGuid() : new Guid(Request.QueryString["IssueRemarksUID"]);

            string status = DDLStatus.SelectedValue;
            if (status == "In-Progress")
            {
                status = "In-Progress(Reply by " + Session["EnggType"] + ")";
                if (Session["TypeOfUser"].ToString() == "PA" || Session["TypeOfUser"].ToString() == "U")
                {
                    status = "In-Progress(Reply by Admin)";
                }
                else if (Session["TypeOfUser"].ToString() == "SP")
                {
                    status = "In-Progress(Reply by Safety Engineer)";

                }
                else if(Session["IsContractor"].ToString() == "Y")
                {
                    status = "In-Progress(Reply by Contractor)";
                }
            }
            string remarks = Session["Username"] + "(" + Session["EnggType"] + ") added :- " + txtremarks.Text;

            int cnt = getdata.Issues_Status_Remarks_Insert(issue_remarks_uid, issue_uid, status, txtremarks.Text, DecryptPagePath, DateTime.Today.Date, new Guid(Session["UserUID"].ToString()));

            if (cnt > 0)
            {
                if (FileUploadDoc.HasFiles)
                {
                    string FileDirectory = "/Documents/IssueRemarks/";

                    byte[] filetobytes = null;

                    if (!Directory.Exists(Server.MapPath(FileDirectory)))
                    {
                        Directory.CreateDirectory(Server.MapPath(FileDirectory));
                    }

                    foreach (HttpPostedFile postedFile in FileUploadDoc.PostedFiles)
                    {
                        string fileName = Path.GetFileName(postedFile.FileName);
                        postedFile.SaveAs(Server.MapPath(FileDirectory) + fileName);

                        string sFileName = Path.GetFileNameWithoutExtension(postedFile.FileName);
                        string Extn = Path.GetExtension(postedFile.FileName);

                        string savedPath = FileDirectory + "/" + fileName;

                        DecryptPagePath = FileDirectory + "/" + sFileName + "_DE" + Extn;

                        files_path = files_path + Server.MapPath(DecryptPagePath) + ",";

                        string fName = sFileName + "_DE" + Extn;

                        if (File.Exists(Server.MapPath(DecryptPagePath)))
                        {
                            fName = sFileName + "_DE_" + (new Random()).Next().ToString() + Extn;
                            DecryptPagePath = FileDirectory + "/" + fName;
                        }

                        getdata.EncryptFile(Server.MapPath(savedPath), Server.MapPath(DecryptPagePath));

                        filetobytes = getdata.FileToByteArray(Server.MapPath(DecryptPagePath));

                       // getdata.InsertIssueRemarksBlob(fileName, savedPath, filetobytes, issue_remarks_uid.ToString());

                        getdata.InsertUploadedDocument(fName, FileDirectory, issue_remarks_uid.ToString(),filetobytes);
                    }
                }
            }

            Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>parent.location.href=parent.location.href;</script>");
        }
    }
}