using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Controls;
using System.Windows.Input;
using HistoriaClinicaApp.Models;
using HistoriaClinicaApp.Services;
using HistoriaClinicaApp.ViewModels;


namespace HistoriaClinicaApp.Views
{
    public partial class EstudiosControl : UserControl, INotifyPropertyChanged
    {
        private readonly DatabaseService _dbService;
        private readonly int _pacienteId;
        private ObservableCollection<Estudio> _estudios;

        public ObservableCollection<Estudio> Estudios
        {
            get => _estudios;
            set
            {
                _estudios = value;
                OnPropertyChanged(nameof(Estudios));
            }
        }

        public ICommand NuevoEstudioCommand { get; }

        public EstudiosControl(int pacienteId)
        {
            InitializeComponent();
            _pacienteId = pacienteId;
            _dbService = new DatabaseService();

            NuevoEstudioCommand = new RelayCommand(NuevoEstudio);

            DataContext = this;
            CargarEstudios();
        }

        private void CargarEstudios()
        {
            Estudios = new ObservableCollection<Estudio>(_dbService.ObtenerEstudios(_pacienteId));
        }

        private void NuevoEstudio()
        {
            var dialog = new EstudioDialog(_pacienteId);
            if (dialog.ShowDialog() == true)
            {
                CargarEstudios();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}