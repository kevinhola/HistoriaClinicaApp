using System;
using System.IO;
using System.Windows;
using Microsoft.Data.Sqlite;

namespace HistoriaClinicaApp
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            base.OnStartup(e);

          MainWindow window = new MainWindow();
         window.Show();

            
        }

    }
}
