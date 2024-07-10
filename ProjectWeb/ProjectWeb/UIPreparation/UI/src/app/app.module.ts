import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { NgModule, CUSTOM_ELEMENTS_SCHEMA } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { HttpClient, HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { RouterModule } from '@angular/router';
import { AppRoutingModule } from './app.routing';
import { AppComponent } from './app.component';
import { NgMultiSelectDropDownModule } from 'ng-multiselect-dropdown';
import { NgbModule } from '@ng-bootstrap/ng-bootstrap';
import { TranslateModule, TranslateLoader } from '@ngx-translate/core';
import { ComponentsModule } from './core/modules/components.module';
import { AdminLayoutComponent } from './core/components/app/layouts/admin-layout/admin-layout.component';
import { TranslationService } from './core/services/translation.service';
import { SweetAlert2Module } from '@sweetalert2/ngx-sweetalert2';
import { LoginGuard } from './core/guards/login-guard';
import { AuthInterceptorService } from './core/interceptors/auth-interceptor.service';
import { HttpEntityRepositoryService } from './core/services/http-entity-repository.service';
import { BlogComponent } from './core/components/personal/blog/blog.component';
import { TeamComponent } from './core/components/personal/team/team.component';
import { AboutComponent } from './core/components/personal/about/about.component';
import { HomePageComponent } from './core/components/personal/home-page/home-page.component';
import { PersonalNavbarComponent } from './core/components/personal-navbar/personal-navbar.component';
import { PersonalFooterComponent } from './core/components/personal-footer/personal-footer.component';
import { PersonalComponentsModule } from './core/modules/personal-components.module';
import { PersonalLayoutComponent } from './core/components/app/layouts/personal-layout/personal-layout.component';
import { TechnologicalInfrastructureComponent } from './core/components/personal/technological-infrastructure/technological-infrastructure.component';
import { ContactComponent } from './core/components/personal/contact/contact.component';
import { MatRadioModule } from '@angular/material/radio'; // MatRadioModule importu ekleyin
import { MatCheckboxModule } from '@angular/material/checkbox'; // MatCheckboxModule importu ekleyin



// i18 kullanıclak ise aşağıdaki metod aktif edilecek

//  export function HttpLoaderFactory(http: HttpClient) {
//    
//    var asd=new TranslateHttpLoader(http, '../../../../assets/i18n/', '.json'); 
//    return asd;
//  }


export function tokenGetter() {
  return localStorage.getItem("token");
}


@NgModule({
  imports: [
    BrowserAnimationsModule,
    FormsModule,
    ReactiveFormsModule,
    HttpClientModule,
    ComponentsModule,
    PersonalComponentsModule,
    RouterModule,
    AppRoutingModule,
    NgMultiSelectDropDownModule.forRoot(),
    SweetAlert2Module.forRoot(),
    NgbModule,
    MatRadioModule,
    MatCheckboxModule,
    TranslateModule.forRoot({
      loader: {
        provide: TranslateLoader,
        //useFactory:HttpLoaderFactory, //i18 kullanılacak ise useClass kapatılıp yukarıda bulunan HttpLoaderFactory ve bu satır aktif edilecek
        useClass: TranslationService,
        deps: [HttpClient]
      }

    })

  ],
  declarations: [
    AppComponent,
    AdminLayoutComponent,
    PersonalLayoutComponent,
    TechnologicalInfrastructureComponent,
    ContactComponent,
  ],

  providers: [
    LoginGuard,
    {
      provide: HTTP_INTERCEPTORS,
      useClass: AuthInterceptorService,
      multi: true,
    },    
    HttpEntityRepositoryService,
  ],
  bootstrap: [AppComponent],
  schemas: [CUSTOM_ELEMENTS_SCHEMA],
})
export class AppModule { }
