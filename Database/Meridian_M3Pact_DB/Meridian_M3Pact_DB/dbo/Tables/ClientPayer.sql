CREATE TABLE [dbo].[ClientPayer] (
    [ClientPayerID] INT           IDENTITY (1, 1) NOT NULL,
    [ClientID]      INT           NULL,
    [PayerID]       INT           NULL,
    [IsM3FeeExempt] BIT           NULL,
	[StartDate]		DATE		  NULL,
	[EndDate]		DATE		  NULL,
    [RecordStatus]  CHAR (1)      NOT NULL,
    [CreatedDate]   DATETIME      NOT NULL,
    [CreatedBy]     NVARCHAR (60) NOT NULL,
    [ModifiedDate]  DATETIME      NOT NULL,
    [ModifiedBy]    NVARCHAR (60) NOT NULL,
	[StartTime]		DATETIME2 GENERATED ALWAYS AS ROW START 
		HIDDEN DEFAULT GETUTCDATE(),
    [EndTime]		DATETIME2 GENERATED ALWAYS AS ROW END
		HIDDEN DEFAULT CONVERT(DATETIME2, '9999-12-31 23:59:59.9999999'),
	PERIOD FOR SYSTEM_TIME (StartTime, EndTime),
    PRIMARY KEY CLUSTERED ([ClientPayerID] ASC),
    CONSTRAINT [FK__ClientPay__Clien__282DF8C2] FOREIGN KEY ([ClientID]) REFERENCES [dbo].[Client] ([ClientID]),
    CONSTRAINT [FK__ClientPay__Payer__29221CFB] FOREIGN KEY ([PayerID]) REFERENCES [dbo].[Payer] ([PayerID])
)WITH (SYSTEM_VERSIONING = ON (HISTORY_TABLE = dbo.ClientPayer_History));

