using System;
using System.IO;
using System.IO.Compression;

namespace HistoriaClinicaApp.Services
{
    public class BackupService
    {
        private readonly string _backupPath;
        private readonly LogService _logService;

        public BackupService()
        {
            _backupPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Backups");
            _logService = new LogService();
            Directory.CreateDirectory(_backupPath);
        }

        public void CrearBackup()
        {
            try
            {
                string timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
                string backupFile = Path.Combine(_backupPath, $"backup_{timestamp}.zip");

                using (var archive = ZipFile.Open(backupFile, ZipArchiveMode.Create))
                {
                    // Backup de la base de datos
                    string dbPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Database", "clinica.db");
                    if (File.Exists(dbPath))
                    {
                        archive.CreateEntryFromFile(dbPath, "clinica.db");
                    }

                    // Backup de archivos cifrados
                    string storagePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Storage");
                    if (Directory.Exists(storagePath))
                    {
                        AgregarDirectorioAlZip(archive, storagePath, "Storage");
                    }

                    // Backup de configuraciÃ³n
                    string configPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Config");
                    if (Directory.Exists(configPath))
                    {
                        AgregarDirectorioAlZip(archive, configPath, "Config");
                    }
                }

                // Limpiar backups antiguos (mantener Ãºltimos 10)
                LimpiarBackupsAntiguos();

                _logService.RegistrarAcceso(
                    Security.SessionManager.CurrentUser?.Id ?? 0,
                    Security.SessionManager.CurrentUser?.NombreUsuario ?? "Sistema",
                    $"Backup creado: {Path.GetFileName(backupFile)}"
                );
            }
            catch (Exception ex)
            {
                _logService.RegistrarError("BackupService.CrearBackup", ex.Message, ex.StackTrace);
                throw;
            }
        }

        private void AgregarDirectorioAlZip(ZipArchive archive, string sourceDir, string entryName)
        {
            var files = Directory.GetFiles(sourceDir, "*", SearchOption.AllDirectories);
            foreach (var file in files)
            {
                var relativePath = Path.GetRelativePath(sourceDir, file);
                var entryPath = Path.Combine(entryName, relativePath).Replace("\\", "/");
                archive.CreateEntryFromFile(file, entryPath);
            }
        }

        private void LimpiarBackupsAntiguos()
        {
            var backups = Directory.GetFiles(_backupPath, "backup_*.zip");
            if (backups.Length > 10)
            {
                Array.Sort(backups);
                for (int i = 0; i < backups.Length - 10; i++)
                {
                    File.Delete(backups[i]);
                }
            }
        }

        public void RestaurarBackup(string rutaBackup)
        {
            try
            {
                string tempPath = Path.Combine(Path.GetTempPath(), "HistoriaClinicaRestore");
                if (Directory.Exists(tempPath))
                {
                    Directory.Delete(tempPath, true);
                }
                Directory.CreateDirectory(tempPath);

                ZipFile.ExtractToDirectory(rutaBackup, tempPath);

                // Restaurar base de datos
                string dbSource = Path.Combine(tempPath, "clinica.db");
                string dbDest = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Database", "clinica.db");
                if (File.Exists(dbSource))
                {
                    File.Copy(dbSource, dbDest, true);
                }

                // Restaurar Storage
                string storageSource = Path.Combine(tempPath, "Storage");
                string storageDest = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Storage");
                if (Directory.Exists(storageSource))
                {
                    CopiarDirectorio(storageSource, storageDest);
                }

                // Restaurar Config
                string configSource = Path.Combine(tempPath, "Config");
                string configDest = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Config");
                if (Directory.Exists(configSource))
                {
                    CopiarDirectorio(configSource, configDest);
                }

                _logService.RegistrarAcceso(
                    Security.SessionManager.CurrentUser?.Id ?? 0,
                    Security.SessionManager.CurrentUser?.NombreUsuario ?? "Sistema",
                    $"Backup restaurado desde: {Path.GetFileName(rutaBackup)}"
                );
            }
            catch (Exception ex)
            {
                _logService.RegistrarError("BackupService.RestaurarBackup", ex.Message, ex.StackTrace);
                throw;
            }
        }

        private void CopiarDirectorio(string source, string destination)
        {
            Directory.CreateDirectory(destination);

            foreach (var file in Directory.GetFiles(source, "*", SearchOption.AllDirectories))
            {
                var relativePath = Path.GetRelativePath(source, file);
                var destPath = Path.Combine(destination, relativePath);
                Directory.CreateDirectory(Path.GetDirectoryName(destPath));
                File.Copy(file, destPath, true);
            }
        }
    }
}
