/*
Script Name : usp_GetProjectedCashBasedOnLastNumberOfWeeks
Module_Name : M3Pact
Created By  : Abhishek Kovvuri
CreatedDate : 06/06/2018

Revision History 
=============
Ver  Date				Who						Comment
1.0  06-June-2018		Abhishek Kovvuri		To get the projected cash based on last number of weeks.
1.1  17-July-2018       Abhishek Kovvuri        Added EndDate parameter to get the data specific to given month.
*/


CREATE PROC [dbo].[usp_GetProjectedCashBasedOnLastNumberOfWeeks]

@ClientCode nvarchar(20),
@Month INT,
@Year INT,
@LastNumberOfWeeks INT,
@EndDate DATE

AS
BEGIN

DECLARE @DepositStartDate Date
DECLARE @WEEKDAY INT;
SET @WEEKDAY = 2;
DECLARE @IsTrue bit;
SET @IsTrue = 0

CREATE TABLE #TempDepositAmount(DepositAmount decimal , LastWeeks int , WeekDayName varchar(50) , StartDate date )
 WHILE(@WEEKDAY != 1 AND @WEEKDAY != 7)
   BEGIN
        
        INSERT INTO #TempDepositAmount(DepositAmount , LastWeeks , WeekDayName , StartDate)
        (SELECT SUM(DA.Amount) AS DepositAmount ,
         COUNT(*) AS LastWeeks  ,
         DA.WeekDayName ,
		 MIN(DA.Date)
        FROM 
        (SELECT TOP (@LastNumberOfWeeks)  SUM(dl.amount) AS Amount , dd.date as Date , dd.WeekDayName 
        FROM Client c 
        INNER JOIN ClientPayer cp ON c.ClientID = cp.ClientID 
        INNER JOIN DepositLog dl  ON  dl.ClientPayerID = cp.ClientPayerID 
        INNER JOIN DateDimension dd on  dl.DepositDateId = dd.DateKey 
        WHERE  c.ClientCode = @ClientCode AND c.IsActive = 'A' AND dl.RecordStatus = 'A'
        AND dd.IsHoliday != 1 and dd.IsWeekend != 1 AND dd.Date <= @EndDate
        AND dd.Weekday = @WEEKDAY GROUP BY dd.date , dd.WeekDayName
        ORDER BY dd.date desc) 
        AS DA
        GROUP BY weekdayname) 
        
        SET @WEEKDAY = @WEEKDAY + 1
         
   END

   IF EXISTS(SELECT * FROM  #TempDepositAmount  WHERE  LastWeeks != @LastNumberOfWeeks)
   BEGIN
     SET @IsTrue = 1
   END

   IF(@IsTrue = 1)
	 BEGIN
		    SELECT @LastNumberOfWeeks =  MIN(LastWeeks) FROM   #TempDepositAmount
			DELETE FROM #TempDepositAmount
			SET @WEEKDAY = 2;
           
		     WHILE(@WEEKDAY != 1 AND @WEEKDAY != 7)
               BEGIN
                    INSERT INTO #TempDepositAmount(DepositAmount , LastWeeks , WeekDayName , StartDate)
                    (SELECT SUM(DA.Amount) AS DepositAmount,
                     COUNT(*) AS LastWeeks,
                     DA.WeekDayName,
		             MIN(DA.Date)
                    FROM 
                    (SELECT TOP (@LastNumberOfWeeks)  SUM(dl.amount) AS Amount , dd.date as Date , dd.WeekDayName 
                    FROM Client c 
                    INNER JOIN ClientPayer cp ON c.ClientID = cp.ClientID 
                    INNER JOIN DepositLog dl  ON  dl.ClientPayerID = cp.ClientPayerID 
                    INNER JOIN DateDimension dd on  dl.DepositDateId = dd.DateKey 
                    WHERE  c.ClientCode = @ClientCode AND c.IsActive = 'A' AND dl.RecordStatus = 'A'
                    AND dd.IsHoliday != 1 and dd.IsWeekend != 1 AND dd.Date <= @EndDate
                    AND dd.Weekday = @WEEKDAY GROUP BY dd.date , dd.WeekDayName
                    ORDER BY dd.date desc) 
                    AS DA
                    GROUP BY weekdayname) 
        
                   SET @WEEKDAY = @WEEKDAY + 1
               END
      END 

     SELECT @DepositStartDate = MIN(StartDate) FROM #TempDepositAmount	
        
     SELECT TWA.WeekDayName AS WeekName ,
     TWA.DepositAmount AS DepositAmount , 
     TWA.LastWeeks AS WeeksDaysCompleted , 
     ISNULL(WDL.WeekDaysLeft , 0) AS WeekDaysLeft ,
     @DepositStartDate AS DepositStartDate
     
     FROM #TempDepositAmount TWA 
     LEFT JOIN 
     (SELECT 
     COUNT(dd.date) AS WeekDaysLeft, dd.WeekDayName  
     FROM datedimension dd 
	 LEFT JOIN 
	 (SELECT 
     DISTINCT  dd.Date,
     dd.DateKey 
     FROM  Client c 
     INNER JOIN ClientPayer cp ON c.ClientID = cp.ClientID 
     INNER JOIN DepositLog dl  ON  dl.ClientPayerID = cp.ClientPayerID 
     INNER JOIN DateDimension dd on  dl.DepositDateId = dd.DateKey 
     WHERE  
     dd.date BETWEEN DATEADD(month, @Month - 1, dateadd(year, @Year - 1900, 0))
     AND DATEADD(month, @Month,     dateadd(year, @Year - 1900, -1))  
     AND  c.ClientCode = @ClientCode AND c.IsActive = 'A' AND dl.RecordStatus = 'A'
     AND dd.IsHoliday != 1 and dd.IsWeekend != 1) AS CDD ON  dd.date = CDD.date
     WHERE 
     dd.date BETWEEN DATEADD(month, @Month - 1, dateadd(year, @Year - 1900, 0))
	 AND DATEADD(month, @Month,     dateadd(year, @Year - 1900, -1))  
     AND cdd.date IS NULL 
     AND dd.IsHoliday != 1 and dd.IsWeekend != 1 
     GROUP BY dd.WeekDayName) AS WDL ON TWA.WeekDayName = WDL.WeekDayName
     					
     DROP TABLE #TempDepositAmount

END

GO