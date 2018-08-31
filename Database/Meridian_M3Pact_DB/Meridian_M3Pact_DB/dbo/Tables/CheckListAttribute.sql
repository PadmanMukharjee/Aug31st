CREATE TABLE [dbo].[CheckListAttribute]
(
	[CheckListAttributeID]  INT            IDENTITY (1, 1) NOT NULL,
	[AttributeCode]         NVARCHAR (60)  NOT NULL,
	[AttributeName]         NVARCHAR (100) NOT NULL,
	[RecordStatus]          CHAR (1)       NOT NULL,
	[CreatedDate]           DATETIME       NOT NULL,
	[CreatedBy]             NVARCHAR (60)  NOT NULL,
	[ModifiedDate]          DATETIME       NOT NULL,
	[ModifiedBy]            NVARCHAR (60)  NOT NULL,
	PRIMARY KEY CLUSTERED ([CheckListAttributeID] ASC),
	CONSTRAINT UQ_CheckListAttribute_AttributeCode UNIQUE (AttributeCode)
)
