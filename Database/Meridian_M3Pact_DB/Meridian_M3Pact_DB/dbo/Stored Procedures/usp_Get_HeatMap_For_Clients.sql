/*
Script Name : usp_Get_HeatMap_For_Clients
Module_Name : M3Pact
Created By  : Shravani P
CreatedDate : 06/22/2018

Revision History 
=============
Ver  Date				Who						Comment
1.0  22-Jun-2018		Shravani Palla			Get Heat Map Data for clients
1.1  25-Jun-2018		Shravani Palla			Get Heat Map Data for all the clients
1.2  25-Jun-2018		Shravani Palla			Sort by Client Name
1.3  07-Jul-2018		Shravani Palla			Heat Map Data Changes for M3 Metrics (to consider daily irrespective measure type)
1.4  01-Aug-2018		Shravani Palla			LTM Changes and Actual & Standard values of Metric data.
1.5  01-Aug-2018		Shravani Palla			Added condition for LTM

Sample Execution Script :
=============
EXEC usp_Get_HeatMap_For_Clients '466271',NULL,NULL,NULL
*/

CREATE PROCEDURE [dbo].[usp_Get_HeatMap_For_Clients]
(
	@EmployeeID NVARCHAR(255),
	@BusinessUnitCode NVARCHAR(255) = NULL,
	@SystemCode NVARCHAR(255) = NULL,
	@SpecialtyCode NVARCHAR(255) = NULL
)
AS
BEGIN

	DECLARE @WeeklyChecklistID INT;
	DECLARE @MonthlyChecklistID INT;
	DECLARE @MetricsID INT;
	DECLARE @AdminRoleID INT;
	DECLARE @IsAdmin BIT;
	DECLARE @MonthID INT;

	SELECT @AdminRoleID = RoleID FROM Roles WHERE RoleCode = 'Admin'
	SELECT @WeeklyChecklistID = ChecklistTypeID FROM CheckListType WHERE CheckListTypeCode = 'WEEK'
	SELECT @MonthlyChecklistID = ChecklistTypeID FROM CheckListType WHERE CheckListTypeCode = 'MONTH'
	SELECT @MetricsID = ChecklistTypeID FROM CheckListType WHERE CheckListTypeCode = 'M3'
	SELECT @MonthID = MonthID FROM [Month] WHERE [MonthName] = DATENAME(MONTH, GETDATE())

	IF EXISTS(SELECT TOP 1 1 FROM UserLogin WHERE UserID = @EmployeeID AND RoleID = @AdminRoleID)
		BEGIN
			SET @IsAdmin = 1
		END
	ELSE
		BEGIN
			SET @IsAdmin = 0
		END

	IF OBJECT_ID('tempdb..#Clients') IS NOT NULL
	BEGIN
		DROP TABLE #Clients
	END

	CREATE TABLE #Clients 
	(	
		ClientID INT, 
		ClientCode NVARCHAR(60),
		ClientName NVARCHAR(100),
		SiteAcronym NVARCHAR(255),
		BusinessUnitCode NVARCHAR(255),
		SpecialityName NVARCHAR(255),
		SystemCode NVARCHAR(255)
	)

	IF(@IsAdmin = 1)
		BEGIN

			INSERT INTO #Clients
			SELECT DISTINCT C.ClientID,
							C.ClientCode,
							C.Name AS ClientName,
							S.SiteCode AS SiteAcronym,
							BU.BusinessUnitCode,
							SP.SpecialityName,
							SY.SystemCode
			FROM Client C
			LEFT JOIN [Site] S ON S.SiteID = C.SiteID
			LEFT JOIN Speciality SP ON SP.SpecialityID = C.SpecialityID
			LEFT JOIN BusinessUnit BU ON BU.BusinessUnitID = C.BusinessUnitID
			LEFT JOIN [System] SY ON SY.SystemID = C.SystemID
			WHERE C.IsActive = 'A' AND SP.RecordStatus = 'A' AND SY.RecordStatus = 'A' AND BU.RecordStatus = 'A'
				AND SP.SpecialityCode = CASE WHEN @SpecialtyCode IS NOT NULL THEN @SpecialtyCode ELSE SP.SpecialityCode END
				AND BU.BusinessUnitCode = CASE WHEN @BusinessUnitCode IS NOT NULL THEN @BusinessUnitCode ELSE BU.BusinessUnitCode END
				AND SY.SystemCode = CASE WHEN @SystemCode IS NOT NULL THEN @SystemCode ELSE SY.SystemCode END

		END
	ELSE
		BEGIN

			INSERT INTO #Clients
			SELECT DISTINCT C.ClientID,
							C.ClientCode,
							C.Name AS ClientName,
							S.SiteCode AS SiteAcronym,
							BU.BusinessUnitCode,
							SP.SpecialityName,
							SY.SystemCode
			FROM UserClientMap UCM
			INNER JOIN UserLogin UL ON UCM.UserId = UL.Id
			INNER JOIN Client C ON UCM.ClientId = C.ClientId
			LEFT JOIN [Site] S ON S.SiteID = C.SiteID
			LEFT JOIN Speciality SP ON SP.SpecialityID = C.SpecialityID
			LEFT JOIN BusinessUnit BU ON BU.BusinessUnitID = C.BusinessUnitID
			LEFT JOIN [System] SY ON SY.SystemID = C.SystemID
			WHERE C.IsActive = 'A' AND UCM.RecordStatus = 'A' AND UL.UserId = @EmployeeID AND SP.RecordStatus = 'A' AND SY.RecordStatus = 'A' AND BU.RecordStatus = 'A'
					AND SP.SpecialityCode = CASE WHEN @SpecialtyCode IS NOT NULL THEN @SpecialtyCode ELSE SP.SpecialityCode END
					AND BU.BusinessUnitCode = CASE WHEN @BusinessUnitCode IS NOT NULL THEN @BusinessUnitCode ELSE BU.BusinessUnitCode END
					AND SY.SystemCode = CASE WHEN @SystemCode IS NOT NULL THEN @SystemCode ELSE SY.SystemCode END

		END

	IF OBJECT_ID('tempdb..#CLIENT_RISK_SCORE') IS NOT NULL
	BEGIN
		DROP TABLE #CLIENT_RISK_SCORE
	END

	SELECT	C.ClientID,
			C.ClientCode,
			C.ClientName,
			C.SiteAcronym,
			C.BusinessUnitCode,
			C.SpecialityName,
			C.SystemCode,
			CHMP.EffectiveTime,
			CHMP.M3DailyDate,
			CHMP.ChecklistWeeklyDate,
			CHMP.ChecklistMonthlyDate,
			CHMP.RiskScore,
			DENSE_RANK() OVER(PARTITION BY CHMP.CLIENTID ORDER BY CHMP.ClientHeatMapRiskID DESC) AS ROW_NUM
	INTO #CLIENT_RISK_SCORE
	FROM #Clients C
	LEFT JOIN ClientHeatMapRisk CHMP ON C.ClientID = CHMP.ClientID

	IF OBJECT_ID('tempdb..#ClientLTM') IS NOT NULL
	BEGIN
		DROP TABLE #ClientLTM
	END

	;WITH CTE AS
	(
		SELECT
			ClientId,
			TotalDepositAmount,
			MonthId,
			[Year],
			ROW_NUMBER() OVER (PARTITION BY ClientId ORDER BY ClientId, monthId desc) AS RowNum
		FROM DepositLogMonthlyDetails 
		WHERE NOT(MonthId = @MonthID AND [Year] = YEAR(GETDATE())) AND TotalDepositAmount IS NOT NULL
	)
	SELECT ClientId, SUM(TotalDepositAmount) AS TotalDepositAmount INTO #ClientLTM FROM CTE WHERE RowNum <= 12 GROUP BY ClientId HAVING COUNT(ClientId) = 12

	SELECT
		HMD.*,
		CLTM.TotalDepositAmount AS LTM
	FROM
	(
		SELECT DISTINCT C.ClientID,
						C.HeatMapItemID,
						K.ChecklistTypeID,
						CASE WHEN K.CheckListTypeId = @WeeklyChecklistID THEN '(W) ' + K.KPIDescription
						     WHEN K.CheckListTypeId = @MonthlyChecklistID THEN '(M) ' + K.KPIDescription
							 WHEN K.CheckListTypeId = @MetricsID THEN K.KPIDescription
						END AS HeatMapItemName,
						CASE WHEN K.CheckListTypeId IN (@WeeklyChecklistID, @MonthlyChecklistID) THEN 'RiskFactor'
						     ELSE 'Metric' 
						END AS HeatMapItemType,
						CR.ClientCode,
						CR.ClientName,
						CR.SiteAcronym,
						CR.BusinessUnitCode,
						CR.SpecialityName,
						CR.SystemCode,
						C.HeatMapItemDate,
						C.Score AS HeatMapItemNameScore,
						CR.ChecklistWeeklyDate,
						CR.ChecklistMonthlyDate,
						CR.RiskScore AS Risk,
						CASE WHEN CR.RiskScore IS NOT NULL THEN (CR.RiskScore * 100) / 200 
							 ELSE NULL
						END AS RiskPercentage,
						CASE WHEN T.CLIENTID IS NULL THEN NULL 
							 ELSE T.TREND 
						END AS Trend,
						MMCK.AlertLevel,
						MMCK.ActualValue
		FROM ClientHeatMapItemScore C
		INNER JOIN #CLIENT_RISK_SCORE CR ON C.ClientID = CR.ClientID AND CR.ROW_NUM = 1 
		INNER JOIN HeatMapItem HM WITH(NOLOCK) ON C.HeatMapItemID = HM.HeatMapItemID AND CR.EffectiveTime BETWEEN HM.StartTime AND HM.EndTime AND HM.EndTime > GETDATE()
		INNER JOIN KPI K ON HM.KPIID = K.KPIID
		INNER JOIN #CLIENT_RISK_SCORE CR1 ON C.ClientID = CR.ClientID AND (
				(C.HeatMapItemDate = CR.ChecklistWeeklyDate AND K.CheckListTypeId = @WeeklyChecklistID) OR 
				(C.HeatMapItemDate = CR.ChecklistMonthlyDate AND K.CheckListTypeId = @MonthlyChecklistID) OR
				(C.HeatMapItemDate = CR.M3DailyDate AND K.CheckListTypeId = @MetricsID)
			)
		LEFT JOIN M3metricClientKpiDaily MMCK ON MMCK.KpiID = K.KPIID AND K.CheckListTypeId = @MetricsID
		LEFT JOIN (	SELECT A.CLIENTID, SUM(A.RiskScore) / 4 AS TREND
					FROM #CLIENT_RISK_SCORE A
					WHERE A.ROW_NUM BETWEEN 2 AND 5 AND 
					A.ClientID IN (SELECT DISTINCT CLIENTID FROM #CLIENT_RISK_SCORE WHERE A.ClientID = CLIENTID AND ROW_NUM > 4)
					GROUP BY A.CLIENTID
				  ) T ON C.CLIENTID = T.ClientID
	
		UNION ALL

		SELECT DISTINCT A.ClientID,
				NULL HeatMapItemID,
				NULL ChecklistTypeID,
				'' AS HeatMapItemName,
				NULL AS HeatMapItemType,
				ClientCode,
				ClientName,
				SiteAcronym,
				BusinessUnitCode,
				SpecialityName,
				SystemCode,
				NULL HeatMapItemDate,
				NULL HeatMapItemNameScore,
				NULL ChecklistWeeklyDate,
				NULL ChecklistMonthlyDate,
				NULL Risk,
				NULL RiskPercentage,
				NULL Trend,
				NULL AlertLevel,
				NULL ActualValue
		FROM #CLIENT_RISK_SCORE A
		WHERE NOT EXISTS (SELECT TOP 1 1 FROM ClientHeatMapItemScore B WHERE A.ClientID = B.ClientID)
	) AS HMD
	LEFT JOIN #ClientLTM CLTM ON CLTM.ClientId = HMD.ClientID
	ORDER BY HMD.ClientName, HMD.CheckListTypeId


	/*****************************************************************************************************************************************************************/
	
	/****************************** To get heat map items (universal kpis)   ******************************/
	SELECT CASE WHEN K.CheckListTypeId = @WeeklyChecklistID THEN '(W) ' + K.KPIDescription
				WHEN K.CheckListTypeId = @MonthlyChecklistID THEN '(M) ' + K.KPIDescription
				WHEN K.CheckListTypeId = @MetricsID THEN K.KPIDescription
			END AS HeatMapItemName,
			CASE WHEN K.CheckListTypeId IN (@WeeklyChecklistID, @MonthlyChecklistID) THEN 'RiskFactor'
				 ELSE 'Metric' 
			END AS HeatMapItemType,
			CASE WHEN K.CheckListTypeId = @WeeklyChecklistID THEN 'Weekly'
				WHEN K.CheckListTypeId = @MonthlyChecklistID THEN 'Monthly'
				WHEN K.CheckListTypeId = @MetricsID THEN KM.Measure
			END AS HeatMapItemMeasureType
	FROM
	HeatMapItem HM
	INNER JOIN KPI K ON K.KPIID = HM.KPIID
	INNER JOIN KPIMeasure KM ON KM.KPIMeasureID = K.KPIMeasureID
	WHERE GETDATE() BETWEEN HM.StartTime AND HM.EndTime
	ORDER BY K.CheckListTypeId DESC, KM.KPIMeasureID

END
