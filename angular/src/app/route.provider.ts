import { RoutesService, eLayoutType } from '@abp/ng.core';
import { APP_INITIALIZER } from '@angular/core';

export const APP_ROUTE_PROVIDER = [
  { provide: APP_INITIALIZER, useFactory: configureRoutes, deps: [RoutesService], multi: true },
];

function configureRoutes(routesService: RoutesService) {
  return () => {
    routesService.add([
      {
        path: '/',
        name: '::Menu:Home',
        iconClass: 'fas fa-home',
        order: 1,
        layout: eLayoutType.application,
      },
      {
        path: '/customers',
        name: 'Customer Management',
        iconClass: 'fas fa-user',
        order: 2,
        layout: eLayoutType.application,
        requiredPolicy: 'VCBRDemo.Customers.GetList',
      },
      {
        path: '/customer-info',
        name: 'Info',
        iconClass: 'fas fa-info',
        order: 3,
        layout: eLayoutType.application,
        requiredPolicy: 'VCBRDemo.Customers.DetailInfo',
      },
      {
        path: '/imports',
        name: 'Import',
        iconClass: 'fas fa-upload',
        order: 4,
        layout: eLayoutType.application,
        requiredPolicy: 'VCBRDemo.Customers.ImportFile',
      },
      {
        path: '/exports',
        name: 'Export',
        iconClass: 'fas fa-download',
        order: 5,
        layout: eLayoutType.application,
        requiredPolicy: 'VCBRDemo.Customers.ExportFile',
      },
    ]);
  };
}
