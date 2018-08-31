CREATE TABLE [dbo].[ApiResource] (
    [ID]          INT             IDENTITY (1, 1) NOT NULL,
    [Name]        NVARCHAR (200)  NOT NULL,
    [Description] NVARCHAR (1000) NULL,
    [Enabled]     BIT             NOT NULL,
    CONSTRAINT [PK_ApiResource] PRIMARY KEY CLUSTERED ([ID] ASC)
);

