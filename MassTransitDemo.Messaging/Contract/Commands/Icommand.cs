using System;

namespace MassTransitDemo.Messaging.Contract.Commands
{

    public interface IMessage
    {
        Guid Id { get; set; }
    }
    public interface ISuperCommaand : IMessage
    {
    }

    public interface ICommand1 : ISuperCommaand
    {
        string Content { get; }
    }


    public interface ICommand2 : IMessage
    {
        string Content { get; }
    }
}
