<%@ Page Title="" Language="C#" MasterPageFile="~/_master_pages/modal.Master" AutoEventWireup="true" CodeBehind="add-newflow.aspx.cs" Inherits="ProjectManagementTool._modal_pages.add_newflow" %>
<asp:Content ID="Content1" ContentPlaceHolderID="modal_master_head" runat="server">
 
    <style type="text/css">
         .circular {
	width: 40px;
	height: 40px;
	border-radius: 50px;
	-webkit-border-radius: 50px;
	-moz-border-radius: 50px;
	
	}
    </style>
    <script type="text/javascript">
function isNumber(evt) {
    evt = (evt) ? evt : window.event;
    var charCode = (evt.which) ? evt.which : evt.keyCode;
    if (charCode > 31 && (charCode < 48 || charCode > 57)) {
        alert("only numbers allowed !");
        return false;
    }
    return true;
        }

       function sync(textbox)
    {
  document.getElementById('txtloginusername').value = textbox.value;
       }

    </script>
    
</asp:Content>


<asp:Content ID="Content2" ContentPlaceHolderID="modal_master_body" runat="server">
    <form id="frmAddIssuesModal" runat="server">
        <div class="container-fluid" style="max-height:80vh; overflow-y:auto;">
            <div class="row">
                <div class="col-sm-6">
                    <div class="form-group">
                        <label class="lblCss" for="txtnewflowname">Flow Name</label>&nbsp;<asp:Label runat="server" Text="*" ForeColor="Red"></asp:Label>
                         <asp:TextBox ID="DDLnewflowname" runat="server" CssClass="form-control" autocomplete="off"></asp:TextBox>
                    </div>
                 </div> 


                <div class="col-sm-6">
                    <div class="form-group">
                        <label class="lblCss" for="txtnewflowname">Flow Steps</label>&nbsp;<asp:Label runat="server" Text="*" ForeColor="Red"></asp:Label>
                        <asp:DropDownList ID="DDLFlowSteps" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="DDLFlowSteps_SelectedIndexChanged">
                            <asp:ListItem Value="00"><--Select Number of Steps--></asp:ListItem>
                            <asp:ListItem Value="01">1</asp:ListItem>
                            <asp:ListItem Value="02">2</asp:ListItem>
                            <asp:ListItem Value="03">3</asp:ListItem>
                            <asp:ListItem Value="04">4</asp:ListItem>
                            <asp:ListItem Value="05">5</asp:ListItem>
                            <asp:ListItem Value="06">6</asp:ListItem>
                            <asp:ListItem Value="07">7</asp:ListItem>
                            <asp:ListItem Value="08">8</asp:ListItem>
                            <asp:ListItem Value="09">9</asp:ListItem>
                            <asp:ListItem Value="10">10</asp:ListItem>
                            <asp:ListItem Value="11">11</asp:ListItem>
                            <asp:ListItem Value="12">12</asp:ListItem>
                            <asp:ListItem Value="13">13</asp:ListItem>
                            <asp:ListItem Value="14">14</asp:ListItem>
                            <asp:ListItem Value="15">15</asp:ListItem>
                            <asp:ListItem Value="16">16</asp:ListItem>
                            <asp:ListItem Value="17">17</asp:ListItem>
                            <asp:ListItem Value="18">18</asp:ListItem>
                            <asp:ListItem Value="19">19</asp:ListItem>
                            <asp:ListItem Value="20">20</asp:ListItem>
                        </asp:DropDownList>
                    </div>
                </div> 
       </div>
                                                   
                    <div class="form-group">
                        <asp:Table class="tblnew" ID="tblmain" runat="server" BackColor="White" BorderColor="Black" CellSpacing="10">
                       <asp:TableRow ID="TableRow1" runat="server" HorizontalAlign="Center" VerticalAlign="Middle" Visible="false">
                           <asp:TableCell>
                           <label class="lblCss" for="txtnewflowname">Flow Step-1 Name</label><asp:Label runat="server" Text="*" ForeColor="Red"></asp:Label> 
                            </asp:TableCell>
                           <asp:TableCell>   
                               <asp:TextBox ID="TextBox1" runat="server" CssClass="form-control" autocomplete="off"  ></asp:TextBox> 
                           </asp:TableCell>
                          <asp:TableCell> 
                              <label class="lblCss" for="txtnewflowname">Flow Step-1 Duration</label><asp:Label runat="server" Text="*" ForeColor="Red"></asp:Label>
                            </asp:TableCell>
                            <asp:TableCell>
                                <asp:TextBox ID="TextBox2" runat="server" CssClass="form-control" autocomplete="off" onkeypress="return isNumber(event)" ></asp:TextBox>
                           </asp:TableCell>
                       </asp:TableRow>
                       <asp:TableRow ID="TableRow2" runat="server" HorizontalAlign="Center" VerticalAlign="Middle" Visible="false">
                           <asp:TableCell>
                           <label class="lblCss" for="txtnewflowname">Flow Step-2 Name</label><asp:Label runat="server" Text="*" ForeColor="Red"></asp:Label> 
                            </asp:TableCell>
                           <asp:TableCell>   
                               <asp:TextBox ID="TextBox3" runat="server" CssClass="form-control" autocomplete="off"  ></asp:TextBox> 
                           </asp:TableCell>
                          <asp:TableCell> 
                              <label class="lblCss" for="txtnewflowname">Flow Step-2 Duration</label><asp:Label runat="server" Text="*" ForeColor="Red"></asp:Label>
                            </asp:TableCell>
                            <asp:TableCell>
                                <asp:TextBox ID="TextBox4" runat="server" CssClass="form-control" autocomplete="off" onkeypress="return isNumber(event)" ></asp:TextBox>
                           </asp:TableCell>
                       </asp:TableRow>
                            <asp:TableRow ID="TableRow3" runat="server" HorizontalAlign="Center" VerticalAlign="Middle" Visible="false">
                           <asp:TableCell>
                           <label class="lblCss" for="txtnewflowname">Flow Step-3 Name</label><asp:Label runat="server" Text="*" ForeColor="Red"></asp:Label> 
                            </asp:TableCell>
                           <asp:TableCell>   
                               <asp:TextBox ID="TextBox5" runat="server" CssClass="form-control" autocomplete="off"  ></asp:TextBox> 
                           </asp:TableCell>
                          <asp:TableCell> 
                              <label class="lblCss" for="txtnewflowname">Flow Step-3 Duration</label><asp:Label runat="server" Text="*" ForeColor="Red"></asp:Label>
                            </asp:TableCell>
                            <asp:TableCell>
                                <asp:TextBox ID="TextBox6" runat="server" CssClass="form-control" autocomplete="off" onkeypress="return isNumber(event)" ></asp:TextBox>
                           </asp:TableCell>
                       </asp:TableRow>
                            <asp:TableRow ID="TableRow4" runat="server" HorizontalAlign="Center" VerticalAlign="Middle" Visible="false">
                           <asp:TableCell>
                           <label class="lblCss" for="txtnewflowname">Flow Step-4 Name</label><asp:Label runat="server" Text="*" ForeColor="Red"></asp:Label> 
                            </asp:TableCell>
                           <asp:TableCell>   
                               <asp:TextBox ID="TextBox7" runat="server" CssClass="form-control" autocomplete="off"  ></asp:TextBox> 
                           </asp:TableCell>
                          <asp:TableCell> 
                              <label class="lblCss" for="txtnewflowname">Flow Step-4 Duration</label><asp:Label runat="server" Text="*" ForeColor="Red"></asp:Label>
                            </asp:TableCell>
                            <asp:TableCell>
                                <asp:TextBox ID="TextBox8" runat="server" CssClass="form-control" autocomplete="off" onkeypress="return isNumber(event)" ></asp:TextBox>
                           </asp:TableCell>
                       </asp:TableRow>
                            <asp:TableRow ID="TableRow5" runat="server" HorizontalAlign="Center" VerticalAlign="Middle" Visible="false">
                           <asp:TableCell>
                           <label class="lblCss" for="txtnewflowname">Flow Step-5 Name</label><asp:Label runat="server" Text="*" ForeColor="Red"></asp:Label> 
                            </asp:TableCell>
                           <asp:TableCell>   
                               <asp:TextBox ID="TextBox9" runat="server" CssClass="form-control" autocomplete="off"  ></asp:TextBox> 
                           </asp:TableCell>
                          <asp:TableCell> 
                              <label class="lblCss" for="txtnewflowname">Flow Step-5 Duration</label><asp:Label runat="server" Text="*" ForeColor="Red"></asp:Label>
                            </asp:TableCell>
                            <asp:TableCell>
                                <asp:TextBox ID="TextBox10" runat="server" CssClass="form-control" autocomplete="off" onkeypress="return isNumber(event)" ></asp:TextBox>
                           </asp:TableCell>
                       </asp:TableRow>
                            <asp:TableRow ID="TableRow6" runat="server" HorizontalAlign="Center" VerticalAlign="Middle" Visible="false">
                           <asp:TableCell>
                           <label class="lblCss" for="txtnewflowname">Flow Step-6 Name</label><asp:Label runat="server" Text="*" ForeColor="Red"></asp:Label> 
                            </asp:TableCell>
                           <asp:TableCell>   
                               <asp:TextBox ID="TextBox11" runat="server" CssClass="form-control" autocomplete="off"  ></asp:TextBox> 
                           </asp:TableCell>
                          <asp:TableCell> 
                              <label class="lblCss" for="txtnewflowname">Flow Step-6 Duration</label><asp:Label runat="server" Text="*" ForeColor="Red"></asp:Label>
                            </asp:TableCell>
                            <asp:TableCell>
                                <asp:TextBox ID="TextBox12" runat="server" CssClass="form-control" autocomplete="off" onkeypress="return isNumber(event)" ></asp:TextBox>
                           </asp:TableCell>
                       </asp:TableRow>
                            <asp:TableRow ID="TableRow7" runat="server" HorizontalAlign="Center" VerticalAlign="Middle" Visible="false">
                           <asp:TableCell>
                           <label class="lblCss" for="txtnewflowname">Flow Step-7 Name</label><asp:Label runat="server" Text="*" ForeColor="Red"></asp:Label> 
                            </asp:TableCell>
                           <asp:TableCell>   
                               <asp:TextBox ID="TextBox13" runat="server" CssClass="form-control" autocomplete="off"  ></asp:TextBox> 
                           </asp:TableCell>
                          <asp:TableCell> 
                              <label class="lblCss" for="txtnewflowname">Flow Step-7 Duration</label><asp:Label runat="server" Text="*" ForeColor="Red"></asp:Label>
                            </asp:TableCell>
                            <asp:TableCell>
                                <asp:TextBox ID="TextBox14" runat="server" CssClass="form-control" autocomplete="off" onkeypress="return isNumber(event)" ></asp:TextBox>
                           </asp:TableCell>
                       </asp:TableRow>
                            <asp:TableRow ID="TableRow8" runat="server" HorizontalAlign="Center" VerticalAlign="Middle" Visible="false">
                           <asp:TableCell>
                           <label class="lblCss" for="txtnewflowname">Flow Step-8 Name</label><asp:Label runat="server" Text="*" ForeColor="Red"></asp:Label> 
                            </asp:TableCell>
                           <asp:TableCell>   
                               <asp:TextBox ID="TextBox15" runat="server" CssClass="form-control" autocomplete="off"  ></asp:TextBox> 
                           </asp:TableCell>
                          <asp:TableCell> 
                              <label class="lblCss" for="txtnewflowname">Flow Step-8 Duration</label><asp:Label runat="server" Text="*" ForeColor="Red"></asp:Label>
                            </asp:TableCell>
                            <asp:TableCell>
                                <asp:TextBox ID="TextBox16" runat="server" CssClass="form-control" autocomplete="off" onkeypress="return isNumber(event)" ></asp:TextBox>
                           </asp:TableCell>
                       </asp:TableRow>
                            <asp:TableRow ID="TableRow9" runat="server" HorizontalAlign="Center" VerticalAlign="Middle" Visible="false">
                           <asp:TableCell>
                           <label class="lblCss" for="txtnewflowname">Flow Step-9 Name</label><asp:Label runat="server" Text="*" ForeColor="Red"></asp:Label> 
                            </asp:TableCell>
                           <asp:TableCell>   
                               <asp:TextBox ID="TextBox17" runat="server" CssClass="form-control" autocomplete="off"  ></asp:TextBox> 
                           </asp:TableCell>
                          <asp:TableCell> 
                              <label class="lblCss" for="txtnewflowname">Flow Step-9 Duration</label><asp:Label runat="server" Text="*" ForeColor="Red"></asp:Label>
                            </asp:TableCell>
                            <asp:TableCell>
                                <asp:TextBox ID="TextBox18" runat="server" CssClass="form-control" autocomplete="off" onkeypress="return isNumber(event)" ></asp:TextBox>
                           </asp:TableCell>
                       </asp:TableRow>
                            <asp:TableRow ID="TableRow10" runat="server" HorizontalAlign="Center" VerticalAlign="Middle" Visible="false">
                           <asp:TableCell>
                           <label class="lblCss" for="txtnewflowname">Flow Step-10 Name</label><asp:Label runat="server" Text="*" ForeColor="Red"></asp:Label> 
                            </asp:TableCell>
                           <asp:TableCell>   
                               <asp:TextBox ID="TextBox19" runat="server" CssClass="form-control" autocomplete="off"  ></asp:TextBox> 
                           </asp:TableCell>
                          <asp:TableCell> 
                              <label class="lblCss" for="txtnewflowname">Flow Step-10 Duration</label><asp:Label runat="server" Text="*" ForeColor="Red"></asp:Label>
                            </asp:TableCell>
                            <asp:TableCell>
                                <asp:TextBox ID="TextBox20" runat="server" CssClass="form-control" autocomplete="off" onkeypress="return isNumber(event)" ></asp:TextBox>
                           </asp:TableCell>
                       </asp:TableRow>
                            <asp:TableRow ID="TableRow11" runat="server" HorizontalAlign="Center" VerticalAlign="Middle" Visible="false">
                           <asp:TableCell>
                           <label class="lblCss" for="txtnewflowname">Flow Step-11 Name</label><asp:Label runat="server" Text="*" ForeColor="Red"></asp:Label> 
                            </asp:TableCell>
                           <asp:TableCell>   
                               <asp:TextBox ID="TextBox21" runat="server" CssClass="form-control" autocomplete="off"  ></asp:TextBox> 
                           </asp:TableCell>
                          <asp:TableCell> 
                              <label class="lblCss" for="txtnewflowname">Flow Step-11 Duration</label><asp:Label runat="server" Text="*" ForeColor="Red"></asp:Label>
                            </asp:TableCell>
                            <asp:TableCell>
                                <asp:TextBox ID="TextBox22" runat="server" CssClass="form-control" autocomplete="off" onkeypress="return isNumber(event)" ></asp:TextBox>
                           </asp:TableCell>
                       </asp:TableRow>
                            <asp:TableRow ID="TableRow12" runat="server" HorizontalAlign="Center" VerticalAlign="Middle" Visible="false">
                           <asp:TableCell>
                           <label class="lblCss" for="txtnewflowname">Flow Step-12 Name</label><asp:Label runat="server" Text="*" ForeColor="Red"></asp:Label> 
                            </asp:TableCell>
                           <asp:TableCell>   
                               <asp:TextBox ID="TextBox23" runat="server" CssClass="form-control" autocomplete="off"  ></asp:TextBox> 
                           </asp:TableCell>
                          <asp:TableCell> 
                              <label class="lblCss" for="txtnewflowname">Flow Step-12 Duration</label><asp:Label runat="server" Text="*" ForeColor="Red"></asp:Label>
                            </asp:TableCell>
                            <asp:TableCell>
                                <asp:TextBox ID="TextBox24" runat="server" CssClass="form-control" autocomplete="off" onkeypress="return isNumber(event)" ></asp:TextBox>
                           </asp:TableCell>
                       </asp:TableRow>
                            <asp:TableRow ID="TableRow13" runat="server" HorizontalAlign="Center" VerticalAlign="Middle" Visible="false">
                           <asp:TableCell>
                           <label class="lblCss" for="txtnewflowname">Flow Step-13 Name</label><asp:Label runat="server" Text="*" ForeColor="Red"></asp:Label> 
                            </asp:TableCell>
                           <asp:TableCell>   
                               <asp:TextBox ID="TextBox25" runat="server" CssClass="form-control" autocomplete="off"  ></asp:TextBox> 
                           </asp:TableCell>
                          <asp:TableCell> 
                              <label class="lblCss" for="txtnewflowname">Flow Step-13 Duration</label><asp:Label runat="server" Text="*" ForeColor="Red"></asp:Label>
                            </asp:TableCell>
                            <asp:TableCell>
                                <asp:TextBox ID="TextBox26" runat="server" CssClass="form-control" autocomplete="off" onkeypress="return isNumber(event)" ></asp:TextBox>
                           </asp:TableCell>
                       </asp:TableRow>
                            <asp:TableRow ID="TableRow14" runat="server" HorizontalAlign="Center" VerticalAlign="Middle" Visible="false">
                           <asp:TableCell>
                           <label class="lblCss" for="txtnewflowname">Flow Step-14 Name</label><asp:Label runat="server" Text="*" ForeColor="Red"></asp:Label> 
                            </asp:TableCell>
                           <asp:TableCell>   
                               <asp:TextBox ID="TextBox27" runat="server" CssClass="form-control" autocomplete="off"  ></asp:TextBox> 
                           </asp:TableCell>
                          <asp:TableCell> 
                              <label class="lblCss" for="txtnewflowname">Flow Step-14 Duration</label><asp:Label runat="server" Text="*" ForeColor="Red"></asp:Label>
                            </asp:TableCell>
                            <asp:TableCell>
                                <asp:TextBox ID="TextBox28" runat="server" CssClass="form-control" autocomplete="off" onkeypress="return isNumber(event)" ></asp:TextBox>
                           </asp:TableCell>
                       </asp:TableRow>
                            <asp:TableRow ID="TableRow15" runat="server" HorizontalAlign="Center" VerticalAlign="Middle" Visible="false">
                           <asp:TableCell>
                           <label class="lblCss" for="txtnewflowname">Flow Step-15 Name</label><asp:Label runat="server" Text="*" ForeColor="Red"></asp:Label> 
                            </asp:TableCell>
                           <asp:TableCell>   
                               <asp:TextBox ID="TextBox29" runat="server" CssClass="form-control" autocomplete="off"  ></asp:TextBox> 
                           </asp:TableCell>
                          <asp:TableCell> 
                              <label class="lblCss" for="txtnewflowname">Flow Step-15 Duration</label><asp:Label runat="server" Text="*" ForeColor="Red"></asp:Label>
                            </asp:TableCell>
                            <asp:TableCell>
                                <asp:TextBox ID="TextBox30" runat="server" CssClass="form-control" autocomplete="off" onkeypress="return isNumber(event)" ></asp:TextBox>
                           </asp:TableCell>
                       </asp:TableRow>
                            <asp:TableRow ID="TableRow16" runat="server" HorizontalAlign="Center" VerticalAlign="Middle" Visible="false">
                           <asp:TableCell>
                           <label class="lblCss" for="txtnewflowname">Flow Step-16 Name</label><asp:Label runat="server" Text="*" ForeColor="Red"></asp:Label> 
                            </asp:TableCell>
                           <asp:TableCell>   
                               <asp:TextBox ID="TextBox31" runat="server" CssClass="form-control" autocomplete="off"  ></asp:TextBox> 
                           </asp:TableCell>
                          <asp:TableCell> 
                              <label class="lblCss" for="txtnewflowname">Flow Step-16 Duration</label><asp:Label runat="server" Text="*" ForeColor="Red"></asp:Label>
                            </asp:TableCell>
                            <asp:TableCell>
                                <asp:TextBox ID="TextBox32" runat="server" CssClass="form-control" autocomplete="off" onkeypress="return isNumber(event)" ></asp:TextBox>
                           </asp:TableCell>
                       </asp:TableRow>
                            <asp:TableRow ID="TableRow17" runat="server" HorizontalAlign="Center" VerticalAlign="Middle" Visible="false">
                           <asp:TableCell>
                           <label class="lblCss" for="txtnewflowname">Flow Step-17 Name</label><asp:Label runat="server" Text="*" ForeColor="Red"></asp:Label> 
                            </asp:TableCell>
                           <asp:TableCell>   
                               <asp:TextBox ID="TextBox33" runat="server" CssClass="form-control" autocomplete="off"  ></asp:TextBox> 
                           </asp:TableCell>
                          <asp:TableCell> 
                              <label class="lblCss" for="txtnewflowname">Flow Step-17 Duration</label><asp:Label runat="server" Text="*" ForeColor="Red"></asp:Label>
                            </asp:TableCell>
                            <asp:TableCell>
                                <asp:TextBox ID="TextBox34" runat="server" CssClass="form-control" autocomplete="off" onkeypress="return isNumber(event)" ></asp:TextBox>
                           </asp:TableCell>
                       </asp:TableRow>
                            <asp:TableRow ID="TableRow18" runat="server" HorizontalAlign="Center" VerticalAlign="Middle" Visible="false">
                           <asp:TableCell>
                           <label class="lblCss" for="txtnewflowname">Flow Step-18 Name</label><asp:Label runat="server" Text="*" ForeColor="Red"></asp:Label> 
                            </asp:TableCell>
                           <asp:TableCell>   
                               <asp:TextBox ID="TextBox35" runat="server" CssClass="form-control" autocomplete="off"  ></asp:TextBox> 
                           </asp:TableCell>
                          <asp:TableCell> 
                              <label class="lblCss" for="txtnewflowname">Flow Step-18 Duration</label><asp:Label runat="server" Text="*" ForeColor="Red"></asp:Label>
                            </asp:TableCell>
                            <asp:TableCell>
                                <asp:TextBox ID="TextBox36" runat="server" CssClass="form-control" autocomplete="off" onkeypress="return isNumber(event)" ></asp:TextBox>
                           </asp:TableCell>
                       </asp:TableRow>
                            <asp:TableRow ID="TableRow19" runat="server" HorizontalAlign="Center" VerticalAlign="Middle" Visible="false">
                           <asp:TableCell>
                           <label class="lblCss" for="txtnewflowname">Flow Step-19 Name</label><asp:Label runat="server" Text="*" ForeColor="Red"></asp:Label> 
                            </asp:TableCell>
                           <asp:TableCell>   
                               <asp:TextBox ID="TextBox37" runat="server" CssClass="form-control" autocomplete="off"  ></asp:TextBox> 
                           </asp:TableCell>
                          <asp:TableCell> 
                              <label class="lblCss" for="txtnewflowname">Flow Step-19 Duration</label><asp:Label runat="server" Text="*" ForeColor="Red"></asp:Label>
                            </asp:TableCell>
                            <asp:TableCell>
                                <asp:TextBox ID="TextBox38" runat="server" CssClass="form-control" autocomplete="off" onkeypress="return isNumber(event)" ></asp:TextBox>
                           </asp:TableCell>
                       </asp:TableRow>
                            <asp:TableRow ID="TableRow20" runat="server" HorizontalAlign="Center" VerticalAlign="Middle" Visible="false">
                           <asp:TableCell>
                           <label class="lblCss" for="txtnewflowname">Flow Step-20 Name</label><asp:Label runat="server" Text="*" ForeColor="Red"></asp:Label> 
                            </asp:TableCell>
                           <asp:TableCell>   
                               <asp:TextBox ID="TextBox39" runat="server" CssClass="form-control" autocomplete="off"  ></asp:TextBox> 
                           </asp:TableCell>
                          <asp:TableCell> 
                              <label class="lblCss" for="txtnewflowname">Flow Step-20 Duration</label><asp:Label runat="server" Text="*" ForeColor="Red"></asp:Label>
                            </asp:TableCell>
                            <asp:TableCell>
                                <asp:TextBox ID="TextBox40" runat="server" CssClass="form-control" autocomplete="off" onkeypress="return isNumber(event)" ></asp:TextBox>
                           </asp:TableCell>
                       </asp:TableRow>
                    </asp:Table>
                    </div>

      </div> 
        <div class="modal-footer">
            <asp:Button ID="btnSubmit" runat="server" Text="Submit" CssClass="btn btn-primary" OnClick="btnSubmit_Click" />
        </div>
         <div id="ModAdd" class="modal it-modal fade">
	    <div class="modal-dialog modal-dialog-centered modal-dialog-scrollable modal-lg">
		    <div class="modal-content">

			    <div class="modal-body">
                    <iframe class="border-0 w-100" style="height:280px;" loading="lazy"></iframe>
			    </div>
              
		    </div>
	    </div>
    </div>
</form>
</asp:Content>
