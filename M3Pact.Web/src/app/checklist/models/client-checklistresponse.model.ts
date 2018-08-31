export class ClientChecklistResponse {
    ChecklistName: string;
    QuestionCode: string;
    Questionid: number | null;
    QuestionText: string;
    IsKPI: boolean|null;
    RequireFreeform: boolean|null;
    ExpectedRespone: boolean|null;
    ActualResponse: boolean|null;
    ActualFreeForm: string;
    ClientCheckListMapID: number|null;
    CheckListAttributeMapID: number | null;
}
