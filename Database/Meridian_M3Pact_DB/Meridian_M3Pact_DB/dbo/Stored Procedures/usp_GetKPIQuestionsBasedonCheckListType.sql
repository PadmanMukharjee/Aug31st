CREATE PROCEDURE [dbo].[usp_GetKPIQuestionsBasedonCheckListType]
(
	@CheckListTypeId int = NUll
)
AS
BEGIN
	with questionCTE (questionId) as (
		select distinct(clam.checkListAttributeValueID) from CheckList cl 
			join checkListAttributeMap clam on cl.CheckListID = clam.CheckListID
			join checkListAttribute cla on clam.CheckListAttributeId = cla.CheckListAttributeId
			where cla.AttributeCode = 'QUE' and cl.CheckListTypeID = @CheckListTypeId
	)
	select q.QuestionID,
		   q.QuestionText,
		   q.ExpectedRespone,
		   q.IsKPI,
		   q.IsUniversal,
		   q.RequireFreeform
	from Question q
	join questionCTE on q.QuestionID = questionCTE.questionId
	where q.IsKPI = 1
END
RETURN 0