using System.Windows.Controls;
using HistoriaClinicaApp.ViewModels;

namespace HistoriaClinicaApp.Views
{
    public partial class ArchivosView : UserControl
    {
        public ArchivosView(int pacienteId)
        {
            InitializeComponent();
            DataContext = new ArchivosViewModel(pacienteId);
        }
    }
}