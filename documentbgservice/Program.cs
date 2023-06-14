using Azure.Messaging.ServiceBus;
using documentbgservice;
using documentbgservice.Services;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Diagnostics;
using System.Text;

JObject config = JObject.Parse(File.ReadAllText(@"configuration.json"));
var connString = (string)config["ServiceBusConfig"]["ConnectionString"];
string queue = "projectsavequeue";
var blobConnection = (string)config["Azure"]["ConnectionString"];
var containerName = (string)config["Azure"]["Container"];

await using var client = new ServiceBusClient(connString);
ServiceBusReceiver receiver = client.CreateReceiver(queue);
DocumentSaveService documentSaveService = new DocumentSaveService(blobConnection, containerName);




while (true)
{
    ServiceBusReceivedMessage receivedMessage = await receiver.ReceiveMessageAsync();

    try
    {
        DocumentMessage message = JsonConvert.DeserializeObject<DocumentMessage>(Encoding.UTF8.GetString(receivedMessage.Body));

        // Do saving here

        await documentSaveService.UpdateDocumentContents(message.Content, message.ProjectID);

        // Update project statistics here


        Debug.WriteLine(receivedMessage.Body.ToString() + " " + "has been received");
    }
    catch (Exception ex)
    {
        Debug.WriteLine(ex.Message + " -------------------- is the problem -------------------- \n");
    }
}


