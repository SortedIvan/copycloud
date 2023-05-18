using Azure.Messaging.EventHubs;
using Azure.Messaging.EventHubs.Processor;
using Azure.Storage.Blobs;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using projectservice.Models;
using System.Text;
using System.Threading;

namespace projectservice.Services
{
    public class ProjectEventsService : BackgroundService
    {
        private readonly ILogger<ProjectEventsService> _logger;
        private readonly IProjectService _projectService;
        private readonly IConfiguration _config;
        BlobContainerClient blobContainerClient;
        EventProcessorClient processor;

        private readonly string blobConnString;
        private readonly string container;
        private readonly string consumerGroup;
        private readonly string eventHubConnectionString;
        private readonly string eventHubName;


        public ProjectEventsService(ILogger<ProjectEventsService> logger, IServiceScopeFactory factory, IConfiguration config) 
        {

            _config = config;
            _logger = logger;

            blobConnString = _config.GetSection("EventHubStorage:ConnectionString").Value;
            container = _config.GetSection("EventHubStorage:ContainerProjectEvents").Value;
            consumerGroup = _config.GetSection("EventHubConfig:ConsumerGroupProjectEvents").Value;
            eventHubConnectionString = _config.GetSection("EventHubConfig:ConnectionString").Value;
            eventHubName = _config.GetSection("EventHubConfig:Hub").Value;

            _projectService = factory.CreateScope().ServiceProvider.GetRequiredService<IProjectService>();

            blobContainerClient = new BlobContainerClient(blobConnString, container);
            processor = new EventProcessorClient(blobContainerClient, consumerGroup, eventHubConnectionString, eventHubName);
            processor.ProcessEventAsync += Processor_ProcessEventAsync;
            processor.ProcessErrorAsync += Processor_ProcessErrorAsync;
        }


        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Service started");

            while (!stoppingToken.IsCancellationRequested)
            {
                await processor.StartProcessingAsync();
            }
        }

        Task Processor_ProcessErrorAsync(Azure.Messaging.EventHubs.Processor.ProcessErrorEventArgs arg)
        {
            Console.WriteLine("Error: " + arg.Exception.ToString());
            return Task.CompletedTask;
        }

        async Task Processor_ProcessEventAsync(ProcessEventArgs arg)
        {
            Console.WriteLine($"Event received from partition {arg.Partition.PartitionId}:{arg.Data.EventBody.ToString()}");
            ProjectEvent message = JsonConvert.DeserializeObject<ProjectEvent>(Encoding.UTF8.GetString(arg.Data.EventBody));

            switch (message.EventType)
            {
                case "deleteuser":
                    await _projectService.DeleteUserFromProject(message.DeleteUserData.ProjectId, message.DeleteUserData.UserEmail);
                    break;
                    //case "inviteAccept":
                    //    await projectEmailService.SendPersonJoinedEmail(message.)

            }

            await arg.UpdateCheckpointAsync();
        }

    }
}
