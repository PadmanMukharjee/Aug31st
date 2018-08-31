import { CheckListItem } from './checklist-item.model';

export interface CheckList {
    checklistId: number;
    name: string;
    sites: Array<number>;
    systems: Array<number>;
    checklistType: number;
    checklistItems: Array<CheckListItem>;
}