using ABCRetailers_Cameron_Chetty_CLDV6212_POE_P3.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace ABCRetailers_Cameron_Chetty_CLDV6212_POE_P3.Data
{
    public class ApplicationDBContext : IdentityDbContext
    {
        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options) : base(options)
        {
        }

        public DbSet<Product> Product { get; set; }

        public DbSet<ApplicationUser> ApplicationUsers { get; set; }

        public virtual DbSet<Order> Orders { get; set; }

        public virtual DbSet<OrderRequest> OrderRequests { get; set; }

        public DbSet<Document> Documents { get; set; }

    }
}
