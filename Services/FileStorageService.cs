using System;
using System.IO;
using HistoriaClinicaApp.Models;

namespace HistoriaClinicaApp.Services
{
    public class FileStorageService
    {
        private readonly string _storagePath;
        private readonly EncryptionService _encryptionService;
        private readonly LogService _logService;

        public FileStorageService()
        {
            _storagePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Storage", "Pacientes");
            _encryptionService = new EncryptionService();
            _logService = new LogService();
            Directory.CreateDirectory(_storagePath);
        }

        public string GuardarArchivo(int pacienteId, string rutaArchivoOriginal)
        {
            try
            {
                string carpetaPaciente = Path.Combine(_storagePath, pacienteId.ToString());
                Directory.CreateDirectory(carpetaPaciente);

                string extension = Path.GetExtension(rutaArchivoOriginal);
                string nombreArchivo = $"{Guid.NewGuid()}{extension}.enc";
                string rutaDestino = Path.Combine(carpetaPaciente, nombreArchivo);

                _encryptionService.CifrarArchivo(rutaArchivoOriginal, rutaDestino);

                _logService.RegistrarCambioMedico(
                    Security.SessionManager.CurrentUser.Id,
                    pacienteId,
                    "Archivo",
                    "Subir",
                    $"Archivo: {Path.GetFileName(rutaArchivoOriginal)}"
                );

                return rutaDestino;
            }
            catch (Exception ex)
            {
                _logService.RegistrarError("FileStorageService.GuardarArchivo", ex.Message, ex.StackTrace);
                throw;
            }
        }

        public string AbrirArchivo(ArchivoMedico archivo)
        {
            try
            {
                string tempPath = Path.Combine(Path.GetTempPath(), "HistoriaClinica");
                Directory.CreateDirectory(tempPath);
                string rutaTemporal = Path.Combine(tempPath, archivo.NombreArchivo);

                _encryptionService.DescifrarArchivo(archivo.RutaArchivo, rutaTemporal);

                _logService.RegistrarAcceso(
                    Security.SessionManager.CurrentUser.Id,
                    Security.SessionManager.CurrentUser.NombreUsuario,
                    $"Abrió archivo: {archivo.NombreArchivo} del paciente ID: {archivo.PacienteId}"
                );

                return rutaTemporal;
            }
            catch (Exception ex)
            {
                _logService.RegistrarError("FileStorageService.AbrirArchivo", ex.Message, ex.StackTrace);
                throw;
            }
        }

        public void EliminarArchivo(string rutaArchivo)
        {
            try
            {
                if (File.Exists(rutaArchivo))
                {
                    File.Delete(rutaArchivo);
                }
            }
            catch (Exception ex)
            {
                _logService.RegistrarError("FileStorageService.EliminarArchivo", ex.Message, ex.StackTrace);
                throw;
            }
        }
    }
}