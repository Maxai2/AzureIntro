import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { EditNewDocComponent } from './edit-new-doc.component';

describe('EditNewDocComponent', () => {
  let component: EditNewDocComponent;
  let fixture: ComponentFixture<EditNewDocComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ EditNewDocComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(EditNewDocComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
