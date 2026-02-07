using System;
using System.IO;

namespace HistoriaClinicaApp.Services
{
    public class LogService
    {
        private readonly string _logsPath;

        public LogService()
        {
            _logsPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Logs");
            Directory.CreateDirectory(_logsPath);
        }

        public void RegistrarAcceso(int usuarioId, string nombreUsuario, string accion)
        {
            string logFile = Path.Combine(_logsPath, "access.log");
            string mensaje = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] Usuario: {nombreUsuario} (ID: {usuarioId}) - {accion}";
            File.AppendAllText(logFile, mensaje + Environment.NewLine);
        }

        public void RegistrarCambioMedico(int usuarioId, int pacienteId, string entidad, string accion, string detalle)
        {
            string logFile = Path.Combine(_logsPath, "medical_changes.log");
            string mensaje = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] Usuario: {usuarioId} | Paciente: {pacienteId} | {entidad} | {accion} | {detalle}";
            File.AppendAllText(logFile, mensaje + Environment.NewLine);
        }

        public void RegistrarError(string origen, string mensaje, string stackTrace)
        {
            string logFile = Path.Combine(_logsPath, "error.log");
            string log = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] {origen}{Environment.NewLine}{mensaje}{Environment.NewLine}{stackTrace}{Environment.NewLine}---{Environment.NewLine}";
            File.AppendAllText(logFile, log);
        }
    }
}