


/*
To get the All weekly and monthly checklists
and associated sites,systes and clients

*/


CREATE PROCEDURE [dbo].[usp_GetViewAllChecklists]
AS
BEGIN

DECLARE @CurrentDate DateTime = GETDATE(),
		@Active char(1) ='A',
		@SiteAttributeCode nvarchar(60) = 'SITE',
		@SystemAttributeCode nvarchar(60) = 'SYS'

--Checklists Data
SELECT  C.CheckListName,
		C.CheckListID,
		CT.CheckListTypeCode
from CheckList C
	  JOIN CheckListType CT on C.CheckListTypeID = CT.CheckListTypeID
WHERE C.RecordStatus=@Active
	  AND CT.RecordStatus=@Active


--Checklist Site Data
SELECT C.CheckListName,
	   S.SiteCode,
	   S.SiteName
FROM Site S
     JOIN CheckListAttributeMap CAM ON CAM.CheckListAttributeValueID = CAST( S.SiteID AS VARCHAR)
     JOIN CheckListAttribute CA ON CA.CheckListAttributeID=CAM.CheckListAttributeID
     JOIN CheckList C ON C.CheckListID = CAM.CheckListID
WHERE CA.AttributeCode = @SiteAttributeCode
     AND CA.RecordStatus = @Active
     AND C.RecordStatus = @Active
     AND CAM.RecordStatus = @Active
     AND @CurrentDate BETWEEN CAM.StartDate AND CAM.EndDate


-- Checklist System Data	 
SELECT C.CheckListName,
	   S.SystemCode,
       S.SystemName
	   
FROM [System] S
     JOIN CheckListAttributeMap CAM ON CAM.CheckListAttributeValueID = CAST( S.SystemID AS VARCHAR)
     JOIN CheckListAttribute CA ON CA.CheckListAttributeID=CAM.CheckListAttributeID
     JOIN CheckList C ON C.CheckListID = CAM.CheckListID
WHERE CA.AttributeCode = @SystemAttributeCode
     AND CA.RecordStatus = @Active
     AND C.RecordStatus = @Active
     AND CAM.RecordStatus = @Active
     AND @CurrentDate BETWEEN CAM.StartDate AND CAM.EndDate


--Clients for Checklists

SELECT CL.CheckListName,
       C.ClientCode,
	   C.Name
FROM ClientCheckListMap CC
    JOIN Client C ON C.ClientID = CC.ClientID
    JOIN CheckList CL ON CL.CheckListID = CC.CheckListID
WHERE @CurrentDate BETWEEN CC.StartDate AND CC.EndDate
	AND C.IsActive = @Active
	AND CC.RecordStatus=@Active
	AND CL.RecordStatus=@Active


END