using System.Windows;
using HistoriaClinicaApp.ViewModels;

namespace HistoriaClinicaApp.Views
{
    public partial class PacienteDetalleWindow : Window
    {
        public PacienteDetalleWindow(int pacienteId)
        {
            InitializeComponent();
            DataContext = new PacienteDetalleViewModel(pacienteId);
            
            HistoriaContent.Content = new HistoriaClinicaControl(pacienteId);
            ConsultasContent.Content = new ConsultasControl(pacienteId);
            EstudiosContent.Content = new EstudiosControl(pacienteId);
            ArchivosContent.Content = new ArchivosControl(pacienteId);
        }
    }
}