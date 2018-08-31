/*
Script Name : 01_ApplicationType
Module_Name : M3Pact
Created By  : Shravani P
CreatedDate : 04/25/2018

Data Verification Script : 

SELECT * FROM [ApplicationType](NOLOCK)
*/

BEGIN TRY
BEGIN TRANSACTION

INSERT INTO [dbo].[ApplicationType] ([Name])
SELECT A.[Name]
FROM
(
	SELECT 'Web-MVC' AS [Name] UNION ALL
	SELECT 'Web-JS' AS [Name] UNION ALL
	SELECT 'Native' AS [Name]
)
AS A
WHERE NOT EXISTS( SELECT 1 FROM [ApplicationType] (NOLOCK) B WHERE B.[Name]=A.[Name]) 

COMMIT TRANSACTION
END TRY

BEGIN CATCH
	SELECT ERROR_MESSAGE()
		,ERROR_LINE()
		,ERROR_SEVERITY()

	ROLLBACK TRANSACTION
END CATCH
GO