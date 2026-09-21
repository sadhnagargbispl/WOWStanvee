using System;
using System.Configuration;
using System.Data.SqlClient;

public partial class CheckClaimStatus : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        string formNo = Request.QueryString["formno"];
        if (string.IsNullOrEmpty(formNo)) return;

        string conStr = ConfigurationManager.ConnectionStrings["constr"].ConnectionString;

        // Claim current WOW package purchase ke against check hota hai. Pichhle purchase ka claim
        // naye purchase ko block nahi karta, aur pichhla unclaimed benefit laps ho jaata hai.
        int cycleId = WowBenefit.GetCurrentCycleId(formNo);

        using (SqlConnection con = new SqlConnection(conStr))
        {
            string sql = @"SELECT COUNT(1)
                           FROM   ScratchClaimOrder
                           WHERE  FormNo = @FormNo
                                  AND CycleId = @CycleId
                                  AND UPPER(Status) = 'SUCCESS'";
            SqlCommand cmd = new SqlCommand(sql, con);
            cmd.Parameters.AddWithValue("@FormNo", formNo);
            cmd.Parameters.AddWithValue("@CycleId", cycleId);

            con.Open();
            int count = Convert.ToInt32(cmd.ExecuteScalar());

            // 1 = already claimed, 0 = not claimed
            Response.Write(count > 0 ? "CLAIMED" : "NOT_CLAIMED");
        }
    }
}
