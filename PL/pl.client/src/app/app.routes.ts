import { Routes } from '@angular/router';

import { GetallComponent  } from './components/cliente/getall/cliente-getall.component';
import { ClienteFormComponent } from './components/cliente/form/cliente-form.component'; // (Si este ya tiene nombre único)

import { GetallComponent as PolizaGetallComponent } from './components/poliza/getall/poliza-getall.component';
import { PolizaFormComponent } from './components/poliza/form/poliza-form.component';

import { LandingComponent } from './components/shared/landing/landing.component';

export const routes: Routes = [
  { path: '', component: LandingComponent },

  { path: 'cliente', component: GetallComponent },
  { path: 'cliente/form', component: ClienteFormComponent },
  { path: 'cliente/form/:id', component: ClienteFormComponent },

  { path: 'poliza', component: PolizaGetallComponent },
  { path: 'poliza/cliente/:idCliente', component: PolizaGetallComponent },


  { path: 'poliza/form/:idCliente', component: PolizaFormComponent },
  { path: 'poliza/form/:idCliente/:idPoliza', component: PolizaFormComponent },

  { path: '**', redirectTo: '', pathMatch: 'full' }
];
