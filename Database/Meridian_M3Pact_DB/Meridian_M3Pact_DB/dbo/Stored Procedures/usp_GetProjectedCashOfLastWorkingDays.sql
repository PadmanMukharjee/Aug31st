

CREATE PROC [dbo].[usp_GetProjectedCashOfLastWorkingDays]

@ClientCode nvarchar(20),
@Month INT,
@Year INT,
@NumberOfLastWorkingDays INT,
@StartDate DATE,
@EndDate DATE


AS
BEGIN

DECLARE @NumberOfWorkingDaysInAMonth INT;
DECLARE @WeekEndDepositStartDate DATE;

SELECT @NumberOfWorkingDaysInAMonth =  bd.BusinessDays 
FROM BusinessDays bd INNER JOIN MONTH m ON bd.MonthId = m.MonthId 
WHERE bd.year = @Year and m.monthid = @Month

CREATE TABLE #TempWeekDaysAmount
(
 StartDate DATE,
 EndDate DATE,
 Day INT,
 Amount decimal
)
CREATE TABLE #TempWeekEndsAmount
(
 StartDate DATE,
 EndDate DATE,
 Day INT,
 Amount decimal
)


IF EXISTS(SELECT TOP 1 ClientCode FROM Client WHERE ClientCode = @ClientCode AND IsActive = 'A')
BEGIN
      
      WHILE(@NumberOfLastWorkingDays  > 0)
         BEGIN
           IF EXISTS(SELECT TOP 1 1 FROM  CLIENT C 
     	    INNER JOIN ClientPayer CP ON C.ClientID = CP.ClientID 
     	    INNER JOIN  DepositLog dl ON CP.ClientPayerID = DL.ClientPayerID 
     	    inner join DateDimension dd on dl.DepositDateID = dd.DateKey WHERE dd.date = @StartDate
     	    AND C.ClientCode = @clientcode AND dd.IsHoliday != 1 AND dd.IsWeekend != 1 AND 
		    C.IsActive = 'A' AND DL.RecordStatus = 'A')
            BEGIN
                INSERT INTO #TempWeekDaysAmount 
                SELECT @StartDate,  @EndDate, @NumberOfLastWorkingDays , (SUM(dl.Amount)/@NumberOfLastWorkingDays) * @NumberOfWorkingDaysInAMonth
                FROM  Client c INNER JOIN ClientPayer cp ON c.ClientID = cp.ClientID INNER JOIN DepositLog dl ON cp.ClientPayerID = dl.ClientPayerID 
                INNER JOIN DateDimension dd ON dd.DateKey = dl.DepositDateID
                WHERE 
                c.ClientCode = @ClientCode AND c.IsActive = 'A' AND dl.RecordStatus = 'A' AND 
                dd.Date BETWEEN @StartDate AND @EndDate AND dd.isholiday = 0 AND dd.isweekend = 0
                AND dl.RecordStatus = 'A'
                
				SET @WeekEndDepositStartDate = @StartDate;
				IF(@StartDate < CAST(DATEADD(DAY,-DAY(GETDATE())+1, CAST(GETDATE() AS DATE)) AS DATE))
				BEGIN
				  SET  @WeekEndDepositStartDate = CAST(DATEADD(DAY,-DAY(GETDATE())+1, CAST(GETDATE() AS DATE)) AS DATE)
				END

				INSERT INTO #TempWeekEndsAmount
                SELECT @StartDate ,@EndDate, @NumberOfLastWorkingDays ,  SUM(dl.Amount)
                FROM  Client c INNER JOIN ClientPayer cp ON c.ClientID = cp.ClientID INNER JOIN DepositLog dl ON cp.ClientPayerID = dl.ClientPayerID 
                INNER JOIN DateDimension dd ON dd.DateKey = dl.DepositDateID
                WHERE 
                c.ClientCode = @ClientCode AND c.IsActive = 'A' AND dl.RecordStatus = 'A' AND 
                dd.Date BETWEEN @WeekEndDepositStartDate AND DATEADD(day,-1,DATEADD(month,@Month,DATEADD(year,@Year-1900,0))) 
				AND dd.isholiday = 0 AND dd.isweekend = 1
                AND dl.RecordStatus = 'A'
                
                SET @StartDate = CONVERT(DATE,DATEADD(DD,1,@StartDate),111)
                SET @NumberOfLastWorkingDays = @NumberOfLastWorkingDays - 1 
             END;
         	ELSE
         	BEGIN
         	   SET @StartDate = CONVERT(DATE,DATEADD(DD,1,@StartDate),111)
         	END;
       END
END

SELECT  twd.Day AS LastWorkingDayNumber , (ISNULL(twe.Amount , 0) + ISNULL(twd.Amount , 0)) AS ProjectedCash 
FROM #TempWeekDaysAmount twd INNER JOIN #TempWeekEndsAmount twe ON twd.Day = twe.Day ORDER BY LastWorkingDayNumber

DROP TABLE #TempWeekDaysAmount
DROP TABLE #TempWeekEndsAmount

END

GO


