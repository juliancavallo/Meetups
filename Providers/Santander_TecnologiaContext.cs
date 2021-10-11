using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Providers
{
    public class Santander_TecnologiaContext : DbContext
    {
        public Santander_TecnologiaContext (DbContextOptions<Santander_TecnologiaContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Meetup>()
                .HasMany(c => c.Attendees)
                .WithOne(e => e.Meetup);

            modelBuilder.Entity<MeetupUsers>()
                .HasOne(x => x.User);


            modelBuilder
                .Entity<Meetup>()
                .HasOne(x => x.Organizer)
                .WithMany(x => x.OrganizedMeetups)
                .HasForeignKey(x => x.OrganizerId)
                .OnDelete(DeleteBehavior.Restrict);

        }

        public DbSet<Meetup> Meetup { get; set; }

        public DbSet<User> User { get; set; }

        public DbSet<MeetupUsers> MeetupUsers { get; set; }
    }
}
