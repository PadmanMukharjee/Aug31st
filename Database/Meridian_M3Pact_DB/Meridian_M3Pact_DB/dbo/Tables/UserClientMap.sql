CREATE TABLE [dbo].[UserClientMap] (
    [UserClientMapID] INT           IDENTITY (1, 1) NOT NULL,
    [UserID]          INT           NOT NULL,
    [ClientID]        INT           NOT NULL,
    [RecordStatus]    CHAR (1)      DEFAULT ('A') NULL,
    [CreatedBy]       NVARCHAR (50) NOT NULL,
    [CreatedDate]     DATETIME      NOT NULL,
    [ModifiedBy]      NVARCHAR (50) NOT NULL,
    [ModifiedDate]    DATETIME      NOT NULL,
	[StartTime]		  DATETIME2 GENERATED ALWAYS AS ROW START 
		HIDDEN DEFAULT GETUTCDATE(),
    [EndTime]		  DATETIME2 GENERATED ALWAYS AS ROW END
		HIDDEN DEFAULT CONVERT(DATETIME2, '9999-12-31 23:59:59.9999999'),
	PERIOD FOR SYSTEM_TIME (StartTime, EndTime),
    CONSTRAINT [PK_UserClient_ID] PRIMARY KEY CLUSTERED ([UserClientMapID] ASC),
    FOREIGN KEY ([UserID]) REFERENCES [dbo].[UserLogin] ([ID]),
    CONSTRAINT [FK_UsersClinetMap_Client] FOREIGN KEY ([ClientID]) REFERENCES [dbo].[Client] ([ClientID])
)WITH (SYSTEM_VERSIONING = ON (HISTORY_TABLE = dbo.UserClientMap_History));

