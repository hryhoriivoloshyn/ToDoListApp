
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using ToDoListApp.Models;

namespace ToDoListApp.Data
{
    public class ApplicationDbContext : DbContext
    {
        

        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
            Database.EnsureCreated();
        }
        public virtual DbSet<ToDoTask> Tasks { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ToDoTask>(entity =>
            {
                entity.HasKey(e => e.Id);
            });
        }
    }

}
