/*
==========================================================================================
  WOW Package - Free Product & Scratch Card ko "per purchase" banane ke liye migration.

  Rule:
    - Ye benefit SIRF WOW Movie package ke liye hai, jiske do kits hain:
          18 = WOW Movie @999 (ReferLink.aspx ka refer link)
          19 = WOW Movie ka doosra kit
      Jab bhi member in me se koi kit purchase karta hai, ek naya purchase cycle ban jaata hai.
      Baaki kits par cycle nahi banti - 13 = FULL TANK CARD @4999,
      4 = ROYAL PACKAGE @9999, 12 = FREE REGISTRATION.
      Ye list App_Code/WowBenefit.cs ke WowKitIds ke saath same rehni chahiye.
    - Free Product aur Scratch Card current cycle ke against hi milte hain.
    - Pichhle cycle ka claim agar nahi kiya to wo LAPSE ho jaata hai - naya purchase karne ke
      baad sirf current cycle ka hi claim kiya ja sakta hai.

  Purana data delete nahi hota - use uske apne cycle par map kar diya jaata hai, isliye
  migration ke baad kisi ko extra (bonus) claim nahi milta.

  Script re-runnable hai (baar baar chala sakte hain).
==========================================================================================
*/

SET NOCOUNT ON;
GO

/*----------------------------------------------------------------------------------------
  1. Purchase cycle master
----------------------------------------------------------------------------------------*/
IF OBJECT_ID('dbo.WowPurchaseCycle', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.WowPurchaseCycle
    (
        CycleId      int IDENTITY(1, 1) NOT NULL CONSTRAINT PK_WowPurchaseCycle PRIMARY KEY,
        FormNo       varchar(50)  NOT NULL,
        -- 'R:<billdate>' = repurchase, 'J:<joining/upgrade date>' = joining kit,
        -- 'LEGACY'       = purchase trace nahi mila (purana FormNo-wise behaviour)
        PurchaseRef  varchar(50)  NOT NULL,
        PurchaseDate datetime     NULL,
        CreatedOn    datetime     NOT NULL CONSTRAINT DF_WowPurchaseCycle_CreatedOn DEFAULT (GETDATE())
    );

    CREATE UNIQUE INDEX UX_WowPurchaseCycle_Form_Ref
        ON dbo.WowPurchaseCycle (FormNo, PurchaseRef);

    PRINT 'Created table WowPurchaseCycle.';
END
GO

/*----------------------------------------------------------------------------------------
  2. Benefit tables par CycleId
----------------------------------------------------------------------------------------*/
IF COL_LENGTH('dbo.ScratchHistory', 'CycleId') IS NULL
BEGIN
    ALTER TABLE dbo.ScratchHistory ADD CycleId int NULL;
    PRINT 'Added ScratchHistory.CycleId.';
END
GO

IF COL_LENGTH('dbo.FreeProductClaim', 'CycleId') IS NULL
BEGIN
    ALTER TABLE dbo.FreeProductClaim ADD CycleId int NULL;
    PRINT 'Added FreeProductClaim.CycleId.';
END
GO

/*----------------------------------------------------------------------------------------
  3. Scratch card claim (paid) order tracking.
     Pehle CheckoutBilling par FormNo-wise check hota tha, jo lifetime block kar deta tha.
     Ab claim order apne cycle ke saath track hota hai.
----------------------------------------------------------------------------------------*/
IF OBJECT_ID('dbo.ScratchClaimOrder', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.ScratchClaimOrder
    (
        OrderId   varchar(30) NOT NULL CONSTRAINT PK_ScratchClaimOrder PRIMARY KEY,
        FormNo    varchar(50) NOT NULL,
        CycleId   int         NOT NULL,
        ProductId varchar(50) NULL,
        Status    varchar(20) NOT NULL CONSTRAINT DF_ScratchClaimOrder_Status DEFAULT ('INITIATED'),
        CreatedOn datetime    NOT NULL CONSTRAINT DF_ScratchClaimOrder_CreatedOn DEFAULT (GETDATE()),
        UpdatedOn datetime    NULL
    );

    CREATE INDEX IX_ScratchClaimOrder_Form_Cycle
        ON dbo.ScratchClaimOrder (FormNo, CycleId, Status);

    PRINT 'Created table ScratchClaimOrder.';
END
GO

/*----------------------------------------------------------------------------------------
  4. Purana "ek FormNo = ek scratch" unique constraint hata do.
     Ab uniqueness (FormNo, CycleId) par hai.
----------------------------------------------------------------------------------------*/
DECLARE @drop nvarchar(max) = N'';

-- unique constraints jinka single key column FormNo hai
SELECT @drop = @drop + N'ALTER TABLE dbo.ScratchHistory DROP CONSTRAINT ' + QUOTENAME(kc.name) + N';' + CHAR(10)
FROM   sys.key_constraints kc
       INNER JOIN sys.indexes i ON i.object_id = kc.parent_object_id AND i.index_id = kc.unique_index_id
WHERE  kc.parent_object_id = OBJECT_ID('dbo.ScratchHistory')
       AND kc.type = 'UQ'
       AND (SELECT COUNT(*) FROM sys.index_columns ic
            WHERE ic.object_id = i.object_id AND ic.index_id = i.index_id AND ic.is_included_column = 0) = 1
       AND EXISTS (SELECT 1 FROM sys.index_columns ic
                   INNER JOIN sys.columns c ON c.object_id = ic.object_id AND c.column_id = ic.column_id
                   WHERE ic.object_id = i.object_id AND ic.index_id = i.index_id AND c.name = 'FormNo');

-- standalone unique indexes jinka single key column FormNo hai
SELECT @drop = @drop + N'DROP INDEX ' + QUOTENAME(i.name) + N' ON dbo.ScratchHistory;' + CHAR(10)
FROM   sys.indexes i
WHERE  i.object_id = OBJECT_ID('dbo.ScratchHistory')
       AND i.is_unique = 1
       AND i.is_primary_key = 0
       AND i.is_unique_constraint = 0
       AND (SELECT COUNT(*) FROM sys.index_columns ic
            WHERE ic.object_id = i.object_id AND ic.index_id = i.index_id AND ic.is_included_column = 0) = 1
       AND EXISTS (SELECT 1 FROM sys.index_columns ic
                   INNER JOIN sys.columns c ON c.object_id = ic.object_id AND c.column_id = ic.column_id
                   WHERE ic.object_id = i.object_id AND ic.index_id = i.index_id AND c.name = 'FormNo');

IF @drop <> N''
BEGIN
    PRINT 'Dropping old FormNo-only uniqueness on ScratchHistory:';
    PRINT @drop;
    EXEC sp_executesql @drop;
END
GO

/*----------------------------------------------------------------------------------------
  5. Backfill - purane rows ko unke current cycle par map karo
----------------------------------------------------------------------------------------*/
IF OBJECT_ID('tempdb..#Members') IS NOT NULL DROP TABLE #Members;

CREATE TABLE #Members (FormNo varchar(50) NOT NULL PRIMARY KEY);

INSERT INTO #Members (FormNo)
SELECT FormNo FROM dbo.ScratchHistory   WHERE FormNo IS NOT NULL AND LTRIM(RTRIM(FormNo)) <> ''
UNION
SELECT FormNo FROM dbo.FreeProductClaim WHERE FormNo IS NOT NULL AND LTRIM(RTRIM(FormNo)) <> ''
UNION
SELECT FormNo FROM dbo.CheckoutBilling  WHERE FormNo IS NOT NULL AND LTRIM(RTRIM(FormNo)) <> '';

-- 5a. har member ka latest WOW purchase -> cycle
IF OBJECT_ID('tempdb..#Latest') IS NOT NULL DROP TABLE #Latest;

SELECT p.FormNo,
       PurchaseRef = p.Src + ':' + CONVERT(varchar(23), p.PurchaseDate, 126),
       p.PurchaseDate
INTO   #Latest
FROM (
        SELECT m.FormNo,
               x.Src,
               x.PurchaseDate,
               rn = ROW_NUMBER() OVER (PARTITION BY m.FormNo ORDER BY x.PurchaseDate DESC)
        FROM   #Members m
               CROSS APPLY (
                    SELECT 'R' AS Src, r.BillDate AS PurchaseDate
                    FROM   dbo.repurchincome r
                    WHERE  r.FormNo = m.FormNo
                           AND r.KitId IN (18, 19)

                    UNION ALL

                    -- M_MemberMaster.KitID number bhi ho sakta hai aur comma separated list bhi
                    SELECT 'J', ISNULL(mm.Upgradedate, mm.Doj)
                    FROM   dbo.M_MemberMaster mm
                    WHERE  mm.FormNo = m.FormNo
                           AND (   ',' + REPLACE(CONVERT(varchar(200), ISNULL(mm.KitID, '')), ' ', '') + ',' LIKE '%,18,%'
                                OR ',' + REPLACE(CONVERT(varchar(200), ISNULL(mm.KitID, '')), ' ', '') + ',' LIKE '%,19,%')
               ) x
        WHERE  x.PurchaseDate IS NOT NULL
     ) p
WHERE p.rn = 1;

INSERT INTO dbo.WowPurchaseCycle (FormNo, PurchaseRef, PurchaseDate)
SELECT l.FormNo, l.PurchaseRef, l.PurchaseDate
FROM   #Latest l
WHERE  NOT EXISTS (SELECT 1 FROM dbo.WowPurchaseCycle c
                   WHERE c.FormNo = l.FormNo AND c.PurchaseRef = l.PurchaseRef);

-- 5b. jinka purchase trace nahi mila unke liye LEGACY cycle
INSERT INTO dbo.WowPurchaseCycle (FormNo, PurchaseRef, PurchaseDate)
SELECT m.FormNo, 'LEGACY', NULL
FROM   #Members m
WHERE  NOT EXISTS (SELECT 1 FROM dbo.WowPurchaseCycle c WHERE c.FormNo = m.FormNo);

-- 5c. current cycle nikaalo (latest purchase, warna LEGACY)
IF OBJECT_ID('tempdb..#Current') IS NOT NULL DROP TABLE #Current;

SELECT m.FormNo,
       CycleId = ISNULL(lc.CycleId, gc.CycleId)
INTO   #Current
FROM   #Members m
       LEFT JOIN #Latest l ON l.FormNo = m.FormNo
       LEFT JOIN dbo.WowPurchaseCycle lc ON lc.FormNo = l.FormNo AND lc.PurchaseRef = l.PurchaseRef
       LEFT JOIN dbo.WowPurchaseCycle gc ON gc.FormNo = m.FormNo AND gc.PurchaseRef = 'LEGACY';

-- 5d. purane benefit rows ko current cycle par map karo (taaki bonus claim na mile)
UPDATE sh
SET    sh.CycleId = c.CycleId
FROM   dbo.ScratchHistory sh
       INNER JOIN #Current c ON c.FormNo = sh.FormNo
WHERE  sh.CycleId IS NULL AND c.CycleId IS NOT NULL;

UPDATE fp
SET    fp.CycleId = c.CycleId
FROM   dbo.FreeProductClaim fp
       INNER JOIN #Current c ON c.FormNo = fp.FormNo
WHERE  fp.CycleId IS NULL AND c.CycleId IS NOT NULL;

-- 5e. CheckoutBilling me jo scratch claims ho chuke hain unhe current cycle par SUCCESS mark karo
INSERT INTO dbo.ScratchClaimOrder (OrderId, FormNo, CycleId, ProductId, Status, CreatedOn, UpdatedOn)
SELECT TOP 100 PERCENT
       OrderId = 'LEGACY-' + CONVERT(varchar(20), c.CycleId),
       c.FormNo,
       c.CycleId,
       NULL,
       'SUCCESS',
       GETDATE(),
       GETDATE()
FROM   #Current c
WHERE  c.CycleId IS NOT NULL
       AND EXISTS (SELECT 1 FROM dbo.CheckoutBilling cb WHERE cb.FormNo = c.FormNo)
       AND NOT EXISTS (SELECT 1 FROM dbo.ScratchClaimOrder o
                       WHERE o.OrderId = 'LEGACY-' + CONVERT(varchar(20), c.CycleId));

DROP TABLE #Current;
DROP TABLE #Latest;
DROP TABLE #Members;
GO

/*----------------------------------------------------------------------------------------
  6. Nayi uniqueness - ek cycle me ek hi scratch / ek hi free product claim
----------------------------------------------------------------------------------------*/
IF NOT EXISTS (SELECT 1 FROM sys.indexes
               WHERE object_id = OBJECT_ID('dbo.ScratchHistory') AND name = 'UX_ScratchHistory_Form_Cycle')
BEGIN
    CREATE UNIQUE INDEX UX_ScratchHistory_Form_Cycle
        ON dbo.ScratchHistory (FormNo, CycleId)
        WHERE CycleId IS NOT NULL;
    PRINT 'Created UX_ScratchHistory_Form_Cycle.';
END
GO

IF NOT EXISTS (SELECT 1 FROM sys.indexes
               WHERE object_id = OBJECT_ID('dbo.FreeProductClaim') AND name = 'UX_FreeProductClaim_Form_Cycle')
BEGIN
    CREATE UNIQUE INDEX UX_FreeProductClaim_Form_Cycle
        ON dbo.FreeProductClaim (FormNo, CycleId)
        WHERE CycleId IS NOT NULL;
    PRINT 'Created UX_FreeProductClaim_Form_Cycle.';
END
GO

PRINT 'WowPurchaseCycle migration complete.';
GO
