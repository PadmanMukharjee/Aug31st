
/*
Script Name : usp_GetKPIQuestionsForClient
Module_Name : M3Pact
Created By  : Rajesh Aavula
CreatedDate : 05/24/2018

Revision History 
=============
Ver  Date				Who						Comment
1.0  24-May-2018		Rajesh Aavula			Get all the KPI questions that can be assigned for Client
1.1  06-JUN-2018		Rajesh Aavula			Added record status and CAM startDate!=EndDate
1.2  06-July-2018       Abhishek Kovvuri        Adding Measure column in the result set.
*/


CREATE PROC [dbo].[usp_GetKPIQuestionsForClient]

@ClientCode nvarchar(20)

AS
BEGIN

DECLARE @M3 nvarchar(60)='M3';
Declare @M3TypeId INT;
SET @M3TypeId= (SELECT CheckListTypeID FROM CheckListType WHERE CheckListTypeCode=@M3)

SELECT	KPIID,
		KPIDescription,
		kpiQ.CheckListTypeCode,
		IsUniversal AS 'IsUniversal',
		CASE WHEN [Company Standard] = 1 
		THEN 'YES' 
		ELSE 'NO' 
		END AS [Company Standard],
		CKQ.EndDate,
		KPIQ.Measure
 FROM
	(SELECT 
			KP.KPIID,
			KP.KPIDescription,
			CT.CheckListTypeCode,
			CAST(0 AS BIT) AS 'IsUniversal',
			KP.Measure
	FROM KPI KP
	INNER JOIN CheckListType CT ON CT.CheckListTypeID = KP.CheckListTypeId
	WHERE CT.CheckListTypeCode != @M3
	AND KP.RecordStatus='A'
	) KPIQ
INNER JOIN (--To get all the kpi questions for a client(both weekly and monthly)
		SELECT  Q.QuestionCode,
				Q.ExpectedRespone AS 'Company Standard',
				Q.IsKPI,
				CL.CheckListTypeCode,
				CL.StartDate,
				CL.EndDate,
				CL.EffectiveDate
		FROM Question Q
			INNER JOIN CheckListAttributeMap CAM ON CAM.CheckListAttributeValueID = Q.QuestionCode AND CAM.CheckListAttributeID = @M3TypeId
			INNER JOIN (
						SELECT	C.ClientCode,
								CL.CheckListID,
								CT.CheckListTypeCode,
								CLM.EffectiveDate,
								CLM.StartDate,
								CLM.EndDate
						FROM Client C
						JOIN ClientCheckListMap CLM ON CLM.ClientID = C.ClientID
						JOIN Checklist CL ON CL.CheckListID=CLM.CheckListID
						JOIN CheckListType CT ON CT.CheckListTypeID = CL.CheckListTypeID
						WHERE c.ClientCode = @ClientCode
							  AND GETDATE() BETWEEN CLM.StartDate AND CLM.EndDate
							  AND CLM.StartDate!=CLM.EndDate
							  AND CLM.RecordStatus='A'
						) CL ON CL.CheckListID = CAM.CheckListID
		WHERE Q.IsKPI = 1 AND Q.IsUniversal != 1
		AND Q.StartDate!=Q.EndDate
		AND GETDATE() BETWEEN Q.StartDate AND Q.EndDate
		AND CAM.StartDate!=CAM.EndDate
		AND CAM.RecordStatus='A'
		AND Q.RecordStatus='A'
) CKQ on CKQ.QuestionCode = KPIQ.Measure and CKQ.CheckListTypeCode = KPIQ.CheckListTypeCode

UNION All

SELECT  kp.KPIID,
		kp.KPIDescription,
		ct.CheckListTypeCode,
		CAST(0 AS BIT) AS 'IsUniversal',
		REPLACE (kp.AlertLevel, ',', ' ') AS [Company Standard],
		null,
		kp.Measure
FROM KPI kp
JOIN CheckListType CT on CT.CheckListTypeID = kp.CheckListTypeId
WHERE CT.CheckListTypeCode = @M3 AND KP.IsUniversal=0
AND KP.RecordStatus='A'
END

GO



