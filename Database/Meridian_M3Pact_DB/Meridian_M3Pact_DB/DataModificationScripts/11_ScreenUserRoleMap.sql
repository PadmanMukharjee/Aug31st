/*
Script Name : 10_Screen
Module_Name : M3Pact
Created By  : Abhishek K
CreatedDate : 05/11/2018

Revision History 
=============
Ver  Date				Who						Comment
1.0  10-May-2018		Abhishek Kovvuri		Scripts to insert data
1.1  08-Aug-2018        Abhishek Kovvuri        Scripts to insert to-do-list screen user roles.

Data Verification Script :

SELECT * FROM [ScreenUserRoleMap](NOLOCK) WHERE RecordStatus = 'A'
*/


BEGIN TRY
BEGIN TRANSACTION

DECLARE @UserName NVARCHAR(60) = 'Admin'
DECLARE @CurrentDate DATETIME = GETDATE()
DECLARE @RecordStatus CHAR(1) = 'A'
DECLARE @ScreenId INT;
DECLARE @RoleId INT;

SELECT  @ScreenId = ScreenId FROM Screen WHERE ScreenCode = 'DL'
SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'Admin'
IF NOT EXISTS ( SELECT TOP 1 1 FROM ScreenUserRoleMap WHERE ScreenId = @ScreenId AND RoleId = @RoleId)
	BEGIN
		INSERT INTO [dbo].[ScreenUserRoleMap]
           ([ScreenId]
           ,[RoleId]
           ,[RecordStatus]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[ModifiedBy]
           ,[ModifiedDate]
           ,[CanEdit]
           ,[DisplayScreenName])
		VALUES
		(
			@ScreenId, 			
			@RoleId,
			@RecordStatus,	
			@UserName,				
			@CurrentDate,	
			@UserName,				
			@CurrentDate,	
			1,
			'Deposit Log'
		)
	END

SELECT  @ScreenId = ScreenId FROM Screen WHERE ScreenCode = 'DL'
SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'Manager'
IF NOT EXISTS ( SELECT TOP 1 1 FROM ScreenUserRoleMap WHERE ScreenId = @ScreenId AND RoleId = @RoleId)
	BEGIN
		INSERT INTO [dbo].[ScreenUserRoleMap]
           ([ScreenId]
           ,[RoleId]
           ,[RecordStatus]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[ModifiedBy]
           ,[ModifiedDate]
           ,[CanEdit]
           ,[DisplayScreenName])
		VALUES
		(
			@ScreenId, 			
			@RoleId,
			@RecordStatus,	
			@UserName,				
			@CurrentDate,	
			@UserName,				
			@CurrentDate,	
			1,
			'Deposit Log'
		)
	END

SELECT  @ScreenId = ScreenId FROM Screen WHERE ScreenCode = 'DL'
SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'Executive'
IF NOT EXISTS ( SELECT TOP 1 1 FROM ScreenUserRoleMap WHERE ScreenId = @ScreenId AND RoleId = @RoleId)
	BEGIN
		INSERT INTO [dbo].[ScreenUserRoleMap]
           ([ScreenId]
           ,[RoleId]
           ,[RecordStatus]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[ModifiedBy]
           ,[ModifiedDate]
           ,[CanEdit]
           ,[DisplayScreenName])
		VALUES
		(
			@ScreenId, 			
			@RoleId,
			@RecordStatus,	
			@UserName,				
			@CurrentDate,	
			@UserName,				
			@CurrentDate,	
			1,
			'Deposit Log'
		)
	END

SELECT  @ScreenId = ScreenId FROM Screen WHERE ScreenCode = 'DL'
SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'User'
IF NOT EXISTS ( SELECT TOP 1 1 FROM ScreenUserRoleMap WHERE ScreenId = @ScreenId AND RoleId = @RoleId)
	BEGIN
		INSERT INTO [dbo].[ScreenUserRoleMap]
           ([ScreenId]
           ,[RoleId]
           ,[RecordStatus]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[ModifiedBy]
           ,[ModifiedDate]
           ,[CanEdit]
           ,[DisplayScreenName])
		VALUES
		(
			@ScreenId, 			
			@RoleId,
			@RecordStatus,	
			@UserName,				
			@CurrentDate,	
			@UserName,				
			@CurrentDate,	
			1,
			'Deposit Log'
		)
	END

SELECT  @ScreenId = ScreenId FROM Screen WHERE ScreenCode = 'DL'
SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'Client'
IF NOT EXISTS ( SELECT TOP 1 1 FROM ScreenUserRoleMap WHERE ScreenId = @ScreenId AND RoleId = @RoleId)
	BEGIN
		INSERT INTO [dbo].[ScreenUserRoleMap]
           ([ScreenId]
           ,[RoleId]
           ,[RecordStatus]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[ModifiedBy]
           ,[ModifiedDate]
           ,[CanEdit]
           ,[DisplayScreenName])
		VALUES
		(
			@ScreenId, 			
			@RoleId,
			@RecordStatus,	
			@UserName,				
			@CurrentDate,	
			@UserName,				
			@CurrentDate,	
			1,
			'Deposit Log'
		)
	END

SELECT  @ScreenId = ScreenId FROM Screen WHERE ScreenCode = 'HM'
SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'Manager'
IF NOT EXISTS ( SELECT TOP 1 1 FROM ScreenUserRoleMap WHERE ScreenId = @ScreenId AND RoleId = @RoleId)
	BEGIN
		INSERT INTO [dbo].[ScreenUserRoleMap]
           ([ScreenId]
           ,[RoleId]
           ,[RecordStatus]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[ModifiedBy]
           ,[ModifiedDate]
           ,[CanEdit]
           ,[DisplayScreenName])
		VALUES
		(
			@ScreenId, 			
			@RoleId,
			@RecordStatus,	
			@UserName,				
			@CurrentDate,	
			@UserName,				
			@CurrentDate,	
			NULL,
			'Heat Map'
		)
	END

SELECT  @ScreenId = ScreenId FROM Screen WHERE ScreenCode = 'HM'
SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'Executive'
IF NOT EXISTS ( SELECT TOP 1 1 FROM ScreenUserRoleMap WHERE ScreenId = @ScreenId AND RoleId = @RoleId)
	BEGIN
		INSERT INTO [dbo].[ScreenUserRoleMap]
           ([ScreenId]
           ,[RoleId]
           ,[RecordStatus]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[ModifiedBy]
           ,[ModifiedDate]
           ,[CanEdit]
           ,[DisplayScreenName])
		VALUES
		(
			@ScreenId, 			
			@RoleId,
			@RecordStatus,	
			@UserName,				
			@CurrentDate,	
			@UserName,				
			@CurrentDate,	
			NULL,
			'Heat Map'
		)
	END

SELECT  @ScreenId = ScreenId FROM Screen WHERE ScreenCode = 'HM'
SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'Admin'
IF NOT EXISTS ( SELECT TOP 1 1 FROM ScreenUserRoleMap WHERE ScreenId = @ScreenId AND RoleId = @RoleId)
	BEGIN
		INSERT INTO [dbo].[ScreenUserRoleMap]
           ([ScreenId]
           ,[RoleId]
           ,[RecordStatus]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[ModifiedBy]
           ,[ModifiedDate]
           ,[CanEdit]
           ,[DisplayScreenName])
		VALUES
		(
			@ScreenId, 			
			@RoleId,
			@RecordStatus,	
			@UserName,				
			@CurrentDate,	
			@UserName,				
			@CurrentDate,	
			NULL,
			'Heat Map'
		)
	END

SELECT  @ScreenId = ScreenId FROM Screen WHERE ScreenCode = 'CL'
SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'Admin'
IF NOT EXISTS ( SELECT TOP 1 1 FROM ScreenUserRoleMap WHERE ScreenId = @ScreenId AND RoleId = @RoleId)
	BEGIN
		INSERT INTO [dbo].[ScreenUserRoleMap]
           ([ScreenId]
           ,[RoleId]
           ,[RecordStatus]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[ModifiedBy]
           ,[ModifiedDate]
           ,[CanEdit]
           ,[DisplayScreenName])
		VALUES
		(
			@ScreenId, 			
			@RoleId,
			@RecordStatus,	
			@UserName,				
			@CurrentDate,	
			@UserName,				
			@CurrentDate,	
			NULL,
			'CheckLists'
		)
	END

SELECT  @ScreenId = ScreenId FROM Screen WHERE ScreenCode = 'CL'
SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'Manager'
IF NOT EXISTS ( SELECT TOP 1 1 FROM ScreenUserRoleMap WHERE ScreenId = @ScreenId AND RoleId = @RoleId)
	BEGIN
		INSERT INTO [dbo].[ScreenUserRoleMap]
           ([ScreenId]
           ,[RoleId]
           ,[RecordStatus]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[ModifiedBy]
           ,[ModifiedDate]
           ,[CanEdit]
           ,[DisplayScreenName])
		VALUES
		(
			@ScreenId, 			
			@RoleId,
			@RecordStatus,	
			@UserName,				
			@CurrentDate,	
			@UserName,				
			@CurrentDate,	
			NULL,
			'CheckLists'
		)
	END

SELECT  @ScreenId = ScreenId FROM Screen WHERE ScreenCode = 'CL'
SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'Executive'
IF NOT EXISTS ( SELECT TOP 1 1 FROM ScreenUserRoleMap WHERE ScreenId = @ScreenId AND RoleId = @RoleId)
	BEGIN
		INSERT INTO [dbo].[ScreenUserRoleMap]
           ([ScreenId]
           ,[RoleId]
           ,[RecordStatus]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[ModifiedBy]
           ,[ModifiedDate]
           ,[CanEdit]
           ,[DisplayScreenName])
		VALUES
		(
			@ScreenId, 			
			@RoleId,
			@RecordStatus,	
			@UserName,				
			@CurrentDate,	
			@UserName,				
			@CurrentDate,	
			NULL,
			'CheckLists'
		)
	END

SELECT  @ScreenId = ScreenId FROM Screen WHERE ScreenCode = 'CL'
SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'User'
IF NOT EXISTS ( SELECT TOP 1 1 FROM ScreenUserRoleMap WHERE ScreenId = @ScreenId AND RoleId = @RoleId)
	BEGIN
		INSERT INTO [dbo].[ScreenUserRoleMap]
           ([ScreenId]
           ,[RoleId]
           ,[RecordStatus]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[ModifiedBy]
           ,[ModifiedDate]
           ,[CanEdit]
           ,[DisplayScreenName])
		VALUES
		(
			@ScreenId, 			
			@RoleId,
			@RecordStatus,	
			@UserName,				
			@CurrentDate,	
			@UserName,				
			@CurrentDate,	
			NULL,
			'CheckLists'
		)
	END

SELECT  @ScreenId = ScreenId FROM Screen WHERE ScreenCode = 'PCL'
SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'User'
IF NOT EXISTS ( SELECT TOP 1 1 FROM ScreenUserRoleMap WHERE ScreenId = @ScreenId AND RoleId = @RoleId)
	BEGIN
		INSERT INTO [dbo].[ScreenUserRoleMap]
           ([ScreenId]
           ,[RoleId]
           ,[RecordStatus]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[ModifiedBy]
           ,[ModifiedDate]
           ,[CanEdit]
           ,[DisplayScreenName])
		VALUES
		(
			@ScreenId, 			
			@RoleId,
			@RecordStatus,	
			@UserName,				
			@CurrentDate,	
			@UserName,				
			@CurrentDate,	
			NULL,
			'Pending CheckLists'
		)
	END

SELECT  @ScreenId = ScreenId FROM Screen WHERE ScreenCode = 'PCL'
SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'Executive'
IF NOT EXISTS ( SELECT TOP 1 1 FROM ScreenUserRoleMap WHERE ScreenId = @ScreenId AND RoleId = @RoleId)
	BEGIN
		INSERT INTO [dbo].[ScreenUserRoleMap]
           ([ScreenId]
           ,[RoleId]
           ,[RecordStatus]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[ModifiedBy]
           ,[ModifiedDate]
           ,[CanEdit]
           ,[DisplayScreenName])
		VALUES
		(
			@ScreenId, 			
			@RoleId,
			@RecordStatus,	
			@UserName,				
			@CurrentDate,	
			@UserName,				
			@CurrentDate,	
			NULL,
			'Pending CheckLists'
		)
	END

SELECT  @ScreenId = ScreenId FROM Screen WHERE ScreenCode = 'PCL'
SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'Manager'
IF NOT EXISTS ( SELECT TOP 1 1 FROM ScreenUserRoleMap WHERE ScreenId = @ScreenId AND RoleId = @RoleId)
	BEGIN
		INSERT INTO [dbo].[ScreenUserRoleMap]
           ([ScreenId]
           ,[RoleId]
           ,[RecordStatus]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[ModifiedBy]
           ,[ModifiedDate]
           ,[CanEdit]
           ,[DisplayScreenName])
		VALUES
		(
			@ScreenId, 			
			@RoleId,
			@RecordStatus,	
			@UserName,				
			@CurrentDate,	
			@UserName,				
			@CurrentDate,	
			NULL,
			'Pending CheckLists'
		)
	END

SELECT  @ScreenId = ScreenId FROM Screen WHERE ScreenCode = 'PCL'
SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'Admin'
IF NOT EXISTS ( SELECT TOP 1 1 FROM ScreenUserRoleMap WHERE ScreenId = @ScreenId AND RoleId = @RoleId)
	BEGIN
		INSERT INTO [dbo].[ScreenUserRoleMap]
           ([ScreenId]
           ,[RoleId]
           ,[RecordStatus]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[ModifiedBy]
           ,[ModifiedDate]
           ,[CanEdit]
           ,[DisplayScreenName])
		VALUES
		(
			@ScreenId, 			
			@RoleId,
			@RecordStatus,	
			@UserName,				
			@CurrentDate,	
			@UserName,				
			@CurrentDate,	
			NULL,
			'Pending CheckLists'
		)
	END

SELECT  @ScreenId = ScreenId FROM Screen WHERE ScreenCode = 'CCL'
SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'Admin'
IF NOT EXISTS ( SELECT TOP 1 1 FROM ScreenUserRoleMap WHERE ScreenId = @ScreenId AND RoleId = @RoleId)
	BEGIN
		INSERT INTO [dbo].[ScreenUserRoleMap]
           ([ScreenId]
           ,[RoleId]
           ,[RecordStatus]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[ModifiedBy]
           ,[ModifiedDate]
           ,[CanEdit]
           ,[DisplayScreenName])
		VALUES
		(
			@ScreenId, 			
			@RoleId,
			@RecordStatus,	
			@UserName,				
			@CurrentDate,	
			@UserName,				
			@CurrentDate,	
			1,
			'Completed CheckLists'
		)
	END

SELECT  @ScreenId = ScreenId FROM Screen WHERE ScreenCode = 'CCL'
SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'Manager'
IF NOT EXISTS ( SELECT TOP 1 1 FROM ScreenUserRoleMap WHERE ScreenId = @ScreenId AND RoleId = @RoleId)
	BEGIN
		INSERT INTO [dbo].[ScreenUserRoleMap]
           ([ScreenId]
           ,[RoleId]
           ,[RecordStatus]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[ModifiedBy]
           ,[ModifiedDate]
           ,[CanEdit]
           ,[DisplayScreenName])
		VALUES
		(
			@ScreenId, 			
			@RoleId,
			@RecordStatus,	
			@UserName,				
			@CurrentDate,	
			@UserName,				
			@CurrentDate,	
			0,
			'Completed CheckLists'
		)
	END

SELECT  @ScreenId = ScreenId FROM Screen WHERE ScreenCode = 'CCL'
SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'Executive'
IF NOT EXISTS ( SELECT TOP 1 1 FROM ScreenUserRoleMap WHERE ScreenId = @ScreenId AND RoleId = @RoleId)
	BEGIN
		INSERT INTO [dbo].[ScreenUserRoleMap]
           ([ScreenId]
           ,[RoleId]
           ,[RecordStatus]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[ModifiedBy]
           ,[ModifiedDate]
           ,[CanEdit]
           ,[DisplayScreenName])
		VALUES
		(
			@ScreenId, 			
			@RoleId,
			@RecordStatus,	
			@UserName,				
			@CurrentDate,	
			@UserName,				
			@CurrentDate,	
			0,
			'Completed CheckLists'
		)
	END

SELECT  @ScreenId = ScreenId FROM Screen WHERE ScreenCode = 'CCL'
SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'User'
IF NOT EXISTS ( SELECT TOP 1 1 FROM ScreenUserRoleMap WHERE ScreenId = @ScreenId AND RoleId = @RoleId)
	BEGIN
		INSERT INTO [dbo].[ScreenUserRoleMap]
           ([ScreenId]
           ,[RoleId]
           ,[RecordStatus]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[ModifiedBy]
           ,[ModifiedDate]
           ,[CanEdit]
           ,[DisplayScreenName])
		VALUES
		(
			@ScreenId, 			
			@RoleId,
			@RecordStatus,	
			@UserName,				
			@CurrentDate,	
			@UserName,				
			@CurrentDate,	
			0,
			'Completed CheckLists'
		)
	END

SELECT  @ScreenId = ScreenId FROM Screen WHERE ScreenCode = 'CLI'
SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'Admin'
IF NOT EXISTS ( SELECT TOP 1 1 FROM ScreenUserRoleMap WHERE ScreenId = @ScreenId AND RoleId = @RoleId)
	BEGIN
		INSERT INTO [dbo].[ScreenUserRoleMap]
           ([ScreenId]
           ,[RoleId]
           ,[RecordStatus]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[ModifiedBy]
           ,[ModifiedDate]
           ,[CanEdit]
           ,[DisplayScreenName])
		VALUES
		(
			@ScreenId, 			
			@RoleId,
			@RecordStatus,	
			@UserName,				
			@CurrentDate,	
			@UserName,				
			@CurrentDate,	
			NULL,
			'Clients'
		)
	END

SELECT  @ScreenId = ScreenId FROM Screen WHERE ScreenCode = 'CLI'
SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'Manager'
IF NOT EXISTS ( SELECT TOP 1 1 FROM ScreenUserRoleMap WHERE ScreenId = @ScreenId AND RoleId = @RoleId)
	BEGIN
		INSERT INTO [dbo].[ScreenUserRoleMap]
           ([ScreenId]
           ,[RoleId]
           ,[RecordStatus]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[ModifiedBy]
           ,[ModifiedDate]
           ,[CanEdit]
           ,[DisplayScreenName])
		VALUES
		(
			@ScreenId, 			
			@RoleId,
			@RecordStatus,	
			@UserName,				
			@CurrentDate,	
			@UserName,				
			@CurrentDate,	
			NULL,
			'Clients'
		)
	END

SELECT  @ScreenId = ScreenId FROM Screen WHERE ScreenCode = 'CLI'
SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'Executive'
IF NOT EXISTS ( SELECT TOP 1 1 FROM ScreenUserRoleMap WHERE ScreenId = @ScreenId AND RoleId = @RoleId)
	BEGIN
		INSERT INTO [dbo].[ScreenUserRoleMap]
           ([ScreenId]
           ,[RoleId]
           ,[RecordStatus]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[ModifiedBy]
           ,[ModifiedDate]
           ,[CanEdit]
           ,[DisplayScreenName])
		VALUES
		(
			@ScreenId, 			
			@RoleId,
			@RecordStatus,	
			@UserName,				
			@CurrentDate,	
			@UserName,				
			@CurrentDate,	
			NULL,
			'Clients'
		)
	END

SELECT  @ScreenId = ScreenId FROM Screen WHERE ScreenCode = 'CLI'
SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'User'
IF NOT EXISTS ( SELECT TOP 1 1 FROM ScreenUserRoleMap WHERE ScreenId = @ScreenId AND RoleId = @RoleId)
	BEGIN
		INSERT INTO [dbo].[ScreenUserRoleMap]
           ([ScreenId]
           ,[RoleId]
           ,[RecordStatus]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[ModifiedBy]
           ,[ModifiedDate]
           ,[CanEdit]
           ,[DisplayScreenName])
		VALUES
		(
			@ScreenId, 			
			@RoleId,
			@RecordStatus,	
			@UserName,				
			@CurrentDate,	
			@UserName,				
			@CurrentDate,	
			NULL,
			'Clients'
		)
	END

SELECT  @ScreenId = ScreenId FROM Screen WHERE ScreenCode = 'CLI'
SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'Client'
IF NOT EXISTS ( SELECT TOP 1 1 FROM ScreenUserRoleMap WHERE ScreenId = @ScreenId AND RoleId = @RoleId)
	BEGIN
		INSERT INTO [dbo].[ScreenUserRoleMap]
           ([ScreenId]
           ,[RoleId]
           ,[RecordStatus]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[ModifiedBy]
           ,[ModifiedDate]
           ,[CanEdit]
           ,[DisplayScreenName])
		VALUES
		(
			@ScreenId, 			
			@RoleId,
			@RecordStatus,	
			@UserName,				
			@CurrentDate,	
			@UserName,				
			@CurrentDate,	
			NULL,
			'Clients'
		)
	END

SELECT  @ScreenId = ScreenId FROM Screen WHERE ScreenCode = 'CVE'
SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'Admin'
IF NOT EXISTS ( SELECT TOP 1 1 FROM ScreenUserRoleMap WHERE ScreenId = @ScreenId AND RoleId = @RoleId)
	BEGIN
		INSERT INTO [dbo].[ScreenUserRoleMap]
           ([ScreenId]
           ,[RoleId]
           ,[RecordStatus]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[ModifiedBy]
           ,[ModifiedDate]
           ,[CanEdit]
           ,[DisplayScreenName])
		VALUES
		(
			@ScreenId, 			
			@RoleId,
			@RecordStatus,	
			@UserName,				
			@CurrentDate,	
			@UserName,				
			@CurrentDate,	
			NULL,
			'View/Edit'
		)
	END

SELECT  @ScreenId = ScreenId FROM Screen WHERE ScreenCode = 'CVE'
SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'Manager'
IF NOT EXISTS ( SELECT TOP 1 1 FROM ScreenUserRoleMap WHERE ScreenId = @ScreenId AND RoleId = @RoleId)
	BEGIN
		INSERT INTO [dbo].[ScreenUserRoleMap]
           ([ScreenId]
           ,[RoleId]
           ,[RecordStatus]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[ModifiedBy]
           ,[ModifiedDate]
           ,[CanEdit]
           ,[DisplayScreenName])
		VALUES
		(
			@ScreenId, 			
			@RoleId,
			@RecordStatus,	
			@UserName,				
			@CurrentDate,	
			@UserName,				
			@CurrentDate,	
			NULL,
			'View/Edit'
		)
	END

SELECT  @ScreenId = ScreenId FROM Screen WHERE ScreenCode = 'CVE'
SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'Executive'
IF NOT EXISTS ( SELECT TOP 1 1 FROM ScreenUserRoleMap WHERE ScreenId = @ScreenId AND RoleId = @RoleId)
	BEGIN
		INSERT INTO [dbo].[ScreenUserRoleMap]
           ([ScreenId]
           ,[RoleId]
           ,[RecordStatus]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[ModifiedBy]
           ,[ModifiedDate]
           ,[CanEdit]
           ,[DisplayScreenName])
		VALUES
		(
			@ScreenId, 			
			@RoleId,
			@RecordStatus,	
			@UserName,				
			@CurrentDate,	
			@UserName,				
			@CurrentDate,	
			NULL,
			'View/Edit'
		)
	END

SELECT  @ScreenId = ScreenId FROM Screen WHERE ScreenCode = 'CVE'
SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'User'
IF NOT EXISTS ( SELECT TOP 1 1 FROM ScreenUserRoleMap WHERE ScreenId = @ScreenId AND RoleId = @RoleId)
	BEGIN
		INSERT INTO [dbo].[ScreenUserRoleMap]
           ([ScreenId]
           ,[RoleId]
           ,[RecordStatus]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[ModifiedBy]
           ,[ModifiedDate]
           ,[CanEdit]
           ,[DisplayScreenName])
		VALUES
		(
			@ScreenId, 			
			@RoleId,
			@RecordStatus,	
			@UserName,				
			@CurrentDate,	
			@UserName,				
			@CurrentDate,	
			NULL,
			'View/Edit'
		)
	END

SELECT  @ScreenId = ScreenId FROM Screen WHERE ScreenCode = 'CVE'
SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'Client'
IF NOT EXISTS ( SELECT TOP 1 1 FROM ScreenUserRoleMap WHERE ScreenId = @ScreenId AND RoleId = @RoleId)
	BEGIN
		INSERT INTO [dbo].[ScreenUserRoleMap]
           ([ScreenId]
           ,[RoleId]
           ,[RecordStatus]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[ModifiedBy]
           ,[ModifiedDate]
           ,[CanEdit]
           ,[DisplayScreenName])
		VALUES
		(
			@ScreenId, 			
			@RoleId,
			@RecordStatus,	
			@UserName,				
			@CurrentDate,	
			@UserName,				
			@CurrentDate,	
			NULL,
			'View/Edit'
		)
	END

SELECT  @ScreenId = ScreenId FROM Screen WHERE ScreenCode = 'CN'
SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'Admin'
IF NOT EXISTS ( SELECT TOP 1 1 FROM ScreenUserRoleMap WHERE ScreenId = @ScreenId AND RoleId = @RoleId)
	BEGIN
		INSERT INTO [dbo].[ScreenUserRoleMap]
           ([ScreenId]
           ,[RoleId]
           ,[RecordStatus]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[ModifiedBy]
           ,[ModifiedDate]
           ,[CanEdit]
           ,[DisplayScreenName])
		VALUES
		(
			@ScreenId, 			
			@RoleId,
			@RecordStatus,	
			@UserName,				
			@CurrentDate,	
			@UserName,				
			@CurrentDate,	
			NULL,
			'Create New'
		)
	END

SELECT  @ScreenId = ScreenId FROM Screen WHERE ScreenCode = 'VA'
SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'Admin'
IF NOT EXISTS ( SELECT TOP 1 1 FROM ScreenUserRoleMap WHERE ScreenId = @ScreenId AND RoleId = @RoleId)
	BEGIN
		INSERT INTO [dbo].[ScreenUserRoleMap]
           ([ScreenId]
           ,[RoleId]
           ,[RecordStatus]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[ModifiedBy]
           ,[ModifiedDate]
           ,[CanEdit]
           ,[DisplayScreenName])
		VALUES
		(
			@ScreenId, 			
			@RoleId,
			@RecordStatus,	
			@UserName,				
			@CurrentDate,	
			@UserName,				
			@CurrentDate,	
			1,
			'View All'
		)
	END

SELECT  @ScreenId = ScreenId FROM Screen WHERE ScreenCode = 'VA'
SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'Manager'
IF NOT EXISTS ( SELECT TOP 1 1 FROM ScreenUserRoleMap WHERE ScreenId = @ScreenId AND RoleId = @RoleId)
	BEGIN
		INSERT INTO [dbo].[ScreenUserRoleMap]
           ([ScreenId]
           ,[RoleId]
           ,[RecordStatus]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[ModifiedBy]
           ,[ModifiedDate]
           ,[CanEdit]
           ,[DisplayScreenName])
		VALUES
		(
			@ScreenId, 			
			@RoleId,
			@RecordStatus,	
			@UserName,				
			@CurrentDate,	
			@UserName,				
			@CurrentDate,	
			1,
			'View All'
		)
	END

SELECT  @ScreenId = ScreenId FROM Screen WHERE ScreenCode = 'VA'
SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'Executive'
IF NOT EXISTS ( SELECT TOP 1 1 FROM ScreenUserRoleMap WHERE ScreenId = @ScreenId AND RoleId = @RoleId)
	BEGIN
		INSERT INTO [dbo].[ScreenUserRoleMap]
           ([ScreenId]
           ,[RoleId]
           ,[RecordStatus]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[ModifiedBy]
           ,[ModifiedDate]
           ,[CanEdit]
           ,[DisplayScreenName])
		VALUES
		(
			@ScreenId, 			
			@RoleId,
			@RecordStatus,	
			@UserName,				
			@CurrentDate,	
			@UserName,				
			@CurrentDate,	
			1,
			'View All'
		)
	END

SELECT  @ScreenId = ScreenId FROM Screen WHERE ScreenCode = 'VA'
SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'User'
IF NOT EXISTS ( SELECT TOP 1 1 FROM ScreenUserRoleMap WHERE ScreenId = @ScreenId AND RoleId = @RoleId)
	BEGIN
		INSERT INTO [dbo].[ScreenUserRoleMap]
           ([ScreenId]
           ,[RoleId]
           ,[RecordStatus]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[ModifiedBy]
           ,[ModifiedDate]
           ,[CanEdit]
           ,[DisplayScreenName])
		VALUES
		(
			@ScreenId, 			
			@RoleId,
			@RecordStatus,	
			@UserName,				
			@CurrentDate,	
			@UserName,				
			@CurrentDate,	
			1,
			'View All'
		)
	END

SELECT  @ScreenId = ScreenId FROM Screen WHERE ScreenCode = 'VA'
SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'Client'
IF NOT EXISTS ( SELECT TOP 1 1 FROM ScreenUserRoleMap WHERE ScreenId = @ScreenId AND RoleId = @RoleId)
	BEGIN
		INSERT INTO [dbo].[ScreenUserRoleMap]
           ([ScreenId]
           ,[RoleId]
           ,[RecordStatus]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[ModifiedBy]
           ,[ModifiedDate]
           ,[CanEdit]
           ,[DisplayScreenName])
		VALUES
		(
			@ScreenId, 			
			@RoleId,
			@RecordStatus,	
			@UserName,				
			@CurrentDate,	
			@UserName,				
			@CurrentDate,	
			0,
			'View All'
		)
	END

SELECT  @ScreenId = ScreenId FROM Screen WHERE ScreenCode = 'ADMIN'
SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'Admin'
IF NOT EXISTS ( SELECT TOP 1 1 FROM ScreenUserRoleMap WHERE ScreenId = @ScreenId AND RoleId = @RoleId)
	BEGIN
		INSERT INTO [dbo].[ScreenUserRoleMap]
           ([ScreenId]
           ,[RoleId]
           ,[RecordStatus]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[ModifiedBy]
           ,[ModifiedDate]
           ,[CanEdit]
           ,[DisplayScreenName])
		VALUES
		(
			@ScreenId, 			
			@RoleId,
			@RecordStatus,	
			@UserName,				
			@CurrentDate,	
			@UserName,				
			@CurrentDate,	
			NULL,
			'Administration'
		)
	END

SELECT  @ScreenId = ScreenId FROM Screen WHERE ScreenCode = 'ADMIN'
SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'Manager'
IF NOT EXISTS ( SELECT TOP 1 1 FROM ScreenUserRoleMap WHERE ScreenId = @ScreenId AND RoleId = @RoleId)
	BEGIN
		INSERT INTO [dbo].[ScreenUserRoleMap]
           ([ScreenId]
           ,[RoleId]
           ,[RecordStatus]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[ModifiedBy]
           ,[ModifiedDate]
           ,[CanEdit]
           ,[DisplayScreenName])
		VALUES
		(
			@ScreenId, 			
			@RoleId,
			@RecordStatus,	
			@UserName,				
			@CurrentDate,	
			@UserName,				
			@CurrentDate,	
			NULL,
			'Administration'
		)
	END

SELECT  @ScreenId = ScreenId FROM Screen WHERE ScreenCode = 'ADMIN'
SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'Executive'
IF NOT EXISTS ( SELECT TOP 1 1 FROM ScreenUserRoleMap WHERE ScreenId = @ScreenId AND RoleId = @RoleId)
	BEGIN
		INSERT INTO [dbo].[ScreenUserRoleMap]
           ([ScreenId]
           ,[RoleId]
           ,[RecordStatus]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[ModifiedBy]
           ,[ModifiedDate]
           ,[CanEdit]
           ,[DisplayScreenName])
		VALUES
		(
			@ScreenId, 			
			@RoleId,
			@RecordStatus,	
			@UserName,				
			@CurrentDate,	
			@UserName,				
			@CurrentDate,	
			NULL,
			'Administration'
		)
	END

SELECT  @ScreenId = ScreenId FROM Screen WHERE ScreenCode = 'ADMIN'
SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'User'
IF NOT EXISTS ( SELECT TOP 1 1 FROM ScreenUserRoleMap WHERE ScreenId = @ScreenId AND RoleId = @RoleId)
	BEGIN
		INSERT INTO [dbo].[ScreenUserRoleMap]
           ([ScreenId]
           ,[RoleId]
           ,[RecordStatus]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[ModifiedBy]
           ,[ModifiedDate]
           ,[CanEdit]
           ,[DisplayScreenName])
		VALUES
		(
			@ScreenId, 			
			@RoleId,
			@RecordStatus,	
			@UserName,				
			@CurrentDate,	
			@UserName,				
			@CurrentDate,	
			NULL,
			'Administration'
		)
	END

SELECT  @ScreenId = ScreenId FROM Screen WHERE ScreenCode = 'BD'
SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'Admin'
IF NOT EXISTS ( SELECT TOP 1 1 FROM ScreenUserRoleMap WHERE ScreenId = @ScreenId AND RoleId = @RoleId)
	BEGIN
		INSERT INTO [dbo].[ScreenUserRoleMap]
           ([ScreenId]
           ,[RoleId]
           ,[RecordStatus]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[ModifiedBy]
           ,[ModifiedDate]
           ,[CanEdit]
           ,[DisplayScreenName])
		VALUES
		(
			@ScreenId, 			
			@RoleId,
			@RecordStatus,	
			@UserName,				
			@CurrentDate,	
			@UserName,				
			@CurrentDate,	
			1,
			'Business Days'
		)
	END

SELECT  @ScreenId = ScreenId FROM Screen WHERE ScreenCode = 'BD'
SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'Executive'
IF NOT EXISTS ( SELECT TOP 1 1 FROM ScreenUserRoleMap WHERE ScreenId = @ScreenId AND RoleId = @RoleId)
	BEGIN
		INSERT INTO [dbo].[ScreenUserRoleMap]
           ([ScreenId]
           ,[RoleId]
           ,[RecordStatus]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[ModifiedBy]
           ,[ModifiedDate]
           ,[CanEdit]
           ,[DisplayScreenName])
		VALUES
		(
			@ScreenId, 			
			@RoleId,
			@RecordStatus,	
			@UserName,				
			@CurrentDate,	
			@UserName,				
			@CurrentDate,	
			0,
			'Business Days'
		)
	END

SELECT  @ScreenId = ScreenId FROM Screen WHERE ScreenCode = 'SITES'
SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'Executive'
IF NOT EXISTS ( SELECT TOP 1 1 FROM ScreenUserRoleMap WHERE ScreenId = @ScreenId AND RoleId = @RoleId)
	BEGIN
		INSERT INTO [dbo].[ScreenUserRoleMap]
           ([ScreenId]
           ,[RoleId]
           ,[RecordStatus]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[ModifiedBy]
           ,[ModifiedDate]
           ,[CanEdit]
           ,[DisplayScreenName])
		VALUES
		(
			@ScreenId, 			
			@RoleId,
			@RecordStatus,	
			@UserName,				
			@CurrentDate,	
			@UserName,				
			@CurrentDate,	
			0,
			'Sites'
		)
	END

SELECT  @ScreenId = ScreenId FROM Screen WHERE ScreenCode = 'SITES'
SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'Admin'
IF NOT EXISTS ( SELECT TOP 1 1 FROM ScreenUserRoleMap WHERE ScreenId = @ScreenId AND RoleId = @RoleId)
	BEGIN
		INSERT INTO [dbo].[ScreenUserRoleMap]
           ([ScreenId]
           ,[RoleId]
           ,[RecordStatus]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[ModifiedBy]
           ,[ModifiedDate]
           ,[CanEdit]
           ,[DisplayScreenName])
		VALUES
		(
			@ScreenId, 			
			@RoleId,
			@RecordStatus,	
			@UserName,				
			@CurrentDate,	
			@UserName,				
			@CurrentDate,	
			1,
			'Sites'
		)
	END

SELECT  @ScreenId = ScreenId FROM Screen WHERE ScreenCode = 'BU'
SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'Admin'
IF NOT EXISTS ( SELECT TOP 1 1 FROM ScreenUserRoleMap WHERE ScreenId = @ScreenId AND RoleId = @RoleId)
	BEGIN
		INSERT INTO [dbo].[ScreenUserRoleMap]
           ([ScreenId]
           ,[RoleId]
           ,[RecordStatus]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[ModifiedBy]
           ,[ModifiedDate]
           ,[CanEdit]
           ,[DisplayScreenName])
		VALUES
		(
			@ScreenId, 			
			@RoleId,
			@RecordStatus,	
			@UserName,				
			@CurrentDate,	
			@UserName,				
			@CurrentDate,	
			1,
			'Business Units'
		)
	END

SELECT  @ScreenId = ScreenId FROM Screen WHERE ScreenCode = 'BU'
SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'Executive'
IF NOT EXISTS ( SELECT TOP 1 1 FROM ScreenUserRoleMap WHERE ScreenId = @ScreenId AND RoleId = @RoleId)
	BEGIN
		INSERT INTO [dbo].[ScreenUserRoleMap]
           ([ScreenId]
           ,[RoleId]
           ,[RecordStatus]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[ModifiedBy]
           ,[ModifiedDate]
           ,[CanEdit]
           ,[DisplayScreenName])
		VALUES
		(
			@ScreenId, 			
			@RoleId,
			@RecordStatus,	
			@UserName,				
			@CurrentDate,	
			@UserName,				
			@CurrentDate,	
			0,
			'Business Units'
		)
	END

SELECT  @ScreenId = ScreenId FROM Screen WHERE ScreenCode = 'ACL'
SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'Executive'
IF NOT EXISTS ( SELECT TOP 1 1 FROM ScreenUserRoleMap WHERE ScreenId = @ScreenId AND RoleId = @RoleId)
	BEGIN
		INSERT INTO [dbo].[ScreenUserRoleMap]
           ([ScreenId]
           ,[RoleId]
           ,[RecordStatus]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[ModifiedBy]
           ,[ModifiedDate]
           ,[CanEdit]
           ,[DisplayScreenName])
		VALUES
		(
			@ScreenId, 			
			@RoleId,
			@RecordStatus,	
			@UserName,				
			@CurrentDate,	
			@UserName,				
			@CurrentDate,	
			NULL,
			'CheckLists'
		)
	END

SELECT  @ScreenId = ScreenId FROM Screen WHERE ScreenCode = 'ACL'
SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'Admin'
IF NOT EXISTS ( SELECT TOP 1 1 FROM ScreenUserRoleMap WHERE ScreenId = @ScreenId AND RoleId = @RoleId)
	BEGIN
		INSERT INTO [dbo].[ScreenUserRoleMap]
           ([ScreenId]
           ,[RoleId]
           ,[RecordStatus]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[ModifiedBy]
           ,[ModifiedDate]
           ,[CanEdit]
           ,[DisplayScreenName])
		VALUES
		(
			@ScreenId, 			
			@RoleId,
			@RecordStatus,	
			@UserName,				
			@CurrentDate,	
			@UserName,				
			@CurrentDate,	
			NULL,
			'CheckLists'
		)
	END

SELECT  @ScreenId = ScreenId FROM Screen WHERE ScreenCode = 'KPI'
SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'Admin'
IF NOT EXISTS ( SELECT TOP 1 1 FROM ScreenUserRoleMap WHERE ScreenId = @ScreenId AND RoleId = @RoleId)
	BEGIN
		INSERT INTO [dbo].[ScreenUserRoleMap]
           ([ScreenId]
           ,[RoleId]
           ,[RecordStatus]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[ModifiedBy]
           ,[ModifiedDate]
           ,[CanEdit]
           ,[DisplayScreenName])
		VALUES
		(
			@ScreenId, 			
			@RoleId,
			@RecordStatus,	
			@UserName,				
			@CurrentDate,	
			@UserName,				
			@CurrentDate,	
			NULL,
			'KPI'
		)
	END

SELECT  @ScreenId = ScreenId FROM Screen WHERE ScreenCode = 'KPI'
SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'Manager'
IF NOT EXISTS ( SELECT TOP 1 1 FROM ScreenUserRoleMap WHERE ScreenId = @ScreenId AND RoleId = @RoleId)
	BEGIN
		INSERT INTO [dbo].[ScreenUserRoleMap]
           ([ScreenId]
           ,[RoleId]
           ,[RecordStatus]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[ModifiedBy]
           ,[ModifiedDate]
           ,[CanEdit]
           ,[DisplayScreenName])
		VALUES
		(
			@ScreenId, 			
			@RoleId,
			@RecordStatus,	
			@UserName,				
			@CurrentDate,	
			@UserName,				
			@CurrentDate,	
			NULL,
			'KPI'
		)
	END

SELECT  @ScreenId = ScreenId FROM Screen WHERE ScreenCode = 'KPI'
SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'Executive'
IF NOT EXISTS ( SELECT TOP 1 1 FROM ScreenUserRoleMap WHERE ScreenId = @ScreenId AND RoleId = @RoleId)
	BEGIN
		INSERT INTO [dbo].[ScreenUserRoleMap]
           ([ScreenId]
           ,[RoleId]
           ,[RecordStatus]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[ModifiedBy]
           ,[ModifiedDate]
           ,[CanEdit]
           ,[DisplayScreenName])
		VALUES
		(
			@ScreenId, 			
			@RoleId,
			@RecordStatus,	
			@UserName,				
			@CurrentDate,	
			@UserName,				
			@CurrentDate,	
			NULL,
			'KPI'
		)
	END

SELECT  @ScreenId = ScreenId FROM Screen WHERE ScreenCode = 'AHM'
SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'Executive'
IF NOT EXISTS ( SELECT TOP 1 1 FROM ScreenUserRoleMap WHERE ScreenId = @ScreenId AND RoleId = @RoleId)
	BEGIN
		INSERT INTO [dbo].[ScreenUserRoleMap]
           ([ScreenId]
           ,[RoleId]
           ,[RecordStatus]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[ModifiedBy]
           ,[ModifiedDate]
           ,[CanEdit]
           ,[DisplayScreenName])
		VALUES
		(
			@ScreenId, 			
			@RoleId,
			@RecordStatus,	
			@UserName,				
			@CurrentDate,	
			@UserName,				
			@CurrentDate,	
			NULL,
			'Heat Map Setup'
		)
	END

SELECT  @ScreenId = ScreenId FROM Screen WHERE ScreenCode = 'AHM'
SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'Admin'
IF NOT EXISTS ( SELECT TOP 1 1 FROM ScreenUserRoleMap WHERE ScreenId = @ScreenId AND RoleId = @RoleId)
	BEGIN
		INSERT INTO [dbo].[ScreenUserRoleMap]
           ([ScreenId]
           ,[RoleId]
           ,[RecordStatus]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[ModifiedBy]
           ,[ModifiedDate]
           ,[CanEdit]
           ,[DisplayScreenName])
		VALUES
		(
			@ScreenId, 			
			@RoleId,
			@RecordStatus,	
			@UserName,				
			@CurrentDate,	
			@UserName,				
			@CurrentDate,	
			NULL,
			'Heat Map Setup'
		)
	END

SELECT  @ScreenId = ScreenId FROM Screen WHERE ScreenCode = 'PAY'
SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'Admin'
IF NOT EXISTS ( SELECT TOP 1 1 FROM ScreenUserRoleMap WHERE ScreenId = @ScreenId AND RoleId = @RoleId)
	BEGIN
		INSERT INTO [dbo].[ScreenUserRoleMap]
           ([ScreenId]
           ,[RoleId]
           ,[RecordStatus]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[ModifiedBy]
           ,[ModifiedDate]
           ,[CanEdit]
           ,[DisplayScreenName])
		VALUES
		(
			@ScreenId, 			
			@RoleId,
			@RecordStatus,	
			@UserName,				
			@CurrentDate,	
			@UserName,				
			@CurrentDate,	
			1,
			'Payers'
		)
	END

SELECT  @ScreenId = ScreenId FROM Screen WHERE ScreenCode = 'PAY'
SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'Manager'
IF NOT EXISTS ( SELECT TOP 1 1 FROM ScreenUserRoleMap WHERE ScreenId = @ScreenId AND RoleId = @RoleId)
	BEGIN
		INSERT INTO [dbo].[ScreenUserRoleMap]
           ([ScreenId]
           ,[RoleId]
           ,[RecordStatus]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[ModifiedBy]
           ,[ModifiedDate]
           ,[CanEdit]
           ,[DisplayScreenName])
		VALUES
		(
			@ScreenId, 			
			@RoleId,
			@RecordStatus,	
			@UserName,				
			@CurrentDate,	
			@UserName,				
			@CurrentDate,	
			1,
			'Payers'
		)
	END

SELECT  @ScreenId = ScreenId FROM Screen WHERE ScreenCode = 'PAY'
SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'Executive'
IF NOT EXISTS ( SELECT TOP 1 1 FROM ScreenUserRoleMap WHERE ScreenId = @ScreenId AND RoleId = @RoleId)
	BEGIN
		INSERT INTO [dbo].[ScreenUserRoleMap]
           ([ScreenId]
           ,[RoleId]
           ,[RecordStatus]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[ModifiedBy]
           ,[ModifiedDate]
           ,[CanEdit]
           ,[DisplayScreenName])
		VALUES
		(
			@ScreenId, 			
			@RoleId,
			@RecordStatus,	
			@UserName,				
			@CurrentDate,	
			@UserName,				
			@CurrentDate,	
			1,
			'Payers'
		)
	END

SELECT  @ScreenId = ScreenId FROM Screen WHERE ScreenCode = 'PAY'
SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'User'
IF NOT EXISTS ( SELECT TOP 1 1 FROM ScreenUserRoleMap WHERE ScreenId = @ScreenId AND RoleId = @RoleId)
	BEGIN
		INSERT INTO [dbo].[ScreenUserRoleMap]
           ([ScreenId]
           ,[RoleId]
           ,[RecordStatus]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[ModifiedBy]
           ,[ModifiedDate]
           ,[CanEdit]
           ,[DisplayScreenName])
		VALUES
		(
			@ScreenId, 			
			@RoleId,
			@RecordStatus,	
			@UserName,				
			@CurrentDate,	
			@UserName,				
			@CurrentDate,	
			1,
			'Payers'
		)
	END

SELECT  @ScreenId = ScreenId FROM Screen WHERE ScreenCode = 'SPEC'
SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'Admin'
IF NOT EXISTS ( SELECT TOP 1 1 FROM ScreenUserRoleMap WHERE ScreenId = @ScreenId AND RoleId = @RoleId)
	BEGIN
		INSERT INTO [dbo].[ScreenUserRoleMap]
           ([ScreenId]
           ,[RoleId]
           ,[RecordStatus]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[ModifiedBy]
           ,[ModifiedDate]
           ,[CanEdit]
           ,[DisplayScreenName])
		VALUES
		(
			@ScreenId, 			
			@RoleId,
			@RecordStatus,	
			@UserName,				
			@CurrentDate,	
			@UserName,				
			@CurrentDate,	
			1,
			'Specialties'
		)
	END

SELECT  @ScreenId = ScreenId FROM Screen WHERE ScreenCode = 'SPEC'
SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'Manager'
IF NOT EXISTS ( SELECT TOP 1 1 FROM ScreenUserRoleMap WHERE ScreenId = @ScreenId AND RoleId = @RoleId)
	BEGIN
		INSERT INTO [dbo].[ScreenUserRoleMap]
           ([ScreenId]
           ,[RoleId]
           ,[RecordStatus]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[ModifiedBy]
           ,[ModifiedDate]
           ,[CanEdit]
           ,[DisplayScreenName])
		VALUES
		(
			@ScreenId, 			
			@RoleId,
			@RecordStatus,	
			@UserName,				
			@CurrentDate,	
			@UserName,				
			@CurrentDate,	
			0,
			'Specialties'
		)
	END

SELECT  @ScreenId = ScreenId FROM Screen WHERE ScreenCode = 'SPEC'
SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'Executive'
IF NOT EXISTS ( SELECT TOP 1 1 FROM ScreenUserRoleMap WHERE ScreenId = @ScreenId AND RoleId = @RoleId)
	BEGIN
		INSERT INTO [dbo].[ScreenUserRoleMap]
           ([ScreenId]
           ,[RoleId]
           ,[RecordStatus]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[ModifiedBy]
           ,[ModifiedDate]
           ,[CanEdit]
           ,[DisplayScreenName])
		VALUES
		(
			@ScreenId, 			
			@RoleId,
			@RecordStatus,	
			@UserName,				
			@CurrentDate,	
			@UserName,				
			@CurrentDate,	
			1,
			'Specialties'
		)
	END

SELECT  @ScreenId = ScreenId FROM Screen WHERE ScreenCode = 'USER'
SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'Executive'
IF NOT EXISTS ( SELECT TOP 1 1 FROM ScreenUserRoleMap WHERE ScreenId = @ScreenId AND RoleId = @RoleId)
	BEGIN
		INSERT INTO [dbo].[ScreenUserRoleMap]
           ([ScreenId]
           ,[RoleId]
           ,[RecordStatus]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[ModifiedBy]
           ,[ModifiedDate]
           ,[CanEdit]
           ,[DisplayScreenName])
		VALUES
		(
			@ScreenId, 			
			@RoleId,
			@RecordStatus,	
			@UserName,				
			@CurrentDate,	
			@UserName,				
			@CurrentDate,	
			NULL,
			'Users'
		)
	END

SELECT  @ScreenId = ScreenId FROM Screen WHERE ScreenCode = 'USER'
SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'Manager'
IF NOT EXISTS ( SELECT TOP 1 1 FROM ScreenUserRoleMap WHERE ScreenId = @ScreenId AND RoleId = @RoleId)
	BEGIN
		INSERT INTO [dbo].[ScreenUserRoleMap]
           ([ScreenId]
           ,[RoleId]
           ,[RecordStatus]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[ModifiedBy]
           ,[ModifiedDate]
           ,[CanEdit]
           ,[DisplayScreenName])
		VALUES
		(
			@ScreenId, 			
			@RoleId,
			@RecordStatus,	
			@UserName,				
			@CurrentDate,	
			@UserName,				
			@CurrentDate,	
			NULL,
			'Users'
		)
	END

SELECT  @ScreenId = ScreenId FROM Screen WHERE ScreenCode = 'USER'
SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'Admin'
IF NOT EXISTS ( SELECT TOP 1 1 FROM ScreenUserRoleMap WHERE ScreenId = @ScreenId AND RoleId = @RoleId)
	BEGIN
		INSERT INTO [dbo].[ScreenUserRoleMap]
           ([ScreenId]
           ,[RoleId]
           ,[RecordStatus]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[ModifiedBy]
           ,[ModifiedDate]
           ,[CanEdit]
           ,[DisplayScreenName])
		VALUES
		(
			@ScreenId, 			
			@RoleId,
			@RecordStatus,	
			@UserName,				
			@CurrentDate,	
			@UserName,				
			@CurrentDate,	
			NULL,
			'Users'
		)
	END

SELECT  @ScreenId = ScreenId FROM Screen WHERE ScreenCode = 'CK'
SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'Admin'
IF NOT EXISTS ( SELECT TOP 1 1 FROM ScreenUserRoleMap WHERE ScreenId = @ScreenId AND RoleId = @RoleId)
	BEGIN
		INSERT INTO [dbo].[ScreenUserRoleMap]
           ([ScreenId]
           ,[RoleId]
           ,[RecordStatus]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[ModifiedBy]
           ,[ModifiedDate]
           ,[CanEdit]
           ,[DisplayScreenName])
		VALUES
		(
			@ScreenId, 			
			@RoleId,
			@RecordStatus,	
			@UserName,				
			@CurrentDate,	
			@UserName,				
			@CurrentDate,	
			1,
			'Create KPI'
		)
	END

SELECT  @ScreenId = ScreenId FROM Screen WHERE ScreenCode = 'CK'
SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'Executive'
IF NOT EXISTS ( SELECT TOP 1 1 FROM ScreenUserRoleMap WHERE ScreenId = @ScreenId AND RoleId = @RoleId)
	BEGIN
		INSERT INTO [dbo].[ScreenUserRoleMap]
           ([ScreenId]
           ,[RoleId]
           ,[RecordStatus]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[ModifiedBy]
           ,[ModifiedDate]
           ,[CanEdit]
           ,[DisplayScreenName])
		VALUES
		(
			@ScreenId, 			
			@RoleId,
			@RecordStatus,	
			@UserName,				
			@CurrentDate,	
			@UserName,				
			@CurrentDate,	
			1,
			'Create KPI'
		)
	END

SELECT  @ScreenId = ScreenId FROM Screen WHERE ScreenCode = 'VEK'
SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'Executive'
IF NOT EXISTS ( SELECT TOP 1 1 FROM ScreenUserRoleMap WHERE ScreenId = @ScreenId AND RoleId = @RoleId)
	BEGIN
		INSERT INTO [dbo].[ScreenUserRoleMap]
           ([ScreenId]
           ,[RoleId]
           ,[RecordStatus]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[ModifiedBy]
           ,[ModifiedDate]
           ,[CanEdit]
           ,[DisplayScreenName])
		VALUES
		(
			@ScreenId, 			
			@RoleId,
			@RecordStatus,	
			@UserName,				
			@CurrentDate,	
			@UserName,				
			@CurrentDate,	
			1,
			'View/Edit KPI'
		)
	END

SELECT  @ScreenId = ScreenId FROM Screen WHERE ScreenCode = 'VEK'
SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'Manager'
IF NOT EXISTS ( SELECT TOP 1 1 FROM ScreenUserRoleMap WHERE ScreenId = @ScreenId AND RoleId = @RoleId)
	BEGIN
		INSERT INTO [dbo].[ScreenUserRoleMap]
           ([ScreenId]
           ,[RoleId]
           ,[RecordStatus]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[ModifiedBy]
           ,[ModifiedDate]
           ,[CanEdit]
           ,[DisplayScreenName])
		VALUES
		(
			@ScreenId, 			
			@RoleId,
			@RecordStatus,	
			@UserName,				
			@CurrentDate,	
			@UserName,				
			@CurrentDate,	
			0,
			'View KPI'
		)
	END

SELECT  @ScreenId = ScreenId FROM Screen WHERE ScreenCode = 'VEK'
SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'Admin'
IF NOT EXISTS ( SELECT TOP 1 1 FROM ScreenUserRoleMap WHERE ScreenId = @ScreenId AND RoleId = @RoleId)
	BEGIN
		INSERT INTO [dbo].[ScreenUserRoleMap]
           ([ScreenId]
           ,[RoleId]
           ,[RecordStatus]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[ModifiedBy]
           ,[ModifiedDate]
           ,[CanEdit]
           ,[DisplayScreenName])
		VALUES
		(
			@ScreenId, 			
			@RoleId,
			@RecordStatus,	
			@UserName,				
			@CurrentDate,	
			@UserName,				
			@CurrentDate,	
			1,
			'View/Edit KPI'
		)
	END

SELECT  @ScreenId = ScreenId FROM Screen WHERE ScreenCode = 'CMU'
SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'Admin'
IF NOT EXISTS ( SELECT TOP 1 1 FROM ScreenUserRoleMap WHERE ScreenId = @ScreenId AND RoleId = @RoleId)
	BEGIN
		INSERT INTO [dbo].[ScreenUserRoleMap]
           ([ScreenId]
           ,[RoleId]
           ,[RecordStatus]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[ModifiedBy]
           ,[ModifiedDate]
           ,[CanEdit]
           ,[DisplayScreenName])
		VALUES
		(
			@ScreenId, 			
			@RoleId,
			@RecordStatus,	
			@UserName,				
			@CurrentDate,	
			@UserName,				
			@CurrentDate,	
			NULL,
			'Create M3 Users'
		)
	END

SELECT  @ScreenId = ScreenId FROM Screen WHERE ScreenCode = 'CMU'
SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'Manager'
IF NOT EXISTS ( SELECT TOP 1 1 FROM ScreenUserRoleMap WHERE ScreenId = @ScreenId AND RoleId = @RoleId)
	BEGIN
		INSERT INTO [dbo].[ScreenUserRoleMap]
           ([ScreenId]
           ,[RoleId]
           ,[RecordStatus]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[ModifiedBy]
           ,[ModifiedDate]
           ,[CanEdit]
           ,[DisplayScreenName])
		VALUES
		(
			@ScreenId, 			
			@RoleId,
			@RecordStatus,	
			@UserName,				
			@CurrentDate,	
			@UserName,				
			@CurrentDate,	
			NULL,
			'Create M3 Users'
		)
	END

SELECT  @ScreenId = ScreenId FROM Screen WHERE ScreenCode = 'CMU'
SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'Executive'
IF NOT EXISTS ( SELECT TOP 1 1 FROM ScreenUserRoleMap WHERE ScreenId = @ScreenId AND RoleId = @RoleId)
	BEGIN
		INSERT INTO [dbo].[ScreenUserRoleMap]
           ([ScreenId]
           ,[RoleId]
           ,[RecordStatus]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[ModifiedBy]
           ,[ModifiedDate]
           ,[CanEdit]
           ,[DisplayScreenName])
		VALUES
		(
			@ScreenId, 			
			@RoleId,
			@RecordStatus,	
			@UserName,				
			@CurrentDate,	
			@UserName,				
			@CurrentDate,	
			NULL,
			'Create M3 Users'
		)
	END

SELECT  @ScreenId = ScreenId FROM Screen WHERE ScreenCode = 'CCU'
SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'Executive'
IF NOT EXISTS ( SELECT TOP 1 1 FROM ScreenUserRoleMap WHERE ScreenId = @ScreenId AND RoleId = @RoleId)
	BEGIN
		INSERT INTO [dbo].[ScreenUserRoleMap]
           ([ScreenId]
           ,[RoleId]
           ,[RecordStatus]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[ModifiedBy]
           ,[ModifiedDate]
           ,[CanEdit]
           ,[DisplayScreenName])
		VALUES
		(
			@ScreenId, 			
			@RoleId,
			@RecordStatus,	
			@UserName,				
			@CurrentDate,	
			@UserName,				
			@CurrentDate,	
			NULL,
			'Create Client Users'
		)
	END

SELECT  @ScreenId = ScreenId FROM Screen WHERE ScreenCode = 'CCU'
SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'Manager'
IF NOT EXISTS ( SELECT TOP 1 1 FROM ScreenUserRoleMap WHERE ScreenId = @ScreenId AND RoleId = @RoleId)
	BEGIN
		INSERT INTO [dbo].[ScreenUserRoleMap]
           ([ScreenId]
           ,[RoleId]
           ,[RecordStatus]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[ModifiedBy]
           ,[ModifiedDate]
           ,[CanEdit]
           ,[DisplayScreenName])
		VALUES
		(
			@ScreenId, 			
			@RoleId,
			@RecordStatus,	
			@UserName,				
			@CurrentDate,	
			@UserName,				
			@CurrentDate,	
			NULL,
			'Create Client Users'
		)
	END

SELECT  @ScreenId = ScreenId FROM Screen WHERE ScreenCode = 'CCU'
SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'Admin'
IF NOT EXISTS ( SELECT TOP 1 1 FROM ScreenUserRoleMap WHERE ScreenId = @ScreenId AND RoleId = @RoleId)
	BEGIN
		INSERT INTO [dbo].[ScreenUserRoleMap]
           ([ScreenId]
           ,[RoleId]
           ,[RecordStatus]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[ModifiedBy]
           ,[ModifiedDate]
           ,[CanEdit]
           ,[DisplayScreenName])
		VALUES
		(
			@ScreenId, 			
			@RoleId,
			@RecordStatus,	
			@UserName,				
			@CurrentDate,	
			@UserName,				
			@CurrentDate,	
			NULL,
			'Create Client Users'
		)
	END

SELECT  @ScreenId = ScreenId FROM Screen WHERE ScreenCode = 'AU'
SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'Admin'
IF NOT EXISTS ( SELECT TOP 1 1 FROM ScreenUserRoleMap WHERE ScreenId = @ScreenId AND RoleId = @RoleId)
	BEGIN
		INSERT INTO [dbo].[ScreenUserRoleMap]
           ([ScreenId]
           ,[RoleId]
           ,[RecordStatus]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[ModifiedBy]
           ,[ModifiedDate]
           ,[CanEdit]
           ,[DisplayScreenName])
		VALUES
		(
			@ScreenId, 			
			@RoleId,
			@RecordStatus,	
			@UserName,				
			@CurrentDate,	
			@UserName,				
			@CurrentDate,	
			NULL,
			'All Users'
		)
	END

SELECT  @ScreenId = ScreenId FROM Screen WHERE ScreenCode = 'AU'
SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'Manager'
IF NOT EXISTS ( SELECT TOP 1 1 FROM ScreenUserRoleMap WHERE ScreenId = @ScreenId AND RoleId = @RoleId)
	BEGIN
		INSERT INTO [dbo].[ScreenUserRoleMap]
           ([ScreenId]
           ,[RoleId]
           ,[RecordStatus]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[ModifiedBy]
           ,[ModifiedDate]
           ,[CanEdit]
           ,[DisplayScreenName])
		VALUES
		(
			@ScreenId, 			
			@RoleId,
			@RecordStatus,	
			@UserName,				
			@CurrentDate,	
			@UserName,				
			@CurrentDate,	
			NULL,
			'All Users'
		)
	END

SELECT  @ScreenId = ScreenId FROM Screen WHERE ScreenCode = 'AU'
SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'Executive'
IF NOT EXISTS ( SELECT TOP 1 1 FROM ScreenUserRoleMap WHERE ScreenId = @ScreenId AND RoleId = @RoleId)
	BEGIN
		INSERT INTO [dbo].[ScreenUserRoleMap]
           ([ScreenId]
           ,[RoleId]
           ,[RecordStatus]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[ModifiedBy]
           ,[ModifiedDate]
           ,[CanEdit]
           ,[DisplayScreenName])
		VALUES
		(
			@ScreenId, 			
			@RoleId,
			@RecordStatus,	
			@UserName,				
			@CurrentDate,	
			@UserName,				
			@CurrentDate,	
			NULL,
			'All Users'
		)
	END

SELECT  @ScreenId = ScreenId FROM Screen WHERE ScreenCode = 'CECI'
SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'Executive'
IF NOT EXISTS ( SELECT TOP 1 1 FROM ScreenUserRoleMap WHERE ScreenId = @ScreenId AND RoleId = @RoleId)
	BEGIN
		INSERT INTO [dbo].[ScreenUserRoleMap]
           ([ScreenId]
           ,[RoleId]
           ,[RecordStatus]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[ModifiedBy]
           ,[ModifiedDate]
           ,[CanEdit]
           ,[DisplayScreenName])
		VALUES
		(
			@ScreenId, 			
			@RoleId,
			@RecordStatus,	
			@UserName,				
			@CurrentDate,	
			@UserName,				
			@CurrentDate,	
			NULL,
			'Create/Edit Checklist Item'
		)
	END

SELECT  @ScreenId = ScreenId FROM Screen WHERE ScreenCode = 'CECI'
SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'Admin'
IF NOT EXISTS ( SELECT TOP 1 1 FROM ScreenUserRoleMap WHERE ScreenId = @ScreenId AND RoleId = @RoleId)
	BEGIN
		INSERT INTO [dbo].[ScreenUserRoleMap]
           ([ScreenId]
           ,[RoleId]
           ,[RecordStatus]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[ModifiedBy]
           ,[ModifiedDate]
           ,[CanEdit]
           ,[DisplayScreenName])
		VALUES
		(
			@ScreenId, 			
			@RoleId,
			@RecordStatus,	
			@UserName,				
			@CurrentDate,	
			@UserName,				
			@CurrentDate,	
			NULL,
			'Create/Edit Checklist Item'
		)
	END

SELECT  @ScreenId = ScreenId FROM Screen WHERE ScreenCode = 'CLS'
SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'Admin'
IF NOT EXISTS ( SELECT TOP 1 1 FROM ScreenUserRoleMap WHERE ScreenId = @ScreenId AND RoleId = @RoleId)
	BEGIN
		INSERT INTO [dbo].[ScreenUserRoleMap]
           ([ScreenId]
           ,[RoleId]
           ,[RecordStatus]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[ModifiedBy]
           ,[ModifiedDate]
           ,[CanEdit]
           ,[DisplayScreenName])
		VALUES
		(
			@ScreenId, 			
			@RoleId,
			@RecordStatus,	
			@UserName,				
			@CurrentDate,	
			@UserName,				
			@CurrentDate,	
			NULL,
			'Checklist Setup'
		)
	END

SELECT  @ScreenId = ScreenId FROM Screen WHERE ScreenCode = 'CLS'
SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'Executive'
IF NOT EXISTS ( SELECT TOP 1 1 FROM ScreenUserRoleMap WHERE ScreenId = @ScreenId AND RoleId = @RoleId)
	BEGIN
		INSERT INTO [dbo].[ScreenUserRoleMap]
           ([ScreenId]
           ,[RoleId]
           ,[RecordStatus]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[ModifiedBy]
           ,[ModifiedDate]
           ,[CanEdit]
           ,[DisplayScreenName])
		VALUES
		(
			@ScreenId, 			
			@RoleId,
			@RecordStatus,	
			@UserName,				
			@CurrentDate,	
			@UserName,				
			@CurrentDate,	
			NULL,
			'Checklist Setup'
		)
	END

SELECT  @ScreenId = ScreenId FROM Screen WHERE ScreenCode = 'VACL'
SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'Executive'
IF NOT EXISTS ( SELECT TOP 1 1 FROM ScreenUserRoleMap WHERE ScreenId = @ScreenId AND RoleId = @RoleId)
	BEGIN
		INSERT INTO [dbo].[ScreenUserRoleMap]
           ([ScreenId]
           ,[RoleId]
           ,[RecordStatus]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[ModifiedBy]
           ,[ModifiedDate]
           ,[CanEdit]
           ,[DisplayScreenName])
		VALUES
		(
			@ScreenId, 			
			@RoleId,
			@RecordStatus,	
			@UserName,				
			@CurrentDate,	
			@UserName,				
			@CurrentDate,	
			1,
			'View All Checklist'
		)
	END

SELECT  @ScreenId = ScreenId FROM Screen WHERE ScreenCode = 'VACL'
SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'Admin'
IF NOT EXISTS ( SELECT TOP 1 1 FROM ScreenUserRoleMap WHERE ScreenId = @ScreenId AND RoleId = @RoleId)
	BEGIN
		INSERT INTO [dbo].[ScreenUserRoleMap]
           ([ScreenId]
           ,[RoleId]
           ,[RecordStatus]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[ModifiedBy]
           ,[ModifiedDate]
           ,[CanEdit]
           ,[DisplayScreenName])
		VALUES
		(
			@ScreenId, 			
			@RoleId,
			@RecordStatus,	
			@UserName,				
			@CurrentDate,	
			@UserName,				
			@CurrentDate,	
			1,
			'View All Checklist'
		)
	END

SELECT  @ScreenId = ScreenId FROM Screen WHERE ScreenCode = 'CD'
SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'Admin'
IF NOT EXISTS ( SELECT TOP 1 1 FROM ScreenUserRoleMap WHERE ScreenId = @ScreenId AND RoleId = @RoleId)
	BEGIN
		INSERT INTO [dbo].[ScreenUserRoleMap]
           ([ScreenId]
           ,[RoleId]
           ,[RecordStatus]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[ModifiedBy]
           ,[ModifiedDate]
           ,[CanEdit]
           ,[DisplayScreenName])
		VALUES
		(
			@ScreenId, 			
			@RoleId,
			@RecordStatus,	
			@UserName,				
			@CurrentDate,	
			@UserName,				
			@CurrentDate,	
			1,
			'Client Data'
		)
	END

SELECT  @ScreenId = ScreenId FROM Screen WHERE ScreenCode = 'CD'
SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'Manager'
IF NOT EXISTS ( SELECT TOP 1 1 FROM ScreenUserRoleMap WHERE ScreenId = @ScreenId AND RoleId = @RoleId)
	BEGIN
		INSERT INTO [dbo].[ScreenUserRoleMap]
           ([ScreenId]
           ,[RoleId]
           ,[RecordStatus]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[ModifiedBy]
           ,[ModifiedDate]
           ,[CanEdit]
           ,[DisplayScreenName])
		VALUES
		(
			@ScreenId, 			
			@RoleId,
			@RecordStatus,	
			@UserName,				
			@CurrentDate,	
			@UserName,				
			@CurrentDate,	
			1,
			'Client Data'
		)
	END

SELECT  @ScreenId = ScreenId FROM Screen WHERE ScreenCode = 'CD'
SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'Executive'
IF NOT EXISTS ( SELECT TOP 1 1 FROM ScreenUserRoleMap WHERE ScreenId = @ScreenId AND RoleId = @RoleId)
	BEGIN
		INSERT INTO [dbo].[ScreenUserRoleMap]
           ([ScreenId]
           ,[RoleId]
           ,[RecordStatus]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[ModifiedBy]
           ,[ModifiedDate]
           ,[CanEdit]
           ,[DisplayScreenName])
		VALUES
		(
			@ScreenId, 			
			@RoleId,
			@RecordStatus,	
			@UserName,				
			@CurrentDate,	
			@UserName,				
			@CurrentDate,	
			1,
			'Client Data'
		)
	END

SELECT  @ScreenId = ScreenId FROM Screen WHERE ScreenCode = 'CD'
SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'User'
IF NOT EXISTS ( SELECT TOP 1 1 FROM ScreenUserRoleMap WHERE ScreenId = @ScreenId AND RoleId = @RoleId)
	BEGIN
		INSERT INTO [dbo].[ScreenUserRoleMap]
           ([ScreenId]
           ,[RoleId]
           ,[RecordStatus]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[ModifiedBy]
           ,[ModifiedDate]
           ,[CanEdit]
           ,[DisplayScreenName])
		VALUES
		(
			@ScreenId, 			
			@RoleId,
			@RecordStatus,	
			@UserName,				
			@CurrentDate,	
			@UserName,				
			@CurrentDate,	
			0,
			'Client Data'
		)
	END

SELECT  @ScreenId = ScreenId FROM Screen WHERE ScreenCode = 'LS'
SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'Admin'
IF NOT EXISTS ( SELECT TOP 1 1 FROM ScreenUserRoleMap WHERE ScreenId = @ScreenId AND RoleId = @RoleId)
	BEGIN
		INSERT INTO [dbo].[ScreenUserRoleMap]
           ([ScreenId]
           ,[RoleId]
           ,[RecordStatus]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[ModifiedBy]
           ,[ModifiedDate]
           ,[CanEdit]
           ,[DisplayScreenName])
		VALUES
		(
			@ScreenId, 			
			@RoleId,
			@RecordStatus,	
			@UserName,				
			@CurrentDate,	
			@UserName,				
			@CurrentDate,	
			1,
			'Log Setup'
		)
	END

SELECT  @ScreenId = ScreenId FROM Screen WHERE ScreenCode = 'LS'
SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'Manager'
IF NOT EXISTS ( SELECT TOP 1 1 FROM ScreenUserRoleMap WHERE ScreenId = @ScreenId AND RoleId = @RoleId)
	BEGIN
		INSERT INTO [dbo].[ScreenUserRoleMap]
           ([ScreenId]
           ,[RoleId]
           ,[RecordStatus]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[ModifiedBy]
           ,[ModifiedDate]
           ,[CanEdit]
           ,[DisplayScreenName])
		VALUES
		(
			@ScreenId, 			
			@RoleId,
			@RecordStatus,	
			@UserName,				
			@CurrentDate,	
			@UserName,				
			@CurrentDate,	
			1,
			'Log Setup'
		)
	END

SELECT  @ScreenId = ScreenId FROM Screen WHERE ScreenCode = 'LS'
SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'Executive'
IF NOT EXISTS ( SELECT TOP 1 1 FROM ScreenUserRoleMap WHERE ScreenId = @ScreenId AND RoleId = @RoleId)
	BEGIN
		INSERT INTO [dbo].[ScreenUserRoleMap]
           ([ScreenId]
           ,[RoleId]
           ,[RecordStatus]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[ModifiedBy]
           ,[ModifiedDate]
           ,[CanEdit]
           ,[DisplayScreenName])
		VALUES
		(
			@ScreenId, 			
			@RoleId,
			@RecordStatus,	
			@UserName,				
			@CurrentDate,	
			@UserName,				
			@CurrentDate,	
			1,
			'Log Setup'
		)
	END

SELECT  @ScreenId = ScreenId FROM Screen WHERE ScreenCode = 'LS'
SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'User'
IF NOT EXISTS ( SELECT TOP 1 1 FROM ScreenUserRoleMap WHERE ScreenId = @ScreenId AND RoleId = @RoleId)
	BEGIN
		INSERT INTO [dbo].[ScreenUserRoleMap]
           ([ScreenId]
           ,[RoleId]
           ,[RecordStatus]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[ModifiedBy]
           ,[ModifiedDate]
           ,[CanEdit]
           ,[DisplayScreenName])
		VALUES
		(
			@ScreenId, 			
			@RoleId,
			@RecordStatus,	
			@UserName,				
			@CurrentDate,	
			@UserName,				
			@CurrentDate,	
			1,
			'Log Setup'
		)
	END

SELECT  @ScreenId = ScreenId FROM Screen WHERE ScreenCode = 'CAU'
SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'Admin'
IF NOT EXISTS ( SELECT TOP 1 1 FROM ScreenUserRoleMap WHERE ScreenId = @ScreenId AND RoleId = @RoleId)
	BEGIN
		INSERT INTO [dbo].[ScreenUserRoleMap]
           ([ScreenId]
           ,[RoleId]
           ,[RecordStatus]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[ModifiedBy]
           ,[ModifiedDate]
           ,[CanEdit]
           ,[DisplayScreenName])
		VALUES
		(
			@ScreenId, 			
			@RoleId,
			@RecordStatus,	
			@UserName,				
			@CurrentDate,	
			@UserName,				
			@CurrentDate,	
			1,
			'Assign User'
		)
	END

SELECT  @ScreenId = ScreenId FROM Screen WHERE ScreenCode = 'CAU'
SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'Manager'
IF NOT EXISTS ( SELECT TOP 1 1 FROM ScreenUserRoleMap WHERE ScreenId = @ScreenId AND RoleId = @RoleId)
	BEGIN
		INSERT INTO [dbo].[ScreenUserRoleMap]
           ([ScreenId]
           ,[RoleId]
           ,[RecordStatus]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[ModifiedBy]
           ,[ModifiedDate]
           ,[CanEdit]
           ,[DisplayScreenName])
		VALUES
		(
			@ScreenId, 			
			@RoleId,
			@RecordStatus,	
			@UserName,				
			@CurrentDate,	
			@UserName,				
			@CurrentDate,	
			1,
			'Assign User'
		)
	END

SELECT  @ScreenId = ScreenId FROM Screen WHERE ScreenCode = 'CAU'
SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'Executive'
IF NOT EXISTS ( SELECT TOP 1 1 FROM ScreenUserRoleMap WHERE ScreenId = @ScreenId AND RoleId = @RoleId)
	BEGIN
		INSERT INTO [dbo].[ScreenUserRoleMap]
           ([ScreenId]
           ,[RoleId]
           ,[RecordStatus]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[ModifiedBy]
           ,[ModifiedDate]
           ,[CanEdit]
           ,[DisplayScreenName])
		VALUES
		(
			@ScreenId, 			
			@RoleId,
			@RecordStatus,	
			@UserName,				
			@CurrentDate,	
			@UserName,				
			@CurrentDate,	
			1,
			'Assign User'
		)
	END

SELECT  @ScreenId = ScreenId FROM Screen WHERE ScreenCode = 'MT'
SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'Admin'
IF NOT EXISTS ( SELECT TOP 1 1 FROM ScreenUserRoleMap WHERE ScreenId = @ScreenId AND RoleId = @RoleId)
	BEGIN
		INSERT INTO [dbo].[ScreenUserRoleMap]
           ([ScreenId]
           ,[RoleId]
           ,[RecordStatus]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[ModifiedBy]
           ,[ModifiedDate]
           ,[CanEdit]
           ,[DisplayScreenName])
		VALUES
		(
			@ScreenId, 			
			@RoleId,
			@RecordStatus,	
			@UserName,				
			@CurrentDate,	
			@UserName,				
			@CurrentDate,	
			1,
			'Monthly Targets'
		)
	END

SELECT  @ScreenId = ScreenId FROM Screen WHERE ScreenCode = 'MT'
SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'Manager'
IF NOT EXISTS ( SELECT TOP 1 1 FROM ScreenUserRoleMap WHERE ScreenId = @ScreenId AND RoleId = @RoleId)
	BEGIN
		INSERT INTO [dbo].[ScreenUserRoleMap]
           ([ScreenId]
           ,[RoleId]
           ,[RecordStatus]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[ModifiedBy]
           ,[ModifiedDate]
           ,[CanEdit]
           ,[DisplayScreenName])
		VALUES
		(
			@ScreenId, 			
			@RoleId,
			@RecordStatus,	
			@UserName,				
			@CurrentDate,	
			@UserName,				
			@CurrentDate,	
			1,
			'Monthly Targets'
		)
	END

SELECT  @ScreenId = ScreenId FROM Screen WHERE ScreenCode = 'MT'
SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'Executive'
IF NOT EXISTS ( SELECT TOP 1 1 FROM ScreenUserRoleMap WHERE ScreenId = @ScreenId AND RoleId = @RoleId)
	BEGIN
		INSERT INTO [dbo].[ScreenUserRoleMap]
           ([ScreenId]
           ,[RoleId]
           ,[RecordStatus]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[ModifiedBy]
           ,[ModifiedDate]
           ,[CanEdit]
           ,[DisplayScreenName])
		VALUES
		(
			@ScreenId, 			
			@RoleId,
			@RecordStatus,	
			@UserName,				
			@CurrentDate,	
			@UserName,				
			@CurrentDate,	
			1,
			'Monthly Targets'
		)
	END

SELECT  @ScreenId = ScreenId FROM Screen WHERE ScreenCode = 'KPIS'
SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'Admin'
IF NOT EXISTS ( SELECT TOP 1 1 FROM ScreenUserRoleMap WHERE ScreenId = @ScreenId AND RoleId = @RoleId)
	BEGIN
		INSERT INTO [dbo].[ScreenUserRoleMap]
           ([ScreenId]
           ,[RoleId]
           ,[RecordStatus]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[ModifiedBy]
           ,[ModifiedDate]
           ,[CanEdit]
           ,[DisplayScreenName])
		VALUES
		(
			@ScreenId, 			
			@RoleId,
			@RecordStatus,	
			@UserName,				
			@CurrentDate,	
			@UserName,				
			@CurrentDate,	
			1,
			'KPI Setup'
		)
	END

SELECT  @ScreenId = ScreenId FROM Screen WHERE ScreenCode = 'KPIS'
SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'Manager'
IF NOT EXISTS ( SELECT TOP 1 1 FROM ScreenUserRoleMap WHERE ScreenId = @ScreenId AND RoleId = @RoleId)
	BEGIN
		INSERT INTO [dbo].[ScreenUserRoleMap]
           ([ScreenId]
           ,[RoleId]
           ,[RecordStatus]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[ModifiedBy]
           ,[ModifiedDate]
           ,[CanEdit]
           ,[DisplayScreenName])
		VALUES
		(
			@ScreenId, 			
			@RoleId,
			@RecordStatus,	
			@UserName,				
			@CurrentDate,	
			@UserName,				
			@CurrentDate,	
			1,
			'KPI Setup'
		)
	END

SELECT  @ScreenId = ScreenId FROM Screen WHERE ScreenCode = 'KPIS'
SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'Executive'
IF NOT EXISTS ( SELECT TOP 1 1 FROM ScreenUserRoleMap WHERE ScreenId = @ScreenId AND RoleId = @RoleId)
	BEGIN
		INSERT INTO [dbo].[ScreenUserRoleMap]
           ([ScreenId]
           ,[RoleId]
           ,[RecordStatus]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[ModifiedBy]
           ,[ModifiedDate]
           ,[CanEdit]
           ,[DisplayScreenName])
		VALUES
		(
			@ScreenId, 			
			@RoleId,
			@RecordStatus,	
			@UserName,				
			@CurrentDate,	
			@UserName,				
			@CurrentDate,	
			1,
			'KPI Setup'
		)
	END

SELECT  @ScreenId = ScreenId FROM Screen WHERE ScreenCode = 'KPIS'
SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'User'
IF NOT EXISTS ( SELECT TOP 1 1 FROM ScreenUserRoleMap WHERE ScreenId = @ScreenId AND RoleId = @RoleId)
	BEGIN
		INSERT INTO [dbo].[ScreenUserRoleMap]
           ([ScreenId]
           ,[RoleId]
           ,[RecordStatus]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[ModifiedBy]
           ,[ModifiedDate]
           ,[CanEdit]
           ,[DisplayScreenName])
		VALUES
		(
			@ScreenId, 			
			@RoleId,
			@RecordStatus,	
			@UserName,				
			@CurrentDate,	
			@UserName,				
			@CurrentDate,	
			0,
			'KPI Setup'
		)
	END

SELECT  @ScreenId = ScreenId FROM Screen WHERE ScreenCode = 'KPIS'
SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'Client'
IF NOT EXISTS ( SELECT TOP 1 1 FROM ScreenUserRoleMap WHERE ScreenId = @ScreenId AND RoleId = @RoleId)
	BEGIN
		INSERT INTO [dbo].[ScreenUserRoleMap]
           ([ScreenId]
           ,[RoleId]
           ,[RecordStatus]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[ModifiedBy]
           ,[ModifiedDate]
           ,[CanEdit]
           ,[DisplayScreenName])
		VALUES
		(
			@ScreenId, 			
			@RoleId,
			@RecordStatus,	
			@UserName,				
			@CurrentDate,	
			@UserName,				
			@CurrentDate,	
			0,
			'KPI Setup'
		)
	END

SELECT  @ScreenId = ScreenId FROM Screen WHERE ScreenCode = 'LP'
SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'Admin'
IF NOT EXISTS ( SELECT TOP 1 1 FROM ScreenUserRoleMap WHERE ScreenId = @ScreenId AND RoleId = @RoleId)
	BEGIN
		INSERT INTO [dbo].[ScreenUserRoleMap]
           ([ScreenId]
           ,[RoleId]
           ,[RecordStatus]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[ModifiedBy]
           ,[ModifiedDate]
           ,[CanEdit]
           ,[DisplayScreenName])
		VALUES
		(
			@ScreenId, 			
			@RoleId,
			@RecordStatus,	
			@UserName,				
			@CurrentDate,	
			@UserName,				
			@CurrentDate,	
			NULL,
			'Landing Page'
		)
	END

SELECT  @ScreenId = ScreenId FROM Screen WHERE ScreenCode = 'LP'
SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'Manager'
IF NOT EXISTS ( SELECT TOP 1 1 FROM ScreenUserRoleMap WHERE ScreenId = @ScreenId AND RoleId = @RoleId)
	BEGIN
		INSERT INTO [dbo].[ScreenUserRoleMap]
           ([ScreenId]
           ,[RoleId]
           ,[RecordStatus]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[ModifiedBy]
           ,[ModifiedDate]
           ,[CanEdit]
           ,[DisplayScreenName])
		VALUES
		(
			@ScreenId, 			
			@RoleId,
			@RecordStatus,	
			@UserName,				
			@CurrentDate,	
			@UserName,				
			@CurrentDate,	
			NULL,
			'Landing Page'
		)
	END

SELECT  @ScreenId = ScreenId FROM Screen WHERE ScreenCode = 'LP'
SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'Executive'
IF NOT EXISTS ( SELECT TOP 1 1 FROM ScreenUserRoleMap WHERE ScreenId = @ScreenId AND RoleId = @RoleId)
	BEGIN
		INSERT INTO [dbo].[ScreenUserRoleMap]
           ([ScreenId]
           ,[RoleId]
           ,[RecordStatus]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[ModifiedBy]
           ,[ModifiedDate]
           ,[CanEdit]
           ,[DisplayScreenName])
		VALUES
		(
			@ScreenId, 			
			@RoleId,
			@RecordStatus,	
			@UserName,				
			@CurrentDate,	
			@UserName,				
			@CurrentDate,	
			NULL,
			'Landing Page'
		)
	END

SELECT  @ScreenId = ScreenId FROM Screen WHERE ScreenCode = 'SYS'
SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'Admin'
IF NOT EXISTS ( SELECT TOP 1 1 FROM ScreenUserRoleMap WHERE ScreenId = @ScreenId AND RoleId = @RoleId)
	BEGIN
		INSERT INTO [dbo].[ScreenUserRoleMap]
           ([ScreenId]
           ,[RoleId]
           ,[RecordStatus]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[ModifiedBy]
           ,[ModifiedDate]
           ,[CanEdit]
           ,[DisplayScreenName])
		VALUES
		(
			@ScreenId, 			
			@RoleId,
			@RecordStatus,	
			@UserName,				
			@CurrentDate,	
			@UserName,				
			@CurrentDate,	
			1,
			'System'
		)
	END

SELECT  @ScreenId = ScreenId FROM Screen WHERE ScreenCode = 'SYS'
SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'Executive'
IF NOT EXISTS ( SELECT TOP 1 1 FROM ScreenUserRoleMap WHERE ScreenId = @ScreenId AND RoleId = @RoleId)
	BEGIN
		INSERT INTO [dbo].[ScreenUserRoleMap]
           ([ScreenId]
           ,[RoleId]
           ,[RecordStatus]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[ModifiedBy]
           ,[ModifiedDate]
           ,[CanEdit]
           ,[DisplayScreenName])
		VALUES
		(
			@ScreenId, 			
			@RoleId,
			@RecordStatus,	
			@UserName,				
			@CurrentDate,	
			@UserName,				
			@CurrentDate,	
			0,
			'System'
		)
	END
	SELECT  @ScreenId = ScreenId FROM Screen WHERE ScreenCode = 'CON'
SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'Admin'
IF NOT EXISTS ( SELECT TOP 1 1 FROM ScreenUserRoleMap WHERE ScreenId = @ScreenId AND RoleId = @RoleId)
	BEGIN
		INSERT INTO [dbo].[ScreenUserRoleMap]
           ([ScreenId]
           ,[RoleId]
           ,[RecordStatus]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[ModifiedBy]
           ,[ModifiedDate]
           ,[CanEdit]
           ,[DisplayScreenName])
		VALUES
		(
			@ScreenId, 			
			@RoleId,
			@RecordStatus,	
			@UserName,				
			@CurrentDate,	
			@UserName,				
			@CurrentDate,	
			1,
			'Configurations'
		)
	END

SELECT  @ScreenId = ScreenId FROM Screen WHERE ScreenCode = 'CON'
SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'Executive'
IF NOT EXISTS ( SELECT TOP 1 1 FROM ScreenUserRoleMap WHERE ScreenId = @ScreenId AND RoleId = @RoleId)
	BEGIN
		INSERT INTO [dbo].[ScreenUserRoleMap]
           ([ScreenId]
           ,[RoleId]
           ,[RecordStatus]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[ModifiedBy]
           ,[ModifiedDate]
           ,[CanEdit]
           ,[DisplayScreenName])
		VALUES
		(
			@ScreenId, 			
			@RoleId,
			@RecordStatus,	
			@UserName,				
			@CurrentDate,	
			@UserName,				
			@CurrentDate,	
			0,
			'Configurations'
		)
	END
	
SELECT  @ScreenId = ScreenId FROM Screen WHERE ScreenCode = 'RP'
SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'Admin'
IF NOT EXISTS ( SELECT TOP 1 1 FROM ScreenUserRoleMap WHERE ScreenId = @ScreenId AND RoleId = @RoleId)
	BEGIN
		INSERT INTO [dbo].[ScreenUserRoleMap]
           ([ScreenId]
           ,[RoleId]
           ,[RecordStatus]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[ModifiedBy]
           ,[ModifiedDate]
           ,[CanEdit]
           ,[DisplayScreenName])
		VALUES
		(
			@ScreenId, 			
			@RoleId,
			@RecordStatus,	
			@UserName,				
			@CurrentDate,	
			@UserName,				
			@CurrentDate,	
			NULL,
			'Reports'
		)
	END

SELECT  @ScreenId = ScreenId FROM Screen WHERE ScreenCode = 'RP'
SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'Manager'
IF NOT EXISTS ( SELECT TOP 1 1 FROM ScreenUserRoleMap WHERE ScreenId = @ScreenId AND RoleId = @RoleId)
	BEGIN
		INSERT INTO [dbo].[ScreenUserRoleMap]
           ([ScreenId]
           ,[RoleId]
           ,[RecordStatus]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[ModifiedBy]
           ,[ModifiedDate]
           ,[CanEdit]
           ,[DisplayScreenName])
		VALUES
		(
			@ScreenId, 			
			@RoleId,
			@RecordStatus,	
			@UserName,				
			@CurrentDate,	
			@UserName,				
			@CurrentDate,	
			NULL,
			'Reports'
		)
	END

SELECT  @ScreenId = ScreenId FROM Screen WHERE ScreenCode = 'RP'
SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'Executive'
IF NOT EXISTS ( SELECT TOP 1 1 FROM ScreenUserRoleMap WHERE ScreenId = @ScreenId AND RoleId = @RoleId)
	BEGIN
		INSERT INTO [dbo].[ScreenUserRoleMap]
           ([ScreenId]
           ,[RoleId]
           ,[RecordStatus]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[ModifiedBy]
           ,[ModifiedDate]
           ,[CanEdit]
           ,[DisplayScreenName])
		VALUES
		(
			@ScreenId, 			
			@RoleId,
			@RecordStatus,	
			@UserName,				
			@CurrentDate,	
			@UserName,				
			@CurrentDate,	
			NULL,
			'Reports'
		)
	END

-- Edit permissions of Completed Checklists page
SELECT @ScreenId = ScreenId FROM Screen WHERE ScreenCode = 'CCL'
IF EXISTS(SELECT TOP 1 1 FROM ScreenUserRoleMap WHERE ScreenId = @ScreenId)
BEGIN
	SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'Admin'
	IF EXISTS(SELECT TOP 1 1 FROM ScreenUserRoleMap WHERE ScreenId = @ScreenId AND RoleId = @RoleId AND CanEdit IS NULL)
	BEGIN
		UPDATE ScreenUserRoleMap
		SET CanEdit = 1
		WHERE ScreenId = @ScreenId AND RoleId = @RoleId
	END

	SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'Executive'
	IF EXISTS(SELECT TOP 1 1 FROM ScreenUserRoleMap WHERE ScreenId = @ScreenId AND RoleId = @RoleId AND CanEdit IS NULL)
	BEGIN
		UPDATE ScreenUserRoleMap
		SET CanEdit = 0
		WHERE ScreenId = @ScreenId AND RoleId = @RoleId
	END

	SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'Manager'
	IF EXISTS(SELECT TOP 1 1 FROM ScreenUserRoleMap WHERE ScreenId = @ScreenId AND RoleId = @RoleId AND CanEdit IS NULL)
	BEGIN
		UPDATE ScreenUserRoleMap
		SET CanEdit = 0
		WHERE ScreenId = @ScreenId AND RoleId = @RoleId
	END

	SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'User'
	IF EXISTS(SELECT TOP 1 1 FROM ScreenUserRoleMap WHERE ScreenId = @ScreenId AND RoleId = @RoleId AND CanEdit IS NULL)
	BEGIN
		UPDATE ScreenUserRoleMap
		SET CanEdit = 0
		WHERE ScreenId = @ScreenId AND RoleId = @RoleId
	END
END

SELECT @ScreenId = ScreenId FROM Screen WHERE ScreenCode = 'VACL'
SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'Admin'
IF EXISTS ( SELECT TOP 1 1 FROM ScreenUserRoleMap WHERE ScreenId = @ScreenId AND RoleId = @RoleId)
BEGIN
   UPDATE ScreenUserRoleMap 
   SET CanEdit = 1 
   WHERE ScreenId = @ScreenId AND RoleId = @RoleId
END

SELECT @ScreenId = ScreenId FROM Screen WHERE ScreenCode = 'VACL'
SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'Executive'
IF EXISTS ( SELECT TOP 1 1 FROM ScreenUserRoleMap WHERE ScreenId = @ScreenId AND RoleId = @RoleId)
BEGIN
   UPDATE ScreenUserRoleMap 
   SET CanEdit = 1 
   WHERE ScreenId = @ScreenId AND RoleId = @RoleId
END

SELECT @ScreenId = ScreenId FROM Screen WHERE ScreenCode = 'CL'
SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'Admin'
IF EXISTS ( SELECT TOP 1 1 FROM ScreenUserRoleMap WHERE ScreenId = @ScreenId AND RoleId = @RoleId)
BEGIN
   UPDATE ScreenUserRoleMap 
   SET DisplayScreenName = 'Checklists' 
   WHERE ScreenId = @ScreenId AND RoleId = @RoleId
END

SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'Executive'
IF EXISTS ( SELECT TOP 1 1 FROM ScreenUserRoleMap WHERE ScreenId = @ScreenId AND RoleId = @RoleId)
BEGIN
   UPDATE ScreenUserRoleMap 
   SET DisplayScreenName = 'Checklists' 
   WHERE ScreenId = @ScreenId AND RoleId = @RoleId
END

SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'Manager'
IF EXISTS ( SELECT TOP 1 1 FROM ScreenUserRoleMap WHERE ScreenId = @ScreenId AND RoleId = @RoleId)
BEGIN
   UPDATE ScreenUserRoleMap 
   SET DisplayScreenName = 'Checklists' 
   WHERE ScreenId = @ScreenId AND RoleId = @RoleId
END

SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'User'
IF EXISTS ( SELECT TOP 1 1 FROM ScreenUserRoleMap WHERE ScreenId = @ScreenId AND RoleId = @RoleId)
BEGIN
   UPDATE ScreenUserRoleMap 
   SET DisplayScreenName = 'Checklists' 
   WHERE ScreenId = @ScreenId AND RoleId = @RoleId
END

SELECT @ScreenId = ScreenId FROM Screen WHERE ScreenCode = 'PCL'
SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'Admin'
IF EXISTS ( SELECT TOP 1 1 FROM ScreenUserRoleMap WHERE ScreenId = @ScreenId AND RoleId = @RoleId)
BEGIN
   UPDATE ScreenUserRoleMap 
   SET DisplayScreenName = 'Pending Checklists' 
   WHERE ScreenId = @ScreenId AND RoleId = @RoleId
END

SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'Executive'
IF EXISTS ( SELECT TOP 1 1 FROM ScreenUserRoleMap WHERE ScreenId = @ScreenId AND RoleId = @RoleId)
BEGIN
   UPDATE ScreenUserRoleMap 
   SET DisplayScreenName = 'Pending Checklists' 
   WHERE ScreenId = @ScreenId AND RoleId = @RoleId
END

SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'Manager'
IF EXISTS ( SELECT TOP 1 1 FROM ScreenUserRoleMap WHERE ScreenId = @ScreenId AND RoleId = @RoleId)
BEGIN
   UPDATE ScreenUserRoleMap 
   SET DisplayScreenName = 'Pending Checklists' 
   WHERE ScreenId = @ScreenId AND RoleId = @RoleId
END

SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'User'
IF EXISTS ( SELECT TOP 1 1 FROM ScreenUserRoleMap WHERE ScreenId = @ScreenId AND RoleId = @RoleId)
BEGIN
   UPDATE ScreenUserRoleMap 
   SET DisplayScreenName = 'Pending Checklists' 
   WHERE ScreenId = @ScreenId AND RoleId = @RoleId
END


SELECT @ScreenId = ScreenId FROM Screen WHERE ScreenCode = 'CCL'
SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'Admin'
IF EXISTS ( SELECT TOP 1 1 FROM ScreenUserRoleMap WHERE ScreenId = @ScreenId AND RoleId = @RoleId)
BEGIN
   UPDATE ScreenUserRoleMap 
   SET DisplayScreenName = 'Completed Checklists' 
   WHERE ScreenId = @ScreenId AND RoleId = @RoleId
END

SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'Executive'
IF EXISTS ( SELECT TOP 1 1 FROM ScreenUserRoleMap WHERE ScreenId = @ScreenId AND RoleId = @RoleId)
BEGIN
   UPDATE ScreenUserRoleMap 
   SET DisplayScreenName = 'Completed Checklists' 
   WHERE ScreenId = @ScreenId AND RoleId = @RoleId
END

SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'Manager'
IF EXISTS ( SELECT TOP 1 1 FROM ScreenUserRoleMap WHERE ScreenId = @ScreenId AND RoleId = @RoleId)
BEGIN
   UPDATE ScreenUserRoleMap 
   SET DisplayScreenName = 'Completed Checklists' 
   WHERE ScreenId = @ScreenId AND RoleId = @RoleId
END

SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'User'
IF EXISTS ( SELECT TOP 1 1 FROM ScreenUserRoleMap WHERE ScreenId = @ScreenId AND RoleId = @RoleId)
BEGIN
   UPDATE ScreenUserRoleMap 
   SET DisplayScreenName = 'Completed Checklists' 
   WHERE ScreenId = @ScreenId AND RoleId = @RoleId
END

SELECT @ScreenId = ScreenId FROM Screen WHERE ScreenCode = 'CVE'
SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'Admin'
IF EXISTS ( SELECT TOP 1 1 FROM ScreenUserRoleMap WHERE ScreenId = @ScreenId AND RoleId = @RoleId)
BEGIN
   UPDATE ScreenUserRoleMap 
   SET DisplayScreenName = 'View / Edit Client' 
   WHERE ScreenId = @ScreenId AND RoleId = @RoleId
END

SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'Executive'
IF EXISTS ( SELECT TOP 1 1 FROM ScreenUserRoleMap WHERE ScreenId = @ScreenId AND RoleId = @RoleId)
BEGIN
   UPDATE ScreenUserRoleMap 
   SET DisplayScreenName = 'View / Edit Client' 
   WHERE ScreenId = @ScreenId AND RoleId = @RoleId
END

SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'Manager'
IF EXISTS ( SELECT TOP 1 1 FROM ScreenUserRoleMap WHERE ScreenId = @ScreenId AND RoleId = @RoleId)
BEGIN
   UPDATE ScreenUserRoleMap 
   SET DisplayScreenName = 'View / Edit Client' 
   WHERE ScreenId = @ScreenId AND RoleId = @RoleId
END

SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'User'
IF EXISTS ( SELECT TOP 1 1 FROM ScreenUserRoleMap WHERE ScreenId = @ScreenId AND RoleId = @RoleId)
BEGIN
   UPDATE ScreenUserRoleMap 
   SET DisplayScreenName = 'View / Edit Client' 
   WHERE ScreenId = @ScreenId AND RoleId = @RoleId
END

SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'Client'
IF EXISTS ( SELECT TOP 1 1 FROM ScreenUserRoleMap WHERE ScreenId = @ScreenId AND RoleId = @RoleId)
BEGIN
   UPDATE ScreenUserRoleMap 
   SET DisplayScreenName = 'View KPI' 
   WHERE ScreenId = @ScreenId AND RoleId = @RoleId
END

SELECT @ScreenId = ScreenId FROM Screen WHERE ScreenCode = 'CN'
SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'Admin'
IF EXISTS ( SELECT TOP 1 1 FROM ScreenUserRoleMap WHERE ScreenId = @ScreenId AND RoleId = @RoleId)
BEGIN
   UPDATE ScreenUserRoleMap 
   SET DisplayScreenName = 'Create Client' 
   WHERE ScreenId = @ScreenId AND RoleId = @RoleId
END

SELECT @ScreenId = ScreenId FROM Screen WHERE ScreenCode = 'VA'
SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'Admin'
IF EXISTS ( SELECT TOP 1 1 FROM ScreenUserRoleMap WHERE ScreenId = @ScreenId AND RoleId = @RoleId)
BEGIN
   UPDATE ScreenUserRoleMap 
   SET DisplayScreenName = 'All Clients' 
   WHERE ScreenId = @ScreenId AND RoleId = @RoleId
END

SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'Executive'
IF EXISTS ( SELECT TOP 1 1 FROM ScreenUserRoleMap WHERE ScreenId = @ScreenId AND RoleId = @RoleId)
BEGIN
   UPDATE ScreenUserRoleMap 
   SET DisplayScreenName = 'All Clients' 
   WHERE ScreenId = @ScreenId AND RoleId = @RoleId
END

SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'Manager'
IF EXISTS ( SELECT TOP 1 1 FROM ScreenUserRoleMap WHERE ScreenId = @ScreenId AND RoleId = @RoleId)
BEGIN
   UPDATE ScreenUserRoleMap 
   SET DisplayScreenName = 'All Clients' 
   WHERE ScreenId = @ScreenId AND RoleId = @RoleId
END

SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'User'
IF EXISTS ( SELECT TOP 1 1 FROM ScreenUserRoleMap WHERE ScreenId = @ScreenId AND RoleId = @RoleId)
BEGIN
   UPDATE ScreenUserRoleMap 
   SET DisplayScreenName = 'All Clients' 
   WHERE ScreenId = @ScreenId AND RoleId = @RoleId
END

SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'Client'
IF EXISTS ( SELECT TOP 1 1 FROM ScreenUserRoleMap WHERE ScreenId = @ScreenId AND RoleId = @RoleId)
BEGIN
   UPDATE ScreenUserRoleMap 
   SET DisplayScreenName = 'All Clients' 
   WHERE ScreenId = @ScreenId AND RoleId = @RoleId
END


SELECT @ScreenId = ScreenId FROM Screen WHERE ScreenCode = 'ACL'
SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'Admin'
IF EXISTS ( SELECT TOP 1 1 FROM ScreenUserRoleMap WHERE ScreenId = @ScreenId AND RoleId = @RoleId)
BEGIN
   UPDATE ScreenUserRoleMap 
   SET DisplayScreenName = 'Checklists' 
   WHERE ScreenId = @ScreenId AND RoleId = @RoleId
END

SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'Executive'
IF EXISTS ( SELECT TOP 1 1 FROM ScreenUserRoleMap WHERE ScreenId = @ScreenId AND RoleId = @RoleId)
BEGIN
   UPDATE ScreenUserRoleMap 
   SET DisplayScreenName = 'Checklists' 
   WHERE ScreenId = @ScreenId AND RoleId = @RoleId
END

SELECT @ScreenId = ScreenId FROM Screen WHERE ScreenCode = 'VEK'
SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'Admin'
IF EXISTS ( SELECT TOP 1 1 FROM ScreenUserRoleMap WHERE ScreenId = @ScreenId AND RoleId = @RoleId)
BEGIN
   UPDATE ScreenUserRoleMap 
   SET DisplayScreenName = 'View / Edit KPI' 
   WHERE ScreenId = @ScreenId AND RoleId = @RoleId
END

SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'Executive'
IF EXISTS ( SELECT TOP 1 1 FROM ScreenUserRoleMap WHERE ScreenId = @ScreenId AND RoleId = @RoleId)
BEGIN
   UPDATE ScreenUserRoleMap 
   SET DisplayScreenName = 'View / Edit KPI' 
   WHERE ScreenId = @ScreenId AND RoleId = @RoleId
END

SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'Manager'
IF EXISTS ( SELECT TOP 1 1 FROM ScreenUserRoleMap WHERE ScreenId = @ScreenId AND RoleId = @RoleId)
BEGIN
   UPDATE ScreenUserRoleMap 
   SET DisplayScreenName = 'View KPI' 
   WHERE ScreenId = @ScreenId AND RoleId = @RoleId
END

SELECT @ScreenId = ScreenId FROM Screen WHERE ScreenCode = 'CECI'
SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'Admin'
IF EXISTS ( SELECT TOP 1 1 FROM ScreenUserRoleMap WHERE ScreenId = @ScreenId AND RoleId = @RoleId)
BEGIN
   UPDATE ScreenUserRoleMap 
   SET DisplayScreenName = 'Create / Edit Checklist Item' 
   WHERE ScreenId = @ScreenId AND RoleId = @RoleId
END

SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'Executive'
IF EXISTS ( SELECT TOP 1 1 FROM ScreenUserRoleMap WHERE ScreenId = @ScreenId AND RoleId = @RoleId)
BEGIN
   UPDATE ScreenUserRoleMap 
   SET DisplayScreenName = 'Create / Edit Checklist Item' 
   WHERE ScreenId = @ScreenId AND RoleId = @RoleId
END

SELECT @ScreenId = ScreenId FROM Screen WHERE ScreenCode = 'CLS'
SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'Admin'
IF EXISTS ( SELECT TOP 1 1 FROM ScreenUserRoleMap WHERE ScreenId = @ScreenId AND RoleId = @RoleId)
BEGIN
   UPDATE ScreenUserRoleMap 
   SET DisplayScreenName = 'Create Checklist' 
   WHERE ScreenId = @ScreenId AND RoleId = @RoleId
END

SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'Executive'
IF EXISTS ( SELECT TOP 1 1 FROM ScreenUserRoleMap WHERE ScreenId = @ScreenId AND RoleId = @RoleId)
BEGIN
   UPDATE ScreenUserRoleMap 
   SET DisplayScreenName = 'Create Checklist' 
   WHERE ScreenId = @ScreenId AND RoleId = @RoleId
END

SELECT @ScreenId = ScreenId FROM Screen WHERE ScreenCode = 'VACL'
SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'Admin'
IF EXISTS ( SELECT TOP 1 1 FROM ScreenUserRoleMap WHERE ScreenId = @ScreenId AND RoleId = @RoleId)
BEGIN
   UPDATE ScreenUserRoleMap 
   SET DisplayScreenName = 'All Checklists' 
   WHERE ScreenId = @ScreenId AND RoleId = @RoleId
END

SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'Executive'
IF EXISTS ( SELECT TOP 1 1 FROM ScreenUserRoleMap WHERE ScreenId = @ScreenId AND RoleId = @RoleId)
BEGIN
   UPDATE ScreenUserRoleMap 
   SET DisplayScreenName = 'All Checklists' 
   WHERE ScreenId = @ScreenId AND RoleId = @RoleId
END

SELECT @ScreenId = ScreenId FROM Screen WHERE ScreenCode = 'CCU'
SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'Manager'
IF EXISTS ( SELECT TOP 1 1 FROM ScreenUserRoleMap WHERE ScreenId = @ScreenId AND RoleId = @RoleId)
BEGIN
   UPDATE ScreenUserRoleMap 
   SET DisplayScreenName = 'Create Client Users' 
   WHERE ScreenId = @ScreenId AND RoleId = @RoleId
END


SELECT @ScreenId = ScreenId FROM Screen WHERE ScreenCode = 'VEK'
SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'Admin'
IF EXISTS ( SELECT TOP 1 1 FROM ScreenUserRoleMap WHERE ScreenId = @ScreenId AND RoleId = @RoleId)
BEGIN
   UPDATE ScreenUserRoleMap 
   SET CanEdit = 1   
   WHERE ScreenId = @ScreenId AND RoleId = @RoleId
END

SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'Executive'
IF EXISTS ( SELECT TOP 1 1 FROM ScreenUserRoleMap WHERE ScreenId = @ScreenId AND RoleId = @RoleId)
BEGIN
   UPDATE ScreenUserRoleMap 
   SET CanEdit = 1 
   WHERE ScreenId = @ScreenId AND RoleId = @RoleId
END

SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'Manager'
IF EXISTS ( SELECT TOP 1 1 FROM ScreenUserRoleMap WHERE ScreenId = @ScreenId AND RoleId = @RoleId)
BEGIN
   UPDATE ScreenUserRoleMap 
   SET CanEdit = 0 
   WHERE ScreenId = @ScreenId AND RoleId = @RoleId
END

SELECT @ScreenId = ScreenId FROM Screen WHERE ScreenCode = 'AHM'
IF EXISTS ( SELECT TOP 1 1 FROM ScreenUserRoleMap WHERE ScreenId = @ScreenId AND DisplayScreenName = 'Heat Map')
BEGIN
   UPDATE ScreenUserRoleMap 
   SET DisplayScreenName = 'Heat Map Setup'
   WHERE ScreenId = @ScreenId
END

SELECT @ScreenId = ScreenId FROM Screen WHERE ScreenCode = 'CK'
SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'Admin'
IF EXISTS ( SELECT TOP 1 1 FROM ScreenUserRoleMap WHERE ScreenId = @ScreenId AND RoleId = @RoleId AND CanEdit IS NULL)
BEGIN
   UPDATE ScreenUserRoleMap
   SET CanEdit = 1
   WHERE ScreenId = @ScreenId AND RoleId = @RoleId
END

SELECT @ScreenId = ScreenId FROM Screen WHERE ScreenCode = 'CK'
SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'Executive'
IF EXISTS ( SELECT TOP 1 1 FROM ScreenUserRoleMap WHERE ScreenId = @ScreenId AND RoleId = @RoleId AND CanEdit IS NULL)
BEGIN
   UPDATE ScreenUserRoleMap
   SET CanEdit = 1
   WHERE ScreenId = @ScreenId AND RoleId = @RoleId
END

SELECT @ScreenId = ScreenId FROM Screen WHERE ScreenCode = 'SYS'
SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'Admin'
IF EXISTS ( SELECT TOP 1 1 FROM ScreenUserRoleMap WHERE ScreenId = @ScreenId AND RoleId = @RoleId AND CanEdit IS NULL)
BEGIN
   UPDATE ScreenUserRoleMap
   SET CanEdit = 1
   WHERE ScreenId = @ScreenId AND RoleId = @RoleId
END

SELECT @ScreenId = ScreenId FROM Screen WHERE ScreenCode = 'SYS'
SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'Executive'
IF EXISTS ( SELECT TOP 1 1 FROM ScreenUserRoleMap WHERE ScreenId = @ScreenId AND RoleId = @RoleId AND CanEdit IS NULL)
BEGIN
   UPDATE ScreenUserRoleMap
   SET CanEdit = 0
   WHERE ScreenId = @ScreenId AND RoleId = @RoleId
END

-- Insert scripts for to-do-list page access.

SELECT  @ScreenId = ScreenId FROM Screen WHERE ScreenCode = 'TDL'
SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'Admin'
IF NOT EXISTS ( SELECT TOP 1 1 FROM ScreenUserRoleMap WHERE ScreenId = @ScreenId AND RoleId = @RoleId)
BEGIN
		INSERT INTO [dbo].[ScreenUserRoleMap]
           ([ScreenId]
           ,[RoleId]
           ,[RecordStatus]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[ModifiedBy]
           ,[ModifiedDate]
           ,[CanEdit]
           ,[DisplayScreenName])
		VALUES
		(
			@ScreenId, 			
			@RoleId,
			@RecordStatus,	
			@UserName,				
			@CurrentDate,	
			@UserName,				
			@CurrentDate,	
			1,
			'To-Do-List'
		)
END

SELECT  @ScreenId = ScreenId FROM Screen WHERE ScreenCode = 'TDL'
SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'Executive'
IF NOT EXISTS ( SELECT TOP 1 1 FROM ScreenUserRoleMap WHERE ScreenId = @ScreenId AND RoleId = @RoleId)
BEGIN
		INSERT INTO [dbo].[ScreenUserRoleMap]
           ([ScreenId]
           ,[RoleId]
           ,[RecordStatus]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[ModifiedBy]
           ,[ModifiedDate]
           ,[CanEdit]
           ,[DisplayScreenName])
		VALUES
		(
			@ScreenId, 			
			@RoleId,
			@RecordStatus,	
			@UserName,				
			@CurrentDate,	
			@UserName,				
			@CurrentDate,	
			1,
			'To-Do-List'
		)
END

SELECT  @ScreenId = ScreenId FROM Screen WHERE ScreenCode = 'TDL'
SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'Manager'
IF NOT EXISTS ( SELECT TOP 1 1 FROM ScreenUserRoleMap WHERE ScreenId = @ScreenId AND RoleId = @RoleId)
BEGIN
		INSERT INTO [dbo].[ScreenUserRoleMap]
           ([ScreenId]
           ,[RoleId]
           ,[RecordStatus]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[ModifiedBy]
           ,[ModifiedDate]
           ,[CanEdit]
           ,[DisplayScreenName])
		VALUES
		(
			@ScreenId, 			
			@RoleId,
			@RecordStatus,	
			@UserName,				
			@CurrentDate,	
			@UserName,				
			@CurrentDate,	
			1,
			'To-Do-List'
		)
END

SELECT  @ScreenId = ScreenId FROM Screen WHERE ScreenCode = 'TDL'
SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'User'
IF NOT EXISTS ( SELECT TOP 1 1 FROM ScreenUserRoleMap WHERE ScreenId = @ScreenId AND RoleId = @RoleId)
BEGIN
		INSERT INTO [dbo].[ScreenUserRoleMap]
           ([ScreenId]
           ,[RoleId]
           ,[RecordStatus]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[ModifiedBy]
           ,[ModifiedDate]
           ,[CanEdit]
           ,[DisplayScreenName])
		VALUES
		(
			@ScreenId, 			
			@RoleId,
			@RecordStatus,	
			@UserName,				
			@CurrentDate,	
			@UserName,				
			@CurrentDate,	
			1,
			'To-Do-List'
		)
END

COMMIT TRANSACTION
END TRY

BEGIN CATCH
	SELECT ERROR_MESSAGE()
		,ERROR_LINE()
		,ERROR_SEVERITY()

	ROLLBACK TRANSACTION
END CATCH
GO
