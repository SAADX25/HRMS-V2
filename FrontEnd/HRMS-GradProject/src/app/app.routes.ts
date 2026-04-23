import { Routes } from '@angular/router';
import { authGuard, noAuthGuard } from './core/guards/auth.guard';

export const routes: Routes = [
  // Redirect root to login
  {
    path: '',
    redirectTo: 'login',
    pathMatch: 'full'
  },
  // Public route: only accessible when NOT logged in
  {
    path: 'login',
    loadComponent: () =>
      import('./features/login/login.component').then(m => m.LoginComponent),
    canActivate: [noAuthGuard],
    title: 'Sign In — HRMS'
  },
  // Protected routes wrapper: requires authentication
  {
    path: '',
    loadComponent: () => 
      import('./layout/dashboard-layout/dashboard-layout.component').then(m => m.DashboardLayoutComponent),
    canActivate: [authGuard],
    children: [
      {
        path: 'dashboard',
        loadComponent: () =>
          import('./features/dashboard/dashboard.component').then(m => m.DashboardComponent),
        title: 'Dashboard — HRMS'
      },
      // Placeholders for future steps
      {
        path: 'employees',
        loadComponent: () =>
          import('./features/employees/employees.component').then(m => m.EmployeesComponent),
        title: 'Employees — HRMS'
      },
      {
        path: 'departments',
        loadComponent: () =>
          import('./features/departments/departments.component').then(m => m.DepartmentsComponent),
        title: 'Departments — HRMS'
      },
      {
        path: 'leave-requests',
        loadComponent: () =>
          import('./features/leave-requests/leave-requests.component').then(m => m.LeaveRequestsComponent),
        title: 'Leave Requests — HRMS'
      }
    ]
  },
  // Wildcard fallback
  {
    path: '**',
    redirectTo: 'dashboard'
  }
];
