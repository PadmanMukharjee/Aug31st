CREATE TABLE [dbo].[Roles] (
    [RoleID]       INT           IDENTITY (1, 1) NOT NULL,
    [RoleCode]     VARCHAR (100) NOT NULL,
    [RoleDesc]     VARCHAR (100) NULL,
    [RecordStatus] CHAR (1)      DEFAULT ('A') NULL,
    [Level]        INT           NULL,
    [CreatedBy]    NVARCHAR (50) NOT NULL,
    [CreatedDate]  DATETIME      NOT NULL,
    [ModifiedBy]   NVARCHAR (50) NOT NULL,
    [ModifiedDate] DATETIME      NOT NULL,
    CONSTRAINT [PK_Role_RoleID] PRIMARY KEY CLUSTERED ([RoleID] ASC)
);



