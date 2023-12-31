import { Environment } from '@abp/ng.core';

const baseUrl = 'https://vtpdemo.azurewebsites.net';

export const environment = {
  production: true,
  application: {
    baseUrl,
    name: 'VCBRDemo',
    logoUrl: '',
  },
  oAuthConfig: {
    issuer: 'https://vcbrdemohttpapihost20230714102118.azurewebsites.net/',
    redirectUri: baseUrl,
    clientId: 'VCBRDemo_App',
    responseType: 'code',
    scope: 'offline_access VCBRDemo',
    requireHttps: true,
  },
  apis: {
    default: {
      url: 'https://vcbrdemohttpapihost20230714102118.azurewebsites.net',
      rootNamespace: 'VCBRDemo',
    },
  },
} as Environment;
