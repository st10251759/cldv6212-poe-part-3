// The using directive includes the System.ComponentModel.DataAnnotations namespace,
// which provides classes that are used to define metadata for data models and enable validation.
using System.ComponentModel.DataAnnotations;

// Defines the namespace for the application models.
// The namespace helps to organize related classes within the application, making it easier to manage and navigate through the codebase.
namespace ABCRetailers_Cameron_Chetty_CLDV6212_POE_P3.Models
{
    // The Product class represents a product in the e-commerce application.
    // It encapsulates all the necessary properties that describe a product, enabling the application to manage and display product information effectively.
    public class Product
    {
        // The ProductId property serves as the unique identifier for each product.
        // This integer property acts as the primary key in the Product database table, allowing the application to reference specific products easily.
        public int ProductId { get; set; }

        // The Name property stores the name of the product.
        // This nullable string property allows for flexibility in product naming and can be left empty if needed.
        public string? Name { get; set; }

        // The ProductDescription property holds a detailed description of the product.
        // This nullable string provides space for additional information about the product's features, specifications, and benefits.
        public string? ProductDescription { get; set; }

        // The Price property indicates the cost of the product.
        // This decimal property is nullable, allowing the price to be set or omitted. 
        // The DisplayFormat attribute is applied to this property to format the price as a currency when displayed in the user interface.
        // The ApplyFormatInEditMode set to false means that the formatting will not be applied when editing the price value.
        [DisplayFormat(DataFormatString = "{0:C}", ApplyFormatInEditMode = false)]
        public decimal? Price { get; set; }

        // The Category property categorizes the product.
        // This nullable string allows for flexibility in classifying products (e.g., Electronics, Clothing) but can be left empty.
        public string? Category { get; set; }

        // The Availability property indicates whether the product is available for purchase.
        // This nullable boolean provides a simple true/false value, allowing for clear representation of stock status.
        public bool? Availability { get; set; }

        // The ImageUrlpath property stores the URL path to the product's image.
        // This nullable string allows for flexibility in handling product images and can be empty if no image is available.
        public string? ImageUrlpath { get; set; }
    }
}
