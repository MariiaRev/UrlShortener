import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ShortUrlList } from './short-url-list';

describe('ShortUrlList', () => {
  let component: ShortUrlList;
  let fixture: ComponentFixture<ShortUrlList>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [ShortUrlList]
    })
    .compileComponents();

    fixture = TestBed.createComponent(ShortUrlList);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
