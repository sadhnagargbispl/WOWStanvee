<%@ Page Title="" Language="C#" MasterPageFile="~/MainMaster.master" AutoEventWireup="true" CodeFile="freeProduct.aspx.cs" Inherits="freeProduct" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link href="assets/css/custom_styelsheet.css?v=1.5" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <section class="section8 destinations" id="holiday-section">
        <div class="destinations-container ">

            <div class="wow-header text-center">
                <h2 class="wow-heading text-center">Free Products  </h2>
            </div>


            <div class="freeproducts">

                <div id="productGrid" class="product-grid">
                    <!-- Product 1 -->
                  <%--  <div class="product-card">
                        <div class="product-img">
                            <img src="https://placehold.co/700x400" alt="">
                        </div>
                        <h2>Sports Guard Thermo Steel</h2>
                        <p>10 Hours Hot & 20 Hours Cold</p>
                        <button class="claim-btn">Claim Now</button>
                    </div>--%>

                    <asp:Repeater ID="RptOffers" runat="server" OnItemDataBound="rptProducts_ItemDataBound">
                        <ItemTemplate>
                            <div class="product-card">
                                <div class="product-img">
                                    <img src="<%# Eval("ImageUrl") %>" alt="">
                                </div>
                                <h2><%# Eval("ProductName") %></h2>
                                <p><%# Eval("Description") %></p>
                                <asp:Button ID="btnClaim" runat="server"
                                    Text="Claim Now"
                                    CssClass="claim-btn"
                                    CommandName="Claim"
                                    CommandArgument='<%# Eval("ProductId") %>'
                                    OnCommand="btnClaim_Command" />
                                <asp:Label ID="lblClaimed" runat="server"
                                    Text="Already Claimed"
                                    CssClass="mt-4 w-full bg-gray-400 text-white py-2 rounded-lg font-medium text-center block"
                                    Visible="false" />
                            </div>
                        </ItemTemplate>
                    </asp:Repeater>



                </div>

            </div>



        </div>
    </section>
    <%--    <section class="section8 destinations" id="holiday-section">
        <div class="destinations-container ">
            <div class="wow-header text-center">
                <h2 class="wow-heading text-center">Free Products  </h2>
            </div>
            <div class="freeproducts">
                <div id="productGrid" class="product-grid">
                    <asp:Repeater ID="RptOffers" runat="server" OnItemDataBound="rptProducts_ItemDataBound">
    <ItemTemplate>
         <div class="product-card">
     <div class="product-img">
         <img src="<%# Eval("ImageUrl") %>" alt="">
     </div>
     <h2><%# Eval("ProductName") %></h2>
     <p><%# Eval("Description") %></p>
                <asp:Button ID="btnClaim" runat="server"
       Text="Claim Now"
       CssClass="claim-btn"
       CommandName="Claim"
       CommandArgument='<%# Eval("ProductId") %>'
       OnCommand="btnClaim_Command" />
   <asp:Label ID="lblClaimed" runat="server"
       Text="Already Claimed"
       CssClass="mt-4 w-full bg-gray-400 text-white py-2 rounded-lg font-medium text-center block"
       Visible="false" />  
 </div>
    </ItemTemplate>
</asp:Repeater>
                </div>
            </div>
        </div>
    </section>
    --%>
</asp:Content>

