using System;
using System.Data.SQLite;
using System.IO;
using HistoriaClinicaApp.Models;
using HistoriaClinicaApp.Security;

namespace HistoriaClinicaApp.Services
{
    public class AuthService
    {
        private readonly string _connectionString;

        public AuthService()
        {
            string dbPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Database", "clinica.db");
            _connectionString = $"Data Source={dbPath};Version=3;";
        }

        public Usuario Login(string nombreUsuario, string password)
        {
            using (var conn = new SQLiteConnection(_connectionString))
            {
                conn.Open();
                string sql = @"SELECT u.*, r.Nombre as RolNombre, r.Descripcion as RolDescripcion 
                              FROM Usuarios u
                              INNER JOIN Roles r ON u.RolId = r.Id
                              WHERE u.NombreUsuario = @NombreUsuario AND u.Activo = 1";

                using (var cmd = new SQLiteCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@NombreUsuario", nombreUsuario);
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            string storedHash = reader["PasswordHash"].ToString();
                            
                            // Verificar password
                            if (PasswordHasher.VerifyPassword(password, storedHash))
                            {
                                var usuario = new Usuario
                                {
                                    Id = Convert.ToInt32(reader["Id"]),
                                    NombreUsuario = reader["NombreUsuario"].ToString(),
                                    PasswordHash = storedHash,
                                    RolId = Convert.ToInt32(reader["RolId"]),
                                    Activo = Convert.ToBoolean(reader["Activo"]),
                                    FechaCreacion = Convert.ToDateTime(reader["FechaCreacion"]),
                                    Rol = new Rol
                                    {
                                        Id = Convert.ToInt32(reader["RolId"]),
                                        Nombre = reader["RolNombre"].ToString(),
                                        Descripcion = reader["RolDescripcion"].ToString()
                                    }
                                };

                                // Establecer sesión
                                SessionManager.Login(usuario);
                                return usuario;
                            }
                        }
                    }
                }
            }

            return null;
        }
    }
}