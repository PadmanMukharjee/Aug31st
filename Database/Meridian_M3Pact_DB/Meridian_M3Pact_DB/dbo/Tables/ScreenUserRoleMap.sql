CREATE TABLE ScreenUserRoleMap(
ScreenUserRoleId INT NOT NULL IDENTITY,
ScreenId INT NOT NULL,
RoleId INT NOT NULL,
RecordStatus VARCHAR(1) NOT NULL,
CanEdit BIT ,
DisplayScreenName NVARCHAR(50) NOT NULL,
CreatedBy VARCHAR(100),
CreatedDate DATE,
ModifiedBy VARCHAR(100),
ModifiedDate DATE

Constraint FK_ScreenId_ScreenUserRoleMap FOREIGN KEY (ScreenId) REFERENCES [dbo].Screen (ScreenId),
Constraint FK_RoleId_ScreenUserRoleMap FOREIGN KEY (RoleId) REFERENCES Roles(RoleId), 
    CONSTRAINT [PK_ScreenUserRoleMap] PRIMARY KEY ([ScreenUserRoleId])
)
