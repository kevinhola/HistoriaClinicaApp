using System;
using Postgrest.Attributes;
using Postgrest.Models;

namespace HistoriaClinicaApp.Models
{
    [Table("estudios")]
    public class Estudio : BaseModel
    {
        [PrimaryKey("id", false)]
        public long Id { get; set; }
        [Column("paciente_id")]
        public long PacienteId { get; set; }
        [Column("usuario_id")]
        public long UsuarioId { get; set; }
        [Column("tipo_estudio")]
        public string TipoEstudio { get; set; }
        [Column("resultado")]
        public string Resultado { get; set; }
        [Column("observaciones")]
        public string Observaciones { get; set; }
        [Column("fecha")]
        public DateTime Fecha { get; set; }
        public string NombreUsuario { get; set; }
    }
}

