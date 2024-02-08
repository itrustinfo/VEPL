<%@ Page Title="" Language="C#" MasterPageFile="~/_master_pages/default.Master" AutoEventWireup="true" CodeBehind="default.aspx.cs" Inherits="ProjectManagementTool._content_pages.assign_current_next_flowstepstatus._default" %>

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
            <div class="col-md-12 col-lg-4 form-group">Assign Current-Next Status to FlowSteps</div>
        </div>
    </div>

    <div class="container-fluid">

        <div class="row">

            <div class="col-md-6 col-lg-4 form-group">
                <div class="form-group" id="div1" runat="server">
                    <label class="lblCss" for="DDlFLow">Flow</label>
                    <asp:DropDownList ID="DDLFlow" runat="server" CssClass="form-control" AutoPostBack="true" Placeholder="Select Flow" OnSelectedIndexChanged="DDLFlow_SelectedIndexChanged">
                    </asp:DropDownList>
                </div>
            </div>
            <div class="col-md-6 col-lg-4 form-group">
                <div class="form-group" id="divFlowStep" runat="server">
                    <label class="lblCss" for="DDLFlowStep">Flow Step</label>
                    <asp:DropDownList ID="DDLFlowStep" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="DDLFlowStep_SelectedIndexChanged">
                    </asp:DropDownList>
                </div>
            </div>
            <div class="col-md-12">
                <!-- Your grid control goes here -->
                <asp:GridView ID="GrdCurrentNextStatus" runat="server" AutoGenerateColumns="False" EmptyDataText="No Data" AllowPaging="true" PageSize="10" Width="100%" CssClass="table table-bordered" Visible="false">
                    <PagerSettings Position="Bottom" />

                    <Columns>
                        <asp:TemplateField HeaderText="Serial Number">
                            <ItemTemplate>
                                <asp:Label ID="lblSerialNumber" runat="server" Text='<%# Container.DataItemIndex + 1 %>'></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="Current_Status" HeaderText="Current Staus">
                            <HeaderStyle HorizontalAlign="Left" />
                        </asp:BoundField>
                        <asp:BoundField DataField="Update_Status" HeaderText="Next Status">
                            <HeaderStyle HorizontalAlign="Left" />
                        </asp:BoundField>
                        <asp:BoundField DataField="UserType" HeaderText="User Type">
                            <HeaderStyle HorizontalAlign="Left" />
                        </asp:BoundField>

                    </Columns>

                </asp:GridView>
            </div>
            <div class="col-md-6 col-lg-4 form-group">
                <div class="form-group" id="divCurrentstatus" runat="server">
                    <label class="lblCss" for="DDlUserType">Current Status</label>
                    <asp:DropDownList ID="DDLCurrentStatus" runat="server" CssClass="form-control" Placeholder="Select Current Status" AutoPostBack="true" OnSelectedIndexChanged="DDLCurrentStatus_SelectedIndexChanged">
                    </asp:DropDownList>
                </div>
            </div>
            <div class="col-md-6 col-lg-4 form-group">
                <div class="form-group">
                    <label class="lblCss" for="DDlProject">Update Status</label>
                    <asp:DropDownList ID="DDLNextStatus" runat="server" CssClass="form-control" Placeholder="Select Next Status" AutoPostBack="true" OnSelectedIndexChanged="DDLNextStatus_SelectedIndexChanged">
                    </asp:DropDownList>
                </div>
            </div>

        </div>

        <div class="row">
            <div class="col-md-12 form-group">

                <div class="form-group" id="divUserType" runat="server">
                    <label class="lblCss" for="DDlUserType">User Type</label>
                    <div style="overflow: scroll;">
                        <asp:CheckBoxList runat="server" CssClass="form-control chkChoice" ID="ChkUserType" RepeatColumns="6" Visible="true" RepeatLayout="Table" RepeatDirection="Horizontal" Height="80%">
                        </asp:CheckBoxList>
                    </div>

                </div>
            </div>

        </div>
    </div>
    <div class="row">
        <div class="col-md-12 form-group">
            <div class="page-footer">
                <asp:Button ID="btnSubmit" runat="server" Text="Submit" CssClass="btn btn-primary" OnClick="btnSubmit_Click" Visible="true" />
            </div>
        </div>

    </div>

</asp:Content>



