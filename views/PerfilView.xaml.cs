using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using HistoriaClinicaApp.ViewModels;

namespace HistoriaClinicaApp.Views
{
    public partial class PerfilView : UserControl
    {
        private readonly PerfilViewModel _viewModel;

        public PerfilView()
        {
            InitializeComponent();
            _viewModel = new PerfilViewModel(this);
            DataContext = _viewModel;
        }

        private void TxtPasswordActual_PasswordChanged(object sender, RoutedEventArgs e)
        {
            _viewModel.PasswordActual = TxtPasswordActual.Password;
        }

        private void TxtPasswordNueva_PasswordChanged(object sender, RoutedEventArgs e)
        {
            _viewModel.PasswordNueva = TxtPasswordNueva.Password;
        }

        private void TxtPasswordConfirmar_PasswordChanged(object sender, RoutedEventArgs e)
        {
            _viewModel.PasswordConfirmar = TxtPasswordConfirmar.Password;
        }

        public void MostrarMensaje(string mensaje, bool esError)
        {
            TxtMensaje.Text = mensaje;
            TxtMensaje.Foreground = new SolidColorBrush(esError ? Colors.Red : Colors.Green);
            TxtMensaje.Visibility = Visibility.Visible;
        }

        public void LimpiarPasswords()
        {
            TxtPasswordActual.Clear();
            TxtPasswordNueva.Clear();
            TxtPasswordConfirmar.Clear();
        }
    }
}