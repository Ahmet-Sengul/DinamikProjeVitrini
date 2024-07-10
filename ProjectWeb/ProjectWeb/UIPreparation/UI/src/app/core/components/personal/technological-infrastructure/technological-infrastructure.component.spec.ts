import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TechnologicalInfrastructureComponent } from './technological-infrastructure.component';

describe('TechnologicalInfrastructureComponent', () => {
  let component: TechnologicalInfrastructureComponent;
  let fixture: ComponentFixture<TechnologicalInfrastructureComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ TechnologicalInfrastructureComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(TechnologicalInfrastructureComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
