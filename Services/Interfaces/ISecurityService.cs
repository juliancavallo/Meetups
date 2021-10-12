using System;
using System.Text;

namespace Services
{
    public interface ISecurityService
    {
        string Encrypt(string texto);
        string Decrypt(string texto);
    }
}
