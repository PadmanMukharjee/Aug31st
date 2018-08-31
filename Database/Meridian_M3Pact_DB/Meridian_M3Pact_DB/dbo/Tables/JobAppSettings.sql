CREATE TABLE [dbo].[JobAppSettings]
(
JobAppSettingsId INT PRIMARY KEY IDENTITY,
SettingName NVARCHAR(100) NOT NULL,
SettingValue NVARCHAR(100) NOT NULL,
Description NVARCHAR(500) NOT NULL,
RecordStatus CHAR(1) NOT NULL,
CreatedDate Datetime NOT NULL,
CreatedBy NVARCHAR(100) NOT NULL,
ModifiedDate Datetime NOT NULL,
ModifiedBy NVARCHAR(100) NOT NULL
)
