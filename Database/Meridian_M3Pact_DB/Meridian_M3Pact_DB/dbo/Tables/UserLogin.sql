CREATE TABLE [dbo].[UserLogin] (
    [ID]                          INT            IDENTITY (1, 1) NOT NULL,
    [UserID]                      NVARCHAR (255) NOT NULL,
    [UserName]                    NVARCHAR (255) NOT NULL,
    [FirstName]                   NVARCHAR (255) NOT NULL,
    [MiddleName]                  NVARCHAR (255) NULL,
    [MobileNumber]                VARCHAR (10)   NULL,
    [Email]                       NVARCHAR (60)  NULL,
    [RoleID]                      INT            NULL,
    [IsMeridianUser]              BIT            NULL,
    [Password]                    NVARCHAR (255) NULL,
    [IncorrectLoginAttemptsCount] SMALLINT       NULL,
    [LastSuccessfulLogin]         DATETIME       NULL,
    [LockoutEndDateUtc]           DATETIME       NULL,
    [LastPasswordChanged]         DATETIME       NULL,
    [LockoutEnabled]              BIT            NULL,
    [LastName]                    NVARCHAR (255) NULL,
    [ForgotPasswordToken]         NVARCHAR (100) NULL,
    [RecordStatus]                CHAR (1)       NOT NULL,
    [CreatedDate]                 DATETIME       NOT NULL,
    [CreatedBy]                   NVARCHAR (60)  NOT NULL,
    [ModifiedDate]                DATETIME       NOT NULL,
    [ModifiedBy]                  NVARCHAR (60)  NOT NULL,
    PRIMARY KEY CLUSTERED ([ID] ASC),
    FOREIGN KEY ([RoleID]) REFERENCES [dbo].[Roles] ([RoleID])
);



