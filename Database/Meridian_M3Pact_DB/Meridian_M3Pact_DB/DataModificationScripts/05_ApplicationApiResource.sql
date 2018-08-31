/*
Script Name : 05_ApplicationType
Module_Name : M3Pact
Created By  : Shravani P
CreatedDate : 04/25/2018

Data Verification Script :

SELECT * FROM [ApplicationApiResource](NOLOCK)
*/

BEGIN TRY
BEGIN TRANSACTION

DECLARE @ApplicationID INT;
DECLARE @ApiResourceID INT;

SELECT @ApplicationID = ID FROM dbo.[Application] WHERE Name = 'M3Pact'
SELECT @ApiResourceID = ID FROM dbo.[ApiResource] WHERE Name = 'M3_API'

INSERT INTO dbo.[ApplicationApiResource]( [ApplicationID], [ApiResourceID] ) 
SELECT A.ApplicationID, A.ApiResourceID
FROM
(
	SELECT @ApplicationID AS ApplicationID, @ApiResourceID AS ApiResourceID
)
AS A
WHERE NOT EXISTS ( SELECT 1 FROM dbo.[ApplicationApiResource] WHERE ApplicationID = @ApplicationID AND ApiResourceID = @ApiResourceID )

COMMIT TRANSACTION
END TRY

BEGIN CATCH
	SELECT ERROR_MESSAGE()
		,ERROR_LINE()
		,ERROR_SEVERITY()

	ROLLBACK TRANSACTION
END CATCH
GO

SET QUOTED_IDENTIFIER OFF
GO