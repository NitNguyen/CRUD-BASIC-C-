using FirstAPI.Models;
using Microsoft.EntityFrameworkCore;


namespace FirstAPI.Data

{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<TaskItem> tasks { get; set; }

        public DbSet<User> users { get; set; }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    modelBuilder.Entity<TaskItem>()
        //        .Property(task => task.id)
        //        .ValueGeneratedOnAdd(); // This configures the Id to be auto-generated on add

        //    // Rest of your modelBuilder configurations

        //    base.OnModelCreating(modelBuilder);
        //}
    }
}
