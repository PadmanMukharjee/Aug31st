/*
Script Name : 18_ChecklistAttribute
Module_Name : M3Pact
Created By  : Shravani P
CreatedDate : 05/17/2018

Data Verification Script :

SELECT * FROM [ChecklistAttribute](NOLOCK) WHERE RecordStatus = 'A'
*/

BEGIN TRY
BEGIN TRANSACTION

DECLARE @User NVARCHAR(30) = 'Admin'

INSERT INTO [dbo].[ChecklistAttribute]
(
	[AttributeCode],
	[AttributeName],
	[RecordStatus],
	[CreatedDate],
	[CreatedBy],
	[ModifiedDate],
	[ModifiedBy]
)
SELECT 
	A.AttributeCode,
	A.AttributeName,
	'A' AS [RecordStatus],
	GETDATE() AS [CreatedDate],
	@User AS [CreatedBy],
	GETDATE() AS [ModifiedDate],
	@User AS [ModifiedBy]
FROM
(
	SELECT 'SITE' AS AttributeCode, 'Site' AS AttributeName UNION ALL
	SELECT 'SYS' AS AttributeCode, 'System' AS AttributeName UNION ALL
	SELECT 'QUE' AS AttributeCode, 'Question' AS AttributeName
)
AS A
WHERE NOT EXISTS( SELECT 1 FROM [ChecklistAttribute] (NOLOCK) B WHERE B.AttributeCode = A.AttributeCode) 

COMMIT TRANSACTION
END TRY

BEGIN CATCH
	SELECT ERROR_MESSAGE()
		,ERROR_LINE()
		,ERROR_SEVERITY()

	ROLLBACK TRANSACTION
END CATCH
GO
