/*
Script Name : usp_ClientConfigStep_Get_Step_Status_Details
Module_Name : M3Pact
Created By  : Shravani P
CreatedDate : 04/25/2018

Revision History 
=============
Ver  Date				Who						Comment
1.0  25-Apr-2018		Shravani Palla			Get all the client's configuration steps along with the step status if exists.
1.1  04-May-2018		Shravani Palla			Added Screen & ScreenUserRoleMap joins to get View/Edit permissions for a role.
1.2  30-May-2018		Shravani Palla			Added Sort by DisplayOrder
*/


CREATE PROCEDURE [dbo].[usp_ClientConfigStep_Get_Step_Status_Details]
(
	@RoleCode VARCHAR(100),
	@ClientCode NVARCHAR(60) = NULL
)
AS
BEGIN

	DECLARE @ClientID INT;
	SELECT @ClientID = ClientID FROM Client WHERE ClientCode = @ClientCode

	DECLARE @RoleID INT;
	SELECT @RoleID = RoleID FROM Roles WHERE RoleCode = @RoleCode

	IF (ISNULL(@ClientID, 0) > 0)
	BEGIN
		SELECT CCS.ClientConfigStepID,
			   CCS.ClientConfigStepName,
			   CCSD.ClientConfigStepStatusID,
			   CCSD.ClientConfigStepDetailID,
			   (CASE WHEN SURM.ScreenId IS NULL THEN 0 ELSE 1 END) AS CanView,
			   SURM.CanEdit
		FROM ClientConfigStep CCS
		INNER JOIN dbo.[Screen] S ON S.ScreenCode = CCS.ScreenCode
		LEFT JOIN ClientConfigStepDetail CCSD ON CCSD.ClientID = @ClientID AND CCS.ClientConfigStepID = CCSD.ClientConfigStepID
		LEFT JOIN ScreenUserRoleMap SURM ON SURM.ScreenId = S.ScreenId AND SURM.RoleId = @RoleID
		WHERE S.RecordStatus = 'A' AND SURM.RecordStatus = 'A'
		ORDER BY CCS.DisplayOrder
	END

	ELSE
	BEGIN
		SELECT CCS.ClientConfigStepID,
			   CCS.ClientConfigStepName,
			   NULL AS ClientConfigStepStatusID,
			   NULL AS ClientConfigStepDetailID,
			   (CASE WHEN SURM.ScreenId IS NULL THEN 0 ELSE 1 END) AS CanView,
			   SURM.CanEdit
		FROM ClientConfigStep CCS
		INNER JOIN dbo.[Screen] S ON S.ScreenCode = CCS.ScreenCode
		LEFT JOIN ScreenUserRoleMap SURM ON SURM.ScreenId = S.ScreenId AND SURM.RoleId = @RoleID
		WHERE S.RecordStatus = 'A' AND SURM.RecordStatus = 'A'
		ORDER BY CCS.DisplayOrder
	END
	
END

SET QUOTED_IDENTIFIER ON
