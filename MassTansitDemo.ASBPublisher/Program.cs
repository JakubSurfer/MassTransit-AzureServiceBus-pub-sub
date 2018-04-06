using System;
using System.Text;
using System.Threading.Tasks;
using MassTransitDemo.Messaging.Configuration;
using Microsoft.Azure.ServiceBus;
using Microsoft.ServiceBus;
using Newtonsoft.Json;

namespace MassTansitDemo.ASBPublisher
{
    class Program
    {
        static void Main()
        {
            CreateTopicIfNonExisting();
            Send().GetAwaiter().GetResult();
        }

        static async Task Send()
        {
            var topicClient = new TopicClient(AzureServiceBusConfiguration.AzureConnectionString, AzureServiceBusConfiguration.ServiceBusTopicName);
            await SendMessagesAsync(topicClient);
            Console.ReadKey();
            await topicClient.CloseAsync();
        }

        static async Task SendMessagesAsync(TopicClient tc)
        {
            try
            {
                for (var i = 0; i < 10; i++)
                {
                    var id = Guid.NewGuid();
                    var ro = new
                    {
                        Id = id,
                        Content = $"Message {i}"
                    };
                    var message = new Message(ToByteArray(ro));
                    Console.WriteLine($"Sending message: {id}");

                    await tc.SendAsync(message);
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine($"{DateTime.Now} :: Exception: {exception.Message}");
            }
        }

        private static void CreateTopicIfNonExisting()
        {
            var topic = AzureServiceBusConfiguration.ServiceBusTopicName;
            var subscription = AzureServiceBusConfiguration.ServiceBusSubscriptionName;
            var nm = NamespaceManager.CreateFromConnectionString(AzureServiceBusConfiguration.AzureConnectionString);

            if (!nm.TopicExists(topic))
            {
                Console.WriteLine($"Creating {topic} topic ");
                nm.CreateTopic(topic);
            }
        }
        public static byte[] ToByteArray(object message)
        {
            string serializeObject = JsonConvert.SerializeObject(message);
            byte[] bytes = Encoding.UTF8.GetBytes(serializeObject);
            return bytes;
        }
    }
}
