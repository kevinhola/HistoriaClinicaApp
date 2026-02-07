using System.ComponentModel;
using HistoriaClinicaApp.Models;
using HistoriaClinicaApp.Services;

namespace HistoriaClinicaApp.ViewModels
{
    public class PacienteDetalleViewModel : INotifyPropertyChanged
    {
        private readonly DatabaseService _dbService;
        private readonly int _pacienteId;
        private Paciente _paciente;

        public Paciente Paciente
        {
            get => _paciente;
            set
            {
                _paciente = value;
                OnPropertyChanged(nameof(Paciente));
                OnPropertyChanged(nameof(NombreCompleto));
            }
        }

        public string NombreCompleto => _paciente != null ? $"{_paciente.Apellido}, {_paciente.Nombre}" : "";

        public PacienteDetalleViewModel(int pacienteId)
        {
            _pacienteId = pacienteId;
            _dbService = new DatabaseService();
            CargarPaciente();
        }

        private void CargarPaciente()
        {
            var pacientes = _dbService.ObtenerPacientes();
            Paciente = pacientes.Find(p => p.Id == _pacienteId);
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}