// Using necessary namespaces for data access, authorization, identity management, and MVC functionality
using ABCRetailers_Cameron_Chetty_CLDV6212_POE_P3.Data; // Namespace for data context of the application
using ABCRetailers_Cameron_Chetty_CLDV6212_POE_P3.Models; // Namespace for models used in the application
using ABCRetailers_Cameron_Chetty_CLDV6212_POE_P3.Services;
using Microsoft.AspNetCore.Authorization; // For role-based authorization of user access
using Microsoft.AspNetCore.Identity; // For managing users and roles in Identity framework
using Microsoft.AspNetCore.Mvc; // For MVC controller functionality
using Microsoft.AspNetCore.Mvc.Rendering; // For working with SelectList and dropdown options in views
using Microsoft.EntityFrameworkCore; // For Entity Framework Core functionalities like asynchronous data retrieval

// Namespace for the OrdersController, which manages order-related actions
namespace ABCRetailers_Cameron_Chetty_CLDV6212_POE_P3.Controllers
{
    // OrdersController class inherits from Controller and handles order management functionality
    public class OrdersController : Controller
    {
        // Private readonly fields for database context and user manager to interact with data and manage users
        private readonly ApplicationDBContext _context; // Access to database context for CRUD operations
        private readonly UserManager<IdentityUser> _userManager; // Manages user information
        private readonly QueueService _queueService; //Queue Service to sednd message to queue

        // Constructor to inject dependencies of ApplicationDBContext and UserManager
        public OrdersController(ApplicationDBContext context, UserManager<IdentityUser> userManager, QueueService queueService)
        {
            _context = context; // Assigning the injected database context to the private field
            _userManager = userManager; // Assigning the injected user manager to the private field
            _queueService = queueService;

        }

        // Method to display all orders for the admin
        [Authorize(Roles = "Admin")] // Restrict access to Admin role
        public async Task<IActionResult> Admin()
        {
            // Retrieve a list of all orders with their associated users
            var orders = await _context.Orders
                .Include(o => o.User) // Include User data for each order
                .Where(o => o.Status != "Shopping" && o.TotalPrice.HasValue) // Exclude in-cart orders and null TotalPrice
                .ToListAsync();

            // Project orders into a list of OrderAdminViewModel to pass to the view
            var orderViewModels = orders.Select(o => new OrderAdminViewModel
            {
                OrderId = o.OrderId, // Order ID
                OrderDate = o.OrderDate, // Date of the order
                UserEmail = o.User.Email, // User's email address
                Status = o.Status, // Current status of the order
                TotalPrice = (decimal)o.TotalPrice // Total price of the order
            }).ToList();

            // Return the view with the list of orders for admin view
            return View(orderViewModels);
        }

        // Method to process a specific order by setting its status to "Approved"
        [Authorize(Roles = "Admin")] // Restrict access to Admin role
        public async Task<IActionResult> ProcessOrder(int id)
        {
            // Retrieve the order with the specified ID from the database
            var order = await _context.Orders.FindAsync(id);

            // If the order does not exist, return a 404 Not Found error
            if (order == null)
            {
                return NotFound();
            }

            // Set the order status to "Approved" to indicate it has been processed
            order.Status = "Approved";

            // Save changes to the database asynchronously
            await _context.SaveChangesAsync();

            // Send a message to the queue
            string message = $"Processing Order: Order ID: {order.OrderId} | Created Date: {order.OrderDate} | Total Price: R {order.TotalPrice} | Customer ID: {order.UserId} | Order Satus: {order.Status}";
            await _queueService.SendMessageAsync("processorders", message);



            // Redirect to the Admin view after processing the order
            return RedirectToAction(nameof(Admin));
        }

        // Method to display the order history for the currently logged-in client or admin
        [Authorize(Roles = "Client,Admin")] // Allows access to both Client and Admin roles
        public async Task<IActionResult> OrderHistory()
        {
            // Get the currently logged-in user
            var user = await _userManager.GetUserAsync(User);
            var userId = await _userManager.GetUserIdAsync(user); // Get the user ID of the logged-in user

            // Retrieve orders for the specific user and project them into OrderHistoryViewModel
            var orders = await _context.Orders
                .Where(o => o.UserId == userId && o.Status != "Shopping" && o.TotalPrice.HasValue) // Filter orders by the user ID
                .Select(o => new OrderHistoryViewModel
                {
                    OrderId = o.OrderId, // Order ID
                    OrderDate = o.OrderDate, // Date of the order
                    Status = o.Status, // Current status of the order
                    TotalPrice = (decimal)o.TotalPrice // Total price of the order
                })
                .ToListAsync();

            // Return the OrderHistory view with the list of orders
            return View(orders);
        }

        // Helper method to check if an order exists in the database based on the order ID
        private bool OrderExists(int id)
        {
            // Returns true if there is any order in the database with the specified ID
            return _context.Orders.Any(e => e.OrderId == id);
        }
    }
}
