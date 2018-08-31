/*
Script Name : usp_GetNumberOfDepositWeeksForClient
Module_Name : M3Pact
Created By  : Abhishek Kovvuri
CreatedDate : 18/05/2018

Revision History 
=============
Ver  Date				Who						Comment
1.0  18-May-2018		Abhishek Kovvuri		To get the number of deposit weeks for a client.
1.1  13-July-2018       Abhishek Kovvuri        Added enddate as a parameter.
*/



CREATE PROC [dbo].[usp_GetNumberOfDepositWeeksForClient]
@ClientCode nvarchar(20),
@EndDate Date

AS
BEGIN
    SET @EndDate =  CONVERT(DATE , @EndDate , 111)
    DECLARE @RowCount INT
    SET @RowCount = 0;

    CREATE TABLE #TempTable(lastweeks int  ,weekdayname varchar(50))
    INSERT INTO #TempTable
    SELECT COUNT(*) AS LastWeeks ,
           DA.WeekDayName 
	FROM 
    (SELECT    dd.date as Date , dd.WeekDayName 
    FROM Client c 
    INNER JOIN ClientPayer cp ON c.ClientID = cp.ClientID 
    INNER JOIN DepositLog dl  ON  dl.ClientPayerID = cp.ClientPayerID 
    INNER JOIN DateDimension dd on  dl.DepositDateId = dd.DateKey 
    WHERE  c.ClientCode = @ClientCode AND c.IsActive = 'A' AND cp.RecordStatus = 'A' AND dl.RecordStatus = 'A'
    AND dd.IsHoliday != 1 and dd.IsWeekend != 1 AND dd.Date <= @EndDate
    GROUP BY dd.date , dd.WeekDayName
    ) 
    AS DA
    GROUP BY weekdayname


	SELECT @RowCount = COUNT(*) from #TempTable
	IF(@rowcount = 5)
	BEGIN
		SELECT MIN(LastWeeks) AS NumberOfDepositWeeksForClient from #TempTable
    END
	ELSE
	BEGIN
	   SELECT 0 AS NumberOfDepositWeeksForClient
	END

    DROP TABLE #TempTable

END

GO