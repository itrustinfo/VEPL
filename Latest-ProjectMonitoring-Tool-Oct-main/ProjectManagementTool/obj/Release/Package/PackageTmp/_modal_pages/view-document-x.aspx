<%@ Page Title="" Language="C#" MasterPageFile="~/_master_pages/modal.Master" AutoEventWireup="true" CodeBehind="view-document-x.aspx.cs" Inherits="ProjectManagementTool._modal_pages.view_document_x" %>
<asp:Content ID="Content1" ContentPlaceHolderID="modal_master_head" runat="server">
     <script type="text/javascript">
        function DeleteItem() {
            if (confirm("Are you sure you want to delete ...?")) {
                return true;
            }
            return false;
    }
    </script>
    <style type="text/css">
        .hiddencol { display: none; }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="modal_master_body" runat="server">
<form id="frmAddDocumentModal" runat="server">
    
    <div class="container-fluid" style="overflow-y:auto; min-height:85vh;">
        <div class="row">
           <div class="col-sm-6">
                <div class="form-group">
                       <div class="form-group" >
                        <label class="lblCss" for="lblWorkPackage">Workpackage Name</label>&nbsp;&nbsp;<b>:</b>&nbsp;&nbsp;
                         <asp:Label ID="lblWorkPackage" class="lblCss" Font-Bold="true" runat="server"></asp:Label>
                    </div>
                 </div>
           </div>
            <div class="col-sm-6">
                 <div class="form-group">
                        <label class="lblCss" for="LblDocName">Original Document Name</label>&nbsp;&nbsp;<b>:</b>&nbsp;&nbsp;
                         <asp:Label ID="LblDocName" class="lblCss" Font-Bold="true" runat="server"></asp:Label>
                    </div>
                </div>
        </div>
          <div class="row">
           <div class="col-sm-6">
                <div class="form-group">
                       <div class="form-group" >
                        
                    </div>
                 </div>
           </div>
            <div class="col-sm-6">
                 <div class="form-group">
                        <label class="lblCss" for="LblDocNameLatest">Latest Document Name</label>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;<b>:</b>&nbsp;&nbsp;
                         <asp:Label ID="LblDocNameLatest" class="lblCss" Font-Bold="true" runat="server"></asp:Label>
                    </div>
                </div>
        </div>

        <div class="row">
           
        <div class="row">
            <div class="col-sm-12">
                <div class="table-responsive">
                        <asp:GridView ID="GrdDocStatus" runat="server" Width="100%" CssClass="table table-bordered" DataKeyNames="StatusUID" EmptyDataText="No Status Found" AutoGenerateColumns="false" OnRowDataBound="GrdDocStatus_RowDataBound" OnRowCommand="GrdDocStatus_RowCommand" OnRowDeleting="GrdDocStatus_RowDeleting">
                        <Columns>
                           
                            <asp:BoundField DataField="ActivityType" HeaderText="Phase Name" >
                            <HeaderStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                            <asp:BoundField DataField="ActivityDate" HeaderText="Actual Date" DataFormatString="{0:dd MMM yyyy}">
                            <HeaderStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Current_Status" HeaderText="Status" HtmlEncode="false" ItemStyle-Width="200px" HeaderStyle-Width="200px">
                            <HeaderStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                            <asp:BoundField DataField="Status_Comments" HeaderText="Comments" HtmlEncode="false">
                            <HeaderStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                            <asp:BoundField DataField="LinkToReviewFile" ItemStyle-CssClass="hiddencol" HtmlEncode="false"  HeaderStyle-CssClass="hiddencol" HeaderText="LinkToReviewFile" >
                            <HeaderStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                            <asp:TemplateField HeaderText="Link Review File">
                                             <ItemTemplate>
                                                     <asp:LinkButton ID="lnkdown" runat="server" CommandArgument='<%#Eval("StatusUID")%>' CausesValidation="false" CommandName="download">Download</asp:LinkButton>
                                             </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="CoverLetterFile" ItemStyle-CssClass="hiddencol"  HeaderStyle-CssClass="hiddencol" HeaderText="CoverLetterFile" >
                            <HeaderStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                            <asp:TemplateField HeaderText="Cover Letter">
                                             <ItemTemplate>
                                                    <asp:LinkButton ID="lnkcoverdownload" runat="server" CommandArgument='<%#Eval("StatusUID")%>' CausesValidation="false" CommandName="Cover Download">Download</asp:LinkButton>
                                             </ItemTemplate>
                             </asp:TemplateField>
                           
                    
                        </Columns>
                    </asp:GridView>
                           </div>
                </div>
            </div>
    </div>
    <div class="modal-footer">
         <%--<button type="button" class="btn btn-secondary" data-dismiss="modal">Close</button>--%>
    </div>

   
 </form>
</asp:Content>
