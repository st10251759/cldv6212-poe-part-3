// Import the Identity namespace to manage authentication and authorization roles
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

// Import the MVC namespace to enable the use of controllers, actions, and views
using Microsoft.AspNetCore.Mvc;

namespace ABCRetailers_Cameron_Chetty_CLDV6212_POE_P3.Controllers
{
    // Define the AppRolesController class to manage user roles
    // This controller inherits from the Controller class, providing MVC functionality
    [Authorize(Roles = "Admin")] // Restrict access to Admin role
    public class AppRolesController : Controller
    {
        // Declare a private readonly field for RoleManager, which will handle role-related operations
        private readonly RoleManager<IdentityRole> _roleManager;

        // Constructor for AppRolesController, injecting RoleManager dependency
        // RoleManager<IdentityRole> is used to create and manage roles in the application
        public AppRolesController(RoleManager<IdentityRole> roleManager)
        {
            // Assign the injected RoleManager instance to the private _roleManager field
            _roleManager = roleManager;
        }

        // Action method to list all roles in the application
        // This method responds to GET requests at /AppRoles and passes the roles to the view
        public IActionResult Index()
        {
            // Get all roles using RoleManager's Roles property
            var roles = _roleManager.Roles;

            // Pass the roles to the view to display them
            return View(roles);
        }

        // Action method to display the role creation form
        // This method responds to GET requests for creating a new role
        [HttpGet]
        public IActionResult Create()
        {
            // Return the view that contains the role creation form
            return View();
        }

        // Action method to handle role creation form submissions
        // This method responds to POST requests, accepting an IdentityRole model as input
        [HttpPost]
        public async Task<IActionResult> Create(IdentityRole model)
        {
            // Check if the role name already exists to prevent duplicates
            // RoleExistsAsync returns a Task<bool>, indicating if the role already exists
            if (!_roleManager.RoleExistsAsync(model.Name).GetAwaiter().GetResult())
            {
                // Create a new role using the role name from the model if it does not already exist
                _roleManager.CreateAsync(new IdentityRole(model.Name)).GetAwaiter().GetResult();
            }

            // Redirect to the Index action to display the list of roles after creation
            return RedirectToAction("Index");
        }
    }
}
