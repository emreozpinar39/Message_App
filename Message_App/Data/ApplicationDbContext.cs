using Message_App.Models;
using Microsoft.EntityFrameworkCore;
using System;

namespace Message_App.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext() : base()
        {

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=DESKTOP-06N3HCH\\SQLEXPRESS;Database=UnitOfWorkDb;Trusted_Connection=True;");
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Message>().HasOne(x => x.User).WithMany(y => y.Messages).HasForeignKey(z => z.UserId);

            modelBuilder.Entity<Users>().HasData(new Users
            {
                Id = Guid.NewGuid(),
                FirstName = "admin",
                LastName = "admin",
                Email = "admin@admin.com",
                Password = "admin"
            });

            base.OnModelCreating(modelBuilder);
        }

        public virtual DbSet<Users> Users { get; set; }
        public virtual DbSet<Message> Messages { get; set; }
    }
}
