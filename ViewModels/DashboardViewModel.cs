using System.ComponentModel;
using System.Windows.Input;
using HistoriaClinicaApp.Security;
using HistoriaClinicaApp.Services;
using HistoriaClinicaApp.Views;

namespace HistoriaClinicaApp.ViewModels
{
    public class DashboardViewModel : INotifyPropertyChanged
    {
        private readonly MainWindow _mainWindow;
        private readonly DashboardView _view;
        private readonly LogService _logService;

        public string UsuarioActual => SessionManager.CurrentUser?.NombreUsuario ?? "";
        public string RolActual => SessionManager.CurrentUser?.Rol?.Nombre ?? "";
        public bool EsAdmin => SessionManager.CurrentUser?.Rol?.Nombre == "Administrador";

        public ICommand NavInicioCommand { get; }
        public ICommand NavPacientesCommand { get; }
        public ICommand NavAdminCommand { get; }
        public ICommand NavPerfilCommand { get; }
        public ICommand CerrarSesionCommand { get; }

        public DashboardViewModel(MainWindow mainWindow, DashboardView view)
        {
            _mainWindow = mainWindow;
            _view = view;
            _logService = new LogService();

            NavInicioCommand = new RelayCommand(() => _view.MostrarInicio());
            NavPacientesCommand = new RelayCommand(() => _view.MostrarPacientes());
            NavAdminCommand = new RelayCommand(() => _view.MostrarAdmin());
            NavPerfilCommand = new RelayCommand(() => _view.MostrarPerfil());
            CerrarSesionCommand = new RelayCommand(CerrarSesion);
        }

        private void CerrarSesion()
        {
            _logService.RegistrarAcceso(
                SessionManager.CurrentUser.Id,
                SessionManager.CurrentUser.NombreUsuario,
                "Logout"
            );

            SessionManager.Logout();
            _mainWindow.NavigateToLogin();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}