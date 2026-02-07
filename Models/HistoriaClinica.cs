using System;

namespace HistoriaClinicaApp.Models
{
    public class HistoriaClinica
    {
        public int Id { get; set; }
        public int PacienteId { get; set; }
        public string Antecedentes { get; set; }
        public string Alergias { get; set; }
        public string MedicacionHabitual { get; set; }
        public string GrupoSanguineo { get; set; }
        public string Observaciones { get; set; }
        public DateTime FechaCreacion { get; set; }
        public DateTime FechaUltimaModificacion { get; set; }
        public int UsuarioCreacion { get; set; }
        public int UsuarioModificacion { get; set; }
    }
}