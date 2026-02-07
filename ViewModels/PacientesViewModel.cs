using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using HistoriaClinicaApp.Models;
using HistoriaClinicaApp.Services;
using HistoriaClinicaApp.Security;
using HistoriaClinicaApp.Views;

namespace HistoriaClinicaApp.ViewModels
{
    public class PacientesViewModel : INotifyPropertyChanged
    {
        private readonly DatabaseService _dbService;
        private readonly LogService _logService;
        private ObservableCollection<Paciente> _pacientes;
        private ObservableCollection<Paciente> _pacientesFiltrados;
        private Paciente _pacienteSeleccionado;
        private string _filtroBusqueda;

        public ObservableCollection<Paciente> Pacientes
        {
            get => _pacientes;
            set
            {
                _pacientes = value;
                OnPropertyChanged(nameof(Pacientes));
            }
        }

        public ObservableCollection<Paciente> PacientesFiltrados
        {
            get => _pacientesFiltrados;
            set
            {
                _pacientesFiltrados = value;
                OnPropertyChanged(nameof(PacientesFiltrados));
            }
        }

        public Paciente PacienteSeleccionado
        {
            get => _pacienteSeleccionado;
            set
            {
                _pacienteSeleccionado = value;
                OnPropertyChanged(nameof(PacienteSeleccionado));
            }
        }

        public string FiltroBusqueda
        {
            get => _filtroBusqueda;
            set
            {
                _filtroBusqueda = value;
                OnPropertyChanged(nameof(FiltroBusqueda));
                FiltrarPacientes();
            }
        }

        public ICommand NuevoPacienteCommand { get; }
        public ICommand EditarPacienteCommand { get; }
        public ICommand EliminarPacienteCommand { get; }
        public ICommand VerDetalleCommand { get; }

        public PacientesViewModel()
        {
            _dbService = new DatabaseService();
            _logService = new LogService();

            NuevoPacienteCommand = new RelayCommand(NuevoPaciente);
            EditarPacienteCommand = new RelayCommand<Paciente>(EditarPaciente);
            EliminarPacienteCommand = new RelayCommand<Paciente>(EliminarPaciente);
            VerDetalleCommand = new RelayCommand<Paciente>(VerDetalle);

            CargarPacientes();
        }

        private void CargarPacientes()
        {
            Pacientes = new ObservableCollection<Paciente>(_dbService.ObtenerPacientes());
            PacientesFiltrados = new ObservableCollection<Paciente>(Pacientes);
        }

        private void FiltrarPacientes()
        {
            if (string.IsNullOrWhiteSpace(FiltroBusqueda))
            {
                PacientesFiltrados = new ObservableCollection<Paciente>(Pacientes);
            }
            else
            {
                var filtro = FiltroBusqueda.ToLower();
                var filtrados = Pacientes.Where(p =>
                    p.DNI.ToLower().Contains(filtro) ||
                    p.Nombre.ToLower().Contains(filtro) ||
                    p.Apellido.ToLower().Contains(filtro)
                ).ToList();
                
                PacientesFiltrados = new ObservableCollection<Paciente>(filtrados);
            }
        }

        private void NuevoPaciente()
        {
            var dialog = new PacienteDialog();
            if (dialog.ShowDialog() == true)
            {
                try
                {
                    var paciente = dialog.Paciente;
                    _dbService.CrearPaciente(paciente);
                    
                    _logService.RegistrarAcceso(
                        SessionManager.CurrentUser.Id,
                        SessionManager.CurrentUser.NombreUsuario,
                        $"Creó paciente: {paciente.Nombre} {paciente.Apellido} (DNI: {paciente.DNI})"
                    );
                    
                    CargarPacientes();
                    MessageBox.Show("Paciente creado exitosamente", "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    _logService.RegistrarError("PacientesViewModel.NuevoPaciente", ex.Message, ex.StackTrace);
                    MessageBox.Show("Error al crear paciente: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void EditarPaciente(Paciente paciente)
        {
            if (paciente == null) return;

            var dialog = new PacienteDialog(paciente);
            if (dialog.ShowDialog() == true)
            {
                try
                {
                    _dbService.ActualizarPaciente(dialog.Paciente);
                    
                    _logService.RegistrarAcceso(
                        SessionManager.CurrentUser.Id,
                        SessionManager.CurrentUser.NombreUsuario,
                        $"Editó paciente: {paciente.Nombre} {paciente.Apellido} (DNI: {paciente.DNI})"
                    );
                    
                    CargarPacientes();
                    MessageBox.Show("Paciente actualizado exitosamente", "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    _logService.RegistrarError("PacientesViewModel.EditarPaciente", ex.Message, ex.StackTrace);
                    MessageBox.Show("Error al actualizar paciente: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void EliminarPaciente(Paciente paciente)
        {
            if (paciente == null) return;

            var result = MessageBox.Show(
                $"¿Está seguro de eliminar al paciente {paciente.Nombre} {paciente.Apellido}?",
                "Confirmar eliminación",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning
            );

            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    _dbService.EliminarPaciente(paciente.Id);
                    
                    _logService.RegistrarAcceso(
                        SessionManager.CurrentUser.Id,
                        SessionManager.CurrentUser.NombreUsuario,
                        $"Eliminó paciente: {paciente.Nombre} {paciente.Apellido} (DNI: {paciente.DNI})"
                    );
                    
                    CargarPacientes();
                    MessageBox.Show("Paciente eliminado exitosamente", "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    _logService.RegistrarError("PacientesViewModel.EliminarPaciente", ex.Message, ex.StackTrace);
                    MessageBox.Show("Error al eliminar paciente: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void VerDetalle(Paciente paciente)
        {
            if (paciente == null) return;
            var window = new PacienteDetalleWindow(paciente.Id);
            window.ShowDialog();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}