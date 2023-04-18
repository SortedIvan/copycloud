
using Azure.Messaging.ServiceBus;
using emailappservice.Service;
using EmailServiceMessages;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Diagnostics;
using System.Text;

JObject config = JObject.Parse(File.ReadAllText(@"appsettings.json"));
var connString = (string)config["ServiceBusConfig"]["ConnectionString"];
var queue = (string)config["ServiceBusConfig"]["EmailQueue"];

await using var client = new ServiceBusClient(connString);
ServiceBusReceiver receiver = client.CreateReceiver(queue);
ProjectEmailService projectEmailService = new ProjectEmailService();

while (true)
{
    ServiceBusReceivedMessage receivedMessage = await receiver.ReceiveMessageAsync();

    try
    {
        EmailMessage message = JsonConvert.DeserializeObject<EmailMessage>(Encoding.UTF8.GetString(receivedMessage.Body));

        switch (message.Type)
        {
            case "invite":
                await projectEmailService.SendProjectInviteEmail(message.Receiver, message.Sender, message.Token, "Test Project", message.ProjectId);
                break;
            //case "inviteAccept":
            //    await projectEmailService.SendPersonJoinedEmail(message.)
                
        }
        Debug.WriteLine(receivedMessage.Body.ToString() + " " + "has been received");
    }
    catch (Exception ex)
    {
        Debug.WriteLine(ex.Message + " -------------------- is the problem -------------------- \n");
    }
}


        