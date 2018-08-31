CREATE TABLE [dbo].[CheckListQuestionMap] (
    [CheckListQuestionMapID] INT            IDENTITY (1, 1) NOT NULL,
    [CheckListID]            INT            NULL,
    [QuestionID]             INT            NULL,
    [QuestionResponse]       NVARCHAR (60)  NULL,
    [FreeFormText]           NVARCHAR (100) NULL,
    [RecordStatus]           CHAR (1)       NOT NULL,
    [CreatedDate]            DATETIME       NOT NULL,
    [CreatedBy]              NVARCHAR (60)  NOT NULL,
    [ModifiedDate]           DATETIME       NOT NULL,
    [ModifiedBy]             NVARCHAR (60)  NOT NULL,
    PRIMARY KEY CLUSTERED ([CheckListQuestionMapID] ASC),
    CONSTRAINT [FK__CheckList__Check__2180FB33] FOREIGN KEY ([CheckListID]) REFERENCES [dbo].[CheckList] ([CheckListID]),
    CONSTRAINT [FK__CheckList__Quest__22751F6C] FOREIGN KEY ([QuestionID]) REFERENCES [dbo].[Question] ([QuestionID])
);

