CREATE TABLE RoleAction(
RoleActionId INT NOT NULL IDENTITY,
RoleId INT NOT NULL,
ScreenActionId INT NOT NULL,
RecordStatus VARCHAR(1) NOT NULL,
CreatedBy VARCHAR(100),
CreatedDate DATE,
ModifiedBy VARCHAR(100),
ModifiedDate DATE
CONSTRAINT FK_ScreenActionId_RoleActions FOREIGN KEY (ScreenActionId) REFERENCES ScreenAction(ScreenActionId),
CONSTRAINT FK_RoleId_RoleActions FOREIGN KEY (RoleId) REFERENCES Roles(RoleId), 
    CONSTRAINT [PK_RoleAction] PRIMARY KEY ([RoleActionId])
)
