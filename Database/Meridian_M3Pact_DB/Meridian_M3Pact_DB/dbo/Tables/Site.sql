CREATE TABLE [dbo].[Site] (
    [SiteID]          INT            IDENTITY (1, 1) NOT NULL,
    [SiteCode]        NVARCHAR (255) NULL,
    [SiteName]        NVARCHAR (255) NOT NULL,
    [SiteDescription] NVARCHAR (255) NULL,
    [RecordStatus]    CHAR (1)       NOT NULL,
    [CreatedDate]     DATETIME       NOT NULL,
    [CreatedBy]       NVARCHAR (60)  NOT NULL,
    [ModifiedDate]    DATETIME       NOT NULL,
    [ModifiedBy]      NVARCHAR (60)  NOT NULL,
    PRIMARY KEY CLUSTERED ([SiteID] ASC)
);

