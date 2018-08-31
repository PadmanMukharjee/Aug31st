CREATE TABLE [dbo].[JobStatus]
(	
JobStatusId INT PRIMARY KEY IDENTITY,
JobStatusCode VARCHAR(20) NOT NULL ,
JobStatusName VARCHAR(100) NOT NULL,
RecordStatus CHAR(1) NOT NULL,
CreatedDate DateTime NOT NULL,
CreatedBy NVARCHAR(100) NOT NULL,
ModifiedDate DateTime NOT NULL,
ModifiedBy NVARCHAR(100) NOT NULL
)

