using System.Windows;
using System.Windows.Controls;
using HistoriaClinicaApp.ViewModels;

namespace HistoriaClinicaApp.Views
{
    public partial class DashboardView : UserControl
    {
        private readonly DashboardViewModel _viewModel;

        public DashboardView(MainWindow mainWindow)
        {
            InitializeComponent();
            _viewModel = new DashboardViewModel(mainWindow, this);
            DataContext = _viewModel;
            
            MostrarInicio();
        }

        public void MostrarInicio()
        {
            var bienvenida = new StackPanel
            {
                VerticalAlignment = VerticalAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Center
            };

            var icono = new TextBlock
            {
                Text = "🏥",
                FontSize = 80,
                HorizontalAlignment = HorizontalAlignment.Center,
                Margin = new Thickness(0, 0, 0, 20)
            };

            var titulo = new TextBlock
            {
                Text = "Sistema de Historias Clínica Electrónica",
                FontSize = 28,
                FontWeight = FontWeights.Bold,
                HorizontalAlignment = HorizontalAlignment.Center,
                Margin = new Thickness(0, 0, 0, 10)
            };

            var subtitulo = new TextBlock
            {
                Text = "Gestión médica profesional y segura",
                FontSize = 16,
                HorizontalAlignment = HorizontalAlignment.Center,
                Foreground = System.Windows.Media.Brushes.Gray
            };

            bienvenida.Children.Add(icono);
            bienvenida.Children.Add(titulo);
            bienvenida.Children.Add(subtitulo);

            DashboardContent.Content = bienvenida;
        }

        public void MostrarPacientes()
        {
            DashboardContent.Content = new PacientesView();
        }

        public void MostrarAdmin()
        {
            DashboardContent.Content = new AdminView();
        }

        public void MostrarPerfil()
        {
            DashboardContent.Content = new PerfilView();
        }
    }
}