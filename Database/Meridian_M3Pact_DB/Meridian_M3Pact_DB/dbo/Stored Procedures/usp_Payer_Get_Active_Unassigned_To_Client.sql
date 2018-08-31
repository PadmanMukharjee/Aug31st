
CREATE PROCEDURE [dbo].[usp_Payer_Get_Active_Unassigned_To_Client]
(
	@ClientCode NVARCHAR(60) = NULL
)
AS
BEGIN

	DECLARE @ClientID INT;
	SELECT @ClientID = ClientID FROM Client WHERE ClientCode = @ClientCode

	IF (ISNULL(@ClientID, 0) > 0)
	BEGIN
		SELECT P.PayerCode,
			   P.PayerName
		FROM Payer P
		LEFT JOIN ClientPayer CP ON P.PayerID = CP.PayerID AND CP.ClientID = @ClientID AND CP.RecordStatus <>'D'
		WHERE P.RecordStatus = 'A' AND CP.PayerID IS NULL 
	END
	
END

SET QUOTED_IDENTIFIER ON
