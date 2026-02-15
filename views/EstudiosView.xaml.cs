using System.Windows.Controls;
using HistoriaClinicaApp.ViewModels;

namespace HistoriaClinicaApp.Views
{
    public partial class EstudiosView : UserControl
    {
        public EstudiosView(int pacienteId)
        {
            InitializeComponent();
            DataContext = new EstudiosViewModel(pacienteId);
        }
    }
}
