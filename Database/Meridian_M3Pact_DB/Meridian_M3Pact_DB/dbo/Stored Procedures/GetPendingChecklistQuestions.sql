CREATE PROCEDURE [dbo].[GetPendingChecklistQuestions]
(
	@clientCode VARCHAR(50),
	@checklistType VARCHAR(10),
	@PendingDate DATETIME	
)
AS
BEGIN

CREATE Table #Checklist
(
	ChecklistName VARCHAR(100),	
	Questionid INT,
	QuestionCode VARCHAR(20),
	QuestionText VARCHAR(200),
	IsKPI bit,
	RequireFreeform bit,
	ExpectedRespone bit,
	ActualResponse bit,
	ActualFreeForm Varchar(MAX),
	ClientCheckListMapID INT,
	CheckListAttributeMapID INT
)

INSERT INTO #Checklist
SELECT CL.CheckListName, Q.QuestionID,Q.QuestionCode, Q.QuestionText,Q.IsKPI,Q.RequireFreeform,Q.ExpectedRespone,NULL,'', CLM.ClientCheckListMapID,CAM.CheckListAttributeMapID 
FROM Client C
JOIN ClientCheckListMap CLM
ON C.ClientID=CLM.ClientID
JOIN CheckList CL
ON CL.CheckListID=CLM.CheckListID
JOIN CheckListType CLT
ON CLT.CheckListTypeID=CL.CheckListTypeID
JOIN CheckListAttributeMap CAM
ON CL.CheckListID=CAM.CheckListID
JOIN CheckListAttribute CA
ON CA.CheckListAttributeID=CAM.CheckListAttributeID
JOIN Question Q
ON Q.QuestionCode= CAM.CheckListAttributeValueID
WHERE c.ClientCode=@clientCode
AND CA.AttributeCode='QUE'
AND CLT.CheckListTypeCode=@checklistType
AND CLM.StartDate<= @PendingDate 
AND @PendingDate < CLM.EndDate
AND CLM.StartDate <> CLM.EndDate
AND CLM.EffectiveDate <= @PendingDate
AND Q.StartDate<= @PendingDate 
AND @PendingDate < Q.EndDate
AND Q.StartDate<> Q.EndDate
AND Q.EffectiveDate <= @PendingDate
AND @PendingDate between CAM.StartDate and CAM.EndDate
AND Q.RecordStatus='A'
AND CLM.RecordStatus='A'
AND CAM.RecordStatus='A'
ORDER BY q.QuestionCode

DECLARE @StatusiD INT =0

SET @StatusiD=( SELECT DISTINCT TOP 1 CSD.ClientCheckListStatusDetailID FROM Client C
				join ClientCheckListMap CLM
				on C.ClientID=CLM.ClientID
				join CheckList CL
				on CLM.CheckListID=CL.CheckListID
				Join CheckListType CLT
				on CLT.CheckListTypeID=CL.CheckListTypeID
				join ClientCheckListStatusDetail CSD
				on CSD.ClientCheckListMapID=CLM.ClientCheckListMapID
				join #Checklist cc
				on cc.ClientCheckListMapID= CSD.ClientCheckListMapID
				where CSD.CheckListEffectiveDate= @PendingDate
				and CLT.CheckListTypeCode=@checklistType
				and CLM.RecordStatus='A'
				)

IF(@StatusiD>0)
BEGIN
UPDATE cc
SET cc.ActualResponse=cqr.ExpectedResponse,
cc.ActualFreeForm=cqr.FreeFormResponse
FROM #Checklist cc
JOIN ClientCheckListQuestionResponse CQR
ON cc.ClientCheckListMapID=CQR.ClientCheckListMapID
AND cc.CheckListAttributeMapID=cqr.CheckListAttributeMapID
WHERE cqr.ClientCheckListStatusDetailID= @StatusiD
END

SELECT * FROM #Checklist

DROP TABLE #Checklist

END

