/*
==========================================================================================
  Testing helper - WOW Movie package (KitId 18 / 19) ek se zyada baar kisne purchase kiya hai.

  Purchase ka source sirf repurchincome hai - joining ka bill bhi wahin hota hai
  (BillType = 'J'), isliye M_MemberMaster alag se nahi dekha jaata.

  Inhi members par naya rule verify karna hai:
    - latest purchase ka Free Product / Scratch Card milna chahiye
    - pichhle purchase ka unclaimed benefit laps ho jaana chahiye

  Read-only hai, koi data change nahi karta.
==========================================================================================
*/

DECLARE @FormNo varchar(50) = '7555462';   -- <<< test karne wala FormNo


/*----------------------------------------------------------------------------------------
  Q1. Jinhone KitId 18 / 19 ek se zyada baar liya hai
----------------------------------------------------------------------------------------*/
SELECT  r.FormNo,
        mm.IDNo,
        MemberName    = LTRIM(RTRIM(ISNULL(mm.MemFirstName, '') + ' ' + ISNULL(mm.MemLastName, ''))),
        TotalPurchase = COUNT(*),
        JoiningBills  = SUM(CASE WHEN r.BillType = 'J' THEN 1 ELSE 0 END),
        OtherBills    = SUM(CASE WHEN r.BillType <> 'J' THEN 1 ELSE 0 END),
        FirstPurchase = MIN(r.BillDate),
        LastPurchase  = MAX(r.BillDate)
FROM    dbo.repurchincome r
        LEFT JOIN dbo.M_MemberMaster mm ON mm.FormNo = r.FormNo
WHERE   r.KitId IN (18, 19)
GROUP BY r.FormNo, mm.IDNo, mm.MemFirstName, mm.MemLastName
HAVING  COUNT(*) > 1
ORDER BY COUNT(*) DESC, MAX(r.BillDate) DESC;


/*----------------------------------------------------------------------------------------
  Q2. Ek member ka poora purchase timeline
----------------------------------------------------------------------------------------*/
SELECT  r.RId,
        r.BillNo,
        r.BillDate,
        r.BillType,
        r.KitId,
        PurchaseRef = 'R:' + CONVERT(varchar(20), r.RId),
        IsCurrent   = CASE WHEN r.RId = (SELECT TOP 1 r2.RId
                                         FROM   dbo.repurchincome r2
                                         WHERE  r2.FormNo = r.FormNo AND r2.KitId IN (18, 19)
                                         ORDER BY r2.BillDate DESC, r2.RId DESC)
                           THEN 'CURRENT' ELSE 'LAPSED' END
FROM    dbo.repurchincome r
WHERE   r.FormNo = @FormNo
        AND r.KitId IN (18, 19)
ORDER BY r.BillDate DESC, r.RId DESC;


/*----------------------------------------------------------------------------------------
  Q3. Member ke cycles aur har cycle ka benefit status
      (migration chalne ke baad hi sahi output dega)
----------------------------------------------------------------------------------------*/
SELECT  c.CycleId,
        c.PurchaseRef,
        c.PurchaseDate,
        IsCurrent    = CASE WHEN c.CycleId = (SELECT TOP 1 c2.CycleId
                                              FROM   dbo.WowPurchaseCycle c2
                                              WHERE  c2.FormNo = c.FormNo
                                              ORDER BY c2.PurchaseDate DESC, c2.CycleId DESC)
                            THEN 'CURRENT' ELSE 'LAPSED' END,
        Scratched    = (SELECT COUNT(*) FROM dbo.ScratchHistory   s WHERE s.FormNo = c.FormNo AND s.WowCycleId = c.CycleId),
        FreeClaimed  = (SELECT COUNT(*) FROM dbo.FreeProductClaim f WHERE f.FormNo = c.FormNo AND f.WowCycleId = c.CycleId),
        ScratchPaid  = (SELECT COUNT(*) FROM dbo.ScratchClaimOrder o WHERE o.FormNo = c.FormNo AND o.WowCycleId = c.CycleId AND UPPER(o.Status) = 'SUCCESS')
FROM    dbo.WowPurchaseCycle c
WHERE   c.FormNo = @FormNo
ORDER BY c.PurchaseDate DESC, c.CycleId DESC;
