using System;
using MassTransit;
using MassTransit.AzureServiceBusTransport;
using MassTransitDemo.Messaging.Configuration;
using MassTransitDemo.Messaging.Contract.Commands;

namespace MassTransitDemo.Registration.Service
{
    class Program
    {
        public static void Main()
        {
            Console.Title = "Registration Service";


            var bus = Bus.Factory.CreateUsingAzureServiceBus(cfg =>
            {
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
                cfg.SubscriptionEndpoint<IRegisterOrder>(host, "test", x =>
                {
                    x.Consumer<RegisterOrderConsumer>();
                });



            });

            bus.Start();

            Console.WriteLine("Listening for Register order commands.. ");
            Console.ReadLine();

            bus.Stop();
        }
    }
}
