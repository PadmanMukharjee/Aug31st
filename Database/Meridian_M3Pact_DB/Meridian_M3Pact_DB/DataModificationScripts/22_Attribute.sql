/*
Script Name : 22_Attribute
Module_Name : M3Pact
Created By  : Abhishek Kovvuri
CreatedDate : 11-July-2018

Data Verification Script : 

SELECT * FROM [Attribute](NOLOCK)
*/

BEGIN TRY
BEGIN TRANSACTION

DECLARE @User NVARCHAR(30) = 'Admin'

INSERT INTO [dbo].[Attribute]
(
	[AttributeCode],
    [AttributeName],
    [AttributeDescription],
	[AttributeType],
	[RecordStatus],
    [CreatedDate],
    [CreatedBy],
    [ModifiedDate],
    [ModifiedBy]
)
SELECT
	A.[AttributeCode],
	A.[AttributeName],
	A.[AttributeDescription],
	A.[AttributeType],
	'A' AS [RecordStatus],
	GETDATE() AS [CreatedDate],
    @User AS [CreatedBy],
    GETDATE() AS [ModifiedDate],
    @User AS [ModifiedBy]
FROM
(
	SELECT 'LastEnteredBusinessDays' AS [AttributeCode], 'Projected Cash - Last Entered Business Days' AS [AttributeName] , 'LastEnteredBusinessDaysInDepositLog' AS [AttributeDescription] , 'Integer' AS [AttributeType] UNION ALL
	SELECT 'LastEnteredWeeks' AS [AttributeCode], 'Projected Cash - Last Entered Weeks' AS [AttributeName] , 'LastEnteredWeeksInDepositLog' AS [AttributeDescription] , 'Integer' AS [AttributeType]
)
AS A
WHERE NOT EXISTS (SELECT 1 FROM [Attribute] (NOLOCK) B WHERE B.AttributeCode = A.AttributeCode)

COMMIT TRANSACTION
END TRY

BEGIN CATCH
	SELECT ERROR_MESSAGE()
		,ERROR_LINE()
		,ERROR_SEVERITY()

	ROLLBACK TRANSACTION
END CATCH
GO