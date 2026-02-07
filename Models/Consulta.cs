using System;

namespace HistoriaClinicaApp.Models
{
    public class Consulta
    {
        public int Id { get; set; }
        public int PacienteId { get; set; }
        public int UsuarioId { get; set; }
        public DateTime FechaHora { get; set; }
        public string Motivo { get; set; }
        public string Sintomas { get; set; }
        public string Diagnostico { get; set; }
        public string Tratamiento { get; set; }
        public string NotasMedicas { get; set; }
        public decimal Peso { get; set; }
        public decimal Altura { get; set; }
        public string PresionArterial { get; set; }
        public decimal Temperatura { get; set; }
        public string NombreUsuario { get; set; }
    }
}