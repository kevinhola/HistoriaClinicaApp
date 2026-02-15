using System;
using Postgrest.Attributes;
using Postgrest.Models;

namespace HistoriaClinicaApp.Models
{
    [Table("archivos_medicos")]
    public class ArchivoMedico : BaseModel
    {
        [PrimaryKey("id", false)]
        public long Id { get; set; }
        [Column("paciente_id")]
        public long PacienteId { get; set; }
        [Column("nombre_archivo")]
        public string NombreArchivo { get; set; }
        [Column("ruta_archivo")]
        public string RutaArchivo { get; set; }
        [Column("tipo_archivo")]
        public string TipoArchivo { get; set; }
        [Column("tamano_bytes")]
        public long TamanoBytes { get; set; }
        [Column("usuario_id")]
        public long UsuarioId { get; set; }
        [Column("cifrado")]
        public bool Cifrado { get; set; }
        [Column("created_at")]
        public DateTime FechaSubida { get; set; }
    }
}

