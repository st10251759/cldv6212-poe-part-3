// Imports the DataAnnotations namespace, which provides attributes that are used to define metadata for classes and properties.
// This includes validation attributes that can be used to enforce rules on model properties.
using System.ComponentModel.DataAnnotations;

// Defines the namespace for the application models. This helps organize the codebase by grouping related classes together.
// In this case, the models for the ABC Retailers application are defined under this namespace.
namespace ABCRetailers_Cameron_Chetty_CLDV6212_POE_P3.Models
{
    // Defines the Document class, which represents a document that will be managed within the application.
    // This class could be used to handle various types of documents, such as images, PDFs, or other files that are uploaded by users.
    public class Document
    {
        // The Id property serves as a unique identifier for each Document instance.
        // It is typically used as the primary key in a database table and allows for easy retrieval and manipulation of individual documents.
        public int Id { get; set; }

        // The Title property holds the title of the document.
        // It is marked with the Required attribute, indicating that this field must be filled out before the document can be saved.
        // The ErrorMessage property specifies the message that will be displayed if validation fails due to this field being empty.
        [Required(ErrorMessage = "The Title field is required.")]
        public string Title { get; set; }

        // The Description property provides a textual description of the document.
        // Like the Title, this property is also marked as required to ensure that users provide context or details about the document.
        // This enhances the information available in the application and aids users in understanding the purpose of the document.
        [Required(ErrorMessage = "The Description field is required.")]
        public string Description { get; set; }

        // The FilePath property stores the path where the document file is located.
        // This change from FileName to FilePath indicates that the application will now store the full path to the file rather than just its name,
        // which is essential for retrieving the file from storage or serving it to users.
        [Required(ErrorMessage = "The FilePath field is required.")]
        public string FilePath { get; set; } // Changed from FileName to FilePath

        // The UploadDate property records the date and time when the document was uploaded.
        // This information can be valuable for tracking document history, displaying upload times to users, 
        // or implementing sorting/filtering features based on upload dates.
        public DateTime UploadDate { get; set; }
    }
}
