using System.Windows.Controls;
using HistoriaClinicaApp.ViewModels;

namespace HistoriaClinicaApp.Views
{
    public partial class HistoriaClinicaControl : UserControl
    {
        public HistoriaClinicaControl(int pacienteId)
        {
            InitializeComponent();
            DataContext = new HistoriaClinicaViewModel(pacienteId);
        }
    }
}
