import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';
import { Poliza } from '../models/poliza.model';

import { Result } from '../models/result.model';

@Injectable({
  providedIn: 'root'
})
export class PolizaService {

  private apiUrl = 'https://localhost:7014/api/Poliza';
  //private apiUrl = `${(environment as any).apiUrl}/Poliza`;
  constructor(private http: HttpClient) { }

  //getByIdCliente(idCliente: number): Observable<Result<Poliza>> {
  //  return this.http.get<Result<Poliza>>(`${this.apiUrl}/GetByIdCliente/${idCliente}`);
  //}

  getByIdCliente(idCliente: number): Observable<Result<Poliza>> {
    // Agregamos un console.log para ver qué está pasando justo antes de la llamada
    console.log('Llamando a:', `${this.apiUrl}/GetByIdCliente/${idCliente}`);
    return this.http.get<Result<Poliza>>(`${this.apiUrl}/GetByIdCliente/${idCliente}`);
  }

  getById(id: number): Observable<Result<Poliza>> {
    return this.http.get<Result<Poliza>>(`${this.apiUrl}/${id}`);
  }

  add(poliza: Poliza): Observable<Result<number>> {
    return this.http.post<Result<number>>(this.apiUrl, poliza);
  }

  update(id: number, poliza: any): Observable<Result<boolean>> {
    return this.http.put<Result<boolean>>(`${this.apiUrl}/${id}`, poliza);
  }

  delete(id: number): Observable<Result<boolean>> {
    return this.http.delete<Result<boolean>>(`${this.apiUrl}/${id}`);
  }
}
