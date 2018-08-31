CREATE TABLE [dbo].[ClientKPIMap]
(
	[ClientKPIMapID] [int] IDENTITY(1,1) NOT NULL,
	[ClientID] [int] NOT NULL,
	[KPIID] [int] NOT NULL,
	[Client Standard] [nvarchar](500) NULL,
	[IsSLA] [BIT] NULL,
	[StartDate] [datetime] NOT NULL,
	[EndDate] [datetime] NOT NULL,
	[RecordStatus] [char](1) NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
	[CreatedBy] [nvarchar](60) NOT NULL,
	[ModifiedDate] [datetime] NOT NULL,
	[ModifiedBy] [nvarchar](60) NOT NULL,
	[StartTime] DATETIME2 GENERATED ALWAYS AS ROW START 
		HIDDEN DEFAULT GETUTCDATE(),
    [EndTime] DATETIME2 GENERATED ALWAYS AS ROW END
		HIDDEN DEFAULT CONVERT(DATETIME2, '9999-12-31 23:59:59.9999999'),
	PERIOD FOR SYSTEM_TIME (StartTime, EndTime),
	PRIMARY KEY CLUSTERED ([ClientKPIMapID] ASC),
	FOREIGN KEY ([ClientID]) REFERENCES [dbo].[Client] ([ClientID]),
	FOREIGN KEY ([KPIID]) REFERENCES [dbo].[KPI] ([KPIID])
)WITH (SYSTEM_VERSIONING = ON (HISTORY_TABLE = dbo.ClientKPIMap_History));
