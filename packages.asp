<!DOCTYPE html>
<html lang="en">

<head>
  <meta charset="UTF-8">
  <meta name="viewport" content="width=device-width, initial-scale=1.0">
  <title>Free Products</title>
  <script src="https://cdn.tailwindcss.com"></script>
</head>

<body>

  <!-- Navbar -->

<!--#include file="inc_header.asp"-->



  <script>
    const toggleButton = document.getElementById("toggle-button");
    const slideMenu = document.getElementById("slide-menu");

    toggleButton.addEventListener("click", () => {
      slideMenu.classList.toggle("hidden");
    });

    document.addEventListener("click", (event) => {
      // Closes the menu if the click is outside the toggle button AND outside the menu itself.
      if (!toggleButton.contains(event.target) && !slideMenu.contains(event.target)) {
        slideMenu.classList.add("hidden");
      }
    });
  </script>
  <!-- navbar -->



  <div class="bg-gradient-to-b from-blue-50 to-blue-200 min-h-screen">
    <div class="p-10">
      <h1 class="text-5xl font-bold text-center mb-10 text-blue-700">
        Packages Offers 
      </h1>

      <!-- Products Grid -->
      <div id="productGrid" class="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-3 gap-8">
      </div>
    </div>
    <script>
      const freeProducts = [
        {
          name: "2 Nights 3 Days  Holidays ",
          desc: "2 Adults, 2 Children above 3 Star Hotels <br> Free Rs. 1000/- Food Coupon",
          img: "assets/c-1.jpg"
        },        
        {
            name: "Free 2 Movie Tickets",
            desc: "Watch your favorite movies for free!  Valid for all movies, <br> all timings, and all formats (2D/3D/IMAX). Enjoy cinematic experience!",
          img: "assets/c-2.jpg"
        },
      ];

      const grid = document.getElementById("productGrid");

      freeProducts.forEach(product => {
        const card = `
        <div class="bg-white p-5 rounded-xl shadow-lg hover:shadow-2xl transition">
          
          <div class="w-full h-60 bg-gray-100 rounded-lg flex items-center justify-center overflow-hidden">
            <img src="${product.img}" 
                 alt="${product.name}"
                 class="w-full h-full object-contain p-2" />
          </div>

          <h2 class="text-md font-semibold text-gray-800 mt-4">${product.name}</h2>

          <p class="text-gray-600 mt-2">${product.desc}</p>

          <button class="mt-4 w-full bg-blue-600 hover:bg-blue-700 text-white py-2 rounded-lg font-medium">
            Claim Now
          </button>
        </div>
      `;

        grid.innerHTML += card;
      });
    </script>

  </div>


</body>

</html>