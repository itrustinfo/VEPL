<%@ Page Title="" Language="C#" MasterPageFile="~/_master_pages/default.Master" AutoEventWireup="true" CodeBehind="default.aspx.cs" Inherits="ProjectManagementTool._content_pages.report_physicalprogress_new1._default" %>
<asp:Content ID="Content1" ContentPlaceHolderID="default_master_head" runat="server">
    <script type="text/javascript" src="https://www.gstatic.com/charts/loader.js"></script>
   <%-- <script src = "Scripts/jquery-1.4.1.min.js" type = "text/javascript" > </script>
    <script src="Scripts/ScrollableGridViewPlugin_ASP.NetAJAXmin.js" type ="text/javascript" > </script>--%>

    <link rel="stylesheet" href="//code.jquery.com/ui/1.12.1/themes/base/jquery-ui.css">

  <script src="https://code.jquery.com/jquery-1.12.4.js"></script>
  <script src="https://code.jquery.com/ui/1.12.1/jquery-ui.js"></script>

    <script type="text/javascript">

        function printdiv(printpage) {
                var frame = document.getElementById(printpage);
                 document.getElementById("btnActivityProgressPrint").style.display = "none";
    var data = frame.innerHTML;
    var win = window.open('', '', 'height=500,width=900');
    win.document.write('<style>@page{size:landscape;}</style><html><head><title></title>');
    win.document.write('</head><body >');
    win.document.write(data);
    win.document.write('</body></html>');
    win.print();
                win.close();
                document.getElementById("btnActivityProgressPrint").style.display = "block";
    return true;
        }

        function DateText() {
            $(function () {

                $(".datepick").datepicker({
                    changeMonth: true,
                    changeYear: true,
                    dateFormat: 'dd/mm/yy'
                });

            });
        }

        $(document).ready(function () {

            DateText();
        });


    </script>

    <style type="text/css">  
            .scrolling {  
                position: absolute;  
            }  
              
            .gvWidthHight {  
                overflow: scroll;  
                height: 600px;  
                width: 100%;  
            }  
        </style>  


     <style type="text/css">
         .hideItem {
         display:none;
     }
    </style>

    <style>
     th {
        background: white!important;
        color:black!important;
        position: sticky!important;
        top: 0;
        box-shadow: 0 2px 2px -1px rgba(0, 0, 0, 0.4);
    }
    th, td {
        padding: 0.25rem;
    }
</style>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="default_master_body" runat="server">
    <div class="container-fluid">
            <div class="row">
                <div class="col-md-12 col-lg-4 form-group">Project Physical Progress</div>
                <div class="col-md-6 col-lg-4 form-group">
                    <label class="sr-only" for="DDLProject">Project</label>
                    <div class="input-group">
                        <div class="input-group-prepend">
                            <span class="input-group-text">Project</span>
                        </div>
                        <asp:DropDownList ID="DDlProject" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="DDlProject_SelectedIndexChanged"></asp:DropDownList>
                    </div>
                </div>
                <div class="col-md-6 col-lg-4 form-group">
                    <label class="sr-only" for="DDLWorkPackage">Work Package</label>
                    <div class="input-group">
                        <div class="input-group-prepend">
                            <span class="input-group-text">Work Package</span>
                        </div>
                        <asp:DropDownList ID="DDLWorkPackage" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="DDLWorkPackage_SelectedIndexChanged"></asp:DropDownList>
                    </div>
                </div>
            </div>
        </div>
    <div class="container-fluid" >
            <div class="row">
                <div class="col-md-6 col-xl-6 mb-4" id="ReportFormat" runat="server">
                     <div class="card">
                        <div class="card-body">
                             <h6 class="card-title text-muted text-uppercase font-weight-bold">Report Format</h6>
                            <div>
                            <div class="col-md-12" style="padding:10px;margin:0px;margin-bottom:20px">
                                <div class="col-md-6 fa-pull-left" style="padding:0px">
                             <asp:RadioButtonList CssClass="form-control" ID="RBLType" BorderStyle="None" runat="server" RepeatDirection="Horizontal" AutoPostBack="true" OnSelectedIndexChanged="RBLType_SelectedIndexChanged">
                                <asp:ListItem Value="Tabular" Selected="false">&nbsp;Tabular</asp:ListItem>  
                                <asp:ListItem Value="Graphical" Selected="True">&nbsp;Graphical</asp:ListItem>  
                            </asp:RadioButtonList>
                                </div>
                                <div class="col-md-6 fa-pull-right">
                             <asp:DropDownList ID="DDLMilestones" runat="server" CssClass="form-control" Width="100%" AutoPostBack="true" OnSelectedIndexChanged="DDLMileStones_SelectedIndexChanged" Enabled="true" >
                             </asp:DropDownList>
                                    </div>
                            </div>
                                <br />
                            <div class="col-md-12" style="padding:0px;margin:10px">
                            <asp:RadioButtonList CssClass="form-control" ID="RBLReportFor" BorderStyle="None" runat="server" RepeatDirection="Horizontal" AutoPostBack="true" OnSelectedIndexChanged="RBLReportFor_SelectedIndexChanged">
                                <asp:ListItem Value="By Week">&nbsp;By Week</asp:ListItem>  
                                <asp:ListItem Value="By Fortnightly">&nbsp;By Fortnight</asp:ListItem>  
                                <asp:ListItem Value="By Month">&nbsp;By Month</asp:ListItem>  
                                <asp:ListItem Value="By Quarter">&nbsp;By Quarter</asp:ListItem> 
                                <asp:ListItem Value="By HalfYear">&nbsp;By Half Year</asp:ListItem>
                                <%--<asp:ListItem Value="Across Months" Enabled="false">&nbsp;Across Months</asp:ListItem>
                                <asp:ListItem Value="By Activity">&nbsp;By Activity</asp:ListItem>  --%>
                        </asp:RadioButtonList>
                            </div>
                            
                            </div>
                            </div>
                         </div>
                    
                    </div>
                <div class="col-md-6 col-xl-6 mb-6" id="ByMonth" runat="server">
                     <div class="card">
                        <div class="card-body" > 
                            <div style="padding:5px">
                                <asp:radiobuttonlist runat="server" CssClass="form-control" ID="rblSelection" RepeatDirection="Horizontal" AutoPostBack="true" OnSelectedIndexChanged="rblSelection_SelectedIndexChanged">
                                    <asp:listitem Value="General" Selected="True">General</asp:listitem>
                                    <asp:listitem Value="Date Range">Date Range</asp:listitem>
                                </asp:radiobuttonlist>
                            </div>
                            <br />
                          
                            <div class="table-responsive">
                            <table style="width:80%;">
                              <tr>
                                  <td>
                                      <%-- <h6 class="card-title text-muted text-uppercase font-weight-bold">Select Month</h6>--%>
                                  </td>
                              </tr>
                             <tr>
                                 <td>
                                        <asp:DropDownList ID="DDLYear" runat="server" CssClass="form-control">
                                        </asp:DropDownList>
                                        <asp:Label ID="lblMonth1" runat="server" Text="From" Visible="false"></asp:Label>
                                        <asp:TextBox runat="server" ID="date1" CssClass="form-control datepick" Width="100%" Visible="false" placeholder="dd/mm/yyyy"></asp:TextBox>
                                 </td>
                                 <td>&nbsp;</td>
                                 <td>
                                        <asp:DropDownList ID="DDLMonth" runat="server" CssClass="form-control">
                                            <asp:ListItem Value="01">Jan</asp:ListItem>
                                            <asp:ListItem Value="02">Feb</asp:ListItem>
                                            <asp:ListItem Value="03">Mar</asp:ListItem>
                                            <asp:ListItem Value="04">Apr</asp:ListItem>
                                            <asp:ListItem Value="05">May</asp:ListItem>
                                            <asp:ListItem Value="06">Jun</asp:ListItem>
                                            <asp:ListItem Value="07">Jul</asp:ListItem>
                                            <asp:ListItem Value="08">Aug</asp:ListItem>
                                            <asp:ListItem Value="09">Sep</asp:ListItem>
                                            <asp:ListItem Value="10">Oct</asp:ListItem>
                                            <asp:ListItem Value="11">Nov</asp:ListItem>
                                            <asp:ListItem Value="12">Dec</asp:ListItem>
                                        </asp:DropDownList>
                                        <asp:Label ID="lblMonth2" runat="server" Text="To" Visible="false"></asp:Label>
                                        <asp:TextBox runat="server" ID="date2" CssClass="form-control datepick" Width="100%" Visible="false" placeholder="dd/mm/yyyy" ></asp:TextBox>
                                 </td>
                                 <td>&nbsp;</td>
                                 <td>
                                     <asp:Button ID="BntSubmit" runat="server" CssClass="btn btn-primary" Text="Submit" OnClick="BntSubmit_Click" />
                                 </td>
                             </tr>
                            </table>
                                </div>
                            </div>
                         </div>
                    </div>

                <div class="col-md-6 col-xl-6 mb-4" id="ByWeek" runat="server">
                     <div class="card">
                        <div class="card-body" > 
                            <div style="padding:5px">
                                <asp:radiobuttonlist runat="server" CssClass="form-control" ID="rbllist1" RepeatDirection="Horizontal" AutoPostBack="true" OnSelectedIndexChanged="rbllist1_SelectedIndexChanged">
                                    <asp:listitem Value="General" Selected="True">General</asp:listitem>
                                    <asp:listitem Value="Date Range">Date Range</asp:listitem>
                                </asp:radiobuttonlist>
                            </div>
                            <br />
                          
                            <div class="table-responsive">
                            <table style="width:70%;">
                              <tr>
                                  <td>
                                       <%--<h6 class="card-title text-muted text-uppercase font-weight-bold">Select Week</h6>--%>
                                  </td>
                              </tr>
                             <tr>
                                  <td>
                                        <asp:DropDownList ID="DDLWeekYear" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="DDLWeekYear_SelectedIndexChanged">
                                        </asp:DropDownList>
                                      <asp:Label ID="lblWeek1" runat="server" Text="From" Visible="false"></asp:Label>
                                      <asp:TextBox runat="server" ID="date3" CssClass="form-control datepick" Width="100%" Visible="false" placeholder="dd/mm/yyyy"></asp:TextBox>
                                 </td>
                                 <td>&nbsp;</td>
                                 <td>
                                        <asp:DropDownList ID="DDLWeekMonth" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="DDLWeekMonth_SelectedIndexChanged">
                                        </asp:DropDownList>
                                         <asp:Label ID="lblWeek2" runat="server" Text="To" Visible="false"></asp:Label>
                                        <asp:TextBox runat="server" ID="date4" CssClass="form-control datepick" Width="100%" Visible="false" placeholder="dd/mm/yyyy"></asp:TextBox>
                                 </td>
                                 <td>&nbsp;</td>
                                 <td>
                                        <asp:DropDownList ID="DDLWeek" runat="server" CssClass="form-control">
                                        </asp:DropDownList>
                                 </td>
                                 <td>&nbsp;</td>
                                 <td>
                                     <asp:Button ID="btnWeekSubmit" runat="server" CssClass="btn btn-primary" Text="Submit" OnClick="btnWeekSubmit_Click" />
                                 </td>
                             </tr>
                            </table>
                                </div>
                            </div>
                         </div>
                    </div>

                <div class="col-md-6 col-xl-6 mb-4" id="Byfortingiht" runat="server">
                     <div class="card">
                        <div class="card-body" > 
                            <div style="padding:5px">
                                <asp:radiobuttonlist runat="server" CssClass="form-control" ID="rbllist2" RepeatDirection="Horizontal" AutoPostBack="true" OnSelectedIndexChanged="rbllist2_SelectedIndexChanged">
                                    <asp:listitem Value="General" Selected="True">General</asp:listitem>
                                    <asp:listitem Value="Date Range">Date Range</asp:listitem>
                                </asp:radiobuttonlist>
                            </div>
                             <br />
                          
                            <div class="table-responsive">
                            <table style="width:70%;">
                              <tr>
                                  <td>
                                      <%-- <h6 class="card-title text-muted text-uppercase font-weight-bold">Select Fortinet</h6>--%>
                                  </td>
                              </tr>
                             <tr>
                                  <td>
                                        <asp:DropDownList ID="DDLFortYear" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="DDLFortYear_SelectedIndexChanged">
                                        </asp:DropDownList>
                                       <asp:Label ID="lblFortnight1" runat="server" Text="From" Visible="false"></asp:Label>
                                       <asp:TextBox runat="server" ID="date5" CssClass="form-control datepick" Width="100%" Visible="false" placeholder="dd/mm/yyyy"></asp:TextBox>
                                 </td>
                                 <td>&nbsp;</td>
                                 <td>
                                        <asp:DropDownList ID="DDLFortiMonth" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="DDLFotMonth_SelectedIndexChanged">
                                        </asp:DropDownList>
                                        <asp:Label ID="lblFortnight2" runat="server" Text="To" Visible="false"></asp:Label>
                                       <asp:TextBox runat="server" ID="date6" CssClass="form-control datepick" Width="100%" Visible="false" placeholder="dd/mm/yyyy"></asp:TextBox>
                                 </td>
                                 <td>&nbsp;</td>
                                 <td>
                                        <asp:DropDownList ID="DDLFortWeek" runat="server" CssClass="form-control">
                                        </asp:DropDownList>
                                 </td>
                                 <td>&nbsp;</td>
                                 <td>
                                     <asp:Button ID="btnFortiSubmit" runat="server" CssClass="btn btn-primary" Text="Submit" OnClick="btnFortiSubmit_Click" />
                                 </td>
                             </tr>
                            </table>
                                </div>
                            </div>
                         </div>
                    </div>

                <div class="col-md-6 col-xl-6 mb-4" id="ByQuarteMonth" runat="server">
                     <div class="card">
                        <div class="card-body" >
                            <div style="padding:5px">
                                <asp:radiobuttonlist runat="server" CssClass="form-control" ID="rbllist3" RepeatDirection="Horizontal" AutoPostBack="true" OnSelectedIndexChanged="rbllist3_SelectedIndexChanged">
                                    <asp:listitem Value="General" Selected="True">General</asp:listitem>
                                    <asp:listitem Value="Date Range">Date Range</asp:listitem>
                                </asp:radiobuttonlist>
                            </div>
                             <br />
                          
                            <div class="table-responsive">
                            <table style="width:80%;">
                              <tr>
                                  <td>
                                       <%--<h6 class="card-title text-muted text-uppercase font-weight-bold">Select Quarter</h6>--%>
                                  </td>
                              </tr>
                             <tr>
                                 <td>
                                        <asp:DropDownList ID="ddlQYear" runat="server" CssClass="form-control" AutoPostBack="true"  OnSelectedIndexChanged="DDLQuarterYear_SelectedIndexChanged">
                                        </asp:DropDownList>
                                     <asp:Label ID="lblQ1" runat="server" Text="From" Visible="false"></asp:Label>
                                      <asp:DropDownList ID="ddlyear1" runat="server" CssClass="form-control" AutoPostBack="false" Visible="false" >
                                        <asp:ListItem Value="2023" Text="2023"> </asp:ListItem>
                                        <asp:ListItem Value="2024" Text="2024"> </asp:ListItem>
                                        <asp:ListItem Value="2025" Text="2025"> </asp:ListItem>
                                        <asp:ListItem Value="2026" Text="2026"> </asp:ListItem>
                                      </asp:DropDownList>

                                 </td>
                                 <td>&nbsp;</td>
                                 <td>
                                        <asp:DropDownList ID="ddlQuarter" runat="server" CssClass="form-control">                                           
                                        </asp:DropDownList>
                                     <asp:Label ID="lblQ2" runat="server" Text="To" Visible="false"></asp:Label>
                                      <asp:DropDownList ID="ddlyear2" runat="server" CssClass="form-control" AutoPostBack="false" Visible="false" >
                                        <asp:ListItem Value="2023" Text="2023"> </asp:ListItem>
                                        <asp:ListItem Value="2024" Text="2024"> </asp:ListItem>
                                        <asp:ListItem Value="2025" Text="2025"> </asp:ListItem>
                                        <asp:ListItem Value="2026" Text="2026"> </asp:ListItem>
                                      </asp:DropDownList>
                                     
                                 </td>
                                 <td>&nbsp;</td>
                                 <td>
                                     <asp:Button ID="btnQuarterSubmit" runat="server" CssClass="btn btn-primary" Text="Submit" OnClick="BtnQuarterSubmit_Click" />
                                 </td>
                             </tr>
                            </table>
                                </div>
                            </div> 
                         </div>
                    </div>

                <div class="col-md-6 col-xl-6 mb-4" id="ByHalfYear" runat="server">
                     <div class="card">
                        <div class="card-body" >  
                            
                            <div style="padding:5px">
                                <asp:radiobuttonlist runat="server" CssClass="form-control" ID="rbllist4" RepeatDirection="Horizontal" AutoPostBack="true" OnSelectedIndexChanged="rbllist4_SelectedIndexChanged">
                                    <asp:listitem Value="General" Selected="True">General</asp:listitem>
                                    <asp:listitem Value="Date Range">Date Range</asp:listitem>
                                </asp:radiobuttonlist>
                            </div>
                             <br />
                          
                            <div class="table-responsive">
                            <table style="width:80%;">
                              <tr>
                                  <td>
                                       <%--<h6 class="card-title text-muted text-uppercase font-weight-bold">Select HalfYear</h6>--%>
                                  </td>
                              </tr>
                             <tr>
                                 <td>
                                        <asp:DropDownList ID="ddlHalfYear" runat="server" CssClass="form-control" AutoPostBack="true"  OnSelectedIndexChanged="DDLHalfYear_SelectedIndexChanged">
                                        </asp:DropDownList>
                                     <asp:Label ID="lblH1" runat="server" Text="From" Visible="false"></asp:Label>
                                      <asp:DropDownList ID="ddlyear3" runat="server" CssClass="form-control" AutoPostBack="false"  Visible="false" >
                                        <asp:ListItem Value="2023" Text="2023"> </asp:ListItem>
                                        <asp:ListItem Value="2024" Text="2024"> </asp:ListItem>
                                        <asp:ListItem Value="2025" Text="2025"> </asp:ListItem>
                                        <asp:ListItem Value="2026" Text="2026"> </asp:ListItem>
                                      </asp:DropDownList>
                                       
                                 </td>
                                 <td>&nbsp;</td>
                                 <td>
                                        <asp:DropDownList ID="ddlHalfyearperiod" runat="server" CssClass="form-control">                                           
                                        </asp:DropDownList>
                                     <asp:Label ID="lblH2" runat="server" Text="To" Visible="false"></asp:Label>
                                     <asp:DropDownList ID="ddlyear4" runat="server" CssClass="form-control" AutoPostBack="false"  Visible="false" >
                                        <asp:ListItem Value="2023" Text="2023"> </asp:ListItem>
                                        <asp:ListItem Value="2024" Text="2024"> </asp:ListItem>
                                        <asp:ListItem Value="2025" Text="2025"> </asp:ListItem>
                                        <asp:ListItem Value="2026" Text="2026"> </asp:ListItem>
                                      </asp:DropDownList>
                                     
                                 </td>
                                 <td>&nbsp;</td>
                                 <td>
                                     <asp:Button ID="BtnHalfyearSubmit" runat="server" CssClass="btn btn-primary" Text="Submit" OnClick="BtnHalfYearSubmit_Click" />
                                 </td>
                             </tr>
                            </table>
                                </div>
                            </div>
                         </div>
                    </div>
                
                <div class="col-md-6 col-xl-6 mb-4" id="ByActivity" runat="server">
                     <div class="card">
                        <div class="card-body" >   
                            <div class="table-responsive">
                            <table style="width:100%;">
                              <tr>
                                  <td>
                                       <h6 class="card-title text-muted text-uppercase font-weight-bold">Select Activity</h6>
                                  </td>
                              </tr>
                             <tr>
                                 <td>
                                        <asp:DropDownList ID="DDLActivity" runat="server" CssClass="form-control" Width="60%" AutoPostBack="true" OnSelectedIndexChanged="DDLActivity_SelectedIndexChanged">
                                        </asp:DropDownList>
                                 </td>
                             </tr>
                            </table>
                                </div>
                            </div>
                         </div>
                    </div>

                <div class="col-md-6 col-xl-6 mb-4" id="AllMilestoneGraph" runat="server">
                     <div class="card">
                            <div class="card-body" style="height:140px" >   
                              <asp:Button id="btnGraphSubmit" runat="server" class="btn btn-primary" style="margin-top:40px" Text="Submit"  OnClick="btnGraphSubmit_Click"/>
                            </div>
                         </div>
                     </div>
                </div>
     </div>

    <div class="container-fluid" id="MonthlyPhysicalProgress" runat="server">
        <div class="row">
            <div class="col-lg-12 col-xl-12 form-group">
            <div class="card mb-4">
                                    <div class="card-body">

                                        <div class="col-lg-12 col-xl-12 form-group" align="center">
                                            <h5 id="MonthlyProgressReportName" style="font-weight:bold;" runat="server">Monthly Progress Report</h5>
                                            <h5 id="MonthlyProgressProjectName" runat="server">Project Name</h5>
                                            </div>

                                        <div class="card-title">
                                            <div class="d-flex justify-content-between">
                                                <h6 class="text-muted">
                                                    <%--<asp:Label ID="ActivityHeading" CssClass="text-uppercase font-weight-bold" runat="server" Text="Physical Progress Achievements in the month" />--%>
                                                </h6>

                                                <div>
                                                    <asp:Button ID="btnMonthlyProgressExporttoExcel" runat="server" Text="Export Excel" Visible="false" CssClass="btn btn-primary" OnClick="btnMonthlyProgressExporttoExcel_Click" />
                                                    <asp:Button ID="btnMonthlyProgressExportPDF" runat="server" Text="Export PDF" Visible="false" CssClass="btn btn-primary" OnClick="btnMonthlyProgressExportPDF_Click" />
                                                    <asp:Button ID="btnMonthlyProgressPrint" runat="server" Text="Print" Visible="false" CssClass="btn btn-primary" OnClick="btnMonthlyProgressPrint_Click" />
                                            </div>
                                            </div>
                                        </div>
                                     
                                        <div class="table-responsive gvWidthHight">
                                                <asp:GridView ID="GrdDailyPhysicalProgress" runat="server" HeaderStyle-HorizontalAlign="Center" DataKeyNames="Range" AutoGenerateColumns="False" EmptyDataText="No Data Found" AlternatingRowStyle-BackColor="LightGray" CssClass="table table-bordered" OnDataBound="GrdDailyPhysicalProgress_DataBound" OnRowDataBound="GrdDailyPhysicalProgress_RowDataBound">
                                                    <Columns>
                                                        <asp:TemplateField HeaderText="Sl.No" ItemStyle-HorizontalAlign="Center">
                                                            <ItemTemplate>
                                                                <%# Container.DataItemIndex + 1 %>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                                        </asp:TemplateField>
                                                       <%--  <asp:TemplateField HeaderText="Milestone" ItemStyle-HorizontalAlign="Left" HeaderStyle-CssClass="text-left">
                                                            <ItemTemplate>
                                                                <%#Eval("MilestoneName")%>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>--%>

                                                        <asp:BoundField DataField="Range" HeaderText="Date Range" ItemStyle-HorizontalAlign="Left" />

                                                      <%--  <asp:TemplateField HeaderText="Description of Work (Level-1)" ItemStyle-HorizontalAlign="Left" HeaderStyle-CssClass="text-left">
                                                            <ItemTemplate>
                                                                <%#Eval("MilestoneName")%>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                         <asp:TemplateField HeaderText="Description of Work (Level-2)" ItemStyle-HorizontalAlign="Left" HeaderStyle-CssClass="text-left">
                                                            <ItemTemplate>
                                                                <%#Eval("MilestoneName")%>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                         <asp:TemplateField HeaderText="Description of Work (Level-3)" ItemStyle-HorizontalAlign="Left" HeaderStyle-CssClass="text-left">
                                                            <ItemTemplate>
                                                                <%#Eval("MilestoneName")%>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                         <asp:TemplateField HeaderText="Description of Work (Level-4)" ItemStyle-HorizontalAlign="Left" HeaderStyle-CssClass="text-left">
                                                            <ItemTemplate>
                                                                <%#Eval("MilestoneName")%>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                         <asp:TemplateField HeaderText="Description of Work (Level-5)" ItemStyle-HorizontalAlign="Left" HeaderStyle-CssClass="text-left">
                                                            <ItemTemplate>
                                                                <%#Eval("MilestoneName")%>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                         <asp:TemplateField HeaderText="Description of Work (Level-6)" ItemStyle-HorizontalAlign="Left" HeaderStyle-CssClass="text-left">
                                                            <ItemTemplate>
                                                                <%#Eval("MilestoneName")%>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Description of Work (Level-7)" ItemStyle-HorizontalAlign="Left" HeaderStyle-CssClass="text-left">
                                                            <ItemTemplate>
                                                                <%#Eval("MilestoneName")%>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>--%>
                                                        
                                                        <asp:BoundField DataField="UnitforProgress" HeaderText="UOM" ItemStyle-HorizontalAlign="Center" />
                                                        <asp:BoundField DataField="UnitQuantity" HeaderText="Scope as per BOQ" ItemStyle-HorizontalAlign="Center" />
                                                       
                                                        <asp:BoundField DataField="Achieved" HeaderText="Achieved" ItemStyle-HorizontalAlign="Center" />
                                                        <asp:BoundField DataField="Planned" HeaderText="Planned" ItemStyle-HorizontalAlign="Center" />
                                                        <asp:BoundField DataField="Achieved" HeaderText="Achieved" ItemStyle-HorizontalAlign="Center" />
                                                        <asp:BoundField DataField="Achieved" HeaderText="Achieved%" ItemStyle-HorizontalAlign="Center" />
                                                        <asp:BoundField DataField="Planned" HeaderText="Planned" ItemStyle-HorizontalAlign="Center" />
                                                        <asp:BoundField DataField="Achieved" HeaderText="Achieved" ItemStyle-HorizontalAlign="Center" />
                                                        <asp:BoundField DataField="Achieved" HeaderText="Balance" ItemStyle-HorizontalAlign="Center" />
                                                        <asp:BoundField DataField="Planned" HeaderText="Plan for the Next Month" ItemStyle-HorizontalAlign="Center" />
                                                        <asp:BoundField DataField="Planned" HeaderText="Overall % of Completion" ItemStyle-HorizontalAlign="Center" />
                                                       
                                                       
                                                         <%--<asp:BoundField DataField="TaskLevel" HeaderText="Level" HeaderStyle-CssClass="hideItem" ItemStyle-CssClass="hideItem"  />--%>
                                                </Columns>
                                                </asp:GridView>

                                                <asp:GridView ID="GrdMonthlyPhysicalProgress" runat="server" HeaderStyle-HorizontalAlign="Center" AutoGenerateColumns="False" EmptyDataText="No Data Found" AlternatingRowStyle-BackColor="LightGray" CssClass="table table-bordered" OnDataBound="GrdMonthlyPhysicalProgress_DataBound" OnRowDataBound="GrdMonthlyPhysicalProgress_RowDataBound">
                                                    <Columns>
                                                        <asp:TemplateField HeaderText="Sl.No" ItemStyle-HorizontalAlign="Center">
                                                            <ItemTemplate>
                                                                <%# Container.DataItemIndex + 1 %>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                                        </asp:TemplateField>
                                                       <%--  <asp:TemplateField HeaderText="Milestone" ItemStyle-HorizontalAlign="Left" HeaderStyle-CssClass="text-left">
                                                            <ItemTemplate>
                                                                <%#Eval("MilestoneName")%>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>--%>
                                                        
                                                         <asp:BoundField DataField="Range" HeaderText="Month Range" ItemStyle-HorizontalAlign="Left" />

                                                      <%--  <asp:TemplateField HeaderText="Description of Work (Level-1)" ItemStyle-HorizontalAlign="Left" HeaderStyle-CssClass="text-left">
                                                            <ItemTemplate>
                                                                <%#Eval("MilestoneName")%>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                         <asp:TemplateField HeaderText="Description of Work (Level-2)" ItemStyle-HorizontalAlign="Left" HeaderStyle-CssClass="text-left">
                                                            <ItemTemplate>
                                                                <%#Eval("MilestoneName")%>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                         <asp:TemplateField HeaderText="Description of Work (Level-3)" ItemStyle-HorizontalAlign="Left" HeaderStyle-CssClass="text-left">
                                                            <ItemTemplate>
                                                                <%#Eval("MilestoneName")%>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                         <asp:TemplateField HeaderText="Description of Work (Level-4)" ItemStyle-HorizontalAlign="Left" HeaderStyle-CssClass="text-left">
                                                            <ItemTemplate>
                                                                <%#Eval("MilestoneName")%>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                         <asp:TemplateField HeaderText="Description of Work (Level-5)" ItemStyle-HorizontalAlign="Left" HeaderStyle-CssClass="text-left">
                                                            <ItemTemplate>
                                                                <%#Eval("MilestoneName")%>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                         <asp:TemplateField HeaderText="Description of Work (Level-6)" ItemStyle-HorizontalAlign="Left" HeaderStyle-CssClass="text-left">
                                                            <ItemTemplate>
                                                                <%#Eval("MilestoneName")%>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Description of Work (Level-7)" ItemStyle-HorizontalAlign="Left" HeaderStyle-CssClass="text-left">
                                                            <ItemTemplate>
                                                                <%#Eval("MilestoneName")%>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>--%>
                                                        
                                                        <asp:BoundField DataField="UnitforProgress" HeaderText="UOM" ItemStyle-HorizontalAlign="Center" />
                                                        <asp:BoundField DataField="UnitQuantity" HeaderText="Scope as per BOQ" ItemStyle-HorizontalAlign="Center" />
                                                       
                                                        <asp:BoundField DataField="Achieved" HeaderText="Achieved" ItemStyle-HorizontalAlign="Center" />
                                                        <asp:BoundField DataField="Planned" HeaderText="Planned" ItemStyle-HorizontalAlign="Center" />
                                                        <asp:BoundField DataField="Achieved" HeaderText="Achieved" ItemStyle-HorizontalAlign="Center" />
                                                        <asp:BoundField DataField="Achieved" HeaderText="Achieved%" ItemStyle-HorizontalAlign="Center" />
                                                        <asp:BoundField DataField="Planned" HeaderText="Planned" ItemStyle-HorizontalAlign="Center" />
                                                        <asp:BoundField DataField="Achieved" HeaderText="Achieved" ItemStyle-HorizontalAlign="Center" />
                                                        <asp:BoundField DataField="Achieved" HeaderText="Balance" ItemStyle-HorizontalAlign="Center" />
                                                        <asp:BoundField DataField="Planned" HeaderText="Plan for the Next Month" ItemStyle-HorizontalAlign="Center" />
                                                        <asp:BoundField DataField="Planned" HeaderText="Overall % of Completion" ItemStyle-HorizontalAlign="Center" />
                                                        <%--<asp:BoundField DataField="Year" HeaderText="Year" ItemStyle-HorizontalAlign="Left" HeaderStyle-CssClass="hideItem" ItemStyle-CssClass="hideItem" />--%>
                                                       
                                                         <%--<asp:BoundField DataField="TaskLevel" HeaderText="Level" HeaderStyle-CssClass="hideItem" ItemStyle-CssClass="hideItem"  />--%>
                                                </Columns>
                                                </asp:GridView>


                                                <asp:GridView ID="GrdAllMilestones" runat="server" HeaderStyle-HorizontalAlign="Center" DataKeyNames="MilestoneUID" AutoGenerateColumns="False" EmptyDataText="No Data Found" AlternatingRowStyle-BackColor="LightGray" CssClass="table table-bordered" OnDataBound="GrdAllMilestones_DataBound" OnRowDataBound="GrdAllMilestones_RowDataBound">
                                                    <Columns>
                                                        <asp:TemplateField HeaderText="Sl.No" ItemStyle-HorizontalAlign="Center">
                                                            <ItemTemplate>
                                                                <%# Container.DataItemIndex + 1 %>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                                        </asp:TemplateField>
                                                         <asp:TemplateField HeaderText="Milestone" ItemStyle-HorizontalAlign="Left" HeaderStyle-CssClass="text-left">
                                                            <ItemTemplate>
                                                                <%#Eval("MilestoneName")%>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                         <asp:TemplateField HeaderText="Description of Work (Level-1)" ItemStyle-HorizontalAlign="Left" HeaderStyle-CssClass="text-left">
                                                            <ItemTemplate>
                                                                <%#Eval("MilestoneName")%>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                         <asp:TemplateField HeaderText="Description of Work (Level-2)" ItemStyle-HorizontalAlign="Left" HeaderStyle-CssClass="text-left">
                                                            <ItemTemplate>
                                                                <%#Eval("MilestoneName")%>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                         <asp:TemplateField HeaderText="Description of Work (Level-3)" ItemStyle-HorizontalAlign="Left" HeaderStyle-CssClass="text-left">
                                                            <ItemTemplate>
                                                                <%#Eval("MilestoneName")%>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                         <asp:TemplateField HeaderText="Description of Work (Level-4)" ItemStyle-HorizontalAlign="Left" HeaderStyle-CssClass="text-left">
                                                            <ItemTemplate>
                                                                <%#Eval("MilestoneName")%>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                         <asp:TemplateField HeaderText="Description of Work (Level-5)" ItemStyle-HorizontalAlign="Left" HeaderStyle-CssClass="text-left">
                                                            <ItemTemplate>
                                                                <%#Eval("MilestoneName")%>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                         <asp:TemplateField HeaderText="Description of Work (Level-6)" ItemStyle-HorizontalAlign="Left" HeaderStyle-CssClass="text-left">
                                                            <ItemTemplate>
                                                                <%#Eval("MilestoneName")%>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Description of Work (Level-7)" ItemStyle-HorizontalAlign="Left" HeaderStyle-CssClass="text-left">
                                                            <ItemTemplate>
                                                                <%#Eval("MilestoneName")%>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        
                                                        <asp:BoundField DataField="UnitforProgress" HeaderText="UOM" ItemStyle-HorizontalAlign="Center" />
                                                        <asp:BoundField DataField="UnitQuantity" HeaderText="Scope as per BOQ" ItemStyle-HorizontalAlign="Center" />
                                                       
                                                        <asp:BoundField DataField="Achieved" HeaderText="Achieved" ItemStyle-HorizontalAlign="Center" />
                                                        <asp:BoundField DataField="Planned" HeaderText="Planned" ItemStyle-HorizontalAlign="Center" />
                                                        <asp:BoundField DataField="Achieved" HeaderText="Achieved" ItemStyle-HorizontalAlign="Center" />
                                                        <asp:BoundField DataField="Achieved" HeaderText="Achieved%" ItemStyle-HorizontalAlign="Center" />
                                                        <asp:BoundField DataField="Planned" HeaderText="Planned" ItemStyle-HorizontalAlign="Center" />
                                                        <asp:BoundField DataField="Achieved" HeaderText="Achieved" ItemStyle-HorizontalAlign="Center" />
                                                        <asp:BoundField DataField="Achieved" HeaderText="Balance" ItemStyle-HorizontalAlign="Center" />
                                                        <asp:BoundField DataField="Planned" HeaderText="Plan for the Next Month" ItemStyle-HorizontalAlign="Center" />
                                                        <asp:BoundField DataField="Planned" HeaderText="Overall % of Completion" ItemStyle-HorizontalAlign="Center" />
                                                       
                                                       
                                                         <%--<asp:BoundField DataField="TaskLevel" HeaderText="Level" HeaderStyle-CssClass="hideItem" ItemStyle-CssClass="hideItem"  />--%>
                                                </Columns>
                                                </asp:GridView>

                                            </div>
                                                
                                          </div>
                                       
                       
                    </div>
                </div>
        </div>
        </div>
    <div class="container-fluid" id="QuarterPhysicalProgress" runat="server">
        <div class="row">
            <div class="col-lg-12 col-xl-12 form-group">
            <div class="card mb-4">
                                    <div class="card-body">

                                        <div class="col-lg-12 col-xl-12 form-group" align="center">
                                            <h5 id="QuarterlyProgressReportName" style="font-weight:bold;" runat="server">Quarterly Progress Report</h5>
                                            <h5 id="QuarterlyProjectName" runat="server">Project Name</h5>
                                        </div>

                                        <div class="card-title">
                                            <div class="d-flex justify-content-between">
                                                <h6 class="text-muted">
                                                    <%--<asp:Label ID="ActivityHeading" CssClass="text-uppercase font-weight-bold" runat="server" Text="Physical Progress Achievements in the month" />--%>
                                                </h6>

                                                <div>
                                                    <asp:Button ID="btnQuarterProgressExporttoExcel" runat="server" Text="Export Excel" Visible="false" CssClass="btn btn-primary" OnClick="btnQuarterProgressExporttoExcel_Click" />
                                                    <asp:Button ID="btnQuarterProgressExportPDF" runat="server" Text="Export PDF" Visible="false" CssClass="btn btn-primary" OnClick="btnQuarterProgressExportPDF_Click" />
                                                    <asp:Button ID="btnQuarterProgressPrint" runat="server" Text="Print" Visible="false" CssClass="btn btn-primary" OnClick="btnQuarterProgressPrint_Click" />
                                            </div>
                                            </div>
                                        </div>
                                        <div class="table-responsive gvWidthHight">
                                                <asp:GridView ID="GrdQuarterPhysicalProgress" runat="server" HeaderStyle-HorizontalAlign="Center" DataKeyNames="TaskUID" AutoGenerateColumns="False" EmptyDataText="No Data Found" Width="100%" AlternatingRowStyle-BackColor="LightGray" CssClass="table table-bordered" OnDataBound="GrdQuarterPhysicalProgress_DataBound" OnRowDataBound="GrdQuarterPhysicalProgress_RowDataBound">
                                                    <Columns>
                                                        <asp:TemplateField HeaderText="Sl.No" ItemStyle-HorizontalAlign="Center">
                                                            <ItemTemplate>
                                                                <%# Container.DataItemIndex + 1 %>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                                        </asp:TemplateField>
                                                         <asp:TemplateField HeaderText="Milestone" ItemStyle-HorizontalAlign="Left" HeaderStyle-CssClass="text-left">
                                                            <ItemTemplate>
                                                                <%#Eval("MilestoneName")%>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Description of Work (Level-1)" ItemStyle-HorizontalAlign="Left" HeaderStyle-CssClass="text-left">
                                                            <ItemTemplate>
                                                                <%#Eval("Name")%>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                      <asp:TemplateField HeaderText="Description of Work (Level-2)" ItemStyle-HorizontalAlign="Left" HeaderStyle-CssClass="text-left">
                                                            <ItemTemplate>
                                                                <%#Eval("Name")%>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                         <asp:TemplateField HeaderText="Description of Work (Level-3)" ItemStyle-HorizontalAlign="Left" HeaderStyle-CssClass="text-left">
                                                            <ItemTemplate>
                                                                <%#Eval("Name")%>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                         <asp:TemplateField HeaderText="Description of Work (Level-4)" ItemStyle-HorizontalAlign="Left" HeaderStyle-CssClass="text-left">
                                                            <ItemTemplate>
                                                                <%#Eval("Name")%>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                         <asp:TemplateField HeaderText="Description of Work (Level-5)" ItemStyle-HorizontalAlign="Left" HeaderStyle-CssClass="text-left">
                                                            <ItemTemplate>
                                                                <%#Eval("Name")%>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                         <asp:TemplateField HeaderText="Description of Work (Level-6)" ItemStyle-HorizontalAlign="Left" HeaderStyle-CssClass="text-left">
                                                            <ItemTemplate>
                                                                <%#Eval("Name")%>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Description of Work (Level-7)" ItemStyle-HorizontalAlign="Left" HeaderStyle-CssClass="text-left">
                                                            <ItemTemplate>
                                                                <%#Eval("Name")%>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:BoundField DataField="UnitforProgress" HeaderText="UOM" ItemStyle-HorizontalAlign="Center" />

                                                        <asp:BoundField DataField="UnitQuantity" HeaderText="Scope as per BOQ" ItemStyle-HorizontalAlign="Center" />
                                                       
                                                        <asp:BoundField DataField="TaskUID" HeaderText="Achieved" ItemStyle-HorizontalAlign="Center" />
                                                        <asp:BoundField DataField="TaskUID" HeaderText="Planned" ItemStyle-HorizontalAlign="Center" />
                                                        <asp:BoundField DataField="TaskUID" HeaderText="Achieved" ItemStyle-HorizontalAlign="Center" />
                                                        <asp:BoundField DataField="TaskUID" HeaderText="Achieved%" ItemStyle-HorizontalAlign="Center" />
                                                        <asp:BoundField DataField="TaskUID" HeaderText="Planned" ItemStyle-HorizontalAlign="Center" />
                                                        <asp:BoundField DataField="TaskUID" HeaderText="Achieved" ItemStyle-HorizontalAlign="Center" />
                                                        <asp:BoundField DataField="TaskUID" HeaderText="Balance" ItemStyle-HorizontalAlign="Center" />
                                                        <asp:BoundField DataField="TaskUID" HeaderText="Plan for the Next Month" ItemStyle-HorizontalAlign="Center" />
                                                        <asp:BoundField DataField="TaskUID" HeaderText="Overall % of Completion" ItemStyle-HorizontalAlign="Center" />
                                                         <asp:BoundField DataField="TaskLevel" HeaderText="Level" HeaderStyle-CssClass="hideItem" ItemStyle-CssClass="hideItem"  />
                                                </Columns>
                                                </asp:GridView>
                                            </div>
                       </div>
                    </div>
                </div>
        </div>
        </div>


    <div class="container-fluid" id="HalfYearPhysicalProgress" runat="server">
        <div class="row">
            <div class="col-lg-12 col-xl-12 form-group">
            <div class="card mb-4">
                                    <div class="card-body">

                                       <div class="col-lg-12 col-xl-12 form-group" align="center">
                                            <h5 id="HalfYearlyReportHeading" style="font-weight:bold;" runat="server">Half Yearly Progress Report</h5>
                                            <h5 id="HalfYearlyProjectName" runat="server">Project Name</h5>
                                       </div>

                                        <div class="card-title">
                                            <div class="d-flex justify-content-between">
                                                <h6 class="text-muted">
                                                    <%--<asp:Label ID="ActivityHeading" CssClass="text-uppercase font-weight-bold" runat="server" Text="Physical Progress Achievements in the month" />--%>
                                                </h6>

                                                <div>
                                                   <asp:Button ID="btnHalfYearProgressExporttoExcel" runat="server" Text="Export Excel" Visible="false" CssClass="btn btn-primary" OnClick="btnHalfYearProgressExporttoExcel_Click" />
                                                    <asp:Button ID="btnHalfYearProgressExportPDF" runat="server" Text="Export PDF" Visible="false" CssClass="btn btn-primary" OnClick="btnHalfYearProgressExportPDF_Click" />
                                                    <asp:Button ID="btnHalfYearProgressPrint" runat="server" Text="Print" Visible="false" CssClass="btn btn-primary" OnClick="btnHalfYearProgressPrint_Click" />
                                            </div>
                                            </div>
                                        </div>
                                        <div class="table-responsive gvWidthHight">
                                                <asp:GridView ID="GrdHalfYearPhysicalProgress" runat="server" HeaderStyle-HorizontalAlign="Center" DataKeyNames="TaskUID" AutoGenerateColumns="False" EmptyDataText="No Data Found" Width="100%" AlternatingRowStyle-BackColor="LightGray" CssClass="table table-bordered" OnDataBound="GrdHalfYearPhysicalProgress_DataBound" OnRowDataBound="GrdHalfYearPhysicalProgress_RowDataBound">
                                                    <Columns>
                                                        <asp:TemplateField HeaderText="Sl.No" ItemStyle-HorizontalAlign="Center">
                                                            <ItemTemplate>
                                                                <%# Container.DataItemIndex + 1 %>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Milestone" ItemStyle-HorizontalAlign="Left" HeaderStyle-CssClass="text-left">
                                                            <ItemTemplate>
                                                                <%#Eval("MilestoneName")%>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                         <asp:TemplateField HeaderText="Description of Work (Level-1)" ItemStyle-HorizontalAlign="Left" HeaderStyle-CssClass="text-left">
                                                            <ItemTemplate>
                                                                <%#Eval("Name")%>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                      <asp:TemplateField HeaderText="Description of Work (Level-2)" ItemStyle-HorizontalAlign="Left" HeaderStyle-CssClass="text-left">
                                                            <ItemTemplate>
                                                                <%#Eval("Name")%>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                         <asp:TemplateField HeaderText="Description of Work (Level-3)" ItemStyle-HorizontalAlign="Left" HeaderStyle-CssClass="text-left">
                                                            <ItemTemplate>
                                                                <%#Eval("Name")%>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                         <asp:TemplateField HeaderText="Description of Work (Level-4)" ItemStyle-HorizontalAlign="Left" HeaderStyle-CssClass="text-left">
                                                            <ItemTemplate>
                                                                <%#Eval("Name")%>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                         <asp:TemplateField HeaderText="Description of Work (Level-5)" ItemStyle-HorizontalAlign="Left" HeaderStyle-CssClass="text-left">
                                                            <ItemTemplate>
                                                                <%#Eval("Name")%>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                         <asp:TemplateField HeaderText="Description of Work (Level-6)" ItemStyle-HorizontalAlign="Left" HeaderStyle-CssClass="text-left">
                                                            <ItemTemplate>
                                                                <%#Eval("Name")%>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Description of Work (Level-7)" ItemStyle-HorizontalAlign="Left" HeaderStyle-CssClass="text-left">
                                                            <ItemTemplate>
                                                                <%#Eval("Name")%>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:BoundField DataField="UnitforProgress" HeaderText="UOM" ItemStyle-HorizontalAlign="Center" />

                                                        <asp:BoundField DataField="UnitQuantity" HeaderText="Scope as per BOQ" ItemStyle-HorizontalAlign="Center" />
                                                       
                                                        <asp:BoundField DataField="TaskUID" HeaderText="Achieved" ItemStyle-HorizontalAlign="Center" />
                                                        <asp:BoundField DataField="TaskUID" HeaderText="Planned" ItemStyle-HorizontalAlign="Center" />
                                                        <asp:BoundField DataField="TaskUID" HeaderText="Achieved" ItemStyle-HorizontalAlign="Center" />
                                                        <asp:BoundField DataField="TaskUID" HeaderText="Achieved%" ItemStyle-HorizontalAlign="Center" />
                                                        <asp:BoundField DataField="TaskUID" HeaderText="Planned" ItemStyle-HorizontalAlign="Center" />
                                                        <asp:BoundField DataField="TaskUID" HeaderText="Achieved" ItemStyle-HorizontalAlign="Center" />
                                                        <asp:BoundField DataField="TaskUID" HeaderText="Balance" ItemStyle-HorizontalAlign="Center" />
                                                        <asp:BoundField DataField="TaskUID" HeaderText="Plan for the Next Month" ItemStyle-HorizontalAlign="Center" />
                                                        <asp:BoundField DataField="TaskUID" HeaderText="Overall % of Completion" ItemStyle-HorizontalAlign="Center" />
                                                         <asp:BoundField DataField="TaskLevel" HeaderText="Level" HeaderStyle-CssClass="hideItem" ItemStyle-CssClass="hideItem"  />
                                                </Columns>
                                                </asp:GridView>
                                            </div>
                       </div>
                    </div>
                </div>
        </div>
        </div>
    <div class="container-fluid" id="WeeklyProgressReport" runat="server">
        <div class="row">
            <div class="col-lg-12 col-xl-12 form-group">
            <div class="card mb-4">
                                    <div class="card-body">
                                        <div class="col-lg-12 col-xl-12 form-group" align="center">
                                            <h5 id="WeeklyReportNameHeading" style="font-weight:bold;" runat="server">Weekly Progress Report</h5>
                                            <h5 id="WeeklyProjectName" runat="server">Project Name</h5>
                                            </div>

                                        <div class="card-title">
                                            <div class="d-flex justify-content-between">
                                                <h6 class="text-muted">
                                                    <%--<asp:Label ID="LblWeeklyHeading" CssClass="text-uppercase font-weight-bold" runat="server" Text="Physical Progress Achievements in the month" />--%>
                                                </h6>
                                                <div>
                                                     <asp:Button ID="btnExportReportExcel" runat="server" Text="Export Excel" Visible="false" CssClass="btn btn-primary" OnClick="btnExportReportExcel_Click" />
                                                    <asp:Button ID="btnExportReportPDF" runat="server" Text="Export PDF" Visible="false" CssClass="btn btn-primary" OnClick="btnExportReportPDF_Click" />
                                                    <asp:Button ID="btnPrintPDF" runat="server" Text="Print" Visible="false" CssClass="btn btn-primary" OnClick="btnPrintPDF_Click" />
                                            </div>
                                            </div>
                                        </div>
                                        <div class="table-responsive gvWidthHight">
                                                <asp:GridView ID="GrdWeeklyprogress" runat="server" HeaderStyle-HorizontalAlign="Center" DataKeyNames="TaskUID" AutoGenerateColumns="False" EmptyDataText="No Data Found" AlternatingRowStyle-BackColor="LightGray" Width="100%" CssClass="table table-bordered" OnDataBound="GrdWeeklyprogress_DataBound" OnRowDataBound="GrdWeeklyprogress_RowDataBound">
                                                    <Columns>
                                                        <asp:TemplateField HeaderText="Sl.No" ItemStyle-HorizontalAlign="Center">
                                                            <ItemTemplate>
                                                                <%# Container.DataItemIndex + 1 %>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Milestone" ItemStyle-HorizontalAlign="Left" HeaderStyle-CssClass="text-left">
                                                            <ItemTemplate>
                                                                <%#Eval("MilestoneName")%>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Description of Work (Level-1)" ItemStyle-HorizontalAlign="Left" HeaderStyle-CssClass="text-left">
                                                            <ItemTemplate>
                                                                <%#Eval("Name")%>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Description of Work (Level-2)" ItemStyle-HorizontalAlign="Left" HeaderStyle-CssClass="text-left">
                                                            <ItemTemplate>
                                                                <%#Eval("Name")%>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                         <asp:TemplateField HeaderText="Description of Work (Level-3)" ItemStyle-HorizontalAlign="Left" HeaderStyle-CssClass="text-left">
                                                            <ItemTemplate>
                                                                <%#Eval("Name")%>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                         <asp:TemplateField HeaderText="Description of Work (Level-4)" ItemStyle-HorizontalAlign="Left" HeaderStyle-CssClass="text-left">
                                                            <ItemTemplate>
                                                                <%#Eval("Name")%>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                         <asp:TemplateField HeaderText="Description of Work (Level-5)" ItemStyle-HorizontalAlign="Left" HeaderStyle-CssClass="text-left">
                                                            <ItemTemplate>
                                                                <%#Eval("Name")%>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                         <asp:TemplateField HeaderText="Description of Work (Level-6)" ItemStyle-HorizontalAlign="Left" HeaderStyle-CssClass="text-left">
                                                            <ItemTemplate>
                                                                <%#Eval("Name")%>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Description of Work (Level-7)" ItemStyle-HorizontalAlign="Left" HeaderStyle-CssClass="text-left">
                                                            <ItemTemplate>
                                                                <%#Eval("Name")%>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="UOM" ItemStyle-HorizontalAlign="Center">
                                                            <ItemTemplate>
                                                                <%#Eval("UnitforProgress")%>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>

                                                        <asp:BoundField DataField="TaskUID" HeaderText="Cumulative" ItemStyle-HorizontalAlign="Center" />
                                                        <asp:BoundField DataField="TaskUID" HeaderText="For the week" ItemStyle-HorizontalAlign="Center" />
                                                        <asp:BoundField DataField="TaskUID" HeaderText="Cumulative" ItemStyle-HorizontalAlign="Center" />
                                                        <asp:BoundField DataField="TaskUID" HeaderText="For the Week" ItemStyle-HorizontalAlign="Center" />
                                                        <asp:BoundField DataField="TaskUID" HeaderText="% of Progress Cumulative" ItemStyle-HorizontalAlign="Center" />
                                                        
                                                        <asp:BoundField DataField="TaskLevel" HeaderText="Level" HeaderStyle-CssClass="hideItem" ItemStyle-CssClass="hideItem"  />
                                                </Columns>
                                                </asp:GridView>
                                            </div>
                       </div>
                    </div>
                </div>
        </div>
        </div>

    <div class="container-fluid" id="FortiProgressReport" runat="server">
        <div class="row">
            <div class="col-lg-12 col-xl-12 form-group">
            <div class="card mb-4">
                                    <div class="card-body">
                                        <div class="col-lg-12 col-xl-12 form-group" align="center">
                                            <h5 id="FortnightlyReportNameHeading" style="font-weight:bold;" runat="server">Fortnightly Progress Report</h5>
                                            <h5 id="FortnightlyProjectName" runat="server">Project Name</h5>
                                            </div>

                                        <div class="card-title">
                                            <div class="d-flex justify-content-between">
                                                <h6 class="text-muted">
                                                    <%--<asp:Label ID="LblWeeklyHeading" CssClass="text-uppercase font-weight-bold" runat="server" Text="Physical Progress Achievements in the month" />--%>
                                                </h6>
                                                <div>
                                                    <asp:Button ID="btnFortExportReportExcel" runat="server" Text="Export Excel" Visible="false" CssClass="btn btn-primary" OnClick="btnFortExportReportExcel_Click" />
                                                    <asp:Button ID="btnFortExportReportPDF" runat="server" Text="Export PDF" Visible="false" CssClass="btn btn-primary" OnClick="btnFortExportReportPDF_Click" />
                                                    <asp:Button ID="btnFortExportReportPrint" runat="server" Text="Print" Visible="false" CssClass="btn btn-primary" OnClick="btnFortExportReportPrint_Click" />
                                            </div>
                                            </div>
                                        </div>
                                        <div class="table-responsive gvWidthHight">
                                                <asp:GridView ID="grdFortiProgressReport" runat="server" HeaderStyle-HorizontalAlign="Center" DataKeyNames="TaskUID" AutoGenerateColumns="False" EmptyDataText="No Data Found" AlternatingRowStyle-BackColor="LightGray" Width="100%" CssClass="table table-bordered" OnDataBound="GrdFortyprogress_DataBound" OnRowDataBound="GrdFortprogress_RowDataBound">
                                                    <Columns>
                                                        <asp:TemplateField HeaderText="Sl.No" ItemStyle-HorizontalAlign="Center">
                                                            <ItemTemplate>
                                                                <%# Container.DataItemIndex + 1 %>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Center"></ItemStyle>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Milestone" ItemStyle-HorizontalAlign="Left" HeaderStyle-CssClass="text-left">
                                                            <ItemTemplate>
                                                                <%#Eval("MilestoneName")%>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Description of Work (Level-1)" ItemStyle-HorizontalAlign="Left" HeaderStyle-CssClass="text-left">
                                                            <ItemTemplate>
                                                                <%#Eval("Name")%>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Description of Work (Level-2)" ItemStyle-HorizontalAlign="Left" HeaderStyle-CssClass="text-left">
                                                            <ItemTemplate>
                                                                <%#Eval("Name")%>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                         <asp:TemplateField HeaderText="Description of Work (Level-3)" ItemStyle-HorizontalAlign="Left" HeaderStyle-CssClass="text-left">
                                                            <ItemTemplate>
                                                                <%#Eval("Name")%>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                         <asp:TemplateField HeaderText="Description of Work (Level-4)" ItemStyle-HorizontalAlign="Left" HeaderStyle-CssClass="text-left">
                                                            <ItemTemplate>
                                                                <%#Eval("Name")%>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                         <asp:TemplateField HeaderText="Description of Work (Level-5)" ItemStyle-HorizontalAlign="Left" HeaderStyle-CssClass="text-left">
                                                            <ItemTemplate>
                                                                <%#Eval("Name")%>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                         <asp:TemplateField HeaderText="Description of Work (Level-6)" ItemStyle-HorizontalAlign="Left" HeaderStyle-CssClass="text-left">
                                                            <ItemTemplate>
                                                                <%#Eval("Name")%>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Description of Work (Level-7)" ItemStyle-HorizontalAlign="Left" HeaderStyle-CssClass="text-left">
                                                            <ItemTemplate>
                                                                <%#Eval("Name")%>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="UOM" ItemStyle-HorizontalAlign="Center">
                                                            <ItemTemplate>
                                                                <%#Eval("UnitforProgress")%>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                        <asp:BoundField DataField="TaskUID" HeaderText="Cumulative" ItemStyle-HorizontalAlign="Center" />
                                                        <asp:BoundField DataField="TaskUID" HeaderText="For the week" ItemStyle-HorizontalAlign="Center" />
                                                        <asp:BoundField DataField="TaskUID" HeaderText="Cumulative" ItemStyle-HorizontalAlign="Center" />
                                                        <asp:BoundField DataField="TaskUID" HeaderText="For the Week" ItemStyle-HorizontalAlign="Center" />
                                                        <asp:BoundField DataField="TaskUID" HeaderText="% of Progress Cumulative" ItemStyle-HorizontalAlign="Center" />
                                                         <asp:BoundField DataField="TaskLevel" HeaderText="Level" HeaderStyle-CssClass="hideItem" ItemStyle-CssClass="hideItem"  />
                                                </Columns>
                                                </asp:GridView>
                                            </div>
                       </div>
                    </div>
                </div>
        </div>
        </div>

    <div class="container-fluid" id="AcrossMonthsProgressReport" runat="server">
        <div class="row">
            <div class="col-lg-12 col-xl-12 form-group">
                <div class="card mb-4">
                                    <div class="card-body">

                                        <div class="col-lg-12 col-xl-12 form-group" align="center">
                                            <h5 id="AcrossMonthReportName" style="font-weight:bold;" runat="server">Physical Progress Monitoring Sheet</h5>
                                            <h5 id="AcrossMonthProjectName" runat="server">Projecct Name</h5>
                                            </div>
                                        <div class="card-title">
                                            <div class="d-flex justify-content-between">
                                                <h6 class="text-muted">
                                                    <%--<asp:Label ID="LblPhysicalProgress" CssClass="text-uppercase font-weight-bold" runat="server" Text="Physical Progress Monitoring Sheet" />--%>
                                                </h6>
                                                <div>
                                                    <asp:Button ID="btnAcrossMonthsExportExcel" runat="server" Visible="false" Text="Export to Excel" CssClass="btn btn-primary" OnClick="btnAcrossMonthsExportExcel_Click" />
                                                </div>
                                            </div>
                                        </div>
                                        <div class="table-responsive" id="DivAcrossMonths" runat="server">
                                                
                                          </div>
                       </div>
                    </div>
                </div>
            </div>
        </div>

    <div class="container-fluid" id="ActivityProgressReport" runat="server">
        <div class="row">
            <div class="col-lg-12 col-xl-12 form-group">
                <div class="card mb-4">
                                    <div class="card-body">
                                        <div class="col-lg-12 col-xl-12 form-group" align="center">
                                            <h4 id="ActivityProgressReportName" style="font-weight:bold; font-size:20px" runat="server">Physical Progress Monitoring Sheet</h4>
                                            <h5 id="ActivityProgressProjectName" runat="server">Project Name</h5>
                                            </div>
                                        <div class="card-title">
                                            <div class="d-flex justify-content-between">
                                                <h6 class="text-muted">
                                                    <%--<asp:Label ID="LblActivityPhysicalProgress" CssClass="text-uppercase font-weight-bold" runat="server" Text="Physical Progress" />--%>
                                                </h6>
                                                <div>
                                                    <asp:Button ID="btnActivityProgressPrint" runat="server" Text="Print Chart" Visible="true" OnClientClick="printdiv('default_master_body_ActivityProgressReport');" ClientIDMode="Static" CssClass="btn btn-primary"/>
                                            </div>
                                            </div>
                                        </div>
                                        <div class="table-responsive">
                                                <asp:Literal ID="ltScript_Progress" runat="server"></asp:Literal>
                                                <div id="chart_div" style="width:100%; height:400px;"></div>
                                            <br /><br /><br />
                                          </div>
                                        <div class="table-responsive">
                                             <div id="DivActivityProgressTabular" runat="server" style="width:70%; float:left;">
                            
                                            </div>
                                            </div>
                       </div>
                    </div>
                </div>
            </div>
        </div>
</asp:Content>
