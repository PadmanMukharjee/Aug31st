CREATE TABLE Screen(
ScreenId INT NOT NULL IDENTITY,
ScreenName VARCHAR(50) NOT NULL,
ScreenCode VARCHAR(50) NOT NULL,
ScreenDescription VARCHAR(100),
Icon VARCHAR(50) NOT NULL,
ScreenPath VARCHAR(100) NOT NULL,
IsMenuItem BIT NOT NULL,
ParentScreenId INT,
Displayorder INT,
RecordStatus VARCHAR(1) NOT NULL,
CreatedBy VARCHAR(100),
CreatedDate DATE,
ModifiedBy VARCHAR(100),
ModifiedDate DATE, 
    CONSTRAINT [PK_SCREENS] PRIMARY KEY ([ScreenId]),
	CONSTRAINT UQ_Screen_ScreenCode UNIQUE (ScreenCode)

)