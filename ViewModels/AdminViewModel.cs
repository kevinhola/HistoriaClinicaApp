using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;
using HistoriaClinicaApp.Models;
using HistoriaClinicaApp.Services;
using HistoriaClinicaApp.Security;
using HistoriaClinicaApp.Views;

namespace HistoriaClinicaApp.ViewModels
{
    public class AdminViewModel : INotifyPropertyChanged
    {
        private readonly DatabaseService _dbService;
        private readonly LogService _logService;
        private ObservableCollection<Usuario> _usuarios;
        private Usuario _usuarioSeleccionado;

        public ObservableCollection<Usuario> Usuarios
        {
            get => _usuarios;
            set
            {
                _usuarios = value;
                OnPropertyChanged(nameof(Usuarios));
            }
        }

        public Usuario UsuarioSeleccionado
        {
            get => _usuarioSeleccionado;
            set
            {
                _usuarioSeleccionado = value;
                OnPropertyChanged(nameof(UsuarioSeleccionado));
            }
        }

        public ICommand NuevoUsuarioCommand { get; }
        public ICommand EditarUsuarioCommand { get; }
        public ICommand ResetearPasswordCommand { get; }
        public ICommand CambiarEstadoCommand { get; }

        public AdminViewModel()
        {
            _dbService = new DatabaseService();
            _logService = new LogService();

            NuevoUsuarioCommand = new RelayCommand(NuevoUsuario);
            EditarUsuarioCommand = new RelayCommand<Usuario>(EditarUsuario);
            ResetearPasswordCommand = new RelayCommand<Usuario>(ResetearPassword);
            CambiarEstadoCommand = new RelayCommand<Usuario>(CambiarEstado);

            CargarUsuarios();
        }

        private void CargarUsuarios()
        {
            Usuarios = new ObservableCollection<Usuario>(_dbService.ObtenerTodosUsuarios());
        }

        private void NuevoUsuario()
        {
            var dialog = new UsuarioDialog();
            if (dialog.ShowDialog() == true)
            {
                try
                {
                    _dbService.CrearUsuario(dialog.Usuario);
                    _logService.RegistrarAcceso(
                        SessionManager.CurrentUser.Id,
                        SessionManager.CurrentUser.NombreUsuario,
                        $"Creó usuario: {dialog.Usuario.NombreUsuario}"
                    );
                    CargarUsuarios();
                    MessageBox.Show("Usuario creado exitosamente", "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    _logService.RegistrarError("AdminViewModel.NuevoUsuario", ex.Message, ex.StackTrace);
                    MessageBox.Show("Error al crear usuario: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void EditarUsuario(Usuario usuario)
        {
            if (usuario == null) return;

            var dialog = new UsuarioDialog(usuario);
            if (dialog.ShowDialog() == true)
            {
                try
                {
                    _dbService.ActualizarUsuario(dialog.Usuario);
                    _logService.RegistrarAcceso(
                        SessionManager.CurrentUser.Id,
                        SessionManager.CurrentUser.NombreUsuario,
                        $"Editó usuario: {usuario.NombreUsuario}"
                    );
                    CargarUsuarios();
                    MessageBox.Show("Usuario actualizado exitosamente", "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    _logService.RegistrarError("AdminViewModel.EditarUsuario", ex.Message, ex.StackTrace);
                    MessageBox.Show("Error al actualizar usuario: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void ResetearPassword(Usuario usuario)
        {
            if (usuario == null) return;

            var result = MessageBox.Show(
                $"¿Resetear contraseña de '{usuario.NombreUsuario}' a 'Temporal123!'?",
                "Confirmar",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question
            );

            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    string nuevoHash = PasswordHasher.HashPassword("Temporal123!");
                    _dbService.ResetearPassword(usuario.Id, nuevoHash);
                    _logService.RegistrarAcceso(
                        SessionManager.CurrentUser.Id,
                        SessionManager.CurrentUser.NombreUsuario,
                        $"Reseteó contraseña de: {usuario.NombreUsuario}"
                    );
                    MessageBox.Show("Contraseña reseteada a: Temporal123!", "Éxito", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    _logService.RegistrarError("AdminViewModel.ResetearPassword", ex.Message, ex.StackTrace);
                    MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void CambiarEstado(Usuario usuario)
        {
            if (usuario == null) return;

            try
            {
                usuario.Activo = !usuario.Activo;
                _dbService.ActualizarUsuario(usuario);
                _logService.RegistrarAcceso(
                    SessionManager.CurrentUser.Id,
                    SessionManager.CurrentUser.NombreUsuario,
                    $"Cambió estado de {usuario.NombreUsuario} a: {(usuario.Activo ? "Activo" : "Inactivo")}"
                );
                CargarUsuarios();
            }
            catch (Exception ex)
            {
                _logService.RegistrarError("AdminViewModel.CambiarEstado", ex.Message, ex.StackTrace);
                MessageBox.Show("Error: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}