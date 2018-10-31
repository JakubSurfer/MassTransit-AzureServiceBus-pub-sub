using System;
using System.Threading.Tasks;
using MassTransit;
using MassTransitDemo.Messaging.Contract.Commands;

namespace MassTransitDemo.Registration.Service
{
    internal class Command1Handler : IConsumer<ISuperCommaand>
    {
        public async Task Consume(ConsumeContext<ISuperCommaand> context)
        {
            await Console.Out.WriteLineAsync($"Command with id {context.Message.Id} been received");
        }
    }
}