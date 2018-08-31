CREATE TABLE [dbo].[BusinessUnit] (
    [BusinessUnitID]          INT            IDENTITY (1, 1) NOT NULL,
    [BusinessUnitCode]        NVARCHAR (255) NULL,
    [BusinessUnitName]        NVARCHAR (255) NOT NULL,
    [BusinessUnitDescription] NVARCHAR (255) NULL,
    [SiteID]                  INT            NULL,
    [RecordStatus]            CHAR (1)       NOT NULL,
    [CreatedDate]             DATETIME       NOT NULL,
    [CreatedBy]               NVARCHAR (60)  NOT NULL,
    [ModifiedDate]            DATETIME       NOT NULL,
    [ModifiedBy]              NVARCHAR (60)  NOT NULL,
    PRIMARY KEY CLUSTERED ([BusinessUnitID] ASC),
    CONSTRAINT [FK__BusinessU__SiteI__1EA48E88] FOREIGN KEY ([SiteID]) REFERENCES [dbo].[Site] ([SiteID])
);

