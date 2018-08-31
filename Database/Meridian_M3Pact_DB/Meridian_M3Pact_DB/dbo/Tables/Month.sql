CREATE TABLE [dbo].[Month] (
    [MonthID]      INT           IDENTITY (1, 1) NOT NULL,
    [MonthCode]    NVARCHAR (60) NOT NULL,
    [MonthName]    NVARCHAR (60) NOT NULL,
    [RecordStatus] CHAR (1)      NOT NULL,
    [CreatedDate]  DATETIME      NOT NULL,
    [CreatedBy]    NVARCHAR (60) NOT NULL,
    [ModifiedDate] DATETIME      NOT NULL,
    [ModifiedBy]   NVARCHAR (60) NOT NULL,
    PRIMARY KEY CLUSTERED ([MonthID] ASC)
);

