using ClosedXML.Excel;
using Irony.Parsing;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Xml;

public partial class ClaimFreePorduct : System.Web.UI.Page
{
    string constr = ConfigurationManager.ConnectionStrings["constr"].ConnectionString;
    string productid;
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (Session["Status"] != null && Session["Status"].ToString() == "OK")
            {
                this.btnPay.Attributes.Add("onclick", DisableTheButton(this.Page, this.btnPay));
                string encoded = Request.QueryString["productid"];
                byte[] data = Convert.FromBase64String(encoded);
                productid = Encoding.UTF8.GetString(data);
                if (!Page.IsPostBack)
                {
                    HdnCheckTrnns.Value = GenerateRandomStringJoining(6);
                }
            }
            else
            {
                Response.Redirect("Login.aspx", false);
            }
        }
        catch (Exception ex)
        {

        }
    }
    public string GenerateRandomStringJoining(int length)
    {
        Random rdm = new Random();
        char[] allowChrs = "123456789".ToCharArray();
        string sResult = "";

        for (int i = 0; i < length; i++)
        {
            sResult += allowChrs[rdm.Next(allowChrs.Length)];
        }

        return sResult;
    }
    private string DisableTheButton(Control pge, Control btn)
    {
        System.Text.StringBuilder sb = new System.Text.StringBuilder();
        sb.Append("if (typeof(Page_ClientValidate) == 'function') {");
        sb.Append("if (Page_ClientValidate() == false) { return false; }} ");
        sb.Append("if (confirm('Are you sure to proceed?') == false) { return false; } ");
        sb.Append("this.value = 'Please wait...';");
        sb.Append("this.disabled = true;");
        sb.Append(pge.Page.GetPostBackEventReference(btn));
        sb.Append(";");
        return sb.ToString();
    }
    public string GetClientIP()
    {
        string ipAddress = Request.ServerVariables["HTTP_X_FORWARDED_FOR"];

        if (!string.IsNullOrEmpty(ipAddress))
        {
            // Agar multiple IPs ho to first lo
            ipAddress = ipAddress.Split(',')[0];
        }
        else
        {
            ipAddress = Request.ServerVariables["REMOTE_ADDR"];
        }

        return ipAddress;
    }

    protected void btnPay_Click(object sender, EventArgs e)
    {
        string ipAddress = GetClientIP();
        string orderId = DateTime.Now.ToString("yyyyMMddHHmmssfff");
        string checkSql = "SELECT COUNT(1) FROM FreeProductClaim WHERE FormNo = @FormNo";
        SqlParameter[] checkParams = {new SqlParameter("@FormNo", Session["formno"].ToString())};
        int exists = Convert.ToInt32(SqlHelper.ExecuteScalar(constr, CommandType.Text, checkSql, checkParams));
        if (exists > 0)
        {
            string script = "window.onload=function(){alert('You have already claimed this free product.');window.location='freeProduct.aspx';}";
            ClientScript.RegisterStartupScript(this.GetType(), "AlreadyClaimed", script, true);
            return;
        }
        else
        {
            string Strqueryquer = "Insert into Trnjoining(Transid)values(" + HdnCheckTrnns.Value + ")";
            int isOk1 = Convert.ToInt32(SqlHelper.ExecuteNonQuery(constr, CommandType.Text, Strqueryquer));

            if (isOk1 > 0)
            {
                string OrderId = DateTime.Now.ToString("yyyyMMddHHmmssfff");
                string sql = "insert into FreeProductClaim(ProductId,FormNo,FullName,Email,Phone,Address,City,ZipCode,IPAddress)" +
                             "VALUES('" + productid + "','" + Session["formno"] + "','" + txtName.Value + "','" + txtEmail.Value + "','" + txtPhone.Value + "','" + txtAddress.Value + "','" + txtCity.Value + "','" + Convert.ToInt32(txtZip.Value) + "','" + ipAddress + "')";

                int i = SqlHelper.ExecuteNonQuery(constr, CommandType.Text, sql);
                if (i > 0)
                {
                    string script = "window.onload=function(){alert('Thank you! Your free product claim has been completed successfully.!');window.location='freeProduct.aspx';}";
                    ClientScript.RegisterStartupScript(this.GetType(), "Redirect", script, true);
                    return;
                }
            }
            else
            {
                string script = "window.onload=function(){alert('Try Again After Some Time.!');window.location='ClaimFreePorduct.aspx?productid=" + Request.QueryString["productid"] + ";}";
                ClientScript.RegisterStartupScript(this.GetType(), "Redirect", script, true);
                return;
            }
        }
        
    }
}