namespace ExampleAPI.Models
{
    using Microsoft.EntityFrameworkCore;

    public class ExampleDbContext : DbContext
    {
        public ExampleDbContext(DbContextOptions<ExampleDbContext> options)
            : base(options)
        {
        }

        public DbSet<Person> Persons { get; set; }

        public DbSet<Person> Countrys { get; set; }
    }
}
