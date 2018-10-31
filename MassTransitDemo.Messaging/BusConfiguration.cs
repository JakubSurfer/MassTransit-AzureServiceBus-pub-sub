using System;
using MassTransit;
using MassTransit.AzureServiceBusTransport;
using MassTransit.AzureServiceBusTransport.Configuration;
using MassTransit.Context;
using MassTransit.RabbitMqTransport;
using MassTransitDemo.Messaging.Configuration;
using MassTransitDemo.Messaging.Contract.Commands;
using Microsoft.ServiceBus;
using Microsoft.ServiceBus.Messaging;

namespace MassTransitDemo.Messaging
{
    public static class BusConfiguration
    {
        public static IBusControl ConfigureBusForRabbitMQ(Action<IRabbitMqBusFactoryConfigurator, IRabbitMqHost> registrationAction = null)
        {
            return Bus.Factory.CreateUsingRabbitMq(cfg =>
            {
                var host = cfg.Host(new Uri(RabbitMqConfiguration.RabbitMqUri), hst =>
                {
                    hst.Username(RabbitMqConfiguration.UserName);
                    hst.Password(RabbitMqConfiguration.Password);
                });

//                MessageCorrelation.UseCorrelationId<IRegisterOrder>(x => x.Id);

                registrationAction?.Invoke(cfg, host);
            });

        }


        public static IBusControl ConfigureBusForAzure(Action<IServiceBusBusFactoryConfigurator, IServiceBusHost> registrationAction = null)
        {
            return Bus.Factory.CreateUsingAzureServiceBus(cfg =>
            {
                var host = cfg.Host(AzureServiceBusConfiguration.AzureConnectionString, hst =>
                {
                    hst.OperationTimeout = TimeSpan.FromSeconds(15);
                });

//                MessageCorrelation.UseCorrelationId<IRegisterOrder>(x => x.Id);
                
                registrationAction?.Invoke(cfg, host);
            });
        }


    }
}
