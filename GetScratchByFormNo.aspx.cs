using Newtonsoft.Json;
using System;
using System.Configuration;
using System.Data.SqlClient;

public partial class GetScratchByFormNo : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string formNo = Request.QueryString["formno"];
        if (string.IsNullOrEmpty(formNo)) return;

        string conStr = ConfigurationManager.ConnectionStrings["constr"].ConnectionString;

        // Sirf current WOW package purchase ka scratch dikhta hai - purane cycle ka laps ho chuka hai
        int cycleId = WowBenefit.GetCurrentCycleId(formNo);

        using (SqlConnection con = new SqlConnection(conStr))
        {
            SqlCommand cmd = new SqlCommand(
                "SELECT TOP 1 ProductId,ProductName,ProductPrice,ProductImage FROM ScratchHistory " +
                "WHERE FormNo=@FormNo AND ISNULL(WowCycleId, 0)=@WowCycleId",
                con);

            cmd.Parameters.AddWithValue("@FormNo", formNo);
            cmd.Parameters.AddWithValue("@WowCycleId", cycleId);
            con.Open();

            SqlDataReader dr = cmd.ExecuteReader();
            if (dr.Read())
            {
                Response.Write(JsonConvert.SerializeObject(new
                {
                    id = dr["ProductId"],
                    name = dr["ProductName"],
                    price = dr["ProductPrice"],
                    image = dr["ProductImage"]
                }));
            }
        }
    }
}
