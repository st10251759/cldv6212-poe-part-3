// Defines the namespace for the application models.
// The namespace organizes related classes and interfaces, aiding developers in maintaining a structured codebase and facilitating easy navigation through the application.
namespace ABCRetailers_Cameron_Chetty_CLDV6212_POE_P3.Models
{
    // The OrderRequest class represents a request for a specific product as part of an order placed by a user.
    // This class is critical for handling the details of each product that is included in an order, allowing for precise management of orders within the system.
    public class OrderRequest
    {
        // The OrderRequestId property serves as the unique identifier for each order request.
        // It is an integer that acts as the primary key in the OrderRequest database table, enabling the application to reference specific requests easily.
        public int OrderRequestId { get; set; }

        // The OrderId property links the order request to the corresponding order.
        // This integer property is a foreign key that connects the OrderRequest to the Order table, establishing a relationship between the order and its requested products.
        public int OrderId { get; set; }

        // The ProductId property identifies the specific product that is being requested in this order.
        // Similar to OrderId, this integer acts as a foreign key connecting the OrderRequest to the Product table, allowing the application to retrieve product details related to the order.
        public int ProductId { get; set; }

        // The OrderStatus property indicates the current status of the order request.
        // This string property can hold values such as "Pending," "Processed," or "Cancelled," providing insight into the state of each requested product in the context of the overall order.
        public string? OrderStatus { get; set; }

        // The Order property is a navigation property that creates a relationship between the OrderRequest and its associated Order.
        // This virtual property allows Entity Framework to load the related order data, enabling access to the order details directly from an order request.
        // The null! annotation indicates that the property should not be null when the class is in use, ensuring that an Order instance is always associated with the request.
        public virtual Order Order { get; set; } = null!;

        // The Product property is a navigation property that links the order request to the corresponding Product.
        // This virtual property facilitates the retrieval of product details from the Product table, making it easier to access the properties of the requested product when working with an order request.
        // The null! annotation ensures that this property is not null, reinforcing the expectation that an OrderRequest should always relate to a Product.
        public virtual Product Product { get; set; } = null!;
    }
}
