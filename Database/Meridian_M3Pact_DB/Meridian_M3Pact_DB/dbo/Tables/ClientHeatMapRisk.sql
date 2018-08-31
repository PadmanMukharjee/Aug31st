CREATE TABLE [dbo].[ClientHeatMapRisk]
(
	[ClientHeatMapRiskID]		INT				IDENTITY (1, 1) NOT NULL,
	[ClientID]					INT				NOT NULL,
	[M3DailyDate]				DATE			NULL,
	[M3WeeklyDate]				DATE			NULL,
	[M3MonthlyDate]				DATE			NULL,
	[ChecklistWeeklyDate]		DATE			NULL,
	[ChecklistMonthlyDate]		DATE			NULL,
	[RiskScore]					INT				NULL,
	[EffectiveTime]				DATETIME		NULL,
	[RecordStatus]				CHAR (1)		NOT NULL,
	[CreatedDate]				DATETIME		NOT NULL,
	[CreatedBy]					NVARCHAR (60)	NOT NULL,
	[ModifiedDate]				DATETIME		NOT NULL,
	[ModifiedBy]				NVARCHAR (60)	NOT NULL,
	PRIMARY KEY CLUSTERED ([ClientHeatMapRiskID] ASC),
	CONSTRAINT FK_ClientHeatMapRisk_Client FOREIGN KEY (ClientID) REFERENCES [Client](ClientID),
	CONSTRAINT [UQ_ClientHeatMapRisk_Dates] UNIQUE NONCLUSTERED
    (
        [ClientID],[M3DailyDate], [M3WeeklyDate], [M3MonthlyDate], [ChecklistWeeklyDate], [ChecklistMonthlyDate]
    )
)
