/**********************************************************************************
Team        :  M3Pact
Date        :  12-Apr-2018
Table Name	:  [dbo].[ClientConfigStepStatus]
ScriptName	:  19_ClientConfigStepStatus.sql

Data Verification Script : 

SELECT * FROM [ClientConfigStepStatus](NOLOCK) WHERE RecordStatus = 'A'

Revision History
================
Ver.   Date           	Who						Description
1.0	   12-Apr-2018		Shravani Palla			Adding Records to ClientConfigStepStatus
***********************************************************************************/

BEGIN TRY
BEGIN TRANSACTION

DECLARE @UserType NVARCHAR(60) = 'Admin'
DECLARE @CurrentDate DATETIME = GETDATE()
DECLARE @RecordStatus CHAR(1) = 'A'

IF NOT EXISTS ( SELECT TOP 1 1 FROM ClientConfigStepStatus WHERE ClientConfigStepStatusName = 'In Progress')
BEGIN
	INSERT INTO ClientConfigStepStatus
	(
		ClientConfigStepStatusID,
		ClientConfigStepStatusName,
		ClientConfigStepStatusDescription,
		RecordStatus,
		CreatedDate,
		CreatedBy,
		ModifiedDate,
		ModifiedBy
	)
	VALUES
	(
		1,
		'In Progress',							--	ClientConfigStepStatusName
		'The step is currently in progress',	--	ClientConfigStepStatusDescription
		@RecordStatus,							--	RecordStatus
		@CurrentDate,							--	CreatedDate
		@UserType,								--	CreatedBy
		@CurrentDate,							--	ModifiedDate
		@UserType								--	ModifiedBy
	)
END

IF NOT EXISTS ( SELECT TOP 1 1 FROM ClientConfigStepStatus WHERE ClientConfigStepStatusName = 'Completed')
BEGIN
	INSERT INTO ClientConfigStepStatus
	(
		ClientConfigStepStatusID,
		ClientConfigStepStatusName,
		ClientConfigStepStatusDescription,
		RecordStatus,
		CreatedDate,
		CreatedBy,
		ModifiedDate,
		ModifiedBy
	)
	VALUES
	(
		2,
		'Completed',							--	ClientConfigStepStatusName
		'The step is completed successfully',	--	ClientConfigStepStatusDescription
		@RecordStatus,							--	RecordStatus
		@CurrentDate,							--	CreatedDate
		@UserType,								--	CreatedBy
		@CurrentDate,							--	ModifiedDate
		@UserType								--	ModifiedBy
	)
END

IF NOT EXISTS ( SELECT TOP 1 1 FROM ClientConfigStepStatus WHERE ClientConfigStepStatusName = 'Failed')
BEGIN
	INSERT INTO ClientConfigStepStatus
	(
		ClientConfigStepStatusID,
		ClientConfigStepStatusName,
		ClientConfigStepStatusDescription,
		RecordStatus,
		CreatedDate,
		CreatedBy,
		ModifiedDate,
		ModifiedBy
	)
	VALUES
	(
		3,
		'Failed',								--	ClientConfigStepStatusName
		'The step is failed',					--	ClientConfigStepStatusDescription
		@RecordStatus,							--	RecordStatus
		@CurrentDate,							--	CreatedDate
		@UserType,								--	CreatedBy
		@CurrentDate,							--	ModifiedDate
		@UserType								--	ModifiedBy
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