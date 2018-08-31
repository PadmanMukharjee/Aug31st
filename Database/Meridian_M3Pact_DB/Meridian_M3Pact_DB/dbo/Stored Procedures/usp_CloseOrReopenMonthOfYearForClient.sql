/*
Script Name : usp_CloseOrReopenMonthOfYearForClient
Module_Name : M3Pact
Created By  : Shravani P
CreatedDate : 16-Jul-2018

Revision History 
=============
Ver  Date				Who						Comment
1.0  16-Jul-2018		Shravani Palla			Open/Close month functionality
1.1  20-Jul-2018		Shravani Palla			While closing a month, insert into DepositLogMonthlyDetails if no deposits entered at all
1.2  3-Aug-2018			Shravani Palla			While closing a month, consider total deposit sum in a month for insert into DepositLogMonthlyDetails with total sum
*/

CREATE PROCEDURE [dbo].[usp_CloseOrReopenMonthOfYearForClient]
(
	@MonthID INT,
	@Year INT,
	@ClientCode NVARCHAR(60),
	@IsCloseMonth BIT,
	@UserID NVARCHAR(255)
)
AS
BEGIN

	DECLARE @Today DATETIME = GETDATE();
	DECLARE @ClientID INT;
	DECLARE @MonthStatus CHAR(1)
	
	SELECT @ClientID = ClientID FROM Client WHERE ClientCode = @ClientCode
	SET @MonthStatus = CASE WHEN @IsCloseMonth = 1 THEN 'C' ELSE 'R' END

	IF(@ClientID IS NOT NULL)
	BEGIN

		BEGIN TRY
		BEGIN TRANSACTION

		-- Insert deposits as 0 for non entered days when closing a month
		IF(@IsCloseMonth = 1)
		BEGIN
			IF OBJECT_ID('tempdb..#DaysAndPayers') IS NOT NULL
			BEGIN
				DROP TABLE #DaysAndPayers
			END

			SELECT 
				DD.DateKey,
				DD.[Date],
				CP.ClientPayerID
			INTO #DaysAndPayers
			FROM DateDimension DD
			INNER JOIN ClientPayer CP ON CP.ClientID = @ClientID AND CP.RecordStatus <> 'D' AND DD.[Date] BETWEEN CP.StartDate AND CP.EndDate 
			WHERE DD.[Month] = @MonthID AND DD.[Year] = @Year AND DD.IsHoliday = 0 AND DD.RecordStatus = 'A'

			INSERT INTO DepositLog
			(
				ClientPayerID,
				DepositDateID,
				Amount,
				IsM3FeeExempt,
				RecordStatus,
				CreatedDate,
				CreatedBy,
				ModifiedDate,
				ModifiedBy
			)
			SELECT
				DP.ClientPayerID,
				DP.DateKey,
				0,
				NULL,
				'A',
				@Today,
				@UserID,
				@Today,
				@UserID
			FROM #DaysAndPayers DP
			LEFT JOIN DepositLog D ON D.DepositDateID = DP.DateKey AND D.ClientPayerID = DP.ClientPayerID
			WHERE D.DepositDateID IS NULL
			ORDER BY DP.DateKey, D.ClientPayerID
		END

		-- Update Month Status of Closed/Opened Month of a year for a client. If not exists then insert
		IF EXISTS(SELECT TOP 1 1 FROM DepositLogMonthlyDetails WHERE ClientID = @ClientID AND MonthId = @MonthID AND [Year] = @Year)
		BEGIN
			UPDATE DepositLogMonthlyDetails
			SET MonthStatus = @MonthStatus,
				ModifiedDate = @Today,
				ModifiedBy = @UserID
			WHERE ClientID = @ClientID AND MonthId = @MonthID AND [Year] = @Year
		END
		ELSE
		BEGIN

			INSERT INTO DepositLogMonthlyDetails
			(
				ClientId,
				MonthId,
				[Year],
				TotalDepositAmount,
				MonthStatus,
				RecordStatus,
				CreatedDate,
				CreatedBy,
				ModifiedDate,
				ModifiedBy
			)
			SELECT 
				@ClientID,
				@MonthID,
				@Year,
				ISNULL(SUM(DL.Amount), 0),
				@MonthStatus,
				'A',
				@Today,
				@UserID,
				@Today,
				@UserID
			FROM Client C
			INNER JOIN ClientPayer CP ON C.ClientId = CP.ClientId
			INNER JOIN DepositLog DL ON CP.ClientPayerId = DL.ClientPayerId
			INNER JOIN DateDimension DD ON  DL.DepositDateId = DD.DateKey
			WHERE C.ClientCode = @ClientCode AND C.IsActive = 'A' AND DL.RecordStatus = 'A'
				AND MONTH(DD.[Date]) = @MonthID AND YEAR(DD.[Date]) = @Year
															                                 
		END

		COMMIT TRANSACTION
		END TRY

		BEGIN CATCH
			SELECT	ERROR_MESSAGE(),
					ERROR_LINE(),
					ERROR_SEVERITY()

			ROLLBACK TRANSACTION
		END CATCH

	END

END
