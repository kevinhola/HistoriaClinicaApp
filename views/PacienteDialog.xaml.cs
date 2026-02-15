using System;
using System.ComponentModel;
using System.Windows;
using HistoriaClinicaApp.Models;

namespace HistoriaClinicaApp.Views
{
    public partial class PacienteDialog : Window, INotifyPropertyChanged
    {
        public Paciente Paciente { get; set; }

        public PacienteDialog(Paciente paciente = null)
        {
            InitializeComponent();
            
            Paciente = paciente ?? new Paciente
            {
                FechaNacimiento = DateTime.Now.AddYears(-30)
            };
            
            DataContext = this;
        }

        private void Guardar_Click(object sender, RoutedEventArgs e)
        {
            // Validaciones
            if (string.IsNullOrWhiteSpace(Paciente.DNI))
            {
                MessageBox.Show("El DNI es obligatorio", "ValidaciÃ³n", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(Paciente.Nombre))
            {
                MessageBox.Show("El nombre es obligatorio", "ValidaciÃ³n", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(Paciente.Apellido))
            {
                MessageBox.Show("El apellido es obligatorio", "ValidaciÃ³n", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            DialogResult = true;
            Close();
        }

        private void Cancelar_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
