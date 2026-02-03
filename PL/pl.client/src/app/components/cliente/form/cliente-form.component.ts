import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router, RouterModule } from '@angular/router';
import { ClienteService } from '../../../services/cliente.service';
import { Cliente } from '../../../models/cliente.model';
import { AuthService } from '../../../services/auth.service';

import Swal from 'sweetalert2';

@Component({
  selector: 'app-cliente-form',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, RouterModule],
  templateUrl: './cliente-form.component.html',
  styleUrls: ['./cliente-form.component.css']
})
export class ClienteFormComponent implements OnInit {
  form: FormGroup;
  idCliente?: number;
  isEdit: boolean = false;
  idRol: string | null = null;

  constructor(
    private fb: FormBuilder,
    private _clienteService: ClienteService,
    private router: Router,
    private aRoute: ActivatedRoute,
    public authService: AuthService
  ) {

    this.idRol = this.authService.getStoredRol();

    this.form = this.fb.group({
      idCliente: [0],
      numeroIdentificacion: ['', [
        Validators.required,
        Validators.pattern('^[0-9]*$'),
        Validators.minLength(10),
        Validators.maxLength(10)
      ]],
      nombre: ['', [
        Validators.required,
        Validators.pattern(/^[a-zA-ZáéíóúÁÉÍÓÚñÑ ]+$/)
      ]],
      apellidoPaterno: ['', [
        Validators.required,
        Validators.pattern(/^[a-zA-ZáéíóúÁÉÍÓÚñÑ ]+$/)
      ]],
      apellidoMaterno: ['', [
        Validators.pattern(/^[a-zA-ZáéíóúÁÉÍÓÚñÑ ]+$/)
      ]],

      email: ['', [Validators.required, Validators.email]],
      telefono: ['', [Validators.required, Validators.pattern('^[0-9]{10}$')]]
    });

  }


  isInvalid(field: string): boolean {
    const control = this.form.get(field);
    if (field === 'apellidoMaterno') {
      return !!(control && control.invalid && control.dirty);
    }
    return !!(control && control.invalid && control.touched);
  }

  getErrorMessage(field: string): string {
    const control = this.form.get(field);
    if (!control || !control.errors) return '';

    if (control.hasError('required')) return 'Este campo es obligatorio.';
    if (control.hasError('pattern')) {
      if (field === 'email') return 'Formato de correo no válido.';
      if (field === 'numeroIdentificacion' || field === 'telefono') return 'Solo se permiten números.';
      return 'No se permiten números ni carácteres especiales.';
    }
    if (control.hasError('minlength') || control.hasError('maxlength')) {
      return `Debe tener exactamente 10 dígitos.`;
    }
    if (control.hasError('email')) return 'Ingrese un correo electrónico válido.';

    return 'Campo inválido.';
  }

  ngOnInit(): void {
    let idUrl = this.aRoute.snapshot.paramMap.get('id');
    const idLogueado = this.authService.getStoredIdCliente();

    if (idUrl) {
      this.idCliente = parseInt(idUrl, 10);
      this.isEdit = true;
      this.obtenerCliente(this.idCliente);
    }
    else
      if (this.idRol === '2' && idLogueado) {

        this.idCliente = parseInt(idLogueado, 10);
        this.isEdit = true;
        this.obtenerCliente(this.idCliente);
      }


  }

  obtenerCliente(idCliente: number) {
    this._clienteService.getById(idCliente).subscribe(result => {
      if (result.correct && result.object) {
        this.form.patchValue(result.object);
      }
    });
  }

  guardar() {
    if (this.form.invalid) return;

    const cliente: Cliente = this.form.value;

    if (this.isEdit) {
      this._clienteService.update(this.idCliente!, cliente).subscribe(res => {
        if (res.correct) {
          Swal.fire('Actualizado', 'Datos actualizados con éxito', 'success');

          if (this.authService.getStoredRol() === '1') {
            this.router.navigate(['/cliente']);
          }
        }
      });
    } else {
      this._clienteService.add(cliente).subscribe(res => {
        if (res.correct) {
          Swal.fire('Registrado', 'Cliente registrado con éxito', 'success');
          this.router.navigate(['/cliente']);
        }
      });
    }
  }
}
