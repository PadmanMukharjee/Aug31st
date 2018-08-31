CREATE TABLE [dbo].[JobRun]
(
JobRunId INT PRIMARY KEY IDENTITY,
JobProcessGroupId INT NOT NULL,
StartTime DateTime NOT NULL,
EndTime DateTime NOT NULL,
JobStatusId INT NOT NULL,
RecordStatus CHAR(1) NOT NULL,
CreatedDate DateTime NOT NULL,
CreatedBy NVARCHAR(100) NOT NULL,
ModifiedDate DateTime NOT NULL,
ModifiedBy NVARCHAR(100) NOT NULL, 
CONSTRAINT [FK_JobRun_JobStatus] FOREIGN KEY (JobStatusId) REFERENCES JobStatus(JobStatusId), 
    CONSTRAINT [FK_JobRun_JobProcessGroup] FOREIGN KEY (JobProcessGroupId) REFERENCES JobProcessGroup(JobProcessGroupId)
)
