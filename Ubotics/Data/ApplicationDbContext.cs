using Microsoft.EntityFrameworkCore;
using Ubotics.Models;

namespace Ubotics.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
          : base(options)
        {
        }

        public DbSet<Review> Reviews {  get; set; } 
    }
}
