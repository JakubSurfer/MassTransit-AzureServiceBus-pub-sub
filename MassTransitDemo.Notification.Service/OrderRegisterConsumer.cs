using System;
using System.Threading.Tasks;
using MassTransit;
using MassTransitDemo.Messaging.Contract.Commands;

namespace MassTransitDemo.Notification.Service
{
    public class OrderRegisteredConsumer : IConsumer<IRegisterOrder>
    {
        public async Task Consume(ConsumeContext<IRegisterOrder> context)
        {
            await Console.Out.WriteLineAsync($"Customer notification sent: " +
                                             $"Order id {context.Message.Id} " +
                                             $"Content : {context.Message.Content}");
        }
    }
}
