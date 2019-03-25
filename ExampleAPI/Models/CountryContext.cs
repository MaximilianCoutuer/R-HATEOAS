using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ExampleAPI.Models
{
    public class CountryContext : DbContext
    {
        public DbSet<Country> Çountrys { get; set; }
    }
}
