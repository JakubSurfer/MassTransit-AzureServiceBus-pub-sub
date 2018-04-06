using System;
using System.Threading.Tasks;
using MassTransit;
using MassTransit.AzureServiceBusTransport;
using MassTransitDemo.Messaging;
using MassTransitDemo.Messaging.Configuration;
using MassTransitDemo.Messaging.Contract.Commands;

namespace MassTransitDemo.Notification.Service
{
    class Program
    {
        public static void Main()
        {
            Console.Title = "Notification Service";

            var bus = Bus.Factory.CreateUsingAzureServiceBus(cfg =>
            {
                var host = cfg.Host(AzureServiceBusConfiguration.AzureConnectionString, hst =>
                {
                    hst.OperationTimeout = TimeSpan.FromSeconds(15);
                });
                cfg.SubscriptionEndpoint<IRegisterOrder>(host, "test1",
                    x => { x.Consumer<OrderRegisteredConsumer>(); });
            });


/*            var bus = BusConfiguration.ConfigureBusForAzure((cfg, host) =>
            {
                //Rabbit MQ
/*                cfg.ReceiveEndpoint(host, RabbitMqConfiguration.NotificationServiceQueue, e =>
                {
//                    e.Bind<IRegisterOrder>();
                    e.Consumer<OrderRegisteredConsumer>();
                });#1#

                //Azure SB
                cfg.SubscriptionEndpoint<IRegisterOrder>(host, "test1", x => { x.Consumer<OrderRegisteredConsumer>(); });
            });
            */
            bus.Start();

            Console.WriteLine("Listening for Order registered events...");
            Console.ReadLine();

            bus.Stop();
        }
    }
}
