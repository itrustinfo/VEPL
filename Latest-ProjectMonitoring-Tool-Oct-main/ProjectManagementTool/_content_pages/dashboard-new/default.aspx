<%@ Page Title="" Language="C#" MasterPageFile="~/_master_pages/default_new.Master" AutoEventWireup="true" CodeBehind="default.aspx.cs" Inherits="ProjectManagementTool._content_pages.dashboard_new._default" %>
<asp:Content ID="Content1" ContentPlaceHolderID="default_master_head" runat="server">
     <style type="text/css">
               .div {
  padding-top: 0px;
  padding-right: 200px;
  padding-bottom: 0px;
  padding-left: 10px;
  width:100% !important;
  overflow-x:scroll;
  overflow-y: hidden;
}

              .gridnew{
                  padding :0px;
              }

               .chklist{
                   padding-left :10px;
               }

              .container {
         /*  position:fixed;*/
         
   
            width:100%; 
            height:100%;
            overflow:hidden;
        }
        .container img {
          position:absolute;
             
    top:0; 
    left:0; 
    right:0; 
    bottom:0; 
           /* margin:auto; */
           /* min-width:100%;
            min-height:100%;*/
            overflow: hidden;
        }



            #chart_div .google-visualization-tooltip {  position:relative !important; top:0 !important;right:0 !important; z-index:+1;} 
        </style>
        <script type="text/javascript" src="https://www.gstatic.com/charts/loader.js"></script>
        <script type="text/javascript">
          function showModal() {
                //e.preventDefault();
                //var url = $(this).attr("href");
                $("#ModViewContractor iframe").attr("src", "/ContractorMsg.html");
                $("#ModViewContractor").modal('show');
            }

             function showModalGD() {
                //e.preventDefault();
                //var url = $(this).attr("href");
                $("#ModViewContractor iframe").attr("src", "/GeneralDocsMsg.html");
                $("#ModViewContractor").modal('show');
            }

            function BindEvents() {
                $(".showModalUploadPhotograph").click(function (e) {
                    e.preventDefault();
                    var url = $(this).attr("href");
                    $("#ModAddUploadSiteImages iframe").attr("src", url);
                    $("#ModAddUploadSiteImages").modal("show");
                });

                $(".showModalViewSitePhotograph").click(function (e) {
                    e.preventDefault();
                    var url = $(this).attr("href");
                    $("#ModViewSitePhotograph iframe").attr("src", url);
                    $("#ModViewSitePhotograph").modal("show");
                });
            }

            function printdiv(printpage) {
                var frame = document.getElementById(printpage);
                 document.getElementById("btnPrint").style.display = "none";
    var data = frame.innerHTML;
    var win = window.open('', '', 'height=500,width=900');
    win.document.write('<style>@page{size:landscape;}</style><html><head><title></title>');
    win.document.write('</head><body >');
    win.document.write(data);
    win.document.write('</body></html>');
    win.print();
                win.close();
                document.getElementById("btnPrint").style.display = "block";
    return true;
            }


             function printdivFin(printpage) {
                var frame = document.getElementById(printpage);
                 document.getElementById("btnPrintFin").style.display = "none";
    var data = frame.innerHTML;
    var win = window.open('', '', 'height=500,width=900');
    win.document.write('<style>@page{size:landscape;}</style><html><head><title></title>');
    win.document.write('</head><body >');
    win.document.write(data);
    win.document.write('</body></html>');
    win.print();
                win.close();
                document.getElementById("btnPrintFin").style.display = "block";
    return true;
             }

            function GetDetails() {
                  var value = document.getElementById("<%=DDlProject.ClientID%>");  
                var getvalue = value.options[value.selectedIndex].value;
                 // var valuew = document.getElementById("<%=DDLWorkPackage.ClientID%>");  
               // var getvaluew = value.options[valuew.selectedIndex].value;
                if (getvalue != '--Select--') {
                    PageMethods.GetDetails('test', OnSuccess);
                }
            }

            function OnSuccess(response) {
                //alert(response);
                  var value = document.getElementById("<%=DDlProject.ClientID%>");  
                var getvalue = value.options[value.selectedIndex].value;
                 
                var getvaluew = response.split("$")[1];
                var username = '<%= Session["UserUID"] %>';

                var divmain = document.getElementById("<%=divUsersdocs.ClientID%>");
                divmain.innerHTML = 'You have <a id="Hluserdocs">following pending documents</a> to act on';
                //alert(divmain.innerHTML);
                
                document.getElementById("Hluserdocs").innerHTML = response.split("$")[0] + ' pending documents';
                document.getElementById("Hluserdocs").href = "/_content_pages/documents-contractor/?&type=Ontb&PrjUID=" + getvalue + "&UserUID=" + username + "&WkpgUID=" + getvaluew;
            }
            window.onload = GetDetails;

            $(document).ready(function () {
                BindEvents();
                //$('#loader').fadeOut();
            });
        </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="default_master_body" runat="server">
     <asp:ScriptManager ID="smMain" runat="server" EnablePageMethods="true" />
    <div class="container-fluid">
            <div class="row">
                <div class="col-md-12 col-lg-12 form-group" id="divUsersdocs" runat="server" style="background-color:orange;color:white;font-size:medium;text-align:center;align-content:center">Please wait processing the documents.....</div>
                <div class="col-md-12 col-lg-4 form-group"><b>Dashboard</b></div>
                <div class="col-md-6 col-lg-4 form-group" style="display:none">
                    <label class="sr-only" for="DDLProject">Project</label>
                    <div class="input-group">
                        <div class="input-group-prepend">
                            <span class="input-group-text">Project</span>
                        </div>
                        <asp:DropDownList ID="DDlProject" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="DDlProject_SelectedIndexChanged"></asp:DropDownList>
                       <%-- <select class="form-control" id="DDlProject" runat="server">
                           
                        </select>--%>
                    </div>
                </div>
                <div class="col-md-6 col-lg-4 form-group" style="display:none">
                    <label class="sr-only" for="DDLWorkPackage">Work Package</label>
                    <div class="input-group">
                        <div class="input-group-prepend">
                            <span class="input-group-text">Work Package</span>
                        </div>
                        <asp:DropDownList ID="DDLWorkPackage" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="DDLWorkPackage_SelectedIndexChanged"></asp:DropDownList>

                        <%--<select class="form-control" id="DDLWorkPackage" runat="server">
                        </select>--%>
                    </div>
                </div>
            </div>
        </div>
    <div id="divdashboardimage" runat="server" visible="true">
         <div class="container-fluid" style="opacity: 0.9 !important;background-color:none;font-weight:800">
        <div class="row">
          <div class="col-lg-6 col-xl-12 form-group">
                        <div class="card">
                            <div class="card-body" style="padding-bottom:0; margin-bottom:0;">
                                <div class="row" >
                                    <%-- <div class="col-lg-1">
                            <h6 class="card-title text-muted text-uppercase font-weight-bold">Chart : </h6>

                        </div>--%>
                        <div class="col-lg-6" align="left">
                            <asp:RadioButtonList ID="rdSelect" runat="server" class="card-title text-muted text-uppercase font-weight-bold text-center" AutoPostBack="true" RepeatDirection="Horizontal" OnSelectedIndexChanged="rdSelect_SelectedIndexChanged">
                       <asp:ListItem Selected="True" Value="0">&nbsp;Without Progress Chart&nbsp;</asp:ListItem>
                       <asp:ListItem Value="1">&nbsp;With Physical Progress Chart&nbsp;</asp:ListItem>
                       <asp:ListItem Value="2">&nbsp;With Financial Progress Chart&nbsp;</asp:ListItem>
                                 
                    </asp:RadioButtonList>

                        </div>  
                                    <%--<div class="col-lg-1" align="right" id="div2" runat="server" visible="false">
                                         <a id="A2" runat="server" href="#" style="color:blue !important">Online Flow 2</a>
                                    </div>
                                     <div class="col-lg-1" align="right" id="divMontlyweeklyReport" runat="server">
                                         <a id="A3" runat="server" href="#" style="color:blue !important">Monthly/Weekly Reports</a>
                                    </div>
                                    <div class="col-lg-2" align="right" id="div1" runat="server" style="display:none">
                                         <a id="A1" runat="server" href="#" style="color:blue !important">ONTB/BWSSB Correspondence</a>
                                    </div>
                                     <div class="col-lg-2" align="right" id="divContractorCPDocs" runat="server">
                                         <a id="hlContractorCPDocs" runat="server" href="#" style="color:red !important">Contractor Correspondence</a>
                                         </div>--%>
                                    <div class="col-lg-6" align="right" id="divPhotographs" runat="server">
                                        <a id="UploadSitePhotograph" runat="server" href="/_modal_pages/upload-sitephotograph.aspx" class="showModalUploadPhotograph">Add Photographs&nbsp;&nbsp;&nbsp;</a>
                                        <a id="ViewSitePhotograph" runat="server"  href="/_modal_pages/view-sitephotographs.aspx" class="showModalViewSitePhotograph">View Photographs</a>
                                    </div>  
                                </div>
                            </div>
                        </div>
                       
                     </div>  
            </div>
    </div>
         <div class="container-fluid" id="divContractordata" runat="server">
        <div class="row">
                                    <div class="col-lg-6 col-xl-12 form-group">
                                      
                                        <div class="row">
                                            <div class="col-md-3 stretch-card grid-margin">
                                                <div class="card bg-gradient card-img-holder text-white"
                                                    style="background-color: #f2a654;">
                                                    <div class="card-body">
                                                        <h4 class="mb-3">Contractor</h4>
                                                        <h6 class="card-text">
                                                            <asp:Label ID="lblContractor" runat="server" Text="Label"></asp:Label></h6>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="col-md-3 stretch-card grid-margin">
                                                <div class="card bg-gradient card-img-holder text-white"
                                                    style="background-color: #57c7d4;">
                                                    <div class="card-body">
                                                        <h4 class="mb-3">Project</h4>
                                                        <h6 class="card-text"><asp:Label ID="lblProject" runat="server" Text="Label"></asp:Label></h6>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="col-md-3 stretch-card grid-margin">
                                                <div class="card bg-gradient card-img-holder text-white"
                                                    style="background-color: #b66dff;">
                                                    <div class="card-body">
                                                        <h4 class="mb-3">Project Start Date</h4>
                                                        <h6 class="card-text"><asp:Label ID="lblPStartDate" runat="server" Text="Label"></asp:Label></h6>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="col-md-3 stretch-card grid-margin">
                                                <div class="card bg-gradient card-img-holder text-white"
                                                    style="background-color: #f96868;">
                                                    <div class="card-body">
                                                        <h4 class="mb-3">Project End Date</h4>
                                                        <h6 class="card-text"><asp:Label ID="lblPEndDate" runat="server" Text="Label"></asp:Label></h6>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>

                                </div>
    </div>

        <div class="container-fluid" id="divProgresschart" runat="server" visible="false" style="opacity: 0.9 !important">
            <div class="row">
                <div class="col-lg-12 col-xl-12 form-group">
                <div class="card mb-4">
                    <div class="card-body">
                        <div class="table-responsive">
                            <div class="row">
                           <div class="col-md-6 col-lg-6 form-group" align="center"><h6 class="card-title text-muted text-uppercase" id="heading" runat="server" >Physical Progress Chart</h6></div>
                            <div class="col-md-6 col-lg-6 form-group" align="right"><asp:Button ID="btnPrint" runat="server" Text="Print Chart" Visible="true" OnClientClick="printdiv('default_master_body_divProgresschart');" ClientIDMode="Static"/></div></div>
                                 <asp:Literal ID="ltScript_PhysicalProgress" runat="server"></asp:Literal>
                                  <div id="chart_divProgress" style="width:100%; height:300px;">
                                     
                                  </div><br /><br /><br />
                            <div id="divtable" runat="server" class="div">
                            
                                </div>
                        </div>
                        </div>
                    </div>

                </div>
            </div>
        </div>
         <div class="container-fluid" id="divFinProgressChart" runat="server" visible="false" style="opacity: 0.9 !important">
            <div class="row">
                <div class="col-lg-12 col-xl-12 form-group">
                <div class="card mb-4">
                    <div class="card-body">
                        <div class="table-responsive">
                            <div class="row">
                           <div class="col-md-6 col-lg-6 form-group" align="center"><h6 class="card-title text-muted text-uppercase" id="headingF" runat="server">Financial Progress Chart</h6></div>
                            <div class="col-md-6 col-lg-6 form-group" align="right"><asp:Button ID="btnPrintFin" runat="server" Text="Print Chart" Visible="true" OnClientClick="printdivFin('default_master_body_divFinProgressChart');" ClientIDMode="Static"/></div></div>
                                 <asp:Literal ID="ltScript_FinProgress" runat="server"></asp:Literal>
                                  <div id="chart_divProgressFin" style="width:100%; height:300px;">
                                     
                                  </div><br /><br /><br />
                            <div id="divtableFin" runat="server" class="div">
                            
                                </div>
                        </div>
                        </div>
                    </div>

                </div>
            </div>
        </div>
        <div id="divMainblocks" runat="server" class="container-fluid"
                            style="background-color:none;font-weight:400">
                            <div class="row">
                                <!-- Chart 1: Cost Chart - Begins-->
                                <div id="divCostChart" runat="server" class="col-md-6 col-xl-4 mb-4">
                                    <div class="card" style="height: 275px">
                                        <div class="card-body" style="background-color:#ffffff !important">
                                            <asp:RadioButtonList ID="RdList" runat="server" RepeatDirection="Horizontal"  class="card-title text-muted text-uppercase font-weight-bold" AutoPostBack="true">
                                  
                                    <asp:ListItem Value="Cost" Selected="True">&nbsp;Cost Chart</asp:ListItem>                      
                                </asp:RadioButtonList>
                               
                                  
                                 <asp:Literal ID="ltScript_Progress" runat="server" Visible="true"></asp:Literal>
                                  <div id="chart_div"
                                                style="width:100%; height:200px; background-color:#ffffff !important"></div>
                                           
                                        </div>
                                    </div>
                                </div>
                                <!-- Chart 1: Cost Chart - Ends-->

                                <!-- Chart 2: Issues/Instructions Chart - Begins-->
                                <div id="divIssues" runat="server" class="col-md-6 col-xl-3 mb-4">
                                    <div class="card" style="height: 275px">
                                        <div class="card-body" style="background-color:#ffffff !important">
                                            <h6 class="card-title text-muted text-uppercase">Issues/Instructions
                                            </h6>
                                           <asp:Literal ID="ltScripts_piechart" runat="server"></asp:Literal>
                                            <div id="piechart_3d" style="width:100%; height:200px;"></div>
                                        </div>
                                    </div>
                                </div>
                                <!-- Chart 2: Issues/Instructions Chart - Ends-->

                                <!-- Chart 3: Alerts Section - Begins-->
                                <div id="divAlerts" runat="server" class="col-md-6 col-xl-5 mb-4">
                                    <div class="card" style="height: 275px">
                                        <div class="card-body"
                                            style="font-weight:800;background-color:#ffffff !important">
                                            <h6 class="card-title text-muted text-uppercase font-weight-bold">Alerts
                                            </h6>
                                            <div id="default_master_body_Panel1"
                                                style="border-width:3px;border-style:None;width:100%;">
                                                <marquee direction="up" behavior="scroll" loop="infinite"
                                                    onmouseover="this.stop()" onmouseout="this.start()"
                                                    scrolldelay="100" style="height:200px; width:100%;">
                                                     <asp:Literal ID="lt1" runat="server"></asp:Literal></marquee>

                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <!-- Chart 3: Alerts Section - Ends-->

                                <!-- Chart 4: Status of Design, Drawings & Documentation - Begins-->
                                <div class="col-md-4 col-xl-5 mb-4">
                                    <div class="card h-100">
                                        <div class="card-body" style="background-color:#ffffff !important; ">
                                            <h6 class="card-title text-muted text-uppercase" style="display: inline-block;font-weight:bold">Status of Design, Drawings & Documentation </h6>
                                          
                                            <div id="DocChart_Div" style="width:100%; height:370px; overflow:scroll;"></div>
                                             <asp:Literal ID="ltScript_Document" runat="server" ></asp:Literal>
                                        </div>
                                      
                                        <asp:Label ID="lblTotalMasterListDocuments" runat="server" CssClass="card-footer font-weight-solid text-center" ForeColor="DarkBlue">
                       </asp:Label>
                                    </div>
                                </div>
                                <!-- Chart 4: Status of Design, Drawings & Documentation - Ends -->

                              
                                <div id="divCamera" class="col-md-6 col-xl-4 mb-4"
                                    style="display:none">
                                    <div class="card h-100">
                                        <div class="card-body" style="background-color:#ffffff !important">
                                            <div class="input-group" style="margin-bottom:8px;">
                                                <div class="input-group-prepend">
                                                    <span class="input-group-text">IP Camera List</span>
                                                </div>
                                            </div><br />
                                            <div id="default_master_body_divCameralist"
                                                style="overflow-y:scroll;font-size:medium">
                                                <div>No Camera added</div>
                                            </div>
                                            <h6 id="camera" style="display:none;font-weight:800">As you have denied
                                                access to
                                                the camera. Image will not be streamed to the dashboard</h6>

                                            <script>
                                                //var video = document.querySelector("#videoElement");

                                                //if (navigator.mediaDevices.getUserMedia) {
                                                //    navigator.mediaDevices.getUserMedia({ video: true })
                                                //            .then(function (stream) {
                                                //                video.srcObject = stream;
                                                //                document.getElementById("camera").style.display = "none";
                                                //            })
                                                //            .catch(function (err0r) {
                                                //                //console.log("Something went wrong!");
                                                //                //alert(err0r);
                                                //                //alert('As you have denied access to the camera. Image will not be streamed to the dashboard. Press OK to continue');
                                                //                 document.getElementById("camera").style.display = "block";
                                                //            });
                                                //}
                                            </script>
                                        </div>
                                    </div>
                                </div>

                                 <!-- Chart 5: Status of Resource Deployment - Begins-->
                                <div class="col-md-6 col-xl-4 mb-4">
                                    <div class="card h-100">
                                        <div class="card-body" style="background-color:#ffffff !important">
                                            <a href="../engineering-status-update-fasttrack/default.aspx?fromdashboard=yes"
                                                <h6 class="card-title text-muted text-uppercase text-decoration-none"
                                                style="display: inline-block;font-weight:bold">Status of Resource Deployment&#128279;
                                                </h6>
                                            </a>
                                            <asp:Literal ID="ltScript_ResourceGraph" runat="server"></asp:Literal>
                                            <div id="ResourceChart_Div" style="width:100%; height:325px;"
                                                visible="true">
                                                <!-- Your Google Chart will be rendered here. -->
                                            </div>
                                        </div>
                                    </div>
                                </div>
                                <!-- Chart 5: Status of Resource Deployment - Ends-->

                                <!-- Chart 6: Quick Links Section - Begins-->
                                <div id="divsyncdetails" class="col-md-6 col-xl-3 mb-4">
                                    <div class="card h-100">
                                        <div class="card-body"
                                            style="font-weight:400;background-color:#ffffff !important">
                                            <h6 class="card-title text-muted text-uppercase"
                                                style="color:forestgreen !important">Quick Access Links</h6>
                                            <table style="width:100%;line-height:30px">
                                                <tr id="ReconDocs">
                                                    <td><a class="text-decoration-none"
                                                            href="../documents-contractor/?&type=Recon&PrjUID=fe0b8a4d-4d13-46e3-bcec-5f14df5e0278"
                                                            id="hlReconciliationdocs" runat="server">Reconciliation
                                                            Documents</a></td>
                                                    <td><span id="default_master_body_lblReconDocs"></span></td>
                                                    <td><span id="lblReconDocsNo" runat="server">0</span></td>
                                                </tr>

                                                <tr>
                                                    <td><a class="text-decoration-none"
                                                            href="../documents-contractor/?&type=Contractor&PrjUID=fe0b8a4d-4d13-46e3-bcec-5f14df5e0278"
                                                            id="hlContractor" runat="server">Total Documents</a>
                                                    </td>
                                                    <td><span id="lblContractorTo"></span></td>
                                                    <td><span id="lblContractorToNo" runat="server">0</span></td>
                                                </tr>
                                                <tr>
                                                    <td><a class="text-decoration-none" href="#"
                                                            id="hlMeasurement" runat="server">Measurments</a></td>
                                                    <td><span></span></td>
                                                    <td><span id="lblMeasurements" runat="server">0</span></td>
                                                </tr>
                                                <tr>
                                                    <td><a class="text-decoration-none"
                                                            href="../rabill-summary/?&PrjUID=fe0b8a4d-4d13-46e3-bcec-5f14df5e0278"
                                                            id="hlRABills" runat="server">RA Bills</a></td>
                                                    <td><span></span></td>
                                                    <td><span id="lblRABills" runat="server">0</span></td>
                                                </tr>
                                                <tr>
                                                    <td><a class="text-decoration-none"
                                                            href="../invoice/?&PrjUID=fe0b8a4d-4d13-46e3-bcec-5f14df5e0278"
                                                            id="hlInvoices" runat="server">Invoices</a></td>
                                                    <td><span></span></td>
                                                    <td><span id="lblInvoices" runat="server">0</span></td>
                                                </tr>
                                                <tr>
                                                    <td><a class="text-decoration-none"
                                                            href="../bank-guarantee/?&PrjUID=fe0b8a4d-4d13-46e3-bcec-5f14df5e0278"
                                                            id="hlBankGuarantee" runat="server">Bank Guarantee</a>
                                                    </td>
                                                    <td><span></span></td>
                                                    <td><span id="lblBankG" runat="server">0</span></td>
                                                </tr>
                                                <tr>
                                                    <td><a class="text-decoration-none"
                                                            href="../insurance/?&PrjUID=fe0b8a4d-4d13-46e3-bcec-5f14df5e0278"
                                                            id="hlInsurance" runat="server">Insurance</a></td>
                                                    <td><span></span></td>
                                                    <td><span id="lblInsurance" runat="server">1</span></td>
                                                </tr>
                                                <tr>
                                                    <td><a class="text-decoration-none"
                                                            href="../documents-ontb/?&fromdashboard=yes"
                                                            id="hlInstructionManual" runat="server">Instruction
                                                            Manual</a>
                                                    </td>
                                                    <td><span></span></td>
                                                    <td><span id="lblInstruction" runat="server">1</span></td>
                                                </tr>
                                                <tr>
                                                    <td><a class="text-decoration-none"
                                                            href="../documents-ontb/?&fromdashboard=yes"
                                                            id="hlMOM" runat="server">MOM Documents</a></td>
                                                    <td><span></span></td>
                                                    <td><span id="lblMOM" runat="server">1</span></td>
                                                </tr>
                                                <tr>
                                                    <td><a class="text-decoration-none"
                                                            href="../documents-ontb/?&fromdashboard=yes"
                                                            id="hlArchived" runat="server">Archived Documents</a></td>
                                                    <td><span></span></td>
                                                    <td><span id="lblArchived" runat="server">0</span></td>
                                                </tr>
                                               
                                            </table>
                                        </div>
                                    </div>
                                </div>
                                <!-- Chart 6: Quick Links Section - Ends-->

                              
                               
                            </div>

                        </div>
        </div>

      <%--Upload Site Photographs--%>
    <div id="ModAddUploadSiteImages" class="modal it-modal fade">
	    <div class="modal-dialog modal-dialog-centered modal-dialog-scrollable modal-xl">
		    <div class="modal-content">
			    <div class="modal-header">
				    <h5 class="modal-title">Upload Site Photographs</h5>
                    <button aria-label="Close" class="close" data-dismiss="modal" type="button"><span aria-hidden="true">&times;</span></button>
			    </div>
			    <div class="modal-body">
                    <iframe class="border-0 w-100" style="height:450px;" loading="lazy"></iframe>
			    </div>
              
		    </div>
	    </div>
    </div>

    <%--View Site Photographs--%>
    <div id="ModViewSitePhotograph" class="modal it-modal fade">
	    <div class="modal-dialog modal-dialog-centered modal-dialog-scrollable modal-xl">
		    <div class="modal-content">
			    <div class="modal-header">
				    <h5 class="modal-title">View Site Photograph/s</h5>
                    <button aria-label="Close" class="close" data-dismiss="modal" type="button"><span aria-hidden="true">&times;</span></button>
			    </div>
			    <div class="modal-body">
                    <iframe class="border-0 w-100" style="height:450px;" loading="lazy"></iframe>
			    </div>
              
		    </div>
	    </div>
    </div>
        <%--View Terms and Conditions--%>
    <div id="ModViewContractor" class="modal it-modal fade">
	    <div class="modal-dialog modal-dialog-centered modal-dialog-scrollable modal-xl">
		    <div class="modal-content">
			    <div class="modal-header" style="text-align:center">
				   <div style="text-align:center"> <h5 class="modal-title">IMPORTANT</h5></div>
                </div>
			    <div class="modal-body">
                    <iframe class="border-0 w-100" style="height:450px;" loading="lazy">
                       

                    </iframe>
			    </div>
              <div class="modal-footer" style="padding:5px;background-color:black">
               <div style="padding:10px;background-color:black;text-align:center"><button type="button" class="btn btn-secondary" data-dismiss="modal" style="color:white">OK</button></div>
              </div>
		    </div>
	    </div>
    </div>
</asp:Content>
