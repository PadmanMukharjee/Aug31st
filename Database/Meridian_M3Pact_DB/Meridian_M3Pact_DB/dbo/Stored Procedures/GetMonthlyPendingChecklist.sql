CREATE PROCEDURE [dbo].[GetMonthlyPendingChecklist](
 @clientCode VARCHAR(50)
)
AS
BEGIN

SET @clientCode =@clientCode
DECLARE @clientStartDate DATE

SELECT @clientStartDate=  MIN(CLM.EffectiveDate) FROM Client C
JOIN ClientCheckListMap CLM
ON C.ClientID=clm.ClientID
JOIN CheckList  Cl
ON Cl.CheckListID=clm.CheckListID
JOIN checklisttype Clt
ON Clt.CheckListTypeID=cl.CheckListTypeID
WHERE c.ClientCode=@clientCode
AND c.IsActive='A'
AND clt.CheckListTypeCode='MONTH'
AND clm.StartDate<> clm.EndDate

CREATE TABLE #MonthChecklist(ChecklistDate DATE )


DECLARE @sDate DATETIME,
        @eDate DATETIME

SELECT  @sDate = @clientStartDate,
        @eDate = GETDATE()

;WITH cte AS (
  SELECT CONVERT(DATE,LEFT(CONVERT(VARCHAR,@sdate,112),6) + '01') startDate,
         MONTH(@sdate) n
  UNION ALL
  SELECT DATEADD(MONTH,n,CONVERT(DATE,CONVERT(VARCHAR,YEAR(@sdate)) + '0101')) startDate,
        (n+1) n
  FROM cte
  WHERE n < MONTH(@sdate) + DATEDIFF(MONTH,@sdate,@edate)
)

INSERT INTO #MonthChecklist
SELECT  startDate
FROM CTE 
ORDER BY startDate

SELECT * FROM #MonthChecklist
WHERE ChecklistDate NOT IN(
SELECT  CSD.CheckListEffectiveDate from Client C
JOIN ClientCheckListMap CLM
ON C.ClientID=CLM.ClientID
JOIN CheckList CL
ON CL.CheckListID=CLM.CheckListID
JOIN ClientCheckListStatusDetail CSD
ON CLM.ClientCheckListMapID=CSD.ClientCheckListMapID
JOIN CheckListType CLT
ON CLT.CheckListTypeID=CL.CheckListTypeID
WHERE c.ClientCode=@clientCode
AND CSD.ChecklistStatus='C'	
AND CLT.CheckListTypeCode='MONTH')
--AND @clientStartDate<=GETDATE()

DROP TABLE #MonthChecklist

END
