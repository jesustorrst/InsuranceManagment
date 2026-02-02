import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router, RouterModule } from '@angular/router';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { PolizaService } from '../../../services/poliza.service';
import { Poliza } from '../../../models/poliza.model';
import Swal from 'sweetalert2';

@Component({
  selector: 'app-poliza-form',
  standalone: true,
  imports: [CommonModule, FormsModule, RouterModule],
  templateUrl: './poliza-form.component.html',
  styleUrls: ['./poliza-form.component.css']

})
export class PolizaFormComponent implements OnInit {

  poliza: Poliza = {
    idPoliza: 0,
    idCliente: 0,
    idTipoPoliza: 0,
    montoAsegurado: 0,
    fechaInicio: '',
    fechaFin: '',
    estado: 'Activa',
    eliminado: false
  };

  tiposPoliza = [
    { id: 1, nombre: 'Vida' },
    { id: 2, nombre: 'Automóvil' },
    { id: 3, nombre: 'Salud' },
    { id: 4, nombre: 'Hogar' }
  ];

  isEdit: boolean = false;

  constructor(
    private polizaService: PolizaService,
    private activatedRoute: ActivatedRoute,
    private router: Router
  ) { }

  ngOnInit(): void {
    const idCliente = this.activatedRoute.snapshot.params['idCliente'];
    const idPoliza = this.activatedRoute.snapshot.params['idPoliza'];

    this.poliza.idCliente = Number(idCliente);

    if (idPoliza) {
      this.isEdit = true;
      this.cargarPoliza(Number(idPoliza));
    }
  }

  validarFormulario(): boolean {
    if (this.poliza.montoAsegurado <= 0) {
      Swal.fire('Error', 'El monto debe ser mayor a cero.', 'error');
      return false;
    }

    const inicio = new Date(this.poliza.fechaInicio);
    const fin = new Date(this.poliza.fechaFin);

    if (fin <= inicio) {
      Swal.fire('Error', 'La fecha de expiración debe ser posterior a la fecha de inicio.', 'error');
      return false;
    }

    return true;
  }
  cargarPoliza(id: number) {
    this.polizaService.getById(id).subscribe({
      next: (res) => {
        if (res.correct && res.object) {
          this.poliza = res.object;
          this.poliza.fechaInicio = this.poliza.fechaInicio.split('T')[0];
          this.poliza.fechaFin = this.poliza.fechaFin.split('T')[0];
        }
      }
    });
  }

  onSubmit() {
    if (this.validarFormulario()) {
      if (this.isEdit) {
        this.actualizar();
      } else {
        this.guardar();
      }
    }
  }

  guardar() {
    this.polizaService.add(this.poliza).subscribe({
      next: (res) => {
        if (res.correct) {
          Swal.fire('Éxito', 'Póliza creada correctamente', 'success');
          this.router.navigate(['/poliza']);
        }
      }
    });
  }

  actualizar() {
    this.polizaService.update(this.poliza.idPoliza, this.poliza).subscribe({
      next: (res) => {
        if (res.correct) {
          Swal.fire('Actualizado', 'Póliza actualizada correctamente', 'success');
          this.router.navigate(['/poliza']);
        }
      }
    });
  }
}
