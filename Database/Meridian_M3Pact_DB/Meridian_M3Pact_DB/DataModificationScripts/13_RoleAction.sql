/*
Script Name : 13_Screen
Module_Name : M3Pact
Created By  : Abhishek K
CreatedDate : 05/11/2018

Data Verification Script :

SELECT * FROM [RoleAction](NOLOCK) WHERE RecordStatus = 'A'


Revision History 
=============
Ver  Date				Who						Comment
1.0  30-July-2018		Abhishek Kovvuri		Inserting view M3 actual and forecasted actions for admin and executive.
1.1  08-Aug-2018        Abhishek Kovvuri        Inserting role based actions for to-do-list page.
*/


BEGIN TRY
BEGIN TRANSACTION

DECLARE @UserName NVARCHAR(60) = 'Admin'
DECLARE @CurrentDate DATETIME = GETDATE()
DECLARE @RecordStatus CHAR(1) = 'A'
DECLARE @RoleId INT;
DECLARE @ScreenActionId INT;

SELECT  @ScreenActionId = ScreenActionId FROM ScreenAction WHERE ActionName = 'ViewCloseMonthButton'
SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'Admin'
IF NOT EXISTS ( SELECT TOP 1 1 FROM RoleAction WHERE RoleId = @RoleId AND ScreenActionId = @ScreenActionId)
	BEGIN
		INSERT INTO [dbo].[RoleAction]
           ([RoleId]
           ,[ScreenActionId]
           ,[RecordStatus]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[ModifiedBy]
           ,[ModifiedDate])
		VALUES
		(
			@RoleId, 			
			@ScreenActionId,
			@RecordStatus,	
			@UserName,				
			@CurrentDate,	
			@UserName,				
			@CurrentDate
		)
	END

SELECT  @ScreenActionId = ScreenActionId FROM ScreenAction WHERE ActionName = 'ViewCloseMonthButton'
SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'Manager'
IF NOT EXISTS ( SELECT TOP 1 1 FROM RoleAction WHERE RoleId = @RoleId AND ScreenActionId = @ScreenActionId)
	BEGIN
		INSERT INTO [dbo].[RoleAction]
           ([RoleId]
           ,[ScreenActionId]
           ,[RecordStatus]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[ModifiedBy]
           ,[ModifiedDate])
		VALUES
		(
			@RoleId, 			
			@ScreenActionId,
			@RecordStatus,	
			@UserName,				
			@CurrentDate,	
			@UserName,				
			@CurrentDate
		)
	END

SELECT  @ScreenActionId = ScreenActionId FROM ScreenAction WHERE ActionName = 'ViewCloseMonthButton'
SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'User'
IF NOT EXISTS ( SELECT TOP 1 1 FROM RoleAction WHERE RoleId = @RoleId AND ScreenActionId = @ScreenActionId)
	BEGIN
		INSERT INTO [dbo].[RoleAction]
           ([RoleId]
           ,[ScreenActionId]
           ,[RecordStatus]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[ModifiedBy]
           ,[ModifiedDate])
		VALUES
		(
			@RoleId, 			
			@ScreenActionId,
			@RecordStatus,	
			@UserName,				
			@CurrentDate,	
			@UserName,				
			@CurrentDate
		)
	END

SELECT  @ScreenActionId = ScreenActionId FROM ScreenAction WHERE ActionName = 'ViewAddDepositSection'
SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'Admin'
IF NOT EXISTS ( SELECT TOP 1 1 FROM RoleAction WHERE RoleId = @RoleId AND ScreenActionId = @ScreenActionId)
	BEGIN
		INSERT INTO [dbo].[RoleAction]
           ([RoleId]
           ,[ScreenActionId]
           ,[RecordStatus]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[ModifiedBy]
           ,[ModifiedDate])
		VALUES
		(
			@RoleId, 			
			@ScreenActionId,
			@RecordStatus,	
			@UserName,				
			@CurrentDate,	
			@UserName,				
			@CurrentDate
		)
	END

SELECT  @ScreenActionId = ScreenActionId FROM ScreenAction WHERE ActionName = 'ViewAddDepositSection'
SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'Manager'
IF NOT EXISTS ( SELECT TOP 1 1 FROM RoleAction WHERE RoleId = @RoleId AND ScreenActionId = @ScreenActionId)
	BEGIN
		INSERT INTO [dbo].[RoleAction]
           ([RoleId]
           ,[ScreenActionId]
           ,[RecordStatus]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[ModifiedBy]
           ,[ModifiedDate])
		VALUES
		(
			@RoleId, 			
			@ScreenActionId,
			@RecordStatus,	
			@UserName,				
			@CurrentDate,	
			@UserName,				
			@CurrentDate
		)
	END

SELECT  @ScreenActionId = ScreenActionId FROM ScreenAction WHERE ActionName = 'ViewAddDepositSection'
SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'User'
IF NOT EXISTS ( SELECT TOP 1 1 FROM RoleAction WHERE RoleId = @RoleId AND ScreenActionId = @ScreenActionId)
	BEGIN
		INSERT INTO [dbo].[RoleAction]
           ([RoleId]
           ,[ScreenActionId]
           ,[RecordStatus]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[ModifiedBy]
           ,[ModifiedDate])
		VALUES
		(
			@RoleId, 			
			@ScreenActionId,
			@RecordStatus,	
			@UserName,				
			@CurrentDate,	
			@UserName,				
			@CurrentDate
		)
	END

SELECT  @ScreenActionId = ScreenActionId FROM ScreenAction WHERE ActionName = 'OpenClosedMonth'
SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'Admin'
IF NOT EXISTS ( SELECT TOP 1 1 FROM RoleAction WHERE RoleId = @RoleId AND ScreenActionId = @ScreenActionId)
	BEGIN
		INSERT INTO [dbo].[RoleAction]
           ([RoleId]
           ,[ScreenActionId]
           ,[RecordStatus]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[ModifiedBy]
           ,[ModifiedDate])
		VALUES
		(
			@RoleId, 			
			@ScreenActionId,
			@RecordStatus,	
			@UserName,				
			@CurrentDate,	
			@UserName,				
			@CurrentDate
		)
	END

SELECT  @ScreenActionId = ScreenActionId FROM ScreenAction WHERE ActionName = 'AddSite'
SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'Admin'
IF NOT EXISTS ( SELECT TOP 1 1 FROM RoleAction WHERE RoleId = @RoleId AND ScreenActionId = @ScreenActionId)
	BEGIN
		INSERT INTO [dbo].[RoleAction]
           ([RoleId]
           ,[ScreenActionId]
           ,[RecordStatus]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[ModifiedBy]
           ,[ModifiedDate])
		VALUES
		(
			@RoleId, 			
			@ScreenActionId,
			@RecordStatus,	
			@UserName,				
			@CurrentDate,	
			@UserName,				
			@CurrentDate
		)
	END

SELECT  @ScreenActionId = ScreenActionId FROM ScreenAction WHERE ActionName = 'EditSite'
SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'Admin'
IF NOT EXISTS ( SELECT TOP 1 1 FROM RoleAction WHERE RoleId = @RoleId AND ScreenActionId = @ScreenActionId)
	BEGIN
		INSERT INTO [dbo].[RoleAction]
           ([RoleId]
           ,[ScreenActionId]
           ,[RecordStatus]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[ModifiedBy]
           ,[ModifiedDate])
		VALUES
		(
			@RoleId, 			
			@ScreenActionId,
			@RecordStatus,	
			@UserName,				
			@CurrentDate,	
			@UserName,				
			@CurrentDate
		)
	END

SELECT  @ScreenActionId = ScreenActionId FROM ScreenAction WHERE ActionName = 'AddSpecialty'
SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'Admin'
IF NOT EXISTS ( SELECT TOP 1 1 FROM RoleAction WHERE RoleId = @RoleId AND ScreenActionId = @ScreenActionId)
	BEGIN
		INSERT INTO [dbo].[RoleAction]
           ([RoleId]
           ,[ScreenActionId]
           ,[RecordStatus]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[ModifiedBy]
           ,[ModifiedDate])
		VALUES
		(
			@RoleId, 			
			@ScreenActionId,
			@RecordStatus,	
			@UserName,				
			@CurrentDate,	
			@UserName,				
			@CurrentDate
		)
	END

SELECT  @ScreenActionId = ScreenActionId FROM ScreenAction WHERE ActionName = 'AddSpecialty'
SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'Executive'
IF NOT EXISTS ( SELECT TOP 1 1 FROM RoleAction WHERE RoleId = @RoleId AND ScreenActionId = @ScreenActionId)
	BEGIN
		INSERT INTO [dbo].[RoleAction]
           ([RoleId]
           ,[ScreenActionId]
           ,[RecordStatus]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[ModifiedBy]
           ,[ModifiedDate])
		VALUES
		(
			@RoleId, 			
			@ScreenActionId,
			@RecordStatus,	
			@UserName,				
			@CurrentDate,	
			@UserName,				
			@CurrentDate
		)
	END

SELECT  @ScreenActionId = ScreenActionId FROM ScreenAction WHERE ActionName = 'EditSpecialty'
SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'Admin'
IF NOT EXISTS ( SELECT TOP 1 1 FROM RoleAction WHERE RoleId = @RoleId AND ScreenActionId = @ScreenActionId)
	BEGIN
		INSERT INTO [dbo].[RoleAction]
           ([RoleId]
           ,[ScreenActionId]
           ,[RecordStatus]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[ModifiedBy]
           ,[ModifiedDate])
		VALUES
		(
			@RoleId, 			
			@ScreenActionId,
			@RecordStatus,	
			@UserName,				
			@CurrentDate,	
			@UserName,				
			@CurrentDate
		)
	END

SELECT  @ScreenActionId = ScreenActionId FROM ScreenAction WHERE ActionName = 'EditSpecialty'
SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'Executive'
IF NOT EXISTS ( SELECT TOP 1 1 FROM RoleAction WHERE RoleId = @RoleId AND ScreenActionId = @ScreenActionId)
	BEGIN
		INSERT INTO [dbo].[RoleAction]
           ([RoleId]
           ,[ScreenActionId]
           ,[RecordStatus]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[ModifiedBy]
           ,[ModifiedDate])
		VALUES
		(
			@RoleId, 			
			@ScreenActionId,
			@RecordStatus,	
			@UserName,				
			@CurrentDate,	
			@UserName,				
			@CurrentDate
		)
	END

SELECT  @ScreenActionId = ScreenActionId FROM ScreenAction WHERE ActionName = 'PerformClientActivateInactivate'
SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'Admin'
IF NOT EXISTS ( SELECT TOP 1 1 FROM RoleAction WHERE RoleId = @RoleId AND ScreenActionId = @ScreenActionId)
	BEGIN
		INSERT INTO [dbo].[RoleAction]
           ([RoleId]
           ,[ScreenActionId]
           ,[RecordStatus]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[ModifiedBy]
           ,[ModifiedDate])
		VALUES
		(
			@RoleId, 			
			@ScreenActionId,
			@RecordStatus,	
			@UserName,				
			@CurrentDate,	
			@UserName,				
			@CurrentDate
		)
	END

SELECT  @ScreenActionId = ScreenActionId FROM ScreenAction WHERE ActionName = 'EditClientData'
SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'Admin'
IF NOT EXISTS ( SELECT TOP 1 1 FROM RoleAction WHERE RoleId = @RoleId AND ScreenActionId = @ScreenActionId)
	BEGIN
		INSERT INTO [dbo].[RoleAction]
           ([RoleId]
           ,[ScreenActionId]
           ,[RecordStatus]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[ModifiedBy]
           ,[ModifiedDate])
		VALUES
		(
			@RoleId, 			
			@ScreenActionId,
			@RecordStatus,	
			@UserName,				
			@CurrentDate,	
			@UserName,				
			@CurrentDate
		)
	END

SELECT  @ScreenActionId = ScreenActionId FROM ScreenAction WHERE ActionName = 'EditClientData'
SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'Manager'
IF NOT EXISTS ( SELECT TOP 1 1 FROM RoleAction WHERE RoleId = @RoleId AND ScreenActionId = @ScreenActionId)
	BEGIN
		INSERT INTO [dbo].[RoleAction]
           ([RoleId]
           ,[ScreenActionId]
           ,[RecordStatus]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[ModifiedBy]
           ,[ModifiedDate])
		VALUES
		(
			@RoleId, 			
			@ScreenActionId,
			@RecordStatus,	
			@UserName,				
			@CurrentDate,	
			@UserName,				
			@CurrentDate
		)
	END

SELECT  @ScreenActionId = ScreenActionId FROM ScreenAction WHERE ActionName = 'EditClientData'
SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'Executive'
IF NOT EXISTS ( SELECT TOP 1 1 FROM RoleAction WHERE RoleId = @RoleId AND ScreenActionId = @ScreenActionId)
	BEGIN
		INSERT INTO [dbo].[RoleAction]
           ([RoleId]
           ,[ScreenActionId]
           ,[RecordStatus]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[ModifiedBy]
           ,[ModifiedDate])
		VALUES
		(
			@RoleId, 			
			@ScreenActionId,
			@RecordStatus,	
			@UserName,				
			@CurrentDate,	
			@UserName,				
			@CurrentDate
		)
	END

SELECT  @ScreenActionId = ScreenActionId FROM ScreenAction WHERE ActionName = 'AddClientData'
SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'Admin'
IF NOT EXISTS ( SELECT TOP 1 1 FROM RoleAction WHERE RoleId = @RoleId AND ScreenActionId = @ScreenActionId)
	BEGIN
		INSERT INTO [dbo].[RoleAction]
           ([RoleId]
           ,[ScreenActionId]
           ,[RecordStatus]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[ModifiedBy]
           ,[ModifiedDate])
		VALUES
		(
			@RoleId, 			
			@ScreenActionId,
			@RecordStatus,	
			@UserName,				
			@CurrentDate,	
			@UserName,				
			@CurrentDate
		)
	END

SELECT  @ScreenActionId = ScreenActionId FROM ScreenAction WHERE ActionName = 'AddPayerAssociationToClient'
SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'Admin'
IF NOT EXISTS ( SELECT TOP 1 1 FROM RoleAction WHERE RoleId = @RoleId AND ScreenActionId = @ScreenActionId)
	BEGIN
		INSERT INTO [dbo].[RoleAction]
           ([RoleId]
           ,[ScreenActionId]
           ,[RecordStatus]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[ModifiedBy]
           ,[ModifiedDate])
		VALUES
		(
			@RoleId, 			
			@ScreenActionId,
			@RecordStatus,	
			@UserName,				
			@CurrentDate,	
			@UserName,				
			@CurrentDate
		)
	END

SELECT  @ScreenActionId = ScreenActionId FROM ScreenAction WHERE ActionName = 'AddPayerAssociationToClient'
SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'Manager'
IF NOT EXISTS ( SELECT TOP 1 1 FROM RoleAction WHERE RoleId = @RoleId AND ScreenActionId = @ScreenActionId)
	BEGIN
		INSERT INTO [dbo].[RoleAction]
           ([RoleId]
           ,[ScreenActionId]
           ,[RecordStatus]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[ModifiedBy]
           ,[ModifiedDate])
		VALUES
		(
			@RoleId, 			
			@ScreenActionId,
			@RecordStatus,	
			@UserName,				
			@CurrentDate,	
			@UserName,				
			@CurrentDate
		)
	END

SELECT  @ScreenActionId = ScreenActionId FROM ScreenAction WHERE ActionName = 'AddPayerAssociationToClient'
SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'Executive'
IF NOT EXISTS ( SELECT TOP 1 1 FROM RoleAction WHERE RoleId = @RoleId AND ScreenActionId = @ScreenActionId)
	BEGIN
		INSERT INTO [dbo].[RoleAction]
           ([RoleId]
           ,[ScreenActionId]
           ,[RecordStatus]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[ModifiedBy]
           ,[ModifiedDate])
		VALUES
		(
			@RoleId, 			
			@ScreenActionId,
			@RecordStatus,	
			@UserName,				
			@CurrentDate,	
			@UserName,				
			@CurrentDate
		)
	END

SELECT  @ScreenActionId = ScreenActionId FROM ScreenAction WHERE ActionName = 'AddPayerAssociationToClient'
SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'User'
IF NOT EXISTS ( SELECT TOP 1 1 FROM RoleAction WHERE RoleId = @RoleId AND ScreenActionId = @ScreenActionId)
	BEGIN
		INSERT INTO [dbo].[RoleAction]
           ([RoleId]
           ,[ScreenActionId]
           ,[RecordStatus]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[ModifiedBy]
           ,[ModifiedDate])
		VALUES
		(
			@RoleId, 			
			@ScreenActionId,
			@RecordStatus,	
			@UserName,				
			@CurrentDate,	
			@UserName,				
			@CurrentDate
		)
	END

SELECT  @ScreenActionId = ScreenActionId FROM ScreenAction WHERE ActionName = 'EditPayerAssociationToClient'
SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'Admin'
IF NOT EXISTS ( SELECT TOP 1 1 FROM RoleAction WHERE RoleId = @RoleId AND ScreenActionId = @ScreenActionId)
	BEGIN
		INSERT INTO [dbo].[RoleAction]
           ([RoleId]
           ,[ScreenActionId]
           ,[RecordStatus]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[ModifiedBy]
           ,[ModifiedDate])
		VALUES
		(
			@RoleId, 			
			@ScreenActionId,
			@RecordStatus,	
			@UserName,				
			@CurrentDate,	
			@UserName,				
			@CurrentDate
		)
	END

SELECT  @ScreenActionId = ScreenActionId FROM ScreenAction WHERE ActionName = 'EditPayerAssociationToClient'
SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'Manager'
IF NOT EXISTS ( SELECT TOP 1 1 FROM RoleAction WHERE RoleId = @RoleId AND ScreenActionId = @ScreenActionId)
	BEGIN
		INSERT INTO [dbo].[RoleAction]
           ([RoleId]
           ,[ScreenActionId]
           ,[RecordStatus]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[ModifiedBy]
           ,[ModifiedDate])
		VALUES
		(
			@RoleId, 			
			@ScreenActionId,
			@RecordStatus,	
			@UserName,				
			@CurrentDate,	
			@UserName,				
			@CurrentDate
		)
	END

SELECT  @ScreenActionId = ScreenActionId FROM ScreenAction WHERE ActionName = 'EditPayerAssociationToClient'
SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'Executive'
IF NOT EXISTS ( SELECT TOP 1 1 FROM RoleAction WHERE RoleId = @RoleId AND ScreenActionId = @ScreenActionId)
	BEGIN
		INSERT INTO [dbo].[RoleAction]
           ([RoleId]
           ,[ScreenActionId]
           ,[RecordStatus]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[ModifiedBy]
           ,[ModifiedDate])
		VALUES
		(
			@RoleId, 			
			@ScreenActionId,
			@RecordStatus,	
			@UserName,				
			@CurrentDate,	
			@UserName,				
			@CurrentDate
		)
	END

SELECT  @ScreenActionId = ScreenActionId FROM ScreenAction WHERE ActionName = 'EditPayerAssociationToClient'
SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'User'
IF NOT EXISTS ( SELECT TOP 1 1 FROM RoleAction WHERE RoleId = @RoleId AND ScreenActionId = @ScreenActionId)
	BEGIN
		INSERT INTO [dbo].[RoleAction]
           ([RoleId]
           ,[ScreenActionId]
           ,[RecordStatus]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[ModifiedBy]
           ,[ModifiedDate])
		VALUES
		(
			@RoleId, 			
			@ScreenActionId,
			@RecordStatus,	
			@UserName,				
			@CurrentDate,	
			@UserName,				
			@CurrentDate
		)
	END

SELECT  @ScreenActionId = ScreenActionId FROM ScreenAction WHERE ActionName = 'PerformClientPayerActivateInactivate'
SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'Admin'
IF NOT EXISTS ( SELECT TOP 1 1 FROM RoleAction WHERE RoleId = @RoleId AND ScreenActionId = @ScreenActionId)
	BEGIN
		INSERT INTO [dbo].[RoleAction]
           ([RoleId]
           ,[ScreenActionId]
           ,[RecordStatus]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[ModifiedBy]
           ,[ModifiedDate])
		VALUES
		(
			@RoleId, 			
			@ScreenActionId,
			@RecordStatus,	
			@UserName,				
			@CurrentDate,	
			@UserName,				
			@CurrentDate
		)
	END

SELECT  @ScreenActionId = ScreenActionId FROM ScreenAction WHERE ActionName = 'PerformClientPayerActivateInactivate'
SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'Manager'
IF NOT EXISTS ( SELECT TOP 1 1 FROM RoleAction WHERE RoleId = @RoleId AND ScreenActionId = @ScreenActionId)
	BEGIN
		INSERT INTO [dbo].[RoleAction]
           ([RoleId]
           ,[ScreenActionId]
           ,[RecordStatus]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[ModifiedBy]
           ,[ModifiedDate])
		VALUES
		(
			@RoleId, 			
			@ScreenActionId,
			@RecordStatus,	
			@UserName,				
			@CurrentDate,	
			@UserName,				
			@CurrentDate
		)
	END

SELECT  @ScreenActionId = ScreenActionId FROM ScreenAction WHERE ActionName = 'PerformClientPayerActivateInactivate'
SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'Executive'
IF NOT EXISTS ( SELECT TOP 1 1 FROM RoleAction WHERE RoleId = @RoleId AND ScreenActionId = @ScreenActionId)
	BEGIN
		INSERT INTO [dbo].[RoleAction]
           ([RoleId]
           ,[ScreenActionId]
           ,[RecordStatus]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[ModifiedBy]
           ,[ModifiedDate])
		VALUES
		(
			@RoleId, 			
			@ScreenActionId,
			@RecordStatus,	
			@UserName,				
			@CurrentDate,	
			@UserName,				
			@CurrentDate
		)
	END

SELECT  @ScreenActionId = ScreenActionId FROM ScreenAction WHERE ActionName = 'PerformClientPayerActivateInactivate'
SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'User'
IF NOT EXISTS ( SELECT TOP 1 1 FROM RoleAction WHERE RoleId = @RoleId AND ScreenActionId = @ScreenActionId)
	BEGIN
		INSERT INTO [dbo].[RoleAction]
           ([RoleId]
           ,[ScreenActionId]
           ,[RecordStatus]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[ModifiedBy]
           ,[ModifiedDate])
		VALUES
		(
			@RoleId, 			
			@ScreenActionId,
			@RecordStatus,	
			@UserName,				
			@CurrentDate,	
			@UserName,				
			@CurrentDate
		)
	END

SELECT  @ScreenActionId = ScreenActionId FROM ScreenAction WHERE ActionName = 'AddUserAssociationToClient'
SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'Admin'
IF NOT EXISTS ( SELECT TOP 1 1 FROM RoleAction WHERE RoleId = @RoleId AND ScreenActionId = @ScreenActionId)
	BEGIN
		INSERT INTO [dbo].[RoleAction]
           ([RoleId]
           ,[ScreenActionId]
           ,[RecordStatus]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[ModifiedBy]
           ,[ModifiedDate])
		VALUES
		(
			@RoleId, 			
			@ScreenActionId,
			@RecordStatus,	
			@UserName,				
			@CurrentDate,	
			@UserName,				
			@CurrentDate
		)
	END

SELECT  @ScreenActionId = ScreenActionId FROM ScreenAction WHERE ActionName = 'AddUserAssociationToClient'
SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'Manager'
IF NOT EXISTS ( SELECT TOP 1 1 FROM RoleAction WHERE RoleId = @RoleId AND ScreenActionId = @ScreenActionId)
	BEGIN
		INSERT INTO [dbo].[RoleAction]
           ([RoleId]
           ,[ScreenActionId]
           ,[RecordStatus]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[ModifiedBy]
           ,[ModifiedDate])
		VALUES
		(
			@RoleId, 			
			@ScreenActionId,
			@RecordStatus,	
			@UserName,				
			@CurrentDate,	
			@UserName,				
			@CurrentDate
		)
	END

SELECT  @ScreenActionId = ScreenActionId FROM ScreenAction WHERE ActionName = 'AddUserAssociationToClient'
SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'Executive'
IF NOT EXISTS ( SELECT TOP 1 1 FROM RoleAction WHERE RoleId = @RoleId AND ScreenActionId = @ScreenActionId)
	BEGIN
		INSERT INTO [dbo].[RoleAction]
           ([RoleId]
           ,[ScreenActionId]
           ,[RecordStatus]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[ModifiedBy]
           ,[ModifiedDate])
		VALUES
		(
			@RoleId, 			
			@ScreenActionId,
			@RecordStatus,	
			@UserName,				
			@CurrentDate,	
			@UserName,				
			@CurrentDate
		)
	END

SELECT  @ScreenActionId = ScreenActionId FROM ScreenAction WHERE ActionName = 'EditUserAssociationToClient'
SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'Admin'
IF NOT EXISTS ( SELECT TOP 1 1 FROM RoleAction WHERE RoleId = @RoleId AND ScreenActionId = @ScreenActionId)
	BEGIN
		INSERT INTO [dbo].[RoleAction]
           ([RoleId]
           ,[ScreenActionId]
           ,[RecordStatus]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[ModifiedBy]
           ,[ModifiedDate])
		VALUES
		(
			@RoleId, 			
			@ScreenActionId,
			@RecordStatus,	
			@UserName,				
			@CurrentDate,	
			@UserName,				
			@CurrentDate
		)
	END

SELECT  @ScreenActionId = ScreenActionId FROM ScreenAction WHERE ActionName = 'EditUserAssociationToClient'
SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'Manager'
IF NOT EXISTS ( SELECT TOP 1 1 FROM RoleAction WHERE RoleId = @RoleId AND ScreenActionId = @ScreenActionId)
	BEGIN
		INSERT INTO [dbo].[RoleAction]
           ([RoleId]
           ,[ScreenActionId]
           ,[RecordStatus]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[ModifiedBy]
           ,[ModifiedDate])
		VALUES
		(
			@RoleId, 			
			@ScreenActionId,
			@RecordStatus,	
			@UserName,				
			@CurrentDate,	
			@UserName,				
			@CurrentDate
		)
	END

SELECT  @ScreenActionId = ScreenActionId FROM ScreenAction WHERE ActionName = 'EditUserAssociationToClient'
SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'Executive'
IF NOT EXISTS ( SELECT TOP 1 1 FROM RoleAction WHERE RoleId = @RoleId AND ScreenActionId = @ScreenActionId)
	BEGIN
		INSERT INTO [dbo].[RoleAction]
           ([RoleId]
           ,[ScreenActionId]
           ,[RecordStatus]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[ModifiedBy]
           ,[ModifiedDate])
		VALUES
		(
			@RoleId, 			
			@ScreenActionId,
			@RecordStatus,	
			@UserName,				
			@CurrentDate,	
			@UserName,				
			@CurrentDate
		)
	END

SELECT  @ScreenActionId = ScreenActionId FROM ScreenAction WHERE ActionName = 'AddMonthlyTargets'
SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'Admin'
IF NOT EXISTS ( SELECT TOP 1 1 FROM RoleAction WHERE RoleId = @RoleId AND ScreenActionId = @ScreenActionId)
	BEGIN
		INSERT INTO [dbo].[RoleAction]
           ([RoleId]
           ,[ScreenActionId]
           ,[RecordStatus]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[ModifiedBy]
           ,[ModifiedDate])
		VALUES
		(
			@RoleId, 			
			@ScreenActionId,
			@RecordStatus,	
			@UserName,				
			@CurrentDate,	
			@UserName,				
			@CurrentDate
		)
	END

SELECT  @ScreenActionId = ScreenActionId FROM ScreenAction WHERE ActionName = 'AddMonthlyTargets'
SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'Manager'
IF NOT EXISTS ( SELECT TOP 1 1 FROM RoleAction WHERE RoleId = @RoleId AND ScreenActionId = @ScreenActionId)
	BEGIN
		INSERT INTO [dbo].[RoleAction]
           ([RoleId]
           ,[ScreenActionId]
           ,[RecordStatus]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[ModifiedBy]
           ,[ModifiedDate])
		VALUES
		(
			@RoleId, 			
			@ScreenActionId,
			@RecordStatus,	
			@UserName,				
			@CurrentDate,	
			@UserName,				
			@CurrentDate
		)
	END

SELECT  @ScreenActionId = ScreenActionId FROM ScreenAction WHERE ActionName = 'AddMonthlyTargets'
SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'Executive'
IF NOT EXISTS ( SELECT TOP 1 1 FROM RoleAction WHERE RoleId = @RoleId AND ScreenActionId = @ScreenActionId)
	BEGIN
		INSERT INTO [dbo].[RoleAction]
           ([RoleId]
           ,[ScreenActionId]
           ,[RecordStatus]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[ModifiedBy]
           ,[ModifiedDate])
		VALUES
		(
			@RoleId, 			
			@ScreenActionId,
			@RecordStatus,	
			@UserName,				
			@CurrentDate,	
			@UserName,				
			@CurrentDate
		)
	END

SELECT  @ScreenActionId = ScreenActionId FROM ScreenAction WHERE ActionName = 'EditMonthlyTargets'
SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'Admin'
IF NOT EXISTS ( SELECT TOP 1 1 FROM RoleAction WHERE RoleId = @RoleId AND ScreenActionId = @ScreenActionId)
	BEGIN
		INSERT INTO [dbo].[RoleAction]
           ([RoleId]
           ,[ScreenActionId]
           ,[RecordStatus]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[ModifiedBy]
           ,[ModifiedDate])
		VALUES
		(
			@RoleId, 			
			@ScreenActionId,
			@RecordStatus,	
			@UserName,				
			@CurrentDate,	
			@UserName,				
			@CurrentDate
		)
	END

SELECT  @ScreenActionId = ScreenActionId FROM ScreenAction WHERE ActionName = 'EditMonthlyTargets'
SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'Manager'
IF NOT EXISTS ( SELECT TOP 1 1 FROM RoleAction WHERE RoleId = @RoleId AND ScreenActionId = @ScreenActionId)
	BEGIN
		INSERT INTO [dbo].[RoleAction]
           ([RoleId]
           ,[ScreenActionId]
           ,[RecordStatus]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[ModifiedBy]
           ,[ModifiedDate])
		VALUES
		(
			@RoleId, 			
			@ScreenActionId,
			@RecordStatus,	
			@UserName,				
			@CurrentDate,	
			@UserName,				
			@CurrentDate
		)
	END

SELECT  @ScreenActionId = ScreenActionId FROM ScreenAction WHERE ActionName = 'EditMonthlyTargets'
SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'Executive'
IF NOT EXISTS ( SELECT TOP 1 1 FROM RoleAction WHERE RoleId = @RoleId AND ScreenActionId = @ScreenActionId)
	BEGIN
		INSERT INTO [dbo].[RoleAction]
           ([RoleId]
           ,[ScreenActionId]
           ,[RecordStatus]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[ModifiedBy]
           ,[ModifiedDate])
		VALUES
		(
			@RoleId, 			
			@ScreenActionId,
			@RecordStatus,	
			@UserName,				
			@CurrentDate,	
			@UserName,				
			@CurrentDate
		)
	END

SELECT  @ScreenActionId = ScreenActionId FROM ScreenAction WHERE ActionName = 'AddKPI'
SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'Admin'
IF NOT EXISTS ( SELECT TOP 1 1 FROM RoleAction WHERE RoleId = @RoleId AND ScreenActionId = @ScreenActionId)
	BEGIN
		INSERT INTO [dbo].[RoleAction]
           ([RoleId]
           ,[ScreenActionId]
           ,[RecordStatus]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[ModifiedBy]
           ,[ModifiedDate])
		VALUES
		(
			@RoleId, 			
			@ScreenActionId,
			@RecordStatus,	
			@UserName,				
			@CurrentDate,	
			@UserName,				
			@CurrentDate
		)
	END

SELECT  @ScreenActionId = ScreenActionId FROM ScreenAction WHERE ActionName = 'AddKPI'
SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'Manager'
IF NOT EXISTS ( SELECT TOP 1 1 FROM RoleAction WHERE RoleId = @RoleId AND ScreenActionId = @ScreenActionId)
	BEGIN
		INSERT INTO [dbo].[RoleAction]
           ([RoleId]
           ,[ScreenActionId]
           ,[RecordStatus]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[ModifiedBy]
           ,[ModifiedDate])
		VALUES
		(
			@RoleId, 			
			@ScreenActionId,
			@RecordStatus,	
			@UserName,				
			@CurrentDate,	
			@UserName,				
			@CurrentDate
		)
	END

SELECT  @ScreenActionId = ScreenActionId FROM ScreenAction WHERE ActionName = 'AddKPI'
SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'Executive'
IF NOT EXISTS ( SELECT TOP 1 1 FROM RoleAction WHERE RoleId = @RoleId AND ScreenActionId = @ScreenActionId)
	BEGIN
		INSERT INTO [dbo].[RoleAction]
           ([RoleId]
           ,[ScreenActionId]
           ,[RecordStatus]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[ModifiedBy]
           ,[ModifiedDate])
		VALUES
		(
			@RoleId, 			
			@ScreenActionId,
			@RecordStatus,	
			@UserName,				
			@CurrentDate,	
			@UserName,				
			@CurrentDate
		)
	END

SELECT  @ScreenActionId = ScreenActionId FROM ScreenAction WHERE ActionName = 'EditKPI'
SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'Admin'
IF NOT EXISTS ( SELECT TOP 1 1 FROM RoleAction WHERE RoleId = @RoleId AND ScreenActionId = @ScreenActionId)
	BEGIN
		INSERT INTO [dbo].[RoleAction]
           ([RoleId]
           ,[ScreenActionId]
           ,[RecordStatus]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[ModifiedBy]
           ,[ModifiedDate])
		VALUES
		(
			@RoleId, 			
			@ScreenActionId,
			@RecordStatus,	
			@UserName,				
			@CurrentDate,	
			@UserName,				
			@CurrentDate
		)
	END

SELECT  @ScreenActionId = ScreenActionId FROM ScreenAction WHERE ActionName = 'EditKPI'
SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'Manager'
IF NOT EXISTS ( SELECT TOP 1 1 FROM RoleAction WHERE RoleId = @RoleId AND ScreenActionId = @ScreenActionId)
	BEGIN
		INSERT INTO [dbo].[RoleAction]
           ([RoleId]
           ,[ScreenActionId]
           ,[RecordStatus]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[ModifiedBy]
           ,[ModifiedDate])
		VALUES
		(
			@RoleId, 			
			@ScreenActionId,
			@RecordStatus,	
			@UserName,				
			@CurrentDate,	
			@UserName,				
			@CurrentDate
		)
	END

SELECT  @ScreenActionId = ScreenActionId FROM ScreenAction WHERE ActionName = 'EditKPI'
SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'Executive'
IF NOT EXISTS ( SELECT TOP 1 1 FROM RoleAction WHERE RoleId = @RoleId AND ScreenActionId = @ScreenActionId)
	BEGIN
		INSERT INTO [dbo].[RoleAction]
           ([RoleId]
           ,[ScreenActionId]
           ,[RecordStatus]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[ModifiedBy]
           ,[ModifiedDate])
		VALUES
		(
			@RoleId, 			
			@ScreenActionId,
			@RecordStatus,	
			@UserName,				
			@CurrentDate,	
			@UserName,				
			@CurrentDate
		)
	END

SELECT  @ScreenActionId = ScreenActionId FROM ScreenAction WHERE ActionName = 'AddEditDeleteHolidaysInBusinessDays'
SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'Admin'
IF NOT EXISTS ( SELECT TOP 1 1 FROM RoleAction WHERE RoleId = @RoleId AND ScreenActionId = @ScreenActionId)
	BEGIN
		INSERT INTO [dbo].[RoleAction]
           ([RoleId]
           ,[ScreenActionId]
           ,[RecordStatus]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[ModifiedBy]
           ,[ModifiedDate])
		VALUES
		(
			@RoleId, 			
			@ScreenActionId,
			@RecordStatus,	
			@UserName,				
			@CurrentDate,	
			@UserName,				
			@CurrentDate
		)
	END

SELECT  @ScreenActionId = ScreenActionId FROM ScreenAction WHERE ActionName = 'ViewGridGraphInBusinessDays'
SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'Admin'
IF NOT EXISTS ( SELECT TOP 1 1 FROM RoleAction WHERE RoleId = @RoleId AND ScreenActionId = @ScreenActionId)
	BEGIN
		INSERT INTO [dbo].[RoleAction]
           ([RoleId]
           ,[ScreenActionId]
           ,[RecordStatus]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[ModifiedBy]
           ,[ModifiedDate])
		VALUES
		(
			@RoleId, 			
			@ScreenActionId,
			@RecordStatus,	
			@UserName,				
			@CurrentDate,	
			@UserName,				
			@CurrentDate
		)
	END

SELECT  @ScreenActionId = ScreenActionId FROM ScreenAction WHERE ActionName = 'ViewGridGraphInBusinessDays'
SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'Executive'
IF NOT EXISTS ( SELECT TOP 1 1 FROM RoleAction WHERE RoleId = @RoleId AND ScreenActionId = @ScreenActionId)
	BEGIN
		INSERT INTO [dbo].[RoleAction]
           ([RoleId]
           ,[ScreenActionId]
           ,[RecordStatus]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[ModifiedBy]
           ,[ModifiedDate])
		VALUES
		(
			@RoleId, 			
			@ScreenActionId,
			@RecordStatus,	
			@UserName,				
			@CurrentDate,	
			@UserName,				
			@CurrentDate
		)
	END

SELECT  @ScreenActionId = ScreenActionId FROM ScreenAction WHERE ActionName = 'AddBusinessUnit'
SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'Admin'
IF NOT EXISTS ( SELECT TOP 1 1 FROM RoleAction WHERE RoleId = @RoleId AND ScreenActionId = @ScreenActionId)
	BEGIN
		INSERT INTO [dbo].[RoleAction]
           ([RoleId]
           ,[ScreenActionId]
           ,[RecordStatus]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[ModifiedBy]
           ,[ModifiedDate])
		VALUES
		(
			@RoleId, 			
			@ScreenActionId,
			@RecordStatus,	
			@UserName,				
			@CurrentDate,	
			@UserName,				
			@CurrentDate
		)
	END

SELECT  @ScreenActionId = ScreenActionId FROM ScreenAction WHERE ActionName = 'EditBusinessUnit'
SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'Admin'
IF NOT EXISTS ( SELECT TOP 1 1 FROM RoleAction WHERE RoleId = @RoleId AND ScreenActionId = @ScreenActionId)
	BEGIN
		INSERT INTO [dbo].[RoleAction]
           ([RoleId]
           ,[ScreenActionId]
           ,[RecordStatus]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[ModifiedBy]
           ,[ModifiedDate])
		VALUES
		(
			@RoleId, 			
			@ScreenActionId,
			@RecordStatus,	
			@UserName,				
			@CurrentDate,	
			@UserName,				
			@CurrentDate
		)
	END

SELECT  @ScreenActionId = ScreenActionId FROM ScreenAction WHERE ActionName = 'AddPayer'
SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'Admin'
IF NOT EXISTS ( SELECT TOP 1 1 FROM RoleAction WHERE RoleId = @RoleId AND ScreenActionId = @ScreenActionId)
	BEGIN
		INSERT INTO [dbo].[RoleAction]
           ([RoleId]
           ,[ScreenActionId]
           ,[RecordStatus]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[ModifiedBy]
           ,[ModifiedDate])
		VALUES
		(
			@RoleId, 			
			@ScreenActionId,
			@RecordStatus,	
			@UserName,				
			@CurrentDate,	
			@UserName,				
			@CurrentDate
		)
	END

SELECT  @ScreenActionId = ScreenActionId FROM ScreenAction WHERE ActionName = 'AddPayer'
SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'Manager'
IF NOT EXISTS ( SELECT TOP 1 1 FROM RoleAction WHERE RoleId = @RoleId AND ScreenActionId = @ScreenActionId)
	BEGIN
		INSERT INTO [dbo].[RoleAction]
           ([RoleId]
           ,[ScreenActionId]
           ,[RecordStatus]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[ModifiedBy]
           ,[ModifiedDate])
		VALUES
		(
			@RoleId, 			
			@ScreenActionId,
			@RecordStatus,	
			@UserName,				
			@CurrentDate,	
			@UserName,				
			@CurrentDate
		)
	END

SELECT  @ScreenActionId = ScreenActionId FROM ScreenAction WHERE ActionName = 'AddPayer'
SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'Executive'
IF NOT EXISTS ( SELECT TOP 1 1 FROM RoleAction WHERE RoleId = @RoleId AND ScreenActionId = @ScreenActionId)
	BEGIN
		INSERT INTO [dbo].[RoleAction]
           ([RoleId]
           ,[ScreenActionId]
           ,[RecordStatus]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[ModifiedBy]
           ,[ModifiedDate])
		VALUES
		(
			@RoleId, 			
			@ScreenActionId,
			@RecordStatus,	
			@UserName,				
			@CurrentDate,	
			@UserName,				
			@CurrentDate
		)
	END

SELECT  @ScreenActionId = ScreenActionId FROM ScreenAction WHERE ActionName = 'AddPayer'
SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'User'
IF NOT EXISTS ( SELECT TOP 1 1 FROM RoleAction WHERE RoleId = @RoleId AND ScreenActionId = @ScreenActionId)
	BEGIN
		INSERT INTO [dbo].[RoleAction]
           ([RoleId]
           ,[ScreenActionId]
           ,[RecordStatus]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[ModifiedBy]
           ,[ModifiedDate])
		VALUES
		(
			@RoleId, 			
			@ScreenActionId,
			@RecordStatus,	
			@UserName,				
			@CurrentDate,	
			@UserName,				
			@CurrentDate
		)
	END

SELECT  @ScreenActionId = ScreenActionId FROM ScreenAction WHERE ActionName = 'EditPayerName'
SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'Admin'
IF NOT EXISTS ( SELECT TOP 1 1 FROM RoleAction WHERE RoleId = @RoleId AND ScreenActionId = @ScreenActionId)
	BEGIN
		INSERT INTO [dbo].[RoleAction]
           ([RoleId]
           ,[ScreenActionId]
           ,[RecordStatus]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[ModifiedBy]
           ,[ModifiedDate])
		VALUES
		(
			@RoleId, 			
			@ScreenActionId,
			@RecordStatus,	
			@UserName,				
			@CurrentDate,	
			@UserName,				
			@CurrentDate
		)
	END

SELECT  @ScreenActionId = ScreenActionId FROM ScreenAction WHERE ActionName = 'EditPayerName'
SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'Manager'
IF NOT EXISTS ( SELECT TOP 1 1 FROM RoleAction WHERE RoleId = @RoleId AND ScreenActionId = @ScreenActionId)
	BEGIN
		INSERT INTO [dbo].[RoleAction]
           ([RoleId]
           ,[ScreenActionId]
           ,[RecordStatus]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[ModifiedBy]
           ,[ModifiedDate])
		VALUES
		(
			@RoleId, 			
			@ScreenActionId,
			@RecordStatus,	
			@UserName,				
			@CurrentDate,	
			@UserName,				
			@CurrentDate
		)
	END

SELECT  @ScreenActionId = ScreenActionId FROM ScreenAction WHERE ActionName = 'EditPayerName'
SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'Executive'
IF NOT EXISTS ( SELECT TOP 1 1 FROM RoleAction WHERE RoleId = @RoleId AND ScreenActionId = @ScreenActionId)
	BEGIN
		INSERT INTO [dbo].[RoleAction]
           ([RoleId]
           ,[ScreenActionId]
           ,[RecordStatus]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[ModifiedBy]
           ,[ModifiedDate])
		VALUES
		(
			@RoleId, 			
			@ScreenActionId,
			@RecordStatus,	
			@UserName,				
			@CurrentDate,	
			@UserName,				
			@CurrentDate
		)
	END

SELECT  @ScreenActionId = ScreenActionId FROM ScreenAction WHERE ActionName = 'MakePayerActiveInActive'
SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'Admin'
IF NOT EXISTS ( SELECT TOP 1 1 FROM RoleAction WHERE RoleId = @RoleId AND ScreenActionId = @ScreenActionId)
	BEGIN
		INSERT INTO [dbo].[RoleAction]
           ([RoleId]
           ,[ScreenActionId]
           ,[RecordStatus]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[ModifiedBy]
           ,[ModifiedDate])
		VALUES
		(
			@RoleId, 			
			@ScreenActionId,
			@RecordStatus,	
			@UserName,				
			@CurrentDate,	
			@UserName,				
			@CurrentDate
		)
	END

SELECT  @ScreenActionId = ScreenActionId FROM ScreenAction WHERE ActionName = 'MakePayerActiveInActive'
SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'Manager'
IF NOT EXISTS ( SELECT TOP 1 1 FROM RoleAction WHERE RoleId = @RoleId AND ScreenActionId = @ScreenActionId)
	BEGIN
		INSERT INTO [dbo].[RoleAction]
           ([RoleId]
           ,[ScreenActionId]
           ,[RecordStatus]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[ModifiedBy]
           ,[ModifiedDate])
		VALUES
		(
			@RoleId, 			
			@ScreenActionId,
			@RecordStatus,	
			@UserName,				
			@CurrentDate,	
			@UserName,				
			@CurrentDate
		)
	END

SELECT  @ScreenActionId = ScreenActionId FROM ScreenAction WHERE ActionName = 'MakePayerActiveInActive'
SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'Executive'
IF NOT EXISTS ( SELECT TOP 1 1 FROM RoleAction WHERE RoleId = @RoleId AND ScreenActionId = @ScreenActionId)
	BEGIN
		INSERT INTO [dbo].[RoleAction]
           ([RoleId]
           ,[ScreenActionId]
           ,[RecordStatus]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[ModifiedBy]
           ,[ModifiedDate])
		VALUES
		(
			@RoleId, 			
			@ScreenActionId,
			@RecordStatus,	
			@UserName,				
			@CurrentDate,	
			@UserName,				
			@CurrentDate
		)
	END

SELECT  @ScreenActionId = ScreenActionId FROM ScreenAction WHERE ActionName = 'EditClient'
SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'Admin'
IF NOT EXISTS ( SELECT TOP 1 1 FROM RoleAction WHERE RoleId = @RoleId AND ScreenActionId = @ScreenActionId)
	BEGIN
		INSERT INTO [dbo].[RoleAction]
           ([RoleId]
           ,[ScreenActionId]
           ,[RecordStatus]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[ModifiedBy]
           ,[ModifiedDate])
		VALUES
		(
			@RoleId, 			
			@ScreenActionId,
			@RecordStatus,	
			@UserName,				
			@CurrentDate,	
			@UserName,				
			@CurrentDate
		)
	END

SELECT  @ScreenActionId = ScreenActionId FROM ScreenAction WHERE ActionName = 'EditClient'
SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'Manager'
IF NOT EXISTS ( SELECT TOP 1 1 FROM RoleAction WHERE RoleId = @RoleId AND ScreenActionId = @ScreenActionId)
	BEGIN
		INSERT INTO [dbo].[RoleAction]
           ([RoleId]
           ,[ScreenActionId]
           ,[RecordStatus]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[ModifiedBy]
           ,[ModifiedDate])
		VALUES
		(
			@RoleId, 			
			@ScreenActionId,
			@RecordStatus,	
			@UserName,				
			@CurrentDate,	
			@UserName,				
			@CurrentDate
		)
	END

SELECT  @ScreenActionId = ScreenActionId FROM ScreenAction WHERE ActionName = 'EditClient'
SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'Executive'
IF NOT EXISTS ( SELECT TOP 1 1 FROM RoleAction WHERE RoleId = @RoleId AND ScreenActionId = @ScreenActionId)
	BEGIN
		INSERT INTO [dbo].[RoleAction]
           ([RoleId]
           ,[ScreenActionId]
           ,[RecordStatus]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[ModifiedBy]
           ,[ModifiedDate])
		VALUES
		(
			@RoleId, 			
			@ScreenActionId,
			@RecordStatus,	
			@UserName,				
			@CurrentDate,	
			@UserName,				
			@CurrentDate
		)
	END

SELECT  @ScreenActionId = ScreenActionId FROM ScreenAction WHERE ActionName = 'EditClient'
SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'User'
IF NOT EXISTS ( SELECT TOP 1 1 FROM RoleAction WHERE RoleId = @RoleId AND ScreenActionId = @ScreenActionId)
	BEGIN
		INSERT INTO [dbo].[RoleAction]
           ([RoleId]
           ,[ScreenActionId]
           ,[RecordStatus]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[ModifiedBy]
           ,[ModifiedDate])
		VALUES
		(
			@RoleId, 			
			@ScreenActionId,
			@RecordStatus,	
			@UserName,				
			@CurrentDate,	
			@UserName,				
			@CurrentDate
		)
	END

SELECT  @ScreenActionId = ScreenActionId FROM ScreenAction WHERE ActionName = 'AddSystem'
SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'Admin'
IF NOT EXISTS ( SELECT TOP 1 1 FROM RoleAction WHERE RoleId = @RoleId AND ScreenActionId = @ScreenActionId)
	BEGIN
		INSERT INTO [dbo].[RoleAction]
           ([RoleId]
           ,[ScreenActionId]
           ,[RecordStatus]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[ModifiedBy]
           ,[ModifiedDate])
		VALUES
		(
			@RoleId, 			
			@ScreenActionId,
			@RecordStatus,	
			@UserName,				
			@CurrentDate,	
			@UserName,				
			@CurrentDate
		)
	END

SELECT  @ScreenActionId = ScreenActionId FROM ScreenAction WHERE ActionName = 'EditSystem'
SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'Admin'
IF NOT EXISTS ( SELECT TOP 1 1 FROM RoleAction WHERE RoleId = @RoleId AND ScreenActionId = @ScreenActionId)
	BEGIN
		INSERT INTO [dbo].[RoleAction]
           ([RoleId]
           ,[ScreenActionId]
           ,[RecordStatus]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[ModifiedBy]
           ,[ModifiedDate])
		VALUES
		(
			@RoleId, 			
			@ScreenActionId,
			@RecordStatus,	
			@UserName,				
			@CurrentDate,	
			@UserName,				
			@CurrentDate
		)
	END

SELECT  @ScreenActionId = ScreenActionId FROM ScreenAction WHERE ActionName = 'EditChecklist'
SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'Admin'
IF NOT EXISTS ( SELECT TOP 1 1 FROM RoleAction WHERE RoleId = @RoleId AND ScreenActionId = @ScreenActionId)
	BEGIN
		INSERT INTO [dbo].[RoleAction]
           ([RoleId]
           ,[ScreenActionId]
           ,[RecordStatus]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[ModifiedBy]
           ,[ModifiedDate])
		VALUES
		(
			@RoleId, 			
			@ScreenActionId,
			@RecordStatus,	
			@UserName,				
			@CurrentDate,	
			@UserName,				
			@CurrentDate
		)
	END

SELECT  @ScreenActionId = ScreenActionId FROM ScreenAction WHERE ActionName = 'EditChecklist'
SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'Executive'
IF NOT EXISTS ( SELECT TOP 1 1 FROM RoleAction WHERE RoleId = @RoleId AND ScreenActionId = @ScreenActionId)
	BEGIN
		INSERT INTO [dbo].[RoleAction]
           ([RoleId]
           ,[ScreenActionId]
           ,[RecordStatus]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[ModifiedBy]
           ,[ModifiedDate])
		VALUES
		(
			@RoleId, 			
			@ScreenActionId,
			@RecordStatus,	
			@UserName,				
			@CurrentDate,	
			@UserName,				
			@CurrentDate
		)
	END

SELECT @RoleId = RoleId FROM Roles WHERE RoleCode = 'Admin'
SELECT @ScreenActionId = ScreenActionId FROM ScreenAction WHERE ActionName = 'Reopen checklist'
IF NOT EXISTS (SELECT TOP 1 1 FROM RoleAction WHERE ScreenActionId = @ScreenActionID AND RoleId = @RoleId)
BEGIN
	INSERT INTO [dbo].[RoleAction]
		([RoleId]
		,[ScreenActionId]
		,[RecordStatus]
		,[CreatedBy]
		,[CreatedDate]
		,[ModifiedBy]
		,[ModifiedDate])
	VALUES
	(
		@RoleId,
		@ScreenActionId,
		@RecordStatus,
		@UserName,
		@CurrentDate,
		@UserName,
		@CurrentDate
	)
END

SELECT @RoleId = RoleId FROM Roles WHERE RoleCode = 'Admin'
SELECT @ScreenActionId = ScreenActionId FROM ScreenAction WHERE ActionName = 'Add KPI'
IF NOT EXISTS (SELECT TOP 1 1 FROM RoleAction WHERE ScreenActionId = @ScreenActionID AND RoleId = @RoleId)
BEGIN
	INSERT INTO [dbo].[RoleAction]
		([RoleId]
		,[ScreenActionId]
		,[RecordStatus]
		,[CreatedBy]
		,[CreatedDate]
		,[ModifiedBy]
		,[ModifiedDate])
	VALUES
	(
		@RoleId,
		@ScreenActionId,
		@RecordStatus,
		@UserName,
		@CurrentDate,
		@UserName,
		@CurrentDate
	)
END

SELECT @RoleId = RoleId FROM Roles WHERE RoleCode = 'Admin'
SELECT @ScreenActionId = ScreenActionId FROM ScreenAction WHERE ActionName = 'Edit KPI'
IF NOT EXISTS (SELECT TOP 1 1 FROM RoleAction WHERE ScreenActionId = @ScreenActionID AND RoleId = @RoleId)
BEGIN
	INSERT INTO [dbo].[RoleAction]
		([RoleId]
		,[ScreenActionId]
		,[RecordStatus]
		,[CreatedBy]
		,[CreatedDate]
		,[ModifiedBy]
		,[ModifiedDate])
	VALUES
	(
		@RoleId,
		@ScreenActionId,
		@RecordStatus,
		@UserName,
		@CurrentDate,
		@UserName,
		@CurrentDate
	)
END

SELECT @RoleId = RoleId FROM Roles WHERE RoleCode = 'Executive'
SELECT @ScreenActionId = ScreenActionId FROM ScreenAction WHERE ActionName = 'Add KPI'
IF NOT EXISTS (SELECT TOP 1 1 FROM RoleAction WHERE ScreenActionId = @ScreenActionID AND RoleId = @RoleId)
BEGIN
	INSERT INTO [dbo].[RoleAction]
		([RoleId]
		,[ScreenActionId]
		,[RecordStatus]
		,[CreatedBy]
		,[CreatedDate]
		,[ModifiedBy]
		,[ModifiedDate])
	VALUES
	(
		@RoleId,
		@ScreenActionId,
		@RecordStatus,
		@UserName,
		@CurrentDate,
		@UserName,
		@CurrentDate
	)
END

SELECT @RoleId = RoleId FROM Roles WHERE RoleCode = 'Executive'
SELECT @ScreenActionId = ScreenActionId FROM ScreenAction WHERE ActionName = 'Edit KPI'
IF NOT EXISTS (SELECT TOP 1 1 FROM RoleAction WHERE ScreenActionId = @ScreenActionID AND RoleId = @RoleId)
BEGIN
	INSERT INTO [dbo].[RoleAction]
		([RoleId]
		,[ScreenActionId]
		,[RecordStatus]
		,[CreatedBy]
		,[CreatedDate]
		,[ModifiedBy]
		,[ModifiedDate])
	VALUES
	(
		@RoleId,
		@ScreenActionId,
		@RecordStatus,
		@UserName,
		@CurrentDate,
		@UserName,
		@CurrentDate
	)
END

SELECT @RoleId = RoleId FROM Roles WHERE RoleCode = 'Admin'
SELECT @ScreenActionId = ScreenActionId FROM ScreenAction WHERE ActionName = 'SaveLastEnteredBusinessDaysOrWeeks'
IF NOT EXISTS (SELECT TOP 1 1 FROM RoleAction WHERE ScreenActionId = @ScreenActionID AND RoleId = @RoleId)
BEGIN
	INSERT INTO [dbo].[RoleAction]
		([RoleId]
		,[ScreenActionId]
		,[RecordStatus]
		,[CreatedBy]
		,[CreatedDate]
		,[ModifiedBy]
		,[ModifiedDate])
	VALUES
	(
		@RoleId,
		@ScreenActionId,
		@RecordStatus,
		@UserName,
		@CurrentDate,
		@UserName,
		@CurrentDate
	)
END

SELECT @RoleId = RoleId FROM Roles WHERE RoleCode = 'Admin'
SELECT @ScreenActionId = ScreenActionId FROM ScreenAction WHERE ActionName = 'EditAdminConfig'
IF NOT EXISTS (SELECT TOP 1 1 FROM RoleAction WHERE ScreenActionId = @ScreenActionID AND RoleId = @RoleId)
BEGIN
	INSERT INTO [dbo].[RoleAction]
		([RoleId]
		,[ScreenActionId]
		,[RecordStatus]
		,[CreatedBy]
		,[CreatedDate]
		,[ModifiedBy]
		,[ModifiedDate])
	VALUES
	(
		@RoleId,
		@ScreenActionId,
		@RecordStatus,
		@UserName,
		@CurrentDate,
		@UserName,
		@CurrentDate
	)
END

SELECT @RoleId = RoleId FROM Roles WHERE RoleCode = 'Admin'
SELECT @ScreenActionId = ScreenActionId FROM ScreenAction WHERE ActionName = 'ViewActualM3Revenue'
IF NOT EXISTS (SELECT TOP 1 1 FROM RoleAction WHERE ScreenActionId = @ScreenActionID AND RoleId = @RoleId)
BEGIN
	INSERT INTO [dbo].[RoleAction]
		([RoleId]
		,[ScreenActionId]
		,[RecordStatus]
		,[CreatedBy]
		,[CreatedDate]
		,[ModifiedBy]
		,[ModifiedDate])
	VALUES
	(
		@RoleId,
		@ScreenActionId,
		@RecordStatus,
		@UserName,
		@CurrentDate,
		@UserName,
		@CurrentDate
	)
END

SELECT @RoleId = RoleId FROM Roles WHERE RoleCode = 'Executive'
SELECT @ScreenActionId = ScreenActionId FROM ScreenAction WHERE ActionName = 'ViewActualM3Revenue'
IF NOT EXISTS (SELECT TOP 1 1 FROM RoleAction WHERE ScreenActionId = @ScreenActionID AND RoleId = @RoleId)
BEGIN
	INSERT INTO [dbo].[RoleAction]
		([RoleId]
		,[ScreenActionId]
		,[RecordStatus]
		,[CreatedBy]
		,[CreatedDate]
		,[ModifiedBy]
		,[ModifiedDate])
	VALUES
	(
		@RoleId,
		@ScreenActionId,
		@RecordStatus,
		@UserName,
		@CurrentDate,
		@UserName,
		@CurrentDate
	)
END

SELECT @RoleId = RoleId FROM Roles WHERE RoleCode = 'Admin'
SELECT @ScreenActionId = ScreenActionId FROM ScreenAction WHERE ActionName = 'ViewForecastedM3Revenue'
IF NOT EXISTS (SELECT TOP 1 1 FROM RoleAction WHERE ScreenActionId = @ScreenActionID AND RoleId = @RoleId)
BEGIN
	INSERT INTO [dbo].[RoleAction]
		([RoleId]
		,[ScreenActionId]
		,[RecordStatus]
		,[CreatedBy]
		,[CreatedDate]
		,[ModifiedBy]
		,[ModifiedDate])
	VALUES
	(
		@RoleId,
		@ScreenActionId,
		@RecordStatus,
		@UserName,
		@CurrentDate,
		@UserName,
		@CurrentDate
	)
END

SELECT @RoleId = RoleId FROM Roles WHERE RoleCode = 'Executive'
SELECT @ScreenActionId = ScreenActionId FROM ScreenAction WHERE ActionName = 'ViewForecastedM3Revenue'
IF NOT EXISTS (SELECT TOP 1 1 FROM RoleAction WHERE ScreenActionId = @ScreenActionID AND RoleId = @RoleId)
BEGIN
	INSERT INTO [dbo].[RoleAction]
		([RoleId]
		,[ScreenActionId]
		,[RecordStatus]
		,[CreatedBy]
		,[CreatedDate]
		,[ModifiedBy]
		,[ModifiedDate])
	VALUES
	(
		@RoleId,
		@ScreenActionId,
		@RecordStatus,
		@UserName,
		@CurrentDate,
		@UserName,
		@CurrentDate
	)
END

-- To-do-list role based actions.

SELECT @RoleId = RoleId FROM Roles WHERE RoleCode = 'Admin'
SELECT @ScreenActionId = ScreenActionId FROM ScreenAction WHERE ActionName = 'Assign Relationship Manager'
IF NOT EXISTS (SELECT TOP 1 1 FROM RoleAction WHERE ScreenActionId = @ScreenActionID AND RoleId = @RoleId)
BEGIN
	INSERT INTO [dbo].[RoleAction]
		([RoleId]
		,[ScreenActionId]
		,[RecordStatus]
		,[CreatedBy]
		,[CreatedDate]
		,[ModifiedBy]
		,[ModifiedDate])
	VALUES
	(
		@RoleId,
		@ScreenActionId,
		@RecordStatus,
		@UserName,
		@CurrentDate,
		@UserName,
		@CurrentDate
	)
END

SELECT @RoleId = RoleId FROM Roles WHERE RoleCode = 'Executive'
SELECT @ScreenActionId = ScreenActionId FROM ScreenAction WHERE ActionName = 'Assign Relationship Manager'
IF NOT EXISTS (SELECT TOP 1 1 FROM RoleAction WHERE ScreenActionId = @ScreenActionID AND RoleId = @RoleId)
BEGIN
	INSERT INTO [dbo].[RoleAction]
		([RoleId]
		,[ScreenActionId]
		,[RecordStatus]
		,[CreatedBy]
		,[CreatedDate]
		,[ModifiedBy]
		,[ModifiedDate])
	VALUES
	(
		@RoleId,
		@ScreenActionId,
		@RecordStatus,
		@UserName,
		@CurrentDate,
		@UserName,
		@CurrentDate
	)
END

SELECT @RoleId = RoleId FROM Roles WHERE RoleCode = 'Manager'
SELECT @ScreenActionId = ScreenActionId FROM ScreenAction WHERE ActionName = 'Assign Relationship Manager'
IF NOT EXISTS (SELECT TOP 1 1 FROM RoleAction WHERE ScreenActionId = @ScreenActionID AND RoleId = @RoleId)
BEGIN
	INSERT INTO [dbo].[RoleAction]
		([RoleId]
		,[ScreenActionId]
		,[RecordStatus]
		,[CreatedBy]
		,[CreatedDate]
		,[ModifiedBy]
		,[ModifiedDate])
	VALUES
	(
		@RoleId,
		@ScreenActionId,
		@RecordStatus,
		@UserName,
		@CurrentDate,
		@UserName,
		@CurrentDate
	)
END

SELECT @RoleId = RoleId FROM Roles WHERE RoleCode = 'Admin'
SELECT @ScreenActionId = ScreenActionId FROM ScreenAction WHERE ActionName = 'Assign Billing Manager'
IF NOT EXISTS (SELECT TOP 1 1 FROM RoleAction WHERE ScreenActionId = @ScreenActionID AND RoleId = @RoleId)
BEGIN
	INSERT INTO [dbo].[RoleAction]
		([RoleId]
		,[ScreenActionId]
		,[RecordStatus]
		,[CreatedBy]
		,[CreatedDate]
		,[ModifiedBy]
		,[ModifiedDate])
	VALUES
	(
		@RoleId,
		@ScreenActionId,
		@RecordStatus,
		@UserName,
		@CurrentDate,
		@UserName,
		@CurrentDate
	)
END

SELECT @RoleId = RoleId FROM Roles WHERE RoleCode = 'Executive'
SELECT @ScreenActionId = ScreenActionId FROM ScreenAction WHERE ActionName = 'Assign Billing Manager'
IF NOT EXISTS (SELECT TOP 1 1 FROM RoleAction WHERE ScreenActionId = @ScreenActionID AND RoleId = @RoleId)
BEGIN
	INSERT INTO [dbo].[RoleAction]
		([RoleId]
		,[ScreenActionId]
		,[RecordStatus]
		,[CreatedBy]
		,[CreatedDate]
		,[ModifiedBy]
		,[ModifiedDate])
	VALUES
	(
		@RoleId,
		@ScreenActionId,
		@RecordStatus,
		@UserName,
		@CurrentDate,
		@UserName,
		@CurrentDate
	)
END

SELECT @RoleId = RoleId FROM Roles WHERE RoleCode = 'Manager'
SELECT @ScreenActionId = ScreenActionId FROM ScreenAction WHERE ActionName = 'Assign Billing Manager'
IF NOT EXISTS (SELECT TOP 1 1 FROM RoleAction WHERE ScreenActionId = @ScreenActionID AND RoleId = @RoleId)
BEGIN
	INSERT INTO [dbo].[RoleAction]
		([RoleId]
		,[ScreenActionId]
		,[RecordStatus]
		,[CreatedBy]
		,[CreatedDate]
		,[ModifiedBy]
		,[ModifiedDate])
	VALUES
	(
		@RoleId,
		@ScreenActionId,
		@RecordStatus,
		@UserName,
		@CurrentDate,
		@UserName,
		@CurrentDate
	)
END

SELECT @RoleId = RoleId FROM Roles WHERE RoleCode = 'Admin'
SELECT @ScreenActionId = ScreenActionId FROM ScreenAction WHERE ActionName = 'Pending Weekly Checklist'
IF NOT EXISTS (SELECT TOP 1 1 FROM RoleAction WHERE ScreenActionId = @ScreenActionID AND RoleId = @RoleId)
BEGIN
	INSERT INTO [dbo].[RoleAction]
		([RoleId]
		,[ScreenActionId]
		,[RecordStatus]
		,[CreatedBy]
		,[CreatedDate]
		,[ModifiedBy]
		,[ModifiedDate])
	VALUES
	(
		@RoleId,
		@ScreenActionId,
		@RecordStatus,
		@UserName,
		@CurrentDate,
		@UserName,
		@CurrentDate
	)
END

SELECT @RoleId = RoleId FROM Roles WHERE RoleCode = 'Executive'
SELECT @ScreenActionId = ScreenActionId FROM ScreenAction WHERE ActionName = 'Pending Weekly Checklist'
IF NOT EXISTS (SELECT TOP 1 1 FROM RoleAction WHERE ScreenActionId = @ScreenActionID AND RoleId = @RoleId)
BEGIN
	INSERT INTO [dbo].[RoleAction]
		([RoleId]
		,[ScreenActionId]
		,[RecordStatus]
		,[CreatedBy]
		,[CreatedDate]
		,[ModifiedBy]
		,[ModifiedDate])
	VALUES
	(
		@RoleId,
		@ScreenActionId,
		@RecordStatus,
		@UserName,
		@CurrentDate,
		@UserName,
		@CurrentDate
	)
END

SELECT @RoleId = RoleId FROM Roles WHERE RoleCode = 'Manager'
SELECT @ScreenActionId = ScreenActionId FROM ScreenAction WHERE ActionName = 'Pending Weekly Checklist'
IF NOT EXISTS (SELECT TOP 1 1 FROM RoleAction WHERE ScreenActionId = @ScreenActionID AND RoleId = @RoleId)
BEGIN
	INSERT INTO [dbo].[RoleAction]
		([RoleId]
		,[ScreenActionId]
		,[RecordStatus]
		,[CreatedBy]
		,[CreatedDate]
		,[ModifiedBy]
		,[ModifiedDate])
	VALUES
	(
		@RoleId,
		@ScreenActionId,
		@RecordStatus,
		@UserName,
		@CurrentDate,
		@UserName,
		@CurrentDate
	)
END

SELECT @RoleId = RoleId FROM Roles WHERE RoleCode = 'User'
SELECT @ScreenActionId = ScreenActionId FROM ScreenAction WHERE ActionName = 'Pending Weekly Checklist'
IF NOT EXISTS (SELECT TOP 1 1 FROM RoleAction WHERE ScreenActionId = @ScreenActionID AND RoleId = @RoleId)
BEGIN
	INSERT INTO [dbo].[RoleAction]
		([RoleId]
		,[ScreenActionId]
		,[RecordStatus]
		,[CreatedBy]
		,[CreatedDate]
		,[ModifiedBy]
		,[ModifiedDate])
	VALUES
	(
		@RoleId,
		@ScreenActionId,
		@RecordStatus,
		@UserName,
		@CurrentDate,
		@UserName,
		@CurrentDate
	)
END

SELECT @RoleId = RoleId FROM Roles WHERE RoleCode = 'Admin'
SELECT @ScreenActionId = ScreenActionId FROM ScreenAction WHERE ActionName = 'Pending Monthly Checklist'
IF NOT EXISTS (SELECT TOP 1 1 FROM RoleAction WHERE ScreenActionId = @ScreenActionID AND RoleId = @RoleId)
BEGIN
	INSERT INTO [dbo].[RoleAction]
		([RoleId]
		,[ScreenActionId]
		,[RecordStatus]
		,[CreatedBy]
		,[CreatedDate]
		,[ModifiedBy]
		,[ModifiedDate])
	VALUES
	(
		@RoleId,
		@ScreenActionId,
		@RecordStatus,
		@UserName,
		@CurrentDate,
		@UserName,
		@CurrentDate
	)
END

SELECT @RoleId = RoleId FROM Roles WHERE RoleCode = 'Executive'
SELECT @ScreenActionId = ScreenActionId FROM ScreenAction WHERE ActionName = 'Pending Monthly Checklist'
IF NOT EXISTS (SELECT TOP 1 1 FROM RoleAction WHERE ScreenActionId = @ScreenActionID AND RoleId = @RoleId)
BEGIN
	INSERT INTO [dbo].[RoleAction]
		([RoleId]
		,[ScreenActionId]
		,[RecordStatus]
		,[CreatedBy]
		,[CreatedDate]
		,[ModifiedBy]
		,[ModifiedDate])
	VALUES
	(
		@RoleId,
		@ScreenActionId,
		@RecordStatus,
		@UserName,
		@CurrentDate,
		@UserName,
		@CurrentDate
	)
END

SELECT @RoleId = RoleId FROM Roles WHERE RoleCode = 'Manager'
SELECT @ScreenActionId = ScreenActionId FROM ScreenAction WHERE ActionName = 'Pending Monthly Checklist'
IF NOT EXISTS (SELECT TOP 1 1 FROM RoleAction WHERE ScreenActionId = @ScreenActionID AND RoleId = @RoleId)
BEGIN
	INSERT INTO [dbo].[RoleAction]
		([RoleId]
		,[ScreenActionId]
		,[RecordStatus]
		,[CreatedBy]
		,[CreatedDate]
		,[ModifiedBy]
		,[ModifiedDate])
	VALUES
	(
		@RoleId,
		@ScreenActionId,
		@RecordStatus,
		@UserName,
		@CurrentDate,
		@UserName,
		@CurrentDate
	)
END

SELECT @RoleId = RoleId FROM Roles WHERE RoleCode = 'User'
SELECT @ScreenActionId = ScreenActionId FROM ScreenAction WHERE ActionName = 'Pending Monthly Checklist'
IF NOT EXISTS (SELECT TOP 1 1 FROM RoleAction WHERE ScreenActionId = @ScreenActionID AND RoleId = @RoleId)
BEGIN
	INSERT INTO [dbo].[RoleAction]
		([RoleId]
		,[ScreenActionId]
		,[RecordStatus]
		,[CreatedBy]
		,[CreatedDate]
		,[ModifiedBy]
		,[ModifiedDate])
	VALUES
	(
		@RoleId,
		@ScreenActionId,
		@RecordStatus,
		@UserName,
		@CurrentDate,
		@UserName,
		@CurrentDate
	)
END

SELECT @RoleId = RoleId FROM Roles WHERE RoleCode = 'Admin'
SELECT @ScreenActionId = ScreenActionId FROM ScreenAction WHERE ActionName = 'Partially Completed Clients'
IF NOT EXISTS (SELECT TOP 1 1 FROM RoleAction WHERE ScreenActionId = @ScreenActionID AND RoleId = @RoleId)
BEGIN
	INSERT INTO [dbo].[RoleAction]
		([RoleId]
		,[ScreenActionId]
		,[RecordStatus]
		,[CreatedBy]
		,[CreatedDate]
		,[ModifiedBy]
		,[ModifiedDate])
	VALUES
	(
		@RoleId,
		@ScreenActionId,
		@RecordStatus,
		@UserName,
		@CurrentDate,
		@UserName,
		@CurrentDate
	)
END

SELECT @RoleId = RoleId FROM Roles WHERE RoleCode = 'Admin'
SELECT @ScreenActionId = ScreenActionId FROM ScreenAction WHERE ActionName = 'Set Targets'
IF NOT EXISTS (SELECT TOP 1 1 FROM RoleAction WHERE ScreenActionId = @ScreenActionID AND RoleId = @RoleId)
BEGIN
	INSERT INTO [dbo].[RoleAction]
		([RoleId]
		,[ScreenActionId]
		,[RecordStatus]
		,[CreatedBy]
		,[CreatedDate]
		,[ModifiedBy]
		,[ModifiedDate])
	VALUES
	(
		@RoleId,
		@ScreenActionId,
		@RecordStatus,
		@UserName,
		@CurrentDate,
		@UserName,
		@CurrentDate
	)
END

SELECT @RoleId = RoleId FROM Roles WHERE RoleCode = 'Executive'
SELECT @ScreenActionId = ScreenActionId FROM ScreenAction WHERE ActionName = 'Set Targets'
IF NOT EXISTS (SELECT TOP 1 1 FROM RoleAction WHERE ScreenActionId = @ScreenActionID AND RoleId = @RoleId)
BEGIN
	INSERT INTO [dbo].[RoleAction]
		([RoleId]
		,[ScreenActionId]
		,[RecordStatus]
		,[CreatedBy]
		,[CreatedDate]
		,[ModifiedBy]
		,[ModifiedDate])
	VALUES
	(
		@RoleId,
		@ScreenActionId,
		@RecordStatus,
		@UserName,
		@CurrentDate,
		@UserName,
		@CurrentDate
	)
END

SELECT @RoleId = RoleId FROM Roles WHERE RoleCode = 'Manager'
SELECT @ScreenActionId = ScreenActionId FROM ScreenAction WHERE ActionName = 'Set Targets'
IF NOT EXISTS (SELECT TOP 1 1 FROM RoleAction WHERE ScreenActionId = @ScreenActionID AND RoleId = @RoleId)
BEGIN
	INSERT INTO [dbo].[RoleAction]
		([RoleId]
		,[ScreenActionId]
		,[RecordStatus]
		,[CreatedBy]
		,[CreatedDate]
		,[ModifiedBy]
		,[ModifiedDate])
	VALUES
	(
		@RoleId,
		@ScreenActionId,
		@RecordStatus,
		@UserName,
		@CurrentDate,
		@UserName,
		@CurrentDate
	)
END

SELECT @RoleId = RoleId FROM Roles WHERE RoleCode = 'Admin'
SELECT @ScreenActionId = ScreenActionId FROM ScreenAction WHERE ActionName = 'Set Holidays'
IF NOT EXISTS (SELECT TOP 1 1 FROM RoleAction WHERE ScreenActionId = @ScreenActionID AND RoleId = @RoleId)
BEGIN
	INSERT INTO [dbo].[RoleAction]
		([RoleId]
		,[ScreenActionId]
		,[RecordStatus]
		,[CreatedBy]
		,[CreatedDate]
		,[ModifiedBy]
		,[ModifiedDate])
	VALUES
	(
		@RoleId,
		@ScreenActionId,
		@RecordStatus,
		@UserName,
		@CurrentDate,
		@UserName,
		@CurrentDate
	)
END

SELECT @RoleId = RoleId FROM Roles WHERE RoleCode = 'Admin'
SELECT @ScreenActionId = ScreenActionId FROM ScreenAction WHERE ActionName = 'Unclosed Months'
IF NOT EXISTS (SELECT TOP 1 1 FROM RoleAction WHERE ScreenActionId = @ScreenActionID AND RoleId = @RoleId)
BEGIN
	INSERT INTO [dbo].[RoleAction]
		([RoleId]
		,[ScreenActionId]
		,[RecordStatus]
		,[CreatedBy]
		,[CreatedDate]
		,[ModifiedBy]
		,[ModifiedDate])
	VALUES
	(
		@RoleId,
		@ScreenActionId,
		@RecordStatus,
		@UserName,
		@CurrentDate,
		@UserName,
		@CurrentDate
	)
END

SELECT @RoleId = RoleId FROM Roles WHERE RoleCode = 'Manager'
SELECT @ScreenActionId = ScreenActionId FROM ScreenAction WHERE ActionName = 'Unclosed Months'
IF NOT EXISTS (SELECT TOP 1 1 FROM RoleAction WHERE ScreenActionId = @ScreenActionID AND RoleId = @RoleId)
BEGIN
	INSERT INTO [dbo].[RoleAction]
		([RoleId]
		,[ScreenActionId]
		,[RecordStatus]
		,[CreatedBy]
		,[CreatedDate]
		,[ModifiedBy]
		,[ModifiedDate])
	VALUES
	(
		@RoleId,
		@ScreenActionId,
		@RecordStatus,
		@UserName,
		@CurrentDate,
		@UserName,
		@CurrentDate
	)
END

SELECT @RoleId = RoleId FROM Roles WHERE RoleCode = 'User'
SELECT @ScreenActionId = ScreenActionId FROM ScreenAction WHERE ActionName = 'Unclosed Months'
IF NOT EXISTS (SELECT TOP 1 1 FROM RoleAction WHERE ScreenActionId = @ScreenActionID AND RoleId = @RoleId)
BEGIN
	INSERT INTO [dbo].[RoleAction]
		([RoleId]
		,[ScreenActionId]
		,[RecordStatus]
		,[CreatedBy]
		,[CreatedDate]
		,[ModifiedBy]
		,[ModifiedDate])
	VALUES
	(
		@RoleId,
		@ScreenActionId,
		@RecordStatus,
		@UserName,
		@CurrentDate,
		@UserName,
		@CurrentDate
	)
END

SELECT @RoleId = RoleId FROM Roles WHERE RoleCode = 'Admin'
SELECT @ScreenActionId = ScreenActionId FROM ScreenAction WHERE ActionName = 'Incomplete Deposit Log'
IF NOT EXISTS (SELECT TOP 1 1 FROM RoleAction WHERE ScreenActionId = @ScreenActionID AND RoleId = @RoleId)
BEGIN
	INSERT INTO [dbo].[RoleAction]
		([RoleId]
		,[ScreenActionId]
		,[RecordStatus]
		,[CreatedBy]
		,[CreatedDate]
		,[ModifiedBy]
		,[ModifiedDate])
	VALUES
	(
		@RoleId,
		@ScreenActionId,
		@RecordStatus,
		@UserName,
		@CurrentDate,
		@UserName,
		@CurrentDate
	)
END

SELECT @RoleId = RoleId FROM Roles WHERE RoleCode = 'Manager'
SELECT @ScreenActionId = ScreenActionId FROM ScreenAction WHERE ActionName = 'Incomplete Deposit Log'
IF NOT EXISTS (SELECT TOP 1 1 FROM RoleAction WHERE ScreenActionId = @ScreenActionID AND RoleId = @RoleId)
BEGIN
	INSERT INTO [dbo].[RoleAction]
		([RoleId]
		,[ScreenActionId]
		,[RecordStatus]
		,[CreatedBy]
		,[CreatedDate]
		,[ModifiedBy]
		,[ModifiedDate])
	VALUES
	(
		@RoleId,
		@ScreenActionId,
		@RecordStatus,
		@UserName,
		@CurrentDate,
		@UserName,
		@CurrentDate
	)
END

SELECT @RoleId = RoleId FROM Roles WHERE RoleCode = 'User'
SELECT @ScreenActionId = ScreenActionId FROM ScreenAction WHERE ActionName = 'Incomplete Deposit Log'
IF NOT EXISTS (SELECT TOP 1 1 FROM RoleAction WHERE ScreenActionId = @ScreenActionID AND RoleId = @RoleId)
BEGIN
	INSERT INTO [dbo].[RoleAction]
		([RoleId]
		,[ScreenActionId]
		,[RecordStatus]
		,[CreatedBy]
		,[CreatedDate]
		,[ModifiedBy]
		,[ModifiedDate])
	VALUES
	(
		@RoleId,
		@ScreenActionId,
		@RecordStatus,
		@UserName,
		@CurrentDate,
		@UserName,
		@CurrentDate
	)
END

SELECT @RoleId = RoleId FROM Roles WHERE RoleCode = 'Admin'
SELECT @ScreenActionId = ScreenActionId FROM ScreenAction WHERE ActionName = 'ViewClientHistory'
IF NOT EXISTS (SELECT TOP 1 1 FROM RoleAction WHERE ScreenActionId = @ScreenActionID AND RoleId = @RoleId)
BEGIN
	INSERT INTO [dbo].[RoleAction]
		([RoleId]
		,[ScreenActionId]
		,[RecordStatus]
		,[CreatedBy]
		,[CreatedDate]
		,[ModifiedBy]
		,[ModifiedDate])
	VALUES
	(
		@RoleId,
		@ScreenActionId,
		@RecordStatus,
		@UserName,
		@CurrentDate,
		@UserName,
		@CurrentDate
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