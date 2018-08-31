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
:r .\DataModificationScripts\01_Site.sql
:r .\DataModificationScripts\02_ApplicationType.sql
:r .\DataModificationScripts\03_Application.sql
:r .\DataModificationScripts\04_ApiResource.sql
:r .\DataModificationScripts\05_ApplicationApiResource.sql
:r .\DataModificationScripts\07_Roles.sql
:r .\DataModificationScripts\09_System.sql
:r .\DataModificationScripts\10_Screen.sql
:r .\DataModificationScripts\11_ScreenUserRoleMap.sql
:r .\DataModificationScripts\12_ScreenAction.sql
:r .\DataModificationScripts\13_RoleAction.sql
:r .\DataModificationScripts\06_ClientConfigStep.sql
:r .\DataModificationScripts\14_CheckListType.sql
:r .\DataModificationScripts\15_M3MetricsQuestion.sql
:r .\DataModificationScripts\16_KPIMeasure.sql
:r .\DataModificationScripts\17_JobStatusAndJobProcess.sql
:r .\DataModificationScripts\18_ChecklistAttribute.sql
:r .\DataModificationScripts\19_ClientConfigStepStatus.sql
:r .\DataModificationScripts\20_Month.sql
:r .\DataModificationScripts\21_Employee.sql
:r .\DataModificationScripts\22_Attribute.sql
:r .\DataModificationScripts\23_ControlType.sql
:r .\DataModificationScripts\24_AdminConfig.sql