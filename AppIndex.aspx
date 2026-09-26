<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AppIndex.aspx.cs" Inherits="AppIndex" %>

<!DOCTYPE html>

<html lang="en">
<head runat="server">
    <meta charset="UTF-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0, viewport-fit=cover" />
    <meta name="theme-color" content="#1A1A2E" />
    <title>Stanvee Services India Limited</title>
    <link rel="icon" type="image/x-icon" href="images/favicon.png" />

    <link rel="preconnect" href="https://fonts.googleapis.com" />
    <link rel="preconnect" href="https://fonts.gstatic.com" crossorigin />
    <link href="https://fonts.googleapis.com/css2?family=Nunito:wght@400;600;700;800&family=Playfair+Display:ital,wght@1,700&display=swap" rel="stylesheet" />
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.5.0/css/all.min.css" />

    <link href="assets/css/AppIndex.css?v=3" rel="stylesheet" />
</head>
<body>
    <form id="form1" runat="server">

        <!-- ═══ HEADER ═══ -->
        <header class="apx-header">
            <div class="wrap">
                <div class="apx-left">
                    <button type="button" class="apx-hamburger" data-ui onclick="apxOpenDrawer()" aria-label="Menu">
                        <span></span><span></span><span></span>
                    </button>
                    <img src="assets/wow_page_images/logo.png" alt="Stanvee" class="apx-logo" />
                </div>
                <nav class="apx-nav" data-ui>
                    <a href="#benefits">Benefits</a>
                    <a href="#how">How it works</a>
                    <a href="#holiday-section">Holidays</a>
                    <a href="#faq">FAQs</a>
                </nav>
                <a href="AppLogin.aspx" class="apx-login"><i class="fa fa-sign-in-alt"></i><span>Login</span></a>
            </div>
        </header>

        <!-- ═══ DRAWER ═══ -->
        <div class="apx-overlay" id="apxOverlay" data-ui onclick="apxCloseDrawer()"></div>
        <aside class="apx-drawer" id="apxDrawer">
            <button type="button" class="apx-close" data-ui onclick="apxCloseDrawer()" aria-label="Close"><i class="fa fa-times"></i></button>
            <div class="apx-d-head">
                <img src="assets/wow_page_images/logo.png" alt="Stanvee" class="apx-logo" />
                <div class="apx-d-sub">Stanvee Services India Limited</div>
            </div>
            <div class="apx-guest">
                <div class="apx-guest-h">Welcome, Guest!</div>
                <div class="apx-guest-p">Login to unlock all your WOW benefits</div>
                <a href="AppLogin.aspx" class="apx-login"><i class="fa fa-sign-in-alt"></i>Login Now</a>
            </div>
            <div class="apx-d-links">
                <a href="MM_Voucher.aspx"><i class="fa fa-box"></i>WOW Package</a>
                <a href="#"><i class="fa fa-gas-pump"></i>Full Tank</a>
                <a href="#"><i class="fa fa-umbrella-beach"></i>Royal Package</a>
                <div class="apx-d-hr"></div>
                <a href="#"><i class="fa fa-bag-shopping"></i>Shop</a>
                <a href="#"><i class="fa fa-shop"></i>Brand Store</a>
                <div class="apx-d-hr"></div>
                <a href="#"><i class="fa fa-plane-departure"></i>Holiday</a>
                <a href="#"><i class="fa fa-shield-halved"></i>Insurance</a>
                <a href="#"><i class="fa fa-ticket"></i>Scratch Card</a>
                <a href="#"><i class="fa fa-gift"></i>Free Product</a>
                <a href="#"><i class="fa fa-clapperboard"></i>Movies</a>
                <div class="apx-d-hr"></div>
                <a href="AppLogin.aspx"><i class="fa fa-sign-in-alt"></i>Login</a>
            </div>
            <div class="apx-d-foot">© 2026 Stanvee Services India Limited. All rights reserved.</div>
        </aside>

        <main>

            <!-- ═══ HERO SLIDER ═══ -->
            <section class="hero" id="hero">
                <div class="hero-track" id="heroTrack">

                    <div class="hero-slide" style="background-image: url('assets/wow_page_images/hero_slide1.png');">
                        <div class="wrap">
                            <div class="hero-content">
                                <span class="hero-badge"><i class="fa fa-star"></i>WOW PACKAGE @ ₹999</span>
                                <h1 class="hero-title">Ek Package.<br />
                                    5 <span class="serif">WOW</span> Benefits.<br />
                                    Unlimited Value.</h1>
                                <p class="hero-desc">
                                    Hotel stays, movie tickets, ₹2 lakh insurance, ₹5,000 shopping coupon, scratch card prizes up to
                                    ₹15,000, and a free product – all in one package that costs less than a dinner for two.
                                </p>
                                <div class="hero-actions">
                                    <a href="https://wow.stanvee.com/MM_Voucher.aspx" class="btn btn-primary">Get Your WOW Package <i class="fas fa-arrow-right"></i></a>
                                </div>
                            </div>
                        </div>
                    </div>

                    <div class="hero-slide" style="background-image: url('assets/wow_page_images/hero_slide2.png');">
                        <div class="wrap">
                            <div class="hero-content">
                                <span class="hero-badge"><i class="fa fa-film"></i>MOVIES + SCRATCH &amp; WIN</span>
                                <h1 class="hero-title">Watch More. Win More.<br />
                                    <span class="serif">Spend Nothing.</span></h1>
                                <p class="hero-desc">
                                    Enjoy your favourite films with two free movie tickets in your WOW package. Or scratch a card and
                                    win premium products worth up to ₹15,000 – because luck should pay off.
                                </p>
                                <div class="hero-actions">
                                    <a href="https://wow.stanvee.com/MM_Voucher.aspx" class="btn btn-primary">Get Your WOW Package <i class="fas fa-arrow-right"></i></a>
                                </div>
                            </div>
                        </div>
                    </div>

                    <div class="hero-slide" style="background-image: url('assets/wow_page_images/hero_slide3.png');">
                        <div class="wrap">
                            <div class="hero-content">
                                <span class="hero-badge"><i class="fa fa-bag-shopping"></i>SHOP MORE. SPEND LESS</span>
                                <h1 class="hero-title">Experience Unlimited<br />
                                    Value with <span class="serif">WOW</span> Benefits.</h1>
                                <p class="hero-desc">
                                    Unlock premium value and exclusive rewards with the WOW Package membership today. Shop, watch, and
                                    enjoy more while spending significantly less.
                                </p>
                                <div class="hero-actions">
                                    <a href="https://wow.stanvee.com/MM_Voucher.aspx" class="btn btn-primary">Get Your WOW Package <i class="fas fa-arrow-right"></i></a>
                                </div>
                            </div>
                        </div>
                    </div>

                </div>
                <div class="hero-dots" id="heroDots" data-ui>
                    <button type="button" class="active" aria-label="Slide 1"></button>
                    <button type="button" aria-label="Slide 2"></button>
                    <button type="button" aria-label="Slide 3"></button>
                </div>
            </section>

            <!-- ═══ PRICE STRIP ═══ -->
            <div class="price-strip">
                <div class="wrap">
                    <div class="price-card">
                        <div>
                            <div class="p-lbl">One-time payment · 365 days</div>
                            <div class="p-val">₹999 <small>/ year</small></div>
                        </div>
                        <a href="https://wow.stanvee.com/MM_Voucher.aspx" class="btn btn-primary">Buy Now <i class="fas fa-arrow-right"></i></a>
                    </div>
                </div>
            </div>

            <!-- ═══ QUICK BENEFITS ═══ -->
            <section class="sec" style="padding-bottom: 0;">
                <div class="wrap">
                    <div class="quick-grid" data-ui>
                        <div class="quick-item" data-go="stay"><i class="fa fa-hotel"></i><span>Holiday Stay</span><small>2N / 3D</small></div>
                        <div class="quick-item" data-go="movies"><i class="fa fa-clapperboard"></i><span>Movie Tickets</span><small>2 Free</small></div>
                        <div class="quick-item" data-go="insurance"><i class="fa fa-shield-halved"></i><span>Insurance</span><small>₹2 Lakh</small></div>
                        <div class="quick-item" data-go="shopping"><i class="fa fa-bag-shopping"></i><span>Shopping</span><small>₹5,000</small></div>
                        <div class="quick-item" data-go="scratch"><i class="fa fa-ticket"></i><span>Scratch Card</span><small>Upto ₹15K</small></div>
                        <div class="quick-item" data-go="product"><i class="fa fa-gift"></i><span>Free Product</span><small>On us</small></div>
                    </div>
                </div>
            </section>

            <!-- ═══ BENEFITS ═══ -->
            <section class="sec" id="benefits">
                <div class="wrap">
                    <div class="sec-head">
                        <span class="eyebrow">What's inside your WOW Package?</span>
                        <h2 class="sec-title">Five reasons it's worth <span class="serif accent">every rupee</span></h2>
                    </div>

                    <div class="benefit-layout">
                        <div class="tabs" id="benefitTabs" data-ui>
                            <button type="button" class="tab" data-target="stay"><i class="fa fa-hotel"></i>Holiday Stay</button>
                            <button type="button" class="tab" data-target="movies"><i class="fa fa-clapperboard"></i>Movie Tickets</button>
                            <span class="tab-or">OR</span>
                            <button type="button" class="tab active" data-target="insurance"><i class="fa fa-shield-halved"></i>Accidental Insurance</button>
                            <button type="button" class="tab" data-target="shopping"><i class="fa fa-bag-shopping"></i>Shopping Coupon</button>
                            <button type="button" class="tab" data-target="scratch"><i class="fa fa-ticket"></i>Scratch Card</button>
                            <button type="button" class="tab" data-target="product"><i class="fa fa-gift"></i>Free Product</button>
                        </div>

                        <div class="panels">

                            <!-- Holiday Stay -->
                            <div class="panel t-stay" id="panel-stay">
                                <div class="panel-card">
                                    <div class="panel-banner">
                                        <div class="pb-top">
                                            <div class="pb-ic"><i class="fa fa-hotel"></i></div>
                                            <div>
                                                <div class="pb-name">Holiday Stay</div>
                                                <div class="pb-meta">2N/3D · 3 Star+ Hotels · India-Wide</div>
                                            </div>
                                        </div>
                                        <h3>From Manali to Mysore. Jaipur to Puri. Your escape is already paid for.</h3>
                                        <div class="tags">
                                            <span class="tag">3 Star+ Hotels</span>
                                            <span class="tag">2 Nights &amp; 3 Days</span>
                                            <span class="tag">60+ Holiday Destinations</span>
                                        </div>
                                    </div>
                                    <div class="features">
                                        <div class="feature"><i class="fas fa-users"></i><div><div class="fv">Family</div><div class="fl">2 Adults + 2 Children</div></div></div>
                                        <div class="feature"><i class="fas fa-utensils"></i><div><div class="fv">₹1,000</div><div class="fl">Food Coupon</div></div></div>
                                        <div class="feature"><i class="fas fa-umbrella-beach"></i><div><div class="fv">@ ₹1,999</div><div class="fl">At the time of booking</div></div></div>
                                    </div>
                                    <div class="asset-row">
                                        <img src="assets/wow_page_images/beach_retreat.png" alt="Beach retreat" loading="lazy" />
                                        <img src="assets/wow_page_images/hill_side.png" alt="Hill side" loading="lazy" />
                                        <img src="assets/wow_page_images/cultural_cities.png" alt="Cultural cities" loading="lazy" />
                                    </div>
                                    <div class="panel-foot"><a href="https://holiday.stanvee.com/" class="btn btn-primary">Explore Holidays <i class="fas fa-arrow-right"></i></a></div>
                                </div>
                            </div>

                            <!-- Movie Tickets -->
                            <div class="panel t-movies" id="panel-movies">
                                <div class="panel-card">
                                    <div class="panel-banner">
                                        <div class="pb-top">
                                            <div class="pb-ic"><i class="fa fa-clapperboard"></i></div>
                                            <div>
                                                <div class="pb-name">Movie Tickets</div>
                                                <div class="pb-meta">2 Tickets · Any Show · Any Cinema</div>
                                            </div>
                                        </div>
                                        <h3>Date Night Sorted. Family Outing Sorted.</h3>
                                        <div class="tags">
                                            <span class="tag">Latest in Cinema</span>
                                            <span class="tag">Your Choice of Film</span>
                                            <span class="tag">Any Showtime</span>
                                        </div>
                                    </div>
                                    <div class="features">
                                        <div class="feature"><i class="fas fa-ticket"></i><div><div class="fv">2</div><div class="fl">Free Tickets</div></div></div>
                                        <div class="feature"><i class="far fa-calendar-alt"></i><div><div class="fv">1 Year</div><div class="fl">Validity</div></div></div>
                                        <div class="feature"><i class="fas fa-film"></i><div><div class="fv">Any</div><div class="fl">Film, any screen</div></div></div>
                                    </div>
                                    <div class="asset-row">
                                        <img src="assets/wow_page_images/multiple_screens.png" alt="Multiple screens" loading="lazy" />
                                        <img src="assets/wow_page_images/muliplex_partners.png" alt="Multiplex partners" loading="lazy" />
                                        <img src="assets/wow_page_images/nationwide_coverage.png" alt="Nationwide coverage" loading="lazy" />
                                    </div>
                                    <div class="panel-foot"><a href="MovieBookingRedirect.aspx" class="btn btn-primary">Book Movies <i class="fas fa-arrow-right"></i></a></div>
                                </div>
                            </div>

                            <!-- Insurance -->
                            <div class="panel t-insurance active" id="panel-insurance">
                                <div class="panel-card">
                                    <div class="panel-banner">
                                        <div class="pb-top">
                                            <div class="pb-ic"><i class="fa fa-shield-halved"></i></div>
                                            <div>
                                                <div class="pb-name">Accidental Insurance</div>
                                                <div class="pb-meta">₹2 Lakh Cover · Zero Extra Cost</div>
                                            </div>
                                        </div>
                                        <h3>Peace of mind doesn't have a price tag. But if it did, it's already included.</h3>
                                        <div class="tags">
                                            <span class="tag">₹2 Lakh Accidental Insurance</span>
                                            <span class="tag">Activated Automatically</span>
                                            <span class="tag">Full Year Coverage</span>
                                        </div>
                                    </div>
                                    <div class="features">
                                        <div class="feature"><i class="fas fa-shield-halved"></i><div><div class="fv">Accidental Death</div><div class="fl">Coverage</div></div></div>
                                        <div class="feature"><i class="fas fa-shield-halved"></i><div><div class="fv">Permanent Disability</div><div class="fl">Coverage</div></div></div>
                                        <div class="feature"><i class="fas fa-shield-halved"></i><div><div class="fv">Partial Disability</div><div class="fl">Coverage</div></div></div>
                                    </div>
                                    <div class="asset-row">
                                        <img src="assets/wow_page_images/injury_protection.png" alt="Injury protection" loading="lazy" />
                                        <img src="assets/wow_page_images/emergency_assistance.png" alt="Emergency assistance" loading="lazy" />
                                        <img src="assets/wow_page_images/easy_claim.png" alt="Easy claim" loading="lazy" />
                                    </div>
                                    <div class="panel-foot"><a href="insurance.aspx" class="btn btn-primary">View Insurance <i class="fas fa-arrow-right"></i></a></div>
                                </div>
                            </div>

                            <!-- Shopping -->
                            <div class="panel t-shopping" id="panel-shopping">
                                <div class="panel-card">
                                    <div class="panel-banner">
                                        <div class="pb-top">
                                            <div class="pb-ic"><i class="fa fa-bag-shopping"></i></div>
                                            <div>
                                                <div class="pb-name">Shopping Coupon</div>
                                                <div class="pb-meta">₹5,000 · 1 Lakh+ Products · 10K Brands</div>
                                            </div>
                                        </div>
                                        <h3>Kitchen appliances. Bluetooth speakers. Shop smart. Save big.</h3>
                                        <div class="tags">
                                            <span class="tag">1 Lakh+ Products</span>
                                            <span class="tag">10,000+ Brands</span>
                                            <span class="tag">Electronics to Kitchenware</span>
                                        </div>
                                    </div>
                                    <div class="features">
                                        <div class="feature"><i class="fas fa-ticket-alt"></i><div><div class="fv">5,000 Points</div><div class="fl">Coupon Value</div></div></div>
                                        <div class="feature"><i class="fas fa-store"></i><div><a class="fv" href="https://brandstore.stanveeservices.com/">Brand Store</a><div class="fl">Use it in our Brand Store</div></div></div>
                                        <div class="feature"><i class="fas fa-box"></i><div><a class="fv" href="https://shop.stanvee.com/">Stanvee Shop</a><div class="fl">Use it in our Shop</div></div></div>
                                    </div>
                                    <div class="asset-row">
                                        <img src="assets/wow_page_images/bluetooth_speakers.png" alt="Bluetooth speakers" loading="lazy" />
                                        <img src="assets/wow_page_images/kitchen_appliances.png" alt="Kitchen appliances" loading="lazy" />
                                        <img src="assets/wow_page_images/headphones_gadgets.png" alt="Headphones & gadgets" loading="lazy" />
                                    </div>
                                    <div class="panel-foot"><a href="https://shop.stanvee.com/" class="btn btn-primary">Start Shopping <i class="fas fa-arrow-right"></i></a></div>
                                </div>
                            </div>

                            <!-- Scratch Card -->
                            <div class="panel t-scratch" id="panel-scratch">
                                <div class="panel-card">
                                    <div class="panel-banner">
                                        <div class="pb-top">
                                            <div class="pb-ic"><i class="fa fa-ticket"></i></div>
                                            <div>
                                                <div class="pb-name">Scratch Card</div>
                                                <div class="pb-meta">Win ₹2,500 – ₹15,000 @ ₹699*</div>
                                            </div>
                                        </div>
                                        <h3>Scratch the card. Reveal your product. It's that easy.</h3>
                                        <div class="tags">
                                            <span class="tag">Digital / Physical Card</span>
                                            <span class="tag">Real Physical Products</span>
                                            <span class="tag">Quick and Easy</span>
                                        </div>
                                    </div>
                                    <div class="features">
                                        <div class="feature"><i class="fas fa-credit-card"></i><div><div class="fv">₹2.5K – ₹15K</div><div class="fl">Worth of Products</div></div></div>
                                        <div class="feature"><i class="fas fa-hand-holding-dollar"></i><div><div class="fv">₹699</div><div class="fl">Your Price</div></div></div>
                                        <div class="feature"><i class="fas fa-boxes-stacked"></i><div><div class="fv">Curated Range</div><div class="fl">Of our products</div></div></div>
                                    </div>
                                    <div class="asset-row">
                                        <img src="assets/wow_page_images/scratch_win.png?v=1.5" alt="Scratch & win" loading="lazy" />
                                        <img src="assets/wow_page_images/30_products.png?v=1.5" alt="30 products" loading="lazy" />
                                        <img src="assets/wow_page_images/scratch_away.png?v=1.5" alt="Scratch away" loading="lazy" />
                                    </div>
                                    <div class="panel-foot"><a href="ScratchCard.aspx" class="btn btn-primary">Try Your Luck <i class="fas fa-arrow-right"></i></a></div>
                                </div>
                            </div>

                            <!-- Free Product -->
                            <div class="panel t-product" id="panel-product">
                                <div class="panel-card">
                                    <div class="panel-banner">
                                        <div class="pb-top">
                                            <div class="pb-ic"><i class="fa fa-gift"></i></div>
                                            <div>
                                                <div class="pb-name">Free Product</div>
                                                <div class="pb-meta">Choose any 1 product · Yours, on us</div>
                                            </div>
                                        </div>
                                        <h3>Choose any one product from our collection. Yours, on us.</h3>
                                        <div class="tags">
                                            <span class="tag">Your Choice of Product</span>
                                            <span class="tag">No Minimum Spend</span>
                                            <span class="tag">Select Range of Products</span>
                                        </div>
                                    </div>
                                    <div class="features">
                                        <div class="feature"><i class="fas fa-gift"></i><div><div class="fv">FREE!</div><div class="fl">Product of your choice</div></div></div>
                                        <div class="feature"><i class="fas fa-square-check"></i><div><div class="fv">₹0</div><div class="fl">Your Price</div></div></div>
                                        <div class="feature"><i class="fas fa-table-cells-large"></i><div><div class="fv">Your Choice</div><div class="fl">From our curated selection</div></div></div>
                                    </div>
                                    <div class="asset-row">
                                        <img src="assets/wow_page_images/our_gift.png" alt="Our gift" loading="lazy" />
                                        <img src="assets/wow_page_images/no_money_spent.png" alt="No money spent" loading="lazy" />
                                        <img src="assets/wow_page_images/choose_from_selection.png" alt="Choose from selection" loading="lazy" />
                                    </div>
                                    <div class="panel-foot"><a href="freeProduct.aspx" class="btn btn-primary">Claim Free Product <i class="fas fa-arrow-right"></i></a></div>
                                </div>
                            </div>

                        </div>
                    </div>
                </div>
            </section>

            <!-- ═══ 3 STEPS ═══ -->
            <section class="sec" id="how" style="padding-top: 0;">
                <div class="wrap">
                    <div class="sec-head">
                        <span class="eyebrow">Simple Process</span>
                        <h2 class="sec-title">3 Steps. <span class="serif accent">That's All</span></h2>
                    </div>
                    <div class="steps">
                        <div class="step">
                            <div class="step-ic">
                                <svg viewBox="0 0 56 49" fill="none" xmlns="http://www.w3.org/2000/svg"><path d="M31.068 37.244L27.552 28.984L23.538 38.49L8.498 31.91L8.848 39.722L27.722 48.47L49.542 37.74L49.242 27.76L31.068 37.244ZM55.82 6.476L33.952 0L27.942 5.34L21.762 0.624L0 9.636L7.592 15.73L0.732 26.336L22.492 35.848L27.552 23.852L32.064 34.468L54.85 22.584L48.64 13.164L55.824 6.474L55.82 6.476ZM27.466 22.396L9.346 14.736L27.898 6.532L47.112 13.042L27.466 22.396Z" fill="#FF7637" /></svg>
                            </div>
                            <div>
                                <div class="step-no">STEP 01</div>
                                <h3>Buy your WOW Package online</h3>
                                <p>Takes 2 minutes. Pay ₹999 via payment gateway. Instant confirmation by SMS and email.</p>
                            </div>
                        </div>
                        <div class="step">
                            <div class="step-ic"><i class="fas fa-mobile-alt"></i></div>
                            <div>
                                <div class="step-no">STEP 02</div>
                                <h3>Log in and explore your 5 benefits</h3>
                                <p>Hotels, movies, shopping, insurance, scratch card – all waiting in your member portal within 24 hours.</p>
                            </div>
                        </div>
                        <div class="step">
                            <div class="step-ic"><i class="fas fa-champagne-glasses"></i></div>
                            <div>
                                <div class="step-no">STEP 03</div>
                                <h3>Start using and saving</h3>
                                <p>It really is that simple. Redeem any benefit anytime within your 365-day membership year.</p>
                            </div>
                        </div>
                    </div>
                </div>
            </section>

            <!-- ═══ MOVIES ═══ -->
            <section class="sec sec-movies" id="movies-section">
                <div class="wrap">
                    <div class="sec-head">
                        <span class="eyebrow">Now Showing</span>
                        <h2 class="sec-title">Choose your <span class="serif accent">Preferred Movie</span></h2>
                        <p class="sec-sub">2 free tickets · Any film · Any screen</p>
                    </div>
                    <div class="carousel">
                        <button type="button" class="c-btn prev" data-ui aria-label="Previous"><i class="fas fa-chevron-left"></i></button>
                        <div class="c-track">
                            <div class="movie">
                                <div class="movie-poster"><img src="assets/movies/ustaad.png" alt="Ustaad" loading="lazy" /></div>
                                <h3>Ustaad</h3>
                                <div class="movie-meta"><span class="badge">U/A 13+</span>Telugu, Tamil</div>
                            </div>
                            <div class="movie">
                                <div class="movie-poster"><img src="assets/movies/biker_poster.png" alt="Biker" loading="lazy" /></div>
                                <h3>Biker</h3>
                                <div class="movie-meta"><span class="badge">U/A 13+</span>Telugu, Tamil, Malayalam</div>
                            </div>
                            <div class="movie">
                                <div class="movie-poster"><img src="assets/movies/dhurandar_poster.png" alt="Dhurandar: The Revenge" loading="lazy" /></div>
                                <h3>Dhurandar: The Revenge</h3>
                                <div class="movie-meta"><span class="badge">A 18+</span>Hindi, Telugu, Tamil, Kannada</div>
                            </div>
                            <div class="movie">
                                <div class="movie-poster"><img src="assets/movies/hail_mary_poster.png" alt="Project Hail Mary" loading="lazy" /></div>
                                <h3>Project Hail Mary</h3>
                                <div class="movie-meta"><span class="badge">U/A 13+</span>English, Hindi, Telugu, Tamil</div>
                            </div>
                            <div class="movie">
                                <div class="movie-poster"><img src="assets/movies/assi.png" alt="Asur" loading="lazy" /></div>
                                <h3>Asur</h3>
                                <div class="movie-meta"><span class="badge">A 18+</span>Hindi</div>
                            </div>
                        </div>
                        <button type="button" class="c-btn next" data-ui aria-label="Next"><i class="fas fa-chevron-right"></i></button>
                    </div>
                    <div class="sec-cta"><a href="http://movie.stanvee.com/" class="btn btn-primary">Browse the latest movies <i class="fas fa-arrow-right"></i></a></div>
                </div>
            </section>

            <!-- ═══ SHOP (5K points) ═══ -->
            <section class="sec sec-shop" id="shopping-section">
                <div class="wrap">
                    <div class="sec-head">
                        <span class="eyebrow">Redeem. Shop. Enjoy</span>
                        <h2 class="sec-title">Redeem 5K points instantly</h2>
                        <p class="sec-sub">with your <span class="serif">WOW Package</span></p>
                    </div>
                    <div class="carousel">
                        <button type="button" class="c-btn prev" data-ui aria-label="Previous"><i class="fas fa-chevron-left"></i></button>
                        <div class="c-track">
                            <div class="pcard">
                                <div class="pcard-img"><img src="assets/wow_page_images/bt_speakers.png" alt="Bluetooth Speaker" loading="lazy" /></div>
                                <div class="pcard-body"><h4>Bluetooth Speaker</h4><span class="rating">4.3 ★</span><span class="price">₹4,590</span></div>
                            </div>
                            <div class="pcard">
                                <span class="corner">SECURE</span>
                                <div class="pcard-img"><img src="assets/wow_page_images/lock_master.png" alt="Lock Master" loading="lazy" /></div>
                                <div class="pcard-body"><h4>Lock Master</h4><span class="rating">4.3 ★</span><span class="price">₹7,490</span></div>
                            </div>
                            <div class="pcard">
                                <div class="pcard-img"><img src="assets/wow_page_images/mixer_juicer.png" alt="Multi-Utility Mixer and Juicer" loading="lazy" /></div>
                                <div class="pcard-body"><h4>Multi-Utility Mixer and Juicer</h4><span class="rating">4.7 ★</span><span class="price">₹3,499</span></div>
                            </div>
                            <div class="pcard">
                                <div class="pcard-img"><img src="assets/wow_page_images/toaster.png" alt="Toasty Delight Sandwich Maker" loading="lazy" /></div>
                                <div class="pcard-body"><h4>Toasty Delight Sandwich Maker</h4><span class="rating">4.5 ★</span><span class="price">₹3,190</span></div>
                            </div>
                            <div class="pcard">
                                <div class="pcard-img"><img src="assets/wow_page_images/bath_towel_set.png" alt="Bath Towel Set" loading="lazy" /></div>
                                <div class="pcard-body"><h4>Bath Towel Set</h4><span class="rating">4.2 ★</span><span class="price">₹1,040</span></div>
                            </div>
                        </div>
                        <button type="button" class="c-btn next" data-ui aria-label="Next"><i class="fas fa-chevron-right"></i></button>
                    </div>
                    <div class="sec-cta"><a href="https://shop.stanvee.com/" class="btn btn-light">Start Shopping Now! <i class="fas fa-arrow-right"></i></a></div>
                </div>
            </section>

            <!-- ═══ SCRATCH CARD (from DB) ═══ -->
            <section class="sec sec-scratch" id="scratch-section">
                <div class="wrap">
                    <div class="sec-head">
                        <span class="eyebrow">Win upto ₹15,000 @ ₹699*</span>
                        <h2 class="sec-title">Try your luck with our <span class="serif accent">Scratch Card</span></h2>
                    </div>
                    <div class="carousel">
                        <button type="button" class="c-btn prev" data-ui aria-label="Previous"><i class="fas fa-chevron-left"></i></button>
                        <div class="c-track">
                            <asp:Repeater ID="Repeater2" runat="server">
                                <ItemTemplate>
                                    <div class="pcard">
                                        <div class="pcard-img"><img src="<%# Eval("ImageUrl") %>" alt="<%# Eval("ProductName") %>" loading="lazy" /></div>
                                        <div class="pcard-body">
                                            <h4><%# Eval("ProductName") %></h4>
                                            <span class="price">₹<%# Eval("price") %></span>
                                            <span class="pill-scratch">Scratch to Win</span>
                                        </div>
                                    </div>
                                </ItemTemplate>
                            </asp:Repeater>
                        </div>
                        <button type="button" class="c-btn next" data-ui aria-label="Next"><i class="fas fa-chevron-right"></i></button>
                    </div>
                    <div class="sec-cta"><a href="ScratchCard.aspx" class="btn btn-primary">Try Your Luck <i class="fas fa-arrow-right"></i></a></div>
                </div>
            </section>

            <!-- ═══ FREE PRODUCT (from DB) ═══ -->
            <section class="sec sec-free" id="product-section">
                <div class="wrap">
                    <div class="sec-head">
                        <span class="eyebrow">Free Product</span>
                        <h2 class="sec-title">Pick One. <span class="serif">It's Free.</span></h2>
                    </div>
                    <div class="carousel">
                        <button type="button" class="c-btn prev" data-ui aria-label="Previous"><i class="fas fa-chevron-left"></i></button>
                        <div class="c-track">
                            <asp:Repeater ID="Rptfreeproduct" runat="server">
                                <ItemTemplate>
                                    <div class="pcard">
                                        <div class="pcard-img"><img src="<%# Eval("ImageUrl") %>" alt="<%# Eval("ProductName") %>" loading="lazy" /></div>
                                        <div class="pcard-body">
                                            <h4><%# Eval("ProductName") %></h4>
                                            <span class="rating">4.2 ★</span>
                                            <span class="free">FREE</span>
                                        </div>
                                    </div>
                                </ItemTemplate>
                            </asp:Repeater>
                        </div>
                        <button type="button" class="c-btn next" data-ui aria-label="Next"><i class="fas fa-chevron-right"></i></button>
                    </div>
                    <div class="sec-cta"><a href="freeProduct.aspx" class="btn btn-light">Claim Your Free Product <i class="fas fa-arrow-right"></i></a></div>
                </div>
            </section>

            <!-- ═══ DESTINATIONS ═══ -->
            <section class="sec" id="holiday-section">
                <div class="wrap">
                    <div class="sec-head">
                        <span class="eyebrow">Included with your WOW Card</span>
                        <h2 class="sec-title">Where will your WOW Card <span class="serif accent">take you?</span></h2>
                        <p class="sec-sub">Pack your bags — your 2-night getaway awaits.</p>
                    </div>
                    <div class="dest-grid">
                        <a class="dest" href="HolidayRedirect.aspx"><span class="state">Himachal Pradesh</span><img src="assets/wow_page_images/manali_new.jpg" alt="Manali" loading="lazy" /><div class="d-info"><h3>Manali</h3><p>Snowy peaks, pine forests</p><span class="d-book">BOOK NOW <i class="fas fa-arrow-right"></i></span></div></a>
                        <a class="dest" href="HolidayRedirect.aspx"><span class="state">Tamil Nadu</span><img src="assets/wow_page_images/ooty_new.jpg" alt="Ooty" loading="lazy" /><div class="d-info"><h3>Ooty</h3><p>Tea Gardens</p><span class="d-book">BOOK NOW <i class="fas fa-arrow-right"></i></span></div></a>
                        <a class="dest" href="HolidayRedirect.aspx"><span class="state">Odisha</span><img src="assets/wow_page_images/puri_new.jpg" alt="Puri" loading="lazy" /><div class="d-info"><h3>Puri</h3><p>Jagannath Temple</p><span class="d-book">BOOK NOW <i class="fas fa-arrow-right"></i></span></div></a>
                        <a class="dest" href="HolidayRedirect.aspx"><span class="state">Punjab</span><img src="assets/wow_page_images/amritsar_new.jpg" alt="Amritsar" loading="lazy" /><div class="d-info"><h3>Amritsar</h3><p>Golden Temple</p><span class="d-book">BOOK NOW <i class="fas fa-arrow-right"></i></span></div></a>
                        <a class="dest" href="HolidayRedirect.aspx"><span class="state">Uttar Pradesh</span><img src="assets/wow_page_images/agra_new.jpg" alt="Agra" loading="lazy" /><div class="d-info"><h3>Agra</h3><p>Taj Mahal</p><span class="d-book">BOOK NOW <i class="fas fa-arrow-right"></i></span></div></a>
                        <a class="dest" href="HolidayRedirect.aspx"><span class="state">Karnataka</span><img src="assets/wow_page_images/mysore_new.jpg" alt="Mysore" loading="lazy" /><div class="d-info"><h3>Mysore</h3><p>Royal Palaces</p><span class="d-book">BOOK NOW <i class="fas fa-arrow-right"></i></span></div></a>
                        <a class="dest" href="HolidayRedirect.aspx"><span class="state">Rajasthan</span><img src="assets/wow_page_images/jaipur_new.jpg" alt="Jaipur" loading="lazy" /><div class="d-info"><h3>Jaipur</h3><p>Hawa Mahal, Pink City</p><span class="d-book">BOOK NOW <i class="fas fa-arrow-right"></i></span></div></a>
                    </div>
                    <div class="sec-cta"><a href="https://holiday.stanvee.com/destination.aspx" class="btn btn-ghost">Explore Destinations <i class="fas fa-arrow-right"></i></a></div>
                </div>
            </section>

            <!-- ═══ STATS + REVIEWS ═══ -->
            <section class="sec sec-reviews">
                <div class="wrap">
                    <div class="sec-head">
                        <span class="eyebrow">Don't just take our word for it</span>
                        <h2 class="sec-title">15 Years. <span class="serif accent">One Million Happy Members.</span></h2>
                    </div>
                    <div class="stats">
                        <div class="stat"><b>15+</b><span>YEARS IN INDUSTRY</span></div>
                        <div class="stat"><b>10,000+</b><span>BRANDS</span></div>
                        <div class="stat"><b>Pan-India</b><span>OPERATIONS</span></div>
                        <div class="stat"><b>1,00,000+</b><span>PRODUCTS</span></div>
                    </div>
                    <div class="carousel">
                        <button type="button" class="c-btn prev" data-ui aria-label="Previous"><i class="fas fa-chevron-left"></i></button>
                        <div class="c-track">
                            <div class="review"><i class="fa fa-quote-left q"></i><p>Went with my friend to Manali on the holiday benefit. 3-star hotel, no extra charges. For ₹999 this is genuinely unbeatable.</p><div class="r-foot"><span class="r-name">Rajan S., Bengaluru</span><span class="stars"><i class="fas fa-star"></i><i class="fas fa-star"></i><i class="fas fa-star"></i><i class="fas fa-star"></i><i class="fas fa-star"></i></span></div></div>
                            <div class="review"><i class="fa fa-quote-left q"></i><p>Won ₹4,500 on the scratch card. Redeemed it for Pantaloons + Starbucks. Still can't believe it was included in ₹999.</p><div class="r-foot"><span class="r-name">Aditi K., Pune</span><span class="stars"><i class="fas fa-star"></i><i class="fas fa-star"></i><i class="fas fa-star"></i><i class="fas fa-star"></i><i class="fas fa-star"></i></span></div></div>
                            <div class="review"><i class="fa fa-quote-left q"></i><p>Used the movie tickets for a date night. Shopping coupon covered a Philips earphone for my friend. Will 100% renew next year.</p><div class="r-foot"><span class="r-name">Priya M., Delhi</span><span class="stars"><i class="fas fa-star"></i><i class="fas fa-star"></i><i class="fas fa-star"></i><i class="fas fa-star"></i><i class="fas fa-star"></i></span></div></div>
                            <div class="review"><i class="fa fa-quote-left q"></i><p>The accidental insurance gave me peace of mind during my trip. Got the free product – a nice water bottle. Excellent value!</p><div class="r-foot"><span class="r-name">Vikram R., Mumbai</span><span class="stars"><i class="fas fa-star"></i><i class="fas fa-star"></i><i class="fas fa-star"></i><i class="fas fa-star"></i><i class="fas fa-star"></i></span></div></div>
                            <div class="review"><i class="fa fa-quote-left q"></i><p>Booked a weekend getaway to Mysore. Used the shopping voucher for groceries. This membership pays for itself multiple times over.</p><div class="r-foot"><span class="r-name">Neha P., Chennai</span><span class="stars"><i class="fas fa-star"></i><i class="fas fa-star"></i><i class="fas fa-star"></i><i class="fas fa-star"></i><i class="fas fa-star"></i></span></div></div>
                            <div class="review"><i class="fa fa-quote-left q"></i><p>Perfect for families! Used the hotel benefit for a trip to Jaipur with kids. No hidden costs, everything included. Highly recommend!</p><div class="r-foot"><span class="r-name">Amit S., Ahmedabad</span><span class="stars"><i class="fas fa-star"></i><i class="fas fa-star"></i><i class="fas fa-star"></i><i class="fas fa-star"></i><i class="fas fa-star"></i></span></div></div>
                        </div>
                        <button type="button" class="c-btn next" data-ui aria-label="Next"><i class="fas fa-chevron-right"></i></button>
                    </div>
                </div>
            </section>

            <!-- ═══ FAQ ═══ -->
            <section class="sec" id="faq">
                <div class="wrap">
                    <div class="sec-head">
                        <span class="eyebrow">Got questions? We have answers</span>
                        <h2 class="sec-title">Frequently Asked <span class="serif accent">Questions</span></h2>
                    </div>
                    <div class="faq" data-ui>
                        <div class="faq-item">
                            <button type="button" class="faq-q">How long is the WOW Package valid?<i class="fas fa-chevron-down"></i></button>
                            <div class="faq-a"><p>The WOW Card benefits are valid for the period mentioned at the time of purchase. Check the terms for specific validity on each benefit.</p></div>
                        </div>
                        <div class="faq-item">
                            <button type="button" class="faq-q">Can I gift the WOW Package?<i class="fas fa-chevron-down"></i></button>
                            <div class="faq-a"><p>Absolutely! The WOW Card makes a perfect gift. Buy it for family, friends, or colleagues.</p></div>
                        </div>
                        <div class="faq-item">
                            <button type="button" class="faq-q">Is the hotel stay really included?<i class="fas fa-chevron-down"></i></button>
                            <div class="faq-a"><p>Yes. 2 Nights / 3 Days at a 3-star or above hotel. Choose from 8+ destinations across India. Plus ₹1,000 food coupon.</p></div>
                        </div>
                        <div class="faq-item">
                            <button type="button" class="faq-q">How does the Scratch Card work?<i class="fas fa-chevron-down"></i></button>
                            <div class="faq-a"><p>After buying the WOW Card, you can purchase a Scratch Card for ₹699 and reveal a product worth ₹2,500 to ₹15,000.</p></div>
                        </div>
                        <div class="faq-item">
                            <button type="button" class="faq-q">What is the Free Product?<i class="fas fa-chevron-down"></i></button>
                            <div class="faq-a"><p>Choose one product from our curated selection at no additional cost. Options include a Sling Bag, Thermo Steel Bottle, or Collapsible Basket.</p></div>
                        </div>
                        <div class="faq-item">
                            <button type="button" class="faq-q">Any hidden charges?<i class="fas fa-chevron-down"></i></button>
                            <div class="faq-a"><p>None. One-time payment. All 5 benefits unlocked immediately.</p></div>
                        </div>
                    </div>
                </div>
            </section>

        </main>

        <!-- ═══ FOOTER ═══ -->
        <footer class="footer">
            <div class="wrap">
                <div class="f-grid">
                    <div class="f-brand">
                        <h2>India Saves With <span>Stanvee</span></h2>
                        <p>Join the movement.</p>
                        <div class="social">
                            <a href="https://www.instagram.com/stanvee.india" aria-label="Instagram"><i class="fab fa-instagram"></i></a>
                            <a href="https://www.facebook.com/stanvee.india" aria-label="Facebook"><i class="fab fa-facebook-f"></i></a>
                            <a href="https://wa.me/919270511515" aria-label="WhatsApp"><i class="fab fa-whatsapp"></i></a>
                            <a href="https://www.linkedin.com/company/stanvee" aria-label="LinkedIn"><i class="fab fa-linkedin-in"></i></a>
                        </div>
                    </div>

                    <div class="f-cols">
                        <div class="f-col">
                            <h4>BENEFITS</h4>
                            <ul>
                                <li><a href="HolidayRedirect.aspx">Holiday Stay</a></li>
                                <li><a href="MovieBookingRedirect.aspx">Movie Tickets</a></li>
                                <li><a href="insurance.aspx">Accidental Insurance</a></li>
                                <li><a href="https://shop.stanvee.com/">Shopping Coupon</a></li>
                                <li><a href="ScratchCard.aspx">Scratch Card</a></li>
                                <li><a href="freeProduct.aspx">Free Product</a></li>
                            </ul>
                        </div>
                        <div class="f-col">
                            <h4>COMPANY</h4>
                            <ul>
                                <li><a href="https://stanvee.com/about-us.asp">About Stanvee</a></li>
                                <li><a href="#how" data-ui>How It Works</a></li>
                                <li><a href="#faq" data-ui>FAQs</a></li>
                                <li><a href="https://stanvee.com/contact-us.asp">Contact Us</a></li>
                            </ul>
                        </div>
                        <div class="f-col">
                            <h4>LEGAL</h4>
                            <ul>
                                <li><a href="https://stanvee.com/terms-conditions.asp">Terms &amp; Conditions</a></li>
                                <li><a href="https://stanvee.com/privacy-policy.asp">Privacy Policy</a></li>
                                <li><a href="https://gv.stanveeservices.com/index.php?mod=refund_cancel">Refund Policy</a></li>
                            </ul>
                        </div>
                    </div>

                    <div class="f-col">
                        <h4>CONTACT US</h4>
                        <div class="f-contact">
                            <div><i class="fas fa-map-marker-alt"></i><span>403, Metropolis, Balewadi High Street, Baner, Pune 411045.</span></div>
                            <div><i class="fas fa-envelope"></i><a href="mailto:info@stanvee.com" data-ui>info@stanvee.com</a></div>
                            <div><i class="fas fa-phone"></i><span><a href="tel:+919270521515" data-ui>+91 92705 21515</a> / <a href="tel:+919270511515" data-ui>+91 92705 11515</a></span></div>
                        </div>
                    </div>
                </div>

                <div class="f-bottom">
                    <span>© 2026 Stanvee Services India Limited. All rights reserved.</span>
                    <div class="links">
                        <a href="https://stanvee.com/terms-conditions.asp">Terms</a>
                        <a href="https://stanvee.com/privacy-policy.asp">Privacy</a>
                        <a href="https://gv.stanveeservices.com/index.php?mod=refund_cancel">Refund</a>
                    </div>
                </div>
            </div>
        </footer>

        <!-- ═══ STICKY CTA (mobile only) ═══ -->
        <div class="sticky-cta" id="stickyCta">
            <div>
                <div class="s-lbl">WOW Package · 5 Benefits</div>
                <div class="s-val">₹999</div>
            </div>
            <a href="https://wow.stanvee.com/MM_Voucher.aspx" class="btn btn-primary">Get It Now <i class="fas fa-arrow-right"></i></a>
        </div>

        <script>
            /* ── Drawer ── */
            function apxOpenDrawer() {
                document.getElementById('apxDrawer').classList.add('open');
                document.getElementById('apxOverlay').classList.add('show');
                document.body.style.overflow = 'hidden';
            }
            function apxCloseDrawer() {
                document.getElementById('apxDrawer').classList.remove('open');
                document.getElementById('apxOverlay').classList.remove('show');
                document.body.style.overflow = '';
            }

            (function () {
                /* ── Hero slider (auto + swipe + dots) ── */
                var track = document.getElementById('heroTrack');
                var dots = document.querySelectorAll('#heroDots button');
                var total = dots.length, cur = 0, timer;

                function go(i) {
                    cur = (i + total) % total;
                    track.style.transform = 'translateX(' + (-cur * 100) + '%)';
                    dots.forEach(function (d, k) { d.classList.toggle('active', k === cur); });
                }
                function auto() { clearInterval(timer); timer = setInterval(function () { go(cur + 1); }, 5000); }
                dots.forEach(function (d, k) { d.addEventListener('click', function () { go(k); auto(); }); });

                var sx = 0, sy = 0;
                track.addEventListener('touchstart', function (e) { sx = e.touches[0].clientX; sy = e.touches[0].clientY; }, { passive: true });
                track.addEventListener('touchend', function (e) {
                    var dx = e.changedTouches[0].clientX - sx, dy = e.changedTouches[0].clientY - sy;
                    if (Math.abs(dx) > 40 && Math.abs(dx) > Math.abs(dy)) { go(cur + (dx < 0 ? 1 : -1)); auto(); }
                }, { passive: true });
                auto();

                /* ── Benefit tabs ── */
                var tabs = document.querySelectorAll('#benefitTabs .tab');
                function showPanel(key, scroll) {
                    tabs.forEach(function (t) { t.classList.toggle('active', t.getAttribute('data-target') === key); });
                    document.querySelectorAll('.panel').forEach(function (p) { p.classList.toggle('active', p.id === 'panel-' + key); });
                    var active = document.querySelector('#benefitTabs .tab.active');
                    if (active && active.scrollIntoView && window.innerWidth < 900) {
                        active.scrollIntoView({ behavior: 'smooth', block: 'nearest', inline: 'center' });
                    }
                    if (scroll) document.getElementById('benefits').scrollIntoView({ behavior: 'smooth' });
                }
                tabs.forEach(function (t) { t.addEventListener('click', function () { showPanel(t.getAttribute('data-target'), false); }); });
                document.querySelectorAll('.quick-item').forEach(function (q) {
                    q.addEventListener('click', function () { showPanel(q.getAttribute('data-go'), true); });
                });

                /* ── Carousels: prev/next scroll by one viewport of cards ── */
                document.querySelectorAll('.carousel').forEach(function (c) {
                    var tr = c.querySelector('.c-track');
                    var step = function () { return Math.max(tr.clientWidth * 0.8, 200); };
                    c.querySelector('.c-btn.prev').addEventListener('click', function () { tr.scrollBy({ left: -step(), behavior: 'smooth' }); });
                    c.querySelector('.c-btn.next').addEventListener('click', function () { tr.scrollBy({ left: step(), behavior: 'smooth' }); });
                    if (!tr.children.length) {
                        tr.innerHTML = '<div class="empty-note">Products will be available soon.</div>';
                        c.querySelectorAll('.c-btn').forEach(function (b) { b.style.display = 'none'; });
                    }
                });

                /* ── FAQ accordion ── */
                document.querySelectorAll('.faq-item').forEach(function (item) {
                    var q = item.querySelector('.faq-q'), a = item.querySelector('.faq-a');
                    q.addEventListener('click', function () {
                        var open = item.classList.contains('open');
                        document.querySelectorAll('.faq-item.open').forEach(function (o) {
                            o.classList.remove('open'); o.querySelector('.faq-a').style.maxHeight = null;
                        });
                        if (!open) { item.classList.add('open'); a.style.maxHeight = a.scrollHeight + 'px'; }
                    });
                });

                /* ── Sticky CTA after hero ── */
                var cta = document.getElementById('stickyCta'), hero = document.getElementById('hero');
                function onScroll() { cta.classList.toggle('show', hero.getBoundingClientRect().bottom < 0); }
                window.addEventListener('scroll', onScroll, { passive: true });
                onScroll();
            })();

            /* ── App landing: every link / button / card opens AppLogin (or WebApp if already logged in).
                  Page-only controls (slider, tabs, carousels, FAQ, drawer, in-page anchors) are marked data-ui. ── */
            (function () {
                var target = '<%= (Session["Status"] != null && Session["Status"].ToString() == "OK") ? "WebApp.aspx" : "AppLogin.aspx" %>';
                var clickable = 'a, button, input[type=submit], input[type=button], [onclick], .movie, .pcard, .dest';

                window.addEventListener('click', function (e) {
                    var el = e.target.closest(clickable);
                    if (!el || el.closest('[data-ui]')) return;
                    e.preventDefault();
                    e.stopImmediatePropagation();
                    window.location.href = target;
                }, true);
            })();
        </script>
    </form>
</body>
</html>
