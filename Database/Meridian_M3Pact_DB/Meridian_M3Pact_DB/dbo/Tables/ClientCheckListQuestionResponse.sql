CREATE TABLE [dbo].[ClientCheckListQuestionResponse]
(
	[ClientCheckListQuestionResponseID]  INT            IDENTITY (1, 1) NOT NULL,
	[ClientCheckListMapID]               INT            NOT NULL,
	[CheckListAttributeMapID]            INT			NOT NULL,
	[ExpectedResponse]                   BIT			NULL,
	[FreeFormResponse]                   NVARCHAR (500) NULL,
	[ClientCheckListStatusDetailID]      INT			NOT NULL,
	[RecordStatus]                       CHAR (1)       NOT NULL,
	[CreatedDate]                        DATETIME       NOT NULL,
	[CreatedBy]                          NVARCHAR (60)  NOT NULL,
	[ModifiedDate]                       DATETIME       NOT NULL,
	[ModifiedBy]                         NVARCHAR (60)  NOT NULL,
	PRIMARY KEY CLUSTERED ([ClientCheckListQuestionResponseID] ASC),
	CONSTRAINT FK_ClientCheckListQuestionResponse_ClientCheckListMap FOREIGN KEY (ClientCheckListMapID) REFERENCES [ClientCheckListMap](ClientCheckListMapID),
	CONSTRAINT FK_ClientCheckListQuestionResponse_CheckListAttributeMap FOREIGN KEY (CheckListAttributeMapID) REFERENCES CheckListAttributeMap(CheckListAttributeMapID), 
	CONSTRAINT FK_ClientCheckListQuestionResponse_ClientCheckListStatusDetail FOREIGN KEY (ClientCheckListStatusDetailID) REFERENCES ClientCheckListStatusDetail(ClientCheckListStatusDetailID)
)
