/*
Script Name : 04_ApiResource
Module_Name : M3Pact
Created By  : Shravani P
CreatedDate : 04/25/2018

Data Verification Script : 

SELECT * FROM [ApiResource](NOLOCK)
*/

BEGIN TRY
BEGIN TRANSACTION

INSERT INTO [dbo].[ApiResource] ( [Name], [Description], [Enabled] )
SELECT A.[Name], A.[Description], A.[Enabled]
FROM
(
	SELECT 'M3_API' AS [Name], 'M3Pact API' AS [Description], 1	AS [Enabled]
)
AS A
WHERE NOT EXISTS( SELECT 1 FROM [ApiResource] (NOLOCK) B WHERE B.[Name]=A.[Name]) 

COMMIT TRANSACTION
END TRY

BEGIN CATCH
	SELECT ERROR_MESSAGE()
		,ERROR_LINE()
		,ERROR_SEVERITY()

	ROLLBACK TRANSACTION
END CATCH
GO