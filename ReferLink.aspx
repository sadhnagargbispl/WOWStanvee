<%@ Page Title="" Language="C#" MasterPageFile="~/MainMaster.master" AutoEventWireup="true" CodeFile="ReferLink.aspx.cs" Inherits="ReferLink" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link href="assets/css/custom_styelsheet.css?v=1" rel="stylesheet" />
    <style>
       
        .refer-page-wrap {
            background: #f5f0e8;
            min-height: 500px;
            padding: 2.5rem 1rem;
        }

        .refer-page-header {
            text-align: center;
            margin-bottom: 2rem;
        }

            .refer-page-header .sub-label {
                font-size: 12px;
                font-weight: 600;
                letter-spacing: 2px;
                text-transform: uppercase;
                color: #b45309;
                margin: 0 0 6px;
            }

            .refer-page-header h2 {
                font-size: 26px;
                font-weight: 600;
                color: #1c1917;
                margin: 0 0 8px;
            }

      .refer-grid {
    display: grid;
    grid-template-columns: repeat(2, 1fr); /* ← changed from repeat(3, 1fr) */
    gap: 16px;
    max-width: 900px;
    margin: 0 auto;
}
        .refer-card-new {
            background: #fff;
            border-radius: 16px;
            border: 0.5px solid #e7e5e4;
            overflow: hidden;
            display: flex;
            flex-direction: column;
        }

        .rcard-header {
            padding: 18px 16px;
            text-align: center;
        }

            .rcard-header.silver {
                background: #f97316;
            }

            .rcard-header.gold {
                background: #d97706;
            }

            .rcard-header.gold-now {
                background: #e1c0cb;
            }

            .rcard-header.plat {
                background: #0369a1;
            }

        .rcard-icon {
            width: 48px;
            height: 48px;
            border-radius: 50%;
            background: rgba(255,255,255,0.18);
            display: flex;
            align-items: center;
            justify-content: center;
            margin: 0 auto 10px;
            font-size: 20px;
            color: #fff;
        }

        .rcard-title {
            font-size: 16px;
            font-weight: 600;
            color: #fff;
            margin: 0 0 3px;
        }

        .rcard-sub {
            font-size: 12px;
            color: rgba(255,255,255,0.72);
            margin: 0;
        }

        .rcard-body {
            padding: 16px;
            flex: 1;
            display: flex;
            flex-direction: column;
        }

            .rcard-body label {
                font-size: 11px;
                color: #78716c;
                display: block;
                margin-bottom: 10px;
                text-align: center;
            }

        .rcard-copy-btn {
            width: 100%;
            display: inline-flex;
            align-items: center;
            justify-content: center;
            gap: 6px;
            border: none;
            border-radius: 8px;
            padding: 10px;
            font-size: 13px;
            font-weight: 600;
            color: #fff;
            cursor: pointer;
            transition: opacity 0.15s, background 0.2s;
        }

            .rcard-copy-btn:hover {
                opacity: 0.88;
            }

            .rcard-copy-btn.silver {
                background: #f97316;
            }

            .rcard-copy-btn.gold {
                background: #d97706;
            }

            .rcard-copy-btn.gold-now {
                background: #e1c0cb;
            }

            .rcard-copy-btn.plat {
                background: #0369a1;
            }

        .copy-ok-msg {
            font-size: 11px;
            color: #16a34a;
            font-weight: 500;
            text-align: center;
            margin: 6px 0 0;
            min-height: 14px;
        }

        /* Share buttons */
        .rcard-share {
            display: flex;
            gap: 8px;
            margin-top: 12px;
        }

        .share-btn {
            flex: 1;
            display: inline-flex;
            align-items: center;
            justify-content: center;
            gap: 6px;
            height: 40px;
            border-radius: 8px;
            border: 1px solid #e7e5e4;
            background: #fff;
            cursor: pointer;
            color: #57534e;
            font-size: 12px;
            font-weight: 600;
            text-decoration: none;
            transition: background 0.15s, border-color 0.15s;
        }

            .share-btn:hover {
                background: #fafaf9;
                border-color: #d6d3d1;
            }

            .share-btn svg {
                width: 18px;
                height: 18px;
                flex-shrink: 0;
            }

        .share-wa svg {
            fill: #25D366;
        }

        .share-fb svg {
            fill: #1877F2;
        }

        .share-li svg {
            fill: #0A66C2;
        }

        .refer-footer-note {
            text-align: center;
            padding: 16px 0 4px;
            font-size: 12px;
            color: #a8a29e;
        }

        /* Mobile: stack vertically */
        @media (max-width: 640px) {
            .refer-grid {
                grid-template-columns: 1fr;
            }
        }
    </style>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">

    <div class="refer-page-wrap">

        <div class="refer-page-header">
            <p class="sub-label">Stanvee Rewards</p>
            <h2>Refer Links</h2>
        </div>

        <div class="refer-grid">
            <div class="refer-card-new">
                <div class="rcard-header gold-now">
                    <div class="rcard-icon">
                        &#127942;
                    </div>
                    <p class="rcard-title">
                        FREE REGISTRATION
                    </p>
                    <p class="rcard-sub">
                        Refer and Earn &#128640;
                    </p>
                </div>
                <div class="rcard-body">
                    <label>
                        Share your refer link</label>
                    <input type="hidden" id="Hidden1" value="<%# ReferralLink4 %>" />
                    <button class="rcard-copy-btn gold-now" id="Button1" onclick="doCopy(document.getElementById('Hidden1').value,'Button1','P1','#d97706'); return false;">
                        &#128203; Copy Link
                    </button>
                    <p class="copy-ok-msg" id="P1">
                    </p>
                    <div class="rcard-share">
                        <a href="#" class="share-btn share-wa" title="Share on WhatsApp" onclick="shareWA(document.getElementById('Hidden1').value); return false;">
                            <svg viewbox="0 0 24 24">
                                <path d="M.057 24l1.687-6.163a11.867 11.867 0 01-1.587-5.946C.16 5.335 5.495 0 12.05 0a11.821 11.821 0 018.413 3.488 11.824 11.824 0 013.48 8.414c-.003 6.557-5.338 11.892-11.893 11.892a11.9 11.9 0 01-5.688-1.448L.057 24zm6.597-3.807c1.676.995 3.276 1.591 5.392 1.592 5.448 0 9.886-4.434 9.889-9.885.002-5.462-4.415-9.89-9.881-9.892-5.452 0-9.887 4.434-9.889 9.884a9.86 9.86 0 001.521 5.26l-.999 3.648 3.967-1.607zm11.387-5.464c-.074-.124-.272-.198-.57-.347-.297-.149-1.758-.868-2.031-.967-.272-.099-.47-.149-.669.149-.198.297-.768.967-.941 1.165-.173.198-.347.223-.644.074-.297-.149-1.255-.462-2.39-1.475-.883-.788-1.48-1.761-1.653-2.059-.173-.297-.018-.458.13-.606.134-.133.297-.347.446-.521.151-.172.2-.296.3-.495.099-.198.05-.372-.025-.521-.075-.148-.669-1.611-.916-2.206-.242-.579-.487-.501-.669-.51l-.57-.01c-.198 0-.52.074-.792.372s-1.04 1.016-1.04 2.479 1.065 2.876 1.213 3.074c.149.198 2.095 3.2 5.076 4.487.709.306 1.263.489 1.694.626.712.226 1.36.194 1.872.118.571-.085 1.758-.719 2.006-1.413.248-.695.248-1.29.173-1.414z" />
                            </svg>
                        </a>
                        <a href="#" class="share-btn share-fb" title="Share on Facebook" onclick="shareFB(document.getElementById('Hidden1').value); return false;">
                            <svg viewbox="0 0 24 24">
                                <path d="M24 12.073c0-6.627-5.373-12-12-12s-12 5.373-12 12c0 5.99 4.388 10.954 10.125 11.854v-8.385H7.078v-3.47h3.047V9.43c0-3.007 1.792-4.669 4.533-4.669 1.312 0 2.686.235 2.686.235v2.953H15.83c-1.491 0-1.956.925-1.956 1.874v2.25h3.328l-.532 3.47h-2.796v8.385C19.612 23.027 24 18.062 24 12.073z" />
                            </svg>
                        </a><a href="#" class="share-btn share-li" title="Share on LinkedIn" onclick="shareLI(document.getElementById('Hidden1').value); return false;">
                            <svg viewbox="0 0 24 24">
                                <path d="M20.447 20.452h-3.554v-5.569c0-1.328-.027-3.037-1.852-3.037-1.853 0-2.136 1.445-2.136 2.939v5.667H9.351V9h3.414v1.561h.046c.477-.9 1.637-1.85 3.37-1.85 3.601 0 4.267 2.37 4.267 5.455v6.286zM5.337 7.433a2.062 2.062 0 01-2.063-2.065 2.064 2.064 0 112.063 2.065zm1.782 13.019H3.555V9h3.564v11.452zM22.225 0H1.771C.792 0 0 .774 0 1.729v20.542C0 23.227.792 24 1.771 24h20.451C23.2 24 24 23.227 24 22.271V1.729C24 .774 23.2 0 22.222 0h.003z" />
                            </svg>
                        </a>
                    </div>
                </div>
            </div>
            <%-- Gold --%>
            <div class="refer-card-new">
                <div class="rcard-header gold">
                    <div class="rcard-icon">
                        <i class="fa fa-trophy"></i>
                    </div>
                    <p class="rcard-title">WOW Movie @999</p>
                    <p class="rcard-sub">Refer and Earn 🚀</p>
                </div>
                <div class="rcard-body">
                    <label>Share your refer link</label>
                    <input type="hidden" id="hl2" value="<%# ReferralLink2 %>" />
                    <button class="rcard-copy-btn gold" id="cb2"
                        onclick="doCopy(document.getElementById('hl2').value,'cb2','cm2','#d97706'); return false;">
                        <i class="fa fa-copy"></i>Copy Link
                   
                    </button>
                    <p class="copy-ok-msg" id="cm2"></p>
                    <div class="rcard-share">
                        <a href="#" class="share-btn share-wa" title="Share on WhatsApp"
                            onclick="shareWA(document.getElementById('hl2').value); return false;">
                            <svg viewBox="0 0 24 24">
                                <path d="M.057 24l1.687-6.163a11.867 11.867 0 01-1.587-5.946C.16 5.335 5.495 0 12.05 0a11.821 11.821 0 018.413 3.488 11.824 11.824 0 013.48 8.414c-.003 6.557-5.338 11.892-11.893 11.892a11.9 11.9 0 01-5.688-1.448L.057 24zm6.597-3.807c1.676.995 3.276 1.591 5.392 1.592 5.448 0 9.886-4.434 9.889-9.885.002-5.462-4.415-9.89-9.881-9.892-5.452 0-9.887 4.434-9.889 9.884a9.86 9.86 0 001.521 5.26l-.999 3.648 3.967-1.607zm11.387-5.464c-.074-.124-.272-.198-.57-.347-.297-.149-1.758-.868-2.031-.967-.272-.099-.47-.149-.669.149-.198.297-.768.967-.941 1.165-.173.198-.347.223-.644.074-.297-.149-1.255-.462-2.39-1.475-.883-.788-1.48-1.761-1.653-2.059-.173-.297-.018-.458.13-.606.134-.133.297-.347.446-.521.151-.172.2-.296.3-.495.099-.198.05-.372-.025-.521-.075-.148-.669-1.611-.916-2.206-.242-.579-.487-.501-.669-.51l-.57-.01c-.198 0-.52.074-.792.372s-1.04 1.016-1.04 2.479 1.065 2.876 1.213 3.074c.149.198 2.095 3.2 5.076 4.487.709.306 1.263.489 1.694.626.712.226 1.36.194 1.872.118.571-.085 1.758-.719 2.006-1.413.248-.695.248-1.29.173-1.414z" />
                            </svg>
                        </a>
                        <a href="#" class="share-btn share-fb" title="Share on Facebook"
                            onclick="shareFB(document.getElementById('hl2').value); return false;">
                            <svg viewBox="0 0 24 24">
                                <path d="M24 12.073c0-6.627-5.373-12-12-12s-12 5.373-12 12c0 5.99 4.388 10.954 10.125 11.854v-8.385H7.078v-3.47h3.047V9.43c0-3.007 1.792-4.669 4.533-4.669 1.312 0 2.686.235 2.686.235v2.953H15.83c-1.491 0-1.956.925-1.956 1.874v2.25h3.328l-.532 3.47h-2.796v8.385C19.612 23.027 24 18.062 24 12.073z" />
                            </svg>
                        </a>
                        <a href="#" class="share-btn share-li" title="Share on LinkedIn"
                            onclick="shareLI(document.getElementById('hl2').value); return false;">
                            <svg viewBox="0 0 24 24">
                                <path d="M20.447 20.452h-3.554v-5.569c0-1.328-.027-3.037-1.852-3.037-1.853 0-2.136 1.445-2.136 2.939v5.667H9.351V9h3.414v1.561h.046c.477-.9 1.637-1.85 3.37-1.85 3.601 0 4.267 2.37 4.267 5.455v6.286zM5.337 7.433a2.062 2.062 0 01-2.063-2.065 2.064 2.064 0 112.063 2.065zm1.782 13.019H3.555V9h3.564v11.452zM22.225 0H1.771C.792 0 0 .774 0 1.729v20.542C0 23.227.792 24 1.771 24h20.451C23.2 24 24 23.227 24 22.271V1.729C24 .774 23.2 0 22.222 0h.003z" />
                            </svg>
                        </a>
                    </div>
                </div>
            </div>

            <%-- Silver --%>
            <div class="refer-card-new">
                <div class="rcard-header silver">
                    <div class="rcard-icon">
                        <i class="fa fa-medal"></i>
                    </div>
                    <p class="rcard-title">FULL TANK CARD @4999</p>
                    <p class="rcard-sub">Refer and Earn 🚀</p>
                </div>
                <div class="rcard-body">
                    <label>Share your refer link</label>
                    <input type="hidden" id="hl1" value="<%# ReferralLink1 %>" />
                    <button class="rcard-copy-btn silver" id="cb1"
                        onclick="doCopy(document.getElementById('hl1').value,'cb1','cm1','#f97316'); return false;">
                        <i class="fa fa-copy"></i>Copy Link
                   
                    </button>
                    <p class="copy-ok-msg" id="cm1"></p>
                    <div class="rcard-share">
                        <a href="#" class="share-btn share-wa" title="Share on WhatsApp"
                            onclick="shareWA(document.getElementById('hl1').value); return false;">
                            <svg viewBox="0 0 24 24">
                                <path d="M.057 24l1.687-6.163a11.867 11.867 0 01-1.587-5.946C.16 5.335 5.495 0 12.05 0a11.821 11.821 0 018.413 3.488 11.824 11.824 0 013.48 8.414c-.003 6.557-5.338 11.892-11.893 11.892a11.9 11.9 0 01-5.688-1.448L.057 24zm6.597-3.807c1.676.995 3.276 1.591 5.392 1.592 5.448 0 9.886-4.434 9.889-9.885.002-5.462-4.415-9.89-9.881-9.892-5.452 0-9.887 4.434-9.889 9.884a9.86 9.86 0 001.521 5.26l-.999 3.648 3.967-1.607zm11.387-5.464c-.074-.124-.272-.198-.57-.347-.297-.149-1.758-.868-2.031-.967-.272-.099-.47-.149-.669.149-.198.297-.768.967-.941 1.165-.173.198-.347.223-.644.074-.297-.149-1.255-.462-2.39-1.475-.883-.788-1.48-1.761-1.653-2.059-.173-.297-.018-.458.13-.606.134-.133.297-.347.446-.521.151-.172.2-.296.3-.495.099-.198.05-.372-.025-.521-.075-.148-.669-1.611-.916-2.206-.242-.579-.487-.501-.669-.51l-.57-.01c-.198 0-.52.074-.792.372s-1.04 1.016-1.04 2.479 1.065 2.876 1.213 3.074c.149.198 2.095 3.2 5.076 4.487.709.306 1.263.489 1.694.626.712.226 1.36.194 1.872.118.571-.085 1.758-.719 2.006-1.413.248-.695.248-1.29.173-1.414z" />
                            </svg>
                        </a>
                        <a href="#" class="share-btn share-fb" title="Share on Facebook"
                            onclick="shareFB(document.getElementById('hl1').value); return false;">
                            <svg viewBox="0 0 24 24">
                                <path d="M24 12.073c0-6.627-5.373-12-12-12s-12 5.373-12 12c0 5.99 4.388 10.954 10.125 11.854v-8.385H7.078v-3.47h3.047V9.43c0-3.007 1.792-4.669 4.533-4.669 1.312 0 2.686.235 2.686.235v2.953H15.83c-1.491 0-1.956.925-1.956 1.874v2.25h3.328l-.532 3.47h-2.796v8.385C19.612 23.027 24 18.062 24 12.073z" />
                            </svg>
                        </a>
                        <a href="#" class="share-btn share-li" title="Share on LinkedIn"
                            onclick="shareLI(document.getElementById('hl1').value); return false;">
                            <svg viewBox="0 0 24 24">
                                <path d="M20.447 20.452h-3.554v-5.569c0-1.328-.027-3.037-1.852-3.037-1.853 0-2.136 1.445-2.136 2.939v5.667H9.351V9h3.414v1.561h.046c.477-.9 1.637-1.85 3.37-1.85 3.601 0 4.267 2.37 4.267 5.455v6.286zM5.337 7.433a2.062 2.062 0 01-2.063-2.065 2.064 2.064 0 112.063 2.065zm1.782 13.019H3.555V9h3.564v11.452zM22.225 0H1.771C.792 0 0 .774 0 1.729v20.542C0 23.227.792 24 1.771 24h20.451C23.2 24 24 23.227 24 22.271V1.729C24 .774 23.2 0 22.222 0h.003z" />
                            </svg>
                        </a>
                    </div>
                </div>
            </div>

            <%-- Platinum --%>
            <div class="refer-card-new">
                <div class="rcard-header plat">
                    <div class="rcard-icon">
                        <i class="fa fa-gem"></i>
                    </div>
                    <p class="rcard-title">ROYAL PACKAGE @9999</p>
                    <p class="rcard-sub">Refer and Earn 🚀</p>
                </div>
                <div class="rcard-body">
                    <label>Share your refer link</label>
                    <input type="hidden" id="hl3" value="<%# ReferralLink3 %>" />
                    <button class="rcard-copy-btn plat" id="cb3"
                        onclick="doCopy(document.getElementById('hl3').value,'cb3','cm3','#0369a1'); return false;">
                        <i class="fa fa-copy"></i>Copy Link
                   
                    </button>
                    <p class="copy-ok-msg" id="cm3"></p>
                    <div class="rcard-share">
                        <a href="#" class="share-btn share-wa" title="Share on WhatsApp"
                            onclick="shareWA(document.getElementById('hl3').value); return false;">
                            <svg viewBox="0 0 24 24">
                                <path d="M.057 24l1.687-6.163a11.867 11.867 0 01-1.587-5.946C.16 5.335 5.495 0 12.05 0a11.821 11.821 0 018.413 3.488 11.824 11.824 0 013.48 8.414c-.003 6.557-5.338 11.892-11.893 11.892a11.9 11.9 0 01-5.688-1.448L.057 24zm6.597-3.807c1.676.995 3.276 1.591 5.392 1.592 5.448 0 9.886-4.434 9.889-9.885.002-5.462-4.415-9.89-9.881-9.892-5.452 0-9.887 4.434-9.889 9.884a9.86 9.86 0 001.521 5.26l-.999 3.648 3.967-1.607zm11.387-5.464c-.074-.124-.272-.198-.57-.347-.297-.149-1.758-.868-2.031-.967-.272-.099-.47-.149-.669.149-.198.297-.768.967-.941 1.165-.173.198-.347.223-.644.074-.297-.149-1.255-.462-2.39-1.475-.883-.788-1.48-1.761-1.653-2.059-.173-.297-.018-.458.13-.606.134-.133.297-.347.446-.521.151-.172.2-.296.3-.495.099-.198.05-.372-.025-.521-.075-.148-.669-1.611-.916-2.206-.242-.579-.487-.501-.669-.51l-.57-.01c-.198 0-.52.074-.792.372s-1.04 1.016-1.04 2.479 1.065 2.876 1.213 3.074c.149.198 2.095 3.2 5.076 4.487.709.306 1.263.489 1.694.626.712.226 1.36.194 1.872.118.571-.085 1.758-.719 2.006-1.413.248-.695.248-1.29.173-1.414z" />
                            </svg>
                        </a>
                        <a href="#" class="share-btn share-fb" title="Share on Facebook"
                            onclick="shareFB(document.getElementById('hl3').value); return false;">
                            <svg viewBox="0 0 24 24">
                                <path d="M24 12.073c0-6.627-5.373-12-12-12s-12 5.373-12 12c0 5.99 4.388 10.954 10.125 11.854v-8.385H7.078v-3.47h3.047V9.43c0-3.007 1.792-4.669 4.533-4.669 1.312 0 2.686.235 2.686.235v2.953H15.83c-1.491 0-1.956.925-1.956 1.874v2.25h3.328l-.532 3.47h-2.796v8.385C19.612 23.027 24 18.062 24 12.073z" />
                            </svg>
                        </a>
                        <a href="#" class="share-btn share-li" title="Share on LinkedIn"
                            onclick="shareLI(document.getElementById('hl3').value); return false;">
                            <svg viewBox="0 0 24 24">
                                <path d="M20.447 20.452h-3.554v-5.569c0-1.328-.027-3.037-1.852-3.037-1.853 0-2.136 1.445-2.136 2.939v5.667H9.351V9h3.414v1.561h.046c.477-.9 1.637-1.85 3.37-1.85 3.601 0 4.267 2.37 4.267 5.455v6.286zM5.337 7.433a2.062 2.062 0 01-2.063-2.065 2.064 2.064 0 112.063 2.065zm1.782 13.019H3.555V9h3.564v11.452zM22.225 0H1.771C.792 0 0 .774 0 1.729v20.542C0 23.227.792 24 1.771 24h20.451C23.2 24 24 23.227 24 22.271V1.729C24 .774 23.2 0 22.222 0h.003z" />
                            </svg>
                        </a>
                    </div>
                </div>
            </div>

        </div>

        <%-- <div class="refer-footer-note">
            <i class="fa fa-info-circle"></i>
            Copy and share the link — both of you will get cashback.
        </div>--%>
    </div>

    <script>
        function doCopy(text, btnId, msgId, color) {
            var msgEl = document.getElementById(msgId);
            var btn = document.getElementById(btnId);

            if (!text || text.trim() === '') {
                msgEl.style.color = '#dc2626';
                msgEl.textContent = 'Link load nahi hua, refresh karein.';
                return;
            }

            if (navigator.clipboard && navigator.clipboard.writeText) {
                navigator.clipboard.writeText(text).then(function () {
                    showOk(msgEl, btn, color);
                });
            } else {
                var tmp = document.createElement('textarea');
                tmp.value = text;
                document.body.appendChild(tmp);
                tmp.select();
                document.execCommand('copy');
                document.body.removeChild(tmp);
                showOk(msgEl, btn, color);
            }
        }

        function showOk(msgEl, btn, color) {
            msgEl.style.color = '#16a34a';
            msgEl.textContent = 'Link copied!';
            btn.innerHTML = '<i class="fa fa-check"></i> Copied';
            btn.style.background = '#16a34a';
            setTimeout(function () {
                msgEl.textContent = '';
                btn.innerHTML = '<i class="fa fa-copy"></i> Copy Link';
                btn.style.background = color;
            }, 2500);
        }

        // function shareWA(link) {
        //     if (!link || link.trim() === '') return;
        //     var msg = 'Check out this Stanvee offer! ' + link;
        //     window.open('https://wa.me/?text=' + encodeURIComponent(msg), '_blank');
        // }
        function shareWA(link) {
            if (!link || link.trim() === '') return;
            var msg = '*Join Stanvee!* 🚀\n' +
                'Register using my referral link and earn rewards!\n\n' +
                '🔗 ' + link;
            window.open('https://wa.me/?text=' + encodeURIComponent(msg), '_blank');
        }
        function shareFB(link) {
            if (!link || link.trim() === '') return;
            window.open('https://www.facebook.com/sharer/sharer.php?u=' + encodeURIComponent(link), '_blank');
        }

        function shareLI(link) {
            if (!link || link.trim() === '') return;
            window.open('https://www.linkedin.com/sharing/share-offsite/?url=' + encodeURIComponent(link), '_blank');
        }
    </script>

</asp:Content>
