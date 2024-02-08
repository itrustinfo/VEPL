
using ProjectManagementTool.DAL;
using ProjectManager.DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace ProjectManagementTool._modal_pages
{
    public partial class view_document_x : System.Web.UI.Page
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
               
                if (!IsPostBack)
                {
                    BindDocStatus();
                }
            }
        }

        //changed on 29/03/2022
        protected void BindDocStatus()
        {
            
            lblWorkPackage.Text = getdata.getWorkPackageNameby_WorkPackageUID(new Guid(Request.QueryString["wUID"].ToString()));

            DataSet ds = getdata.ActualDocuments_SelectBy_ActualDocumentUID(new Guid(Request.QueryString["DocID"].ToString()));

            if (ds.Tables[0].Rows.Count >0)
                LblDocName.Text = ds.Tables[0].Rows[0]["ActualDocument_Name"].ToString();

            DataSet dsNew = getdata.getTop1_DocumentStatusSelect(new Guid(Request.QueryString["DocID"].ToString()));
            if (dsNew != null)
            {
                if (dsNew.Tables[0].Rows[0]["ActivityType"].ToString() != "" && dsNew.Tables[0].Rows[0]["TopVersion"].ToString() != "")
                {
                    try
                    {
                        string newVersionFileName = Path.GetFileNameWithoutExtension(Server.MapPath(dsNew.Tables[0].Rows[0]["Doc_Path"].ToString()));
                        LblDocNameLatest.Text = newVersionFileName.Substring(0, (newVersionFileName.Length - 2));// + " [ Ver. " + ds.Tables[0].Rows[0]["TopVersion"].ToString() + " ]"; ;
                    }
                    catch
                    {
                        LblDocNameLatest.Text = "";
                    }
                    
                }
            }

            DataSet documentSTatusList = getdata.getActualDocumentStatusListNew(new Guid(Request.QueryString["DocID"].ToString()));
           

            GrdDocStatus.DataSource = documentSTatusList;
            GrdDocStatus.DataBind();

        }

        protected void GrdDocStatus_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {

                LinkButton button1 = (LinkButton) e.Row.Cells[5].FindControl("lnkdown");

                if (button1 != null)
                {
                    if (e.Row.Cells[4].Text == "&nbsp;")
                    {
                        button1.Text = "";
                    }
                }

                LinkButton button2 = (LinkButton)e.Row.Cells[7].FindControl("lnkcoverdownload");

                if (button2 != null)
                {
                    if (e.Row.Cells[5].Text == "&nbsp;")
                    {
                        button2.Text = "";
                    }
                }
            }
        }
               

        protected void GrdDocStatus_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            string StatusUID = e.CommandArgument.ToString();
            if (e.CommandName == "download")
            {
                DataSet ds = getdata.getDocumentStatusList_by_StatusUID(new Guid(StatusUID));
                if (ds.Tables[0].Rows.Count > 0)
                {
                    string path = Server.MapPath(ds.Tables[0].Rows[0]["LinkToReviewFile"].ToString());
                    ////
                    //System.IO.FileInfo filenew = new System.IO.FileInfo(path);

                    //if (!filenew.Exists)
                    //{
                    //    using (FileStream fs = File.Create(path))

                    //    { }
                    //}
                    //

                    //for blob
                    byte[] bytes;
                    string filepath = Server.MapPath("~/_PreviewLoad/" + Path.GetFileName(path));
                    string Connectionstring = getdata.getProjectConnectionString(new Guid(Request.QueryString["ProjectUID"]));
                    DataSet docBlob = getdata.GetDocumentStatusBlob_by_StatusUID(new Guid(StatusUID), Connectionstring);
                    if (docBlob.Tables[0].Rows.Count > 0)
                    {
                        bytes = (byte[])docBlob.Tables[0].Rows[0]["ReviewFileBlob_Data"];

                        BinaryWriter Writer = null;
                        Writer = new BinaryWriter(File.OpenWrite(filepath));

                        // Writer raw data                
                        Writer.Write(bytes);
                        Writer.Flush();
                        Writer.Close();
                    }

                    string getExtension = System.IO.Path.GetExtension(filepath);
                    string outPath = path.Replace(getExtension, "") + "_download" + getExtension;
                    getdata.DecryptFile(filepath, outPath);
                    System.IO.FileInfo file = new System.IO.FileInfo(outPath);

                    if (file.Exists)
                    {

                        int Cnt = getdata.DocumentHistroy_InsertorUpdate(Guid.NewGuid(), new Guid(ds.Tables[0].Rows[0]["DocumentUID"].ToString()), new Guid(Session["UserUID"].ToString()), "Downloaded", "Documents");
                        if (Cnt <= 0)
                        {
                            Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>alert('Error Code: DDH-01. there is a problem with updating histroy. Please contact system admin.');</script>");
                        }

                        Response.Clear();

                        Response.AddHeader("Content-Disposition", "attachment; filename=" + Path.GetFileName(filepath).Replace(',', '_'));

                        Response.AddHeader("Content-Length", file.Length.ToString());

                        Response.ContentType = "application/octet-stream";

                        Response.WriteFile(file.FullName);

                        Response.End();

                    }

                    else
                    {

                        //Response.Write("This file does not exist.");
                        Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>alert('File does not exist.');</script>");

                    }
                }
            }
            if (e.CommandName == "Cover Download")
            {
                DataSet ds = getdata.getDocumentStatusList_by_StatusUID(new Guid(StatusUID));
                if (ds.Tables[0].Rows.Count > 0)
                {
                    string path = Server.MapPath(ds.Tables[0].Rows[0]["CoverLetterFile"].ToString());
                    ////
                    //System.IO.FileInfo filenew = new System.IO.FileInfo(path);

                    //if (!filenew.Exists)
                    //{
                    //    using (FileStream fs = File.Create(path))

                    //    { }
                    //}
                    //
                    //for blob
                    byte[] bytes;
                    string filepath = Server.MapPath("~/_PreviewLoad/" + Path.GetFileName(path));
                    string Connectionstring = getdata.getProjectConnectionString(new Guid(Request.QueryString["ProjectUID"]));
                    DataSet docBlob = getdata.GetDocumentStatusBlob_by_StatusUID(new Guid(StatusUID), Connectionstring);
                    if (docBlob.Tables[0].Rows.Count > 0)
                    {
                        bytes = (byte[])docBlob.Tables[0].Rows[0]["CoverFileBlob_Data"];

                        BinaryWriter Writer = null;
                        Writer = new BinaryWriter(File.OpenWrite(filepath));

                        // Writer raw data                
                        Writer.Write(bytes);
                        Writer.Flush();
                        Writer.Close();
                    }
                    string getExtension = System.IO.Path.GetExtension(filepath);
                    string outPath = path.Replace(getExtension, "") + "_download" + getExtension;
                    getdata.DecryptFile(filepath, outPath);
                    System.IO.FileInfo file = new System.IO.FileInfo(outPath);

                    if (file.Exists)
                    {

                        Response.Clear();

                        Response.AddHeader("Content-Disposition", "attachment; filename=" + Path.GetFileName(filepath).Replace(',', '_'));

                        Response.AddHeader("Content-Length", file.Length.ToString());

                        Response.ContentType = "application/octet-stream";

                        Response.WriteFile(file.FullName);

                        Response.End();

                    }

                    else
                    {

                        //Response.Write("This file does not exist.");
                        Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>alert('File does not exist.');</script>");

                    }
                }
            }

            if (e.CommandName == "delete")
            {
                int cnt = getdata.Document_Status_Delete(new Guid(StatusUID), new Guid(Session["UserUID"].ToString()));
                if (cnt > 0)
                {
                    BindDocStatus();
                }
            }
        }

       

        protected void GrdDocStatus_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {

        }

       
    }
}