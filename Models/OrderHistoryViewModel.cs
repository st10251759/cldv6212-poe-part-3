// Defines the namespace for the application's models.
// The namespace organizes related classes and interfaces, helping to maintain a structured codebase and making it easier for developers to navigate through the application.
namespace ABCRetailers_Cameron_Chetty_CLDV6212_POE_P3.Models
{
    // The OrderHistoryViewModel class acts as a data transfer object (DTO) that encapsulates the information required to display a user's order history.
    // By using a view model, the application ensures that the data presented to the user interface is both relevant and structured according to specific needs.
    public class OrderHistoryViewModel
    {
        // The OrderId property serves as a unique identifier for each order in the user's order history.
        // It is an integer that corresponds to the primary key in the Order database table, allowing for easy reference and identification of specific orders.
        public int OrderId { get; set; }

        // The OrderDate property records the date and time when the order was placed.
        // This DateTime property is crucial for displaying the chronological order of purchases, enabling users to track their ordering patterns over time.
        public DateTime OrderDate { get; set; }

        // The Status property indicates the current processing status of the order, such as "Pending," "Shipped," or "Delivered."
        // This string property allows users to quickly understand the progress of their orders, enhancing the user experience by providing essential order details at a glance.
        public string Status { get; set; }

        // The TotalPrice property represents the total cost of the order.
        // This decimal property allows users to view the financial aspect of each order, helping them keep track of their spending and budget effectively.
        public decimal TotalPrice { get; set; }
    }
}
