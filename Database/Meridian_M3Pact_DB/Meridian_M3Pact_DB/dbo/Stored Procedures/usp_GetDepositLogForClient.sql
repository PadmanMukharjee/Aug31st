

--exec [dbo].[usp_GetDepositLogForClient] 'abcr',4,2018

CREATE PROC [dbo].[usp_GetDepositLogForClient]

@ClientCode nvarchar(20) ,
@Month INT ,
@Year INT 

AS
BEGIN

DECLARE @DynamicPivotQuery AS NVARCHAR(MAX)
DECLARE @ColumnName AS NVARCHAR(MAX)
DECLARE @MonthStartDate DateTime
DECLARE @MonthEndDate DateTime


SET @MonthStartDate=DATEADD(month,@Month-1,DATEADD(year,@Year-1900,0)) /*First of Month*/

SET @MonthEndDate=DATEADD(day,-1,DATEADD(month,@Month,DATEADD(year,@Year-1900,0))) /*Last of Month*/

IF OBJECT_ID('tempdb..#payers') IS NOT NULL 
DROP TABLE #payers

 -- Client Payers
select * into #payers from
(select cp.ClientPayerID from Client c
join ClientPayer cp
on c.ClientID= cp.ClientID
join payer p
on p.PayerID=cp.PayerID
where c.ClientCode=@ClientCode
and cp.RecordStatus <> 'D'
and cp.endDate >= @MonthStartDate ) a


IF OBJECT_ID('tempdb..#dates') IS NOT NULL 
DROP TABLE #dates

-- Clinet's Deposit dates till today
SELECT * INTO #dates 
FROM 
(SELECT DISTINCT d.DepositDateID,dd.Date FROM Client c
JOIN ClientPayer cp
ON C.ClientID= cp.ClientID
JOIN DepositLog d
ON D.ClientPayerID=cp.ClientPayerID
LEFT join DateDimension dd
ON DD.DateKey=d.DepositDateID
JOIN payer p
ON P.PayerID=cp.PayerID
WHERE c.ClientCode=@ClientCode
and  dd.Date BETWEEN @MonthStartDate and  @MonthEndDate)b


-- Payers comma seperated
--Get distinct values for the PIVOT table Columns
SELECT @ColumnName= ISNULL(@ColumnName + ',','') + QUOTENAME(PayerName)
FROM (SELECT DISTINCT PayerName FROM Client C
INNER JOIN ClientPayer CP
ON C.ClientID=CP.ClientID
INNER JOIN Payer P
ON CP.PayerID=P.PayerID 
JOIN #payers tp
ON tp.ClientPayerID=cp.ClientPayerID
WHERE C.IsActive='A' AND CP.RecordStatus<> 'D' and C.ClientCode=@ClientCode
AND cp.endDate >= @MonthStartDate 
) AS PayerCode


DECLARE @COLUMNS NVARCHAR(MAX)
SELECT @COLUMNS = REPLACE(@ColumnName,']','] VARCHAR(20)')
SET @DynamicPivotQuery = N'
SELECT * INTO #temp from
(
SELECT CONVERT(VARCHAR(10), convert (date,d.Date), 101) as DepositDate,
 dl.Amount
 as amount
,pa.PayerName
,p.ClientPayerID
from #payers p
cross join #dates d
left join DepositLog dl
on dl.DepositDateID=d.DepositDateID
and p.ClientPayerID=dl.ClientPayerID
join Clientpayer cp
on p.ClientPayerID=cp.ClientPayerID
and cp.RecordStatus <> ''D''
join Payer pa
on pa.PayerID=cp.PayerID
left join DateDimension dd
on dl.DepositDateID=dd.DateKey
WHERE dl.RecordStatus=''A'' OR dl.RecordStatus IS NULL
)S

update t
set t.Amount = 
CASE WHEN t.DepositDate<cp.EndDate AND t.Amount IS NULL
THEN 0
ELSE t.Amount
END
from #temp t
join ClientPayer cp
on cp.ClientPayerID=t.ClientPayerID

SELECT * INTO #temp1 FROM
(
SELECT t.DepositDate,t.amount,t.PayerName FROM #temp t
)t
PIVOT
(
SUM(Amount)
FOR [PayerName] IN (
'+@ColumnName+')
) AS PVT 

SELECT * INTO #FINAL_DATA FROM (
SELECT TOP 1 ''TEMP'' AS DepositDate,'+@ColumnName+' FROM #temp1
UNION
SELECT DepositDate,ISNULL('+REPLACE(@ColumnName,',',',0.00),ISNULL(')+',0.00) FROM  #temp1 ) A
WHERE A.DepositDate <> ''TEMP''


SELECT * INTO #FINAL_DATA_WITH_SUM FROM(
SELECT DepositDate,ISNULL('+REPLACE(@ColumnName,',',',0)+ISNULL(')+',0) AS [TOTAL], '+@ColumnName+' FROM #FINAL_DATA
UNION 
SELECT ''TOTAL'', SUM('+REPLACE(@ColumnName,',',')+SUM(')+'), SUM('+REPLACE(@ColumnName,',','),SUM(')+') FROM #FINAL_DATA
) F


CREATE TABLE #TEST('+@COLUMNS+')

SELECT '''' AS [Deposit Date],0 AS TOTAL, '+@ColumnName+' FROM #TEST
UNION
select a.DepositDate AS [Deposit Date],b.TOTAL, '+REPLACE(REPLACE(REPLACE(@ColumnName,'[','a.['),']','] AS VARCHAR(20)),''-'')'),'a.','ISNULL(CAST(A.')+' 
from #temp1 a
inner join #FINAL_DATA_WITH_SUM b
on a.depositDate = b.depositDate
UNION
SELECT F.DepositDate AS [Deposit Date],F.TOTAL, '+REPLACE(REPLACE(REPLACE(@ColumnName,'[','F.['),']','] AS VARCHAR(20)),''-'')'),'F.','ISNULL(CAST(F.')+'
FROM #FINAL_DATA_WITH_SUM F
WHERE F.DepositDate=''TOTAL''
'

--select @DynamicPivotQuery
EXEC sp_executesql @DynamicPivotQuery

END



