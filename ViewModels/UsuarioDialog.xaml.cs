using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using HistoriaClinicaApp.Models;
using HistoriaClinicaApp.Security;
using HistoriaClinicaApp.Services;

namespace HistoriaClinicaApp.Views
{
    public partial class UsuarioDialog : Window, INotifyPropertyChanged
    {
        private readonly DatabaseService _dbService;
        private string _password;

        public Usuario Usuario { get; set; }
        public ObservableCollection<Rol> Roles { get; set; }
        
        private Rol _rolSeleccionado;
        public Rol RolSeleccionado
        {
            get => _rolSeleccionado;
            set
            {
                _rolSeleccionado = value;
                if (value != null)
                {
                    Usuario.RolId = value.Id;
                }
                OnPropertyChanged(nameof(RolSeleccionado));
            }
        }

        public bool EsNuevo { get; set; }

        public UsuarioDialog(Usuario usuario = null)
        {
            InitializeComponent();
            _dbService = new DatabaseService();
            
            Roles = new ObservableCollection<Rol>(_dbService.ObtenerRoles());
            
            if (usuario == null)
            {
                Usuario = new Usuario { Activo = true };
                EsNuevo = true;
            }
            else
            {
                Usuario = new Usuario
                {
                    Id = usuario.Id,
                    NombreUsuario = usuario.NombreUsuario,
                    RolId = usuario.RolId,
                    Activo = usuario.Activo
                };
                EsNuevo = false;
                RolSeleccionado = Roles.FirstOrDefault(r => r.Id == usuario.RolId);
            }
            
            DataContext = this;
        }

        private void TxtPassword_PasswordChanged(object sender, RoutedEventArgs e)
        {
            _password = TxtPassword.Password;
        }

        private void Guardar_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(Usuario.NombreUsuario))
            {
                MessageBox.Show("Ingrese nombre de usuario", "Validación", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (EsNuevo && string.IsNullOrWhiteSpace(_password))
            {
                MessageBox.Show("Ingrese contraseña", "Validación", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (RolSeleccionado == null)
            {
                MessageBox.Show("Seleccione un rol", "Validación", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (EsNuevo)
            {
                Usuario.PasswordHash = PasswordHasher.HashPassword(_password);
            }

            DialogResult = true;
            Close();
        }

        private void Cancelar_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}