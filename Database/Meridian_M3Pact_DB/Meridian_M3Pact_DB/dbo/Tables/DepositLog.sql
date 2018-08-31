CREATE TABLE [dbo].[DepositLog] (
    [DepositLogID]  INT             IDENTITY (1, 1) NOT NULL,
    [ClientPayerID] INT             NULL,
    [DepositDateID] INT             NULL,
    [Amount]        NUMERIC (10, 2) NULL,
    [IsM3FeeExempt] BIT             NULL,
    [RecordStatus]  CHAR (1)        NOT NULL,
    [CreatedDate]   DATETIME        NOT NULL,
    [CreatedBy]     NVARCHAR (60)   NOT NULL,
    [ModifiedDate]  DATETIME        NOT NULL,
    [ModifiedBy]    NVARCHAR (60)   NOT NULL,
    PRIMARY KEY CLUSTERED ([DepositLogID] ASC),
    CONSTRAINT [FK__DepositLo__Clien__69FBBC1F] FOREIGN KEY ([ClientPayerID]) REFERENCES [dbo].[ClientPayer] ([ClientPayerID]),
    CONSTRAINT [FK__DepositLo__Depos__6AEFE058] FOREIGN KEY ([DepositDateID]) REFERENCES [dbo].[DateDimension] ([DateKey])
);

