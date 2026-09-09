<%@ Page Title="" Language="C#" MasterPageFile="~/AppMaster.master" AutoEventWireup="true" CodeFile="AppfreeProduct.aspx.cs" Inherits="AppfreeProduct" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>

<%-- HEADER: back + title --%>
<asp:Content ID="Content3" ContentPlaceHolderID="HeaderContent" runat="Server">
    <a href="wow-package.aspx" class="back-btn" aria-label="Back"><i class="fa fa-arrow-left"></i></a>
    <div class="h-title">Free <span>Products</span></div>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <div class="scroll-body">

        <!-- ═══ HERO ═══ -->
        <div class="wow-hero" style="--hero-a: #6d28d9; --hero-b: #c084fc; --hero-c: #4c1d95;">
            <div class="wow-hero-icon">
                <i class="fa fa-gift"></i>
            </div>
            <div class="wow-hero-text">
                <div class="wow-hero-label">Stanvee Services</div>
                <div class="wow-hero-title">Free Products</div>
                <div class="wow-hero-sub">Apne package ke free rewards claim karein</div>
            </div>
        </div>

        <!-- ═══ SECTION HEADER ═══ -->
        <div class="sec-header">
            <div class="sec-header-title">
                <div class="hdr-dot"></div>
                Available Rewards
            </div>
            <span class="sec-count">
                <asp:Literal ID="litCount" runat="server" />
            </span>
        </div>

        <!-- ═══ PRODUCTS ═══ -->
        <div class="prod-wrap">
            <div id="productGrid" class="prod-grid">

                <asp:Repeater ID="RptOffers" runat="server" OnItemDataBound="rptProducts_ItemDataBound">
                    <ItemTemplate>
                        <div class="prod-card">

                            <div class="prod-img">
                                <img src='<%# Eval("ImageUrl") %>'
                                    alt='<%# Eval("ProductName") %>'
                                    loading="lazy"
                                    onerror="this.style.visibility='hidden'" />
                            </div>

                            <div class="prod-body">
                                <div class="prod-name"><%# Eval("ProductName") %></div>
                                <div class="prod-desc"><%# Eval("Description") %></div>

                                <div class="prod-foot">
                                    <asp:Button ID="btnClaim" runat="server"
                                        Text="Claim Now"
                                        CssClass="claim-btn"
                                        CommandName="Claim"
                                        CommandArgument='<%# Eval("ProductId") %>'
                                        OnCommand="btnClaim_Command" />

                                    <asp:Label ID="lblClaimed" runat="server"
                                        Text="Already Claimed"
                                        CssClass="prod-claimed"
                                        Visible="false" />
                                </div>
                            </div>

                        </div>
                    </ItemTemplate>
                </asp:Repeater>

            </div>

            <!-- Empty state: code-behind se Visible = (count == 0) set karein -->
            <asp:Panel ID="pnlEmpty" runat="server" CssClass="prod-empty" Visible="false">
                <i class="fa fa-box-open"></i>
                <p>Abhi koi free product available nahi hai.</p>
            </asp:Panel>
        </div>

        <div style="height: 12px"></div>

    </div>

</asp:Content>
