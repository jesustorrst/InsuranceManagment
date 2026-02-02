import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { ClienteService } from '../../../services/cliente.service';
import { Cliente } from '../../../models/cliente.model';
import Swal from 'sweetalert2';

@Component({
  selector: 'app-cliente-getall',
  standalone: true,
  imports: [CommonModule, RouterModule, FormsModule],
  templateUrl: './cliente-getall.component.html',
  styleUrls: ['./cliente-getall.component.css']
})
export class GetallComponent implements OnInit {
  // Listas para manejar los datos
  clientes: Cliente[] = [];
  clientesOriginal: Cliente[] = [];

  // Variable para el input de búsqueda
  searchTerm: string = '';

  constructor(private clienteService: ClienteService) { }

  ngOnInit(): void {
    this.cargarClientes();
  }

  cargarClientes(): void {
    this.clienteService.getAll().subscribe({
      next: (result) => {
        if (result.correct && result.objects) {
          // Guardamos una copia en 'clientesOriginal' para no perder datos al filtrar
          this.clientesOriginal = [...result.objects];
          this.clientes = [...result.objects];
        } else {
          Swal.fire('Error', 'No se pudieron cargar los clientes', 'error');
        }
      },
      error: () => Swal.fire('Error', 'Error de conexión con el servidor', 'error')
    });
  }

  filtrarClientes(): void {
    const search = this.searchTerm.toLowerCase().trim();

    if (!search) {
      this.clientes = [...this.clientesOriginal];
      return;
    }

    // Filtramos sobre la lista original
    this.clientes = this.clientesOriginal.filter(c => {
      const nombreCompleto = `${c.nombre} ${c.apellidoPaterno} ${c.apellidoMaterno || ''}`.toLowerCase();
      const identificacion = (c.numeroIdentificacion || '').toString().toLowerCase();
      const email = (c.email || '').toLowerCase();

      return nombreCompleto.includes(search) ||
        identificacion.includes(search) ||
        email.includes(search);
    });
  }

  eliminar(id: number | undefined): void {
    if (!id) return;

    Swal.fire({
      title: '¿Eliminar cliente?',
      text: "Esta acción no se puede deshacer",
      icon: 'warning',
      showCancelButton: true,
      confirmButtonColor: '#d33',
      cancelButtonColor: '#3085d6',
      confirmButtonText: 'Sí, eliminar'
    }).then((result) => {
      if (result.isConfirmed) {
        this.clienteService.delete(id).subscribe({
          next: (res) => {
            if (res.correct) {
              Swal.fire('Eliminado', 'Cliente borrado con éxito', 'success');
              this.cargarClientes();
            }
          }
        });
      }
    });
  }
}
