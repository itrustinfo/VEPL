<%@ Page Title="" Language="C#" MasterPageFile="~/_master_pages/modal.Master" AutoEventWireup="true" CodeBehind="view-data-form1.aspx.cs" Inherits="ProjectManagementTool._modal_pages.view_data_form1" %>
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
        <div style="text-align:center;padding:10px"><h3><b>COMMENT RESOLUTION SHEET (CRS)</b></h3></div>
        <div class="row">
        <div class="col-lg-12">
            
        <table class="table table-bordered table-responsive">
            <thead>
                <tr class="col-12">
                    <th class="col-md-2">Contractor Letter Ref.No.</th>
                    <th class="col-md-2"><asp:Label runat="server" Text="Label" ID="lblContractorno"></asp:Label></th>
                    <th class="col-md-2">Date</th>
                    <th class="col-md-2"><asp:Label runat="server" Text="Label" ID="lblContractorDate"></asp:Label></th>

                </tr>
                <tr class="col-12">
                    <th class="col-md-2">ONTB Ref.No.</th>
                    <th class="col-md-2"><asp:Label runat="server" Text="Label" ID="lblONTBRefNo"></asp:Label></th>
                    <th class="col-md-2">Date</th>
                    <th class="col-md-2"><asp:Label runat="server" Text="Label" ID="lblONTbdate"></asp:Label></th>
                </tr>
            </thead>
        </table>
         <table class="table table-bordered table-responsive">
            <thead>
                <tr class="col-12">
                    <th class="col-md-12">
                        Design and Construction of New 150 MLD Capacity STP Based on ASP Process with BNR System in lieu of Existing 180 MLD STP at v Valley, Nayandanahalli, Bangalore and Operation and Maintenance for a period of Five (5) years
                    </th>
                </tr>
                 <tr class="col-12">
                    <th class="col-md-12">
                        Document Description : <asp:Label runat="server" Text="" ID="lblDescription"></asp:Label>
                    </th>
                </tr>
            </thead>
        </table>
        <div>
        <table class="table table-bordered table-responsive">
            <thead>
                <tr class="col-lg-12">
                    <th class="col-lg-8">Document No. <asp:Label runat="server" Text="" ID="lblDocNo"></asp:Label></th>
                    <th class="col-lg-4">Latest Status/Action Code X : <asp:Label runat="server" Text="" ID="lblCurrentStatus"></asp:Label></th>
                 </tr>
           </thead>
        </table>
       </div>
       </div>
            </div>
            <div class="col-sm-12" style="padding:0px">
                <div class="table-responsive" style="padding:0px">
                        <asp:GridView ID="GrdDataList" runat="server" Width="100%" CssClass="table table-bordered" EmptyDataText="No Data" AutoGenerateColumns="false" OnRowDataBound="GrdDataList_RowDataBound"  >
                        <Columns>
                            <asp:BoundField DataField="Version"  ItemStyle-HorizontalAlign="Left" HeaderText="Revision" HtmlEncode="False" />
                            <asp:BoundField DataField="SerialNo"  ItemStyle-HorizontalAlign="Left" HeaderText="Serial No." HtmlEncode="False" />
                             <asp:BoundField DataField="Status"  ItemStyle-HorizontalAlign="Left" HeaderText="Status" HtmlEncode="False" />
                            <asp:BoundField DataField="Comment"  ItemStyle-HorizontalAlign="Left" HeaderText="ONTB Comments" HtmlEncode="False" />
                            <asp:BoundField DataField="Reply"  ItemStyle-HorizontalAlign="Left" HeaderText="Contractor Replies" HtmlEncode="False" />
                            <asp:BoundField DataField="Version"  ItemStyle-HorizontalAlign="Left" HeaderText="Version" HtmlEncode="False" ItemStyle-CssClass="hiddencol"  HeaderStyle-CssClass="hiddencol"/>
                        </Columns>
                    </asp:GridView>
                 </div>
             </div>
    </div>
    
    <div class="modal-footer">
            <asp:Button ID="btnSubmit" runat="server" Text="Close" CssClass="btn btn-primary" OnClick="btnSave_Click" />
    </div>
 </form>
</asp:Content>
