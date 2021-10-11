using Domain.Models;
using System;
using System.Linq;
using System.Text;

namespace Providers
{
    public static class DbInitializer
    {
        private static string Encrypt(string texto)
        {
            byte[] bytes = Encoding.Unicode.GetBytes(texto);
            return Convert.ToBase64String(bytes);
        }

        public static void Initialize(Santander_TecnologiaContext context)
        {
            if (context.User.Any())
                return;

            var users = new User[]
            {
                new User
                {
                    Name = "Usuario",
                    LastName ="Administrador",
                    UserName = "admin",
                    Password = Encrypt("123456")
                },
                new User
                {
                    Name = "Julián",
                    LastName ="Cavallo",
                    UserName = "jcavallo",
                    Password = Encrypt("123456")
                },
                new User
                {
                    Name = "Juan",
                    LastName ="Perez",
                    UserName = "jperez",
                    Password = Encrypt("123456")
                },
                new User
                {
                    Name = "Jose",
                    LastName ="Rodriguez",
                    UserName = "jrodriguez",
                    Password = Encrypt("123456")
                },
                new User
                {
                    Name = "Ana",
                    LastName ="Gimenez",
                    UserName = "agimenez",
                    Password = Encrypt("123456")
                }
            };

            context.User.AddRange(users);
            context.SaveChanges();
        }
    }
}