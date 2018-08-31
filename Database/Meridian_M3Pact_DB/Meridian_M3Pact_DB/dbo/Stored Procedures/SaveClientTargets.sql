/*
Script Name : SaveClientTargets
Module_Name : M3Pact

Revision History 
=============
Ver  Date				Who						Comment
1.0  ??					??						Save Client Targets
1.1  16-Aug-2018		Shravani Palla			Insert CreatedBy, ModifiedBy fields with UserID
*/

CREATE PROCEDURE [dbo].[SaveClientTargets]
	@ClientCode VARCHAR(100),
	@CalendarYear INT,
	@AnnualCharges NUMERIC,
	@GrossCollectionRate NUMERIC(5,2),
	@UserID NVARCHAR(255)
AS
BEGIN

	DECLARE @TotalWorkingDays INT,
	@monthlyWorkingDays INT,
	@HolidaysInYear INT,
	@HolidaysInMonth INT,
	@LastDayofYear DATE,
	@FirstDayofYear DATE

	DECLARE @ClientID INT

	DECLARE	@Charges DECIMAL,
	@Payments DECIMAL,
	@Revenue DECIMAL,
	@ChargesPerEachDay DECIMAL,
	@TotalCharges DECIMAL = 0,
	@TotalRevenue DECIMAL = 0,
	@TotalPayments DECIMAL = 0,
	@PercentageOfCash FLOAT		

	SELECT @ClientID = ClientID FROM Client WHERE ClientCode = @ClientCode 

	IF EXISTS(SELECT 1 FROM ClientTarget WHERE ClientID = @ClientID AND RecordStatus = 'A' AND CalendarYear = @CalendarYear )
	BEGIN		
		UPDATE ClientTarget
		SET RecordStatus = 'I'
		WHERE ClientID = @ClientID AND RecordStatus = 'A' AND CalendarYear = @CalendarYear
	END

	SELECT @HolidaysInYear = COUNT(*) FROM DateDimension
	WHERE [Year] = @CalendarYear AND IsHoliday = 1 AND IsWeekend = 0

	SELECT @TotalWorkingDays = COUNT(*) FROM DateDimension
	WHERE [Year] = @CalendarYear AND IsWeekend = 0 

	SET @TotalWorkingDays= @TotalWorkingDays - @HolidaysInYear
	
	DECLARE @LoopCounter INT = 1

	WHILE (@LoopCounter <= 12)
	BEGIN
		DECLARE @DayOfMonth TinyInt Set @DayOfMonth = 1
		DECLARE @Month TinyInt Set @Month = @LoopCounter
		DECLARE @Year Integer Set @Year = @CalendarYear
		DECLARE @Date Date
		SELECT @Date= DateAdd(day, @DayOfMonth - 1, DateAdd(month, @Month - 1, DateAdd(Year, @Year-1900, 0)))												 
										 
		SELECT @monthlyWorkingDays = COUNT(*) FROM DateDimension
		WHERE [Year] = @CalendarYear AND IsWeekend = 0 AND [Month] = @Month											 	
		
		SELECT @HolidaysInMonth= COUNT(*) FROM DateDimension
		WHERE Year = @CalendarYear AND IsHoliday = 1 AND IsWeekend = 0 AND [Month] = @Month

		SET @monthlyWorkingDays = @monthlyWorkingDays - @HolidaysInMonth
		SET @Charges = (CAST( @monthlyWorkingDays AS FLOAT)/CAST( @TotalWorkingDays AS FLOAT))*@AnnualCharges
		SET @Payments= @Charges * (@GrossCollectionRate/100)	

		SELECT @PercentageOfCash = PercentageOfCash FROM Client WHERE ClientID = @ClientID
		
		IF (( @PercentageOfCash = 0) OR @PercentageOfCash IS NULL)
		BEGIN
			SET @Revenue= (SELECT FlatFee FROM Client WHERE ClientID = @ClientID )
		END			
		ELSE
		BEGIN
			SET @Revenue= @Payments * @PercentageOfCash/100
		END
		
		INSERT INTO ClientTarget
		(
			ClientID,
			MonthID,
			CalendarYear,
			IsManualEntry,
			AnnualCharges,
			GrossCollectionRate,
			Charges,
			Payments,
			Revenue,
			RecordStatus,
			CreatedDate,
			CreatedBy,
			ModifiedDate,
			ModifiedBy
		)
		VALUES
		(
			@ClientID,
			@Month,
			@CalendarYear,
			0,
			@AnnualCharges,
			@GrossCollectionRate,
			@Charges,
			@Payments,
			@Revenue,
			'A',
			GETDATE(),
			@UserID,
			GETDATE(),
			@UserID
		)

		SET @LoopCounter = @LoopCounter + 1
	END
		
	SELECT * FROM ClientTarget
	WHERE ClientId = @ClientId AND recordstatus = 'A' AND CalendarYear = @CalendarYear AND IsManualEntry = 0

END



