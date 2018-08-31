/*
Script Name : usp_GetMonthDepositsOfAYearForClient
Module_Name : M3Pact
Created By  : Shravani P
CreatedDate : 16-Jul-2018

Revision History 
=============
Ver  Date				Who						Comment
1.0  16-Jul-2018		Shravani Palla			Get all 12 months deposits in a year for a client
*/

CREATE PROCEDURE [dbo].[usp_GetMonthDepositsOfAYearForClient]
(
	@ClientCode NVARCHAR(60),
	@Year INT
)
AS
BEGIN

	SELECT
		C.ClientID,
		M.MonthID,
		M.[MonthName],
		CT.Payments,
		DMD.TotalDepositAmount,
		DMD.MonthStatus,
		CAST(ROUND((DMD.TotalDepositAmount / CT.Payments ) * 100, 2) AS NUMERIC(36,2)) AS MetPercent
	FROM Client C
	INNER JOIN ClientTarget CT ON CT.ClientID = C.ClientID
	LEFT JOIN [Month] M ON M.MonthID = CT.MonthID  
	LEFT JOIN DepositLogMonthlyDetails DMD ON DMD.MonthId = M.MonthID and CT.CalendarYear = DMD.[Year] AND C.ClientID = DMD.ClientId
	WHERE CT.RecordStatus = 'A' AND C.ClientCode = @ClientCode AND CT.CalendarYear = @Year

END