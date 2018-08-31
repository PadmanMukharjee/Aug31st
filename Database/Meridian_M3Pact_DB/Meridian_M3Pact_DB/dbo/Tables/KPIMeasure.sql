CREATE TABLE [dbo].[KPIMeasure]
(
[KPIMeasureID]     INT   IDENTITY (1, 1) NOT NULL,
[CheckListTypeID]  INT   NOT NULL,
[Measure]          NVARCHAR (100) NULL,
[RecordStatus]     CHAR (1)       NOT NULL,
[CreatedDate]      DATETIME       NOT NULL,
[CreatedBy]        NVARCHAR (60)  NOT NULL,
[ModifiedDate]     DATETIME       NOT NULL,
[ModifiedBy]       NVARCHAR (60)  NOT NULL,
PRIMARY KEY CLUSTERED ([KPIMeasureID] ASC),
CONSTRAINT FK_KPIMeasure_CHECKLISTTYPEID  FOREIGN KEY (CheckListTypeID) REFERENCES [CheckListType](CheckListTypeID),
)
