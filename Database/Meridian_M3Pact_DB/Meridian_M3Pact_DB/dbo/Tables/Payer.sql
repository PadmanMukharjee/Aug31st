CREATE TABLE [dbo].[Payer] (
    [PayerID]          INT            IDENTITY (1, 1) NOT NULL,
    [PayerCode]        NVARCHAR (255) NULL,
    [PayerName]        NVARCHAR (255) NOT NULL,
    [PayerDescription] NVARCHAR (255) NULL,
	[StartDate]		   DATE		  NULL,
	[EndDate]		   DATE		  NULL,
    [RecordStatus]     CHAR (1)       NOT NULL,
    [CreatedDate]      DATETIME       NOT NULL,
    [CreatedBy]        NVARCHAR (60)  NOT NULL,
    [ModifiedDate]     DATETIME       NOT NULL,
    [ModifiedBy]       NVARCHAR (60)  NOT NULL,
    PRIMARY KEY CLUSTERED ([PayerID] ASC)
);

