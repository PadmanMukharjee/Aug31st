/*
Script Name : 01_Site
Module_Name : M3Pact
Created By  : Sampath Y
CreatedDate : 04/25/2018

Data Verification Script :

SELECT * FROM [Site](NOLOCK) WHERE RecordStatus = 'A'
*/

BEGIN TRY
BEGIN TRANSACTION

DECLARE @User NVARCHAR(30) = 'Admin'

INSERT INTO [dbo].[Site]
           ([SiteCode]
           ,[SiteName]
           ,[SiteDescription]
           ,[RecordStatus]
           ,[CreatedDate]
           ,[CreatedBy]
           ,[ModifiedDate]
           ,[ModifiedBy])
           SELECT 
            S.SiteCode
           ,S.SiteName
           ,S.SiteDescription
           ,'A' AS [RecordStatus]
           ,GETDATE() AS [CreatedDate]
           ,@User AS [CreatedBy]
           ,GETDATE() AS [ModifiedDate]
           ,@User AS [ModifiedBy]
           FROM
           (
           SELECT 'New Jersey' AS SiteCode , 'New Jersey' AS SiteName,'New Jersey' AS SiteDescription UNION ALL
           SELECT 'Washington' AS SiteCode , 'Washington' AS SiteName,'Washington' AS SiteDescription UNION ALL
           SELECT 'SouthEast' AS SiteCode , 'SouthEast' AS SiteName,'SouthEast' AS SiteDescription UNION ALL
           SELECT 'Connecticut' AS SiteCode , 'Connecticut' AS SiteName,'Connecticut' AS SiteDescription UNION ALL
           SELECT 'Kansas' AS SiteCode , 'Kansas' AS SiteName,'Kansas' AS SiteDescription UNION ALL
           SELECT 'Corporate' AS SiteCode , 'Corporate' AS SiteName,'Corporate' AS SiteDescription
           )
           AS S
 WHERE NOT EXISTS( SELECT 1 FROM [Site] (NOLOCK) B WHERE B.SiteCode=S.SiteCode) 

 UPDATE SITE 
 SET SiteCode='NJ'
 WHERE SiteName='New Jersey'
 AND SiteCode='New Jersey'

 UPDATE SITE 
 SET SiteCode='WT'
 WHERE SiteName='Washington'
  AND SiteCode='Washington'

 UPDATE SITE 
 SET SiteCode='SE'
 WHERE SiteName='SouthEast'
  AND SiteCode='SouthEast'

 UPDATE SITE 
 SET SiteCode='CT'
 WHERE SiteName='Connecticut'
  AND SiteCode='Connecticut'

 UPDATE SITE 
 SET SiteCode='KS'
 WHERE SiteName='Kansas'
  AND SiteCode='Kansas'

 UPDATE SITE 
 SET SiteCode='CP'
 WHERE SiteName='Corporate'
  AND SiteCode='Corporate'

COMMIT TRANSACTION
END TRY

BEGIN CATCH
	SELECT ERROR_MESSAGE()
		,ERROR_LINE()
		,ERROR_SEVERITY()

	ROLLBACK TRANSACTION
END CATCH
GO
