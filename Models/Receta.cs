using System;

namespace HistoriaClinicaApp.Models
{
    public class Receta
    {
        public int Id { get; set; }
        public int ConsultaId { get; set; }
        public string TextoMedico { get; set; }
        public DateTime Fecha { get; set; }
        public int UsuarioId { get; set; }
        public string NombreUsuario { get; set; }
    }
}