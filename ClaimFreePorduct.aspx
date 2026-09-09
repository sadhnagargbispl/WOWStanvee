<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ClaimFreePorduct.aspx.cs" Inherits="ClaimFreePorduct" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Checkout</title>
    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
    <!-- Tailwind CDN -->
    <script src="https://cdn.tailwindcss.com"></script>
    <script>
        function isNumber(evt) {
            var charCode = evt.which ? evt.which : evt.keyCode;
            if (charCode < 48 || charCode > 57)
                return false;
            return true;
        }
    </script>
</head>
<body class="bg-gray-100">
    <form id="form1" runat="server">
        <div>

            <div class="max-w-6xl mx-auto p-6">
                <div class="bg-white rounded-lg shadow-md p-6 grid grid-cols-1 md:grid-cols-2 gap-8">

                    <!-- Address Form -->
                    <div>
                        <h2 class="text-xl font-semibold mb-6 border-b pb-2">Billing Address</h2>

                        <div class="space-y-4">
                            <div>
                                <label class="block text-sm font-medium mb-1">Full Name</label>
                                <input type="text" id="txtName" runat="server"
                                    class="w-full border rounded-md p-2 focus:ring focus:ring-blue-300">
                            </div>

                            <div>
                                <label class="block text-sm font-medium mb-1">Email</label>
                                <input type="email" id="txtEmail" runat="server"
                                    class="w-full border rounded-md p-2 focus:ring focus:ring-blue-300">
                            </div>

                            <div>
                                <label class="block text-sm font-medium mb-1">Phone</label>
                                <input type="tel" id="txtPhone" runat="server" maxlength="10" onkeypress="return isNumber(event)"
                                    class="w-full border rounded-md p-2 focus:ring focus:ring-blue-300">
                            </div>

                            <div>
                                <label class="block text-sm font-medium mb-1">Address</label>
                                <textarea id="txtAddress" runat="server" rows="3"
                                    class="w-full border rounded-md p-2 focus:ring focus:ring-blue-300"></textarea>
                            </div>

                            <div class="grid grid-cols-2 gap-4">
                                <div>
                                    <label class="block text-sm font-medium mb-1">City</label>
                                    <input type="text" id="txtCity" runat="server"
                                        class="w-full border rounded-md p-2 focus:ring focus:ring-blue-300">
                                </div>
                                <asp:HiddenField ID="HdnCheckTrnns" runat="server" />
                                <div>
                                    <label class="block text-sm font-medium mb-1">Zip Code</label>
                                    <input type="text" id="txtZip" runat="server" onkeypress="return isNumber(event)"
                                        class="w-full border rounded-md p-2 focus:ring focus:ring-blue-300">
                                </div>
                            </div>
                            <asp:Button ID="btnPay" runat="server"
                                Text="Claim Now"
                                CssClass="w-full bg-green-600 text-white py-3 rounded-md font-medium hover:bg-green-700 transition"
                                OnClientClick="return validateForm();" OnClick="btnPay_Click" />
                        </div>
                    </div>


                </div>
            </div>
        </div>
        <script>
            function validateForm() {

                var name = document.getElementById('<%= txtName.ClientID %>').value.trim();
                var email = document.getElementById('<%= txtEmail.ClientID %>').value.trim();
                var phone = document.getElementById('<%= txtPhone.ClientID %>').value.trim();
                var address = document.getElementById('<%= txtAddress.ClientID %>').value.trim();
                var city = document.getElementById('<%= txtCity.ClientID %>').value.trim();
                var zip = document.getElementById('<%= txtZip.ClientID %>').value.trim();

                if (name === "") {
                    alert("Please enter Full Name");
                    return false;
                }

                var emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
                if (!emailRegex.test(email)) {
                    alert("Please enter valid Email");
                    return false;
                }
                if (phone === "") {
                    alert("Enter valid 10 digit mobile number");
                    return false;
                }
                if (address === "") {
                    alert("Please enter Address");
                    return false;
                }
                if (city === "") {
                    alert("Please enter City");
                    return false;
                }
                if (zip.length < 5) {
                    alert("Enter valid Zip Code");
                    return false;
                }

                return true;
            }
        </script>
    </form>
</body>
</html>
