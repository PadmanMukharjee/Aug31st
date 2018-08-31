
/*
Script Name : usp_UpdateClientKPIEffectiveDateOnChecklistUpdate
Module_Name : M3Pact
Created By  : Abhishek Kovvuri
CreatedDate : 06/06/2018

Revision History 
=============
Ver  Date				Who						Comment
1.0  06-June-2018		Abhishek Kovvuri		Update Client KPI effective date on checklist update.
*/

CREATE PROC [dbo].[usp_UpdateClientKPIEffectiveDateOnChecklistUpdate]
@ClientId INT,
@CurrentClientWeeklyCheckListId INT,
@UpdatedClientWeeklyCheckListId INT,
@AssignedClientWeeklyCheckListId INT,
@WeeklyCheckListEffectiveDate DATETIME,
@MonthlyCheckListEffectiveDate DATETIME,
@User NVARCHAR(50),
@CurrentClientMonthlyCheckListId INT,  --current active
@UpdatedClientMonthlyCheckListId INT,  --newly added now
@AssignedClientMonthlyCheckListId INT, --current asigned will be effecyive from next week/month
@MaxDate DATETIME


AS
BEGIN
DECLARE @NewCheckListKPI VARCHAR(50) = 'NewCheckListKPI'
DECLARE @OldCheckListKPI VARCHAR(50) = 'OldCheckListKPI'
DECLARE @CommonCheckListKPI VARCHAR(50) = 'CommonCheckListKPI'

     CREATE TABLE #TempCheckListType(CheckListID INT , QuestionType VARCHAR(50))
	 INSERT INTO #TempCheckListType
	 SELECT cclm.CheckListID ,
	   CASE 
	     WHEN cclm.EffectiveDate  < GETDATE() THEN @OldCheckListKPI
		 WHEN cclm.EffectiveDate > GETDATE() THEN @NewCheckListKPI 
	   END AS QuestionType 
	 FROM ClientCheckListMap cclm 
	 INNER JOIN CheckList cl ON cclm.CheckListID = cl.CheckListID 
	 WHERE  ClientID = @ClientId AND cclm.StartDate <= getdate() AND cclm.EndDate > GETDATE() AND StartDate != EndDate

     IF(@CurrentClientWeeklyCheckListId = @UpdatedClientWeeklyCheckListId)
	 BEGIN
	     INSERT INTO #TempCheckListType
	     SELECT @AssignedClientWeeklyCheckListId , @OldCheckListKPI AS QuestionType

		  UPDATE #TempCheckListType
		  SET QuestionType=@NewCheckListKPI
		  WHERE CheckListID=@CurrentClientWeeklyCheckListId
     END
     
	 CREATE TABLE #TempChecklistQuestions(CheckListAttributeValueID VARCHAR(50) , QuestionType VARCHAR(50))
     INSERT INTO #TempChecklistQuestions 
     SELECT cam.CheckListAttributeValueID , ccl.QuestionType
	 FROM CheckListAttributeMap cam
     INNER JOIN #TempCheckListType
     ccl ON cam.CheckListID = ccl.CheckListID 
	 INNER JOIN CheckListAttribute ca ON cam.CheckListAttributeID = ca.CheckListAttributeID
     WHERE AttributeCode = 'QUE' 


	CREATE TABLE #TempOldCheckListQuestions(CheckListAttributeValueID VARCHAR(50) , QuestionType VARCHAR(50))
    INSERT INTO #TempOldCheckListQuestions
    SELECT DISTINCT CheckListAttributeValueID , QuestionType
    FROM #TempChecklistQuestions WHERE QuestionType = @OldCheckListKPI 
  
 

    UPDATE #TempOldCheckListQuestions 
	SET QuestionType=@CommonCheckListKPI
	WHERE CheckListAttributeValueID IN (
    SELECT CheckListAttributeValueID FROM 
	(SELECT CheckListAttributeValueID  FROM #TempOldCheckListQuestions
    INTERSECT
    SELECT CheckListAttributeValueID FROM 
	(SELECT DISTINCT CheckListAttributeValueID , QuestionType 
    FROM #TempChecklistQuestions  WHERE QuestionType = @NewCheckListKPI) AS TEQ) AS TOCQ)

	
	CREATE TABLE #TempClientKPI(ClientKPIMapId BIGINT, KPIID INT, [Client Stanadard] NVARCHAR(50),
	 IsSLA BIT , StartDate DATETIME, EndDate DATETIME , RecordStatus CHAR, QuestionType VARCHAR(50) , CheckListTypeCode VARCHAR(50))
	INSERT INTO #TempClientKPI(ClientKPIMapId , KPIID , [Client Stanadard] , IsSLA , StartDate , EndDate , RecordStatus , QuestionType , CheckListTypeCode)
	SELECT DISTINCT CKM.ClientKPIMapId , CKM.KPIID , CKM.[Client Standard] , CKM.IsSLA , CKM.StartDate , CKM.EndDate , CKM.RecordStatus ,
	 KP.QuestionType , KP.CheckListTypeCode 
	FROM ClientKPIMap CKM INNER JOIN 
	(SELECT KPIID , qc.QuestionType , qc.CheckListTypeCode 
	FROM kpi kp INNER JOIN 
	(SELECT distinct q.QuestionCode , oc.QuestionType , ct.CheckListTypeCode FROM Question q INNER JOIN 
    #TempOldCheckListQuestions oc ON q.QuestionCode = oc.CheckListAttributeValueID
	INNER JOIN CheckListType ct ON q.CheckListTypeId = ct.CheckListTypeID
    WHERE ISKPI = 1  AND q.StartDate <= GETDATE() AND q.EndDate > GETDATE() AND IsUniversal = 0 AND q.RecordStatus = 'A' 
	AND StartDate != EndDate) qc 
    ON kp.Measure = qc.QuestionCode
    ) KP ON CKM.KPIID = KP.KPIID WHERE CKM.ClientID = @ClientId AND CKM.StartDate <= GETDATE() AND CKM.EndDate > GETDATE()

	IF (@AssignedClientWeeklyCheckListId != @UpdatedClientWeeklyCheckListId)
	BEGIN
	   IF(@CurrentClientWeeklyCheckListId = @UpdatedClientWeeklyCheckListId)
	   BEGIN
	    
	      UPDATE CKM SET EndDate = 
	       CASE WHEN QuestionType = @OldCheckListKPI THEN TCK.StartDate 
		        WHEN QuestionType = @CommonCheckListKPI THEN @MaxDate 
		   END  
	      FROM ClientKPIMap CKM INNER JOIN #TempClientKPI TCK ON CKM.ClientKPIMapID = TCK.ClientKPIMapId WHERE CheckListTypeCode = 'WEEK'
	   
	   END
	   ELSE
	   BEGIN
	    
		  UPDATE CKM SET EndDate = 
	       CASE WHEN QuestionType = @OldCheckListKPI THEN @WeeklyCheckListEffectiveDate 
		        ELSE @MaxDate 
		   END  
	      FROM ClientKPIMap CKM INNER JOIN #TempClientKPI TCK ON CKM.ClientKPIMapID = TCK.ClientKPIMapId WHERE CheckListTypeCode = 'WEEK'
	      
       END
	END

	IF (@AssignedClientMonthlyCheckListId != @UpdatedClientMonthlyCheckListId)
	BEGIN
	IF(@CurrentClientMonthlyCheckListId = @UpdatedClientMonthlyCheckListId)
	     BEGIN
	      
	        UPDATE CKM SET EndDate = 
			CASE WHEN QuestionType = @OldCheckListKPI THEN TCK.StartDate 
		         WHEN QuestionType = @CommonCheckListKPI THEN @MaxDate 
		    END  
	        FROM ClientKPIMap CKM INNER JOIN #TempClientKPI TCK ON CKM.ClientKPIMapID = TCK.ClientKPIMapId WHERE CheckListTypeCode = 'MONTH'
	     
	     END
	     ELSE
	     BEGIN
	        
		   UPDATE CKM SET EndDate = 
	       CASE WHEN QuestionType = @OldCheckListKPI THEN @MonthlyCheckListEffectiveDate 
		        ELSE @MaxDate 
		   END  
	       FROM ClientKPIMap CKM INNER JOIN #TempClientKPI TCK ON CKM.ClientKPIMapID = TCK.ClientKPIMapId WHERE CheckListTypeCode = 'MONTH'

         END
    END

    DROP TABLE #TempChecklistQuestions
    DROP TABLE #TempOldCheckListQuestions
	DROP TABLE #TempClientKPI
	DROP TABLE #TempCheckListType
	
	SELECT 'SUCCESS' AS [Status]
END
