using ABCRetailers_Cameron_Chetty_CLDV6212_POE_P3.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace ABCRetailers_Cameron_Chetty_CLDV6212_POE_P3.Data
{
    public class ApplicationDBContext : DbContext
    {
        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options) : base(options)
        {
        }

        public DbSet<Product> Product { get; set; }
    }
}
