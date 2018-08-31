/*
Script Name : 17_JobStatusAndJobProcess
Module_Name : M3Pact
Created By  : Mukharjee
CreatedDate : 06/07/2018

Data Verification Script :

SELECT * FROM [jobstatus](NOLOCK) WHERE RecordStatus = 'A'
SELECT * FROM [jobprocessgroup](NOLOCK) WHERE RecordStatus = 'A'
*/

BEGIN TRY
BEGIN TRANSACTION

DECLARE @User NVARCHAR(30) = 'Admin'

INSERT INTO JOBSTATUS 
(
JobStatusCode,
JobStatusName,
RecordStatus,
CreatedDate,
CreatedBy,
ModifiedDate,
ModifiedBy
)
SELECT * FROM
(SELECT 'Completed' As JobStatusCode ,'Completed' AS JobStatusName,'A' AS RecordStatus,GETDATE() AS CreatedDate,'Admin' AS CreatedBy,GETDATE() AS ModifiedDate,'Admin' AS ModifiedBy UNION ALL
SELECT'Started' As JobStatusCode,'Started'AS JobStatusName,'A' AS RecordStatus,GETDATE() AS CreatedDate,'Admin' AS CreatedBy,GETDATE() ModifiedDate,'Admin' AS ModifiedBy UNION ALL
SELECT'Failed' As JobStatusCode,'Failed'AS JobStatusName,'A' AS RecordStatus,GETDATE() AS CreatedDate,'Admin' AS CreatedBy,GETDATE() ModifiedDate,'Admin' AS ModifiedBy UNION ALL
SELECT 'EscalationStart' As JobStatusCode,'EscalationStart'AS JobStatusName,'A' AS RecordStatus,GETDATE() AS CreatedDate,'Admin' AS CreatedBy,GETDATE() ModifiedDate,'Admin' AS ModifiedBy UNION ALL
SELECT 'EscalationEnd' As JobStatusCode,'EscalationEnd'AS JobStatusName,'A' AS RecordStatus,GETDATE() AS CreatedDate,'Admin' AS CreatedBy,GETDATE() ModifiedDate,'Admin' AS ModifiedBy
) AS A
WHERE NOT EXISTS( SELECT 1 FROM JOBSTATUS (NOLOCK) B WHERE B.JobStatusCode=A.JobStatusCode) 



INSERT INTO JOBPROCESSGROUP
(

ProcessGroupCode,
ProcessGroupName,
RecordStatus,
CreatedDate,
CreatedBy,
ModifiedDate,
ModifiedBy
)
SELECT * FROM
(SELECT 'AlertAndEscaltion' AS ProcessGroupCode ,'Alert And Escaltion' As ProcessGroupName,'A' As RecordStatus ,GETDATE() AS CreatedDate,'Admin' As CreatedBy,GETDATE() AS ModifiedDate,'Admin' AS ModifiedBy )
 AS A
 WHERE NOT EXISTS( SELECT 1 FROM JOBPROCESSGROUP (NOLOCK) B WHERE B.ProcessGroupCode=A.ProcessGroupCode) 


COMMIT TRANSACTION
END TRY

BEGIN CATCH
	SELECT ERROR_MESSAGE()
		,ERROR_LINE()
		,ERROR_SEVERITY()

	ROLLBACK TRANSACTION
END CATCH
GO
