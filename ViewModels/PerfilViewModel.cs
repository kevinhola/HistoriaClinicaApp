using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using HistoriaClinicaApp.Security;

namespace HistoriaClinicaApp.ViewModels
{
    public class PerfilViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        
        public string NombreUsuario => SessionManager.CurrentUser?.NombreUsuario ?? "Usuario";
        public string RolNombre => SessionManager.CurrentUser?.Rol?.Nombre ?? "Sin rol";
        public string FechaCreacion => "N/A";
        
        private string _nuevoNombre;
        public string NuevoNombre
        {
            get => _nuevoNombre;
            set { _nuevoNombre = value; OnPropertyChanged(nameof(NuevoNombre)); }
        }
        
        private string _passwordActual;
        public string PasswordActual
        {
            get => _passwordActual;
            set { _passwordActual = value; OnPropertyChanged(nameof(PasswordActual)); }
        }
        
        private string _passwordNueva;
        public string PasswordNueva
        {
            get => _passwordNueva;
            set { _passwordNueva = value; OnPropertyChanged(nameof(PasswordNueva)); }
        }
        
        private ICommand _cambiarNombreCommand;
        public ICommand CambiarNombreCommand
        {
            get
            {
                if (_cambiarNombreCommand == null)
                    _cambiarNombreCommand = new RelayCommand(CambiarNombre);
                return _cambiarNombreCommand;
            }
        }
        
        private ICommand _cambiarPasswordCommand;
        public ICommand CambiarPasswordCommand
        {
            get
            {
                if (_cambiarPasswordCommand == null)
                    _cambiarPasswordCommand = new RelayCommand(CambiarPassword);
                return _cambiarPasswordCommand;
            }
        }
        
        private void CambiarNombre(object parameter)
        {
            if (string.IsNullOrWhiteSpace(NuevoNombre))
            {
                MessageBox.Show("Ingrese un nombre valido", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            MessageBox.Show("Funcionalidad temporalmente deshabilitada", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        
        private void CambiarPassword(object parameter)
        {
            if (string.IsNullOrWhiteSpace(PasswordActual) || string.IsNullOrWhiteSpace(PasswordNueva))
            {
                MessageBox.Show("Complete todos los campos", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            MessageBox.Show("Funcionalidad temporalmente deshabilitada", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
