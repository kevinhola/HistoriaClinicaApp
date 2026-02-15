using System;
using System.ComponentModel;
using System.Windows;
using HistoriaClinicaApp.Models;
using HistoriaClinicaApp.Security;
using HistoriaClinicaApp.Services;

namespace HistoriaClinicaApp.Views
{
    public partial class EstudioDialog : Window, INotifyPropertyChanged
    {
        private readonly DatabaseService _dbService;
        private readonly LogService _logService;
        private readonly int _pacienteId;

        public Estudio Estudio { get; set; }

        public EstudioDialog(int pacienteId, Estudio estudioExistente)
{
    InitializeComponent();
    _pacienteId = pacienteId;
    _dbService = new DatabaseService();
    _logService = new LogService();

    Estudio = estudioExistente;

    DataContext = this;
}

        public EstudioDialog(int pacienteId)
        {
            InitializeComponent();
            _pacienteId = pacienteId;
            _dbService = new DatabaseService();
            _logService = new LogService();

            Estudio = new Estudio
            {
                PacienteId = pacienteId,
                UsuarioId = SessionManager.CurrentUser.Id,
                Fecha = DateTime.Now
            };

            DataContext = this;
        }

        private void Guardar_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(Estudio.TipoEstudio))
            {
                MessageBox.Show("Seleccione tipo de estudio", "ValidaciÃ³n", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                _dbService.CrearEstudio(Estudio);
                _logService.RegistrarCambioMedico(
                    SessionManager.CurrentUser.Id,
                    _pacienteId,
                    "Estudio",
                    "Crear",
                    $"Tipo: {Estudio.TipoEstudio}"
                );

                MessageBox.Show("Estudio guardado", "Ã‰xito", MessageBoxButton.OK, MessageBoxImage.Information);
                DialogResult = true;
                Close();
            }
            catch (Exception ex)
            {
                _logService.RegistrarError("EstudioDialog.Guardar", ex.Message, ex.StackTrace);
                MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Cancelar_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
