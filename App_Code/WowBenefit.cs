using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web;

/// <summary>
/// WOW package ke "purchase cycle" ko resolve karta hai.
///
/// Rule:
///  - Member jab bhi WOW Movie package (KitId 18 / 19) purchase karta hai, ek naya cycle ban
///    jaata hai. Baaki kits (Royal / Full Tank Card / Free Registration) par cycle nahi banti.
///  - Free Product aur Scratch Card ka entitlement HAMESHA current (latest) cycle ka hota hai.
///  - Pichhle cycle me agar claim nahi kiya to wo lapse ho jaata hai - sirf current cycle
///    claim ho sakta hai.
///
/// Har benefit row (ScratchHistory / FreeProductClaim / ScratchClaimOrder) apne WowCycleId ke
/// saath store hoti hai, isliye purana data history ke liye bana rehta hai par claimable nahi.
///
/// NOTE: column ka naam WowCycleId hai, CycleId nahi - ScratchHistory aur FreeProductClaim me
/// CycleId naam ka column pehle se maujood hai jiska matlab alag hai (MLM session cycle).
/// </summary>
public static class WowBenefit
{
    /// <summary>
    /// Kit ids jinke purchase par Free Product / Scratch Card ka benefit milta hai.
    /// Ye benefit SIRF WOW Movie package ke liye hai, jiske do kits hain:
    ///   18 = WOW Movie @999 (ReferLink.aspx ka refer link)
    ///   19 = WOW Movie ka doosra kit
    /// Login.aspx.cs ka portal access check bhi inhi 18 / 19 ko allow karta hai.
    /// Baaki kits par naya cycle nahi banta:
    ///   13 = FULL TANK CARD @4999, 4 = ROYAL PACKAGE @9999, 12 = FREE REGISTRATION
    /// Kit list badle to yahan aur sql/ ki scripts me update karni hai.
    /// </summary>
    public static readonly int[] WowKitIds = { 18, 19 };

    /// <summary>WowKitIds ka "18, 19" form - SQL IN (...) ke liye.</summary>
    private static string KitIdList
    {
        get { return string.Join(", ", Array.ConvertAll(WowKitIds, k => k.ToString())); }
    }

    private static string ConnectionString
    {
        get { return ConfigurationManager.ConnectionStrings["constr"].ConnectionString; }
    }

    /// <summary>
    /// Member ke latest WOW package purchase ka CycleId. Row na ho to on-demand bana deta hai.
    ///
    /// Purchase ka source sirf repurchincome hai - joining ka bill bhi wahin aata hai
    /// (BillType = 'J'), isliye M_MemberMaster alag se dekhne ki zarurat nahi. PurchaseRef
    /// bill ki apni RId se banta hai, isliye ek hi din ke do bills bhi alag cycles rehte hain.
    ///
    /// Koi purchase trace na mile to "LEGACY" cycle par gir jaata hai (purana FormNo-wise
    /// behaviour). Ek hi request me baar baar call hone par cached value milti hai.
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
       @Ref = 'R:' + CONVERT(varchar(20), r.RId),
       @Dt  = r.BillDate
FROM   repurchincome r
WHERE  r.FormNo = @FormNo
       AND r.KitId IN (" + KitIdList + @")
ORDER BY r.BillDate DESC, r.RId DESC;

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
                                    AND ISNULL(WowCycleId, 0) = @WowCycleId";

        object claimed = SqlHelper.ExecuteScalar(
            ConnectionString, CommandType.Text, sql,
            new SqlParameter("@FormNo", formNo),
            new SqlParameter("@WowCycleId", cycleId));

        return (claimed == null || claimed == DBNull.Value) ? null : claimed.ToString();
    }

    /// <summary>Current cycle ke against free product already claimed hai ya nahi.</summary>
    public static bool IsFreeProductClaimed(string formNo, int cycleId)
    {
        return GetClaimedFreeProductId(formNo, cycleId) != null;
    }
}
