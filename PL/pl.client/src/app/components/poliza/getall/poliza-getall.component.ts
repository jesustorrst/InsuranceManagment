import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { ClienteService } from '../../../services/cliente.service';
import { PolizaService } from '../../../services/poliza.service';
import { Poliza } from '../../../models/poliza.model';
import { Cliente } from '../../../models/cliente.model';
import { AuthService } from '../../../services/auth.service';
import Swal from 'sweetalert2';

@Component({
  selector: 'app-poliza-getall',
  standalone: true,
  imports: [CommonModule, RouterModule, FormsModule],
  templateUrl: './poliza-getall.component.html',
  styleUrls: ['./poliza-getall.component.css']
})
export class GetallComponent implements OnInit {

  clientes: Cliente[] = [];
  polizas: Poliza[] = [];
  polizasOriginal: Poliza[] = []; 

  public clienteSeleccionado: Cliente | null = null;
  loadingClientes: boolean = false;
  loadingPolizas: boolean = false;

  filtros = {
    tipo: '',
    estado: 'Todas',
    fechaInicio: '',
    fechaFin: ''
  };

  constructor(
    private clienteService: ClienteService,
    private polizaService: PolizaService,
    public authService: AuthService
  ) { }

  ngOnInit(): void {
    const rol = this.authService.getStoredRol();
    const idLogueado = this.authService.getStoredIdCliente();

    if (rol === '2') {
      const clienteFake = { idCliente: idLogueado, nombre: 'Cliente' } as any as Cliente;
      this.seleccionarCliente(clienteFake);
    } else {
      this.cargarClientes();
    }

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

  solicitarCancelacion(id: number): void {
    Swal.fire({
      title: '¿Deseas cancelar esta póliza?',
      text: "Esta acción cambiará el estado a Inactiva",
      icon: 'question',
      showCancelButton: true,
      confirmButtonColor: '#d33',
      confirmButtonText: 'Sí, cancelar'
    }).then((result) => {
      if (result.isConfirmed) {
        // Enviamos el objeto con el nuevo estado
        this.polizaService.update(id, { estado: 'Inactiva' }).subscribe({
          next: (res) => {
            if (res.correct) {
              Swal.fire('Cancelada', 'La póliza ha sido cancelada.', 'success');
              if (this.clienteSeleccionado) this.seleccionarCliente(this.clienteSeleccionado);
            }
          }
        });
      }
    });
  }

  seleccionarCliente(cliente: Cliente): void {
    if (!cliente || !cliente.idCliente) return;

    this.clienteSeleccionado = cliente;
    this.loadingPolizas = true;

    // --- PASO 1: Limpieza preventiva ---
    this.polizas = [];
    this.polizasOriginal = [];
    // ------------------------------------

    this.filtros = { tipo: '', estado: 'Todas', fechaInicio: '', fechaFin: '' };

    this.polizaService.getByIdCliente(cliente.idCliente).subscribe({
      next: (result) => {
        // Verificamos que sea correcto Y que realmente existan objetos con longitud > 0
        if (result.correct && result.objects && result.objects.length > 0) {
          this.polizasOriginal = result.objects;
          this.polizas = [...this.polizasOriginal];
        } else {
          // --- PASO 2: Si no hay pólizas, aseguramos que quede vacío ---
          this.polizasOriginal = [];
          this.polizas = [];
        }
        this.loadingPolizas = false;
      },
      error: () => {
        this.loadingPolizas = false;
        this.polizasOriginal = []; // Limpiar también en caso de error
        this.polizas = [];
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
