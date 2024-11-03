// Imports the System.ComponentModel.DataAnnotations.Schema namespace.
// This namespace contains classes that enable you to work with attributes that are used for data modeling and mapping properties to database columns.
// In this case, it is specifically used to define foreign key relationships in the entity model.
using System.ComponentModel.DataAnnotations.Schema;

// Defines the namespace for the application models.
// This organization helps keep related classes together and enhances code maintainability.
// In this example, the Order class is part of the ABCRetailers application models.
namespace ABCRetailers_Cameron_Chetty_CLDV6212_POE_P3.Models
{
    // Defines the Order class, which represents an order placed by a user in the retail application.
    // This class encapsulates all relevant properties and relationships associated with an order.
    public class Order
    {
        // The OrderId property serves as a unique identifier for each order.
        // It is typically used as the primary key in a database table to ensure that each order can be uniquely referenced.
        public int OrderId { get; set; }

        // The UserId property holds the unique identifier for the user who placed the order.
        // This property establishes a relationship between the order and the user, allowing the application to track which user made the purchase.
        public string UserId { get; set; }

        // The OrderDate property records the date and time when the order was placed.
        // This information is essential for order management, reporting, and tracking order history.
        public DateTime OrderDate { get; set; }

        // The Status property indicates the current status of the order (e.g., "Pending," "Shipped," "Completed").
        // It is marked as nullable (using string?) to allow for cases where the order status may not yet be defined.
        public string? Status { get; set; }

        // The TotalPrice property holds the total amount for the order.
        // It is defined as nullable (using decimal?) to accommodate scenarios where the total price may not be calculated or set at the time of order creation.
        public decimal? TotalPrice { get; set; }

        // The OrderRequests property is a collection that holds all OrderRequest instances associated with the Order.
        // This establishes a one-to-many relationship, where one order can have multiple requests associated with it.
        // The property is initialized to a new List<OrderRequest>() to ensure that it is never null, allowing for safe access to the collection.
        public virtual ICollection<OrderRequest> OrderRequests { get; set; } = new List<OrderRequest>();

        // The User property establishes a foreign key relationship to the ApplicationUser class.
        // This allows for easy navigation from an order to the user who placed it.
        // The [ForeignKey] attribute specifies that the UserId property is the foreign key that links to the ApplicationUser table.
        [ForeignKey("UserId")]
        public virtual ApplicationUser User { get; set; }
    }
}
