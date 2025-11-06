using Microsoft.EntityFrameworkCore;
using MyAlumniApp.Models;

namespace MyAlumniApp.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options) { }

        public DbSet<Person> People { get; set; }
    }
}
