CREATE TABLE [dbo].[ClientConfigStep] (
    [ClientConfigStepID]          INT            IDENTITY (1, 1) NOT NULL,
    [ClientConfigStepName]        NVARCHAR (255) NOT NULL,
    [ClientConfigStepDescription] NVARCHAR (255) NOT NULL,
    [ScreenCode]				  VARCHAR (50)   NULL,
    [DisplayOrder]				  INT			 NULL,
    [RecordStatus]                CHAR (1)       NOT NULL,
    [CreatedDate]                 DATETIME       NOT NULL,
    [CreatedBy]                   NVARCHAR (60)  NOT NULL,
    [ModifiedDate]                DATETIME       NOT NULL,
    [ModifiedBy]                  NVARCHAR (60)  NOT NULL,
    PRIMARY KEY CLUSTERED ([ClientConfigStepID] ASC),
	FOREIGN KEY ([ScreenCode]) REFERENCES [dbo].[Screen] ([ScreenCode])
);

