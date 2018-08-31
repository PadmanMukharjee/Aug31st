/*
Script Name : 06_ClientConfigStep
Module_Name : M3Pact
Created By  : Shravani P
CreatedDate : 04/12/2018

Data Verification Script : 

SELECT * FROM [ClientConfigStep](NOLOCK) WHERE RecordStatus = 'A'
*/

BEGIN TRY
BEGIN TRANSACTION

DECLARE @UserType NVARCHAR(60) = 'Admin'
DECLARE @CurrentDate DATETIME = GETDATE()
DECLARE @RecordStatus CHAR(1) = 'A'
DECLARE @ScreenCode VARCHAR(50);

SELECT  @ScreenCode = ScreenCode FROM Screen WHERE ScreenName = 'Client Data'
IF NOT EXISTS ( SELECT TOP 1 1 FROM ClientConfigStep WHERE ClientConfigStepName = 'Client Data')
	BEGIN
		INSERT INTO ClientConfigStep
		(
			ClientConfigStepName,
			ClientConfigStepDescription,
			ScreenCode,
			DisplayOrder,
			RecordStatus,
			CreatedDate,
			CreatedBy,
			ModifiedDate,
			ModifiedBy
		)
		VALUES
		(
			'Client Data',			--	ClientConfigStepName
			'Details of a Client',	--	ClientConfigStepDescription
			@ScreenCode,			--  ScreenCode
			1,						--  DisplayOrder
			@RecordStatus,			--	RecordStatus
			@CurrentDate,			--	CreatedDate
			@UserType,				--	CreatedBy
			@CurrentDate,			--	ModifiedDate
			@UserType				--	ModifiedBy
		)
	END
ELSE
	BEGIN
		IF EXISTS( SELECT TOP 1 1 FROM ClientConfigStep WHERE ClientConfigStepName = 'Client Data' AND ScreenCode IS NULL)
			BEGIN
				UPDATE ClientConfigStep
				SET ScreenCode = @ScreenCode
				WHERE ClientConfigStepName = 'Client Data'
			END

		IF EXISTS( SELECT TOP 1 1 FROM ClientConfigStep WHERE ClientConfigStepName = 'Client Data' AND DisplayOrder IS NULL)
			BEGIN
				UPDATE ClientConfigStep
				SET DisplayOrder = 1
				WHERE ClientConfigStepName = 'Client Data'
			END
	END

SELECT  @ScreenCode = ScreenCode FROM Screen WHERE ScreenName = 'Log Setup'
IF NOT EXISTS ( SELECT TOP 1 1 FROM ClientConfigStep WHERE ClientConfigStepName = 'Log Setup')
	BEGIN
		INSERT INTO ClientConfigStep
		(
			ClientConfigStepName,
			ClientConfigStepDescription,
			ScreenCode,
			DisplayOrder,
			RecordStatus,
			CreatedDate,
			CreatedBy,
			ModifiedDate,
			ModifiedBy
		)
		VALUES
		(
			'Log Setup',					--	ClientConfigStepName
			'Assigning Payers to a Client',	--	ClientConfigStepDescription
			@ScreenCode,					--  ScreenCode
			3,								--  DisplayOrder
			@RecordStatus,					--	RecordStatus
			@CurrentDate,					--	CreatedDate
			@UserType,						--	CreatedBy
			@CurrentDate,					--	ModifiedDate
			@UserType						--	ModifiedBy
		)
	END
ELSE
	BEGIN
		IF EXISTS( SELECT TOP 1 1 FROM ClientConfigStep WHERE ClientConfigStepName = 'Log Setup' AND ScreenCode IS NULL)
			BEGIN
				UPDATE ClientConfigStep
				SET ScreenCode = @ScreenCode
				WHERE ClientConfigStepName = 'Log Setup'
			END

		IF EXISTS( SELECT TOP 1 1 FROM ClientConfigStep WHERE ClientConfigStepName = 'Log Setup' AND DisplayOrder IS NULL)
			BEGIN
				UPDATE ClientConfigStep
				SET DisplayOrder = 3
				WHERE ClientConfigStepName = 'Log Setup'
			END
	END

SELECT  @ScreenCode = ScreenCode FROM Screen WHERE ScreenName = 'Assign User'
IF NOT EXISTS ( SELECT TOP 1 1 FROM ClientConfigStep WHERE ClientConfigStepName = 'Assign User')
	BEGIN
	INSERT INTO ClientConfigStep
	(
		ClientConfigStepName,
		ClientConfigStepDescription,
		ScreenCode,
		DisplayOrder,
		RecordStatus,
		CreatedDate,
		CreatedBy,
		ModifiedDate,
		ModifiedBy
	)
	VALUES
	(
		'Assign User',					--	ClientConfigStepName
		'Assigning Users to a Client',	--	ClientConfigStepDescription
		@ScreenCode,					--  ScreenCode
		4,								--  DisplayOrder
		@RecordStatus,					--	RecordStatus
		@CurrentDate,					--	CreatedDate
		@UserType,						--	CreatedBy
		@CurrentDate,					--	ModifiedDate
		@UserType						--	ModifiedBy
	)
END
ELSE
	BEGIN
		IF EXISTS( SELECT TOP 1 1 FROM ClientConfigStep WHERE ClientConfigStepName = 'Assign User' AND ScreenCode IS NULL)
			BEGIN
				UPDATE ClientConfigStep
				SET ScreenCode = @ScreenCode
				WHERE ClientConfigStepName = 'Assign User'
			END

		IF EXISTS( SELECT TOP 1 1 FROM ClientConfigStep WHERE ClientConfigStepName = 'Assign User' AND DisplayOrder IS NULL)
			BEGIN
				UPDATE ClientConfigStep
				SET DisplayOrder = 4
				WHERE ClientConfigStepName = 'Assign User'
			END
	END

SELECT  @ScreenCode = ScreenCode FROM Screen WHERE ScreenName = 'Monthly Targets'
IF NOT EXISTS ( SELECT TOP 1 1 FROM ClientConfigStep WHERE ClientConfigStepName = 'Monthly Targets')
	BEGIN
		INSERT INTO ClientConfigStep
		(
			ClientConfigStepName,
			ClientConfigStepDescription,
			ScreenCode,
			DisplayOrder,
			RecordStatus,
			CreatedDate,
			CreatedBy,
			ModifiedDate,
			ModifiedBy
		)
		VALUES
		(
			'Monthly Targets',					--	ClientConfigStepName
			'Set Monthly Targets of a Client',	--	ClientConfigStepDescription
			@ScreenCode,						--  ScreenCode
			5,									--  DisplayOrder
			@RecordStatus,						--	RecordStatus
			@CurrentDate,						--	CreatedDate
			@UserType,							--	CreatedBy
			@CurrentDate,						--	ModifiedDate
			@UserType							--	ModifiedBy
		)
	END
ELSE
	BEGIN
		IF EXISTS( SELECT TOP 1 1 FROM ClientConfigStep WHERE ClientConfigStepName = 'Monthly Targets' AND ScreenCode IS NULL)
			BEGIN
				UPDATE ClientConfigStep
				SET ScreenCode = @ScreenCode
				WHERE ClientConfigStepName = 'Monthly Targets'
			END

		IF EXISTS( SELECT TOP 1 1 FROM ClientConfigStep WHERE ClientConfigStepName = 'Monthly Targets' AND DisplayOrder IS NULL)
			BEGIN
				UPDATE ClientConfigStep
				SET DisplayOrder = 5
				WHERE ClientConfigStepName = 'Monthly Targets'
			END
	END

SELECT  @ScreenCode = ScreenCode FROM Screen WHERE ScreenName = 'KPI Setup'
IF NOT EXISTS ( SELECT TOP 1 1 FROM ClientConfigStep WHERE ClientConfigStepName = 'KPI Setup')
	BEGIN
	INSERT INTO ClientConfigStep
	(
		ClientConfigStepName,
		ClientConfigStepDescription,
		ScreenCode,
		DisplayOrder,
		RecordStatus,
		CreatedDate,
		CreatedBy,
		ModifiedDate,
		ModifiedBy
	)
	VALUES
	(
		'KPI Setup',						--	ClientConfigStepName
		'Assign KPI to a Client',			--	ClientConfigStepDescription
		@ScreenCode,						--  ScreenCode
		2,									--  DisplayOrder
		@RecordStatus,						--	RecordStatus
		@CurrentDate,						--	CreatedDate
		@UserType,							--	CreatedBy
		@CurrentDate,						--	ModifiedDate
		@UserType							--	ModifiedBy
	)
END
ELSE
	BEGIN
		IF EXISTS( SELECT TOP 1 1 FROM ClientConfigStep WHERE ClientConfigStepName = 'KPI Setup' AND ScreenCode IS NULL)
			BEGIN
				UPDATE ClientConfigStep
				SET ScreenCode = @ScreenCode
				WHERE ClientConfigStepName = 'KPI Setup'
			END

		IF EXISTS( SELECT TOP 1 1 FROM ClientConfigStep WHERE ClientConfigStepName = 'KPI Setup' AND DisplayOrder IS NULL)
			BEGIN
				UPDATE ClientConfigStep
				SET DisplayOrder = 2
				WHERE ClientConfigStepName = 'KPI Setup'
			END
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