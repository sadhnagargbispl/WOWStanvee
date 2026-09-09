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

        using (SqlConnection con = new SqlConnection(conStr))
        {
            SqlCommand cmd = new SqlCommand(
                "SELECT TOP 1 ProductId,ProductName,ProductPrice,ProductImage FROM ScratchHistory WHERE FormNo=@FormNo",
                con);

            cmd.Parameters.AddWithValue("@FormNo", formNo);
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
