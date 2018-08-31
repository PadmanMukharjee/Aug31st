
--exec [dbo].[usp_GetDepositLogForClient_test] 'abcr',4,2018

CREATE PROC [dbo].[usp_GetDepositLogForClient_TEST]

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

--Get distinct values of the PIVOT Column 
SELECT @ColumnName= ISNULL(@ColumnName + ',','') + QUOTENAME(PayerName)
FROM (Select DISTINCT PayerName from Client C
inner join ClientPayer CP
on C.ClientID=CP.ClientID
inner join Payer P
on CP.PayerID=P.PayerID 
Where C.RecordStatus='A' and CP.RecordStatus='A' and C.ClientCode=@ClientCode  ) AS PayerCode

SET @DynamicPivotQuery = N'
SELECT * INTO #CLIENT_DATA
FROM
(
SELECT CONVERT(VARCHAR(10), convert (date,dd.Date), 101) as DepositDate,p.PayerName,CAST(ISNULL(dl.Amount,0) AS VARCHAR(20)) as Amount from Client cl
inner join ClientPayer clp
on cl.ClientID=clp.ClientID
inner join Payer p
on clp.PayerID=p.payerID
inner join DepositLog dl
on clp.ClientPayerID=dl.ClientPayerID
inner join DateDimension dd
on dl.DepositDateID=dd.DateKey
where clp.RecordStatus=''A'' and  p.RecordStatus=''A'' 
and dl.RecordStatus=''A'' and  cl.ClientCode='''+@ClientCode+''' 
and dd.Date BETWEEN CONVERT(VarChar(50), @MonthStartDate, 103) and CONVERT(VarChar(50), @MonthEndDate, 103)
) AS S
PIVOT
(
SUM(Amount)
FOR [PayerName] IN (
'+@ColumnName+')
) AS PVT 

SELECT * INTO #FINAL_DATA FROM (
SELECT TOP 1 ''TEMP'' AS DepositDate,'+@ColumnName+' FROM #CLIENT_DATA 
UNION
SELECT DepositDate,ISNULL('+REPLACE(@ColumnName,',',',''''),ISNULL(')+','''') FROM  #CLIENT_DATA ) A
WHERE A.DepositDate <> ''TEMP''

SELECT * FROM #FINAL_DATA '

--SELECT DepositDate,'+@ColumnName+',ISNULL('+REPLACE(@ColumnName,',',',0)+ISNULL(')+',0) AS [TOTAL] FROM #FINAL_DATA
--UNION 
--SELECT ''TOTAL'',SUM('+REPLACE(@ColumnName,',','),SUM(')+'),SUM('+REPLACE(@ColumnName,',',')+SUM(')+') FROM #FINAL_DATA'

SELECT @DynamicPivotQuery

EXEC sp_executesql @DynamicPivotQuery,N'@MonthStartDate NVARCHar(100), @MonthEndDate NVARCHar(100)',@MonthStartDate =@MonthStartDate, @MonthEndDate =@MonthEndDate

END

