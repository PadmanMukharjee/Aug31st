CREATE TABLE [dbo].[Application] (
    [ID]          INT            IDENTITY (1, 1) NOT NULL,
    [Name]        NVARCHAR (255) NOT NULL,
    [Key]         NVARCHAR (255) NOT NULL,
    [Secret]      NVARCHAR (255) NOT NULL,
    [TypeID]      INT            NULL,
    [RedirectURL] VARCHAR (MAX)  NULL,
    PRIMARY KEY CLUSTERED ([ID] ASC),
    FOREIGN KEY ([TypeID]) REFERENCES [dbo].[ApplicationType] ([ID])
);

