// Imports the Identity namespace from ASP.NET Core, which contains classes and interfaces for managing user identities,
// such as user creation, authentication, and authorization functionalities.
using Microsoft.AspNetCore.Identity;

// Defines the namespace for the application models. This helps organize the codebase by grouping related classes together.
// In this case, the models for the ABC Retailers application are defined under this namespace.
namespace ABCRetailers_Cameron_Chetty_CLDV6212_POE_P3.Models
{
    // Defines the ApplicationUser class, which inherits from IdentityUser.
    // IdentityUser is a class provided by ASP.NET Core Identity that represents a user in the system.
    // By inheriting from IdentityUser, ApplicationUser gains all the properties and methods defined in IdentityUser, 
    // allowing it to function as a user in the ASP.NET Identity framework.
    public class ApplicationUser : IdentityUser
    {
        // Property to store the user's first name.
        // This property can be used to capture more personal information about the user 
        // beyond the standard properties provided by IdentityUser (like Username and Email).
        public string Firstname { get; set; }

        // Property to store the user's last name.
        // Similar to Firstname, this property allows for additional user information storage,
        // enabling better user identification and personalization within the application.
        public string Lastname { get; set; }
    }
}
