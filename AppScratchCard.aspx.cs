using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class AppScratchCard : System.Web.UI.Page
{
    string constr = ConfigurationManager.ConnectionStrings["constr"].ConnectionString;

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (Session["Status"] != null && Session["Status"].ToString() == "OK")
            {
                hfFormNo.Value = Session["FormNo"].ToString();
                BindServices();
            }
            else
            {
                Response.Redirect("Login.aspx", false);
            }
        }
        catch (Exception ex)
        {
            // log ex if needed
        }
    }

    private void BindServices()
    {
        try
        {
            DataSet ds = new DataSet();
            string sql = "Exec Sp_GetDetailsdis";
            ds = SqlHelper.ExecuteDataset(constr, CommandType.Text, sql);

            var list = new List<object>();

            if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
            {
                RptProducts.DataSource = ds.Tables[0];
                RptProducts.DataBind();

                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    list.Add(new
                    {
                        id = row["ProductId"].ToString(),
                        name = row["ProductName"].ToString(),
                        description = row["Description"].ToString(),
                        price = row["Price"].ToString(),
                        image = row["ImageUrl"].ToString()
                    });
                }
            }

            string json = JsonConvert.SerializeObject(list); // ✅ serialize list to JSON string

            Page.ClientScript.RegisterStartupScript(
                this.GetType(),
                "productsData",
                "window.jsonFromBackend = " + json + ";",  // ✅ paste raw JSON — NOT SerializeObject(json) again
                true
            );
        }
        catch (Exception ex)
        {
            Page.ClientScript.RegisterStartupScript(
                this.GetType(),
                "productsData",
                "window.jsonFromBackend = [];",
                true
            );
        }
    }
}

public class ProductApp
{
    public string id { get; set; }
    public string name { get; set; }
    public string description { get; set; }
    public string price { get; set; }
    public string image { get; set; }
}