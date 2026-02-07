using System.Windows.Controls;
using HistoriaClinicaApp.ViewModels;

namespace HistoriaClinicaApp.Views
{
    public partial class PacientesView : UserControl
    {
        public PacientesView()
        {
            InitializeComponent();
            DataContext = new PacientesViewModel();
        }
    }
}