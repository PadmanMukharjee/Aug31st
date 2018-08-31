/*
Script Name : 23_ControlType
Module_Name : M3Pact
Created By  : Mukharjee Chippa
CreatedDate : 31-July-2018

Data Verification Script : 

SELECT * FROM [ControlType](NOLOCK)
*/

BEGIN TRY
BEGIN TRANSACTION

DECLARE @User NVARCHAR(30) = 'Admin'

INSERT INTO [dbo].[ControlType]
(
	[ControlName], 
	[CreatedDate],
	[CreatedBy],
	[ModifiedDate],
	[ModifiedBy]
)
SELECT
	A.[ControlName], 
	A.[CreatedDate],
	@User AS [CreatedBy],
	A.[ModifiedDate],
	@User AS[ModifiedBy]
FROM
(
	SELECT 'TextBox' AS [ControlName], GETDATE() AS [CreatedDate],GETDATE() AS [ModifiedDate] UNION ALL
	SELECT 'TextArea' AS [ControlName], GETDATE() AS [CreatedDate],GETDATE() AS [ModifiedDate] UNION ALL
	SELECT 'Radio' AS [ControlName], GETDATE() AS [CreatedDate],GETDATE() AS [ModifiedDate] UNION ALL
	SELECT 'Checklist' AS [ControlName], GETDATE() AS [CreatedDate],GETDATE() AS [ModifiedDate] UNION ALL
	SELECT 'Dropdown' AS [ControlName], GETDATE() AS [CreatedDate],GETDATE() AS [ModifiedDate] UNION ALL
	SELECT 'DateTime' AS [ControlName], GETDATE() AS [CreatedDate],GETDATE() AS [ModifiedDate] UNION ALL
	SELECT 'RadioList' AS [ControlName], GETDATE() AS [CreatedDate],GETDATE() AS [ModifiedDate] UNION ALL
	SELECT 'Phone' AS [ControlName], GETDATE() AS [CreatedDate],GETDATE() AS [ModifiedDate] 	
)
AS A
WHERE NOT EXISTS (SELECT 1 FROM [ControlType] (NOLOCK) B WHERE B.ControlName = A.ControlName)

COMMIT TRANSACTION
END TRY

BEGIN CATCH
	SELECT ERROR_MESSAGE()
		,ERROR_LINE()
		,ERROR_SEVERITY()

	ROLLBACK TRANSACTION
END CATCH
GO