using Microsoft.EntityFrameworkCore;
using SignalRApiForSql.Models;

namespace SignalRApiForSql.DAL
{
    public class Context:DbContext
    {
        protected readonly IConfiguration Configuration;

        public Context(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=signalR.db");
        }

        public DbSet<Visitor> Visitors { get; set; }

    }
}
