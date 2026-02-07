using System;
using System.IO;
using System.Security.Cryptography;

namespace HistoriaClinicaApp.Services
{
    public class EncryptionService
    {
        private readonly byte[] _key;
        private readonly byte[] _iv;

        public EncryptionService()
        {
            string keyPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Config", "encryption.key");
            
            if (File.Exists(keyPath))
            {
                string keyData = File.ReadAllText(keyPath);
                string[] parts = keyData.Split('|');
                _key = Convert.FromBase64String(parts[0]);
                _iv = Convert.FromBase64String(parts[1]);
            }
            else
            {
                using (var aes = Aes.Create())
                {
                    aes.KeySize = 256;
                    aes.GenerateKey();
                    aes.GenerateIV();
                    _key = aes.Key;
                    _iv = aes.IV;
                }
            }
        }

        public void CifrarArchivo(string rutaOrigen, string rutaDestino)
        {
            using (var aes = Aes.Create())
            {
                aes.Key = _key;
                aes.IV = _iv;
                aes.Mode = CipherMode.CBC;
                aes.Padding = PaddingMode.PKCS7;

                using (var encryptor = aes.CreateEncryptor())
                using (var fsInput = new FileStream(rutaOrigen, FileMode.Open, FileAccess.Read))
                using (var fsOutput = new FileStream(rutaDestino, FileMode.Create, FileAccess.Write))
                using (var cryptoStream = new CryptoStream(fsOutput, encryptor, CryptoStreamMode.Write))
                {
                    fsInput.CopyTo(cryptoStream);
                }
            }
        }

        public void DescifrarArchivo(string rutaOrigen, string rutaDestino)
        {
            using (var aes = Aes.Create())
            {
                aes.Key = _key;
                aes.IV = _iv;
                aes.Mode = CipherMode.CBC;
                aes.Padding = PaddingMode.PKCS7;

                using (var decryptor = aes.CreateDecryptor())
                using (var fsInput = new FileStream(rutaOrigen, FileMode.Open, FileAccess.Read))
                using (var fsOutput = new FileStream(rutaDestino, FileMode.Create, FileAccess.Write))
                using (var cryptoStream = new CryptoStream(fsInput, decryptor, CryptoStreamMode.Read))
                {
                    cryptoStream.CopyTo(fsOutput);
                }
            }
        }

        public void GuardarClave(string rutaConfig)
        {
            Directory.CreateDirectory(Path.GetDirectoryName(rutaConfig));
            string keyData = Convert.ToBase64String(_key) + "|" + Convert.ToBase64String(_iv);
            File.WriteAllText(rutaConfig, keyData);
        }
    }
}