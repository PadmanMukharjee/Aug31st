CREATE PROCEDURE [dbo].[GetWeeklyPendingChecklist](
@clientCode varchar(50)
)
AS
BEGIN

DECLARE @clientStartDate DATE

SELECT @clientStartDate=  MIN(CLM.EffectiveDate) FROM Client C
JOIN ClientCheckListMap CLM
ON C.ClientID=clm.ClientID
JOIN CheckList  Cl
ON Cl.CheckListID=clm.CheckListID
JOIN checklisttype Clt
ON Clt.CheckListTypeID=cl.CheckListTypeID
WHERE c.ClientCode=@clientCode
AND c.IsActive='A'
AND clt.CheckListTypeCode='WEEK'
AND clm.StartDate<> clm.EndDate
AND GETDATE() between clm.StartDate and clm.EndDate

DECLARE @DateFrom DateTime = @clientStartDate ,
        @DateTo DateTime = GETDATE()


create table #WeekChecklistDays(Checklistdate DATETIME)

;WITH CTE(dt)
AS
(
      SELECT @DateFrom
      UNION ALL
      SELECT DATEADD(d, 1, dt) FROM CTE
      WHERE dt < @DateTo
)

INSERT INTO #WeekChecklistDays
SELECT  dt FROM CTE
WHERE DATENAME(dw, dt) In ('Monday')
option (maxrecursion 0)

SELECT * FROM #WeekChecklistDays where Checklistdate NOT IN(
select CSD.CheckListEffectiveDate from Client C
join ClientCheckListMap CLM
on C.ClientID=CLM.ClientID
join CheckList CL
on CL.CheckListID=CLM.CheckListID
join ClientCheckListStatusDetail CSD
on CLM.ClientCheckListMapID=CSD.ClientCheckListMapID
join CheckListType CLT
on CLT.CheckListTypeID=CL.CheckListTypeID
where c.ClientCode=@clientCode
and CSD.ChecklistStatus='C'	
and CLT.CheckListTypeCode='WEEK')
--and @clientStartDate<=GETDATE()


DROP TABLE #WeekChecklistDays

END