import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, RouterStateSnapshot, Resolve } from '@angular/router';
import { Observable } from 'rxjs/Observable';
import 'rxjs/add/observable/empty';

import { CheckList } from '../../models/checklist.model';
import { CheckListService } from '../../services/checklist.service';

@Injectable()
export class ChecklistResolve implements Resolve<CheckList[]> {

  constructor(private checklistService: CheckListService) {

  }

  resolve(next: ActivatedRouteSnapshot,
    state: RouterStateSnapshot) {
    if (next.params['id'] && parseInt(next.params['id']) && parseInt(next.params['id']) > 0) {
      return this.checklistService.getCheckList(next.params['id']);
    }
    return Observable.empty<CheckList[]>();
  }
}
