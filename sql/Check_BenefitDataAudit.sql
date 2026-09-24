/*
==========================================================================================
  Audit - migration chalane se pehle aur baad me chalaiye. Sab read-only hai.
==========================================================================================
*/

SET NOCOUNT ON;

DECLARE @FormNo varchar(50) = '7555462';   -- test member


/*----------------------------------------------------------------------------------------
  A1. (Migration se PEHLE) Kya koi member aisa hai jiska joining kit 18/19 hai par
      repurchincome me uska koi bill hi nahi?

      Aise members ka purchase trace nahi milega aur wo LEGACY cycle par chale jayenge
      (yani purana lifetime-ek-baar behaviour). Count 0 aaye to bilkul clean hai.
----------------------------------------------------------------------------------------*/
SELECT  Audit = 'A1_JoiningWithoutBill',
        Members = COUNT(*)
FROM    dbo.M_MemberMaster mm
WHERE   (   ',' + REPLACE(CONVERT(varchar(200), ISNULL(mm.KitID, '')), ' ', '') + ',' LIKE '%,18,%'
         OR ',' + REPLACE(CONVERT(varchar(200), ISNULL(mm.KitID, '')), ' ', '') + ',' LIKE '%,19,%')
        AND NOT EXISTS (SELECT 1 FROM dbo.repurchincome r
                        WHERE r.FormNo = mm.FormNo AND r.KitId IN (18, 19));


/*----------------------------------------------------------------------------------------
  A2. repurchincome me kit 18/19 ke bills ka BillType breakup
      ('J' = joining ka bill, baaki = baad ke purchases)
----------------------------------------------------------------------------------------*/
SELECT  Audit     = 'A2_BillTypeBreakup',
        r.KitId,
        r.BillType,
        Bills     = COUNT(*),
        Members   = COUNT(DISTINCT r.FormNo),
        FirstBill = MIN(r.BillDate),
        LastBill  = MAX(r.BillDate)
FROM    dbo.repurchincome r
WHERE   r.KitId IN (18, 19)
GROUP BY r.KitId, r.BillType
ORDER BY r.KitId, r.BillType;


/*----------------------------------------------------------------------------------------
  A3. (Migration ke BAAD) Kuch bhi unmapped to nahi reh gaya?
      Dono counts 0 hone chahiye.
----------------------------------------------------------------------------------------*/
SELECT  Audit = 'A3_Unmapped',
        ScratchWithoutCycle     = (SELECT COUNT(*) FROM dbo.ScratchHistory   WHERE WowCycleId IS NULL),
        FreeProductWithoutCycle = (SELECT COUNT(*) FROM dbo.FreeProductClaim WHERE WowCycleId IS NULL);


/*----------------------------------------------------------------------------------------
  A4. (Migration ke BAAD) Cycle summary
----------------------------------------------------------------------------------------*/
SELECT  Audit        = 'A4_CycleSummary',
        TotalCycles  = COUNT(*),
        Members      = COUNT(DISTINCT FormNo),
        BillCycles   = SUM(CASE WHEN PurchaseRef LIKE 'R:%'   THEN 1 ELSE 0 END),
        LegacyCycles = SUM(CASE WHEN PurchaseRef = 'LEGACY'   THEN 1 ELSE 0 END)
FROM    dbo.WowPurchaseCycle;


/*----------------------------------------------------------------------------------------
  A5. (Migration ke BAAD) Kitne members ko abhi fresh benefit milna chahiye -
      yani current cycle me na scratch hua hai na free product claim
----------------------------------------------------------------------------------------*/
SELECT  Audit = 'A5_EligibleNow',
        Members = COUNT(*)
FROM (
        SELECT c.FormNo, c.CycleId,
               rn = ROW_NUMBER() OVER (PARTITION BY c.FormNo ORDER BY c.PurchaseDate DESC, c.CycleId DESC)
        FROM   dbo.WowPurchaseCycle c
     ) cur
WHERE   cur.rn = 1
        AND NOT EXISTS (SELECT 1 FROM dbo.ScratchHistory   s WHERE s.FormNo = cur.FormNo AND s.WowCycleId = cur.CycleId)
        AND NOT EXISTS (SELECT 1 FROM dbo.FreeProductClaim f WHERE f.FormNo = cur.FormNo AND f.WowCycleId = cur.CycleId);


/*----------------------------------------------------------------------------------------
  A6. Test member ka poora picture
----------------------------------------------------------------------------------------*/
SELECT Audit = 'A6_Bills',        r.RId, r.BillNo, r.BillDate, r.BillType, r.KitId
FROM   dbo.repurchincome r WHERE r.FormNo = @FormNo AND r.KitId IN (18, 19)
ORDER BY r.BillDate DESC, r.RId DESC;

SELECT Audit = 'A6_Cycles',       c.* FROM dbo.WowPurchaseCycle c WHERE c.FormNo = @FormNo
ORDER BY c.PurchaseDate DESC, c.CycleId DESC;

SELECT Audit = 'A6_Scratch',      s.Id, s.ProductName, s.ScratchDate, s.CycleId, s.WowCycleId
FROM   dbo.ScratchHistory s WHERE s.FormNo = @FormNo;

SELECT Audit = 'A6_FreeProduct',  f.ClaimId, f.ProductId, f.ClaimStatus, f.CreatedDate, f.CycleId, f.WowCycleId
FROM   dbo.FreeProductClaim f WHERE f.FormNo = @FormNo;

SELECT Audit = 'A6_ClaimOrders',  o.* FROM dbo.ScratchClaimOrder o WHERE o.FormNo = @FormNo;
