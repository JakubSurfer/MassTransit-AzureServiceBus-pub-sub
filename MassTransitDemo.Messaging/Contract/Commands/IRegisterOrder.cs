using System;

namespace MassTransitDemo.Messaging.Contract.Commands
{
    public interface IRegisterOrder
    {
        string Content { get; }
        Guid Id { get; set; }
    }
}
