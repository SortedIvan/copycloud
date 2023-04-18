using Azure.Storage.Blobs;
using projectservice.Utility;

namespace projectservice.Services
{
    public class DocumentService
    {
        private IConfiguration config;
        private IBlobStorageHelper blobStorageHelper;

        public DocumentService(IConfiguration _config, IBlobStorageHelper _blobStorageHelper)
        {
            config = _config;
            this.blobStorageHelper = _blobStorageHelper;
        }


        public async Task<Tuple<bool, string>> SaveDocument(string content, string projectId)
        {
            // 1) Check if the document exists
            // 2) Send a message to the document background app with the content and the projectid


            return Tuple.Create(true, "Document has been succesfully saved");
           
        }

        public async Task<Tuple<bool, string>> CreateDocument(string projectName, string projectId)
        {
            // 1) Check if the document name already exists

            return await blobStorageHelper.CreateDocumentBlob(projectName + projectId);
        }

    }
}