// Import necessary namespaces for data access, model usage, authorization, and file management
using ABCRetailers_Cameron_Chetty_CLDV6212_POE_P3.Data;  // For database context
using ABCRetailers_Cameron_Chetty_CLDV6212_POE_P3.Models; // For accessing Document model
using Microsoft.AspNetCore.Authorization; // For role-based and policy-based authorization
using Microsoft.AspNetCore.Mvc; // For MVC functionality
using Microsoft.EntityFrameworkCore; // For advanced database operations with Entity Framework Core
using System.IO; // For file handling
using System.Linq; // For LINQ query support
using System.Threading.Tasks; // For asynchronous operations

namespace ABCRetailers_Cameron_Chetty_CLDV6212_POE_P3.Controllers
{
    // Restrict access to authorized users only for this controller
    [Authorize]
    public class DocumentController : Controller
    {
        // Declare private fields for database context and environment to manage web root paths
        private readonly ApplicationDBContext _context;
        private readonly IWebHostEnvironment _environment;

        // Constructor to initialize DocumentController with ApplicationDBContext and IWebHostEnvironment
        public DocumentController(ApplicationDBContext context, IWebHostEnvironment environment)
        {
            _context = context; // Initialize the database context
            _environment = environment; // Initialize environment settings to manage files
        }

        // GET action to display the Upload form - restricted to Admin role
        [Authorize(Roles = "Admin")]
        public IActionResult Upload()
        {
            return View(); // Returns the view where admins can upload files
        }

        // POST action to handle file uploads, restricted to Admin role
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Upload(Document model, IFormFile file)
        {
            // Ensure a file was selected before continuing
            if (file == null || file.Length == 0)
            {
                ModelState.AddModelError("file", "Please upload a PDF file.");
                return View(model); // Return view with an error if no file was uploaded
            }

            var validMimeType = "application/pdf"; // MIME type for PDF validation
            var maxFileSize = 15 * 1024 * 1024; // Maximum allowed file size set to 15MB

            // Check if the uploaded file is a PDF by validating its MIME type and file extension
            if (file.ContentType != validMimeType || !file.FileName.EndsWith(".pdf"))
            {
                ModelState.AddModelError("", "Only PDF files are allowed.");
                return View(model); // Return view with an error if file is not a PDF
            }

            // Check if the file exceeds the maximum file size
            if (file.Length > maxFileSize)
            {
                ModelState.AddModelError("", "File size must be 15MB or smaller.");
                return View(model); // Return view with an error if file size exceeds limit
            }

            // Assign file path for saving in the "uploads" directory with the file's original name
            model.FilePath = Path.Combine("uploads", file.FileName);

            // Resolve the full path to save the file in the web root directory
            var filePath = Path.Combine(_environment.WebRootPath, model.FilePath);
            try
            {
                // Save the file to the resolved path
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream); // Asynchronously copy file data to the new file
                }
            }
            catch (Exception ex)
            {
                // Handle file save errors by returning an error message
                ModelState.AddModelError("", $"File upload failed: {ex.Message}");
                return View(model); // Return view with error message if file save fails
            }

            // Set upload date for the document record
            model.UploadDate = DateTime.Now;

            // Add the new document record to the database and save changes
            _context.Documents.Add(model);
            await _context.SaveChangesAsync();

            // Redirect to the Index action to show updated list of documents after upload
            return RedirectToAction("Index");
        }

        // GET action to confirm deletion of a document, restricted to Admin role
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteConfirmation(int id)
        {
            // Fetch the document by ID from the database
            var document = await _context.Documents.FindAsync(id);
            if (document == null)
            {
                return NotFound(); // Return 404 if the document does not exist
            }

            // Return the view with the document information to confirm deletion
            return View(document);
        }

        // POST action to delete a document, restricted to Admin role
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int id)
        {
            // Retrieve the document from the database by ID
            var document = await _context.Documents.FindAsync(id);
            if (document == null)
            {
                return NotFound(); // Return 404 if the document does not exist
            }

            // Construct the full path to the file to delete it from the server
            var filePath = Path.Combine(_environment.WebRootPath, document.FilePath);
            if (System.IO.File.Exists(filePath))
            {
                System.IO.File.Delete(filePath); // Delete the file if it exists
            }

            // Remove the document record from the database and save changes
            _context.Documents.Remove(document);
            await _context.SaveChangesAsync();

            // Redirect to the Index action to display updated list after deletion
            return RedirectToAction("Index");
        }

        // GET action to list all documents available, accessible to all users (no authorization required)
        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            // Retrieve all documents from the database and convert them to a list asynchronously
            var documents = await _context.Documents.ToListAsync();
            // Return the view with the list of documents
            return View(documents);
        }

        // GET action to download a document by ID, accessible to all users (no authorization required)
        [AllowAnonymous]
        public IActionResult Download(int id)
        {
            // Retrieve the document by ID from the database
            var document = _context.Documents.Find(id);
            if (document == null) return NotFound(); // Return 404 if document does not exist

            // Construct full path to the document file
            var filePath = Path.Combine(_environment.WebRootPath, document.FilePath);

            // Prepare a memory stream to read the file contents for download
            var memory = new MemoryStream();
            using (var stream = new FileStream(filePath, FileMode.Open))
            {
                stream.CopyTo(memory); // Copy file data to memory stream
            }
            memory.Position = 0; // Reset stream position to start for download

            // Return the file for download as a PDF with appropriate MIME type and original file name
            return File(memory, "application/pdf", Path.GetFileName(document.FilePath));
        }
    }
}
