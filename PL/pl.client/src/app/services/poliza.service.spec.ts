import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Poliza } from '../models/poliza.model';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class PolizaService {
  // Asegúrate de que en tu environment la URL termine en /api
  private apiUrl = `${environment.apiUrl}/Poliza`;

  constructor(private http: HttpClient) { }

  /**
   * ESTE ES EL MÉTODO QUE NECESITAS AHORA
   * Trae las pólizas asociadas a un cliente específico
   */
  getByIdCliente(idCliente: number): Observable<any> {
    return this.http.get(`${this.apiUrl}/GetByIdCliente/${idCliente}`);
  }

  // Obtener una póliza específica (para editar)
  getById(idPoliza: number): Observable<any> {
    return this.http.get(`${this.apiUrl}/GetById/${idPoliza}`);
  }

  // Guardar nueva póliza
  add(poliza: Poliza): Observable<any> {
    return this.http.post(`${this.apiUrl}/Add`, poliza);
  }

  // Actualizar póliza existente
  update(idPoliza: number, poliza: Poliza): Observable<any> {
    return this.http.put(`${this.apiUrl}/Update/${idPoliza}`, poliza);
  }

  // Eliminar (Baja lógica o física según tu Backend)
  delete(idPoliza: number): Observable<any> {
    return this.http.delete(`${this.apiUrl}/Delete/${idPoliza}`);
  }
}
