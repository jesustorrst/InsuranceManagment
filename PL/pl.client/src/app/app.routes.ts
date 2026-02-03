import { Routes } from '@angular/router';
import { GetallComponent as ClienteGetallComponent } from './components/cliente/getall/cliente-getall.component';
import { ClienteFormComponent } from './components/cliente/form/cliente-form.component';

import { GetallComponent as PolizaGetallComponent } from './components/poliza/getall/poliza-getall.component';
import { PolizaFormComponent } from './components/poliza/form/poliza-form.component';

import { LandingComponent } from './components/shared/landing/landing.component';

import { LoginComponent } from './components/shared/login/login.component';

export const routes: Routes = [

  { path: '', redirectTo: 'login', pathMatch: 'full' },

  { path: 'login', component: LoginComponent },

  // Rutas de Cliente
  { path: 'cliente', component: ClienteGetallComponent },
  { path: 'cliente/form', component: ClienteFormComponent },
  { path: 'cliente/form/:id', component: ClienteFormComponent },

  // Rutas de Póliza
  { path: 'poliza', component: PolizaGetallComponent },
  { path: 'poliza/cliente/:idCliente', component: PolizaGetallComponent },
  { path: 'poliza/form/:idCliente', component: PolizaFormComponent },
  { path: 'poliza/form/:idCliente/:idPoliza', component: PolizaFormComponent },

  { path: '**', redirectTo: '', pathMatch: 'full' }
];
