
/*
Script Name : usp_GetAssignedKPIsForClient
Module_Name : M3Pact
Created By  : Rajesh Aavula
CreatedDate : 05/26/2018

Revision History 
=============
Ver  Date				Who						Comment
1.0  26-May-2018		Rajesh Aavula			Get all the KPI questions assigned for a client.
1.1  06-July-2018       Abhishek Kovvuri        Adding a kpiMeasureId column in the result set.
*/



CREATE PROC [dbo].[usp_GetAssignedM3KPIsForClient]
@ClientCode nvarchar(20)

AS
BEGIN

    DECLARE @ClientId INT
	Declare @M3TypeId INT

SET @ClientId= (SELECT ClientID FROM Client WHERE ClientCode=@ClientCode)
SET @M3TypeId= (SELECT CheckListTypeID FROM CheckListType WHERE CheckListTypeCode='M3')

CREATE TABLE #kPITempTable(
			ClientKPIMapID INT,
			KPIID int,
			KPIDescription nvarchar(60),
			CheckListTypeCode nvarchar(60),
			IsUniversal bit null,
			[Company Standard] nvarchar(500) null ,
			[Client Standard] nvarchar(500) null,
			IsSLA bit null,
			KpiMeasureId nvarchar(50) null
			)


--Selecting M3 kpis for client
INSERT INTO #kPITempTable
SELECT 
		CKM.ClientKPIMapID,
		KPI.KPIID,
		KPI.KPIDescription,
		'M3' AS CheckListTypeCode,
		KPI.IsUniversal,
		KPI.AlertLevel AS [Company Standard],
		CKM.[Client Standard],
		CKM.IsSLA,
		KPI.Measure
FROM 
 ClientKPIMap CKM
JOIN KPI KPI ON KPI.KPIID=CKM.KPIID AND KPI.CheckListTypeId=@M3TypeId
WHERE 
CKM.ClientID=@ClientId
AND GETDATE() BETWEEN CKM.StartDate AND CKM.EndDate
AND CKM.RecordStatus ='A' 
AND CKM.StartDate != CKM.EndDate


SELECT * FROM #kPITempTable
SELECT 
		KPIT.ClientKPIMapID,
		U.ID,
		U.Email,
		U.FirstName,
		U.IsMeridianUser,
		U.LastName,
		U.MobileNumber,
		R.RoleCode,
		U.UserName,
		U.UserID
FROM #kPITempTable KPIT
JOIN ClientKPIUserMap CKM on KPIT.ClientKPIMapID = CKM.ClientKPIMapID
JOIN UserLogin u on u.ID=ckm.UserId
JOIN Roles R ON U.RoleID=R.RoleID
WHERE CKM.RecordStatus='A'

DROP TABLE #kPITempTable

END
GO

