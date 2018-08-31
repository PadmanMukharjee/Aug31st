CREATE TABLE [dbo].[Holiday] (
    [HolidayID]          INT            IDENTITY (1, 1) NOT NULL,
    [HolidayDate]        DATETIME       NOT NULL,
    [HolidayDescription] NVARCHAR (255) NULL,
    [RecordStatus]       CHAR (1)       NOT NULL,
    [CreatedDate]        DATETIME       NOT NULL,
    [CreatedBy]          NVARCHAR (60)  NOT NULL,
    [ModifiedDate]       DATETIME       NOT NULL,
    [ModifiedBy]         NVARCHAR (60)  NOT NULL,
    PRIMARY KEY CLUSTERED ([HolidayID] ASC)
);

