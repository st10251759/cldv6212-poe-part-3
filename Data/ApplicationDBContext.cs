// Imports the Models namespace from the project, where data models like Product, ApplicationUser, etc., are defined.
using ABCRetailers_Cameron_Chetty_CLDV6212_POE_P3.Models;

// Imports IdentityEntityFrameworkCore from ASP.NET Core Identity, which is required for user authentication and authorization.
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

// Imports Entity Framework Core, a popular ORM (Object-Relational Mapper) for .NET applications, which enables database interaction.
using Microsoft.EntityFrameworkCore;

// Imports generic collection types to allow the use of collections such as lists, which may be used in this class or associated models.
using System.Collections.Generic;

namespace ABCRetailers_Cameron_Chetty_CLDV6212_POE_P3.Data
{
    // Defines the ApplicationDBContext class, which serves as the database context for the application.
    // Inherits from IdentityDbContext, which includes identity-specific properties like AspNetUsers and AspNetRoles for managing authentication.
    public class ApplicationDBContext : IdentityDbContext
    {
        // Constructor for ApplicationDBContext. Accepts DbContextOptions of type ApplicationDBContext as a parameter.
        // This allows configuration of the context options (e.g., connection string) when setting up the database context in the application startup.
        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options) : base(options)
        {
        }

        // DbSet property for the Product entity. Represents the collection of Product records in the database.
        // Each Product entry in this collection corresponds to a row in the Product table in the database.
        public DbSet<Product> Product { get; set; }

        // DbSet property for ApplicationUser, a custom user entity that likely extends IdentityUser.
        // This enables management of users in the application, using custom properties in addition to standard Identity properties.
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }

        // Virtual DbSet property for Orders. The 'virtual' keyword allows lazy loading if configured.
        // Represents the collection of Orders in the database, allowing CRUD operations on order data.
        public virtual DbSet<Order> Orders { get; set; }

        // Virtual DbSet property for OrderRequests, which represents a collection of OrderRequest entries in the database.
        // This allows querying and manipulation of order request data for the application.
        public virtual DbSet<OrderRequest> OrderRequests { get; set; }

        // DbSet property for Documents. Represents a collection of Document records in the database.
        // This can be used to store and manage file metadata, such as paths, names, and types of documents associated with the application.
        public DbSet<Document> Documents { get; set; }
    }
}
