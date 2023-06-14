
using Azure.Messaging.EventHubs;
using Azure.Messaging.EventHubs.Processor;
using Azure.Storage.Blobs;
using emailappservice.Service;
using emailappservice.Utility;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;
using System.Text;

JObject config = JObject.Parse(File.ReadAllText(@"configuration.json"));

var connString = (string)config["EventHubConfig"]["ConnectionString"];
var hub = (string)config["EventHubConfig"]["Hub"];
var consumerGroup = (string)config["EventHubConfig"]["ConsumerGroup"];

var blobConnString = (string)config["BlobStorage"]["ConnectionString"];
var container = (string)config["BlobStorage"]["Container"];


ProjectEmailService projectEmailService = new ProjectEmailService();

// < ----------------------- Event consuming ------------------------------->
Task Processor_ProcessErrorAsync(Azure.Messaging.EventHubs.Processor.ProcessErrorEventArgs arg)
{
    Console.WriteLine("Error: " + arg.Exception.ToString());
    return Task.CompletedTask;
}

async Task Processor_ProcessEventAsync(ProcessEventArgs arg)
{
    Console.WriteLine($"Event received from partition {arg.Partition.PartitionId}:{arg.Data.EventBody.ToString()}");
    EmailMessage message = JsonConvert.DeserializeObject<EmailMessage>(Encoding.UTF8.GetString(arg.Data.EventBody));

    switch (message.Type)
    {
        case "invite":
            await projectEmailService.SendProjectInviteEmail(message.Receiver, message.Sender, message.Token, "Test Project", message.ProjectId);
            break;
            //case "inviteAccept":
            //    await projectEmailService.SendPersonJoinedEmail(message.)

    }

    await arg.UpdateCheckpointAsync();
}

BlobContainerClient blobContainerClient = new BlobContainerClient(blobConnString, container);
EventProcessorClient processor = new EventProcessorClient(blobContainerClient, consumerGroup, connString, hub);

processor.ProcessEventAsync += Processor_ProcessEventAsync;
processor.ProcessErrorAsync += Processor_ProcessErrorAsync;

while (true)
{
    try
    {
        await processor.StartProcessingAsync();
    }
    catch (Exception ex)
    {
        Console.WriteLine(ex.ToString());
    }
}


        