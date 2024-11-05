using ABCRetailers_Cameron_Chetty_CLDV6212_POE_P3.Data;  // Imports the data namespace for accessing the ApplicationDBContext (EF Core DbContext for database interactions).
using ABCRetailers_Cameron_Chetty_CLDV6212_POE_P3.Models;  // Imports the models defined in the application, including Order, OrderRequest, Product, etc.
using ABCRetailers_Cameron_Chetty_CLDV6212_POE_P3.Services;
using Microsoft.AspNetCore.Authorization;  // Imports authorization classes to control access based on user roles.
using Microsoft.AspNetCore.Identity;  // Imports Identity classes to manage user authentication and authorization.
using Microsoft.AspNetCore.Mvc;  // Imports MVC functionalities, such as controllers, views, and model binding.
using Microsoft.EntityFrameworkCore;  // Imports EF Core functionality to interact with the database asynchronously.

namespace ABCRetailers_Cameron_Chetty_CLDV6212_POE_P3.Controllers
{
    // Restricts access to users with either "Client" or "Admin" roles.
    [Authorize(Roles = "Client,Admin")]
    public class MyWorkController : Controller
    {
        // Private fields to hold references to ApplicationDBContext (for database access) and UserManager for user management.
        private readonly ApplicationDBContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly QueueService _queueService;

        // Constructor that receives dependencies through dependency injection.
        public MyWorkController(ApplicationDBContext context, UserManager<IdentityUser> userManager, QueueService queueService)
        {
            _context = context;  // Initializes the _context field with the injected database context.
            _userManager = userManager;  // Initializes the _userManager field for user-related operations.
            _queueService = queueService;
        }

        // Index action method:
        // This action fetches a list of products and returns it to the "Index" view.
        public async Task<IActionResult> Index()
        {
            return View(await _context.Product.ToListAsync());  // Asynchronously retrieves a list of products and passes it to the view.
        }

        // CreateOrder action method:
        // Adds a new order for a specified product if it is available and the user does not already have an open order.
        [HttpPost]
        public async Task<IActionResult> CreateOrder(int productId)
        {
            // Retrieves the current user and their userId from the user manager.
            var user = await _userManager.GetUserAsync(User);
            var userId = await _userManager.GetUserIdAsync(user);

            // Finds the product by productId and ensures it is available.
            var product = _context.Product.FirstOrDefault(p => p.ProductId == productId && p.Availability == true);

            // Checks for an existing open order with "Shopping" status for this user.
            var openOrder = await _context.Orders
                .FirstOrDefaultAsync(o => o.UserId == userId && o.Status == "Shopping");

            // If there is no open order, creates a new order with "Shopping" status.
            if (openOrder == null)
            {
                openOrder = new Order
                {
                    UserId = userId,
                    OrderDate = DateTime.Now,
                    Status = "Shopping"
                };
                _context.Orders.Add(openOrder);  // Adds the new order to the database context.
                await _context.SaveChangesAsync();  // Saves changes to ensure the new order is created.
            }

            // Creates a new OrderRequest for the selected product, setting its status as "Pending".
            var orderRequest = new OrderRequest
            {
                OrderId = openOrder.OrderId,
                ProductId = productId,
                OrderStatus = "Pending"
            };
            _context.OrderRequests.Add(orderRequest);  // Adds the order request to the database.

            // Marks the product as unavailable to reflect its addition to the order.
            product.Availability = false;

            await _context.SaveChangesAsync();  // Saves changes to persist the order request and product availability updates.

            return Json(new { success = true });  // Returns a JSON response indicating successful order creation.
        }

        // Cart action method:
        // Displays the user's open order (if any) along with all related order requests (products in the cart).
        public async Task<IActionResult> Cart()
        {
            var user = await _userManager.GetUserAsync(User);  // Retrieves the current user.
            var userId = await _userManager.GetUserIdAsync(user);  // Gets the user's ID.

            // Fetches the user's open order (with "Shopping" status) and includes related OrderRequests and Product details.
            var openOrder = await _context.Orders
                .Include(o => o.OrderRequests)
                .ThenInclude(or => or.Product)
                .FirstOrDefaultAsync(o => o.UserId == userId && o.Status == "Shopping");

            return View(openOrder);  // Returns the open order to the "Cart" view for display.
        }

        // RemoveFromCart action method:
        // Removes a specific product from the user's cart.
        [HttpPost]
        public async Task<IActionResult> RemoveFromCart(int productId)
        {
            var user = await _userManager.GetUserAsync(User);  // Retrieves the current user.
            var userId = await _userManager.GetUserIdAsync(user);  // Gets the user's ID.

            // Finds the user's open order with "Shopping" status.
            var openOrder = await _context.Orders
                .Include(o => o.OrderRequests)
                .FirstOrDefaultAsync(o => o.UserId == userId && o.Status == "Shopping");

            if (openOrder != null)
            {
                // Finds the specific order request for the product being removed.
                var orderRequest = openOrder.OrderRequests
                    .FirstOrDefault(or => or.ProductId == productId);

                if (orderRequest != null)
                {
                    _context.OrderRequests.Remove(orderRequest);  // Removes the order request from the context.

                    // Restores the product’s availability to "In Stock".
                    var product = await _context.Product.FindAsync(productId);
                    if (product != null)
                    {
                        product.Availability = true;
                    }

                    await _context.SaveChangesAsync();  // Saves changes to update the database.

                    return Json(new { success = true });  // Returns success response.
                }
            }

            return Json(new { success = false, message = "Item not found in cart" });  // Returns error if item is not found.
        }

        // Checkout action method:
        // Processes the checkout by updating the order status to "Pending" and setting the total price.
        [HttpPost]
        public async Task<IActionResult> Checkout(decimal totalPrice)
        {
            var user = await _userManager.GetUserAsync(User);  // Retrieves the current user.
            var userId = await _userManager.GetUserIdAsync(user);  // Gets the user's ID.

            // Finds the open order and includes related OrderRequests.
            var openOrder = await _context.Orders
                .Include(o => o.OrderRequests)
                .FirstOrDefaultAsync(o => o.UserId == userId && o.Status == "Shopping");

            if (openOrder == null || !openOrder.OrderRequests.Any())
            {
                return Json(new { success = false, message = "No items in cart" });  // Returns an error if the cart is empty.
            }

            // Sets the order’s total price and updates the status to "Pending" to reflect checkout.
            openOrder.TotalPrice = totalPrice;
            openOrder.Status = "Pending";
            await _context.SaveChangesAsync();  // Saves changes to complete the checkout.

            // Send a message to the queue
            string message = $" Order: Order ID: {openOrder.OrderId} added succesfully on Order Date: {openOrder.OrderDate} by User: {openOrder.User} values at Total Price: R {openOrder.TotalPrice} with the Status: {openOrder.Status}";
            await _queueService.SendMessageAsync("createdorders", message);

            return Json(new { success = true });  // Returns success response.
        }

        // CheckProductAvailability action method:
        // Checks if a product is available and returns the result as JSON.
        [HttpPost]
        public IActionResult CheckProductAvailability(int productId)
        {
            // Checks if the specified product exists and is available.
            var product = _context.Product.FirstOrDefault(p => p.ProductId == productId && p.Availability == true);

            if (product != null)
            {
                // Product is available.
                return Json(new { success = true });
            }
            else
            {
                // Product is not available.
                return Json(new { success = false, message = "Product is not available" });
            }
        }
    }
}
