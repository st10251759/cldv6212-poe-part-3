// Defines the namespace for the application's models.
// The namespace organizes the classes related to the application's data representation, making the codebase easier to navigate and maintain.
namespace ABCRetailers_Cameron_Chetty_CLDV6212_POE_P3.Models
{
    // The OrderAdminViewModel class serves as a data transfer object (DTO) designed specifically for presenting order details in the admin interface.
    // It is commonly used to separate the view logic from the data access logic, allowing the application to present a tailored view of the order data.
    public class OrderAdminViewModel
    {
        // The OrderId property uniquely identifies each order.
        // It is an integer that corresponds to the primary key in the Order database table, enabling the administration panel to reference specific orders easily.
        public int OrderId { get; set; }

        // The OrderDate property captures the date and time when the order was placed.
        // This DateTime property is essential for tracking orders chronologically and is useful for reporting purposes.
        public DateTime OrderDate { get; set; }

        // The UserEmail property stores the email address of the user who placed the order.
        // This string property allows administrators to quickly identify the customer associated with each order, facilitating customer service and communication.
        public string UserEmail { get; set; }

        // The Status property indicates the current processing status of the order (e.g., "Pending," "Shipped," "Completed").
        // It is declared as a nullable string (using string?) to allow for cases where the status may not be set or known at the time of order creation.
        public string? Status { get; set; }

        // The TotalPrice property represents the total cost of the order.
        // This decimal property provides an overview of the financial aspect of the order, allowing administrators to assess order value quickly.
        public decimal TotalPrice { get; set; }
    }
}
