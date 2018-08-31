CREATE TABLE [dbo].[BusinessDays] (
    [BusinessDaysID] INT           IDENTITY (1, 1) NOT NULL,
    [Year]           INT           NOT NULL,
    [MonthID]        INT           NULL,
    [BusinessDays]   INT           NOT NULL,
    [RecordStatus]   CHAR (1)      NOT NULL,
    [CreatedDate]    DATETIME      NOT NULL,
    [CreatedBy]      NVARCHAR (60) NOT NULL,
    [ModifiedDate]   DATETIME      NOT NULL,
    [ModifiedBy]     NVARCHAR (60) NOT NULL,
    PRIMARY KEY CLUSTERED ([BusinessDaysID] ASC),
    CONSTRAINT [FK__BusinessD__Month__498EEC8D] FOREIGN KEY ([MonthID]) REFERENCES [dbo].[Month] ([MonthID])
);

