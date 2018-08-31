CREATE TABLE [dbo].[Log] (
    [Id]         INT            IDENTITY (1, 1) NOT NULL,
    [Exception]  NVARCHAR (MAX) NULL,
    [LogLevel]   NVARCHAR (50)  NULL,
    [Message]    NVARCHAR (MAX) NULL,
    [StackTrace] NVARCHAR (MAX) NULL,
    [Time]       DATETIME       NOT NULL,
    CONSTRAINT [PK_Log] PRIMARY KEY CLUSTERED ([Id] ASC)
);

