using System;
using System.Text;

namespace Services
{
    public class SecurityService
    {
        public string Encriptar(string texto)
        {
            byte[] bytes = Encoding.Unicode.GetBytes(texto);
            return Convert.ToBase64String(bytes);
        }

        public string Desencriptar(string texto)
        {
            byte[] bytes = Convert.FromBase64String(texto);
            return Encoding.Unicode.GetString(bytes);
        }
    }
}
