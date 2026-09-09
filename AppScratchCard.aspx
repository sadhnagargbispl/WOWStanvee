<%@ Page Title="" Language="C#" MasterPageFile="~/AppMaster.master" AutoEventWireup="true" CodeFile="AppScratchCard.aspx.cs" Inherits="AppScratchCard" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>

<%-- HEADER: back + title --%>
<asp:Content ID="Content3" ContentPlaceHolderID="HeaderContent" runat="Server">
    <a href="wow-package.aspx" class="back-btn" aria-label="Back"><i class="fa fa-arrow-left"></i></a>
    <div class="h-title">Scratch <span>Card</span></div>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <div class="scroll-body">

        <!-- ═══ HERO ═══ -->
        <div class="wow-hero" style="--hero-a: #b45309; --hero-b: #fbbf24; --hero-c: #92400e;">
            <div class="wow-hero-icon">
                <i class="fa fa-ticket"></i>
            </div>
            <div class="wow-hero-text">
                <div class="wow-hero-label">Stanvee Services</div>
                <div class="wow-hero-title">Scratch &amp; Win</div>
                <div class="wow-hero-sub">Apna surprise product reveal karein</div>
            </div>
        </div>

        <!-- ═══ SCRATCH SECTION ═══ -->
        <div class="scratch-wrap">

            <!-- Info panel -->
            <div class="scratch-info">
                <div class="scratch-title">
                    <span>👉</span>WOW DISCOUNT
                </div>

                <div class="highlight-box">
                    <p>You will WIN a Premium Product</p>
                    <h2>₹15,000</h2>
                </div>

                <div class="scratch-meta">
                    <p class="tag">Products Worth ₹2,500 – ₹15,000</p>
                    <p class="price">Scratch Card Price: <span>₹699 Only</span></p>
                    <p class="note">*Available after purchasing a <b>WOW Package</b></p>
                </div>
            </div>

            <!-- Card -->
            <div class="scratch-card-box">

                <p id="claimStatusLabel" class="scratch-status"></p>

                <!-- Canvas sirf is area ko cover karta hai, button ko nahi -->
                <div class="scratch-area" id="scratchArea">
                    <img id="productImage" class="scratch-prize-img" src="" alt="Product Prize" />
                    <h6 id="productName" class="scratch-prize-name"></h6>
                    <p id="productPrice" class="scratch-prize-price"></p>
                    <canvas id="scratchCanvas"></canvas>
                </div>

                <button type="button" id="claimBtn" class="claim-btn" disabled>Claim Now</button>

                <div class="scratch-hint">Scratch the card to reveal your surprise 🎉</div>
            </div>

            <asp:HiddenField ID="hfFormNo" runat="server" />
        </div>

        <!-- ═══ PRIZE LIST ═══ -->
        <div class="sec-header">
            <div class="sec-header-title">
                <div class="hdr-dot"></div>
                Win One Of These
            </div>
            <span class="sec-count">Prize Pool</span>
        </div>

        <div class="prod-wrap">
            <div id="productGrid" class="prod-grid">
                <asp:Repeater ID="RptProducts" runat="server">
                    <ItemTemplate>
                        <div class="prod-card">
                            <div class="prod-img">
                                <img src='<%# Eval("ImageUrl") %>'
                                    alt='<%# Eval("ProductName") %>'
                                    loading="lazy"
                                    onerror="this.style.visibility='hidden'" />
                            </div>
                            <div class="prod-body">
                                <div class="prod-name"><%# Eval("ProductName") %></div>
                                <div class="prod-desc"><%# Eval("price") %></div>
                            </div>
                        </div>
                    </ItemTemplate>
                </asp:Repeater>
            </div>
        </div>

        <div style="height: 12px"></div>

    </div>

    <script>
        document.addEventListener("DOMContentLoaded", function () {

            /* ---------- 1. ELEMENTS ---------- */
            const formNo = document.getElementById("<%=hfFormNo.ClientID%>").value;
            const canvas = document.getElementById("scratchCanvas");
            const ctx = canvas.getContext("2d");
            const claimBtn = document.getElementById("claimBtn");
            const statusLabel = document.getElementById("claimStatusLabel");

            /* ---------- 2. HIDE CANVAS ---------- */
            function hideCanvas() {
                canvas.classList.add("hidden-canvas");
            }

            /* ---------- 3. PRODUCTS ---------- */
            let products = [];
            if (Array.isArray(window.jsonFromBackend)) {
                products = window.jsonFromBackend;
            } else if (typeof window.jsonFromBackend === "string") {
                try { products = JSON.parse(window.jsonFromBackend); } catch (e) { }
            }

            /* ---------- 4. CHECK SCRATCH HISTORY ---------- */
            fetch("GetScratchByFormNo.aspx?formno=" + encodeURIComponent(formNo))
                .then(r => r.text())
                .then(r => {
                    if (r && r.trim() !== "") {
                        try {
                            const product = JSON.parse(r);
                            showProduct(product);
                            hideCanvas();
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

            /* ---------- 5. STATUS LABEL ---------- */
            function showStatus(msg) {
                statusLabel.innerText = msg || "";
                statusLabel.style.display = msg ? "block" : "none";
            }

            /* ---------- 6. CLAIM STATUS ---------- */
            function checkClaimStatus(product) {
                fetch("CheckClaimStatus.aspx?formno=" + encodeURIComponent(formNo))
                    .then(r => r.text())
                    .then(status => {
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

            /* ---------- 7. ENABLE CLAIM ---------- */
            function enableClaim(product) {
                hideCanvas();
                claimBtn.innerText = "Claim Now";
                claimBtn.disabled = false;

                claimBtn.onclick = function (e) {
                    e.preventDefault();
                    if (!product || !product.id) {
                        showToast("Invalid product. Please refresh.");
                        return;
                    }
                    try {
                        localStorage.setItem("selectedProduct", JSON.stringify(product));
                    } catch (ex) { }

                    var encodedId = btoa(unescape(encodeURIComponent(String(product.id))));
                    window.location.href = "claim.aspx?productid=" + encodedId;
                };
            }

            /* ---------- 8. SHOW PRODUCT ---------- */
            function showProduct(product) {
                document.getElementById("productImage").src = product.image || "";
                document.getElementById("productName").innerText = product.name || "";
                document.getElementById("productPrice").innerText =
                    "₹" + Number(product.price || 0).toLocaleString("en-IN");
            }

            /* ---------- 9. SELECT PRODUCT BY TIER ---------- */
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

            /* ---------- 10. SCRATCH SYSTEM ---------- */
            function initScratch() {
                const product = selectProduct();
                if (!product) { showStatus("No products available."); return; }

                showProduct(product);

                /* Retina-safe sizing */
                function sizeCanvas() {
                    const dpr = window.devicePixelRatio || 1;
                    const w = canvas.offsetWidth;
                    const h = canvas.offsetHeight;
                    canvas.width = Math.round(w * dpr);
                    canvas.height = Math.round(h * dpr);
                    ctx.setTransform(dpr, 0, 0, dpr, 0, 0);

                    const g = ctx.createLinearGradient(0, 0, w, h);
                    g.addColorStop(0, "#5c7b94");
                    g.addColorStop(1, "#3d5568");
                    ctx.fillStyle = g;
                    ctx.fillRect(0, 0, w, h);

                    ctx.fillStyle = "rgba(255,255,255,.92)";
                    ctx.font = "bold 20px Nunito, Arial, sans-serif";
                    ctx.textAlign = "center";
                    ctx.textBaseline = "middle";
                    ctx.fillText("SCRATCH TO WIN", w / 2, h / 2);
                }
                sizeCanvas();

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
                    const pt = getXY(e);
                    ctx.globalCompositeOperation = "destination-out";
                    ctx.beginPath();
                    ctx.arc(pt.x, pt.y, 26, 0, Math.PI * 2);
                    ctx.fill();
                    checkReveal();
                }

                function checkReveal() {
                    const data = ctx.getImageData(0, 0, canvas.width, canvas.height).data;
                    let cleared = 0;
                    for (let i = 3; i < data.length; i += 4)
                        if (data[i] === 0) cleared++;

                    if ((cleared / (canvas.width * canvas.height)) * 100 > 40) {
                        hideCanvas();
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
