using DocumentFormat.OpenXml.Wordprocessing;
using System;
using System.Activities.Expressions;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class SiteMaster : System.Web.UI.MasterPage
{
    string constr = ConfigurationManager.ConnectionStrings["constr"].ConnectionString;
    string constr1 = ConfigurationManager.ConnectionStrings["constr1"].ConnectionString;
    public string shopEnc = "";
    public string gvEnc = "";
    public string utilityEnc = "";
    public string foodEnc = "";
    public string movieEnc = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (Application["WebStatus"] != null)
            {
                if (Application["WebStatus"].ToString() == "N")
                {
                    Session.Abandon();
                    Response.Redirect("default.aspx", false);
                }
            }
     
            if (!Page.IsPostBack)
            {
                shopEnc = EncryptText("shop");
                gvEnc = EncryptText("gv");
                utilityEnc = EncryptText("utility");
                foodEnc = EncryptText("fd");
                movieEnc = EncryptText("movie");
                DataTable dt = new DataTable();
                if (Session["Status"] != null && Session["Status"].ToString() == "OK")
                {
                    string sql = "select profilepic, * from M_MemberMaster where formno=" + Session["FormNo"];
                    dt = SqlHelper.ExecuteDataset(constr1, CommandType.Text, sql).Tables[0];
                    if (dt.Rows.Count > 0)
                    {
                        LblIDno.Text = dt.Rows[0]["idno"].ToString();
                        LblIDnos.Text = dt.Rows[0]["idno"].ToString();
                        LblName.Text = dt.Rows[0]["memfirstname"].ToString();
                        lblnmaes.Text = dt.Rows[0]["memfirstname"].ToString();
                        //LblIDD.Text = "(" + dt.Rows[0]["memfirstname"].ToString() + ")";
                    }
                }
            }
        }
        catch (Exception ex)
        {
            // Handle exception
        }

    }
    public static string EncryptText(string input)
    {
        byte[] key = Encoding.UTF8.GetBytes("A1B2C3D4E5F6G7H8");  // 16 char key
        byte[] iv = Encoding.UTF8.GetBytes("1H2G3F4E5D6C7B8A");   // 16 char IV

        using (Aes aes = Aes.Create())
        {
            aes.Key = key;
            aes.IV = iv;

            var encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

            using (var ms = new MemoryStream())
            {
                using (var cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                using (var sw = new StreamWriter(cs))
                {
                    sw.Write(input);
                }
                return Convert.ToBase64String(ms.ToArray());
            }
        }
    }

}
