import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Cliente } from '../models/cliente.model';
import { Result } from '../models/result.model';


@Injectable({
  providedIn: 'root'
})
export class ClienteService {

  private baseUrl = 'https://localhost:7014/api/Cliente';

  constructor(private http: HttpClient) { }

  getAll(): Observable<Result<Cliente>> {
    return this.http.get<Result<Cliente>>(this.baseUrl);
  }

  getById(id: number): Observable<Result<Cliente>> {
    return this.http.get<Result<Cliente>>(`${this.baseUrl}/${id}`);
  }

  // Registrar (Post)
  add(cliente: Cliente): Observable<Result<number>> {
    return this.http.post<Result<number>>(this.baseUrl, cliente);
  }

  // Actualizar (Put)
  update(id: number, cliente: Cliente): Observable<Result<boolean>> {
    return this.http.put<Result<boolean>>(`${this.baseUrl}/${id}`, cliente);
  }

  // Eliminar (Delete)
  delete(id: number): Observable<Result<boolean>> {
    return this.http.delete<Result<boolean>>(`${this.baseUrl}/${id}`);
  }
}
