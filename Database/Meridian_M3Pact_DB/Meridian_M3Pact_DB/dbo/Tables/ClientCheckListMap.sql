CREATE TABLE [dbo].[ClientCheckListMap]
(
	[ClientCheckListMapID]  INT           IDENTITY (1, 1) NOT NULL,
	[ClientID]              INT            NOT NULL,
	[CheckListID]           INT            NOT NULL,
	[StartDate]             DATETIME       NOT NULL,
	[EndDate]               DATETIME       NOT NULL,
	[EffectiveDate]         DATETIME       NULL,
	[RecordStatus]          CHAR (1)       NOT NULL,
	[CreatedDate]           DATETIME       NOT NULL,
	[CreatedBy]             NVARCHAR (60)  NOT NULL,
	[ModifiedDate]          DATETIME       NOT NULL,
	[ModifiedBy]            NVARCHAR (60)  NOT NULL,
	PRIMARY KEY CLUSTERED ([ClientCheckListMapID] ASC),
	CONSTRAINT FK_ClientCheckListMap_Client FOREIGN KEY (ClientID) REFERENCES Client(ClientID),
	CONSTRAINT FK_ClientCheckListMap_CheckList FOREIGN KEY (CheckListID) REFERENCES CheckList(CheckListID)
)
