<%@ Page Title="" Language="C#" MasterPageFile="~/AppMaster.master" AutoEventWireup="true" CodeFile="AppMM_Voucher.aspx.cs" Inherits="AppMM_Voucher" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>

<%-- HEADER: back + title --%>
<asp:Content ID="Content3" ContentPlaceHolderID="HeaderContent" runat="Server">
    <a href="WebApp.aspx" class="back-btn" aria-label="Back"><i class="fa fa-arrow-left"></i></a>
    <div class="h-title">Buy <span>Packages</span></div>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <div class="scroll-body">

        <!-- ═══ HERO ═══ -->
        <div class="wow-hero" style="--hero-a: #15803d; --hero-b: #86efac; --hero-c: #14532d;">
            <div class="wow-hero-icon">
                <i class="fa fa-box-open"></i>
            </div>
            <div class="wow-hero-text">
                <div class="wow-hero-label">Stanvee Services</div>
                <div class="wow-hero-title">Packages We Offer</div>
                <div class="wow-hero-sub">Apne liye best package chunein</div>
            </div>
        </div>

        <!-- ═══ SECTION HEADER ═══ -->
        <div class="sec-header">
            <div class="sec-header-title">
                <div class="hdr-dot"></div>
                Available Packages
            </div>
            <span class="sec-count">
                <asp:Literal ID="litCount" runat="server" />
            </span>
        </div>

        <!-- ═══ PACKAGES ═══ -->
        <div class="prod-wrap">
            <div id="productGrid" class="prod-grid">

                <asp:Repeater ID="RptProducts" runat="server">
                    <ItemTemplate>
                        <div class="prod-card is-package">

                            <div class="prod-tag"><%# Eval("ProdName") %></div>

                            <div class="prod-img">
                                <img src='<%# Eval("image_path") %>'
                                    alt='<%# Eval("ProdName") %>'
                                    loading="lazy"
                                    onerror="this.style.visibility='hidden'" />
                            </div>

                            <div class="prod-body">
                                <div class="prod-name"><%# Eval("Name") %></div>

                                <div class="prod-foot">
                                    <a href='<%# Eval("Link") %>' class="claim-btn">Buy Now</a>
                                </div>
                            </div>

                        </div>
                    </ItemTemplate>
                </asp:Repeater>

            </div>

            <!-- Empty state: code-behind se Visible = (count == 0) -->
            <asp:Panel ID="pnlEmpty" runat="server" CssClass="prod-empty" Visible="false">
                <i class="fa fa-box-open"></i>
                <p>Abhi koi package available nahi hai.</p>
            </asp:Panel>
        </div>

        <div style="height: 12px"></div>

    </div>

</asp:Content>
