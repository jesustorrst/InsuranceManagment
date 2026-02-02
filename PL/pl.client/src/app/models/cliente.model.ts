export interface Cliente {
  idCliente?: number;
  numeroIdentificacion: string;
  nombre: string;
  apellidoPaterno: string;
  apellidoMaterno?: string;
  email: string;
  telefono: string;
  // Campos de auditoría (opcionales en el front, pero útiles)
  fechaCreacion?: Date;
  eliminado?: boolean;
}
