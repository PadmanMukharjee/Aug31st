CREATE TABLE [dbo].[CheckList] (
    [CheckListID]     INT           IDENTITY (1, 1) NOT NULL,
    [CheckListTypeID] INT           NOT NULL,
    [CheckListName]   NVARCHAR (60) NOT NULL,
    [RecordStatus]    CHAR (1)      NOT NULL,
    [CreatedDate]     DATETIME      NOT NULL,
    [CreatedBy]       NVARCHAR (60) NOT NULL,
    [ModifiedDate]    DATETIME      NOT NULL,
    [ModifiedBy]      NVARCHAR (60) NOT NULL,
    PRIMARY KEY CLUSTERED ([CheckListID] ASC),
    CONSTRAINT [FK_CheckList_CheckListType] FOREIGN KEY ([CheckListTypeID]) REFERENCES [dbo].[CheckListType] ([CheckListTypeID])
);

