CREATE TABLE [dbo].[ClientConfig]
(
	[ClientConfigId] INT NOT NULL PRIMARY KEY IDENTITY(1,1),
    [ClientId] INT NOT NULL,
    [AttributeId] INT NOT NULL,
    [AttributeValue] NVARCHAR(250), 
    [RecordStatus] char(1) NOT NULL , 
    [CreatedDate] DATETIME NOT NULL , 
    [CreatedBy] NVARCHAR(250) NOT NULL , 
    [ModifiedDate] DATETIME NOT NULL , 
    [ModifiedBy] NVARCHAR(250) NOT NULL

	Constraint FK_ClientId_ClientConfig FOREIGN KEY (ClientId) REFERENCES [dbo].Client(ClientId),
    Constraint FK_AttributeId_ClientConfig FOREIGN KEY (AttributeId) REFERENCES Attribute(AttributeId), 
)
