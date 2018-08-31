CREATE TABLE [dbo].[ApplicationApiResource] (
    [ID]            INT IDENTITY (1, 1) NOT NULL,
    [ApplicationID] INT NOT NULL,
    [ApiResourceID] INT NOT NULL,
    CONSTRAINT [PK_ApplicationApiResource] PRIMARY KEY CLUSTERED ([ID] ASC),
    FOREIGN KEY ([ApiResourceID]) REFERENCES [dbo].[ApiResource] ([ID]),
    FOREIGN KEY ([ApplicationID]) REFERENCES [dbo].[Application] ([ID])
);

