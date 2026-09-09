<%@ Page Title="" Language="C#" MasterPageFile="~/MainMaster.master" AutoEventWireup="true" CodeFile="insurance.aspx.cs" Inherits="insurance" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script>
        function isNumber(evt) {
            var charCode = evt.which ? evt.which : evt.keyCode;
            if (charCode < 48 || charCode > 57)
                return false;
            return true;
        }
    </script>
    <link href="assets/css/custom_styelsheet.css?v=1" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <section class="section8 destinations" id="holiday-section">
        <div class="destinations-container ">


            <div class="wow-header text-center">
                <h2 class="wow-heading text-center">Insurance Details  </h2>
            </div>







            <div class="insurance">



                <div class="text">

                    <div class="form-box">



                        <!-- Full Name -->
                        <div class="form-group">
                            <label>Full Name <span class="required">*</span></label>
                            <asp:TextBox ID="txtName" runat="server" Placeholder="Enter your name"></asp:TextBox>
                        </div>

                        <!-- Date of Birth -->
                        <div class="form-group">
                            <label>Date of Birth <span class="required">*</span></label>
                            <asp:TextBox ID="txtDOB" runat="server" TextMode="Date"></asp:TextBox>
                        </div>

                        <!-- Mobile -->
                        <div class="form-group">
                            <label>Mobile Number <span class="required">*</span></label>
                            <asp:TextBox ID="txtMobile" runat="server"
                                MaxLength="10"
                                onkeypress="return isNumber(event)"
                                Placeholder="Enter your mobile number"></asp:TextBox>
                        </div>

                        <!-- Email -->
                        <div class="form-group">
                            <label>Email ID <span class="required">*</span></label>
                            <asp:TextBox ID="txtEmail" runat="server"
                                TextMode="Email"
                                Placeholder="Enter your email"></asp:TextBox>
                        </div>

                        <!-- Nominee -->
                        <div class="form-group">
                            <label>Nominee Name <span class="required">*</span></label>
                            <asp:TextBox ID="txtNominee" runat="server"
                                Placeholder="Enter nominee's name"></asp:TextBox>
                        </div>

                        <!-- Nominee DOB -->
                        <div class="form-group">
                            <label>Nominee DOB <span class="required">*</span></label>
                            <asp:TextBox ID="txtNomineeDOB" runat="server"
                                TextMode="Date"></asp:TextBox>
                        </div>

                        <!-- Relationship -->
                        <div class="form-group">
                            <label>Relationship <span class="required">*</span></label>
                            <asp:TextBox ID="txtRelation" runat="server"
                                Placeholder="Enter relationship"></asp:TextBox>
                        </div>
                        <asp:HiddenField ID="HdnCheckTrnns" runat="server" />
                    </div>

                    <div class="shop-cta">
                        <asp:Button ID="btnSubmit" runat="server" Text="Submit Now!"
                            CssClass="white-pill-btn"
                            OnClick="btnSubmit_Click" />
                        <%--   <a href="https://shop.stanvee.com/" target="_blank" class="white-pill-btn">Submit Now! <span><i class="fas fa-arrow-right"></i></span>
                        </a>--%>
                    </div>

                    <asp:Label ID="lblMsg" runat="server" CssClass="text-green-600 font-semibold"></asp:Label>



                </div>
            </div>



        </div>
    </section>
    <!-- Insurance Form -->



    <!-- Mobile Menu Script -->
    <script>
        const btn = document.getElementById('menu-btn');
        const menu = document.getElementById('mobile-menu');

        btn.addEventListener('click', () => {
            menu.classList.toggle('hidden');
        });
    </script>
</asp:Content>

