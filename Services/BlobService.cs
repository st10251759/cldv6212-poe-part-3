// The using directive imports the Azure.Storage.Blobs namespace,
// which contains classes for working with Azure Blob Storage.
// This includes functionalities for uploading, deleting, and managing blob storage.
using Azure.Storage.Blobs;
// This using directive imports the Azure.Storage.Blobs.Models namespace,
// which includes various model classes related to blob storage operations.
using Azure.Storage.Blobs.Models;

// Defines the namespace for the services within the application.
// The namespace helps organize code logically, making it easier to manage and navigate.
namespace ABCRetailers_Cameron_Chetty_CLDV6212_POE_P3.Services
{
    // The BlobService class encapsulates methods for interacting with Azure Blob Storage.
    // It provides functionality to upload files to a blob container and delete blobs as needed.
    public class BlobService
    {
        // The _blobServiceClient field holds an instance of BlobServiceClient,
        // which is responsible for connecting to the Azure Blob Storage service.
        private readonly BlobServiceClient _blobServiceClient;

        // The _containerName field specifies the name of the blob container where the files will be uploaded.
        // In this case, the container is named "products", which suggests it stores product-related files.
        private readonly string _containerName = "products";

        // The constructor for BlobService takes a connection string as a parameter.
        // This connection string is used to instantiate the BlobServiceClient, enabling access to the Blob Storage account.
        public BlobService(string connectionString)
        {
            // Initializes the _blobServiceClient with the provided connection string.
            _blobServiceClient = new BlobServiceClient(connectionString);
        }

        // The UploadAsync method uploads a file to the blob container asynchronously.
        // It takes a Stream representing the file's data and a string for the file name.
        public async Task<string> UploadAsync(Stream fileStream, string fileName)
        {
            // Retrieves a reference to the blob container using the BlobServiceClient instance.
            var containerClient = _blobServiceClient.GetBlobContainerClient(_containerName);

            // Retrieves a reference to the specific blob (file) within the container using the provided file name.
            var blobClient = containerClient.GetBlobClient(fileName);

            // Asynchronously uploads the file stream to the specified blob.
            await blobClient.UploadAsync(fileStream);

            // Returns the URI of the uploaded blob as a string, which can be used to access the blob.
            return blobClient.Uri.ToString();
        }

        // The DeleteBlobAsync method deletes a specified blob from the blob container asynchronously.
        // It takes the URI of the blob as a parameter.
        public async Task DeleteBlobAsync(string blobUri)
        {
            // Parses the blob URI to create a Uri object.
            Uri uri = new Uri(blobUri);

            // Extracts the blob name from the URI by accessing the last segment of the URI path.
            // The syntax `uri.Segments[^1]` retrieves the last segment of the URI,
            // which corresponds to the blob name.
            string blobName = uri.Segments[^1];

            // Retrieves a reference to the blob container using the BlobServiceClient instance.
            var containerClient = _blobServiceClient.GetBlobContainerClient(_containerName);

            // Retrieves a reference to the specific blob (file) using the extracted blob name.
            var blobClient = containerClient.GetBlobClient(blobName);

            // Asynchronously deletes the blob, including any associated snapshots if they exist.
            await blobClient.DeleteIfExistsAsync(DeleteSnapshotsOption.IncludeSnapshots);
        }

    }
}
