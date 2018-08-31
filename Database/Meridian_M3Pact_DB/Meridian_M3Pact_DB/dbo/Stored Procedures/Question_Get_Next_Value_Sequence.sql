CREATE PROCEDURE [dbo].[Question_Get_Next_Value_Sequence]
AS
BEGIN
 SET NOCOUNT ON 
 DECLARE @seqNum int; 

    SET @seqNum= NEXT VALUE FOR [Question_Sequence]

 SELECT 'Q'+ RTRIM(@seqNum) AS SequenceNumber

END