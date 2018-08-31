/*
Script Name : usp_Escalate_Mail_Daily_Job
Module_Name : M3Pact
Created By  : Shravani P
CreatedDate : 04/12/2018

Revision History 
=============
Ver  Date				Who						Comment
1.0  12-Jun-2018		Shravani Palla			Escalated mail to be sent when KPIs are deviated
1.1  14-Jun-2018		Shravani Palla			Added ClientName & changed submitted date logic
1.2  26-Jun-2018		Shravani Palla			Handling scenario when Relation/Billig is Admin (i.e., there is no higher level)
1.3  28-Jun-2018		Shravani Palla			Consider Metrics data

Sample Execution Script :
=============
EXEC usp_Escalate_Mail_Daily_Job
*/


CREATE PROCEDURE [dbo].[usp_Escalate_Mail_Daily_Job]
AS
BEGIN
	DECLARE @Today DATE = CAST(GETDATE() AS DATE);

	DECLARE @WeeklyChecklistID INT;
	DECLARE @MonthlyChecklistID INT;
	DECLARE @MetricsID INT;
	DECLARE @MetricsDaily NVARCHAR(100) = 'Daily';
	DECLARE @MetricsWeekly NVARCHAR(100) = 'Weekly';
	DECLARE @MetricsMonthly NVARCHAR(100) = 'Monthly';
	
	SELECT @WeeklyChecklistID = ChecklistTypeID FROM CheckListType WHERE CheckListTypeCode = 'WEEK'
	SELECT @MonthlyChecklistID = ChecklistTypeID FROM CheckListType WHERE CheckListTypeCode = 'MONTH'
	SELECT @MetricsID = ChecklistTypeID FROM CheckListType WHERE CheckListTypeCode = 'M3'

	SELECT	
		C.KPIID,
		C.IsSLA,
		C.SendToRelationshipManager,
		C.SendToBillingManager,
		(NoOfRepeatitions/CAST(SUBSTRING(C.EscalateTriggerTime, 0, 2) AS INT)) ManagerLevel,
		SUBSTRING(C.EscalateTriggerTime, 0, 2) EscalateTriggerTime,
		B.KPIDescription,
		B.[Standard],
		A.*
	INTO #Res
	FROM
	(
		SELECT *
		FROM
		(
			SELECT	
				A.QuestionCode,
				A.ChecklistTypeNumber,
				A.ClientID,
				A.StartDate,
				A.ExpectedResponse,
				CASE 
					WHEN A.ChecklistTypeNumber IN (1,4) THEN DATEADD(DAY, -7, B.EndDate)
					WHEN A.ChecklistTypeNumber IN (2,5) THEN DATEADD(MONTH, -1, B.EndDate)
					WHEN A.ChecklistTypeNumber = 3		THEN DATEADD(DAY, -1, B.EndDate)
				END EndDate,
				ROW_NUMBER() OVER(PARTITION BY A.QuestionCode, A.ChecklistTypeNumber, A.ClientID ORDER BY B.EndDate DESC) A, 
				CASE 
					WHEN A.ChecklistTypeNumber IN (1,4) THEN DATEDIFF(DAY, A.StartDate, DATEADD(DAY, -7, B.EndDate))/7
					WHEN A.ChecklistTypeNumber IN (2,5) THEN DATEDIFF(MONTH, A.StartDate, DATEADD(MONTH, -1, B.EndDate))
					WHEN A.ChecklistTypeNumber = 3		THEN DATEDIFF(DAY, A.StartDate, DATEADD(DAY, -1, B.EndDate))
				END+1 NoOfRepeatitions
			FROM 
			(
				SELECT	
					*,
					ROW_NUMBER() OVER(PARTITION BY A.QuestionCode, A.ChecklistTypeNumber, A.ClientID, A, A.ExpectedResponse ORDER BY A.QuestionCode, A.ChecklistTypeNumber, A.ClientID, NewEndDate) Ran
				FROM
				(
					SELECT	
						A.*,
						CASE 
							WHEN LEAD(EndDate) OVER(PARTITION BY A.QuestionCode, A.ChecklistTypeNumber, A.ClientID, A.ExpectedResponse ORDER BY EndDate) IS NULL THEN EndDate 
							ELSE 
								CASE 
									WHEN Lead(EndDate) OVER(PARTITION BY A.QuestionCode, A.ChecklistTypeNumber, A.ClientID, A.ExpectedResponse ORDER BY EndDate) IS NOT NULL AND Lead(StartDate) OVER(PARTITION BY A.QuestionCode, A.ChecklistTypeNumber, A.ClientID, A.ExpectedResponse ORDER BY EndDate) = EndDate THEN NULL 
									ELSE EndDate 
								END
						END NewEndDate,
						CASE
							WHEN EndDate = 
								CASE 
									WHEN LEAD(EndDate) OVER(PARTITION BY A.QuestionCode, A.ChecklistTypeNumber, A.ClientID, A.ExpectedResponse ORDER BY EndDate) IS NULL THEN EndDate
									ELSE 
										CASE
											WHEN Lead(EndDate) OVER(PARTITION BY A.QuestionCode, A.ChecklistTypeNumber, A.ClientID, A.ExpectedResponse ORDER BY EndDate) IS NOT NULL AND Lead(StartDate) OVER(PARTITION BY A.QuestionCode, A.ChecklistTypeNumber, A.ClientID, A.ExpectedResponse ORDER BY EndDate) = EndDate THEN NULL
											ELSE EndDate
										END 
								END 
							THEN -1
							ELSE 1 
						END A
					FROM 
					(
						SELECT	
							A.*, 
							A.CheckListDate AS StartDate,
							CASE 
								WHEN A.ChecklistTypeId = @WeeklyChecklistID THEN 1
								WHEN A.ChecklistTypeId = @MonthlyChecklistID THEN 2
								WHEN A.ChecklistTypeId = @MetricsID AND C.Measure = @MetricsDaily THEN 3
								WHEN A.ChecklistTypeId = @MetricsID AND C.Measure = @MetricsWeekly THEN 4
								WHEN A.ChecklistTypeId = @MetricsID AND C.Measure = @MetricsMonthly THEN 5
							END ChecklistTypeNumber,
							CASE 
								WHEN (A.ChecklistTypeId = @WeeklyChecklistID) OR (A.ChecklistTypeId = @MetricsID AND C.Measure = @MetricsWeekly)  THEN DATEADD(DAY, 7, A.CheckListDate)
								WHEN (A.ChecklistTypeId = @MonthlyChecklistID) OR (A.ChecklistTypeId = @MetricsID AND C.Measure = @MetricsMonthly) THEN DATEADD(MONTH, 1, A.CheckListDate)
								WHEN A.ChecklistTypeId = @MetricsID AND C.Measure = @MetricsDaily THEN DATEADD(DAY, 1, A.CheckListDate)
							END AS EndDate,
							MAX(SubmittedDate) OVER(PARTITION BY A.QuestionCode, A.ChecklistTypeId, A.ClientID, A.ExpectedResponse) AS MaxSubmittedDate
						FROM DeviatedClientKPI A
						INNER JOIN KPI B ON A.QuestionCode = B.Measure AND B.RecordStatus = 'A'
						INNER JOIN KPIMeasure C ON C.KPIMeasureID = B.KPIMeasureID AND C.RecordStatus = 'A'
						WHERE A.RecordStatus = 'A' 
					) A
					WHERE MaxSubmittedDate = @Today
				) A
				WHERE A <> 1
			) B
			INNER JOIN 
			(
				SELECT *,ROW_NUMBER() OVER(PARTITION BY A.QuestionCode, A.ChecklistTypeNumber, A.ClientID, A, A.ExpectedResponse ORDER BY A.QuestionCode, A.ChecklistTypeNumber, A.ClientID, NewStartDate) Ran
				FROM
				(
					SELECT	
						A.*,
						CASE WHEN LAG(StartDate) OVER(PARTITION BY A.QuestionCode, A.ChecklistTypeNumber, A.ClientID, A.ExpectedResponse ORDER BY StartDate) IS NULL THEN StartDate ELSE CASE WHEN LAG(StartDate) OVER(PARTITION BY A.QuestionCode,A.ChecklistTypeNumber,A.ClientID,A.ExpectedResponse Order by StartDate) IS NOT NULL AND LAG(EndDate) OVER(PARTITION BY A.QuestionCode,A.ChecklistTypeNumber,A.ClientID,A.ExpectedResponse Order by StartDate) = StartDate THEN NULL ELSE StartDate END  END NewStartDate,
						CASE WHEN StartDate = CASE WHEN LAG(StartDate) OVER(PARTITION BY A.QuestionCode, A.ChecklistTypeNumber, A.ClientID, A.ExpectedResponse ORDER BY StartDate) IS NULL THEN StartDate ELSE CASE WHEN LAG(StartDate) OVER(PARTITION BY A.QuestionCode, A.ChecklistTypeNumber, A.ClientID, A.ExpectedResponse Order by StartDate) IS NOT NULL AND LAG(EndDate) OVER(PARTITION BY A.QuestionCode, A.ChecklistTypeNumber, A.ClientID, A.ExpectedResponse Order by StartDate) = StartDate THEN NULL ELSE StartDate END  END 
							 THEN -1 
							 ELSE 1 
						END A
					FROM 
					(
						SELECT	
							A.*, 
							A.CheckListDate AS StartDate,
							CASE 
								WHEN A.ChecklistTypeId = @WeeklyChecklistID THEN 1
								WHEN A.ChecklistTypeId = @MonthlyChecklistID THEN 2
								WHEN A.ChecklistTypeId = @MetricsID AND C.Measure = @MetricsDaily THEN 3
								WHEN A.ChecklistTypeId = @MetricsID AND C.Measure = @MetricsWeekly THEN 4
								WHEN A.ChecklistTypeId = @MetricsID AND C.Measure = @MetricsMonthly THEN 5
							END ChecklistTypeNumber,
							CASE 
								WHEN (A.ChecklistTypeId = @WeeklyChecklistID) OR (A.ChecklistTypeId = @MetricsID AND C.Measure = @MetricsWeekly) THEN DATEADD(DAY, 7, A.CheckListDate)
								WHEN (A.ChecklistTypeId = @MonthlyChecklistID) OR (A.ChecklistTypeId = @MetricsID AND C.Measure = @MetricsMonthly) THEN DATEADD(MONTH, 1, A.CheckListDate)
								WHEN A.ChecklistTypeId = @MetricsID AND C.Measure = @MetricsDaily THEN DATEADD(DAY, 1, A.CheckListDate)
							END EndDate,
							MAX(SubmittedDate) OVER(PARTITION BY A.QuestionCode, A.ChecklistTypeId, A.ClientID, A.ExpectedResponse) AS MaxSubmittedDate
						FROM DeviatedClientKPI A
						INNER JOIN KPI B ON A.QuestionCode = B.Measure AND B.RecordStatus = 'A'
						INNER JOIN KPIMeasure C ON C.KPIMeasureID = B.KPIMeasureID AND C.RecordStatus = 'A'
						WHERE A.RecordStatus = 'A'
					) A
					WHERE MaxSubmittedDate = @Today
				) A
				WHERE A <> 1
			) A
			ON A.Ran = B.Ran AND A.QuestionCode = B.QuestionCode AND A.ChecklistTypeNumber = B.ChecklistTypeNumber AND A.ClientID = B.ClientID AND A.ExpectedResponse = B.ExpectedResponse
		) A
		WHERE A = 1
	) A
	INNER JOIN KPI B ON A.QuestionCode = B.Measure AND B.RecordStatus = 'A'
	INNER JOIN kpialert C ON C.KPIID = B.KPIID AND C.RecordStatus = 'A'
	WHERE NoOfRepeatitions >= CAST(SUBSTRING(C.EscalateTriggerTime,0,2) AS INT)
	AND A.EndDate IN
	(
		SELECT MAX(IB.CheckListDate)
		FROM DeviatedClientKPI IB
		INNER JOIN KPI ID ON IB.QuestionCode = ID.Measure AND ID.RecordStatus = 'A' AND IB.RecordStatus = 'A'
		INNER JOIN KPIMeasure IC ON IC.KPIMeasureID = ID.KPIMeasureID AND IC.RecordStatus = 'A'
		WHERE A.QuestionCode = IB.QuestionCode
			AND A.ChecklistTypeNumber = CASE WHEN IB.ChecklistTypeId = @WeeklyChecklistID THEN 1
											 WHEN IB.ChecklistTypeId = @MonthlyChecklistID THEN 2
											 WHEN IB.ChecklistTypeId = @MetricsID AND IC.Measure = @MetricsDaily THEN 3
											 WHEN IB.ChecklistTypeId = @MetricsID AND IC.Measure = @MetricsWeekly THEN 4
											 WHEN IB.ChecklistTypeId = @MetricsID AND IC.Measure = @MetricsMonthly THEN 5
										END
			AND A.ClientID = IB.ClientID
			AND A.ExpectedResponse = IB.ExpectedResponse
			AND IB.CheckListDate BETWEEN A.StartDate AND A.EndDate
		GROUP BY IB.QuestionCode, CASE WHEN IB.ChecklistTypeId = @WeeklyChecklistID THEN 1
									   WHEN IB.ChecklistTypeId = @MonthlyChecklistID THEN 2
									   WHEN IB.ChecklistTypeId = @MetricsID AND IC.Measure = @MetricsDaily THEN 3
									   WHEN IB.ChecklistTypeId = @MetricsID AND IC.Measure = @MetricsWeekly THEN 4
									   WHEN IB.ChecklistTypeId = @MetricsID AND IC.Measure = @MetricsMonthly THEN 5
								  END, IB.ClientID, IB.ExpectedResponse
		HAVING MAX(IB.SubmittedDate) = @Today
	)

	SELECT	
		UserID,
		ClientID,
		KPIID,
		IsBillingManager
	INTO #R1
	FROM
	(
		SELECT
			UL.UserID,
			0 AS IsBillingManager,
			R.* 
		FROM #Res R
		INNER JOIN Client C ON R.ClientID = C.ClientID AND C.IsActive = 'A' AND C.RecordStatus = 'A'
		INNER JOIN UserLogin UL ON R.SendToRelationshipManager = 1 AND C.RelationShipManagerID = UL.ID AND UL.RecordStatus = 'A'
		UNION
		SELECT 
			UL.UserID, 
			1 AS IsBillingManager,
			R.* 
		FROM #Res R
		INNER JOIN Client C ON R.ClientID = C.ClientID AND C.IsActive = 'A' AND C.RecordStatus = 'A'
		INNER JOIN UserLogin UL ON R.SendToBillingManager = 1 AND C.BillingManagerID = UL.ID AND UL.RecordStatus = 'A'
	) A

	;WITH CTE AS
	(
		SELECT
			E.ReportsTo,
			E.EmployeeID,
			E.Email,
			A.KPIID,
			1 AS EmployeeManagerLevel,
			A.IsBillingManager,
			A.ClientId
		FROM #R1 A
		INNER JOIN Employee E ON E.EmployeeID = A.UserID AND E.RecordStatus = 'A'
		UNION ALL
		SELECT
			E.ReportsTo,
			E.EmployeeID,
			E.Email,
			A.KPIID,
			A.EmployeeManagerLevel + 1 AS EmployeeManagerLevel,
			A.IsBillingManager,
			A.ClientId
		FROM Employee E
		INNER JOIN CTE A ON E.EmployeeID = A.ReportsTo AND E.RecordStatus = 'A'
	) 
	SELECT DISTINCT EmployeeID,KPIID,EmployeeManagerLevel, Email, IsBillingManager,ClientId INTO #ManagerLevels
	FROM CTE 

	SELECT	
		R.ClientID,
		R.KPIID,
		CKM.IsSLA,
		R.ExpectedResponse,
		R.ManagerLevel,
		R.KPIDescription,
		C.ClientCode + ' - ' + C.[Name] AS Client,
		DC.ActualResponse,
		DC.ChecklistDate,
		DC.ExpectedResponse AS [Standard],
		DC.DeviatedClientKPIId,
		CASE 
			WHEN R.ChecklistTypeNumber IN (1,4) THEN CAST(R.NoOfRepeatitions AS NVARCHAR) + ' week(s)'
			WHEN R.ChecklistTypeNumber IN (2,5) THEN CAST(R.NoOfRepeatitions AS NVARCHAR) + ' month(s)'
			WHEN R.ChecklistTypeNumber = 3		THEN CAST(R.NoOfRepeatitions AS NVARCHAR) + ' day(s)'
		END DeviatedSince,
		CASE 
			WHEN R.ChecklistTypeNumber = 1 THEN 'Weekly Checklist'
			WHEN R.ChecklistTypeNumber = 2 THEN 'Monthly Checklist'
			WHEN R.ChecklistTypeNumber = 3 THEN 'Daily M3'
			WHEN R.ChecklistTypeNumber = 4 THEN 'Weekly M3'
			WHEN R.ChecklistTypeNumber = 5 THEN 'Monthly M3'
		END KPIType,
		RelationshipManager = (	
								SELECT REPLACE(LEFT(A.Email, CHARINDEX('@', A.Email, 0) - 1),'.',' ')
								FROM #ManagerLevels A
								WHERE A.EmployeeManagerLevel = 1 AND A.KPIID = R.KPIID AND A.ClientId = R.ClientId AND A.IsBillingManager = 0
							  ),
		BillingManager = (
							SELECT REPLACE(LEFT(A.Email, CHARINDEX('@', A.Email, 0) - 1),'.',' ')
							FROM #ManagerLevels A
							WHERE A.EmployeeManagerLevel = 1 AND A.KPIID = R.KPIID AND A.ClientId = R.ClientId AND A.IsBillingManager = 1
						 ),
		EmailAddress = 
			STUFF((
				SELECT ',' + A.Email + '-' + CAST(A.EmployeeID AS NVARCHAR) 
				FROM #ManagerLevels A 
				WHERE 
				(
					(
						A.IsBillingManager = 1 
						AND A.EmployeeManagerLevel = 
							CASE WHEN R.ManagerLevel + 1 > A.EmployeeManagerLevel AND A.IsBillingManager = 1
								 THEN (SELECT MAX(IA.EmployeeManagerLevel) FROM #ManagerLevels IA WHERE IA.IsBillingManager = 1 AND IA.KPIID = R.KPIID AND IA.ClientId = R.ClientId) 
								 ELSE R.ManagerLevel + 1 
							END 
					)
					OR 
					(
						A.IsBillingManager = 0 
						AND A.EmployeeManagerLevel = 
							CASE WHEN R.ManagerLevel + 1 > A.EmployeeManagerLevel AND A.IsBillingManager = 0
								 THEN (SELECT MAX(IA.EmployeeManagerLevel) FROM #ManagerLevels IA WHERE IA.IsBillingManager = 0 AND IA.KPIID = R.KPIID AND IA.ClientId = R.ClientId) 
								 ELSE R.ManagerLevel + 1 
							END 
					)
				)
				AND A.KPIID = R.KPIID AND A.ClientId = R.ClientId
				ORDER BY A.IsBillingManager
				FOR XML PATH('')
			), 1, 1, '')
	FROM #Res R
	CROSS APPLY
	(
		SELECT TOP 1 *
		FROM DeviatedClientKPI AS DC
		WHERE DC.CheckListDate = R.EndDate AND DC.ClientId = R.ClientId AND DC.QuestionCode = R.QuestionCode AND DC.RecordStatus = 'A'
		Order by DeviatedClientKPIId DESC
	) DC 
	INNER JOIN KPI ON DC.QuestionCode = KPI.Measure AND KPI.RecordStatus = 'A'
	INNER JOIN KPIMeasure ON KPIMeasure.KPIMeasureID = KPI.KPIMeasureID AND KPIMeasure.RecordStatus = 'A' 
	INNER JOIN Client C ON C.ClientID = R.ClientId
	INNER JOIN ClientKPIMap CKM ON CKM.ClientID = R.ClientID AND CKM.KPIID = R.KPIID
	LEFT JOIN MailRecepientsDetailsDayWise MR ON MR.DeviatedClientKPIId = DC.DeviatedClientKPIId AND CAST(MR.SentDate AS DATE) = @Today AND MR.AlertType = 'Escalation'
	WHERE MR.Id IS NULL 

END
