CREATE TABLE [dbo].[Question] (
    [QuestionID]		INT				IDENTITY (1, 1) NOT NULL,
	[QuestionCode]      NVARCHAR(50),
    [QuestionText]		NVARCHAR (750)	NOT NULL,
	[ExpectedRespone]	BIT				NOT NULL,
	[IsKPI]				BIT				NOT NULL,
	[IsUniversal]		BIT				NOT NULL,
	[RequireFreeform]	BIT				NOT NULL,
	[CheckListTypeId]   INT             NOT NULL  DEFAULT 1,
    [RecordStatus]		CHAR (1)		NOT NULL,
	[StartDate]         DATETIME        NOT NULL  DEFAULT '1900-01-01 00:00:00.000',
	[EndDate]           DATETIME        NOT NULL  DEFAULT '9900-12-31 00:00:00.000',
	[EffectiveDate]     DATETIME		NOT NULL  DEFAULT '1900-01-01 00:00:00.000',
    [CreatedDate]		DATETIME		NOT NULL,
    [CreatedBy]			NVARCHAR (60)	NOT NULL,
    [ModifiedDate]		DATETIME		NOT NULL,
    [ModifiedBy]		NVARCHAR (60)	NOT NULL,
    PRIMARY KEY CLUSTERED ([QuestionID] ASC),
	CONSTRAINT FK_Question_CheckListType FOREIGN KEY (CheckListTypeId) REFERENCES CheckListType(CheckListTypeId) 
);



