CREATE TABLE [dbo].[ClientConfigStepStatus] (
    [ClientConfigStepStatusID]          INT            IDENTITY (1, 1) NOT NULL,
    [ClientConfigStepStatusName]        NVARCHAR (255) NOT NULL,
    [ClientConfigStepStatusDescription] NVARCHAR (255) NOT NULL,
    [RecordStatus]                      CHAR (1)       NOT NULL,
    [CreatedDate]                       DATETIME       NOT NULL,
    [CreatedBy]                         NVARCHAR (60)  NOT NULL,
    [ModifiedDate]                      DATETIME       NOT NULL,
    [ModifiedBy]                        NVARCHAR (60)  NOT NULL,
    PRIMARY KEY CLUSTERED ([ClientConfigStepStatusID] ASC)
);

