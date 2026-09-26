<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AppLogin.aspx.cs" Inherits="AppLogin" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta charset="UTF-8" />
    <meta name="viewport" content="width=device-width, initial-scale=1.0, maximum-scale=1.0, user-scalable=no" />
    <title>Login – ePay India</title>
    <link rel="icon" type="image/x-icon" href="demoepay/images/favicon.png" />
    <link href="https://fonts.googleapis.com/css2?family=Nunito:wght@400;500;600;700;800&display=swap" rel="stylesheet" />
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.5.0/css/all.min.css" />
    <style>
        *, *::before, *::after {
            box-sizing: border-box;
            margin: 0;
            padding: 0;
        }

        :root {
            --primary: #E84000;
            --primary-d: #c43600;
            --accent: #ff6b35;
            --dark: #1A1A2E;
            --bg: #f4f6fb;
            --text: #2d2d2d;
            --muted: #6b7280;
            --radius: 16px;
        }

        html, body {
            height: 100%;
            font-family: 'Nunito', sans-serif;
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
            box-shadow: 0 0 60px rgba(0,0,0,.18);
            display: flex;
            flex-direction: column;
        }

        @media (min-width:481px) {
            body {
                background: #c8cdd8;
            }
        }

        /* ── header ── */
        .app-header {
            height: 62px;
            background: var(--dark);
            display: flex;
            align-items: center;
            padding: 0 16px;
            gap: 12px;
            box-shadow: 0 2px 16px rgba(0,0,0,.3);
            flex-shrink: 0;
        }

        .h-back {
            background: rgba(255,255,255,.1);
            border: none;
            color: #fff;
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
            transition: background .2s;
        }

            .h-back:hover {
                background: rgba(255,255,255,.22);
            }

        .h-logo {
            height: 34px;
            object-fit: contain;
            flex-shrink: 0;
        }

        .h-title {
            color: #fff;
            font-size: 1.05rem;
            font-weight: 800;
        }

            .h-title span {
                color: var(--accent);
            }

        /* ── hero strip ── */
        .hero-strip {
            background: linear-gradient(135deg, #1A1A2E 0%, #2d1b4e 60%, #1a3050 100%);
            padding: 30px 20px 36px;
            text-align: center;
            position: relative;
            overflow: hidden;
        }

            .hero-strip::before {
                content: '';
                position: absolute;
                top: -60px;
                right: -60px;
                width: 200px;
                height: 200px;
                border-radius: 50%;
                background: radial-gradient(circle, rgba(232,64,0,.3) 0%, transparent 70%);
            }

            .hero-strip::after {
                content: '';
                position: absolute;
                bottom: -40px;
                left: -30px;
                width: 140px;
                height: 140px;
                border-radius: 50%;
                background: radial-gradient(circle, rgba(255,107,53,.2) 0%, transparent 70%);
            }

        .logo-circle {
            width: 76px;
            height: 76px;
            border-radius: 20px;
            background: #fff;
            margin: 0 auto 14px;
            display: flex;
            align-items: center;
            justify-content: center;
            padding: 6px;
            position: relative;
            z-index: 1;
            box-shadow: 0 6px 24px rgba(0,0,0,.25);
        }

            .logo-circle img {
                width: 100%;
                height: 100%;
                object-fit: contain;
            }

        .logo-icon-fallback {
            font-size: 2rem;
            color: var(--primary);
        }

        .hero-h {
            color: #fff;
            font-size: 1.3rem;
            font-weight: 800;
            position: relative;
            z-index: 1;
        }

        .hero-sub {
            color: rgba(255,255,255,.55);
            font-size: .78rem;
            margin-top: 4px;
            position: relative;
            z-index: 1;
        }

        /* ── card ── */
        .login-card {
            background: #fff;
            margin: -18px 16px 0;
            border-radius: 20px;
            padding: 28px 20px 24px;
            box-shadow: 0 4px 24px rgba(0,0,0,.1);
            position: relative;
            z-index: 2;
        }

        .card-title {
            font-size: 1rem;
            font-weight: 800;
            color: var(--text);
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
                font-size: .75rem;
                font-weight: 700;
                color: var(--muted);
                margin-bottom: 6px;
                text-transform: uppercase;
                letter-spacing: .5px;
            }

        .input-wrap {
            position: relative;
        }

            .input-wrap .f-icon {
                position: absolute;
                left: 13px;
                top: 50%;
                transform: translateY(-50%);
                color: var(--muted);
                font-size: .9rem;
                pointer-events: none;
            }

            .input-wrap input {
                width: 100%;
                padding: 12px 44px 12px 40px;
                border: 1.8px solid #e5e7eb;
                border-radius: 12px;
                font-family: 'Nunito', sans-serif;
                font-size: .9rem;
                font-weight: 600;
                color: var(--text);
                background: #fafafa;
                outline: none;
                transition: border-color .2s, box-shadow .2s, background .2s;
            }

                .input-wrap input:focus {
                    border-color: var(--primary);
                    background: #fff;
                    box-shadow: 0 0 0 3px rgba(232,64,0,.1);
                }

                .input-wrap input::placeholder {
                    color: #c4c8d0;
                    font-weight: 500;
                }

        .toggle-pw {
            position: absolute;
            right: 13px;
            top: 50%;
            transform: translateY(-50%);
            background: none;
            border: none;
            color: var(--muted);
            cursor: pointer;
            font-size: .85rem;
            padding: 2px;
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
            font-size: .78rem;
            font-weight: 600;
            color: var(--muted);
            cursor: pointer;
        }

            .remember input[type=checkbox] {
                accent-color: var(--primary);
                width: 15px;
                height: 15px;
            }

        .forgot-link {
            font-size: .78rem;
            font-weight: 700;
            color: var(--primary);
            text-decoration: none;
        }

            .forgot-link:hover {
                text-decoration: underline;
            }

        /* ── submit btn ── */
        .btn-login {
            width: 100%;
            padding: 14px;
            background: linear-gradient(135deg, var(--primary), var(--accent));
            color: #fff;
            border: none;
            border-radius: 13px;
            font-family: 'Nunito', sans-serif;
            font-size: 1rem;
            font-weight: 800;
            cursor: pointer;
            display: flex;
            align-items: center;
            justify-content: center;
            gap: 8px;
            transition: opacity .2s, transform .15s;
            box-shadow: 0 4px 16px rgba(232,64,0,.35);
        }

            .btn-login:hover {
                opacity: .92;
                transform: translateY(-1px);
            }

            .btn-login:active {
                transform: translateY(0);
            }

        /* ── divider ── */
        .divider {
            display: flex;
            align-items: center;
            gap: 10px;
            margin: 20px 0;
            color: var(--muted);
            font-size: .75rem;
            font-weight: 600;
        }

            .divider::before, .divider::after {
                content: '';
                flex: 1;
                height: 1.5px;
                background: #e5e7eb;
            }

        /* ── social btns ── */
        .social-row {
            display: flex;
            gap: 10px;
        }

        .btn-social {
            flex: 1;
            padding: 11px 8px;
            border: 1.8px solid #e5e7eb;
            border-radius: 12px;
            background: #fff;
            font-family: 'Nunito', sans-serif;
            font-size: .8rem;
            font-weight: 700;
            color: var(--text);
            cursor: pointer;
            display: flex;
            align-items: center;
            justify-content: center;
            gap: 7px;
            transition: border-color .2s, background .2s;
        }

            .btn-social:hover {
                border-color: var(--primary);
                background: #fff8f5;
            }

            .btn-social .fa-google {
                color: #ea4335;
            }

            .btn-social .fa-mobile-alt {
                color: var(--primary);
            }

        /* ── register link ── */
        .register-row {
            text-align: center;
            margin-top: 22px;
            font-size: .82rem;
            color: var(--muted);
            font-weight: 600;
        }

            .register-row a {
                color: var(--primary);
                font-weight: 800;
                text-decoration: none;
            }

                .register-row a:hover {
                    text-decoration: underline;
                }

        /* ── error msg ── */
        .err-msg {
            background: #fff1f2;
            border: 1.5px solid #fca5a5;
            border-radius: 10px;
            padding: 10px 14px;
            font-size: .8rem;
            font-weight: 700;
            color: #dc2626;
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
            text-align: center;
            padding: 18px 16px 24px;
            font-size: .68rem;
            color: var(--muted);
        }

            .page-footer a {
                color: var(--primary);
                text-decoration: none;
            }

        /* ── toast ── */
        .toast {
            position: fixed;
            bottom: 24px;
            left: 50%;
            transform: translateX(-50%) translateY(16px);
            background: #222;
            color: #fff;
            padding: 9px 20px;
            border-radius: 50px;
            font-size: .8rem;
            font-weight: 600;
            opacity: 0;
            transition: opacity .28s, transform .28s;
            pointer-events: none;
            z-index: 999;
            white-space: nowrap;
            box-shadow: 0 4px 16px rgba(0,0,0,.25);
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
                    <img src="demoepay/images/logo_light.png" alt="ePay" class="h-logo" onerror="this.style.display='none'" />
                  <%--  <div class="h-title">e<span>Pay</span> India</div>--%>
                </header>

                <!-- HERO STRIP -->
                <div class="hero-strip">
                    <div class="logo-circle">
                        <img src="demoepay/images/logo.png" alt="ePay India" onerror="this.style.display='none'; this.nextElementSibling.style.display='block';" />
                        <i class="fa fa-wallet logo-icon-fallback" style="display: none;"></i>
                    </div>
                    <div class="hero-h">Welcome Back!</div>
                    <div class="hero-sub">Sign in to your ePay India account</div>
                </div>

                <!-- LOGIN CARD -->
                <div class="login-card">

                    <div class="card-title">
                        <i class="fa fa-sign-in-alt"></i>
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
                        <label for="userId">User ID</label>
                        <div class="input-wrap">
                            <i class="fa fa-user f-icon"></i>
                            <asp:TextBox ID="TxtUserID" runat="server"
                                placeholder="Enter your User ID"
                                MaxLength="50">
                            </asp:TextBox>
                            <%--  <input type="text" id="userId" name="userId"
                            placeholder="Enter your User ID" autocomplete="username" required />--%>
                        </div>
                    </div>

                    <!-- Password -->
                    <div class="field">
                        <label for="password">Password</label>
                        <div class="input-wrap">
                            <i class="fa fa-lock f-icon"></i>
                            <%--  <input type="password" id="password" name="password"
                                placeholder="Enter your password" autocomplete="current-password" required />--%>
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
                            <%--       <a href="webapp.html">
                                <button type="button" class="toggle-pw" onclick="togglePw()" id="pwToggle" title="Show / Hide">
                                    <i class="fa fa-eye" id="pwIcon"></i>
                                </button>
                            </a>--%>
                        </div>
                    </div>

                    <!-- Remember / Forgot -->
                    <div class="row-extra">
                        <label class="remember">
                            <input type="checkbox" id="rememberMe" />
                            Remember me
       
                        </label>
                        <a href="#" class="forgot-link" onclick="showToast('Password reset link will be sent to your registered email.')">Forgot Password?
                        </a>
                    </div>

                    <!-- Submit -->
                    <asp:Button ID="BtnLogin" runat="server"
                        Text="Login  →"
                        CssClass="btn-login"
                        OnClick="BtnLogin_Click" />

                    <%-- <button type="submit" class="btn-login">
                        <i class="fa fa-sign-in-alt"></i>
                        Login to Account
     
                    </button>--%>

                    <!-- Divider -->
                    <%--   <div class="divider">OR LOGIN WITH</div>--%>

                    <!-- Social -->
                    <%--         <div class="social-row">
                        <button class="btn-social" onclick="showToast('Google login coming soon')">
                            <i class="fab fa-google"></i>Google
     
                        </button>
                        <button class="btn-social" onclick="showToast('OTP login coming soon')">
                            <i class="fa fa-mobile-alt"></i>OTP Login
     
                        </button>
                    </div>--%>

                    <!-- Register link -->
                    <%--     <div class="register-row">
                        Don't have an account?
     
                        <a href="register.html">Register Now</a>
                    </div>--%>
                </div>
                <!-- /login-card -->

                <!-- FOOTER NOTE -->
                <div class="page-footer">
                    By logging in you agree to our
   
                    <a href="#">Terms of Service</a> &amp;
   
                    <a href="#">Privacy Policy</a><br />
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

            // Show server-side error label if it has text
            window.addEventListener('DOMContentLoaded', function () {
                var lbl = document.getElementById('<%= lblError.ClientID %>');
                if (lbl && lbl.innerText.trim() !== '') {
                    lbl.style.display = 'block';
                }
            });
        </script >

    </form>
</body>
</html>
