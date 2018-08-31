CREATE TABLE [dbo].[AdminConfigValues]
(	
	[AdminConfigId] INT NOT NULL PRIMARY KEY IDENTITY(1,1),  
    [AttributeId] INT NOT NULL,
    [AttributeValue] NVARCHAR(250), 
    [RecordStatus] char(1) NOT NULL , 
    [CreatedDate] DATETIME NOT NULL , 
    [CreatedBy] NVARCHAR(250) NOT NULL , 
    [ModifiedDate] DATETIME NOT NULL , 
    [ModifiedBy] NVARCHAR(250) NOT NULL
	
    Constraint FK_Attribute_AdminConfigValues FOREIGN KEY (AttributeId) REFERENCES Attribute(AttributeId), 
)
