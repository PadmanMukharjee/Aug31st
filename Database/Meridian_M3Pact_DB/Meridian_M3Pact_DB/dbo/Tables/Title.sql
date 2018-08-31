CREATE TABLE [dbo].[Title] (
    [TitleID]          INT            IDENTITY (1, 1) NOT NULL,
    [TitleCode]        NVARCHAR (255) NULL,
    [TitleName]        NVARCHAR (255) NOT NULL,
    [TitleDescription] NVARCHAR (255) NULL,
    [RecordStatus]     CHAR (1)       NOT NULL,
    [CreatedDate]      DATETIME       NOT NULL,
    [CreatedBy]        NVARCHAR (60)  NOT NULL,
    [ModifiedDate]     DATETIME       NOT NULL,
    [ModifiedBy]       NVARCHAR (60)  NOT NULL,
    PRIMARY KEY CLUSTERED ([TitleID] ASC)
);

