

BEGIN TRY
BEGIN TRANSACTION

DECLARE @User NVARCHAR(30) = 'Admin'

INSERT INTO [dbo].[CheckListType]
			([CheckListTypeCode]
			,[CheckListTypeName]
			,[RecordStatus]
			,[CreatedDate]
            ,[CreatedBy]
            ,[ModifiedDate]
            ,[ModifiedBy]
            )
			SELECT 
            clt.[CheckListTypeCode]
           ,clt.[CheckListTypeName]
           ,'A' AS [RecordStatus]
           ,GETDATE() AS [CreatedDate]
           ,@User AS [CreatedBy]
           ,GETDATE() AS [ModifiedDate]
           ,@User AS [ModifiedBy]
           FROM
           (
           SELECT 'WEEK' AS CheckListTypeCode, 'Weekly Checklist' AS CheckListTypeName UNION ALL
           SELECT 'MONTH' AS CheckListTypeCode, 'Monthly Checklist' AS CheckListTypeName UNION ALL
           SELECT 'M3'  AS CheckListTypeCode, 'M3Metrics'  AS CheckListTypeName
           )
           AS clt
 WHERE NOT EXISTS( SELECT 1 FROM [CheckListType] (NOLOCK) B WHERE B.CheckListTypeCode=clt.CheckListTypeCode) 

COMMIT TRANSACTION
END TRY

BEGIN CATCH
	SELECT ERROR_MESSAGE()
		,ERROR_LINE()
		,ERROR_SEVERITY()

	ROLLBACK TRANSACTION
END CATCH
GO