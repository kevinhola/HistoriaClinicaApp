using System;
using Postgrest.Attributes;
using Postgrest.Models;

namespace HistoriaClinicaApp.Models
{
    [Table("pacientes")]
    public class Paciente : BaseModel
    {
        [PrimaryKey("id", false)]
        public long Id { get; set; }
        [Column("dni")]
        public string DNI { get; set; }
        [Column("nombre")]
        public string Nombre { get; set; }
        [Column("apellido")]
        public string Apellido { get; set; }
        [Column("fecha_nacimiento")]
        public DateTime FechaNacimiento { get; set; }
        [Column("sexo")]
        public string Sexo { get; set; }
        [Column("telefono")]
        public string Telefono { get; set; }
        [Column("email")]
        public string Email { get; set; }
        [Column("direccion")]
        public string Direccion { get; set; }
        [Column("obra_social")]
        public string ObraSocial { get; set; }
        [Column("numero_afiliado")]
        public string NumeroAfiliado { get; set; }
        [Column("created_at")]
        public DateTime FechaRegistro { get; set; }
        
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

