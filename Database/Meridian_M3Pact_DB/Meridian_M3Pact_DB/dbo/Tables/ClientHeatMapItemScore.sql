CREATE TABLE [dbo].[ClientHeatMapItemScore]
(
	[ClientHeatMapItemScoreID]	INT				IDENTITY (1, 1) NOT NULL,
	[HeatMapItemID]				INT				NOT NULL,
	[ClientID]					INT				NOT NULL,
	[HeatMapItemDate]			DATE			NOT NULL,
	[Score]						INT				NULL,
	[RecordStatus]              CHAR (1)		NOT NULL,
	[CreatedDate]               DATETIME		NOT NULL,
	[CreatedBy]                 NVARCHAR (60)	NOT NULL,
	[ModifiedDate]              DATETIME		NOT NULL,
	[ModifiedBy]                NVARCHAR (60)	NOT NULL,
	PRIMARY KEY CLUSTERED ([ClientHeatMapItemScoreID] ASC),
	CONSTRAINT FK_ClientHeatMapItemScore_HeatMapItem FOREIGN KEY (HeatMapItemID) REFERENCES [HeatMapItem](HeatMapItemID),
	CONSTRAINT FK_ClientHeatMapItemScore_Client FOREIGN KEY (ClientID) REFERENCES [Client](ClientID)
)
