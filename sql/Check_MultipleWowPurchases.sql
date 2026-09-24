/*
==========================================================================================
  Testing helper - WOW Movie package (KitId 18) ek se zyada baar kisne purchase kiya hai.

  Inhi members par naya rule verify karna hai:
    - latest purchase ka Free Product / Scratch Card milna chahiye
    - pichhle purchase ka unclaimed benefit laps ho jaana chahiye

  Read-only hai, koi data change nahi karta.
==========================================================================================
*/

/*----------------------------------------------------------------------------------------
  Q1. Jinhone KitId 18 ek se zyada baar liya hai (joining + repurchase dono count)
----------------------------------------------------------------------------------------*/
SELECT  p.FormNo,
        mm.IDNo,
        MemberName    = LTRIM(RTRIM(ISNULL(mm.MemFirstName, '') + ' ' + ISNULL(mm.MemLastName, ''))),
        TotalPurchase = COUNT(*),
        JoiningKit    = SUM(CASE WHEN p.Src = 'J' THEN 1 ELSE 0 END),
        Repurchase    = SUM(CASE WHEN p.Src = 'R' THEN 1 ELSE 0 END),
        FirstPurchase = MIN(p.PurchaseDate),
        LastPurchase  = MAX(p.PurchaseDate)
FROM (
        SELECT r.FormNo, PurchaseDate = r.BillDate, Src = 'R'
        FROM   dbo.repurchincome r
        WHERE  r.KitId = 18

        UNION ALL

        SELECT mm2.FormNo, ISNULL(mm2.Upgradedate, mm2.Doj), 'J'
        FROM   dbo.M_MemberMaster mm2
        WHERE  ',' + REPLACE(CONVERT(varchar(200), ISNULL(mm2.KitID, '')), ' ', '') + ',' LIKE '%,18,%'
     ) p
     LEFT JOIN dbo.M_MemberMaster mm ON mm.FormNo = p.FormNo
WHERE  p.PurchaseDate IS NOT NULL
GROUP BY p.FormNo, mm.IDNo, mm.MemFirstName, mm.MemLastName
HAVING COUNT(*) > 1
ORDER BY COUNT(*) DESC, MAX(p.PurchaseDate) DESC;


/*----------------------------------------------------------------------------------------
  Q2. Ek member ka poora purchase timeline (Q1 se FormNo uthakar yahan daaliye)
----------------------------------------------------------------------------------------*/
DECLARE @FormNo varchar(50) = '0';   -- <<< test karne wala FormNo yahan

SELECT  Src          = CASE WHEN p.Src = 'J' THEN 'Joining' ELSE 'Repurchase' END,
        p.PurchaseDate,
        PurchaseRef  = p.Src + ':' + CONVERT(varchar(23), p.PurchaseDate, 126),
        IsCurrent    = CASE WHEN p.PurchaseDate = MAX(p.PurchaseDate) OVER () THEN 'CURRENT' ELSE 'LAPSED' END
FROM (
        SELECT r.FormNo, PurchaseDate = r.BillDate, Src = 'R'
        FROM   dbo.repurchincome r
        WHERE  r.KitId = 18 AND r.FormNo = @FormNo

        UNION ALL

        SELECT mm.FormNo, ISNULL(mm.Upgradedate, mm.Doj), 'J'
        FROM   dbo.M_MemberMaster mm
        WHERE  mm.FormNo = @FormNo
               AND ',' + REPLACE(CONVERT(varchar(200), ISNULL(mm.KitID, '')), ' ', '') + ',' LIKE '%,18,%'
     ) p
WHERE  p.PurchaseDate IS NOT NULL
ORDER BY p.PurchaseDate DESC;


/*----------------------------------------------------------------------------------------
  Q3. Multiple purchase + abhi tak ka benefit status
      (CycleId column tabhi bharega jab migration chal chuka ho)
----------------------------------------------------------------------------------------*/
SELECT  x.FormNo,
        x.TotalPurchase,
        x.LastPurchase,
        ScratchRows      = (SELECT COUNT(*) FROM dbo.ScratchHistory   s WHERE s.FormNo = x.FormNo),
        FreeProductRows  = (SELECT COUNT(*) FROM dbo.FreeProductClaim f WHERE f.FormNo = x.FormNo),
        CycleRows        = (SELECT COUNT(*) FROM dbo.WowPurchaseCycle c WHERE c.FormNo = x.FormNo)
FROM (
        SELECT  p.FormNo,
                TotalPurchase = COUNT(*),
                LastPurchase  = MAX(p.PurchaseDate)
        FROM (
                SELECT r.FormNo, PurchaseDate = r.BillDate, Src = 'R'
                FROM   dbo.repurchincome r
                WHERE  r.KitId = 18

                UNION ALL

                SELECT mm.FormNo, ISNULL(mm.Upgradedate, mm.Doj), 'J'
                FROM   dbo.M_MemberMaster mm
                WHERE  ',' + REPLACE(CONVERT(varchar(200), ISNULL(mm.KitID, '')), ' ', '') + ',' LIKE '%,18,%'
             ) p
        WHERE  p.PurchaseDate IS NOT NULL
        GROUP BY p.FormNo
        HAVING COUNT(*) > 1
     ) x
ORDER BY x.TotalPurchase DESC, x.LastPurchase DESC;
