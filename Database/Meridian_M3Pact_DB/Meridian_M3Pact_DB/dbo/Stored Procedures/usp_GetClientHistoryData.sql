/*
Script Name : usp_GetClientHistoryData
Module_Name : M3Pact
Created By  : Shravani P
CreatedDate : 08/14/2018

Revision History 
=============
Ver  Date				Who						Comment
1.0  14-Aug-2018		Shravani Palla			History for Client Data, KPI Setup & Log setup steps
1.1  16-Aug-2018		Shravani Palla			History for Assign User, Monthly Target steps
1.2  17-Aug-2018		Shravani Palla			Fee Structure History changes
1.3  17-Aug-2018		Shravani Palla			Client Standard Unit changes

Sample Execution Script :
=============
EXEC usp_GetClientHistoryData '19230', '2018-08-01', '2018-08-14 23:59'
*/


CREATE PROCEDURE [dbo].[usp_GetClientHistoryData]
	@ClientCode NVARCHAR(60),
	@StartDate DATETIME,
	@EndDate DATETIME
AS
BEGIN

	-- Client Data Step Properties
	DECLARE @Prop_ClientName NVARCHAR(60)= 'Client Name';
	DECLARE @Prop_Acronym NVARCHAR(60)= 'Client Acronym';
	DECLARE @Prop_Site NVARCHAR(60)= 'Site';
	DECLARE @Prop_BusinessUnit NVARCHAR(60)= 'Business Unit';
	DECLARE @Prop_FeeStructure NVARCHAR(60) = 'Fee Structure';
	DECLARE @Prop_FlatFee NVARCHAR(60) = 'Flat Fee';
	DECLARE @Prop_PercentCash NVARCHAR(60) = '% of cash';

	-- KPI Setup Step Properties
	DECLARE @Prop_ClientStandard NVARCHAR(60)= 'Client Standard';

	-- Log Setup Step Properties
	DECLARE @Prop_Payer NVARCHAR(60)= 'Payer';

	-- Assign User Step Properties
	DECLARE @Prop_User NVARCHAR(60)= 'User';

	-- Monthly Target Step Properties
	DECLARE @Prop_AnnualCharges NVARCHAR(60)= 'Annual Charges';
	DECLARE @Prop_GrossCollectionRate NVARCHAR(60)= 'Gross Collection %';
	DECLARE @Prop_Payments NVARCHAR(60)= 'Payments';

	-- Units of M3 Metric Questions
	DECLARE @Unit_Amount NVARCHAR(60)= 'Amount';
	DECLARE @Unit_Days NVARCHAR(60)= 'Days';
	DECLARE @Unit_Percentage NVARCHAR(60)= 'Percentage'; 

	DECLARE @ClientID INT;
	DECLARE @MetricID INT;

	SELECT @ClientID = ClientID FROM Client WHERE ClientCode = @ClientCode
	SELECT @MetricID = CheckListTypeID FROM CheckListType WHERE CheckListTypeCode = 'M3'

	;WITH ClientDataCTE AS
	(
		SELECT
			Client.ModifiedDate,
			Client.ModifiedBy,
			Client.StartTime,
			Client.EndTime,
			Client.[Name],
			Client.Acronym,
			S.SiteName,
			BU.BusinessUnitName,
			Client.FlatFee,
			Client.PercentageOfCash,
			PreviousName = LEAD(Client.[Name]) OVER (PARTITION BY Client.ClientID ORDER BY Client.ModifiedDate DESC),
			PreviousAcronym = LEAD(Client.Acronym) OVER (PARTITION BY Client.ClientID ORDER BY Client.ModifiedDate DESC),
			PreviousSiteName = LEAD(S.SiteName) OVER (PARTITION BY Client.ClientID ORDER BY Client.ModifiedDate DESC),
			PreviousBusinessUnitName = LEAD(BU.BusinessUnitName) OVER (PARTITION BY Client.ClientID ORDER BY Client.ModifiedDate DESC),
			PreviousFlatFee = LEAD(Client.FlatFee) OVER (PARTITION BY Client.ClientID ORDER BY Client.ModifiedDate DESC),
			PreviousPercentageOfCash = LEAD(Client.PercentageOfCash) OVER (PARTITION BY Client.ClientID ORDER BY Client.ModifiedDate DESC)
		FROM Client
		FOR SYSTEM_TIME CONTAINED IN ( @StartDate , @EndDate )
		INNER JOIN [Site] S ON S.SiteID = Client.SiteID
		INNER JOIN [BusinessUnit] BU ON BU.BusinessUnitID = Client.BusinessUnitID
		WHERE ClientID = @ClientID
	)
	, KPICTE AS
	(
		SELECT
			ClientKPIMap.ModifiedDate,
			ClientKPIMap.ModifiedBy,
			ClientKPIMap.StartTime,
			ClientKPIMap.EndTime,
			K.KPIId,
			K.KPIDescription,
			MMQ.M3MetricsUnit,
			ClientKPIMap.[Client Standard] AS ClientStandard,
			PreviousClientStandard = LEAD(ClientKPIMap.[Client Standard]) OVER (PARTITION BY ClientKPIMap.ClientID, ClientKPIMap.KPIID ORDER BY ClientKPIMap.ModifiedDate DESC)
		FROM ClientKPIMap
		FOR SYSTEM_TIME CONTAINED IN ( @StartDate , @EndDate )
		INNER JOIN KPI K ON K.KPIID = ClientKPIMap.KPIID
		INNER JOIN M3MetricsQuestion MMQ ON MMQ.M3MetricsQuestionID = K.Measure
		WHERE ClientID = @ClientID AND K.ChecklistTypeID = @MetricID
	)
	, LogSetupCTE AS
	(
		SELECT
			ClientPayer.ModifiedDate,
			ClientPayer.ModifiedBy,
			ClientPayer.StartTime,
			ClientPayer.EndTime,
			P.PayerName,
			ClientPayer.RecordStatus,
			PreviousRecordStatus = LEAD(ClientPayer.RecordStatus) OVER (PARTITION BY ClientPayer.ClientID, ClientPayer.ClientPayerID ORDER BY ClientPayer.ModifiedDate DESC)
		FROM ClientPayer
		FOR SYSTEM_TIME CONTAINED IN ( @StartDate , @EndDate ) 
		INNER JOIN Payer P ON P.PayerID = ClientPayer.PayerID
		WHERE ClientID = @ClientID
	)
	, AssignUserCTE AS
	(
		SELECT
			UserClientMap.ModifiedDate,
			UserClientMap.ModifiedBy,
			UserClientMap.StartTime,
			UserClientMap.EndTime,
			UL.LastName + ' ' + UL.FirstName + ', ' + R.RoleCode AS UserName,
			UserClientMap.RecordStatus,
			PreviousRecordStatus = LEAD(UserClientMap.RecordStatus) OVER (PARTITION BY UserClientMap.ClientID, UserClientMap.UserClientMapID ORDER BY UserClientMap.ModifiedDate DESC)
		FROM UserClientMap
		FOR SYSTEM_TIME CONTAINED IN ( @StartDate , @EndDate ) 
		INNER JOIN UserLogin UL ON UL.ID = UserClientMap.UserID
		INNER JOIN Roles R ON R.RoleID = UL.RoleID
		WHERE ClientID = @ClientID
	)
	, MonthlyTargetCTE AS
	(
		SELECT
			ClientTarget.ModifiedDate,
			ClientTarget.ModifiedBy,
			ClientTarget.StartTime,
			ClientTarget.EndTime,
			M.MonthCode,
			M.[MonthName],
			ClientTarget.CalendarYear,
			ClientTarget.RecordStatus,
			ClientTarget.AnnualCharges,
			ClientTarget.GrossCollectionRate,
			ClientTarget.Payments,
			PreviousAnnualCharges = LEAD(ClientTarget.AnnualCharges) OVER (PARTITION BY ClientTarget.ClientID, ClientTarget.CalendarYear, M.MonthCode, ClientTarget.RecordStatus ORDER BY ClientTarget.ModifiedDate DESC),
			PreviousGrossCollectionRate = LEAD(ClientTarget.GrossCollectionRate) OVER (PARTITION BY ClientTarget.ClientID, ClientTarget.CalendarYear, M.MonthCode, ClientTarget.RecordStatus ORDER BY ClientTarget.ModifiedDate DESC),
			PreviousPayments = LEAD(ClientTarget.Payments) OVER (PARTITION BY ClientTarget.ClientID, ClientTarget.CalendarYear, M.MonthCode, ClientTarget.RecordStatus ORDER BY ClientTarget.ModifiedDate DESC)
		FROM ClientTarget
		FOR SYSTEM_TIME CONTAINED IN ( @StartDate , @EndDate )
		INNER JOIN [Month] M ON M.MonthID = ClientTarget.MonthID
		WHERE ClientID = @ClientID
	)

	SELECT
		A.ModifiedDate,
		ISNULL(UL.LastName + ' ' + UL.FirstName, 'Admin') AS ModifiedBy,
		A.[ColumnProperty],
		A.ActionName,
		A.OldValue,
		A.NewValue
	FROM 
	(
		-- Client Data History
		SELECT
			StartTime AS ModifiedDate,
			ModifiedBy,
			CASE WHEN ColumnName = @Prop_FlatFee OR ColumnName = @Prop_PercentCash THEN @Prop_FeeStructure
			     ELSE ColumnName END AS [ColumnProperty],
			CASE WHEN OldValue IS NULL THEN 'Created' ELSE 'Updated' END AS ActionName,
			CASE WHEN OldValue IS NULL THEN '--'
			     WHEN ColumnName = @Prop_FlatFee THEN @Prop_FlatFee + ': $' + CONVERT(NVARCHAR(100), CAST(OldValue AS MONEY), -1)
			     WHEN ColumnName = @Prop_PercentCash THEN @Prop_PercentCash + ': ' + OldValue + '%'
			     ELSE OldValue END AS OldValue,
			CASE WHEN ColumnName = @Prop_FlatFee THEN @Prop_FlatFee + ': $' + CONVERT(NVARCHAR(100), CAST(NewValue AS MONEY), -1)
			     WHEN ColumnName = @Prop_PercentCash THEN @Prop_PercentCash + ': ' + NewValue + '%'
			     ELSE NewValue END AS NewValue
		FROM ClientDataCTE
		CROSS APPLY 
		( VALUES 
    		(@Prop_ClientName, CAST(Name AS NVARCHAR(4000)), CAST(PreviousName AS NVARCHAR(4000))),
    		(@Prop_Acronym, CAST(Acronym AS NVARCHAR(4000)), CAST(PreviousAcronym AS NVARCHAR(4000))),
    		(@Prop_Site, CAST(SiteName AS NVARCHAR(4000)), CAST(PreviousSiteName AS NVARCHAR(4000))),
    		(@Prop_BusinessUnit, CAST(BusinessUnitName AS NVARCHAR(4000)), CAST(PreviousBusinessUnitName AS NVARCHAR(4000))),
    		(@Prop_FlatFee, CAST(FlatFee AS NVARCHAR(4000)), CAST(PreviousFlatFee AS NVARCHAR(4000))),
    		(@Prop_PercentCash, CAST(PercentageOfCash AS NVARCHAR(4000)), CAST(PreviousPercentageOfCash AS NVARCHAR(4000)))	
		) CA(ColumnName, NewValue, OldValue)
		WHERE EXISTS(SELECT NewValue EXCEPT SELECT OldValue) AND EndTime <> '9999-12-31 23:59:59'
	
		UNION 

		-- KPI Setup History
		SELECT
			StartTime AS ModifiedDate,
			ModifiedBy,
			ColumnName AS [ColumnProperty],
			CASE WHEN OldValue IS NULL THEN 'Created' ELSE 'Updated' END AS ActionName,
			CASE WHEN OldValue IS NULL THEN '--'
			     WHEN ColumnName = @Prop_ClientStandard AND M3MetricsUnit = @Unit_Days AND OldValue <> '' THEN KPIDescription + ': ' + LEFT(OldValue, CHARINDEX(',', OldValue, 0) - 1) + CONVERT(NVARCHAR(100), SUBSTRING(OldValue, LEN(OldValue) -  CHARINDEX(',',REVERSE(OldValue)) + 2, LEN(OldValue)), -1) + ' days'
			     WHEN ColumnName = @Prop_ClientStandard AND M3MetricsUnit = @Unit_Percentage AND OldValue <> '' THEN KPIDescription + ': ' + LEFT(OldValue, CHARINDEX(',', OldValue, 0) - 1) + CONVERT(NVARCHAR(100), CAST(SUBSTRING(OldValue, LEN(OldValue) -  CHARINDEX(',',REVERSE(OldValue)) + 2, LEN(OldValue)) AS NUMERIC(10,2)) * 100, -1) + '%'
			     WHEN ColumnName = @Prop_ClientStandard AND M3MetricsUnit = @Unit_Amount AND OldValue <> '' THEN KPIDescription + ': ' + LEFT(OldValue, CHARINDEX(',', OldValue, 0) - 1) + ' $'+ CONVERT(NVARCHAR(100), CAST(SUBSTRING(OldValue, LEN(OldValue) -  CHARINDEX(',',REVERSE(OldValue)) + 2, LEN(OldValue)) AS MONEY), -1)
			     ELSE KPIDescription + ': ' + OldValue END AS OldValue,
			CASE WHEN ColumnName = @Prop_ClientStandard AND M3MetricsUnit = @Unit_Days AND NewValue <> '' THEN KPIDescription + ': ' + LEFT(NewValue, CHARINDEX(',', NewValue, 0) - 1) + CONVERT(NVARCHAR(100), SUBSTRING(NewValue, LEN(NewValue) -  CHARINDEX(',',REVERSE(NewValue)) + 2, LEN(NewValue)), -1) + ' days'
			     WHEN ColumnName = @Prop_ClientStandard AND M3MetricsUnit = @Unit_Percentage AND NewValue <> '' THEN KPIDescription + ': ' + LEFT(NewValue, CHARINDEX(',', NewValue, 0) - 1) + CONVERT(NVARCHAR(100), CAST(SUBSTRING(NewValue, LEN(NewValue) -  CHARINDEX(',',REVERSE(NewValue)) + 2, LEN(NewValue)) AS NUMERIC(10,2)) * 100, -1) + '%'
			     WHEN ColumnName = @Prop_ClientStandard AND M3MetricsUnit = @Unit_Amount AND NewValue <> '' THEN KPIDescription + ': ' + LEFT(NewValue, CHARINDEX(',', NewValue, 0) - 1) + ' $'+ CONVERT(NVARCHAR(100), CAST(SUBSTRING(NewValue, LEN(NewValue) -  CHARINDEX(',',REVERSE(NewValue)) + 2, LEN(NewValue)) AS MONEY), -1)
			     ELSE KPIDescription + ': ' + NewValue END AS NewValue
		FROM KPICTE
		CROSS APPLY 
		( VALUES 
    		(@Prop_ClientStandard, CAST(ClientStandard AS NVARCHAR(4000)), CAST(PreviousClientStandard AS NVARCHAR(4000)))
		) CA(ColumnName, NewValue, OldValue)
		WHERE EXISTS(SELECT NewValue EXCEPT SELECT OldValue) AND EndTime <> '9999-12-31 23:59:59'
	
		UNION

		--Log Setup History
		SELECT
			StartTime AS ModifiedDate,
			ModifiedBy,
			ColumnName AS [ColumnProperty],
			CASE WHEN OldValue IS NULL THEN 'Added'
			     WHEN NewValue = 'A' THEN 'Activated'
			     WHEN NewValue = 'I' THEN 'Inactivated'
			     WHEN NewValue = 'D' THEN 'Deleted'
			     ELSE 'Updated' END AS ActionName,
			CASE WHEN OldValue IS NULL THEN '--' ELSE PayerName END AS OldValue,
			CASE WHEN NewValue = 'D' THEN '--' ELSE PayerName END AS NewValue
		FROM LogSetupCTE
		CROSS APPLY 
		( VALUES 
    		(@Prop_Payer, CAST(RecordStatus AS NVARCHAR(4000)), CAST(PreviousRecordStatus AS NVARCHAR(4000)))
		) CA(ColumnName, NewValue, OldValue)
		WHERE EXISTS(SELECT NewValue EXCEPT SELECT OldValue) AND EndTime <> '9999-12-31 23:59:59'

		UNION

		--Assign User History
		SELECT
			StartTime AS ModifiedDate,
			ModifiedBy,
			ColumnName AS [ColumnProperty],
			CASE WHEN OldValue IS NULL THEN 'Assigned' ELSE 'Unassigned' END AS ActionName,
			CASE WHEN OldValue IS NULL THEN '--' ELSE UserName END AS OldValue,
			CASE WHEN NewValue = 'I' THEN '--' ELSE UserName END AS NewValue
		FROM AssignUserCTE
		CROSS APPLY
		( VALUES
    		(@Prop_User, CAST(RecordStatus AS NVARCHAR(4000)), CAST(PreviousRecordStatus AS NVARCHAR(4000)))
		) CA(ColumnName, NewValue, OldValue)
		WHERE EXISTS(SELECT NewValue EXCEPT SELECT OldValue) AND EndTime <> '9999-12-31 23:59:59'
		
		UNION

		--Monthly Target History (Annual Charges & Gross Collection Rate)
		SELECT
			StartTime AS ModifiedDate,
			ModifiedBy,
			ColumnName + ' - ' + CAST(CalendarYear AS NVARCHAR) AS [ColumnProperty],
			CASE WHEN OldValue IS NULL THEN 'Created' ELSE 'Updated' END AS ActionName,
			CASE WHEN ColumnName = @Prop_AnnualCharges THEN ISNULL('$'+ CONVERT(NVARCHAR(100), CAST(OldValue AS MONEY), -1), '--') 
			     WHEN ColumnName = @Prop_GrossCollectionRate THEN ISNULL(OldValue + '%', '--')
			     ELSE ISNULL(OldValue, '--') END AS OldValue,
			CASE WHEN ColumnName = @Prop_AnnualCharges THEN ISNULL('$'+ CONVERT(NVARCHAR(100), CAST(NewValue AS MONEY), -1), '--') 
			     WHEN ColumnName = @Prop_GrossCollectionRate THEN ISNULL(NewValue + '%', '--')
			     ELSE NewValue END AS NewValue
		FROM MonthlyTargetCTE
		CROSS APPLY 
		( VALUES
    		(@Prop_AnnualCharges, CAST(AnnualCharges AS NVARCHAR(4000)), CAST(PreviousAnnualCharges AS NVARCHAR(4000))),
    		(@Prop_GrossCollectionRate, CAST(GrossCollectionRate AS NVARCHAR(4000)), CAST(PreviousGrossCollectionRate AS NVARCHAR(4000)))
		) CA(ColumnName, NewValue, OldValue)
		WHERE EXISTS(SELECT NewValue EXCEPT SELECT OldValue) AND EndTime <> '9999-12-31 23:59:59' AND MonthCode = 'JAN' AND RecordStatus = 'A'

		UNION

		--Monthly Target History (Payments)
		SELECT
			StartTime AS ModifiedDate,
			ModifiedBy,
			ColumnName + ' - ' + CAST(CalendarYear AS NVARCHAR) AS [ColumnProperty],
			CASE WHEN OldValue IS NULL THEN 'Created' ELSE 'Updated' END AS ActionName,
			ISNULL(SUBSTRING([MonthName], 1, 3) + ': ' + '$'+ CONVERT(NVARCHAR(100), CAST(OldValue AS MONEY), -1) , '--') AS OldValue,
			SUBSTRING([MonthName], 1, 3) + ': ' + '$' + CONVERT(NVARCHAR(100), CAST(NewValue AS MONEY), -1) AS NewValue
		FROM MonthlyTargetCTE
		CROSS APPLY 
		( VALUES
    		(@Prop_Payments, CAST(Payments AS NVARCHAR(4000)), CAST(PreviousPayments AS NVARCHAR(4000)))
		) CA(ColumnName, NewValue, OldValue)
		WHERE EXISTS(SELECT NewValue EXCEPT SELECT OldValue) AND EndTime <> '9999-12-31 23:59:59' AND RecordStatus = 'A'

	) A
	LEFT JOIN UserLogin UL ON A.ModifiedBy = UL.UserID
	ORDER BY A.ModifiedDate DESC, A.[ColumnProperty] DESC

END



