using System;
using Postgrest.Attributes;
using Postgrest.Models;

namespace HistoriaClinicaApp.Models
{
    [Table("consultas")]
    public class Consulta : BaseModel
    {
        [PrimaryKey("id", false)]
        public long Id { get; set; }
        [Column("paciente_id")]
        public long PacienteId { get; set; }
        [Column("usuario_id")]
        public long UsuarioId { get; set; }
        [Column("fecha_hora")]
        public DateTime FechaHora { get; set; }
        [Column("motivo")]
        public string Motivo { get; set; }
        [Column("sintomas")]
        public string Sintomas { get; set; }
        [Column("diagnostico")]
        public string Diagnostico { get; set; }
        [Column("tratamiento")]
        public string Tratamiento { get; set; }
        [Column("notas_medicas")]
        public string NotasMedicas { get; set; }
        [Column("peso")]
        public double Peso { get; set; }
        [Column("altura")]
        public double Altura { get; set; }
        [Column("presion_arterial")]
        public string PresionArterial { get; set; }
        [Column("temperatura")]
        public double Temperatura { get; set; }
        public string NombreUsuario { get; set; }
    }
}

