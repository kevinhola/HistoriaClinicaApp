using System;
using System.ComponentModel;
using System.Windows;
using HistoriaClinicaApp.Models;
using HistoriaClinicaApp.Security;
using HistoriaClinicaApp.Services;

namespace HistoriaClinicaApp.Views
{
    public partial class ConsultaDialog : Window, INotifyPropertyChanged
    {
        private readonly DatabaseService _dbService;
        private readonly LogService _logService;
        private readonly int _pacienteId;

        public Consulta Consulta { get; set; }
        public bool EsSoloLectura { get; set; }
        public bool EsEditable => !EsSoloLectura;

        public ConsultaDialog(int pacienteId, Consulta consulta = null, bool soloLectura = false)
        {
            InitializeComponent();
            _pacienteId = pacienteId;
            _dbService = new DatabaseService();
            _logService = new LogService();
            EsSoloLectura = soloLectura;

            Consulta = consulta ?? new Consulta
            {
                PacienteId = pacienteId,
                UsuarioId = SessionManager.CurrentUser.Id,
                FechaHora = DateTime.Now
            };

            DataContext = this;
        }

        private void Guardar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                _dbService.CrearConsulta(Consulta);
                _logService.RegistrarCambioMedico(
                    SessionManager.CurrentUser.Id,
                    _pacienteId,
                    "Consulta",
                    "Crear",
                    $"Motivo: {Consulta.Motivo}"
                );

                MessageBox.Show("Consulta guardada", "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);
                DialogResult = true;
                Close();
            }
            catch (Exception ex)
            {
                _logService.RegistrarError("ConsultaDialog.Guardar", ex.Message, ex.StackTrace);
                MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Cerrar_Click(object sender, RoutedEventArgs e)
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