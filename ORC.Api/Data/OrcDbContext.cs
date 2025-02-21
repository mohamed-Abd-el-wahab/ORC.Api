using Microsoft.EntityFrameworkCore;
using ORC.Api.Models;

namespace ORC.Api.Data
{
    public class OrcDbContext : DbContext
    {
        public OrcDbContext(DbContextOptions<OrcDbContext> options) : base(options)
        {
        }

        public DbSet<Registration> Registrations { get; set; }
    }
} 