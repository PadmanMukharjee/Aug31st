/*
Script Name : 09_System
Module_Name : M3Pact
Created By  : Abhishek K
CreatedDate : 05/10/2018

Data Verification Script :

SELECT * FROM [System](NOLOCK) WHERE RecordStatus = 'A'
*/

BEGIN TRY
BEGIN TRANSACTION

DECLARE @User NVARCHAR(30) = 'Admin'

INSERT INTO [dbo].[System]
           ([SystemCode]
           ,[SystemName]
           ,[SystemDescription]
           ,[RecordStatus]
           ,[CreatedDate]
           ,[CreatedBy]
           ,[ModifiedDate]
           ,[ModifiedBy])
           SELECT 
            S.SystemCode
           ,S.SystemName
		   ,S.SystemDescription
           ,'A' AS [RecordStatus]
           ,GETDATE() AS [CreatedDate]
           ,@User AS [CreatedBy]
           ,GETDATE() AS [ModifiedDate]
           ,@User AS [ModifiedBy]
           FROM
           (
           SELECT 'Allscripts PM' AS SystemCode , 'Allscripts PM' AS SystemName , 'Allscripts PM' AS SystemDescription  UNION ALL
           SELECT 'Athena' AS SystemCode , 'Athena' AS SystemName , 'Athena' AS SystemDescription  UNION ALL
           SELECT 'Centricity Business' AS SystemCode , 'Centricity Business' AS SystemName , 'Centricity Business' AS SystemDescription  UNION ALL
           SELECT 'Centricity CPS' AS SystemCode , 'Centricity CPS' AS SystemName , 'Centricity CPS' AS SystemDescription  UNION ALL
           SELECT 'Centricity Manager' AS SystemCode , 'Centricity Manager' AS SystemName , 'Centricity Manager' AS SystemDescription  UNION ALL
           SELECT 'Eclinical Works' AS SystemCode , 'Eclinical Works' AS SystemName , 'Eclinical Works' AS SystemDescription  UNION ALL
           SELECT 'EPIC' AS SystemCode , 'EPIC' AS SystemName , 'EPIC' AS SystemDescription  UNION ALL
           SELECT 'Greenway' AS SystemCode , 'Greenway' AS SystemName , 'Greenway' AS SystemDescription  UNION ALL
           SELECT 'VertexDR' AS SystemCode , 'VertexDR' AS SystemName , 'VertexDR' AS SystemDescription  
           )
           AS S
 WHERE NOT EXISTS( SELECT 1 FROM [System] (NOLOCK) B WHERE B.SystemCode=S.SystemCode) 

COMMIT TRANSACTION
END TRY

BEGIN CATCH
	SELECT ERROR_MESSAGE()
		,ERROR_LINE()
		,ERROR_SEVERITY()

	ROLLBACK TRANSACTION
END CATCH
GO
