using System;
using System.IO;
using System.Text;
using System.Security.Cryptography;

public partial class AppReferLink : System.Web.UI.Page
{
    public string ReferralLink1 { get; set; }
    public string ReferralLink2 { get; set; }
    public string ReferralLink3 { get; set; }
    public string ReferralLink4 { get; set; }
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (Session["Status"] != null && Session["Status"].ToString() == "OK")
            {
                string userCode = Session["idno"].ToString();

                // Kit Ids web wale ReferLink.aspx jaise hi hain
                ReferralLink1 = BuildLink("13", userCode); // FULL TANK CARD @4999
                ReferralLink2 = BuildLink("18", userCode); // WOW Movie @999
                ReferralLink3 = BuildLink("4", userCode);  // ROYAL PACKAGE @9999
                ReferralLink4 = BuildLink("12", userCode); // FREE REGISTRATION
                this.DataBind();
            }
            else
            {
                Response.Redirect("AppLogin.aspx", false);
            }
        }
        catch (Exception ex)
        {
            // throw new Exception(ex.Message);
        }
    }

    private static string BuildLink(string kitid, string userCode)
    {
        string token = Encrypt(kitid + "|" + userCode);
        return "https://stanveeservices.com/paymentGateway.aspx?x=" + token;
    }

    public static string Encrypt(string clearText)
    {
        string EncryptionKey = "Stanvee123";

        byte[] clearBytes = Encoding.Unicode.GetBytes(clearText);

        using (Aes encryptor = Aes.Create())
        {
            Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(
                EncryptionKey,
                new byte[]
                {
                    0x49, 0x76, 0x61, 0x6E,
                    0x20, 0x4D, 0x65, 0x64,
                    0x76, 0x65, 0x64, 0x65,
                    0x76
                });

            encryptor.Key = pdb.GetBytes(32);
            encryptor.IV = pdb.GetBytes(16);

            using (MemoryStream ms = new MemoryStream())
            {
                using (CryptoStream cs = new CryptoStream(
                    ms,
                    encryptor.CreateEncryptor(),
                    CryptoStreamMode.Write))
                {
                    cs.Write(clearBytes, 0, clearBytes.Length);
                    cs.Close();
                }

                clearText = Convert.ToBase64String(ms.ToArray());
            }
        }

        return clearText.Replace("+", "%2B");
    }
}
