import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { TranslateModule } from '@ngx-translate/core';
import { PersonalFooterComponent } from '../components/personal-footer/personal-footer.component';
import { PersonalNavbarComponent } from '../components/personal-navbar/personal-navbar.component';


@NgModule({
  imports: [
    CommonModule,
    RouterModule,
    TranslateModule,
  ],
  declarations: [
   PersonalFooterComponent,
   PersonalNavbarComponent
  ],
  exports: [
    PersonalFooterComponent,
   PersonalNavbarComponent
  ]
})
export class PersonalComponentsModule { }
