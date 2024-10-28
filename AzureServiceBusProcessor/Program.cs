using Azure.Messaging.ServiceBus;

// connection
await using var client = new ServiceBusClient("<ConnectionString>");

// create Sender
ServiceBusSender sender = client.CreateSender ("<QueueName>");

// Send Message
await sender.SendMessageAsync(new ServiceBusMessage("I will check it later"));

//  Create Processor
await using ServiceBusProcessor serviceBusProcessor = client.CreateProcessor("<QueueName>", new ServiceBusProcessorOptions
{
    AutoCompleteMessages = true,
    MaxConcurrentCalls = 1
});

// Configure Message
serviceBusProcessor.ProcessMessageAsync += MessageHandler;
serviceBusProcessor.ProcessErrorAsync += ErrorHandler;
// Configure Handler
async Task MessageHandler(ProcessMessageEventArgs processMessageEventArgs)
{
    Console.WriteLine(processMessageEventArgs.Message.Body.ToString());
}

// Configure Error
Task ErrorHandler(ProcessErrorEventArgs processErrorEventArgs)
{
    Console.WriteLine(processErrorEventArgs.ErrorSource);
    return Task.CompletedTask;
}
// Start Processing

await serviceBusProcessor.StartProcessingAsync();

Console.ReadLine();

