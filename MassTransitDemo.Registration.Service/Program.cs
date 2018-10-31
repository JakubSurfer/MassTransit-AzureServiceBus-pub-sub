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

                cfg.SubscriptionEndpoint<ICommand1>(host, "command1", x =>
                {
                    x.Consumer<Command1Handler>();
                });



            });


            bus.Start();

            Console.WriteLine("Listening for commands.. ");
            Console.ReadLine();

            bus.Stop();
        }
    }
}
