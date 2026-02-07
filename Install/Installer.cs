using System;
using System.Data.SQLite;
using System.IO;
using HistoriaClinicaApp.Security;
using HistoriaClinicaApp.Services;

namespace HistoriaClinicaApp.Install
{
    public class Installer
    {
        private readonly string _baseDirectory;
        private readonly string _dbPath;

        public Installer()
        {
            _baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
            _dbPath = Path.Combine(_baseDirectory, "Database", "clinica.db");
        }

        public void Install()
        {
            CrearDirectorios();
            CrearBaseDatos();
            InsertarDatosIniciales();
            GenerarClaveEncriptacion();
        }

        private void CrearDirectorios()
        {
            Directory.CreateDirectory(Path.Combine(_baseDirectory, "Database"));
            Directory.CreateDirectory(Path.Combine(_baseDirectory, "Logs"));
            Directory.CreateDirectory(Path.Combine(_baseDirectory, "Storage", "Pacientes"));
            Directory.CreateDirectory(Path.Combine(_baseDirectory, "Config"));
            Directory.CreateDirectory(Path.Combine(_baseDirectory, "Backups"));
        }

        private void CrearBaseDatos()
        {
            SQLiteConnection.CreateFile(_dbPath);

            using (var conn = new SQLiteConnection($"Data Source={_dbPath};Version=3;"))
            {
                conn.Open();

                string sqlRoles = @"CREATE TABLE Roles (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    Nombre TEXT NOT NULL UNIQUE,
                    Descripcion TEXT
                )";
                new SQLiteCommand(sqlRoles, conn).ExecuteNonQuery();

                string sqlUsuarios = @"CREATE TABLE Usuarios (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    NombreUsuario TEXT NOT NULL UNIQUE,
                    PasswordHash TEXT NOT NULL,
                    RolId INTEGER NOT NULL,
                    Activo INTEGER NOT NULL DEFAULT 1,
                    FechaCreacion TEXT NOT NULL,
                    FOREIGN KEY (RolId) REFERENCES Roles(Id)
                )";
                new SQLiteCommand(sqlUsuarios, conn).ExecuteNonQuery();

                string sqlPacientes = @"CREATE TABLE Pacientes (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    DNI TEXT NOT NULL UNIQUE,
                    Nombre TEXT NOT NULL,
                    Apellido TEXT NOT NULL,
                    FechaNacimiento TEXT NOT NULL,
                    Sexo TEXT,
                    Telefono TEXT,
                    Email TEXT,
                    Direccion TEXT,
                    ObraSocial TEXT,
                    NumeroAfiliado TEXT,
                    FechaRegistro TEXT NOT NULL
                )";
                new SQLiteCommand(sqlPacientes, conn).ExecuteNonQuery();

                string sqlHistorias = @"CREATE TABLE HistoriasClinicas (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    PacienteId INTEGER NOT NULL,
                    Antecedentes TEXT,
                    Alergias TEXT,
                    MedicacionHabitual TEXT,
                    GrupoSanguineo TEXT,
                    Observaciones TEXT,
                    FechaCreacion TEXT NOT NULL,
                    FechaUltimaModificacion TEXT NOT NULL,
                    UsuarioCreacion INTEGER NOT NULL,
                    UsuarioModificacion INTEGER NOT NULL,
                    FOREIGN KEY (PacienteId) REFERENCES Pacientes(Id) ON DELETE CASCADE,
                    FOREIGN KEY (UsuarioCreacion) REFERENCES Usuarios(Id),
                    FOREIGN KEY (UsuarioModificacion) REFERENCES Usuarios(Id)
                )";
                new SQLiteCommand(sqlHistorias, conn).ExecuteNonQuery();

                string sqlConsultas = @"CREATE TABLE Consultas (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    PacienteId INTEGER NOT NULL,
                    UsuarioId INTEGER NOT NULL,
                    FechaHora TEXT NOT NULL,
                    Motivo TEXT,
                    Sintomas TEXT,
                    Diagnostico TEXT,
                    Tratamiento TEXT,
                    NotasMedicas TEXT,
                    Peso REAL DEFAULT 0,
                    Altura REAL DEFAULT 0,
                    PresionArterial TEXT,
                    Temperatura REAL DEFAULT 0,
                    FOREIGN KEY (PacienteId) REFERENCES Pacientes(Id) ON DELETE CASCADE,
                    FOREIGN KEY (UsuarioId) REFERENCES Usuarios(Id)
                )";
                new SQLiteCommand(sqlConsultas, conn).ExecuteNonQuery();

                string sqlRecetas = @"CREATE TABLE Recetas (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    ConsultaId INTEGER NOT NULL,
                    TextoMedico TEXT NOT NULL,
                    Fecha TEXT NOT NULL,
                    UsuarioId INTEGER NOT NULL,
                    FOREIGN KEY (ConsultaId) REFERENCES Consultas(Id) ON DELETE CASCADE,
                    FOREIGN KEY (UsuarioId) REFERENCES Usuarios(Id)
                )";
                new SQLiteCommand(sqlRecetas, conn).ExecuteNonQuery();

                string sqlEstudios = @"CREATE TABLE Estudios (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    PacienteId INTEGER NOT NULL,
                    TipoEstudio TEXT NOT NULL,
                    Resultado TEXT,
                    Fecha TEXT NOT NULL,
                    Observaciones TEXT,
                    UsuarioId INTEGER NOT NULL,
                    FOREIGN KEY (PacienteId) REFERENCES Pacientes(Id) ON DELETE CASCADE,
                    FOREIGN KEY (UsuarioId) REFERENCES Usuarios(Id)
                )";
                new SQLiteCommand(sqlEstudios, conn).ExecuteNonQuery();

                string sqlArchivos = @"CREATE TABLE ArchivosMedicos (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    PacienteId INTEGER NOT NULL,
                    NombreArchivo TEXT NOT NULL,
                    RutaArchivo TEXT NOT NULL,
                    TipoArchivo TEXT,
                    TamanoBytes INTEGER NOT NULL,
                    FechaSubida TEXT NOT NULL,
                    UsuarioId INTEGER NOT NULL,
                    Cifrado INTEGER NOT NULL DEFAULT 1,
                    FOREIGN KEY (PacienteId) REFERENCES Pacientes(Id) ON DELETE CASCADE,
                    FOREIGN KEY (UsuarioId) REFERENCES Usuarios(Id)
                )";
                new SQLiteCommand(sqlArchivos, conn).ExecuteNonQuery();
            }
        }

        private void InsertarDatosIniciales()
        {
            using (var conn = new SQLiteConnection($"Data Source={_dbPath};Version=3;"))
            {
                conn.Open();

                string sqlRoles = @"INSERT INTO Roles (Nombre, Descripcion) VALUES 
                    ('Administrador', 'Acceso total al sistema'),
                    ('Médico', 'Acceso a pacientes e historias clínicas'),
                    ('Recepcionista', 'Acceso limitado a gestión de pacientes')";
                new SQLiteCommand(sqlRoles, conn).ExecuteNonQuery();

                string passwordHash = PasswordHasher.HashPassword("Admin123!");
                string sqlAdmin = @"INSERT INTO Usuarios (NombreUsuario, PasswordHash, RolId, Activo, FechaCreacion)
                    VALUES ('admin', @PasswordHash, 1, 1, @FechaCreacion)";
                
                using (var cmd = new SQLiteCommand(sqlAdmin, conn))
                {
                    cmd.Parameters.AddWithValue("@PasswordHash", passwordHash);
                    cmd.Parameters.AddWithValue("@FechaCreacion", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                    cmd.ExecuteNonQuery();
                }
            }
        }

        private void GenerarClaveEncriptacion()
        {
            var encryptionService = new EncryptionService();
            string keyPath = Path.Combine(_baseDirectory, "Config", "encryption.key");
            encryptionService.GuardarClave(keyPath);
        }
    }
}