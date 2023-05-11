using Azure.Messaging.ServiceBus;
using Azure.Storage.Blobs;
using Newtonsoft.Json;
using projectservice.Utility;


public class DocumentMessage
{
    public string ProjectID { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;

}


namespace projectservice.Services
{
    public class DocumentService : IDocumentService
    {
        private IConfiguration config;
        private IBlobStorageHelper blobStorageHelper;
        private readonly ServiceBusSender serviceBusSender;
        private readonly ServiceBusClient busClient;

        public DocumentService(IConfiguration _config, IBlobStorageHelper _blobStorageHelper)
        {
            config = _config;
            this.blobStorageHelper = _blobStorageHelper;
            busClient = new ServiceBusClient(config.GetSection("ServiceBusConfig:ConnectionStringDocument").Value);
            serviceBusSender = busClient.CreateSender("projectsavequeue");
        }


        public async Task<Tuple<bool, string>> SaveDocument(string content, string projectId)
        {
            // 1) Check if the document exists
            // 2) Send a message to the document background app with the content and the projectid
            
            try
            {
                DocumentMessage documentMessage = new DocumentMessage
                {
                    ProjectID = projectId,
                    Content = content
                };

                string messageBody = JsonConvert.SerializeObject(documentMessage);
                // SEND MESSAGE
                await serviceBusSender.SendMessageAsync(new ServiceBusMessage(messageBody)); // Message is sent to queue
            }
            catch (Exception ex) 
            {
                return Tuple.Create(false, ex.Message);
            }

            return Tuple.Create(true, "Document has been succesfully saved");
           
        }

        public async Task<Tuple<bool, string>> CreateDocument(string projectId)
        {
            // 1) Check if the document name already exists - This is mitigated for now as there can't be two of the same identifiers

            return await blobStorageHelper.CreateDocumentBlob(projectId);
        }

        public async Task<string> GetDocumentContent(string projectId)
        {
            BlobClient client = await blobStorageHelper.GetBlobClient(projectId);

            if (client == null)
            {
                return "";
            }

            return client.DownloadContent().Value.Content.ToString();
        }

    }
}