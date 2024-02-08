<%@ Page Title="" Language="C#" MasterPageFile="~/_master_pages/default.Master" AutoEventWireup="true" CodeBehind="default.aspx.cs" Inherits="ProjectManagementTool._content_pages.newform_submittal._default" %>

<asp:Content ID="Content1" ContentPlaceHolderID="default_master_head" runat="server">
    <script type="text/javascript">
        function DeleteItem() {
            if (confirm("Are you sure you want to delete ...?")) {
                return true;
            }
            return false;
        }
    </script>
    <script type="text/javascript">
        $(document).ready(function () {
            $(".showAddUserModal").click(function (e) {
                e.preventDefault();
                var url = $(this).attr("href");
                $("#ModAddUser iframe").attr("src", url);
                $("#ModAddUser").modal("show");
            });

            $(".showEditUserModal").click(function (e) {
                e.preventDefault();
                var url = $(this).attr("href");
                $("#ModEditUser iframe").attr("src", url);
                $("#ModEditUser").modal("show");
            });
        });
    </script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="default_master_body" runat="server">
    <div class="container-fluid">
        <div class="row">
            <div class="col-md-12 col-lg-4 form-group">Submittal Flows</div>
        </div>
    </div>

    <div class="container-fluid">

        <div class="row">

            <div class="col-lg-12 col-xl-12 form-group">

                <div class="card mb-4">

                    <div class="card-body">

                        <div class="card-title">
                            <div class="d-flex justify-content-between">
                                <div class="form-group">
                                </div>

                                <h6 class="text-muted">
                                    <asp:Label ID="ActivityHeading" Text="List of flows" CssClass="text-uppercase font-weight-bold" runat="server" />
                                    <%--<asp:Label Text="Foo bar" CssClass="pl-1" runat="server" />--%>
                                                </h6>
                                <div>
                                    <a href="/_modal_pages/add-newflow.aspx" class="showAddUserModal">
                                        <asp:Button ID="btnadd" runat="server" Text="+ Add Flows" CssClass="btn btn-primary"></asp:Button></a>
                                </div>
                            </div>
                        </div>

                        <div class="table-responsive">
                            <asp:GridView ID="GrdnewFormSubmittal" runat="server" AutoGenerateColumns="False" EmptyDataText="No Data" AllowPaging="true" PageSize="10" Width="100%" CssClass="table table-bordered" OnPageIndexChanging="GrdnewFormSubmittal_PageIndexChanging">
                                <PagerSettings Position="Bottom" />

                                <Columns>
                                    <asp:TemplateField HeaderText="Serial Number">
                                        <ItemTemplate>
                                            <asp:Label ID="lblSerialNumber" runat="server" Text='<%# Container.DataItemIndex + 1 %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="Flow_Name" HeaderText="Flow Name">
                                        <HeaderStyle HorizontalAlign="Left" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="Steps_Count" HeaderText="Total Steps">
                                        <HeaderStyle HorizontalAlign="Left" />
                                    </asp:BoundField>
                                    <asp:TemplateField HeaderText="Status">
                                        <ItemTemplate>
                                            <a id="EditUsers" href='/_modal_pages/add-userflowstatus.aspx/?Flow_Name=<%#Eval("Flow_Name")%>' class="showEditUserModal"><span title="Edit" class="fas fa-edit"></span></a>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <%--add Add New flow modal--%>
    <div id="ModAddUser" class="modal it-modal fade">
        <div class="modal-dialog modal-dialog-centered modal-dialog-scrollable modal-xl">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title">Add New Flow</h5>
                    <button aria-label="Close" class="close" data-dismiss="modal" type="button" onclick="javascript:closepopup();"><span aria-hidden="true">&times;</span></button>
                </div>
                <div class="modal-body">
                    <iframe class="border-0 w-100" style="height: 500px;" loading="lazy"></iframe>
                </div>

            </div>
        </div>
    </div>



    <%--add user flow status modal--%>
    <div id="ModEditUser" class="modal it-modal fade">
        <div class="modal-dialog modal-dialog-centered modal-dialog-scrollable modal-xl">
            <div class="modal-content">
                <div class="modal-header">
                    <h5 class="modal-title">View/ Add Flow Status</h5>
                    <button aria-label="Close" class="close" data-dismiss="modal" type="button"><span aria-hidden="true">&times;</span></button>
                </div>
                <div class="modal-body">
                    <iframe class="border-0 w-100" style="height: 500px;" loading="lazy"></iframe>
                </div>

            </div>
        </div>
    </div>

</asp:Content>
