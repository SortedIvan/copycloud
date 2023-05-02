using System.Text;
using Azure.Storage.Blobs;

namespace documentbgservice.Services
{
    public class DocumentSaveService
    {
        private BlobContainerClient containerClient;

        public DocumentSaveService(string connectionString, string containerName)
        {

            containerClient = new BlobContainerClient(connectionString, containerName);
        }

        public async Task<BlobClient> GetBlobClient(string documentName)
        {
            BlobClient blob = containerClient.GetBlobClient(documentName + ".txt");
            if (blob == null)
            {
                return null;
            }

            return blob;
        }

        public async Task<Tuple<bool, string>> UpdateDocumentContents(string documentContent, string documentName)
        {
            // Assume that the document name has been checked if it already exists
            BlobClient blobClient = await GetBlobClient(documentName);

            byte[] byteArray = Encoding.UTF8.GetBytes(documentContent);
            using (MemoryStream stream = new MemoryStream(byteArray))
            {
                await blobClient.UploadAsync(stream, overwrite: true);
                return Tuple.Create(true, "Document has been succesfully uploaded");
            }
        }
    }
}
