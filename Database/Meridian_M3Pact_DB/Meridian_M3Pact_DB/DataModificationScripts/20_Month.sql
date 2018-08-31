/*
Script Name : 20_Month
Module_Name : M3Pact
Created By  : Shravani P
CreatedDate : 12-Jun-2018

Data Verification Script : 

SELECT * FROM [Month](NOLOCK)
*/

BEGIN TRY
BEGIN TRANSACTION

DECLARE @User NVARCHAR(30) = 'Admin'

INSERT INTO [dbo].[Month]
(
	[MonthCode],
    [MonthName],
    [RecordStatus],
    [CreatedDate],
    [CreatedBy],
    [ModifiedDate],
    [ModifiedBy]
)
SELECT
	M.[MonthCode],
	M.[MonthName],
	'A' AS [RecordStatus],
	GETDATE() AS [CreatedDate],
    @User AS [CreatedBy],
    GETDATE() AS [ModifiedDate],
    @User AS [ModifiedBy]
FROM
(
	SELECT 'JAN' AS MonthCode, 'January' AS [MonthName] UNION ALL
	SELECT 'FEB' AS MonthCode, 'February' AS [MonthName] UNION ALL
	SELECT 'MAR' AS MonthCode, 'March' AS [MonthName] UNION ALL
	SELECT 'APR' AS MonthCode, 'April' AS [MonthName] UNION ALL
	SELECT 'MY' AS MonthCode, 'May' AS [MonthName] UNION ALL
	SELECT 'JUN' AS MonthCode, 'June' AS [MonthName] UNION ALL
	SELECT 'JUL' AS MonthCode, 'July' AS [MonthName] UNION ALL
	SELECT 'AUG' AS MonthCode, 'August' AS [MonthName] UNION ALL
	SELECT 'SEP' AS MonthCode, 'September' AS [MonthName] UNION ALL
	SELECT 'OCT' AS MonthCode, 'October' AS [MonthName] UNION ALL
	SELECT 'NOV' AS MonthCode, 'November' AS [MonthName] UNION ALL
	SELECT 'DEC' AS MonthCode, 'December' AS [MonthName]
)
AS M
WHERE NOT EXISTS (SELECT 1 FROM [Month] (NOLOCK) B WHERE B.MonthCode = M.MonthCode)

COMMIT TRANSACTION
END TRY

BEGIN CATCH
	SELECT ERROR_MESSAGE()
		,ERROR_LINE()
		,ERROR_SEVERITY()

	ROLLBACK TRANSACTION
END CATCH
GO






