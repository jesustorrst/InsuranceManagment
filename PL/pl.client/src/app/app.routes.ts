import { Routes } from '@angular/router';
import { GetallComponent } from './components/cliente/getall/getall.component';
import { ClienteFormComponent } from './components/cliente/form/form.component';
import { LandingComponent } from './components/shared/landing/landing.component';

export const routes: Routes = [
  { path: '', component: LandingComponent },
  { path: 'clientes', component: GetallComponent },
  { path: 'cliente/form', component: ClienteFormComponent },
  { path: 'cliente/form/:id', component: ClienteFormComponent },
  { path: '', redirectTo: 'clientes', pathMatch: 'full' } // Ruta por defecto
];
