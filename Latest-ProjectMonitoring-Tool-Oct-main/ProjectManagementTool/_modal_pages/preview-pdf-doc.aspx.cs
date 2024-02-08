using ProjectManager.DAL;
using System;
using System.Data;
using System.Data.OleDb;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text.RegularExpressions;
using System.Web.Configuration;
using System.Drawing.Printing;
using System.Drawing;
using System.Net;

namespace ProjectManagementTool._modal_pages
{
    public partial class preview_pdf_doc : System.Web.UI.Page
    {
        DBGetData getdata = new DBGetData();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["Username"] == null)
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>parent.location.href=parent.location.href;</script>");
            }
            else
            {
                string doc_name = "";

                if (Request.QueryString["is_id"] == "1")
                {
                    byte[] bytes;

                    string filename = "";

                    if (Request.QueryString["itype"] == "issue")
                    {
                        bytes = getdata.DownloadIssueDocument(Convert.ToInt32(Request.QueryString["doc_id"]), out filename);
                    }
                    else
                    {
                        bytes = getdata.DownloadDocument(Request.QueryString["doc_id"], out filename);
                    }

                    if (bytes != null)
                    {
                        string filepath = Server.MapPath("~/_PreviewLoad/" + Path.GetFileName(filename));

                        BinaryWriter Writer = null;
                        Writer = new BinaryWriter(File.OpenWrite(filepath));

                        // Writer raw data                
                        Writer.Write(bytes);
                        Writer.Flush();
                        Writer.Close();

                        string Extension = Path.GetExtension(filepath);
                        string outPath = filepath.Replace(Extension, "") + "_download" + Extension;
                        getdata.DecryptFile(filepath, outPath);

                        FileInfo f = new FileInfo(outPath);

                        doc_name = "/_PreviewLoad/" + f.Name;
                    }
                }
                else
                {
                    doc_name = Request.QueryString["doc_name"];
                }


                doc_name = Server.MapPath(doc_name);

                FileInfo file = new System.IO.FileInfo(doc_name);

                if (file.Exists)
                {
                    string getExtension = System.IO.Path.GetExtension(doc_name);

                    string filename = Path.GetFileName(doc_name).Replace(getExtension, "") + getExtension;


                    string outPath = Server.MapPath("~/_PreviewLoad/" + filename);


                    //  getdata.DecryptFile(doc_name, outPath);

                    if (getExtension == ".pdf")
                    {
                        btnImgPrint.Visible = false;
                        WebClient User = new WebClient();

                        Byte[] FileBuffer = User.DownloadData(outPath);
                        if (FileBuffer != null)
                        {
                            Response.ContentType = "application/pdf";
                            Response.AddHeader("content-length", FileBuffer.Length.ToString());
                            Response.BinaryWrite(FileBuffer);
                        }
                    }
                    else
                    {
                        btnImgPrint.Visible = true;
                        image1.ImageUrl = "~/_PreviewLoad/" + filename;
                    }

                }
                else
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>alert('File not found.');</script>");
                }
            }
        }

    
    }
}