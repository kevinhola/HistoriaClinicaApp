using System.Windows;
using System.Windows.Controls;
using HistoriaClinicaApp.ViewModels;

namespace HistoriaClinicaApp.Views
{
    public partial class LoginView : UserControl
    {
        
        private string _connectionString = "Data Source=historiaclinica.db";

        private readonly LoginViewModel _viewModel;

        public LoginView(MainWindow mainWindow)
        {
            MessageBox.Show(_connectionString);
            InitializeComponent();
            _viewModel = new LoginViewModel(mainWindow, this);
            DataContext = _viewModel;
        }

        private void TxtPassword_PasswordChanged(object sender, RoutedEventArgs e)
        {
            _viewModel.Password = TxtPassword.Password;
        }

        public void MostrarError(string mensaje)
        {
            TxtError.Text = mensaje;
            TxtError.Visibility = Visibility.Visible;
        }

        public void OcultarError()
        {
            TxtError.Visibility = Visibility.Collapsed;
        }
    }
}
