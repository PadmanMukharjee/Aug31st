/*
Script Name : 10_Screen
Module_Name : M3Pact
Created By  : Abhishek K
CreatedDate : 05/10/2018

Revision History 
=============
Ver  Date				Who						Comment
1.0  10-May-2018		Abhishek Kovvuri		Scripts to insert data
1.1  ??					??						Scripts to update data
1.2  15-Jun-2018		Shravani Palla			Scripts to update data for heatmap page
1.3  08-Aug-2018        Abhishek Kovvuri        Scripts to insert to-do-list screen

Data Verification Script :

SELECT * FROM [Screen](NOLOCK) WHERE RecordStatus = 'A'
*/


/*
   Need to be inserted in the same order. 
   If not will lead to errors in parent screen id column which may be dependent on the above records in the table .
*/


BEGIN TRY
BEGIN TRANSACTION

DECLARE @User NVARCHAR(30) = 'Admin'
DECLARE @ScreenId INT;

INSERT INTO [dbo].[Screen]
           ([ScreenName]
           ,[ScreenCode]
           ,[ScreenDescription]
           ,[Icon]
           ,[ScreenPath]
           ,[IsMenuItem]
           ,[ParentScreenId]
           ,[RecordStatus]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[ModifiedBy]
           ,[ModifiedDate]
           ,[Displayorder])
            SELECT 
            S.ScreenName
           ,S.ScreenCode
           ,S.ScreenDescription
		   ,S.Icon
		   ,S.ScreenPath
		   ,S.IsMenuItem
		   ,S.ParentScreenId
           ,'A' AS [RecordStatus]
           ,@User AS [CreatedBy]
           ,GETDATE() AS [CreatedDate]
           ,@User AS [ModifiedBy]
           ,GETDATE() AS [ModifiedDate]
		   ,S.Displayorder
           FROM
           (
           SELECT 'Deposit Log' AS ScreenName , 'DL' AS ScreenCode , 'Deposit Log' AS ScreenDescription , 'fa fa-lock' AS Icon , '/depositlog' AS ScreenPath , 1 AS IsMenuItem , NULL AS ParentScreenId , 2 AS Displayorder UNION ALL
           SELECT 'Heat Map' AS ScreenName , 'HM' AS ScreenCode , 'Heat Map' AS ScreenDescription , 'fa fa-braille' AS Icon , '/heatmap' AS ScreenPath , 1 AS IsMenuItem , NULL AS ParentScreenId , 3 AS Displayorder UNION ALL
		   SELECT 'CheckLists' AS ScreenName , 'CL' AS ScreenCode , 'CheckLists' AS ScreenDescription , 'fa fa-list-alt' AS Icon , '' AS ScreenPath , 1 AS IsMenuItem , NULL AS ParentScreenId , 4 AS Displayorder UNION ALL
		   SELECT 'Clients' AS ScreenName , 'CLI' AS ScreenCode , 'Clients' AS ScreenDescription , 'fa fa-users' AS Icon , '' AS ScreenPath , 1 AS IsMenuItem , NULL AS ParentScreenId , 5 AS Displayorder UNION ALL
		   SELECT 'Administration' AS ScreenName , 'ADMIN' AS ScreenCode , 'Administration' AS ScreenDescription , 'fa fa-lock' AS Icon , '' AS ScreenPath , 1 AS IsMenuItem , NULL AS ParentScreenId , 6 AS Displayorder UNION ALL
		   SELECT 'ClientViewEdit' AS ScreenName , 'CVE' AS ScreenCode , 'edit' AS ScreenDescription , 'fa fa-search' AS Icon , '/client' AS ScreenPath , 1 AS IsMenuItem , 4 AS ParentScreenId , 1 AS Displayorder UNION ALL
		   SELECT 'ClientCreateNew' AS ScreenName , 'CN' AS ScreenCode , 'create' AS ScreenDescription , 'fa fa-search' AS Icon , '/client' AS ScreenPath , 1 AS IsMenuItem , 4 AS ParentScreenId , 2 AS Displayorder UNION ALL
		   SELECT 'ClientViewAll' AS ScreenName , 'VA' AS ScreenCode , 'viewClient' AS ScreenDescription , 'fa fa-search' AS Icon , '/client/ViewAllClients' AS ScreenPath , 1 AS IsMenuItem , 4 AS ParentScreenId , 3 AS Displayorder UNION ALL
		   SELECT 'Business Days' AS ScreenName , 'BD' AS ScreenCode , 'Business Days' AS ScreenDescription , 'fa fa-search' AS Icon , '/administration/businessdays' AS ScreenPath , 1 AS IsMenuItem , 5 AS ParentScreenId , 1 AS Displayorder UNION ALL
		   SELECT 'Sites' AS ScreenName , 'SITES' AS ScreenCode , 'Sites' AS ScreenDescription , 'fa fa-search' AS Icon , '/administration/sites' AS ScreenPath , 1 AS IsMenuItem , 5 AS ParentScreenId , 2 AS Displayorder UNION ALL
		   SELECT 'Business Units' AS ScreenName , 'BU' AS ScreenCode , 'Business Units' AS ScreenDescription , 'fa fa-search' AS Icon , '/administration/businessunits' AS ScreenPath , 1 AS IsMenuItem , 5 AS ParentScreenId , 3 AS Displayorder UNION ALL
		   SELECT 'AdminCheckLists' AS ScreenName , 'ACL' AS ScreenCode , 'CheckLists' AS ScreenDescription , 'fa fa-search' AS Icon , '' AS ScreenPath , 1 AS IsMenuItem , 5 AS ParentScreenId , 4 AS Displayorder UNION ALL
		   SELECT 'KPI' AS ScreenName , 'KPI' AS ScreenCode , 'KPI' AS ScreenDescription , 'fa fa-search' AS Icon , '' AS ScreenPath , 1 AS IsMenuItem , 5 AS ParentScreenId , 5 AS Displayorder UNION ALL
		   SELECT 'AdminHeatMap' AS ScreenName , 'AHM' AS ScreenCode , 'Heat Map' AS ScreenDescription , 'fa fa-search' AS Icon , '' AS ScreenPath , 1 AS IsMenuItem , 5 AS ParentScreenId , 6 AS Displayorder UNION ALL
		   SELECT 'Payers' AS ScreenName , 'PAY' AS ScreenCode , 'Payers' AS ScreenDescription , 'fa fa-search' AS Icon , '/administration/payers' AS ScreenPath , 1 AS IsMenuItem , 5 AS ParentScreenId , 7 AS Displayorder UNION ALL
		   SELECT 'Specialties' AS ScreenName , 'SPEC' AS ScreenCode , 'Specialties' AS ScreenDescription , 'fa fa-search' AS Icon , '/administration/specialties' AS ScreenPath , 1 AS IsMenuItem , 5 AS ParentScreenId , 8 AS Displayorder UNION ALL
		   SELECT 'Users' AS ScreenName , 'USER' AS ScreenCode , 'Users' AS ScreenDescription , 'fa fa-search' AS Icon , '' AS ScreenPath , 1 AS IsMenuItem , 5 AS ParentScreenId , 9 AS Displayorder UNION ALL
		   SELECT 'Create KPI' AS ScreenName , 'CK' AS ScreenCode , 'Create KPI' AS ScreenDescription , 'fa fa-usd' AS Icon , '' AS ScreenPath , 1 AS IsMenuItem , 13 AS ParentScreenId , 1 AS Displayorder UNION ALL
		   SELECT 'ViewEdit KPI' AS ScreenName , 'VEK' AS ScreenCode , 'View/Edit KPI' AS ScreenDescription , 'fa fa-usd' AS Icon , '' AS ScreenPath , 1 AS IsMenuItem , 13 AS ParentScreenId , 2 AS Displayorder UNION ALL
		   SELECT 'Create M3 Users' AS ScreenName , 'CMU' AS ScreenCode , 'Create M3 Users' AS ScreenDescription , 'fa fa-usd' AS Icon , '/administration/users/m3users' AS ScreenPath , 1 AS IsMenuItem , 17 AS ParentScreenId , 1 AS Displayorder UNION ALL
		   SELECT 'Create Client Users' AS ScreenName , 'CCU' AS ScreenCode , 'Create Client Users' AS ScreenDescription , 'fa fa-usd' AS Icon , '/administration/users/clientusers' AS ScreenPath , 1 AS IsMenuItem , 17 AS ParentScreenId , 2 AS Displayorder UNION ALL
		   SELECT 'All Users' AS ScreenName , 'AU' AS ScreenCode , 'All Users' AS ScreenDescription , 'fa fa-usd' AS Icon , '/administration/users/allusers' AS ScreenPath , 1 AS IsMenuItem , 17 AS ParentScreenId , 3 AS Displayorder UNION ALL
		   SELECT 'Client Data' AS ScreenName , 'CD' AS ScreenCode , 'Client Data' AS ScreenDescription , 'fa fa-usd' AS Icon , '' AS ScreenPath , 0 AS IsMenuItem , NULL AS ParentScreenId , NULL AS Displayorder UNION ALL
		   SELECT 'Log Setup' AS ScreenName , 'LS' AS ScreenCode , 'Log Setup' AS ScreenDescription , 'fa fa-usd' AS Icon , '' AS ScreenPath , 0 AS IsMenuItem , NULL AS ParentScreenId , NULL AS Displayorder UNION ALL
		   SELECT 'Assign User' AS ScreenName , 'CAU' AS ScreenCode , 'Assign User' AS ScreenDescription , 'fa fa-usd' AS Icon , '' AS ScreenPath , 0 AS IsMenuItem , NULL AS ParentScreenId , NULL AS Displayorder UNION ALL
		   SELECT 'Monthly Targets' AS ScreenName , 'MT' AS ScreenCode , 'Monthly Targets' AS ScreenDescription , 'fa fa-usd' AS Icon , '' AS ScreenPath , 0 AS IsMenuItem , NULL AS ParentScreenId , NULL AS Displayorder UNION ALL
		   SELECT 'KPI Setup' AS ScreenName , 'KPIS' AS ScreenCode , 'KPI Setup' AS ScreenDescription , 'fa fa-usd' AS Icon , '' AS ScreenPath , 0 AS IsMenuItem , NULL AS ParentScreenId , NULL AS Displayorder UNION ALL
		   SELECT 'Pending CheckLists' AS ScreenName , 'PCL' AS ScreenCode , 'Pending CheckLists' AS ScreenDescription , 'fa fa-usd' AS Icon , 'checklist/pending' AS ScreenPath , 1 AS IsMenuItem , 3 AS ParentScreenId , 1 AS Displayorder UNION ALL
		   SELECT 'Completed CheckLists' AS ScreenName , 'CCL' AS ScreenCode , 'Completed CheckLists' AS ScreenDescription , 'fa fa-usd' AS Icon , 'checklist/completed' AS ScreenPath , 1 AS IsMenuItem , 3 AS ParentScreenId , 2 AS Displayorder UNION ALL
		   SELECT 'Landing Page' AS ScreenName , 'LP' AS ScreenCode , 'Landing Page' AS ScreenDescription , 'fa fa-usd' AS Icon , '' AS ScreenPath , 1 AS IsMenuItem , NULL AS ParentScreenId , 1 AS Displayorder UNION ALL
		   SELECT 'CreateEdit Checklist Item' AS ScreenName , 'CECI' AS ScreenCode , 'Create/ Edit Checklist Item' AS ScreenDescription , 'fa fa-usd' AS Icon , '' AS ScreenPath , 1 AS IsMenuItem , 12 AS ParentScreenId , 1 AS Displayorder UNION ALL
		   SELECT 'Checklist Setup' AS ScreenName , 'CLS' AS ScreenCode , 'Checklist Setup' AS ScreenDescription , 'fa fa-usd' AS Icon , '' AS ScreenPath , 1 AS IsMenuItem , 12 AS ParentScreenId , 2 AS Displayorder UNION ALL
		   SELECT 'View All Checklist' AS ScreenName , 'VACL' AS ScreenCode , 'View All Checklist' AS ScreenDescription , 'fa fa-usd' AS Icon , '' AS ScreenPath , 1 AS IsMenuItem , 12 AS ParentScreenId , 3 AS Displayorder UNION ALL
		   SELECT 'System' AS ScreenName , 'SYS' AS ScreenCode , 'System' AS ScreenDescription , 'fa fa-usd' AS Icon , '/administration/systems' AS ScreenPath , 1 AS IsMenuItem , 5 AS ParentScreenId , 10 AS Displayorder UNION ALL
		   SELECT 'Configurations' AS ScreenName , 'CON' AS ScreenCode , 'Configurations' AS ScreenDescription , 'fa fa-braille' AS Icon , '/administration/config' AS ScreenPath , 1 AS IsMenuItem , 5 AS ParentScreenId , 11 AS Displayorder  UNION ALL
		   SELECT 'To-Do-List' AS ScreenName , 'TDL' AS ScreenCode , 'To-Do-List' AS ScreenDescription , 'fa fa-search' AS Icon , '' AS ScreenPath , 0 AS IsMenuItem , NULL AS ParentScreenId , NULL AS Displayorder 
           )
           AS S
 WHERE NOT EXISTS( SELECT 1 FROM [Screen] (NOLOCK) B WHERE B.ScreenCode=S.ScreenCode) 

SELECT @ScreenId = ScreenId FROM Screen WHERE ScreenCode = 'CECI'
IF EXISTS(SELECT TOP 1 1 FROM [Screen] WHERE ScreenId = @ScreenId)
BEGIN
	Update Screen
	Set ScreenPath='/administration/checklistitems',
		ModifiedBy = @User,
		ModifiedDate =GETDATE()
	where ScreenId= @ScreenId
END

SELECT @ScreenId = ScreenId FROM Screen WHERE ScreenCode = 'CK'
IF EXISTS(SELECT TOP 1 1 FROM [Screen] WHERE ScreenId = @ScreenId)
BEGIN
	Update Screen
	Set ScreenPath='/administration/kpi/createKpi',
		ModifiedBy = @User,
		ModifiedDate =GETDATE()
	where ScreenId= @ScreenId
END

SELECT @ScreenId = ScreenId FROM Screen WHERE ScreenCode = 'VEK'
IF EXISTS(SELECT TOP 1 1 FROM [Screen] WHERE ScreenId = @ScreenId)
BEGIN
	Update Screen
	Set ScreenPath='/administration/kpi/viewKpi',
		ModifiedBy = @User,
		ModifiedDate =GETDATE()
	where ScreenId= @ScreenId
END

SELECT @ScreenId = ScreenId FROM Screen WHERE ScreenCode = 'CLS'
IF EXISTS(SELECT TOP 1 1 FROM [Screen] WHERE ScreenId = @ScreenId)
BEGIN
	Update Screen
	Set ScreenPath='/administration/checklist',
		ModifiedBy = @User,
		ModifiedDate =GETDATE()
	where ScreenId= @ScreenId
END

SELECT @ScreenId = ScreenId FROM Screen WHERE ScreenCode = 'VACL'
IF EXISTS(SELECT TOP 1 1 FROM [Screen] WHERE ScreenId = @ScreenId)
BEGIN
	Update Screen
	Set ScreenPath='/administration/checklists/viewallchecklists',
		ModifiedBy = @User,
		ModifiedDate =GETDATE()
	where ScreenId= @ScreenId
END

SELECT @ScreenId = ScreenId FROM Screen WHERE ScreenCode = 'PCL'
IF EXISTS(SELECT TOP 1 1 FROM [Screen] WHERE ScreenId = @ScreenId AND ScreenPath = '')
BEGIN
	UPDATE Screen
	SET ScreenPath='checklist/pending',
		ModifiedBy = @User,
		ModifiedDate = GETDATE()
	WHERE ScreenId = @ScreenId
END

SELECT @ScreenId = ScreenId FROM Screen WHERE ScreenCode = 'CCL'
IF EXISTS(SELECT TOP 1 1 FROM [Screen] WHERE ScreenId = @ScreenId AND ScreenPath = '')
BEGIN
	UPDATE Screen
	SET ScreenPath='checklist/completed',
		ModifiedBy = @User,
		ModifiedDate = GETDATE()
	WHERE ScreenId = @ScreenId
END

SELECT @ScreenId = ScreenId FROM Screen WHERE ScreenCode = 'HM'
IF EXISTS(SELECT TOP 1 1 FROM [Screen] WHERE ScreenId = @ScreenId AND ScreenPath = '')
BEGIN
	UPDATE Screen
	SET ScreenPath='/heatmap',
		ModifiedBy = @User,
		ModifiedDate = GETDATE()
	WHERE ScreenId = @ScreenId
END

SELECT @ScreenId = ScreenId FROM Screen WHERE ScreenCode = 'AHM'
IF EXISTS(SELECT TOP 1 1 FROM [Screen] WHERE ScreenId = @ScreenId)
BEGIN
	Update Screen
	Set ScreenPath='/administration/heatmap',
		ModifiedBy = @User,
		ModifiedDate =GETDATE()
	where ScreenId= @ScreenId
END

SELECT @ScreenId = ScreenId FROM Screen WHERE ScreenCode = 'LP'
IF EXISTS(SELECT TOP 1 1 FROM [Screen] WHERE ScreenId = @ScreenId)
BEGIN
	Update Screen
	Set ScreenPath='/dashboard',
		 Icon='fa fa-home',
		ModifiedBy = @User,
		ModifiedDate =GETDATE()
	where ScreenId= @ScreenId
END

--Updating Screen Order

SELECT @ScreenId = ScreenId FROM Screen WHERE ScreenCode = 'RP'
IF NOT EXISTS(SELECT TOP 1 1 FROM [Screen] WHERE ScreenId = @ScreenId)
BEGIN
UPDATE screen
SET Displayorder = Displayorder+1
WHERE IsMenuItem=1 and ParentScreenId Is null and Displayorder!=1

INSERT INTO screen VALUES('Reports','RP','Reports','fa fa-line-chart','/reports',1,null,2,'A','Admin',GETDATE(),'Admin',GETDATE())

END

SELECT @ScreenId = ScreenId FROM Screen WHERE ScreenCode = 'DL'
IF EXISTS(SELECT TOP 1 1 FROM [Screen] WHERE ScreenId = @ScreenId)
BEGIN
	Update Screen
	Set Icon='fa fa-usd',
		ModifiedBy = @User,
		ModifiedDate =GETDATE()
	where ScreenId= @ScreenId
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
