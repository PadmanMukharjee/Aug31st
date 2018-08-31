
/*
Script Name : usp_GetAllClientsData
Module_Name : M3Pact
Created By  : Abhishek Kovvuri
CreatedDate : 27/07/2018

Revision History 
=============
Ver  Date				Who						Comment
1.0  27-July-2018	    Abhishek Kovvuri		To get all the clients data.
1.1  31-July-2018       Abhishek Kovvuri        Query optimization in projected cash calculation.
*/

CREATE PROC [dbo].[usp_GetAllClientsData]
@UserId NVARCHAR(100) ,
@Month INT , 
@Year INT , 
@Role NVARCHAR(100)

AS
BEGIN

DECLARE @MonthStartDate DATE;
DECLARE @CurrentDate DATE;
DECLARE @AttributeCode NVARCHAR(1000);

SET @MonthStartDate = DATEADD(month, @Month - 1, dateadd(year, @Year - 1900, 0));
SET @CurrentDate = CONVERT(DATE , GETDATE() , 111);

-- Stores the client list with respect to logged in user.

CREATE TABLE #UserClientList
(
  ClientId INT ,
  ClientCode NVARCHAR(50) , 
  ClientName NVARCHAR(1000) ,
  Status CHAR , 
  PercentageOfCash NUMERIC(5,2),
  FlatFee DECIMAL , 
  BillingManagerId INT , 
  RelationshipManagerId INT
)

IF (@Role = 'Admin')
BEGIN
   INSERT INTO #UserClientList
   SELECT  c.ClientID , c.ClientCode , c.Name , c.IsActive , c.PercentageOfCash , c.FlatFee , c.BillingManagerID , c.RelationShipManagerID
   FROM 
   Client c
END
ELSE
BEGIN
  
  INSERT INTO #UserClientList
  SELECT  c.ClientID , c.ClientCode , c.Name , c.IsActive , c.PercentageOfCash , c.FlatFee , c.BillingManagerID , c.RelationShipManagerID
  FROM 
  Client c 
  INNER JOIN UserClientMap uc on c.ClientID = uc.ClientID 
  INNER JOIN UserLogin ul on uc.UserID = ul.ID 
  WHERE 
  ul.RecordStatus = 'A' 
  AND uc.RecordStatus = 'A' 
  AND ul.UserID = @UserId order by c.ClientID asc
END


-- Stores the ultimate result set

CREATE TABLE #AllClientsResultSet
(
  ClientId INT ,
  ClientCode NVARCHAR(50) , 
  ClientName NVARCHAR(1000) , 
  Site NVARCHAR(1000) , 
  BillingManager NVARCHAR(1000) , 
  RelationshipManager NVARCHAR(1000) , 
  MTDDeposit DECIMAL ,
  MTDTarget DECIMAL ,
  ProjectedCash DECIMAL , 
  MonthlyTarget DECIMAL , 
  ActualM3Revenue DECIMAL , 
  ForecastedM3Revenue DECIMAL , 
  Status NVARCHAR(1000) , 
  NumberOfBusinessDaysInMonth INT , 
  ClientRevenue DECIMAL

)

---   Insert into #AllClientsResultSet table.

INSERT INTO #AllClientsResultSet
SELECT DISTINCT (c.ClientID) , c.ClientCode , c.Name , s.SiteName , 
c.BillingManagerID , c.RelationShipManagerID , NULL , NULL , NULL , 
NULL , NULL , NULL , c.IsActive , NULL , NULL
FROM 
#UserClientList ucl 
LEFT JOIN Client c ON ucl.ClientID = c.ClientID
LEFT JOIN Site s ON c.SiteID = s.SiteID

-- Updating monthly target , number of business days in a month and client revenue of the result set.

UPDATE #AllClientsResultSet SET MonthlyTarget = ct.Payments ,
                                NumberOfBusinessDaysInMonth = bd.BusinessDays , 
								ClientRevenue = ct.Revenue
                                
FROM #AllClientsResultSet acrs 
LEFT JOIN ClientTarget ct ON acrs.ClientId = ct.ClientID
INNER JOIN  Month m on ct.MonthID = m.MonthID
LEFT JOIN BusinessDays bd on m.MonthID = bd.MonthID
WHERE  
ct.RecordStatus = 'A' 
AND bd.Year = @Year 
AND bd.MonthID = @Month 
AND m.RecordStatus = 'A'
AND bd.RecordStatus = 'A' 
AND ct.CalendarYear = @Year 
AND m.MonthID = @Month 

--- Client wise number of deposit days of the current month

CREATE TABLE #ClientDepositDaysTempTable
(
  ClientId INT ,
  NumberOfDepositDaysOfMonth INT ,
  DepositAmountOfMonth DECIMAL
)

--- Client wise sum of deposits of the current month

CREATE TABLE #ClientMonthlyDeposits
(
 ClientId INT ,
 DepositAmountOfMonth DECIMAL
)

-- Inserting data into #ClientDepositDaysTempTable

INSERT INTO #ClientDepositDaysTempTable
SELECT uccl.ClientId , ISNULL(ndd.NumberOfDepositDaysOfMonth , 0) , NULL
FROM 
#UserClientList uccl LEFT JOIN 
(SELECT ucl.ClientId  ,  COUNT(DISTINCT dd.Date) AS NumberOfDepositDaysOfMonth , SUM(dl.Amount) AS DepositAmountOfMonth
FROM 
#UserClientList ucl 
LEFT JOIN Client c ON ucl.ClientId = c.ClientID
LEFT JOIN ClientPayer cp ON c.ClientID = cp.ClientID
LEFT JOIN DepositLog dl ON cp.ClientPayerID = dl.ClientPayerID
LEFT JOIN DateDimension dd ON dl.DepositDateID = dd.DateKey
WHERE 
dd.IsHoliday != 1 
AND dd.IsWeekend != 1 
AND dd.Date BETWEEN @MonthStartDate AND @CurrentDate
AND dl.RecordStatus = 'A' 
GROUP BY ucl.ClientID) AS ndd ON uccl.ClientId = ndd.ClientId

-- Inserting data into ##ClientMonthlyDeposits

INSERT INTO #ClientMonthlyDeposits
SELECT uccl.ClientId , ISNULL(cmd.DepositAmountOfMonth , 0) 
FROM #UserClientList uccl 
LEFT JOIN 
(SELECT ucl.ClientId  , SUM(dl.Amount) AS DepositAmountOfMonth
FROM #UserClientList ucl LEFT JOIN 
Client c ON ucl.ClientId = c.ClientID
LEFT JOIN ClientPayer cp ON c.ClientID = cp.ClientID
LEFT JOIN DepositLog dl ON cp.ClientPayerID = dl.ClientPayerID
LEFT JOIN DateDimension dd ON dl.DepositDateID = dd.DateKey
WHERE 
dd.Date BETWEEN @MonthStartDate AND @CurrentDate
AND dl.RecordStatus = 'A' 
GROUP BY ucl.ClientID) AS cmd 
ON uccl.ClientId = cmd.ClientId

-- Update DepositAmountOfMonth in #ClientDepositDaysTempTable table.

UPDATE cddtt 
SET cddtt.DepositAmountOfMonth = cmod.DepositAmountOfMonth
FROM #ClientDepositDaysTempTable cddtt INNER JOIN 
#ClientMonthlyDeposits cmod ON cddtt.ClientId = cmod.ClientId

----   Updating MTD Deposit and MTD Target of the result set.

UPDATE acrs
SET  acrs.MTDDeposit = CASE 
                       WHEN acrs.MonthlyTarget IS NULL THEN NULL
                       ELSE cdtt.DepositAmountOfMonth 
					   END
   , acrs.MTDTarget = CASE 
                      WHEN acrs.MonthlyTarget IS NULL THEN NULL
					  ELSE acrs.MonthlyTarget * cdtt.NumberOfDepositDaysOfMonth / acrs.NumberOfBusinessDaysInMonth
					  END
FROM #AllClientsResultSet acrs INNER JOIN #ClientDepositDaysTempTable cdtt ON acrs.ClientId = cdtt.ClientId

----   Updating BillingManager of the result set.

UPDATE acrs
SET acrs.BillingManager = ul.LastName + ' ' + ul.FirstName
FROM #AllClientsResultSet acrs INNER JOIN #UserClientList ucl ON acrs.ClientId = ucl.ClientId INNER JOIN UserLogin ul ON acrs.BillingManager = ul.ID 

----   Updating RelationshipManager of the result set.

UPDATE acrs
SET acrs.RelationshipManager = ul.LastName + ' ' + ul.FirstName
FROM #AllClientsResultSet acrs INNER JOIN #UserClientList ucl ON acrs.ClientId = ucl.ClientId INNER JOIN UserLogin ul ON acrs.RelationshipManager = ul.ID 

----   Updating ActualM3Revenue of the result set.

UPDATE acrs
SET acrs.ActualM3Revenue = CASE 
                           WHEN acrs.MonthlyTarget IS NULL THEN NULL
                           WHEN ucl.FlatFee = 0 THEN cddtt.DepositAmountOfMonth * ucl.PercentageOfCash / 100 
						   ELSE acrs.ClientRevenue
						   END
FROM #AllClientsResultSet acrs INNER JOIN #UserClientList ucl ON acrs.ClientId = ucl.ClientId INNER JOIN #ClientDepositDaysTempTable cddtt ON ucl.ClientId = cddtt.ClientId

----   Updating ForecastedM3Revenue of the result set.

UPDATE acrs
SET acrs.ForecastedM3Revenue = CASE 
                               WHEN acrs.MonthlyTarget IS NULL THEN NULL
                               WHEN ucl.FlatFee = 0 THEN acrs.ClientRevenue
						       ELSE acrs.ClientRevenue
						       END
FROM #AllClientsResultSet acrs INNER JOIN #UserClientList ucl ON acrs.ClientId = ucl.ClientId INNER JOIN #ClientDepositDaysTempTable cddtt ON ucl.ClientId = cddtt.ClientId

----   Updating ProjectedCash of the result set.

UPDATE acrs 
SET acrs.ProjectedCash = CASE
                         WHEN acrs.MonthlyTarget IS NULL THEN NULL
                         WHEN dlmd.ProjectedCash IS NULL THEN 0
						 ELSE dlmd.ProjectedCash
						 END
FROM #AllClientsResultSet acrs INNER JOIN DepositLogMonthlyDetails dlmd ON acrs.ClientId = dlmd.ClientId
WHERE dlmd.MonthId = @Month AND dlmd.Year = @Year

--- Get the result set

SELECT ClientId , ClientCode , ClientName , Site , BillingManager , RelationshipManager ,
       MTDDeposit , MTDTarget , ProjectedCash , MonthlyTarget , ActualM3Revenue , ForecastedM3Revenue , Status  
FROM #AllClientsResultSet

DROP TABLE #ClientDepositDaysTempTable
DROP TABLE #AllClientsResultSet
DROP TABLE #UserClientList
DROP TABLE #ClientMonthlyDeposits

END


