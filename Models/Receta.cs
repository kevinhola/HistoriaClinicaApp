using System;
using Postgrest.Attributes;
using Postgrest.Models;

namespace HistoriaClinicaApp.Models
{
    [Table("recetas")]
    public class Receta : BaseModel
    {
        [PrimaryKey("id", false)]
        public long Id { get; set; }
        [Column("consulta_id")]
        public long ConsultaId { get; set; }
        [Column("texto_medico")]
        public string TextoMedico { get; set; }
        [Column("usuario_id")]
        public long UsuarioId { get; set; }
        [Column("created_at")]
        public DateTime Fecha { get; set; }
        public string NombreUsuario { get; set; }
    }
}

