using System;

namespace HistoriaClinicaApp.Models
{
    public class Estudio
    {
        public int Id { get; set; }
        public int PacienteId { get; set; }
        public string TipoEstudio { get; set; }
        public string Resultado { get; set; }
        public DateTime Fecha { get; set; }
        public string Observaciones { get; set; }
        public int UsuarioId { get; set; }
        public string NombreUsuario { get; set; }

    public string Estado { get; set; }
    public DateTime? FechaResultado { get; set; }

    }
}