using System.ComponentModel;
using System.Windows;
using HistoriaClinicaApp.Models;
using HistoriaClinicaApp.Services;

namespace HistoriaClinicaApp.Views
{
    public partial class PacienteDetalleDialog : Window, INotifyPropertyChanged
    {
        private readonly DatabaseService _dbService;
        private readonly int _pacienteId;
        private Paciente _paciente;

        public string NombrePaciente => _paciente != null ? $"{_paciente.Apellido}, {_paciente.Nombre}" : "";
        public string DNIPaciente => _paciente?.DNI ?? "";
        public string EdadPaciente => _paciente != null ? $"{_paciente.Edad} aÃ±os" : "";
        public string SexoPaciente => _paciente?.Sexo ?? "";

        public PacienteDetalleDialog(int pacienteId)
        {
            InitializeComponent();
            _pacienteId = pacienteId;
            _dbService = new DatabaseService();
            
            DataContext = this;
            CargarDatos();
        }

        private void CargarDatos()
        {
            var pacientes = _dbService.ObtenerPacientes();
            _paciente = pacientes.Find(p => p.Id == _pacienteId);
            
            OnPropertyChanged(nameof(NombrePaciente));
            OnPropertyChanged(nameof(DNIPaciente));
            OnPropertyChanged(nameof(EdadPaciente));
            OnPropertyChanged(nameof(SexoPaciente));
            
            // Cargar vistas en pestaÃ±as
            HistoriaContent.Content = new HistoriaClinicaView(_pacienteId);
            ConsultasContent.Content = new ConsultasView(_pacienteId);
            EstudiosContent.Content = new EstudiosView(_pacienteId);
            ArchivosContent.Content = new ArchivosView(_pacienteId);
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

