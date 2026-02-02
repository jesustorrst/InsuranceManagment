import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { ClienteService } from '../../../services/cliente.service';
import { Cliente } from '../../../models/cliente.model';
import Swal from 'sweetalert2';

@Component({
  selector: 'app-getall',
  standalone: true,
  imports: [CommonModule, RouterModule],
  templateUrl: './cliente-getall.component.html',
  styleUrl: './cliente-getall.component.css'
})
export class GetallComponent implements OnInit {

  clientes: Cliente[] = [];

  constructor(private clienteService: ClienteService) { }

  ngOnInit(): void {
    this.cargarClientes();
  }

  cargarClientes(): void {
    this.clienteService.getAll().subscribe({
      next: (result) => {
        if (result.correct && result.objects) {
          this.clientes = result.objects;
        } else {
          Swal.fire('Error', result.errorMessage || 'No se pudieron obtener los cliente', 'error');
        }
      },
      error: (err) => {
        Swal.fire('Error', 'Error de conexión con el servidor', 'error');
      }
    });
  }


  eliminar(idCliente: number | undefined): void {

    if (idCliente === undefined) {
      Swal.fire('Error', 'No se puede eliminar un registro ya que no tiene el IdCliente', 'error');
      return;
    }

    Swal.fire({
      title: '¿Estás seguro?',
      text: "El cliente será marcado como inactivo",
      icon: 'warning',
      showCancelButton: true,
      confirmButtonColor: '#3085d6',
      cancelButtonColor: '#d33',
      confirmButtonText: 'Sí, desactivar',
      cancelButtonText: 'Cancelar'
    }).then((confirmacion) => {
      if (confirmacion.isConfirmed) {

        this.clienteService.delete(idCliente).subscribe({
          next: (result) => {
            if (result.correct) {
              Swal.fire('Desactivado', 'El cliente ha sido desactivado con éxito.', 'success');
              this.cargarClientes();
            } else {
              Swal.fire('Error', result.errorMessage, 'error');
            }
          },
          error: () => {
            Swal.fire('Error', 'Ocurrió un error en el servidor', 'error');
          }
        });
      }
    });
  }

 
}

