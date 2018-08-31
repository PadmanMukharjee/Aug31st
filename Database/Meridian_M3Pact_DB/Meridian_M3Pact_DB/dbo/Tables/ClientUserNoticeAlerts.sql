CREATE TABLE [dbo].[ClientUserNoticeAlerts]
(
     [ClientUserNoticeAlertId] INT IDENTITY(1,1)    NOT NULL,
     [ClientId]                INT                  NOT NULL,
     [UserLoginId]             INT                  NOT NULL,
     [RecordStatus]            CHAR(1)              NOT NULL,
     [CreatedBy]               NVARCHAR(50)         NOT NULL,
     [CreatedDate]             DATETIME             NOT NULL,
     [ModifiedBy]              NVARCHAR(50)         NOT NULL,
     [ModifiedDate]            DATETIME             NOT NULL
	 CONSTRAINT [PK_ClientUserNoticeAlerts] PRIMARY KEY ([ClientUserNoticeAlertId])
     CONSTRAINT FK_ClientUserNoticeAlerts_Client FOREIGN KEY ([ClientId]) REFERENCES Client([ClientID]),
     CONSTRAINT FK_ClientUserNoticeAlerts_UserLogin FOREIGN KEY ([UserLoginId]) REFERENCES UserLogin([ID]),
)
