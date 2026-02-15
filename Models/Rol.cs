using Postgrest.Attributes;
using Postgrest.Models;

namespace HistoriaClinicaApp.Models
{
    [Table("roles")]
    public class Rol : BaseModel
    {
        [PrimaryKey("id", false)]
        public long Id { get; set; }
        [Column("nombre")]
        public string Nombre { get; set; }
        [Column("descripcion")]
        public string Descripcion { get; set; }
    }
}

