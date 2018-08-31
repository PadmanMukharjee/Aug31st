CREATE TABLE [dbo].[MailRecepientsDetailsDayWise]
(
[Id] INT NOT NULL PRIMARY KEY IDENTITY,
DeviatedClientKPIId INT,
UserID NVARCHAR(510) NOT NULL,
SentDate DATETIME,
AlertType VARCHAR(20), 
CONSTRAINT [FK_MailRecepientsDetailsDayWise_DeviatedClientKPI] FOREIGN KEY (DeviatedClientKPIId) REFERENCES DeviatedClientKPI(DeviatedClientKPIId)
)
