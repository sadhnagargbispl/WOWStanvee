using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class SaveScratch : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string json;
        using (StreamReader sr = new StreamReader(Request.InputStream))
        {
            json = sr.ReadToEnd();
        }
        dynamic data = JsonConvert.DeserializeObject(json);
        int productId = data.ProductId;
        string name = data.ProductName;
        decimal price = data.ProductPrice;
        string image = data.ProductImage;
        string FormNo = data.FormNo;
        Save(productId, name, price, image, FormNo);
        Response.Write("OK");
    }
    private void Save(int productId, string name, decimal price, string image,string FormNo)
    {
        string conStr = ConfigurationManager.ConnectionStrings["constr"].ConnectionString;

        // Scratch card current WOW package purchase ke against hota hai - pichhla cycle laps ho chuka hota hai
        int cycleId = WowBenefit.GetCurrentCycleId(FormNo);

        using (SqlConnection con = new SqlConnection(conStr))
        {
            string sql = @"
    IF NOT EXISTS (SELECT 1 FROM ScratchHistory WHERE FormNo = @FormNo AND ISNULL(WowCycleId, 0) = @WowCycleId)
    BEGIN
        INSERT INTO ScratchHistory
        (ProductId, ProductName, ProductPrice, ProductImage, FormNo, WowCycleId)
        VALUES (@pid, @name, @price, @image, @FormNo, NULLIF(@WowCycleId, 0))
    END";

            SqlCommand cmd = new SqlCommand(sql, con);
            cmd.Parameters.AddWithValue("@pid", productId);
            cmd.Parameters.AddWithValue("@name", name);
            cmd.Parameters.AddWithValue("@price", price);
            cmd.Parameters.AddWithValue("@image", image);
            cmd.Parameters.AddWithValue("@FormNo", FormNo);
            cmd.Parameters.AddWithValue("@WowCycleId", cycleId);
            con.Open();
            try
            {
                cmd.ExecuteNonQuery();
            }
            catch (SqlException ex)
            {
                if (ex.Number == 2627 || ex.Number == 2601)
                {
                    // Duplicate FormNo – ignore
                }
                else
                {
                    throw;
                }
            }
        }
    }
}