import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { ClienteService } from '../../../services/cliente.service';
import { PolizaService } from '../../../services/poliza.service';
import { Poliza } from '../../../models/poliza.model';
import { Cliente } from '../../../models/cliente.model';
import Swal from 'sweetalert2';

@Component({
  selector: 'app-getall',
  standalone: true,
  imports: [CommonModule, RouterModule],
  templateUrl: './poliza-getall.component.html',
  styleUrls: ['./poliza-getall.component.css']
})
export class GetallComponent implements OnInit {

  clientes: Cliente[] = [];
  polizas: Poliza[] = [];

  // Cambio clave: ahora es de tipo Cliente o null
  public clienteSeleccionado: Cliente | null = null;
  loadingClientes: boolean = false;
  loadingPolizas: boolean = false;

  constructor(
    private clienteService: ClienteService,
    private polizaService: PolizaService
  ) { }

  ngOnInit(): void {
    this.cargarClientes();
  }

  cargarClientes(): void {
    this.loadingClientes = true;
    this.clienteService.getAll().subscribe({
      next: (result) => {
        if (result.correct && result.objects) {
          this.clientes = result.objects;
        } else {
          Swal.fire('Error', result.errorMessage || 'No se pudieron obtener los clientes', 'error');
        }
        this.loadingClientes = false;
      },
      error: (err) => {
        Swal.fire('Error', 'Error de conexión con el servidor', 'error');
        this.loadingClientes = false;
      }
    });
  }

  seleccionarCliente(cliente: Cliente): void {
    if (!cliente || !cliente.idCliente) {
      Swal.fire('Error', 'Cliente inválido.', 'error');
      return;
    }

    this.clienteSeleccionado = cliente;
    this.loadingPolizas = true;
    this.polizas = []; 

    this.polizaService.getByIdCliente(cliente.idCliente).subscribe({
      next: (result) => {
        if (result.correct && result.objects) {
          this.polizas = result.objects;
        } else {
          this.polizas = [];
        }
        this.loadingPolizas = false;
      },
      error: (err) => {
        console.error('Error al cargar pólizas', err);
        Swal.fire('Error', 'No se pudieron obtener las pólizas', 'error');
        this.loadingPolizas = false;
      }
    });
  }

  eliminarPoliza(id: number): void {
    Swal.fire({
      title: '¿Deseas eliminar esta póliza?',
      text: "Esta acción no se puede deshacer",
      icon: 'warning',
      showCancelButton: true,
      confirmButtonColor: '#d33',
      cancelButtonColor: '#3085d6',
      confirmButtonText: 'Sí, eliminar',
      cancelButtonText: 'Cancelar'
    }).then((result) => {
      if (result.isConfirmed) {
        this.polizaService.delete(id).subscribe({
          next: (res) => {
            if (res.correct) {
              Swal.fire('Eliminado', 'La póliza ha sido eliminada.', 'success');
              if (this.clienteSeleccionado) {
                this.seleccionarCliente(this.clienteSeleccionado);
              }
            }
          }
        });
      }
    });
  }
}
