using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Controls;
using System.Windows.Input;
using HistoriaClinicaApp.Models;
using HistoriaClinicaApp.Services;
using HistoriaClinicaApp.ViewModels;


namespace HistoriaClinicaApp.Views
{
    public partial class ConsultasControl : UserControl, INotifyPropertyChanged
    {
        private readonly DatabaseService _dbService;
        private readonly int _pacienteId;
        private ObservableCollection<Consulta> _consultas;

        public ObservableCollection<Consulta> Consultas
        {
            get => _consultas;
            set
            {
                _consultas = value;
                OnPropertyChanged(nameof(Consultas));
            }
        }

        public ICommand NuevaConsultaCommand { get; }
        public ICommand VerConsultaCommand { get; }

        public ConsultasControl(int pacienteId)
        {
            InitializeComponent();
            _pacienteId = pacienteId;
            _dbService = new DatabaseService();

            NuevaConsultaCommand = new RelayCommand(NuevaConsulta);
            VerConsultaCommand = new RelayCommand<Consulta>(VerConsulta);

            DataContext = this;
            CargarConsultas();
        }

        private void CargarConsultas()
        {
            Consultas = new ObservableCollection<Consulta>(_dbService.ObtenerConsultas(_pacienteId));
        }

        private void NuevaConsulta()
        {
            var dialog = new ConsultaDialog(_pacienteId);
            if (dialog.ShowDialog() == true)
            {
                CargarConsultas();
            }
        }

        private void VerConsulta(Consulta consulta)
        {
            if (consulta != null)
            {
                var dialog = new ConsultaDialog(_pacienteId, consulta, true);
                dialog.ShowDialog();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}

