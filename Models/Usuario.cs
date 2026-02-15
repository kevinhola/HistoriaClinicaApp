using Postgrest.Attributes;
using Postgrest.Models;

namespace HistoriaClinicaApp.Models
{
    [Table("usuarios")]
    public class Usuario : BaseModel
    {
        [PrimaryKey("id", false)]
        public long Id { get; set; }
        [Column("nombre_usuario")]
        public string NombreUsuario { get; set; }
        [Column("password_hash")]
        public string PasswordHash { get; set; }
        [Column("rol_id")]
        public long RolId { get; set; }
        [Column("activo")]
        public bool Activo { get; set; }
        [Reference(typeof(Rol))]
        public Rol Rol { get; set; }
    }
}

