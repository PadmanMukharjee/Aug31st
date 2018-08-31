CREATE TABLE [dbo].[M3metricClientKpiDaily]
(
	[ID]			INT IDENTITY (1, 1) NOT NULL,
	[ClientID]		INT					NOT NULL,
	[KpiID]			INT					NOT NULL,
	[AlertLevel]	NVARCHAR(500)		NULL,
	[ActualValue]	NVARCHAR(500)		NULL,
    [IsDeviated]	BIT					NOT NULL,
	[InsertedDate]	DATE				NOT NULL,
    [RecordStatus]	CHAR (1)			NOT NULL,
    [CreatedDate]	DATETIME			NOT NULL,
    [CreatedBy]		NVARCHAR (60)		NOT NULL,
    [ModifiedDate]	DATETIME			NOT NULL,
    [ModifiedBy]	NVARCHAR (60)		NOT NULL,
    PRIMARY KEY CLUSTERED ([ID] ASC),
	CONSTRAINT FK_M3MetricClientKpiDaily_KPI FOREIGN KEY (KpiID) REFERENCES [KPI](KPIID),
	CONSTRAINT FK_M3MetricClientKpiDaily_Client FOREIGN KEY (ClientID) REFERENCES [Client](ClientID)
)
