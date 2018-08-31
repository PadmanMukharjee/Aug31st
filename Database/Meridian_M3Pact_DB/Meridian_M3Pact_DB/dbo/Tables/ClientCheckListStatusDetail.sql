CREATE TABLE [dbo].[ClientCheckListStatusDetail]
(
	[ClientCheckListStatusDetailID] INT				IDENTITY (1, 1) NOT NULL,
	[ClientCheckListMapID]			INT				NOT NULL,
	[CheckListEffectiveDate]		DATETIME		NOT NULL,
	[ChecklistStatus]				CHAR (1)		NOT NULL,
	[RecordStatus]					CHAR (1)		NOT NULL,
	[CreatedDate]					DATETIME		NOT NULL,
	[CreatedBy]						NVARCHAR (60)	NOT NULL,
	[ModifiedDate]					DATETIME		NOT NULL,
	[ModifiedBy]					NVARCHAR (60)	NOT NULL,
	PRIMARY KEY CLUSTERED ([ClientCheckListStatusDetailID] ASC),
	CONSTRAINT FK_ClientCheckListStatusDetail_ClientCheckListMap FOREIGN KEY (ClientCheckListMapID) REFERENCES [ClientCheckListMap](ClientCheckListMapID)
)
