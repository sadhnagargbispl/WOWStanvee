document.addEventListener('DOMContentLoaded', () => {
  // Mobile Side Drawer Logic
  const menuToggle = document.getElementById('menuToggle');
  const sideDrawer = document.getElementById('sideDrawer');
  const closeDrawer = document.getElementById('closeDrawer');
  const drawerOverlay = document.getElementById('drawerOverlay');

  if (menuToggle && sideDrawer && closeDrawer && drawerOverlay) {
    console.log('Side drawer elements found');
    const toggleDrawer = () => {
      console.log('Toggling side drawer');
      sideDrawer.classList.toggle('open');
      drawerOverlay.classList.toggle('active');
      document.body.style.overflow = sideDrawer.classList.contains('open') ? 'hidden' : 'auto';
    };

    menuToggle.addEventListener('click', (e) => {
      console.log('Hamburger clicked');
      toggleDrawer();
    });

    closeDrawer.addEventListener('click', toggleDrawer);
    drawerOverlay.addEventListener('click', toggleDrawer);

    // Close drawer when a link is clicked
    const drawerLinks = sideDrawer.querySelectorAll('a');
    drawerLinks.forEach(link => {
      link.addEventListener('click', () => {
        if (!link.classList.contains('drawer-dropbtn')) {
          toggleDrawer();
        }
      });
    });

    // Drawer Dropdowns
    const drawerDropdowns = document.querySelectorAll('.drawer-dropdown');
    drawerDropdowns.forEach(dropdown => {
      const btn = dropdown.querySelector('.drawer-dropbtn');
      if (btn) {
        btn.addEventListener('click', (e) => {
          e.preventDefault();
          e.stopPropagation();
          dropdown.classList.toggle('active');
        });
      }
    });
  }

  // Section 2 Accordion / Slider Logic
  const items = document.querySelectorAll('.wow-item');
  const panels = document.querySelectorAll('.content-panel');

  items.forEach(item => {
    const togglePanel = () => {
      const accordion = document.querySelector('.wow-accordion');
      if (accordion && window.getComputedStyle(accordion).display === 'none') {
        return;
      }

      const targetId = item.dataset.target;
      items.forEach(i => i.classList.remove('active'));
      panels.forEach(p => p.classList.remove('active'));
      item.classList.add('active');
      const targetPanel = document.getElementById(targetId);
      if (targetPanel) {
        targetPanel.classList.add('active');
      }
    };

    item.addEventListener('mouseenter', togglePanel);
    item.addEventListener('click', togglePanel);
  });

  // FAQ accordion behavior
  const openIconSVG = `<svg width="16" height="8" viewBox="0 0 16 8" fill="none" xmlns="http://www.w3.org/2000/svg"><path d="M0 0L8 8L16 0L0 0Z" fill="black"/></svg>`;
  const closeIconSVG = `<svg width="14" height="14" viewBox="0 0 14 14" fill="none" xmlns="http://www.w3.org/2000/svg"><path d="M13.5379 11.2409L9.0675 6.76975L13.5363 2.29938L11.2393 0L6.76812 4.47038L2.29775 0L0 2.29938L4.46875 6.76975L0 11.2401L2.29938 13.5379L6.76812 9.0675L11.2369 13.5379L13.5379 11.2409Z" fill="black"/></svg>`;

  const questions = document.querySelectorAll('.faq-question');
  questions.forEach(question => {
    question.addEventListener('click', () => {
      const currentItem = question.parentElement;
      const iconContainer = question.querySelector('.faq-question-icon');
      
      document.querySelectorAll('.faq-item').forEach(item => {
        if (item !== currentItem && item.classList.contains('active')) {
          item.classList.remove('active');
          const otherIcon = item.querySelector('.faq-question-icon');
          if (otherIcon) otherIcon.innerHTML = openIconSVG;
        }
      });

      const isActive = currentItem.classList.toggle('active');
      if (iconContainer) {
        iconContainer.innerHTML = isActive ? closeIconSVG : openIconSVG;
      }
    });
  });

  // Reviews Carousel
  const reviewsWrapper = document.querySelector('.reviews-wrapper');
  const prevBtn = document.getElementById('prevBtn');
  const nextBtn = document.getElementById('nextBtn');

  let currentSlide = 0;

  function getSlidesToShow() {
    return window.innerWidth <= 768 ? 1 : 3;
  }

  function updateCarousel() {
    const cards = document.querySelectorAll('.review-card');
    if (!reviewsWrapper || cards.length === 0) return;

    const totalSlides = cards.length;
    const slidesToShow = getSlidesToShow();
    const maxSlide = Math.max(0, totalSlides - slidesToShow);
    
    currentSlide = Math.min(currentSlide, maxSlide);
    currentSlide = Math.max(currentSlide, 0);
    
    const cardWidth = cards[0].offsetWidth;
    const gap = parseInt(window.getComputedStyle(reviewsWrapper).gap) || 0;
    
    const translateX = -currentSlide * (cardWidth + gap);
    reviewsWrapper.style.transform = `translateX(${translateX}px)`;
    
    if (prevBtn) {
      prevBtn.disabled = currentSlide === 0;
      prevBtn.style.opacity = currentSlide === 0 ? '0.3' : '1';
      prevBtn.style.cursor = currentSlide === 0 ? 'default' : 'pointer';
    }
    if (nextBtn) {
      nextBtn.disabled = currentSlide === maxSlide;
      nextBtn.style.opacity = currentSlide === maxSlide ? '0.3' : '1';
      nextBtn.style.cursor = currentSlide === maxSlide ? 'default' : 'pointer';
    }
  }

  if (prevBtn) {
    prevBtn.addEventListener('click', () => {
      if (currentSlide > 0) {
        currentSlide--;
        updateCarousel();
      }
    });
  }

  if (nextBtn) {
    nextBtn.addEventListener('click', () => {
      const totalSlides = document.querySelectorAll('.review-card').length;
      const slidesToShow = getSlidesToShow();
      const maxSlide = Math.max(0, totalSlides - slidesToShow);
      if (currentSlide < maxSlide) {
        currentSlide++;
        updateCarousel();
      }
    });
  }

  window.addEventListener('resize', updateCarousel);

  if (reviewsWrapper && prevBtn && nextBtn) {
    updateCarousel();
  }

  // Stats Carousel for Mobile (Section 4)
  const statsCards = document.querySelectorAll('.stat-card');
  if (statsCards.length === 4) {
    let statsSlide = 0;
    let statsInterval;

    function updateStatsCarousel() {
      if (window.innerWidth > 768) {
        statsCards.forEach(card => {
          card.classList.remove('fade-hidden', 'fade-visible');
        });
        return;
      }

      statsCards.forEach((card, index) => {
        const isFirstPair = index < 2;
        const shouldShow = (statsSlide === 0 && isFirstPair) || (statsSlide === 1 && !isFirstPair);

        if (shouldShow) {
          card.classList.add('fade-visible');
          card.classList.remove('fade-hidden');
        } else {
          card.classList.add('fade-hidden');
          card.classList.remove('fade-visible');
        }
      });
      statsSlide = (statsSlide + 1) % 2;
    }

    function startStatsCarousel() {
      if (window.innerWidth <= 768 && !statsInterval) {
        updateStatsCarousel();
        statsInterval = setInterval(updateStatsCarousel, 4000);
      } else if (window.innerWidth > 768 && statsInterval) {
        clearInterval(statsInterval);
        statsInterval = null;
        statsCards.forEach(card => card.classList.remove('fade-hidden', 'fade-visible'));
      }
    }

    startStatsCarousel();
    window.addEventListener('resize', startStatsCarousel);
  }

  // Movie Carousel 3D Logic
  const movieTrack = document.getElementById('movieTrack');
  const movieSlides = Array.from(document.querySelectorAll('.movie-slide'));
  const moviePrevBtn = document.getElementById('moviePrev');
  const movieNextBtn = document.getElementById('movieNext');

  if (movieTrack && movieSlides.length > 0) {
    let movieCurrentIndex = 0;

    function updateMovieCarouselCircular() {
      const total = movieSlides.length;
      movieSlides.forEach((slide, index) => {
        slide.classList.remove('active', 'prev', 'next', 'far-prev', 'far-next', 'hidden');
        
        let diff = index - movieCurrentIndex;
        
        // Normalize diff for circularity
        if (diff > total / 2) diff -= total;
        if (diff < -total / 2) diff += total;

        if (diff === 0) {
          slide.classList.add('active');
        } else if (diff === -1) {
          slide.classList.add('prev');
        } else if (diff === 1) {
          slide.classList.add('next');
        } else if (diff === -2) {
          slide.classList.add('far-prev');
        } else if (diff === 2) {
          slide.classList.add('far-next');
        } else {
          slide.classList.add('hidden');
        }
      });
    }

    // Navigation click
    movieSlides.forEach(slide => {
      slide.addEventListener('click', () => {
        window.open('http://movie.stanvee.com/', '_blank');
      });
    });

    if (moviePrevBtn) {
      moviePrevBtn.addEventListener('click', () => {
        movieCurrentIndex = (movieCurrentIndex - 1 + movieSlides.length) % movieSlides.length;
        updateMovieCarouselCircular();
      });
    }

    if (movieNextBtn) {
      movieNextBtn.addEventListener('click', () => {
        movieCurrentIndex = (movieCurrentIndex + 1) % movieSlides.length;
        updateMovieCarouselCircular();
      });
    }

    // Auto-play
    let movieAutoPlay = setInterval(() => {
      movieCurrentIndex = (movieCurrentIndex + 1) % movieSlides.length;
      updateMovieCarouselCircular();
    }, 5000);

    movieTrack.addEventListener('mouseenter', () => clearInterval(movieAutoPlay));
    movieTrack.addEventListener('mouseleave', () => {
      clearInterval(movieAutoPlay);
      movieAutoPlay = setInterval(() => {
        movieCurrentIndex = (movieCurrentIndex + 1) % movieSlides.length;
        updateMovieCarouselCircular();
      }, 5000);
    });

    // Initialize
    updateMovieCarouselCircular();
  }

  // Shop Carousel 3D Logic
  const shopTrack = document.getElementById('shopTrack');
  const shopSlides = Array.from(document.querySelectorAll('.shop-slide'));
  const shopPrevBtn = document.getElementById('shopPrev');
  const shopNextBtn = document.getElementById('shopNext');

  if (shopTrack && shopSlides.length > 0) {
    let shopCurrentIndex = 0;

    function updateShopCarousel() {
      const total = shopSlides.length;
      shopSlides.forEach((slide, index) => {
        slide.classList.remove('active', 'prev', 'next', 'far-prev', 'far-next', 'hidden');
        
        let diff = index - shopCurrentIndex;
        
        // Normalize diff for circularity
        if (diff > total / 2) diff -= total;
        if (diff < -total / 2) diff += total;

        if (diff === 0) {
          slide.classList.add('active');
        } else if (diff === -1) {
          slide.classList.add('prev');
        } else if (diff === 1) {
          slide.classList.add('next');
        } else if (diff === -2) {
          slide.classList.add('far-prev');
        } else if (diff === 2) {
          slide.classList.add('far-next');
        } else {
          slide.classList.add('hidden');
        }
      });
    }

    // Navigation click
    shopSlides.forEach(slide => {
      slide.addEventListener('click', () => {
        window.open('http://shop.stanvee.com/', '_blank');
      });
    });

    if (shopPrevBtn) {
      shopPrevBtn.addEventListener('click', () => {
        shopCurrentIndex = (shopCurrentIndex - 1 + shopSlides.length) % shopSlides.length;
        updateShopCarousel();
      });
    }

    if (shopNextBtn) {
      shopNextBtn.addEventListener('click', () => {
        shopCurrentIndex = (shopCurrentIndex + 1) % shopSlides.length;
        updateShopCarousel();
      });
    }

    // Auto-play
    let shopAutoPlay = setInterval(() => {
      shopCurrentIndex = (shopCurrentIndex + 1) % shopSlides.length;
      updateShopCarousel();
    }, 4500);

    shopTrack.addEventListener('mouseenter', () => clearInterval(shopAutoPlay));
    shopTrack.addEventListener('mouseleave', () => {
      clearInterval(shopAutoPlay);
      shopAutoPlay = setInterval(() => {
        shopCurrentIndex = (shopCurrentIndex + 1) % shopSlides.length;
        updateShopCarousel();
      }, 4500);
    });

    // Initialize
    updateShopCarousel();
  }

  // Hero Slider Logic
  const heroSlider = document.querySelector('.hero-slider');
  const heroTrack = document.querySelector('.hero-track');
  const heroSlides = document.querySelectorAll('.hero-slide');
  const heroDots = document.querySelectorAll('.hero-pagination .dot');
  
  if (heroTrack && heroSlides.length > 0) {
    let heroCurrentIndex = 0;
    let heroAutoPlay;

    const updateHeroPosition = () => {
      heroTrack.style.transform = `translateX(-${heroCurrentIndex * 100}%)`;
      
      // Update Active Classes
      heroSlides.forEach((slide, i) => {
        slide.classList.toggle('active', i === heroCurrentIndex);
      });
      
      heroDots.forEach((dot, i) => {
        dot.classList.toggle('active', i === heroCurrentIndex);
      });
    };

    const nextHeroSlide = () => {
      heroCurrentIndex = (heroCurrentIndex + 1) % heroSlides.length;
      updateHeroPosition();
    };

    const goToHeroSlide = (n) => {
      heroCurrentIndex = n;
      updateHeroPosition();
      resetHeroInterval();
    };

    const startHeroAutoPlay = () => {
      heroAutoPlay = setInterval(nextHeroSlide, 5000); // 5 Seconds as requested
    };

    const resetHeroInterval = () => {
      clearInterval(heroAutoPlay);
      startHeroAutoPlay();
    };

    // Expose to window for HTML onclick
    window.currentSlide = (n) => {
      goToHeroSlide(n);
    };

    // Auto-play management
    startHeroAutoPlay();

    heroSlider.addEventListener('mouseenter', () => clearInterval(heroAutoPlay));
    heroSlider.addEventListener('mouseleave', startHeroAutoPlay);
  }

  // Scratch Carousel 3D Logic
  const scratchTrack = document.getElementById('scratchTrack');
  const scratchSlides = Array.from(document.querySelectorAll('.scratch-slide'));
  const scratchPrevBtn = document.getElementById('scratchPrev');
  const scratchNextBtn = document.getElementById('scratchNext');

  if (scratchTrack && scratchSlides.length > 0) {
    let scratchCurrentIndex = 0;

    function updateScratchCarousel() {
      const total = scratchSlides.length;
      scratchSlides.forEach((slide, index) => {
        slide.classList.remove('active', 'prev', 'next', 'far-prev', 'far-next', 'hidden');
        
        let diff = index - scratchCurrentIndex;
        
        // Normalize diff for circularity
        if (diff > total / 2) diff -= total;
        if (diff < -total / 2) diff += total;

        if (diff === 0) {
          slide.classList.add('active');
        } else if (diff === -1) {
          slide.classList.add('prev');
        } else if (diff === 1) {
          slide.classList.add('next');
        } else if (diff === -2) {
          slide.classList.add('far-prev');
        } else if (diff === 2) {
          slide.classList.add('far-next');
        } else {
          slide.classList.add('hidden');
        }
      });
    }

    // Navigation click
    scratchSlides.forEach(slide => {
      slide.addEventListener('click', () => {
        window.open('https://wow.stanvee.com/ScratchCard.aspx', '_blank');
      });
    });

    if (scratchPrevBtn) {
      scratchPrevBtn.addEventListener('click', () => {
        scratchCurrentIndex = (scratchCurrentIndex - 1 + scratchSlides.length) % scratchSlides.length;
        updateScratchCarousel();
      });
    }

    if (scratchNextBtn) {
      scratchNextBtn.addEventListener('click', () => {
        scratchCurrentIndex = (scratchCurrentIndex + 1) % scratchSlides.length;
        updateScratchCarousel();
      });
    }

    // Auto-play
    let scratchAutoPlay = setInterval(() => {
      scratchCurrentIndex = (scratchCurrentIndex + 1) % scratchSlides.length;
      updateScratchCarousel();
    }, 5000);

    scratchTrack.addEventListener('mouseenter', () => clearInterval(scratchAutoPlay));
    scratchTrack.addEventListener('mouseleave', () => {
      clearInterval(scratchAutoPlay);
      scratchAutoPlay = setInterval(() => {
        scratchCurrentIndex = (scratchCurrentIndex + 1) % scratchSlides.length;
        updateScratchCarousel();
      }, 5000);
    });

    // Initialize
    updateScratchCarousel();
  }

  // Section 2 WOW Carousel Pagination Logic
  const wowContent = document.querySelector('.wow-content');
  const wowPagination = document.querySelector('.wow-pagination');
  const wowPanels = document.querySelectorAll('.content-panel');

  if (wowContent && wowPagination && wowPanels.length > 0) {
    // Clear existing pagination (if any)
    wowPagination.innerHTML = '';

    // Generate dots
    wowPanels.forEach((_, i) => {
      const dot = document.createElement('div');
      dot.classList.add('wow-dot');
      if (i === 0) dot.classList.add('active');
      wowPagination.appendChild(dot);
    });

    // Add index indicator
    const indexLabel = document.createElement('div');
    indexLabel.classList.add('wow-index');
    indexLabel.textContent = `1/${wowPanels.length}`;
    wowPagination.appendChild(indexLabel);

    const dots = wowPagination.querySelectorAll('.wow-dot');

    wowContent.addEventListener('scroll', () => {
      if (window.innerWidth > 768) return;

      const scrollLeft = wowContent.scrollLeft;
      const containerWidth = wowContent.offsetWidth;
      const index = Math.round(scrollLeft / (containerWidth + 16)); // Adjusting for gap if any

      dots.forEach((dot, i) => {
        dot.classList.toggle('active', i === index);
      });
      if (index >= 0 && index < wowPanels.length) {
        indexLabel.textContent = `${index + 1}/${wowPanels.length}`;
      }
    });
  }
  // Section 7 - Free Product Carousel Logic
  const freeProductGrid = document.querySelector('.section7 .free-product-grid');
  const freeProductPrev = document.getElementById('freeProductPrev');
  const freeProductNext = document.getElementById('freeProductNext');

  if (freeProductGrid && freeProductPrev && freeProductNext) {
    freeProductPrev.addEventListener('click', () => {
      const cardWidth = freeProductGrid.querySelector('.free-product-card').offsetWidth;
      freeProductGrid.scrollBy({ left: -(cardWidth + 16), behavior: 'smooth' });
    });

    freeProductNext.addEventListener('click', () => {
      const cardWidth = freeProductGrid.querySelector('.free-product-card').offsetWidth;
      freeProductGrid.scrollBy({ left: cardWidth + 16, behavior: 'smooth' });
    });

    const freeCards = freeProductGrid.querySelectorAll('.free-product-card');
    freeCards.forEach(card => {
      card.addEventListener('click', () => {
        window.open('https://wow.stanvee.com/freeProduct.aspx', '_blank');
      });
    });
  }

  // Section 8 - Destinations Card Navigation
  const destinationsCards = document.querySelectorAll('.destinations-card');
  destinationsCards.forEach(card => {
    card.addEventListener('click', () => {
      window.open('https://holiday.stanvee.com/destination.aspx', '_blank');
    });
  });
});

