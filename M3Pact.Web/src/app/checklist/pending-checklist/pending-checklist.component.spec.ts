import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { PendingChecklistComponent } from './pending-checklist.component';

describe('PendingChecklistComponent', () => {
  let component: PendingChecklistComponent;
  let fixture: ComponentFixture<PendingChecklistComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ PendingChecklistComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(PendingChecklistComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
