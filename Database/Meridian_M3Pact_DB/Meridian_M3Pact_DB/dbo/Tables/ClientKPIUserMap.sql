CREATE TABLE [dbo].[ClientKPIUserMap]
(
	[ClientKPIUserMapID] [int] IDENTITY(1,1) NOT NULL,
	[ClientKPIMapID] [int] NOT NULL,
	[UserId] [int] NOT NULL,
	[RecordStatus] [char](1) NOT NULL,
	[CreatedDate] [datetime] NOT NULL,
	[CreatedBy] [nvarchar](60) NOT NULL,
	[ModifiedDate] [datetime] NOT NULL,
	[ModifiedBy] [nvarchar](60) NOT NULL
  PRIMARY KEY CLUSTERED ([ClientKPIUserMapID] ASC),
  FOREIGN KEY ([ClientKPIMapID]) REFERENCES [dbo].[ClientKPIMap] ([ClientKPIMapID]),
  FOREIGN KEY ([UserId]) REFERENCES [dbo].[UserLogin] ([ID])
)
