using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using HistoriaClinicaApp.Models;
using HistoriaClinicaApp.Services;
using HistoriaClinicaApp.Views;
using HistoriaClinicaApp.Security;
using HistoriaClinicaApp.ViewModels;


namespace HistoriaClinicaApp.ViewModels
{
    public class EstudiosViewModel : INotifyPropertyChanged
    {
        private readonly DatabaseService _dbService;
        private readonly LogService _logService;
        private readonly int _pacienteId;
        private Paciente _paciente;
        private ObservableCollection<Estudio> _estudios;
        private Estudio _estudioSeleccionado;

        public ObservableCollection<Estudio> Estudios
        {
            get => _estudios;
            set
            {
                _estudios = value;
                OnPropertyChanged(nameof(Estudios));
            }
        }

        public Estudio EstudioSeleccionado
        {
            get => _estudioSeleccionado;
            set
            {
                _estudioSeleccionado = value;
                OnPropertyChanged(nameof(EstudioSeleccionado));
            }
        }

        public string NombrePaciente => _paciente != null ? $"{_paciente.Apellido}, {_paciente.Nombre}" : "";
        public string DNIPaciente => _paciente?.DNI ?? "";

        public ICommand NuevoEstudioCommand { get; }
        public ICommand EditarEstudioCommand { get; }
        public ICommand CompletarEstudioCommand { get; }

        public EstudiosViewModel(int pacienteId)
        {
            _pacienteId = pacienteId;
            _dbService = new DatabaseService();
            _logService = new LogService();

            NuevoEstudioCommand = new RelayCommand(NuevoEstudio);
            EditarEstudioCommand = new RelayCommand<Estudio>(EditarEstudio);
            CompletarEstudioCommand = new RelayCommand<Estudio>(CompletarEstudio);

            CargarDatos();
        }

        private void CargarDatos()
        {
            var pacientes = _dbService.ObtenerPacientes();
            _paciente = pacientes.Find(p => p.Id == _pacienteId);
            
            Estudios = new ObservableCollection<Estudio>(_dbService.ObtenerEstudios(_pacienteId));
            
            OnPropertyChanged(nameof(NombrePaciente));
            OnPropertyChanged(nameof(DNIPaciente));
        }

        private void NuevoEstudio()
        {
            var dialog = new EstudioDialog(_pacienteId);
            if (dialog.ShowDialog() == true)
            {
                CargarDatos();
            }
        }

        private void EditarEstudio(Estudio estudio)
        {
            if (estudio == null) return;
            var dialog = new EstudioDialog(_pacienteId, estudio);
            if (dialog.ShowDialog() == true)
            {
                CargarDatos();
            }
        }

        private void CompletarEstudio(Estudio estudio)
        {
            if (estudio == null) return;

            try
            {
                estudio.Estado = "Completado";
                estudio.FechaResultado = DateTime.Now;
                _dbService.ActualizarEstudio(estudio);

                _logService.RegistrarCambioMedico(
                    SessionManager.CurrentUser.Id,
                    _pacienteId,
                    "Estudio",
                    "Completar",
                    $"Estudio completado: {estudio.TipoEstudio}"
                );

                CargarDatos();
                MessageBox.Show("Estudio marcado como completado", "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                _logService.RegistrarError("EstudiosViewModel.CompletarEstudio", ex.Message, ex.StackTrace);
                MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}