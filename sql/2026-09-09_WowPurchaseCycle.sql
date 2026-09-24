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
    - Pichhle cycle ka claim agar nahi kiya to wo LAPSE ho jaata hai.

  Purchase ka source sirf repurchincome hai - joining ka bill bhi wahin hota hai
  (BillType = 'J'), isliye M_MemberMaster alag se nahi dekha jaata (warna ek hi joining
  do baar count hoti thi). PurchaseRef = 'R:' + repurchincome.RId.

  Purana data delete nahi hota - har purani row apni date ke hisaab se us cycle par map
  hoti hai jo us waqt current tha, isliye migration ke baad kisi ko na extra claim milta
  hai aur na hi kisi ka banta hua benefit chhinta hai.

  Column ka naam WowCycleId hai, CycleId NAHI - ScratchHistory aur FreeProductClaim me
  CycleId naam ka column pehle se maujood hai jiska matlab alag hai (MLM session cycle).

  Script re-runnable aur self-healing hai - pehle chal chuki galat version ko khud saaf
  kar leta hai (Step 0), isliye Reset_WowBenefitCycle.sql chalana zaruri nahi.
==========================================================================================
*/

SET NOCOUNT ON;
GO

/*----------------------------------------------------------------------------------------
  0. Pehle chal chuki purani version ki safai

     0a. ScratchClaimOrder purane run me CycleId naam ke column ke saath bana tha -
         rename kar do (data safe rehta hai).
----------------------------------------------------------------------------------------*/
IF OBJECT_ID('dbo.ScratchClaimOrder', 'U') IS NOT NULL
   AND COL_LENGTH('dbo.ScratchClaimOrder', 'WowCycleId') IS NULL
   AND COL_LENGTH('dbo.ScratchClaimOrder', 'CycleId') IS NOT NULL
BEGIN
    EXEC sp_rename 'dbo.ScratchClaimOrder.CycleId', 'WowCycleId', 'COLUMN';
    PRINT 'Renamed ScratchClaimOrder.CycleId -> WowCycleId.';
END
GO

/*  0b. Purane format ke cycles hata do.
        Naya format: 'R:<RId>' (colon ke baad sirf digits) ya 'LEGACY'.
        Purana format tha 'J:2026-06-20T15:02:08.620' / 'R:2026-09-07T00:00:00'.        */
IF OBJECT_ID('dbo.WowPurchaseCycle', 'U') IS NOT NULL
BEGIN
    DECLARE @stale int;

    SELECT @stale = COUNT(*)
    FROM   dbo.WowPurchaseCycle
    WHERE  PurchaseRef LIKE 'J:%' OR PurchaseRef LIKE 'R:%[^0-9]%';

    IF @stale > 0
    BEGIN
        -- un par point kar rahi benefit rows ko chhoda do, backfill dobara sahi map karega
        IF COL_LENGTH('dbo.ScratchHistory', 'WowCycleId') IS NOT NULL
            UPDATE sh
            SET    sh.WowCycleId = NULL
            FROM   dbo.ScratchHistory sh
                   INNER JOIN dbo.WowPurchaseCycle c ON c.CycleId = sh.WowCycleId
            WHERE  c.PurchaseRef LIKE 'J:%' OR c.PurchaseRef LIKE 'R:%[^0-9]%';

        IF COL_LENGTH('dbo.FreeProductClaim', 'WowCycleId') IS NOT NULL
            UPDATE fp
            SET    fp.WowCycleId = NULL
            FROM   dbo.FreeProductClaim fp
                   INNER JOIN dbo.WowPurchaseCycle c ON c.CycleId = fp.WowCycleId
            WHERE  c.PurchaseRef LIKE 'J:%' OR c.PurchaseRef LIKE 'R:%[^0-9]%';

        IF OBJECT_ID('dbo.ScratchClaimOrder', 'U') IS NOT NULL
            DELETE o
            FROM   dbo.ScratchClaimOrder o
                   INNER JOIN dbo.WowPurchaseCycle c ON c.CycleId = o.WowCycleId
            WHERE  o.OrderId LIKE 'LEGACY-%'
                   AND (c.PurchaseRef LIKE 'J:%' OR c.PurchaseRef LIKE 'R:%[^0-9]%');

        DELETE FROM dbo.WowPurchaseCycle
        WHERE  PurchaseRef LIKE 'J:%' OR PurchaseRef LIKE 'R:%[^0-9]%';

        PRINT 'Removed ' + CONVERT(varchar(20), @stale) + ' stale cycles from earlier run.';
    END
END
GO

/*  0c. Jo benefit rows ab kisi maujood cycle ko point nahi karti, unhe bhi chhoda do  */
IF OBJECT_ID('dbo.WowPurchaseCycle', 'U') IS NOT NULL
   AND COL_LENGTH('dbo.ScratchHistory', 'WowCycleId') IS NOT NULL
BEGIN
    UPDATE dbo.ScratchHistory
    SET    WowCycleId = NULL
    WHERE  WowCycleId IS NOT NULL
           AND NOT EXISTS (SELECT 1 FROM dbo.WowPurchaseCycle c WHERE c.CycleId = WowCycleId);
END

IF OBJECT_ID('dbo.WowPurchaseCycle', 'U') IS NOT NULL
   AND COL_LENGTH('dbo.FreeProductClaim', 'WowCycleId') IS NOT NULL
BEGIN
    UPDATE dbo.FreeProductClaim
    SET    WowCycleId = NULL
    WHERE  WowCycleId IS NOT NULL
           AND NOT EXISTS (SELECT 1 FROM dbo.WowPurchaseCycle c WHERE c.CycleId = WowCycleId);
END
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
        -- 'R:<repurchincome.RId>' = ek bill, 'LEGACY' = purchase trace nahi mila
        PurchaseRef  varchar(50)  NOT NULL,
        BillRId      int          NULL,   -- repurchincome.RId
        BillNo       varchar(50)  NULL,   -- repurchincome.BillNo - report me yahi order no dikhta hai
        PurchaseDate datetime     NULL,
        CreatedOn    datetime     NOT NULL CONSTRAINT DF_WowPurchaseCycle_CreatedOn DEFAULT (GETDATE())
    );

    CREATE UNIQUE INDEX UX_WowPurchaseCycle_Form_Ref
        ON dbo.WowPurchaseCycle (FormNo, PurchaseRef);

    CREATE INDEX IX_WowPurchaseCycle_Form_Date
        ON dbo.WowPurchaseCycle (FormNo, PurchaseDate);

    PRINT 'Created table WowPurchaseCycle.';
END
GO

-- pehle se bani table me BillRId / BillNo add karo
IF OBJECT_ID('dbo.WowPurchaseCycle', 'U') IS NOT NULL AND COL_LENGTH('dbo.WowPurchaseCycle', 'BillRId') IS NULL
BEGIN
    ALTER TABLE dbo.WowPurchaseCycle ADD BillRId int NULL;
    PRINT 'Added WowPurchaseCycle.BillRId.';
END
GO

IF OBJECT_ID('dbo.WowPurchaseCycle', 'U') IS NOT NULL AND COL_LENGTH('dbo.WowPurchaseCycle', 'BillNo') IS NULL
BEGIN
    ALTER TABLE dbo.WowPurchaseCycle ADD BillNo varchar(50) NULL;
    PRINT 'Added WowPurchaseCycle.BillNo.';
END
GO

/*----------------------------------------------------------------------------------------
  2. Benefit tables par WowCycleId aur WowBillNo

     WowBillNo denormalized hai - report seedha benefit row se order no padh sake,
     har baar WowPurchaseCycle join kiye bina.
----------------------------------------------------------------------------------------*/
IF COL_LENGTH('dbo.ScratchHistory', 'WowCycleId') IS NULL
BEGIN
    ALTER TABLE dbo.ScratchHistory ADD WowCycleId int NULL;
    PRINT 'Added ScratchHistory.WowCycleId.';
END
GO

IF COL_LENGTH('dbo.ScratchHistory', 'WowBillNo') IS NULL
BEGIN
    ALTER TABLE dbo.ScratchHistory ADD WowBillNo varchar(50) NULL;
    PRINT 'Added ScratchHistory.WowBillNo.';
END
GO

IF COL_LENGTH('dbo.FreeProductClaim', 'WowCycleId') IS NULL
BEGIN
    ALTER TABLE dbo.FreeProductClaim ADD WowCycleId int NULL;
    PRINT 'Added FreeProductClaim.WowCycleId.';
END
GO

IF COL_LENGTH('dbo.FreeProductClaim', 'WowBillNo') IS NULL
BEGIN
    ALTER TABLE dbo.FreeProductClaim ADD WowBillNo varchar(50) NULL;
    PRINT 'Added FreeProductClaim.WowBillNo.';
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
        OrderId    varchar(30) NOT NULL CONSTRAINT PK_ScratchClaimOrder PRIMARY KEY,
        FormNo     varchar(50) NOT NULL,
        WowCycleId int         NOT NULL,
        WowBillNo  varchar(50) NULL,   -- kis package order ke against claim hua
        ProductId  varchar(50) NULL,
        Status     varchar(20) NOT NULL CONSTRAINT DF_ScratchClaimOrder_Status DEFAULT ('INITIATED'),
        CreatedOn  datetime    NOT NULL CONSTRAINT DF_ScratchClaimOrder_CreatedOn DEFAULT (GETDATE()),
        UpdatedOn  datetime    NULL
    );

    CREATE INDEX IX_ScratchClaimOrder_Form_Cycle
        ON dbo.ScratchClaimOrder (FormNo, WowCycleId, Status);

    PRINT 'Created table ScratchClaimOrder.';
END
GO

IF OBJECT_ID('dbo.ScratchClaimOrder', 'U') IS NOT NULL AND COL_LENGTH('dbo.ScratchClaimOrder', 'WowBillNo') IS NULL
BEGIN
    ALTER TABLE dbo.ScratchClaimOrder ADD WowBillNo varchar(50) NULL;
    PRINT 'Added ScratchClaimOrder.WowBillNo.';
END
GO

/*----------------------------------------------------------------------------------------
  4. Purana "ek FormNo = ek scratch" unique constraint hata do.
     Ab uniqueness (FormNo, WowCycleId) par hai.
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
  5. Cycles banao - har WOW bill ka apna cycle (sirf latest nahi, poori history)
----------------------------------------------------------------------------------------*/
INSERT INTO dbo.WowPurchaseCycle (FormNo, PurchaseRef, BillRId, BillNo, PurchaseDate)
SELECT r.FormNo,
       'R:' + CONVERT(varchar(20), r.RId),
       r.RId,
       CONVERT(varchar(50), r.BillNo),
       r.BillDate
FROM   dbo.repurchincome r
WHERE  r.KitId IN (18, 19)
       AND r.FormNo IS NOT NULL
       AND LTRIM(RTRIM(CONVERT(varchar(50), r.FormNo))) <> ''
       AND NOT EXISTS (SELECT 1 FROM dbo.WowPurchaseCycle c
                       WHERE c.FormNo = r.FormNo
                             AND c.PurchaseRef = 'R:' + CONVERT(varchar(20), r.RId));

-- pehle se bane cycles (jinme BillRId/BillNo khali tha) ko bhar do
UPDATE c
SET    c.BillRId = r.RId,
       c.BillNo  = CONVERT(varchar(50), r.BillNo)
FROM   dbo.WowPurchaseCycle c
       INNER JOIN dbo.repurchincome r
               ON r.RId = CONVERT(int, SUBSTRING(c.PurchaseRef, 3, 20))
WHERE  c.PurchaseRef LIKE 'R:%'
       AND (c.BillNo IS NULL OR c.BillRId IS NULL);

PRINT 'Purchase cycles ready.';
GO

/*----------------------------------------------------------------------------------------
  6. Jinke paas benefit row hai par koi WOW bill nahi mila - unke liye LEGACY cycle
----------------------------------------------------------------------------------------*/
INSERT INTO dbo.WowPurchaseCycle (FormNo, PurchaseRef, PurchaseDate)
SELECT m.FormNo, 'LEGACY', NULL
FROM (
        SELECT FormNo FROM dbo.ScratchHistory   WHERE FormNo IS NOT NULL AND LTRIM(RTRIM(FormNo)) <> ''
        UNION
        SELECT FormNo FROM dbo.FreeProductClaim WHERE FormNo IS NOT NULL AND LTRIM(RTRIM(FormNo)) <> ''
     ) m
WHERE NOT EXISTS (SELECT 1 FROM dbo.WowPurchaseCycle c WHERE c.FormNo = m.FormNo);
GO

/*----------------------------------------------------------------------------------------
  7. Backfill - har purani row apni date wale cycle par map karo

     Pass 1: us waqt ka current cycle (latest purchase jo row ki date se pehle ya barabar ho)
     Pass 2: jo bach gayi (date NULL, ya sab purchases row ke baad ke) -> earliest cycle
----------------------------------------------------------------------------------------*/
-- 7a. ScratchHistory - ScratchDate ke hisaab se
UPDATE sh
SET    sh.WowCycleId = ca.CycleId
FROM   dbo.ScratchHistory sh
       CROSS APPLY (
            SELECT TOP 1 c.CycleId
            FROM   dbo.WowPurchaseCycle c
            WHERE  c.FormNo = sh.FormNo
                   AND (c.PurchaseDate IS NULL OR c.PurchaseDate <= sh.ScratchDate)
            ORDER BY c.PurchaseDate DESC, c.CycleId DESC
       ) ca
WHERE  sh.WowCycleId IS NULL;

UPDATE sh
SET    sh.WowCycleId = ca.CycleId
FROM   dbo.ScratchHistory sh
       CROSS APPLY (
            SELECT TOP 1 c.CycleId
            FROM   dbo.WowPurchaseCycle c
            WHERE  c.FormNo = sh.FormNo
            ORDER BY c.PurchaseDate ASC, c.CycleId ASC
       ) ca
WHERE  sh.WowCycleId IS NULL;

-- 7b. FreeProductClaim - CreatedDate ke hisaab se
UPDATE fp
SET    fp.WowCycleId = ca.CycleId
FROM   dbo.FreeProductClaim fp
       CROSS APPLY (
            SELECT TOP 1 c.CycleId
            FROM   dbo.WowPurchaseCycle c
            WHERE  c.FormNo = fp.FormNo
                   AND (c.PurchaseDate IS NULL OR c.PurchaseDate <= fp.CreatedDate)
            ORDER BY c.PurchaseDate DESC, c.CycleId DESC
       ) ca
WHERE  fp.WowCycleId IS NULL;

UPDATE fp
SET    fp.WowCycleId = ca.CycleId
FROM   dbo.FreeProductClaim fp
       CROSS APPLY (
            SELECT TOP 1 c.CycleId
            FROM   dbo.WowPurchaseCycle c
            WHERE  c.FormNo = fp.FormNo
            ORDER BY c.PurchaseDate ASC, c.CycleId ASC
       ) ca
WHERE  fp.WowCycleId IS NULL;

-- 7c. WowBillNo ko cycle se sync karo (report ke liye denormalized copy)
UPDATE sh
SET    sh.WowBillNo = c.BillNo
FROM   dbo.ScratchHistory sh
       INNER JOIN dbo.WowPurchaseCycle c ON c.CycleId = sh.WowCycleId
WHERE  ISNULL(sh.WowBillNo, '') <> ISNULL(c.BillNo, '');

UPDATE fp
SET    fp.WowBillNo = c.BillNo
FROM   dbo.FreeProductClaim fp
       INNER JOIN dbo.WowPurchaseCycle c ON c.CycleId = fp.WowCycleId
WHERE  ISNULL(fp.WowBillNo, '') <> ISNULL(c.BillNo, '');

UPDATE o
SET    o.WowBillNo = c.BillNo
FROM   dbo.ScratchClaimOrder o
       INNER JOIN dbo.WowPurchaseCycle c ON c.CycleId = o.WowCycleId
WHERE  ISNULL(o.WowBillNo, '') <> ISNULL(c.BillNo, '');

PRINT 'Backfill done.';
GO

/*----------------------------------------------------------------------------------------
  8. CheckoutBilling me jo scratch claims paid ho chuke hain, unhe unke scratch wale
     cycle par SUCCESS mark karo (claim hamesha scratch ke baad hota hai)
----------------------------------------------------------------------------------------*/
INSERT INTO dbo.ScratchClaimOrder (OrderId, FormNo, WowCycleId, WowBillNo, ProductId, Status, CreatedOn, UpdatedOn)
SELECT 'LEGACY-' + CONVERT(varchar(20), sh.Id),
       sh.FormNo,
       sh.WowCycleId,
       sh.WowBillNo,
       CONVERT(varchar(50), sh.ProductId),
       'SUCCESS',
       ISNULL(sh.ScratchDate, GETDATE()),
       GETDATE()
FROM   dbo.ScratchHistory sh
WHERE  sh.WowCycleId IS NOT NULL
       AND EXISTS (SELECT 1 FROM dbo.CheckoutBilling cb WHERE cb.FormNo = sh.FormNo)
       AND NOT EXISTS (SELECT 1 FROM dbo.ScratchClaimOrder o
                       WHERE o.OrderId = 'LEGACY-' + CONVERT(varchar(20), sh.Id));

PRINT 'Legacy claim orders seeded.';
GO

/*----------------------------------------------------------------------------------------
  9. Index - ek cycle me ek hi scratch / ek hi free product claim.

     Agar purane data me duplicate (FormNo, WowCycleId) mil gaye to unique index nahi ban
     sakta; aise me non-unique index banta hai aur warning print hoti hai. Duplicates
     dekhne ke liye Check_BenefitDataAudit.sql ka A7 chalaiye. Duplicates saaf karne ke
     baad ye script dobara chalayenge to unique index ban jayega.
----------------------------------------------------------------------------------------*/
IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE object_id = OBJECT_ID('dbo.ScratchHistory')
                                               AND name IN ('UX_ScratchHistory_Form_WowCycle',
                                                            'IX_ScratchHistory_Form_WowCycle'))
BEGIN
    IF EXISTS (SELECT 1 FROM dbo.ScratchHistory
               WHERE WowCycleId IS NOT NULL
               GROUP BY FormNo, WowCycleId HAVING COUNT(*) > 1)
    BEGIN
        PRINT '*** WARNING: ScratchHistory me duplicate (FormNo, WowCycleId) rows hain.';
        PRINT '*** Non-unique index bana raha hoon. Duplicates ke liye audit ka A7 dekhiye.';
        CREATE INDEX IX_ScratchHistory_Form_WowCycle
            ON dbo.ScratchHistory (FormNo, WowCycleId);
    END
    ELSE
    BEGIN
        CREATE UNIQUE INDEX UX_ScratchHistory_Form_WowCycle
            ON dbo.ScratchHistory (FormNo, WowCycleId)
            WHERE WowCycleId IS NOT NULL;
        PRINT 'Created UX_ScratchHistory_Form_WowCycle.';
    END
END
GO

IF NOT EXISTS (SELECT 1 FROM sys.indexes WHERE object_id = OBJECT_ID('dbo.FreeProductClaim')
                                               AND name IN ('UX_FreeProductClaim_Form_WowCycle',
                                                            'IX_FreeProductClaim_Form_WowCycle'))
BEGIN
    IF EXISTS (SELECT 1 FROM dbo.FreeProductClaim
               WHERE WowCycleId IS NOT NULL
               GROUP BY FormNo, WowCycleId HAVING COUNT(*) > 1)
    BEGIN
        PRINT '*** WARNING: FreeProductClaim me duplicate (FormNo, WowCycleId) rows hain.';
        PRINT '*** Non-unique index bana raha hoon. Duplicates ke liye audit ka A7 dekhiye.';
        CREATE INDEX IX_FreeProductClaim_Form_WowCycle
            ON dbo.FreeProductClaim (FormNo, WowCycleId);
    END
    ELSE
    BEGIN
        CREATE UNIQUE INDEX UX_FreeProductClaim_Form_WowCycle
            ON dbo.FreeProductClaim (FormNo, WowCycleId)
            WHERE WowCycleId IS NOT NULL;
        PRINT 'Created UX_FreeProductClaim_Form_WowCycle.';
    END
END
GO

/*----------------------------------------------------------------------------------------
  10. Report view - kis order (BillNo) ke against kya mila, purchase order me

      Example:
          SELECT * FROM dbo.vw_WowBenefitReport
          WHERE FormNo = '7555462' ORDER BY PurchaseNo;
----------------------------------------------------------------------------------------*/
IF OBJECT_ID('dbo.vw_WowBenefitReport', 'V') IS NOT NULL
    DROP VIEW dbo.vw_WowBenefitReport;
GO

CREATE VIEW dbo.vw_WowBenefitReport
AS
SELECT  c.FormNo,
        PurchaseNo   = ROW_NUMBER() OVER (PARTITION BY c.FormNo ORDER BY c.PurchaseDate, c.CycleId),
        OrderNo      = c.BillNo,
        c.PurchaseDate,
        c.CycleId,
        CycleStatus  = CASE WHEN c.CycleId = (SELECT TOP 1 c2.CycleId
                                              FROM   dbo.WowPurchaseCycle c2
                                              WHERE  c2.FormNo = c.FormNo
                                              ORDER BY c2.PurchaseDate DESC, c2.CycleId DESC)
                            THEN 'CURRENT' ELSE 'LAPSED' END,

        ScratchStatus = CASE WHEN s.Id IS NULL THEN 'NOT SCRATCHED' ELSE 'SCRATCHED' END,
        ScratchProduct = s.ProductName,
        ScratchPrice   = s.ProductPrice,
        s.ScratchDate,
        ScratchClaim   = CASE WHEN o.OrderId IS NULL THEN 'NOT CLAIMED' ELSE 'CLAIMED' END,
        ScratchDispatch = s.DispStatus,

        FreeProductStatus = CASE WHEN f.ClaimId IS NULL THEN 'NOT CLAIMED' ELSE 'CLAIMED' END,
        FreeProductId     = f.ProductId,
        FreeClaimDate     = f.CreatedDate,
        FreeClaimStatus   = f.ClaimStatus,
        FreeDispatch      = f.DispStatus
FROM    dbo.WowPurchaseCycle c
        LEFT JOIN dbo.ScratchHistory   s ON s.FormNo = c.FormNo AND s.WowCycleId = c.CycleId
        LEFT JOIN dbo.FreeProductClaim f ON f.FormNo = c.FormNo AND f.WowCycleId = c.CycleId
        LEFT JOIN dbo.ScratchClaimOrder o ON o.FormNo = c.FormNo AND o.WowCycleId = c.CycleId
                                         AND UPPER(o.Status) = 'SUCCESS';
GO

PRINT 'Created view vw_WowBenefitReport.';
GO

PRINT 'WowPurchaseCycle migration complete.';
GO
