	CREATE TABLE [dbo].[Attribute]
	(
		[AttributeId] INT NOT NULL PRIMARY KEY IDENTITY(1,1),
		[AttributeCode] NVARCHAR(250) NOT NULL ,
		[AttributeName] NVARCHAR(250) NOT NULL, 
		[AttributeDescription] NVARCHAR(255) , 
		[AttributeType] NVARCHAR(250) NOT NULL,
		[ControlTypeId] INT,
		[RecordStatus] CHAR(1) NOT NULL , 
		[CreatedDate] DATETIME NOT NULL , 
		[CreatedBy] NVARCHAR(250) NOT NULL , 
		[ModifiedDate] DATETIME NOT NULL , 
		[ModifiedBy] NVARCHAR(250) NOT NULL, 
		CONSTRAINT [FK_Attribute_ControlType] FOREIGN KEY ([ControlTypeId]) REFERENCES ControlType([ControlTypeId])
	)
