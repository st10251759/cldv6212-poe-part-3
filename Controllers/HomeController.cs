using ABCRetailers_Cameron_Chetty_CLDV6212_POE_P3.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace ABCRetailers_Cameron_Chetty_CLDV6212_POE_P3.Controllers
{
    public class HomeController : Controller
    {
        // This readonly field is a logger instance specific to the HomeController class,
        // used to log various types of messages (e.g., errors, information) throughout this controller.
        private readonly ILogger<HomeController> _logger;

        // Constructor injection of the logger instance:
        // The ILogger<HomeController> instance is injected by ASP.NET Core's dependency injection (DI) framework
        // and assigned to the private _logger field. This enables logging functionality within this controller.
        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        // The Index action method:
        // This method is invoked when the user navigates to the root URL or the Home page of the application.
        // It simply returns the "Index" view associated with this action.
        public IActionResult Index()
        {
            return View();
        }

        // The Error action method:
        // This action handles errors that occur within the application.
        // It is marked with a ResponseCache attribute, which controls how the response from this action is cached.
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            // Creates and returns the Error view, passing an instance of
