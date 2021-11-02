using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Providers
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext (DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Meetup>()
                .HasMany(c => c.Attendees)
                .WithOne(e => e.Meetup);

            modelBuilder.Entity<MeetupUser>()
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

        public DbSet<MeetupUser> MeetupUser { get; set; }
    }
}
