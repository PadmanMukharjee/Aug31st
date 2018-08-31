CREATE TABLE [dbo].[ClientNoticePeriodMail]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY,
	ClientId INT NOT NULL,
	ContractEndDate DATE NOT NULL,
	MailSentDate DATE NOT NULL,
	MailType VARCHAR(20) NOT NULL,
	NoticePeriod INT NOT NULL,
	CreatedDate DATETIME NOT NULL
)
