




CREATE PROCEDURE [dbo].[usp_GetDepositLogsForClientDate]
(
	@DepositDate date = NULL,
	@ClientCode nvarchar(60) = NULL
)
AS
BEGIN
	DECLARE @DateKey INT;

	SELECT @DateKey = DateKey FROM DateDimension WHERE [Date]=@DepositDate

	
  SELECT 
		 P.PayerCode,
		 P.PayerName,
		 D.Amount
  FROM CLIENT C
  JOIN CLIENTPAYER CP ON CP.ClientID = C.ClientID 
  JOIN PAYER P on P.PayerID=CP.PayerID 
  LEFT JOIN DEPOSITLOG D ON D.ClientPayerID=CP.ClientPayerID
    AND D.RecordStatus = 'A' 
	AND D.DepositDateID = @DateKey
  WHERE C.ClientCode= @ClientCode
		AND C.IsActive='A'
		AND P.RecordStatus!='D'
		AND CP.RecordStatus!='D'
		AND @DepositDate BETWEEN P.StartDate AND P.EndDate
		AND @DepositDate BETWEEN CP.StartDate AND CP.EndDate
		order by P.PayerName
END






