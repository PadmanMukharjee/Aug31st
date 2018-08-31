CREATE TABLE [dbo].[JobProcessGroup]
(
JobProcessGroupId INT PRIMARY KEY IDENTITY,
ProcessGroupCode VARCHAR(30) NOT NULL,
ProcessGroupName VARCHAR(100) NOT NULL,
RecordStatus CHAR(1) NOT NULL,
CreatedDate Datetime NOT NULL,
CreatedBy NVARCHAR(100) NOT NULL,
ModifiedDate DateTime NOT NULL,
ModifiedBy NVARCHAR(100) NOT NULL
)
