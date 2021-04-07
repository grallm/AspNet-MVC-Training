using Microsoft.EntityFrameworkCore;
using AspNet_MVC_Training.Models;

namespace AspNet_MVC_Training.Data
{
    public class AspNet_MVC_TrainingContext : DbContext
    {
        public AspNet_MVC_TrainingContext (DbContextOptions<AspNet_MVC_TrainingContext> options)
            : base(options)
        {
        }

        public DbSet<Training> Training { get; set; }
    }
}