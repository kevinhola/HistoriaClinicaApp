using System.Windows.Controls;
using HistoriaClinicaApp.ViewModels;

namespace HistoriaClinicaApp.Views
{
    public partial class HistoriaClinicaView : UserControl
    {
        public HistoriaClinicaView(int pacienteId)
        {
            InitializeComponent();
            DataContext = new HistoriaClinicaViewModel(pacienteId);
        }
    }
}