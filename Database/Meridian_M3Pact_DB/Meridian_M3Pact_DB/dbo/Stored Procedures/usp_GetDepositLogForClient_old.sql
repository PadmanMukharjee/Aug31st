
CREATE PROC [dbo].[usp_GetDepositLogForClient_old]

@ClientCode nvarchar(20),
@Month INT,
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
SELECT *
FROM
(
SELECT CONVERT(VARCHAR(10), convert (date,dl.DepositDate), 101) as DepositDate,p.PayerName,dl.Amount from Client cl
inner join ClientPayer clp
on cl.ClientID=clp.ClientID
inner join Payer p
on clp.PayerID=p.payerID
inner join DepositLog dl
on clp.ClientPayerID=dl.ClientPayerID
where clp.RecordStatus=''A'' and  p.RecordStatus=''A'' 
and dl.RecordStatus=''A'' and  cl.ClientCode='''+@ClientCode+''' 
and dl.DepositDate BETWEEN CONVERT(VarChar(50), @MonthStartDate, 103) and CONVERT(VarChar(50), @MonthEndDate, 103)
) AS S
PIVOT
(
SUM(Amount)
FOR [PayerName] IN (
'+@ColumnName+')
) AS PVT '


EXEC sp_executesql @DynamicPivotQuery,N'@MonthStartDate NVARCHar(100), @MonthEndDate NVARCHar(100)',@MonthStartDate =@MonthStartDate, @MonthEndDate =@MonthEndDate

END
