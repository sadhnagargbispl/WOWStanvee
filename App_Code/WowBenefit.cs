using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web;

/// <summary>
/// WOW package ke "purchase cycle" ko resolve karta hai.
///
/// Rule:
///  - Member jab bhi koi WOW package purchase karta hai, ek naya cycle ban jaata hai.
///    (Package kit ids ReferLink.aspx ke refer links se: 4 / 13 / 18 / 19 - free registration
///     wala KitId 12 package nahi hai, isliye usse cycle nahi banti.)
///  - Free Product aur Scratch Card ka entitlement HAMESHA current (latest) cycle ka hi hota hai.
///  - Pichhle cycle me agar claim nahi kiya to wo lapse ho jaata hai - sirf current cycle claim ho sakta hai.
///
/// Har benefit row (ScratchHistory / FreeProductClaim / ScratchClaimOrder) apne CycleId ke saath
/// store hoti hai, isliye purana data history ke liye bana rehta hai par claimable nahi rehta.
/// </summary>
public static class WowBenefit
{
    /// <summary>
    /// Kit ids jo WOW package maane jaate hain - ReferLink.aspx ke refer links ke hisaab se:
    ///   4  = ROYAL PACKAGE @9999
    ///   13 = FULL TANK CARD @4999
    ///   18 = WOW Movie @999
    ///   19 = WOW Movie ka doosra kit (Login.aspx.cs ka access check isse allow karta hai)
    /// KitId 12 (FREE REGISTRATION) jaan bujh kar bahar hai - wo package purchase nahi hai.
    /// Naya package add ho to sirf yahan aur migration script me kit list update karni hai.
    /// </summary>
    public static readonly int[] WowKitIds = { 4, 13, 18, 19 };

    /// <summary>WowKitIds ka "4, 13, 18, 19" form - SQL IN (...) ke liye.</summary>
    private static string KitIdList
    {
        get { return string.Join(", ", Array.ConvertAll(WowKitIds, k => k.ToString())); }
    }

    /// <summary>
    /// M_MemberMaster.KitID plain number bhi ho sakta hai aur comma separated list bhi
    /// (Login.aspx.cs me Contains() se check hota hai), isliye dono handle karne wala predicate.
    /// </summary>
    private static string KitIdCsvPredicate(string columnExpression)
    {
        string normalized = "',' + REPLACE(CONVERT(varchar(200), ISNULL(" + columnExpression + ", '')), ' ', '') + ','";
        string[] parts = Array.ConvertAll(WowKitIds, k => normalized + " LIKE '%," + k + ",%'");
        return "(" + string.Join(" OR ", parts) + ")";
    }

    private static string ConnectionString
    {
        get { return ConfigurationManager.ConnectionStrings["constr"].ConnectionString; }
    }

    /// <summary>
    /// Member ke latest WOW package purchase ka CycleId. Row na ho to on-demand bana deta hai.
    /// Purchase trace na mile to "LEGACY" cycle par gir jaata hai (purana FormNo-wise behaviour).
    /// Ek hi request me baar baar call hone par cached value milti hai.
    /// </summary>
    public static int GetCurrentCycleId(string formNo)
    {
        if (string.IsNullOrEmpty(formNo)) return 0;

        string cacheKey = "WowCycle_" + formNo;
        HttpContext ctx = HttpContext.Current;
        if (ctx != null && ctx.Items[cacheKey] != null)
        {
            return Convert.ToInt32(ctx.Items[cacheKey]);
        }

        int cycleId = 0;
        try
        {
            string sql = @"
DECLARE @Ref varchar(50), @Dt datetime;

SELECT TOP 1
       @Ref = Src + ':' + CONVERT(varchar(23), PurchaseDate, 126),
       @Dt  = PurchaseDate
FROM (
        SELECT 'R' AS Src, BillDate AS PurchaseDate
        FROM   repurchincome
        WHERE  FormNo = @FormNo
               AND KitId IN (" + KitIdList + @")

        UNION ALL

        SELECT 'J', ISNULL(Upgradedate, Doj)
        FROM   M_MemberMaster
        WHERE  FormNo = @FormNo
               AND " + KitIdCsvPredicate("KitID") + @"
     ) x
WHERE PurchaseDate IS NOT NULL
ORDER BY PurchaseDate DESC;

IF @Ref IS NULL SET @Ref = 'LEGACY';

-- Do parallel request ek saath aayen to duplicate key ignore karo, cycle to ban hi chuka hoga
BEGIN TRY
    IF NOT EXISTS (SELECT 1 FROM WowPurchaseCycle WHERE FormNo = @FormNo AND PurchaseRef = @Ref)
    BEGIN
        INSERT INTO WowPurchaseCycle (FormNo, PurchaseRef, PurchaseDate)
        VALUES (@FormNo, @Ref, @Dt);
    END
END TRY
BEGIN CATCH
END CATCH

SELECT TOP 1 CycleId FROM WowPurchaseCycle WHERE FormNo = @FormNo AND PurchaseRef = @Ref;";

            object result = SqlHelper.ExecuteScalar(
                ConnectionString, CommandType.Text, sql,
                new SqlParameter("@FormNo", formNo));

            if (result != null && result != DBNull.Value)
            {
                cycleId = Convert.ToInt32(result);
            }
        }
        catch (Exception)
        {
            cycleId = 0;
        }

        if (ctx != null) ctx.Items[cacheKey] = cycleId;
        return cycleId;
    }

    /// <summary>Session se FormNo uthakar current CycleId deta hai.</summary>
    public static int GetCurrentCycleIdFromSession()
    {
        HttpContext ctx = HttpContext.Current;
        if (ctx == null || ctx.Session == null) return 0;

        object formNo = ctx.Session["FormNo"] ?? ctx.Session["formno"];
        if (formNo == null) return 0;

        return GetCurrentCycleId(formNo.ToString());
    }

    /// <summary>Current cycle me free product claim ho chuka hai to uska ProductId, warna null.</summary>
    public static string GetClaimedFreeProductId(string formNo, int cycleId)
    {
        if (string.IsNullOrEmpty(formNo)) return null;

        const string sql = @"SELECT TOP 1 ProductId
                             FROM   FreeProductClaim
                             WHERE  FormNo = @FormNo
                                    AND ISNULL(CycleId, 0) = @CycleId";

        object claimed = SqlHelper.ExecuteScalar(
            ConnectionString, CommandType.Text, sql,
            new SqlParameter("@FormNo", formNo),
            new SqlParameter("@CycleId", cycleId));

        return (claimed == null || claimed == DBNull.Value) ? null : claimed.ToString();
    }

    /// <summary>Current cycle ke against free product already claimed hai ya nahi.</summary>
    public static bool IsFreeProductClaimed(string formNo, int cycleId)
    {
        return GetClaimedFreeProductId(formNo, cycleId) != null;
    }
}
