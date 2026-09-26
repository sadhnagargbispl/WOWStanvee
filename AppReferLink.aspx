<%@ Page Title="" Language="C#" MasterPageFile="~/AppMaster.master" AutoEventWireup="true" CodeFile="AppReferLink.aspx.cs" Inherits="AppReferLink" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>

<%-- HEADER: back + title --%>
<asp:Content ID="Content3" ContentPlaceHolderID="HeaderContent" runat="Server">
    <a href="WebApp.aspx" class="back-btn" aria-label="Back"><i class="fa fa-arrow-left"></i></a>
    <div class="h-title">Refer <span>Links</span></div>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <div class="scroll-body">

        <!-- ═══ HERO ═══ -->
        <div class="wow-hero" style="--hero-a: #0f766e; --hero-b: #5eead4; --hero-c: #134e4a;">
            <div class="wow-hero-icon">
                <i class="fa fa-share-nodes"></i>
            </div>
            <div class="wow-hero-text">
                <div class="wow-hero-label">Stanvee Rewards</div>
                <div class="wow-hero-title">Refer &amp; Earn</div>
                <div class="wow-hero-sub">Apna link share karein aur rewards kamayein</div>
            </div>
        </div>

        <!-- ═══ SECTION HEADER ═══ -->
        <div class="sec-header">
            <div class="sec-header-title">
                <div class="hdr-dot"></div>
                Your Refer Links
            </div>
            <span class="sec-count">4 Links</span>
        </div>

        <!-- ═══ REFER CARDS ═══ -->
        <div class="refer-wrap">

            <div class="refer-card">
                <div class="refer-head" style="--rc: #db2777;">
                    <div class="refer-icon"><i class="fa fa-user-plus"></i></div>
                    <div>
                        <div class="refer-title">FREE REGISTRATION</div>
                        <div class="refer-sub">Refer and Earn 🚀</div>
                    </div>
                </div>
                <div class="refer-body">
                    <input type="hidden" id="hl4" value="<%# ReferralLink4 %>" />
                    <button type="button" class="refer-copy" id="cb4" style="--rc: #db2777;"
                        onclick="doCopy('hl4','cb4')">
                        <i class="fa fa-copy"></i> Copy Link
                    </button>
                    <div class="refer-share">
                        <button type="button" class="share-btn share-wa" onclick="shareWA('hl4')" aria-label="WhatsApp"><i class="fa-brands fa-whatsapp"></i></button>
                        <button type="button" class="share-btn share-fb" onclick="shareFB('hl4')" aria-label="Facebook"><i class="fa-brands fa-facebook-f"></i></button>
                        <button type="button" class="share-btn share-li" onclick="shareLI('hl4')" aria-label="LinkedIn"><i class="fa-brands fa-linkedin-in"></i></button>
                        <button type="button" class="share-btn share-more" onclick="shareNative('hl4')" aria-label="More"><i class="fa fa-share-alt"></i></button>
                    </div>
                </div>
            </div>

            <div class="refer-card">
                <div class="refer-head" style="--rc: #d97706;">
                    <div class="refer-icon"><i class="fa fa-trophy"></i></div>
                    <div>
                        <div class="refer-title">WOW Movie @999</div>
                        <div class="refer-sub">Refer and Earn 🚀</div>
                    </div>
                </div>
                <div class="refer-body">
                    <input type="hidden" id="hl2" value="<%# ReferralLink2 %>" />
                    <button type="button" class="refer-copy" id="cb2" style="--rc: #d97706;"
                        onclick="doCopy('hl2','cb2')">
                        <i class="fa fa-copy"></i> Copy Link
                    </button>
                    <div class="refer-share">
                        <button type="button" class="share-btn share-wa" onclick="shareWA('hl2')" aria-label="WhatsApp"><i class="fa-brands fa-whatsapp"></i></button>
                        <button type="button" class="share-btn share-fb" onclick="shareFB('hl2')" aria-label="Facebook"><i class="fa-brands fa-facebook-f"></i></button>
                        <button type="button" class="share-btn share-li" onclick="shareLI('hl2')" aria-label="LinkedIn"><i class="fa-brands fa-linkedin-in"></i></button>
                        <button type="button" class="share-btn share-more" onclick="shareNative('hl2')" aria-label="More"><i class="fa fa-share-alt"></i></button>
                    </div>
                </div>
            </div>

            <div class="refer-card">
                <div class="refer-head" style="--rc: #f97316;">
                    <div class="refer-icon"><i class="fa fa-medal"></i></div>
                    <div>
                        <div class="refer-title">FULL TANK CARD @4999</div>
                        <div class="refer-sub">Refer and Earn 🚀</div>
                    </div>
                </div>
                <div class="refer-body">
                    <input type="hidden" id="hl1" value="<%# ReferralLink1 %>" />
                    <button type="button" class="refer-copy" id="cb1" style="--rc: #f97316;"
                        onclick="doCopy('hl1','cb1')">
                        <i class="fa fa-copy"></i> Copy Link
                    </button>
                    <div class="refer-share">
                        <button type="button" class="share-btn share-wa" onclick="shareWA('hl1')" aria-label="WhatsApp"><i class="fa-brands fa-whatsapp"></i></button>
                        <button type="button" class="share-btn share-fb" onclick="shareFB('hl1')" aria-label="Facebook"><i class="fa-brands fa-facebook-f"></i></button>
                        <button type="button" class="share-btn share-li" onclick="shareLI('hl1')" aria-label="LinkedIn"><i class="fa-brands fa-linkedin-in"></i></button>
                        <button type="button" class="share-btn share-more" onclick="shareNative('hl1')" aria-label="More"><i class="fa fa-share-alt"></i></button>
                    </div>
                </div>
            </div>

            <div class="refer-card">
                <div class="refer-head" style="--rc: #0369a1;">
                    <div class="refer-icon"><i class="fa fa-gem"></i></div>
                    <div>
                        <div class="refer-title">ROYAL PACKAGE @9999</div>
                        <div class="refer-sub">Refer and Earn 🚀</div>
                    </div>
                </div>
                <div class="refer-body">
                    <input type="hidden" id="hl3" value="<%# ReferralLink3 %>" />
                    <button type="button" class="refer-copy" id="cb3" style="--rc: #0369a1;"
                        onclick="doCopy('hl3','cb3')">
                        <i class="fa fa-copy"></i> Copy Link
                    </button>
                    <div class="refer-share">
                        <button type="button" class="share-btn share-wa" onclick="shareWA('hl3')" aria-label="WhatsApp"><i class="fa-brands fa-whatsapp"></i></button>
                        <button type="button" class="share-btn share-fb" onclick="shareFB('hl3')" aria-label="Facebook"><i class="fa-brands fa-facebook-f"></i></button>
                        <button type="button" class="share-btn share-li" onclick="shareLI('hl3')" aria-label="LinkedIn"><i class="fa-brands fa-linkedin-in"></i></button>
                        <button type="button" class="share-btn share-more" onclick="shareNative('hl3')" aria-label="More"><i class="fa fa-share-alt"></i></button>
                    </div>
                </div>
            </div>

        </div>

        <div style="height: 12px"></div>

    </div>

    <script>
        function getLink(hiddenId) {
            var el = document.getElementById(hiddenId);
            var link = el ? el.value : "";
            if (!link || link.trim() === "") {
                showToast("Link load nahi hua, refresh karein.");
                return null;
            }
            return link;
        }

        function doCopy(hiddenId, btnId) {
            var text = getLink(hiddenId);
            if (!text) return;
            var btn = document.getElementById(btnId);

            function done() {
                showToast("Link copied!");
                btn.classList.add("copied");
                btn.innerHTML = '<i class="fa fa-check"></i> Copied';
                setTimeout(function () {
                    btn.classList.remove("copied");
                    btn.innerHTML = '<i class="fa fa-copy"></i> Copy Link';
                }, 2500);
            }

            if (navigator.clipboard && navigator.clipboard.writeText) {
                navigator.clipboard.writeText(text).then(done, function () { fallbackCopy(text); done(); });
            } else {
                fallbackCopy(text);
                done();
            }
        }

        function fallbackCopy(text) {
            var tmp = document.createElement("textarea");
            tmp.value = text;
            tmp.setAttribute("readonly", "");
            tmp.style.position = "fixed";
            tmp.style.opacity = "0";
            document.body.appendChild(tmp);
            tmp.select();
            document.execCommand("copy");
            document.body.removeChild(tmp);
        }

        function shareWA(hiddenId) {
            var link = getLink(hiddenId);
            if (!link) return;
            var msg = '*Join Stanvee!* 🚀\n' +
                'Register using my referral link and earn rewards!\n\n' +
                '🔗 ' + link;
            window.open('https://wa.me/?text=' + encodeURIComponent(msg), '_blank');
        }

        function shareFB(hiddenId) {
            var link = getLink(hiddenId);
            if (!link) return;
            window.open('https://www.facebook.com/sharer/sharer.php?u=' + encodeURIComponent(link), '_blank');
        }

        function shareLI(hiddenId) {
            var link = getLink(hiddenId);
            if (!link) return;
            window.open('https://www.linkedin.com/sharing/share-offsite/?url=' + encodeURIComponent(link), '_blank');
        }

        /* Mobile par phone ka share sheet kholta hai, warna WhatsApp */
        function shareNative(hiddenId) {
            var link = getLink(hiddenId);
            if (!link) return;
            if (navigator.share) {
                navigator.share({
                    title: "Join Stanvee!",
                    text: "Register using my referral link and earn rewards!",
                    url: link
                }).catch(function () { });
            } else {
                shareWA(hiddenId);
            }
        }
    </script>

</asp:Content>
