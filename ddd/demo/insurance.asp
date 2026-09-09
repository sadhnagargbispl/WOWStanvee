<!DOCTYPE html>
<html lang="en">
<head>
  <meta charset="UTF-8">
  <meta name="viewport" content="width=device-width, initial-scale=1.0">
  <title>WOW Landing Page</title>
  <link rel="stylesheet" href="style.css?v=5">
  <!-- Font Awesome -->
  <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.5.0/css/all.min.css">
  <!-- Google Fonts -->
  <link rel="preconnect" href="https://fonts.googleapis.com">
  <link rel="preconnect" href="https://fonts.gstatic.com" crossorigin>
  <link href="https://fonts.googleapis.com/css2?family=Instrument+Sans:ital,wght@0,400..700;1,400..700&family=Montserrat:ital,wght@0,100..900;1,100..900&family=Inter:wght@400;600;700&family=Playfair+Display:ital,wght@0,400..900;1,400..900&display=swap" rel="stylesheet">
  <link rel="stylesheet" href="custom_styelsheet.css?v=5">
  </head>


<body>

  <!-- #include file="inc_header.asp" -->



  <!-- Side Drawer for Mobile -->
  <div class="side-drawer" id="sideDrawer">
    <div class="drawer-header">
      <div class="logo">
        <img src="wow_page_images/logo.png" alt="Logo">
      </div>
      <div class="close-drawer" id="closeDrawer">
        <i class="fas fa-times"></i>
      </div>
    </div>
    <div class="drawer-nav">
      <div class="drawer-dropdown">
        <div class="drawer-dropbtn">Packages <i class="fas fa-caret-down"></i></div>
        <div class="drawer-dropdown-content">
          <a href="https://wow.stanvee.com/">WOW Package</a>
          <a href="https://stanvee.com/full_tank_detail.asp">Full Tank Card</a>
          <a href="https://stanvee.com/royal_package.asp">Royal Package</a>
        </div>
      </div>
      <a href="https://holiday.stanvee.com/">Holiday</a>
      <div class="drawer-dropdown">
        <div class="drawer-dropbtn">Shop <i class="fas fa-caret-down"></i></div>
        <div class="drawer-dropdown-content">
          <a href="https://shop.stanvee.com/">Shop</a>
          <a href="https://brandstore.stanveeservices.com/">Brand Store</a>
        </div>
      </div>
      <a href="https://movie.stanvee.com/">Movies</a>
      <a href="https://wow.stanvee.com/insurance.aspx">Insurance</a>
      <a href="https://wow.stanvee.com/ScratchCard.aspx">Scratch Card</a>
      <a href="https://wow.stanvee.com/freeProduct.aspx">Free Product</a>
    </div>
    <button class="cta-btn drawer-cta">
      <a href="https://wow.stanvee.com/MM_Voucher.aspx">Get WOW Card Now!</a>
    </button>
    <div class="drawer-footer" style="display: none;">
      <div class="drawer-socials">
        <i class="fas fa-user"></i>
      </div>
    </div>
  </div>
  <div class="drawer-overlay" id="drawerOverlay"></div>

 

 
 

  <section class="section8 destinations" id="holiday-section">
    <div class="destinations-container ">


      <div class="wow-header text-center">      
      <h2 class="wow-heading text-center"> Insurance Details  </h2>
      </div>



     


      
      <div class="insurance">



  <div class="text">

    <div class="form-box">

      

      <!-- Full Name -->
      <div class="form-group">
        <label>Full Name <span class="required">*</span></label>
        <input type="text" value="sadhna" disabled>
      </div>

      <!-- Date of Birth -->
      <div class="form-group">
        <label>Date of Birth <span class="required">*</span></label>
        <input type="date" value="1999-01-04" disabled>
      </div>

      <!-- Mobile -->
      <div class="form-group">
        <label>Mobile Number <span class="required">*</span></label>
        <input type="text" value="5677899000" disabled>
      </div>

      <!-- Email -->
      <div class="form-group">
        <label>Email ID <span class="required">*</span></label>
        <input type="email" value="sadhnagarg.bispl@gmail.com" disabled>
      </div>

      <!-- Nominee -->
      <div class="form-group">
        <label>Nominee Name <span class="required">*</span></label>
        <input type="text" value="test" disabled>
      </div>

      <!-- Nominee DOB -->
      <div class="form-group">
        <label>Nominee DOB <span class="required">*</span></label>
        <input type="date" value="2026-02-01" disabled>
      </div>

      <!-- Relationship -->
      <div class="form-group">
        <label>Relationship <span class="required">*</span></label>
        <input type="text" value="FATHER" disabled>
      </div>

    </div>

    <div class="shop-cta">
        <a href="https://shop.stanvee.com/" target="_blank" class="white-pill-btn">
           Submit Now! <span><i class="fas fa-arrow-right"></i></span>
        </a>
      </div>



  </div>
</div>

     

    </div>
  </section>

 

 


  <!-- #include file="inc_footer.asp" -->




  <script src="script.js?v=2"></script>

</body>

</html>