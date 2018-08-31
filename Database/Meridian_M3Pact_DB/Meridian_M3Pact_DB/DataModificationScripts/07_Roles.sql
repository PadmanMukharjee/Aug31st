/*
Script Name : 07_Roles
Module_Name : M3Pact
Created By  : Abhishek K
CreatedDate : 05/10/2018

Data Verification Script :

SELECT * FROM [Roles](NOLOCK) WHERE RecordStatus = 'A'
*/

BEGIN TRY
BEGIN TRANSACTION

DECLARE @User NVARCHAR(30) = 'Admin'

INSERT INTO [dbo].[Roles]
           ([RoleCode]
           ,[RoleDesc]
           ,[RecordStatus]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[ModifiedBy]
           ,[ModifiedDate]
           ,[Level])
           SELECT 
            R.RoleCode
           ,R.RoleDesc
           ,'A' AS [RecordStatus]
           ,@User AS [CreatedBy]
           ,GETDATE() AS [CreatedDate]
           ,@User AS [ModifiedBy]
           ,GETDATE() AS [ModifiedDate]
		   ,R.Level AS [Level]
           FROM
           (
           SELECT 'Admin' AS RoleCode , 'Admin' AS RoleDesc , 1 AS Level UNION ALL
           SELECT 'Manager' AS RoleCode , 'Manager' AS RoleDesc, 3 AS Level UNION ALL
           SELECT 'Executive' AS RoleCode , 'Executive' AS RoleDesc, 2 AS Level UNION ALL
           SELECT 'User' AS RoleCode , 'User' AS RoleDesc, 4 AS Level UNION ALL
           SELECT 'Client' AS RoleCode , 'Client' AS RoleDesc, 5 AS Level 
           )
           AS R
 WHERE NOT EXISTS( SELECT 1 FROM [Roles] (NOLOCK) B WHERE B.RoleCode=R.RoleCode) 

COMMIT TRANSACTION
END TRY

BEGIN CATCH
	SELECT ERROR_MESSAGE()
		,ERROR_LINE()
		,ERROR_SEVERITY()

	ROLLBACK TRANSACTION
END CATCH
GO
