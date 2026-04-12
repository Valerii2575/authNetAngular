import { Routes } from '@angular/router';
import { User } from './user/user';
import { userInfo } from 'os';
import { register } from 'module';
import { Registration } from './user/registration/registration';
import { Login } from './user/login/login';
import { Dashboard } from './dashboard/dashboard';
import { authGuard } from './shared/auth-guard';

export const routes: Routes = [
    {
        path: '', redirectTo: '/user/signin', pathMatch: 'full'
    },
    {
        path: '', component: Dashboard
    },
    {        
        path: 'user', component: User,
        children: [
            {path: 'signup', component: Registration},
            {path: 'signin', component: Login}
        ]
    },
    {
        path: 'dashboard', component: Dashboard,
        canActivate: [authGuard]
    }
];
