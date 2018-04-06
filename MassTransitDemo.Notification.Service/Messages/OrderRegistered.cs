using System;
using MassTransitDemo.Messaging.Contract.Events;

namespace MassTransitDemo.Notification.Service.Messages
{
    public class OrderRegistered : IOrderRegistered
    {
        public string Stuff { get; set; }
        public Guid Id { get; set; }
    }
}
