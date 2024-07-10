import { NgModule } from '@angular/core';
import { CommonModule, } from '@angular/common';
import { BrowserModule  } from '@angular/platform-browser';
import { Routes, RouterModule } from '@angular/router';
import { AdminLayoutComponent } from './core/components/app/layouts/admin-layout/admin-layout.component';
import { PersonalLayoutComponent } from './core/components/app/layouts/personal-layout/personal-layout.component';
// import { AdminLayoutComponent } from './layouts/admin-layout/admin-layout.component';
// import { LoginComponent } from './login/login.component';

const routes: Routes =[
  {
    path: '',
    redirectTo: 'Phome',
    pathMatch: 'full',
  }, 


  {
    path: '',
    component: AdminLayoutComponent,
    children: [{
      path: '',
      // loadChildren: './core/components/app/layouts/admin-layout/admin-layout.module#AdminLayoutModule'
      loadChildren: './core/modules/admin-layout.module#AdminLayoutModule'
    }]
  },
  {
    path: '',
    component: PersonalLayoutComponent,
    children: [{
      path: '',
      // loadChildren: './core/components/app/layouts/admin-layout/admin-layout.module#AdminLayoutModule'
      loadChildren: './core/modules/personal-layout.module#PersonalLayoutModule'
    }],
    
  }
];

@NgModule({
  imports: [
    CommonModule,
    BrowserModule,
    RouterModule.forRoot(routes,{
       useHash: true
    })
  ],
  exports: [
    [RouterModule]
  ],
})
export class AppRoutingModule { }
