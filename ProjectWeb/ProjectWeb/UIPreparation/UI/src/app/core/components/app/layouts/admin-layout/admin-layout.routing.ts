import { Routes } from '@angular/router';
import { GroupComponent } from 'app/core/components/admin/group/group.component';
import { LanguageComponent } from 'app/core/components/admin/language/language.component';
import { LogDtoComponent } from 'app/core/components/admin/log/logDto.component';
import { LoginComponent } from 'app/core/components/admin/login/login.component';
import { OperationClaimComponent } from 'app/core/components/admin/operationclaim/operationClaim.component';
import { TranslateComponent } from 'app/core/components/admin/translate/translate.component';
import { UserComponent } from 'app/core/components/admin/user/user.component';
import { LoginGuard } from 'app/core/guards/login-guard';
import { DashboardComponent } from '../../dashboard/dashboard.component';
import { BlogComponent } from 'app/core/components/admin/blog/blog.component';
import { AboutComponent } from 'app/core/components/admin/about/about.component';
import { HomePageComponent } from 'app/core/components/admin/homePage/homePage.component';
import { TeamComponent } from 'app/core/components/admin/team/team.component';
import { TecnologyComponent } from 'app/core/components/admin/tecnology/tecnology.component';
import { TeamRoleComponent } from 'app/core/components/admin/teamRole/teamRole.component';
import { ContactComponent } from 'app/core/components/admin/contact/contact.component';
import { MemberComponent } from 'app/core/components/admin/member/member.component';
import { ContactMessageComponent } from 'app/core/components/admin/contactMessage/contactMessage.component';





export const AdminLayoutRoutes: Routes = [

    { path: 'dashboard',      component: DashboardComponent,canActivate:[LoginGuard] }, 
    { path: 'user',           component: UserComponent, canActivate:[LoginGuard] },
    { path: 'group',          component: GroupComponent, canActivate:[LoginGuard] },
    { path: 'login',          component: LoginComponent },
    { path: 'language',       component: LanguageComponent,canActivate:[LoginGuard]},
    { path: 'translate',      component: TranslateComponent,canActivate:[LoginGuard]},
    { path: 'operationclaim', component: OperationClaimComponent,canActivate:[LoginGuard]},
    { path: 'log',            component: LogDtoComponent,canActivate:[LoginGuard]},
    { path: 'blog',            component: BlogComponent,canActivate:[LoginGuard]},
    { path: 'about',            component: AboutComponent,canActivate:[LoginGuard]},
    { path: 'home-page',            component: HomePageComponent,canActivate:[LoginGuard]},
    { path: 'team',            component: TeamComponent,canActivate:[LoginGuard]},
    { path: 'tecnology',            component: TecnologyComponent,canActivate:[LoginGuard]},
    { path: 'team-role',            component: TeamRoleComponent,canActivate:[LoginGuard]},
    { path: 'contact',            component: ContactComponent,canActivate:[LoginGuard]},
    { path: 'member',            component: MemberComponent,canActivate:[LoginGuard]},
    { path: 'contact-message',            component: ContactMessageComponent,canActivate:[LoginGuard]},
];
