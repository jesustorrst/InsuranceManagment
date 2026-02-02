import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router, RouterModule } from '@angular/router';
import { ClienteService } from '../../../services/cliente.service';
import { Cliente } from '../../../models/cliente.model';
import Swal from 'sweetalert2';

@Component({
  selector: 'app-cliente-form',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule, RouterModule],
  templateUrl: './form.component.html',
  styleUrls: ['./form.component.css']
})
export class ClienteFormComponent implements OnInit {
  form: FormGroup;
  idCliente?: number;
  isEdit: boolean = false;

  constructor(
    private fb: FormBuilder,
    private _clienteService: ClienteService,
    private router: Router,
    private aRoute: ActivatedRoute
  ) {

    this.form = this.fb.group({
      idCliente: [0],
      numeroIdentificacion: ['', [
        Validators.required,
        Validators.pattern('^[0-9]*$'), // Solo números
        Validators.minLength(10),
        Validators.maxLength(10)
      ]],
      nombre: ['', [
        Validators.required,
        Validators.pattern(/^[a-zA-ZáéíóúÁÉÍÓÚñÑ ]+$/) // Quitamos el * y ponemos + para obligar contenido
      ]],
      apellidoPaterno: ['', [
        Validators.required,
        Validators.pattern(/^[a-zA-ZáéíóúÁÉÍÓÚñÑ ]+$/)
      ]],
      apellidoMaterno: ['', [
        // Aquí NO ponemos required, pero SI el pattern
        Validators.pattern(/^[a-zA-ZáéíóúÁÉÍÓÚñÑ ]+$/)
      ]],

      email: ['', [Validators.required, Validators.email]],
      telefono: ['', [Validators.required, Validators.pattern('^[0-9]{10}$')]]
    });

  //  this.idCliente = Number(this.aRoute.snapshot.paramMap.get('id'));
  }

  ngOnInit(): void {
    const idClient = this.aRoute.snapshot.paramMap.get('id');

    if (idClient !== null) {
      this.idCliente = parseInt(idClient, 10);
      this.isEdit = true;
      this.obtenerCliente(this.idCliente);
    }
  }

  obtenerCliente(idCliente: number) {
    this._clienteService.getById(idCliente).subscribe(result => {
      if (result.correct && result.object) {
        this.form.patchValue(result.object); // Rellenamos el formulario
      }
    });
  }

  guardar() {
    if (this.form.invalid) return;

    const cliente: Cliente = this.form.value;

    if (this.isEdit) {
      this._clienteService.update(this.idCliente!, cliente).subscribe(res => {
        if (res.correct) {
          Swal.fire('Actualizado', 'Cliente actualizado con éxito', 'success');
          this.router.navigate(['/clientes']);
        }
      });
    } else {
      this._clienteService.add(cliente).subscribe(res => {
        if (res.correct) {
          Swal.fire('Registrado', 'Cliente registrado con éxito', 'success');
          this.router.navigate(['/clientes']);
        }
      });
    }
  }
}
