<%@ Page Title="" Language="C#" MasterPageFile="~/_master_pages/default.Master" AutoEventWireup="true" CodeBehind="default.aspx.cs" Inherits="ProjectManagementTool._content_pages.add_flow_status._default" %>

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
            <div class="col-md-12 col-lg-4 form-group">Flow Status Master</div>
        </div>
    </div>

    <div class="container-fluid">

        <div class="row">
            <asp:HiddenField ID="HiddenPaging" runat="server" />
            <div class="col-md-6 col-lg-4 form-group">
                <label class="sr-only" for="DDLProject">Flow</label>
                <div class="input-group">
                    <div class="input-group-prepend">
                        <span class="input-group-text">Flow</span>
                    </div>
                    <asp:DropDownList ID="DDlFlow" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="DDlFlow_SelectedIndexChanged"></asp:DropDownList>
                    <%--View document modal--%>
                </div>

            </div>
            <div class="col-md-6 col-lg-4 form-group" id="divWP" runat="server">
                <label class="sr-only" for="DDLWorkPackage">Status</label>
                <div class="input-group">
                    <div class="input-group-prepend">
                        <span class="input-group-text">Flow Status</span>
                    </div>
                    <asp:TextBox ID="TXTFlowStatus" runat="server" CssClass="form-control" Visible="true" EnableViewState="false" Placeholder="Please Enter Status"></asp:TextBox>


                    <%--edit document modal--%>
                </div>
            </div>
            <div>
                <asp:Button ID="btnadd" runat="server" Text="+ Add Status" CssClass="btn btn-primary" OnClick="btnadd_Click" Visible="true"></asp:Button>

            </div>


            <div class="col-lg-12 col-xl-12 form-group">

                <div class="card mb-4">

                    <div class="card-body">

                        <div class="card-title">
                            <div class="d-flex justify-content-between">


                                <h6 class="text-muted">
                                    <asp:Label ID="ActivityHeading" Text="Existing Status" CssClass="text-uppercase font-weight-bold" runat="server" Visible="true" />
                                    <%--<asp:Label Text="Foo bar" CssClass="pl-1" runat="server" />--%>
                                                </h6>
                                <div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div class="row">

            <div class="col-lg-12 col-xl-12 form-group">
                <div class="table-responsive">
                    <asp:GridView ID="GrdFlowStatus" runat="server" AutoGenerateColumns="False" EmptyDataText="No Data" AllowPaging="true" PageSize="10" Width="100%" CssClass="table table-bordered" OnPageIndexChanging="GrdFlowStatus_PageIndexChanging" OnRowCommand="GrdFlowStatus_RowCommand" OnRowDeleting="GrdFlowStatus_RowDeleting" Visible="false">
                        <PagerSettings Position="Bottom" />

                        <Columns>
                            <asp:TemplateField HeaderText="Serial Number">
                                <ItemTemplate>
                                    <asp:Label ID="lblSerialNumber" runat="server" Text='<%# Container.DataItemIndex + 1 %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="Status" HeaderText="Status">
                                <HeaderStyle HorizontalAlign="Left" />
                            </asp:BoundField>
                            <asp:TemplateField HeaderText="">
                                <ItemTemplate>
                                  <asp:LinkButton ID="lnkdelete" runat="server" OnClientClick="return DeleteItem()" CausesValidation="false" CommandArgument='<%#Eval("FlowStatusUID")%>' CommandName="Delete"><span title="Delete" class="fas fa-trash"></span></asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
