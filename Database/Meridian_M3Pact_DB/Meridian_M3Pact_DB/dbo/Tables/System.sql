CREATE TABLE [dbo].[System] (
    [SystemID]          INT            IDENTITY (1, 1) NOT NULL,
    [SystemCode]        NVARCHAR (255) NULL,
    [SystemName]        NVARCHAR (255) NOT NULL,
    [SystemDescription] NVARCHAR (255) NULL,
    [RecordStatus]      CHAR (1)       NOT NULL,
    [CreatedDate]       DATETIME       NOT NULL,
    [CreatedBy]         NVARCHAR (60)  NOT NULL,
    [ModifiedDate]      DATETIME       NOT NULL,
    [ModifiedBy]        NVARCHAR (60)  NOT NULL,
    PRIMARY KEY CLUSTERED ([SystemID] ASC)
);

