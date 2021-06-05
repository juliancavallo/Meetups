using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Santander_Tecnologia.Models;

namespace Santander_Tecnologia.Data
{
    public class Santander_TecnologiaContext : DbContext
    {
        public Santander_TecnologiaContext (DbContextOptions<Santander_TecnologiaContext> options)
            : base(options)
        {
        }

        public DbSet<Santander_Tecnologia.Models.Meetup> Meetup { get; set; }

        public DbSet<Santander_Tecnologia.Models.User> User { get; set; }
    }
}
