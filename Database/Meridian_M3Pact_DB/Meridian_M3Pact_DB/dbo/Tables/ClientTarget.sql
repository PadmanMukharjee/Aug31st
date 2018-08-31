CREATE TABLE [dbo].[ClientTarget] (
    [ClientTargetID]      INT            IDENTITY (1, 1) NOT NULL,
    [ClientID]            INT            NULL,
    [MonthID]             INT            NULL,
    [CalendarYear]        INT            NULL,
    [IsManualEntry]       BIT            NULL,
    [AnnualCharges]       BIGINT         NULL,
    [GrossCollectionRate] NUMERIC (5, 2) NULL,
    [Charges]             BIGINT         NULL,
    [Payments]            BIGINT         NULL,
    [Revenue]             BIGINT         NULL,
    [RecordStatus]        CHAR (1)       NOT NULL,
    [CreatedDate]         DATETIME       NOT NULL,
    [CreatedBy]           NVARCHAR (60)  NOT NULL,
    [ModifiedDate]        DATETIME       NOT NULL,
    [ModifiedBy]          NVARCHAR (60)  NOT NULL,
	[StartTime]			  DATETIME2 GENERATED ALWAYS AS ROW START 
		HIDDEN DEFAULT GETUTCDATE(),
    [EndTime]			  DATETIME2 GENERATED ALWAYS AS ROW END
		HIDDEN DEFAULT CONVERT(DATETIME2, '9999-12-31 23:59:59.9999999'),
	PERIOD FOR SYSTEM_TIME (StartTime, EndTime),
    PRIMARY KEY CLUSTERED ([ClientTargetID] ASC),
    CONSTRAINT [FK__ClientTar__Clien__3F115E1A] FOREIGN KEY ([ClientID]) REFERENCES [dbo].[Client] ([ClientID]),
    CONSTRAINT [FK__ClientTar__Month__40058253] FOREIGN KEY ([MonthID]) REFERENCES [dbo].[Month] ([MonthID])
)WITH (SYSTEM_VERSIONING = ON (HISTORY_TABLE = dbo.ClientTarget_History));

