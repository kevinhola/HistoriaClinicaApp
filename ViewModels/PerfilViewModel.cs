using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using HistoriaClinicaApp.Security;
using HistoriaClinicaApp.Services;
using HistoriaClinicaApp.Views;

namespace HistoriaClinicaApp.ViewModels
{
    public class PerfilViewModel : INotifyPropertyChanged
    {
        private readonly PerfilView _view;
        private readonly UserService _userService;
        private readonly LogService _logService;

        public string NombreUsuario => SessionManager.CurrentUser?.NombreUsuario ?? "";
        public string Rol => SessionManager.CurrentUser?.Rol?.Nombre ?? "";
        public string FechaCreacion => SessionManager.CurrentUser?.FechaCreacion.ToString("dd/MM/yyyy HH:mm") ?? "";

        private string _nuevoNombreUsuario;
        public string NuevoNombreUsuario
        {
            get => _nuevoNombreUsuario;
            set
            {
                _nuevoNombreUsuario = value;
                OnPropertyChanged(nameof(NuevoNombreUsuario));
            }
        }

        public string PasswordActual { get; set; }
        public string PasswordNueva { get; set; }
        public string PasswordConfirmar { get; set; }

        public ICommand CambiarNombreUsuarioCommand { get; }
        public ICommand CambiarPasswordCommand { get; }

        public PerfilViewModel(PerfilView view)
        {
            _view = view;
            _userService = new UserService();
            _logService = new LogService();

            CambiarNombreUsuarioCommand = new RelayCommand(CambiarNombreUsuario);
            CambiarPasswordCommand = new RelayCommand(CambiarPassword);
        }

        private void CambiarNombreUsuario()
        {
            if (string.IsNullOrWhiteSpace(NuevoNombreUsuario))
            {
                MessageBox.Show("Ingrese un nuevo nombre de usuario", "Validación", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                _userService.CambiarNombreUsuario(SessionManager.CurrentUser.Id, NuevoNombreUsuario);
                
                _logService.RegistrarAcceso(
                    SessionManager.CurrentUser.Id,
                    SessionManager.CurrentUser.NombreUsuario,
                    $"Cambió nombre de usuario a: {NuevoNombreUsuario}"
                );

                // Actualizar sesión
                SessionManager.CurrentUser.NombreUsuario = NuevoNombreUsuario;
                
                MessageBox.Show("Nombre de usuario actualizado correctamente", "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);
                NuevoNombreUsuario = "";
                
                // Refrescar vista
                OnPropertyChanged(nameof(NombreUsuario));
            }
            catch (Exception ex)
            {
                _logService.RegistrarError("PerfilViewModel.CambiarNombreUsuario", ex.Message, ex.StackTrace);
                MessageBox.Show("Error al cambiar nombre de usuario: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void CambiarPassword()
        {
            if (string.IsNullOrWhiteSpace(PasswordActual) || 
                string.IsNullOrWhiteSpace(PasswordNueva) || 
                string.IsNullOrWhiteSpace(PasswordConfirmar))
            {
                _view.MostrarMensaje("Complete todos los campos", true);
                return;
            }

            if (PasswordNueva != PasswordConfirmar)
            {
                _view.MostrarMensaje("Las contraseñas no coinciden", true);
                return;
            }

            if (PasswordNueva.Length < 6)
            {
                _view.MostrarMensaje("La contraseña debe tener al menos 6 caracteres", true);
                return;
            }

            try
            {
                bool resultado = _userService.CambiarPassword(
                    SessionManager.CurrentUser.Id,
                    PasswordActual,
                    PasswordNueva
                );

                if (resultado)
                {
                    _logService.RegistrarAcceso(
                        SessionManager.CurrentUser.Id,
                        SessionManager.CurrentUser.NombreUsuario,
                        "Cambió su contraseña"
                    );

                    _view.MostrarMensaje("Contraseña actualizada correctamente", false);
                    _view.LimpiarPasswords();
                }
                else
                {
                    _view.MostrarMensaje("La contraseña actual es incorrecta", true);
                }
            }
            catch (Exception ex)
            {
                _logService.RegistrarError("PerfilViewModel.CambiarPassword", ex.Message, ex.StackTrace);
                _view.MostrarMensaje("Error al cambiar contraseña: " + ex.Message, true);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}