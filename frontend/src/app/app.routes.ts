import { Routes } from '@angular/router';
import { authGuard } from './core/guards/auth.guard';

export const routes: Routes = [
  { path: '', redirectTo: 'dashboard', pathMatch: 'full' },
  { path: 'login', loadComponent: () => import('./features/auth/login/login.component').then(m => m.LoginComponent) },
  {
    path: '',
    loadComponent: () => import('./shared/layout/main-layout/main-layout.component').then(m => m.MainLayoutComponent),
    canActivate: [authGuard],
    children: [
      { path: 'dashboard',     loadComponent: () => import('./features/dashboard/dashboard.component').then(m => m.DashboardComponent) },
      { path: 'articles',      loadComponent: () => import('./features/articles/articles.component').then(m => m.ArticlesComponent) },
      { path: 'categories',    loadComponent: () => import('./features/categories/categories.component').then(m => m.CategoriesComponent) },
      { path: 'clients',       loadComponent: () => import('./features/clients/clients.component').then(m => m.ClientsComponent) },
      { path: 'fournisseurs',  loadComponent: () => import('./features/fournisseurs/fournisseurs.component').then(m => m.FournisseursComponent) },
      { path: 'commandes',     loadComponent: () => import('./features/commandes/commandes.component').then(m => m.CommandesComponent) },
      { path: 'factures',      loadComponent: () => import('./features/factures/factures.component').then(m => m.FacturesComponent) },
      { path: 'utilisateurs',  loadComponent: () => import('./features/users/users.component').then(m => m.UsersComponent) },
    ]
  },
  { path: '**', redirectTo: 'dashboard' }
];
