using Azure.Storage.Blobs;
using MongoDB.Driver.Core.Configuration;
using System.Text;

namespace projectservice.Utility
{
    public class BlobStorageHelper : IBlobStorageHelper
    {
        private readonly IConfiguration config;
        private string connectionString;
        private string containerName;
        private BlobContainerClient containerClient;

        public BlobStorageHelper(IConfiguration _config) 
        {
            this.config = _config;
            connectionString = config.GetSection("BlobStorage:ConnectionString").Value.ToString();
            containerName = config.GetSection("BlobStorage:Container").Value.ToString();
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

        public async Task<Tuple<bool, string>> CreateDocumentBlob(string documentName)
        {
            // Assume that the document name has been checked if it already exists
            BlobClient blobClient = await GetBlobClient(documentName);

            // Create a new memory stream to hold the file contents
            using (MemoryStream memoryStream = new MemoryStream())
            {
                // Write some text to the memory stream
                using (StreamWriter writer = new StreamWriter(memoryStream))
                {
                    await blobClient.UploadAsync(memoryStream);
                    return Tuple.Create(true, "Document has been succesfully created");
                }
            }
        }


    }
}
