using Santander_Tecnologia.Data;
using Santander_Tecnologia.Models;
using Services;
using System;
using System.Linq;

namespace Santander_Tecnologia.Data
{
    public static class DbInitializer
    {
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
                    Password = new SecurityService().Crypt("1234")
                },
            };

            context.User.AddRange(users);
            context.SaveChanges();
        }
    }
}