using ProjectManager.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ProjectManagementTool._modal_pages
{
    public partial class add_newflow : System.Web.UI.Page
    {
        DBGetData getData = new DBGetData();


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

                }
            }
        }



        protected void DDLFlowSteps_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Get the selected value from the dropdown control
            int selectedValue = int.Parse(DDLFlowSteps.SelectedValue);

            // Loop through the table rows and unhide the desired number of rows based on the selected value
            for (int i = 0; i < tblmain.Rows.Count; i++)
            {
                if (i < selectedValue)
                {
                    tblmain.Rows[i].Visible = true;
                   
                }
                else
                {
                    tblmain.Rows[i].Visible = false;
                }
            }
        }



    protected void btnSubmit_Click(object sender, EventArgs e)
        {
            
                Guid FlowMasterUID = Guid.NewGuid();
                string Flow_Name = DDLnewflowname.Text.ToString();
                int Steps_Count = int.Parse(DDLFlowSteps.SelectedValue);
                string FlowStep1_DisplayName = TextBox1.Text.ToString();
                string FlowStep1_Duration = TextBox2.Text.ToString();
                string FlowStep2_DisplayName = TextBox3.Text.ToString();
                string FlowStep2_Duration = TextBox4.Text.ToString();
                string FlowStep3_DisplayName = TextBox5.Text.ToString();
                string FlowStep3_Duration = TextBox6.Text.ToString();
                string FlowStep4_DisplayName = TextBox7.Text.ToString();
                string FlowStep4_Duration = TextBox8.Text.ToString();
                string FlowStep5_DisplayName = TextBox9.Text.ToString();
                string FlowStep5_Duration = TextBox10.Text.ToString();
                string FlowStep6_DisplayName = TextBox11.Text.ToString();
                string FlowStep6_Duration = TextBox12.Text.ToString();
                string FlowStep7_DisplayName = TextBox13.Text.ToString();
                string FlowStep7_Duration = TextBox14.Text.ToString();
                string FlowStep8_DisplayName = TextBox15.Text.ToString();
                string FlowStep8_Duration = TextBox16.Text.ToString();
                string FlowStep9_DisplayName = TextBox17.Text.ToString();
                string FlowStep9_Duration = TextBox18.Text.ToString();
                string FlowStep10_DisplayName = TextBox19.Text.ToString();
                string FlowStep10_Duration = TextBox20.Text.ToString();
                string FlowStep11_DisplayName = TextBox21.Text.ToString();
                string FlowStep11_Duration = TextBox22.Text.ToString();
                string FlowStep12_DisplayName = TextBox23.Text.ToString();
                string FlowStep12_Duration = TextBox24.Text.ToString();
                string FlowStep13_DisplayName = TextBox25.Text.ToString();
                string FlowStep13_Duration =  TextBox26.Text.ToString();
                string FlowStep14_DisplayName = TextBox27.Text.ToString();
                string FlowStep14_Duration = TextBox28.Text.ToString();
                string FlowStep15_DisplayName = TextBox29.Text.ToString();
                string FlowStep15_Duration = TextBox30.Text.ToString();
                string FlowStep16_DisplayName = TextBox31.Text.ToString();
                string FlowStep16_Duration = TextBox32.Text.ToString();
                string FlowStep17_DisplayName = TextBox33.Text.ToString();
                string FlowStep17_Duration = TextBox34.Text.ToString();
                string FlowStep18_DisplayName = TextBox35.Text.ToString();
                string FlowStep18_Duration = TextBox36.Text.ToString();
                string FlowStep19_DisplayName = TextBox37.Text.ToString();
                string FlowStep19_Duration = TextBox38.Text.ToString();
                string FlowStep20_DisplayName = TextBox39.Text.ToString();
                string FlowStep20_Duration = TextBox40.Text.ToString();
            
            



            if (string.IsNullOrEmpty(Flow_Name))
            {
                Response.Write("<script>alert('Please Enter The Flow Name');</script>");
                return;

            }

            else if (Steps_Count == 00)
            {
                Response.Write("<script>alert('Please select Flow Step/s');</script>");
                return;
            }

            else if (getData.CheckFlowName_Exists(DDLnewflowname.Text) != 0)
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>alert('Entered  Flow already exists! Please try different FlowName');</script>");
                return;
            }

            else
            {

                int cnt = getData.InsertUserDefinedFlow(FlowMasterUID, Flow_Name, Steps_Count, FlowStep1_DisplayName, FlowStep1_Duration, FlowStep2_DisplayName, FlowStep2_Duration, FlowStep3_DisplayName, FlowStep3_Duration, FlowStep4_DisplayName, FlowStep4_Duration,
                    FlowStep5_DisplayName, FlowStep5_Duration, FlowStep6_DisplayName, FlowStep6_Duration , FlowStep7_DisplayName, FlowStep7_Duration, FlowStep8_DisplayName, FlowStep8_Duration, FlowStep9_DisplayName, FlowStep9_Duration, FlowStep10_DisplayName, FlowStep10_Duration,
                    FlowStep11_DisplayName, FlowStep11_Duration, FlowStep12_DisplayName, FlowStep12_Duration, FlowStep13_DisplayName, FlowStep13_Duration, FlowStep14_DisplayName, FlowStep14_Duration, FlowStep15_DisplayName, FlowStep15_Duration,
                    FlowStep16_DisplayName, FlowStep16_Duration, FlowStep17_DisplayName, FlowStep17_Duration, FlowStep18_DisplayName, FlowStep18_Duration, FlowStep19_DisplayName, FlowStep19_Duration, FlowStep20_DisplayName, FlowStep20_Duration);

                if (cnt > 0)
                {
                    Response.Write("<script>alert('New flow added successfully.');</script>");
                }
                else
                {
                    Response.Write("<script>alert('Problem occured during insertion! Please check all the fields.');</script>");
                }
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "CLOSE", "<script language='javascript'>parent.location.href=parent.location.href;</script>");

            }
        }

    }
}