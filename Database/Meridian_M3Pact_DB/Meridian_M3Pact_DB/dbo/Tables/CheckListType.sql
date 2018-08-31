CREATE TABLE [dbo].[CheckListType] (
    [CheckListTypeID]   INT           IDENTITY (1, 1) NOT NULL,
    [CheckListTypeCode] NVARCHAR (60) NULL,
    [CheckListTypeName] NVARCHAR (60) NOT NULL,
    [RecordStatus]      CHAR (1)      NOT NULL,
    [CreatedDate]       DATETIME      NOT NULL,
    [CreatedBy]         NVARCHAR (60) NOT NULL,
    [ModifiedDate]      DATETIME      NOT NULL,
    [ModifiedBy]        NVARCHAR (60) NOT NULL,
    PRIMARY KEY CLUSTERED ([CheckListTypeID] ASC)
);

