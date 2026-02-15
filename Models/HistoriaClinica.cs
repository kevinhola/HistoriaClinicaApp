using System;
using Postgrest.Attributes;
using Postgrest.Models;

namespace HistoriaClinicaApp.Models
{
    [Table("historias_clinicas")]
    public class HistoriaClinica : BaseModel
    {
        [PrimaryKey("id", false)]
        public long Id { get; set; }
        [Column("paciente_id")]
        public long PacienteId { get; set; }
        [Column("antecedentes")]
        public string Antecedentes { get; set; }
        [Column("alergias")]
        public string Alergias { get; set; }
        [Column("medicacion_habitual")]
        public string MedicacionHabitual { get; set; }
        [Column("grupo_sanguineo")]
        public string GrupoSanguineo { get; set; }
        [Column("observaciones")]
        public string Observaciones { get; set; }
        [Column("usuario_creacion")]
        public long UsuarioCreacion { get; set; }
        [Column("usuario_modificacion")]
        public long UsuarioModificacion { get; set; }
    }
}

