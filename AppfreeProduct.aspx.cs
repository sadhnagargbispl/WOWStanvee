using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class AppfreeProduct : System.Web.UI.Page
{
    string constr = ConfigurationManager.ConnectionStrings["constr"].ConnectionString;
    string constr1 = ConfigurationManager.ConnectionStrings["constr1"].ConnectionString;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["Status"] != null && Session["Status"].ToString() == "OK")
        {
            if (!Page.IsPostBack)
            {
                BindServices();
            }
        }
        else
        {
            Response.Redirect("Login.aspx", false);
        }
    }
    private void BindServices()
    {
        try
        {
            DataSet Ds = new DataSet();
            string Sql = "Exec Sp_GetDetails";
            Ds = SqlHelper.ExecuteDataset(constr, CommandType.Text, Sql);
            if (Ds.Tables[1].Rows.Count > 0)
            {
                RptOffers.DataSource = Ds.Tables[1];
                RptOffers.DataBind();
            }
        }
        catch (Exception ex)
        {
            //throw new Exception(ex.Message);
        }
    }
    protected void btnClaim_Command(object sender, CommandEventArgs e)
    {
        if (e.CommandName == "Claim")
        {
            string productId = e.CommandArgument.ToString();
            string encodedProductId = Convert.ToBase64String(
           System.Text.Encoding.UTF8.GetBytes(productId)
       );
            // Free product current WOW package purchase ke against milta hai - pichhla unclaimed laps ho chuka hai
            string formNo = Session["formno"].ToString();
            if (WowBenefit.IsFreeProductClaimed(formNo, WowBenefit.GetCurrentCycleId(formNo)))
            {
                string script = "window.onload=function(){alert('You have already claimed.');window.location='freeProduct.aspx';}";
                ClientScript.RegisterStartupScript(this.GetType(), "AlreadyClaimed", script, true);
                return;
            }
            else
            {
                Response.Redirect("ClaimFreePorduct.aspx?productid=" + Server.UrlEncode(encodedProductId));
            }

        }
    }
    protected void rptProducts_ItemDataBound(object sender, RepeaterItemEventArgs e)
    {
        if (e.Item.ItemType == ListItemType.Item ||
            e.Item.ItemType == ListItemType.AlternatingItem)
        {
            if (Session["formno"] == null) return;

            string formNo = Session["formno"].ToString();
            string currentProductId = DataBinder.Eval(e.Item.DataItem, "ProductId").ToString();

            Button btnClaim = (Button)e.Item.FindControl("btnClaim");
            Label lblClaimed = (Label)e.Item.FindControl("lblClaimed");

            // 🔥 Current WOW package purchase ke against claimed ProductId nikaalo.
            // Pichhle purchase ka claim ya unclaimed benefit ab count nahi hota.
            string claimedProductId = WowBenefit.GetClaimedFreeProductId(
                formNo, WowBenefit.GetCurrentCycleId(formNo)
            );

            // ❌ Agar is purchase ke against koi product claim ho chuka hai
            if (claimedProductId != null)
            {
                if (claimedProductId == currentProductId)
                {
                    // ✅ Ye wahi product hai jo claim hua
                    btnClaim.Visible = false;
                    lblClaimed.Visible = true;
                }
                else
                {
                    // ❌ Baaki sab products disable
                    btnClaim.Enabled = false;
                    btnClaim.Text = "Claim Now";
                    btnClaim.CssClass = "mt-4 w-full bg-gray-400 text-white py-2 rounded-lg font-medium";
                }
            }
            else
            {
                // ✅ Abhi koi claim nahi hua
                btnClaim.Visible = true;
                btnClaim.Enabled = true;
                lblClaimed.Visible = false;
            }
        }
    }
}