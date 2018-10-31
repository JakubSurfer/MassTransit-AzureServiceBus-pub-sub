using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using MassTransit;
using MassTransit.Util;
using MassTransitDemo.Messaging;
using MassTransitDemo.Messaging.Contract.Commands;

namespace MassTransitDemo.Publisher
{
    class Program
    {
        public static void Main()
        {
            var bus = BusConfiguration.ConfigureBusForAzure();

            bus.Start();

            Console.Title = "Registration Publisher";

            while (true)
            {
                Console.WriteLine("Send a command ...");

                var message = Console.ReadLine();
                try
                {
                    var corellationId = NewId.NextGuid();

                    bus.Publish<ICommand1>(new 
                    {
                        Id = corellationId
                    });
                    Console.WriteLine($"Command with id {corellationId} has been sent");
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    bus.Stop();
                    throw;
                }
            }
        }
    }
}
