using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Microsoft.Win32;
using HistoriaClinicaApp.Models;
using HistoriaClinicaApp.Security;
using HistoriaClinicaApp.Services;
using HistoriaClinicaApp.ViewModels;


namespace HistoriaClinicaApp.Views
{
    public partial class ArchivosControl : UserControl, INotifyPropertyChanged
    {
        private readonly DatabaseService _dbService;
        private readonly FileStorageService _fileService;
        private readonly LogService _logService;
        private readonly int _pacienteId;
        private ObservableCollection<ArchivoMedico> _archivos;

        public ObservableCollection<ArchivoMedico> Archivos
        {
            get => _archivos;
            set
            {
                _archivos = value;
                OnPropertyChanged(nameof(Archivos));
            }
        }

        public ICommand SubirArchivoCommand { get; }
        public ICommand AbrirArchivoCommand { get; }
        public ICommand EliminarArchivoCommand { get; }

        public ArchivosControl(int pacienteId)
        {
            InitializeComponent();
            _pacienteId = pacienteId;
            _dbService = new DatabaseService();
            _fileService = new FileStorageService();
            _logService = new LogService();

            SubirArchivoCommand = new RelayCommand(SubirArchivo);
            AbrirArchivoCommand = new RelayCommand<ArchivoMedico>(AbrirArchivo);
            EliminarArchivoCommand = new RelayCommand<ArchivoMedico>(EliminarArchivo);

            DataContext = this;
            CargarArchivos();
        }

        private void CargarArchivos()
        {
            Archivos = new ObservableCollection<ArchivoMedico>(_dbService.ObtenerArchivosMedicos(_pacienteId));
        }

        private void SubirArchivo()
        {
            try
            {
                var openFileDialog = new OpenFileDialog
                {
                    Title = "Seleccionar archivo",
                    Filter = "Todos los archivos (*.*)|*.*|PDF (*.pdf)|*.pdf|Imágenes (*.jpg;*.png)|*.jpg;*.png"
                };

                if (openFileDialog.ShowDialog() == true)
                {
                    string rutaOrigen = openFileDialog.FileName;
                    string nombreArchivo = Path.GetFileName(rutaOrigen);
                    string extension = Path.GetExtension(rutaOrigen);
                    long tamano = new FileInfo(rutaOrigen).Length;

                    string rutaCifrada = _fileService.GuardarArchivo(_pacienteId, rutaOrigen);

                    var archivo = new ArchivoMedico
                    {
                        PacienteId = _pacienteId,
                        NombreArchivo = nombreArchivo,
                        RutaArchivo = rutaCifrada,
                        TipoArchivo = extension,
                        TamanoBytes = tamano,
                        FechaSubida = DateTime.Now,
                        UsuarioId = SessionManager.CurrentUser.Id,
                        Cifrado = true
                    };

                    _dbService.CrearArchivoMedico(archivo);
                    CargarArchivos();
                    MessageBox.Show("Archivo subido y cifrado", "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                _logService.RegistrarError("ArchivosControl.SubirArchivo", ex.Message, ex.StackTrace);
                MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void AbrirArchivo(ArchivoMedico archivo)
        {
            if (archivo == null) return;

            try
            {
                string rutaTemporal = _fileService.AbrirArchivo(archivo);
                Process.Start(new ProcessStartInfo { FileName = rutaTemporal, UseShellExecute = true });
                MessageBox.Show("Archivo descifrado temporalmente", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                _logService.RegistrarError("ArchivosControl.AbrirArchivo", ex.Message, ex.StackTrace);
                MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void EliminarArchivo(ArchivoMedico archivo)
        {
            if (archivo == null) return;

            var result = MessageBox.Show(
                $"¿Eliminar '{archivo.NombreArchivo}'?",
                "Confirmar",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning
            );

            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    _fileService.EliminarArchivo(archivo.RutaArchivo);
                    _dbService.EliminarArchivoMedico(archivo.Id);
                    _logService.RegistrarCambioMedico(
                        SessionManager.CurrentUser.Id,
                        _pacienteId,
                        "Archivo",
                        "Eliminar",
                        archivo.NombreArchivo
                    );
                    CargarArchivos();
                    MessageBox.Show("Archivo eliminado", "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    _logService.RegistrarError("ArchivosControl.EliminarArchivo", ex.Message, ex.StackTrace);
                    MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}