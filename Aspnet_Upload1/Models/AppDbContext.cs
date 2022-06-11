using Microsoft.EntityFrameworkCore;

namespace Aspnet_Upload1.Models
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }
        public DbSet<Cliente> Clientes { get; set; }
    }
}
