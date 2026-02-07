using System;

namespace HistoriaClinicaApp.Models
{
    public class ArchivoMedico
    {
        public int Id { get; set; }
        public int PacienteId { get; set; }
        public string NombreArchivo { get; set; }
        public string RutaArchivo { get; set; }
        public string TipoArchivo { get; set; }
        public long TamanoBytes { get; set; }
        public DateTime FechaSubida { get; set; }
        public int UsuarioId { get; set; }
        public bool Cifrado { get; set; }
        public string Descripcion { get; set; }

    }
}