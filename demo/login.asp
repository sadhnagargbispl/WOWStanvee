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
      <h2 class="wow-heading text-center"> Login Account  </h2>
      </div>



     


<div class="login">
  <div class="text">

    <div class="form-box">

      <h2 class="form-title">User Login</h2>

      <!-- Email / Username -->
      <div class="form-group">
        <label>Email / Mobile <span class="required">*</span></label>
        <input type="text" placeholder="Enter email or mobile">
      </div>

      <!-- Password -->
      <div class="form-group">
        <label>Password <span class="required">*</span></label>
        <input type="password" placeholder="Enter password">
      </div>

      <!-- Remember + Forgot -->
      <div class="form-extra">
        <label><input type="checkbox"> Remember Me</label>
        <a href="#">Forgot Password?</a>
      </div>

      <!-- Login Button -->
      <button class="login-btn">Login</button>

      <!-- Register -->
      <p class="register-text">
        Don't have an account? <a href="#">Register</a>
      </p>

    </div>

  </div>
</div>

     

    </div>
  </section>

 

 


  <!-- #include file="inc_footer.asp" -->




  <script src="script.js?v=2"></script>

</body>

</html>