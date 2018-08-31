CREATE TABLE [dbo].[ClientCloseMonth] (
    [ClientCloseMonthID] INT           IDENTITY (1, 1) NOT NULL,
    [ClientID]           INT           NOT NULL,
    [MonthID]            INT           NOT NULL,
    [Year]               INT           NOT NULL,
    [MonthStatus]        NVARCHAR (60) NOT NULL,
    [ClosedDate]         DATETIME      NULL,
    [ReOpenDate]         DATETIME      NULL,
    [RecordStatus]       CHAR (1)      NOT NULL,
    [CreatedDate]        DATETIME      NOT NULL,
    [CreatedBy]          NVARCHAR (60) NOT NULL,
    [ModifiedDate]       DATETIME      NOT NULL,
    [ModifiedBy]         NVARCHAR (60) NOT NULL,
    PRIMARY KEY CLUSTERED ([ClientCloseMonthID] ASC),
    FOREIGN KEY ([ClientID]) REFERENCES [dbo].[Client] ([ClientID]),
    FOREIGN KEY ([MonthID]) REFERENCES [dbo].[Month] ([MonthID])
);

