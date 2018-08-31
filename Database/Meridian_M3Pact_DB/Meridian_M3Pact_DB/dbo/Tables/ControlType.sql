CREATE TABLE [dbo].[ControlType]
(
	[ControlTypeId] INT NOT NULL PRIMARY KEY IDENTITY (1, 1),
	ControlName VARCHAR(50) NOT NULL,
	CreatedDate Datetime,
	CreatedBy VARCHAR(50),
	ModifiedDate Datetime,
	ModifiedBy VARCHAR(50)
)
