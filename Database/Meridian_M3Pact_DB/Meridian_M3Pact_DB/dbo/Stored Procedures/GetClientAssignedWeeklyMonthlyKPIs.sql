/*
Script Name : usp_GetClientAssignedWeeklyMonthlyKPIs
Module_Name : M3Pact
Created By  : Rajesh Aavula
CreatedDate : 06/01/2018

Revision History 
=============
Ver  Date				Who						Comment
1.0  01-JUN-2018		Rajesh Aavula			Get all the Weekly and Monthly KPI questions assigned for a client.
*/



CREATE PROC [dbo].[usp_GetClientAssignedWeeklyMonthlyKPIs]
@ClientCode nvarchar(20)

AS
BEGIN

    DECLARE @ClientId INT
	Declare @M3TypeId INT

SET @ClientId= (SELECT ClientID FROM Client WHERE ClientCode=@ClientCode)
SET @M3TypeId= (SELECT CheckListTypeID FROM CheckListType WHERE CheckListTypeCode='M3')

--Selecting Weekly and Monthly kpis for client
CREATE TABLE #kPIWeeklyMonthlyTempTable(
					ClientCode nvarchar(60),
					ChecklistId Int,
					ChecklistType nvarchar(60),
					ChecklistStartDate DateTime,
					ChecklistEndDate DateTime,
					ChecklistEffectiveDate DateTime,
					ChecklistQuestionAssignedStartDate DateTime,
					ChecklistQuestionAssignedEndDate DateTime,
					ChecklistQuestionEffectiveDate DateTime,
					KPIID Int,
					KPIDescription nvarchar(60),
					IsKPI Bit null,
					IsUnivarsal Bit null,
					CompanyStandard Bit null,
					IsSLA Bit null,
					QuestionCode nvarchar(50),
					QuestionStartDate DateTime,
					QuestionEndDate DateTime,
					QuestionEffectiveDate DateTime,
					ClientKPIMapID Int,
					KPIAssignedStartDate DateTime,
					KPIAssignedEndDate DateTime
			)
INSERT INTO #kPIWeeklyMonthlyTempTable
SELECT 
			C.CLIENTCODE as ClientCode,
			CL.CHECKLISTID as ChecklistId,
			CT.CheckListTypeCode AS ChecklistType,
			CCLM.StartDate as ChecklistStartDate,
			CCLM.EndDate as ChecklistEndDate,
			CCLM.EffectiveDate as ChecklistEffectiveDate,
			CAM.StartDate as ChecklistQuestionAssignedStartDate,
			CAM.endDate as ChecklistQuestionAssignedEndDate,
			CAM.EffectiveDate as ChecklistQuestionEffectiveDate,
			KPI.KPIID as KPIID,
			KPI.KPIDescription as KPIDescription,
			Q.IsKPI as IsKPI,
			Q.IsUniversal as IsUnivarsal,
			Q.ExpectedRespone as CompanyStandard,
			CKM.IsSLA as IsSLA,
			Q.QuestionCode as QuestionCode,
			Q.StartDate as QuestionStartDate,
			Q.EndDate as QuestionEndDate,
			Q.EffectiveDate as QuestionEffectiveDate,
			CKM.ClientKPIMapID as ClientKPIMapID,
			CKM.StartDate as KPIAssignedStartDate,
			CKM.EndDate as KPIAssignedEndDate
FROM 
CLIENT C
JOIN ClientChecklistMap CCLM on CCLM.CLIENTiD=C.CLIENTID
JOIN CHECKLIST CL ON CL.CHECKLISTID=CCLM.CHECKLISTID
JOIN CheckListType CT ON CL.CheckListTypeID=CT.CheckListTypeID
JOIN CHECKLISTATTRIBUTEMAP CAM ON CAM.CHECKLISTID=CL.CHECKLISTID AND CAM.CHECKLISTATTRIBUTEID=@M3TypeId
JOIN QUESTION Q ON Q.QUESTIONCODE=CAM.CHECKLISTATTRIBUTEVALUEID
JOIN KPI KPI ON KPI.MEASURE=Q.QUESTIONCODE
JOIN CLIENTKPIMAP CKM on CKM.KPIID=KPI.KPIID AND CKM.ClientID=@ClientId
WHERE C.CLIENTID=@ClientId
AND GETDATE() BETWEEN CCLM.STARTDATE AND CCLM.ENDDATE
AND CCLM.StartDate!=CCLM.EndDate
AND GETDATE() BETWEEN CAM.STARTDATE AND CAM.ENDDATE
AND CAM.StartDate!=CAM.EndDate
AND GETDATE() BETWEEN q.StartDate AND Q.EndDate
AND Q.StartDate!=Q.EndDate
AND GETDATE() BETWEEN CKM.StartDate AND CKM.EndDate
AND CKM.StartDate!=CKM.EndDate
AND CCLM.RecordStatus = 'A'
AND CAM.RecordStatus = 'A'
AND Q.RecordStatus = 'A'
AND CKM.RecordStatus = 'A'


SELECT * FROM #kPIWeeklyMonthlyTempTable
SELECT DISTINCT
		KPIT.ClientKPIMapID,
		U.ID,
		U.Email,
		U.FirstName,
		U.IsMeridianUser,
		U.LastName,
		U.MobileNumber,
		R.RoleCode,
		U.UserName,
		U.UserID
FROM #kPIWeeklyMonthlyTempTable KPIT
JOIN ClientKPIUserMap CKM on KPIT.ClientKPIMapID = CKM.ClientKPIMapID
JOIN UserLogin u on u.ID=ckm.UserId
JOIN Roles R ON U.RoleID=R.RoleID
WHERE CKM.RecordStatus='A'

DROP TABLE #kPIWeeklyMonthlyTempTable

END
