/*
Script Name : 12_ScreenAction
Module_Name : M3Pact
Created By  : Abhishek K
CreatedDate : 05/10/2018

Data Verification Script :

SELECT * FROM [ScreenAction](NOLOCK) WHERE RecordStatus = 'A'


Revision History 
=============
Ver  Date				Who						Comment
1.0  30-July-2018		Abhishek Kovvuri		Inserting View Actions for All Clients page.
1.1  08-Aug-2018        Abhishek Kovvuri        Inserting actions for to-do-list page.
*/

BEGIN TRY
BEGIN TRANSACTION

DECLARE @UserName NVARCHAR(60) = 'Admin'
DECLARE @CurrentDate DATETIME = GETDATE()
DECLARE @RecordStatus CHAR(1) = 'A'
DECLARE @ScreenId INT;

SELECT  @ScreenId = ScreenId FROM Screen WHERE ScreenCode = 'DL'
IF NOT EXISTS ( SELECT TOP 1 1 FROM ScreenAction WHERE ActionName = 'ViewCloseMonthButton')
	BEGIN
		INSERT INTO [dbo].[ScreenAction]
           ([ScreenId]
           ,[ActionName]
           ,[RecordStatus]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[ModifiedBy]
           ,[ModifiedDate])
		VALUES
		( 
		    @ScreenId,
			'ViewCloseMonthButton',	  --    ScreenId
			@RecordStatus,	          --	RecordStatus
			@UserName,				  --	CreatedBy
			@CurrentDate,			  --	CreatedDate
			@UserName,				  --	ModifiedBy
			@CurrentDate			  --	ModifiedDate
		)
	END

SELECT  @ScreenId = ScreenId FROM Screen WHERE ScreenCode = 'DL'
IF NOT EXISTS ( SELECT TOP 1 1 FROM ScreenAction WHERE ActionName = 'ViewAddDepositSection')
	BEGIN
		INSERT INTO [dbo].[ScreenAction]
           ([ScreenId]
           ,[ActionName]
           ,[RecordStatus]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[ModifiedBy]
           ,[ModifiedDate])
		VALUES
		( 
		    @ScreenId,
			'ViewAddDepositSection',	     --  ScreenId
			@RecordStatus,	                 --	 RecordStatus
			@UserName,				         --	 CreatedBy
			@CurrentDate,			         --	 CreatedDate
			@UserName,				         --	 ModifiedBy
			@CurrentDate			         --	 ModifiedDate
		)
	END

SELECT  @ScreenId = ScreenId FROM Screen WHERE ScreenCode = 'DL'
IF NOT EXISTS ( SELECT TOP 1 1 FROM ScreenAction WHERE ActionName = 'OpenClosedMonth')
	BEGIN
		INSERT INTO [dbo].[ScreenAction]
           ([ScreenId]
           ,[ActionName]
           ,[RecordStatus]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[ModifiedBy]
           ,[ModifiedDate])
		VALUES
		( 
		    @ScreenId,
			'OpenClosedMonth',	    
			@RecordStatus,	        
			@UserName,				
			@CurrentDate,			
			@UserName,				
			@CurrentDate			
		)
	END

SELECT  @ScreenId = ScreenId FROM Screen WHERE ScreenCode = 'SITES'
IF NOT EXISTS ( SELECT TOP 1 1 FROM ScreenAction WHERE ActionName = 'AddSite')
	BEGIN
		INSERT INTO [dbo].[ScreenAction]
           ([ScreenId]
           ,[ActionName]
           ,[RecordStatus]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[ModifiedBy]
           ,[ModifiedDate])
		VALUES
		( 
		    @ScreenId,
			'AddSite',	    
			@RecordStatus,	        
			@UserName,				
			@CurrentDate,			
			@UserName,				
			@CurrentDate			
		)
	END

SELECT  @ScreenId = ScreenId FROM Screen WHERE ScreenCode = 'SITES'
IF NOT EXISTS ( SELECT TOP 1 1 FROM ScreenAction WHERE ActionName = 'EditSite')
	BEGIN
		INSERT INTO [dbo].[ScreenAction]
           ([ScreenId]
           ,[ActionName]
           ,[RecordStatus]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[ModifiedBy]
           ,[ModifiedDate])
		VALUES
		( 
		    @ScreenId,
			'EditSite',	    
			@RecordStatus,	        
			@UserName,				
			@CurrentDate,			
			@UserName,				
			@CurrentDate			
		)
	END

SELECT  @ScreenId = ScreenId FROM Screen WHERE ScreenCode = 'SPEC'
IF NOT EXISTS ( SELECT TOP 1 1 FROM ScreenAction WHERE ActionName = 'AddSpecialty')
	BEGIN
		INSERT INTO [dbo].[ScreenAction]
           ([ScreenId]
           ,[ActionName]
           ,[RecordStatus]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[ModifiedBy]
           ,[ModifiedDate])
		VALUES
		( 
		    @ScreenId,
			'AddSpecialty',	    
			@RecordStatus,	        
			@UserName,				
			@CurrentDate,			
			@UserName,				
			@CurrentDate			
		)
	END

SELECT  @ScreenId = ScreenId FROM Screen WHERE ScreenCode = 'SPEC'
IF NOT EXISTS ( SELECT TOP 1 1 FROM ScreenAction WHERE ActionName = 'EditSpecialty')
	BEGIN
		INSERT INTO [dbo].[ScreenAction]
           ([ScreenId]
           ,[ActionName]
           ,[RecordStatus]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[ModifiedBy]
           ,[ModifiedDate])
		VALUES
		( 
		    @ScreenId,
			'EditSpecialty',	    
			@RecordStatus,	        
			@UserName,				
			@CurrentDate,			
			@UserName,				
			@CurrentDate			
		)
	END

SELECT  @ScreenId = ScreenId FROM Screen WHERE ScreenCode = 'CD'
IF NOT EXISTS ( SELECT TOP 1 1 FROM ScreenAction WHERE ActionName = 'PerformClientActivateInactivate')
	BEGIN
		INSERT INTO [dbo].[ScreenAction]
           ([ScreenId]
           ,[ActionName]
           ,[RecordStatus]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[ModifiedBy]
           ,[ModifiedDate])
		VALUES
		( 
		    @ScreenId,
			'PerformClientActivateInactivate',	    
			@RecordStatus,	        
			@UserName,				
			@CurrentDate,			
			@UserName,				
			@CurrentDate			
		)
	END

SELECT  @ScreenId = ScreenId FROM Screen WHERE ScreenCode = 'CD'
IF NOT EXISTS ( SELECT TOP 1 1 FROM ScreenAction WHERE ActionName = 'EditClientData')
	BEGIN
		INSERT INTO [dbo].[ScreenAction]
           ([ScreenId]
           ,[ActionName]
           ,[RecordStatus]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[ModifiedBy]
           ,[ModifiedDate])
		VALUES
		( 
		    @ScreenId,
			'EditClientData',	    
			@RecordStatus,	        
			@UserName,				
			@CurrentDate,			
			@UserName,				
			@CurrentDate			
		)
	END

SELECT  @ScreenId = ScreenId FROM Screen WHERE ScreenCode = 'CD'
IF NOT EXISTS ( SELECT TOP 1 1 FROM ScreenAction WHERE ActionName = 'AddClientData')
	BEGIN
		INSERT INTO [dbo].[ScreenAction]
           ([ScreenId]
           ,[ActionName]
           ,[RecordStatus]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[ModifiedBy]
           ,[ModifiedDate])
		VALUES
		( 
		    @ScreenId,
			'AddClientData',	    
			@RecordStatus,	        
			@UserName,				
			@CurrentDate,			
			@UserName,				
			@CurrentDate			
		)
	END

SELECT  @ScreenId = ScreenId FROM Screen WHERE ScreenCode = 'LS'
IF NOT EXISTS ( SELECT TOP 1 1 FROM ScreenAction WHERE ActionName = 'AddPayerAssociationToClient')
	BEGIN
		INSERT INTO [dbo].[ScreenAction]
           ([ScreenId]
           ,[ActionName]
           ,[RecordStatus]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[ModifiedBy]
           ,[ModifiedDate])
		VALUES
		( 
		    @ScreenId,
			'AddPayerAssociationToClient',	    
			@RecordStatus,	        
			@UserName,				
			@CurrentDate,			
			@UserName,				
			@CurrentDate			
		)
	END

SELECT  @ScreenId = ScreenId FROM Screen WHERE ScreenCode = 'LS'
IF NOT EXISTS ( SELECT TOP 1 1 FROM ScreenAction WHERE ActionName = 'EditPayerAssociationToClient')
	BEGIN
		INSERT INTO [dbo].[ScreenAction]
           ([ScreenId]
           ,[ActionName]
           ,[RecordStatus]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[ModifiedBy]
           ,[ModifiedDate])
		VALUES
		( 
		    @ScreenId,
			'EditPayerAssociationToClient',	    
			@RecordStatus,	        
			@UserName,				
			@CurrentDate,			
			@UserName,				
			@CurrentDate			
		)
	END

SELECT  @ScreenId = ScreenId FROM Screen WHERE ScreenCode = 'LS'
IF NOT EXISTS ( SELECT TOP 1 1 FROM ScreenAction WHERE ActionName = 'PerformClientPayerActivateInactivate')
	BEGIN
		INSERT INTO [dbo].[ScreenAction]
           ([ScreenId]
           ,[ActionName]
           ,[RecordStatus]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[ModifiedBy]
           ,[ModifiedDate])
		VALUES
		( 
		    @ScreenId,
			'PerformClientPayerActivateInactivate',	    
			@RecordStatus,	        
			@UserName,				
			@CurrentDate,			
			@UserName,				
			@CurrentDate			
		)
	END

SELECT  @ScreenId = ScreenId FROM Screen WHERE ScreenCode = 'CAU'
IF NOT EXISTS ( SELECT TOP 1 1 FROM ScreenAction WHERE ActionName = 'AddUserAssociationToClient')
	BEGIN
		INSERT INTO [dbo].[ScreenAction]
           ([ScreenId]
           ,[ActionName]
           ,[RecordStatus]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[ModifiedBy]
           ,[ModifiedDate])
		VALUES
		( 
		    @ScreenId,
			'AddUserAssociationToClient',	    
			@RecordStatus,	        
			@UserName,				
			@CurrentDate,			
			@UserName,				
			@CurrentDate			
		)
	END

SELECT  @ScreenId = ScreenId FROM Screen WHERE ScreenCode = 'CAU'
IF NOT EXISTS ( SELECT TOP 1 1 FROM ScreenAction WHERE ActionName = 'EditUserAssociationToClient')
	BEGIN
		INSERT INTO [dbo].[ScreenAction]
           ([ScreenId]
           ,[ActionName]
           ,[RecordStatus]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[ModifiedBy]
           ,[ModifiedDate])
		VALUES
		( 
		    @ScreenId,
			'EditUserAssociationToClient',	    
			@RecordStatus,	        
			@UserName,				
			@CurrentDate,			
			@UserName,				
			@CurrentDate			
		)
	END

SELECT  @ScreenId = ScreenId FROM Screen WHERE ScreenCode = 'MT'
IF NOT EXISTS ( SELECT TOP 1 1 FROM ScreenAction WHERE ActionName = 'AddMonthlyTargets')
	BEGIN
		INSERT INTO [dbo].[ScreenAction]
           ([ScreenId]
           ,[ActionName]
           ,[RecordStatus]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[ModifiedBy]
           ,[ModifiedDate])
		VALUES
		( 
		    @ScreenId,
			'AddMonthlyTargets',	    
			@RecordStatus,	        
			@UserName,				
			@CurrentDate,			
			@UserName,				
			@CurrentDate			
		)
	END

SELECT  @ScreenId = ScreenId FROM Screen WHERE ScreenCode = 'MT'
IF NOT EXISTS ( SELECT TOP 1 1 FROM ScreenAction WHERE ActionName = 'EditMonthlyTargets')
	BEGIN
		INSERT INTO [dbo].[ScreenAction]
           ([ScreenId]
           ,[ActionName]
           ,[RecordStatus]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[ModifiedBy]
           ,[ModifiedDate])
		VALUES
		( 
		    @ScreenId,
			'EditMonthlyTargets',	    
			@RecordStatus,	        
			@UserName,				
			@CurrentDate,			
			@UserName,				
			@CurrentDate			
		)
	END

SELECT  @ScreenId = ScreenId FROM Screen WHERE ScreenCode = 'KPIS'
IF NOT EXISTS ( SELECT TOP 1 1 FROM ScreenAction WHERE ActionName = 'AddKPI')
	BEGIN
		INSERT INTO [dbo].[ScreenAction]
           ([ScreenId]
           ,[ActionName]
           ,[RecordStatus]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[ModifiedBy]
           ,[ModifiedDate])
		VALUES
		( 
		    @ScreenId,
			'AddKPI',	    
			@RecordStatus,	        
			@UserName,				
			@CurrentDate,			
			@UserName,				
			@CurrentDate			
		)
	END

SELECT  @ScreenId = ScreenId FROM Screen WHERE ScreenCode = 'KPIS'
IF NOT EXISTS ( SELECT TOP 1 1 FROM ScreenAction WHERE ActionName = 'EditKPI')
	BEGIN
		INSERT INTO [dbo].[ScreenAction]
           ([ScreenId]
           ,[ActionName]
           ,[RecordStatus]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[ModifiedBy]
           ,[ModifiedDate])
		VALUES
		( 
		    @ScreenId,
			'EditKPI',	    
			@RecordStatus,	        
			@UserName,				
			@CurrentDate,			
			@UserName,				
			@CurrentDate			
		)
	END

SELECT  @ScreenId = ScreenId FROM Screen WHERE ScreenCode = 'BD'
IF NOT EXISTS ( SELECT TOP 1 1 FROM ScreenAction WHERE ActionName = 'AddEditDeleteHolidaysInBusinessDays')
	BEGIN
		INSERT INTO [dbo].[ScreenAction]
           ([ScreenId]
           ,[ActionName]
           ,[RecordStatus]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[ModifiedBy]
           ,[ModifiedDate])
		VALUES
		( 
		    @ScreenId,
			'AddEditDeleteHolidaysInBusinessDays',	    
			@RecordStatus,	        
			@UserName,				
			@CurrentDate,			
			@UserName,				
			@CurrentDate			
		)
	END

SELECT  @ScreenId = ScreenId FROM Screen WHERE ScreenCode = 'BD'
IF NOT EXISTS ( SELECT TOP 1 1 FROM ScreenAction WHERE ActionName = 'ViewGridGraphInBusinessDays')
	BEGIN
		INSERT INTO [dbo].[ScreenAction]
           ([ScreenId]
           ,[ActionName]
           ,[RecordStatus]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[ModifiedBy]
           ,[ModifiedDate])
		VALUES
		( 
		    @ScreenId,
			'ViewGridGraphInBusinessDays',	    
			@RecordStatus,	        
			@UserName,				
			@CurrentDate,			
			@UserName,				
			@CurrentDate			
		)
	END

SELECT  @ScreenId = ScreenId FROM Screen WHERE ScreenCode = 'BU'
IF NOT EXISTS ( SELECT TOP 1 1 FROM ScreenAction WHERE ActionName = 'AddBusinessUnit')
	BEGIN
		INSERT INTO [dbo].[ScreenAction]
           ([ScreenId]
           ,[ActionName]
           ,[RecordStatus]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[ModifiedBy]
           ,[ModifiedDate])
		VALUES
		( 
		    @ScreenId,
			'AddBusinessUnit',	    
			@RecordStatus,	        
			@UserName,				
			@CurrentDate,			
			@UserName,				
			@CurrentDate			
		)
	END

SELECT  @ScreenId = ScreenId FROM Screen WHERE ScreenCode = 'BU'
IF NOT EXISTS ( SELECT TOP 1 1 FROM ScreenAction WHERE ActionName = 'EditBusinessUnit')
	BEGIN
		INSERT INTO [dbo].[ScreenAction]
           ([ScreenId]
           ,[ActionName]
           ,[RecordStatus]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[ModifiedBy]
           ,[ModifiedDate])
		VALUES
		( 
		    @ScreenId,
			'EditBusinessUnit',	    
			@RecordStatus,	        
			@UserName,				
			@CurrentDate,			
			@UserName,				
			@CurrentDate			
		)
	END

SELECT  @ScreenId = ScreenId FROM Screen WHERE ScreenCode = 'PAY'
IF NOT EXISTS ( SELECT TOP 1 1 FROM ScreenAction WHERE ActionName = 'AddPayer')
	BEGIN
		INSERT INTO [dbo].[ScreenAction]
           ([ScreenId]
           ,[ActionName]
           ,[RecordStatus]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[ModifiedBy]
           ,[ModifiedDate])
		VALUES
		( 
		    @ScreenId,
			'AddPayer',	    
			@RecordStatus,	        
			@UserName,				
			@CurrentDate,			
			@UserName,				
			@CurrentDate			
		)
	END

SELECT  @ScreenId = ScreenId FROM Screen WHERE ScreenCode = 'PAY'
IF NOT EXISTS ( SELECT TOP 1 1 FROM ScreenAction WHERE ActionName = 'EditPayerName')
	BEGIN
		INSERT INTO [dbo].[ScreenAction]
           ([ScreenId]
           ,[ActionName]
           ,[RecordStatus]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[ModifiedBy]
           ,[ModifiedDate])
		VALUES
		( 
		    @ScreenId,
			'EditPayerName',	    
			@RecordStatus,	        
			@UserName,				
			@CurrentDate,			
			@UserName,				
			@CurrentDate			
		)
	END

SELECT  @ScreenId = ScreenId FROM Screen WHERE ScreenCode = 'PAY'
IF NOT EXISTS ( SELECT TOP 1 1 FROM ScreenAction WHERE ActionName = 'MakePayerActiveInActive')
	BEGIN
		INSERT INTO [dbo].[ScreenAction]
           ([ScreenId]
           ,[ActionName]
           ,[RecordStatus]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[ModifiedBy]
           ,[ModifiedDate])
		VALUES
		( 
		    @ScreenId,
			'MakePayerActiveInActive',	    
			@RecordStatus,	        
			@UserName,				
			@CurrentDate,			
			@UserName,				
			@CurrentDate			
		)
	END


SELECT  @ScreenId = ScreenId FROM Screen WHERE ScreenCode = 'VA'
IF NOT EXISTS ( SELECT TOP 1 1 FROM ScreenAction WHERE ActionName = 'EditClient')
	BEGIN
		INSERT INTO [dbo].[ScreenAction]
           ([ScreenId]
           ,[ActionName]
           ,[RecordStatus]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[ModifiedBy]
           ,[ModifiedDate])
		VALUES
		( 
		    @ScreenId,
			'EditClient',	    
			@RecordStatus,	        
			@UserName,				
			@CurrentDate,			
			@UserName,				
			@CurrentDate			
		)
	END

SELECT  @ScreenId = ScreenId FROM Screen WHERE ScreenCode = 'SYS'
IF NOT EXISTS ( SELECT TOP 1 1 FROM ScreenAction WHERE ActionName = 'AddSystem')
	BEGIN
		INSERT INTO [dbo].[ScreenAction]
           ([ScreenId]
           ,[ActionName]
           ,[RecordStatus]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[ModifiedBy]
           ,[ModifiedDate])
		VALUES
		( 
		    @ScreenId,
			'AddSystem',	    
			@RecordStatus,	        
			@UserName,				
			@CurrentDate,			
			@UserName,				
			@CurrentDate			
		)
	END

SELECT  @ScreenId = ScreenId FROM Screen WHERE ScreenCode = 'SYS'
IF NOT EXISTS ( SELECT TOP 1 1 FROM ScreenAction WHERE ActionName = 'EditSystem')
	BEGIN
		INSERT INTO [dbo].[ScreenAction]
           ([ScreenId]
           ,[ActionName]
           ,[RecordStatus]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[ModifiedBy]
           ,[ModifiedDate])
		VALUES
		( 
		    @ScreenId,
			'EditSystem',	    
			@RecordStatus,	        
			@UserName,				
			@CurrentDate,			
			@UserName,				
			@CurrentDate			
		)
	END

SELECT  @ScreenId = ScreenId FROM Screen WHERE ScreenCode = 'VACL'
IF NOT EXISTS ( SELECT TOP 1 1 FROM ScreenAction WHERE ActionName = 'EditChecklist')
	BEGIN
		INSERT INTO [dbo].[ScreenAction]
           ([ScreenId]
           ,[ActionName]
           ,[RecordStatus]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[ModifiedBy]
           ,[ModifiedDate])
		VALUES
		( 
		    @ScreenId,
			'EditChecklist',	    
			@RecordStatus,	        
			@UserName,				
			@CurrentDate,			
			@UserName,				
			@CurrentDate			
		)
	END

SELECT @ScreenId = ScreenId FROM Screen WHERE ScreenCode = 'CCL'
IF NOT EXISTS (SELECT TOP 1 1 FROM ScreenAction WHERE ScreenId = @ScreenId AND ActionName = 'Reopen checklist')
BEGIN
	INSERT INTO [dbo].[ScreenAction]
        ([ScreenId]
        ,[ActionName]
        ,[RecordStatus]
        ,[CreatedBy]
        ,[CreatedDate]
        ,[ModifiedBy]
        ,[ModifiedDate])
	VALUES
	(
		@ScreenId,
		'Reopen checklist',
		@RecordStatus,
		@UserName,
		@CurrentDate,
		@UserName,
		@CurrentDate
	)
END

SELECT @ScreenId = ScreenId FROM Screen WHERE ScreenCode = 'VEK'
IF NOT EXISTS (SELECT TOP 1 1 FROM ScreenAction WHERE ScreenId = @ScreenId AND ActionName = 'Add KPI')
BEGIN
	INSERT INTO [dbo].[ScreenAction]
        ([ScreenId]
        ,[ActionName]
        ,[RecordStatus]
        ,[CreatedBy]
        ,[CreatedDate]
        ,[ModifiedBy]
        ,[ModifiedDate])
	VALUES
	(
		@ScreenId,
		'Add KPI',
		@RecordStatus,
		@UserName,
		@CurrentDate,
		@UserName,
		@CurrentDate
	)
END


SELECT @ScreenId = ScreenId FROM Screen WHERE ScreenCode = 'VEK'
IF NOT EXISTS (SELECT TOP 1 1 FROM ScreenAction WHERE ScreenId = @ScreenId AND ActionName = 'Edit KPI')
BEGIN
	INSERT INTO [dbo].[ScreenAction]
        ([ScreenId]
        ,[ActionName]
        ,[RecordStatus]
        ,[CreatedBy]
        ,[CreatedDate]
        ,[ModifiedBy]
        ,[ModifiedDate])
	VALUES
	(
		@ScreenId,
		'Edit KPI',
		@RecordStatus,
		@UserName,
		@CurrentDate,
		@UserName,
		@CurrentDate
	)
END

SELECT @ScreenId = ScreenId FROM Screen WHERE ScreenCode = 'DL'
IF NOT EXISTS (SELECT TOP 1 1 FROM ScreenAction WHERE ScreenId = @ScreenId AND ActionName = 'SaveLastEnteredBusinessDaysOrWeeks')
BEGIN
	INSERT INTO [dbo].[ScreenAction]
        ([ScreenId]
        ,[ActionName]
        ,[RecordStatus]
        ,[CreatedBy]
        ,[CreatedDate]
        ,[ModifiedBy]
        ,[ModifiedDate])
	VALUES
	(
		@ScreenId,
		'SaveLastEnteredBusinessDaysOrWeeks',
		@RecordStatus,
		@UserName,
		@CurrentDate,
		@UserName,
		@CurrentDate
	)
END

SELECT @ScreenId = ScreenId FROM Screen WHERE ScreenCode = 'DL'
IF NOT EXISTS (SELECT TOP 1 1 FROM ScreenAction WHERE ScreenId = @ScreenId AND ActionName = 'SaveLastEnteredBusinessDaysOrWeeks')
BEGIN
	INSERT INTO [dbo].[ScreenAction]
        ([ScreenId]
        ,[ActionName]
        ,[RecordStatus]
        ,[CreatedBy]
        ,[CreatedDate]
        ,[ModifiedBy]
        ,[ModifiedDate])
	VALUES
	(
		@ScreenId,
		'SaveLastEnteredBusinessDaysOrWeeks',
		@RecordStatus,
		@UserName,
		@CurrentDate,
		@UserName,
		@CurrentDate
	)
END

SELECT @ScreenId = ScreenId FROM Screen WHERE ScreenCode = 'CON'
IF NOT EXISTS (SELECT TOP 1 1 FROM ScreenAction WHERE ScreenId = @ScreenId AND ActionName = 'EditAdminConfig')
BEGIN
	INSERT INTO [dbo].[ScreenAction]
        ([ScreenId]
        ,[ActionName]
        ,[RecordStatus]
        ,[CreatedBy]
        ,[CreatedDate]
        ,[ModifiedBy]
        ,[ModifiedDate])
	VALUES
	(
		@ScreenId,
		'EditAdminConfig',
		@RecordStatus,
		@UserName,
		@CurrentDate,
		@UserName,
		@CurrentDate
	)
END

SELECT @ScreenId = ScreenId FROM Screen WHERE ScreenCode = 'VA'
IF NOT EXISTS (SELECT TOP 1 1 FROM ScreenAction WHERE ScreenId = @ScreenId AND ActionName = 'ViewActualM3Revenue')
BEGIN
	INSERT INTO [dbo].[ScreenAction]
        ([ScreenId]
        ,[ActionName]
        ,[RecordStatus]
        ,[CreatedBy]
        ,[CreatedDate]
        ,[ModifiedBy]
        ,[ModifiedDate])
	VALUES
	(
		@ScreenId,
		'ViewActualM3Revenue',
		@RecordStatus,
		@UserName,
		@CurrentDate,
		@UserName,
		@CurrentDate
	)
END

SELECT @ScreenId = ScreenId FROM Screen WHERE ScreenCode = 'VA'
IF NOT EXISTS (SELECT TOP 1 1 FROM ScreenAction WHERE ScreenId = @ScreenId AND ActionName = 'ViewForecastedM3Revenue')
BEGIN
	INSERT INTO [dbo].[ScreenAction]
        ([ScreenId]
        ,[ActionName]
        ,[RecordStatus]
        ,[CreatedBy]
        ,[CreatedDate]
        ,[ModifiedBy]
        ,[ModifiedDate])
	VALUES
	(
		@ScreenId,
		'ViewForecastedM3Revenue',
		@RecordStatus,
		@UserName,
		@CurrentDate,
		@UserName,
		@CurrentDate
	)
END

-- Inserting actions for to-do-list page.

SELECT @ScreenId = ScreenId FROM Screen WHERE ScreenCode = 'TDL'
IF NOT EXISTS (SELECT TOP 1 1 FROM ScreenAction WHERE ScreenId = @ScreenId AND ActionName = 'Assign Relationship Manager')
BEGIN
	INSERT INTO [dbo].[ScreenAction]
        ([ScreenId]
        ,[ActionName]
        ,[RecordStatus]
        ,[CreatedBy]
        ,[CreatedDate]
        ,[ModifiedBy]
        ,[ModifiedDate])
	VALUES
	(
		@ScreenId,
		'Assign Relationship Manager',
		@RecordStatus,
		@UserName,
		@CurrentDate,
		@UserName,
		@CurrentDate
	)
END

SELECT @ScreenId = ScreenId FROM Screen WHERE ScreenCode = 'TDL'
IF NOT EXISTS (SELECT TOP 1 1 FROM ScreenAction WHERE ScreenId = @ScreenId AND ActionName = 'Assign Billing Manager')
BEGIN
	INSERT INTO [dbo].[ScreenAction]
        ([ScreenId]
        ,[ActionName]
        ,[RecordStatus]
        ,[CreatedBy]
        ,[CreatedDate]
        ,[ModifiedBy]
        ,[ModifiedDate])
	VALUES
	(
		@ScreenId,
		'Assign Billing Manager',
		@RecordStatus,
		@UserName,
		@CurrentDate,
		@UserName,
		@CurrentDate
	)
END

SELECT @ScreenId = ScreenId FROM Screen WHERE ScreenCode = 'TDL'
IF NOT EXISTS (SELECT TOP 1 1 FROM ScreenAction WHERE ScreenId = @ScreenId AND ActionName = 'Pending Weekly Checklist')
BEGIN
	INSERT INTO [dbo].[ScreenAction]
        ([ScreenId]
        ,[ActionName]
        ,[RecordStatus]
        ,[CreatedBy]
        ,[CreatedDate]
        ,[ModifiedBy]
        ,[ModifiedDate])
	VALUES
	(
		@ScreenId,
		'Pending Weekly Checklist',
		@RecordStatus,
		@UserName,
		@CurrentDate,
		@UserName,
		@CurrentDate
	)
END

SELECT @ScreenId = ScreenId FROM Screen WHERE ScreenCode = 'TDL'
IF NOT EXISTS (SELECT TOP 1 1 FROM ScreenAction WHERE ScreenId = @ScreenId AND ActionName = 'Pending Monthly Checklist')
BEGIN
	INSERT INTO [dbo].[ScreenAction]
        ([ScreenId]
        ,[ActionName]
        ,[RecordStatus]
        ,[CreatedBy]
        ,[CreatedDate]
        ,[ModifiedBy]
        ,[ModifiedDate])
	VALUES
	(
		@ScreenId,
		'Pending Monthly Checklist',
		@RecordStatus,
		@UserName,
		@CurrentDate,
		@UserName,
		@CurrentDate
	)
END

SELECT @ScreenId = ScreenId FROM Screen WHERE ScreenCode = 'TDL'
IF NOT EXISTS (SELECT TOP 1 1 FROM ScreenAction WHERE ScreenId = @ScreenId AND ActionName = 'Partially Completed Clients')
BEGIN
	INSERT INTO [dbo].[ScreenAction]
        ([ScreenId]
        ,[ActionName]
        ,[RecordStatus]
        ,[CreatedBy]
        ,[CreatedDate]
        ,[ModifiedBy]
        ,[ModifiedDate])
	VALUES
	(
		@ScreenId,
		'Partially Completed Clients',
		@RecordStatus,
		@UserName,
		@CurrentDate,
		@UserName,
		@CurrentDate
	)
END

SELECT @ScreenId = ScreenId FROM Screen WHERE ScreenCode = 'TDL'
IF NOT EXISTS (SELECT TOP 1 1 FROM ScreenAction WHERE ScreenId = @ScreenId AND ActionName = 'Set Targets')
BEGIN
	INSERT INTO [dbo].[ScreenAction]
        ([ScreenId]
        ,[ActionName]
        ,[RecordStatus]
        ,[CreatedBy]
        ,[CreatedDate]
        ,[ModifiedBy]
        ,[ModifiedDate])
	VALUES
	(
		@ScreenId,
		'Set Targets',
		@RecordStatus,
		@UserName,
		@CurrentDate,
		@UserName,
		@CurrentDate
	)
END

SELECT @ScreenId = ScreenId FROM Screen WHERE ScreenCode = 'TDL'
IF NOT EXISTS (SELECT TOP 1 1 FROM ScreenAction WHERE ScreenId = @ScreenId AND ActionName = 'Set Holidays')
BEGIN
	INSERT INTO [dbo].[ScreenAction]
        ([ScreenId]
        ,[ActionName]
        ,[RecordStatus]
        ,[CreatedBy]
        ,[CreatedDate]
        ,[ModifiedBy]
        ,[ModifiedDate])
	VALUES
	(
		@ScreenId,
		'Set Holidays',
		@RecordStatus,
		@UserName,
		@CurrentDate,
		@UserName,
		@CurrentDate
	)
END

SELECT @ScreenId = ScreenId FROM Screen WHERE ScreenCode = 'TDL'
IF NOT EXISTS (SELECT TOP 1 1 FROM ScreenAction WHERE ScreenId = @ScreenId AND ActionName = 'Unclosed Months')
BEGIN
	INSERT INTO [dbo].[ScreenAction]
        ([ScreenId]
        ,[ActionName]
        ,[RecordStatus]
        ,[CreatedBy]
        ,[CreatedDate]
        ,[ModifiedBy]
        ,[ModifiedDate])
	VALUES
	(
		@ScreenId,
		'Unclosed Months',
		@RecordStatus,
		@UserName,
		@CurrentDate,
		@UserName,
		@CurrentDate
	)
END

SELECT @ScreenId = ScreenId FROM Screen WHERE ScreenCode = 'TDL'
IF NOT EXISTS (SELECT TOP 1 1 FROM ScreenAction WHERE ScreenId = @ScreenId AND ActionName = 'Incomplete Deposit Log')
BEGIN
	INSERT INTO [dbo].[ScreenAction]
        ([ScreenId]
        ,[ActionName]
        ,[RecordStatus]
        ,[CreatedBy]
        ,[CreatedDate]
        ,[ModifiedBy]
        ,[ModifiedDate])
	VALUES
	(
		@ScreenId,
		'Incomplete Deposit Log',
		@RecordStatus,
		@UserName,
		@CurrentDate,
		@UserName,
		@CurrentDate
	)
END

SELECT @ScreenId = ScreenId FROM Screen WHERE ScreenCode = 'VA'
IF NOT EXISTS (SELECT TOP 1 1 FROM ScreenAction WHERE ScreenId = @ScreenId AND ActionName = 'ViewClientHistory')
BEGIN
	INSERT INTO [dbo].[ScreenAction]
        ([ScreenId]
        ,[ActionName]
        ,[RecordStatus]
        ,[CreatedBy]
        ,[CreatedDate]
        ,[ModifiedBy]
        ,[ModifiedDate])
	VALUES
	(
		@ScreenId,
		'ViewClientHistory',
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
