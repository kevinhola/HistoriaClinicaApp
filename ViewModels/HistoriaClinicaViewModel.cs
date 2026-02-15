using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using HistoriaClinicaApp.Models;
using HistoriaClinicaApp.Services;
using HistoriaClinicaApp.Security;

namespace HistoriaClinicaApp.ViewModels
{
    public class HistoriaClinicaViewModel : INotifyPropertyChanged
    {
        private readonly DatabaseService _dbService;
        private readonly LogService _logService;
        private readonly int _pacienteId;
        private HistoriaClinica _historia;

        public HistoriaClinica Historia
        {
            get => _historia;
            set
            {
                _historia = value;
                OnPropertyChanged(nameof(Historia));
            }
        }

        public ICommand GuardarCommand { get; }

        public HistoriaClinicaViewModel(int pacienteId)
        {
            _pacienteId = pacienteId;
            _dbService = new DatabaseService();
            _logService = new LogService();

            GuardarCommand = new RelayCommand(Guardar);

            CargarHistoria();
        }

        private void CargarHistoria()
        {
            Historia = _dbService.ObtenerHistoriaClinica(_pacienteId).Result;
            if (Historia == null)
            {
                Historia = new HistoriaClinica
                {
                    PacienteId = _pacienteId,
                    UsuarioCreacion = SessionManager.CurrentUser.Id,
                    UsuarioModificacion = SessionManager.CurrentUser.Id
                };
            }
        }

        private void Guardar()
        {
            try
            {
                Historia.UsuarioModificacion = SessionManager.CurrentUser.Id;

                if (Historia.Id == 0)
                {
                    _dbService.CrearHistoriaClinica(Historia);
                    _logService.RegistrarCambioMedico(
                        SessionManager.CurrentUser.Id,
                        _pacienteId,
                        "HistoriaClinica",
                        "Crear",
                        "Historia clÃ­nica creada"
                    );
                }
                else
                {
                    _dbService.ActualizarHistoriaClinica(Historia);
                    _logService.RegistrarCambioMedico(
                        SessionManager.CurrentUser.Id,
                        _pacienteId,
                        "HistoriaClinica",
                        "Actualizar",
                        "Historia clÃ­nica actualizada"
                    );
                }

                MessageBox.Show("Historia clÃ­nica guardada", "Ã‰xito", MessageBoxButton.OK, MessageBoxImage.Information);
                CargarHistoria();
            }
            catch (Exception ex)
            {
                _logService.RegistrarError("HistoriaClinicaViewModel.Guardar", ex.Message, ex.StackTrace);
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
