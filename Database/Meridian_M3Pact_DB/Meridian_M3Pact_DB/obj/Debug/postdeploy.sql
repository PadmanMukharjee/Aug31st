/*
Post-Deployment Script Template							
--------------------------------------------------------------------------------------
 This file contains SQL statements that will be appended to the build script.		
 Use SQLCMD syntax to include a file in the post-deployment script.			
 Example:      :r .\myfile.sql								
 Use SQLCMD syntax to reference a variable in the post-deployment script.		
 Example:      :setvar TableName MyTable							
               SELECT * FROM [$(TableName)]					
--------------------------------------------------------------------------------------
*/
/*
Script Name : 01_Site
Module_Name : M3Pact
Created By  : Sampath Y
CreatedDate : 04/25/2018

Data Verification Script :

SELECT * FROM [Site](NOLOCK) WHERE RecordStatus = 'A'
*/

BEGIN TRY
BEGIN TRANSACTION

DECLARE @User NVARCHAR(30) = 'Admin'

INSERT INTO [dbo].[Site]
           ([SiteCode]
           ,[SiteName]
           ,[SiteDescription]
           ,[RecordStatus]
           ,[CreatedDate]
           ,[CreatedBy]
           ,[ModifiedDate]
           ,[ModifiedBy])
           SELECT 
            S.SiteCode
           ,S.SiteName
           ,S.SiteDescription
           ,'A' AS [RecordStatus]
           ,GETDATE() AS [CreatedDate]
           ,@User AS [CreatedBy]
           ,GETDATE() AS [ModifiedDate]
           ,@User AS [ModifiedBy]
           FROM
           (
           SELECT 'New Jersey' AS SiteCode , 'New Jersey' AS SiteName,'New Jersey' AS SiteDescription UNION ALL
           SELECT 'Washington' AS SiteCode , 'Washington' AS SiteName,'Washington' AS SiteDescription UNION ALL
           SELECT 'SouthEast' AS SiteCode , 'SouthEast' AS SiteName,'SouthEast' AS SiteDescription UNION ALL
           SELECT 'Connecticut' AS SiteCode , 'Connecticut' AS SiteName,'Connecticut' AS SiteDescription UNION ALL
           SELECT 'Kansas' AS SiteCode , 'Kansas' AS SiteName,'Kansas' AS SiteDescription UNION ALL
           SELECT 'Corporate' AS SiteCode , 'Corporate' AS SiteName,'Corporate' AS SiteDescription
           )
           AS S
 WHERE NOT EXISTS( SELECT 1 FROM [Site] (NOLOCK) B WHERE B.SiteCode=S.SiteCode) 

 UPDATE SITE 
 SET SiteCode='NJ'
 WHERE SiteName='New Jersey'
 AND SiteCode='New Jersey'

 UPDATE SITE 
 SET SiteCode='WT'
 WHERE SiteName='Washington'
  AND SiteCode='Washington'

 UPDATE SITE 
 SET SiteCode='SE'
 WHERE SiteName='SouthEast'
  AND SiteCode='SouthEast'

 UPDATE SITE 
 SET SiteCode='CT'
 WHERE SiteName='Connecticut'
  AND SiteCode='Connecticut'

 UPDATE SITE 
 SET SiteCode='KS'
 WHERE SiteName='Kansas'
  AND SiteCode='Kansas'

 UPDATE SITE 
 SET SiteCode='CP'
 WHERE SiteName='Corporate'
  AND SiteCode='Corporate'

COMMIT TRANSACTION
END TRY

BEGIN CATCH
	SELECT ERROR_MESSAGE()
		,ERROR_LINE()
		,ERROR_SEVERITY()

	ROLLBACK TRANSACTION
END CATCH
GO

/*
Script Name : 01_ApplicationType
Module_Name : M3Pact
Created By  : Shravani P
CreatedDate : 04/25/2018

Data Verification Script : 

SELECT * FROM [ApplicationType](NOLOCK)
*/

BEGIN TRY
BEGIN TRANSACTION

INSERT INTO [dbo].[ApplicationType] ([Name])
SELECT A.[Name]
FROM
(
	SELECT 'Web-MVC' AS [Name] UNION ALL
	SELECT 'Web-JS' AS [Name] UNION ALL
	SELECT 'Native' AS [Name]
)
AS A
WHERE NOT EXISTS( SELECT 1 FROM [ApplicationType] (NOLOCK) B WHERE B.[Name]=A.[Name]) 

COMMIT TRANSACTION
END TRY

BEGIN CATCH
	SELECT ERROR_MESSAGE()
		,ERROR_LINE()
		,ERROR_SEVERITY()

	ROLLBACK TRANSACTION
END CATCH
GO
/*
Script Name : 03_Application
Module_Name : M3Pact
Created By  : Shravani P
CreatedDate : 04/25/2018

Data Verification Script :

SELECT * FROM [Application](NOLOCK)
*/

BEGIN TRY
BEGIN TRANSACTION

DECLARE @AppTypeID INT;
SELECT @AppTypeID = ID FROM [dbo].ApplicationType WHERE [Name] = 'Web-JS'

INSERT INTO [dbo].[Application]
(
	[Name], [Key], [Secret], [TypeID], [RedirectURL]
)
SELECT [Name], [Key], [Secret], [TypeID], [RedirectURL]
FROM
(
	SELECT 
		'M3PACT' AS [Name],
		'M3pact' AS [Key],
		'secret' AS [Secret],
		@AppTypeID AS [TypeID],
		'www.m3pact.com/callback.html' AS [RedirectURL]
)
AS A
WHERE NOT EXISTS( SELECT 1 FROM [Application] (NOLOCK) B WHERE B.[Name]=A.[Name]) 

COMMIT TRANSACTION
END TRY

BEGIN CATCH
	SELECT ERROR_MESSAGE()
		,ERROR_LINE()
		,ERROR_SEVERITY()

	ROLLBACK TRANSACTION
END CATCH
GO
/*
Script Name : 04_ApiResource
Module_Name : M3Pact
Created By  : Shravani P
CreatedDate : 04/25/2018

Data Verification Script : 

SELECT * FROM [ApiResource](NOLOCK)
*/

BEGIN TRY
BEGIN TRANSACTION

INSERT INTO [dbo].[ApiResource] ( [Name], [Description], [Enabled] )
SELECT A.[Name], A.[Description], A.[Enabled]
FROM
(
	SELECT 'M3_API' AS [Name], 'M3Pact API' AS [Description], 1	AS [Enabled]
)
AS A
WHERE NOT EXISTS( SELECT 1 FROM [ApiResource] (NOLOCK) B WHERE B.[Name]=A.[Name]) 

COMMIT TRANSACTION
END TRY

BEGIN CATCH
	SELECT ERROR_MESSAGE()
		,ERROR_LINE()
		,ERROR_SEVERITY()

	ROLLBACK TRANSACTION
END CATCH
GO
/*
Script Name : 05_ApplicationType
Module_Name : M3Pact
Created By  : Shravani P
CreatedDate : 04/25/2018

Data Verification Script :

SELECT * FROM [ApplicationApiResource](NOLOCK)
*/

BEGIN TRY
BEGIN TRANSACTION

DECLARE @ApplicationID INT;
DECLARE @ApiResourceID INT;

SELECT @ApplicationID = ID FROM dbo.[Application] WHERE Name = 'M3Pact'
SELECT @ApiResourceID = ID FROM dbo.[ApiResource] WHERE Name = 'M3_API'

INSERT INTO dbo.[ApplicationApiResource]( [ApplicationID], [ApiResourceID] ) 
SELECT A.ApplicationID, A.ApiResourceID
FROM
(
	SELECT @ApplicationID AS ApplicationID, @ApiResourceID AS ApiResourceID
)
AS A
WHERE NOT EXISTS ( SELECT 1 FROM dbo.[ApplicationApiResource] WHERE ApplicationID = @ApplicationID AND ApiResourceID = @ApiResourceID )

COMMIT TRANSACTION
END TRY

BEGIN CATCH
	SELECT ERROR_MESSAGE()
		,ERROR_LINE()
		,ERROR_SEVERITY()

	ROLLBACK TRANSACTION
END CATCH
GO

SET QUOTED_IDENTIFIER OFF
GO
/*
Script Name : 07_Roles
Module_Name : M3Pact
Created By  : Abhishek K
CreatedDate : 05/10/2018

Data Verification Script :

SELECT * FROM [Roles](NOLOCK) WHERE RecordStatus = 'A'
*/

BEGIN TRY
BEGIN TRANSACTION

DECLARE @User NVARCHAR(30) = 'Admin'

INSERT INTO [dbo].[Roles]
           ([RoleCode]
           ,[RoleDesc]
           ,[RecordStatus]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[ModifiedBy]
           ,[ModifiedDate]
           ,[Level])
           SELECT 
            R.RoleCode
           ,R.RoleDesc
           ,'A' AS [RecordStatus]
           ,@User AS [CreatedBy]
           ,GETDATE() AS [CreatedDate]
           ,@User AS [ModifiedBy]
           ,GETDATE() AS [ModifiedDate]
		   ,R.Level AS [Level]
           FROM
           (
           SELECT 'Admin' AS RoleCode , 'Admin' AS RoleDesc , 1 AS Level UNION ALL
           SELECT 'Manager' AS RoleCode , 'Manager' AS RoleDesc, 3 AS Level UNION ALL
           SELECT 'Executive' AS RoleCode , 'Executive' AS RoleDesc, 2 AS Level UNION ALL
           SELECT 'User' AS RoleCode , 'User' AS RoleDesc, 4 AS Level UNION ALL
           SELECT 'Client' AS RoleCode , 'Client' AS RoleDesc, 5 AS Level 
           )
           AS R
 WHERE NOT EXISTS( SELECT 1 FROM [Roles] (NOLOCK) B WHERE B.RoleCode=R.RoleCode) 

COMMIT TRANSACTION
END TRY

BEGIN CATCH
	SELECT ERROR_MESSAGE()
		,ERROR_LINE()
		,ERROR_SEVERITY()

	ROLLBACK TRANSACTION
END CATCH
GO

/*
Script Name : 09_System
Module_Name : M3Pact
Created By  : Abhishek K
CreatedDate : 05/10/2018

Data Verification Script :

SELECT * FROM [System](NOLOCK) WHERE RecordStatus = 'A'
*/

BEGIN TRY
BEGIN TRANSACTION

DECLARE @User NVARCHAR(30) = 'Admin'

INSERT INTO [dbo].[System]
           ([SystemCode]
           ,[SystemName]
           ,[SystemDescription]
           ,[RecordStatus]
           ,[CreatedDate]
           ,[CreatedBy]
           ,[ModifiedDate]
           ,[ModifiedBy])
           SELECT 
            S.SystemCode
           ,S.SystemName
		   ,S.SystemDescription
           ,'A' AS [RecordStatus]
           ,GETDATE() AS [CreatedDate]
           ,@User AS [CreatedBy]
           ,GETDATE() AS [ModifiedDate]
           ,@User AS [ModifiedBy]
           FROM
           (
           SELECT 'Allscripts PM' AS SystemCode , 'Allscripts PM' AS SystemName , 'Allscripts PM' AS SystemDescription  UNION ALL
           SELECT 'Athena' AS SystemCode , 'Athena' AS SystemName , 'Athena' AS SystemDescription  UNION ALL
           SELECT 'Centricity Business' AS SystemCode , 'Centricity Business' AS SystemName , 'Centricity Business' AS SystemDescription  UNION ALL
           SELECT 'Centricity CPS' AS SystemCode , 'Centricity CPS' AS SystemName , 'Centricity CPS' AS SystemDescription  UNION ALL
           SELECT 'Centricity Manager' AS SystemCode , 'Centricity Manager' AS SystemName , 'Centricity Manager' AS SystemDescription  UNION ALL
           SELECT 'Eclinical Works' AS SystemCode , 'Eclinical Works' AS SystemName , 'Eclinical Works' AS SystemDescription  UNION ALL
           SELECT 'EPIC' AS SystemCode , 'EPIC' AS SystemName , 'EPIC' AS SystemDescription  UNION ALL
           SELECT 'Greenway' AS SystemCode , 'Greenway' AS SystemName , 'Greenway' AS SystemDescription  UNION ALL
           SELECT 'VertexDR' AS SystemCode , 'VertexDR' AS SystemName , 'VertexDR' AS SystemDescription  
           )
           AS S
 WHERE NOT EXISTS( SELECT 1 FROM [System] (NOLOCK) B WHERE B.SystemCode=S.SystemCode) 

COMMIT TRANSACTION
END TRY

BEGIN CATCH
	SELECT ERROR_MESSAGE()
		,ERROR_LINE()
		,ERROR_SEVERITY()

	ROLLBACK TRANSACTION
END CATCH
GO

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

/*
Script Name : 10_Screen
Module_Name : M3Pact
Created By  : Abhishek K
CreatedDate : 05/11/2018

Revision History 
=============
Ver  Date				Who						Comment
1.0  10-May-2018		Abhishek Kovvuri		Scripts to insert data
1.1  08-Aug-2018        Abhishek Kovvuri        Scripts to insert to-do-list screen user roles.

Data Verification Script :

SELECT * FROM [ScreenUserRoleMap](NOLOCK) WHERE RecordStatus = 'A'
*/


BEGIN TRY
BEGIN TRANSACTION

DECLARE @UserName NVARCHAR(60) = 'Admin'
DECLARE @CurrentDate DATETIME = GETDATE()
DECLARE @RecordStatus CHAR(1) = 'A'
DECLARE @ScreenId INT;
DECLARE @RoleId INT;

SELECT  @ScreenId = ScreenId FROM Screen WHERE ScreenCode = 'DL'
SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'Admin'
IF NOT EXISTS ( SELECT TOP 1 1 FROM ScreenUserRoleMap WHERE ScreenId = @ScreenId AND RoleId = @RoleId)
	BEGIN
		INSERT INTO [dbo].[ScreenUserRoleMap]
           ([ScreenId]
           ,[RoleId]
           ,[RecordStatus]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[ModifiedBy]
           ,[ModifiedDate]
           ,[CanEdit]
           ,[DisplayScreenName])
		VALUES
		(
			@ScreenId, 			
			@RoleId,
			@RecordStatus,	
			@UserName,				
			@CurrentDate,	
			@UserName,				
			@CurrentDate,	
			1,
			'Deposit Log'
		)
	END

SELECT  @ScreenId = ScreenId FROM Screen WHERE ScreenCode = 'DL'
SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'Manager'
IF NOT EXISTS ( SELECT TOP 1 1 FROM ScreenUserRoleMap WHERE ScreenId = @ScreenId AND RoleId = @RoleId)
	BEGIN
		INSERT INTO [dbo].[ScreenUserRoleMap]
           ([ScreenId]
           ,[RoleId]
           ,[RecordStatus]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[ModifiedBy]
           ,[ModifiedDate]
           ,[CanEdit]
           ,[DisplayScreenName])
		VALUES
		(
			@ScreenId, 			
			@RoleId,
			@RecordStatus,	
			@UserName,				
			@CurrentDate,	
			@UserName,				
			@CurrentDate,	
			1,
			'Deposit Log'
		)
	END

SELECT  @ScreenId = ScreenId FROM Screen WHERE ScreenCode = 'DL'
SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'Executive'
IF NOT EXISTS ( SELECT TOP 1 1 FROM ScreenUserRoleMap WHERE ScreenId = @ScreenId AND RoleId = @RoleId)
	BEGIN
		INSERT INTO [dbo].[ScreenUserRoleMap]
           ([ScreenId]
           ,[RoleId]
           ,[RecordStatus]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[ModifiedBy]
           ,[ModifiedDate]
           ,[CanEdit]
           ,[DisplayScreenName])
		VALUES
		(
			@ScreenId, 			
			@RoleId,
			@RecordStatus,	
			@UserName,				
			@CurrentDate,	
			@UserName,				
			@CurrentDate,	
			1,
			'Deposit Log'
		)
	END

SELECT  @ScreenId = ScreenId FROM Screen WHERE ScreenCode = 'DL'
SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'User'
IF NOT EXISTS ( SELECT TOP 1 1 FROM ScreenUserRoleMap WHERE ScreenId = @ScreenId AND RoleId = @RoleId)
	BEGIN
		INSERT INTO [dbo].[ScreenUserRoleMap]
           ([ScreenId]
           ,[RoleId]
           ,[RecordStatus]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[ModifiedBy]
           ,[ModifiedDate]
           ,[CanEdit]
           ,[DisplayScreenName])
		VALUES
		(
			@ScreenId, 			
			@RoleId,
			@RecordStatus,	
			@UserName,				
			@CurrentDate,	
			@UserName,				
			@CurrentDate,	
			1,
			'Deposit Log'
		)
	END

SELECT  @ScreenId = ScreenId FROM Screen WHERE ScreenCode = 'DL'
SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'Client'
IF NOT EXISTS ( SELECT TOP 1 1 FROM ScreenUserRoleMap WHERE ScreenId = @ScreenId AND RoleId = @RoleId)
	BEGIN
		INSERT INTO [dbo].[ScreenUserRoleMap]
           ([ScreenId]
           ,[RoleId]
           ,[RecordStatus]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[ModifiedBy]
           ,[ModifiedDate]
           ,[CanEdit]
           ,[DisplayScreenName])
		VALUES
		(
			@ScreenId, 			
			@RoleId,
			@RecordStatus,	
			@UserName,				
			@CurrentDate,	
			@UserName,				
			@CurrentDate,	
			1,
			'Deposit Log'
		)
	END

SELECT  @ScreenId = ScreenId FROM Screen WHERE ScreenCode = 'HM'
SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'Manager'
IF NOT EXISTS ( SELECT TOP 1 1 FROM ScreenUserRoleMap WHERE ScreenId = @ScreenId AND RoleId = @RoleId)
	BEGIN
		INSERT INTO [dbo].[ScreenUserRoleMap]
           ([ScreenId]
           ,[RoleId]
           ,[RecordStatus]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[ModifiedBy]
           ,[ModifiedDate]
           ,[CanEdit]
           ,[DisplayScreenName])
		VALUES
		(
			@ScreenId, 			
			@RoleId,
			@RecordStatus,	
			@UserName,				
			@CurrentDate,	
			@UserName,				
			@CurrentDate,	
			NULL,
			'Heat Map'
		)
	END

SELECT  @ScreenId = ScreenId FROM Screen WHERE ScreenCode = 'HM'
SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'Executive'
IF NOT EXISTS ( SELECT TOP 1 1 FROM ScreenUserRoleMap WHERE ScreenId = @ScreenId AND RoleId = @RoleId)
	BEGIN
		INSERT INTO [dbo].[ScreenUserRoleMap]
           ([ScreenId]
           ,[RoleId]
           ,[RecordStatus]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[ModifiedBy]
           ,[ModifiedDate]
           ,[CanEdit]
           ,[DisplayScreenName])
		VALUES
		(
			@ScreenId, 			
			@RoleId,
			@RecordStatus,	
			@UserName,				
			@CurrentDate,	
			@UserName,				
			@CurrentDate,	
			NULL,
			'Heat Map'
		)
	END

SELECT  @ScreenId = ScreenId FROM Screen WHERE ScreenCode = 'HM'
SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'Admin'
IF NOT EXISTS ( SELECT TOP 1 1 FROM ScreenUserRoleMap WHERE ScreenId = @ScreenId AND RoleId = @RoleId)
	BEGIN
		INSERT INTO [dbo].[ScreenUserRoleMap]
           ([ScreenId]
           ,[RoleId]
           ,[RecordStatus]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[ModifiedBy]
           ,[ModifiedDate]
           ,[CanEdit]
           ,[DisplayScreenName])
		VALUES
		(
			@ScreenId, 			
			@RoleId,
			@RecordStatus,	
			@UserName,				
			@CurrentDate,	
			@UserName,				
			@CurrentDate,	
			NULL,
			'Heat Map'
		)
	END

SELECT  @ScreenId = ScreenId FROM Screen WHERE ScreenCode = 'CL'
SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'Admin'
IF NOT EXISTS ( SELECT TOP 1 1 FROM ScreenUserRoleMap WHERE ScreenId = @ScreenId AND RoleId = @RoleId)
	BEGIN
		INSERT INTO [dbo].[ScreenUserRoleMap]
           ([ScreenId]
           ,[RoleId]
           ,[RecordStatus]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[ModifiedBy]
           ,[ModifiedDate]
           ,[CanEdit]
           ,[DisplayScreenName])
		VALUES
		(
			@ScreenId, 			
			@RoleId,
			@RecordStatus,	
			@UserName,				
			@CurrentDate,	
			@UserName,				
			@CurrentDate,	
			NULL,
			'CheckLists'
		)
	END

SELECT  @ScreenId = ScreenId FROM Screen WHERE ScreenCode = 'CL'
SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'Manager'
IF NOT EXISTS ( SELECT TOP 1 1 FROM ScreenUserRoleMap WHERE ScreenId = @ScreenId AND RoleId = @RoleId)
	BEGIN
		INSERT INTO [dbo].[ScreenUserRoleMap]
           ([ScreenId]
           ,[RoleId]
           ,[RecordStatus]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[ModifiedBy]
           ,[ModifiedDate]
           ,[CanEdit]
           ,[DisplayScreenName])
		VALUES
		(
			@ScreenId, 			
			@RoleId,
			@RecordStatus,	
			@UserName,				
			@CurrentDate,	
			@UserName,				
			@CurrentDate,	
			NULL,
			'CheckLists'
		)
	END

SELECT  @ScreenId = ScreenId FROM Screen WHERE ScreenCode = 'CL'
SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'Executive'
IF NOT EXISTS ( SELECT TOP 1 1 FROM ScreenUserRoleMap WHERE ScreenId = @ScreenId AND RoleId = @RoleId)
	BEGIN
		INSERT INTO [dbo].[ScreenUserRoleMap]
           ([ScreenId]
           ,[RoleId]
           ,[RecordStatus]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[ModifiedBy]
           ,[ModifiedDate]
           ,[CanEdit]
           ,[DisplayScreenName])
		VALUES
		(
			@ScreenId, 			
			@RoleId,
			@RecordStatus,	
			@UserName,				
			@CurrentDate,	
			@UserName,				
			@CurrentDate,	
			NULL,
			'CheckLists'
		)
	END

SELECT  @ScreenId = ScreenId FROM Screen WHERE ScreenCode = 'CL'
SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'User'
IF NOT EXISTS ( SELECT TOP 1 1 FROM ScreenUserRoleMap WHERE ScreenId = @ScreenId AND RoleId = @RoleId)
	BEGIN
		INSERT INTO [dbo].[ScreenUserRoleMap]
           ([ScreenId]
           ,[RoleId]
           ,[RecordStatus]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[ModifiedBy]
           ,[ModifiedDate]
           ,[CanEdit]
           ,[DisplayScreenName])
		VALUES
		(
			@ScreenId, 			
			@RoleId,
			@RecordStatus,	
			@UserName,				
			@CurrentDate,	
			@UserName,				
			@CurrentDate,	
			NULL,
			'CheckLists'
		)
	END

SELECT  @ScreenId = ScreenId FROM Screen WHERE ScreenCode = 'PCL'
SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'User'
IF NOT EXISTS ( SELECT TOP 1 1 FROM ScreenUserRoleMap WHERE ScreenId = @ScreenId AND RoleId = @RoleId)
	BEGIN
		INSERT INTO [dbo].[ScreenUserRoleMap]
           ([ScreenId]
           ,[RoleId]
           ,[RecordStatus]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[ModifiedBy]
           ,[ModifiedDate]
           ,[CanEdit]
           ,[DisplayScreenName])
		VALUES
		(
			@ScreenId, 			
			@RoleId,
			@RecordStatus,	
			@UserName,				
			@CurrentDate,	
			@UserName,				
			@CurrentDate,	
			NULL,
			'Pending CheckLists'
		)
	END

SELECT  @ScreenId = ScreenId FROM Screen WHERE ScreenCode = 'PCL'
SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'Executive'
IF NOT EXISTS ( SELECT TOP 1 1 FROM ScreenUserRoleMap WHERE ScreenId = @ScreenId AND RoleId = @RoleId)
	BEGIN
		INSERT INTO [dbo].[ScreenUserRoleMap]
           ([ScreenId]
           ,[RoleId]
           ,[RecordStatus]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[ModifiedBy]
           ,[ModifiedDate]
           ,[CanEdit]
           ,[DisplayScreenName])
		VALUES
		(
			@ScreenId, 			
			@RoleId,
			@RecordStatus,	
			@UserName,				
			@CurrentDate,	
			@UserName,				
			@CurrentDate,	
			NULL,
			'Pending CheckLists'
		)
	END

SELECT  @ScreenId = ScreenId FROM Screen WHERE ScreenCode = 'PCL'
SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'Manager'
IF NOT EXISTS ( SELECT TOP 1 1 FROM ScreenUserRoleMap WHERE ScreenId = @ScreenId AND RoleId = @RoleId)
	BEGIN
		INSERT INTO [dbo].[ScreenUserRoleMap]
           ([ScreenId]
           ,[RoleId]
           ,[RecordStatus]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[ModifiedBy]
           ,[ModifiedDate]
           ,[CanEdit]
           ,[DisplayScreenName])
		VALUES
		(
			@ScreenId, 			
			@RoleId,
			@RecordStatus,	
			@UserName,				
			@CurrentDate,	
			@UserName,				
			@CurrentDate,	
			NULL,
			'Pending CheckLists'
		)
	END

SELECT  @ScreenId = ScreenId FROM Screen WHERE ScreenCode = 'PCL'
SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'Admin'
IF NOT EXISTS ( SELECT TOP 1 1 FROM ScreenUserRoleMap WHERE ScreenId = @ScreenId AND RoleId = @RoleId)
	BEGIN
		INSERT INTO [dbo].[ScreenUserRoleMap]
           ([ScreenId]
           ,[RoleId]
           ,[RecordStatus]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[ModifiedBy]
           ,[ModifiedDate]
           ,[CanEdit]
           ,[DisplayScreenName])
		VALUES
		(
			@ScreenId, 			
			@RoleId,
			@RecordStatus,	
			@UserName,				
			@CurrentDate,	
			@UserName,				
			@CurrentDate,	
			NULL,
			'Pending CheckLists'
		)
	END

SELECT  @ScreenId = ScreenId FROM Screen WHERE ScreenCode = 'CCL'
SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'Admin'
IF NOT EXISTS ( SELECT TOP 1 1 FROM ScreenUserRoleMap WHERE ScreenId = @ScreenId AND RoleId = @RoleId)
	BEGIN
		INSERT INTO [dbo].[ScreenUserRoleMap]
           ([ScreenId]
           ,[RoleId]
           ,[RecordStatus]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[ModifiedBy]
           ,[ModifiedDate]
           ,[CanEdit]
           ,[DisplayScreenName])
		VALUES
		(
			@ScreenId, 			
			@RoleId,
			@RecordStatus,	
			@UserName,				
			@CurrentDate,	
			@UserName,				
			@CurrentDate,	
			1,
			'Completed CheckLists'
		)
	END

SELECT  @ScreenId = ScreenId FROM Screen WHERE ScreenCode = 'CCL'
SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'Manager'
IF NOT EXISTS ( SELECT TOP 1 1 FROM ScreenUserRoleMap WHERE ScreenId = @ScreenId AND RoleId = @RoleId)
	BEGIN
		INSERT INTO [dbo].[ScreenUserRoleMap]
           ([ScreenId]
           ,[RoleId]
           ,[RecordStatus]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[ModifiedBy]
           ,[ModifiedDate]
           ,[CanEdit]
           ,[DisplayScreenName])
		VALUES
		(
			@ScreenId, 			
			@RoleId,
			@RecordStatus,	
			@UserName,				
			@CurrentDate,	
			@UserName,				
			@CurrentDate,	
			0,
			'Completed CheckLists'
		)
	END

SELECT  @ScreenId = ScreenId FROM Screen WHERE ScreenCode = 'CCL'
SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'Executive'
IF NOT EXISTS ( SELECT TOP 1 1 FROM ScreenUserRoleMap WHERE ScreenId = @ScreenId AND RoleId = @RoleId)
	BEGIN
		INSERT INTO [dbo].[ScreenUserRoleMap]
           ([ScreenId]
           ,[RoleId]
           ,[RecordStatus]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[ModifiedBy]
           ,[ModifiedDate]
           ,[CanEdit]
           ,[DisplayScreenName])
		VALUES
		(
			@ScreenId, 			
			@RoleId,
			@RecordStatus,	
			@UserName,				
			@CurrentDate,	
			@UserName,				
			@CurrentDate,	
			0,
			'Completed CheckLists'
		)
	END

SELECT  @ScreenId = ScreenId FROM Screen WHERE ScreenCode = 'CCL'
SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'User'
IF NOT EXISTS ( SELECT TOP 1 1 FROM ScreenUserRoleMap WHERE ScreenId = @ScreenId AND RoleId = @RoleId)
	BEGIN
		INSERT INTO [dbo].[ScreenUserRoleMap]
           ([ScreenId]
           ,[RoleId]
           ,[RecordStatus]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[ModifiedBy]
           ,[ModifiedDate]
           ,[CanEdit]
           ,[DisplayScreenName])
		VALUES
		(
			@ScreenId, 			
			@RoleId,
			@RecordStatus,	
			@UserName,				
			@CurrentDate,	
			@UserName,				
			@CurrentDate,	
			0,
			'Completed CheckLists'
		)
	END

SELECT  @ScreenId = ScreenId FROM Screen WHERE ScreenCode = 'CLI'
SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'Admin'
IF NOT EXISTS ( SELECT TOP 1 1 FROM ScreenUserRoleMap WHERE ScreenId = @ScreenId AND RoleId = @RoleId)
	BEGIN
		INSERT INTO [dbo].[ScreenUserRoleMap]
           ([ScreenId]
           ,[RoleId]
           ,[RecordStatus]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[ModifiedBy]
           ,[ModifiedDate]
           ,[CanEdit]
           ,[DisplayScreenName])
		VALUES
		(
			@ScreenId, 			
			@RoleId,
			@RecordStatus,	
			@UserName,				
			@CurrentDate,	
			@UserName,				
			@CurrentDate,	
			NULL,
			'Clients'
		)
	END

SELECT  @ScreenId = ScreenId FROM Screen WHERE ScreenCode = 'CLI'
SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'Manager'
IF NOT EXISTS ( SELECT TOP 1 1 FROM ScreenUserRoleMap WHERE ScreenId = @ScreenId AND RoleId = @RoleId)
	BEGIN
		INSERT INTO [dbo].[ScreenUserRoleMap]
           ([ScreenId]
           ,[RoleId]
           ,[RecordStatus]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[ModifiedBy]
           ,[ModifiedDate]
           ,[CanEdit]
           ,[DisplayScreenName])
		VALUES
		(
			@ScreenId, 			
			@RoleId,
			@RecordStatus,	
			@UserName,				
			@CurrentDate,	
			@UserName,				
			@CurrentDate,	
			NULL,
			'Clients'
		)
	END

SELECT  @ScreenId = ScreenId FROM Screen WHERE ScreenCode = 'CLI'
SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'Executive'
IF NOT EXISTS ( SELECT TOP 1 1 FROM ScreenUserRoleMap WHERE ScreenId = @ScreenId AND RoleId = @RoleId)
	BEGIN
		INSERT INTO [dbo].[ScreenUserRoleMap]
           ([ScreenId]
           ,[RoleId]
           ,[RecordStatus]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[ModifiedBy]
           ,[ModifiedDate]
           ,[CanEdit]
           ,[DisplayScreenName])
		VALUES
		(
			@ScreenId, 			
			@RoleId,
			@RecordStatus,	
			@UserName,				
			@CurrentDate,	
			@UserName,				
			@CurrentDate,	
			NULL,
			'Clients'
		)
	END

SELECT  @ScreenId = ScreenId FROM Screen WHERE ScreenCode = 'CLI'
SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'User'
IF NOT EXISTS ( SELECT TOP 1 1 FROM ScreenUserRoleMap WHERE ScreenId = @ScreenId AND RoleId = @RoleId)
	BEGIN
		INSERT INTO [dbo].[ScreenUserRoleMap]
           ([ScreenId]
           ,[RoleId]
           ,[RecordStatus]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[ModifiedBy]
           ,[ModifiedDate]
           ,[CanEdit]
           ,[DisplayScreenName])
		VALUES
		(
			@ScreenId, 			
			@RoleId,
			@RecordStatus,	
			@UserName,				
			@CurrentDate,	
			@UserName,				
			@CurrentDate,	
			NULL,
			'Clients'
		)
	END

SELECT  @ScreenId = ScreenId FROM Screen WHERE ScreenCode = 'CLI'
SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'Client'
IF NOT EXISTS ( SELECT TOP 1 1 FROM ScreenUserRoleMap WHERE ScreenId = @ScreenId AND RoleId = @RoleId)
	BEGIN
		INSERT INTO [dbo].[ScreenUserRoleMap]
           ([ScreenId]
           ,[RoleId]
           ,[RecordStatus]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[ModifiedBy]
           ,[ModifiedDate]
           ,[CanEdit]
           ,[DisplayScreenName])
		VALUES
		(
			@ScreenId, 			
			@RoleId,
			@RecordStatus,	
			@UserName,				
			@CurrentDate,	
			@UserName,				
			@CurrentDate,	
			NULL,
			'Clients'
		)
	END

SELECT  @ScreenId = ScreenId FROM Screen WHERE ScreenCode = 'CVE'
SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'Admin'
IF NOT EXISTS ( SELECT TOP 1 1 FROM ScreenUserRoleMap WHERE ScreenId = @ScreenId AND RoleId = @RoleId)
	BEGIN
		INSERT INTO [dbo].[ScreenUserRoleMap]
           ([ScreenId]
           ,[RoleId]
           ,[RecordStatus]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[ModifiedBy]
           ,[ModifiedDate]
           ,[CanEdit]
           ,[DisplayScreenName])
		VALUES
		(
			@ScreenId, 			
			@RoleId,
			@RecordStatus,	
			@UserName,				
			@CurrentDate,	
			@UserName,				
			@CurrentDate,	
			NULL,
			'View/Edit'
		)
	END

SELECT  @ScreenId = ScreenId FROM Screen WHERE ScreenCode = 'CVE'
SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'Manager'
IF NOT EXISTS ( SELECT TOP 1 1 FROM ScreenUserRoleMap WHERE ScreenId = @ScreenId AND RoleId = @RoleId)
	BEGIN
		INSERT INTO [dbo].[ScreenUserRoleMap]
           ([ScreenId]
           ,[RoleId]
           ,[RecordStatus]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[ModifiedBy]
           ,[ModifiedDate]
           ,[CanEdit]
           ,[DisplayScreenName])
		VALUES
		(
			@ScreenId, 			
			@RoleId,
			@RecordStatus,	
			@UserName,				
			@CurrentDate,	
			@UserName,				
			@CurrentDate,	
			NULL,
			'View/Edit'
		)
	END

SELECT  @ScreenId = ScreenId FROM Screen WHERE ScreenCode = 'CVE'
SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'Executive'
IF NOT EXISTS ( SELECT TOP 1 1 FROM ScreenUserRoleMap WHERE ScreenId = @ScreenId AND RoleId = @RoleId)
	BEGIN
		INSERT INTO [dbo].[ScreenUserRoleMap]
           ([ScreenId]
           ,[RoleId]
           ,[RecordStatus]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[ModifiedBy]
           ,[ModifiedDate]
           ,[CanEdit]
           ,[DisplayScreenName])
		VALUES
		(
			@ScreenId, 			
			@RoleId,
			@RecordStatus,	
			@UserName,				
			@CurrentDate,	
			@UserName,				
			@CurrentDate,	
			NULL,
			'View/Edit'
		)
	END

SELECT  @ScreenId = ScreenId FROM Screen WHERE ScreenCode = 'CVE'
SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'User'
IF NOT EXISTS ( SELECT TOP 1 1 FROM ScreenUserRoleMap WHERE ScreenId = @ScreenId AND RoleId = @RoleId)
	BEGIN
		INSERT INTO [dbo].[ScreenUserRoleMap]
           ([ScreenId]
           ,[RoleId]
           ,[RecordStatus]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[ModifiedBy]
           ,[ModifiedDate]
           ,[CanEdit]
           ,[DisplayScreenName])
		VALUES
		(
			@ScreenId, 			
			@RoleId,
			@RecordStatus,	
			@UserName,				
			@CurrentDate,	
			@UserName,				
			@CurrentDate,	
			NULL,
			'View/Edit'
		)
	END

SELECT  @ScreenId = ScreenId FROM Screen WHERE ScreenCode = 'CVE'
SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'Client'
IF NOT EXISTS ( SELECT TOP 1 1 FROM ScreenUserRoleMap WHERE ScreenId = @ScreenId AND RoleId = @RoleId)
	BEGIN
		INSERT INTO [dbo].[ScreenUserRoleMap]
           ([ScreenId]
           ,[RoleId]
           ,[RecordStatus]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[ModifiedBy]
           ,[ModifiedDate]
           ,[CanEdit]
           ,[DisplayScreenName])
		VALUES
		(
			@ScreenId, 			
			@RoleId,
			@RecordStatus,	
			@UserName,				
			@CurrentDate,	
			@UserName,				
			@CurrentDate,	
			NULL,
			'View/Edit'
		)
	END

SELECT  @ScreenId = ScreenId FROM Screen WHERE ScreenCode = 'CN'
SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'Admin'
IF NOT EXISTS ( SELECT TOP 1 1 FROM ScreenUserRoleMap WHERE ScreenId = @ScreenId AND RoleId = @RoleId)
	BEGIN
		INSERT INTO [dbo].[ScreenUserRoleMap]
           ([ScreenId]
           ,[RoleId]
           ,[RecordStatus]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[ModifiedBy]
           ,[ModifiedDate]
           ,[CanEdit]
           ,[DisplayScreenName])
		VALUES
		(
			@ScreenId, 			
			@RoleId,
			@RecordStatus,	
			@UserName,				
			@CurrentDate,	
			@UserName,				
			@CurrentDate,	
			NULL,
			'Create New'
		)
	END

SELECT  @ScreenId = ScreenId FROM Screen WHERE ScreenCode = 'VA'
SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'Admin'
IF NOT EXISTS ( SELECT TOP 1 1 FROM ScreenUserRoleMap WHERE ScreenId = @ScreenId AND RoleId = @RoleId)
	BEGIN
		INSERT INTO [dbo].[ScreenUserRoleMap]
           ([ScreenId]
           ,[RoleId]
           ,[RecordStatus]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[ModifiedBy]
           ,[ModifiedDate]
           ,[CanEdit]
           ,[DisplayScreenName])
		VALUES
		(
			@ScreenId, 			
			@RoleId,
			@RecordStatus,	
			@UserName,				
			@CurrentDate,	
			@UserName,				
			@CurrentDate,	
			1,
			'View All'
		)
	END

SELECT  @ScreenId = ScreenId FROM Screen WHERE ScreenCode = 'VA'
SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'Manager'
IF NOT EXISTS ( SELECT TOP 1 1 FROM ScreenUserRoleMap WHERE ScreenId = @ScreenId AND RoleId = @RoleId)
	BEGIN
		INSERT INTO [dbo].[ScreenUserRoleMap]
           ([ScreenId]
           ,[RoleId]
           ,[RecordStatus]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[ModifiedBy]
           ,[ModifiedDate]
           ,[CanEdit]
           ,[DisplayScreenName])
		VALUES
		(
			@ScreenId, 			
			@RoleId,
			@RecordStatus,	
			@UserName,				
			@CurrentDate,	
			@UserName,				
			@CurrentDate,	
			1,
			'View All'
		)
	END

SELECT  @ScreenId = ScreenId FROM Screen WHERE ScreenCode = 'VA'
SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'Executive'
IF NOT EXISTS ( SELECT TOP 1 1 FROM ScreenUserRoleMap WHERE ScreenId = @ScreenId AND RoleId = @RoleId)
	BEGIN
		INSERT INTO [dbo].[ScreenUserRoleMap]
           ([ScreenId]
           ,[RoleId]
           ,[RecordStatus]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[ModifiedBy]
           ,[ModifiedDate]
           ,[CanEdit]
           ,[DisplayScreenName])
		VALUES
		(
			@ScreenId, 			
			@RoleId,
			@RecordStatus,	
			@UserName,				
			@CurrentDate,	
			@UserName,				
			@CurrentDate,	
			1,
			'View All'
		)
	END

SELECT  @ScreenId = ScreenId FROM Screen WHERE ScreenCode = 'VA'
SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'User'
IF NOT EXISTS ( SELECT TOP 1 1 FROM ScreenUserRoleMap WHERE ScreenId = @ScreenId AND RoleId = @RoleId)
	BEGIN
		INSERT INTO [dbo].[ScreenUserRoleMap]
           ([ScreenId]
           ,[RoleId]
           ,[RecordStatus]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[ModifiedBy]
           ,[ModifiedDate]
           ,[CanEdit]
           ,[DisplayScreenName])
		VALUES
		(
			@ScreenId, 			
			@RoleId,
			@RecordStatus,	
			@UserName,				
			@CurrentDate,	
			@UserName,				
			@CurrentDate,	
			1,
			'View All'
		)
	END

SELECT  @ScreenId = ScreenId FROM Screen WHERE ScreenCode = 'VA'
SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'Client'
IF NOT EXISTS ( SELECT TOP 1 1 FROM ScreenUserRoleMap WHERE ScreenId = @ScreenId AND RoleId = @RoleId)
	BEGIN
		INSERT INTO [dbo].[ScreenUserRoleMap]
           ([ScreenId]
           ,[RoleId]
           ,[RecordStatus]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[ModifiedBy]
           ,[ModifiedDate]
           ,[CanEdit]
           ,[DisplayScreenName])
		VALUES
		(
			@ScreenId, 			
			@RoleId,
			@RecordStatus,	
			@UserName,				
			@CurrentDate,	
			@UserName,				
			@CurrentDate,	
			0,
			'View All'
		)
	END

SELECT  @ScreenId = ScreenId FROM Screen WHERE ScreenCode = 'ADMIN'
SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'Admin'
IF NOT EXISTS ( SELECT TOP 1 1 FROM ScreenUserRoleMap WHERE ScreenId = @ScreenId AND RoleId = @RoleId)
	BEGIN
		INSERT INTO [dbo].[ScreenUserRoleMap]
           ([ScreenId]
           ,[RoleId]
           ,[RecordStatus]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[ModifiedBy]
           ,[ModifiedDate]
           ,[CanEdit]
           ,[DisplayScreenName])
		VALUES
		(
			@ScreenId, 			
			@RoleId,
			@RecordStatus,	
			@UserName,				
			@CurrentDate,	
			@UserName,				
			@CurrentDate,	
			NULL,
			'Administration'
		)
	END

SELECT  @ScreenId = ScreenId FROM Screen WHERE ScreenCode = 'ADMIN'
SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'Manager'
IF NOT EXISTS ( SELECT TOP 1 1 FROM ScreenUserRoleMap WHERE ScreenId = @ScreenId AND RoleId = @RoleId)
	BEGIN
		INSERT INTO [dbo].[ScreenUserRoleMap]
           ([ScreenId]
           ,[RoleId]
           ,[RecordStatus]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[ModifiedBy]
           ,[ModifiedDate]
           ,[CanEdit]
           ,[DisplayScreenName])
		VALUES
		(
			@ScreenId, 			
			@RoleId,
			@RecordStatus,	
			@UserName,				
			@CurrentDate,	
			@UserName,				
			@CurrentDate,	
			NULL,
			'Administration'
		)
	END

SELECT  @ScreenId = ScreenId FROM Screen WHERE ScreenCode = 'ADMIN'
SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'Executive'
IF NOT EXISTS ( SELECT TOP 1 1 FROM ScreenUserRoleMap WHERE ScreenId = @ScreenId AND RoleId = @RoleId)
	BEGIN
		INSERT INTO [dbo].[ScreenUserRoleMap]
           ([ScreenId]
           ,[RoleId]
           ,[RecordStatus]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[ModifiedBy]
           ,[ModifiedDate]
           ,[CanEdit]
           ,[DisplayScreenName])
		VALUES
		(
			@ScreenId, 			
			@RoleId,
			@RecordStatus,	
			@UserName,				
			@CurrentDate,	
			@UserName,				
			@CurrentDate,	
			NULL,
			'Administration'
		)
	END

SELECT  @ScreenId = ScreenId FROM Screen WHERE ScreenCode = 'ADMIN'
SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'User'
IF NOT EXISTS ( SELECT TOP 1 1 FROM ScreenUserRoleMap WHERE ScreenId = @ScreenId AND RoleId = @RoleId)
	BEGIN
		INSERT INTO [dbo].[ScreenUserRoleMap]
           ([ScreenId]
           ,[RoleId]
           ,[RecordStatus]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[ModifiedBy]
           ,[ModifiedDate]
           ,[CanEdit]
           ,[DisplayScreenName])
		VALUES
		(
			@ScreenId, 			
			@RoleId,
			@RecordStatus,	
			@UserName,				
			@CurrentDate,	
			@UserName,				
			@CurrentDate,	
			NULL,
			'Administration'
		)
	END

SELECT  @ScreenId = ScreenId FROM Screen WHERE ScreenCode = 'BD'
SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'Admin'
IF NOT EXISTS ( SELECT TOP 1 1 FROM ScreenUserRoleMap WHERE ScreenId = @ScreenId AND RoleId = @RoleId)
	BEGIN
		INSERT INTO [dbo].[ScreenUserRoleMap]
           ([ScreenId]
           ,[RoleId]
           ,[RecordStatus]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[ModifiedBy]
           ,[ModifiedDate]
           ,[CanEdit]
           ,[DisplayScreenName])
		VALUES
		(
			@ScreenId, 			
			@RoleId,
			@RecordStatus,	
			@UserName,				
			@CurrentDate,	
			@UserName,				
			@CurrentDate,	
			1,
			'Business Days'
		)
	END

SELECT  @ScreenId = ScreenId FROM Screen WHERE ScreenCode = 'BD'
SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'Executive'
IF NOT EXISTS ( SELECT TOP 1 1 FROM ScreenUserRoleMap WHERE ScreenId = @ScreenId AND RoleId = @RoleId)
	BEGIN
		INSERT INTO [dbo].[ScreenUserRoleMap]
           ([ScreenId]
           ,[RoleId]
           ,[RecordStatus]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[ModifiedBy]
           ,[ModifiedDate]
           ,[CanEdit]
           ,[DisplayScreenName])
		VALUES
		(
			@ScreenId, 			
			@RoleId,
			@RecordStatus,	
			@UserName,				
			@CurrentDate,	
			@UserName,				
			@CurrentDate,	
			0,
			'Business Days'
		)
	END

SELECT  @ScreenId = ScreenId FROM Screen WHERE ScreenCode = 'SITES'
SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'Executive'
IF NOT EXISTS ( SELECT TOP 1 1 FROM ScreenUserRoleMap WHERE ScreenId = @ScreenId AND RoleId = @RoleId)
	BEGIN
		INSERT INTO [dbo].[ScreenUserRoleMap]
           ([ScreenId]
           ,[RoleId]
           ,[RecordStatus]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[ModifiedBy]
           ,[ModifiedDate]
           ,[CanEdit]
           ,[DisplayScreenName])
		VALUES
		(
			@ScreenId, 			
			@RoleId,
			@RecordStatus,	
			@UserName,				
			@CurrentDate,	
			@UserName,				
			@CurrentDate,	
			0,
			'Sites'
		)
	END

SELECT  @ScreenId = ScreenId FROM Screen WHERE ScreenCode = 'SITES'
SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'Admin'
IF NOT EXISTS ( SELECT TOP 1 1 FROM ScreenUserRoleMap WHERE ScreenId = @ScreenId AND RoleId = @RoleId)
	BEGIN
		INSERT INTO [dbo].[ScreenUserRoleMap]
           ([ScreenId]
           ,[RoleId]
           ,[RecordStatus]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[ModifiedBy]
           ,[ModifiedDate]
           ,[CanEdit]
           ,[DisplayScreenName])
		VALUES
		(
			@ScreenId, 			
			@RoleId,
			@RecordStatus,	
			@UserName,				
			@CurrentDate,	
			@UserName,				
			@CurrentDate,	
			1,
			'Sites'
		)
	END

SELECT  @ScreenId = ScreenId FROM Screen WHERE ScreenCode = 'BU'
SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'Admin'
IF NOT EXISTS ( SELECT TOP 1 1 FROM ScreenUserRoleMap WHERE ScreenId = @ScreenId AND RoleId = @RoleId)
	BEGIN
		INSERT INTO [dbo].[ScreenUserRoleMap]
           ([ScreenId]
           ,[RoleId]
           ,[RecordStatus]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[ModifiedBy]
           ,[ModifiedDate]
           ,[CanEdit]
           ,[DisplayScreenName])
		VALUES
		(
			@ScreenId, 			
			@RoleId,
			@RecordStatus,	
			@UserName,				
			@CurrentDate,	
			@UserName,				
			@CurrentDate,	
			1,
			'Business Units'
		)
	END

SELECT  @ScreenId = ScreenId FROM Screen WHERE ScreenCode = 'BU'
SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'Executive'
IF NOT EXISTS ( SELECT TOP 1 1 FROM ScreenUserRoleMap WHERE ScreenId = @ScreenId AND RoleId = @RoleId)
	BEGIN
		INSERT INTO [dbo].[ScreenUserRoleMap]
           ([ScreenId]
           ,[RoleId]
           ,[RecordStatus]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[ModifiedBy]
           ,[ModifiedDate]
           ,[CanEdit]
           ,[DisplayScreenName])
		VALUES
		(
			@ScreenId, 			
			@RoleId,
			@RecordStatus,	
			@UserName,				
			@CurrentDate,	
			@UserName,				
			@CurrentDate,	
			0,
			'Business Units'
		)
	END

SELECT  @ScreenId = ScreenId FROM Screen WHERE ScreenCode = 'ACL'
SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'Executive'
IF NOT EXISTS ( SELECT TOP 1 1 FROM ScreenUserRoleMap WHERE ScreenId = @ScreenId AND RoleId = @RoleId)
	BEGIN
		INSERT INTO [dbo].[ScreenUserRoleMap]
           ([ScreenId]
           ,[RoleId]
           ,[RecordStatus]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[ModifiedBy]
           ,[ModifiedDate]
           ,[CanEdit]
           ,[DisplayScreenName])
		VALUES
		(
			@ScreenId, 			
			@RoleId,
			@RecordStatus,	
			@UserName,				
			@CurrentDate,	
			@UserName,				
			@CurrentDate,	
			NULL,
			'CheckLists'
		)
	END

SELECT  @ScreenId = ScreenId FROM Screen WHERE ScreenCode = 'ACL'
SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'Admin'
IF NOT EXISTS ( SELECT TOP 1 1 FROM ScreenUserRoleMap WHERE ScreenId = @ScreenId AND RoleId = @RoleId)
	BEGIN
		INSERT INTO [dbo].[ScreenUserRoleMap]
           ([ScreenId]
           ,[RoleId]
           ,[RecordStatus]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[ModifiedBy]
           ,[ModifiedDate]
           ,[CanEdit]
           ,[DisplayScreenName])
		VALUES
		(
			@ScreenId, 			
			@RoleId,
			@RecordStatus,	
			@UserName,				
			@CurrentDate,	
			@UserName,				
			@CurrentDate,	
			NULL,
			'CheckLists'
		)
	END

SELECT  @ScreenId = ScreenId FROM Screen WHERE ScreenCode = 'KPI'
SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'Admin'
IF NOT EXISTS ( SELECT TOP 1 1 FROM ScreenUserRoleMap WHERE ScreenId = @ScreenId AND RoleId = @RoleId)
	BEGIN
		INSERT INTO [dbo].[ScreenUserRoleMap]
           ([ScreenId]
           ,[RoleId]
           ,[RecordStatus]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[ModifiedBy]
           ,[ModifiedDate]
           ,[CanEdit]
           ,[DisplayScreenName])
		VALUES
		(
			@ScreenId, 			
			@RoleId,
			@RecordStatus,	
			@UserName,				
			@CurrentDate,	
			@UserName,				
			@CurrentDate,	
			NULL,
			'KPI'
		)
	END

SELECT  @ScreenId = ScreenId FROM Screen WHERE ScreenCode = 'KPI'
SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'Manager'
IF NOT EXISTS ( SELECT TOP 1 1 FROM ScreenUserRoleMap WHERE ScreenId = @ScreenId AND RoleId = @RoleId)
	BEGIN
		INSERT INTO [dbo].[ScreenUserRoleMap]
           ([ScreenId]
           ,[RoleId]
           ,[RecordStatus]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[ModifiedBy]
           ,[ModifiedDate]
           ,[CanEdit]
           ,[DisplayScreenName])
		VALUES
		(
			@ScreenId, 			
			@RoleId,
			@RecordStatus,	
			@UserName,				
			@CurrentDate,	
			@UserName,				
			@CurrentDate,	
			NULL,
			'KPI'
		)
	END

SELECT  @ScreenId = ScreenId FROM Screen WHERE ScreenCode = 'KPI'
SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'Executive'
IF NOT EXISTS ( SELECT TOP 1 1 FROM ScreenUserRoleMap WHERE ScreenId = @ScreenId AND RoleId = @RoleId)
	BEGIN
		INSERT INTO [dbo].[ScreenUserRoleMap]
           ([ScreenId]
           ,[RoleId]
           ,[RecordStatus]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[ModifiedBy]
           ,[ModifiedDate]
           ,[CanEdit]
           ,[DisplayScreenName])
		VALUES
		(
			@ScreenId, 			
			@RoleId,
			@RecordStatus,	
			@UserName,				
			@CurrentDate,	
			@UserName,				
			@CurrentDate,	
			NULL,
			'KPI'
		)
	END

SELECT  @ScreenId = ScreenId FROM Screen WHERE ScreenCode = 'AHM'
SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'Executive'
IF NOT EXISTS ( SELECT TOP 1 1 FROM ScreenUserRoleMap WHERE ScreenId = @ScreenId AND RoleId = @RoleId)
	BEGIN
		INSERT INTO [dbo].[ScreenUserRoleMap]
           ([ScreenId]
           ,[RoleId]
           ,[RecordStatus]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[ModifiedBy]
           ,[ModifiedDate]
           ,[CanEdit]
           ,[DisplayScreenName])
		VALUES
		(
			@ScreenId, 			
			@RoleId,
			@RecordStatus,	
			@UserName,				
			@CurrentDate,	
			@UserName,				
			@CurrentDate,	
			NULL,
			'Heat Map Setup'
		)
	END

SELECT  @ScreenId = ScreenId FROM Screen WHERE ScreenCode = 'AHM'
SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'Admin'
IF NOT EXISTS ( SELECT TOP 1 1 FROM ScreenUserRoleMap WHERE ScreenId = @ScreenId AND RoleId = @RoleId)
	BEGIN
		INSERT INTO [dbo].[ScreenUserRoleMap]
           ([ScreenId]
           ,[RoleId]
           ,[RecordStatus]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[ModifiedBy]
           ,[ModifiedDate]
           ,[CanEdit]
           ,[DisplayScreenName])
		VALUES
		(
			@ScreenId, 			
			@RoleId,
			@RecordStatus,	
			@UserName,				
			@CurrentDate,	
			@UserName,				
			@CurrentDate,	
			NULL,
			'Heat Map Setup'
		)
	END

SELECT  @ScreenId = ScreenId FROM Screen WHERE ScreenCode = 'PAY'
SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'Admin'
IF NOT EXISTS ( SELECT TOP 1 1 FROM ScreenUserRoleMap WHERE ScreenId = @ScreenId AND RoleId = @RoleId)
	BEGIN
		INSERT INTO [dbo].[ScreenUserRoleMap]
           ([ScreenId]
           ,[RoleId]
           ,[RecordStatus]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[ModifiedBy]
           ,[ModifiedDate]
           ,[CanEdit]
           ,[DisplayScreenName])
		VALUES
		(
			@ScreenId, 			
			@RoleId,
			@RecordStatus,	
			@UserName,				
			@CurrentDate,	
			@UserName,				
			@CurrentDate,	
			1,
			'Payers'
		)
	END

SELECT  @ScreenId = ScreenId FROM Screen WHERE ScreenCode = 'PAY'
SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'Manager'
IF NOT EXISTS ( SELECT TOP 1 1 FROM ScreenUserRoleMap WHERE ScreenId = @ScreenId AND RoleId = @RoleId)
	BEGIN
		INSERT INTO [dbo].[ScreenUserRoleMap]
           ([ScreenId]
           ,[RoleId]
           ,[RecordStatus]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[ModifiedBy]
           ,[ModifiedDate]
           ,[CanEdit]
           ,[DisplayScreenName])
		VALUES
		(
			@ScreenId, 			
			@RoleId,
			@RecordStatus,	
			@UserName,				
			@CurrentDate,	
			@UserName,				
			@CurrentDate,	
			1,
			'Payers'
		)
	END

SELECT  @ScreenId = ScreenId FROM Screen WHERE ScreenCode = 'PAY'
SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'Executive'
IF NOT EXISTS ( SELECT TOP 1 1 FROM ScreenUserRoleMap WHERE ScreenId = @ScreenId AND RoleId = @RoleId)
	BEGIN
		INSERT INTO [dbo].[ScreenUserRoleMap]
           ([ScreenId]
           ,[RoleId]
           ,[RecordStatus]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[ModifiedBy]
           ,[ModifiedDate]
           ,[CanEdit]
           ,[DisplayScreenName])
		VALUES
		(
			@ScreenId, 			
			@RoleId,
			@RecordStatus,	
			@UserName,				
			@CurrentDate,	
			@UserName,				
			@CurrentDate,	
			1,
			'Payers'
		)
	END

SELECT  @ScreenId = ScreenId FROM Screen WHERE ScreenCode = 'PAY'
SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'User'
IF NOT EXISTS ( SELECT TOP 1 1 FROM ScreenUserRoleMap WHERE ScreenId = @ScreenId AND RoleId = @RoleId)
	BEGIN
		INSERT INTO [dbo].[ScreenUserRoleMap]
           ([ScreenId]
           ,[RoleId]
           ,[RecordStatus]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[ModifiedBy]
           ,[ModifiedDate]
           ,[CanEdit]
           ,[DisplayScreenName])
		VALUES
		(
			@ScreenId, 			
			@RoleId,
			@RecordStatus,	
			@UserName,				
			@CurrentDate,	
			@UserName,				
			@CurrentDate,	
			1,
			'Payers'
		)
	END

SELECT  @ScreenId = ScreenId FROM Screen WHERE ScreenCode = 'SPEC'
SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'Admin'
IF NOT EXISTS ( SELECT TOP 1 1 FROM ScreenUserRoleMap WHERE ScreenId = @ScreenId AND RoleId = @RoleId)
	BEGIN
		INSERT INTO [dbo].[ScreenUserRoleMap]
           ([ScreenId]
           ,[RoleId]
           ,[RecordStatus]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[ModifiedBy]
           ,[ModifiedDate]
           ,[CanEdit]
           ,[DisplayScreenName])
		VALUES
		(
			@ScreenId, 			
			@RoleId,
			@RecordStatus,	
			@UserName,				
			@CurrentDate,	
			@UserName,				
			@CurrentDate,	
			1,
			'Specialties'
		)
	END

SELECT  @ScreenId = ScreenId FROM Screen WHERE ScreenCode = 'SPEC'
SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'Manager'
IF NOT EXISTS ( SELECT TOP 1 1 FROM ScreenUserRoleMap WHERE ScreenId = @ScreenId AND RoleId = @RoleId)
	BEGIN
		INSERT INTO [dbo].[ScreenUserRoleMap]
           ([ScreenId]
           ,[RoleId]
           ,[RecordStatus]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[ModifiedBy]
           ,[ModifiedDate]
           ,[CanEdit]
           ,[DisplayScreenName])
		VALUES
		(
			@ScreenId, 			
			@RoleId,
			@RecordStatus,	
			@UserName,				
			@CurrentDate,	
			@UserName,				
			@CurrentDate,	
			0,
			'Specialties'
		)
	END

SELECT  @ScreenId = ScreenId FROM Screen WHERE ScreenCode = 'SPEC'
SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'Executive'
IF NOT EXISTS ( SELECT TOP 1 1 FROM ScreenUserRoleMap WHERE ScreenId = @ScreenId AND RoleId = @RoleId)
	BEGIN
		INSERT INTO [dbo].[ScreenUserRoleMap]
           ([ScreenId]
           ,[RoleId]
           ,[RecordStatus]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[ModifiedBy]
           ,[ModifiedDate]
           ,[CanEdit]
           ,[DisplayScreenName])
		VALUES
		(
			@ScreenId, 			
			@RoleId,
			@RecordStatus,	
			@UserName,				
			@CurrentDate,	
			@UserName,				
			@CurrentDate,	
			1,
			'Specialties'
		)
	END

SELECT  @ScreenId = ScreenId FROM Screen WHERE ScreenCode = 'USER'
SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'Executive'
IF NOT EXISTS ( SELECT TOP 1 1 FROM ScreenUserRoleMap WHERE ScreenId = @ScreenId AND RoleId = @RoleId)
	BEGIN
		INSERT INTO [dbo].[ScreenUserRoleMap]
           ([ScreenId]
           ,[RoleId]
           ,[RecordStatus]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[ModifiedBy]
           ,[ModifiedDate]
           ,[CanEdit]
           ,[DisplayScreenName])
		VALUES
		(
			@ScreenId, 			
			@RoleId,
			@RecordStatus,	
			@UserName,				
			@CurrentDate,	
			@UserName,				
			@CurrentDate,	
			NULL,
			'Users'
		)
	END

SELECT  @ScreenId = ScreenId FROM Screen WHERE ScreenCode = 'USER'
SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'Manager'
IF NOT EXISTS ( SELECT TOP 1 1 FROM ScreenUserRoleMap WHERE ScreenId = @ScreenId AND RoleId = @RoleId)
	BEGIN
		INSERT INTO [dbo].[ScreenUserRoleMap]
           ([ScreenId]
           ,[RoleId]
           ,[RecordStatus]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[ModifiedBy]
           ,[ModifiedDate]
           ,[CanEdit]
           ,[DisplayScreenName])
		VALUES
		(
			@ScreenId, 			
			@RoleId,
			@RecordStatus,	
			@UserName,				
			@CurrentDate,	
			@UserName,				
			@CurrentDate,	
			NULL,
			'Users'
		)
	END

SELECT  @ScreenId = ScreenId FROM Screen WHERE ScreenCode = 'USER'
SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'Admin'
IF NOT EXISTS ( SELECT TOP 1 1 FROM ScreenUserRoleMap WHERE ScreenId = @ScreenId AND RoleId = @RoleId)
	BEGIN
		INSERT INTO [dbo].[ScreenUserRoleMap]
           ([ScreenId]
           ,[RoleId]
           ,[RecordStatus]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[ModifiedBy]
           ,[ModifiedDate]
           ,[CanEdit]
           ,[DisplayScreenName])
		VALUES
		(
			@ScreenId, 			
			@RoleId,
			@RecordStatus,	
			@UserName,				
			@CurrentDate,	
			@UserName,				
			@CurrentDate,	
			NULL,
			'Users'
		)
	END

SELECT  @ScreenId = ScreenId FROM Screen WHERE ScreenCode = 'CK'
SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'Admin'
IF NOT EXISTS ( SELECT TOP 1 1 FROM ScreenUserRoleMap WHERE ScreenId = @ScreenId AND RoleId = @RoleId)
	BEGIN
		INSERT INTO [dbo].[ScreenUserRoleMap]
           ([ScreenId]
           ,[RoleId]
           ,[RecordStatus]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[ModifiedBy]
           ,[ModifiedDate]
           ,[CanEdit]
           ,[DisplayScreenName])
		VALUES
		(
			@ScreenId, 			
			@RoleId,
			@RecordStatus,	
			@UserName,				
			@CurrentDate,	
			@UserName,				
			@CurrentDate,	
			1,
			'Create KPI'
		)
	END

SELECT  @ScreenId = ScreenId FROM Screen WHERE ScreenCode = 'CK'
SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'Executive'
IF NOT EXISTS ( SELECT TOP 1 1 FROM ScreenUserRoleMap WHERE ScreenId = @ScreenId AND RoleId = @RoleId)
	BEGIN
		INSERT INTO [dbo].[ScreenUserRoleMap]
           ([ScreenId]
           ,[RoleId]
           ,[RecordStatus]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[ModifiedBy]
           ,[ModifiedDate]
           ,[CanEdit]
           ,[DisplayScreenName])
		VALUES
		(
			@ScreenId, 			
			@RoleId,
			@RecordStatus,	
			@UserName,				
			@CurrentDate,	
			@UserName,				
			@CurrentDate,	
			1,
			'Create KPI'
		)
	END

SELECT  @ScreenId = ScreenId FROM Screen WHERE ScreenCode = 'VEK'
SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'Executive'
IF NOT EXISTS ( SELECT TOP 1 1 FROM ScreenUserRoleMap WHERE ScreenId = @ScreenId AND RoleId = @RoleId)
	BEGIN
		INSERT INTO [dbo].[ScreenUserRoleMap]
           ([ScreenId]
           ,[RoleId]
           ,[RecordStatus]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[ModifiedBy]
           ,[ModifiedDate]
           ,[CanEdit]
           ,[DisplayScreenName])
		VALUES
		(
			@ScreenId, 			
			@RoleId,
			@RecordStatus,	
			@UserName,				
			@CurrentDate,	
			@UserName,				
			@CurrentDate,	
			1,
			'View/Edit KPI'
		)
	END

SELECT  @ScreenId = ScreenId FROM Screen WHERE ScreenCode = 'VEK'
SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'Manager'
IF NOT EXISTS ( SELECT TOP 1 1 FROM ScreenUserRoleMap WHERE ScreenId = @ScreenId AND RoleId = @RoleId)
	BEGIN
		INSERT INTO [dbo].[ScreenUserRoleMap]
           ([ScreenId]
           ,[RoleId]
           ,[RecordStatus]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[ModifiedBy]
           ,[ModifiedDate]
           ,[CanEdit]
           ,[DisplayScreenName])
		VALUES
		(
			@ScreenId, 			
			@RoleId,
			@RecordStatus,	
			@UserName,				
			@CurrentDate,	
			@UserName,				
			@CurrentDate,	
			0,
			'View KPI'
		)
	END

SELECT  @ScreenId = ScreenId FROM Screen WHERE ScreenCode = 'VEK'
SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'Admin'
IF NOT EXISTS ( SELECT TOP 1 1 FROM ScreenUserRoleMap WHERE ScreenId = @ScreenId AND RoleId = @RoleId)
	BEGIN
		INSERT INTO [dbo].[ScreenUserRoleMap]
           ([ScreenId]
           ,[RoleId]
           ,[RecordStatus]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[ModifiedBy]
           ,[ModifiedDate]
           ,[CanEdit]
           ,[DisplayScreenName])
		VALUES
		(
			@ScreenId, 			
			@RoleId,
			@RecordStatus,	
			@UserName,				
			@CurrentDate,	
			@UserName,				
			@CurrentDate,	
			1,
			'View/Edit KPI'
		)
	END

SELECT  @ScreenId = ScreenId FROM Screen WHERE ScreenCode = 'CMU'
SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'Admin'
IF NOT EXISTS ( SELECT TOP 1 1 FROM ScreenUserRoleMap WHERE ScreenId = @ScreenId AND RoleId = @RoleId)
	BEGIN
		INSERT INTO [dbo].[ScreenUserRoleMap]
           ([ScreenId]
           ,[RoleId]
           ,[RecordStatus]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[ModifiedBy]
           ,[ModifiedDate]
           ,[CanEdit]
           ,[DisplayScreenName])
		VALUES
		(
			@ScreenId, 			
			@RoleId,
			@RecordStatus,	
			@UserName,				
			@CurrentDate,	
			@UserName,				
			@CurrentDate,	
			NULL,
			'Create M3 Users'
		)
	END

SELECT  @ScreenId = ScreenId FROM Screen WHERE ScreenCode = 'CMU'
SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'Manager'
IF NOT EXISTS ( SELECT TOP 1 1 FROM ScreenUserRoleMap WHERE ScreenId = @ScreenId AND RoleId = @RoleId)
	BEGIN
		INSERT INTO [dbo].[ScreenUserRoleMap]
           ([ScreenId]
           ,[RoleId]
           ,[RecordStatus]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[ModifiedBy]
           ,[ModifiedDate]
           ,[CanEdit]
           ,[DisplayScreenName])
		VALUES
		(
			@ScreenId, 			
			@RoleId,
			@RecordStatus,	
			@UserName,				
			@CurrentDate,	
			@UserName,				
			@CurrentDate,	
			NULL,
			'Create M3 Users'
		)
	END

SELECT  @ScreenId = ScreenId FROM Screen WHERE ScreenCode = 'CMU'
SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'Executive'
IF NOT EXISTS ( SELECT TOP 1 1 FROM ScreenUserRoleMap WHERE ScreenId = @ScreenId AND RoleId = @RoleId)
	BEGIN
		INSERT INTO [dbo].[ScreenUserRoleMap]
           ([ScreenId]
           ,[RoleId]
           ,[RecordStatus]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[ModifiedBy]
           ,[ModifiedDate]
           ,[CanEdit]
           ,[DisplayScreenName])
		VALUES
		(
			@ScreenId, 			
			@RoleId,
			@RecordStatus,	
			@UserName,				
			@CurrentDate,	
			@UserName,				
			@CurrentDate,	
			NULL,
			'Create M3 Users'
		)
	END

SELECT  @ScreenId = ScreenId FROM Screen WHERE ScreenCode = 'CCU'
SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'Executive'
IF NOT EXISTS ( SELECT TOP 1 1 FROM ScreenUserRoleMap WHERE ScreenId = @ScreenId AND RoleId = @RoleId)
	BEGIN
		INSERT INTO [dbo].[ScreenUserRoleMap]
           ([ScreenId]
           ,[RoleId]
           ,[RecordStatus]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[ModifiedBy]
           ,[ModifiedDate]
           ,[CanEdit]
           ,[DisplayScreenName])
		VALUES
		(
			@ScreenId, 			
			@RoleId,
			@RecordStatus,	
			@UserName,				
			@CurrentDate,	
			@UserName,				
			@CurrentDate,	
			NULL,
			'Create Client Users'
		)
	END

SELECT  @ScreenId = ScreenId FROM Screen WHERE ScreenCode = 'CCU'
SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'Manager'
IF NOT EXISTS ( SELECT TOP 1 1 FROM ScreenUserRoleMap WHERE ScreenId = @ScreenId AND RoleId = @RoleId)
	BEGIN
		INSERT INTO [dbo].[ScreenUserRoleMap]
           ([ScreenId]
           ,[RoleId]
           ,[RecordStatus]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[ModifiedBy]
           ,[ModifiedDate]
           ,[CanEdit]
           ,[DisplayScreenName])
		VALUES
		(
			@ScreenId, 			
			@RoleId,
			@RecordStatus,	
			@UserName,				
			@CurrentDate,	
			@UserName,				
			@CurrentDate,	
			NULL,
			'Create Client Users'
		)
	END

SELECT  @ScreenId = ScreenId FROM Screen WHERE ScreenCode = 'CCU'
SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'Admin'
IF NOT EXISTS ( SELECT TOP 1 1 FROM ScreenUserRoleMap WHERE ScreenId = @ScreenId AND RoleId = @RoleId)
	BEGIN
		INSERT INTO [dbo].[ScreenUserRoleMap]
           ([ScreenId]
           ,[RoleId]
           ,[RecordStatus]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[ModifiedBy]
           ,[ModifiedDate]
           ,[CanEdit]
           ,[DisplayScreenName])
		VALUES
		(
			@ScreenId, 			
			@RoleId,
			@RecordStatus,	
			@UserName,				
			@CurrentDate,	
			@UserName,				
			@CurrentDate,	
			NULL,
			'Create Client Users'
		)
	END

SELECT  @ScreenId = ScreenId FROM Screen WHERE ScreenCode = 'AU'
SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'Admin'
IF NOT EXISTS ( SELECT TOP 1 1 FROM ScreenUserRoleMap WHERE ScreenId = @ScreenId AND RoleId = @RoleId)
	BEGIN
		INSERT INTO [dbo].[ScreenUserRoleMap]
           ([ScreenId]
           ,[RoleId]
           ,[RecordStatus]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[ModifiedBy]
           ,[ModifiedDate]
           ,[CanEdit]
           ,[DisplayScreenName])
		VALUES
		(
			@ScreenId, 			
			@RoleId,
			@RecordStatus,	
			@UserName,				
			@CurrentDate,	
			@UserName,				
			@CurrentDate,	
			NULL,
			'All Users'
		)
	END

SELECT  @ScreenId = ScreenId FROM Screen WHERE ScreenCode = 'AU'
SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'Manager'
IF NOT EXISTS ( SELECT TOP 1 1 FROM ScreenUserRoleMap WHERE ScreenId = @ScreenId AND RoleId = @RoleId)
	BEGIN
		INSERT INTO [dbo].[ScreenUserRoleMap]
           ([ScreenId]
           ,[RoleId]
           ,[RecordStatus]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[ModifiedBy]
           ,[ModifiedDate]
           ,[CanEdit]
           ,[DisplayScreenName])
		VALUES
		(
			@ScreenId, 			
			@RoleId,
			@RecordStatus,	
			@UserName,				
			@CurrentDate,	
			@UserName,				
			@CurrentDate,	
			NULL,
			'All Users'
		)
	END

SELECT  @ScreenId = ScreenId FROM Screen WHERE ScreenCode = 'AU'
SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'Executive'
IF NOT EXISTS ( SELECT TOP 1 1 FROM ScreenUserRoleMap WHERE ScreenId = @ScreenId AND RoleId = @RoleId)
	BEGIN
		INSERT INTO [dbo].[ScreenUserRoleMap]
           ([ScreenId]
           ,[RoleId]
           ,[RecordStatus]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[ModifiedBy]
           ,[ModifiedDate]
           ,[CanEdit]
           ,[DisplayScreenName])
		VALUES
		(
			@ScreenId, 			
			@RoleId,
			@RecordStatus,	
			@UserName,				
			@CurrentDate,	
			@UserName,				
			@CurrentDate,	
			NULL,
			'All Users'
		)
	END

SELECT  @ScreenId = ScreenId FROM Screen WHERE ScreenCode = 'CECI'
SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'Executive'
IF NOT EXISTS ( SELECT TOP 1 1 FROM ScreenUserRoleMap WHERE ScreenId = @ScreenId AND RoleId = @RoleId)
	BEGIN
		INSERT INTO [dbo].[ScreenUserRoleMap]
           ([ScreenId]
           ,[RoleId]
           ,[RecordStatus]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[ModifiedBy]
           ,[ModifiedDate]
           ,[CanEdit]
           ,[DisplayScreenName])
		VALUES
		(
			@ScreenId, 			
			@RoleId,
			@RecordStatus,	
			@UserName,				
			@CurrentDate,	
			@UserName,				
			@CurrentDate,	
			NULL,
			'Create/Edit Checklist Item'
		)
	END

SELECT  @ScreenId = ScreenId FROM Screen WHERE ScreenCode = 'CECI'
SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'Admin'
IF NOT EXISTS ( SELECT TOP 1 1 FROM ScreenUserRoleMap WHERE ScreenId = @ScreenId AND RoleId = @RoleId)
	BEGIN
		INSERT INTO [dbo].[ScreenUserRoleMap]
           ([ScreenId]
           ,[RoleId]
           ,[RecordStatus]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[ModifiedBy]
           ,[ModifiedDate]
           ,[CanEdit]
           ,[DisplayScreenName])
		VALUES
		(
			@ScreenId, 			
			@RoleId,
			@RecordStatus,	
			@UserName,				
			@CurrentDate,	
			@UserName,				
			@CurrentDate,	
			NULL,
			'Create/Edit Checklist Item'
		)
	END

SELECT  @ScreenId = ScreenId FROM Screen WHERE ScreenCode = 'CLS'
SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'Admin'
IF NOT EXISTS ( SELECT TOP 1 1 FROM ScreenUserRoleMap WHERE ScreenId = @ScreenId AND RoleId = @RoleId)
	BEGIN
		INSERT INTO [dbo].[ScreenUserRoleMap]
           ([ScreenId]
           ,[RoleId]
           ,[RecordStatus]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[ModifiedBy]
           ,[ModifiedDate]
           ,[CanEdit]
           ,[DisplayScreenName])
		VALUES
		(
			@ScreenId, 			
			@RoleId,
			@RecordStatus,	
			@UserName,				
			@CurrentDate,	
			@UserName,				
			@CurrentDate,	
			NULL,
			'Checklist Setup'
		)
	END

SELECT  @ScreenId = ScreenId FROM Screen WHERE ScreenCode = 'CLS'
SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'Executive'
IF NOT EXISTS ( SELECT TOP 1 1 FROM ScreenUserRoleMap WHERE ScreenId = @ScreenId AND RoleId = @RoleId)
	BEGIN
		INSERT INTO [dbo].[ScreenUserRoleMap]
           ([ScreenId]
           ,[RoleId]
           ,[RecordStatus]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[ModifiedBy]
           ,[ModifiedDate]
           ,[CanEdit]
           ,[DisplayScreenName])
		VALUES
		(
			@ScreenId, 			
			@RoleId,
			@RecordStatus,	
			@UserName,				
			@CurrentDate,	
			@UserName,				
			@CurrentDate,	
			NULL,
			'Checklist Setup'
		)
	END

SELECT  @ScreenId = ScreenId FROM Screen WHERE ScreenCode = 'VACL'
SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'Executive'
IF NOT EXISTS ( SELECT TOP 1 1 FROM ScreenUserRoleMap WHERE ScreenId = @ScreenId AND RoleId = @RoleId)
	BEGIN
		INSERT INTO [dbo].[ScreenUserRoleMap]
           ([ScreenId]
           ,[RoleId]
           ,[RecordStatus]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[ModifiedBy]
           ,[ModifiedDate]
           ,[CanEdit]
           ,[DisplayScreenName])
		VALUES
		(
			@ScreenId, 			
			@RoleId,
			@RecordStatus,	
			@UserName,				
			@CurrentDate,	
			@UserName,				
			@CurrentDate,	
			1,
			'View All Checklist'
		)
	END

SELECT  @ScreenId = ScreenId FROM Screen WHERE ScreenCode = 'VACL'
SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'Admin'
IF NOT EXISTS ( SELECT TOP 1 1 FROM ScreenUserRoleMap WHERE ScreenId = @ScreenId AND RoleId = @RoleId)
	BEGIN
		INSERT INTO [dbo].[ScreenUserRoleMap]
           ([ScreenId]
           ,[RoleId]
           ,[RecordStatus]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[ModifiedBy]
           ,[ModifiedDate]
           ,[CanEdit]
           ,[DisplayScreenName])
		VALUES
		(
			@ScreenId, 			
			@RoleId,
			@RecordStatus,	
			@UserName,				
			@CurrentDate,	
			@UserName,				
			@CurrentDate,	
			1,
			'View All Checklist'
		)
	END

SELECT  @ScreenId = ScreenId FROM Screen WHERE ScreenCode = 'CD'
SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'Admin'
IF NOT EXISTS ( SELECT TOP 1 1 FROM ScreenUserRoleMap WHERE ScreenId = @ScreenId AND RoleId = @RoleId)
	BEGIN
		INSERT INTO [dbo].[ScreenUserRoleMap]
           ([ScreenId]
           ,[RoleId]
           ,[RecordStatus]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[ModifiedBy]
           ,[ModifiedDate]
           ,[CanEdit]
           ,[DisplayScreenName])
		VALUES
		(
			@ScreenId, 			
			@RoleId,
			@RecordStatus,	
			@UserName,				
			@CurrentDate,	
			@UserName,				
			@CurrentDate,	
			1,
			'Client Data'
		)
	END

SELECT  @ScreenId = ScreenId FROM Screen WHERE ScreenCode = 'CD'
SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'Manager'
IF NOT EXISTS ( SELECT TOP 1 1 FROM ScreenUserRoleMap WHERE ScreenId = @ScreenId AND RoleId = @RoleId)
	BEGIN
		INSERT INTO [dbo].[ScreenUserRoleMap]
           ([ScreenId]
           ,[RoleId]
           ,[RecordStatus]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[ModifiedBy]
           ,[ModifiedDate]
           ,[CanEdit]
           ,[DisplayScreenName])
		VALUES
		(
			@ScreenId, 			
			@RoleId,
			@RecordStatus,	
			@UserName,				
			@CurrentDate,	
			@UserName,				
			@CurrentDate,	
			1,
			'Client Data'
		)
	END

SELECT  @ScreenId = ScreenId FROM Screen WHERE ScreenCode = 'CD'
SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'Executive'
IF NOT EXISTS ( SELECT TOP 1 1 FROM ScreenUserRoleMap WHERE ScreenId = @ScreenId AND RoleId = @RoleId)
	BEGIN
		INSERT INTO [dbo].[ScreenUserRoleMap]
           ([ScreenId]
           ,[RoleId]
           ,[RecordStatus]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[ModifiedBy]
           ,[ModifiedDate]
           ,[CanEdit]
           ,[DisplayScreenName])
		VALUES
		(
			@ScreenId, 			
			@RoleId,
			@RecordStatus,	
			@UserName,				
			@CurrentDate,	
			@UserName,				
			@CurrentDate,	
			1,
			'Client Data'
		)
	END

SELECT  @ScreenId = ScreenId FROM Screen WHERE ScreenCode = 'CD'
SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'User'
IF NOT EXISTS ( SELECT TOP 1 1 FROM ScreenUserRoleMap WHERE ScreenId = @ScreenId AND RoleId = @RoleId)
	BEGIN
		INSERT INTO [dbo].[ScreenUserRoleMap]
           ([ScreenId]
           ,[RoleId]
           ,[RecordStatus]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[ModifiedBy]
           ,[ModifiedDate]
           ,[CanEdit]
           ,[DisplayScreenName])
		VALUES
		(
			@ScreenId, 			
			@RoleId,
			@RecordStatus,	
			@UserName,				
			@CurrentDate,	
			@UserName,				
			@CurrentDate,	
			0,
			'Client Data'
		)
	END

SELECT  @ScreenId = ScreenId FROM Screen WHERE ScreenCode = 'LS'
SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'Admin'
IF NOT EXISTS ( SELECT TOP 1 1 FROM ScreenUserRoleMap WHERE ScreenId = @ScreenId AND RoleId = @RoleId)
	BEGIN
		INSERT INTO [dbo].[ScreenUserRoleMap]
           ([ScreenId]
           ,[RoleId]
           ,[RecordStatus]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[ModifiedBy]
           ,[ModifiedDate]
           ,[CanEdit]
           ,[DisplayScreenName])
		VALUES
		(
			@ScreenId, 			
			@RoleId,
			@RecordStatus,	
			@UserName,				
			@CurrentDate,	
			@UserName,				
			@CurrentDate,	
			1,
			'Log Setup'
		)
	END

SELECT  @ScreenId = ScreenId FROM Screen WHERE ScreenCode = 'LS'
SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'Manager'
IF NOT EXISTS ( SELECT TOP 1 1 FROM ScreenUserRoleMap WHERE ScreenId = @ScreenId AND RoleId = @RoleId)
	BEGIN
		INSERT INTO [dbo].[ScreenUserRoleMap]
           ([ScreenId]
           ,[RoleId]
           ,[RecordStatus]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[ModifiedBy]
           ,[ModifiedDate]
           ,[CanEdit]
           ,[DisplayScreenName])
		VALUES
		(
			@ScreenId, 			
			@RoleId,
			@RecordStatus,	
			@UserName,				
			@CurrentDate,	
			@UserName,				
			@CurrentDate,	
			1,
			'Log Setup'
		)
	END

SELECT  @ScreenId = ScreenId FROM Screen WHERE ScreenCode = 'LS'
SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'Executive'
IF NOT EXISTS ( SELECT TOP 1 1 FROM ScreenUserRoleMap WHERE ScreenId = @ScreenId AND RoleId = @RoleId)
	BEGIN
		INSERT INTO [dbo].[ScreenUserRoleMap]
           ([ScreenId]
           ,[RoleId]
           ,[RecordStatus]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[ModifiedBy]
           ,[ModifiedDate]
           ,[CanEdit]
           ,[DisplayScreenName])
		VALUES
		(
			@ScreenId, 			
			@RoleId,
			@RecordStatus,	
			@UserName,				
			@CurrentDate,	
			@UserName,				
			@CurrentDate,	
			1,
			'Log Setup'
		)
	END

SELECT  @ScreenId = ScreenId FROM Screen WHERE ScreenCode = 'LS'
SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'User'
IF NOT EXISTS ( SELECT TOP 1 1 FROM ScreenUserRoleMap WHERE ScreenId = @ScreenId AND RoleId = @RoleId)
	BEGIN
		INSERT INTO [dbo].[ScreenUserRoleMap]
           ([ScreenId]
           ,[RoleId]
           ,[RecordStatus]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[ModifiedBy]
           ,[ModifiedDate]
           ,[CanEdit]
           ,[DisplayScreenName])
		VALUES
		(
			@ScreenId, 			
			@RoleId,
			@RecordStatus,	
			@UserName,				
			@CurrentDate,	
			@UserName,				
			@CurrentDate,	
			1,
			'Log Setup'
		)
	END

SELECT  @ScreenId = ScreenId FROM Screen WHERE ScreenCode = 'CAU'
SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'Admin'
IF NOT EXISTS ( SELECT TOP 1 1 FROM ScreenUserRoleMap WHERE ScreenId = @ScreenId AND RoleId = @RoleId)
	BEGIN
		INSERT INTO [dbo].[ScreenUserRoleMap]
           ([ScreenId]
           ,[RoleId]
           ,[RecordStatus]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[ModifiedBy]
           ,[ModifiedDate]
           ,[CanEdit]
           ,[DisplayScreenName])
		VALUES
		(
			@ScreenId, 			
			@RoleId,
			@RecordStatus,	
			@UserName,				
			@CurrentDate,	
			@UserName,				
			@CurrentDate,	
			1,
			'Assign User'
		)
	END

SELECT  @ScreenId = ScreenId FROM Screen WHERE ScreenCode = 'CAU'
SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'Manager'
IF NOT EXISTS ( SELECT TOP 1 1 FROM ScreenUserRoleMap WHERE ScreenId = @ScreenId AND RoleId = @RoleId)
	BEGIN
		INSERT INTO [dbo].[ScreenUserRoleMap]
           ([ScreenId]
           ,[RoleId]
           ,[RecordStatus]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[ModifiedBy]
           ,[ModifiedDate]
           ,[CanEdit]
           ,[DisplayScreenName])
		VALUES
		(
			@ScreenId, 			
			@RoleId,
			@RecordStatus,	
			@UserName,				
			@CurrentDate,	
			@UserName,				
			@CurrentDate,	
			1,
			'Assign User'
		)
	END

SELECT  @ScreenId = ScreenId FROM Screen WHERE ScreenCode = 'CAU'
SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'Executive'
IF NOT EXISTS ( SELECT TOP 1 1 FROM ScreenUserRoleMap WHERE ScreenId = @ScreenId AND RoleId = @RoleId)
	BEGIN
		INSERT INTO [dbo].[ScreenUserRoleMap]
           ([ScreenId]
           ,[RoleId]
           ,[RecordStatus]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[ModifiedBy]
           ,[ModifiedDate]
           ,[CanEdit]
           ,[DisplayScreenName])
		VALUES
		(
			@ScreenId, 			
			@RoleId,
			@RecordStatus,	
			@UserName,				
			@CurrentDate,	
			@UserName,				
			@CurrentDate,	
			1,
			'Assign User'
		)
	END

SELECT  @ScreenId = ScreenId FROM Screen WHERE ScreenCode = 'MT'
SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'Admin'
IF NOT EXISTS ( SELECT TOP 1 1 FROM ScreenUserRoleMap WHERE ScreenId = @ScreenId AND RoleId = @RoleId)
	BEGIN
		INSERT INTO [dbo].[ScreenUserRoleMap]
           ([ScreenId]
           ,[RoleId]
           ,[RecordStatus]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[ModifiedBy]
           ,[ModifiedDate]
           ,[CanEdit]
           ,[DisplayScreenName])
		VALUES
		(
			@ScreenId, 			
			@RoleId,
			@RecordStatus,	
			@UserName,				
			@CurrentDate,	
			@UserName,				
			@CurrentDate,	
			1,
			'Monthly Targets'
		)
	END

SELECT  @ScreenId = ScreenId FROM Screen WHERE ScreenCode = 'MT'
SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'Manager'
IF NOT EXISTS ( SELECT TOP 1 1 FROM ScreenUserRoleMap WHERE ScreenId = @ScreenId AND RoleId = @RoleId)
	BEGIN
		INSERT INTO [dbo].[ScreenUserRoleMap]
           ([ScreenId]
           ,[RoleId]
           ,[RecordStatus]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[ModifiedBy]
           ,[ModifiedDate]
           ,[CanEdit]
           ,[DisplayScreenName])
		VALUES
		(
			@ScreenId, 			
			@RoleId,
			@RecordStatus,	
			@UserName,				
			@CurrentDate,	
			@UserName,				
			@CurrentDate,	
			1,
			'Monthly Targets'
		)
	END

SELECT  @ScreenId = ScreenId FROM Screen WHERE ScreenCode = 'MT'
SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'Executive'
IF NOT EXISTS ( SELECT TOP 1 1 FROM ScreenUserRoleMap WHERE ScreenId = @ScreenId AND RoleId = @RoleId)
	BEGIN
		INSERT INTO [dbo].[ScreenUserRoleMap]
           ([ScreenId]
           ,[RoleId]
           ,[RecordStatus]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[ModifiedBy]
           ,[ModifiedDate]
           ,[CanEdit]
           ,[DisplayScreenName])
		VALUES
		(
			@ScreenId, 			
			@RoleId,
			@RecordStatus,	
			@UserName,				
			@CurrentDate,	
			@UserName,				
			@CurrentDate,	
			1,
			'Monthly Targets'
		)
	END

SELECT  @ScreenId = ScreenId FROM Screen WHERE ScreenCode = 'KPIS'
SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'Admin'
IF NOT EXISTS ( SELECT TOP 1 1 FROM ScreenUserRoleMap WHERE ScreenId = @ScreenId AND RoleId = @RoleId)
	BEGIN
		INSERT INTO [dbo].[ScreenUserRoleMap]
           ([ScreenId]
           ,[RoleId]
           ,[RecordStatus]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[ModifiedBy]
           ,[ModifiedDate]
           ,[CanEdit]
           ,[DisplayScreenName])
		VALUES
		(
			@ScreenId, 			
			@RoleId,
			@RecordStatus,	
			@UserName,				
			@CurrentDate,	
			@UserName,				
			@CurrentDate,	
			1,
			'KPI Setup'
		)
	END

SELECT  @ScreenId = ScreenId FROM Screen WHERE ScreenCode = 'KPIS'
SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'Manager'
IF NOT EXISTS ( SELECT TOP 1 1 FROM ScreenUserRoleMap WHERE ScreenId = @ScreenId AND RoleId = @RoleId)
	BEGIN
		INSERT INTO [dbo].[ScreenUserRoleMap]
           ([ScreenId]
           ,[RoleId]
           ,[RecordStatus]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[ModifiedBy]
           ,[ModifiedDate]
           ,[CanEdit]
           ,[DisplayScreenName])
		VALUES
		(
			@ScreenId, 			
			@RoleId,
			@RecordStatus,	
			@UserName,				
			@CurrentDate,	
			@UserName,				
			@CurrentDate,	
			1,
			'KPI Setup'
		)
	END

SELECT  @ScreenId = ScreenId FROM Screen WHERE ScreenCode = 'KPIS'
SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'Executive'
IF NOT EXISTS ( SELECT TOP 1 1 FROM ScreenUserRoleMap WHERE ScreenId = @ScreenId AND RoleId = @RoleId)
	BEGIN
		INSERT INTO [dbo].[ScreenUserRoleMap]
           ([ScreenId]
           ,[RoleId]
           ,[RecordStatus]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[ModifiedBy]
           ,[ModifiedDate]
           ,[CanEdit]
           ,[DisplayScreenName])
		VALUES
		(
			@ScreenId, 			
			@RoleId,
			@RecordStatus,	
			@UserName,				
			@CurrentDate,	
			@UserName,				
			@CurrentDate,	
			1,
			'KPI Setup'
		)
	END

SELECT  @ScreenId = ScreenId FROM Screen WHERE ScreenCode = 'KPIS'
SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'User'
IF NOT EXISTS ( SELECT TOP 1 1 FROM ScreenUserRoleMap WHERE ScreenId = @ScreenId AND RoleId = @RoleId)
	BEGIN
		INSERT INTO [dbo].[ScreenUserRoleMap]
           ([ScreenId]
           ,[RoleId]
           ,[RecordStatus]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[ModifiedBy]
           ,[ModifiedDate]
           ,[CanEdit]
           ,[DisplayScreenName])
		VALUES
		(
			@ScreenId, 			
			@RoleId,
			@RecordStatus,	
			@UserName,				
			@CurrentDate,	
			@UserName,				
			@CurrentDate,	
			0,
			'KPI Setup'
		)
	END

SELECT  @ScreenId = ScreenId FROM Screen WHERE ScreenCode = 'KPIS'
SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'Client'
IF NOT EXISTS ( SELECT TOP 1 1 FROM ScreenUserRoleMap WHERE ScreenId = @ScreenId AND RoleId = @RoleId)
	BEGIN
		INSERT INTO [dbo].[ScreenUserRoleMap]
           ([ScreenId]
           ,[RoleId]
           ,[RecordStatus]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[ModifiedBy]
           ,[ModifiedDate]
           ,[CanEdit]
           ,[DisplayScreenName])
		VALUES
		(
			@ScreenId, 			
			@RoleId,
			@RecordStatus,	
			@UserName,				
			@CurrentDate,	
			@UserName,				
			@CurrentDate,	
			0,
			'KPI Setup'
		)
	END

SELECT  @ScreenId = ScreenId FROM Screen WHERE ScreenCode = 'LP'
SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'Admin'
IF NOT EXISTS ( SELECT TOP 1 1 FROM ScreenUserRoleMap WHERE ScreenId = @ScreenId AND RoleId = @RoleId)
	BEGIN
		INSERT INTO [dbo].[ScreenUserRoleMap]
           ([ScreenId]
           ,[RoleId]
           ,[RecordStatus]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[ModifiedBy]
           ,[ModifiedDate]
           ,[CanEdit]
           ,[DisplayScreenName])
		VALUES
		(
			@ScreenId, 			
			@RoleId,
			@RecordStatus,	
			@UserName,				
			@CurrentDate,	
			@UserName,				
			@CurrentDate,	
			NULL,
			'Landing Page'
		)
	END

SELECT  @ScreenId = ScreenId FROM Screen WHERE ScreenCode = 'LP'
SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'Manager'
IF NOT EXISTS ( SELECT TOP 1 1 FROM ScreenUserRoleMap WHERE ScreenId = @ScreenId AND RoleId = @RoleId)
	BEGIN
		INSERT INTO [dbo].[ScreenUserRoleMap]
           ([ScreenId]
           ,[RoleId]
           ,[RecordStatus]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[ModifiedBy]
           ,[ModifiedDate]
           ,[CanEdit]
           ,[DisplayScreenName])
		VALUES
		(
			@ScreenId, 			
			@RoleId,
			@RecordStatus,	
			@UserName,				
			@CurrentDate,	
			@UserName,				
			@CurrentDate,	
			NULL,
			'Landing Page'
		)
	END

SELECT  @ScreenId = ScreenId FROM Screen WHERE ScreenCode = 'LP'
SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'Executive'
IF NOT EXISTS ( SELECT TOP 1 1 FROM ScreenUserRoleMap WHERE ScreenId = @ScreenId AND RoleId = @RoleId)
	BEGIN
		INSERT INTO [dbo].[ScreenUserRoleMap]
           ([ScreenId]
           ,[RoleId]
           ,[RecordStatus]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[ModifiedBy]
           ,[ModifiedDate]
           ,[CanEdit]
           ,[DisplayScreenName])
		VALUES
		(
			@ScreenId, 			
			@RoleId,
			@RecordStatus,	
			@UserName,				
			@CurrentDate,	
			@UserName,				
			@CurrentDate,	
			NULL,
			'Landing Page'
		)
	END

SELECT  @ScreenId = ScreenId FROM Screen WHERE ScreenCode = 'SYS'
SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'Admin'
IF NOT EXISTS ( SELECT TOP 1 1 FROM ScreenUserRoleMap WHERE ScreenId = @ScreenId AND RoleId = @RoleId)
	BEGIN
		INSERT INTO [dbo].[ScreenUserRoleMap]
           ([ScreenId]
           ,[RoleId]
           ,[RecordStatus]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[ModifiedBy]
           ,[ModifiedDate]
           ,[CanEdit]
           ,[DisplayScreenName])
		VALUES
		(
			@ScreenId, 			
			@RoleId,
			@RecordStatus,	
			@UserName,				
			@CurrentDate,	
			@UserName,				
			@CurrentDate,	
			1,
			'System'
		)
	END

SELECT  @ScreenId = ScreenId FROM Screen WHERE ScreenCode = 'SYS'
SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'Executive'
IF NOT EXISTS ( SELECT TOP 1 1 FROM ScreenUserRoleMap WHERE ScreenId = @ScreenId AND RoleId = @RoleId)
	BEGIN
		INSERT INTO [dbo].[ScreenUserRoleMap]
           ([ScreenId]
           ,[RoleId]
           ,[RecordStatus]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[ModifiedBy]
           ,[ModifiedDate]
           ,[CanEdit]
           ,[DisplayScreenName])
		VALUES
		(
			@ScreenId, 			
			@RoleId,
			@RecordStatus,	
			@UserName,				
			@CurrentDate,	
			@UserName,				
			@CurrentDate,	
			0,
			'System'
		)
	END
	SELECT  @ScreenId = ScreenId FROM Screen WHERE ScreenCode = 'CON'
SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'Admin'
IF NOT EXISTS ( SELECT TOP 1 1 FROM ScreenUserRoleMap WHERE ScreenId = @ScreenId AND RoleId = @RoleId)
	BEGIN
		INSERT INTO [dbo].[ScreenUserRoleMap]
           ([ScreenId]
           ,[RoleId]
           ,[RecordStatus]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[ModifiedBy]
           ,[ModifiedDate]
           ,[CanEdit]
           ,[DisplayScreenName])
		VALUES
		(
			@ScreenId, 			
			@RoleId,
			@RecordStatus,	
			@UserName,				
			@CurrentDate,	
			@UserName,				
			@CurrentDate,	
			1,
			'Configurations'
		)
	END

SELECT  @ScreenId = ScreenId FROM Screen WHERE ScreenCode = 'CON'
SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'Executive'
IF NOT EXISTS ( SELECT TOP 1 1 FROM ScreenUserRoleMap WHERE ScreenId = @ScreenId AND RoleId = @RoleId)
	BEGIN
		INSERT INTO [dbo].[ScreenUserRoleMap]
           ([ScreenId]
           ,[RoleId]
           ,[RecordStatus]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[ModifiedBy]
           ,[ModifiedDate]
           ,[CanEdit]
           ,[DisplayScreenName])
		VALUES
		(
			@ScreenId, 			
			@RoleId,
			@RecordStatus,	
			@UserName,				
			@CurrentDate,	
			@UserName,				
			@CurrentDate,	
			0,
			'Configurations'
		)
	END
	
SELECT  @ScreenId = ScreenId FROM Screen WHERE ScreenCode = 'RP'
SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'Admin'
IF NOT EXISTS ( SELECT TOP 1 1 FROM ScreenUserRoleMap WHERE ScreenId = @ScreenId AND RoleId = @RoleId)
	BEGIN
		INSERT INTO [dbo].[ScreenUserRoleMap]
           ([ScreenId]
           ,[RoleId]
           ,[RecordStatus]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[ModifiedBy]
           ,[ModifiedDate]
           ,[CanEdit]
           ,[DisplayScreenName])
		VALUES
		(
			@ScreenId, 			
			@RoleId,
			@RecordStatus,	
			@UserName,				
			@CurrentDate,	
			@UserName,				
			@CurrentDate,	
			NULL,
			'Reports'
		)
	END

SELECT  @ScreenId = ScreenId FROM Screen WHERE ScreenCode = 'RP'
SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'Manager'
IF NOT EXISTS ( SELECT TOP 1 1 FROM ScreenUserRoleMap WHERE ScreenId = @ScreenId AND RoleId = @RoleId)
	BEGIN
		INSERT INTO [dbo].[ScreenUserRoleMap]
           ([ScreenId]
           ,[RoleId]
           ,[RecordStatus]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[ModifiedBy]
           ,[ModifiedDate]
           ,[CanEdit]
           ,[DisplayScreenName])
		VALUES
		(
			@ScreenId, 			
			@RoleId,
			@RecordStatus,	
			@UserName,				
			@CurrentDate,	
			@UserName,				
			@CurrentDate,	
			NULL,
			'Reports'
		)
	END

SELECT  @ScreenId = ScreenId FROM Screen WHERE ScreenCode = 'RP'
SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'Executive'
IF NOT EXISTS ( SELECT TOP 1 1 FROM ScreenUserRoleMap WHERE ScreenId = @ScreenId AND RoleId = @RoleId)
	BEGIN
		INSERT INTO [dbo].[ScreenUserRoleMap]
           ([ScreenId]
           ,[RoleId]
           ,[RecordStatus]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[ModifiedBy]
           ,[ModifiedDate]
           ,[CanEdit]
           ,[DisplayScreenName])
		VALUES
		(
			@ScreenId, 			
			@RoleId,
			@RecordStatus,	
			@UserName,				
			@CurrentDate,	
			@UserName,				
			@CurrentDate,	
			NULL,
			'Reports'
		)
	END

-- Edit permissions of Completed Checklists page
SELECT @ScreenId = ScreenId FROM Screen WHERE ScreenCode = 'CCL'
IF EXISTS(SELECT TOP 1 1 FROM ScreenUserRoleMap WHERE ScreenId = @ScreenId)
BEGIN
	SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'Admin'
	IF EXISTS(SELECT TOP 1 1 FROM ScreenUserRoleMap WHERE ScreenId = @ScreenId AND RoleId = @RoleId AND CanEdit IS NULL)
	BEGIN
		UPDATE ScreenUserRoleMap
		SET CanEdit = 1
		WHERE ScreenId = @ScreenId AND RoleId = @RoleId
	END

	SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'Executive'
	IF EXISTS(SELECT TOP 1 1 FROM ScreenUserRoleMap WHERE ScreenId = @ScreenId AND RoleId = @RoleId AND CanEdit IS NULL)
	BEGIN
		UPDATE ScreenUserRoleMap
		SET CanEdit = 0
		WHERE ScreenId = @ScreenId AND RoleId = @RoleId
	END

	SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'Manager'
	IF EXISTS(SELECT TOP 1 1 FROM ScreenUserRoleMap WHERE ScreenId = @ScreenId AND RoleId = @RoleId AND CanEdit IS NULL)
	BEGIN
		UPDATE ScreenUserRoleMap
		SET CanEdit = 0
		WHERE ScreenId = @ScreenId AND RoleId = @RoleId
	END

	SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'User'
	IF EXISTS(SELECT TOP 1 1 FROM ScreenUserRoleMap WHERE ScreenId = @ScreenId AND RoleId = @RoleId AND CanEdit IS NULL)
	BEGIN
		UPDATE ScreenUserRoleMap
		SET CanEdit = 0
		WHERE ScreenId = @ScreenId AND RoleId = @RoleId
	END
END

SELECT @ScreenId = ScreenId FROM Screen WHERE ScreenCode = 'VACL'
SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'Admin'
IF EXISTS ( SELECT TOP 1 1 FROM ScreenUserRoleMap WHERE ScreenId = @ScreenId AND RoleId = @RoleId)
BEGIN
   UPDATE ScreenUserRoleMap 
   SET CanEdit = 1 
   WHERE ScreenId = @ScreenId AND RoleId = @RoleId
END

SELECT @ScreenId = ScreenId FROM Screen WHERE ScreenCode = 'VACL'
SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'Executive'
IF EXISTS ( SELECT TOP 1 1 FROM ScreenUserRoleMap WHERE ScreenId = @ScreenId AND RoleId = @RoleId)
BEGIN
   UPDATE ScreenUserRoleMap 
   SET CanEdit = 1 
   WHERE ScreenId = @ScreenId AND RoleId = @RoleId
END

SELECT @ScreenId = ScreenId FROM Screen WHERE ScreenCode = 'CL'
SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'Admin'
IF EXISTS ( SELECT TOP 1 1 FROM ScreenUserRoleMap WHERE ScreenId = @ScreenId AND RoleId = @RoleId)
BEGIN
   UPDATE ScreenUserRoleMap 
   SET DisplayScreenName = 'Checklists' 
   WHERE ScreenId = @ScreenId AND RoleId = @RoleId
END

SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'Executive'
IF EXISTS ( SELECT TOP 1 1 FROM ScreenUserRoleMap WHERE ScreenId = @ScreenId AND RoleId = @RoleId)
BEGIN
   UPDATE ScreenUserRoleMap 
   SET DisplayScreenName = 'Checklists' 
   WHERE ScreenId = @ScreenId AND RoleId = @RoleId
END

SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'Manager'
IF EXISTS ( SELECT TOP 1 1 FROM ScreenUserRoleMap WHERE ScreenId = @ScreenId AND RoleId = @RoleId)
BEGIN
   UPDATE ScreenUserRoleMap 
   SET DisplayScreenName = 'Checklists' 
   WHERE ScreenId = @ScreenId AND RoleId = @RoleId
END

SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'User'
IF EXISTS ( SELECT TOP 1 1 FROM ScreenUserRoleMap WHERE ScreenId = @ScreenId AND RoleId = @RoleId)
BEGIN
   UPDATE ScreenUserRoleMap 
   SET DisplayScreenName = 'Checklists' 
   WHERE ScreenId = @ScreenId AND RoleId = @RoleId
END

SELECT @ScreenId = ScreenId FROM Screen WHERE ScreenCode = 'PCL'
SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'Admin'
IF EXISTS ( SELECT TOP 1 1 FROM ScreenUserRoleMap WHERE ScreenId = @ScreenId AND RoleId = @RoleId)
BEGIN
   UPDATE ScreenUserRoleMap 
   SET DisplayScreenName = 'Pending Checklists' 
   WHERE ScreenId = @ScreenId AND RoleId = @RoleId
END

SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'Executive'
IF EXISTS ( SELECT TOP 1 1 FROM ScreenUserRoleMap WHERE ScreenId = @ScreenId AND RoleId = @RoleId)
BEGIN
   UPDATE ScreenUserRoleMap 
   SET DisplayScreenName = 'Pending Checklists' 
   WHERE ScreenId = @ScreenId AND RoleId = @RoleId
END

SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'Manager'
IF EXISTS ( SELECT TOP 1 1 FROM ScreenUserRoleMap WHERE ScreenId = @ScreenId AND RoleId = @RoleId)
BEGIN
   UPDATE ScreenUserRoleMap 
   SET DisplayScreenName = 'Pending Checklists' 
   WHERE ScreenId = @ScreenId AND RoleId = @RoleId
END

SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'User'
IF EXISTS ( SELECT TOP 1 1 FROM ScreenUserRoleMap WHERE ScreenId = @ScreenId AND RoleId = @RoleId)
BEGIN
   UPDATE ScreenUserRoleMap 
   SET DisplayScreenName = 'Pending Checklists' 
   WHERE ScreenId = @ScreenId AND RoleId = @RoleId
END


SELECT @ScreenId = ScreenId FROM Screen WHERE ScreenCode = 'CCL'
SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'Admin'
IF EXISTS ( SELECT TOP 1 1 FROM ScreenUserRoleMap WHERE ScreenId = @ScreenId AND RoleId = @RoleId)
BEGIN
   UPDATE ScreenUserRoleMap 
   SET DisplayScreenName = 'Completed Checklists' 
   WHERE ScreenId = @ScreenId AND RoleId = @RoleId
END

SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'Executive'
IF EXISTS ( SELECT TOP 1 1 FROM ScreenUserRoleMap WHERE ScreenId = @ScreenId AND RoleId = @RoleId)
BEGIN
   UPDATE ScreenUserRoleMap 
   SET DisplayScreenName = 'Completed Checklists' 
   WHERE ScreenId = @ScreenId AND RoleId = @RoleId
END

SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'Manager'
IF EXISTS ( SELECT TOP 1 1 FROM ScreenUserRoleMap WHERE ScreenId = @ScreenId AND RoleId = @RoleId)
BEGIN
   UPDATE ScreenUserRoleMap 
   SET DisplayScreenName = 'Completed Checklists' 
   WHERE ScreenId = @ScreenId AND RoleId = @RoleId
END

SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'User'
IF EXISTS ( SELECT TOP 1 1 FROM ScreenUserRoleMap WHERE ScreenId = @ScreenId AND RoleId = @RoleId)
BEGIN
   UPDATE ScreenUserRoleMap 
   SET DisplayScreenName = 'Completed Checklists' 
   WHERE ScreenId = @ScreenId AND RoleId = @RoleId
END

SELECT @ScreenId = ScreenId FROM Screen WHERE ScreenCode = 'CVE'
SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'Admin'
IF EXISTS ( SELECT TOP 1 1 FROM ScreenUserRoleMap WHERE ScreenId = @ScreenId AND RoleId = @RoleId)
BEGIN
   UPDATE ScreenUserRoleMap 
   SET DisplayScreenName = 'View / Edit Client' 
   WHERE ScreenId = @ScreenId AND RoleId = @RoleId
END

SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'Executive'
IF EXISTS ( SELECT TOP 1 1 FROM ScreenUserRoleMap WHERE ScreenId = @ScreenId AND RoleId = @RoleId)
BEGIN
   UPDATE ScreenUserRoleMap 
   SET DisplayScreenName = 'View / Edit Client' 
   WHERE ScreenId = @ScreenId AND RoleId = @RoleId
END

SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'Manager'
IF EXISTS ( SELECT TOP 1 1 FROM ScreenUserRoleMap WHERE ScreenId = @ScreenId AND RoleId = @RoleId)
BEGIN
   UPDATE ScreenUserRoleMap 
   SET DisplayScreenName = 'View / Edit Client' 
   WHERE ScreenId = @ScreenId AND RoleId = @RoleId
END

SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'User'
IF EXISTS ( SELECT TOP 1 1 FROM ScreenUserRoleMap WHERE ScreenId = @ScreenId AND RoleId = @RoleId)
BEGIN
   UPDATE ScreenUserRoleMap 
   SET DisplayScreenName = 'View / Edit Client' 
   WHERE ScreenId = @ScreenId AND RoleId = @RoleId
END

SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'Client'
IF EXISTS ( SELECT TOP 1 1 FROM ScreenUserRoleMap WHERE ScreenId = @ScreenId AND RoleId = @RoleId)
BEGIN
   UPDATE ScreenUserRoleMap 
   SET DisplayScreenName = 'View KPI' 
   WHERE ScreenId = @ScreenId AND RoleId = @RoleId
END

SELECT @ScreenId = ScreenId FROM Screen WHERE ScreenCode = 'CN'
SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'Admin'
IF EXISTS ( SELECT TOP 1 1 FROM ScreenUserRoleMap WHERE ScreenId = @ScreenId AND RoleId = @RoleId)
BEGIN
   UPDATE ScreenUserRoleMap 
   SET DisplayScreenName = 'Create Client' 
   WHERE ScreenId = @ScreenId AND RoleId = @RoleId
END

SELECT @ScreenId = ScreenId FROM Screen WHERE ScreenCode = 'VA'
SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'Admin'
IF EXISTS ( SELECT TOP 1 1 FROM ScreenUserRoleMap WHERE ScreenId = @ScreenId AND RoleId = @RoleId)
BEGIN
   UPDATE ScreenUserRoleMap 
   SET DisplayScreenName = 'All Clients' 
   WHERE ScreenId = @ScreenId AND RoleId = @RoleId
END

SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'Executive'
IF EXISTS ( SELECT TOP 1 1 FROM ScreenUserRoleMap WHERE ScreenId = @ScreenId AND RoleId = @RoleId)
BEGIN
   UPDATE ScreenUserRoleMap 
   SET DisplayScreenName = 'All Clients' 
   WHERE ScreenId = @ScreenId AND RoleId = @RoleId
END

SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'Manager'
IF EXISTS ( SELECT TOP 1 1 FROM ScreenUserRoleMap WHERE ScreenId = @ScreenId AND RoleId = @RoleId)
BEGIN
   UPDATE ScreenUserRoleMap 
   SET DisplayScreenName = 'All Clients' 
   WHERE ScreenId = @ScreenId AND RoleId = @RoleId
END

SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'User'
IF EXISTS ( SELECT TOP 1 1 FROM ScreenUserRoleMap WHERE ScreenId = @ScreenId AND RoleId = @RoleId)
BEGIN
   UPDATE ScreenUserRoleMap 
   SET DisplayScreenName = 'All Clients' 
   WHERE ScreenId = @ScreenId AND RoleId = @RoleId
END

SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'Client'
IF EXISTS ( SELECT TOP 1 1 FROM ScreenUserRoleMap WHERE ScreenId = @ScreenId AND RoleId = @RoleId)
BEGIN
   UPDATE ScreenUserRoleMap 
   SET DisplayScreenName = 'All Clients' 
   WHERE ScreenId = @ScreenId AND RoleId = @RoleId
END


SELECT @ScreenId = ScreenId FROM Screen WHERE ScreenCode = 'ACL'
SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'Admin'
IF EXISTS ( SELECT TOP 1 1 FROM ScreenUserRoleMap WHERE ScreenId = @ScreenId AND RoleId = @RoleId)
BEGIN
   UPDATE ScreenUserRoleMap 
   SET DisplayScreenName = 'Checklists' 
   WHERE ScreenId = @ScreenId AND RoleId = @RoleId
END

SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'Executive'
IF EXISTS ( SELECT TOP 1 1 FROM ScreenUserRoleMap WHERE ScreenId = @ScreenId AND RoleId = @RoleId)
BEGIN
   UPDATE ScreenUserRoleMap 
   SET DisplayScreenName = 'Checklists' 
   WHERE ScreenId = @ScreenId AND RoleId = @RoleId
END

SELECT @ScreenId = ScreenId FROM Screen WHERE ScreenCode = 'VEK'
SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'Admin'
IF EXISTS ( SELECT TOP 1 1 FROM ScreenUserRoleMap WHERE ScreenId = @ScreenId AND RoleId = @RoleId)
BEGIN
   UPDATE ScreenUserRoleMap 
   SET DisplayScreenName = 'View / Edit KPI' 
   WHERE ScreenId = @ScreenId AND RoleId = @RoleId
END

SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'Executive'
IF EXISTS ( SELECT TOP 1 1 FROM ScreenUserRoleMap WHERE ScreenId = @ScreenId AND RoleId = @RoleId)
BEGIN
   UPDATE ScreenUserRoleMap 
   SET DisplayScreenName = 'View / Edit KPI' 
   WHERE ScreenId = @ScreenId AND RoleId = @RoleId
END

SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'Manager'
IF EXISTS ( SELECT TOP 1 1 FROM ScreenUserRoleMap WHERE ScreenId = @ScreenId AND RoleId = @RoleId)
BEGIN
   UPDATE ScreenUserRoleMap 
   SET DisplayScreenName = 'View KPI' 
   WHERE ScreenId = @ScreenId AND RoleId = @RoleId
END

SELECT @ScreenId = ScreenId FROM Screen WHERE ScreenCode = 'CECI'
SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'Admin'
IF EXISTS ( SELECT TOP 1 1 FROM ScreenUserRoleMap WHERE ScreenId = @ScreenId AND RoleId = @RoleId)
BEGIN
   UPDATE ScreenUserRoleMap 
   SET DisplayScreenName = 'Create / Edit Checklist Item' 
   WHERE ScreenId = @ScreenId AND RoleId = @RoleId
END

SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'Executive'
IF EXISTS ( SELECT TOP 1 1 FROM ScreenUserRoleMap WHERE ScreenId = @ScreenId AND RoleId = @RoleId)
BEGIN
   UPDATE ScreenUserRoleMap 
   SET DisplayScreenName = 'Create / Edit Checklist Item' 
   WHERE ScreenId = @ScreenId AND RoleId = @RoleId
END

SELECT @ScreenId = ScreenId FROM Screen WHERE ScreenCode = 'CLS'
SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'Admin'
IF EXISTS ( SELECT TOP 1 1 FROM ScreenUserRoleMap WHERE ScreenId = @ScreenId AND RoleId = @RoleId)
BEGIN
   UPDATE ScreenUserRoleMap 
   SET DisplayScreenName = 'Create Checklist' 
   WHERE ScreenId = @ScreenId AND RoleId = @RoleId
END

SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'Executive'
IF EXISTS ( SELECT TOP 1 1 FROM ScreenUserRoleMap WHERE ScreenId = @ScreenId AND RoleId = @RoleId)
BEGIN
   UPDATE ScreenUserRoleMap 
   SET DisplayScreenName = 'Create Checklist' 
   WHERE ScreenId = @ScreenId AND RoleId = @RoleId
END

SELECT @ScreenId = ScreenId FROM Screen WHERE ScreenCode = 'VACL'
SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'Admin'
IF EXISTS ( SELECT TOP 1 1 FROM ScreenUserRoleMap WHERE ScreenId = @ScreenId AND RoleId = @RoleId)
BEGIN
   UPDATE ScreenUserRoleMap 
   SET DisplayScreenName = 'All Checklists' 
   WHERE ScreenId = @ScreenId AND RoleId = @RoleId
END

SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'Executive'
IF EXISTS ( SELECT TOP 1 1 FROM ScreenUserRoleMap WHERE ScreenId = @ScreenId AND RoleId = @RoleId)
BEGIN
   UPDATE ScreenUserRoleMap 
   SET DisplayScreenName = 'All Checklists' 
   WHERE ScreenId = @ScreenId AND RoleId = @RoleId
END

SELECT @ScreenId = ScreenId FROM Screen WHERE ScreenCode = 'CCU'
SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'Manager'
IF EXISTS ( SELECT TOP 1 1 FROM ScreenUserRoleMap WHERE ScreenId = @ScreenId AND RoleId = @RoleId)
BEGIN
   UPDATE ScreenUserRoleMap 
   SET DisplayScreenName = 'Create Client Users' 
   WHERE ScreenId = @ScreenId AND RoleId = @RoleId
END


SELECT @ScreenId = ScreenId FROM Screen WHERE ScreenCode = 'VEK'
SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'Admin'
IF EXISTS ( SELECT TOP 1 1 FROM ScreenUserRoleMap WHERE ScreenId = @ScreenId AND RoleId = @RoleId)
BEGIN
   UPDATE ScreenUserRoleMap 
   SET CanEdit = 1   
   WHERE ScreenId = @ScreenId AND RoleId = @RoleId
END

SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'Executive'
IF EXISTS ( SELECT TOP 1 1 FROM ScreenUserRoleMap WHERE ScreenId = @ScreenId AND RoleId = @RoleId)
BEGIN
   UPDATE ScreenUserRoleMap 
   SET CanEdit = 1 
   WHERE ScreenId = @ScreenId AND RoleId = @RoleId
END

SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'Manager'
IF EXISTS ( SELECT TOP 1 1 FROM ScreenUserRoleMap WHERE ScreenId = @ScreenId AND RoleId = @RoleId)
BEGIN
   UPDATE ScreenUserRoleMap 
   SET CanEdit = 0 
   WHERE ScreenId = @ScreenId AND RoleId = @RoleId
END

SELECT @ScreenId = ScreenId FROM Screen WHERE ScreenCode = 'AHM'
IF EXISTS ( SELECT TOP 1 1 FROM ScreenUserRoleMap WHERE ScreenId = @ScreenId AND DisplayScreenName = 'Heat Map')
BEGIN
   UPDATE ScreenUserRoleMap 
   SET DisplayScreenName = 'Heat Map Setup'
   WHERE ScreenId = @ScreenId
END

SELECT @ScreenId = ScreenId FROM Screen WHERE ScreenCode = 'CK'
SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'Admin'
IF EXISTS ( SELECT TOP 1 1 FROM ScreenUserRoleMap WHERE ScreenId = @ScreenId AND RoleId = @RoleId AND CanEdit IS NULL)
BEGIN
   UPDATE ScreenUserRoleMap
   SET CanEdit = 1
   WHERE ScreenId = @ScreenId AND RoleId = @RoleId
END

SELECT @ScreenId = ScreenId FROM Screen WHERE ScreenCode = 'CK'
SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'Executive'
IF EXISTS ( SELECT TOP 1 1 FROM ScreenUserRoleMap WHERE ScreenId = @ScreenId AND RoleId = @RoleId AND CanEdit IS NULL)
BEGIN
   UPDATE ScreenUserRoleMap
   SET CanEdit = 1
   WHERE ScreenId = @ScreenId AND RoleId = @RoleId
END

SELECT @ScreenId = ScreenId FROM Screen WHERE ScreenCode = 'SYS'
SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'Admin'
IF EXISTS ( SELECT TOP 1 1 FROM ScreenUserRoleMap WHERE ScreenId = @ScreenId AND RoleId = @RoleId AND CanEdit IS NULL)
BEGIN
   UPDATE ScreenUserRoleMap
   SET CanEdit = 1
   WHERE ScreenId = @ScreenId AND RoleId = @RoleId
END

SELECT @ScreenId = ScreenId FROM Screen WHERE ScreenCode = 'SYS'
SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'Executive'
IF EXISTS ( SELECT TOP 1 1 FROM ScreenUserRoleMap WHERE ScreenId = @ScreenId AND RoleId = @RoleId AND CanEdit IS NULL)
BEGIN
   UPDATE ScreenUserRoleMap
   SET CanEdit = 0
   WHERE ScreenId = @ScreenId AND RoleId = @RoleId
END

-- Insert scripts for to-do-list page access.

SELECT  @ScreenId = ScreenId FROM Screen WHERE ScreenCode = 'TDL'
SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'Admin'
IF NOT EXISTS ( SELECT TOP 1 1 FROM ScreenUserRoleMap WHERE ScreenId = @ScreenId AND RoleId = @RoleId)
BEGIN
		INSERT INTO [dbo].[ScreenUserRoleMap]
           ([ScreenId]
           ,[RoleId]
           ,[RecordStatus]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[ModifiedBy]
           ,[ModifiedDate]
           ,[CanEdit]
           ,[DisplayScreenName])
		VALUES
		(
			@ScreenId, 			
			@RoleId,
			@RecordStatus,	
			@UserName,				
			@CurrentDate,	
			@UserName,				
			@CurrentDate,	
			1,
			'To-Do-List'
		)
END

SELECT  @ScreenId = ScreenId FROM Screen WHERE ScreenCode = 'TDL'
SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'Executive'
IF NOT EXISTS ( SELECT TOP 1 1 FROM ScreenUserRoleMap WHERE ScreenId = @ScreenId AND RoleId = @RoleId)
BEGIN
		INSERT INTO [dbo].[ScreenUserRoleMap]
           ([ScreenId]
           ,[RoleId]
           ,[RecordStatus]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[ModifiedBy]
           ,[ModifiedDate]
           ,[CanEdit]
           ,[DisplayScreenName])
		VALUES
		(
			@ScreenId, 			
			@RoleId,
			@RecordStatus,	
			@UserName,				
			@CurrentDate,	
			@UserName,				
			@CurrentDate,	
			1,
			'To-Do-List'
		)
END

SELECT  @ScreenId = ScreenId FROM Screen WHERE ScreenCode = 'TDL'
SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'Manager'
IF NOT EXISTS ( SELECT TOP 1 1 FROM ScreenUserRoleMap WHERE ScreenId = @ScreenId AND RoleId = @RoleId)
BEGIN
		INSERT INTO [dbo].[ScreenUserRoleMap]
           ([ScreenId]
           ,[RoleId]
           ,[RecordStatus]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[ModifiedBy]
           ,[ModifiedDate]
           ,[CanEdit]
           ,[DisplayScreenName])
		VALUES
		(
			@ScreenId, 			
			@RoleId,
			@RecordStatus,	
			@UserName,				
			@CurrentDate,	
			@UserName,				
			@CurrentDate,	
			1,
			'To-Do-List'
		)
END

SELECT  @ScreenId = ScreenId FROM Screen WHERE ScreenCode = 'TDL'
SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'User'
IF NOT EXISTS ( SELECT TOP 1 1 FROM ScreenUserRoleMap WHERE ScreenId = @ScreenId AND RoleId = @RoleId)
BEGIN
		INSERT INTO [dbo].[ScreenUserRoleMap]
           ([ScreenId]
           ,[RoleId]
           ,[RecordStatus]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[ModifiedBy]
           ,[ModifiedDate]
           ,[CanEdit]
           ,[DisplayScreenName])
		VALUES
		(
			@ScreenId, 			
			@RoleId,
			@RecordStatus,	
			@UserName,				
			@CurrentDate,	
			@UserName,				
			@CurrentDate,	
			1,
			'To-Do-List'
		)
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

/*
Script Name : 12_ScreenAction
Module_Name : M3Pact
Created By  : Abhishek K
CreatedDate : 05/10/2018

Data Verification Script :

SELECT * FROM [ScreenAction](NOLOCK) WHERE RecordStatus = 'A'


Revision History 
=============
Ver  Date				Who						Comment
1.0  30-July-2018		Abhishek Kovvuri		Inserting View Actions for All Clients page.
1.1  08-Aug-2018        Abhishek Kovvuri        Inserting actions for to-do-list page.
*/

BEGIN TRY
BEGIN TRANSACTION

DECLARE @UserName NVARCHAR(60) = 'Admin'
DECLARE @CurrentDate DATETIME = GETDATE()
DECLARE @RecordStatus CHAR(1) = 'A'
DECLARE @ScreenId INT;

SELECT  @ScreenId = ScreenId FROM Screen WHERE ScreenCode = 'DL'
IF NOT EXISTS ( SELECT TOP 1 1 FROM ScreenAction WHERE ActionName = 'ViewCloseMonthButton')
	BEGIN
		INSERT INTO [dbo].[ScreenAction]
           ([ScreenId]
           ,[ActionName]
           ,[RecordStatus]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[ModifiedBy]
           ,[ModifiedDate])
		VALUES
		( 
		    @ScreenId,
			'ViewCloseMonthButton',	  --    ScreenId
			@RecordStatus,	          --	RecordStatus
			@UserName,				  --	CreatedBy
			@CurrentDate,			  --	CreatedDate
			@UserName,				  --	ModifiedBy
			@CurrentDate			  --	ModifiedDate
		)
	END

SELECT  @ScreenId = ScreenId FROM Screen WHERE ScreenCode = 'DL'
IF NOT EXISTS ( SELECT TOP 1 1 FROM ScreenAction WHERE ActionName = 'ViewAddDepositSection')
	BEGIN
		INSERT INTO [dbo].[ScreenAction]
           ([ScreenId]
           ,[ActionName]
           ,[RecordStatus]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[ModifiedBy]
           ,[ModifiedDate])
		VALUES
		( 
		    @ScreenId,
			'ViewAddDepositSection',	     --  ScreenId
			@RecordStatus,	                 --	 RecordStatus
			@UserName,				         --	 CreatedBy
			@CurrentDate,			         --	 CreatedDate
			@UserName,				         --	 ModifiedBy
			@CurrentDate			         --	 ModifiedDate
		)
	END

SELECT  @ScreenId = ScreenId FROM Screen WHERE ScreenCode = 'DL'
IF NOT EXISTS ( SELECT TOP 1 1 FROM ScreenAction WHERE ActionName = 'OpenClosedMonth')
	BEGIN
		INSERT INTO [dbo].[ScreenAction]
           ([ScreenId]
           ,[ActionName]
           ,[RecordStatus]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[ModifiedBy]
           ,[ModifiedDate])
		VALUES
		( 
		    @ScreenId,
			'OpenClosedMonth',	    
			@RecordStatus,	        
			@UserName,				
			@CurrentDate,			
			@UserName,				
			@CurrentDate			
		)
	END

SELECT  @ScreenId = ScreenId FROM Screen WHERE ScreenCode = 'SITES'
IF NOT EXISTS ( SELECT TOP 1 1 FROM ScreenAction WHERE ActionName = 'AddSite')
	BEGIN
		INSERT INTO [dbo].[ScreenAction]
           ([ScreenId]
           ,[ActionName]
           ,[RecordStatus]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[ModifiedBy]
           ,[ModifiedDate])
		VALUES
		( 
		    @ScreenId,
			'AddSite',	    
			@RecordStatus,	        
			@UserName,				
			@CurrentDate,			
			@UserName,				
			@CurrentDate			
		)
	END

SELECT  @ScreenId = ScreenId FROM Screen WHERE ScreenCode = 'SITES'
IF NOT EXISTS ( SELECT TOP 1 1 FROM ScreenAction WHERE ActionName = 'EditSite')
	BEGIN
		INSERT INTO [dbo].[ScreenAction]
           ([ScreenId]
           ,[ActionName]
           ,[RecordStatus]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[ModifiedBy]
           ,[ModifiedDate])
		VALUES
		( 
		    @ScreenId,
			'EditSite',	    
			@RecordStatus,	        
			@UserName,				
			@CurrentDate,			
			@UserName,				
			@CurrentDate			
		)
	END

SELECT  @ScreenId = ScreenId FROM Screen WHERE ScreenCode = 'SPEC'
IF NOT EXISTS ( SELECT TOP 1 1 FROM ScreenAction WHERE ActionName = 'AddSpecialty')
	BEGIN
		INSERT INTO [dbo].[ScreenAction]
           ([ScreenId]
           ,[ActionName]
           ,[RecordStatus]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[ModifiedBy]
           ,[ModifiedDate])
		VALUES
		( 
		    @ScreenId,
			'AddSpecialty',	    
			@RecordStatus,	        
			@UserName,				
			@CurrentDate,			
			@UserName,				
			@CurrentDate			
		)
	END

SELECT  @ScreenId = ScreenId FROM Screen WHERE ScreenCode = 'SPEC'
IF NOT EXISTS ( SELECT TOP 1 1 FROM ScreenAction WHERE ActionName = 'EditSpecialty')
	BEGIN
		INSERT INTO [dbo].[ScreenAction]
           ([ScreenId]
           ,[ActionName]
           ,[RecordStatus]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[ModifiedBy]
           ,[ModifiedDate])
		VALUES
		( 
		    @ScreenId,
			'EditSpecialty',	    
			@RecordStatus,	        
			@UserName,				
			@CurrentDate,			
			@UserName,				
			@CurrentDate			
		)
	END

SELECT  @ScreenId = ScreenId FROM Screen WHERE ScreenCode = 'CD'
IF NOT EXISTS ( SELECT TOP 1 1 FROM ScreenAction WHERE ActionName = 'PerformClientActivateInactivate')
	BEGIN
		INSERT INTO [dbo].[ScreenAction]
           ([ScreenId]
           ,[ActionName]
           ,[RecordStatus]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[ModifiedBy]
           ,[ModifiedDate])
		VALUES
		( 
		    @ScreenId,
			'PerformClientActivateInactivate',	    
			@RecordStatus,	        
			@UserName,				
			@CurrentDate,			
			@UserName,				
			@CurrentDate			
		)
	END

SELECT  @ScreenId = ScreenId FROM Screen WHERE ScreenCode = 'CD'
IF NOT EXISTS ( SELECT TOP 1 1 FROM ScreenAction WHERE ActionName = 'EditClientData')
	BEGIN
		INSERT INTO [dbo].[ScreenAction]
           ([ScreenId]
           ,[ActionName]
           ,[RecordStatus]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[ModifiedBy]
           ,[ModifiedDate])
		VALUES
		( 
		    @ScreenId,
			'EditClientData',	    
			@RecordStatus,	        
			@UserName,				
			@CurrentDate,			
			@UserName,				
			@CurrentDate			
		)
	END

SELECT  @ScreenId = ScreenId FROM Screen WHERE ScreenCode = 'CD'
IF NOT EXISTS ( SELECT TOP 1 1 FROM ScreenAction WHERE ActionName = 'AddClientData')
	BEGIN
		INSERT INTO [dbo].[ScreenAction]
           ([ScreenId]
           ,[ActionName]
           ,[RecordStatus]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[ModifiedBy]
           ,[ModifiedDate])
		VALUES
		( 
		    @ScreenId,
			'AddClientData',	    
			@RecordStatus,	        
			@UserName,				
			@CurrentDate,			
			@UserName,				
			@CurrentDate			
		)
	END

SELECT  @ScreenId = ScreenId FROM Screen WHERE ScreenCode = 'LS'
IF NOT EXISTS ( SELECT TOP 1 1 FROM ScreenAction WHERE ActionName = 'AddPayerAssociationToClient')
	BEGIN
		INSERT INTO [dbo].[ScreenAction]
           ([ScreenId]
           ,[ActionName]
           ,[RecordStatus]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[ModifiedBy]
           ,[ModifiedDate])
		VALUES
		( 
		    @ScreenId,
			'AddPayerAssociationToClient',	    
			@RecordStatus,	        
			@UserName,				
			@CurrentDate,			
			@UserName,				
			@CurrentDate			
		)
	END

SELECT  @ScreenId = ScreenId FROM Screen WHERE ScreenCode = 'LS'
IF NOT EXISTS ( SELECT TOP 1 1 FROM ScreenAction WHERE ActionName = 'EditPayerAssociationToClient')
	BEGIN
		INSERT INTO [dbo].[ScreenAction]
           ([ScreenId]
           ,[ActionName]
           ,[RecordStatus]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[ModifiedBy]
           ,[ModifiedDate])
		VALUES
		( 
		    @ScreenId,
			'EditPayerAssociationToClient',	    
			@RecordStatus,	        
			@UserName,				
			@CurrentDate,			
			@UserName,				
			@CurrentDate			
		)
	END

SELECT  @ScreenId = ScreenId FROM Screen WHERE ScreenCode = 'LS'
IF NOT EXISTS ( SELECT TOP 1 1 FROM ScreenAction WHERE ActionName = 'PerformClientPayerActivateInactivate')
	BEGIN
		INSERT INTO [dbo].[ScreenAction]
           ([ScreenId]
           ,[ActionName]
           ,[RecordStatus]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[ModifiedBy]
           ,[ModifiedDate])
		VALUES
		( 
		    @ScreenId,
			'PerformClientPayerActivateInactivate',	    
			@RecordStatus,	        
			@UserName,				
			@CurrentDate,			
			@UserName,				
			@CurrentDate			
		)
	END

SELECT  @ScreenId = ScreenId FROM Screen WHERE ScreenCode = 'CAU'
IF NOT EXISTS ( SELECT TOP 1 1 FROM ScreenAction WHERE ActionName = 'AddUserAssociationToClient')
	BEGIN
		INSERT INTO [dbo].[ScreenAction]
           ([ScreenId]
           ,[ActionName]
           ,[RecordStatus]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[ModifiedBy]
           ,[ModifiedDate])
		VALUES
		( 
		    @ScreenId,
			'AddUserAssociationToClient',	    
			@RecordStatus,	        
			@UserName,				
			@CurrentDate,			
			@UserName,				
			@CurrentDate			
		)
	END

SELECT  @ScreenId = ScreenId FROM Screen WHERE ScreenCode = 'CAU'
IF NOT EXISTS ( SELECT TOP 1 1 FROM ScreenAction WHERE ActionName = 'EditUserAssociationToClient')
	BEGIN
		INSERT INTO [dbo].[ScreenAction]
           ([ScreenId]
           ,[ActionName]
           ,[RecordStatus]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[ModifiedBy]
           ,[ModifiedDate])
		VALUES
		( 
		    @ScreenId,
			'EditUserAssociationToClient',	    
			@RecordStatus,	        
			@UserName,				
			@CurrentDate,			
			@UserName,				
			@CurrentDate			
		)
	END

SELECT  @ScreenId = ScreenId FROM Screen WHERE ScreenCode = 'MT'
IF NOT EXISTS ( SELECT TOP 1 1 FROM ScreenAction WHERE ActionName = 'AddMonthlyTargets')
	BEGIN
		INSERT INTO [dbo].[ScreenAction]
           ([ScreenId]
           ,[ActionName]
           ,[RecordStatus]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[ModifiedBy]
           ,[ModifiedDate])
		VALUES
		( 
		    @ScreenId,
			'AddMonthlyTargets',	    
			@RecordStatus,	        
			@UserName,				
			@CurrentDate,			
			@UserName,				
			@CurrentDate			
		)
	END

SELECT  @ScreenId = ScreenId FROM Screen WHERE ScreenCode = 'MT'
IF NOT EXISTS ( SELECT TOP 1 1 FROM ScreenAction WHERE ActionName = 'EditMonthlyTargets')
	BEGIN
		INSERT INTO [dbo].[ScreenAction]
           ([ScreenId]
           ,[ActionName]
           ,[RecordStatus]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[ModifiedBy]
           ,[ModifiedDate])
		VALUES
		( 
		    @ScreenId,
			'EditMonthlyTargets',	    
			@RecordStatus,	        
			@UserName,				
			@CurrentDate,			
			@UserName,				
			@CurrentDate			
		)
	END

SELECT  @ScreenId = ScreenId FROM Screen WHERE ScreenCode = 'KPIS'
IF NOT EXISTS ( SELECT TOP 1 1 FROM ScreenAction WHERE ActionName = 'AddKPI')
	BEGIN
		INSERT INTO [dbo].[ScreenAction]
           ([ScreenId]
           ,[ActionName]
           ,[RecordStatus]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[ModifiedBy]
           ,[ModifiedDate])
		VALUES
		( 
		    @ScreenId,
			'AddKPI',	    
			@RecordStatus,	        
			@UserName,				
			@CurrentDate,			
			@UserName,				
			@CurrentDate			
		)
	END

SELECT  @ScreenId = ScreenId FROM Screen WHERE ScreenCode = 'KPIS'
IF NOT EXISTS ( SELECT TOP 1 1 FROM ScreenAction WHERE ActionName = 'EditKPI')
	BEGIN
		INSERT INTO [dbo].[ScreenAction]
           ([ScreenId]
           ,[ActionName]
           ,[RecordStatus]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[ModifiedBy]
           ,[ModifiedDate])
		VALUES
		( 
		    @ScreenId,
			'EditKPI',	    
			@RecordStatus,	        
			@UserName,				
			@CurrentDate,			
			@UserName,				
			@CurrentDate			
		)
	END

SELECT  @ScreenId = ScreenId FROM Screen WHERE ScreenCode = 'BD'
IF NOT EXISTS ( SELECT TOP 1 1 FROM ScreenAction WHERE ActionName = 'AddEditDeleteHolidaysInBusinessDays')
	BEGIN
		INSERT INTO [dbo].[ScreenAction]
           ([ScreenId]
           ,[ActionName]
           ,[RecordStatus]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[ModifiedBy]
           ,[ModifiedDate])
		VALUES
		( 
		    @ScreenId,
			'AddEditDeleteHolidaysInBusinessDays',	    
			@RecordStatus,	        
			@UserName,				
			@CurrentDate,			
			@UserName,				
			@CurrentDate			
		)
	END

SELECT  @ScreenId = ScreenId FROM Screen WHERE ScreenCode = 'BD'
IF NOT EXISTS ( SELECT TOP 1 1 FROM ScreenAction WHERE ActionName = 'ViewGridGraphInBusinessDays')
	BEGIN
		INSERT INTO [dbo].[ScreenAction]
           ([ScreenId]
           ,[ActionName]
           ,[RecordStatus]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[ModifiedBy]
           ,[ModifiedDate])
		VALUES
		( 
		    @ScreenId,
			'ViewGridGraphInBusinessDays',	    
			@RecordStatus,	        
			@UserName,				
			@CurrentDate,			
			@UserName,				
			@CurrentDate			
		)
	END

SELECT  @ScreenId = ScreenId FROM Screen WHERE ScreenCode = 'BU'
IF NOT EXISTS ( SELECT TOP 1 1 FROM ScreenAction WHERE ActionName = 'AddBusinessUnit')
	BEGIN
		INSERT INTO [dbo].[ScreenAction]
           ([ScreenId]
           ,[ActionName]
           ,[RecordStatus]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[ModifiedBy]
           ,[ModifiedDate])
		VALUES
		( 
		    @ScreenId,
			'AddBusinessUnit',	    
			@RecordStatus,	        
			@UserName,				
			@CurrentDate,			
			@UserName,				
			@CurrentDate			
		)
	END

SELECT  @ScreenId = ScreenId FROM Screen WHERE ScreenCode = 'BU'
IF NOT EXISTS ( SELECT TOP 1 1 FROM ScreenAction WHERE ActionName = 'EditBusinessUnit')
	BEGIN
		INSERT INTO [dbo].[ScreenAction]
           ([ScreenId]
           ,[ActionName]
           ,[RecordStatus]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[ModifiedBy]
           ,[ModifiedDate])
		VALUES
		( 
		    @ScreenId,
			'EditBusinessUnit',	    
			@RecordStatus,	        
			@UserName,				
			@CurrentDate,			
			@UserName,				
			@CurrentDate			
		)
	END

SELECT  @ScreenId = ScreenId FROM Screen WHERE ScreenCode = 'PAY'
IF NOT EXISTS ( SELECT TOP 1 1 FROM ScreenAction WHERE ActionName = 'AddPayer')
	BEGIN
		INSERT INTO [dbo].[ScreenAction]
           ([ScreenId]
           ,[ActionName]
           ,[RecordStatus]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[ModifiedBy]
           ,[ModifiedDate])
		VALUES
		( 
		    @ScreenId,
			'AddPayer',	    
			@RecordStatus,	        
			@UserName,				
			@CurrentDate,			
			@UserName,				
			@CurrentDate			
		)
	END

SELECT  @ScreenId = ScreenId FROM Screen WHERE ScreenCode = 'PAY'
IF NOT EXISTS ( SELECT TOP 1 1 FROM ScreenAction WHERE ActionName = 'EditPayerName')
	BEGIN
		INSERT INTO [dbo].[ScreenAction]
           ([ScreenId]
           ,[ActionName]
           ,[RecordStatus]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[ModifiedBy]
           ,[ModifiedDate])
		VALUES
		( 
		    @ScreenId,
			'EditPayerName',	    
			@RecordStatus,	        
			@UserName,				
			@CurrentDate,			
			@UserName,				
			@CurrentDate			
		)
	END

SELECT  @ScreenId = ScreenId FROM Screen WHERE ScreenCode = 'PAY'
IF NOT EXISTS ( SELECT TOP 1 1 FROM ScreenAction WHERE ActionName = 'MakePayerActiveInActive')
	BEGIN
		INSERT INTO [dbo].[ScreenAction]
           ([ScreenId]
           ,[ActionName]
           ,[RecordStatus]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[ModifiedBy]
           ,[ModifiedDate])
		VALUES
		( 
		    @ScreenId,
			'MakePayerActiveInActive',	    
			@RecordStatus,	        
			@UserName,				
			@CurrentDate,			
			@UserName,				
			@CurrentDate			
		)
	END


SELECT  @ScreenId = ScreenId FROM Screen WHERE ScreenCode = 'VA'
IF NOT EXISTS ( SELECT TOP 1 1 FROM ScreenAction WHERE ActionName = 'EditClient')
	BEGIN
		INSERT INTO [dbo].[ScreenAction]
           ([ScreenId]
           ,[ActionName]
           ,[RecordStatus]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[ModifiedBy]
           ,[ModifiedDate])
		VALUES
		( 
		    @ScreenId,
			'EditClient',	    
			@RecordStatus,	        
			@UserName,				
			@CurrentDate,			
			@UserName,				
			@CurrentDate			
		)
	END

SELECT  @ScreenId = ScreenId FROM Screen WHERE ScreenCode = 'SYS'
IF NOT EXISTS ( SELECT TOP 1 1 FROM ScreenAction WHERE ActionName = 'AddSystem')
	BEGIN
		INSERT INTO [dbo].[ScreenAction]
           ([ScreenId]
           ,[ActionName]
           ,[RecordStatus]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[ModifiedBy]
           ,[ModifiedDate])
		VALUES
		( 
		    @ScreenId,
			'AddSystem',	    
			@RecordStatus,	        
			@UserName,				
			@CurrentDate,			
			@UserName,				
			@CurrentDate			
		)
	END

SELECT  @ScreenId = ScreenId FROM Screen WHERE ScreenCode = 'SYS'
IF NOT EXISTS ( SELECT TOP 1 1 FROM ScreenAction WHERE ActionName = 'EditSystem')
	BEGIN
		INSERT INTO [dbo].[ScreenAction]
           ([ScreenId]
           ,[ActionName]
           ,[RecordStatus]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[ModifiedBy]
           ,[ModifiedDate])
		VALUES
		( 
		    @ScreenId,
			'EditSystem',	    
			@RecordStatus,	        
			@UserName,				
			@CurrentDate,			
			@UserName,				
			@CurrentDate			
		)
	END

SELECT  @ScreenId = ScreenId FROM Screen WHERE ScreenCode = 'VACL'
IF NOT EXISTS ( SELECT TOP 1 1 FROM ScreenAction WHERE ActionName = 'EditChecklist')
	BEGIN
		INSERT INTO [dbo].[ScreenAction]
           ([ScreenId]
           ,[ActionName]
           ,[RecordStatus]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[ModifiedBy]
           ,[ModifiedDate])
		VALUES
		( 
		    @ScreenId,
			'EditChecklist',	    
			@RecordStatus,	        
			@UserName,				
			@CurrentDate,			
			@UserName,				
			@CurrentDate			
		)
	END

SELECT @ScreenId = ScreenId FROM Screen WHERE ScreenCode = 'CCL'
IF NOT EXISTS (SELECT TOP 1 1 FROM ScreenAction WHERE ScreenId = @ScreenId AND ActionName = 'Reopen checklist')
BEGIN
	INSERT INTO [dbo].[ScreenAction]
        ([ScreenId]
        ,[ActionName]
        ,[RecordStatus]
        ,[CreatedBy]
        ,[CreatedDate]
        ,[ModifiedBy]
        ,[ModifiedDate])
	VALUES
	(
		@ScreenId,
		'Reopen checklist',
		@RecordStatus,
		@UserName,
		@CurrentDate,
		@UserName,
		@CurrentDate
	)
END

SELECT @ScreenId = ScreenId FROM Screen WHERE ScreenCode = 'VEK'
IF NOT EXISTS (SELECT TOP 1 1 FROM ScreenAction WHERE ScreenId = @ScreenId AND ActionName = 'Add KPI')
BEGIN
	INSERT INTO [dbo].[ScreenAction]
        ([ScreenId]
        ,[ActionName]
        ,[RecordStatus]
        ,[CreatedBy]
        ,[CreatedDate]
        ,[ModifiedBy]
        ,[ModifiedDate])
	VALUES
	(
		@ScreenId,
		'Add KPI',
		@RecordStatus,
		@UserName,
		@CurrentDate,
		@UserName,
		@CurrentDate
	)
END


SELECT @ScreenId = ScreenId FROM Screen WHERE ScreenCode = 'VEK'
IF NOT EXISTS (SELECT TOP 1 1 FROM ScreenAction WHERE ScreenId = @ScreenId AND ActionName = 'Edit KPI')
BEGIN
	INSERT INTO [dbo].[ScreenAction]
        ([ScreenId]
        ,[ActionName]
        ,[RecordStatus]
        ,[CreatedBy]
        ,[CreatedDate]
        ,[ModifiedBy]
        ,[ModifiedDate])
	VALUES
	(
		@ScreenId,
		'Edit KPI',
		@RecordStatus,
		@UserName,
		@CurrentDate,
		@UserName,
		@CurrentDate
	)
END

SELECT @ScreenId = ScreenId FROM Screen WHERE ScreenCode = 'DL'
IF NOT EXISTS (SELECT TOP 1 1 FROM ScreenAction WHERE ScreenId = @ScreenId AND ActionName = 'SaveLastEnteredBusinessDaysOrWeeks')
BEGIN
	INSERT INTO [dbo].[ScreenAction]
        ([ScreenId]
        ,[ActionName]
        ,[RecordStatus]
        ,[CreatedBy]
        ,[CreatedDate]
        ,[ModifiedBy]
        ,[ModifiedDate])
	VALUES
	(
		@ScreenId,
		'SaveLastEnteredBusinessDaysOrWeeks',
		@RecordStatus,
		@UserName,
		@CurrentDate,
		@UserName,
		@CurrentDate
	)
END

SELECT @ScreenId = ScreenId FROM Screen WHERE ScreenCode = 'DL'
IF NOT EXISTS (SELECT TOP 1 1 FROM ScreenAction WHERE ScreenId = @ScreenId AND ActionName = 'SaveLastEnteredBusinessDaysOrWeeks')
BEGIN
	INSERT INTO [dbo].[ScreenAction]
        ([ScreenId]
        ,[ActionName]
        ,[RecordStatus]
        ,[CreatedBy]
        ,[CreatedDate]
        ,[ModifiedBy]
        ,[ModifiedDate])
	VALUES
	(
		@ScreenId,
		'SaveLastEnteredBusinessDaysOrWeeks',
		@RecordStatus,
		@UserName,
		@CurrentDate,
		@UserName,
		@CurrentDate
	)
END

SELECT @ScreenId = ScreenId FROM Screen WHERE ScreenCode = 'CON'
IF NOT EXISTS (SELECT TOP 1 1 FROM ScreenAction WHERE ScreenId = @ScreenId AND ActionName = 'EditAdminConfig')
BEGIN
	INSERT INTO [dbo].[ScreenAction]
        ([ScreenId]
        ,[ActionName]
        ,[RecordStatus]
        ,[CreatedBy]
        ,[CreatedDate]
        ,[ModifiedBy]
        ,[ModifiedDate])
	VALUES
	(
		@ScreenId,
		'EditAdminConfig',
		@RecordStatus,
		@UserName,
		@CurrentDate,
		@UserName,
		@CurrentDate
	)
END

SELECT @ScreenId = ScreenId FROM Screen WHERE ScreenCode = 'VA'
IF NOT EXISTS (SELECT TOP 1 1 FROM ScreenAction WHERE ScreenId = @ScreenId AND ActionName = 'ViewActualM3Revenue')
BEGIN
	INSERT INTO [dbo].[ScreenAction]
        ([ScreenId]
        ,[ActionName]
        ,[RecordStatus]
        ,[CreatedBy]
        ,[CreatedDate]
        ,[ModifiedBy]
        ,[ModifiedDate])
	VALUES
	(
		@ScreenId,
		'ViewActualM3Revenue',
		@RecordStatus,
		@UserName,
		@CurrentDate,
		@UserName,
		@CurrentDate
	)
END

SELECT @ScreenId = ScreenId FROM Screen WHERE ScreenCode = 'VA'
IF NOT EXISTS (SELECT TOP 1 1 FROM ScreenAction WHERE ScreenId = @ScreenId AND ActionName = 'ViewForecastedM3Revenue')
BEGIN
	INSERT INTO [dbo].[ScreenAction]
        ([ScreenId]
        ,[ActionName]
        ,[RecordStatus]
        ,[CreatedBy]
        ,[CreatedDate]
        ,[ModifiedBy]
        ,[ModifiedDate])
	VALUES
	(
		@ScreenId,
		'ViewForecastedM3Revenue',
		@RecordStatus,
		@UserName,
		@CurrentDate,
		@UserName,
		@CurrentDate
	)
END

-- Inserting actions for to-do-list page.

SELECT @ScreenId = ScreenId FROM Screen WHERE ScreenCode = 'TDL'
IF NOT EXISTS (SELECT TOP 1 1 FROM ScreenAction WHERE ScreenId = @ScreenId AND ActionName = 'Assign Relationship Manager')
BEGIN
	INSERT INTO [dbo].[ScreenAction]
        ([ScreenId]
        ,[ActionName]
        ,[RecordStatus]
        ,[CreatedBy]
        ,[CreatedDate]
        ,[ModifiedBy]
        ,[ModifiedDate])
	VALUES
	(
		@ScreenId,
		'Assign Relationship Manager',
		@RecordStatus,
		@UserName,
		@CurrentDate,
		@UserName,
		@CurrentDate
	)
END

SELECT @ScreenId = ScreenId FROM Screen WHERE ScreenCode = 'TDL'
IF NOT EXISTS (SELECT TOP 1 1 FROM ScreenAction WHERE ScreenId = @ScreenId AND ActionName = 'Assign Billing Manager')
BEGIN
	INSERT INTO [dbo].[ScreenAction]
        ([ScreenId]
        ,[ActionName]
        ,[RecordStatus]
        ,[CreatedBy]
        ,[CreatedDate]
        ,[ModifiedBy]
        ,[ModifiedDate])
	VALUES
	(
		@ScreenId,
		'Assign Billing Manager',
		@RecordStatus,
		@UserName,
		@CurrentDate,
		@UserName,
		@CurrentDate
	)
END

SELECT @ScreenId = ScreenId FROM Screen WHERE ScreenCode = 'TDL'
IF NOT EXISTS (SELECT TOP 1 1 FROM ScreenAction WHERE ScreenId = @ScreenId AND ActionName = 'Pending Weekly Checklist')
BEGIN
	INSERT INTO [dbo].[ScreenAction]
        ([ScreenId]
        ,[ActionName]
        ,[RecordStatus]
        ,[CreatedBy]
        ,[CreatedDate]
        ,[ModifiedBy]
        ,[ModifiedDate])
	VALUES
	(
		@ScreenId,
		'Pending Weekly Checklist',
		@RecordStatus,
		@UserName,
		@CurrentDate,
		@UserName,
		@CurrentDate
	)
END

SELECT @ScreenId = ScreenId FROM Screen WHERE ScreenCode = 'TDL'
IF NOT EXISTS (SELECT TOP 1 1 FROM ScreenAction WHERE ScreenId = @ScreenId AND ActionName = 'Pending Monthly Checklist')
BEGIN
	INSERT INTO [dbo].[ScreenAction]
        ([ScreenId]
        ,[ActionName]
        ,[RecordStatus]
        ,[CreatedBy]
        ,[CreatedDate]
        ,[ModifiedBy]
        ,[ModifiedDate])
	VALUES
	(
		@ScreenId,
		'Pending Monthly Checklist',
		@RecordStatus,
		@UserName,
		@CurrentDate,
		@UserName,
		@CurrentDate
	)
END

SELECT @ScreenId = ScreenId FROM Screen WHERE ScreenCode = 'TDL'
IF NOT EXISTS (SELECT TOP 1 1 FROM ScreenAction WHERE ScreenId = @ScreenId AND ActionName = 'Partially Completed Clients')
BEGIN
	INSERT INTO [dbo].[ScreenAction]
        ([ScreenId]
        ,[ActionName]
        ,[RecordStatus]
        ,[CreatedBy]
        ,[CreatedDate]
        ,[ModifiedBy]
        ,[ModifiedDate])
	VALUES
	(
		@ScreenId,
		'Partially Completed Clients',
		@RecordStatus,
		@UserName,
		@CurrentDate,
		@UserName,
		@CurrentDate
	)
END

SELECT @ScreenId = ScreenId FROM Screen WHERE ScreenCode = 'TDL'
IF NOT EXISTS (SELECT TOP 1 1 FROM ScreenAction WHERE ScreenId = @ScreenId AND ActionName = 'Set Targets')
BEGIN
	INSERT INTO [dbo].[ScreenAction]
        ([ScreenId]
        ,[ActionName]
        ,[RecordStatus]
        ,[CreatedBy]
        ,[CreatedDate]
        ,[ModifiedBy]
        ,[ModifiedDate])
	VALUES
	(
		@ScreenId,
		'Set Targets',
		@RecordStatus,
		@UserName,
		@CurrentDate,
		@UserName,
		@CurrentDate
	)
END

SELECT @ScreenId = ScreenId FROM Screen WHERE ScreenCode = 'TDL'
IF NOT EXISTS (SELECT TOP 1 1 FROM ScreenAction WHERE ScreenId = @ScreenId AND ActionName = 'Set Holidays')
BEGIN
	INSERT INTO [dbo].[ScreenAction]
        ([ScreenId]
        ,[ActionName]
        ,[RecordStatus]
        ,[CreatedBy]
        ,[CreatedDate]
        ,[ModifiedBy]
        ,[ModifiedDate])
	VALUES
	(
		@ScreenId,
		'Set Holidays',
		@RecordStatus,
		@UserName,
		@CurrentDate,
		@UserName,
		@CurrentDate
	)
END

SELECT @ScreenId = ScreenId FROM Screen WHERE ScreenCode = 'TDL'
IF NOT EXISTS (SELECT TOP 1 1 FROM ScreenAction WHERE ScreenId = @ScreenId AND ActionName = 'Unclosed Months')
BEGIN
	INSERT INTO [dbo].[ScreenAction]
        ([ScreenId]
        ,[ActionName]
        ,[RecordStatus]
        ,[CreatedBy]
        ,[CreatedDate]
        ,[ModifiedBy]
        ,[ModifiedDate])
	VALUES
	(
		@ScreenId,
		'Unclosed Months',
		@RecordStatus,
		@UserName,
		@CurrentDate,
		@UserName,
		@CurrentDate
	)
END

SELECT @ScreenId = ScreenId FROM Screen WHERE ScreenCode = 'TDL'
IF NOT EXISTS (SELECT TOP 1 1 FROM ScreenAction WHERE ScreenId = @ScreenId AND ActionName = 'Incomplete Deposit Log')
BEGIN
	INSERT INTO [dbo].[ScreenAction]
        ([ScreenId]
        ,[ActionName]
        ,[RecordStatus]
        ,[CreatedBy]
        ,[CreatedDate]
        ,[ModifiedBy]
        ,[ModifiedDate])
	VALUES
	(
		@ScreenId,
		'Incomplete Deposit Log',
		@RecordStatus,
		@UserName,
		@CurrentDate,
		@UserName,
		@CurrentDate
	)
END

SELECT @ScreenId = ScreenId FROM Screen WHERE ScreenCode = 'VA'
IF NOT EXISTS (SELECT TOP 1 1 FROM ScreenAction WHERE ScreenId = @ScreenId AND ActionName = 'ViewClientHistory')
BEGIN
	INSERT INTO [dbo].[ScreenAction]
        ([ScreenId]
        ,[ActionName]
        ,[RecordStatus]
        ,[CreatedBy]
        ,[CreatedDate]
        ,[ModifiedBy]
        ,[ModifiedDate])
	VALUES
	(
		@ScreenId,
		'ViewClientHistory',
		@RecordStatus,
		@UserName,
		@CurrentDate,
		@UserName,
		@CurrentDate
	)
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

/*
Script Name : 13_Screen
Module_Name : M3Pact
Created By  : Abhishek K
CreatedDate : 05/11/2018

Data Verification Script :

SELECT * FROM [RoleAction](NOLOCK) WHERE RecordStatus = 'A'


Revision History 
=============
Ver  Date				Who						Comment
1.0  30-July-2018		Abhishek Kovvuri		Inserting view M3 actual and forecasted actions for admin and executive.
1.1  08-Aug-2018        Abhishek Kovvuri        Inserting role based actions for to-do-list page.
*/


BEGIN TRY
BEGIN TRANSACTION

DECLARE @UserName NVARCHAR(60) = 'Admin'
DECLARE @CurrentDate DATETIME = GETDATE()
DECLARE @RecordStatus CHAR(1) = 'A'
DECLARE @RoleId INT;
DECLARE @ScreenActionId INT;

SELECT  @ScreenActionId = ScreenActionId FROM ScreenAction WHERE ActionName = 'ViewCloseMonthButton'
SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'Admin'
IF NOT EXISTS ( SELECT TOP 1 1 FROM RoleAction WHERE RoleId = @RoleId AND ScreenActionId = @ScreenActionId)
	BEGIN
		INSERT INTO [dbo].[RoleAction]
           ([RoleId]
           ,[ScreenActionId]
           ,[RecordStatus]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[ModifiedBy]
           ,[ModifiedDate])
		VALUES
		(
			@RoleId, 			
			@ScreenActionId,
			@RecordStatus,	
			@UserName,				
			@CurrentDate,	
			@UserName,				
			@CurrentDate
		)
	END

SELECT  @ScreenActionId = ScreenActionId FROM ScreenAction WHERE ActionName = 'ViewCloseMonthButton'
SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'Manager'
IF NOT EXISTS ( SELECT TOP 1 1 FROM RoleAction WHERE RoleId = @RoleId AND ScreenActionId = @ScreenActionId)
	BEGIN
		INSERT INTO [dbo].[RoleAction]
           ([RoleId]
           ,[ScreenActionId]
           ,[RecordStatus]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[ModifiedBy]
           ,[ModifiedDate])
		VALUES
		(
			@RoleId, 			
			@ScreenActionId,
			@RecordStatus,	
			@UserName,				
			@CurrentDate,	
			@UserName,				
			@CurrentDate
		)
	END

SELECT  @ScreenActionId = ScreenActionId FROM ScreenAction WHERE ActionName = 'ViewCloseMonthButton'
SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'User'
IF NOT EXISTS ( SELECT TOP 1 1 FROM RoleAction WHERE RoleId = @RoleId AND ScreenActionId = @ScreenActionId)
	BEGIN
		INSERT INTO [dbo].[RoleAction]
           ([RoleId]
           ,[ScreenActionId]
           ,[RecordStatus]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[ModifiedBy]
           ,[ModifiedDate])
		VALUES
		(
			@RoleId, 			
			@ScreenActionId,
			@RecordStatus,	
			@UserName,				
			@CurrentDate,	
			@UserName,				
			@CurrentDate
		)
	END

SELECT  @ScreenActionId = ScreenActionId FROM ScreenAction WHERE ActionName = 'ViewAddDepositSection'
SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'Admin'
IF NOT EXISTS ( SELECT TOP 1 1 FROM RoleAction WHERE RoleId = @RoleId AND ScreenActionId = @ScreenActionId)
	BEGIN
		INSERT INTO [dbo].[RoleAction]
           ([RoleId]
           ,[ScreenActionId]
           ,[RecordStatus]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[ModifiedBy]
           ,[ModifiedDate])
		VALUES
		(
			@RoleId, 			
			@ScreenActionId,
			@RecordStatus,	
			@UserName,				
			@CurrentDate,	
			@UserName,				
			@CurrentDate
		)
	END

SELECT  @ScreenActionId = ScreenActionId FROM ScreenAction WHERE ActionName = 'ViewAddDepositSection'
SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'Manager'
IF NOT EXISTS ( SELECT TOP 1 1 FROM RoleAction WHERE RoleId = @RoleId AND ScreenActionId = @ScreenActionId)
	BEGIN
		INSERT INTO [dbo].[RoleAction]
           ([RoleId]
           ,[ScreenActionId]
           ,[RecordStatus]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[ModifiedBy]
           ,[ModifiedDate])
		VALUES
		(
			@RoleId, 			
			@ScreenActionId,
			@RecordStatus,	
			@UserName,				
			@CurrentDate,	
			@UserName,				
			@CurrentDate
		)
	END

SELECT  @ScreenActionId = ScreenActionId FROM ScreenAction WHERE ActionName = 'ViewAddDepositSection'
SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'User'
IF NOT EXISTS ( SELECT TOP 1 1 FROM RoleAction WHERE RoleId = @RoleId AND ScreenActionId = @ScreenActionId)
	BEGIN
		INSERT INTO [dbo].[RoleAction]
           ([RoleId]
           ,[ScreenActionId]
           ,[RecordStatus]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[ModifiedBy]
           ,[ModifiedDate])
		VALUES
		(
			@RoleId, 			
			@ScreenActionId,
			@RecordStatus,	
			@UserName,				
			@CurrentDate,	
			@UserName,				
			@CurrentDate
		)
	END

SELECT  @ScreenActionId = ScreenActionId FROM ScreenAction WHERE ActionName = 'OpenClosedMonth'
SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'Admin'
IF NOT EXISTS ( SELECT TOP 1 1 FROM RoleAction WHERE RoleId = @RoleId AND ScreenActionId = @ScreenActionId)
	BEGIN
		INSERT INTO [dbo].[RoleAction]
           ([RoleId]
           ,[ScreenActionId]
           ,[RecordStatus]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[ModifiedBy]
           ,[ModifiedDate])
		VALUES
		(
			@RoleId, 			
			@ScreenActionId,
			@RecordStatus,	
			@UserName,				
			@CurrentDate,	
			@UserName,				
			@CurrentDate
		)
	END

SELECT  @ScreenActionId = ScreenActionId FROM ScreenAction WHERE ActionName = 'AddSite'
SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'Admin'
IF NOT EXISTS ( SELECT TOP 1 1 FROM RoleAction WHERE RoleId = @RoleId AND ScreenActionId = @ScreenActionId)
	BEGIN
		INSERT INTO [dbo].[RoleAction]
           ([RoleId]
           ,[ScreenActionId]
           ,[RecordStatus]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[ModifiedBy]
           ,[ModifiedDate])
		VALUES
		(
			@RoleId, 			
			@ScreenActionId,
			@RecordStatus,	
			@UserName,				
			@CurrentDate,	
			@UserName,				
			@CurrentDate
		)
	END

SELECT  @ScreenActionId = ScreenActionId FROM ScreenAction WHERE ActionName = 'EditSite'
SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'Admin'
IF NOT EXISTS ( SELECT TOP 1 1 FROM RoleAction WHERE RoleId = @RoleId AND ScreenActionId = @ScreenActionId)
	BEGIN
		INSERT INTO [dbo].[RoleAction]
           ([RoleId]
           ,[ScreenActionId]
           ,[RecordStatus]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[ModifiedBy]
           ,[ModifiedDate])
		VALUES
		(
			@RoleId, 			
			@ScreenActionId,
			@RecordStatus,	
			@UserName,				
			@CurrentDate,	
			@UserName,				
			@CurrentDate
		)
	END

SELECT  @ScreenActionId = ScreenActionId FROM ScreenAction WHERE ActionName = 'AddSpecialty'
SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'Admin'
IF NOT EXISTS ( SELECT TOP 1 1 FROM RoleAction WHERE RoleId = @RoleId AND ScreenActionId = @ScreenActionId)
	BEGIN
		INSERT INTO [dbo].[RoleAction]
           ([RoleId]
           ,[ScreenActionId]
           ,[RecordStatus]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[ModifiedBy]
           ,[ModifiedDate])
		VALUES
		(
			@RoleId, 			
			@ScreenActionId,
			@RecordStatus,	
			@UserName,				
			@CurrentDate,	
			@UserName,				
			@CurrentDate
		)
	END

SELECT  @ScreenActionId = ScreenActionId FROM ScreenAction WHERE ActionName = 'AddSpecialty'
SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'Executive'
IF NOT EXISTS ( SELECT TOP 1 1 FROM RoleAction WHERE RoleId = @RoleId AND ScreenActionId = @ScreenActionId)
	BEGIN
		INSERT INTO [dbo].[RoleAction]
           ([RoleId]
           ,[ScreenActionId]
           ,[RecordStatus]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[ModifiedBy]
           ,[ModifiedDate])
		VALUES
		(
			@RoleId, 			
			@ScreenActionId,
			@RecordStatus,	
			@UserName,				
			@CurrentDate,	
			@UserName,				
			@CurrentDate
		)
	END

SELECT  @ScreenActionId = ScreenActionId FROM ScreenAction WHERE ActionName = 'EditSpecialty'
SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'Admin'
IF NOT EXISTS ( SELECT TOP 1 1 FROM RoleAction WHERE RoleId = @RoleId AND ScreenActionId = @ScreenActionId)
	BEGIN
		INSERT INTO [dbo].[RoleAction]
           ([RoleId]
           ,[ScreenActionId]
           ,[RecordStatus]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[ModifiedBy]
           ,[ModifiedDate])
		VALUES
		(
			@RoleId, 			
			@ScreenActionId,
			@RecordStatus,	
			@UserName,				
			@CurrentDate,	
			@UserName,				
			@CurrentDate
		)
	END

SELECT  @ScreenActionId = ScreenActionId FROM ScreenAction WHERE ActionName = 'EditSpecialty'
SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'Executive'
IF NOT EXISTS ( SELECT TOP 1 1 FROM RoleAction WHERE RoleId = @RoleId AND ScreenActionId = @ScreenActionId)
	BEGIN
		INSERT INTO [dbo].[RoleAction]
           ([RoleId]
           ,[ScreenActionId]
           ,[RecordStatus]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[ModifiedBy]
           ,[ModifiedDate])
		VALUES
		(
			@RoleId, 			
			@ScreenActionId,
			@RecordStatus,	
			@UserName,				
			@CurrentDate,	
			@UserName,				
			@CurrentDate
		)
	END

SELECT  @ScreenActionId = ScreenActionId FROM ScreenAction WHERE ActionName = 'PerformClientActivateInactivate'
SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'Admin'
IF NOT EXISTS ( SELECT TOP 1 1 FROM RoleAction WHERE RoleId = @RoleId AND ScreenActionId = @ScreenActionId)
	BEGIN
		INSERT INTO [dbo].[RoleAction]
           ([RoleId]
           ,[ScreenActionId]
           ,[RecordStatus]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[ModifiedBy]
           ,[ModifiedDate])
		VALUES
		(
			@RoleId, 			
			@ScreenActionId,
			@RecordStatus,	
			@UserName,				
			@CurrentDate,	
			@UserName,				
			@CurrentDate
		)
	END

SELECT  @ScreenActionId = ScreenActionId FROM ScreenAction WHERE ActionName = 'EditClientData'
SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'Admin'
IF NOT EXISTS ( SELECT TOP 1 1 FROM RoleAction WHERE RoleId = @RoleId AND ScreenActionId = @ScreenActionId)
	BEGIN
		INSERT INTO [dbo].[RoleAction]
           ([RoleId]
           ,[ScreenActionId]
           ,[RecordStatus]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[ModifiedBy]
           ,[ModifiedDate])
		VALUES
		(
			@RoleId, 			
			@ScreenActionId,
			@RecordStatus,	
			@UserName,				
			@CurrentDate,	
			@UserName,				
			@CurrentDate
		)
	END

SELECT  @ScreenActionId = ScreenActionId FROM ScreenAction WHERE ActionName = 'EditClientData'
SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'Manager'
IF NOT EXISTS ( SELECT TOP 1 1 FROM RoleAction WHERE RoleId = @RoleId AND ScreenActionId = @ScreenActionId)
	BEGIN
		INSERT INTO [dbo].[RoleAction]
           ([RoleId]
           ,[ScreenActionId]
           ,[RecordStatus]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[ModifiedBy]
           ,[ModifiedDate])
		VALUES
		(
			@RoleId, 			
			@ScreenActionId,
			@RecordStatus,	
			@UserName,				
			@CurrentDate,	
			@UserName,				
			@CurrentDate
		)
	END

SELECT  @ScreenActionId = ScreenActionId FROM ScreenAction WHERE ActionName = 'EditClientData'
SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'Executive'
IF NOT EXISTS ( SELECT TOP 1 1 FROM RoleAction WHERE RoleId = @RoleId AND ScreenActionId = @ScreenActionId)
	BEGIN
		INSERT INTO [dbo].[RoleAction]
           ([RoleId]
           ,[ScreenActionId]
           ,[RecordStatus]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[ModifiedBy]
           ,[ModifiedDate])
		VALUES
		(
			@RoleId, 			
			@ScreenActionId,
			@RecordStatus,	
			@UserName,				
			@CurrentDate,	
			@UserName,				
			@CurrentDate
		)
	END

SELECT  @ScreenActionId = ScreenActionId FROM ScreenAction WHERE ActionName = 'AddClientData'
SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'Admin'
IF NOT EXISTS ( SELECT TOP 1 1 FROM RoleAction WHERE RoleId = @RoleId AND ScreenActionId = @ScreenActionId)
	BEGIN
		INSERT INTO [dbo].[RoleAction]
           ([RoleId]
           ,[ScreenActionId]
           ,[RecordStatus]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[ModifiedBy]
           ,[ModifiedDate])
		VALUES
		(
			@RoleId, 			
			@ScreenActionId,
			@RecordStatus,	
			@UserName,				
			@CurrentDate,	
			@UserName,				
			@CurrentDate
		)
	END

SELECT  @ScreenActionId = ScreenActionId FROM ScreenAction WHERE ActionName = 'AddPayerAssociationToClient'
SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'Admin'
IF NOT EXISTS ( SELECT TOP 1 1 FROM RoleAction WHERE RoleId = @RoleId AND ScreenActionId = @ScreenActionId)
	BEGIN
		INSERT INTO [dbo].[RoleAction]
           ([RoleId]
           ,[ScreenActionId]
           ,[RecordStatus]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[ModifiedBy]
           ,[ModifiedDate])
		VALUES
		(
			@RoleId, 			
			@ScreenActionId,
			@RecordStatus,	
			@UserName,				
			@CurrentDate,	
			@UserName,				
			@CurrentDate
		)
	END

SELECT  @ScreenActionId = ScreenActionId FROM ScreenAction WHERE ActionName = 'AddPayerAssociationToClient'
SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'Manager'
IF NOT EXISTS ( SELECT TOP 1 1 FROM RoleAction WHERE RoleId = @RoleId AND ScreenActionId = @ScreenActionId)
	BEGIN
		INSERT INTO [dbo].[RoleAction]
           ([RoleId]
           ,[ScreenActionId]
           ,[RecordStatus]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[ModifiedBy]
           ,[ModifiedDate])
		VALUES
		(
			@RoleId, 			
			@ScreenActionId,
			@RecordStatus,	
			@UserName,				
			@CurrentDate,	
			@UserName,				
			@CurrentDate
		)
	END

SELECT  @ScreenActionId = ScreenActionId FROM ScreenAction WHERE ActionName = 'AddPayerAssociationToClient'
SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'Executive'
IF NOT EXISTS ( SELECT TOP 1 1 FROM RoleAction WHERE RoleId = @RoleId AND ScreenActionId = @ScreenActionId)
	BEGIN
		INSERT INTO [dbo].[RoleAction]
           ([RoleId]
           ,[ScreenActionId]
           ,[RecordStatus]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[ModifiedBy]
           ,[ModifiedDate])
		VALUES
		(
			@RoleId, 			
			@ScreenActionId,
			@RecordStatus,	
			@UserName,				
			@CurrentDate,	
			@UserName,				
			@CurrentDate
		)
	END

SELECT  @ScreenActionId = ScreenActionId FROM ScreenAction WHERE ActionName = 'AddPayerAssociationToClient'
SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'User'
IF NOT EXISTS ( SELECT TOP 1 1 FROM RoleAction WHERE RoleId = @RoleId AND ScreenActionId = @ScreenActionId)
	BEGIN
		INSERT INTO [dbo].[RoleAction]
           ([RoleId]
           ,[ScreenActionId]
           ,[RecordStatus]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[ModifiedBy]
           ,[ModifiedDate])
		VALUES
		(
			@RoleId, 			
			@ScreenActionId,
			@RecordStatus,	
			@UserName,				
			@CurrentDate,	
			@UserName,				
			@CurrentDate
		)
	END

SELECT  @ScreenActionId = ScreenActionId FROM ScreenAction WHERE ActionName = 'EditPayerAssociationToClient'
SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'Admin'
IF NOT EXISTS ( SELECT TOP 1 1 FROM RoleAction WHERE RoleId = @RoleId AND ScreenActionId = @ScreenActionId)
	BEGIN
		INSERT INTO [dbo].[RoleAction]
           ([RoleId]
           ,[ScreenActionId]
           ,[RecordStatus]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[ModifiedBy]
           ,[ModifiedDate])
		VALUES
		(
			@RoleId, 			
			@ScreenActionId,
			@RecordStatus,	
			@UserName,				
			@CurrentDate,	
			@UserName,				
			@CurrentDate
		)
	END

SELECT  @ScreenActionId = ScreenActionId FROM ScreenAction WHERE ActionName = 'EditPayerAssociationToClient'
SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'Manager'
IF NOT EXISTS ( SELECT TOP 1 1 FROM RoleAction WHERE RoleId = @RoleId AND ScreenActionId = @ScreenActionId)
	BEGIN
		INSERT INTO [dbo].[RoleAction]
           ([RoleId]
           ,[ScreenActionId]
           ,[RecordStatus]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[ModifiedBy]
           ,[ModifiedDate])
		VALUES
		(
			@RoleId, 			
			@ScreenActionId,
			@RecordStatus,	
			@UserName,				
			@CurrentDate,	
			@UserName,				
			@CurrentDate
		)
	END

SELECT  @ScreenActionId = ScreenActionId FROM ScreenAction WHERE ActionName = 'EditPayerAssociationToClient'
SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'Executive'
IF NOT EXISTS ( SELECT TOP 1 1 FROM RoleAction WHERE RoleId = @RoleId AND ScreenActionId = @ScreenActionId)
	BEGIN
		INSERT INTO [dbo].[RoleAction]
           ([RoleId]
           ,[ScreenActionId]
           ,[RecordStatus]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[ModifiedBy]
           ,[ModifiedDate])
		VALUES
		(
			@RoleId, 			
			@ScreenActionId,
			@RecordStatus,	
			@UserName,				
			@CurrentDate,	
			@UserName,				
			@CurrentDate
		)
	END

SELECT  @ScreenActionId = ScreenActionId FROM ScreenAction WHERE ActionName = 'EditPayerAssociationToClient'
SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'User'
IF NOT EXISTS ( SELECT TOP 1 1 FROM RoleAction WHERE RoleId = @RoleId AND ScreenActionId = @ScreenActionId)
	BEGIN
		INSERT INTO [dbo].[RoleAction]
           ([RoleId]
           ,[ScreenActionId]
           ,[RecordStatus]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[ModifiedBy]
           ,[ModifiedDate])
		VALUES
		(
			@RoleId, 			
			@ScreenActionId,
			@RecordStatus,	
			@UserName,				
			@CurrentDate,	
			@UserName,				
			@CurrentDate
		)
	END

SELECT  @ScreenActionId = ScreenActionId FROM ScreenAction WHERE ActionName = 'PerformClientPayerActivateInactivate'
SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'Admin'
IF NOT EXISTS ( SELECT TOP 1 1 FROM RoleAction WHERE RoleId = @RoleId AND ScreenActionId = @ScreenActionId)
	BEGIN
		INSERT INTO [dbo].[RoleAction]
           ([RoleId]
           ,[ScreenActionId]
           ,[RecordStatus]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[ModifiedBy]
           ,[ModifiedDate])
		VALUES
		(
			@RoleId, 			
			@ScreenActionId,
			@RecordStatus,	
			@UserName,				
			@CurrentDate,	
			@UserName,				
			@CurrentDate
		)
	END

SELECT  @ScreenActionId = ScreenActionId FROM ScreenAction WHERE ActionName = 'PerformClientPayerActivateInactivate'
SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'Manager'
IF NOT EXISTS ( SELECT TOP 1 1 FROM RoleAction WHERE RoleId = @RoleId AND ScreenActionId = @ScreenActionId)
	BEGIN
		INSERT INTO [dbo].[RoleAction]
           ([RoleId]
           ,[ScreenActionId]
           ,[RecordStatus]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[ModifiedBy]
           ,[ModifiedDate])
		VALUES
		(
			@RoleId, 			
			@ScreenActionId,
			@RecordStatus,	
			@UserName,				
			@CurrentDate,	
			@UserName,				
			@CurrentDate
		)
	END

SELECT  @ScreenActionId = ScreenActionId FROM ScreenAction WHERE ActionName = 'PerformClientPayerActivateInactivate'
SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'Executive'
IF NOT EXISTS ( SELECT TOP 1 1 FROM RoleAction WHERE RoleId = @RoleId AND ScreenActionId = @ScreenActionId)
	BEGIN
		INSERT INTO [dbo].[RoleAction]
           ([RoleId]
           ,[ScreenActionId]
           ,[RecordStatus]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[ModifiedBy]
           ,[ModifiedDate])
		VALUES
		(
			@RoleId, 			
			@ScreenActionId,
			@RecordStatus,	
			@UserName,				
			@CurrentDate,	
			@UserName,				
			@CurrentDate
		)
	END

SELECT  @ScreenActionId = ScreenActionId FROM ScreenAction WHERE ActionName = 'PerformClientPayerActivateInactivate'
SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'User'
IF NOT EXISTS ( SELECT TOP 1 1 FROM RoleAction WHERE RoleId = @RoleId AND ScreenActionId = @ScreenActionId)
	BEGIN
		INSERT INTO [dbo].[RoleAction]
           ([RoleId]
           ,[ScreenActionId]
           ,[RecordStatus]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[ModifiedBy]
           ,[ModifiedDate])
		VALUES
		(
			@RoleId, 			
			@ScreenActionId,
			@RecordStatus,	
			@UserName,				
			@CurrentDate,	
			@UserName,				
			@CurrentDate
		)
	END

SELECT  @ScreenActionId = ScreenActionId FROM ScreenAction WHERE ActionName = 'AddUserAssociationToClient'
SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'Admin'
IF NOT EXISTS ( SELECT TOP 1 1 FROM RoleAction WHERE RoleId = @RoleId AND ScreenActionId = @ScreenActionId)
	BEGIN
		INSERT INTO [dbo].[RoleAction]
           ([RoleId]
           ,[ScreenActionId]
           ,[RecordStatus]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[ModifiedBy]
           ,[ModifiedDate])
		VALUES
		(
			@RoleId, 			
			@ScreenActionId,
			@RecordStatus,	
			@UserName,				
			@CurrentDate,	
			@UserName,				
			@CurrentDate
		)
	END

SELECT  @ScreenActionId = ScreenActionId FROM ScreenAction WHERE ActionName = 'AddUserAssociationToClient'
SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'Manager'
IF NOT EXISTS ( SELECT TOP 1 1 FROM RoleAction WHERE RoleId = @RoleId AND ScreenActionId = @ScreenActionId)
	BEGIN
		INSERT INTO [dbo].[RoleAction]
           ([RoleId]
           ,[ScreenActionId]
           ,[RecordStatus]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[ModifiedBy]
           ,[ModifiedDate])
		VALUES
		(
			@RoleId, 			
			@ScreenActionId,
			@RecordStatus,	
			@UserName,				
			@CurrentDate,	
			@UserName,				
			@CurrentDate
		)
	END

SELECT  @ScreenActionId = ScreenActionId FROM ScreenAction WHERE ActionName = 'AddUserAssociationToClient'
SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'Executive'
IF NOT EXISTS ( SELECT TOP 1 1 FROM RoleAction WHERE RoleId = @RoleId AND ScreenActionId = @ScreenActionId)
	BEGIN
		INSERT INTO [dbo].[RoleAction]
           ([RoleId]
           ,[ScreenActionId]
           ,[RecordStatus]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[ModifiedBy]
           ,[ModifiedDate])
		VALUES
		(
			@RoleId, 			
			@ScreenActionId,
			@RecordStatus,	
			@UserName,				
			@CurrentDate,	
			@UserName,				
			@CurrentDate
		)
	END

SELECT  @ScreenActionId = ScreenActionId FROM ScreenAction WHERE ActionName = 'EditUserAssociationToClient'
SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'Admin'
IF NOT EXISTS ( SELECT TOP 1 1 FROM RoleAction WHERE RoleId = @RoleId AND ScreenActionId = @ScreenActionId)
	BEGIN
		INSERT INTO [dbo].[RoleAction]
           ([RoleId]
           ,[ScreenActionId]
           ,[RecordStatus]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[ModifiedBy]
           ,[ModifiedDate])
		VALUES
		(
			@RoleId, 			
			@ScreenActionId,
			@RecordStatus,	
			@UserName,				
			@CurrentDate,	
			@UserName,				
			@CurrentDate
		)
	END

SELECT  @ScreenActionId = ScreenActionId FROM ScreenAction WHERE ActionName = 'EditUserAssociationToClient'
SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'Manager'
IF NOT EXISTS ( SELECT TOP 1 1 FROM RoleAction WHERE RoleId = @RoleId AND ScreenActionId = @ScreenActionId)
	BEGIN
		INSERT INTO [dbo].[RoleAction]
           ([RoleId]
           ,[ScreenActionId]
           ,[RecordStatus]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[ModifiedBy]
           ,[ModifiedDate])
		VALUES
		(
			@RoleId, 			
			@ScreenActionId,
			@RecordStatus,	
			@UserName,				
			@CurrentDate,	
			@UserName,				
			@CurrentDate
		)
	END

SELECT  @ScreenActionId = ScreenActionId FROM ScreenAction WHERE ActionName = 'EditUserAssociationToClient'
SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'Executive'
IF NOT EXISTS ( SELECT TOP 1 1 FROM RoleAction WHERE RoleId = @RoleId AND ScreenActionId = @ScreenActionId)
	BEGIN
		INSERT INTO [dbo].[RoleAction]
           ([RoleId]
           ,[ScreenActionId]
           ,[RecordStatus]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[ModifiedBy]
           ,[ModifiedDate])
		VALUES
		(
			@RoleId, 			
			@ScreenActionId,
			@RecordStatus,	
			@UserName,				
			@CurrentDate,	
			@UserName,				
			@CurrentDate
		)
	END

SELECT  @ScreenActionId = ScreenActionId FROM ScreenAction WHERE ActionName = 'AddMonthlyTargets'
SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'Admin'
IF NOT EXISTS ( SELECT TOP 1 1 FROM RoleAction WHERE RoleId = @RoleId AND ScreenActionId = @ScreenActionId)
	BEGIN
		INSERT INTO [dbo].[RoleAction]
           ([RoleId]
           ,[ScreenActionId]
           ,[RecordStatus]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[ModifiedBy]
           ,[ModifiedDate])
		VALUES
		(
			@RoleId, 			
			@ScreenActionId,
			@RecordStatus,	
			@UserName,				
			@CurrentDate,	
			@UserName,				
			@CurrentDate
		)
	END

SELECT  @ScreenActionId = ScreenActionId FROM ScreenAction WHERE ActionName = 'AddMonthlyTargets'
SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'Manager'
IF NOT EXISTS ( SELECT TOP 1 1 FROM RoleAction WHERE RoleId = @RoleId AND ScreenActionId = @ScreenActionId)
	BEGIN
		INSERT INTO [dbo].[RoleAction]
           ([RoleId]
           ,[ScreenActionId]
           ,[RecordStatus]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[ModifiedBy]
           ,[ModifiedDate])
		VALUES
		(
			@RoleId, 			
			@ScreenActionId,
			@RecordStatus,	
			@UserName,				
			@CurrentDate,	
			@UserName,				
			@CurrentDate
		)
	END

SELECT  @ScreenActionId = ScreenActionId FROM ScreenAction WHERE ActionName = 'AddMonthlyTargets'
SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'Executive'
IF NOT EXISTS ( SELECT TOP 1 1 FROM RoleAction WHERE RoleId = @RoleId AND ScreenActionId = @ScreenActionId)
	BEGIN
		INSERT INTO [dbo].[RoleAction]
           ([RoleId]
           ,[ScreenActionId]
           ,[RecordStatus]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[ModifiedBy]
           ,[ModifiedDate])
		VALUES
		(
			@RoleId, 			
			@ScreenActionId,
			@RecordStatus,	
			@UserName,				
			@CurrentDate,	
			@UserName,				
			@CurrentDate
		)
	END

SELECT  @ScreenActionId = ScreenActionId FROM ScreenAction WHERE ActionName = 'EditMonthlyTargets'
SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'Admin'
IF NOT EXISTS ( SELECT TOP 1 1 FROM RoleAction WHERE RoleId = @RoleId AND ScreenActionId = @ScreenActionId)
	BEGIN
		INSERT INTO [dbo].[RoleAction]
           ([RoleId]
           ,[ScreenActionId]
           ,[RecordStatus]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[ModifiedBy]
           ,[ModifiedDate])
		VALUES
		(
			@RoleId, 			
			@ScreenActionId,
			@RecordStatus,	
			@UserName,				
			@CurrentDate,	
			@UserName,				
			@CurrentDate
		)
	END

SELECT  @ScreenActionId = ScreenActionId FROM ScreenAction WHERE ActionName = 'EditMonthlyTargets'
SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'Manager'
IF NOT EXISTS ( SELECT TOP 1 1 FROM RoleAction WHERE RoleId = @RoleId AND ScreenActionId = @ScreenActionId)
	BEGIN
		INSERT INTO [dbo].[RoleAction]
           ([RoleId]
           ,[ScreenActionId]
           ,[RecordStatus]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[ModifiedBy]
           ,[ModifiedDate])
		VALUES
		(
			@RoleId, 			
			@ScreenActionId,
			@RecordStatus,	
			@UserName,				
			@CurrentDate,	
			@UserName,				
			@CurrentDate
		)
	END

SELECT  @ScreenActionId = ScreenActionId FROM ScreenAction WHERE ActionName = 'EditMonthlyTargets'
SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'Executive'
IF NOT EXISTS ( SELECT TOP 1 1 FROM RoleAction WHERE RoleId = @RoleId AND ScreenActionId = @ScreenActionId)
	BEGIN
		INSERT INTO [dbo].[RoleAction]
           ([RoleId]
           ,[ScreenActionId]
           ,[RecordStatus]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[ModifiedBy]
           ,[ModifiedDate])
		VALUES
		(
			@RoleId, 			
			@ScreenActionId,
			@RecordStatus,	
			@UserName,				
			@CurrentDate,	
			@UserName,				
			@CurrentDate
		)
	END

SELECT  @ScreenActionId = ScreenActionId FROM ScreenAction WHERE ActionName = 'AddKPI'
SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'Admin'
IF NOT EXISTS ( SELECT TOP 1 1 FROM RoleAction WHERE RoleId = @RoleId AND ScreenActionId = @ScreenActionId)
	BEGIN
		INSERT INTO [dbo].[RoleAction]
           ([RoleId]
           ,[ScreenActionId]
           ,[RecordStatus]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[ModifiedBy]
           ,[ModifiedDate])
		VALUES
		(
			@RoleId, 			
			@ScreenActionId,
			@RecordStatus,	
			@UserName,				
			@CurrentDate,	
			@UserName,				
			@CurrentDate
		)
	END

SELECT  @ScreenActionId = ScreenActionId FROM ScreenAction WHERE ActionName = 'AddKPI'
SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'Manager'
IF NOT EXISTS ( SELECT TOP 1 1 FROM RoleAction WHERE RoleId = @RoleId AND ScreenActionId = @ScreenActionId)
	BEGIN
		INSERT INTO [dbo].[RoleAction]
           ([RoleId]
           ,[ScreenActionId]
           ,[RecordStatus]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[ModifiedBy]
           ,[ModifiedDate])
		VALUES
		(
			@RoleId, 			
			@ScreenActionId,
			@RecordStatus,	
			@UserName,				
			@CurrentDate,	
			@UserName,				
			@CurrentDate
		)
	END

SELECT  @ScreenActionId = ScreenActionId FROM ScreenAction WHERE ActionName = 'AddKPI'
SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'Executive'
IF NOT EXISTS ( SELECT TOP 1 1 FROM RoleAction WHERE RoleId = @RoleId AND ScreenActionId = @ScreenActionId)
	BEGIN
		INSERT INTO [dbo].[RoleAction]
           ([RoleId]
           ,[ScreenActionId]
           ,[RecordStatus]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[ModifiedBy]
           ,[ModifiedDate])
		VALUES
		(
			@RoleId, 			
			@ScreenActionId,
			@RecordStatus,	
			@UserName,				
			@CurrentDate,	
			@UserName,				
			@CurrentDate
		)
	END

SELECT  @ScreenActionId = ScreenActionId FROM ScreenAction WHERE ActionName = 'EditKPI'
SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'Admin'
IF NOT EXISTS ( SELECT TOP 1 1 FROM RoleAction WHERE RoleId = @RoleId AND ScreenActionId = @ScreenActionId)
	BEGIN
		INSERT INTO [dbo].[RoleAction]
           ([RoleId]
           ,[ScreenActionId]
           ,[RecordStatus]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[ModifiedBy]
           ,[ModifiedDate])
		VALUES
		(
			@RoleId, 			
			@ScreenActionId,
			@RecordStatus,	
			@UserName,				
			@CurrentDate,	
			@UserName,				
			@CurrentDate
		)
	END

SELECT  @ScreenActionId = ScreenActionId FROM ScreenAction WHERE ActionName = 'EditKPI'
SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'Manager'
IF NOT EXISTS ( SELECT TOP 1 1 FROM RoleAction WHERE RoleId = @RoleId AND ScreenActionId = @ScreenActionId)
	BEGIN
		INSERT INTO [dbo].[RoleAction]
           ([RoleId]
           ,[ScreenActionId]
           ,[RecordStatus]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[ModifiedBy]
           ,[ModifiedDate])
		VALUES
		(
			@RoleId, 			
			@ScreenActionId,
			@RecordStatus,	
			@UserName,				
			@CurrentDate,	
			@UserName,				
			@CurrentDate
		)
	END

SELECT  @ScreenActionId = ScreenActionId FROM ScreenAction WHERE ActionName = 'EditKPI'
SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'Executive'
IF NOT EXISTS ( SELECT TOP 1 1 FROM RoleAction WHERE RoleId = @RoleId AND ScreenActionId = @ScreenActionId)
	BEGIN
		INSERT INTO [dbo].[RoleAction]
           ([RoleId]
           ,[ScreenActionId]
           ,[RecordStatus]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[ModifiedBy]
           ,[ModifiedDate])
		VALUES
		(
			@RoleId, 			
			@ScreenActionId,
			@RecordStatus,	
			@UserName,				
			@CurrentDate,	
			@UserName,				
			@CurrentDate
		)
	END

SELECT  @ScreenActionId = ScreenActionId FROM ScreenAction WHERE ActionName = 'AddEditDeleteHolidaysInBusinessDays'
SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'Admin'
IF NOT EXISTS ( SELECT TOP 1 1 FROM RoleAction WHERE RoleId = @RoleId AND ScreenActionId = @ScreenActionId)
	BEGIN
		INSERT INTO [dbo].[RoleAction]
           ([RoleId]
           ,[ScreenActionId]
           ,[RecordStatus]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[ModifiedBy]
           ,[ModifiedDate])
		VALUES
		(
			@RoleId, 			
			@ScreenActionId,
			@RecordStatus,	
			@UserName,				
			@CurrentDate,	
			@UserName,				
			@CurrentDate
		)
	END

SELECT  @ScreenActionId = ScreenActionId FROM ScreenAction WHERE ActionName = 'ViewGridGraphInBusinessDays'
SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'Admin'
IF NOT EXISTS ( SELECT TOP 1 1 FROM RoleAction WHERE RoleId = @RoleId AND ScreenActionId = @ScreenActionId)
	BEGIN
		INSERT INTO [dbo].[RoleAction]
           ([RoleId]
           ,[ScreenActionId]
           ,[RecordStatus]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[ModifiedBy]
           ,[ModifiedDate])
		VALUES
		(
			@RoleId, 			
			@ScreenActionId,
			@RecordStatus,	
			@UserName,				
			@CurrentDate,	
			@UserName,				
			@CurrentDate
		)
	END

SELECT  @ScreenActionId = ScreenActionId FROM ScreenAction WHERE ActionName = 'ViewGridGraphInBusinessDays'
SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'Executive'
IF NOT EXISTS ( SELECT TOP 1 1 FROM RoleAction WHERE RoleId = @RoleId AND ScreenActionId = @ScreenActionId)
	BEGIN
		INSERT INTO [dbo].[RoleAction]
           ([RoleId]
           ,[ScreenActionId]
           ,[RecordStatus]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[ModifiedBy]
           ,[ModifiedDate])
		VALUES
		(
			@RoleId, 			
			@ScreenActionId,
			@RecordStatus,	
			@UserName,				
			@CurrentDate,	
			@UserName,				
			@CurrentDate
		)
	END

SELECT  @ScreenActionId = ScreenActionId FROM ScreenAction WHERE ActionName = 'AddBusinessUnit'
SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'Admin'
IF NOT EXISTS ( SELECT TOP 1 1 FROM RoleAction WHERE RoleId = @RoleId AND ScreenActionId = @ScreenActionId)
	BEGIN
		INSERT INTO [dbo].[RoleAction]
           ([RoleId]
           ,[ScreenActionId]
           ,[RecordStatus]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[ModifiedBy]
           ,[ModifiedDate])
		VALUES
		(
			@RoleId, 			
			@ScreenActionId,
			@RecordStatus,	
			@UserName,				
			@CurrentDate,	
			@UserName,				
			@CurrentDate
		)
	END

SELECT  @ScreenActionId = ScreenActionId FROM ScreenAction WHERE ActionName = 'EditBusinessUnit'
SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'Admin'
IF NOT EXISTS ( SELECT TOP 1 1 FROM RoleAction WHERE RoleId = @RoleId AND ScreenActionId = @ScreenActionId)
	BEGIN
		INSERT INTO [dbo].[RoleAction]
           ([RoleId]
           ,[ScreenActionId]
           ,[RecordStatus]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[ModifiedBy]
           ,[ModifiedDate])
		VALUES
		(
			@RoleId, 			
			@ScreenActionId,
			@RecordStatus,	
			@UserName,				
			@CurrentDate,	
			@UserName,				
			@CurrentDate
		)
	END

SELECT  @ScreenActionId = ScreenActionId FROM ScreenAction WHERE ActionName = 'AddPayer'
SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'Admin'
IF NOT EXISTS ( SELECT TOP 1 1 FROM RoleAction WHERE RoleId = @RoleId AND ScreenActionId = @ScreenActionId)
	BEGIN
		INSERT INTO [dbo].[RoleAction]
           ([RoleId]
           ,[ScreenActionId]
           ,[RecordStatus]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[ModifiedBy]
           ,[ModifiedDate])
		VALUES
		(
			@RoleId, 			
			@ScreenActionId,
			@RecordStatus,	
			@UserName,				
			@CurrentDate,	
			@UserName,				
			@CurrentDate
		)
	END

SELECT  @ScreenActionId = ScreenActionId FROM ScreenAction WHERE ActionName = 'AddPayer'
SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'Manager'
IF NOT EXISTS ( SELECT TOP 1 1 FROM RoleAction WHERE RoleId = @RoleId AND ScreenActionId = @ScreenActionId)
	BEGIN
		INSERT INTO [dbo].[RoleAction]
           ([RoleId]
           ,[ScreenActionId]
           ,[RecordStatus]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[ModifiedBy]
           ,[ModifiedDate])
		VALUES
		(
			@RoleId, 			
			@ScreenActionId,
			@RecordStatus,	
			@UserName,				
			@CurrentDate,	
			@UserName,				
			@CurrentDate
		)
	END

SELECT  @ScreenActionId = ScreenActionId FROM ScreenAction WHERE ActionName = 'AddPayer'
SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'Executive'
IF NOT EXISTS ( SELECT TOP 1 1 FROM RoleAction WHERE RoleId = @RoleId AND ScreenActionId = @ScreenActionId)
	BEGIN
		INSERT INTO [dbo].[RoleAction]
           ([RoleId]
           ,[ScreenActionId]
           ,[RecordStatus]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[ModifiedBy]
           ,[ModifiedDate])
		VALUES
		(
			@RoleId, 			
			@ScreenActionId,
			@RecordStatus,	
			@UserName,				
			@CurrentDate,	
			@UserName,				
			@CurrentDate
		)
	END

SELECT  @ScreenActionId = ScreenActionId FROM ScreenAction WHERE ActionName = 'AddPayer'
SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'User'
IF NOT EXISTS ( SELECT TOP 1 1 FROM RoleAction WHERE RoleId = @RoleId AND ScreenActionId = @ScreenActionId)
	BEGIN
		INSERT INTO [dbo].[RoleAction]
           ([RoleId]
           ,[ScreenActionId]
           ,[RecordStatus]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[ModifiedBy]
           ,[ModifiedDate])
		VALUES
		(
			@RoleId, 			
			@ScreenActionId,
			@RecordStatus,	
			@UserName,				
			@CurrentDate,	
			@UserName,				
			@CurrentDate
		)
	END

SELECT  @ScreenActionId = ScreenActionId FROM ScreenAction WHERE ActionName = 'EditPayerName'
SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'Admin'
IF NOT EXISTS ( SELECT TOP 1 1 FROM RoleAction WHERE RoleId = @RoleId AND ScreenActionId = @ScreenActionId)
	BEGIN
		INSERT INTO [dbo].[RoleAction]
           ([RoleId]
           ,[ScreenActionId]
           ,[RecordStatus]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[ModifiedBy]
           ,[ModifiedDate])
		VALUES
		(
			@RoleId, 			
			@ScreenActionId,
			@RecordStatus,	
			@UserName,				
			@CurrentDate,	
			@UserName,				
			@CurrentDate
		)
	END

SELECT  @ScreenActionId = ScreenActionId FROM ScreenAction WHERE ActionName = 'EditPayerName'
SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'Manager'
IF NOT EXISTS ( SELECT TOP 1 1 FROM RoleAction WHERE RoleId = @RoleId AND ScreenActionId = @ScreenActionId)
	BEGIN
		INSERT INTO [dbo].[RoleAction]
           ([RoleId]
           ,[ScreenActionId]
           ,[RecordStatus]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[ModifiedBy]
           ,[ModifiedDate])
		VALUES
		(
			@RoleId, 			
			@ScreenActionId,
			@RecordStatus,	
			@UserName,				
			@CurrentDate,	
			@UserName,				
			@CurrentDate
		)
	END

SELECT  @ScreenActionId = ScreenActionId FROM ScreenAction WHERE ActionName = 'EditPayerName'
SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'Executive'
IF NOT EXISTS ( SELECT TOP 1 1 FROM RoleAction WHERE RoleId = @RoleId AND ScreenActionId = @ScreenActionId)
	BEGIN
		INSERT INTO [dbo].[RoleAction]
           ([RoleId]
           ,[ScreenActionId]
           ,[RecordStatus]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[ModifiedBy]
           ,[ModifiedDate])
		VALUES
		(
			@RoleId, 			
			@ScreenActionId,
			@RecordStatus,	
			@UserName,				
			@CurrentDate,	
			@UserName,				
			@CurrentDate
		)
	END

SELECT  @ScreenActionId = ScreenActionId FROM ScreenAction WHERE ActionName = 'MakePayerActiveInActive'
SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'Admin'
IF NOT EXISTS ( SELECT TOP 1 1 FROM RoleAction WHERE RoleId = @RoleId AND ScreenActionId = @ScreenActionId)
	BEGIN
		INSERT INTO [dbo].[RoleAction]
           ([RoleId]
           ,[ScreenActionId]
           ,[RecordStatus]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[ModifiedBy]
           ,[ModifiedDate])
		VALUES
		(
			@RoleId, 			
			@ScreenActionId,
			@RecordStatus,	
			@UserName,				
			@CurrentDate,	
			@UserName,				
			@CurrentDate
		)
	END

SELECT  @ScreenActionId = ScreenActionId FROM ScreenAction WHERE ActionName = 'MakePayerActiveInActive'
SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'Manager'
IF NOT EXISTS ( SELECT TOP 1 1 FROM RoleAction WHERE RoleId = @RoleId AND ScreenActionId = @ScreenActionId)
	BEGIN
		INSERT INTO [dbo].[RoleAction]
           ([RoleId]
           ,[ScreenActionId]
           ,[RecordStatus]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[ModifiedBy]
           ,[ModifiedDate])
		VALUES
		(
			@RoleId, 			
			@ScreenActionId,
			@RecordStatus,	
			@UserName,				
			@CurrentDate,	
			@UserName,				
			@CurrentDate
		)
	END

SELECT  @ScreenActionId = ScreenActionId FROM ScreenAction WHERE ActionName = 'MakePayerActiveInActive'
SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'Executive'
IF NOT EXISTS ( SELECT TOP 1 1 FROM RoleAction WHERE RoleId = @RoleId AND ScreenActionId = @ScreenActionId)
	BEGIN
		INSERT INTO [dbo].[RoleAction]
           ([RoleId]
           ,[ScreenActionId]
           ,[RecordStatus]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[ModifiedBy]
           ,[ModifiedDate])
		VALUES
		(
			@RoleId, 			
			@ScreenActionId,
			@RecordStatus,	
			@UserName,				
			@CurrentDate,	
			@UserName,				
			@CurrentDate
		)
	END

SELECT  @ScreenActionId = ScreenActionId FROM ScreenAction WHERE ActionName = 'EditClient'
SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'Admin'
IF NOT EXISTS ( SELECT TOP 1 1 FROM RoleAction WHERE RoleId = @RoleId AND ScreenActionId = @ScreenActionId)
	BEGIN
		INSERT INTO [dbo].[RoleAction]
           ([RoleId]
           ,[ScreenActionId]
           ,[RecordStatus]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[ModifiedBy]
           ,[ModifiedDate])
		VALUES
		(
			@RoleId, 			
			@ScreenActionId,
			@RecordStatus,	
			@UserName,				
			@CurrentDate,	
			@UserName,				
			@CurrentDate
		)
	END

SELECT  @ScreenActionId = ScreenActionId FROM ScreenAction WHERE ActionName = 'EditClient'
SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'Manager'
IF NOT EXISTS ( SELECT TOP 1 1 FROM RoleAction WHERE RoleId = @RoleId AND ScreenActionId = @ScreenActionId)
	BEGIN
		INSERT INTO [dbo].[RoleAction]
           ([RoleId]
           ,[ScreenActionId]
           ,[RecordStatus]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[ModifiedBy]
           ,[ModifiedDate])
		VALUES
		(
			@RoleId, 			
			@ScreenActionId,
			@RecordStatus,	
			@UserName,				
			@CurrentDate,	
			@UserName,				
			@CurrentDate
		)
	END

SELECT  @ScreenActionId = ScreenActionId FROM ScreenAction WHERE ActionName = 'EditClient'
SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'Executive'
IF NOT EXISTS ( SELECT TOP 1 1 FROM RoleAction WHERE RoleId = @RoleId AND ScreenActionId = @ScreenActionId)
	BEGIN
		INSERT INTO [dbo].[RoleAction]
           ([RoleId]
           ,[ScreenActionId]
           ,[RecordStatus]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[ModifiedBy]
           ,[ModifiedDate])
		VALUES
		(
			@RoleId, 			
			@ScreenActionId,
			@RecordStatus,	
			@UserName,				
			@CurrentDate,	
			@UserName,				
			@CurrentDate
		)
	END

SELECT  @ScreenActionId = ScreenActionId FROM ScreenAction WHERE ActionName = 'EditClient'
SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'User'
IF NOT EXISTS ( SELECT TOP 1 1 FROM RoleAction WHERE RoleId = @RoleId AND ScreenActionId = @ScreenActionId)
	BEGIN
		INSERT INTO [dbo].[RoleAction]
           ([RoleId]
           ,[ScreenActionId]
           ,[RecordStatus]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[ModifiedBy]
           ,[ModifiedDate])
		VALUES
		(
			@RoleId, 			
			@ScreenActionId,
			@RecordStatus,	
			@UserName,				
			@CurrentDate,	
			@UserName,				
			@CurrentDate
		)
	END

SELECT  @ScreenActionId = ScreenActionId FROM ScreenAction WHERE ActionName = 'AddSystem'
SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'Admin'
IF NOT EXISTS ( SELECT TOP 1 1 FROM RoleAction WHERE RoleId = @RoleId AND ScreenActionId = @ScreenActionId)
	BEGIN
		INSERT INTO [dbo].[RoleAction]
           ([RoleId]
           ,[ScreenActionId]
           ,[RecordStatus]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[ModifiedBy]
           ,[ModifiedDate])
		VALUES
		(
			@RoleId, 			
			@ScreenActionId,
			@RecordStatus,	
			@UserName,				
			@CurrentDate,	
			@UserName,				
			@CurrentDate
		)
	END

SELECT  @ScreenActionId = ScreenActionId FROM ScreenAction WHERE ActionName = 'EditSystem'
SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'Admin'
IF NOT EXISTS ( SELECT TOP 1 1 FROM RoleAction WHERE RoleId = @RoleId AND ScreenActionId = @ScreenActionId)
	BEGIN
		INSERT INTO [dbo].[RoleAction]
           ([RoleId]
           ,[ScreenActionId]
           ,[RecordStatus]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[ModifiedBy]
           ,[ModifiedDate])
		VALUES
		(
			@RoleId, 			
			@ScreenActionId,
			@RecordStatus,	
			@UserName,				
			@CurrentDate,	
			@UserName,				
			@CurrentDate
		)
	END

SELECT  @ScreenActionId = ScreenActionId FROM ScreenAction WHERE ActionName = 'EditChecklist'
SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'Admin'
IF NOT EXISTS ( SELECT TOP 1 1 FROM RoleAction WHERE RoleId = @RoleId AND ScreenActionId = @ScreenActionId)
	BEGIN
		INSERT INTO [dbo].[RoleAction]
           ([RoleId]
           ,[ScreenActionId]
           ,[RecordStatus]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[ModifiedBy]
           ,[ModifiedDate])
		VALUES
		(
			@RoleId, 			
			@ScreenActionId,
			@RecordStatus,	
			@UserName,				
			@CurrentDate,	
			@UserName,				
			@CurrentDate
		)
	END

SELECT  @ScreenActionId = ScreenActionId FROM ScreenAction WHERE ActionName = 'EditChecklist'
SELECT  @RoleId = RoleId FROM Roles WHERE RoleCode = 'Executive'
IF NOT EXISTS ( SELECT TOP 1 1 FROM RoleAction WHERE RoleId = @RoleId AND ScreenActionId = @ScreenActionId)
	BEGIN
		INSERT INTO [dbo].[RoleAction]
           ([RoleId]
           ,[ScreenActionId]
           ,[RecordStatus]
           ,[CreatedBy]
           ,[CreatedDate]
           ,[ModifiedBy]
           ,[ModifiedDate])
		VALUES
		(
			@RoleId, 			
			@ScreenActionId,
			@RecordStatus,	
			@UserName,				
			@CurrentDate,	
			@UserName,				
			@CurrentDate
		)
	END

SELECT @RoleId = RoleId FROM Roles WHERE RoleCode = 'Admin'
SELECT @ScreenActionId = ScreenActionId FROM ScreenAction WHERE ActionName = 'Reopen checklist'
IF NOT EXISTS (SELECT TOP 1 1 FROM RoleAction WHERE ScreenActionId = @ScreenActionID AND RoleId = @RoleId)
BEGIN
	INSERT INTO [dbo].[RoleAction]
		([RoleId]
		,[ScreenActionId]
		,[RecordStatus]
		,[CreatedBy]
		,[CreatedDate]
		,[ModifiedBy]
		,[ModifiedDate])
	VALUES
	(
		@RoleId,
		@ScreenActionId,
		@RecordStatus,
		@UserName,
		@CurrentDate,
		@UserName,
		@CurrentDate
	)
END

SELECT @RoleId = RoleId FROM Roles WHERE RoleCode = 'Admin'
SELECT @ScreenActionId = ScreenActionId FROM ScreenAction WHERE ActionName = 'Add KPI'
IF NOT EXISTS (SELECT TOP 1 1 FROM RoleAction WHERE ScreenActionId = @ScreenActionID AND RoleId = @RoleId)
BEGIN
	INSERT INTO [dbo].[RoleAction]
		([RoleId]
		,[ScreenActionId]
		,[RecordStatus]
		,[CreatedBy]
		,[CreatedDate]
		,[ModifiedBy]
		,[ModifiedDate])
	VALUES
	(
		@RoleId,
		@ScreenActionId,
		@RecordStatus,
		@UserName,
		@CurrentDate,
		@UserName,
		@CurrentDate
	)
END

SELECT @RoleId = RoleId FROM Roles WHERE RoleCode = 'Admin'
SELECT @ScreenActionId = ScreenActionId FROM ScreenAction WHERE ActionName = 'Edit KPI'
IF NOT EXISTS (SELECT TOP 1 1 FROM RoleAction WHERE ScreenActionId = @ScreenActionID AND RoleId = @RoleId)
BEGIN
	INSERT INTO [dbo].[RoleAction]
		([RoleId]
		,[ScreenActionId]
		,[RecordStatus]
		,[CreatedBy]
		,[CreatedDate]
		,[ModifiedBy]
		,[ModifiedDate])
	VALUES
	(
		@RoleId,
		@ScreenActionId,
		@RecordStatus,
		@UserName,
		@CurrentDate,
		@UserName,
		@CurrentDate
	)
END

SELECT @RoleId = RoleId FROM Roles WHERE RoleCode = 'Executive'
SELECT @ScreenActionId = ScreenActionId FROM ScreenAction WHERE ActionName = 'Add KPI'
IF NOT EXISTS (SELECT TOP 1 1 FROM RoleAction WHERE ScreenActionId = @ScreenActionID AND RoleId = @RoleId)
BEGIN
	INSERT INTO [dbo].[RoleAction]
		([RoleId]
		,[ScreenActionId]
		,[RecordStatus]
		,[CreatedBy]
		,[CreatedDate]
		,[ModifiedBy]
		,[ModifiedDate])
	VALUES
	(
		@RoleId,
		@ScreenActionId,
		@RecordStatus,
		@UserName,
		@CurrentDate,
		@UserName,
		@CurrentDate
	)
END

SELECT @RoleId = RoleId FROM Roles WHERE RoleCode = 'Executive'
SELECT @ScreenActionId = ScreenActionId FROM ScreenAction WHERE ActionName = 'Edit KPI'
IF NOT EXISTS (SELECT TOP 1 1 FROM RoleAction WHERE ScreenActionId = @ScreenActionID AND RoleId = @RoleId)
BEGIN
	INSERT INTO [dbo].[RoleAction]
		([RoleId]
		,[ScreenActionId]
		,[RecordStatus]
		,[CreatedBy]
		,[CreatedDate]
		,[ModifiedBy]
		,[ModifiedDate])
	VALUES
	(
		@RoleId,
		@ScreenActionId,
		@RecordStatus,
		@UserName,
		@CurrentDate,
		@UserName,
		@CurrentDate
	)
END

SELECT @RoleId = RoleId FROM Roles WHERE RoleCode = 'Admin'
SELECT @ScreenActionId = ScreenActionId FROM ScreenAction WHERE ActionName = 'SaveLastEnteredBusinessDaysOrWeeks'
IF NOT EXISTS (SELECT TOP 1 1 FROM RoleAction WHERE ScreenActionId = @ScreenActionID AND RoleId = @RoleId)
BEGIN
	INSERT INTO [dbo].[RoleAction]
		([RoleId]
		,[ScreenActionId]
		,[RecordStatus]
		,[CreatedBy]
		,[CreatedDate]
		,[ModifiedBy]
		,[ModifiedDate])
	VALUES
	(
		@RoleId,
		@ScreenActionId,
		@RecordStatus,
		@UserName,
		@CurrentDate,
		@UserName,
		@CurrentDate
	)
END

SELECT @RoleId = RoleId FROM Roles WHERE RoleCode = 'Admin'
SELECT @ScreenActionId = ScreenActionId FROM ScreenAction WHERE ActionName = 'EditAdminConfig'
IF NOT EXISTS (SELECT TOP 1 1 FROM RoleAction WHERE ScreenActionId = @ScreenActionID AND RoleId = @RoleId)
BEGIN
	INSERT INTO [dbo].[RoleAction]
		([RoleId]
		,[ScreenActionId]
		,[RecordStatus]
		,[CreatedBy]
		,[CreatedDate]
		,[ModifiedBy]
		,[ModifiedDate])
	VALUES
	(
		@RoleId,
		@ScreenActionId,
		@RecordStatus,
		@UserName,
		@CurrentDate,
		@UserName,
		@CurrentDate
	)
END

SELECT @RoleId = RoleId FROM Roles WHERE RoleCode = 'Admin'
SELECT @ScreenActionId = ScreenActionId FROM ScreenAction WHERE ActionName = 'ViewActualM3Revenue'
IF NOT EXISTS (SELECT TOP 1 1 FROM RoleAction WHERE ScreenActionId = @ScreenActionID AND RoleId = @RoleId)
BEGIN
	INSERT INTO [dbo].[RoleAction]
		([RoleId]
		,[ScreenActionId]
		,[RecordStatus]
		,[CreatedBy]
		,[CreatedDate]
		,[ModifiedBy]
		,[ModifiedDate])
	VALUES
	(
		@RoleId,
		@ScreenActionId,
		@RecordStatus,
		@UserName,
		@CurrentDate,
		@UserName,
		@CurrentDate
	)
END

SELECT @RoleId = RoleId FROM Roles WHERE RoleCode = 'Executive'
SELECT @ScreenActionId = ScreenActionId FROM ScreenAction WHERE ActionName = 'ViewActualM3Revenue'
IF NOT EXISTS (SELECT TOP 1 1 FROM RoleAction WHERE ScreenActionId = @ScreenActionID AND RoleId = @RoleId)
BEGIN
	INSERT INTO [dbo].[RoleAction]
		([RoleId]
		,[ScreenActionId]
		,[RecordStatus]
		,[CreatedBy]
		,[CreatedDate]
		,[ModifiedBy]
		,[ModifiedDate])
	VALUES
	(
		@RoleId,
		@ScreenActionId,
		@RecordStatus,
		@UserName,
		@CurrentDate,
		@UserName,
		@CurrentDate
	)
END

SELECT @RoleId = RoleId FROM Roles WHERE RoleCode = 'Admin'
SELECT @ScreenActionId = ScreenActionId FROM ScreenAction WHERE ActionName = 'ViewForecastedM3Revenue'
IF NOT EXISTS (SELECT TOP 1 1 FROM RoleAction WHERE ScreenActionId = @ScreenActionID AND RoleId = @RoleId)
BEGIN
	INSERT INTO [dbo].[RoleAction]
		([RoleId]
		,[ScreenActionId]
		,[RecordStatus]
		,[CreatedBy]
		,[CreatedDate]
		,[ModifiedBy]
		,[ModifiedDate])
	VALUES
	(
		@RoleId,
		@ScreenActionId,
		@RecordStatus,
		@UserName,
		@CurrentDate,
		@UserName,
		@CurrentDate
	)
END

SELECT @RoleId = RoleId FROM Roles WHERE RoleCode = 'Executive'
SELECT @ScreenActionId = ScreenActionId FROM ScreenAction WHERE ActionName = 'ViewForecastedM3Revenue'
IF NOT EXISTS (SELECT TOP 1 1 FROM RoleAction WHERE ScreenActionId = @ScreenActionID AND RoleId = @RoleId)
BEGIN
	INSERT INTO [dbo].[RoleAction]
		([RoleId]
		,[ScreenActionId]
		,[RecordStatus]
		,[CreatedBy]
		,[CreatedDate]
		,[ModifiedBy]
		,[ModifiedDate])
	VALUES
	(
		@RoleId,
		@ScreenActionId,
		@RecordStatus,
		@UserName,
		@CurrentDate,
		@UserName,
		@CurrentDate
	)
END

-- To-do-list role based actions.

SELECT @RoleId = RoleId FROM Roles WHERE RoleCode = 'Admin'
SELECT @ScreenActionId = ScreenActionId FROM ScreenAction WHERE ActionName = 'Assign Relationship Manager'
IF NOT EXISTS (SELECT TOP 1 1 FROM RoleAction WHERE ScreenActionId = @ScreenActionID AND RoleId = @RoleId)
BEGIN
	INSERT INTO [dbo].[RoleAction]
		([RoleId]
		,[ScreenActionId]
		,[RecordStatus]
		,[CreatedBy]
		,[CreatedDate]
		,[ModifiedBy]
		,[ModifiedDate])
	VALUES
	(
		@RoleId,
		@ScreenActionId,
		@RecordStatus,
		@UserName,
		@CurrentDate,
		@UserName,
		@CurrentDate
	)
END

SELECT @RoleId = RoleId FROM Roles WHERE RoleCode = 'Executive'
SELECT @ScreenActionId = ScreenActionId FROM ScreenAction WHERE ActionName = 'Assign Relationship Manager'
IF NOT EXISTS (SELECT TOP 1 1 FROM RoleAction WHERE ScreenActionId = @ScreenActionID AND RoleId = @RoleId)
BEGIN
	INSERT INTO [dbo].[RoleAction]
		([RoleId]
		,[ScreenActionId]
		,[RecordStatus]
		,[CreatedBy]
		,[CreatedDate]
		,[ModifiedBy]
		,[ModifiedDate])
	VALUES
	(
		@RoleId,
		@ScreenActionId,
		@RecordStatus,
		@UserName,
		@CurrentDate,
		@UserName,
		@CurrentDate
	)
END

SELECT @RoleId = RoleId FROM Roles WHERE RoleCode = 'Manager'
SELECT @ScreenActionId = ScreenActionId FROM ScreenAction WHERE ActionName = 'Assign Relationship Manager'
IF NOT EXISTS (SELECT TOP 1 1 FROM RoleAction WHERE ScreenActionId = @ScreenActionID AND RoleId = @RoleId)
BEGIN
	INSERT INTO [dbo].[RoleAction]
		([RoleId]
		,[ScreenActionId]
		,[RecordStatus]
		,[CreatedBy]
		,[CreatedDate]
		,[ModifiedBy]
		,[ModifiedDate])
	VALUES
	(
		@RoleId,
		@ScreenActionId,
		@RecordStatus,
		@UserName,
		@CurrentDate,
		@UserName,
		@CurrentDate
	)
END

SELECT @RoleId = RoleId FROM Roles WHERE RoleCode = 'Admin'
SELECT @ScreenActionId = ScreenActionId FROM ScreenAction WHERE ActionName = 'Assign Billing Manager'
IF NOT EXISTS (SELECT TOP 1 1 FROM RoleAction WHERE ScreenActionId = @ScreenActionID AND RoleId = @RoleId)
BEGIN
	INSERT INTO [dbo].[RoleAction]
		([RoleId]
		,[ScreenActionId]
		,[RecordStatus]
		,[CreatedBy]
		,[CreatedDate]
		,[ModifiedBy]
		,[ModifiedDate])
	VALUES
	(
		@RoleId,
		@ScreenActionId,
		@RecordStatus,
		@UserName,
		@CurrentDate,
		@UserName,
		@CurrentDate
	)
END

SELECT @RoleId = RoleId FROM Roles WHERE RoleCode = 'Executive'
SELECT @ScreenActionId = ScreenActionId FROM ScreenAction WHERE ActionName = 'Assign Billing Manager'
IF NOT EXISTS (SELECT TOP 1 1 FROM RoleAction WHERE ScreenActionId = @ScreenActionID AND RoleId = @RoleId)
BEGIN
	INSERT INTO [dbo].[RoleAction]
		([RoleId]
		,[ScreenActionId]
		,[RecordStatus]
		,[CreatedBy]
		,[CreatedDate]
		,[ModifiedBy]
		,[ModifiedDate])
	VALUES
	(
		@RoleId,
		@ScreenActionId,
		@RecordStatus,
		@UserName,
		@CurrentDate,
		@UserName,
		@CurrentDate
	)
END

SELECT @RoleId = RoleId FROM Roles WHERE RoleCode = 'Manager'
SELECT @ScreenActionId = ScreenActionId FROM ScreenAction WHERE ActionName = 'Assign Billing Manager'
IF NOT EXISTS (SELECT TOP 1 1 FROM RoleAction WHERE ScreenActionId = @ScreenActionID AND RoleId = @RoleId)
BEGIN
	INSERT INTO [dbo].[RoleAction]
		([RoleId]
		,[ScreenActionId]
		,[RecordStatus]
		,[CreatedBy]
		,[CreatedDate]
		,[ModifiedBy]
		,[ModifiedDate])
	VALUES
	(
		@RoleId,
		@ScreenActionId,
		@RecordStatus,
		@UserName,
		@CurrentDate,
		@UserName,
		@CurrentDate
	)
END

SELECT @RoleId = RoleId FROM Roles WHERE RoleCode = 'Admin'
SELECT @ScreenActionId = ScreenActionId FROM ScreenAction WHERE ActionName = 'Pending Weekly Checklist'
IF NOT EXISTS (SELECT TOP 1 1 FROM RoleAction WHERE ScreenActionId = @ScreenActionID AND RoleId = @RoleId)
BEGIN
	INSERT INTO [dbo].[RoleAction]
		([RoleId]
		,[ScreenActionId]
		,[RecordStatus]
		,[CreatedBy]
		,[CreatedDate]
		,[ModifiedBy]
		,[ModifiedDate])
	VALUES
	(
		@RoleId,
		@ScreenActionId,
		@RecordStatus,
		@UserName,
		@CurrentDate,
		@UserName,
		@CurrentDate
	)
END

SELECT @RoleId = RoleId FROM Roles WHERE RoleCode = 'Executive'
SELECT @ScreenActionId = ScreenActionId FROM ScreenAction WHERE ActionName = 'Pending Weekly Checklist'
IF NOT EXISTS (SELECT TOP 1 1 FROM RoleAction WHERE ScreenActionId = @ScreenActionID AND RoleId = @RoleId)
BEGIN
	INSERT INTO [dbo].[RoleAction]
		([RoleId]
		,[ScreenActionId]
		,[RecordStatus]
		,[CreatedBy]
		,[CreatedDate]
		,[ModifiedBy]
		,[ModifiedDate])
	VALUES
	(
		@RoleId,
		@ScreenActionId,
		@RecordStatus,
		@UserName,
		@CurrentDate,
		@UserName,
		@CurrentDate
	)
END

SELECT @RoleId = RoleId FROM Roles WHERE RoleCode = 'Manager'
SELECT @ScreenActionId = ScreenActionId FROM ScreenAction WHERE ActionName = 'Pending Weekly Checklist'
IF NOT EXISTS (SELECT TOP 1 1 FROM RoleAction WHERE ScreenActionId = @ScreenActionID AND RoleId = @RoleId)
BEGIN
	INSERT INTO [dbo].[RoleAction]
		([RoleId]
		,[ScreenActionId]
		,[RecordStatus]
		,[CreatedBy]
		,[CreatedDate]
		,[ModifiedBy]
		,[ModifiedDate])
	VALUES
	(
		@RoleId,
		@ScreenActionId,
		@RecordStatus,
		@UserName,
		@CurrentDate,
		@UserName,
		@CurrentDate
	)
END

SELECT @RoleId = RoleId FROM Roles WHERE RoleCode = 'User'
SELECT @ScreenActionId = ScreenActionId FROM ScreenAction WHERE ActionName = 'Pending Weekly Checklist'
IF NOT EXISTS (SELECT TOP 1 1 FROM RoleAction WHERE ScreenActionId = @ScreenActionID AND RoleId = @RoleId)
BEGIN
	INSERT INTO [dbo].[RoleAction]
		([RoleId]
		,[ScreenActionId]
		,[RecordStatus]
		,[CreatedBy]
		,[CreatedDate]
		,[ModifiedBy]
		,[ModifiedDate])
	VALUES
	(
		@RoleId,
		@ScreenActionId,
		@RecordStatus,
		@UserName,
		@CurrentDate,
		@UserName,
		@CurrentDate
	)
END

SELECT @RoleId = RoleId FROM Roles WHERE RoleCode = 'Admin'
SELECT @ScreenActionId = ScreenActionId FROM ScreenAction WHERE ActionName = 'Pending Monthly Checklist'
IF NOT EXISTS (SELECT TOP 1 1 FROM RoleAction WHERE ScreenActionId = @ScreenActionID AND RoleId = @RoleId)
BEGIN
	INSERT INTO [dbo].[RoleAction]
		([RoleId]
		,[ScreenActionId]
		,[RecordStatus]
		,[CreatedBy]
		,[CreatedDate]
		,[ModifiedBy]
		,[ModifiedDate])
	VALUES
	(
		@RoleId,
		@ScreenActionId,
		@RecordStatus,
		@UserName,
		@CurrentDate,
		@UserName,
		@CurrentDate
	)
END

SELECT @RoleId = RoleId FROM Roles WHERE RoleCode = 'Executive'
SELECT @ScreenActionId = ScreenActionId FROM ScreenAction WHERE ActionName = 'Pending Monthly Checklist'
IF NOT EXISTS (SELECT TOP 1 1 FROM RoleAction WHERE ScreenActionId = @ScreenActionID AND RoleId = @RoleId)
BEGIN
	INSERT INTO [dbo].[RoleAction]
		([RoleId]
		,[ScreenActionId]
		,[RecordStatus]
		,[CreatedBy]
		,[CreatedDate]
		,[ModifiedBy]
		,[ModifiedDate])
	VALUES
	(
		@RoleId,
		@ScreenActionId,
		@RecordStatus,
		@UserName,
		@CurrentDate,
		@UserName,
		@CurrentDate
	)
END

SELECT @RoleId = RoleId FROM Roles WHERE RoleCode = 'Manager'
SELECT @ScreenActionId = ScreenActionId FROM ScreenAction WHERE ActionName = 'Pending Monthly Checklist'
IF NOT EXISTS (SELECT TOP 1 1 FROM RoleAction WHERE ScreenActionId = @ScreenActionID AND RoleId = @RoleId)
BEGIN
	INSERT INTO [dbo].[RoleAction]
		([RoleId]
		,[ScreenActionId]
		,[RecordStatus]
		,[CreatedBy]
		,[CreatedDate]
		,[ModifiedBy]
		,[ModifiedDate])
	VALUES
	(
		@RoleId,
		@ScreenActionId,
		@RecordStatus,
		@UserName,
		@CurrentDate,
		@UserName,
		@CurrentDate
	)
END

SELECT @RoleId = RoleId FROM Roles WHERE RoleCode = 'User'
SELECT @ScreenActionId = ScreenActionId FROM ScreenAction WHERE ActionName = 'Pending Monthly Checklist'
IF NOT EXISTS (SELECT TOP 1 1 FROM RoleAction WHERE ScreenActionId = @ScreenActionID AND RoleId = @RoleId)
BEGIN
	INSERT INTO [dbo].[RoleAction]
		([RoleId]
		,[ScreenActionId]
		,[RecordStatus]
		,[CreatedBy]
		,[CreatedDate]
		,[ModifiedBy]
		,[ModifiedDate])
	VALUES
	(
		@RoleId,
		@ScreenActionId,
		@RecordStatus,
		@UserName,
		@CurrentDate,
		@UserName,
		@CurrentDate
	)
END

SELECT @RoleId = RoleId FROM Roles WHERE RoleCode = 'Admin'
SELECT @ScreenActionId = ScreenActionId FROM ScreenAction WHERE ActionName = 'Partially Completed Clients'
IF NOT EXISTS (SELECT TOP 1 1 FROM RoleAction WHERE ScreenActionId = @ScreenActionID AND RoleId = @RoleId)
BEGIN
	INSERT INTO [dbo].[RoleAction]
		([RoleId]
		,[ScreenActionId]
		,[RecordStatus]
		,[CreatedBy]
		,[CreatedDate]
		,[ModifiedBy]
		,[ModifiedDate])
	VALUES
	(
		@RoleId,
		@ScreenActionId,
		@RecordStatus,
		@UserName,
		@CurrentDate,
		@UserName,
		@CurrentDate
	)
END

SELECT @RoleId = RoleId FROM Roles WHERE RoleCode = 'Admin'
SELECT @ScreenActionId = ScreenActionId FROM ScreenAction WHERE ActionName = 'Set Targets'
IF NOT EXISTS (SELECT TOP 1 1 FROM RoleAction WHERE ScreenActionId = @ScreenActionID AND RoleId = @RoleId)
BEGIN
	INSERT INTO [dbo].[RoleAction]
		([RoleId]
		,[ScreenActionId]
		,[RecordStatus]
		,[CreatedBy]
		,[CreatedDate]
		,[ModifiedBy]
		,[ModifiedDate])
	VALUES
	(
		@RoleId,
		@ScreenActionId,
		@RecordStatus,
		@UserName,
		@CurrentDate,
		@UserName,
		@CurrentDate
	)
END

SELECT @RoleId = RoleId FROM Roles WHERE RoleCode = 'Executive'
SELECT @ScreenActionId = ScreenActionId FROM ScreenAction WHERE ActionName = 'Set Targets'
IF NOT EXISTS (SELECT TOP 1 1 FROM RoleAction WHERE ScreenActionId = @ScreenActionID AND RoleId = @RoleId)
BEGIN
	INSERT INTO [dbo].[RoleAction]
		([RoleId]
		,[ScreenActionId]
		,[RecordStatus]
		,[CreatedBy]
		,[CreatedDate]
		,[ModifiedBy]
		,[ModifiedDate])
	VALUES
	(
		@RoleId,
		@ScreenActionId,
		@RecordStatus,
		@UserName,
		@CurrentDate,
		@UserName,
		@CurrentDate
	)
END

SELECT @RoleId = RoleId FROM Roles WHERE RoleCode = 'Manager'
SELECT @ScreenActionId = ScreenActionId FROM ScreenAction WHERE ActionName = 'Set Targets'
IF NOT EXISTS (SELECT TOP 1 1 FROM RoleAction WHERE ScreenActionId = @ScreenActionID AND RoleId = @RoleId)
BEGIN
	INSERT INTO [dbo].[RoleAction]
		([RoleId]
		,[ScreenActionId]
		,[RecordStatus]
		,[CreatedBy]
		,[CreatedDate]
		,[ModifiedBy]
		,[ModifiedDate])
	VALUES
	(
		@RoleId,
		@ScreenActionId,
		@RecordStatus,
		@UserName,
		@CurrentDate,
		@UserName,
		@CurrentDate
	)
END

SELECT @RoleId = RoleId FROM Roles WHERE RoleCode = 'Admin'
SELECT @ScreenActionId = ScreenActionId FROM ScreenAction WHERE ActionName = 'Set Holidays'
IF NOT EXISTS (SELECT TOP 1 1 FROM RoleAction WHERE ScreenActionId = @ScreenActionID AND RoleId = @RoleId)
BEGIN
	INSERT INTO [dbo].[RoleAction]
		([RoleId]
		,[ScreenActionId]
		,[RecordStatus]
		,[CreatedBy]
		,[CreatedDate]
		,[ModifiedBy]
		,[ModifiedDate])
	VALUES
	(
		@RoleId,
		@ScreenActionId,
		@RecordStatus,
		@UserName,
		@CurrentDate,
		@UserName,
		@CurrentDate
	)
END

SELECT @RoleId = RoleId FROM Roles WHERE RoleCode = 'Admin'
SELECT @ScreenActionId = ScreenActionId FROM ScreenAction WHERE ActionName = 'Unclosed Months'
IF NOT EXISTS (SELECT TOP 1 1 FROM RoleAction WHERE ScreenActionId = @ScreenActionID AND RoleId = @RoleId)
BEGIN
	INSERT INTO [dbo].[RoleAction]
		([RoleId]
		,[ScreenActionId]
		,[RecordStatus]
		,[CreatedBy]
		,[CreatedDate]
		,[ModifiedBy]
		,[ModifiedDate])
	VALUES
	(
		@RoleId,
		@ScreenActionId,
		@RecordStatus,
		@UserName,
		@CurrentDate,
		@UserName,
		@CurrentDate
	)
END

SELECT @RoleId = RoleId FROM Roles WHERE RoleCode = 'Manager'
SELECT @ScreenActionId = ScreenActionId FROM ScreenAction WHERE ActionName = 'Unclosed Months'
IF NOT EXISTS (SELECT TOP 1 1 FROM RoleAction WHERE ScreenActionId = @ScreenActionID AND RoleId = @RoleId)
BEGIN
	INSERT INTO [dbo].[RoleAction]
		([RoleId]
		,[ScreenActionId]
		,[RecordStatus]
		,[CreatedBy]
		,[CreatedDate]
		,[ModifiedBy]
		,[ModifiedDate])
	VALUES
	(
		@RoleId,
		@ScreenActionId,
		@RecordStatus,
		@UserName,
		@CurrentDate,
		@UserName,
		@CurrentDate
	)
END

SELECT @RoleId = RoleId FROM Roles WHERE RoleCode = 'User'
SELECT @ScreenActionId = ScreenActionId FROM ScreenAction WHERE ActionName = 'Unclosed Months'
IF NOT EXISTS (SELECT TOP 1 1 FROM RoleAction WHERE ScreenActionId = @ScreenActionID AND RoleId = @RoleId)
BEGIN
	INSERT INTO [dbo].[RoleAction]
		([RoleId]
		,[ScreenActionId]
		,[RecordStatus]
		,[CreatedBy]
		,[CreatedDate]
		,[ModifiedBy]
		,[ModifiedDate])
	VALUES
	(
		@RoleId,
		@ScreenActionId,
		@RecordStatus,
		@UserName,
		@CurrentDate,
		@UserName,
		@CurrentDate
	)
END

SELECT @RoleId = RoleId FROM Roles WHERE RoleCode = 'Admin'
SELECT @ScreenActionId = ScreenActionId FROM ScreenAction WHERE ActionName = 'Incomplete Deposit Log'
IF NOT EXISTS (SELECT TOP 1 1 FROM RoleAction WHERE ScreenActionId = @ScreenActionID AND RoleId = @RoleId)
BEGIN
	INSERT INTO [dbo].[RoleAction]
		([RoleId]
		,[ScreenActionId]
		,[RecordStatus]
		,[CreatedBy]
		,[CreatedDate]
		,[ModifiedBy]
		,[ModifiedDate])
	VALUES
	(
		@RoleId,
		@ScreenActionId,
		@RecordStatus,
		@UserName,
		@CurrentDate,
		@UserName,
		@CurrentDate
	)
END

SELECT @RoleId = RoleId FROM Roles WHERE RoleCode = 'Manager'
SELECT @ScreenActionId = ScreenActionId FROM ScreenAction WHERE ActionName = 'Incomplete Deposit Log'
IF NOT EXISTS (SELECT TOP 1 1 FROM RoleAction WHERE ScreenActionId = @ScreenActionID AND RoleId = @RoleId)
BEGIN
	INSERT INTO [dbo].[RoleAction]
		([RoleId]
		,[ScreenActionId]
		,[RecordStatus]
		,[CreatedBy]
		,[CreatedDate]
		,[ModifiedBy]
		,[ModifiedDate])
	VALUES
	(
		@RoleId,
		@ScreenActionId,
		@RecordStatus,
		@UserName,
		@CurrentDate,
		@UserName,
		@CurrentDate
	)
END

SELECT @RoleId = RoleId FROM Roles WHERE RoleCode = 'User'
SELECT @ScreenActionId = ScreenActionId FROM ScreenAction WHERE ActionName = 'Incomplete Deposit Log'
IF NOT EXISTS (SELECT TOP 1 1 FROM RoleAction WHERE ScreenActionId = @ScreenActionID AND RoleId = @RoleId)
BEGIN
	INSERT INTO [dbo].[RoleAction]
		([RoleId]
		,[ScreenActionId]
		,[RecordStatus]
		,[CreatedBy]
		,[CreatedDate]
		,[ModifiedBy]
		,[ModifiedDate])
	VALUES
	(
		@RoleId,
		@ScreenActionId,
		@RecordStatus,
		@UserName,
		@CurrentDate,
		@UserName,
		@CurrentDate
	)
END

SELECT @RoleId = RoleId FROM Roles WHERE RoleCode = 'Admin'
SELECT @ScreenActionId = ScreenActionId FROM ScreenAction WHERE ActionName = 'ViewClientHistory'
IF NOT EXISTS (SELECT TOP 1 1 FROM RoleAction WHERE ScreenActionId = @ScreenActionID AND RoleId = @RoleId)
BEGIN
	INSERT INTO [dbo].[RoleAction]
		([RoleId]
		,[ScreenActionId]
		,[RecordStatus]
		,[CreatedBy]
		,[CreatedDate]
		,[ModifiedBy]
		,[ModifiedDate])
	VALUES
	(
		@RoleId,
		@ScreenActionId,
		@RecordStatus,
		@UserName,
		@CurrentDate,
		@UserName,
		@CurrentDate
	)
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
/*
Script Name : 06_ClientConfigStep
Module_Name : M3Pact
Created By  : Shravani P
CreatedDate : 04/12/2018

Data Verification Script : 

SELECT * FROM [ClientConfigStep](NOLOCK) WHERE RecordStatus = 'A'
*/

BEGIN TRY
BEGIN TRANSACTION

DECLARE @UserType NVARCHAR(60) = 'Admin'
DECLARE @CurrentDate DATETIME = GETDATE()
DECLARE @RecordStatus CHAR(1) = 'A'
DECLARE @ScreenCode VARCHAR(50);

SELECT  @ScreenCode = ScreenCode FROM Screen WHERE ScreenName = 'Client Data'
IF NOT EXISTS ( SELECT TOP 1 1 FROM ClientConfigStep WHERE ClientConfigStepName = 'Client Data')
	BEGIN
		INSERT INTO ClientConfigStep
		(
			ClientConfigStepName,
			ClientConfigStepDescription,
			ScreenCode,
			DisplayOrder,
			RecordStatus,
			CreatedDate,
			CreatedBy,
			ModifiedDate,
			ModifiedBy
		)
		VALUES
		(
			'Client Data',			--	ClientConfigStepName
			'Details of a Client',	--	ClientConfigStepDescription
			@ScreenCode,			--  ScreenCode
			1,						--  DisplayOrder
			@RecordStatus,			--	RecordStatus
			@CurrentDate,			--	CreatedDate
			@UserType,				--	CreatedBy
			@CurrentDate,			--	ModifiedDate
			@UserType				--	ModifiedBy
		)
	END
ELSE
	BEGIN
		IF EXISTS( SELECT TOP 1 1 FROM ClientConfigStep WHERE ClientConfigStepName = 'Client Data' AND ScreenCode IS NULL)
			BEGIN
				UPDATE ClientConfigStep
				SET ScreenCode = @ScreenCode
				WHERE ClientConfigStepName = 'Client Data'
			END

		IF EXISTS( SELECT TOP 1 1 FROM ClientConfigStep WHERE ClientConfigStepName = 'Client Data' AND DisplayOrder IS NULL)
			BEGIN
				UPDATE ClientConfigStep
				SET DisplayOrder = 1
				WHERE ClientConfigStepName = 'Client Data'
			END
	END

SELECT  @ScreenCode = ScreenCode FROM Screen WHERE ScreenName = 'Log Setup'
IF NOT EXISTS ( SELECT TOP 1 1 FROM ClientConfigStep WHERE ClientConfigStepName = 'Log Setup')
	BEGIN
		INSERT INTO ClientConfigStep
		(
			ClientConfigStepName,
			ClientConfigStepDescription,
			ScreenCode,
			DisplayOrder,
			RecordStatus,
			CreatedDate,
			CreatedBy,
			ModifiedDate,
			ModifiedBy
		)
		VALUES
		(
			'Log Setup',					--	ClientConfigStepName
			'Assigning Payers to a Client',	--	ClientConfigStepDescription
			@ScreenCode,					--  ScreenCode
			3,								--  DisplayOrder
			@RecordStatus,					--	RecordStatus
			@CurrentDate,					--	CreatedDate
			@UserType,						--	CreatedBy
			@CurrentDate,					--	ModifiedDate
			@UserType						--	ModifiedBy
		)
	END
ELSE
	BEGIN
		IF EXISTS( SELECT TOP 1 1 FROM ClientConfigStep WHERE ClientConfigStepName = 'Log Setup' AND ScreenCode IS NULL)
			BEGIN
				UPDATE ClientConfigStep
				SET ScreenCode = @ScreenCode
				WHERE ClientConfigStepName = 'Log Setup'
			END

		IF EXISTS( SELECT TOP 1 1 FROM ClientConfigStep WHERE ClientConfigStepName = 'Log Setup' AND DisplayOrder IS NULL)
			BEGIN
				UPDATE ClientConfigStep
				SET DisplayOrder = 3
				WHERE ClientConfigStepName = 'Log Setup'
			END
	END

SELECT  @ScreenCode = ScreenCode FROM Screen WHERE ScreenName = 'Assign User'
IF NOT EXISTS ( SELECT TOP 1 1 FROM ClientConfigStep WHERE ClientConfigStepName = 'Assign User')
	BEGIN
	INSERT INTO ClientConfigStep
	(
		ClientConfigStepName,
		ClientConfigStepDescription,
		ScreenCode,
		DisplayOrder,
		RecordStatus,
		CreatedDate,
		CreatedBy,
		ModifiedDate,
		ModifiedBy
	)
	VALUES
	(
		'Assign User',					--	ClientConfigStepName
		'Assigning Users to a Client',	--	ClientConfigStepDescription
		@ScreenCode,					--  ScreenCode
		4,								--  DisplayOrder
		@RecordStatus,					--	RecordStatus
		@CurrentDate,					--	CreatedDate
		@UserType,						--	CreatedBy
		@CurrentDate,					--	ModifiedDate
		@UserType						--	ModifiedBy
	)
END
ELSE
	BEGIN
		IF EXISTS( SELECT TOP 1 1 FROM ClientConfigStep WHERE ClientConfigStepName = 'Assign User' AND ScreenCode IS NULL)
			BEGIN
				UPDATE ClientConfigStep
				SET ScreenCode = @ScreenCode
				WHERE ClientConfigStepName = 'Assign User'
			END

		IF EXISTS( SELECT TOP 1 1 FROM ClientConfigStep WHERE ClientConfigStepName = 'Assign User' AND DisplayOrder IS NULL)
			BEGIN
				UPDATE ClientConfigStep
				SET DisplayOrder = 4
				WHERE ClientConfigStepName = 'Assign User'
			END
	END

SELECT  @ScreenCode = ScreenCode FROM Screen WHERE ScreenName = 'Monthly Targets'
IF NOT EXISTS ( SELECT TOP 1 1 FROM ClientConfigStep WHERE ClientConfigStepName = 'Monthly Targets')
	BEGIN
		INSERT INTO ClientConfigStep
		(
			ClientConfigStepName,
			ClientConfigStepDescription,
			ScreenCode,
			DisplayOrder,
			RecordStatus,
			CreatedDate,
			CreatedBy,
			ModifiedDate,
			ModifiedBy
		)
		VALUES
		(
			'Monthly Targets',					--	ClientConfigStepName
			'Set Monthly Targets of a Client',	--	ClientConfigStepDescription
			@ScreenCode,						--  ScreenCode
			5,									--  DisplayOrder
			@RecordStatus,						--	RecordStatus
			@CurrentDate,						--	CreatedDate
			@UserType,							--	CreatedBy
			@CurrentDate,						--	ModifiedDate
			@UserType							--	ModifiedBy
		)
	END
ELSE
	BEGIN
		IF EXISTS( SELECT TOP 1 1 FROM ClientConfigStep WHERE ClientConfigStepName = 'Monthly Targets' AND ScreenCode IS NULL)
			BEGIN
				UPDATE ClientConfigStep
				SET ScreenCode = @ScreenCode
				WHERE ClientConfigStepName = 'Monthly Targets'
			END

		IF EXISTS( SELECT TOP 1 1 FROM ClientConfigStep WHERE ClientConfigStepName = 'Monthly Targets' AND DisplayOrder IS NULL)
			BEGIN
				UPDATE ClientConfigStep
				SET DisplayOrder = 5
				WHERE ClientConfigStepName = 'Monthly Targets'
			END
	END

SELECT  @ScreenCode = ScreenCode FROM Screen WHERE ScreenName = 'KPI Setup'
IF NOT EXISTS ( SELECT TOP 1 1 FROM ClientConfigStep WHERE ClientConfigStepName = 'KPI Setup')
	BEGIN
	INSERT INTO ClientConfigStep
	(
		ClientConfigStepName,
		ClientConfigStepDescription,
		ScreenCode,
		DisplayOrder,
		RecordStatus,
		CreatedDate,
		CreatedBy,
		ModifiedDate,
		ModifiedBy
	)
	VALUES
	(
		'KPI Setup',						--	ClientConfigStepName
		'Assign KPI to a Client',			--	ClientConfigStepDescription
		@ScreenCode,						--  ScreenCode
		2,									--  DisplayOrder
		@RecordStatus,						--	RecordStatus
		@CurrentDate,						--	CreatedDate
		@UserType,							--	CreatedBy
		@CurrentDate,						--	ModifiedDate
		@UserType							--	ModifiedBy
	)
END
ELSE
	BEGIN
		IF EXISTS( SELECT TOP 1 1 FROM ClientConfigStep WHERE ClientConfigStepName = 'KPI Setup' AND ScreenCode IS NULL)
			BEGIN
				UPDATE ClientConfigStep
				SET ScreenCode = @ScreenCode
				WHERE ClientConfigStepName = 'KPI Setup'
			END

		IF EXISTS( SELECT TOP 1 1 FROM ClientConfigStep WHERE ClientConfigStepName = 'KPI Setup' AND DisplayOrder IS NULL)
			BEGIN
				UPDATE ClientConfigStep
				SET DisplayOrder = 2
				WHERE ClientConfigStepName = 'KPI Setup'
			END
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
/*
Script Name : 17_JobStatusAndJobProcess
Module_Name : M3Pact
Created By  : Mukharjee
CreatedDate : 06/07/2018

Data Verification Script :

SELECT * FROM [jobstatus](NOLOCK) WHERE RecordStatus = 'A'
SELECT * FROM [jobprocessgroup](NOLOCK) WHERE RecordStatus = 'A'
*/

BEGIN TRY
BEGIN TRANSACTION

DECLARE @User NVARCHAR(30) = 'Admin'

INSERT INTO JOBSTATUS 
(
JobStatusCode,
JobStatusName,
RecordStatus,
CreatedDate,
CreatedBy,
ModifiedDate,
ModifiedBy
)
SELECT * FROM
(SELECT 'Completed' As JobStatusCode ,'Completed' AS JobStatusName,'A' AS RecordStatus,GETDATE() AS CreatedDate,'Admin' AS CreatedBy,GETDATE() AS ModifiedDate,'Admin' AS ModifiedBy UNION ALL
SELECT'Started' As JobStatusCode,'Started'AS JobStatusName,'A' AS RecordStatus,GETDATE() AS CreatedDate,'Admin' AS CreatedBy,GETDATE() ModifiedDate,'Admin' AS ModifiedBy UNION ALL
SELECT'Failed' As JobStatusCode,'Failed'AS JobStatusName,'A' AS RecordStatus,GETDATE() AS CreatedDate,'Admin' AS CreatedBy,GETDATE() ModifiedDate,'Admin' AS ModifiedBy UNION ALL
SELECT 'EscalationStart' As JobStatusCode,'EscalationStart'AS JobStatusName,'A' AS RecordStatus,GETDATE() AS CreatedDate,'Admin' AS CreatedBy,GETDATE() ModifiedDate,'Admin' AS ModifiedBy UNION ALL
SELECT 'EscalationEnd' As JobStatusCode,'EscalationEnd'AS JobStatusName,'A' AS RecordStatus,GETDATE() AS CreatedDate,'Admin' AS CreatedBy,GETDATE() ModifiedDate,'Admin' AS ModifiedBy
) AS A
WHERE NOT EXISTS( SELECT 1 FROM JOBSTATUS (NOLOCK) B WHERE B.JobStatusCode=A.JobStatusCode) 



INSERT INTO JOBPROCESSGROUP
(

ProcessGroupCode,
ProcessGroupName,
RecordStatus,
CreatedDate,
CreatedBy,
ModifiedDate,
ModifiedBy
)
SELECT * FROM
(SELECT 'AlertAndEscaltion' AS ProcessGroupCode ,'Alert And Escaltion' As ProcessGroupName,'A' As RecordStatus ,GETDATE() AS CreatedDate,'Admin' As CreatedBy,GETDATE() AS ModifiedDate,'Admin' AS ModifiedBy )
 AS A
 WHERE NOT EXISTS( SELECT 1 FROM JOBPROCESSGROUP (NOLOCK) B WHERE B.ProcessGroupCode=A.ProcessGroupCode) 


COMMIT TRANSACTION
END TRY

BEGIN CATCH
	SELECT ERROR_MESSAGE()
		,ERROR_LINE()
		,ERROR_SEVERITY()

	ROLLBACK TRANSACTION
END CATCH
GO

/*
Script Name : 18_ChecklistAttribute
Module_Name : M3Pact
Created By  : Shravani P
CreatedDate : 05/17/2018

Data Verification Script :

SELECT * FROM [ChecklistAttribute](NOLOCK) WHERE RecordStatus = 'A'
*/

BEGIN TRY
BEGIN TRANSACTION

DECLARE @User NVARCHAR(30) = 'Admin'

INSERT INTO [dbo].[ChecklistAttribute]
(
	[AttributeCode],
	[AttributeName],
	[RecordStatus],
	[CreatedDate],
	[CreatedBy],
	[ModifiedDate],
	[ModifiedBy]
)
SELECT 
	A.AttributeCode,
	A.AttributeName,
	'A' AS [RecordStatus],
	GETDATE() AS [CreatedDate],
	@User AS [CreatedBy],
	GETDATE() AS [ModifiedDate],
	@User AS [ModifiedBy]
FROM
(
	SELECT 'SITE' AS AttributeCode, 'Site' AS AttributeName UNION ALL
	SELECT 'SYS' AS AttributeCode, 'System' AS AttributeName UNION ALL
	SELECT 'QUE' AS AttributeCode, 'Question' AS AttributeName
)
AS A
WHERE NOT EXISTS( SELECT 1 FROM [ChecklistAttribute] (NOLOCK) B WHERE B.AttributeCode = A.AttributeCode) 

COMMIT TRANSACTION
END TRY

BEGIN CATCH
	SELECT ERROR_MESSAGE()
		,ERROR_LINE()
		,ERROR_SEVERITY()

	ROLLBACK TRANSACTION
END CATCH
GO

/**********************************************************************************
Team        :  M3Pact
Date        :  12-Apr-2018
Table Name	:  [dbo].[ClientConfigStepStatus]
ScriptName	:  19_ClientConfigStepStatus.sql

Data Verification Script : 

SELECT * FROM [ClientConfigStepStatus](NOLOCK) WHERE RecordStatus = 'A'

Revision History
================
Ver.   Date           	Who						Description
1.0	   12-Apr-2018		Shravani Palla			Adding Records to ClientConfigStepStatus
***********************************************************************************/

BEGIN TRY
BEGIN TRANSACTION

DECLARE @UserType NVARCHAR(60) = 'Admin'
DECLARE @CurrentDate DATETIME = GETDATE()
DECLARE @RecordStatus CHAR(1) = 'A'

IF NOT EXISTS ( SELECT TOP 1 1 FROM ClientConfigStepStatus WHERE ClientConfigStepStatusName = 'In Progress')
BEGIN
	INSERT INTO ClientConfigStepStatus
	(
		ClientConfigStepStatusID,
		ClientConfigStepStatusName,
		ClientConfigStepStatusDescription,
		RecordStatus,
		CreatedDate,
		CreatedBy,
		ModifiedDate,
		ModifiedBy
	)
	VALUES
	(
		1,
		'In Progress',							--	ClientConfigStepStatusName
		'The step is currently in progress',	--	ClientConfigStepStatusDescription
		@RecordStatus,							--	RecordStatus
		@CurrentDate,							--	CreatedDate
		@UserType,								--	CreatedBy
		@CurrentDate,							--	ModifiedDate
		@UserType								--	ModifiedBy
	)
END

IF NOT EXISTS ( SELECT TOP 1 1 FROM ClientConfigStepStatus WHERE ClientConfigStepStatusName = 'Completed')
BEGIN
	INSERT INTO ClientConfigStepStatus
	(
		ClientConfigStepStatusID,
		ClientConfigStepStatusName,
		ClientConfigStepStatusDescription,
		RecordStatus,
		CreatedDate,
		CreatedBy,
		ModifiedDate,
		ModifiedBy
	)
	VALUES
	(
		2,
		'Completed',							--	ClientConfigStepStatusName
		'The step is completed successfully',	--	ClientConfigStepStatusDescription
		@RecordStatus,							--	RecordStatus
		@CurrentDate,							--	CreatedDate
		@UserType,								--	CreatedBy
		@CurrentDate,							--	ModifiedDate
		@UserType								--	ModifiedBy
	)
END

IF NOT EXISTS ( SELECT TOP 1 1 FROM ClientConfigStepStatus WHERE ClientConfigStepStatusName = 'Failed')
BEGIN
	INSERT INTO ClientConfigStepStatus
	(
		ClientConfigStepStatusID,
		ClientConfigStepStatusName,
		ClientConfigStepStatusDescription,
		RecordStatus,
		CreatedDate,
		CreatedBy,
		ModifiedDate,
		ModifiedBy
	)
	VALUES
	(
		3,
		'Failed',								--	ClientConfigStepStatusName
		'The step is failed',					--	ClientConfigStepStatusDescription
		@RecordStatus,							--	RecordStatus
		@CurrentDate,							--	CreatedDate
		@UserType,								--	CreatedBy
		@CurrentDate,							--	ModifiedDate
		@UserType								--	ModifiedBy
	)
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







 
/**********************************************************************************
Team        :  M3Pact
Date        :  14-June-2018
Table Name	:  [dbo].[Employee]
ScriptName	:  21_Employee.sql

Data Verification Script : 

SELECT * FROM [Employee](NOLOCK) WHERE RecordStatus = 'A'

Revision History
================
Ver.   Date           	Who						Description
1.0	   14-June-2018		Abhishek Kovvuri		Adding Records to Employee
***********************************************************************************/

BEGIN TRY
BEGIN TRANSACTION

IF NOT EXISTS(SELECT TOP 1 1 FROM Employee WHERE EmployeeID = 432)
BEGIN
INSERT INTO EMPLOYEE VALUES(432, 'Anthony', 'Maiscalco', '' ,'Anthony.Maiscalco@suprema.com' , 'Accounts Payable Coordinator' , NULL , 'Corporate' , 'Sawa, Joseph', 'HOLDINGS' , '' , 'A' , 'Admin' , GETDATE() ,'Admin' , GETDATE()  );
END														

IF NOT EXISTS(SELECT TOP 1 1 FROM Employee WHERE EmployeeID = 806784)
BEGIN
INSERT INTO EMPLOYEE VALUES(806784, 'Nancy', 'Gomez', '' ,'Nancy.Gomez@suprema.com' , 'Administrative Assistant' , NULL , 'Meridian' , 'Medina, Edward', 'MASSACHUSETTS' , '' , 'A' , 'Admin' , GETDATE() ,'Admin' , GETDATE()  );
END														
														
														
IF NOT EXISTS(SELECT TOP 1 1 FROM Employee WHERE EmployeeID = 504)
BEGIN
INSERT INTO EMPLOYEE VALUES(504, 'Shirley', 'Nanci', '' ,'Shirley.Nanci@suprema.com' , 'Bookkeeper' , NULL , 'Corporate' , 'Sawa, Joseph', 'HOLDINGS' , '' , 'A' , 'Admin' , GETDATE() ,'Admin' , GETDATE()  );
END														
IF NOT EXISTS(SELECT TOP 1 1 FROM Employee WHERE EmployeeID = 275)
BEGIN
INSERT INTO EMPLOYEE VALUES(275, 'Beth', 'Fink', '' ,'Beth.Fink@suprema.com' , 'Business Analyst' , NULL , 'Precision.BI' , 'LaMarche, Maryann', 'PENNSYLVANIA' , '' , 'A' , 'Admin' , GETDATE() ,'Admin' , GETDATE()  );
END														
														
														
IF NOT EXISTS(SELECT TOP 1 1 FROM Employee WHERE EmployeeID = 41305)
BEGIN
INSERT INTO EMPLOYEE VALUES(41305, 'Jason', 'Gauthier', '' ,'Jason.Gauthier@suprema.com' , 'Claims Analyst' , NULL , 'Meridian' , 'Clark, Timothy', 'MASSACHUSETTS' , '' , 'A' , 'Admin' , GETDATE() ,'Admin' , GETDATE()  );
END														
IF NOT EXISTS(SELECT TOP 1 1 FROM Employee WHERE EmployeeID = 459)
BEGIN
INSERT INTO EMPLOYEE VALUES(459,'Stacey', 'McGuinness', '' ,'Stacey.McGuinness@suprema.com' , 'Client Implementation Billing' , NULL , 'Meridian' , 'Kleckowski, Caryn', 'CONNECTICUT' , '' , 'A' , 'Admin' , GETDATE() ,'Admin' , GETDATE()  );
END														
																											
														
IF NOT EXISTS(SELECT TOP 1 1 FROM Employee WHERE EmployeeID = 64872)
BEGIN
INSERT INTO EMPLOYEE VALUES(64872,'Constantine', 'Papasavvas', '' ,'Constantine.Papasavvas@suprema.com' , 'Clinical Process Analyst' , NULL , 'Meridian' , 'Travers, Linda', 'MASSACHUSETTS' , '' , 'A' , 'Admin' , GETDATE() ,'Admin' , GETDATE()  );
END														
IF NOT EXISTS(SELECT TOP 1 1 FROM Employee WHERE EmployeeID = 248)
BEGIN
INSERT INTO EMPLOYEE VALUES(248,'Alicia', 'Dole', '' ,'Alicia.Dole@suprema.com' , 'Coding Representative' , NULL , 'Meridian' , 'Jodway, Deborah', 'SOUTHEAST' , '' , 'A' , 'Admin' , GETDATE() ,'Admin' , GETDATE()  );
END														
IF NOT EXISTS(SELECT TOP 1 1 FROM Employee WHERE EmployeeID = 806820)
BEGIN
INSERT INTO EMPLOYEE VALUES(806820,'David', 'Boice', '' ,'David.Boice@suprema.com' , 'Coding Specialist' , NULL , 'Meridian' , 'Jodway, Deborah', 'SOUTHEAST' , '' , 'A' , 'Admin' , GETDATE() ,'Admin' , GETDATE()  );
END														
IF NOT EXISTS(SELECT TOP 1 1 FROM Employee WHERE EmployeeID = 7253)
BEGIN
INSERT INTO EMPLOYEE VALUES(7253,'Cheryl', 'Bowers', '' ,'Cheryl.Bowers@suprema.com' , 'Coding Specialist' , NULL , 'Meridian' , 'Jodway, Deborah', 'SOUTHEAST' , '' , 'A' , 'Admin' , GETDATE() ,'Admin' , GETDATE()  );
END														
IF NOT EXISTS(SELECT TOP 1 1 FROM Employee WHERE EmployeeID = 155)
BEGIN
INSERT INTO EMPLOYEE VALUES(155,'Shirley', 'Bratton', '' ,'Shirley.Bratton@suprema.com' , 'Coding Specialist' , NULL , 'Meridian' , 'McSweeney, Kristine', 'WASHINGTON' , '' , 'A' , 'Admin' , GETDATE() ,'Admin' , GETDATE()  );
END														
IF NOT EXISTS(SELECT TOP 1 1 FROM Employee WHERE EmployeeID = 7290)
BEGIN
INSERT INTO EMPLOYEE VALUES(7290,'Jill', 'Burnham', '' ,'Jill.Burnham@suprema.com' , 'Coding Specialist' , NULL , 'Meridian' , 'McSweeney, Kristine', 'WASHINGTON' , '' , 'A' , 'Admin' , GETDATE() ,'Admin' , GETDATE()  );
END														
IF NOT EXISTS(SELECT TOP 1 1 FROM Employee WHERE EmployeeID = 166)
BEGIN
INSERT INTO EMPLOYEE VALUES(166,'Nancy', 'Bustos', '' ,'Nancy.Bustos@suprema.com' , 'Coding Specialist' , NULL , 'Origin' , 'Laluz, Reyes', 'NEW JERSEY' , '' , 'A' , 'Admin' , GETDATE() ,'Admin' , GETDATE()  );
END														
IF NOT EXISTS(SELECT TOP 1 1 FROM Employee WHERE EmployeeID = 806679)
BEGIN
INSERT INTO EMPLOYEE VALUES(806679,'Steven', 'Carter', '' ,'Steven.Carter@suprema.com' , 'Coding Specialist' , NULL , 'Meridian' , 'Steele, Latosha', 'WASHINGTON' , '' , 'A' , 'Admin' , GETDATE() ,'Admin' , GETDATE()  );
END														
IF NOT EXISTS(SELECT TOP 1 1 FROM Employee WHERE EmployeeID = 806685)
BEGIN
INSERT INTO EMPLOYEE VALUES(806685,'Sujaya', 'Ghosh', '' ,'Sujaya.Ghosh@suprema.com' , 'Coding Specialist' , NULL , 'Meridian' , 'McSweeney, Kristine', 'WASHINGTON' , '' , 'A' , 'Admin' , GETDATE() ,'Admin' , GETDATE()  );
END														
IF NOT EXISTS(SELECT TOP 1 1 FROM Employee WHERE EmployeeID = 334)
BEGIN
INSERT INTO EMPLOYEE VALUES(334,'Angela', 'Hart', '' ,'Angela.Hart@suprema.com' , 'Coding Specialist' , NULL , 'Origin' , 'Inigo, Elizabeth', 'NEW JERSEY' , '' , 'A' , 'Admin' , GETDATE() ,'Admin' , GETDATE()  );
END														
IF NOT EXISTS(SELECT TOP 1 1 FROM Employee WHERE EmployeeID = 346)
BEGIN
INSERT INTO EMPLOYEE VALUES(346,'Jill', 'Holbrook', '' ,'Jill.Holbrook@suprema.com' , 'Coding Specialist' , NULL , 'Meridian' , 'Jodway, Deborah', 'SOUTHEAST' , '' , 'A' , 'Admin' , GETDATE() ,'Admin' , GETDATE()  );
END														
IF NOT EXISTS(SELECT TOP 1 1 FROM Employee WHERE EmployeeID = 400)
BEGIN
INSERT INTO EMPLOYEE VALUES(400,'Jerry', 'Labutong', '' ,'Jerry.Labutong@suprema.com' , 'Coding Specialist' , NULL , 'Origin' , 'Laluz, Reyes', 'NEW JERSEY' , '' , 'A' , 'Admin' , GETDATE() ,'Admin' , GETDATE()  );
END														
IF NOT EXISTS(SELECT TOP 1 1 FROM Employee WHERE EmployeeID = 130021)
BEGIN
INSERT INTO EMPLOYEE VALUES(130021,'Mariana', 'Lalloz', '' ,'Mariana.Lalloz@suprema.com' , 'Coding Specialist' , NULL , 'Meridian' , 'Rustic-Ragno, Kris', 'MASSACHUSETTS' , '' , 'A' , 'Admin' , GETDATE() ,'Admin' , GETDATE()  );
END														
IF NOT EXISTS(SELECT TOP 1 1 FROM Employee WHERE EmployeeID = 763)
BEGIN
INSERT INTO EMPLOYEE VALUES(763,'Ellie-Ann', 'Marchese', '' ,'Ellie-Ann.Marchese@suprema.com' , 'Coding Specialist' , NULL , 'Origin' , 'Harmon, Jacqueline', 'NEW JERSEY' , '' , 'A' , 'Admin' , GETDATE() ,'Admin' , GETDATE()  );
END														
IF NOT EXISTS(SELECT TOP 1 1 FROM Employee WHERE EmployeeID = 479)
BEGIN
INSERT INTO EMPLOYEE VALUES(479,'Jayshree', 'Mistry', '' ,'Jayshree.Mistry@suprema.com' , 'Coding Specialist' , NULL , 'Origin' , 'Deluco, Leslie', 'NEW JERSEY' , '' , 'A' , 'Admin' , GETDATE() ,'Admin' , GETDATE()  );
END														
IF NOT EXISTS(SELECT TOP 1 1 FROM Employee WHERE EmployeeID = 7292)
BEGIN
INSERT INTO EMPLOYEE VALUES(7292,'Malgorzata', 'Olbrycht', '' ,'Malgorzata.Olbrycht@suprema.com' , 'Coding Specialist' , NULL , 'Meridian' , 'Rustic-Ragno, Kris', 'CONNECTICUT' , '' , 'A' , 'Admin' , GETDATE() ,'Admin' , GETDATE()  );
END														
IF NOT EXISTS(SELECT TOP 1 1 FROM Employee WHERE EmployeeID = 806716)
BEGIN
INSERT INTO EMPLOYEE VALUES(806716,'Christie', 'Plumb', '' ,'Christie.Plumb@suprema.com' , 'Coding Specialist' , NULL , 'Meridian' , 'Steele, Latosha', 'WASHINGTON' , '' , 'A' , 'Admin' , GETDATE() ,'Admin' , GETDATE()  );
END														
IF NOT EXISTS(SELECT TOP 1 1 FROM Employee WHERE EmployeeID = 569)
BEGIN
INSERT INTO EMPLOYEE VALUES(569,'Doreen', 'Ramos', '' ,'Doreen.Ramos@suprema.com' , 'Coding Specialist' , NULL , 'Meridian' , 'Rustic-Ragno, Kris', 'CONNECTICUT' , '' , 'A' , 'Admin' , GETDATE() ,'Admin' , GETDATE()  );
END														
IF NOT EXISTS(SELECT TOP 1 1 FROM Employee WHERE EmployeeID = 600)
BEGIN
INSERT INTO EMPLOYEE VALUES(600,'Donna', 'Rose', '' ,'Donna.Rose@suprema.com' , 'Coding Specialist' , NULL , 'Origin' , 'Laluz, Reyes', 'NEW JERSEY' , '' , 'A' , 'Admin' , GETDATE() ,'Admin' , GETDATE()  );
END														
IF NOT EXISTS(SELECT TOP 1 1 FROM Employee WHERE EmployeeID = 605)
BEGIN
INSERT INTO EMPLOYEE VALUES(605,'Kris', 'Rustic-Ragno', '' ,'Kris.Rustic-Ragno@suprema.com' , 'Coding Supervisor' , NULL , 'Meridian' , 'Ricci, Debbie', 'CONNECTICUT' , '' , 'A' , 'Admin' , GETDATE() ,'Admin' , GETDATE()  );
END														
IF NOT EXISTS(SELECT TOP 1 1 FROM Employee WHERE EmployeeID = 806805)
BEGIN
INSERT INTO EMPLOYEE VALUES(806805,'Evelynn', 'Kelley', '' ,'Evelynn.Kelley@suprema.com' , 'Coding Team Lead' , NULL , 'Meridian' , 'Jodway, Deborah', 'SOUTHEAST' , '' , 'A' , 'Admin' , GETDATE() ,'Admin' , GETDATE()  );
END														
IF NOT EXISTS(SELECT TOP 1 1 FROM Employee WHERE EmployeeID = 806682)
BEGIN
INSERT INTO EMPLOYEE VALUES(806682,'Deena', 'McIntyre', '' ,'Deena.McIntyre@suprema.com' , 'Coding Team Lead' , NULL , 'Meridian' , 'McSweeney, Kristine', 'WASHINGTON' , '' , 'A' , 'Admin' , GETDATE() ,'Admin' , GETDATE()  );
END														
IF NOT EXISTS(SELECT TOP 1 1 FROM Employee WHERE EmployeeID = 806733)
BEGIN
INSERT INTO EMPLOYEE VALUES(806733,'Prabha', 'Sivaratnam', '' ,'Prabha.Sivaratnam@suprema.com' , 'Coding Team Lead' , NULL , 'Meridian' , 'Steele, Latosha', 'WASHINGTON' , '' , 'A' , 'Admin' , GETDATE() ,'Admin' , GETDATE()  );
END														
														
IF NOT EXISTS(SELECT TOP 1 1 FROM Employee WHERE EmployeeID = 7342)
BEGIN
INSERT INTO EMPLOYEE VALUES(7342,'Helen', 'Lee', '' ,'Helen.Lee@suprema.com' , 'Contract Admin' , NULL , 'Origin' , 'Mercer, Joseph', 'NEW JERSEY' , '' , 'A' , 'Admin' , GETDATE() ,'Admin' , GETDATE()  );
END														
IF NOT EXISTS(SELECT TOP 1 1 FROM Employee WHERE EmployeeID = 806811)
BEGIN
INSERT INTO EMPLOYEE VALUES(806811,'Elizabeth', 'Picardo', '' ,'Elizabeth.Picardo@suprema.com' , 'Credentialing Coordinator-Morris' , NULL , 'Origin' , 'Walker, Pamela', 'NEW JERSEY' , '' , 'A' , 'Admin' , GETDATE() ,'Admin' , GETDATE()  );
END														
IF NOT EXISTS(SELECT TOP 1 1 FROM Employee WHERE EmployeeID = 103)
BEGIN
INSERT INTO EMPLOYEE VALUES(103,'Tania', 'Abreu', '' ,'Tania.Abreu@suprema.com' , 'Credentialing Manager' , NULL , 'Origin' , 'Harmon, Jacqueline', 'NEW JERSEY' , '' , 'A' , 'Admin' , GETDATE() ,'Admin' , GETDATE()  );
END														
IF NOT EXISTS(SELECT TOP 1 1 FROM Employee WHERE EmployeeID = 227)
BEGIN
INSERT INTO EMPLOYEE VALUES(227,'Ann', 'DeAnni', '' ,'Ann.DeAnni@suprema.com' , 'Credentialing Specialist' , NULL , 'Origin' , 'Abreu, Tania', 'NEW JERSEY' , '' , 'A' , 'Admin' , GETDATE() ,'Admin' , GETDATE()  );
END														
IF NOT EXISTS(SELECT TOP 1 1 FROM Employee WHERE EmployeeID = 806772)
BEGIN
INSERT INTO EMPLOYEE VALUES(806772,'Debra', 'McNerney', '' ,'Debra.McNerney@suprema.com' , 'Credentialing Specialist' , NULL , 'Origin' , 'Abreu, Tania', 'NEW JERSEY' , '' , 'A' , 'Admin' , GETDATE() ,'Admin' , GETDATE()  );
END														
IF NOT EXISTS(SELECT TOP 1 1 FROM Employee WHERE EmployeeID = 901)
BEGIN
INSERT INTO EMPLOYEE VALUES(901,'Laura', 'Roberts', '' ,'Laura.Roberts@suprema.com' , 'Credentialing Specialist' , NULL , 'Meridian' , 'Jodway, Deborah', 'CONNECTICUT' , '' , 'A' , 'Admin' , GETDATE() ,'Admin' , GETDATE()  );
END														
IF NOT EXISTS(SELECT TOP 1 1 FROM Employee WHERE EmployeeID = 684)
BEGIN
INSERT INTO EMPLOYEE VALUES(684,'Jamie', 'Terrero', '' ,'Jamie.Terrero@suprema.com' , 'Credentialing Specialist' , NULL , 'Origin' , 'Abreu, Tania', 'NEW JERSEY' , '' , 'A' , 'Admin' , GETDATE() ,'Admin' , GETDATE()  );
END														
IF NOT EXISTS(SELECT TOP 1 1 FROM Employee WHERE EmployeeID = 806735)
BEGIN
INSERT INTO EMPLOYEE VALUES(806735,'Ricardo', 'Agcamaran', '' ,'Ricardo.Agcamaran@suprema.com' , 'Credit Balance  Representative' , NULL , 'Meridian' , 'Miller, Beverly', 'WASHINGTON' , '' , 'A' , 'Admin' , GETDATE() ,'Admin' , GETDATE()  );
END														
IF NOT EXISTS(SELECT TOP 1 1 FROM Employee WHERE EmployeeID = 806714)
BEGIN
INSERT INTO EMPLOYEE VALUES(806714,'Leslie', 'Chung', '' ,'Leslie.Chung@suprema.com' , 'Credit Balance  Representative' , NULL , 'Meridian' , 'Miller, Beverly', 'WASHINGTON' , '' , 'A' , 'Admin' , GETDATE() ,'Admin' , GETDATE()  );
END														
IF NOT EXISTS(SELECT TOP 1 1 FROM Employee WHERE EmployeeID = 41354)
BEGIN
INSERT INTO EMPLOYEE VALUES(41354,'Jessica', 'Gadbois', '' ,'Jessica.Gadbois@suprema.com' , 'Credit Balance  Representative' , NULL , 'Meridian' , 'Figueroa, Lizzie', 'MASSACHUSETTS' , '' , 'A' , 'Admin' , GETDATE() ,'Admin' , GETDATE()  );
END														
IF NOT EXISTS(SELECT TOP 1 1 FROM Employee WHERE EmployeeID = 192875)
BEGIN
INSERT INTO EMPLOYEE VALUES(192875,'Peter', 'Gauthier', '' ,'Peter.Gauthier@suprema.com' , 'Credit Balance  Representative' , NULL , 'Meridian' , 'Figueroa, Lizzie', 'MASSACHUSETTS' , '' , 'A' , 'Admin' , GETDATE() ,'Admin' , GETDATE()  );
END														
IF NOT EXISTS(SELECT TOP 1 1 FROM Employee WHERE EmployeeID = 806730)
BEGIN
INSERT INTO EMPLOYEE VALUES(806730,'Joan', 'Hameister', '' ,'Joan.Hameister@suprema.com' , 'Credit Balance  Representative' , NULL , 'Meridian' , 'Miller, Beverly', 'WASHINGTON' , '' , 'A' , 'Admin' , GETDATE() ,'Admin' , GETDATE()  );
END														
IF NOT EXISTS(SELECT TOP 1 1 FROM Employee WHERE EmployeeID = 265)
BEGIN
INSERT INTO EMPLOYEE VALUES(265,'Carissa', 'Falcone', '' ,'Carissa.Falcone@suprema.com' , 'Customer Service Rep' , NULL , 'Meridian' , 'Newman, Theodore', 'CONNECTICUT' , '' , 'A' , 'Admin' , GETDATE() ,'Admin' , GETDATE()  );
END														
IF NOT EXISTS(SELECT TOP 1 1 FROM Employee WHERE EmployeeID = 7333)
BEGIN
INSERT INTO EMPLOYEE VALUES(7333,'Elizabeth', 'Nash', '' ,'Elizabeth.Nash@suprema.com' , 'Customer Service Rep' , NULL , 'Meridian' , 'Newman, Theodore', 'CONNECTICUT' , '' , 'A' , 'Admin' , GETDATE() ,'Admin' , GETDATE()  );
END														
IF NOT EXISTS(SELECT TOP 1 1 FROM Employee WHERE EmployeeID = 611)
BEGIN
INSERT INTO EMPLOYEE VALUES(611,'Nicole', 'Salvatore', '' ,'Nicole.Salvatore@suprema.com' , 'Customer Service Rep' , NULL , 'Meridian' , 'Hull, Heather', 'CONNECTICUT' , '' , 'A' , 'Admin' , GETDATE() ,'Admin' , GETDATE()  );
END														
IF NOT EXISTS(SELECT TOP 1 1 FROM Employee WHERE EmployeeID = 696)
BEGIN
INSERT INTO EMPLOYEE VALUES(696,'Victoria', 'Treat', '' ,'Victoria.Treat@suprema.com' , 'Customer Service Rep' , NULL , 'Meridian' , 'Newman, Theodore', 'CONNECTICUT' , '' , 'A' , 'Admin' , GETDATE() ,'Admin' , GETDATE()  );
END														
IF NOT EXISTS(SELECT TOP 1 1 FROM Employee WHERE EmployeeID = 709)
BEGIN
INSERT INTO EMPLOYEE VALUES(709,'Mary', 'Velazquez', '' ,'Mary.Velazquez@suprema.com' , 'Customer Support Analyst' , NULL , 'Meridian' , 'Newman, Theodore', 'CONNECTICUT' , '' , 'A' , 'Admin' , GETDATE() ,'Admin' , GETDATE()  );
END														
IF NOT EXISTS(SELECT TOP 1 1 FROM Employee WHERE EmployeeID = 388)
BEGIN
INSERT INTO EMPLOYEE VALUES(388,'Caryn', 'Kleckowski', '' ,'Caryn.Kleckowski@suprema.com' , 'DIR PROJECT MGMT&CLIENT IMPL' , NULL , 'Robotics' , 'Perrotta, Angelo', 'Growth' , '' , 'A' , 'Admin' , GETDATE() ,'Admin' , GETDATE()  );
END														
IF NOT EXISTS(SELECT TOP 1 1 FROM Employee WHERE EmployeeID = 7308)
BEGIN
INSERT INTO EMPLOYEE VALUES(7308,'Robin', 'Pickerill', '' ,'Robin.Pickerill@suprema.com' , 'Dir. of Business Development' , NULL , 'Corporate' , 'Stone, William', 'HOLDINGS' , '' , 'A' , 'Admin' , GETDATE() ,'Admin' , GETDATE()  );
END														
IF NOT EXISTS(SELECT TOP 1 1 FROM Employee WHERE EmployeeID = 637)
BEGIN
INSERT INTO EMPLOYEE VALUES(637,'Judith', 'Sheldone', '' ,'Judith.Sheldone@suprema.com' , 'Director Inscribe Services' , NULL , 'Meridian' , 'Travers, Linda', 'SOUTHEAST' , '' , 'A' , 'Admin' , GETDATE() ,'Admin' , GETDATE()  );
END														
														
IF NOT EXISTS(SELECT TOP 1 1 FROM Employee WHERE EmployeeID = 712)
BEGIN
INSERT INTO EMPLOYEE VALUES(712,'Michael', 'Vesce', '' ,'Michael.Vesce@suprema.com' , 'Director of IT' , NULL , 'Meridian' , 'Nagel, Donavan', 'CONNECTICUT' , '' , 'A' , 'Admin' , GETDATE() ,'Admin' , GETDATE()  );
END														
IF NOT EXISTS(SELECT TOP 1 1 FROM Employee WHERE EmployeeID = 40933)
BEGIN
INSERT INTO EMPLOYEE VALUES(40933,'Timothy', 'Clark', '' ,'Timothy.Clark@suprema.com' , 'Director, Application Support' , NULL , 'Meridian' , 'Gontarek, Robert', 'MASSACHUSETTS' , '' , 'A' , 'Admin' , GETDATE() ,'Admin' , GETDATE()  );
END														
IF NOT EXISTS(SELECT TOP 1 1 FROM Employee WHERE EmployeeID = 59406)
BEGIN
INSERT INTO EMPLOYEE VALUES(59406,'Linda', 'Travers', '' ,'Linda.Travers@suprema.com' , 'Director, Clinical Technology' , NULL , 'Meridian' , 'Gontarek, Robert', 'MASSACHUSETTS' , '' , 'A' , 'Admin' , GETDATE() ,'Admin' , GETDATE()  );
END														
														
IF NOT EXISTS(SELECT TOP 1 1 FROM Employee WHERE EmployeeID = 759)
BEGIN
INSERT INTO EMPLOYEE VALUES(759,'Michael', 'Rowley', '' ,'Michael.Rowley@suprema.com' , 'Director, Outcomes' , NULL , 'Precision.BI' , 'Abraham, Lance', 'PENNSYLVANIA' , '' , 'A' , 'Admin' , GETDATE() ,'Admin' , GETDATE()  );
END														
IF NOT EXISTS(SELECT TOP 1 1 FROM Employee WHERE EmployeeID = 63051)
BEGIN
INSERT INTO EMPLOYEE VALUES(63051,'Donavan', 'Nagel', '' ,'Donavan.Nagel@suprema.com' , 'Director, Programming' , NULL , 'Meridian' , 'Medina, Edward', 'MASSACHUSETTS' , '' , 'A' , 'Admin' , GETDATE() ,'Admin' , GETDATE()  );
END														
IF NOT EXISTS(SELECT TOP 1 1 FROM Employee WHERE EmployeeID = 7278)
BEGIN
INSERT INTO EMPLOYEE VALUES(7278,'Maryann', 'LaMarche', '' ,'Maryann.LaMarche@suprema.com' , 'Director, Solutions Management' , NULL , 'Precision.BI' , 'Abraham, Lance', 'PENNSYLVANIA' , '' , 'A' , 'Admin' , GETDATE() ,'Admin' , GETDATE()  );
END														
IF NOT EXISTS(SELECT TOP 1 1 FROM Employee WHERE EmployeeID = 7338)
BEGIN
INSERT INTO EMPLOYEE VALUES(7338,'Maria', 'Cerqua', '' ,'Maria.Cerqua@suprema.com' , 'Document Management Representative' , NULL , 'Origin' , 'Chin-Hydzik, Caroline', 'NEW JERSEY' , '' , 'A' , 'Admin' , GETDATE() ,'Admin' , GETDATE()  );
END														
IF NOT EXISTS(SELECT TOP 1 1 FROM Employee WHERE EmployeeID = 384)
BEGIN
INSERT INTO EMPLOYEE VALUES(384,'Kathleen', 'Kirby', '' ,'Kathleen.Kirby@suprema.com' , 'Document Management Representative' , NULL , 'Meridian' , 'Figueroa, Lizzie', 'CONNECTICUT' , '' , 'A' , 'Admin' , GETDATE() ,'Admin' , GETDATE()  );
END														
IF NOT EXISTS(SELECT TOP 1 1 FROM Employee WHERE EmployeeID = 397)
BEGIN
INSERT INTO EMPLOYEE VALUES(397,'Jacqueline', 'Kulzy-Gauthier', '' ,'Jacqueline.Kulzy-Gauthier@suprema.com' , 'Document Management Representative' , NULL , 'Origin' , 'McFadden, Demond', 'NEW JERSEY' , '' , 'A' , 'Admin' , GETDATE() ,'Admin' , GETDATE()  );
END														
IF NOT EXISTS(SELECT TOP 1 1 FROM Employee WHERE EmployeeID = 487)
BEGIN
INSERT INTO EMPLOYEE VALUES(487,'Kent', 'Moore', '' ,'Kent.Moore@suprema.com' , 'Document Management Representative' , NULL , 'Origin' , 'Elliott-Dolor, Agnella', 'NEW JERSEY' , '' , 'A' , 'Admin' , GETDATE() ,'Admin' , GETDATE()  );
END														
IF NOT EXISTS(SELECT TOP 1 1 FROM Employee WHERE EmployeeID = 501)
BEGIN
INSERT INTO EMPLOYEE VALUES(501,'Karen', 'Mundell', '' ,'Karen.Mundell@suprema.com' , 'Document Management Representative' , NULL , 'Meridian' , 'Figueroa, Lizzie', 'CONNECTICUT' , '' , 'A' , 'Admin' , GETDATE() ,'Admin' , GETDATE()  );
END														
IF NOT EXISTS(SELECT TOP 1 1 FROM Employee WHERE EmployeeID = 641)
BEGIN
INSERT INTO EMPLOYEE VALUES(641,'Ellen', 'Silkowski', '' ,'Ellen.Silkowski@suprema.com' , 'Document Management Representative' , NULL , 'Origin' , 'Rodriguez, Luz', 'NEW JERSEY' , '' , 'A' , 'Admin' , GETDATE() ,'Admin' , GETDATE()  );
END														
IF NOT EXISTS(SELECT TOP 1 1 FROM Employee WHERE EmployeeID = 130009)
BEGIN
INSERT INTO EMPLOYEE VALUES(130009,'Elizabeth', 'Dente', '' ,'Elizabeth.Dente@suprema.com' , 'EDI  Representative' , NULL , 'Meridian' , 'Germain, Lori', 'MASSACHUSETTS' , '' , 'A' , 'Admin' , GETDATE() ,'Admin' , GETDATE()  );
END														
IF NOT EXISTS(SELECT TOP 1 1 FROM Employee WHERE EmployeeID = 806649)
BEGIN
INSERT INTO EMPLOYEE VALUES(806649,'Irmaris', 'Gonzalez', '' ,'Irmaris.Gonzalez@suprema.com' , 'EDI  Representative' , NULL , 'Meridian' , 'Figueroa, Lizzie', 'MASSACHUSETTS' , '' , 'A' , 'Admin' , GETDATE() ,'Admin' , GETDATE()  );
END														
IF NOT EXISTS(SELECT TOP 1 1 FROM Employee WHERE EmployeeID = 41314)
BEGIN
INSERT INTO EMPLOYEE VALUES(41314,'Glen', 'Goodwin', '' ,'Glen.Goodwin@suprema.com' , 'EDI Analyst' , NULL , 'Meridian' , 'Carreira, Alda', 'CONNECTICUT' , '' , 'A' , 'Admin' , GETDATE() ,'Admin' , GETDATE()  );
END														
IF NOT EXISTS(SELECT TOP 1 1 FROM Employee WHERE EmployeeID = 835)
BEGIN
INSERT INTO EMPLOYEE VALUES(835,'Tanisha', 'Byer', '' ,'Tanisha.Byer@suprema.com' , 'EDI Specialist' , NULL , 'Meridian' , 'Jodway, Deborah', 'SOUTHEAST' , '' , 'A' , 'Admin' , GETDATE() ,'Admin' , GETDATE()  );
END														
IF NOT EXISTS(SELECT TOP 1 1 FROM Employee WHERE EmployeeID = 799)
BEGIN
INSERT INTO EMPLOYEE VALUES(799,'Christina', 'Gustafson', '' ,'Christina.Gustafson@suprema.com' , 'EDI Specialist' , NULL , 'Meridian' , 'Jodway, Deborah', 'SOUTHEAST' , '' , 'A' , 'Admin' , GETDATE() ,'Admin' , GETDATE()  );
END														
IF NOT EXISTS(SELECT TOP 1 1 FROM Employee WHERE EmployeeID = 796)
BEGIN
INSERT INTO EMPLOYEE VALUES(796,'Juanita', 'Freeman', '' ,'Juanita.Freeman@suprema.com' , 'EDI Team Lead' , NULL , 'Meridian' , 'Jodway, Deborah', 'SOUTHEAST' , '' , 'A' , 'Admin' , GETDATE() ,'Admin' , GETDATE()  );
END														
IF NOT EXISTS(SELECT TOP 1 1 FROM Employee WHERE EmployeeID = 1200)
BEGIN
INSERT INTO EMPLOYEE VALUES(1200,'Kevin', 'Spearnock', '' ,'Kevin.Spearnock@suprema.com' , 'EMR ANALYST' , NULL , 'Meridian' , 'Travers, Linda', 'MASSACHUSETTS' , '' , 'A' , 'Admin' , GETDATE() ,'Admin' , GETDATE()  );
END														
IF NOT EXISTS(SELECT TOP 1 1 FROM Employee WHERE EmployeeID = 806620)
BEGIN
INSERT INTO EMPLOYEE VALUES(806620,'Kristen', 'Lane', '' ,'Kristen.Lane@suprema.com' , 'EMR Document Mgmt Supervisor' , NULL , 'Meridian' , 'Travers, Linda', 'MASSACHUSETTS' , '' , 'A' , 'Admin' , GETDATE() ,'Admin' , GETDATE()  );
END														
IF NOT EXISTS(SELECT TOP 1 1 FROM Employee WHERE EmployeeID = 806622)
BEGIN
INSERT INTO EMPLOYEE VALUES(806622,'Farah', 'Khalid', '' ,'Farah.Khalid@suprema.com' , 'EMR Index Lead' , NULL , 'Meridian' , 'Lane, Kristen', 'MASSACHUSETTS' , '' , 'A' , 'Admin' , GETDATE() ,'Admin' , GETDATE()  );
END														
IF NOT EXISTS(SELECT TOP 1 1 FROM Employee WHERE EmployeeID = 806830)
BEGIN
INSERT INTO EMPLOYEE VALUES(806830,'Sharon ', 'Barger', '' ,'Sharon .Barger@suprema.com' , 'EMR Scanner' , NULL , 'Meridian' , 'Lane, Kristen', 'MASSACHUSETTS' , '' , 'A' , 'Admin' , GETDATE() ,'Admin' , GETDATE()  );
END														
IF NOT EXISTS(SELECT TOP 1 1 FROM Employee WHERE EmployeeID = 806636)
BEGIN
INSERT INTO EMPLOYEE VALUES(806636,'Susanne', 'Bellesi', '' ,'Susanne.Bellesi@suprema.com' , 'EMR Scanner' , NULL , 'Meridian' , 'Lane, Kristen', 'MASSACHUSETTS' , '' , 'A' , 'Admin' , GETDATE() ,'Admin' , GETDATE()  );
END														
IF NOT EXISTS(SELECT TOP 1 1 FROM Employee WHERE EmployeeID = 806675)
BEGIN
INSERT INTO EMPLOYEE VALUES(806675,'Patricia', 'Fundaro', '' ,'Patricia.Fundaro@suprema.com' , 'EMR Scanner' , NULL , 'Meridian' , 'Lane, Kristen', 'MASSACHUSETTS' , '' , 'A' , 'Admin' , GETDATE() ,'Admin' , GETDATE()  );
END														
IF NOT EXISTS(SELECT TOP 1 1 FROM Employee WHERE EmployeeID = 806625)
BEGIN
INSERT INTO EMPLOYEE VALUES(806625,'Jeannette', 'Paredes', '' ,'Jeannette.Paredes@suprema.com' , 'EMR Scanner' , NULL , 'Meridian' , 'Lane, Kristen', 'MASSACHUSETTS' , '' , 'A' , 'Admin' , GETDATE() ,'Admin' , GETDATE()  );
END														
IF NOT EXISTS(SELECT TOP 1 1 FROM Employee WHERE EmployeeID = 806623)
BEGIN
INSERT INTO EMPLOYEE VALUES(806623,'David', 'Sackiel', '' ,'David.Sackiel@suprema.com' , 'EMR Scanner' , NULL , 'Meridian' , 'Lane, Kristen', 'MASSACHUSETTS' , '' , 'A' , 'Admin' , GETDATE() ,'Admin' , GETDATE()  );
END														
IF NOT EXISTS(SELECT TOP 1 1 FROM Employee WHERE EmployeeID = 806798)
BEGIN
INSERT INTO EMPLOYEE VALUES(806798,'Jael ', 'Pelegrin ', '' ,'Jael .Pelegrin @suprema.com' , 'EMR Specialist' , NULL , 'Meridian' , 'Spearnock, Kevin', 'MASSACHUSETTS' , '' , 'A' , 'Admin' , GETDATE() ,'Admin' , GETDATE()  );
END														
IF NOT EXISTS(SELECT TOP 1 1 FROM Employee WHERE EmployeeID = 806631)
BEGIN
INSERT INTO EMPLOYEE VALUES(806631,'Allen', 'Porter', '' ,'Allen.Porter@suprema.com' , 'EMR Specialist' , NULL , 'Meridian' , 'Spearnock, Kevin', 'MASSACHUSETTS' , '' , 'A' , 'Admin' , GETDATE() ,'Admin' , GETDATE()  );
END														
IF NOT EXISTS(SELECT TOP 1 1 FROM Employee WHERE EmployeeID = 806617)
BEGIN
INSERT INTO EMPLOYEE VALUES(806617,'Wilfredo', 'Camara', '' ,'Wilfredo.Camara@suprema.com' , 'EMR Support Specialist' , NULL , 'Meridian' , 'Spearnock, Kevin', 'MASSACHUSETTS' , '' , 'A' , 'Admin' , GETDATE() ,'Admin' , GETDATE()  );
END														
IF NOT EXISTS(SELECT TOP 1 1 FROM Employee WHERE EmployeeID = 7235)
BEGIN
INSERT INTO EMPLOYEE VALUES(7235,'William', 'Stone', '' ,'William.Stone@suprema.com' , 'EVP Sales' , NULL , 'Meridian' , 'Gontarek, Robert', 'MASSACHUSETTS' , '' , 'A' , 'Admin' , GETDATE() ,'Admin' , GETDATE()  );
END														
														
IF NOT EXISTS(SELECT TOP 1 1 FROM Employee WHERE EmployeeID = 283)
BEGIN
INSERT INTO EMPLOYEE VALUES(283,'William', 'Foight', '' ,'William.Foight@suprema.com' , 'Healthcare Data Manager of Engineering' , NULL , 'Precision.BI' , 'Ahmad, Kaiser', 'PENNSYLVANIA' , '' , 'A' , 'Admin' , GETDATE() ,'Admin' , GETDATE()  );
END														
IF NOT EXISTS(SELECT TOP 1 1 FROM Employee WHERE EmployeeID = 7209)
BEGIN
INSERT INTO EMPLOYEE VALUES(7209,'Pamela', 'Walker', '' ,'Pamela.Walker@suprema.com' , 'Human Resources Manager' , NULL , 'Corporate' , 'Rooney, Judith', 'HOLDINGS' , '' , 'A' , 'Admin' , GETDATE() ,'Admin' , GETDATE()  );
END														
IF NOT EXISTS(SELECT TOP 1 1 FROM Employee WHERE EmployeeID = 7336)
BEGIN
INSERT INTO EMPLOYEE VALUES(7336,'Jonathan', 'Jicks', '' ,'Jonathan.Jicks@suprema.com' , 'Inside Sales' , NULL , 'Corporate' , 'Rosenberg, Jeffrey', 'HOLDINGS' , '' , 'A' , 'Admin' , GETDATE() ,'Admin' , GETDATE()  );
END														
IF NOT EXISTS(SELECT TOP 1 1 FROM Employee WHERE EmployeeID = 7273)
BEGIN
INSERT INTO EMPLOYEE VALUES(7273,'Jeffrey', 'Rosenberg', '' ,'Jeffrey.Rosenberg@suprema.com' , 'Inside Sales' , NULL , 'Corporate' , 'Stone, William', 'HOLDINGS' , '' , 'A' , 'Admin' , GETDATE() ,'Admin' , GETDATE()  );
END														
IF NOT EXISTS(SELECT TOP 1 1 FROM Employee WHERE EmployeeID = 489)
BEGIN
INSERT INTO EMPLOYEE VALUES(489,'Scarlett', 'Moran', '' ,'Scarlett.Moran@suprema.com' , 'Interface Ops' , NULL , 'Origin' , 'McFadden, Demond', 'NEW JERSEY' , '' , 'A' , 'Admin' , GETDATE() ,'Admin' , GETDATE()  );
END														
IF NOT EXISTS(SELECT TOP 1 1 FROM Employee WHERE EmployeeID = 806770)
BEGIN
INSERT INTO EMPLOYEE VALUES(806770,'Nicholas', 'Schile', '' ,'Nicholas.Schile@suprema.com' , 'IT Analyst' , NULL , 'Meridian' , 'McCallum, Jesse', 'CONNECTICUT' , '' , 'A' , 'Admin' , GETDATE() ,'Admin' , GETDATE()  );
END														
IF NOT EXISTS(SELECT TOP 1 1 FROM Employee WHERE EmployeeID = 845)
BEGIN
INSERT INTO EMPLOYEE VALUES(845,'Terrill', 'Cushnie', '' ,'Terrill.Cushnie@suprema.com' , 'It Coordinator' , NULL , 'Meridian' , 'McFadden, Demond', 'NEW JERSEY' , '' , 'A' , 'Admin' , GETDATE() ,'Admin' , GETDATE()  );
END														
IF NOT EXISTS(SELECT TOP 1 1 FROM Employee WHERE EmployeeID = 454)
BEGIN
INSERT INTO EMPLOYEE VALUES(454,'Demond', 'McFadden', '' ,'Demond.McFadden@suprema.com' , 'It Director' , NULL , 'Meridian' , 'Nagel, Donavan', 'NEW JERSEY' , '' , 'A' , 'Admin' , GETDATE() ,'Admin' , GETDATE()  );
END														
IF NOT EXISTS(SELECT TOP 1 1 FROM Employee WHERE EmployeeID = 475)
BEGIN
INSERT INTO EMPLOYEE VALUES(475,'John', 'Miller', '' ,'John.Miller@suprema.com' , 'It Manager' , NULL , 'Precision.BI' , 'Ahmad, Kaiser', 'PENNSYLVANIA' , '' , 'A' , 'Admin' , GETDATE() ,'Admin' , GETDATE()  );
END														
														
IF NOT EXISTS(SELECT TOP 1 1 FROM Employee WHERE EmployeeID = 511)
BEGIN
INSERT INTO EMPLOYEE VALUES(511,'Theodore', 'Newman', '' ,'Theodore.Newman@suprema.com' , 'Manager, Application Support & Analytics' , NULL , 'Meridian' , 'Clark, Timothy', 'CONNECTICUT' , '' , 'A' , 'Admin' , GETDATE() ,'Admin' , GETDATE()  );
END														
IF NOT EXISTS(SELECT TOP 1 1 FROM Employee WHERE EmployeeID = 806786)
BEGIN
INSERT INTO EMPLOYEE VALUES(806786,'Yesenia', 'Guerrero-Ivanovic', '' ,'Yesenia.Guerrero-Ivanovic@suprema.com' , 'Manager, Human Resources & Benefits' , NULL , 'Meridian' , 'Rooney, Judith', 'MASSACHUSETTS' , '' , 'A' , 'Admin' , GETDATE() ,'Admin' , GETDATE()  );
END														
IF NOT EXISTS(SELECT TOP 1 1 FROM Employee WHERE EmployeeID = 890)
BEGIN
INSERT INTO EMPLOYEE VALUES(890,'Leah', 'Kelly', '' ,'Leah.Kelly@suprema.com' , 'Manager, Human Resources & Payroll' , NULL , 'Corporate' , 'Rooney, Judith', 'HOLDINGS' , '' , 'A' , 'Admin' , GETDATE() ,'Admin' , GETDATE()  );
END														
IF NOT EXISTS(SELECT TOP 1 1 FROM Employee WHERE EmployeeID = 44105)
BEGIN
INSERT INTO EMPLOYEE VALUES(44105,'Amber', 'Hersma', '' ,'Amber.Hersma@suprema.com' , 'Manager, Quality Assurance/Compliance' , NULL , 'Meridian' , 'Rooney, Judith', 'MASSACHUSETTS' , '' , 'A' , 'Admin' , GETDATE() ,'Admin' , GETDATE()  );
END														
IF NOT EXISTS(SELECT TOP 1 1 FROM Employee WHERE EmployeeID = 249)
BEGIN
INSERT INTO EMPLOYEE VALUES(249,'Robert', 'Donnelly', '' ,'Robert.Donnelly@suprema.com' , 'Managing Director Anesthesia Services' , NULL , 'Origin' , 'Gontarek, Robert', 'NEW JERSEY' , '' , 'A' , 'Admin' , GETDATE() ,'Admin' , GETDATE()  );
END														
														
IF NOT EXISTS(SELECT TOP 1 1 FROM Employee WHERE EmployeeID = 806816)
BEGIN
INSERT INTO EMPLOYEE VALUES(806816,'Lashonda', 'Adams', '' ,'Lashonda.Adams@suprema.com' , 'Medical Billing Representative' , NULL , 'Origin' , 'Mayzck, Toni', 'NEW JERSEY' , '' , 'A' , 'Admin' , GETDATE() ,'Admin' , GETDATE()  );
END														
IF NOT EXISTS(SELECT TOP 1 1 FROM Employee WHERE EmployeeID = 108)
BEGIN
INSERT INTO EMPLOYEE VALUES(108,'Kimberly', 'Alexander', '' ,'Kimberly.Alexander@suprema.com' , 'Medical Billing Representative' , NULL , 'Origin' , 'Nirdlinger, Misty', 'NEW JERSEY' , '' , 'A' , 'Admin' , GETDATE() ,'Admin' , GETDATE()  );
END														
IF NOT EXISTS(SELECT TOP 1 1 FROM Employee WHERE EmployeeID = 806781)
BEGIN
INSERT INTO EMPLOYEE VALUES(806781,'Donna', 'Anastasi', '' ,'Donna.Anastasi@suprema.com' , 'Medical Billing Representative' , NULL , 'Origin' , 'Mayzck, Toni', 'NEW JERSEY' , '' , 'A' , 'Admin' , GETDATE() ,'Admin' , GETDATE()  );
END														
IF NOT EXISTS(SELECT TOP 1 1 FROM Employee WHERE EmployeeID = 144)
BEGIN
INSERT INTO EMPLOYEE VALUES(144,'Jacqueline', 'Bertoline', '' ,'Jacqueline.Bertoline@suprema.com' , 'Medical Billing Representative' , NULL , 'Origin' , 'Williams, Phillip', 'NEW JERSEY' , '' , 'A' , 'Admin' , GETDATE() ,'Admin' , GETDATE()  );
END														
IF NOT EXISTS(SELECT TOP 1 1 FROM Employee WHERE EmployeeID = 806800)
BEGIN
INSERT INTO EMPLOYEE VALUES(806800,'Ashley', 'Buono', '' ,'Ashley.Buono@suprema.com' , 'Medical Billing Representative' , NULL , 'Origin' , 'Mayzck, Toni', 'NEW JERSEY' , '' , 'A' , 'Admin' , GETDATE() ,'Admin' , GETDATE()  );
END														
IF NOT EXISTS(SELECT TOP 1 1 FROM Employee WHERE EmployeeID = 172)
BEGIN
INSERT INTO EMPLOYEE VALUES(172,'Shirley', 'Camodeo', '' ,'Shirley.Camodeo@suprema.com' , 'Medical Billing Representative' , NULL , 'Origin' , 'Nirdlinger, Misty', 'NEW JERSEY' , '' , 'A' , 'Admin' , GETDATE() ,'Admin' , GETDATE()  );
END														
IF NOT EXISTS(SELECT TOP 1 1 FROM Employee WHERE EmployeeID = 900)
BEGIN
INSERT INTO EMPLOYEE VALUES(900,'Rhyna', 'Coca', '' ,'Rhyna.Coca@suprema.com' , 'Medical Billing Representative' , NULL , 'Origin' , 'Harmon, Jacqueline', 'NEW JERSEY' , '' , 'A' , 'Admin' , GETDATE() ,'Admin' , GETDATE()  );
END														
IF NOT EXISTS(SELECT TOP 1 1 FROM Employee WHERE EmployeeID = 806779)
BEGIN
INSERT INTO EMPLOYEE VALUES(806779,'Chandan', 'Deep', '' ,'Chandan.Deep@suprema.com' , 'Medical Billing Representative' , NULL , 'Origin' , 'Mayzck, Toni', 'NEW JERSEY' , '' , 'A' , 'Admin' , GETDATE() ,'Admin' , GETDATE()  );
END														
IF NOT EXISTS(SELECT TOP 1 1 FROM Employee WHERE EmployeeID = 806688)
BEGIN
INSERT INTO EMPLOYEE VALUES(806688,'Mercy', 'Del Rosario', '' ,'Mercy.Del Rosario@suprema.com' , 'Medical Billing Representative' , NULL , 'Meridian' , 'Steele, Latosha', 'WASHINGTON' , '' , 'A' , 'Admin' , GETDATE() ,'Admin' , GETDATE()  );
END														
IF NOT EXISTS(SELECT TOP 1 1 FROM Employee WHERE EmployeeID = 41347)
BEGIN
INSERT INTO EMPLOYEE VALUES(41347,'Claudette', 'Desmarais', '' ,'Claudette.Desmarais@suprema.com' , 'Medical Billing Representative' , NULL , 'Meridian' , 'Germain, Lori', 'MASSACHUSETTS' , '' , 'A' , 'Admin' , GETDATE() ,'Admin' , GETDATE()  );
END														
IF NOT EXISTS(SELECT TOP 1 1 FROM Employee WHERE EmployeeID = 806676)
BEGIN
INSERT INTO EMPLOYEE VALUES(806676,'Jessica', 'Doble', '' ,'Jessica.Doble@suprema.com' , 'Medical Billing Representative' , NULL , 'Origin' , 'Laluz, Reyes', 'NEW JERSEY' , '' , 'A' , 'Admin' , GETDATE() ,'Admin' , GETDATE()  );
END														
IF NOT EXISTS(SELECT TOP 1 1 FROM Employee WHERE EmployeeID = 806796)
BEGIN
INSERT INTO EMPLOYEE VALUES(806796,'Rebekah', 'Elam', '' ,'Rebekah.Elam@suprema.com' , 'Medical Billing Representative' , NULL , 'Origin' , 'Tercero, Emily', 'NEW JERSEY' , '' , 'A' , 'Admin' , GETDATE() ,'Admin' , GETDATE()  );
END														
IF NOT EXISTS(SELECT TOP 1 1 FROM Employee WHERE EmployeeID = 7345)
BEGIN
INSERT INTO EMPLOYEE VALUES(7345,'Emily', 'Ferreira', '' ,'Emily.Ferreira@suprema.com' , 'Medical Billing Representative' , NULL , 'Origin' , 'Fisher, Rachel', 'NEW JERSEY' , '' , 'A' , 'Admin' , GETDATE() ,'Admin' , GETDATE()  );
END														
IF NOT EXISTS(SELECT TOP 1 1 FROM Employee WHERE EmployeeID = 271)
BEGIN
INSERT INTO EMPLOYEE VALUES(271,'Ivelise', 'Figueroa', '' ,'Ivelise.Figueroa@suprema.com' , 'Medical Billing Representative' , NULL , 'Origin' , 'Nangle, Michele', 'NEW JERSEY' , '' , 'A' , 'Admin' , GETDATE() ,'Admin' , GETDATE()  );
END														
IF NOT EXISTS(SELECT TOP 1 1 FROM Employee WHERE EmployeeID = 281)
BEGIN
INSERT INTO EMPLOYEE VALUES(281,'Merly', 'Flores', '' ,'Merly.Flores@suprema.com' , 'Medical Billing Representative' , NULL , 'Origin' , 'Nangle, Michele', 'NEW JERSEY' , '' , 'A' , 'Admin' , GETDATE() ,'Admin' , GETDATE()  );
END														
IF NOT EXISTS(SELECT TOP 1 1 FROM Employee WHERE EmployeeID = 291)
BEGIN
INSERT INTO EMPLOYEE VALUES(291,'Jainie', 'Frias', '' ,'Jainie.Frias@suprema.com' , 'Medical Billing Representative' , NULL , 'Origin' , 'Deluco, Leslie', 'NEW JERSEY' , '' , 'A' , 'Admin' , GETDATE() ,'Admin' , GETDATE()  );
END														
IF NOT EXISTS(SELECT TOP 1 1 FROM Employee WHERE EmployeeID = 295)
BEGIN
INSERT INTO EMPLOYEE VALUES(295,'Carmen', 'Galarza', '' ,'Carmen.Galarza@suprema.com' , 'Medical Billing Representative' , NULL , 'Origin' , 'Williams, Phillip', 'NEW JERSEY' , '' , 'A' , 'Admin' , GETDATE() ,'Admin' , GETDATE()  );
END														
IF NOT EXISTS(SELECT TOP 1 1 FROM Employee WHERE EmployeeID = 308)
BEGIN
INSERT INTO EMPLOYEE VALUES(308,'Marzanna', 'Glab', '' ,'Marzanna.Glab@suprema.com' , 'Medical Billing Representative' , NULL , 'Origin' , 'Williams, Phillip', 'NEW JERSEY' , '' , 'A' , 'Admin' , GETDATE() ,'Admin' , GETDATE()  );
END														
IF NOT EXISTS(SELECT TOP 1 1 FROM Employee WHERE EmployeeID = 806815)
BEGIN
INSERT INTO EMPLOYEE VALUES(806815,'Sandra', 'Gonzaga', '' ,'Sandra.Gonzaga@suprema.com' , 'Medical Billing Representative' , NULL , 'Origin' , 'Tercero, Emily', 'NEW JERSEY' , '' , 'A' , 'Admin' , GETDATE() ,'Admin' , GETDATE()  );
END														
IF NOT EXISTS(SELECT TOP 1 1 FROM Employee WHERE EmployeeID = 7317)
BEGIN
INSERT INTO EMPLOYEE VALUES(7317,'Jennifer', 'Gonzalez', '' ,'Jennifer.Gonzalez@suprema.com' , 'Medical Billing Representative' , NULL , 'Origin' , 'Williams, Phillip', 'NEW JERSEY' , '' , 'A' , 'Admin' , GETDATE() ,'Admin' , GETDATE()  );
END														
IF NOT EXISTS(SELECT TOP 1 1 FROM Employee WHERE EmployeeID = 7275)
BEGIN
INSERT INTO EMPLOYEE VALUES(7275,'Princess', 'Goodson', '' ,'Princess.Goodson@suprema.com' , 'Medical Billing Representative' , NULL , 'Origin' , 'Tercero, Emily', 'NEW JERSEY' , '' , 'A' , 'Admin' , GETDATE() ,'Admin' , GETDATE()  );
END														
IF NOT EXISTS(SELECT TOP 1 1 FROM Employee WHERE EmployeeID = 806659)
BEGIN
INSERT INTO EMPLOYEE VALUES(806659,'Daysi', 'Hattem', '' ,'Daysi.Hattem@suprema.com' , 'Medical Billing Representative' , NULL , 'Origin' , 'Tercero, Emily', 'NEW JERSEY' , '' , 'A' , 'Admin' , GETDATE() ,'Admin' , GETDATE()  );
END														
IF NOT EXISTS(SELECT TOP 1 1 FROM Employee WHERE EmployeeID = 389)
BEGIN
INSERT INTO EMPLOYEE VALUES(389,'Maureen', 'Kolesar', '' ,'Maureen.Kolesar@suprema.com' , 'Medical Billing Representative' , NULL , 'Origin' , 'Williams, Phillip', 'NEW JERSEY' , '' , 'A' , 'Admin' , GETDATE() ,'Admin' , GETDATE()  );
END														
IF NOT EXISTS(SELECT TOP 1 1 FROM Employee WHERE EmployeeID = 806757)
BEGIN
INSERT INTO EMPLOYEE VALUES(806757,'Sylvia', 'Lopez', '' ,'Sylvia.Lopez@suprema.com' , 'Medical Billing Representative' , NULL , 'Origin' , 'Williams, Phillip', 'NEW JERSEY' , '' , 'A' , 'Admin' , GETDATE() ,'Admin' , GETDATE()  );
END														
IF NOT EXISTS(SELECT TOP 1 1 FROM Employee WHERE EmployeeID = 745)
BEGIN
INSERT INTO EMPLOYEE VALUES(745,'Megan', 'Maher', '' ,'Megan.Maher@suprema.com' , 'Medical Billing Representative' , NULL , 'Origin' , 'Deluco, Leslie', 'NEW JERSEY' , '' , 'A' , 'Admin' , GETDATE() ,'Admin' , GETDATE()  );
END														
IF NOT EXISTS(SELECT TOP 1 1 FROM Employee WHERE EmployeeID = 7282)
BEGIN
INSERT INTO EMPLOYEE VALUES(7282,'Luz', 'Martinez', '' ,'Luz.Martinez@suprema.com' , 'Medical Billing Representative' , NULL , 'Origin' , 'Williams, Phillip', 'NEW JERSEY' , '' , 'A' , 'Admin' , GETDATE() ,'Admin' , GETDATE()  );
END														
IF NOT EXISTS(SELECT TOP 1 1 FROM Employee WHERE EmployeeID = 450)
BEGIN
INSERT INTO EMPLOYEE VALUES(450,'Cecilia', 'Matthes', '' ,'Cecilia.Matthes@suprema.com' , 'Medical Billing Representative' , NULL , 'Origin' , 'Rodriguez, Luz', 'NEW JERSEY' , '' , 'A' , 'Admin' , GETDATE() ,'Admin' , GETDATE()  );
END														
IF NOT EXISTS(SELECT TOP 1 1 FROM Employee WHERE EmployeeID = 465)
BEGIN
INSERT INTO EMPLOYEE VALUES(465,'Kathleen', 'Medeiros', '' ,'Kathleen.Medeiros@suprema.com' , 'Medical Billing Representative' , NULL , 'Origin' , 'Mayzck, Toni', 'NEW JERSEY' , '' , 'A' , 'Admin' , GETDATE() ,'Admin' , GETDATE()  );
END														
IF NOT EXISTS(SELECT TOP 1 1 FROM Employee WHERE EmployeeID = 481)
BEGIN
INSERT INTO EMPLOYEE VALUES(481,'Brunilda', 'Mojica', '' ,'Brunilda.Mojica@suprema.com' , 'Medical Billing Representative' , NULL , 'Origin' , 'Nirdlinger, Misty', 'NEW JERSEY' , '' , 'A' , 'Admin' , GETDATE() ,'Admin' , GETDATE()  );
END														
IF NOT EXISTS(SELECT TOP 1 1 FROM Employee WHERE EmployeeID = 486)
BEGIN
INSERT INTO EMPLOYEE VALUES(486,'Lourdes', 'Montalvo', '' ,'Lourdes.Montalvo@suprema.com' , 'Medical Billing Representative' , NULL , 'Origin' , 'Mayzck, Toni', 'NEW JERSEY' , '' , 'A' , 'Admin' , GETDATE() ,'Admin' , GETDATE()  );
END														
IF NOT EXISTS(SELECT TOP 1 1 FROM Employee WHERE EmployeeID = 7320)
BEGIN
INSERT INTO EMPLOYEE VALUES(7320,'Lesly', 'Morales', '' ,'Lesly.Morales@suprema.com' , 'Medical Billing Representative' , NULL , 'Origin' , 'Rodriguez, Luz', 'NEW JERSEY' , '' , 'A' , 'Admin' , GETDATE() ,'Admin' , GETDATE()  );
END														
IF NOT EXISTS(SELECT TOP 1 1 FROM Employee WHERE EmployeeID = 7305)
BEGIN
INSERT INTO EMPLOYEE VALUES(7305,'Debra', 'Morales-Green', '' ,'Debra.Morales-Green@suprema.com' , 'Medical Billing Representative' , NULL , 'Origin' , 'Nirdlinger, Misty', 'NEW JERSEY' , '' , 'A' , 'Admin' , GETDATE() ,'Admin' , GETDATE()  );
END														
IF NOT EXISTS(SELECT TOP 1 1 FROM Employee WHERE EmployeeID = 7287)
BEGIN
INSERT INTO EMPLOYEE VALUES(7287,'Kimberly', 'Moscoso', '' ,'Kimberly.Moscoso@suprema.com' , 'Medical Billing Representative' , NULL , 'Origin' , 'Rodriguez, Luz', 'NEW JERSEY' , '' , 'A' , 'Admin' , GETDATE() ,'Admin' , GETDATE()  );
END														
IF NOT EXISTS(SELECT TOP 1 1 FROM Employee WHERE EmployeeID = 498)
BEGIN
INSERT INTO EMPLOYEE VALUES(498,'Erik', 'Mulero', '' ,'Erik.Mulero@suprema.com' , 'Medical Billing Representative' , NULL , 'Origin' , 'Harmon, Jacqueline', 'NEW JERSEY' , '' , 'A' , 'Admin' , GETDATE() ,'Admin' , GETDATE()  );
END														
IF NOT EXISTS(SELECT TOP 1 1 FROM Employee WHERE EmployeeID = 806787)
BEGIN
INSERT INTO EMPLOYEE VALUES(806787,'Jose', 'Olivencia', '' ,'Jose.Olivencia@suprema.com' , 'Medical Billing Representative' , NULL , 'Origin' , 'Inigo, Elizabeth', 'NEW JERSEY' , '' , 'A' , 'Admin' , GETDATE() ,'Admin' , GETDATE()  );
END														
IF NOT EXISTS(SELECT TOP 1 1 FROM Employee WHERE EmployeeID = 558)
BEGIN
INSERT INTO EMPLOYEE VALUES(558,'Carmen', 'Puente', '' ,'Carmen.Puente@suprema.com' , 'Medical Billing Representative' , NULL , 'Origin' , 'Harmon, Jacqueline', 'NEW JERSEY' , '' , 'A' , 'Admin' , GETDATE() ,'Admin' , GETDATE()  );
END														
IF NOT EXISTS(SELECT TOP 1 1 FROM Employee WHERE EmployeeID = 806669)
BEGIN
INSERT INTO EMPLOYEE VALUES(806669,'Evelyn', 'Puente de la Vega', '' ,'Evelyn.Puente de la Vega@suprema.com' , 'Medical Billing Representative' , NULL , 'Origin' , 'Beck, Suzanne', 'NEW JERSEY' , '' , 'A' , 'Admin' , GETDATE() ,'Admin' , GETDATE()  );
END														
IF NOT EXISTS(SELECT TOP 1 1 FROM Employee WHERE EmployeeID = 7274)
BEGIN
INSERT INTO EMPLOYEE VALUES(7274,'Gina', 'Quevedo', '' ,'Gina.Quevedo@suprema.com' , 'Medical Billing Representative' , NULL , 'Origin' , 'Beck, Suzanne', 'NEW JERSEY' , '' , 'A' , 'Admin' , GETDATE() ,'Admin' , GETDATE()  );
END														
IF NOT EXISTS(SELECT TOP 1 1 FROM Employee WHERE EmployeeID = 806672)
BEGIN
INSERT INTO EMPLOYEE VALUES(806672,'Vanessa', 'Raxach', '' ,'Vanessa.Raxach@suprema.com' , 'Medical Billing Representative' , NULL , 'Origin' , 'Nirdlinger, Misty', 'NEW JERSEY' , '' , 'A' , 'Admin' , GETDATE() ,'Admin' , GETDATE()  );
END														
IF NOT EXISTS(SELECT TOP 1 1 FROM Employee WHERE EmployeeID = 806780)
BEGIN
INSERT INTO EMPLOYEE VALUES(806780,'Alba', 'Rodriguez', '' ,'Alba.Rodriguez@suprema.com' , 'Medical Billing Representative' , NULL , 'Origin' , 'Rodriguez, Luz', 'NEW JERSEY' , '' , 'A' , 'Admin' , GETDATE() ,'Admin' , GETDATE()  );
END														
IF NOT EXISTS(SELECT TOP 1 1 FROM Employee WHERE EmployeeID = 591)
BEGIN
INSERT INTO EMPLOYEE VALUES(591,'Carmen', 'Rodriguez', '' ,'Carmen.Rodriguez@suprema.com' , 'Medical Billing Representative' , NULL , 'Origin' , 'Mayzck, Toni', 'NEW JERSEY' , '' , 'A' , 'Admin' , GETDATE() ,'Admin' , GETDATE()  );
END														
IF NOT EXISTS(SELECT TOP 1 1 FROM Employee WHERE EmployeeID = 598)
BEGIN
INSERT INTO EMPLOYEE VALUES(598,'Jeanette', 'Roman', '' ,'Jeanette.Roman@suprema.com' , 'Medical Billing Representative' , NULL , 'Origin' , 'Harmon, Jacqueline', 'NEW JERSEY' , '' , 'A' , 'Admin' , GETDATE() ,'Admin' , GETDATE()  );
END														
IF NOT EXISTS(SELECT TOP 1 1 FROM Employee WHERE EmployeeID = 7271)
BEGIN
INSERT INTO EMPLOYEE VALUES(7271,'Valerie', 'Roman', '' ,'Valerie.Roman@suprema.com' , 'Medical Billing Representative' , NULL , 'Origin' , 'Mayzck, Toni', 'NEW JERSEY' , '' , 'A' , 'Admin' , GETDATE() ,'Admin' , GETDATE()  );
END														
IF NOT EXISTS(SELECT TOP 1 1 FROM Employee WHERE EmployeeID = 806678)
BEGIN
INSERT INTO EMPLOYEE VALUES(806678,'Jodilyn', 'Rosaldo', '' ,'Jodilyn.Rosaldo@suprema.com' , 'Medical Billing Representative' , NULL , 'Meridian' , 'Steele, Latosha', 'WASHINGTON' , '' , 'A' , 'Admin' , GETDATE() ,'Admin' , GETDATE()  );
END														
IF NOT EXISTS(SELECT TOP 1 1 FROM Employee WHERE EmployeeID = 806793)
BEGIN
INSERT INTO EMPLOYEE VALUES(806793,'Jacqueline', 'Salkeld', '' ,'Jacqueline.Salkeld@suprema.com' , 'Medical Billing Representative' , NULL , 'Origin' , 'Harmon, Jacqueline', 'NEW JERSEY' , '' , 'A' , 'Admin' , GETDATE() ,'Admin' , GETDATE()  );
END														
IF NOT EXISTS(SELECT TOP 1 1 FROM Employee WHERE EmployeeID = 614)
BEGIN
INSERT INTO EMPLOYEE VALUES(614,'Joanna', 'Sanabria', '' ,'Joanna.Sanabria@suprema.com' , 'Medical Billing Representative' , NULL , 'Origin' , 'Harmon, Jacqueline', 'NEW JERSEY' , '' , 'A' , 'Admin' , GETDATE() ,'Admin' , GETDATE()  );
END														
IF NOT EXISTS(SELECT TOP 1 1 FROM Employee WHERE EmployeeID = 615)
BEGIN
INSERT INTO EMPLOYEE VALUES(615,'Scarlett', 'Sanchez', '' ,'Scarlett.Sanchez@suprema.com' , 'Medical Billing Representative' , NULL , 'Origin' , 'Beck, Suzanne', 'NEW JERSEY' , '' , 'A' , 'Admin' , GETDATE() ,'Admin' , GETDATE()  );
END														
IF NOT EXISTS(SELECT TOP 1 1 FROM Employee WHERE EmployeeID = 792)
BEGIN
INSERT INTO EMPLOYEE VALUES(792,'Vanda', 'Santo', '' ,'Vanda.Santo@suprema.com' , 'Medical Billing Representative' , NULL , 'Origin' , 'Fabbricatore, Linda', 'NEW JERSEY' , '' , 'A' , 'Admin' , GETDATE() ,'Admin' , GETDATE()  );
END														
IF NOT EXISTS(SELECT TOP 1 1 FROM Employee WHERE EmployeeID = 806683)
BEGIN
INSERT INTO EMPLOYEE VALUES(806683,'Diane', 'Sawyer', '' ,'Diane.Sawyer@suprema.com' , 'Medical Billing Representative' , NULL , 'Meridian' , 'Steele, Latosha', 'WASHINGTON' , '' , 'A' , 'Admin' , GETDATE() ,'Admin' , GETDATE()  );
END														
IF NOT EXISTS(SELECT TOP 1 1 FROM Employee WHERE EmployeeID = 627)
BEGIN
INSERT INTO EMPLOYEE VALUES(627,'Showanda', 'Sconiers', '' ,'Showanda.Sconiers@suprema.com' , 'Medical Billing Representative' , NULL , 'Origin' , 'Mayzck, Toni', 'NEW JERSEY' , '' , 'A' , 'Admin' , GETDATE() ,'Admin' , GETDATE()  );
END														
IF NOT EXISTS(SELECT TOP 1 1 FROM Employee WHERE EmployeeID = 631)
BEGIN
INSERT INTO EMPLOYEE VALUES(631,'Melissa', 'Semidey', '' ,'Melissa.Semidey@suprema.com' , 'Medical Billing Representative' , NULL , 'Origin' , 'Nirdlinger, Misty', 'NEW JERSEY' , '' , 'A' , 'Admin' , GETDATE() ,'Admin' , GETDATE()  );
END														
IF NOT EXISTS(SELECT TOP 1 1 FROM Employee WHERE EmployeeID = 767)
BEGIN
INSERT INTO EMPLOYEE VALUES(767,'Tatiana', 'Sims', '' ,'Tatiana.Sims@suprema.com' , 'Medical Billing Representative' , NULL , 'Origin' , 'Deluco, Leslie', 'NEW JERSEY' , '' , 'A' , 'Admin' , GETDATE() ,'Admin' , GETDATE()  );
END														
IF NOT EXISTS(SELECT TOP 1 1 FROM Employee WHERE EmployeeID = 783)
BEGIN
INSERT INTO EMPLOYEE VALUES(783,'Johanna', 'Soriano', '' ,'Johanna.Soriano@suprema.com' , 'Medical Billing Representative' , NULL , 'Origin' , 'Williams, Phillip', 'NEW JERSEY' , '' , 'A' , 'Admin' , GETDATE() ,'Admin' , GETDATE()  );
END														
IF NOT EXISTS(SELECT TOP 1 1 FROM Employee WHERE EmployeeID = 806763)
BEGIN
INSERT INTO EMPLOYEE VALUES(806763,'Keishla', 'Surillo', '' ,'Keishla.Surillo@suprema.com' , 'Medical Billing Representative' , NULL , 'Origin' , 'Inigo, Elizabeth', 'NEW JERSEY' , '' , 'A' , 'Admin' , GETDATE() ,'Admin' , GETDATE()  );
END														
IF NOT EXISTS(SELECT TOP 1 1 FROM Employee WHERE EmployeeID = 674)
BEGIN
INSERT INTO EMPLOYEE VALUES(674,'Renee', 'Surma', '' ,'Renee.Surma@suprema.com' , 'Medical Billing Representative' , NULL , 'Origin' , 'Williams, Phillip', 'NEW JERSEY' , '' , 'A' , 'Admin' , GETDATE() ,'Admin' , GETDATE()  );
END														
IF NOT EXISTS(SELECT TOP 1 1 FROM Employee WHERE EmployeeID = 806761)
BEGIN
INSERT INTO EMPLOYEE VALUES(806761,'Natalie', 'Vega', '' ,'Natalie.Vega@suprema.com' , 'Medical Billing Representative' , NULL , 'Origin' , 'Fabbricatore, Linda', 'NEW JERSEY' , '' , 'A' , 'Admin' , GETDATE() ,'Admin' , GETDATE()  );
END														
IF NOT EXISTS(SELECT TOP 1 1 FROM Employee WHERE EmployeeID = 710)
BEGIN
INSERT INTO EMPLOYEE VALUES(710,'Sarai', 'Velez', '' ,'Sarai.Velez@suprema.com' , 'Medical Billing Representative' , NULL , 'Origin' , 'Rodriguez, Luz', 'NEW JERSEY' , '' , 'A' , 'Admin' , GETDATE() ,'Admin' , GETDATE()  );
END														
IF NOT EXISTS(SELECT TOP 1 1 FROM Employee WHERE EmployeeID = 806762)
BEGIN
INSERT INTO EMPLOYEE VALUES(806762,'Denise', 'Vieceli', '' ,'Denise.Vieceli@suprema.com' , 'Medical Billing Representative' , NULL , 'Origin' , 'Fisher, Rachel', 'NEW JERSEY' , '' , 'A' , 'Admin' , GETDATE() ,'Admin' , GETDATE()  );
END														
IF NOT EXISTS(SELECT TOP 1 1 FROM Employee WHERE EmployeeID = 768)
BEGIN
INSERT INTO EMPLOYEE VALUES(768,'Tashera', 'Webb', '' ,'Tashera.Webb@suprema.com' , 'Medical Billing Representative' , NULL , 'Origin' , 'Deluco, Leslie', 'NEW JERSEY' , '' , 'A' , 'Admin' , GETDATE() ,'Admin' , GETDATE()  );
END														
IF NOT EXISTS(SELECT TOP 1 1 FROM Employee WHERE EmployeeID = 731)
BEGIN
INSERT INTO EMPLOYEE VALUES(731,'Audrey', 'Wilson', '' ,'Audrey.Wilson@suprema.com' , 'Medical Billing Representative' , NULL , 'Origin' , 'Deluco, Leslie', 'NEW JERSEY' , '' , 'A' , 'Admin' , GETDATE() ,'Admin' , GETDATE()  );
END														
IF NOT EXISTS(SELECT TOP 1 1 FROM Employee WHERE EmployeeID = 806734)
BEGIN
INSERT INTO EMPLOYEE VALUES(806734,'Kristina', 'Cadelina', '' ,'Kristina.Cadelina@suprema.com' , 'Medical Billing Specialist' , NULL , 'Meridian' , 'Steele, Latosha', 'WASHINGTON' , '' , 'A' , 'Admin' , GETDATE() ,'Admin' , GETDATE()  );
END														
IF NOT EXISTS(SELECT TOP 1 1 FROM Employee WHERE EmployeeID = 806689)
BEGIN
INSERT INTO EMPLOYEE VALUES(806689,'Edward', 'Gee', '' ,'Edward.Gee@suprema.com' , 'Medical Billing Specialist' , NULL , 'Meridian' , 'Steele, Latosha', 'WASHINGTON' , '' , 'A' , 'Admin' , GETDATE() ,'Admin' , GETDATE()  );
END														
IF NOT EXISTS(SELECT TOP 1 1 FROM Employee WHERE EmployeeID = 806723)
BEGIN
INSERT INTO EMPLOYEE VALUES(806723,'Michelle', 'Huizar', '' ,'Michelle.Huizar@suprema.com' , 'Medical Billing Specialist' , NULL , 'Meridian' , 'Steele, Latosha', 'WASHINGTON' , '' , 'A' , 'Admin' , GETDATE() ,'Admin' , GETDATE()  );
END														
IF NOT EXISTS(SELECT TOP 1 1 FROM Employee WHERE EmployeeID = 806720)
BEGIN
INSERT INTO EMPLOYEE VALUES(806720,'Bernice', 'Molina', '' ,'Bernice.Molina@suprema.com' , 'Medical Billing Specialist' , NULL , 'Meridian' , 'Steele, Latosha', 'WASHINGTON' , '' , 'A' , 'Admin' , GETDATE() ,'Admin' , GETDATE()  );
END														
														
IF NOT EXISTS(SELECT TOP 1 1 FROM Employee WHERE EmployeeID = 820)
BEGIN
INSERT INTO EMPLOYEE VALUES(820,'Michelle', 'Rowley', '' ,'Michelle.Rowley@suprema.com' , 'Office Administrator' , NULL , 'Precision.BI' , 'Ahmad, Kaiser', 'PENNSYLVANIA' , '' , 'A' , 'Admin' , GETDATE() ,'Admin' , GETDATE()  );
END														
														
IF NOT EXISTS(SELECT TOP 1 1 FROM Employee WHERE EmployeeID = 200)
BEGIN
INSERT INTO EMPLOYEE VALUES(200,'Jacob', 'Cochran', '' ,'Jacob.Cochran@suprema.com' , 'Operations Supervisor' , NULL , 'Meridian' , 'Perrotta, Angelo', 'CONNECTICUT' , '' , 'A' , 'Admin' , GETDATE() ,'Admin' , GETDATE()  );
END														
														
														
IF NOT EXISTS(SELECT TOP 1 1 FROM Employee WHERE EmployeeID = 7285)
BEGIN
INSERT INTO EMPLOYEE VALUES(7285,'Lance', 'Abraham', '' ,'Lance.Abraham@suprema.com' , 'Outcomes Director, Sr' , NULL , 'Precision.BI' , 'Gontarek, Robert', 'PENNSYLVANIA' , '' , 'A' , 'Admin' , GETDATE() ,'Admin' , GETDATE()  );
END														
IF NOT EXISTS(SELECT TOP 1 1 FROM Employee WHERE EmployeeID = 66140)
BEGIN
INSERT INTO EMPLOYEE VALUES(66140,'Jodi', 'Ammons', '' ,'Jodi.Ammons@suprema.com' , 'Patient Accounting Analyst' , NULL , 'Meridian' , 'Clark, Timothy', 'MASSACHUSETTS' , '' , 'A' , 'Admin' , GETDATE() ,'Admin' , GETDATE()  );
END														
IF NOT EXISTS(SELECT TOP 1 1 FROM Employee WHERE EmployeeID = 56904)
BEGIN
INSERT INTO EMPLOYEE VALUES(56904,'Paulette', 'Collins', '' ,'Paulette.Collins@suprema.com' , 'Patient Accounting Liaison' , NULL , 'Meridian' , 'Clark, Timothy', 'MASSACHUSETTS' , '' , 'A' , 'Admin' , GETDATE() ,'Admin' , GETDATE()  );
END														
IF NOT EXISTS(SELECT TOP 1 1 FROM Employee WHERE EmployeeID = 806684)
BEGIN
INSERT INTO EMPLOYEE VALUES(806684,'Dominga', 'Acain', '' ,'Dominga.Acain@suprema.com' , 'Patient Accounting Representative' , NULL , 'Meridian' , 'McSweeney, Kristine', 'WASHINGTON' , '' , 'A' , 'Admin' , GETDATE() ,'Admin' , GETDATE()  );
END														
IF NOT EXISTS(SELECT TOP 1 1 FROM Employee WHERE EmployeeID = 806825)
BEGIN
INSERT INTO EMPLOYEE VALUES(806825,'Colleen', 'Ackerman', '' ,'Colleen.Ackerman@suprema.com' , 'Patient Accounting Representative' , NULL , 'Meridian' , 'Jodway, Deborah', 'SOUTHEAST' , '' , 'A' , 'Admin' , GETDATE() ,'Admin' , GETDATE()  );
END														
IF NOT EXISTS(SELECT TOP 1 1 FROM Employee WHERE EmployeeID = 806738)
BEGIN
INSERT INTO EMPLOYEE VALUES(806738,'Elma', 'Alcoba', '' ,'Elma.Alcoba@suprema.com' , 'Patient Accounting Representative' , NULL , 'Meridian' , 'McSweeney, Kristine', 'WASHINGTON' , '' , 'A' , 'Admin' , GETDATE() ,'Admin' , GETDATE()  );
END														
IF NOT EXISTS(SELECT TOP 1 1 FROM Employee WHERE EmployeeID = 41311)
BEGIN
INSERT INTO EMPLOYEE VALUES(41311,'Joanne', 'Barr', '' ,'Joanne.Barr@suprema.com' , 'Patient Accounting Representative' , NULL , 'Meridian' , 'Robinson, Amy', 'CONNECTICUT' , '' , 'A' , 'Admin' , GETDATE() ,'Admin' , GETDATE()  );
END														
IF NOT EXISTS(SELECT TOP 1 1 FROM Employee WHERE EmployeeID = 404)
BEGIN
INSERT INTO EMPLOYEE VALUES(404,'Beverly', 'Bartholomew', '' ,'Beverly.Bartholomew@suprema.com' , 'Patient Accounting Representative' , NULL , 'Meridian' , 'Jodway, Deborah', 'SOUTHEAST' , '' , 'A' , 'Admin' , GETDATE() ,'Admin' , GETDATE()  );
END														
IF NOT EXISTS(SELECT TOP 1 1 FROM Employee WHERE EmployeeID = 181)
BEGIN
INSERT INTO EMPLOYEE VALUES(181,'Alisha', 'Bigos', '' ,'Alisha.Bigos@suprema.com' , 'Patient Accounting Representative' , NULL , 'Meridian' , 'Robinson, Amy', 'CONNECTICUT' , '' , 'A' , 'Admin' , GETDATE() ,'Admin' , GETDATE()  );
END														
IF NOT EXISTS(SELECT TOP 1 1 FROM Employee WHERE EmployeeID = 811)
BEGIN
INSERT INTO EMPLOYEE VALUES(811,'Ryle', 'Blackshaw', '' ,'Ryle.Blackshaw@suprema.com' , 'Patient Accounting Representative' , NULL , 'Meridian' , 'Jodway, Deborah', 'SOUTHEAST' , '' , 'A' , 'Admin' , GETDATE() ,'Admin' , GETDATE()  );
END														
IF NOT EXISTS(SELECT TOP 1 1 FROM Employee WHERE EmployeeID = 148)
BEGIN
INSERT INTO EMPLOYEE VALUES(148,'Pamela', 'Blewett', '' ,'Pamela.Blewett@suprema.com' , 'Patient Accounting Representative' , NULL , 'Meridian' , 'Robinson, Amy', 'CONNECTICUT' , '' , 'A' , 'Admin' , GETDATE() ,'Admin' , GETDATE()  );
END														
IF NOT EXISTS(SELECT TOP 1 1 FROM Employee WHERE EmployeeID = 806807)
BEGIN
INSERT INTO EMPLOYEE VALUES(806807,'Nancy ', 'Bois', '' ,'Nancy .Bois@suprema.com' , 'Patient Accounting Representative' , NULL , 'Meridian' , 'Carreira, Alda', 'CONNECTICUT' , '' , 'A' , 'Admin' , GETDATE() ,'Admin' , GETDATE()  );
END														
IF NOT EXISTS(SELECT TOP 1 1 FROM Employee WHERE EmployeeID = 41317)
BEGIN
INSERT INTO EMPLOYEE VALUES(41317,'Joan', 'Bowen', '' ,'Joan.Bowen@suprema.com' , 'Patient Accounting Representative' , NULL , 'Meridian' , 'Orozco, Jessica', 'MASSACHUSETTS' , '' , 'A' , 'Admin' , GETDATE() ,'Admin' , GETDATE()  );
END														
IF NOT EXISTS(SELECT TOP 1 1 FROM Employee WHERE EmployeeID = 169)
BEGIN
INSERT INTO EMPLOYEE VALUES(169,'Deborah', 'Calano', '' ,'Deborah.Calano@suprema.com' , 'Patient Accounting Representative' , NULL , 'Meridian' , 'Orozco, Jessica', 'CONNECTICUT' , '' , 'A' , 'Admin' , GETDATE() ,'Admin' , GETDATE()  );
END														
IF NOT EXISTS(SELECT TOP 1 1 FROM Employee WHERE EmployeeID = 806660)
BEGIN
INSERT INTO EMPLOYEE VALUES(806660,'Christine', 'Christensen', '' ,'Christine.Christensen@suprema.com' , 'Patient Accounting Representative' , NULL , 'Meridian' , 'Carreira, Alda', 'CONNECTICUT' , '' , 'A' , 'Admin' , GETDATE() ,'Admin' , GETDATE()  );
END														
IF NOT EXISTS(SELECT TOP 1 1 FROM Employee WHERE EmployeeID = 806821)
BEGIN
INSERT INTO EMPLOYEE VALUES(806821,'Debra', 'Crook', '' ,'Debra.Crook@suprema.com' , 'Patient Accounting Representative' , NULL , 'Meridian' , 'Jodway, Deborah', 'SOUTHEAST' , '' , 'A' , 'Admin' , GETDATE() ,'Admin' , GETDATE()  );
END														
IF NOT EXISTS(SELECT TOP 1 1 FROM Employee WHERE EmployeeID = 806694)
BEGIN
INSERT INTO EMPLOYEE VALUES(806694,'Madelaine', 'Daligdig', '' ,'Madelaine.Daligdig@suprema.com' , 'Patient Accounting Representative' , NULL , 'Meridian' , 'Miller, Beverly', 'WASHINGTON' , '' , 'A' , 'Admin' , GETDATE() ,'Admin' , GETDATE()  );
END														
IF NOT EXISTS(SELECT TOP 1 1 FROM Employee WHERE EmployeeID = 236)
BEGIN
INSERT INTO EMPLOYEE VALUES(236,'Lisa', 'Dery', '' ,'Lisa.Dery@suprema.com' , 'Patient Accounting Representative' , NULL , 'Meridian' , 'Hull, Heather', 'CONNECTICUT' , '' , 'A' , 'Admin' , GETDATE() ,'Admin' , GETDATE()  );
END														
IF NOT EXISTS(SELECT TOP 1 1 FROM Employee WHERE EmployeeID = 253)
BEGIN
INSERT INTO EMPLOYEE VALUES(253,'Tracy', 'Drozd', '' ,'Tracy.Drozd@suprema.com' , 'Patient Accounting Representative' , NULL , 'Meridian' , 'Hull, Heather', 'CONNECTICUT' , '' , 'A' , 'Admin' , GETDATE() ,'Admin' , GETDATE()  );
END														
IF NOT EXISTS(SELECT TOP 1 1 FROM Employee WHERE EmployeeID = 315)
BEGIN
INSERT INTO EMPLOYEE VALUES(315,'Martha', 'Gridley', '' ,'Martha.Gridley@suprema.com' , 'Patient Accounting Representative' , NULL , 'Meridian' , 'Hull, Heather', 'CONNECTICUT' , '' , 'A' , 'Admin' , GETDATE() ,'Admin' , GETDATE()  );
END														
IF NOT EXISTS(SELECT TOP 1 1 FROM Employee WHERE EmployeeID = 806804)
BEGIN
INSERT INTO EMPLOYEE VALUES(806804,'Carla ', 'Griffith', '' ,'Carla .Griffith@suprema.com' , 'Patient Accounting Representative' , NULL , 'Meridian' , 'Hull, Heather', 'CONNECTICUT' , '' , 'A' , 'Admin' , GETDATE() ,'Admin' , GETDATE()  );
END														
IF NOT EXISTS(SELECT TOP 1 1 FROM Employee WHERE EmployeeID = 317)
BEGIN
INSERT INTO EMPLOYEE VALUES(317,'Robin', 'Guimond', '' ,'Robin.Guimond@suprema.com' , 'Patient Accounting Representative' , NULL , 'Meridian' , 'Robinson, Amy', 'CONNECTICUT' , '' , 'A' , 'Admin' , GETDATE() ,'Admin' , GETDATE()  );
END														
IF NOT EXISTS(SELECT TOP 1 1 FROM Employee WHERE EmployeeID = 336)
BEGIN
INSERT INTO EMPLOYEE VALUES(336,'Jennifer', 'Hayes', '' ,'Jennifer.Hayes@suprema.com' , 'Patient Accounting Representative' , NULL , 'Meridian' , 'Figueroa, Lizzie', 'CONNECTICUT' , '' , 'A' , 'Admin' , GETDATE() ,'Admin' , GETDATE()  );
END														
IF NOT EXISTS(SELECT TOP 1 1 FROM Employee WHERE EmployeeID = 57778)
BEGIN
INSERT INTO EMPLOYEE VALUES(57778,'Jessica', 'Hines', '' ,'Jessica.Hines@suprema.com' , 'Patient Accounting Representative' , NULL , 'Meridian' , 'Orozco, Jessica', 'MASSACHUSETTS' , '' , 'A' , 'Admin' , GETDATE() ,'Admin' , GETDATE()  );
END														
IF NOT EXISTS(SELECT TOP 1 1 FROM Employee WHERE EmployeeID = 806819)
BEGIN
INSERT INTO EMPLOYEE VALUES(806819,'Francie ', 'Iverson', '' ,'Francie .Iverson@suprema.com' , 'Patient Accounting Representative' , NULL , 'Meridian' , 'Jodway, Deborah', 'SOUTHEAST' , '' , 'A' , 'Admin' , GETDATE() ,'Admin' , GETDATE()  );
END														
IF NOT EXISTS(SELECT TOP 1 1 FROM Employee WHERE EmployeeID = 361)
BEGIN
INSERT INTO EMPLOYEE VALUES(361,'Amanda', 'Jean', '' ,'Amanda.Jean@suprema.com' , 'Patient Accounting Representative' , NULL , 'Meridian' , 'Hull, Heather', 'CONNECTICUT' , '' , 'A' , 'Admin' , GETDATE() ,'Admin' , GETDATE()  );
END														
IF NOT EXISTS(SELECT TOP 1 1 FROM Employee WHERE EmployeeID = 841)
BEGIN
INSERT INTO EMPLOYEE VALUES(841,'Dora', 'Kopyscinski', '' ,'Dora.Kopyscinski@suprema.com' , 'Patient Accounting Representative' , NULL , 'Meridian' , 'Hull, Heather', 'CONNECTICUT' , '' , 'A' , 'Admin' , GETDATE() ,'Admin' , GETDATE()  );
END														
IF NOT EXISTS(SELECT TOP 1 1 FROM Employee WHERE EmployeeID = 780)
BEGIN
INSERT INTO EMPLOYEE VALUES(780,'Melanie', 'Kosky', '' ,'Melanie.Kosky@suprema.com' , 'Patient Accounting Representative' , NULL , 'Meridian' , 'Hull, Heather', 'CONNECTICUT' , '' , 'A' , 'Admin' , GETDATE() ,'Admin' , GETDATE()  );
END														
IF NOT EXISTS(SELECT TOP 1 1 FROM Employee WHERE EmployeeID = 130013)
BEGIN
INSERT INTO EMPLOYEE VALUES(130013,'Delissa', 'Kraus', '' ,'Delissa.Kraus@suprema.com' , 'Patient Accounting Representative' , NULL , 'Origin' , 'Inigo, Elizabeth', 'NEW JERSEY' , '' , 'A' , 'Admin' , GETDATE() ,'Admin' , GETDATE()  );
END														
IF NOT EXISTS(SELECT TOP 1 1 FROM Employee WHERE EmployeeID = 41363)
BEGIN
INSERT INTO EMPLOYEE VALUES(41363,'Sabrina', 'Lynch', '' ,'Sabrina.Lynch@suprema.com' , 'Patient Accounting Representative' , NULL , 'Meridian' , 'Orozco, Jessica', 'MASSACHUSETTS' , '' , 'A' , 'Admin' , GETDATE() ,'Admin' , GETDATE()  );
END														
IF NOT EXISTS(SELECT TOP 1 1 FROM Employee WHERE EmployeeID = 806719)
BEGIN
INSERT INTO EMPLOYEE VALUES(806719,'Maria', 'Magno', '' ,'Maria.Magno@suprema.com' , 'Patient Accounting Representative' , NULL , 'Meridian' , 'McSweeney, Kristine', 'WASHINGTON' , '' , 'A' , 'Admin' , GETDATE() ,'Admin' , GETDATE()  );
END														
IF NOT EXISTS(SELECT TOP 1 1 FROM Employee WHERE EmployeeID = 806713)
BEGIN
INSERT INTO EMPLOYEE VALUES(806713,'Shella', 'Magsakay', '' ,'Shella.Magsakay@suprema.com' , 'Patient Accounting Representative' , NULL , 'Meridian' , 'McSweeney, Kristine', 'WASHINGTON' , '' , 'A' , 'Admin' , GETDATE() ,'Admin' , GETDATE()  );
END														
IF NOT EXISTS(SELECT TOP 1 1 FROM Employee WHERE EmployeeID = 806718)
BEGIN
INSERT INTO EMPLOYEE VALUES(806718,'Scott', 'Matheson', '' ,'Scott.Matheson@suprema.com' , 'Patient Accounting Representative' , NULL , 'Meridian' , 'McSweeney, Kristine', 'WASHINGTON' , '' , 'A' , 'Admin' , GETDATE() ,'Admin' , GETDATE()  );
END														
IF NOT EXISTS(SELECT TOP 1 1 FROM Employee WHERE EmployeeID = 7254)
BEGIN
INSERT INTO EMPLOYEE VALUES(7254,'Christine', 'Metcalf', '' ,'Christine.Metcalf@suprema.com' , 'Patient Accounting Representative' , NULL , 'Meridian' , 'Jodway, Deborah', 'SOUTHEAST' , '' , 'A' , 'Admin' , GETDATE() ,'Admin' , GETDATE()  );
END														
IF NOT EXISTS(SELECT TOP 1 1 FROM Employee WHERE EmployeeID = 806690)
BEGIN
INSERT INTO EMPLOYEE VALUES(806690,'Jennifer', 'Montoya', '' ,'Jennifer.Montoya@suprema.com' , 'Patient Accounting Representative' , NULL , 'Meridian' , 'Steele, Latosha', 'WASHINGTON' , '' , 'A' , 'Admin' , GETDATE() ,'Admin' , GETDATE()  );
END														
IF NOT EXISTS(SELECT TOP 1 1 FROM Employee WHERE EmployeeID = 806693)
BEGIN
INSERT INTO EMPLOYEE VALUES(806693,'Le-Uyen', 'Nguyen', '' ,'Le-Uyen.Nguyen@suprema.com' , 'Patient Accounting Representative' , NULL , 'Meridian' , 'McSweeney, Kristine', 'WASHINGTON' , '' , 'A' , 'Admin' , GETDATE() ,'Admin' , GETDATE()  );
END														
IF NOT EXISTS(SELECT TOP 1 1 FROM Employee WHERE EmployeeID = 7323)
BEGIN
INSERT INTO EMPLOYEE VALUES(7323,'Susan', 'Nieves', '' ,'Susan.Nieves@suprema.com' , 'Patient Accounting Representative' , NULL , 'Meridian' , 'Robinson, Amy', 'CONNECTICUT' , '' , 'A' , 'Admin' , GETDATE() ,'Admin' , GETDATE()  );
END														
IF NOT EXISTS(SELECT TOP 1 1 FROM Employee WHERE EmployeeID = 806657)
BEGIN
INSERT INTO EMPLOYEE VALUES(806657,'Vimary', 'Nieves', '' ,'Vimary.Nieves@suprema.com' , 'Patient Accounting Representative' , NULL , 'Meridian' , 'Orozco, Jessica', 'CONNECTICUT' , '' , 'A' , 'Admin' , GETDATE() ,'Admin' , GETDATE()  );
END														
IF NOT EXISTS(SELECT TOP 1 1 FROM Employee WHERE EmployeeID = 535)
BEGIN
INSERT INTO EMPLOYEE VALUES(535,'Christine', 'Pagano', '' ,'Christine.Pagano@suprema.com' , 'Patient Accounting Representative' , NULL , 'Meridian' , 'Robinson, Amy', 'CONNECTICUT' , '' , 'A' , 'Admin' , GETDATE() ,'Admin' , GETDATE()  );
END														
IF NOT EXISTS(SELECT TOP 1 1 FROM Employee WHERE EmployeeID = 599)
BEGIN
INSERT INTO EMPLOYEE VALUES(599,'Kathryn', 'Root', '' ,'Kathryn.Root@suprema.com' , 'Patient Accounting Representative' , NULL , 'Meridian' , 'Robinson, Amy', 'CONNECTICUT' , '' , 'A' , 'Admin' , GETDATE() ,'Admin' , GETDATE()  );
END														
IF NOT EXISTS(SELECT TOP 1 1 FROM Employee WHERE EmployeeID = 7233)
BEGIN
INSERT INTO EMPLOYEE VALUES(7233,'Ryan', 'Smith', '' ,'Ryan.Smith@suprema.com' , 'Patient Accounting Representative' , NULL , 'Meridian' , 'Jodway, Deborah', 'SOUTHEAST' , '' , 'A' , 'Admin' , GETDATE() ,'Admin' , GETDATE()  );
END														
IF NOT EXISTS(SELECT TOP 1 1 FROM Employee WHERE EmployeeID = 827)
BEGIN
INSERT INTO EMPLOYEE VALUES(827,'Sherry', 'Steines', '' ,'Sherry.Steines@suprema.com' , 'Patient Accounting Representative' , NULL , 'Meridian' , 'Robinson, Amy', 'CONNECTICUT' , '' , 'A' , 'Admin' , GETDATE() ,'Admin' , GETDATE()  );
END														
IF NOT EXISTS(SELECT TOP 1 1 FROM Employee WHERE EmployeeID = 7283)
BEGIN
INSERT INTO EMPLOYEE VALUES(7283,'Anne', 'Stewart', '' ,'Anne.Stewart@suprema.com' , 'Patient Accounting Representative' , NULL , 'Meridian' , 'Jodway, Deborah', 'SOUTHEAST' , '' , 'A' , 'Admin' , GETDATE() ,'Admin' , GETDATE()  );
END														
IF NOT EXISTS(SELECT TOP 1 1 FROM Employee WHERE EmployeeID = 806814)
BEGIN
INSERT INTO EMPLOYEE VALUES(806814,'Christine', 'Stover', '' ,'Christine.Stover@suprema.com' , 'Patient Accounting Representative' , NULL , 'Meridian' , 'Jodway, Deborah', 'SOUTHEAST' , '' , 'A' , 'Admin' , GETDATE() ,'Admin' , GETDATE()  );
END														
IF NOT EXISTS(SELECT TOP 1 1 FROM Employee WHERE EmployeeID = 806809)
BEGIN
INSERT INTO EMPLOYEE VALUES(806809,'Mary', 'Wemheuer', '' ,'Mary.Wemheuer@suprema.com' , 'Patient Accounting Representative' , NULL , 'Meridian' , 'Jodway, Deborah', 'SOUTHEAST' , '' , 'A' , 'Admin' , GETDATE() ,'Admin' , GETDATE()  );
END														
IF NOT EXISTS(SELECT TOP 1 1 FROM Employee WHERE EmployeeID = 806828)
BEGIN
INSERT INTO EMPLOYEE VALUES(806828,'Tara', 'Winters', '' ,'Tara.Winters@suprema.com' , 'Patient Accounting Representative' , NULL , 'Meridian' , 'Orozco, Jessica', 'MASSACHUSETTS' , '' , 'A' , 'Admin' , GETDATE() ,'Admin' , GETDATE()  );
END														
IF NOT EXISTS(SELECT TOP 1 1 FROM Employee WHERE EmployeeID = 7255)
BEGIN
INSERT INTO EMPLOYEE VALUES(7255,'Valerie', 'Toulouse', '' ,'Valerie.Toulouse@suprema.com' , 'Patient Accounting Specialist' , NULL , 'Meridian' , 'Jodway, Deborah', 'SOUTHEAST' , '' , 'A' , 'Admin' , GETDATE() ,'Admin' , GETDATE()  );
END														
IF NOT EXISTS(SELECT TOP 1 1 FROM Employee WHERE EmployeeID = 806826)
BEGIN
INSERT INTO EMPLOYEE VALUES(806826,'Kristen', 'Childress', '' ,'Kristen.Childress@suprema.com' , 'Patient Accounting Supervisor' , NULL , 'Meridian' , 'Jodway, Deborah', 'SOUTHEAST' , '' , 'A' , 'Admin' , GETDATE() ,'Admin' , GETDATE()  );
END														
IF NOT EXISTS(SELECT TOP 1 1 FROM Employee WHERE EmployeeID = 349)
BEGIN
INSERT INTO EMPLOYEE VALUES(349,'Heather', 'Hull', '' ,'Heather.Hull@suprema.com' , 'Patient Accounting Supervisor' , NULL , 'Meridian' , 'Williams, Crystal', 'CONNECTICUT' , '' , 'A' , 'Admin' , GETDATE() ,'Admin' , GETDATE()  );
END														
IF NOT EXISTS(SELECT TOP 1 1 FROM Employee WHERE EmployeeID = 806677)
BEGIN
INSERT INTO EMPLOYEE VALUES(806677,'Beverly', 'Miller', '' ,'Beverly.Miller@suprema.com' , 'Patient Accounting Supervisor' , NULL , 'Meridian' , 'McDaniel, Sidonie', 'WASHINGTON' , '' , 'A' , 'Admin' , GETDATE() ,'Admin' , GETDATE()  );
END														
IF NOT EXISTS(SELECT TOP 1 1 FROM Employee WHERE EmployeeID = 41776)
BEGIN
INSERT INTO EMPLOYEE VALUES(41776,'Jessica', 'Orozco', '' ,'Jessica.Orozco@suprema.com' , 'Patient Accounting Supervisor' , NULL , 'Meridian' , 'Williams, Crystal', 'MASSACHUSETTS' , '' , 'A' , 'Admin' , GETDATE() ,'Admin' , GETDATE()  );
END														
IF NOT EXISTS(SELECT TOP 1 1 FROM Employee WHERE EmployeeID = 41381)
BEGIN
INSERT INTO EMPLOYEE VALUES(41381,'Amy', 'Robinson', '' ,'Amy.Robinson@suprema.com' , 'Patient Accounting Supervisor' , NULL , 'Meridian' , 'Williams, Crystal', 'CONNECTICUT' , '' , 'A' , 'Admin' , GETDATE() ,'Admin' , GETDATE()  );
END														
IF NOT EXISTS(SELECT TOP 1 1 FROM Employee WHERE EmployeeID = 371)
BEGIN
INSERT INTO EMPLOYEE VALUES(371,'Kathleen', 'Johnston', '' ,'Kathleen.Johnston@suprema.com' , 'Patient Accounting Team Lead' , NULL , 'Meridian' , 'Hull, Heather', 'CONNECTICUT' , '' , 'A' , 'Admin' , GETDATE() ,'Admin' , GETDATE()  );
END														
IF NOT EXISTS(SELECT TOP 1 1 FROM Employee WHERE EmployeeID = 411)
BEGIN
INSERT INTO EMPLOYEE VALUES(411,'Mary', 'Lazarus', '' ,'Mary.Lazarus@suprema.com' , 'Patient Accounting Team Lead' , NULL , 'Meridian' , 'Robinson, Amy', 'CONNECTICUT' , '' , 'A' , 'Admin' , GETDATE() ,'Admin' , GETDATE()  );
END														
IF NOT EXISTS(SELECT TOP 1 1 FROM Employee WHERE EmployeeID = 112)
BEGIN
INSERT INTO EMPLOYEE VALUES(112,'Sophia', 'Anglin', '' ,'Sophia.Anglin@suprema.com' , 'Patient Services  Representative' , NULL , 'Meridian' , 'DiGeronimo, Corey', 'CONNECTICUT' , '' , 'A' , 'Admin' , GETDATE() ,'Admin' , GETDATE()  );
END														
IF NOT EXISTS(SELECT TOP 1 1 FROM Employee WHERE EmployeeID = 806818)
BEGIN
INSERT INTO EMPLOYEE VALUES(806818,'Shannon', 'Brodnax', '' ,'Shannon.Brodnax@suprema.com' , 'Patient Services  Representative' , NULL , 'Meridian' , 'Madison, Suprena', 'SOUTHEAST' , '' , 'A' , 'Admin' , GETDATE() ,'Admin' , GETDATE()  );
END														
IF NOT EXISTS(SELECT TOP 1 1 FROM Employee WHERE EmployeeID = 64130)
BEGIN
INSERT INTO EMPLOYEE VALUES(64130,'Diane', 'Brosseau', '' ,'Diane.Brosseau@suprema.com' , 'Patient Services  Representative' , NULL , 'Meridian' , 'DiGeronimo, Corey', 'MASSACHUSETTS' , '' , 'A' , 'Admin' , GETDATE() ,'Admin' , GETDATE()  );
END														
IF NOT EXISTS(SELECT TOP 1 1 FROM Employee WHERE EmployeeID = 806736)
BEGIN
INSERT INTO EMPLOYEE VALUES(806736,'Ricci', 'Brown', '' ,'Ricci.Brown@suprema.com' , 'Patient Services  Representative' , NULL , 'Meridian' , 'McSweeney, Kristine', 'WASHINGTON' , '' , 'A' , 'Admin' , GETDATE() ,'Admin' , GETDATE()  );
END														
IF NOT EXISTS(SELECT TOP 1 1 FROM Employee WHERE EmployeeID = 806746)
BEGIN
INSERT INTO EMPLOYEE VALUES(806746,'Rodel', 'Cuenca', '' ,'Rodel.Cuenca@suprema.com' , 'Patient Services  Representative' , NULL , 'Meridian' , 'McSweeney, Kristine', 'WASHINGTON' , '' , 'A' , 'Admin' , GETDATE() ,'Admin' , GETDATE()  );
END														
IF NOT EXISTS(SELECT TOP 1 1 FROM Employee WHERE EmployeeID = 44719)
BEGIN
INSERT INTO EMPLOYEE VALUES(44719,'Linda', 'David', '' ,'Linda.David@suprema.com' , 'Patient Services  Representative' , NULL , 'Meridian' , 'DiGeronimo, Corey', 'MASSACHUSETTS' , '' , 'A' , 'Admin' , GETDATE() ,'Admin' , GETDATE()  );
END														
IF NOT EXISTS(SELECT TOP 1 1 FROM Employee WHERE EmployeeID = 230)
BEGIN
INSERT INTO EMPLOYEE VALUES(230,'Ramona', 'Delira', '' ,'Ramona.Delira@suprema.com' , 'Patient Services  Representative' , NULL , 'Meridian' , 'Madison, Suprena', 'SOUTHEAST' , '' , 'A' , 'Admin' , GETDATE() ,'Admin' , GETDATE()  );
END														
IF NOT EXISTS(SELECT TOP 1 1 FROM Employee WHERE EmployeeID = 806721)
BEGIN
INSERT INTO EMPLOYEE VALUES(806721,'Deland', 'George-Wilson', '' ,'Deland.George-Wilson@suprema.com' , 'Patient Services  Representative' , NULL , 'Meridian' , 'McSweeney, Kristine', 'WASHINGTON' , '' , 'A' , 'Admin' , GETDATE() ,'Admin' , GETDATE()  );
END														
IF NOT EXISTS(SELECT TOP 1 1 FROM Employee WHERE EmployeeID = 806642)
BEGIN
INSERT INTO EMPLOYEE VALUES(806642,'Regina', 'Griffin', '' ,'Regina.Griffin@suprema.com' , 'Patient Services  Representative' , NULL , 'Meridian' , 'DiGeronimo, Corey', 'MASSACHUSETTS' , '' , 'A' , 'Admin' , GETDATE() ,'Admin' , GETDATE()  );
END														
IF NOT EXISTS(SELECT TOP 1 1 FROM Employee WHERE EmployeeID = 7262)
BEGIN
INSERT INTO EMPLOYEE VALUES(7262,'Tanya', 'Harris', '' ,'Tanya.Harris@suprema.com' , 'Patient Services  Representative' , NULL , 'Meridian' , 'Madison, Suprena', 'SOUTHEAST' , '' , 'A' , 'Admin' , GETDATE() ,'Admin' , GETDATE()  );
END														
IF NOT EXISTS(SELECT TOP 1 1 FROM Employee WHERE EmployeeID = 407)
BEGIN
INSERT INTO EMPLOYEE VALUES(407,'Cheryl', 'Lash', '' ,'Cheryl.Lash@suprema.com' , 'Patient Services  Representative' , NULL , 'Meridian' , 'DiGeronimo, Corey', 'CONNECTICUT' , '' , 'A' , 'Admin' , GETDATE() ,'Admin' , GETDATE()  );
END														
IF NOT EXISTS(SELECT TOP 1 1 FROM Employee WHERE EmployeeID = 806707)
BEGIN
INSERT INTO EMPLOYEE VALUES(806707,'Alyssa', 'McLuen', '' ,'Alyssa.McLuen@suprema.com' , 'Patient Services  Representative' , NULL , 'Meridian' , 'McSweeney, Kristine', 'WASHINGTON' , '' , 'A' , 'Admin' , GETDATE() ,'Admin' , GETDATE()  );
END														
IF NOT EXISTS(SELECT TOP 1 1 FROM Employee WHERE EmployeeID = 806616)
BEGIN
INSERT INTO EMPLOYEE VALUES(806616,'Scheana', 'McMillian', '' ,'Scheana.McMillian@suprema.com' , 'Patient Services  Representative' , NULL , 'Meridian' , 'DiGeronimo, Corey', 'MASSACHUSETTS' , '' , 'A' , 'Admin' , GETDATE() ,'Admin' , GETDATE()  );
END														
IF NOT EXISTS(SELECT TOP 1 1 FROM Employee WHERE EmployeeID = 806700)
BEGIN
INSERT INTO EMPLOYEE VALUES(806700,'Lynette', 'Mitchell', '' ,'Lynette.Mitchell@suprema.com' , 'Patient Services  Representative' , NULL , 'Meridian' , 'McSweeney, Kristine', 'WASHINGTON' , '' , 'A' , 'Admin' , GETDATE() ,'Admin' , GETDATE()  );
END														
IF NOT EXISTS(SELECT TOP 1 1 FROM Employee WHERE EmployeeID = 806749)
BEGIN
INSERT INTO EMPLOYEE VALUES(806749,'Sharika', 'Nared', '' ,'Sharika.Nared@suprema.com' , 'Patient Services  Representative' , NULL , 'Meridian' , 'McSweeney, Kristine', 'WASHINGTON' , '' , 'A' , 'Admin' , GETDATE() ,'Admin' , GETDATE()  );
END														
IF NOT EXISTS(SELECT TOP 1 1 FROM Employee WHERE EmployeeID = 806699)
BEGIN
INSERT INTO EMPLOYEE VALUES(806699,'June', 'Neu', '' ,'June.Neu@suprema.com' , 'Patient Services  Representative' , NULL , 'Meridian' , 'McSweeney, Kristine', 'WASHINGTON' , '' , 'A' , 'Admin' , GETDATE() ,'Admin' , GETDATE()  );
END														
IF NOT EXISTS(SELECT TOP 1 1 FROM Employee WHERE EmployeeID = 806829)
BEGIN
INSERT INTO EMPLOYEE VALUES(806829,'Rohalia', 'Parker', '' ,'Rohalia.Parker@suprema.com' , 'Patient Services  Representative' , NULL , 'Meridian' , 'DiGeronimo, Corey', 'MASSACHUSETTS' , '' , 'A' , 'Admin' , GETDATE() ,'Admin' , GETDATE()  );
END														
IF NOT EXISTS(SELECT TOP 1 1 FROM Employee WHERE EmployeeID = 904)
BEGIN
INSERT INTO EMPLOYEE VALUES(904,'Milagros', 'Pedraza', '' ,'Milagros.Pedraza@suprema.com' , 'Patient Services  Representative' , NULL , 'Meridian' , 'Carreira, Alda', 'CONNECTICUT' , '' , 'A' , 'Admin' , GETDATE() ,'Admin' , GETDATE()  );
END														
IF NOT EXISTS(SELECT TOP 1 1 FROM Employee WHERE EmployeeID = 806808)
BEGIN
INSERT INTO EMPLOYEE VALUES(806808,'Shirllee', 'Perri', '' ,'Shirllee.Perri@suprema.com' , 'Patient Services  Representative' , NULL , 'Meridian' , 'Carreira, Alda', 'CONNECTICUT' , '' , 'A' , 'Admin' , GETDATE() ,'Admin' , GETDATE()  );
END														
IF NOT EXISTS(SELECT TOP 1 1 FROM Employee WHERE EmployeeID = 806803)
BEGIN
INSERT INTO EMPLOYEE VALUES(806803,'Daisy', 'Plasencia', '' ,'Daisy.Plasencia@suprema.com' , 'Patient Services  Representative' , NULL , 'Meridian' , 'Madison, Suprena', 'SOUTHEAST' , '' , 'A' , 'Admin' , GETDATE() ,'Admin' , GETDATE()  );
END														
IF NOT EXISTS(SELECT TOP 1 1 FROM Employee WHERE EmployeeID = 566)
BEGIN
INSERT INTO EMPLOYEE VALUES(566,'Lisa', 'Ramaeka', '' ,'Lisa.Ramaeka@suprema.com' , 'Patient Services  Representative' , NULL , 'Meridian' , 'DiGeronimo, Corey', 'CONNECTICUT' , '' , 'A' , 'Admin' , GETDATE() ,'Admin' , GETDATE()  );
END														
IF NOT EXISTS(SELECT TOP 1 1 FROM Employee WHERE EmployeeID = 806782)
BEGIN
INSERT INTO EMPLOYEE VALUES(806782,'Jenifer', 'Spencer', '' ,'Jenifer.Spencer@suprema.com' , 'Patient Services  Representative' , NULL , 'Meridian' , 'Madison, Suprena', 'SOUTHEAST' , '' , 'A' , 'Admin' , GETDATE() ,'Admin' , GETDATE()  );
END														
IF NOT EXISTS(SELECT TOP 1 1 FROM Employee WHERE EmployeeID = 806619)
BEGIN
INSERT INTO EMPLOYEE VALUES(806619,'Lynn', 'White', '' ,'Lynn.White@suprema.com' , 'Patient Services  Representative' , NULL , 'Meridian' , 'DiGeronimo, Corey', 'MASSACHUSETTS' , '' , 'A' , 'Admin' , GETDATE() ,'Admin' , GETDATE()  );
END														
IF NOT EXISTS(SELECT TOP 1 1 FROM Employee WHERE EmployeeID = 59812)
BEGIN
INSERT INTO EMPLOYEE VALUES(59812,'Corey', 'DiGeronimo', '' ,'Corey.DiGeronimo@suprema.com' , 'Patient Services  Supervisor' , NULL , 'Meridian' , 'Ricci, Debbie', 'CONNECTICUT' , '' , 'A' , 'Admin' , GETDATE() ,'Admin' , GETDATE()  );
END														
IF NOT EXISTS(SELECT TOP 1 1 FROM Employee WHERE EmployeeID = 431)
BEGIN
INSERT INTO EMPLOYEE VALUES(431,'Suprena', 'Madison', '' ,'Suprena.Madison@suprema.com' , 'Patient Services  Supervisor' , NULL , 'Meridian' , 'Jodway, Deborah', 'SOUTHEAST' , '' , 'A' , 'Admin' , GETDATE() ,'Admin' , GETDATE()  );
END														
IF NOT EXISTS(SELECT TOP 1 1 FROM Employee WHERE EmployeeID = 806737)
BEGIN
INSERT INTO EMPLOYEE VALUES(806737,'Kristine', 'McSweeney', '' ,'Kristine.McSweeney@suprema.com' , 'Patient Services  Supervisor' , NULL , 'Meridian' , 'McDaniel, Sidonie', 'WASHINGTON' , '' , 'A' , 'Admin' , GETDATE() ,'Admin' , GETDATE()  );
END														
IF NOT EXISTS(SELECT TOP 1 1 FROM Employee WHERE EmployeeID = 806753)
BEGIN
INSERT INTO EMPLOYEE VALUES(806753,'Shannon', 'Drennan', '' ,'Shannon.Drennan@suprema.com' , 'Patient Services Specialist' , NULL , 'Meridian' , 'McSweeney, Kristine', 'WASHINGTON' , '' , 'A' , 'Admin' , GETDATE() ,'Admin' , GETDATE()  );
END														
														
IF NOT EXISTS(SELECT TOP 1 1 FROM Employee WHERE EmployeeID = 666)
BEGIN
INSERT INTO EMPLOYEE VALUES(666,'Melissa', 'Stevenson', '' ,'Melissa.Stevenson@suprema.com' , 'Payment Posting Supervisor' , NULL , 'Meridian' , 'Jodway, Deborah', 'SOUTHEAST' , 'User' , 'A' , 'Admin' , GETDATE() ,'Admin' , GETDATE()  );
END														
IF NOT EXISTS(SELECT TOP 1 1 FROM Employee WHERE EmployeeID = 806744)
BEGIN
INSERT INTO EMPLOYEE VALUES(806744,'Calvin', 'Hua', '' ,'Calvin.Hua@suprema.com' , 'Payment Posting Team Lead' , NULL , 'Meridian' , 'Miller, Beverly', 'WASHINGTON' , 'User' , 'A' , 'Admin' , GETDATE() ,'Admin' , GETDATE()  );
END														
IF NOT EXISTS(SELECT TOP 1 1 FROM Employee WHERE EmployeeID = 64925)
BEGIN
INSERT INTO EMPLOYEE VALUES(64925,'Sandra', 'Lombardi', '' ,'Sandra.Lombardi@suprema.com' , 'Payment Posting Team Lead' , NULL , 'Meridian' , 'Figueroa, Lizzie', 'MASSACHUSETTS' , 'User' , 'A' , 'Admin' , GETDATE() ,'Admin' , GETDATE()  );
END														
IF NOT EXISTS(SELECT TOP 1 1 FROM Employee WHERE EmployeeID = 375)
BEGIN
INSERT INTO EMPLOYEE VALUES(375,'Cynthia', 'Jute', '' ,'Cynthia.Jute@suprema.com' , 'Payment Processing Representative' , NULL , 'Meridian' , 'Figueroa, Lizzie', 'CONNECTICUT' , 'User' , 'A' , 'Admin' , GETDATE() ,'Admin' , GETDATE()  );
END														
IF NOT EXISTS(SELECT TOP 1 1 FROM Employee WHERE EmployeeID = 806791)
BEGIN
INSERT INTO EMPLOYEE VALUES(806791,'Steven', 'Joblin', '' ,'Steven.Joblin@suprema.com' , 'PMO Manager' , NULL , 'Precision.BI' , 'Ahmad, Kaiser', 'PENNSYLVANIA' , '' , 'A' , 'Admin' , GETDATE() ,'Admin' , GETDATE()  );
END														
IF NOT EXISTS(SELECT TOP 1 1 FROM Employee WHERE EmployeeID = 1206)
BEGIN
INSERT INTO EMPLOYEE VALUES(1206,'Jennifer', 'Leone', '' ,'Jennifer.Leone@suprema.com' , 'Principal Healthcare Financial Analyst' , NULL , 'Precision.BI' , 'LaMarche, Maryann', 'PENNSYLVANIA' , '' , 'A' , 'Admin' , GETDATE() ,'Admin' , GETDATE()  );
END														
IF NOT EXISTS(SELECT TOP 1 1 FROM Employee WHERE EmployeeID = 806823)
BEGIN
INSERT INTO EMPLOYEE VALUES(806823,'Gregory', 'Thomas', '' ,'Gregory.Thomas@suprema.com' , 'Principal Healthcare Solutions Architect' , NULL , 'Precision.BI' , 'LaMarche, Maryann', 'PENNSYLVANIA' , '' , 'A' , 'Admin' , GETDATE() ,'Admin' , GETDATE()  );
END														
IF NOT EXISTS(SELECT TOP 1 1 FROM Employee WHERE EmployeeID = 114)
BEGIN
INSERT INTO EMPLOYEE VALUES(114,'Alexander', 'Arnold', '' ,'Alexander.Arnold@suprema.com' , 'Principal Software Engineer' , NULL , 'Precision.BI' , 'Miller, John', 'PENNSYLVANIA' , '' , 'A' , 'Admin' , GETDATE() ,'Admin' , GETDATE()  );
END														
IF NOT EXISTS(SELECT TOP 1 1 FROM Employee WHERE EmployeeID = 178)
BEGIN
INSERT INTO EMPLOYEE VALUES(178,'Colin', 'Carpi', '' ,'Colin.Carpi@suprema.com' , 'Principal Software Engineer' , NULL , 'Precision.BI' , 'Ahmad, Kaiser', 'PENNSYLVANIA' , '' , 'A' , 'Admin' , GETDATE() ,'Admin' , GETDATE()  );
END														
IF NOT EXISTS(SELECT TOP 1 1 FROM Employee WHERE EmployeeID = 311)
BEGIN
INSERT INTO EMPLOYEE VALUES(311,'Emily', 'Goyne', '' ,'Emily.Goyne@suprema.com' , 'Prinicipal Healthcare Solutions Architect' , NULL , 'Precision.BI' , 'Pearson, Jamie', 'PENNSYLVANIA' , '' , 'A' , 'Admin' , GETDATE() ,'Admin' , GETDATE()  );
END														
IF NOT EXISTS(SELECT TOP 1 1 FROM Employee WHERE EmployeeID = 442)
BEGIN
INSERT INTO EMPLOYEE VALUES(442,'Eric', 'Martin', '' ,'Eric.Martin@suprema.com' , 'Programmer' , NULL , 'Meridian' , 'Perrotta, Angelo', 'CONNECTICUT' , '' , 'A' , 'Admin' , GETDATE() ,'Admin' , GETDATE()  );
END														
IF NOT EXISTS(SELECT TOP 1 1 FROM Employee WHERE EmployeeID = 806802)
BEGIN
INSERT INTO EMPLOYEE VALUES(806802,'Matthew', 'Mastell', '' ,'Matthew.Mastell@suprema.com' , 'Programmer' , NULL , 'Robotics' , 'Kleckowski, Caryn', 'Growth' , '' , 'A' , 'Admin' , GETDATE() ,'Admin' , GETDATE()  );
END														
IF NOT EXISTS(SELECT TOP 1 1 FROM Employee WHERE EmployeeID = 7251)
BEGIN
INSERT INTO EMPLOYEE VALUES(7251,'Joseph', 'Wojnilo', '' ,'Joseph.Wojnilo@suprema.com' , 'Programmer' , NULL , 'Meridian' , 'Perrotta, Angelo', 'CONNECTICUT' , '' , 'A' , 'Admin' , GETDATE() ,'Admin' , GETDATE()  );
END														
IF NOT EXISTS(SELECT TOP 1 1 FROM Employee WHERE EmployeeID = 603)
BEGIN
INSERT INTO EMPLOYEE VALUES(603,'Robert', 'Russell', '' ,'Robert.Russell@suprema.com' , 'Programmer II' , NULL , 'Meridian' , 'Perrotta, Angelo', 'CONNECTICUT' , '' , 'A' , 'Admin' , GETDATE() ,'Admin' , GETDATE()  );
END														
IF NOT EXISTS(SELECT TOP 1 1 FROM Employee WHERE EmployeeID = 7210)
BEGIN
INSERT INTO EMPLOYEE VALUES(7210,'Jennifer', 'Crose', '' ,'Jennifer.Crose@suprema.com' , 'Project Coordinator' , NULL , 'Precision.BI' , 'Joblin, Steven', 'PENNSYLVANIA' , '' , 'A' , 'Admin' , GETDATE() ,'Admin' , GETDATE()  );
END														
IF NOT EXISTS(SELECT TOP 1 1 FROM Employee WHERE EmployeeID = 736)
BEGIN
INSERT INTO EMPLOYEE VALUES(736,'Christopher', 'Wyman', '' ,'Christopher.Wyman@suprema.com' , 'QA Analyst, Sr' , NULL , 'Precision.BI' , 'Miller, John', 'PENNSYLVANIA' , '' , 'A' , 'Admin' , GETDATE() ,'Admin' , GETDATE()  );
END														
IF NOT EXISTS(SELECT TOP 1 1 FROM Employee WHERE EmployeeID = 7297)
BEGIN
INSERT INTO EMPLOYEE VALUES(7297,'Tara', 'Cobb', '' ,'Tara.Cobb@suprema.com' , 'Quality Assurance and Auditing Specialist' , NULL , 'Meridian' , 'Dias, Lisa', 'CONNECTICUT' , 'User' , 'A' , 'Admin' , GETDATE() ,'Admin' , GETDATE()  );
END														
IF NOT EXISTS(SELECT TOP 1 1 FROM Employee WHERE EmployeeID = 261)
BEGIN
INSERT INTO EMPLOYEE VALUES(261,'James', 'Ethington', '' ,'James.Ethington@suprema.com' , 'Quality Assurance and Auditing Specialist' , NULL , 'Origin' , 'Hersma, Amber', 'NEW JERSEY' , 'User' , 'A' , 'Admin' , GETDATE() ,'Admin' , GETDATE()  );
END														
IF NOT EXISTS(SELECT TOP 1 1 FROM Employee WHERE EmployeeID = 7340)
BEGIN
INSERT INTO EMPLOYEE VALUES(7340,'Brenda', 'Israel', '' ,'Brenda.Israel@suprema.com' , 'Quality Assurance and Auditing Specialist' , NULL , 'Meridian' , 'Hersma, Amber', 'CONNECTICUT' , 'User' , 'A' , 'Admin' , GETDATE() ,'Admin' , GETDATE()  );
END														
IF NOT EXISTS(SELECT TOP 1 1 FROM Employee WHERE EmployeeID = 617)
BEGIN
INSERT INTO EMPLOYEE VALUES(617,'Jaime', 'Santiago', '' ,'Jaime.Santiago@suprema.com' , 'Quality Assurance and Auditing Specialist' , NULL , 'Origin' , 'Dias, Lisa', 'NEW JERSEY' , 'User' , 'A' , 'Admin' , GETDATE() ,'Admin' , GETDATE()  );
END														
IF NOT EXISTS(SELECT TOP 1 1 FROM Employee WHERE EmployeeID = 806608)
BEGIN
INSERT INTO EMPLOYEE VALUES(806608,'Jessy', 'Torres Collins', '' ,'Jessy.Torres Collins@suprema.com' , 'Quality Assurance and Auditing Specialist' , NULL , 'Meridian' , 'Dias, Lisa', 'MASSACHUSETTS' , 'User' , 'A' , 'Admin' , GETDATE() ,'Admin' , GETDATE()  );
END														
IF NOT EXISTS(SELECT TOP 1 1 FROM Employee WHERE EmployeeID = 64369)
BEGIN
INSERT INTO EMPLOYEE VALUES(64369,'Annamaria', 'Wright', '' ,'Annamaria.Wright@suprema.com' , 'Quality Assurance and Auditing Specialist' , NULL , 'Meridian' , 'Dias, Lisa', 'MASSACHUSETTS' , 'User' , 'A' , 'Admin' , GETDATE() ,'Admin' , GETDATE()  );
END														
IF NOT EXISTS(SELECT TOP 1 1 FROM Employee WHERE EmployeeID = 63396)
BEGIN
INSERT INTO EMPLOYEE VALUES(63396,'Lisa', 'Dias', '' ,'Lisa.Dias@suprema.com' , 'Quality Assurance Supervisor' , NULL , 'Meridian' , 'Hersma, Amber', 'MASSACHUSETTS' , 'User' , 'A' , 'Admin' , GETDATE() ,'Admin' , GETDATE()  );
END														
IF NOT EXISTS(SELECT TOP 1 1 FROM Employee WHERE EmployeeID = 386)
BEGIN
INSERT INTO EMPLOYEE VALUES(386,'Michael', 'Kirk', '' ,'Michael.Kirk@suprema.com' , 'Quality Control Analyst' , NULL , 'Meridian' , 'Perrotta, Angelo', 'CONNECTICUT' , 'User' , 'A' , 'Admin' , GETDATE() ,'Admin' , GETDATE()  );
END														
IF NOT EXISTS(SELECT TOP 1 1 FROM Employee WHERE EmployeeID = 7343)
BEGIN
INSERT INTO EMPLOYEE VALUES(7343,'Yvonne', 'Eggenberger', '' ,'Yvonne.Eggenberger@suprema.com' , 'Receptionist-Morris Anesthesia' , NULL , 'Meridian' , 'Walker, Pamela', 'NEW JERSEY' , '' , 'A' , 'Admin' , GETDATE() ,'Admin' , GETDATE()  );
END														
IF NOT EXISTS(SELECT TOP 1 1 FROM Employee WHERE EmployeeID = 750)
BEGIN
INSERT INTO EMPLOYEE VALUES(750,'Lorraine', 'Kogut', '' ,'Lorraine.Kogut@suprema.com' , 'Receptionist-Morris Anesthesia' , NULL , 'Meridian' , 'Walker, Pamela', 'NEW JERSEY' , '' , 'A' , 'Admin' , GETDATE() ,'Admin' , GETDATE()  );
END														
IF NOT EXISTS(SELECT TOP 1 1 FROM Employee WHERE EmployeeID = 806788)
BEGIN
INSERT INTO EMPLOYEE VALUES(806788,'Sari', 'Leon', '' ,'Sari.Leon@suprema.com' , 'Receptionist-Morris Anesthesia' , NULL , 'Meridian' , 'Walker, Pamela', 'NEW JERSEY' , '' , 'A' , 'Admin' , GETDATE() ,'Admin' , GETDATE()  );
END														
IF NOT EXISTS(SELECT TOP 1 1 FROM Employee WHERE EmployeeID = 7324)
BEGIN
INSERT INTO EMPLOYEE VALUES(7324,'Audrey', 'Reusch', '' ,'Audrey.Reusch@suprema.com' , 'Receptionist-Morris Anesthesia' , NULL , 'Meridian' , 'Walker, Pamela', 'NEW JERSEY' , '' , 'A' , 'Admin' , GETDATE() ,'Admin' , GETDATE()  );
END														
IF NOT EXISTS(SELECT TOP 1 1 FROM Employee WHERE EmployeeID = 529)
BEGIN
INSERT INTO EMPLOYEE VALUES(529,'Leydi', 'Ospina', '' ,'Leydi.Ospina@suprema.com' , 'Robotics Automation Analyst' , NULL , 'Meridian' , 'Kleckowski, Caryn', 'CONNECTICUT' , '' , 'A' , 'Admin' , GETDATE() ,'Admin' , GETDATE()  );
END														
IF NOT EXISTS(SELECT TOP 1 1 FROM Employee WHERE EmployeeID = 250)
BEGIN
INSERT INTO EMPLOYEE VALUES(250,'Claudia', 'Dossantos', '' ,'Claudia.Dossantos@suprema.com' , 'Robotics Automation Manager' , NULL , 'Robotics' , 'Kleckowski, Caryn', 'Growth' , '' , 'A' , 'Admin' , GETDATE() ,'Admin' , GETDATE()  );
END														
IF NOT EXISTS(SELECT TOP 1 1 FROM Employee WHERE EmployeeID = 272)
BEGIN
INSERT INTO EMPLOYEE VALUES(272,'Laurie', 'Fijal', '' ,'Laurie.Fijal@suprema.com' , 'Robotics Automation Manager' , NULL , 'Robotics' , 'Kleckowski, Caryn', 'Growth' , '' , 'A' , 'Admin' , GETDATE() ,'Admin' , GETDATE()  );
END														
IF NOT EXISTS(SELECT TOP 1 1 FROM Employee WHERE EmployeeID = 332)
BEGIN
INSERT INTO EMPLOYEE VALUES(332,'Jacqueline', 'Harmon', '' ,'Jacqueline.Harmon@suprema.com' , 'Sen Vp Operations' , NULL , 'Origin' , 'Donnelly, Robert', 'NEW JERSEY' , 'Executive' , 'A' , 'Admin' , GETDATE() ,'Admin' , GETDATE()  );
END														
IF NOT EXISTS(SELECT TOP 1 1 FROM Employee WHERE EmployeeID = 806812)
BEGIN
INSERT INTO EMPLOYEE VALUES(806812,'Charles', 'Panicker', '' ,'Charles.Panicker@suprema.com' , 'Sen Vp Operations' , NULL , 'Corporate' , 'Gontarek, Robert', 'HOLDINGS' , 'Admin' , 'A' , 'Admin' , GETDATE() ,'Admin' , GETDATE()  );
END														
IF NOT EXISTS(SELECT TOP 1 1 FROM Employee WHERE EmployeeID = 575)
BEGIN
INSERT INTO EMPLOYEE VALUES(575,'David', 'Reimer', '' ,'David.Reimer@suprema.com' , 'Senior Sales Executive' , NULL , 'Precision.BI' , 'Stone, William', 'PENNSYLVANIA' , '' , 'A' , 'Admin' , GETDATE() ,'Admin' , GETDATE()  );
END														
IF NOT EXISTS(SELECT TOP 1 1 FROM Employee WHERE EmployeeID = 156)
BEGIN
INSERT INTO EMPLOYEE VALUES(156,'Andrew', 'Breisch', '' ,'Andrew.Breisch@suprema.com' , 'Senior Technical Support Analyst' , NULL , 'Precision.BI' , 'Ahmad, Kaiser', 'PENNSYLVANIA' , '' , 'A' , 'Admin' , GETDATE() ,'Admin' , GETDATE()  );
END														
IF NOT EXISTS(SELECT TOP 1 1 FROM Employee WHERE EmployeeID = 41299)
BEGIN
INSERT INTO EMPLOYEE VALUES(41299,'Norman', 'DeRome', '' ,'Norman.DeRome@suprema.com' , 'Server Engineer' , NULL , 'Meridian' , 'Nagel, Donavan', 'MASSACHUSETTS' , '' , 'A' , 'Admin' , GETDATE() ,'Admin' , GETDATE()  );
END														
IF NOT EXISTS(SELECT TOP 1 1 FROM Employee WHERE EmployeeID = 7217)
BEGIN
INSERT INTO EMPLOYEE VALUES(7217,'Aerial', 'Wallenda', '' ,'Aerial.Wallenda@suprema.com' , 'Server Engineer' , NULL , 'Meridian' , 'Nagel, Donavan', 'SOUTHEAST' , '' , 'A' , 'Admin' , GETDATE() ,'Admin' , GETDATE()  );
END														
IF NOT EXISTS(SELECT TOP 1 1 FROM Employee WHERE EmployeeID = 7267)
BEGIN
INSERT INTO EMPLOYEE VALUES(7267,'Lindsey', 'Welz', '' ,'Lindsey.Welz@suprema.com' , 'Software & Perf. Specialist' , NULL , 'Meridian' , 'Kleckowski, Caryn', 'CONNECTICUT' , '' , 'A' , 'Admin' , GETDATE() ,'Admin' , GETDATE()  );
END														
IF NOT EXISTS(SELECT TOP 1 1 FROM Employee WHERE EmployeeID = 806)
BEGIN
INSERT INTO EMPLOYEE VALUES(806,'Mason', 'Albright', '' ,'Mason.Albright@suprema.com' , 'Software Engineer' , NULL , 'Precision.BI' , 'Miller, John', 'PENNSYLVANIA' , '' , 'A' , 'Admin' , GETDATE() ,'Admin' , GETDATE()  );
END														
IF NOT EXISTS(SELECT TOP 1 1 FROM Employee WHERE EmployeeID = 806773)
BEGIN
INSERT INTO EMPLOYEE VALUES(806773,'Christopher', 'Schlegel', '' ,'Christopher.Schlegel@suprema.com' , 'Software Engineer' , NULL , 'Precision.BI' , 'Miller, John', 'PENNSYLVANIA' , '' , 'A' , 'Admin' , GETDATE() ,'Admin' , GETDATE()  );
END														
IF NOT EXISTS(SELECT TOP 1 1 FROM Employee WHERE EmployeeID = 544)
BEGIN
INSERT INTO EMPLOYEE VALUES(544,'Jamie', 'Pearson', '' ,'Jamie.Pearson@suprema.com' , 'Solutions Manager' , NULL , 'Precision.BI' , 'LaMarche, Maryann', 'PENNSYLVANIA' , '' , 'A' , 'Admin' , GETDATE() ,'Admin' , GETDATE()  );
END														
IF NOT EXISTS(SELECT TOP 1 1 FROM Employee WHERE EmployeeID = 7263)
BEGIN
INSERT INTO EMPLOYEE VALUES(7263,'Mitchell', 'Rivera', '' ,'Mitchell.Rivera@suprema.com' , 'Sr Application Developer' , NULL , 'Meridian' , 'Perrotta, Angelo', 'CONNECTICUT' , '' , 'A' , 'Admin' , GETDATE() ,'Admin' , GETDATE()  );
END														
IF NOT EXISTS(SELECT TOP 1 1 FROM Employee WHERE EmployeeID = 41242)
BEGIN
INSERT INTO EMPLOYEE VALUES(41242,'Claudia', 'Maiorana', '' ,'Claudia.Maiorana@suprema.com' , 'Sr Director of Client Services' , NULL , 'Meridian' , 'Gontarek, Robert', 'MASSACHUSETTS' , 'Admin' , 'A' , 'Admin' , GETDATE() ,'Admin' , GETDATE()  );
END														
IF NOT EXISTS(SELECT TOP 1 1 FROM Employee WHERE EmployeeID = 41318)
BEGIN
INSERT INTO EMPLOYEE VALUES(41318,'Crystal', 'Williams', '' ,'Crystal.Williams@suprema.com' , 'Sr Director of Operations' , NULL , 'Meridian' , 'Gontarek, Robert', 'CONNECTICUT' , 'Admin' , 'A' , 'Admin' , GETDATE() ,'Admin' , GETDATE()  );
END														
IF NOT EXISTS(SELECT TOP 1 1 FROM Employee WHERE EmployeeID = 7244)
BEGIN
INSERT INTO EMPLOYEE VALUES(7244,'Kaiser', 'Ahmad', '' ,'Kaiser.Ahmad@suprema.com' , 'Sr Director, Engineering and Operations' , NULL , 'Precision.BI' , 'Gontarek, Robert', 'PENNSYLVANIA' , '' , 'A' , 'Admin' , GETDATE() ,'Admin' , GETDATE()  );
END														
IF NOT EXISTS(SELECT TOP 1 1 FROM Employee WHERE EmployeeID = 233)
BEGIN
INSERT INTO EMPLOYEE VALUES(233,'Tayhan', 'Denk', '' ,'Tayhan.Denk@suprema.com' , 'Sr Healthcare Data Engineer' , NULL , 'Precision.BI' , 'Foight, William', 'PENNSYLVANIA' , '' , 'A' , 'Admin' , GETDATE() ,'Admin' , GETDATE()  );
END														
IF NOT EXISTS(SELECT TOP 1 1 FROM Employee WHERE EmployeeID = 499)
BEGIN
INSERT INTO EMPLOYEE VALUES(499,'Rebecca', 'Mulhollem', '' ,'Rebecca.Mulhollem@suprema.com' , 'Sr Healthcare Data Engineer' , NULL , 'Precision.BI' , 'Foight, William', 'PENNSYLVANIA' , '' , 'A' , 'Admin' , GETDATE() ,'Admin' , GETDATE()  );
END														
IF NOT EXISTS(SELECT TOP 1 1 FROM Employee WHERE EmployeeID = 199)
BEGIN
INSERT INTO EMPLOYEE VALUES(199,'Linda', 'Cline', '' ,'Linda.Cline@suprema.com' , 'Sr Healthcare Solutions Architect' , NULL , 'Precision.BI' , 'Pearson, Jamie', 'PENNSYLVANIA' , '' , 'A' , 'Admin' , GETDATE() ,'Admin' , GETDATE()  );
END														
IF NOT EXISTS(SELECT TOP 1 1 FROM Employee WHERE EmployeeID = 7344)
BEGIN
INSERT INTO EMPLOYEE VALUES(7344,'Susan', 'D'+'Agostino', '' ,'Susan.D'+'Agostino@suprema.com' , 'Sr Healthcare Solutions Architect' , NULL , 'Precision.BI' , 'LaMarche, Maryann', 'PENNSYLVANIA' , '' , 'A' , 'Admin' , GETDATE() ,'Admin' , GETDATE()  );
END														
IF NOT EXISTS(SELECT TOP 1 1 FROM Employee WHERE EmployeeID = 352)
BEGIN
INSERT INTO EMPLOYEE VALUES(352,'Anwar', 'Shaik', '' ,'Anwar.Shaik@suprema.com' , 'Sr Software Engineer' , NULL , 'Precision.BI' , 'Miller, John', 'PENNSYLVANIA' , '' , 'A' , 'Admin' , GETDATE() ,'Admin' , GETDATE()  );
END														
IF NOT EXISTS(SELECT TOP 1 1 FROM Employee WHERE EmployeeID = 482)
BEGIN
INSERT INTO EMPLOYEE VALUES(482,'Judy', 'Mollenhour', '' ,'Judy.Mollenhour@suprema.com' , 'Surgery Scheduler-Morris Anest' , NULL , 'Meridian' , 'Walker, Pamela', 'NEW JERSEY' , '' , 'A' , 'Admin' , GETDATE() ,'Admin' , GETDATE()  );
END														
IF NOT EXISTS(SELECT TOP 1 1 FROM Employee WHERE EmployeeID = 456)
BEGIN
INSERT INTO EMPLOYEE VALUES(456,'Jesse', 'McCallum', '' ,'Jesse.McCallum@suprema.com' , 'Systems Admin/IT Supervisor' , NULL , 'Meridian' , 'Vesce, Michael', 'CONNECTICUT' , '' , 'A' , 'Admin' , GETDATE() ,'Admin' , GETDATE()  );
END														
IF NOT EXISTS(SELECT TOP 1 1 FROM Employee WHERE EmployeeID = 806822)
BEGIN
INSERT INTO EMPLOYEE VALUES(806822,'Patricia', 'LeBeau', '' ,'Patricia.LeBeau@suprema.com' , 'Team Lead' , NULL , 'Meridian' , 'McSweeney, Kristine', 'WASHINGTON' , 'User' , 'A' , 'Admin' , GETDATE() ,'Admin' , GETDATE()  );
END														
IF NOT EXISTS(SELECT TOP 1 1 FROM Employee WHERE EmployeeID = 130017)
BEGIN
INSERT INTO EMPLOYEE VALUES(130017,'Lisa', 'Ruell', '' ,'Lisa.Ruell@suprema.com' , 'Technical Analyst' , NULL , 'Meridian' , 'Nagel, Donavan', 'MASSACHUSETTS' , '' , 'A' , 'Admin' , GETDATE() ,'Admin' , GETDATE()  );
END														
IF NOT EXISTS(SELECT TOP 1 1 FROM Employee WHERE EmployeeID = 176)
BEGIN
INSERT INTO EMPLOYEE VALUES(176,'Michael', 'Capiotis', '' ,'Michael.Capiotis@suprema.com' , 'Technical Support Analyst, Lead' , NULL , 'Precision.BI' , 'Ahmad, Kaiser', 'PENNSYLVANIA' , '' , 'A' , 'Admin' , GETDATE() ,'Admin' , GETDATE()  );
END														
IF NOT EXISTS(SELECT TOP 1 1 FROM Employee WHERE EmployeeID = 7277)
BEGIN
INSERT INTO EMPLOYEE VALUES(7277,'Judy', 'Hennessey', '' ,'Judy.Hennessey@suprema.com' , 'Technical Writer' , NULL , 'Precision.BI' , 'Miller, John', 'PENNSYLVANIA' , '' , 'A' , 'Admin' , GETDATE() ,'Admin' , GETDATE()  );
END														
IF NOT EXISTS(SELECT TOP 1 1 FROM Employee WHERE EmployeeID = 42801)
BEGIN
INSERT INTO EMPLOYEE VALUES(42801,'Alda', 'Carreira', '' ,'Alda.Carreira@suprema.com' , 'Training & Implementation Manager' , NULL , 'Meridian' , 'Maiorana, Claudia', 'MASSACHUSETTS' , '' , 'A' , 'Admin' , GETDATE() ,'Admin' , GETDATE()  );
END														
IF NOT EXISTS(SELECT TOP 1 1 FROM Employee WHERE EmployeeID = 484)
BEGIN
INSERT INTO EMPLOYEE VALUES(484,'Karilyn', 'Monroe', '' ,'Karilyn.Monroe@suprema.com' , 'Training & Implementation Specialist' , NULL , 'Meridian' , 'Kleckowski, Caryn', 'CONNECTICUT' , '' , 'A' , 'Admin' , GETDATE() ,'Admin' , GETDATE()  );
END														
IF NOT EXISTS(SELECT TOP 1 1 FROM Employee WHERE EmployeeID = 344)
BEGIN
INSERT INTO EMPLOYEE VALUES(344,'Debra', 'Hines', '' ,'Debra.Hines@suprema.com' , 'Transcriptionist' , NULL , 'Meridian' , 'Sheldone, Judith', 'SOUTHEAST' , '' , 'A' , 'Admin' , GETDATE() ,'Admin' , GETDATE()  );
END														
IF NOT EXISTS(SELECT TOP 1 1 FROM Employee WHERE EmployeeID = 433)
BEGIN
INSERT INTO EMPLOYEE VALUES(433,'Tami', 'Mambretti', '' ,'Tami.Mambretti@suprema.com' , 'Transcriptionist' , NULL , 'Meridian' , 'Sheldone, Judith', 'SOUTHEAST' , '' , 'A' , 'Admin' , GETDATE() ,'Admin' , GETDATE()  );
END														
IF NOT EXISTS(SELECT TOP 1 1 FROM Employee WHERE EmployeeID = 471)
BEGIN
INSERT INTO EMPLOYEE VALUES(471,'Joseph', 'Mercer', '' ,'Joseph.Mercer@suprema.com' , 'Vice President' , NULL , 'Origin' , 'Harmon, Jacqueline', 'NEW JERSEY' , 'User' , 'A' , 'Admin' , GETDATE() ,'Admin' , GETDATE()  );
END														
IF NOT EXISTS(SELECT TOP 1 1 FROM Employee WHERE EmployeeID = 548)
BEGIN
INSERT INTO EMPLOYEE VALUES(548,'Angelo', 'Perrotta', '' ,'Angelo.Perrotta@suprema.com' , 'VP EDI Applications' , NULL , 'Meridian' , 'Gontarek, Robert', 'CONNECTICUT' , '' , 'A' , 'Admin' , GETDATE() ,'Admin' , GETDATE()  );
END				

UPDATE E SET ReportsTo = R.EmployeeID
FROM Employee E
INNER JOIN Employee R ON REPLACE(REPLACE(E.ReportsTo,',',''),' ','') = LTRIM(RTRIM(R.LastName))+LTRIM(RTRIM(R.FirstName))

COMMIT TRANSACTION
END TRY

BEGIN CATCH
	SELECT ERROR_MESSAGE()
		,ERROR_LINE()
		,ERROR_SEVERITY()

	ROLLBACK TRANSACTION
END CATCH
GO
/*
Script Name : 22_Attribute
Module_Name : M3Pact
Created By  : Abhishek Kovvuri
CreatedDate : 11-July-2018

Data Verification Script : 

SELECT * FROM [Attribute](NOLOCK)
*/

BEGIN TRY
BEGIN TRANSACTION

DECLARE @User NVARCHAR(30) = 'Admin'

INSERT INTO [dbo].[Attribute]
(
	[AttributeCode],
    [AttributeName],
    [AttributeDescription],
	[AttributeType],
	[RecordStatus],
    [CreatedDate],
    [CreatedBy],
    [ModifiedDate],
    [ModifiedBy]
)
SELECT
	A.[AttributeCode],
	A.[AttributeName],
	A.[AttributeDescription],
	A.[AttributeType],
	'A' AS [RecordStatus],
	GETDATE() AS [CreatedDate],
    @User AS [CreatedBy],
    GETDATE() AS [ModifiedDate],
    @User AS [ModifiedBy]
FROM
(
	SELECT 'LastEnteredBusinessDays' AS [AttributeCode], 'Projected Cash - Last Entered Business Days' AS [AttributeName] , 'LastEnteredBusinessDaysInDepositLog' AS [AttributeDescription] , 'Integer' AS [AttributeType] UNION ALL
	SELECT 'LastEnteredWeeks' AS [AttributeCode], 'Projected Cash - Last Entered Weeks' AS [AttributeName] , 'LastEnteredWeeksInDepositLog' AS [AttributeDescription] , 'Integer' AS [AttributeType]
)
AS A
WHERE NOT EXISTS (SELECT 1 FROM [Attribute] (NOLOCK) B WHERE B.AttributeCode = A.AttributeCode)

COMMIT TRANSACTION
END TRY

BEGIN CATCH
	SELECT ERROR_MESSAGE()
		,ERROR_LINE()
		,ERROR_SEVERITY()

	ROLLBACK TRANSACTION
END CATCH
GO
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
DECLARE @AttributeId INT


INSERT INTO [dbo].[AdminConfigValues]
(
	 [AttributeId] ,
    [AttributeValue] , 
    [RecordStatus]  , 
    [CreatedDate] , 
    [CreatedBy] , 
    [ModifiedDate]  , 
    [ModifiedBy] 
)
SELECT
	A.[AttributeId] ,
    A.[AttributeValue] , 
    A.[RecordStatus]  , 
    A.[CreatedDate] , 
    @User AS [CreatedBy] , 
    A.[ModifiedDate]  , 
    @User AS [ModifiedBy] 
FROM
(
	SELECT (SELECT AttributeId FROM Attribute WHERE AttributeCode='LastEnteredBusinessDays') AS [AttributeId],15 AS [AttributeValue],'A' AS [RecordStatus],GETDATE() AS [CreatedDate],GETDATE() AS [ModifiedDate] UNION ALL
	SELECT (SELECT AttributeId FROM Attribute WHERE AttributeCode='LastEnteredWeeks') AS [AttributeId],3 AS [AttributeValue],'A' AS [RecordStatus],GETDATE() AS [CreatedDate],GETDATE() AS [ModifiedDate]
)
AS A
WHERE NOT EXISTS (SELECT 1 FROM [AdminConfigValues] (NOLOCK) B WHERE B.AttributeId = A.AttributeId)

COMMIT TRANSACTION
END TRY

BEGIN CATCH
	SELECT ERROR_MESSAGE()
		,ERROR_LINE()
		,ERROR_SEVERITY()

	ROLLBACK TRANSACTION
END CATCH
GO

GO
