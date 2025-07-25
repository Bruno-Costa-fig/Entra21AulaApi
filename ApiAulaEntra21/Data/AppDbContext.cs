using ApiAulaEntra21.Models;
using Microsoft.EntityFrameworkCore;

namespace ApiAulaEntra21.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Produto> Produto { get; set; }
        public DbSet<Cliente> Cliente { get; set; }
        public DbSet<Loja> Loja { get; set; }
        public DbSet<Usuario> Usuario { get; set; }
    }
}
