<%@ Page Title="" Language="C#" MasterPageFile="~/AppMaster.master" AutoEventWireup="true" CodeFile="WebApp.aspx.cs" Inherits="WebApp" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <div class="scroll-body">

        <!-- ═══ HERO ═══ -->
        <div class="hero">
            <div class="hero-greeting">Good Day,</div>
            <div class="hero-name"><%=Session["MemName"] %></div>
            <div class="hero-sub">Stanvee Services India Limited</div>
        </div>

        <!-- ═══ OUR SERVICES ═══ -->
        <div class="nsvc-section">

            <div class="nsvc-header">
                <div class="nsvc-header-title">
                    <div class="hdr-dot"></div>
                    Our Services
               
                </div>
                <span class="sec-count">10 Services</span>
            </div>

            <div class="nsvc-grid">

                <!-- 1. WOW Package -->
                <a class="nsvc-item" href="wow-package.aspx" style="--nsvc-c1: #2dd4bf; --nsvc-c2: #0f766e;">
                    <div class="nsvc-icon" style="background: linear-gradient(145deg, #2dd4bf, #0f766e);">
                        <i class="fa fa-box"></i>
                    </div>
                    <span class="nsvc-name">WOW Package</span>
                    <i class="fa fa-chevron-right nsvc-arrow"></i>
                </a>

                <!-- 2. Full Tank -->
                <a class="nsvc-item" href="https://stanvee.com/full_tank_detail.asp" target="_blank" style="--nsvc-c1: #fbbf24; --nsvc-c2: #b45309;">
                    <div class="nsvc-icon" style="background: linear-gradient(145deg, #fbbf24, #b45309);">
                        <i class="fa fa-gas-pump"></i>
                    </div>
                    <span class="nsvc-name">Full Tank</span>
                    <i class="fa fa-chevron-right nsvc-arrow"></i>
                </a>

                <!-- 3. Utility — pending -->
                <a class="nsvc-item" href="#" style="--nsvc-c1: #c084fc; --nsvc-c2: #6d28d9;" onclick="showToast('Utility coming soon!'); return false;">
                    <div class="nsvc-icon" style="background: linear-gradient(145deg, #c084fc, #6d28d9);">
                        <i class="fa fa-screwdriver-wrench"></i>
                    </div>
                    <span class="nsvc-name">Utility</span>
                    <i class="fa fa-chevron-right nsvc-arrow"></i>
                </a>

                <!-- 4. Gift Voucher — pending -->
                <a class="nsvc-item" href="#" style="--nsvc-c1: #f472b6; --nsvc-c2: #be185d;" onclick="showToast('Gift Voucher coming soon!'); return false;">
                    <div class="nsvc-icon" style="background: linear-gradient(145deg, #f472b6, #be185d);">
                        <i class="fa fa-gift"></i>
                    </div>
                    <span class="nsvc-name">Gift Voucher</span>
                    <i class="fa fa-chevron-right nsvc-arrow"></i>
                </a>

                <!-- 5. Stanvee Shop -->
                <a class="nsvc-item" href="https://shop.stanvee.com/" target="_blank" style="--nsvc-c1: #ff9a5c; --nsvc-c2: #E84000;">
                    <div class="nsvc-icon" style="background: linear-gradient(145deg, #ff9a5c, #E84000);">
                        <i class="fa fa-bag-shopping"></i>
                    </div>
                    <span class="nsvc-name">Stanvee Shop</span>
                    <i class="fa fa-chevron-right nsvc-arrow"></i>
                </a>

                <!-- 6. Brand Store -->
                <a class="nsvc-item" href="BrandStoreRedirect.aspx" style="--nsvc-c1: #34d399; --nsvc-c2: #059669;">
                    <div class="nsvc-icon" style="background: linear-gradient(145deg, #34d399, #059669);">
                        <i class="fa fa-shop"></i>
                    </div>
                    <span class="nsvc-name">Brand Store</span>
                    <i class="fa fa-chevron-right nsvc-arrow"></i>
                </a>

                <!-- 7. Royal Holiday -->
                <a class="nsvc-item" href="https://stanvee.com/royal_package.asp" target="_blank" style="--nsvc-c1: #38bdf8; --nsvc-c2: #0369a1;">
                    <div class="nsvc-icon" style="background: linear-gradient(145deg, #38bdf8, #0369a1);">
                        <i class="fa fa-umbrella-beach"></i>
                    </div>
                    <span class="nsvc-name">Royal Holiday</span>
                    <i class="fa fa-chevron-right nsvc-arrow"></i>
                </a>

                <!-- 8. WOW Holiday -->
                <a class="nsvc-item" href="https://holiday.stanvee.com/" target="_blank" style="--nsvc-c1: #67e8f9; --nsvc-c2: #0e7490;">
                    <div class="nsvc-icon" style="background: linear-gradient(145deg, #67e8f9, #0e7490);">
                        <i class="fa fa-plane-departure"></i>
                    </div>
                    <span class="nsvc-name">WOW Holiday</span>
                    <i class="fa fa-chevron-right nsvc-arrow"></i>
                </a>

                <!-- 9. My Business — pending -->
                <a class="nsvc-item" href="#" style="--nsvc-c1: #818cf8; --nsvc-c2: #3730a3;" onclick="showToast('My Business coming soon!'); return false;">
                    <div class="nsvc-icon" style="background: linear-gradient(145deg, #818cf8, #3730a3);">
                        <i class="fa fa-briefcase"></i>
                    </div>
                    <span class="nsvc-name">My Business</span>
                    <i class="fa fa-chevron-right nsvc-arrow"></i>
                </a>

                <!-- 10. Buy Packages -->
                <a class="nsvc-item" href="AppMM_Voucher.aspx" style="--nsvc-c1: #86efac; --nsvc-c2: #15803d;">
                    <div class="nsvc-icon" style="background: linear-gradient(145deg, #86efac, #15803d);">
                        <i class="fa fa-box-open"></i>
                    </div>
                    <span class="nsvc-name">Buy Packages</span>
                    <i class="fa fa-chevron-right nsvc-arrow"></i>
                </a>

            </div>
        </div>

        <div style="height: 16px"></div>

    </div>

</asp:Content>
