using ABCRetailers_Cameron_Chetty_CLDV6212_POE_P3.Data; // Importing the application's data context
using ABCRetailers_Cameron_Chetty_CLDV6212_POE_P3.Models; // Importing the model classes
using ABCRetailers_Cameron_Chetty_CLDV6212_POE_P3.Services; // Importing custom services, including BlobService
using Microsoft.AspNetCore.Mvc; // Importing ASP.NET Core MVC features
using Microsoft.AspNetCore.Mvc.Rendering; // Importing classes for working with dropdown lists in views
using Microsoft.EntityFrameworkCore; // Importing EF Core for database access and LINQ operations

namespace ABCRetailers_Cameron_Chetty_CLDV6212_POE_P3.Controllers
{
    public class ProductsController : Controller
    {
        private readonly ApplicationDBContext _context; // Database context instance for accessing database tables
        private readonly BlobService _blobService; // Service for handling file uploads and deletions in Blob Storage
        private readonly QueueService _queueService;

        // Constructor for dependency injection of database context and blob storage service
        public ProductsController(ApplicationDBContext context, BlobService blobService, QueueService queueService)
        {
            _context = context;
            _blobService = blobService;
            _queueService = queueService;
        }

        // GET: Products
        // Method to display the list of products, optionally filtered by category
        public IActionResult Index(string category)
        {
            // Query to fetch distinct product categories from the database for dropdown filtering
            IQueryable<string> categoryQuery = from p in _context.Product
                                               orderby p.Category
                                               select p.Category;

            var distinctCategories = categoryQuery.Distinct().ToList(); // Fetch unique categories
            ViewBag.Category = new SelectList(distinctCategories); // Passing categories to the view for the dropdown list

            // Retrieve all products or filter them by the selected category
            IQueryable<Product> products = _context.Product;
            if (!string.IsNullOrEmpty(category))
            {
                products = products.Where(p => p.Category == category); // Filter by selected category if not null or empty
            }

            return View(products.ToList()); // Return the list of products to the view
        }

        // GET: Products/Details/5
        // Method to show details of a specific product based on its ID
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound(); // Return 404 if the product ID is null
            }

            var product = await _context.Product
                .FirstOrDefaultAsync(m => m.ProductId == id); // Fetch the product from the database
            if (product == null)
            {
                return NotFound(); // Return 404 if the product does not exist
            }

            return View(product); // Pass the product details to the view
        }

        // GET: Products/Create
        // Method to display the product creation form
        public IActionResult Create()
        {
            return View(); // Render the Create view
        }

        // POST: Products/Create
        // Handles the product creation, including optional image upload
        [HttpPost]
        [ValidateAntiForgeryToken] // Ensures that the form submission is secure
        public async Task<IActionResult> Create([Bind("ProductId,Name,ProductDescription,Price,Category,Availability,ImageUrlpath")] Product product, IFormFile file)
        {
            // Check if a file is provided for upload
            if (file != null && file.Length > 0)
            {
                // Only allow specific image formats
                var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
                var extension = Path.GetExtension(file.FileName).ToLower(); // Get the file extension

                if (allowedExtensions.Contains(extension)) // Validate the file format
                {
                    using var stream = file.OpenReadStream(); // Open the file stream
                    var imageUrl = await _blobService.UploadAsync(stream, file.FileName); // Upload file to Blob storage
                    product.ImageUrlpath = imageUrl; // Set the product's image URL
                }
                else
                {
                    // Display an error if the file format is invalid
                    ModelState.AddModelError("file", "Only image files (.jpg, .jpeg, .png, .gif) are allowed.");
                    return View(product); // Return to view with the error message
                }
            }

            // Check if the model state is valid before adding the product to the database
            if (ModelState.IsValid)
            {
                _context.Add(product); // Add the product to the context
                await _context.SaveChangesAsync(); // Save changes to the database

                // Send messages to queues
                string imageUploadMessage = $"Product ID:{product.ProductId}, Image url: {product.ImageUrlpath}, Status: Uploaded Successfully";
                await _queueService.SendMessageAsync("imageupload", imageUploadMessage);


                return RedirectToAction(nameof(Index)); // Redirect to the Index view
            }
            return View(product); // Return to the Create view if the model state is invalid
        }

        // GET: Products/Edit/5
        // Method to show the product editing form
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound(); // Return 404 if the product ID is null
            }

            var product = await _context.Product.FindAsync(id); // Retrieve the product from the database
            if (product == null)
            {
                return NotFound(); // Return 404 if the product does not exist
            }
            return View(product); // Pass the product details to the edit view
        }

        // POST: Products/Edit/5
        // Handles product editing
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ProductId,Name,ProductDescription,Price,Category,Availability")] Product product)
        {
            if (id != product.ProductId)
            {
                return NotFound(); // Return 404 if the product ID does not match
            }

            // Retrieve the existing product to retain its ImageUrlpath
            var existingProduct = await _context.Product.AsNoTracking().FirstOrDefaultAsync(p => p.ProductId == id);
            if (existingProduct == null)
            {
                return NotFound(); // Return 404 if the product does not exist
            }

            product.ImageUrlpath = existingProduct.ImageUrlpath; // Preserve the existing image URL

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(product); // Update the product in the context
                    await _context.SaveChangesAsync(); // Save changes to the database
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.ProductId)) // Check if the product still exists
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw; // Rethrow the exception if it's a concurrency issue
                    }
                }
                return RedirectToAction(nameof(Index)); // Redirect to the Index view
            }
            return View(product); // Return to the Edit view if the model state is invalid
        }

        // GET: Products/Delete/5
        // Method to confirm product deletion
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound(); // Return 404 if the product ID is null
            }

            var product = await _context.Product
                .FirstOrDefaultAsync(m => m.ProductId == id); // Retrieve the product from the database
            if (product == null)
            {
                return NotFound(); // Return 404 if the product does not exist
            }

            return View(product); // Pass the product details to the delete view
        }

        // POST: Products/Delete/5
        // Handles the product deletion confirmation
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var product = await _context.Product.FindAsync(id); // Retrieve the product from the database
            if (product != null)
            {
                // Delete the associated image in Blob Storage if it exists
                if (!string.IsNullOrEmpty(product.ImageUrlpath))
                {
                    await _blobService.DeleteBlobAsync(product.ImageUrlpath); // Delete the image in Blob Storage
                }

                _context.Product.Remove(product); // Remove the product from the context
                await _context.SaveChangesAsync(); // Save changes to the database
            }

            return RedirectToAction(nameof(Index)); // Redirect to the Index view
        }

        // Helper method to check if a product exists by ID
        private bool ProductExists(int id)
        {
            return _context.Product.Any(e => e.ProductId == id); // Return true if the product exists
        }
    }
}
