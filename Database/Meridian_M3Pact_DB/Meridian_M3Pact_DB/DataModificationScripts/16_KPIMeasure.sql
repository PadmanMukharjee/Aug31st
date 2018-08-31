

BEGIN TRY
BEGIN TRANSACTION

DECLARE @User NVARCHAR(30) = 'Admin'

INSERT INTO [dbo].[KPIMeasure]
			([CheckListTypeID]
			,[Measure]
			,[RecordStatus]
            ,[CreatedBy]
            ,[CreatedDate]
            ,[ModifiedBy]
            ,[ModifiedDate]
            )
			SELECT 
            kpiM.[CheckListTypeID]
           ,kpiM.[Measure]
           ,'A' AS [RecordStatus]
		   ,@User AS [CreatedBy]
           ,GETDATE() AS [CreatedDate]
           ,@User AS [ModifiedBy]
           ,GETDATE() AS [ModifiedDate]
           FROM
           (
           SELECT 1 AS CheckListTypeID, 'On Change' AS Measure UNION ALL
           SELECT 2 AS CheckListTypeID, 'On Change' AS Measure UNION ALL
           SELECT 3 AS CheckListTypeID, 'Daily' AS Measure UNION ALL
           SELECT 3 AS CheckListTypeID, 'Weekly' AS Measure UNION ALL
           SELECT 3 AS CheckListTypeID, 'Monthly' AS Measure
           )
           AS kpiM
 WHERE NOT EXISTS( SELECT 1 FROM [KPIMeasure] (NOLOCK) B WHERE B.CheckListTypeID=kpiM.CheckListTypeID) 

COMMIT TRANSACTION
END TRY

BEGIN CATCH
	SELECT ERROR_MESSAGE()
		,ERROR_LINE()
		,ERROR_SEVERITY()

	ROLLBACK TRANSACTION
END CATCH
GO