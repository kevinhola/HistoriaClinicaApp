using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using HistoriaClinicaApp.Models;

namespace HistoriaClinicaApp.Services
{
    public class AuthService
    {
        private readonly DatabaseService _dbService;

        public AuthService()
        {
            _dbService = new DatabaseService();
        }

        public Usuario Autenticar(string nombreUsuario, string password)
        {
            try
            {
                var usuarios = _dbService.ObtenerTodosUsuarios().Result;
                var usuario = usuarios.FirstOrDefault(u => u.NombreUsuario == nombreUsuario && u.Activo);
                if (usuario == null) return null;
                
                string hashedPassword = HashPassword(password);
                if (usuario.PasswordHash == hashedPassword)
                    return usuario;
                
                return null;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error en autenticación: {ex.Message}");
            }
        }

        public static string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return Convert.ToBase64String(bytes);
            }
        }
    }
}
