<%@ Page Title="" Language="C#" MasterPageFile="~/AppMaster.master" AutoEventWireup="true" CodeFile="wow-package.aspx.cs" Inherits="wow_package" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>

<%-- HEADER: back button + title (default hamburger header ko replace karta hai) --%>
<asp:Content ID="Content3" ContentPlaceHolderID="HeaderContent" runat="Server">
    <a href="WebApp.aspx" class="back-btn" aria-label="Back"><i class="fa fa-arrow-left"></i></a>
    <div class="h-title">WOW <span>Package</span></div>
</asp:Content>

<%-- BOTTOM NAV: khaali override = hidden. Agar chahiye to ye poora block hata dijiye. --%>
<asp:Content ID="Content4" ContentPlaceHolderID="BottomNav" runat="Server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <div class="scroll-body">

        <!-- ═══ HERO ═══ -->
        <div class="wow-hero" style="--hero-a: #0f766e; --hero-b: #2dd4bf; --hero-c: #0e7490;">
            <div class="wow-hero-icon">
                <i class="fa fa-box"></i>
            </div>
            <div class="wow-hero-text">
                <div class="wow-hero-label">Stanvee Services</div>
                <div class="wow-hero-title">WOW Package</div>
                <div class="wow-hero-sub">Explore all WOW Package services</div>
            </div>
        </div>

        <!-- ═══ SECTION HEADER ═══ -->
        <div class="sec-header">
            <div class="sec-header-title">
                <div class="hdr-dot"></div>
                Our Services
            </div>
            <span class="sec-count">7 Services</span>
        </div>

        <!-- ═══ SERVICES GRID ═══ -->
        <div class="svc-grid">

            <!-- 1. Holiday -->
            <a class="svc-item" href="https://holiday.stanvee.com/" target="_blank">
                <div class="svc-icon" style="background: linear-gradient(145deg, #38bdf8, #0369a1);">
                    <i class="fa fa-umbrella-beach"></i>
                </div>
                <span class="svc-name">Holiday</span>
            </a>

            <!-- 2. Scratch Card -->
            <a class="svc-item" href="AppScratchCard.aspx">
                <div class="svc-icon" style="background: linear-gradient(145deg, #fbbf24, #b45309);">
                    <i class="fa fa-ticket"></i>
                </div>
                <span class="svc-name">Scratch Card</span>
            </a>

            <!-- 3. Movie -->
            <a class="svc-item" href="MovieBookingRedirect.aspx">
                <div class="svc-icon" style="background: linear-gradient(145deg, #fb7185, #be123c);">
                    <i class="fa fa-clapperboard"></i>
                </div>
                <span class="svc-name">Movie</span>
            </a>

            <!-- 4. Insurance -->
            <a class="svc-item" href="Appinsurance.aspx">
                <div class="svc-icon" style="background: linear-gradient(145deg, #818cf8, #3730a3);">
                    <i class="fa fa-shield-halved"></i>
                </div>
                <span class="svc-name">Insurance</span>
            </a>

            <!-- 5. Stanvee Shop -->
            <a class="svc-item" href="https://shop.stanvee.com/" target="_blank">
                <div class="svc-icon" style="background: linear-gradient(145deg, #ff9a5c, #E84000);">
                    <i class="fa fa-bag-shopping"></i>
                </div>
                <span class="svc-name">Stanvee Shop</span>
            </a>

            <!-- 6. Brand Store -->
            <a class="svc-item" href="BrandStoreRedirect.aspx">
                <div class="svc-icon" style="background: linear-gradient(145deg, #34d399, #059669);">
                    <i class="fa fa-shop"></i>
                </div>
                <span class="svc-name">Brand Store</span>
            </a>

            <!-- 7. Free Product -->
            <a class="svc-item" href="AppfreeProduct.aspx">
                <div class="svc-icon" style="background: linear-gradient(145deg, #c084fc, #6d28d9);">
                    <i class="fa fa-gift"></i>
                </div>
                <span class="svc-name">Free Product</span>
            </a>

        </div>

    </div>

</asp:Content>
