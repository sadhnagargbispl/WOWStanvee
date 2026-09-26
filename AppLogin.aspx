<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AppLogin.aspx.cs" Inherits="AppLogin" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="UTF-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0, maximum-scale=1.0, user-scalable=no" />
    <meta name="theme-color" content="#002E6E" />
    <title>Login – ePay India</title>
    <link rel="icon" type="image/x-icon" href="demoepay/images/favicon.png" />
    <link href="https://fonts.googleapis.com/css2?family=Inter:wght@400;500;600;700;800&display=swap" rel="stylesheet" />
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.5.0/css/all.min.css" />
    <style>
        *, *::before, *::after {
            box-sizing: border-box;
            margin: 0;
            padding: 0;
        }

        :root {
            --primary: #00BAF2;
            --primary-d: #00A3D9;
            --navy: #002E6E;
            --navy-2: #0A3D8F;
            --sky: #E8F7FD;
            --bg: #FFFFFF;
            --line: #E3E8EF;
            --text: #1B2A41;
            --muted: #6B7A90;
            --radius: 14px;
        }

        html, body {
            height: 100%;
            font-family: 'Inter', sans-serif;
            background: var(--bg);
            color: var(--text);
        }

        /* ── app shell centred on desktop ── */
        .app-shell {
            position: relative;
            max-width: 480px;
            margin: 0 auto;
            min-height: 100vh;
            background: var(--bg);
            box-shadow: 0 0 60px rgba(0,46,110,.12);
            display: flex;
            flex-direction: column;
        }

        @media (min-width:481px) {
            body {
                background: #EEF3F9;
            }
        }

        /* ── header ── */
        .app-header {
            height: 58px;
            background: #fff;
            display: flex;
            align-items: center;
            padding: 0 16px;
            gap: 12px;
            border-bottom: 1px solid var(--line);
            flex-shrink: 0;
        }

        .h-back {
            background: var(--sky);
            border: none;
            color: var(--navy);
            width: 36px;
            height: 36px;
            border-radius: 50%;
            display: flex;
            align-items: center;
            justify-content: center;
            font-size: .9rem;
            cursor: pointer;
            text-decoration: none;
            flex-shrink: 0;
        }

        .h-logo {
            height: 30px;
            object-fit: contain;
            flex-shrink: 0;
        }

        .h-brand {
            font-size: 1.25rem;
            font-weight: 800;
            color: var(--navy);
            letter-spacing: -.3px;
        }

            .h-brand span {
                color: var(--primary);
            }

        .h-help {
            margin-left: auto;
            color: var(--navy);
            font-size: .8rem;
            font-weight: 600;
            text-decoration: none;
            display: flex;
            align-items: center;
            gap: 6px;
        }

        /* ── hero strip ── */
        .hero-strip {
            background: linear-gradient(180deg, var(--sky) 0%, #F5FBFE 100%);
            padding: 28px 20px 44px;
            text-align: center;
            position: relative;
            overflow: hidden;
        }

            .hero-strip::before {
                content: '';
                position: absolute;
                top: -70px;
                right: -50px;
                width: 200px;
                height: 200px;
                border-radius: 50%;
                background: radial-gradient(circle, rgba(0,186,242,.22) 0%, transparent 70%);
            }

        .logo-circle {
            width: 72px;
            height: 72px;
            border-radius: 50%;
            background: linear-gradient(135deg, var(--primary), var(--navy-2));
            margin: 0 auto 14px;
            display: flex;
            align-items: center;
            justify-content: center;
            padding: 12px;
            position: relative;
            z-index: 1;
            box-shadow: 0 8px 22px rgba(0,186,242,.35);
        }

            .logo-circle img {
                width: 100%;
                height: 100%;
                object-fit: contain;
            }

        .logo-icon-fallback {
            font-size: 1.8rem;
            color: #fff;
        }

        .hero-h {
            color: var(--navy);
            font-size: 1.35rem;
            font-weight: 800;
            position: relative;
            z-index: 1;
        }

        .hero-sub {
            color: var(--muted);
            font-size: .82rem;
            margin-top: 4px;
            position: relative;
            z-index: 1;
        }

        /* ── card ── */
        .login-card {
            background: #fff;
            margin: -24px 16px 0;
            border-radius: 20px;
            padding: 24px 20px 22px;
            border: 1px solid var(--line);
            box-shadow: 0 10px 30px rgba(0,46,110,.08);
            position: relative;
            z-index: 2;
        }

        .card-title {
            font-size: 1.02rem;
            font-weight: 700;
            color: var(--navy);
            margin-bottom: 20px;
            display: flex;
            align-items: center;
            gap: 8px;
        }

            .card-title i {
                color: var(--primary);
            }

        /* ── form fields ── */
        .field {
            margin-bottom: 16px;
        }

            .field label {
                display: block;
                font-size: .78rem;
                font-weight: 600;
                color: var(--muted);
                margin-bottom: 6px;
            }

        .input-wrap {
            position: relative;
        }

            .input-wrap .f-icon {
                position: absolute;
                left: 14px;
                top: 50%;
                transform: translateY(-50%);
                color: var(--primary);
                font-size: .9rem;
                pointer-events: none;
            }

            .input-wrap input {
                width: 100%;
                height: 50px;
                padding: 0 44px 0 42px;
                border: 1.5px solid var(--line);
                border-radius: 12px;
                font-family: 'Inter', sans-serif;
                font-size: .95rem;
                font-weight: 600;
                color: var(--text);
                background: #fff;
                outline: none;
                transition: border-color .2s, box-shadow .2s;
            }

                .input-wrap input:focus {
                    border-color: var(--primary);
                    box-shadow: 0 0 0 4px rgba(0,186,242,.12);
                }

                .input-wrap input::placeholder {
                    color: #A9B4C4;
                    font-weight: 500;
                }

        .toggle-pw {
            position: absolute;
            right: 12px;
            top: 50%;
            transform: translateY(-50%);
            background: none;
            border: none;
            color: var(--muted);
            cursor: pointer;
            font-size: .9rem;
            padding: 4px;
        }

        /* ── remember / forgot row ── */
        .row-extra {
            display: flex;
            align-items: center;
            justify-content: space-between;
            margin-bottom: 22px;
        }

        .remember {
            display: flex;
            align-items: center;
            gap: 7px;
            font-size: .8rem;
            font-weight: 500;
            color: var(--muted);
            cursor: pointer;
        }

            .remember input[type=checkbox] {
                accent-color: var(--primary);
                width: 16px;
                height: 16px;
            }

        .forgot-link {
            font-size: .8rem;
            font-weight: 600;
            color: var(--primary-d);
            text-decoration: none;
        }

            .forgot-link:hover {
                text-decoration: underline;
            }

        /* ── submit btn ── */
        .btn-login {
            width: 100%;
            height: 52px;
            background: var(--primary);
            color: #fff;
            border: none;
            border-radius: 12px;
            font-family: 'Inter', sans-serif;
            font-size: 1rem;
            font-weight: 700;
            letter-spacing: .2px;
            cursor: pointer;
            transition: background .2s, transform .15s;
            box-shadow: 0 6px 16px rgba(0,186,242,.3);
        }

            .btn-login:hover {
                background: var(--primary-d);
            }

            .btn-login:active {
                transform: scale(.99);
            }

        /* ── trust strip ── */
        .trust-row {
            display: flex;
            justify-content: center;
            gap: 18px;
            margin-top: 18px;
            font-size: .72rem;
            color: var(--muted);
            font-weight: 500;
        }

            .trust-row i {
                color: var(--primary);
                margin-right: 4px;
            }

        /* ── error msg ── */
        .err-msg {
            background: #FFF1F2;
            border: 1.5px solid #FCA5A5;
            border-radius: 10px;
            padding: 10px 14px;
            font-size: .8rem;
            font-weight: 600;
            color: #DC2626;
            margin-bottom: 16px;
            display: none;
            align-items: center;
            gap: 8px;
        }

            .err-msg.show {
                display: flex;
            }

        /* ── footer note ── */
        .page-footer {
            margin-top: auto;
            text-align: center;
            padding: 22px 16px 24px;
            font-size: .7rem;
            color: var(--muted);
            line-height: 1.6;
        }

            .page-footer a {
                color: var(--primary-d);
                text-decoration: none;
                font-weight: 600;
            }

        .footer-brand {
            margin-top: 10px;
            font-size: .72rem;
            color: var(--navy);
            font-weight: 600;
        }

            .footer-brand i {
                color: var(--primary);
            }

        /* ── toast ── */
        .toast {
            position: fixed;
            bottom: 24px;
            left: 50%;
            transform: translateX(-50%) translateY(16px);
            background: var(--navy);
            color: #fff;
            padding: 10px 20px;
            border-radius: 50px;
            font-size: .8rem;
            font-weight: 500;
            opacity: 0;
            transition: opacity .28s, transform .28s;
            pointer-events: none;
            z-index: 999;
            white-space: nowrap;
            box-shadow: 0 4px 16px rgba(0,46,110,.3);
        }

            .toast.show {
                opacity: 1;
                transform: translateX(-50%) translateY(0);
            }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <div class="app-shell">

                <!-- HEADER -->
                <header class="app-header">
                    <a href="webapp.aspx" class="h-back" title="Back" style="display: none;">
                        <i class="fa fa-arrow-left"></i>
                    </a>
                    <img src="demoepay/images/logo.png" alt="ePay" class="h-logo"
                        onerror="this.style.display='none'; this.nextElementSibling.style.display='block';" />
                    <div class="h-brand" style="display: none;">e<span>Pay</span></div>
                    <a href="#" class="h-help" onclick="showToast('Support: help@epayindia.com'); return false;">
                        <i class="fa fa-headset"></i>Help
                    </a>
                </header>

                <!-- HERO STRIP -->
                <div class="hero-strip">
                    <div class="logo-circle">
                        <i class="fa fa-wallet logo-icon-fallback"></i>
                    </div>
                    <div class="hero-h">Welcome Back!</div>
                    <div class="hero-sub">Login to your ePay India account</div>
                </div>

                <!-- LOGIN CARD -->
                <div class="login-card">

                    <div class="card-title">
                        <i class="fa fa-circle-user"></i>
                        Member Login
                    </div>

                    <!-- error box -->
                    <div class="err-msg" id="errMsg">
                        <i class="fa fa-exclamation-circle"></i>
                        <span id="errText">
                            <asp:Label ID="lblError" runat="server" CssClass="login-error-box"
                                Style="display: none;" EnableViewState="false"></asp:Label></span>
                    </div>


                    <!-- User ID -->
                    <div class="field">
                        <label for="<%= TxtUserID.ClientID %>">User ID</label>
                        <div class="input-wrap">
                            <i class="fa fa-user f-icon"></i>
                            <asp:TextBox ID="TxtUserID" runat="server"
                                placeholder="Enter your User ID"
                                MaxLength="50">
                            </asp:TextBox>
                        </div>
                    </div>

                    <!-- Password -->
                    <div class="field">
                        <label for="<%= TxtPassword.ClientID %>">Password</label>
                        <div class="input-wrap">
                            <i class="fa fa-lock f-icon"></i>
                            <asp:TextBox ID="TxtPassword" runat="server"
                                TextMode="Password"
                                placeholder="Enter your password"
                                MaxLength="50">
                            </asp:TextBox>
                            <button type="button" class="toggle-pw" id="eyeBtn"
                                onclick="togglePassword()" tabindex="-1"
                                aria-label="Show/hide password">
                                <i id="eyeIcon" class="fa fa-eye"></i>
                            </button>
                        </div>
                    </div>

                    <!-- Remember / Forgot -->
                    <div class="row-extra">
                        <label class="remember">
                            <input type="checkbox" id="rememberMe" />
                            Remember me
                        </label>
                        <a href="#" class="forgot-link" onclick="showToast('Password reset link will be sent to your registered email.'); return false;">Forgot Password?
                        </a>
                    </div>

                    <!-- Submit -->
                    <asp:Button ID="BtnLogin" runat="server"
                        Text="Proceed Securely"
                        CssClass="btn-login"
                        OnClick="BtnLogin_Click" />

                    <div class="trust-row">
                        <span><i class="fa fa-shield-halved"></i>100% Secure</span>
                        <span><i class="fa fa-lock"></i>Encrypted Login</span>
                    </div>
                </div>
                <!-- /login-card -->

                <!-- FOOTER NOTE -->
                <div class="page-footer">
                    By logging in you agree to our
                    <a href="#">Terms of Service</a> &amp;
                    <a href="#">Privacy Policy</a>
                    <div class="footer-brand"><i class="fa fa-heart"></i> Made in India</div>
                    © 2025 ePay India. All rights reserved.
                </div>

            </div>
            <!-- /app-shell -->

            <!-- TOAST -->
            <div class="toast" id="toast"></div>

        </div>
        <script>
            function togglePassword() {
                var pwBox = document.getElementById('<%= TxtPassword.ClientID %>');
                var icon = document.getElementById('eyeIcon');
                if (pwBox.type === 'password') {
                    pwBox.type = 'text';
                    icon.classList.remove('fa-eye');
                    icon.classList.add('fa-eye-slash');
                } else {
                    pwBox.type = 'password';
                    icon.classList.remove('fa-eye-slash');
                    icon.classList.add('fa-eye');
                }
            }

            function showToast(msg) {
                var t = document.getElementById('toast');
                t.textContent = msg;
                t.classList.add('show');
                clearTimeout(t._timer);
                t._timer = setTimeout(function () { t.classList.remove('show'); }, 2600);
            }

            // Show server-side error label if it has text
            window.addEventListener('DOMContentLoaded', function () {
                var lbl = document.getElementById('<%= lblError.ClientID %>');
                if (lbl && lbl.innerText.trim() !== '') {
                    lbl.style.display = 'block';
                    document.getElementById('errMsg').classList.add('show');
                }
            });
        </script>

    </form>
</body>
</html>
