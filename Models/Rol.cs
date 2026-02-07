namespace HistoriaClinicaApp.Models
{
    public enum TipoRol
    {
        Administrador = 1,
        Medico = 2,
        Recepcionista = 3
    }

    public class Rol
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public TipoRol Tipo { get; set; }
    }
}
