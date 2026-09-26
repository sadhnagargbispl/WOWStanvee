<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AppIndex.aspx.cs" Inherits="AppIndex" %>

<!DOCTYPE html>

<html>
<head runat="server">
    <title>Stanvee Services India Limited</title>
    <meta name="viewport" content="width=device-width, initial-scale=1, viewport-fit=cover" />
    <link rel="icon" type="image/x-icon" href="images/favicon.png" />
    <link href="https://fonts.googleapis.com/css2?family=Nunito:wght@400;500;600;700;800&display=swap" rel="stylesheet" />
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.5.0/css/all.min.css" />
    <link href="assets/css/AppWeb.css" rel="stylesheet" />
    <link href="assets/css/AppIndex.css?v=1" rel="stylesheet" />
</head>
<body>
    <form id="form1" runat="server">
        <div class="app-root">
            <div class="app-shell" id="appShell">

                <!-- OVERLAY -->
                <div class="overlay" id="overlay" data-ui onclick="closeDrawer()"></div>

                <!-- DRAWER (guest) -->
                <div class="drawer" id="drawer">
                    <button type="button" class="close-btn" data-ui onclick="closeDrawer()"><i class="fa fa-times"></i></button>

                    <div class="drawer-header">
                        <img src="images/logo.png" alt="Stanvee" class="drawer-logo" onerror="this.style.display='none'" />
                        <div class="drawer-brand">
                            <h2>Stanvee Services</h2>
                            <p>Stanvee Services India Limited</p>
                        </div>
                    </div>

                    <div class="ai-guest">
                        <div class="ai-guest-h">Welcome, Guest!</div>
                        <div class="ai-guest-p">Login to unlock all your WOW benefits</div>
                        <a href="AppLogin.aspx" class="claim-btn"><i class="fa fa-sign-in-alt"></i>Login Now</a>
                    </div>

                    <nav class="drawer-nav">
                        <ul>
                            <li><a href="#" class="d-active" data-ui onclick="closeDrawer(); scrollToTop(); return false;"><span class="d-icon"><i class="fa fa-home"></i></span>Home</a></li>
                            <li class="d-hr"></li>
                            <li><a href="wow-package.aspx"><span class="d-icon"><i class="fa fa-box"></i></span>WOW Package</a></li>
                            <li><a href="#"><span class="d-icon"><i class="fa fa-gas-pump"></i></span>Full Tank</a></li>
                            <li><a href="#"><span class="d-icon"><i class="fa fa-bag-shopping"></i></span>Stanvee Shop</a></li>
                            <li><a href="#"><span class="d-icon"><i class="fa fa-shop"></i></span>Brand Store</a></li>
                            <li><a href="#"><span class="d-icon"><i class="fa fa-umbrella-beach"></i></span>Royal Holiday</a></li>
                            <li><a href="#"><span class="d-icon"><i class="fa fa-plane-departure"></i></span>WOW Holiday</a></li>
                            <li><a href="#"><span class="d-icon"><i class="fa fa-box-open"></i></span>Buy Packages</a></li>
                            <li class="d-hr"></li>
                            <li><a href="#"><span class="d-icon"><i class="fa fa-ticket"></i></span>Scratch Card</a></li>
                            <li><a href="#"><span class="d-icon"><i class="fa fa-gift"></i></span>Free Product</a></li>
                            <li><a href="#"><span class="d-icon"><i class="fa fa-shield-halved"></i></span>Insurance</a></li>
                            <li><a href="#"><span class="d-icon"><i class="fa fa-clapperboard"></i></span>Movie Booking</a></li>
                            <li class="d-hr"></li>
                            <li><a href="AppLogin.aspx"><span class="d-icon"><i class="fa fa-sign-in-alt"></i></span>Login</a></li>
                        </ul>
                    </nav>

                    <div class="drawer-footer-note">© 2026 Stanvee Services India Limited. All rights reserved.</div>
                </div>

                <!-- HEADER -->
                <header class="app-header">
                    <div class="h-left">
                        <button type="button" class="hamburger" data-ui onclick="openDrawer()" aria-label="Menu">
                            <span></span><span></span><span></span>
                        </button>
                        <img src="images/logo.png" alt="Stanvee" class="h-logo" onerror="this.style.display='none'" />
                    </div>
                    <div class="h-right">
                        <a href="AppLogin.aspx" class="ai-login-pill" aria-label="Login"><i class="fa fa-sign-in-alt"></i><span>Login</span></a>
                    </div>
                </header>

                <div class="scroll-body ai-body" id="scrollBody">

                    <!-- ═══ HERO SLIDER ═══ -->
                    <div class="ai-hero">
                        <div class="ai-hero-track" id="heroTrack">
                            <div class="ai-slide" style="background-image: url('assets/wow_page_images/hero_slide1.png');">
                                <span class="ai-slide-tag">WOW Package</span>
                                <div class="ai-slide-h">Ek Package.<br />5 <em>WOW</em> Benefits.</div>
                                <div class="ai-slide-p">Hotel stay, movie tickets, ₹2 lakh insurance, ₹5,000 shopping coupon, scratch card &amp; a free product.</div>
                                <button type="button" class="ai-slide-btn">Get Your WOW Package <i class="fa fa-arrow-right"></i></button>
                            </div>
                            <div class="ai-slide" style="background-image: url('assets/wow_page_images/hero_slide2.png');">
                                <span class="ai-slide-tag">Movies &amp; Scratch Card</span>
                                <div class="ai-slide-h">Watch More. Win More.<br /><em>Spend Nothing.</em></div>
                                <div class="ai-slide-p">Two free movie tickets, plus scratch cards with rewards worth up to ₹15,000.</div>
                                <button type="button" class="ai-slide-btn">Explore Benefits <i class="fa fa-arrow-right"></i></button>
                            </div>
                            <div class="ai-slide" style="background-image: url('assets/wow_page_images/hero_slide3.png');">
                                <span class="ai-slide-tag">Shop &amp; Save</span>
                                <div class="ai-slide-h">Experience Unlimited<br />Value with <em>WOW</em>.</div>
                                <div class="ai-slide-p">Shop, watch and enjoy more while spending significantly less.</div>
                                <button type="button" class="ai-slide-btn">Start Now <i class="fa fa-arrow-right"></i></button>
                            </div>
                        </div>
                        <div class="ai-dots" id="heroDots">
                            <button type="button" class="ai-dot active" data-ui aria-label="Slide 1"></button>
                            <button type="button" class="ai-dot" data-ui aria-label="Slide 2"></button>
                            <button type="button" class="ai-dot" data-ui aria-label="Slide 3"></button>
                        </div>
                    </div>

                    <!-- ═══ PRICE STRIP ═══ -->
                    <div class="ai-price">
                        <div class="ai-price-icon"><i class="fa fa-crown"></i></div>
                        <div class="ai-price-text">
                            <div class="ai-price-h">All 5 benefits @ ₹999</div>
                            <div class="ai-price-s">One-time payment · 365-day membership</div>
                        </div>
                        <button type="button" class="ai-price-btn">Buy Now</button>
                    </div>

                    <!-- ═══ OUR SERVICES (WebApp jaisa) ═══ -->
                    <div class="nsvc-section" id="servicesSec">
                        <div class="nsvc-header">
                            <div class="nsvc-header-title">
                                <div class="hdr-dot"></div>
                                Our Services
                            </div>
                            <span class="sec-count">8 Services</span>
                        </div>
                        <div class="nsvc-grid">
                            <a class="nsvc-item" href="#" style="--nsvc-c1: #2dd4bf; --nsvc-c2: #0f766e;">
                                <div class="nsvc-icon" style="background: linear-gradient(145deg, #2dd4bf, #0f766e);"><i class="fa fa-box"></i></div>
                                <span class="nsvc-name">WOW Package</span>
                                <i class="fa fa-chevron-right nsvc-arrow"></i>
                            </a>
                            <a class="nsvc-item" href="#" style="--nsvc-c1: #fbbf24; --nsvc-c2: #b45309;">
                                <div class="nsvc-icon" style="background: linear-gradient(145deg, #fbbf24, #b45309);"><i class="fa fa-gas-pump"></i></div>
                                <span class="nsvc-name">Full Tank</span>
                                <i class="fa fa-chevron-right nsvc-arrow"></i>
                            </a>
                            <a class="nsvc-item" href="#" style="--nsvc-c1: #ff9a5c; --nsvc-c2: #E84000;">
                                <div class="nsvc-icon" style="background: linear-gradient(145deg, #ff9a5c, #E84000);"><i class="fa fa-bag-shopping"></i></div>
                                <span class="nsvc-name">Stanvee Shop</span>
                                <i class="fa fa-chevron-right nsvc-arrow"></i>
                            </a>
                            <a class="nsvc-item" href="#" style="--nsvc-c1: #34d399; --nsvc-c2: #059669;">
                                <div class="nsvc-icon" style="background: linear-gradient(145deg, #34d399, #059669);"><i class="fa fa-shop"></i></div>
                                <span class="nsvc-name">Brand Store</span>
                                <i class="fa fa-chevron-right nsvc-arrow"></i>
                            </a>
                            <a class="nsvc-item" href="#" style="--nsvc-c1: #38bdf8; --nsvc-c2: #0369a1;">
                                <div class="nsvc-icon" style="background: linear-gradient(145deg, #38bdf8, #0369a1);"><i class="fa fa-umbrella-beach"></i></div>
                                <span class="nsvc-name">Royal Holiday</span>
                                <i class="fa fa-chevron-right nsvc-arrow"></i>
                            </a>
                            <a class="nsvc-item" href="#" style="--nsvc-c1: #67e8f9; --nsvc-c2: #0e7490;">
                                <div class="nsvc-icon" style="background: linear-gradient(145deg, #67e8f9, #0e7490);"><i class="fa fa-plane-departure"></i></div>
                                <span class="nsvc-name">WOW Holiday</span>
                                <i class="fa fa-chevron-right nsvc-arrow"></i>
                            </a>
                            <a class="nsvc-item" href="#" style="--nsvc-c1: #fb7185; --nsvc-c2: #be123c;">
                                <div class="nsvc-icon" style="background: linear-gradient(145deg, #fb7185, #be123c);"><i class="fa fa-clapperboard"></i></div>
                                <span class="nsvc-name">Movie Booking</span>
                                <i class="fa fa-chevron-right nsvc-arrow"></i>
                            </a>
                            <a class="nsvc-item" href="#" style="--nsvc-c1: #86efac; --nsvc-c2: #15803d;">
                                <div class="nsvc-icon" style="background: linear-gradient(145deg, #86efac, #15803d);"><i class="fa fa-box-open"></i></div>
                                <span class="nsvc-name">Buy Packages</span>
                                <i class="fa fa-chevron-right nsvc-arrow"></i>
                            </a>
                        </div>
                    </div>

                    <!-- ═══ 5 WOW BENEFITS ═══ -->
                    <div class="ai-sec">
                        <div class="ai-sec-head">
                            <div>
                                <div class="ai-sec-label">What's inside your WOW Package</div>
                                <div class="ai-sec-title">Worth every rupee</div>
                            </div>
                        </div>
                        <div class="ai-scroller">
                            <button type="button" class="ai-benefit" style="--b1: #f59e0b; --b2: #b45309;">
                                <div class="ai-benefit-top">
                                    <div class="ai-benefit-icon"><i class="fa fa-hotel"></i></div>
                                    <div>
                                        <div class="ai-benefit-name">Holiday Stay</div>
                                        <div class="ai-benefit-sub">2N/3D · 3 Star+ Hotels</div>
                                    </div>
                                </div>
                                <img class="ai-benefit-img" src="assets/wow_page_images/beach_retreat.png" alt="Holiday Stay" loading="lazy" />
                                <div class="ai-benefit-body">
                                    <div class="ai-tags"><span class="ai-tag">2 Adults + 2 Kids</span><span class="ai-tag">₹1,000 Food Coupon</span><span class="ai-tag">60+ Destinations</span></div>
                                    <div class="ai-benefit-cta">Explore More <i class="fa fa-arrow-right"></i></div>
                                </div>
                            </button>
                            <button type="button" class="ai-benefit" style="--b1: #fb7185; --b2: #be123c;">
                                <div class="ai-benefit-top">
                                    <div class="ai-benefit-icon"><i class="fa fa-film"></i></div>
                                    <div>
                                        <div class="ai-benefit-name">Movie Tickets</div>
                                        <div class="ai-benefit-sub">2 tickets · Any show</div>
                                    </div>
                                </div>
                                <img class="ai-benefit-img" src="assets/wow_page_images/multiple_screens.png" alt="Movie Tickets" loading="lazy" />
                                <div class="ai-benefit-body">
                                    <div class="ai-tags"><span class="ai-tag">2 Free Tickets</span><span class="ai-tag">1 Year Validity</span><span class="ai-tag">Any Cinema</span></div>
                                    <div class="ai-benefit-cta">Explore More <i class="fa fa-arrow-right"></i></div>
                                </div>
                            </button>
                            <button type="button" class="ai-benefit" style="--b1: #818cf8; --b2: #3730a3;">
                                <div class="ai-benefit-top">
                                    <div class="ai-benefit-icon"><i class="fa fa-shield-halved"></i></div>
                                    <div>
                                        <div class="ai-benefit-name">Accidental Insurance</div>
                                        <div class="ai-benefit-sub">₹2 Lakh cover · Zero extra cost</div>
                                    </div>
                                </div>
                                <img class="ai-benefit-img" src="assets/wow_page_images/injury_protection.png" alt="Accidental Insurance" loading="lazy" />
                                <div class="ai-benefit-body">
                                    <div class="ai-tags"><span class="ai-tag">Accident Death</span><span class="ai-tag">Permanent Disability</span><span class="ai-tag">Partial Disability</span></div>
                                    <div class="ai-benefit-cta">Explore More <i class="fa fa-arrow-right"></i></div>
                                </div>
                            </button>
                            <button type="button" class="ai-benefit" style="--b1: #ff9a5c; --b2: #E84000;">
                                <div class="ai-benefit-top">
                                    <div class="ai-benefit-icon"><i class="fa fa-bag-shopping"></i></div>
                                    <div>
                                        <div class="ai-benefit-name">Shopping Coupon</div>
                                        <div class="ai-benefit-sub">₹5,000 · 10k+ brands</div>
                                    </div>
                                </div>
                                <img class="ai-benefit-img" src="assets/wow_page_images/bluetooth_speakers.png" alt="Shopping Coupon" loading="lazy" />
                                <div class="ai-benefit-body">
                                    <div class="ai-tags"><span class="ai-tag">5,000 Points</span><span class="ai-tag">1 Lakh+ Products</span><span class="ai-tag">Brand Store</span></div>
                                    <div class="ai-benefit-cta">Explore More <i class="fa fa-arrow-right"></i></div>
                                </div>
                            </button>
                            <button type="button" class="ai-benefit" style="--b1: #fbbf24; --b2: #c2410c;">
                                <div class="ai-benefit-top">
                                    <div class="ai-benefit-icon"><i class="fa fa-ticket"></i></div>
                                    <div>
                                        <div class="ai-benefit-name">Scratch Card</div>
                                        <div class="ai-benefit-sub">Win ₹2,500 – ₹15,000 @ ₹699</div>
                                    </div>
                                </div>
                                <img class="ai-benefit-img" src="assets/wow_page_images/scratch_win.png?v=1.5" alt="Scratch Card" loading="lazy" />
                                <div class="ai-benefit-body">
                                    <div class="ai-tags"><span class="ai-tag">Digital / Physical</span><span class="ai-tag">Real Products</span><span class="ai-tag">Quick &amp; Easy</span></div>
                                    <div class="ai-benefit-cta">Explore More <i class="fa fa-arrow-right"></i></div>
                                </div>
                            </button>
                            <button type="button" class="ai-benefit" style="--b1: #c084fc; --b2: #6d28d9;">
                                <div class="ai-benefit-top">
                                    <div class="ai-benefit-icon"><i class="fa fa-gift"></i></div>
                                    <div>
                                        <div class="ai-benefit-name">Free Product</div>
                                        <div class="ai-benefit-sub">Choose any 1 · Yours, on us</div>
                                    </div>
                                </div>
                                <img class="ai-benefit-img" src="assets/wow_page_images/our_gift.png" alt="Free Product" loading="lazy" />
                                <div class="ai-benefit-body">
                                    <div class="ai-tags"><span class="ai-tag">₹0 Price</span><span class="ai-tag">No Minimum Spend</span><span class="ai-tag">Curated Range</span></div>
                                    <div class="ai-benefit-cta">Explore More <i class="fa fa-arrow-right"></i></div>
                                </div>
                            </button>
                        </div>
                    </div>

                    <!-- ═══ 3 STEPS ═══ -->
                    <div class="ai-sec">
                        <div class="ai-sec-head">
                            <div>
                                <div class="ai-sec-label">Simple Process</div>
                                <div class="ai-sec-title">3 Steps. That's all.</div>
                            </div>
                        </div>
                        <div class="ai-steps">
                            <div class="ai-step">
                                <div class="ai-step-no"><i class="fa fa-cart-shopping"></i></div>
                                <div>
                                    <div class="ai-step-k">STEP 01</div>
                                    <div class="ai-step-h">Buy your WOW Package</div>
                                    <div class="ai-step-p">Takes 2 minutes. Pay ₹999 online. Instant confirmation by SMS and email.</div>
                                </div>
                            </div>
                            <div class="ai-step">
                                <div class="ai-step-no"><i class="fa fa-mobile-screen"></i></div>
                                <div>
                                    <div class="ai-step-k">STEP 02</div>
                                    <div class="ai-step-h">Log in &amp; explore 5 benefits</div>
                                    <div class="ai-step-p">Hotels, movies, shopping, insurance, scratch card – all in your app within 24 hours.</div>
                                </div>
                            </div>
                            <div class="ai-step">
                                <div class="ai-step-no"><i class="fa fa-champagne-glasses"></i></div>
                                <div>
                                    <div class="ai-step-k">STEP 03</div>
                                    <div class="ai-step-h">Start using and saving</div>
                                    <div class="ai-step-p">Redeem any benefit anytime within your 365-day membership year.</div>
                                </div>
                            </div>
                        </div>
                    </div>

                    <!-- ═══ SCRATCH CARD PRODUCTS (DB) ═══ -->
                    <div class="ai-sec">
                        <div class="ai-sec-head">
                            <div>
                                <div class="ai-sec-label">Win up to ₹15,000 @ ₹699*</div>
                                <div class="ai-sec-title">Try your luck – Scratch Card</div>
                            </div>
                            <button type="button" class="ai-sec-link">View all</button>
                        </div>
                        <div class="ai-scroller">
                            <asp:Repeater ID="Repeater2" runat="server">
                                <ItemTemplate>
                                    <div class="ai-prod">
                                        <div class="ai-prod-img">
                                            <span class="ai-prod-badge">Scratch to Win</span>
                                            <img src="<%# Eval("ImageUrl") %>" alt="<%# Eval("ProductName") %>" loading="lazy" />
                                        </div>
                                        <div class="ai-prod-body">
                                            <div class="ai-prod-name"><%# Eval("ProductName") %></div>
                                            <div class="ai-prod-price">₹<%# Eval("price") %></div>
                                        </div>
                                    </div>
                                </ItemTemplate>
                            </asp:Repeater>
                        </div>
                        <div class="ai-cta-wrap">
                            <button type="button" class="btn-primary">Try Your Luck <i class="fa fa-arrow-right"></i></button>
                        </div>
                    </div>

                    <!-- ═══ FREE PRODUCTS (DB) ═══ -->
                    <div class="ai-sec">
                        <div class="ai-sec-head">
                            <div>
                                <div class="ai-sec-label">Free Product</div>
                                <div class="ai-sec-title">Pick one. It's free.</div>
                            </div>
                            <button type="button" class="ai-sec-link">View all</button>
                        </div>
                        <div class="ai-scroller">
                            <asp:Repeater ID="Rptfreeproduct" runat="server">
                                <ItemTemplate>
                                    <div class="ai-prod">
                                        <div class="ai-prod-img">
                                            <span class="ai-prod-badge free">FREE</span>
                                            <img src="<%# Eval("ImageUrl") %>" alt="<%# Eval("ProductName") %>" loading="lazy" />
                                        </div>
                                        <div class="ai-prod-body">
                                            <div class="ai-prod-name"><%# Eval("ProductName") %></div>
                                            <div class="ai-prod-price free">FREE</div>
                                        </div>
                                    </div>
                                </ItemTemplate>
                            </asp:Repeater>
                        </div>
                    </div>

                    <!-- ═══ SHOP WITH 5K POINTS ═══ -->
                    <div class="ai-sec">
                        <div class="ai-sec-head">
                            <div>
                                <div class="ai-sec-label">Redeem. Shop. Enjoy.</div>
                                <div class="ai-sec-title">Redeem 5K points instantly</div>
                            </div>
                            <button type="button" class="ai-sec-link">Shop now</button>
                        </div>
                        <div class="ai-scroller">
                            <div class="ai-prod">
                                <div class="ai-prod-img"><img src="assets/wow_page_images/bt_speakers.png" alt="Bluetooth Speaker" loading="lazy" /></div>
                                <div class="ai-prod-body"><div class="ai-prod-name">Bluetooth Speaker</div><div class="ai-prod-price">₹4,590</div></div>
                            </div>
                            <div class="ai-prod">
                                <div class="ai-prod-img"><img src="assets/wow_page_images/lock_master.png" alt="Lock Master" loading="lazy" /></div>
                                <div class="ai-prod-body"><div class="ai-prod-name">Lock Master</div><div class="ai-prod-price">₹7,490</div></div>
                            </div>
                            <div class="ai-prod">
                                <div class="ai-prod-img"><img src="assets/wow_page_images/mixer_juicer.png" alt="Mixer and Juicer" loading="lazy" /></div>
                                <div class="ai-prod-body"><div class="ai-prod-name">Multi-Utility Mixer and Juicer</div><div class="ai-prod-price">₹3,499</div></div>
                            </div>
                            <div class="ai-prod">
                                <div class="ai-prod-img"><img src="assets/wow_page_images/toaster.png" alt="Sandwich Maker" loading="lazy" /></div>
                                <div class="ai-prod-body"><div class="ai-prod-name">Toasty Delight Sandwich Maker</div><div class="ai-prod-price">₹3,190</div></div>
                            </div>
                            <div class="ai-prod">
                                <div class="ai-prod-img"><img src="assets/wow_page_images/bath_towel_set.png" alt="Bath Towel Set" loading="lazy" /></div>
                                <div class="ai-prod-body"><div class="ai-prod-name">Bath Towel Set</div><div class="ai-prod-price">₹1,040</div></div>
                            </div>
                        </div>
                    </div>

                    <!-- ═══ DESTINATIONS ═══ -->
                    <div class="ai-sec">
                        <div class="ai-sec-head">
                            <div>
                                <div class="ai-sec-label">Included with your WOW Card</div>
                                <div class="ai-sec-title">Your 2-night getaway awaits</div>
                            </div>
                            <button type="button" class="ai-sec-link">View all</button>
                        </div>
                        <div class="ai-scroller">
                            <a class="ai-dest" href="#"><img src="assets/wow_page_images/manali_new.jpg" alt="Manali" loading="lazy" /><span class="ai-dest-state">Himachal Pradesh</span><div class="ai-dest-info"><div class="ai-dest-name">Manali</div><div class="ai-dest-sub">Snowy peaks, pine forests</div></div></a>
                            <a class="ai-dest" href="#"><img src="assets/wow_page_images/ooty_new.jpg" alt="Ooty" loading="lazy" /><span class="ai-dest-state">Tamil Nadu</span><div class="ai-dest-info"><div class="ai-dest-name">Ooty</div><div class="ai-dest-sub">Tea Gardens</div></div></a>
                            <a class="ai-dest" href="#"><img src="assets/wow_page_images/puri_new.jpg" alt="Puri" loading="lazy" /><span class="ai-dest-state">Odisha</span><div class="ai-dest-info"><div class="ai-dest-name">Puri</div><div class="ai-dest-sub">Jagannath Temple</div></div></a>
                            <a class="ai-dest" href="#"><img src="assets/wow_page_images/amritsar_new.jpg" alt="Amritsar" loading="lazy" /><span class="ai-dest-state">Punjab</span><div class="ai-dest-info"><div class="ai-dest-name">Amritsar</div><div class="ai-dest-sub">Golden Temple</div></div></a>
                            <a class="ai-dest" href="#"><img src="assets/wow_page_images/agra_new.jpg" alt="Agra" loading="lazy" /><span class="ai-dest-state">Uttar Pradesh</span><div class="ai-dest-info"><div class="ai-dest-name">Agra</div><div class="ai-dest-sub">Taj Mahal</div></div></a>
                            <a class="ai-dest" href="#"><img src="assets/wow_page_images/mysore_new.jpg" alt="Mysore" loading="lazy" /><span class="ai-dest-state">Karnataka</span><div class="ai-dest-info"><div class="ai-dest-name">Mysore</div><div class="ai-dest-sub">Royal Palaces</div></div></a>
                            <a class="ai-dest" href="#"><img src="assets/wow_page_images/jaipur_new.jpg" alt="Jaipur" loading="lazy" /><span class="ai-dest-state">Rajasthan</span><div class="ai-dest-info"><div class="ai-dest-name">Jaipur</div><div class="ai-dest-sub">Hawa Mahal, Pink City</div></div></a>
                        </div>
                    </div>

                    <!-- ═══ STATS + REVIEWS ═══ -->
                    <div class="ai-sec">
                        <div class="ai-sec-head">
                            <div>
                                <div class="ai-sec-label">Don't just take our word for it</div>
                                <div class="ai-sec-title">15 Years. One Million Happy Members.</div>
                            </div>
                        </div>
                        <div class="ai-stats">
                            <div class="ai-stat"><div class="ai-stat-n">15+</div><div class="ai-stat-l">Years</div></div>
                            <div class="ai-stat"><div class="ai-stat-n">10,000+</div><div class="ai-stat-l">Brands</div></div>
                            <div class="ai-stat"><div class="ai-stat-n">Pan-India</div><div class="ai-stat-l">Operations</div></div>
                            <div class="ai-stat"><div class="ai-stat-n">1 Lakh+</div><div class="ai-stat-l">Products</div></div>
                        </div>
                        <div class="ai-scroller">
                            <div class="ai-review">
                                <div class="ai-review-stars"><i class="fa fa-star"></i><i class="fa fa-star"></i><i class="fa fa-star"></i><i class="fa fa-star"></i><i class="fa fa-star"></i></div>
                                <div class="ai-review-text">Went with my friend to Manali on the holiday benefit. 3-star hotel, no extra charges. For ₹999 this is genuinely unbeatable.</div>
                                <div class="ai-review-by"><span class="ai-review-av">R</span>Rajan S., Bengaluru</div>
                            </div>
                            <div class="ai-review">
                                <div class="ai-review-stars"><i class="fa fa-star"></i><i class="fa fa-star"></i><i class="fa fa-star"></i><i class="fa fa-star"></i><i class="fa fa-star"></i></div>
                                <div class="ai-review-text">Won ₹4,500 on the scratch card. Still can't believe it was included in ₹999.</div>
                                <div class="ai-review-by"><span class="ai-review-av">A</span>Aditi K., Pune</div>
                            </div>
                            <div class="ai-review">
                                <div class="ai-review-stars"><i class="fa fa-star"></i><i class="fa fa-star"></i><i class="fa fa-star"></i><i class="fa fa-star"></i><i class="fa fa-star"></i></div>
                                <div class="ai-review-text">Used the movie tickets for a date night. Shopping coupon covered a Philips earphone. Will 100% renew next year.</div>
                                <div class="ai-review-by"><span class="ai-review-av">P</span>Priya M., Delhi</div>
                            </div>
                            <div class="ai-review">
                                <div class="ai-review-stars"><i class="fa fa-star"></i><i class="fa fa-star"></i><i class="fa fa-star"></i><i class="fa fa-star"></i><i class="fa fa-star"></i></div>
                                <div class="ai-review-text">The accidental insurance gave me peace of mind during my trip. Got the free product too. Excellent value!</div>
                                <div class="ai-review-by"><span class="ai-review-av">V</span>Vikram R., Mumbai</div>
                            </div>
                            <div class="ai-review">
                                <div class="ai-review-stars"><i class="fa fa-star"></i><i class="fa fa-star"></i><i class="fa fa-star"></i><i class="fa fa-star"></i><i class="fa fa-star"></i></div>
                                <div class="ai-review-text">Perfect for families! Used the hotel benefit for a trip to Jaipur with kids. No hidden costs. Highly recommend!</div>
                                <div class="ai-review-by"><span class="ai-review-av">A</span>Amit S., Ahmedabad</div>
                            </div>
                        </div>
                    </div>

                    <!-- ═══ FAQ ═══ -->
                    <div class="ai-sec">
                        <div class="ai-sec-head">
                            <div>
                                <div class="ai-sec-label">Got questions?</div>
                                <div class="ai-sec-title">Frequently Asked Questions</div>
                            </div>
                        </div>
                        <div class="ai-faq">
                            <details>
                                <summary>How long is the WOW Package valid? <i class="fa fa-chevron-down"></i></summary>
                                <p>The WOW Card benefits are valid for the period mentioned at the time of purchase. Check the terms for specific validity on each benefit.</p>
                            </details>
                            <details>
                                <summary>Can I gift the WOW Package? <i class="fa fa-chevron-down"></i></summary>
                                <p>Absolutely! The WOW Card makes a perfect gift. Buy it for family, friends, or colleagues.</p>
                            </details>
                            <details>
                                <summary>Is the hotel stay really included? <i class="fa fa-chevron-down"></i></summary>
                                <p>Yes. 2 Nights / 3 Days at a 3-star or above hotel. Choose from 8+ destinations across India. Plus ₹1,000 food coupon.</p>
                            </details>
                            <details>
                                <summary>How does the Scratch Card work? <i class="fa fa-chevron-down"></i></summary>
                                <p>After buying the WOW Card, you can purchase a Scratch Card for ₹699 and reveal a product worth ₹2,500 to ₹15,000.</p>
                            </details>
                            <details>
                                <summary>What is the Free Product? <i class="fa fa-chevron-down"></i></summary>
                                <p>Choose one product from our curated selection at no additional cost. Options include a Sling Bag, Thermo Steel Bottle, or Collapsible Basket.</p>
                            </details>
                            <details>
                                <summary>Any hidden charges? <i class="fa fa-chevron-down"></i></summary>
                                <p>None. One-time payment. All 5 benefits unlocked immediately.</p>
                            </details>
                        </div>
                    </div>

                    <div class="ai-foot">
                        403, Metropolis, Balewadi High Street, Baner, 411045<br />
                        info@stanvee.com · +91 92705 21515<br />
                        © 2026 Stanvee Services India Limited. All rights reserved.
                    </div>

                </div>

                <!-- BOTTOM NAV -->
                <nav class="bottom-nav">
                    <button type="button" class="nav-tab active" data-ui onclick="scrollToTop()">
                        <span class="n-bar"></span>
                        <i class="fa fa-home n-icon"></i>
                        <span>Home</span>
                    </button>
                    <button type="button" class="nav-tab" data-ui onclick="scrollToServices()">
                        <span class="n-bar"></span>
                        <i class="fa fa-grip n-icon"></i>
                        <span>Services</span>
                    </button>
                    <a class="nav-tab" href="#">
                        <span class="n-bar"></span>
                        <i class="fa fa-cart-shopping n-icon"></i>
                        <span>Buy Package</span>
                    </a>
                    <a class="nav-tab" href="AppLogin.aspx">
                        <span class="n-bar"></span>
                        <i class="fa fa-sign-in-alt n-icon"></i>
                        <span>Login</span>
                    </a>
                </nav>

            </div>

            <!-- TOAST -->
            <div class="toast" id="toast"></div>

            <script>
                function openDrawer() {
                    document.getElementById('drawer').classList.add('open');
                    document.getElementById('overlay').classList.add('show');
                }

                function closeDrawer() {
                    document.getElementById('drawer').classList.remove('open');
                    document.getElementById('overlay').classList.remove('show');
                }

                function scrollToTop() {
                    document.getElementById('scrollBody').scrollTo({ top: 0, behavior: 'smooth' });
                }

                function scrollToServices() {
                    var body = document.getElementById('scrollBody');
                    var sec = document.getElementById('servicesSec');
                    body.scrollTo({ top: sec.offsetTop - body.offsetTop, behavior: 'smooth' });
                }

                // Guest landing: every action button/link opens AppLogin (or WebApp if already logged in).
                // Elements marked data-ui (menu, slider dots, scroll tabs) keep their own behaviour.
                (function () {
                    var target = '<%= (Session["Status"] != null && Session["Status"].ToString() == "OK") ? "WebApp.aspx" : "AppLogin.aspx" %>';
                    window.addEventListener('click', function (e) {
                        var el = e.target.closest('a, button, input[type=submit], input[type=button], [onclick]');
                        if (!el || el.closest('[data-ui]')) return;
                        e.preventDefault();
                        e.stopImmediatePropagation();
                        window.location.href = target;
                    }, true);
                })();

                // Hero slider: swipe + auto-play + dots
                (function () {
                    var track = document.getElementById('heroTrack');
                    var dots = document.querySelectorAll('#heroDots .ai-dot');
                    var index = 0, timer;

                    function go(i) {
                        index = (i + dots.length) % dots.length;
                        track.scrollTo({ left: track.clientWidth * index, behavior: 'smooth' });
                    }

                    function restart() {
                        clearInterval(timer);
                        timer = setInterval(function () { go(index + 1); }, 4500);
                    }

                    dots.forEach(function (d, i) {
                        d.addEventListener('click', function () { go(i); restart(); });
                    });

                    track.addEventListener('scroll', function () {
                        var i = Math.round(track.scrollLeft / track.clientWidth);
                        if (i === index && dots[i].classList.contains('active')) return;
                        index = i;
                        dots.forEach(function (d, k) { d.classList.toggle('active', k === i); });
                    }, { passive: true });

                    track.addEventListener('touchstart', function () { clearInterval(timer); }, { passive: true });
                    track.addEventListener('touchend', restart, { passive: true });
                    window.addEventListener('resize', function () { go(index); });
                    restart();
                })();

                // Mobile browser address-bar height fix (fallback for 100dvh)
                function setShellHeight() {
                    var shell = document.getElementById('appShell');
                    if (shell) shell.style.height = window.innerHeight + 'px';
                }
                if (!CSS.supports || !CSS.supports('height', '100dvh')) {
                    setShellHeight();
                    window.addEventListener('resize', setShellHeight);
                    window.addEventListener('orientationchange', setShellHeight);
                }
            </script>
        </div>
    </form>
</body>
</html>
