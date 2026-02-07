using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using HistoriaClinicaApp.Models;
using HistoriaClinicaApp.Services;
using HistoriaClinicaApp.Views;

namespace HistoriaClinicaApp.ViewModels
{
    public class ConsultasViewModel : INotifyPropertyChanged
    {
        private readonly DatabaseService _dbService;
        private readonly int _pacienteId;
        private Paciente _paciente;
        private ObservableCollection<Consulta> _consultas;
        private Consulta _consultaSeleccionada;

        public ObservableCollection<Consulta> Consultas
        {
            get => _consultas;
            set
            {
                _consultas = value;
                OnPropertyChanged(nameof(Consultas));
            }
        }

        public Consulta ConsultaSeleccionada
        {
            get => _consultaSeleccionada;
            set
            {
                _consultaSeleccionada = value;
                OnPropertyChanged(nameof(ConsultaSeleccionada));
            }
        }

        public string NombrePaciente => _paciente != null ? $"{_paciente.Apellido}, {_paciente.Nombre}" : "";
        public string DNIPaciente => _paciente?.DNI ?? "";

        public ICommand NuevaConsultaCommand { get; }
        public ICommand VerConsultaCommand { get; }
        public ICommand RecetasCommand { get; }

        public ConsultasViewModel(int pacienteId)
        {
            _pacienteId = pacienteId;
            _dbService = new DatabaseService();

            NuevaConsultaCommand = new RelayCommand(NuevaConsulta);
            VerConsultaCommand = new RelayCommand<Consulta>(VerConsulta);
            RecetasCommand = new RelayCommand<Consulta>(VerRecetas);

            CargarDatos();
        }

        private void CargarDatos()
        {
            var pacientes = _dbService.ObtenerPacientes();
            _paciente = pacientes.Find(p => p.Id == _pacienteId);
            
            Consultas = new ObservableCollection<Consulta>(_dbService.ObtenerConsultas(_pacienteId));
            
            OnPropertyChanged(nameof(NombrePaciente));
            OnPropertyChanged(nameof(DNIPaciente));
        }

        private void NuevaConsulta()
        {
            var dialog = new ConsultaDialog(_pacienteId);
            if (dialog.ShowDialog() == true)
            {
                CargarDatos();
            }
        }

        private void VerConsulta(Consulta consulta)
        {
            if (consulta == null) return;
            var dialog = new ConsultaDialog(_pacienteId, consulta);
            dialog.ShowDialog();
        }

        private void VerRecetas(Consulta consulta)
        {
            if (consulta == null) return;
            MessageBox.Show($"Recetas de la consulta del {consulta.FechaHora:dd/MM/yyyy}", "Recetas", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}