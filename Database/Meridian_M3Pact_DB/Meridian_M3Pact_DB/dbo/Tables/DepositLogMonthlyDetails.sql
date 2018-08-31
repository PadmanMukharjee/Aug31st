CREATE TABLE [dbo].[DepositLogMonthlyDetails]
(
	[CloseMonthId] INT NOT NULL PRIMARY KEY IDENTITY(1,1),
    [ClientId] INT NOT NULL,
    [MonthId] INT NOT NULL,
    [Year] INT NOT NULL,
    [TotalDepositAmount] Numeric(15,2) NULL,
	[ProjectedCash] Numeric(15,2) NULL,
    [MonthStatus] char(1) NOT NULL ,
    [RecordStatus] char(1) NOT NULL , 
    [CreatedDate] DATETIME NOT NULL , 
    [CreatedBy] NVARCHAR(250) NOT NULL , 
    [ModifiedDate] DATETIME NOT NULL , 
    [ModifiedBy] NVARCHAR(250) NOT NULL

	Constraint FK_ClientId_CloseMonth FOREIGN KEY (ClientId) REFERENCES [dbo].Client(ClientId),
    Constraint FK_MonthId_CloseMonth FOREIGN KEY (MonthId) REFERENCES Month(MonthId), 
)
