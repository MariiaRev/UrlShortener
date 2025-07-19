import { Routes } from '@angular/router';
import { Login } from './login/login-register';
import { ShortUrlList } from './short-url-list/short-url-list';

export const routes: Routes = [
  { path: '', redirectTo: 'login', pathMatch: 'full' },
  { path: 'login', component: Login },
  { path: 'urls', component: ShortUrlList }
];