using System.Windows.Controls;
using HistoriaClinicaApp.ViewModels;

namespace HistoriaClinicaApp.Views
{
    public partial class ConsultasView : UserControl
    {
        public ConsultasView(int pacienteId)
        {
            InitializeComponent();
            DataContext = new ConsultasViewModel(pacienteId);
        }
    }
}