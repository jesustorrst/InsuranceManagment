import { inject, Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, tap } from 'rxjs';
import { environment } from '../../environments/environment';
import { Router } from '@angular/router';
import { BehaviorSubject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private http = inject(HttpClient);
  private router = inject(Router);

  private loggedIn = new BehaviorSubject<boolean>(!!localStorage.getItem('token'));
  isLoggedIn$ = this.loggedIn.asObservable();

  private apiUrl = `${environment.apiUrl}/Usuario`;

  login(credentials: any): Observable<any> {
    return this.http.post<any>(`${this.apiUrl}/Login`, credentials).pipe(
      tap(response => {
        if (response.correct && response.object.token) {

          const info = response.object;

          localStorage.setItem('token', info.token);
          localStorage.setItem('rol', info.rol);
         
          if (info.idCliente) {
            localStorage.setItem('idCliente', info.idCliente.toString());
          }

          this.loggedIn.next(true);
        }
      })
    );
  }

  getStoredRol(): string | null {
    return localStorage.getItem('rol');
  }

  getStoredIdCliente(): string | null {
    return localStorage.getItem('idCliente');
  }


  logout() {
    localStorage.clear();
    this.loggedIn.next(false);
    this.router.navigate(['/login']);
  }

  isLoggedIn(): boolean {
    return !!localStorage.getItem('token');
  }
}
