CREATE TABLE [dbo].[ClientConfigStepDetail] (
    [ClientConfigStepDetailID] INT           IDENTITY (1, 1) NOT NULL,
    [ClientID]                 INT           NOT NULL,
    [ClientConfigStepID]       INT           NOT NULL,
    [ClientConfigStepStatusID] INT           NOT NULL,
    [RecordStatus]             CHAR (1)      NOT NULL,
    [CreatedDate]              DATETIME      NOT NULL,
    [CreatedBy]                NVARCHAR (60) NOT NULL,
    [ModifiedDate]             DATETIME      NOT NULL,
    [ModifiedBy]               NVARCHAR (60) NOT NULL,
    PRIMARY KEY CLUSTERED ([ClientConfigStepDetailID] ASC),
    FOREIGN KEY ([ClientConfigStepID]) REFERENCES [dbo].[ClientConfigStep] ([ClientConfigStepID]),
    FOREIGN KEY ([ClientConfigStepStatusID]) REFERENCES [dbo].[ClientConfigStepStatus] ([ClientConfigStepStatusID]),
    FOREIGN KEY ([ClientID]) REFERENCES [dbo].[Client] ([ClientID])
);

