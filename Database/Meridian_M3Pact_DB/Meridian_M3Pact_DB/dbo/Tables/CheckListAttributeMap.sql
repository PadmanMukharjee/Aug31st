CREATE TABLE [dbo].[CheckListAttributeMap]
(
	[CheckListAttributeMapID]      INT            IDENTITY (1, 1) NOT NULL,
	[CheckListID]                  INT            NOT NULL,
	[CheckListAttributeID]         INT            NOT NULL,
	[CheckListAttributeValueID]    NVARCHAR(50)   NOT NULL,
	[StartDate]                    DATETIME       NOT NULL,
	[EndDate]                      DATETIME       NOT NULL,
	[EffectiveDate]                DATETIME       NULL,
	[RecordStatus]                 CHAR (1)       NOT NULL,
	[CreatedDate]                  DATETIME       NOT NULL,
	[CreatedBy]                    NVARCHAR (60)  NOT NULL,
	[ModifiedDate]                 DATETIME       NOT NULL,
	[ModifiedBy]                   NVARCHAR (60)  NOT NULL,
	PRIMARY KEY CLUSTERED ([CheckListAttributeMapID] ASC),
	CONSTRAINT FK_CheckListAttributeMap_CheckList  FOREIGN KEY (CheckListID) REFERENCES CheckList(CheckListID), 
	CONSTRAINT FK_CheckListAttributeMap_CheckListAttribute  FOREIGN KEY (CheckListAttributeID) REFERENCES CheckListAttribute(CheckListAttributeID)
)
