/*
Script Name : 03_Application
Module_Name : M3Pact
Created By  : Shravani P
CreatedDate : 04/25/2018

Data Verification Script :

SELECT * FROM [Application](NOLOCK)
*/

BEGIN TRY
BEGIN TRANSACTION

DECLARE @AppTypeID INT;
SELECT @AppTypeID = ID FROM [dbo].ApplicationType WHERE [Name] = 'Web-JS'

INSERT INTO [dbo].[Application]
(
	[Name], [Key], [Secret], [TypeID], [RedirectURL]
)
SELECT [Name], [Key], [Secret], [TypeID], [RedirectURL]
FROM
(
	SELECT 
		'M3PACT' AS [Name],
		'M3pact' AS [Key],
		'secret' AS [Secret],
		@AppTypeID AS [TypeID],
		'www.m3pact.com/callback.html' AS [RedirectURL]
)
AS A
WHERE NOT EXISTS( SELECT 1 FROM [Application] (NOLOCK) B WHERE B.[Name]=A.[Name]) 

COMMIT TRANSACTION
END TRY

BEGIN CATCH
	SELECT ERROR_MESSAGE()
		,ERROR_LINE()
		,ERROR_SEVERITY()

	ROLLBACK TRANSACTION
END CATCH
GO