using System;
using System.Text;

namespace Meetups.Domain.Interfaces.Services
{
    public interface ISecurityService
    {
        string Encrypt(string texto);
        string Decrypt(string texto);
    }
}
