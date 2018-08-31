CREATE TABLE M3MetricsQuestion(
[M3MetricsQuestionID]          INT  IDENTITY (1, 1) NOT NULL,
[M3MetricsQuestionCode]        NVARCHAR (60)  NOT NULL,
[M3MetricsQuestionText]        NVARCHAR (60)  NOT NULL,
[M3MetricsUnit]                NVARCHAR (60),   
[RecordStatus]                 CHAR (1)       NOT NULL,
[CreatedDate]                  DATETIME       NOT NULL,
[CreatedBy]                    NVARCHAR (60)  NOT NULL,
[ModifiedDate]                 DATETIME       NOT NULL,
[ModifiedBy]                   NVARCHAR (60)  NOT NULL,
PRIMARY KEY CLUSTERED ([M3MetricsQuestionID] ASC),
)
