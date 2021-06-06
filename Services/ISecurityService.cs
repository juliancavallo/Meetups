using System;
using System.Text;

namespace Services
{
    public interface ISecurityService
    {
        string Crypt(string texto);
        string Decrypt(string texto);
    }
}
