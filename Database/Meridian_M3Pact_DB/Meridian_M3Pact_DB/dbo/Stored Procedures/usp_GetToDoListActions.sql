
/*
Script Name : GetToDoListActions
Module_Name : M3Pact
Created By  : Abhishek Kovvuri
CreatedDate : 08/14/2018

Revision History 
=============
Ver  Date				Who						Comment
1.1  14-August-2018     Abhishek Kovvuri        Get all the to-do-list actions with clients.  
*/


CREATE PROCEDURE [dbo].[usp_GetToDoListActions](
 @role VARCHAR(50) , 
 @UserId NVARCHAR(100)  
)
AS
BEGIN

DECLARE @CURRENTDATE DATETIME;
DECLARE @CURRENTMONTH INT;
DECLARE @CURRENTYEAR INT;
SET @CURRENTDATE = GETDATE();
SET @CURRENTMONTH = MONTH(@CURRENTDATE);
SET @CURRENTYEAR = YEAR(@CURRENTDATE);

CREATE TABLE #Actions(ActionName NVARCHAR(1000))
INSERT INTO #Actions
SELECT sa.ActionName 
FROM Screen s INNER JOIN ScreenAction sa ON s.ScreenId = sa.ScreenId 
INNER JOIN RoleAction ra ON ra.ScreenActionId = sa.ScreenActionId 
WHERE s.RecordStatus = 'A'
AND sa.RecordStatus = 'A' 
AND ra.RecordStatus = 'A' 
AND ra.RoleId = (SELECT RoleId FROM Roles WHERE RoleCode = @role) 
AND s.ScreenId = (SELECT ScreenId FROM Screen WHERE ScreenCode = 'TDL')

-- Creates a table for complete result set.
CREATE TABLE #ResultSet(ActionName NVARCHAR(1000) , ClientId INT , ClientCode NVARCHAR(100) , ClientName NVARCHAR(255))

-- Create a table which contains clients of logged in user.
CREATE TABLE #TempClient(TempId INT PRIMARY KEY IDENTITY(1,1) , ClientId INT , ClientCode VARCHAR(250))

IF(@Role = 'Admin')
BEGIN
  INSERT INTO #TempClient 
  SELECT ClientId , ClientCode
  FROM 
  Client WHERE IsActive = 'A' ORDER BY Name ASC
END
ELSE
BEGIN
  INSERT INTO #TempClient 
  SELECT  c.ClientID , ClientCode
  FROM 
  Client c 
  INNER JOIN UserClientMap uc ON c.ClientID = uc.ClientID 
  INNER JOIN UserLogin ul ON uc.UserID = ul.ID 
  WHERE 
  ul.RecordStatus = 'A' 
  AND uc.RecordStatus = 'A' 
  AND ul.UserID = @UserId ORDER BY Name ASC
END

-- Create a table which contains clients of logged in user.

---  unclosed month logic --------
IF EXISTS(SELECT * FROM #Actions WHERE ActionName = 'Unclosed Months')
BEGIN
      DECLARE @NEXTFIFTHDAY DATE;
      SELECT top 1 @NEXTFIFTHDAY = td.date
      				               FROM ( 
      				               SELECT TOP 5 dd.date 
      				               FROM DateDimension dd 
      				               WHERE dd.date >= DATEADD(DAY,1,EOMONTH(@CURRENTDATE,-1)) 
      				               AND IsHoliday != 1 AND IsWeekend != 1 ORDER BY Date asc) AS td ORDER BY td.Date DESC
      
      
      IF(CONVERT(DATE , @CURRENTDATE , 111) <=  @NEXTFIFTHDAY)
      BEGIN
      	  INSERT INTO #ResultSet
          SELECT DISTINCT 'Unclosed Months' , c.ClientID , c.ClientCode ,  c.Name 
          FROM 
          #TempClient tc INNER JOIN Client c ON tc.ClientId = c.ClientID 
      	  INNER JOIN DepositLogMonthlyDetails dlmd ON c.ClientID = dlmd.ClientId
          WHERE c.IsActive = 'A' 
          AND DATEFROMPARTS (dlmd.Year, dlmd.MonthId, DATEPART(day,c.ContractStartDate)) > c.ContractStartDate 
          AND dlmd.MonthStatus != 'C' AND dlmd.RecordStatus = 'A' 
          AND dlmd.MonthId NOT IN (@CURRENTMONTH , @CURRENTMONTH - 1) ORDER BY c.Name ASC
      
      END
      ELSE
      BEGIN
         INSERT INTO #ResultSet
         SELECT DISTINCT 'Unclosed Month',  c.ClientID , c.ClientCode ,  c.Name 
         FROM 
         #TempClient tc INNER JOIN Client c ON tc.ClientId = c.ClientID 
         INNER JOIN DepositLogMonthlyDetails dlmd ON c.ClientID = dlmd.ClientId
         WHERE c.IsActive = 'A' AND 
         DATEFROMPARTS (dlmd.Year, dlmd.MonthId, DATEPART(day,c.ContractStartDate)) > c.ContractStartDate 
         AND dlmd.MonthStatus != 'C' 
         AND dlmd.RecordStatus = 'A'
         AND dlmd.MonthId != @CURRENTMONTH ORDER BY c.Name ASC
      END
END

---  unclosed month logic --------

------  incomplete deposit log ------------

IF EXISTS(SELECT * FROM #Actions WHERE ActionName = 'Incomplete Deposit Log')
BEGIN
      DECLARE @PREVIOUSSECONDDAY DATE;
      SELECT top 1 @PREVIOUSSECONDDAY = lwd.PrevoiusSecondDay
                                        FROM
                                        (SELECT TOP 1
                                         lead(date,2) OVER(ORDER BY date DESC) AS PrevoiusSecondDay
                                        FROM DateDimension
                                        WHERE IsHoliday = 0 AND IsWeekend = 0 AND Date < @CURRENTDATE
                                        ORDER BY date DESC) AS lwd ORDER BY lwd.PrevoiusSecondDay DESC
      
      CREATE TABLE #TempExpectedNumberOfDepositDays(ClientId INT, ContractStartDate DATE , ExpectedNumberOfDepositDays BIGINT , 
                                                    ClientCode NVARCHAR(50) , ClientName NVARCHAR(255))
      INSERT INTO #TempExpectedNumberOfDepositDays
      SELECT c.ClientID , c.ContractStartDate , 
      (Select COUNT(DISTINCT Date) 
       FROM DateDimension WHERE Date BETWEEN c.ContractStartDate AND @PREVIOUSSECONDDAY 
       AND IsHoliday != 1 AND IsWeekend != 1) 
       AS ExpectedNumberOfDepositDays ,
       c.ClientCode , c.Name
      FROM #TempClient tc INNER JOIN Client c ON tc.ClientId = c.ClientID 
      
      CREATE TABLE #TempActualNumberOfDepositDays(ClientId INT , ActualNumberOfDepositDays BIGINT)
      INSERT INTO #TempActualNumberOfDepositDays
      SELECT c.ClientId  ,  ISNULL(COUNT(DISTINCT dd.Date), 0) AS ActualNumberOfDepositDays
      FROM 
      #TempClient tc 
      LEFT JOIN Client c ON tc.ClientId = c.ClientID
      LEFT JOIN ClientPayer cp ON c.ClientID = cp.ClientID
      LEFT JOIN DepositLog dl ON cp.ClientPayerID = dl.ClientPayerID
      LEFT JOIN DateDimension dd ON dl.DepositDateID = dd.DateKey
      WHERE 
      dd.IsHoliday != 1
      AND dd.IsWeekend != 1
      AND dd.Date BETWEEN c.ContractStartDate AND @PREVIOUSSECONDDAY
      AND dl.RecordStatus = 'A' 
      GROUP BY c.ClientID

      INSERT INTO #ResultSet
      SELECT 'Incomplete Deposit Log' , tend.ClientId , tend.ClientCode , tend.ClientName 
      FROM #TempExpectedNumberOfDepositDays tend LEFT JOIN #TempActualNumberOfDepositDays tand ON tend.ClientId = tand.ClientId
      WHERE tend.ExpectedNumberOfDepositDays != CASE 
                                                WHEN tand.ActualNumberOfDepositDays IS NULL THEN 0
      										    ELSE tand.ActualNumberOfDepositDays
      										    END
	  ORDER BY tend.ClientName ASC
      
      DROP TABLE #TempExpectedNumberOfDepositDays
      DROP TABLE #TempActualNumberOfDepositDays

END

------  incomplete deposit log ------------

--------  Partially Completed clients --------------
IF EXISTS(SELECT * FROM #Actions WHERE ActionName = 'Partially Completed Clients')
BEGIN
     INSERT INTO #ResultSet
     SELECT 'Partially Completed Clients' , ClientID , ClientCode , Name FROM Client WHERE IsActive = 'P' ORDER BY Name ASC
END
--------  Partially Completed clients --------------

--------  Set Targets for upcoming year -----------

IF EXISTS(SELECT * FROM #Actions WHERE ActionName = 'Set Targets')
BEGIN
     IF(@CURRENTMONTH = 12)
     BEGIN
       
       INSERT INTO #ResultSet
       SELECT DISTINCT 'Set Targets' , c.ClientID , c.ClientCode , c.Name 
       FROM #TempClient tc INNER JOIN Client c ON tc.ClientId = c.ClientID 
       LEFT JOIN ClientTarget ct ON c.ClientID = ct.ClientID
       WHERE ct.RecordStatus = 'A' AND c.ClientID NOT IN 
       (SELECT DISTINCT c.ClientID 
       FROM #TempClient tc INNER JOIN Client c ON tc.ClientId = c.ClientID 
       LEFT JOIN ClientTarget ct ON c.ClientID = ct.ClientID
       WHERE ct.RecordStatus = 'A' AND ct.CalendarYear = @CURRENTYEAR)
     
       UNION 
     
       SELECT DISTINCT 'Set Targets' , c.ClientID , c.ClientCode , c.Name 
       FROM #TempClient tc INNER JOIN Client c ON tc.ClientId = c.ClientID
       LEFT JOIN ClientTarget ct ON c.ClientID = ct.ClientID
       WHERE ct.RecordStatus = 'A' AND c.ClientID NOT IN 
       (SELECT DISTINCT c.ClientID 
       FROM #TempClient tc INNER JOIN Client c ON tc.ClientId = c.ClientID 
       LEFT JOIN ClientTarget ct ON c.ClientID = ct.ClientID
       WHERE ct.RecordStatus = 'A' AND ct.CalendarYear > @CURRENTYEAR)
	   ORDER BY c.Name ASC
     
     END
     
     IF(@CURRENTMONTH < 12)
     BEGIN
       
       INSERT INTO #ResultSet
       SELECT DISTINCT 'Set Targets' , c.ClientID , c.ClientCode , c.Name 
       FROM #TempClient tc INNER JOIN Client c ON tc.ClientId = c.ClientID 
       LEFT join ClientTarget ct ON c.ClientID = ct.ClientID
       WHERE ct.RecordStatus = 'A' AND c.ClientID NOT IN 
       (SELECT DISTINCT c.ClientID 
       FROM #TempClient tc INNER JOIN Client c ON tc.ClientId = c.ClientID 
       LEFT JOIN ClientTarget ct ON c.ClientID = ct.ClientID
       WHERE ct.RecordStatus = 'A' AND ct.CalendarYear = @CURRENTYEAR)
	   ORDER BY c.Name ASC
     
     END
END

--------  Set Targets for upcoming year -----------

-------------  Set Holidays for the upcoming year ----------

IF EXISTS(SELECT * FROM #Actions WHERE ActionName = 'Set Holidays')
BEGIN
     DECLARE @NumberOfHolidays INT;
     IF(@CURRENTMONTH = 12)
     BEGIN
       SELECT @NumberOfHolidays =  COUNT(*) FROM DateDimension WHERE IsHoliday = 1 AND Year = @CURRENTYEAR + 1
     END
     ELSE
     BEGIN
       SELECT @NumberOfHolidays =  COUNT(*) FROM DateDimension WHERE IsHoliday = 1 AND Year = @CURRENTYEAR
     END
       
     IF(@NumberOfHolidays < 5)
     BEGIN
        INSERT INTO #ResultSet VALUES('Set Holidays' , NULL , NULL , NULL)
     END
END

-------------  Set Holidays for the upcoming year ----------

------------    Pending Weekly Monthly Checklist -----------------

IF EXISTS(SELECT * FROM #Actions WHERE ActionName = 'Pending Weekly Checklist' OR ActionName = 'Pending Monthly Checklist')
BEGIN
      DECLARE @MAXID INT, @Counter INT
      SET @COUNTER = 1
      SELECT @MAXID = COUNT(*) FROM #TempClient
      
      WHILE (@COUNTER <= @MAXID)
        BEGIN
             DECLARE @ClientCode NVARCHAR(255);
      	     DECLARE @ClientId NVARCHAR(255);
      	     DECLARE @ClientName NVARCHAR(255);
      	     SELECT @ClientCode = tc.ClientCode
                                  FROM #TempClient AS tc
                                  WHERE TempId = @COUNTER
             SELECT @ClientId = ClientId
                                FROM Client 
                                WHERE ClientCode = @ClientCode
             SELECT @ClientName = Name
                                  FROM Client 
                                  WHERE ClientCode = @ClientCode
      	
		     CREATE TABLE #PendingWeeklyChecklistsResultSet(PendingDates DATETIME) 
      	     INSERT INTO #PendingWeeklyChecklistsResultSet
             EXEC GetWeeklyPendingChecklist @ClientCode
      
      	     DECLARE @WEEKLYCOUNT INT;
      	     SELECT @WEEKLYCOUNT = COUNT(*) FROM #PendingWeeklyChecklistsResultSet
      
      	     CREATE TABLE #PendingMonthlyChecklistsResultSet(PendingDates DATETIME) 
      	     INSERT INTO #PendingMonthlyChecklistsResultSet
             EXEC GetMonthlyPendingChecklist @ClientCode
      
      	     DECLARE @MONTHLYCOUNT INT;
      	     SELECT @MONTHLYCOUNT = COUNT(*) FROM #PendingMonthlyChecklistsResultSet
      
      	     IF(@WEEKLYCOUNT > 1)
      	     BEGIN
      	         INSERT INTO #ResultSet values('Pending Weekly Checklist' , @ClientId , @ClientCode , @ClientName)
      	     END
      
      	     IF(@MONTHLYCOUNT > 1)
      	     BEGIN
      	         INSERT INTO #ResultSet values('Pending Monthly Checklist' , @ClientId , @ClientCode , @ClientName)
      	     END

      	     DROP TABLE #PendingWeeklyChecklistsResultSet
      	     DROP TABLE #PendingMonthlyChecklistsResultSet
      	     
      	     SET @COUNTER = @COUNTER + 1
       END
END



------------    Pending Weekly Monthly Checklist -----------------

SELECT * FROM #ResultSet
DROP TABLE #ResultSet
DROP TABLE #TempClient
DROP TABLE #Actions

END
GO
