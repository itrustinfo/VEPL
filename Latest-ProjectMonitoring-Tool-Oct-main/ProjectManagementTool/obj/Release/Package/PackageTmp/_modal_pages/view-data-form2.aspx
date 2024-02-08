<%@ Page Title="" Language="C#" MasterPageFile="~/_master_pages/modal.Master" AutoEventWireup="true" CodeBehind="view-data-form2.aspx.cs" Inherits="ProjectManagementTool._modal_pages.view_data_form2" %>
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
    <script type="text/javascript">
        $(document).ready(function () {
           
        });
    </script>
   
    
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="modal_master_body" runat="server">
    <form id="frmDataViewModal" runat="server">
    
    <div class="container-fluid" style="overflow-y:auto; min-height:85vh;">
        <asp:Label runat="server" Text="" Visible="false" ID="lblRevision"></asp:Label>
        <div style="text-align:center;padding:10px"><h3><b>NJSEI COMMENT SHEET</b></h3></div>
        <div class="row">
        <div class="col-lg-12">
             <table class="table table-bordered table-responsive" style="border : 0 hidden hidden !important">
            <thead>
                <tr class="col-12">
                    <th class="col-md-4">Doc No :</th>
                    <th class="col-md-4"><asp:Label runat="server" Text="Label" ID="lblDocNo"></asp:Label></th>
                  <th class="col-md-4">Date : <asp:Label runat="server" Text="Label" ID="lblMainDate"/></th>
                     <th class="col-md-4"></th>

                </tr>
                <tr class="col-12">
                    <th class="col-md-4">Document Ref No :</th>
                      <th class="col-md-4"><asp:Label runat="server" Text="Label" ID="lblDocRefno"></asp:Label></th>
                    <th class="col-md-8"></th>
                
                </tr>
            </thead>

        </table>
         <table class="table table-bordered table-responsive">
            <thead>
                <tr class="col-12">
                    <th class="col-md-4">EPC Letter Ref No :</th>
                    <th class="col-md-4"><asp:Label runat="server" Text="Label" ID="lblContractorno"></asp:Label></th>
                    <th class="col-md-4">Date</th>
                    <th class="col-md-4"><asp:Label runat="server" Text="Label" ID="lblContractorDate"></asp:Label></th>

                </tr>
                <tr class="col-12">
                    <th class="col-md-4">NJSEI Inward Code :</th>
                    <th class="col-md-4"><asp:Label runat="server" Text="-" ID="lblONTBRefNo"></asp:Label></th>
                    <th class="col-md-4">Date</th>
                    <th class="col-md-4"><asp:Label runat="server" Text="Label" ID="lblONTbdate"></asp:Label></th>
                </tr>
            </thead>

        </table>
         <table class="table table-bordered table-responsive">
            <thead>
                <tr class="col-12">
                    <th class="col-md-12">
                        Project : <b><span lang="EN-IN">Design and Construction of New 150MLD STP in lieu of existing 180MLD Capacity STP at V.Valley, Nayandanahalli, Bangalore.</span></b></th>
                </tr>
                 <tr class="col-12">
                    <th class="col-md-12">
                        Document Description : <b><span lang="EN-IN"><asp:Label runat="server" Text="Label" ID="lblDocumentDescription"/></span></b></th>
                </tr>
            </thead>
        </table>
        
       </div>
            </div>
            <div class="col-sm-12" style="padding:0px">
                <div class="table-responsive" style="padding:0px">
                        <asp:GridView ID="GrdDataList" runat="server" Width="100%" CssClass="table table-bordered" EmptyDataText="No Data" AutoGenerateColumns="false" OnRowDataBound="GrdDataList_RowDataBound"  >
                        <Columns>
                            <asp:BoundField DataField="Id"  ItemStyle-HorizontalAlign="Left" HeaderText="Serial No." HtmlEncode="false" />
                            <asp:BoundField DataField="FullName"  ItemStyle-HorizontalAlign="Left" HeaderText="User" HtmlEncode="false" />
                            <asp:BoundField DataField="Comment"  ItemStyle-HorizontalAlign="Left" HeaderText="Comments on Rev-A" HtmlEncode="false" />
                            <asp:BoundField DataField="Status"  ItemStyle-HorizontalAlign="Left" HeaderText="" HtmlEncode="false" />
                         </Columns>
                    </asp:GridView>
                 </div>
             </div>
    </div>
    
    <div class="modal-footer">
            <asp:Button ID="btnSubmit" runat="server" Text="Close" CssClass="btn btn-primary" OnClick="btnSave_Click" Visible="false" />
    </div>
 </form>
</asp:Content>
