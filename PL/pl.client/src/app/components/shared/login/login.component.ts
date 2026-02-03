import { Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormBuilder, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { AuthService } from '../../../services/auth.service';
import { MatCardModule } from '@angular/material/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import Swal from 'sweetalert2';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [CommonModule,
    ReactiveFormsModule,
    MatCardModule,
    MatFormFieldModule,
    MatInputModule,
    MatButtonModule,
    MatIconModule],
  templateUrl: './login.component.html',
  styleUrl: './login.component.css'
})

export class LoginComponent {

  private fb = inject(FormBuilder);
  private authService = inject(AuthService);
  private router = inject(Router);

  loginForm: FormGroup = this.fb.group({
    email: ['', [Validators.required, Validators.email]],
    password: ['', [Validators.required, Validators.minLength(3)]]
  });

  onSubmit() {
    if (this.loginForm.valid) {
      this.authService.login(this.loginForm.value).subscribe({
        next: (res) => {
          if (res.correct && res.object) {
            const user = res.object;

            // GUARDAR DATOS CRÍTICOS
            // Es vital guardar el idCliente para que el form lo use en el ngOnInit
            localStorage.setItem('token', user.token);
            localStorage.setItem('rol', user.rol.toString());

            if (user.idCliente) {
              localStorage.setItem('idCliente', user.idCliente.toString());
            }

            // REDIRECCIÓN DINÁMICA
            if (user.rol.toString() === '1') {
              this.router.navigate(['/poliza']);
            } else {
              // Al ir a /cliente/form, el componente hará: 
              // id = localStorage.getItem('idCliente') y cargará los datos
              this.router.navigate(['/cliente/form']);
            }
          } else {
            Swal.fire('Error', 'Credenciales incorrectas', 'error');
          }
        },
        error: (err) => {
          Swal.fire('Error', 'No se pudo conectar con el servidor', 'error');
        }
      });
    }
  }
}
