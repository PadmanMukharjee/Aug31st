BEGIN TRY
BEGIN TRANSACTION

DECLARE @User NVARCHAR(30) = 'Admin'
DECLARE @M3MetricsQuestionCode NVARCHAR(200);

INSERT INTO [dbo].[M3MetricsQuestion]
			([M3MetricsQuestionCode]
			,[M3MetricsQuestionText]
			,[RecordStatus]
            ,[CreatedBy]
            ,[CreatedDate]
            ,[ModifiedBy]
            ,[ModifiedDate]
			)
			SELECT 
            clt.[M3MetricsQuestionCode]
           ,clt.[M3MetricsQuestionText]
           ,'A' AS [RecordStatus]
		   ,@User AS [CreatedBy]
           ,GETDATE() AS [CreatedDate]
           ,@User AS [ModifiedBy]
           ,GETDATE() AS [ModifiedDate]
           FROM
           (
		   SELECT 'Month-to-Date Charges' AS M3MetricsQuestionCode, 'Month-to-Date Charges' AS M3MetricsQuestionText UNION ALL
		   SELECT 'Charges <90 Days' AS M3MetricsQuestionCode,'Charges <90 Days' AS M3MetricsQuestionText UNION ALL
		   SELECT 'Month-to-Date Payments' AS M3MetricsQuestionCode,'Month-to-Date Payments' AS M3MetricsQuestionText UNION ALL
		   SELECT 'Payments <90 Days' AS M3MetricsQuestionCode,'Payments <90 Days' AS M3MetricsQuestionText UNION ALL
		   SELECT 'Month-to-Date Charge Lag' AS M3MetricsQuestionCode, 'Month-to-Date Charge Lag' AS M3MetricsQuestionText UNION ALL
		   SELECT 'Month-to-Date Payment Lag' AS M3MetricsQuestionCode, 'Month-to-Date Payment Lag' AS M3MetricsQuestionText UNION ALL
		   SELECT 'Aged AR > 120 Days' AS M3MetricsQuestionCode, 'Aged AR > 120 Days' AS M3MetricsQuestionText UNION ALL
		   SELECT 'Days In AR' AS M3MetricsQuestionCode, 'Days In AR' AS M3MetricsQuestionText UNION ALL
		   SELECT 'Net Collection Rate' AS M3MetricsQuestionCode, 'Net Collection Rate' AS M3MetricsQuestionText 
           )
           AS clt
 WHERE NOT EXISTS( SELECT 1 FROM [M3MetricsQuestion] (NOLOCK) B WHERE B.M3MetricsQuestionCode=clt.M3MetricsQuestionCode) 

SELECT @M3MetricsQuestionCode = M3MetricsQuestionCode FROM M3MetricsQuestion WHERE M3MetricsQuestionText = 'Month-to-Date Charges'
IF EXISTS ( SELECT TOP 1 1 FROM M3MetricsQuestion WHERE M3MetricsQuestionCode = @M3MetricsQuestionCode)
BEGIN
   UPDATE M3MetricsQuestion 
   SET M3MetricsUnit = 'Amount' 
   WHERE M3MetricsQuestionCode = @M3MetricsQuestionCode
END

SELECT @M3MetricsQuestionCode = M3MetricsQuestionCode FROM M3MetricsQuestion WHERE M3MetricsQuestionText = 'Charges <90 Days'
IF EXISTS ( SELECT TOP 1 1 FROM M3MetricsQuestion WHERE M3MetricsQuestionCode = @M3MetricsQuestionCode)
BEGIN
   UPDATE M3MetricsQuestion 
   SET M3MetricsUnit = 'Amount' 
   WHERE M3MetricsQuestionCode = @M3MetricsQuestionCode
END

SELECT @M3MetricsQuestionCode = M3MetricsQuestionCode FROM M3MetricsQuestion WHERE M3MetricsQuestionText = 'Month-to-Date Payments'
IF EXISTS ( SELECT TOP 1 1 FROM M3MetricsQuestion WHERE M3MetricsQuestionCode = @M3MetricsQuestionCode)
BEGIN
   UPDATE M3MetricsQuestion 
   SET M3MetricsUnit = 'Amount' 
   WHERE M3MetricsQuestionCode = @M3MetricsQuestionCode
END

SELECT @M3MetricsQuestionCode = M3MetricsQuestionCode FROM M3MetricsQuestion WHERE M3MetricsQuestionText = 'Payments <90 Days'
IF EXISTS ( SELECT TOP 1 1 FROM M3MetricsQuestion WHERE M3MetricsQuestionCode = @M3MetricsQuestionCode)
BEGIN
   UPDATE M3MetricsQuestion 
   SET M3MetricsUnit = 'Amount' 
   WHERE M3MetricsQuestionCode = @M3MetricsQuestionCode
END

SELECT @M3MetricsQuestionCode = M3MetricsQuestionCode FROM M3MetricsQuestion WHERE M3MetricsQuestionText = 'Month-to-Date Charge Lag'
IF EXISTS ( SELECT TOP 1 1 FROM M3MetricsQuestion WHERE M3MetricsQuestionCode = @M3MetricsQuestionCode)
BEGIN
   UPDATE M3MetricsQuestion 
   SET M3MetricsUnit = 'Days' 
   WHERE M3MetricsQuestionCode = @M3MetricsQuestionCode
END

SELECT @M3MetricsQuestionCode = M3MetricsQuestionCode FROM M3MetricsQuestion WHERE M3MetricsQuestionText = 'Month-to-Date Payment Lag'
IF EXISTS ( SELECT TOP 1 1 FROM M3MetricsQuestion WHERE M3MetricsQuestionCode = @M3MetricsQuestionCode)
BEGIN
   UPDATE M3MetricsQuestion 
   SET M3MetricsUnit = 'Days' 
   WHERE M3MetricsQuestionCode = @M3MetricsQuestionCode
END

SELECT @M3MetricsQuestionCode = M3MetricsQuestionCode FROM M3MetricsQuestion WHERE M3MetricsQuestionText = 'Aged AR > 120 Days'
IF EXISTS ( SELECT TOP 1 1 FROM M3MetricsQuestion WHERE M3MetricsQuestionCode = @M3MetricsQuestionCode)
BEGIN
   UPDATE M3MetricsQuestion 
   SET M3MetricsUnit = 'Percentage' 
   WHERE M3MetricsQuestionCode = @M3MetricsQuestionCode
END

SELECT @M3MetricsQuestionCode = M3MetricsQuestionCode FROM M3MetricsQuestion WHERE M3MetricsQuestionText = 'Days In AR'
IF EXISTS ( SELECT TOP 1 1 FROM M3MetricsQuestion WHERE M3MetricsQuestionCode = @M3MetricsQuestionCode)
BEGIN
   UPDATE M3MetricsQuestion 
   SET M3MetricsUnit = 'Days' 
   WHERE M3MetricsQuestionCode = @M3MetricsQuestionCode
END

SELECT @M3MetricsQuestionCode = M3MetricsQuestionCode FROM M3MetricsQuestion WHERE M3MetricsQuestionText = 'Net Collection Rate'
IF EXISTS ( SELECT TOP 1 1 FROM M3MetricsQuestion WHERE M3MetricsQuestionCode = @M3MetricsQuestionCode)
BEGIN
   UPDATE M3MetricsQuestion 
   SET M3MetricsUnit = 'Percentage' 
   WHERE M3MetricsQuestionCode = @M3MetricsQuestionCode
END

COMMIT TRANSACTION
END TRY

BEGIN CATCH
	SELECT ERROR_MESSAGE()
		,ERROR_LINE()
		,ERROR_SEVERITY()

	ROLLBACK TRANSACTION
END CATCH
GO