using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using HistoriaClinicaApp.Services;
using HistoriaClinicaApp.Views;

namespace HistoriaClinicaApp.ViewModels
{
    public class LoginViewModel : INotifyPropertyChanged
    {
        private readonly MainWindow _mainWindow;
        private readonly LoginView _view;
        private readonly AuthService _authService;
        private readonly LogService _logService;
        
        private string _usuario;

        public string Usuario
        {
            get => _usuario;
            set
            {
                _usuario = value;
                OnPropertyChanged(nameof(Usuario));
            }
        }

        public string Password { get; set; }

        public ICommand LoginCommand { get; }

        public LoginViewModel(MainWindow mainWindow, LoginView view)
        {
            _mainWindow = mainWindow;
            _view = view;
            _authService = new AuthService();
            _logService = new LogService();
            
            LoginCommand = new RelayCommand(Login);
        }

        private void Login()
        {
            _view.OcultarError();
            
            if (string.IsNullOrWhiteSpace(Usuario) || string.IsNullOrWhiteSpace(Password))
            {
                _view.MostrarError("Por favor ingrese usuario y contraseÃ±a");
                return;
            }

            try
            {
                var usuario = _authService.Autenticar(Usuario, Password);
                
                if (usuario != null)
                {
                    // Registrar log de acceso exitoso
                    _logService.RegistrarAcceso(usuario.Id, usuario.NombreUsuario, "Login exitoso");
                    
                    // Navegar al dashboard
                    _mainWindow.NavigateToDashboard();
                }
                else
                {
                    // Registrar intento fallido
                    _logService.RegistrarAcceso(0, Usuario, "Login fallido - Credenciales invÃ¡lidas");
                    _view.MostrarError("Usuario o contraseÃ±a incorrectos");
                }
            }
            catch (Exception ex)
            {
                _logService.RegistrarError("LoginViewModel.Login", ex.Message, ex.StackTrace);
                _view.MostrarError("Error al iniciar sesiÃ³n: " + ex.Message);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
