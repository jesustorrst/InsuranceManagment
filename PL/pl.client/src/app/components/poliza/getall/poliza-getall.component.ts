import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { FormsModule } from '@angular/forms'; // 1. IMPORTANTE: Para que funcione [(ngModel)]
import { ClienteService } from '../../../services/cliente.service';
import { PolizaService } from '../../../services/poliza.service';
import { Poliza } from '../../../models/poliza.model';
import { Cliente } from '../../../models/cliente.model';
import Swal from 'sweetalert2';

@Component({
  selector: 'app-poliza-getall',
  standalone: true,
  imports: [CommonModule, RouterModule, FormsModule], // 2. IMPORTANTE: Agregado FormsModule
  templateUrl: './poliza-getall.component.html',
  styleUrls: ['./poliza-getall.component.css']
})
export class GetallComponent implements OnInit {

  clientes: Cliente[] = [];
  polizas: Poliza[] = [];
  polizasOriginal: Poliza[] = []; // 3. Respaldo para el filtrado local

  public clienteSeleccionado: Cliente | null = null;
  loadingClientes: boolean = false;
  loadingPolizas: boolean = false;

  // 4. Objeto para los filtros vinculados al HTML
  filtros = {
    tipo: '',
    estado: 'Todas',
    fechaInicio: '',
    fechaFin: ''
  };

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

    // Reiniciamos los filtros cada que se cambia de cliente
    this.filtros = { tipo: '', estado: 'Todas', fechaInicio: '', fechaFin: '' };

    this.polizaService.getByIdCliente(cliente.idCliente).subscribe({
      next: (result) => {
        if (result.correct && result.objects) {
          this.polizasOriginal = result.objects; // Guardamos la fuente original
          this.polizas = [...this.polizasOriginal]; // Mostramos todas inicialmente
        } else {
          this.polizasOriginal = [];
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

  // 5. Función que realiza el filtrado
  filtrarPolizas(): void {
    this.polizas = this.polizasOriginal.filter(p => {
      // Filtro por Tipo (si está vacío, pasan todos)
      const cumpleTipo = !this.filtros.tipo || p.nombreTipoPoliza === this.filtros.tipo;

      // Filtro por Estado (si es 'Todas', pasan todos)
      const cumpleEstado = this.filtros.estado === 'Todas' || p.estado === this.filtros.estado;

      // Filtro por Fechas
      const fechaP = p.fechaInicio ? new Date(p.fechaInicio).getTime() : 0;
      const inicio = this.filtros.fechaInicio ? new Date(this.filtros.fechaInicio).getTime() : null;
      const fin = this.filtros.fechaFin ? new Date(this.filtros.fechaFin).getTime() : null;

      const cumpleFecha = (!inicio || fechaP >= inicio) && (!fin || fechaP <= fin);

      return cumpleTipo && cumpleEstado && cumpleFecha;
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
