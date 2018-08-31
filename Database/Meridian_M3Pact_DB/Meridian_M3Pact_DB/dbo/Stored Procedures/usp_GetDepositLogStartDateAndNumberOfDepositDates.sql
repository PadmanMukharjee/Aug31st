/*
Script Name : usp_GetDepositLogStartDateAndNumberOfDepositDates
Module_Name : M3Pact
Created By  : Abhishek Kovvuri
CreatedDate : 18/05/2018

Revision History 
=============
Ver  Date				Who						Comment
1.0  18-May-2018		Abhishek Kovvuri		To get the deposit log start date and number of deposit dates for a client.
1.1  13-July-2018       Abhishek Kovvuri        Updated sproc to get date and deposit dates specific for month.
*/

CREATE PROC [dbo].[usp_GetDepositLogStartDateAndNumberOfDepositDates]

@ClientCode nvarchar(20) ,
@LastNumberOfDays INT ,
@EndDate Date

AS
BEGIN

DECLARE @StartDate date;
DECLARE @TempLastNumberOfDays int;
DECLARE @TotalNumberOfDepositDates int;
DECLARE @ClientId INT;

SET @EndDate =  CONVERT(DATE , @EndDate , 111)

IF NOT(@EndDate < CONVERT(DATE , GETDATE() , 111))
BEGIN
   IF NOT EXISTS(SELECT TOP 1 1 FROM  CLIENT C 
        	INNER JOIN ClientPayer CP ON C.ClientID = CP.ClientID 
        	INNER JOIN  DepositLog dl ON CP.ClientPayerID = DL.ClientPayerID 
        	inner join DateDimension dd on dl.DepositDateID = dd.DateKey WHERE dd.date = @EndDate
        	AND C.ClientCode = @clientcode AND dd.IsHoliday != 1 AND dd.IsWeekend != 1 AND 
   		    C.IsActive = 'A' AND CP.RecordStatus = 'A' AND DL.RecordStatus = 'A')
   BEGIN
           SET @EndDate =  CONVERT(DATE , DATEADD(DD , -1 , @EndDate) , 111)     
   END
END

SELECT @TotalNumberOfDepositDates = COUNT(DISTINCT dd.date) FROM CLIENT C 
	                                INNER JOIN ClientPayer CP ON C.ClientID = CP.ClientID 
	                                INNER JOIN  DepositLog dl ON CP.ClientPayerID = DL.ClientPayerID 
	                                INNER JOIN DateDimension dd on dl.DepositDateID = dd.DateKey WHERE 
	                                C.ClientCode = @clientcode and dd.IsHoliday != 1 and dd.IsWeekend != 1 AND
									C.IsActive = 'A' AND CP.RecordStatus = 'A' AND DL.RecordStatus = 'A' AND dd.Date <= @EndDate

SET @TempLastNumberOfDays = @LastNumberOfDays
IF(@LastNumberOfDays  > @TotalNumberOfDepositDates)
 BEGIN
   SET @LastNumberOfDays = @TotalNumberOfDepositDates;
   SET @TempLastNumberOfDays = @LastNumberOfDays
 END
    
 WHILE(@LastNumberOfDays > 0 )
 BEGIN
       SET @StartDate = @EndDate
       IF EXISTS(SELECT TOP 1 1 FROM  CLIENT C 
     	INNER JOIN ClientPayer CP ON C.ClientID = CP.ClientID 
     	INNER JOIN  DepositLog dl ON CP.ClientPayerID = DL.ClientPayerID 
     	inner join DateDimension dd on dl.DepositDateID = dd.DateKey WHERE dd.date = @StartDate
     	AND C.ClientCode = @clientcode and dd.IsHoliday != 1 and dd.IsWeekend != 1 AND
	    C.IsActive = 'A' AND CP.RecordStatus = 'A' AND DL.RecordStatus = 'A')
     	  BEGIN
     	    SET @LastNumberOfDays = @LastNumberOfDays - 1;
     	    SET @EndDate = CONVERT(DATE,DATEADD(DD , -1 , @EndDate),111)
     	  END
       ELSE
           BEGIN
             SET @EndDate = CONVERT(DATE,DATEADD(DD , -1 , @EndDate),111)
           END
 END

SELECT @ClientId = ClientId FROM Client WHERE ClientCode = @ClientCode
SELECT @ClientId AS ClientId , @ClientCode AS ClientCode , @StartDate AS DepositStartDate ,  @TempLastNumberOfDays AS NumberOfDepositDates

END
GO
