using HistoriaClinicaApp.Models;

namespace HistoriaClinicaApp.Security
{
    public static class Permisos
    {
        public static bool PuedeEditarDatosPaciente(TipoRol rol)
        {
            return rol == TipoRol.Administrador ||
                   rol == TipoRol.Recepcionista;
        }

        public static bool PuedeEditarHistoriaClinica(TipoRol rol)
        {
            return rol == TipoRol.Administrador ||
                   rol == TipoRol.Medico;
        }

        public static bool EsAdministrador(TipoRol rol)
        {
            return rol == TipoRol.Administrador;
        }
    }
}
