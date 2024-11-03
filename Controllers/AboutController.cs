// Import the necessary namespace for MVC-related functionality
using Microsoft.AspNetCore.Mvc;

// Define the namespace for the project, organizing it by application name
namespace ABCRetailers_Cameron_Chetty_CLDV6212_POE_P3.Controllers
{
    // Define the AboutController class which inherits from the base Controller class
    // This controller will manage requests for the "About" page
    public class AboutController : Controller
    {
        // Define the Index action method, which is the default action for this controller
        // This method handles HTTP GET requests sent to /About and returns the associated view
        public IActionResult Index()
        {
            // Render and return the default "Index" view for this action
            return View();
        }
    }
}
