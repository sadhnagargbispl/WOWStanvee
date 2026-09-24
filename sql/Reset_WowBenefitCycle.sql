/*
==========================================================================================
  RESET - pehle chal chuki galat migration ko saaf karta hai.

  Ye sirf UN objects ko hataata hai jo migration ne khud banaye the. In me koi original
  business data nahi hai - sab kuch repurchincome / ScratchHistory / FreeProductClaim se
  dobara derive ho jaata hai.

  !!! IMPORTANT !!!
  Ye script production me live chalne ke BAAD mat chalaiye - ScratchClaimOrder me us waqt
  asli payment orders (INITIATED / SUCCESS) hote hain jo dobara derive nahi ho sakte.
  Abhi (go-live se pehle) chalana bilkul safe hai.

  Chalane ka order:
      1. Reset_WowBenefitCycle.sql   <- ye file
      2. 2026-09-09_WowPurchaseCycle.sql
==========================================================================================
*/

SET NOCOUNT ON;
GO

-- 1. Nayi uniqueness indexes hata do (data dobara map hoga)
IF EXISTS (SELECT 1 FROM sys.indexes
           WHERE object_id = OBJECT_ID('dbo.ScratchHistory') AND name = 'UX_ScratchHistory_Form_WowCycle')
BEGIN
    DROP INDEX UX_ScratchHistory_Form_WowCycle ON dbo.ScratchHistory;
    PRINT 'Dropped UX_ScratchHistory_Form_WowCycle.';
END

IF EXISTS (SELECT 1 FROM sys.indexes
           WHERE object_id = OBJECT_ID('dbo.FreeProductClaim') AND name = 'UX_FreeProductClaim_Form_WowCycle')
BEGIN
    DROP INDEX UX_FreeProductClaim_Form_WowCycle ON dbo.FreeProductClaim;
    PRINT 'Dropped UX_FreeProductClaim_Form_WowCycle.';
END

-- pehle wale galat naam wale indexes (agar bane the)
IF EXISTS (SELECT 1 FROM sys.indexes
           WHERE object_id = OBJECT_ID('dbo.ScratchHistory') AND name = 'UX_ScratchHistory_Form_Cycle')
BEGIN
    DROP INDEX UX_ScratchHistory_Form_Cycle ON dbo.ScratchHistory;
    PRINT 'Dropped UX_ScratchHistory_Form_Cycle (old name).';
END

IF EXISTS (SELECT 1 FROM sys.indexes
           WHERE object_id = OBJECT_ID('dbo.FreeProductClaim') AND name = 'UX_FreeProductClaim_Form_Cycle')
BEGIN
    DROP INDEX UX_FreeProductClaim_Form_Cycle ON dbo.FreeProductClaim;
    PRINT 'Dropped UX_FreeProductClaim_Form_Cycle (old name).';
END
GO

-- 2. WowCycleId ko khali karo taaki backfill dobara sahi se chale
--    (CycleId ko haath nahi lagaya jaa raha - wo original MLM column hai)
IF COL_LENGTH('dbo.ScratchHistory', 'WowCycleId') IS NOT NULL
BEGIN
    UPDATE dbo.ScratchHistory SET WowCycleId = NULL WHERE WowCycleId IS NOT NULL;
    PRINT 'Cleared ScratchHistory.WowCycleId.';
END

IF COL_LENGTH('dbo.FreeProductClaim', 'WowCycleId') IS NOT NULL
BEGIN
    UPDATE dbo.FreeProductClaim SET WowCycleId = NULL WHERE WowCycleId IS NOT NULL;
    PRINT 'Cleared FreeProductClaim.WowCycleId.';
END
GO

-- 3. Migration ki apni tables gira do - migration inhe dobara bana lega
IF OBJECT_ID('dbo.ScratchClaimOrder', 'U') IS NOT NULL
BEGIN
    DROP TABLE dbo.ScratchClaimOrder;
    PRINT 'Dropped ScratchClaimOrder.';
END

IF OBJECT_ID('dbo.WowPurchaseCycle', 'U') IS NOT NULL
BEGIN
    DROP TABLE dbo.WowPurchaseCycle;
    PRINT 'Dropped WowPurchaseCycle.';
END
GO

PRINT 'Reset complete. Ab 2026-09-09_WowPurchaseCycle.sql chalaiye.';
GO
