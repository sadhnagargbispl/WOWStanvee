<%@ Page Title="" Language="C#" MasterPageFile="~/AppMaster.master" AutoEventWireup="true" CodeFile="Appinsurance.aspx.cs" Inherits="Appinsurance" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script>
        function isNumber(evt) {
            var charCode = evt.which ? evt.which : evt.keyCode;
            if (charCode < 48 || charCode > 57)
                return false;
            return true;
        }
    </script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <div class="scroll-body">

        <!-- ═══ PAGE HEAD ═══ -->
        <div class="page-head">
            <a href="WebApp.aspx" class="page-back"><i class="fa fa-arrow-left"></i>Back</a>
            <div class="page-head-title">Insurance Details</div>
            <div class="page-head-sub">Free 2 Lakh accidental cover ke liye details bharein</div>
        </div>

        <!-- ═══ FORM ═══ -->
        <div class="form-wrap">
            <div class="form-card">

                <div class="form-card-title">Applicant Details</div>

                <div class="form-group">
                    <label for="<%= txtName.ClientID %>">Full Name <span class="required">*</span></label>
                    <asp:TextBox ID="txtName" runat="server" placeholder="Enter your name" />
                </div>

                <div class="form-group">
                    <label for="<%= txtDOB.ClientID %>">Date of Birth <span class="required">*</span></label>
                    <asp:TextBox ID="txtDOB" runat="server" TextMode="Date" />
                </div>

                <div class="form-row">
                    <div class="form-group">
                        <label for="<%= txtMobile.ClientID %>">Mobile Number <span class="required">*</span></label>
                        <asp:TextBox ID="txtMobile" runat="server" MaxLength="10"
                            TextMode="Phone"
                            onkeypress="return isNumber(event)"
                            placeholder="10 digit mobile" />
                    </div>

                    <div class="form-group">
                        <label for="<%= txtEmail.ClientID %>">Email ID <span class="required">*</span></label>
                        <asp:TextBox ID="txtEmail" runat="server" TextMode="Email"
                            placeholder="you@example.com" />
                    </div>
                </div>

                <div class="form-card-title" style="margin-top: 6px;">Nominee Details</div>

                <div class="form-group">
                    <label for="<%= txtNominee.ClientID %>">Nominee Name <span class="required">*</span></label>
                    <asp:TextBox ID="txtNominee" runat="server" placeholder="Enter nominee's name" />
                </div>

                <div class="form-row">
                    <div class="form-group">
                        <label for="<%= txtNomineeDOB.ClientID %>">Nominee DOB <span class="required">*</span></label>
                        <asp:TextBox ID="txtNomineeDOB" runat="server" TextMode="Date" />
                    </div>

                    <div class="form-group">
                        <label for="<%= txtRelation.ClientID %>">Relationship <span class="required">*</span></label>
                        <asp:TextBox ID="txtRelation" runat="server" placeholder="e.g. Father" />
                    </div>
                </div>

                <asp:HiddenField ID="HdnCheckTrnns" runat="server" />

                <div class="form-actions">
                    <asp:Button ID="btnSubmit" runat="server" Text="Submit Now"
                        CssClass="btn-primary"
                        OnClick="btnSubmit_Click" />
                </div>

                <asp:Label ID="lblMsg" runat="server" CssClass="form-msg" />

            </div>

            <div style="height: 8px"></div>
        </div>

    </div>

</asp:Content>
