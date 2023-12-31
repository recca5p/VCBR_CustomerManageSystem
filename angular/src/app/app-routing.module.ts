import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

const routes: Routes = [
  {
    path: '',
    pathMatch: 'full',
    loadChildren: () => import('./pages/home/home.module').then(m => m.HomeModule),
  },
  {
    path: 'account',
    loadChildren: () => import('@abp/ng.account').then(m => m.AccountModule.forLazy()),
  },
  {
    path: 'identity',
    loadChildren: () => import('@abp/ng.identity').then(m => m.IdentityModule.forLazy()),
  },
  {
    path: 'tenant-management',
    loadChildren: () =>
      import('@abp/ng.tenant-management').then(m => m.TenantManagementModule.forLazy()),
  },
  {
    path: 'setting-management',
    loadChildren: () =>
      import('@abp/ng.setting-management').then(m => m.SettingManagementModule.forLazy()),
  },
  {
    path: 'customers',
    loadChildren: () => import('./pages/customer/customer.module').then(m => m.CustomerModule),
  },
  {
    path: 'customer-info',
    loadChildren: () =>
      import('./pages/customer-info/customer-info.module').then(m => m.CustomerInfoModule),
  },
  {
    path: 'imports',
    loadChildren: () => import('./pages/import/import.module').then(m => m.ImportModule),
  },
  {
    path: 'exports',
    loadChildren: () => import('./pages/export/export.module').then(m => m.ExportModule),
  },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule],
})
export class AppRoutingModule {}
