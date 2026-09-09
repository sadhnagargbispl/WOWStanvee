<%@ Page Title="" Language="C#" MasterPageFile="~/MainMaster.master" AutoEventWireup="true" CodeFile="MM_Voucher.aspx.cs" Inherits="MM_Voucher" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link href="assets/css/custom_styelsheet.css?v=1.5" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <section class="section8 destinations" id="holiday-section">
        <div class="destinations-container ">

            <div class="wow-header text-center">
                <h2 class="wow-heading text-center">Packages We Offer  </h2>
            </div>


            <div class="freeproducts">

                <div id="productGrid" class="package-grid">
                    <!-- Product 1 -->
                    <asp:Repeater ID="RptProducts" runat="server">
                        <ItemTemplate>
                            <div class="product-card">
                                <h2><%# Eval("ProdName") %></h2>
                                <div class="product-img">
                                    <img src="<%# Eval("image_path") %>" alt="">
                                </div>
                                <h2><%# Eval("Name") %></h2>
                                <a href="<%# Eval("Link") %>"
                                    class="claim-btn" style="text-decoration: none;">Buy Now
                                </a>
                                <%-- <p>10 Hours Hot & 20 Hours Cold</p>--%>
                                <%-- <button class="claim-btn">Claim Now</button>--%>
                            </div>

                        </ItemTemplate>
                    </asp:Repeater>
                   <%-- <div class="product-card">
                        <h2>WOW Insurance @999 </h2>
                        <div class="product-img">
                            <img src="wow_page_images/insurance.jpeg" alt="">
                        </div>
                        <p>Free 2 Lakh Insurance</p>
                        <button class="claim-btn">Claim Now</button>
                    </div>--%>

                    <!-- Product 2 -->
                    <%-- <div class="product-card">
                        <h2>WOW Movie @999 </h2>
                        <div class="product-img">
                            <img src="wow_page_images/freeMovies.jpeg" alt="">
                        </div>

                        <p>Free Movie Tickets</p>
                        <button class="claim-btn">Claim Now</button>
                    </div>--%>
                </div>

            </div>



        </div>
    </section>

    <!-- navbar -->



</asp:Content>

