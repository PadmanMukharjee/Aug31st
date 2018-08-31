/*
Script Name : 23_ControlType
Module_Name : M3Pact
Created By  : Mukharjee Chippa
CreatedDate : 31-July-2018

Data Verification Script : 

SELECT * FROM [ControlType](NOLOCK)
*/

BEGIN TRY
BEGIN TRANSACTION

DECLARE @User NVARCHAR(30) = 'Admin'
DECLARE @AttributeId INT


INSERT INTO [dbo].[AdminConfigValues]
(
	 [AttributeId] ,
    [AttributeValue] , 
    [RecordStatus]  , 
    [CreatedDate] , 
    [CreatedBy] , 
    [ModifiedDate]  , 
    [ModifiedBy] 
)
SELECT
	A.[AttributeId] ,
    A.[AttributeValue] , 
    A.[RecordStatus]  , 
    A.[CreatedDate] , 
    @User AS [CreatedBy] , 
    A.[ModifiedDate]  , 
    @User AS [ModifiedBy] 
FROM
(
	SELECT (SELECT AttributeId FROM Attribute WHERE AttributeCode='LastEnteredBusinessDays') AS [AttributeId],15 AS [AttributeValue],'A' AS [RecordStatus],GETDATE() AS [CreatedDate],GETDATE() AS [ModifiedDate] UNION ALL
	SELECT (SELECT AttributeId FROM Attribute WHERE AttributeCode='LastEnteredWeeks') AS [AttributeId],3 AS [AttributeValue],'A' AS [RecordStatus],GETDATE() AS [CreatedDate],GETDATE() AS [ModifiedDate]
)
AS A
WHERE NOT EXISTS (SELECT 1 FROM [AdminConfigValues] (NOLOCK) B WHERE B.AttributeId = A.AttributeId)

COMMIT TRANSACTION
END TRY

BEGIN CATCH
	SELECT ERROR_MESSAGE()
		,ERROR_LINE()
		,ERROR_SEVERITY()

	ROLLBACK TRANSACTION
END CATCH
GO
