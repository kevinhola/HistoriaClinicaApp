using System.Windows;
using HistoriaClinicaApp.Views;

namespace HistoriaClinicaApp
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            NavigateToLogin();
        }

        public void NavigateToLogin()
        {
            MainContent.Content = new LoginView(this);
        }

        public void NavigateToDashboard()
        {
            MainContent.Content = new DashboardView(this);
        }
    }
}