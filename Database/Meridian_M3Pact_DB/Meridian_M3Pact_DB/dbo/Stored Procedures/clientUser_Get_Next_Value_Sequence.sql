

CREATE PROCEDURE [dbo].[clientUser_Get_Next_Value_Sequence]
AS
BEGIN
 SET NOCOUNT ON 
 DECLARE @seqNum int; 

    SET @seqNum= NEXT VALUE FOR [ClientUser_Sequence]

 SELECT 'C'+ RTRIM(@seqNum) AS SequenceNumber

END