<%@ Page Title="" Language="C#" MasterPageFile="~/MainMaster.master" AutoEventWireup="true" CodeFile="ScratchCard.aspx.cs" Inherits="ScratchCard" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link href="assets/css/custom_styelsheet.css?v=1" rel="stylesheet" />
    <style>
        /* ✅ FORCE canvas behind button when hidden */
        #scratchCanvas {
            position: absolute;
            top: 0; left: 0;
            width: 100%; height: 100%;
            cursor: pointer;
            z-index: 10;
        }
        #scratchCanvas.hidden-canvas {
            display: none !important;
            pointer-events: none !important;
            z-index: -1 !important;
        }
        #claimBtn {
            position: relative;
            z-index: 20; /* ✅ always above canvas */
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <div class="scratchcard">
        <div class="hero-section">
            <div class="container">

                <div class="left-content">
                    <h1 class="main-title">
                        <span class="icon">👉</span>
                        WOW DISCOUNT
                    </h1>
                    <div class="highlight-box">
                        <p>You will WIN a Premium Product</p>
                        <h2>₹15,000</h2>
                    </div>
                    <div class="details">
                        <p class="tag">Products Worth ₹2,500 – ₹15,000</p>
                        <p class="price">Scratch Card Price: <span>₹699 Only</span></p>
                        <p class="note">*Available after purchasing a <b>WOW Package</b></p>
                        <p class="info">Scratch the card to reveal your surprise 🎉</p>
                    </div>
                </div>

                <div class="right-card">
                    <div class="card" style="position:relative; overflow:hidden;">

                        <p id="claimStatusLabel" style="color:#f15343; font-weight:bold; display:none;"></p>
                        <img id="productImage" src="" alt="Product Prize Image" />
                        <h6 id="productName"></h6>
                        <p id="productPrice" class="product-price"></p>
                        <button id="claimBtn" disabled class="claim-btn">Claim Now</button>
                        <canvas id="scratchCanvas"></canvas>

                    </div>
                </div>

                <asp:HiddenField ID="hfFormNo" runat="server" />
            </div>
        </div>
    </div>


    <section class="section8 destinations" id="holiday-section">
        <div class="destinations-container">
            <div class="wow-header text-center">
                <h2 class="wow-heading text-center">Win One of These Exciting Products!</h2>
            </div>
            <div class="freeproducts">
                <div id="productGrid" class="product-grid">
                    <asp:Repeater ID="RptProducts" runat="server">
                        <ItemTemplate>
                            <div class="product-card">
                                <div class="product-img">
                                    <img src="<%# Eval("ImageUrl") %>" alt="" />
                                </div>
                                <h2><%# Eval("ProductName") %></h2>
                                <p><%# Eval("price") %></p>
                            </div>
                        </ItemTemplate>
                    </asp:Repeater>
                </div>
            </div>
        </div>
    </section>


    <script>
        document.addEventListener("DOMContentLoaded", function () {

            /* ---------- 1. NAVBAR ---------- */
            const toggleBtn = document.getElementById('toggle-button');
            const slideMenu = document.getElementById('slide-menu');
            if (toggleBtn) {
                toggleBtn.addEventListener('click', () => slideMenu.classList.toggle('hidden'));
                slideMenu.querySelectorAll('a').forEach(link => {
                    link.addEventListener('click', () => slideMenu.classList.add('hidden'));
                });
            }

            /* ---------- 2. ELEMENTS ---------- */
            const formNo      = document.getElementById("<%=hfFormNo.ClientID%>").value;
            const canvas = document.getElementById("scratchCanvas");
            const ctx = canvas.getContext("2d");
            const claimBtn = document.getElementById("claimBtn");
            const statusLabel = document.getElementById("claimStatusLabel");

            /* ---------- 3. HIDE CANVAS HELPER ---------- */
            function hideCanvas() {
                canvas.classList.add("hidden-canvas");
                canvas.style.display = "none";
                canvas.style.pointerEvents = "none";
                canvas.style.zIndex = "-1";
            }

            /* ---------- 4. PRODUCTS ---------- */
            let products = [];
            if (Array.isArray(window.jsonFromBackend)) {
                products = window.jsonFromBackend;
            } else if (typeof window.jsonFromBackend === "string") {
                try { products = JSON.parse(window.jsonFromBackend); } catch (e) { }
            }
            console.log("Products ready:", products.length);

            /* ---------- 5. CHECK SCRATCH HISTORY ---------- */
            fetch("GetScratchByFormNo.aspx?formno=" + encodeURIComponent(formNo))
                .then(r => r.text())
                .then(r => {
                    if (r && r.trim() !== "") {
                        try {
                            const product = JSON.parse(r);
                            showProduct(product);
                            hideCanvas(); // ✅
                            showStatus("Already Scratched");
                            checkClaimStatus(product);
                        } catch (e) {
                            initScratch();
                        }
                    } else {
                        initScratch();
                    }
                })
                .catch(() => initScratch());

            /* ---------- 6. STATUS LABEL ---------- */
            function showStatus(msg) {
                statusLabel.innerText = msg;
                statusLabel.style.display = msg ? "block" : "none";
            }

            /* ---------- 7. CHECK CLAIM STATUS ---------- */
            function checkClaimStatus(product) {
                fetch("CheckClaimStatus.aspx?formno=" + encodeURIComponent(formNo))
                    .then(r => r.text())
                    .then(status => {
                        console.log("Claim status:", status.trim());
                        if (status.trim().toUpperCase() === "CLAIMED") {
                            claimBtn.innerText = "Already Claimed";
                            claimBtn.disabled = true;
                            showStatus("");
                        } else {
                            enableClaim(product);
                        }
                    })
                    .catch(() => enableClaim(product));
            }

            /* ---------- 8. ENABLE CLAIM ---------- */
            function enableClaim(product) {
                console.log("enableClaim called:", product);

                hideCanvas(); // ✅ make sure canvas never blocks button

                claimBtn.innerText = "Claim Now";
                claimBtn.disabled = false;
                claimBtn.className = "claim-btn";
                claimBtn.style.cssText = "";
                claimBtn.onclick = null;

                claimBtn.onclick = function (e) {
                    e.preventDefault();
                    e.stopPropagation();
                    console.log("Claim clicked! product:", product);

                    if (!product || !product.id) {
                        alert("Invalid product. Please refresh.");
                        return;
                    }

                    try {
                        localStorage.setItem("selectedProduct", JSON.stringify(product));
                    } catch (ex) { }

                    var encodedId = btoa(unescape(encodeURIComponent(String(product.id))));
                    console.log("Navigating to: claim.aspx?productid=" + encodedId);
                    window.location.href = "claim.aspx?productid=" + encodedId;
                };
            }

            /* ---------- 9. SHOW PRODUCT ---------- */
            function showProduct(product) {
                document.getElementById("productImage").src = product.image || "";
                document.getElementById("productName").innerText = product.name || "";
                document.getElementById("productPrice").innerText =
                    "₹" + Number(product.price || 0).toLocaleString("en-IN");
            }

            /* ---------- 10. SELECT PRODUCT BY TIER ---------- */
            function selectProduct() {
                const total = products.length;
                if (total === 0) return null;
                const rand = Math.random() * 100;
                if (rand >= 7 && total > 13) { const p = products.slice(13); return p[Math.floor(Math.random() * p.length)]; }
                if (rand >= 3 && total > 9) { const p = products.slice(9, 13); return p[Math.floor(Math.random() * p.length)]; }
                if (rand >= 1 && total > 4) { const p = products.slice(4, 9); return p[Math.floor(Math.random() * p.length)]; }
                const p = products.slice(0, Math.min(4, total));
                return p[Math.floor(Math.random() * p.length)];
            }

            /* ---------- 11. SCRATCH SYSTEM ---------- */
            function initScratch() {
                const product = selectProduct();
                if (!product) { showStatus("No products available."); return; }

                showProduct(product);

                canvas.width = canvas.offsetWidth;
                canvas.height = canvas.offsetHeight;

                ctx.fillStyle = "#5c7b94";
                ctx.fillRect(0, 0, canvas.width, canvas.height);
                ctx.fillStyle = "white";
                ctx.font = "bold 24px Arial";
                ctx.textAlign = "center";
                ctx.fillText("SCRATCH TO WIN", canvas.width / 2, canvas.height / 2);

                let scratching = false;
                let saved = false;

                function saveToDB() {
                    if (saved) return;
                    saved = true;
                    fetch("SaveScratch.aspx", {
                        method: "POST",
                        headers: { "Content-Type": "application/json" },
                        body: JSON.stringify({
                            ProductId: product.id,
                            ProductName: product.name,
                            ProductPrice: product.price,
                            ProductImage: product.image,
                            FormNo: formNo
                        })
                    }).catch(err => console.error("SaveScratch error:", err));
                }

                function getXY(e) {
                    const rect = canvas.getBoundingClientRect();
                    const clientX = e.touches ? e.touches[0].clientX : e.clientX;
                    const clientY = e.touches ? e.touches[0].clientY : e.clientY;
                    return { x: clientX - rect.left, y: clientY - rect.top };
                }

                function scratch(e) {
                    if (!scratching) return;
                    if (e.cancelable) e.preventDefault();
                    const { x, y } = getXY(e);
                    ctx.globalCompositeOperation = "destination-out";
                    ctx.beginPath();
                    ctx.arc(x, y, 30, 0, Math.PI * 2);
                    ctx.fill();
                    checkReveal();
                }

                function checkReveal() {
                    const data = ctx.getImageData(0, 0, canvas.width, canvas.height).data;
                    let cleared = 0;
                    for (let i = 3; i < data.length; i += 4)
                        if (data[i] === 0) cleared++;

                    if ((cleared / (canvas.width * canvas.height)) * 100 > 40) {
                        hideCanvas(); // ✅ properly hide canvas
                        checkClaimStatus(product);
                    }
                }

                canvas.addEventListener("mousedown", e => { scratching = true; saveToDB(); scratch(e); });
                canvas.addEventListener("mousemove", scratch);
                document.addEventListener("mouseup", () => scratching = false);
                canvas.addEventListener("touchstart", e => { scratching = true; saveToDB(); scratch(e); }, { passive: false });
                canvas.addEventListener("touchmove", scratch, { passive: false });
                document.addEventListener("touchend", () => scratching = false);
            }

        });
    </script>

</asp:Content>