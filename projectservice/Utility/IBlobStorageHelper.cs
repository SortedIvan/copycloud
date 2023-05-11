using Azure.Storage.Blobs;
using System.Text;

namespace projectservice.Utility
{
    public interface IBlobStorageHelper
    {
        Task<BlobClient> GetBlobClient(string documentName);
        Task<Tuple<bool, string>> UpdateDocumentContents(string documentContent, string documentName);
        Task<Tuple<bool, string>> CreateDocumentBlob(string documentName);
    }
}
