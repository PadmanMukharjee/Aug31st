import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { CompletedChecklistComponent } from './completed-checklist.component';

describe('CompletedChecklistComponent', () => {
  let component: CompletedChecklistComponent;
  let fixture: ComponentFixture<CompletedChecklistComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ CompletedChecklistComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CompletedChecklistComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
