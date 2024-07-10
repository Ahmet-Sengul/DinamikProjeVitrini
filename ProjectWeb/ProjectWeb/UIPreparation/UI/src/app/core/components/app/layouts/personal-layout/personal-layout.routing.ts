import { Routes } from '@angular/router';
import { LoginGuard } from 'app/core/guards/login-guard';
import { BlogComponent } from 'app/core/components/personal/blog/blog.component';
import { AboutComponent } from 'app/core/components/personal/about/about.component';
import { HomePageComponent } from 'app/core/components/personal/home-page/home-page.component';
import { TeamComponent } from 'app/core/components/personal/team/team.component';
import { TechnologicalInfrastructureComponent } from 'app/core/components/personal/technological-infrastructure/technological-infrastructure.component';
import { ContactComponent } from 'app/core/components/personal/contact/contact.component';





export const PersonalLayoutRoutes: Routes = [

    { path: 'Ptec', component: TechnologicalInfrastructureComponent },
    { path: 'P', pathMatch: 'full', component: HomePageComponent },
    { path: 'Pabout', component: AboutComponent },
    { path: 'Phome', component: HomePageComponent },
    { path: 'Pteam', component: TeamComponent },
    { path: 'Pcontact', component: ContactComponent },

];
