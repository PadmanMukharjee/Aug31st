/*
Script Name : UpdateClientTargets
Module_Name : M3Pact

Revision History 
=============
Ver  Date				Who						Comment
1.0  ??					??						Save Client Targets
1.1  16-Aug-2018		Shravani Palla			Update ModifiedBy with UserID
*/

CREATE Procedure [dbo].[UpdateClientTargets]
	@CalendarYear INT,
	@UserID NVARCHAR(255)
AS
BEGIN

	DECLARE @TotalWorkingDays INT;
	SELECT @TotalWorkingDays = SUM(bd.BusinessDays) FROM dbo.BusinessDays bd GROUP BY bd.[Year] HAVING bd.[Year] = @CalendarYear

	UPDATE CT 
	SET CT.Charges = ((CAST( bd.BusinessDays AS FLOAT)/CAST( @TotalWorkingDays AS FLOAT))*ct.AnnualCharges),
	CT.Payments = ((CAST( bd.BusinessDays AS FLOAT)/CAST( @TotalWorkingDays AS FLOAT))*ct.AnnualCharges)*(ct.GrossCollectionRate/100),
	CT.Revenue = CASE WHEN (c.PercentageOfCash = 0 OR c.PercentageOfCash IS NULL) THEN c.FlatFee ELSE (((CAST( bd.BusinessDays AS FLOAT)/CAST( @TotalWorkingDays AS FLOAT))*ct.AnnualCharges)*(ct.GrossCollectionRate/100)) * c.PercentageOfCash/100 END,
	CT.ModifiedDate = GETDATE(),
	CT.ModifiedBy = @UserID
	FROM dbo.Client c
	INNER JOIN dbo.ClientTarget ct ON c.ClientID = ct.ClientID
	INNER JOIN dbo.[Month] m ON ct.MonthID = m.MonthID
	INNER JOIN dbo.BusinessDays bd ON m.MonthID = bd.MonthID
	WHERE c.IsActive ='A' AND ct.RecordStatus ='A' AND ct.CalendarYear = @CalendarYear AND bd.[Year] = @CalendarYear
	 	 
END