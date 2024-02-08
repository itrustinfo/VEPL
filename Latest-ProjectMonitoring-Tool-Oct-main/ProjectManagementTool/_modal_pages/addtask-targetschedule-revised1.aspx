<%@ Page Title="" Language="C#" MasterPageFile="~/_master_pages/modal.Master" AutoEventWireup="true" CodeBehind="addtask-targetschedule-revised1.aspx.cs" Inherits="ProjectManagementTool._modal_pages.addtask_targetschedule_revised1" %>
<%--<%@ Register Src="~/_modal_pages/TaskScheduleUserControl.ascx" TagName="UserControl" TagPrefix="uc1" %>--%>
<asp:Content ID="Content1" ContentPlaceHolderID="modal_master_head" runat="server">
    <script type = "text/javascript">
        function SetSource(SourceID) {
            var hidSourceID = document.getElementById("<%=hidSourceID.ClientID%>");
            hidSourceID.value = SourceID;
        }
    </script>
     <script type="text/javascript">
         function DeleteItem() {
             if (confirm("Are you sure you want to delete ...?")) {
                 return true;
             }
             return false;
         }

         function text_changed() {
             $("#<%=btnUpdate.ClientID%>").removeAttr('disabled');
         }

         function VersionConfirm() {
             
             if (document.getElementById('<%= HiddenAction.ClientID%>').value == "Update") {
                 var confirm_value = document.createElement("INPUT");
                 confirm_value.type = "hidden";
                 confirm_value.name = "confirm_value";
                 if (confirm("Do you want to create a new schedule version?")) {
                     confirm_value.value = "Yes";
                 } else {
                     confirm_value.value = "No";
                 }

                 document.forms[0].appendChild(confirm_value);
             }
         }
     </script>

    
   <style type="text/css">
     .hiddencol
     {
       display: none;
     }
   </style>

    <style type="text/css">
        .TheDateTimePicker{display:block;width:100%;height:calc(1.5em + .75rem + 2px);padding:.375rem .75rem;font-size:1rem;font-weight:400;line-height:1.5;color:#495057;background-color:#fff;background-clip:padding-box;border:1px solid #ced4da;border-radius:.25rem;transition:border-color .15s ease-in-out,box-shadow .15s ease-in-out;}
    </style>

      <%--<script type="text/javascript">
    //    $(function () {
    //    bindDatePickers(); // bind date picker on first page load
    //    Sys.WebForms.PageRequestManager.getInstance().add_endRequest(bindDatePickers); // bind date picker on every UpdatePanel refresh
    //});

          //function bindDatePickers() {
          //    $('.TheDateTimePicker').datepicker({ dateFormat: 'dd/mm/yy',changeMonth: true,changeYear: true });
          //}

      //$("input[id$='dtEndDate']").datepicker({
      //changeMonth: true,
      //  changeYear: true,
      //dateFormat:'dd/mm/yy'
      //});
</script>--%>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="modal_master_body" runat="server">
    <form id="frmAddTaskSchedule" runat="server">
    <div>
        <asp:HiddenField ID="hidSourceID" runat="server" />
        <asp:ScriptManager ID="sm1" runat="server" />
        <div class="container-fluid" style="max-height:84vh; overflow-y:auto; min-height:84vh;">
             <asp:UpdatePanel ID="up1" runat="server">
            <ContentTemplate>
                
                <asp:HiddenField ID="HiddenAction" runat="server" Value="Save" />
            <div class="row">
                 <div class="col-sm-6">
                     <div class="form-group">
                        <label class="lblCss" for="RBScheduleTye">Planned Type</label>
                          <asp:RadioButtonList ID="RBScheduleTye" runat="server" Width="100%" CssClass="lblCss" CellPadding="5" RepeatDirection="Horizontal" AutoPostBack="true"  RepeatColumns="4" OnSelectedIndexChanged="RBScheduleType_Changed">
                                 <asp:ListItem Selected="True" Value="Month">&nbsp;&nbsp;By Month</asp:ListItem>
                                 <asp:ListItem Value="Week">&nbsp;&nbsp;By Week</asp:ListItem>
                          </asp:RadioButtonList>
                    </div>
                     
                     </div>
            </div>
            <div class="row">
               
                <div class="col-lg-4" style="margin:5px;margin-left:15px">
                        <asp:DropDownList runat="server" ID="DDLYear" Width="100%" CssClass="form-control" Visible="false">
                             <asp:ListItem Value="2023">2023</asp:ListItem>
                             <asp:ListItem Value="2024">2024</asp:ListItem>
                             <asp:ListItem Value="2025">2025</asp:ListItem>
                             <asp:ListItem Value="2026">2026</asp:ListItem>
                         </asp:DropDownList>
                    </div>
                <div class="col-lg-4" style="margin:5px;margin-left:15px">
                         <asp:DropDownList runat="server" ID="DDLMonth" Width="100%" CssClass="form-control" AutoPostBack="true" Visible="false"  OnSelectedIndexChanged ="DDLMonth_selectedIndexChanged">
                             <asp:ListItem Value="">Select Month</asp:ListItem>
                             <asp:ListItem Value="Jan">Jan</asp:ListItem>
                             <asp:ListItem Value="Feb">Feb</asp:ListItem>
                             <asp:ListItem Value="Mar">Mar</asp:ListItem>
                             <asp:ListItem Value="Apr">Apr</asp:ListItem>
                             <asp:ListItem Value="May">May</asp:ListItem>
                             <asp:ListItem Value="Jun">Jun</asp:ListItem>
                             <asp:ListItem Value="Jul">Jul</asp:ListItem>
                             <asp:ListItem Value="Aug">Aug</asp:ListItem>
                             <asp:ListItem Value="Sep">Sep</asp:ListItem>
                             <asp:ListItem Value="Oct">Oct</asp:ListItem>
                             <asp:ListItem Value="Nov">Nov</asp:ListItem>
                             <asp:ListItem Value="Dec">Dec</asp:ListItem>
                         </asp:DropDownList>
                    </div>
             
            </div> 
                <br />


                <div class="container-fluid" style="width:100%">
                   <div>
                   <asp:GridView ID="GridView1" CssClass="table table-bordered" runat="server" AutoGenerateColumns="false"  OnRowDataBound="OnRowDataBound" OnRowCommand="OnRowCommand" OnRowDeleting="GrdView1_RowDeleting" Width="100%" >
                     <Columns> 
                       
                        <asp:BoundField DataField="TaskScheduleUID"  HeaderText="TaskSchedule" HeaderStyle-CssClass="hiddencol" ItemStyle-CssClass="hiddencol" >
                                                <HeaderStyle HorizontalAlign="Left" />

                        <ItemStyle CssClass="hiddencol" ></ItemStyle>
                                                </asp:BoundField>

                        <asp:TemplateField HeaderText = "Month">
                            <ItemTemplate>
                            <asp:Label ID="lblMonth" runat="server" Text='<%# Eval("s_month") %>' Visible = "false" />
                            <asp:DropDownList ID="ddlMonths" runat="server" Width="100%">
                            </asp:DropDownList>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText = "Year">
                            <ItemTemplate>
                            <asp:Label ID="lblYear" runat="server" Text='<%# Eval("s_year") %>' Visible = "false" />
                            <asp:DropDownList ID="ddlYears" runat="server" Width="100%">
                            </asp:DropDownList>
                            </ItemTemplate>
                        </asp:TemplateField>

                          <asp:TemplateField HeaderText = "Schedule Value">
                            <ItemTemplate>
                            <asp:Label ID="lblScheduleValue" runat="server" Text='<%# Eval("Schedule_Value") %>' Visible = "false" />
                            <asp:TextBox ID="txtScheduleValue" runat="server" Width="100%" onKeyPress="javascript:text_changed();" >
                            </asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                          <asp:TemplateField HeaderText = "">
                            <ItemTemplate>
                               <asp:LinkButton ID="lnkdelete1" runat="server" OnClientClick="return DeleteItem()"  CausesValidation="true" CommandArgument='<%# Eval("TaskScheduleUID") %>' CommandName="delete"><span title="x" class="fas fa-trash"></span></asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                    </asp:GridView>

                 <div style="padding:15px">
                    <asp:Button ID="btnAddNew" runat="server" Text="Add New Row" CssClass="btn btn-primary" OnClick="btnAddNew_Click" Enabled="true" OnClientClick="javascript:disableButton('btnSave');" />
                    <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="btn btn-primary" OnClick="btnCancel_Click" Enabled="false" />
                    <asp:Button ID="btnSaveNew" runat="server" Text="Save New Row" CssClass="btn btn-primary" OnClick="btnSaveNew_Click" Enabled="false" />
                </div>
                </div>
                    <div>
                  <asp:GridView ID="GridView2" CssClass="table table-bordered" runat="server" AutoGenerateColumns="false"  OnRowDataBound="GridView2_OnRowDataBound"  OnRowCommand="GrdView2_OnRowCommand" OnRowDeleting="GrdView2_RowDeleting" Width="100%" >
                     <Columns> 
                       
                        <asp:BoundField DataField="TaskScheduleUID"  HeaderText="TaskSchedule" HeaderStyle-CssClass="hiddencol" ItemStyle-CssClass="hiddencol" >
                                                <HeaderStyle HorizontalAlign="Left" />

                        <ItemStyle CssClass="hiddencol" ></ItemStyle>
                                                </asp:BoundField>

                        <asp:TemplateField HeaderText = "Start Date">
                            <ItemTemplate>
                            <asp:Label ID="lblDate1" runat="server" Text='<%# Eval("StartDate") %>' Visible = "false" />
                            <asp:TextBox ID="txtDate1" runat="server" Width="100%" CssClass="form-control" ReadOnly="true">

                            </asp:TextBox>
                                
                            </ItemTemplate>
                        </asp:TemplateField>

                          <asp:TemplateField HeaderText = "End Date">
                            <ItemTemplate>
                            <asp:Label ID="lblDate2" runat="server" Text='<%# Eval("EndDate") %>' Visible = "false" />
                            <asp:TextBox ID="txtDate2" runat="server" Width="100%" CssClass="form-control" ReadOnly="true">
                            </asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText = "Schedule Value">
                            <ItemTemplate>
                            <asp:Label ID="lblScheduleValue" runat="server" Text='<%# Eval("Schedule_Value") %>' Visible = "false" />
                            <asp:TextBox ID="txtScheduleValue" runat="server" Width="100%" CssClass="form-control" onKeyPress="javascript:text_changed();" >
                            </asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                          <asp:TemplateField HeaderText = "">
                            <ItemTemplate>
                               <asp:LinkButton ID="lnkdelete2" runat="server" OnClientClick="return DeleteItem()"  CausesValidation="true" CommandArgument='<%# Eval("TaskScheduleUID") %>' CommandName="delete"><span title="x" class="fas fa-trash"></span></asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                    </asp:GridView>

                <div style="padding:15px">
                    <asp:Button ID="btnAddNew1" runat="server" Text="Add New Row" CssClass="btn btn-primary" OnClick="btnAddNew1_Click" Enabled="true" Visible="false" OnClientClick="javascript:disableButton('btnSave');" />
                    <asp:Button ID="btnCancel1" runat="server" Text="Cancel" CssClass="btn btn-primary" OnClick="btnCancel1_Click" Enabled="false" Visible="false" />
                    <asp:Button ID="btnSaveNew1" runat="server" Text="Save New Row" CssClass="btn btn-primary" OnClick="btnSaveNew1_Click" Enabled="false" Visible="false"/>
                </div>
                     </div>  
                </div>
               
               

                </ContentTemplate>
        </asp:UpdatePanel>

            <div>
               
            </div>
        <asp:Literal ID="ltlCount" runat="server" Text="0" Visible="false" />
        <asp:Literal ID="ltlRemoved" runat="server" Visible="false" />
            </div>
       
    </div>
        <div class="modal-footer">
            <asp:Button ID="btnUpdate" runat="server" Text="Update/Close" CssClass="btn btn-primary" OnClientClick="javascript:VersionConfirm()" OnClick="btnUpdate_Click" Enabled="true" />
        </div>
        </form>
</asp:Content>
