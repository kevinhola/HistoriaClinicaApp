using System;

namespace HistoriaClinicaApp.Models
{
    public class Paciente
    {
        public int Id { get; set; }
        public string DNI { get; set; }
        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public DateTime FechaNacimiento { get; set; }
        public string Sexo { get; set; }
        public string Telefono { get; set; }
        public string Email { get; set; }
        public string Direccion { get; set; }
        public string ObraSocial { get; set; }
        public string NumeroAfiliado { get; set; }
        public DateTime FechaRegistro { get; set; }

        // Propiedad calculada para edad
        public int Edad
        {
            get
            {
                var today = DateTime.Today;
                var age = today.Year - FechaNacimiento.Year;
                if (FechaNacimiento.Date > today.AddYears(-age)) age--;
                return age;
            }
        }
    }
}
