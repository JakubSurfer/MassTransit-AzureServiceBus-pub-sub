namespace MassTransitDemo.Messaging.Configuration
{
    public class AzureServiceBusConfiguration
    {
        public const string AzureConnectionString = "Endpoint=sb://brregsync.servicebus.windows.net/;SharedAccessKeyName=RootManageSharedAccessKey;SharedAccessKey=2PIAb6SruXkxsKmzHVUlOoqfYTiAdfJX0w+/z1ZNkA8=";
        public const string ServiceBusSubscriptionName = "test";
        public const string ServiceBusTopicName = "test";
    }
}
