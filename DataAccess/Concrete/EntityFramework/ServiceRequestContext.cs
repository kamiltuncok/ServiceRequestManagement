using Microsoft.EntityFrameworkCore;
using Entities.Concrete;

namespace DataAccess.Concrete.EntityFramework
{
    public class ServiceRequestContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=ServiceRequestDb.db");
        }

        public DbSet<ServiceRequest> ServiceRequests { get; set; }
    }
}
