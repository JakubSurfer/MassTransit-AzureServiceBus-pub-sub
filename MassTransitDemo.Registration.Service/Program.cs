using GreenPipes.Internals.Reflection;
using MassTransit;
using MassTransit.AzureServiceBusTransport;
using MassTransit.Serialization;
using MassTransit.Serialization.JsonConverters;
using MassTransitDemo.Messaging.Configuration;
using MassTransitDemo.Messaging.Contract.Commands;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;

namespace MassTransitDemo.Registration.Service
{
    public class MyOwnConverter : JsonConverter
    {
        readonly IImplementationBuilder _builder;
        public MyOwnConverter(IImplementationBuilder builder)
        {
            _builder = builder ?? throw new ArgumentNullException(nameof(builder));
        }
        public override bool CanConvert(Type objectType)
        {
            return objectType.Name == "MessageEnvelope";
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var proxyType = _builder.GetImplementationType(typeof(IRegisterOrder));
            var actual = serializer.Deserialize<MyOrder>(reader);
            var asdsa = JToken.FromObject(actual);
            var msg = new MyMessage
            {
                Message = asdsa,
                DestinationAddress = AzureServiceBusConfiguration.AzureConnectionString,
                MessageType = new[] {
                    "urn:message:MassTransitDemo.Messaging.Contract.Commands:IRegisterOrder",
                "urn:message:MassTransitDemo.Registration.Service:MyOrder"},
                Headers = new Dictionary<string, object>(),
                MessageId = actual.Id.ToString(),
                CorrelationId = actual.Id.ToString(),
                ConversationId = actual.Id.ToString(),
                SourceAddress = "sb://janysbtest.servicebus.windows.net/PCJANY01_MassTransitDemoPublisher_bus_zazoyybntybbmprtbdk353wobf?express=true&autodelete=300"
            };

            return msg;
        }

        public override bool CanWrite => false;

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var messageData = value as IMessageData;


        }
    }

    public class MyMessage : MessageEnvelope
    {
        public string MessageId { get; set; }

        public string RequestId { get; set; }

        public string CorrelationId { get; set; }

        public string ConversationId { get; set; }

        public string InitiatorId { get; set; }

        public string SourceAddress { get; set; }

        public string DestinationAddress { get; set; }

        public string ResponseAddress { get; set; }

        public string FaultAddress { get; set; }

        public string[] MessageType { get; set; }

        public object Message { get; set; }

        public DateTime? ExpirationTime { get; set; }

        public IDictionary<string, object> Headers { get; set; }

        public HostInfo Host { get; set; }
    }

    public class MyOrder : IRegisterOrder
    {
        public string Content { get; set; }

        public Guid Id { get; set; }
    }

    class Program
    {
        public static void Main()
        {
            Console.Title = "Registration Service";


            var bus = Bus.Factory.CreateUsingAzureServiceBus(cfg =>
            {
                cfg.ConfigureJsonDeserializer(settings =>
                {
                    var c = settings.Converters;
                    c.Add(new MyOwnConverter(new DynamicImplementationBuilder()));
                    return settings;
                });
                var host = cfg.Host(AzureServiceBusConfiguration.AzureConnectionString, hst =>
                {
                    hst.OperationTimeout = TimeSpan.FromSeconds(15);
                });
                //configure enpoint for ASB publish
                cfg.SubscriptionEndpoint(host, "test", "test", x =>
                {
                    x.Consumer<RegisterOrderConsumer>();
                });
                //configure enpoint for MAsstransit publish
                //cfg.SubscriptionEndpoint<IRegisterOrder>(host, "test", x =>
                //{
                //    x.Consumer<RegisterOrderConsumer>();
                //});



            });


            bus.Start();

            Console.WriteLine("Listening for Register order commands.. ");
            Console.ReadLine();

            bus.Stop();
        }
    }
}
