export interface Poliza {
  idPoliza: number;
  idCliente: number;
  idTipoPoliza: number;
  fechaInicio: string;
  fechaFin: string;
  montoAsegurado: number;
  estado: string;
  eliminado: boolean;

  nombreTipoPoliza?: string;     
  descripcionTipoPoliza?: string;
}
