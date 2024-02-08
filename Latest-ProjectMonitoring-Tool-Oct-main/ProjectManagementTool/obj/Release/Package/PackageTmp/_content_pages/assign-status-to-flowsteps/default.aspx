<%@ Page Title="" Language="C#" MasterPageFile="~/_master_pages/default.Master" AutoEventWireup="true" CodeBehind="default.aspx.cs" Inherits="ProjectManagementTool._content_pages.assign_status_to_flowsteps._default" %>

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
            <div class="col-md-12 col-lg-4 form-group">Assign Status to FlowSteps</div>
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
            <div class="col-md-6 col-lg-4 form-group">
                <label class="sr-only" for="DDLProject">Flow Step</label>
                <div class="input-group">
                    <div class="input-group-prepend">
                        <span class="input-group-text">Flow Step</span>
                    </div>
                    <asp:DropDownList ID="DDLFlowStep" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="DDLFlowStep_SelectedIndexChanged" Visible="false"></asp:DropDownList>
                    <%--View document modal--%>
                </div>
            </div>
            <div class="col-md-6 col-lg-4 form-group">
                <asp:Button ID="btnSubmit" runat="server" Text="Submit" CssClass="btn btn-primary" OnClick="btnSubmit_Click" Visible="false" />

            </div>
        </div>

        <div class="row">
            <div class="col-sm-12">
                <div class="form-group">
                    <label class="lblCss" for="DDlProject">Select Status</label>
                    <asp:CheckBoxList runat="server" CssClass="form-control chkChoice" ID="chkFlowStatus" RepeatColumns="3" Visible="false" RepeatLayout="Table" RepeatDirection="Horizontal" Height="100%"></asp:CheckBoxList>
                </div>
            </div>
        </div>


    </div>



</asp:Content>
