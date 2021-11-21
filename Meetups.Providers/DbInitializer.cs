using Meetups.Domain.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Text;

namespace Meetups.Providers
{
    public class DbInitializer : IDbInitializer
    {

        private readonly IServiceScopeFactory _scopeFactory;

        public DbInitializer(IServiceScopeFactory scopeFactory)
        {
            this._scopeFactory = scopeFactory;
        }

        public void Initialize()
        {
            using (var serviceScope = _scopeFactory.CreateScope())
            {
                using (var context = serviceScope.ServiceProvider.GetService<ApplicationDbContext>())
                {
                    context.Database.Migrate();
                }
            }
        }

        public void SeedData()
        {
            using (var serviceScope = _scopeFactory.CreateScope())
            {
                using (var context = serviceScope.ServiceProvider.GetService<ApplicationDbContext>())
                {

                    if (context.User.Any())
                        return;

                    var users = new User[]
                    {
                        new User
                        {
                            FirstName = "Usuario",
                            LastName ="Administrador",
                            UserName = "admin",
                            Password = Encrypt("123456")
                        },
                        new User
                        {
                            FirstName = "Julián",
                            LastName ="Cavallo",
                            UserName = "jcavallo",
                            Password = Encrypt("123456")
                        },
                        new User
                        {
                            FirstName = "Juan",
                            LastName ="Perez",
                            UserName = "jperez",
                            Password = Encrypt("123456")
                        },
                        new User
                        {
                            FirstName = "Jose",
                            LastName ="Rodriguez",
                            UserName = "jrodriguez",
                            Password = Encrypt("123456")
                        },
                        new User
                        {
                            FirstName = "Ana",
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

        private static string Encrypt(string texto)
        {
            byte[] bytes = Encoding.Unicode.GetBytes(texto);
            return Convert.ToBase64String(bytes);
        }
    }
}