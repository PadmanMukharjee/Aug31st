CREATE TABLE [dbo].[APIMethodsRoleMap]
(
ApimethodsRoleMapId INT NOT NULL IDENTITY,
APIMethodId INT NOT NULL,
RoleId INT NOT NULL,
RecordStatus VARCHAR(1) NOT NULL,
CreatedBy VARCHAR(100),
CreatedDate DATE,
ModifiedBy VARCHAR(100),
ModifiedDate DATE
CONSTRAINT FK_APIMethodId_APIMethodsRoleMap FOREIGN KEY (APIMethodId) REFERENCES APIMethod(APIMethodId),
CONSTRAINT FK_APIMethodId_RoleId FOREIGN KEY (RoleId) REFERENCES Roles(RoleId), 
    CONSTRAINT [PK_APIMethodsRoleMap] PRIMARY KEY ([ApimethodsRoleMapId])
)
