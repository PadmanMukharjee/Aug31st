CREATE TABLE [dbo].[Speciality] (
    [SpecialityID]          INT            IDENTITY (1, 1) NOT NULL,
    [SpecialityCode]        NVARCHAR (255) NULL,
    [SpecialityName]        NVARCHAR (255) NOT NULL,
    [SpecialityDescription] NVARCHAR (255) NULL,
    [RecordStatus]          CHAR (1)       NOT NULL,
    [CreatedDate]           DATETIME       NOT NULL,
    [CreatedBy]             NVARCHAR (60)  NOT NULL,
    [ModifiedDate]          DATETIME       NOT NULL,
    [ModifiedBy]            NVARCHAR (60)  NOT NULL,
    PRIMARY KEY CLUSTERED ([SpecialityID] ASC)
);

