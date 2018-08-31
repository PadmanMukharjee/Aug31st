CREATE TABLE [dbo].[DeviatedClientKPI]
(
DeviatedClientKPIId INT  NOT NULL PRIMARY KEY IDENTITY,
SubmittedDate DateTime NULL,
CheckListDate DATE NOT NULL,
ChecklistTypeId INT NOT NULL,
ClientId INT NOT NULL,
QuestionCode VARCHAR(20),
RecordStatus char NOT NULL,
ExpectedResponse NVARCHAR(50) NULL ,
ActualResponse NVARCHAR(50)  NULL
CONSTRAINT [FK_DeviatedClientKPI_ChecklistType ] FOREIGN KEY (ChecklistTypeId) REFERENCES ChecklistType(ChecklistTypeId), 
CONSTRAINT [FK_DeviatedClientKPI_Client] FOREIGN KEY (ClientId) REFERENCES Client(ClientId) ,
)
