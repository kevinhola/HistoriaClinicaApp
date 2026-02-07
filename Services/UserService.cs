using System.Data.SQLite;
using System.IO;
using System;
using HistoriaClinicaApp.Security;

namespace HistoriaClinicaApp.Services
{
    public class UserService
    {
        private readonly string _connectionString;

        public UserService()
        {
            string dbPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Database", "clinica.db");
            _connectionString = $"Data Source={dbPath};Version=3;";
        }

        public void CambiarNombreUsuario(int usuarioId, string nuevoNombre)
        {
            using (var conn = new SQLiteConnection(_connectionString))
            {
                conn.Open();
                string sql = "UPDATE Usuarios SET NombreUsuario = @NombreUsuario WHERE Id = @Id";
                using (var cmd = new SQLiteCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@NombreUsuario", nuevoNombre);
                    cmd.Parameters.AddWithValue("@Id", usuarioId);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public bool CambiarPassword(int usuarioId, string passwordActual, string passwordNueva)
        {
            using (var conn = new SQLiteConnection(_connectionString))
            {
                conn.Open();
                
                // Verificar password actual
                string sqlSelect = "SELECT PasswordHash FROM Usuarios WHERE Id = @Id";
                using (var cmd = new SQLiteCommand(sqlSelect, conn))
                {
                    cmd.Parameters.AddWithValue("@Id", usuarioId);
                    string storedHash = cmd.ExecuteScalar()?.ToString();
                    
                    if (storedHash == null || !PasswordHasher.VerifyPassword(passwordActual, storedHash))
                    {
                        return false;
                    }
                }

                // Actualizar password
                string sqlUpdate = "UPDATE Usuarios SET PasswordHash = @PasswordHash WHERE Id = @Id";
                using (var cmd = new SQLiteCommand(sqlUpdate, conn))
                {
                    cmd.Parameters.AddWithValue("@PasswordHash", PasswordHasher.HashPassword(passwordNueva));
                    cmd.Parameters.AddWithValue("@Id", usuarioId);
                    cmd.ExecuteNonQuery();
                }

                return true;
            }
        }
    }
}