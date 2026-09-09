using System;
using System.IO;
using System.Text;
using System.Security.Cryptography;

public partial class ReferLink : System.Web.UI.Page
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
                string userCode = Session["idno"].ToString(); ; // pehle hardcode karke test karo
                string kitid = "13";
                string data = kitid + "|" + userCode;
                string token = Encrypt(data);
                string baseUrl = "https://stanveeservices.com/paymentGateway.aspx?x=" + token;
                ReferralLink1 = baseUrl;

                string kitid1 = "18";
                string data1 = kitid1 + "|" + userCode;
                string token1 = Encrypt(data1);
                string baseUrl1 = "https://stanveeservices.com/paymentGateway.aspx?x=" + token1;
                ReferralLink2 = baseUrl1;

                string kitid2 = "4";
                string data2 = kitid2 + "|" + userCode;
                string token2 = Encrypt(data2);
                string baseUrl2 = "https://stanveeservices.com/paymentGateway.aspx?x=" + token2;
                ReferralLink3 = baseUrl2;

                string kitid3 = "12";
                string data3 = kitid3 + "|" + userCode;
                string token3 = Encrypt(data3);
                string baseUrl3 = "https://stanveeservices.com/paymentGateway.aspx?x=" + token3;
                ReferralLink4 = baseUrl3;
                this.DataBind();
            }

            else
            {
                Response.Redirect("Login.aspx", false);
            }
        }
        catch (Exception ex)
        {
            // throw new Exception(ex.Message);
        }


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