<%@ Page Title="" Language="C#" MasterPageFile="~/MainMaster.master" AutoEventWireup="true" CodeFile="Login.aspx.cs" Inherits="Login" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link href="assets/css/custom_styelsheet.css?v=1" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">


    <section class="section8 destinations" id="holiday-section">
        <div class="destinations-container ">


            <div class="wow-header text-center">
                <h2 class="wow-heading text-center">Login Account  </h2>
            </div>






            <div class="login">
                <div class="text">

                    <div class="form-box">

                        <h2 class="form-title">User Login</h2>

                        <!-- Email / Username -->
                        <div class="form-group">
                            <label>User ID <span class="required">*</span></label>
                            <asp:TextBox ID="TxtUserID" runat="server" placeholder="Enter Email ID"></asp:TextBox>
                        </div>
                        <!-- Password -->
                        <div class="form-group">
                            <label>Password <span class="required">*</span></label>
                            <asp:TextBox ID="TxtPassword" runat="server" placeholder="Enter Password" TextMode="password"></asp:TextBox>
                        </div>

                        <!-- Remember + Forgot -->
                        <div class="form-extra">
                            <label>
                                <input type="checkbox">
                                Remember Me</label>
                            <a href="#">Forgot Password?</a>
                        </div>

                        <!-- Login Button -->
                        <%--<button class="login-btn">Login</button>--%>
                        <asp:Button ID="BtnLogin" runat="server" Text="Login" class="login-btn" OnClick="BtnLogin_Click" />

                        <!-- Register -->
                        <p class="register-text">
                            Don't have an account? <a href="#">Register</a>
                        </p>

                    </div>

                </div>
            </div>



        </div>
    </section>




</asp:Content>

