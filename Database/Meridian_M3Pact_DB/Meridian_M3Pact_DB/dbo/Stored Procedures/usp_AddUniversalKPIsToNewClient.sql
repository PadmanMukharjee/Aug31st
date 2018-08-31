
/*
Script Name : usp_AddUniversalKPIsToNewClient
Module_Name : M3Pact
Created By  : Abhishek Kovvuri
CreatedDate : 30/05/2018

Revision History 
=============
Ver  Date				Who						Comment
1.0  05-May-2018		Abhishek Kovvuri		Add universal KPIs to new client.
*/


CREATE PROC [dbo].[usp_AddUniversalKPIsToNewClient]
@ClientCode nvarchar(20),
@User nvarchar(20)

AS
BEGIN

DECLARE @EndDate DATE;
SET @EndDate = '9999-12-31 00:00:00.000'

DECLARE @ClientId INT
SELECT @ClientId = ClientId FROM client where ClientCode = @ClientCode

INSERT INTO ClientKPIMap(ClientID , KPIID , [Client Standard] , IsSLA , StartDate , EndDate , RecordStatus , CreatedDate , CreatedBy , ModifiedDate , ModifiedBy)
(SELECT DISTINCT @ClientId , 
                 kp.KPIID,
				 NULL AS ClientStandard ,
				 NULL AS SLA , 
				 DATEADD(dd, 0, DATEDIFF(dd, 0, GETDATE())) AS StartDate , 
				 @EndDate AS EndDate,
				'A' AS RecordStatus,
				 GETDATE() AS CreatedDate,
				 @User AS CreatedBy,
				 GETDATE() AS ModifiedDate, 
				 @User AS ModifiedBy
FROM 
KPI kp INNER JOIN Question q ON kp.measure = q.QuestionCode 
INNER JOIN CheckListType clt ON kp.CheckListTypeId = clt.CheckListTypeID
WHERE kp.RecordStatus = 'A'
  AND q.RecordStatus = 'A'
  AND q.IsUniversal = 1
   AND q.EndDate=@EndDate
  AND clt.CheckListTypeCode in ( 'WEEK' , 'MONTH')

UNION 
 
 SELECT DISTINCT @ClientId , 
				kp.KPIID,
				'' AS ClientStandard ,
				0 AS SLA , 
				DATEADD(dd, 0, DATEDIFF(dd, 0, GETDATE())) AS StartDate , 
				 @EndDate AS EndDate,
				'A' AS RecordStatus,
				 GETDATE() AS CreatedDate,
				 @User AS CreatedBy,
				 GETDATE() AS ModifiedDate, 
				 @User AS ModifiedBy

FROM 
KPI kp 
INNER JOIN CheckListType clt ON kp.CheckListTypeId = clt.CheckListTypeID
WHERE kp.recordstatus = 'A'
  AND kp.IsUniversal = 1
  AND clt.CheckListTypeCode = 'M3')	

SELECT 'SUCCESS' AS Status

END
