using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using HistoriaClinicaApp.Models;

namespace HistoriaClinicaApp.Services
{

    public class DatabaseService
    {
    public void ActualizarEstudio(Estudio estudio)
    {
      using (var conn = GetConnection())
      {
           conn.Open();
           string sql = @"UPDATE Estudios SET 
               TipoEstudio = @TipoEstudio,
              Resultado = @Resultado,
              Fecha = @Fecha,
              Observaciones = @Observaciones
              WHERE Id = @Id";

          using (var cmd = new SQLiteCommand(sql, conn))
          {
              cmd.Parameters.AddWithValue("@Id", estudio.Id);
              cmd.Parameters.AddWithValue("@TipoEstudio", estudio.TipoEstudio);
             cmd.Parameters.AddWithValue("@Resultado", estudio.Resultado ?? "");
              cmd.Parameters.AddWithValue("@Fecha", estudio.Fecha);
             cmd.Parameters.AddWithValue("@Observaciones", estudio.Observaciones ?? "");
             cmd.ExecuteNonQuery();
         }
     }
    }
        private readonly string _connectionString;

        public DatabaseService()
        {
            string dbPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Database", "HistoriaClinicaApp.db");
            _connectionString = $"Data Source={dbPath};Version=3;";
        }

        private SQLiteConnection GetConnection()
        {
            return new SQLiteConnection(_connectionString);
        }

        // ==================== PACIENTES ====================
        public List<Paciente> ObtenerPacientes()
        {
            var pacientes = new List<Paciente>();

            using (var conn = GetConnection())
            {
                conn.Open();
                string sql = "SELECT * FROM Pacientes ORDER BY Apellido, Nombre";
                using (var cmd = new SQLiteCommand(sql, conn))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        pacientes.Add(new Paciente
                        {
                            Id = Convert.ToInt32(reader["Id"]),
                            DNI = reader["DNI"].ToString(),
                            Nombre = reader["Nombre"].ToString(),
                            Apellido = reader["Apellido"].ToString(),
                            FechaNacimiento = Convert.ToDateTime(reader["FechaNacimiento"]),
                            Sexo = reader["Sexo"].ToString(),
                            Telefono = reader["Telefono"].ToString(),
                            Email = reader["Email"].ToString(),
                            Direccion = reader["Direccion"].ToString(),
                            ObraSocial = reader["ObraSocial"].ToString(),
                            NumeroAfiliado = reader["NumeroAfiliado"].ToString(),
                            FechaRegistro = Convert.ToDateTime(reader["FechaRegistro"])
                        });
                    }
                }
            }

            return pacientes;
        }

        public void CrearPaciente(Paciente paciente)
        {
            using (var conn = GetConnection())
            {
                conn.Open();
                string sql = @"INSERT INTO Pacientes 
                    (DNI, Nombre, Apellido, FechaNacimiento, Sexo, Telefono, Email, Direccion, ObraSocial, NumeroAfiliado, FechaRegistro)
                    VALUES (@DNI, @Nombre, @Apellido, @FechaNacimiento, @Sexo, @Telefono, @Email, @Direccion, @ObraSocial, @NumeroAfiliado, @FechaRegistro)";

                using (var cmd = new SQLiteCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@DNI", paciente.DNI);
                    cmd.Parameters.AddWithValue("@Nombre", paciente.Nombre);
                    cmd.Parameters.AddWithValue("@Apellido", paciente.Apellido);
                    cmd.Parameters.AddWithValue("@FechaNacimiento", paciente.FechaNacimiento);
                    cmd.Parameters.AddWithValue("@Sexo", paciente.Sexo);
                    cmd.Parameters.AddWithValue("@Telefono", paciente.Telefono ?? "");
                    cmd.Parameters.AddWithValue("@Email", paciente.Email ?? "");
                    cmd.Parameters.AddWithValue("@Direccion", paciente.Direccion ?? "");
                    cmd.Parameters.AddWithValue("@ObraSocial", paciente.ObraSocial ?? "");
                    cmd.Parameters.AddWithValue("@NumeroAfiliado", paciente.NumeroAfiliado ?? "");
                    cmd.Parameters.AddWithValue("@FechaRegistro", DateTime.Now);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void ActualizarPaciente(Paciente paciente)
        {
            using (var conn = GetConnection())
            {
                conn.Open();
                string sql = @"UPDATE Pacientes SET 
                    DNI = @DNI,
                    Nombre = @Nombre,
                    Apellido = @Apellido,
                    FechaNacimiento = @FechaNacimiento,
                    Sexo = @Sexo,
                    Telefono = @Telefono,
                    Email = @Email,
                    Direccion = @Direccion,
                    ObraSocial = @ObraSocial,
                    NumeroAfiliado = @NumeroAfiliado
                    WHERE Id = @Id";

                using (var cmd = new SQLiteCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@Id", paciente.Id);
                    cmd.Parameters.AddWithValue("@DNI", paciente.DNI);
                    cmd.Parameters.AddWithValue("@Nombre", paciente.Nombre);
                    cmd.Parameters.AddWithValue("@Apellido", paciente.Apellido);
                    cmd.Parameters.AddWithValue("@FechaNacimiento", paciente.FechaNacimiento);
                    cmd.Parameters.AddWithValue("@Sexo", paciente.Sexo);
                    cmd.Parameters.AddWithValue("@Telefono", paciente.Telefono ?? "");
                    cmd.Parameters.AddWithValue("@Email", paciente.Email ?? "");
                    cmd.Parameters.AddWithValue("@Direccion", paciente.Direccion ?? "");
                    cmd.Parameters.AddWithValue("@ObraSocial", paciente.ObraSocial ?? "");
                    cmd.Parameters.AddWithValue("@NumeroAfiliado", paciente.NumeroAfiliado ?? "");
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void EliminarPaciente(int id)
        {
            using (var conn = GetConnection())
            {
                conn.Open();
                string sql = "DELETE FROM Pacientes WHERE Id = @Id";
                using (var cmd = new SQLiteCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@Id", id);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        // ==================== HISTORIAS CLÍNICAS ====================
        public HistoriaClinica ObtenerHistoriaClinica(int pacienteId)
        {
            using (var conn = GetConnection())
            {
                conn.Open();
                string sql = "SELECT * FROM HistoriasClinicas WHERE PacienteId = @PacienteId";
                using (var cmd = new SQLiteCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@PacienteId", pacienteId);
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new HistoriaClinica
                            {
                                Id = Convert.ToInt32(reader["Id"]),
                                PacienteId = Convert.ToInt32(reader["PacienteId"]),
                                Antecedentes = reader["Antecedentes"].ToString(),
                                Alergias = reader["Alergias"].ToString(),
                                MedicacionHabitual = reader["MedicacionHabitual"].ToString(),
                                GrupoSanguineo = reader["GrupoSanguineo"].ToString(),
                                Observaciones = reader["Observaciones"].ToString(),
                                FechaCreacion = Convert.ToDateTime(reader["FechaCreacion"]),
                                FechaUltimaModificacion = Convert.ToDateTime(reader["FechaUltimaModificacion"]),
                                UsuarioCreacion = Convert.ToInt32(reader["UsuarioCreacion"]),
                                UsuarioModificacion = Convert.ToInt32(reader["UsuarioModificacion"])
                            };
                        }
                    }
                }
            }
            return null;
        }

        public void CrearHistoriaClinica(HistoriaClinica historia)
        {
            using (var conn = GetConnection())
            {
                conn.Open();
                string sql = @"INSERT INTO HistoriasClinicas 
                    (PacienteId, Antecedentes, Alergias, MedicacionHabitual, GrupoSanguineo, Observaciones, 
                     FechaCreacion, FechaUltimaModificacion, UsuarioCreacion, UsuarioModificacion)
                    VALUES (@PacienteId, @Antecedentes, @Alergias, @MedicacionHabitual, @GrupoSanguineo, @Observaciones,
                            @FechaCreacion, @FechaUltimaModificacion, @UsuarioCreacion, @UsuarioModificacion)";

                using (var cmd = new SQLiteCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@PacienteId", historia.PacienteId);
                    cmd.Parameters.AddWithValue("@Antecedentes", historia.Antecedentes ?? "");
                    cmd.Parameters.AddWithValue("@Alergias", historia.Alergias ?? "");
                    cmd.Parameters.AddWithValue("@MedicacionHabitual", historia.MedicacionHabitual ?? "");
                    cmd.Parameters.AddWithValue("@GrupoSanguineo", historia.GrupoSanguineo ?? "");
                    cmd.Parameters.AddWithValue("@Observaciones", historia.Observaciones ?? "");
                    cmd.Parameters.AddWithValue("@FechaCreacion", DateTime.Now);
                    cmd.Parameters.AddWithValue("@FechaUltimaModificacion", DateTime.Now);
                    cmd.Parameters.AddWithValue("@UsuarioCreacion", historia.UsuarioCreacion);
                    cmd.Parameters.AddWithValue("@UsuarioModificacion", historia.UsuarioModificacion);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void ActualizarHistoriaClinica(HistoriaClinica historia)
        {
            using (var conn = GetConnection())
            {
                conn.Open();
                string sql = @"UPDATE HistoriasClinicas SET 
                    Antecedentes = @Antecedentes,
                    Alergias = @Alergias,
                    MedicacionHabitual = @MedicacionHabitual,
                    GrupoSanguineo = @GrupoSanguineo,
                    Observaciones = @Observaciones,
                    FechaUltimaModificacion = @FechaUltimaModificacion,
                    UsuarioModificacion = @UsuarioModificacion
                    WHERE Id = @Id";

                using (var cmd = new SQLiteCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@Id", historia.Id);
                    cmd.Parameters.AddWithValue("@Antecedentes", historia.Antecedentes ?? "");
                    cmd.Parameters.AddWithValue("@Alergias", historia.Alergias ?? "");
                    cmd.Parameters.AddWithValue("@MedicacionHabitual", historia.MedicacionHabitual ?? "");
                    cmd.Parameters.AddWithValue("@GrupoSanguineo", historia.GrupoSanguineo ?? "");
                    cmd.Parameters.AddWithValue("@Observaciones", historia.Observaciones ?? "");
                    cmd.Parameters.AddWithValue("@FechaUltimaModificacion", DateTime.Now);
                    cmd.Parameters.AddWithValue("@UsuarioModificacion", historia.UsuarioModificacion);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        // ==================== CONSULTAS ====================
        public List<Consulta> ObtenerConsultas(int pacienteId)
        {
            var consultas = new List<Consulta>();
            using (var conn = GetConnection())
            {
                conn.Open();
                string sql = @"SELECT c.*, u.NombreUsuario 
                              FROM Consultas c
                              INNER JOIN Usuarios u ON c.UsuarioId = u.Id
                              WHERE c.PacienteId = @PacienteId 
                              ORDER BY c.FechaHora DESC";
                using (var cmd = new SQLiteCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@PacienteId", pacienteId);
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            consultas.Add(new Consulta
                            {
                                Id = Convert.ToInt32(reader["Id"]),
                                PacienteId = Convert.ToInt32(reader["PacienteId"]),
                                UsuarioId = Convert.ToInt32(reader["UsuarioId"]),
                                FechaHora = Convert.ToDateTime(reader["FechaHora"]),
                                Motivo = reader["Motivo"].ToString(),
                                Sintomas = reader["Sintomas"].ToString(),
                                Diagnostico = reader["Diagnostico"].ToString(),
                                Tratamiento = reader["Tratamiento"].ToString(),
                                NotasMedicas = reader["NotasMedicas"].ToString(),
                                Peso = reader["Peso"] != DBNull.Value ? Convert.ToDecimal(reader["Peso"]) : 0,
                                Altura = reader["Altura"] != DBNull.Value ? Convert.ToDecimal(reader["Altura"]) : 0,
                                PresionArterial = reader["PresionArterial"].ToString(),
                                Temperatura = reader["Temperatura"] != DBNull.Value ? Convert.ToDecimal(reader["Temperatura"]) : 0,
                                NombreUsuario = reader["NombreUsuario"].ToString()
                            });
                        }
                    }
                }
            }
            return consultas;
        }

        public void CrearConsulta(Consulta consulta)
        {
            using (var conn = GetConnection())
            {
                conn.Open();
                string sql = @"INSERT INTO Consultas 
                    (PacienteId, UsuarioId, FechaHora, Motivo, Sintomas, Diagnostico, Tratamiento, NotasMedicas,
                     Peso, Altura, PresionArterial, Temperatura)
                    VALUES (@PacienteId, @UsuarioId, @FechaHora, @Motivo, @Sintomas, @Diagnostico, @Tratamiento, @NotasMedicas,
                            @Peso, @Altura, @PresionArterial, @Temperatura)";

                using (var cmd = new SQLiteCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@PacienteId", consulta.PacienteId);
                    cmd.Parameters.AddWithValue("@UsuarioId", consulta.UsuarioId);
                    cmd.Parameters.AddWithValue("@FechaHora", consulta.FechaHora);
                    cmd.Parameters.AddWithValue("@Motivo", consulta.Motivo ?? "");
                    cmd.Parameters.AddWithValue("@Sintomas", consulta.Sintomas ?? "");
                    cmd.Parameters.AddWithValue("@Diagnostico", consulta.Diagnostico ?? "");
                    cmd.Parameters.AddWithValue("@Tratamiento", consulta.Tratamiento ?? "");
                    cmd.Parameters.AddWithValue("@NotasMedicas", consulta.NotasMedicas ?? "");
                    cmd.Parameters.AddWithValue("@Peso", consulta.Peso);
                    cmd.Parameters.AddWithValue("@Altura", consulta.Altura);
                    cmd.Parameters.AddWithValue("@PresionArterial", consulta.PresionArterial ?? "");
                    cmd.Parameters.AddWithValue("@Temperatura", consulta.Temperatura);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        // ==================== RECETAS ====================
        public List<Receta> ObtenerRecetas(int consultaId)
        {
            var recetas = new List<Receta>();
            using (var conn = GetConnection())
            {
                conn.Open();
                string sql = @"SELECT r.*, u.NombreUsuario 
                              FROM Recetas r
                              INNER JOIN Usuarios u ON r.UsuarioId = u.Id
                              WHERE r.ConsultaId = @ConsultaId";
                using (var cmd = new SQLiteCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@ConsultaId", consultaId);
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            recetas.Add(new Receta
                            {
                                Id = Convert.ToInt32(reader["Id"]),
                                ConsultaId = Convert.ToInt32(reader["ConsultaId"]),
                                TextoMedico = reader["TextoMedico"].ToString(),
                                Fecha = Convert.ToDateTime(reader["Fecha"]),
                                UsuarioId = Convert.ToInt32(reader["UsuarioId"]),
                                NombreUsuario = reader["NombreUsuario"].ToString()
                            });
                        }
                    }
                }
            }
            return recetas;
        }

        public void CrearReceta(Receta receta)
        {
            using (var conn = GetConnection())
            {
                conn.Open();
                string sql = @"INSERT INTO Recetas (ConsultaId, TextoMedico, Fecha, UsuarioId)
                              VALUES (@ConsultaId, @TextoMedico, @Fecha, @UsuarioId)";

                using (var cmd = new SQLiteCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@ConsultaId", receta.ConsultaId);
                    cmd.Parameters.AddWithValue("@TextoMedico", receta.TextoMedico);
                    cmd.Parameters.AddWithValue("@Fecha", receta.Fecha);
                    cmd.Parameters.AddWithValue("@UsuarioId", receta.UsuarioId);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        // ==================== ESTUDIOS ====================
        public List<Estudio> ObtenerEstudios(int pacienteId)
        {
            var estudios = new List<Estudio>();
            using (var conn = GetConnection())
            {
                conn.Open();
                string sql = @"SELECT e.*, u.NombreUsuario 
                              FROM Estudios e
                              INNER JOIN Usuarios u ON e.UsuarioId = u.Id
                              WHERE e.PacienteId = @PacienteId 
                              ORDER BY e.Fecha DESC";
                using (var cmd = new SQLiteCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@PacienteId", pacienteId);
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            estudios.Add(new Estudio
                            {
                                Id = Convert.ToInt32(reader["Id"]),
                                PacienteId = Convert.ToInt32(reader["PacienteId"]),
                                TipoEstudio = reader["TipoEstudio"].ToString(),
                                Resultado = reader["Resultado"].ToString(),
                                Fecha = Convert.ToDateTime(reader["Fecha"]),
                                Observaciones = reader["Observaciones"].ToString(),
                                UsuarioId = Convert.ToInt32(reader["UsuarioId"]),
                                NombreUsuario = reader["NombreUsuario"].ToString()
                            });
                        }
                    }
                }
            }
            return estudios;
        }

        public void CrearEstudio(Estudio estudio)
        {
            
            using (var conn = GetConnection())
            {
                conn.Open();
                string sql = @"INSERT INTO Estudios (PacienteId, TipoEstudio, Resultado, Fecha, Observaciones, UsuarioId)
                              VALUES (@PacienteId, @TipoEstudio, @Resultado, @Fecha, @Observaciones, @UsuarioId)";

                using (var cmd = new SQLiteCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@PacienteId", estudio.PacienteId);
                    cmd.Parameters.AddWithValue("@TipoEstudio", estudio.TipoEstudio);
                    cmd.Parameters.AddWithValue("@Resultado", estudio.Resultado ?? "");
                    cmd.Parameters.AddWithValue("@Fecha", estudio.Fecha);
                    cmd.Parameters.AddWithValue("@Observaciones", estudio.Observaciones ?? "");
                    cmd.Parameters.AddWithValue("@UsuarioId", estudio.UsuarioId);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        // ==================== ARCHIVOS MÉDICOS ====================
        public List<ArchivoMedico> ObtenerArchivosMedicos(int pacienteId)
        {
            var archivos = new List<ArchivoMedico>();
            using (var conn = GetConnection())
            {
                conn.Open();
                string sql = "SELECT * FROM ArchivosMedicos WHERE PacienteId = @PacienteId ORDER BY FechaSubida DESC";
                using (var cmd = new SQLiteCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@PacienteId", pacienteId);
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            archivos.Add(new ArchivoMedico
                            {
                                Id = Convert.ToInt32(reader["Id"]),
                                PacienteId = Convert.ToInt32(reader["PacienteId"]),
                                NombreArchivo = reader["NombreArchivo"].ToString(),
                                RutaArchivo = reader["RutaArchivo"].ToString(),
                                TipoArchivo = reader["TipoArchivo"].ToString(),
                                TamanoBytes = Convert.ToInt64(reader["TamanoBytes"]),
                                FechaSubida = Convert.ToDateTime(reader["FechaSubida"]),
                                UsuarioId = Convert.ToInt32(reader["UsuarioId"]),
                                Cifrado = Convert.ToBoolean(reader["Cifrado"])
                            });
                        }
                    }
                }
            }
            return archivos;
        }

        public void CrearArchivoMedico(ArchivoMedico archivo)
        {
            using (var conn = GetConnection())
            {
                conn.Open();
                string sql = @"INSERT INTO ArchivosMedicos 
                    (PacienteId, NombreArchivo, RutaArchivo, TipoArchivo, TamanoBytes, FechaSubida, UsuarioId, Cifrado)
                    VALUES (@PacienteId, @NombreArchivo, @RutaArchivo, @TipoArchivo, @TamanoBytes, @FechaSubida, @UsuarioId, @Cifrado)";

                using (var cmd = new SQLiteCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@PacienteId", archivo.PacienteId);
                    cmd.Parameters.AddWithValue("@NombreArchivo", archivo.NombreArchivo);
                    cmd.Parameters.AddWithValue("@RutaArchivo", archivo.RutaArchivo);
                    cmd.Parameters.AddWithValue("@TipoArchivo", archivo.TipoArchivo);
                    cmd.Parameters.AddWithValue("@TamanoBytes", archivo.TamanoBytes);
                    cmd.Parameters.AddWithValue("@FechaSubida", archivo.FechaSubida);
                    cmd.Parameters.AddWithValue("@UsuarioId", archivo.UsuarioId);
                    cmd.Parameters.AddWithValue("@Cifrado", archivo.Cifrado);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void EliminarArchivoMedico(int id)
        {
            using (var conn = GetConnection())
            {
                conn.Open();
                string sql = "DELETE FROM ArchivosMedicos WHERE Id = @Id";
                using (var cmd = new SQLiteCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@Id", id);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        // ==================== USUARIOS (ADMIN) ====================
        public List<Usuario> ObtenerTodosUsuarios()
        {
            var usuarios = new List<Usuario>();
            using (var conn = GetConnection())
            {
                conn.Open();
                string sql = @"SELECT u.*, r.Nombre as RolNombre, r.Descripcion as RolDescripcion 
                              FROM Usuarios u
                              INNER JOIN Roles r ON u.RolId = r.Id
                              ORDER BY u.NombreUsuario";
                using (var cmd = new SQLiteCommand(sql, conn))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        usuarios.Add(new Usuario
                        {
                            Id = Convert.ToInt32(reader["Id"]),
                            NombreUsuario = reader["NombreUsuario"].ToString(),
                            PasswordHash = reader["PasswordHash"].ToString(),
                            RolId = Convert.ToInt32(reader["RolId"]),
                            Activo = Convert.ToBoolean(reader["Activo"]),
                            FechaCreacion = Convert.ToDateTime(reader["FechaCreacion"]),
                            Rol = new Rol
                            {
                                Id = Convert.ToInt32(reader["RolId"]),
                                Nombre = reader["RolNombre"].ToString(),
                                Descripcion = reader["RolDescripcion"].ToString()
                            }
                        });
                    }
                }
            }
            return usuarios;
        }

        public void CrearUsuario(Usuario usuario)
        {
            using (var conn = GetConnection())
            {
                conn.Open();
                string sql = @"INSERT INTO Usuarios (NombreUsuario, PasswordHash, RolId, Activo, FechaCreacion)
                              VALUES (@NombreUsuario, @PasswordHash, @RolId, @Activo, @FechaCreacion)";

                using (var cmd = new SQLiteCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@NombreUsuario", usuario.NombreUsuario);
                    cmd.Parameters.AddWithValue("@PasswordHash", usuario.PasswordHash);
                    cmd.Parameters.AddWithValue("@RolId", usuario.RolId);
                    cmd.Parameters.AddWithValue("@Activo", usuario.Activo);
                    cmd.Parameters.AddWithValue("@FechaCreacion", DateTime.Now);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void ActualizarUsuario(Usuario usuario)
        {
            using (var conn = GetConnection())
            {
                conn.Open();
                string sql = @"UPDATE Usuarios SET 
                    NombreUsuario = @NombreUsuario,
                    RolId = @RolId,
                    Activo = @Activo
                    WHERE Id = @Id";

                using (var cmd = new SQLiteCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@Id", usuario.Id);
                    cmd.Parameters.AddWithValue("@NombreUsuario", usuario.NombreUsuario);
                    cmd.Parameters.AddWithValue("@RolId", usuario.RolId);
                    cmd.Parameters.AddWithValue("@Activo", usuario.Activo);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void ResetearPassword(int usuarioId, string nuevoPasswordHash)
        {
            using (var conn = GetConnection())
            {
                conn.Open();
                string sql = "UPDATE Usuarios SET PasswordHash = @PasswordHash WHERE Id = @Id";
                using (var cmd = new SQLiteCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@PasswordHash", nuevoPasswordHash);
                    cmd.Parameters.AddWithValue("@Id", usuarioId);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        public List<Rol> ObtenerRoles()
        {
            var roles = new List<Rol>();
            using (var conn = GetConnection())
            {
                conn.Open();
                string sql = "SELECT * FROM Roles ORDER BY Nombre";
                using (var cmd = new SQLiteCommand(sql, conn))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        roles.Add(new Rol
                        {
                            Id = Convert.ToInt32(reader["Id"]),
                            Nombre = reader["Nombre"].ToString(),
                            Descripcion = reader["Descripcion"].ToString()
                        });
                    }
                }
            }
            return roles;
        }
    }
}