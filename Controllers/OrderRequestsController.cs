// Importing necessary namespaces for data access, authorization, identity, and MVC functionality
using ABCRetailers_Cameron_Chetty_CLDV6212_POE_P3.Data; // Provides access to application's database context
using ABCRetailers_Cameron_Chetty_CLDV6212_POE_P3.Models; // Includes application models (like OrderRequests, Order, Product) for data structure definitions
using Microsoft.AspNetCore.Authorization; // Enables role-based authorization for actions
using Microsoft.AspNetCore.Identity; // Provides Identity framework for user authentication and role management
using Microsoft.AspNetCore.Mvc; // Core namespace for MVC pattern implementation in ASP.NET
using Microsoft.AspNetCore.Mvc.Rendering; // Provides support for rendering dropdown lists and other UI components in views
using Microsoft.EntityFrameworkCore; // Enables Entity Framework Core functionalities like asynchronous data retrieval

// Defining the namespace of the controller, matching the project structure
namespace ABCRetailers_Cameron_Chetty_CLDV6212_POE_P3.Controllers
{
    // OrderRequestsController handles actions related to order requests within the application
    public class OrderRequestsController : Controller
    {
        // Private readonly fields for dependency injection of the database context and user manager
        private readonly ApplicationDBContext _context; // Instance of database context for accessing and modifying data
        private readonly UserManager<IdentityUser> _userManager; // Manages user information and roles in Identity framework

        // Constructor for injecting dependencies into the controller
        public OrderRequestsController(ApplicationDBContext context, UserManager<IdentityUser> userManager)
        {
            _context = context; // Assign the injected ApplicationDBContext to the private _context field
            _userManager = userManager; // Assign the injected UserManager to the private _userManager field
        }

        // Action method to display a list of order requests, restricted to Admin role
        [Authorize(Roles = "Admin")] // Only users in the "Admin" role can access this action
        // GET: OrderRequests
        public async Task<IActionResult> Index(string searchString)
        {
            // Store the current search string in ViewData to be accessible in the view for displaying or reusing
            ViewData["CurrentFilter"] = searchString;

            // Start a query to retrieve order requests, including related Order and Product data for each request
            var orderRequests = from o in _context.OrderRequests.Include(o => o.Order).Include(o => o.Product)
                                select o;

            // Check if searchString has a value, then filter the order requests based on OrderId
            if (!String.IsNullOrEmpty(searchString))
            {
                // Narrow down the list of order requests where the OrderId matches the search string
                orderRequests = orderRequests.Where(o => o.OrderId.ToString() == searchString);
            }

            // Execute the query asynchronously, converting the results to a list, and return the list to the Index view
            return View(await orderRequests.ToListAsync());
        }

        // Helper method to check if a specific order request exists based on the OrderRequestId
        private bool OrderRequestExists(int id)
        {
            // Returns true if there is an order request in the database with the specified OrderRequestId
            return _context.OrderRequests.Any(e => e.OrderRequestId == id);
        }
    }
}
