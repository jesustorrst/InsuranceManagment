import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { ClienteService } from '../../../services/cliente.service';
import { PolizaService } from '../../../services/poliza.service';
import { Poliza } from '../../../models/poliza.model';
import { Cliente } from '../../../models/cliente.model';
import Swal from 'sweetalert2';

@Component({
  selector: 'app-poliza-getall',
  standalone: true,
  imports: [CommonModule, RouterModule],
  templateUrl: './poliza-getall.component.html',
  styleUrls: ['./poliza-getall.component.css']
})
export class GetallComponent implements OnInit {

  clientes: Cliente[] = [];
  polizas: Poliza[] = [];

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
        }
        this.loadingClientes = false;
      },
      error: () => {
        this.loadingClientes = false;
        Swal.fire('Error', 'No se pudieron obtener los clientes', 'error');
      }
    });
  }

  seleccionarCliente(cliente: Cliente): void {
    if (!cliente || !cliente.idCliente) return;

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
      error: () => {
        this.loadingPolizas = false;
        Swal.fire('Error', 'No se pudieron obtener las pólizas', 'error');
      }
    });
  }

  eliminarPoliza(id: number): void {
    Swal.fire({
      title: '¿Eliminar póliza?',
      text: "Esta acción no se puede deshacer",
      icon: 'warning',
      showCancelButton: true,
      confirmButtonText: 'Sí, eliminar'
    }).then((result) => {
      if (result.isConfirmed) {
        this.polizaService.delete(id).subscribe({
          next: (res) => {
            if (res.correct && this.clienteSeleccionado) {
              this.seleccionarCliente(this.clienteSeleccionado);
            }
          }
        });
      }
    });
  }
}
