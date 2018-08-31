CREATE TABLE [dbo].[Employee] (
    [ID]           INT            IDENTITY (1, 1) NOT NULL,
    [EmployeeID]   INT            NOT NULL,
    [FirstName]    NVARCHAR (100) NULL,
    [LastName]     NVARCHAR (100) NULL,
    [MobileNumber] NVARCHAR (50)  NULL,
    [Email]        NVARCHAR (50)  NOT NULL,
    [Title]        NVARCHAR (50)  NULL,
	[SSO]		   INT			  NULL,
    [BusinessUnit] NVARCHAR (50)  NULL,
    [ReportsTo]    NVARCHAR (100) NULL,
    [Site]         NVARCHAR (100) NULL,
    [Role]         NVARCHAR (100) NULL,
    [RecordStatus] CHAR (1)       CONSTRAINT [DF__Users__RecordSta__0EF836A4] DEFAULT ('A') NULL,
    [CreatedBy]    NVARCHAR (50)  NOT NULL,
    [CreatedDate]  DATETIME       NOT NULL,
    [ModifiedBy]   NVARCHAR (50)  NOT NULL,
    [ModifiedDate] DATETIME       NOT NULL,
    CONSTRAINT [PK_Users_ID] PRIMARY KEY CLUSTERED ([ID] ASC)
);

