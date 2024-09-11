using LibraryApi_Repository.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryApi_Repository.Data
{
    public partial class LibraryApiContext : DbContext
    {

        public DbSet<Books> Books { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Reservations> Reservations { get; set; }
        public DbSet<Notifications> Notifications { get; set; }

        public LibraryApiContext(DbContextOptions<LibraryApiContext> options) : base(options)
        {
        }
        public IConfiguration Configuration { get; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(Configuration.GetConnectionString("DefaultConnection"));
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<User>()
            .HasIndex(u => u.Email)
            .IsUnique();
        }
    }
}
