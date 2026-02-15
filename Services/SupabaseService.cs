using System;
using System.IO;
using Supabase;
using Newtonsoft.Json;

namespace HistoriaClinicaApp.Services
{
    public class SupabaseService
    {
        private static Client _client;
        private static readonly object _lock = new object();

        public static Client GetClient()
        {
            if (_client == null)
            {
                lock (_lock)
                {
                    if (_client == null)
                    {
                        var config = LoadConfiguration();
                        var options = new SupabaseOptions
                        {
                            AutoRefreshToken = true,
                            AutoConnectRealtime = true
                        };
                        _client = new Client(config.Url, config.Key, options);
                    }
                }
            }
            return _client;
        }

        private static SupabaseConfig LoadConfiguration()
        {
            try
            {
                string jsonPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "appsettings.json");
                if (!File.Exists(jsonPath))
                    throw new FileNotFoundException($"appsettings.json no encontrado en: {jsonPath}");
                
                string json = File.ReadAllText(jsonPath);
                var config = JsonConvert.DeserializeObject<AppSettings>(json);
                
                if (config?.Supabase == null)
                    throw new Exception("Configuración de Supabase no encontrada");
                
                return config.Supabase;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al cargar configuración: {ex.Message}");
            }
        }

        private class AppSettings
        {
            public SupabaseConfig Supabase { get; set; }
        }

        private class SupabaseConfig
        {
            public string Url { get; set; }
            public string Key { get; set; }
        }
    }
}

