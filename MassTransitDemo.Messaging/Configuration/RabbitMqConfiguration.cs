namespace MassTransitDemo.Messaging.Configuration
{
    public class RabbitMqConfiguration
    {
        public const string RabbitMqUri = "rabbitmq://localhost/";
        public const string UserName = "powel";
        public const string Password = "powel";
        public const string RegisterOrderServiceQueue = "registerorder.service";
        public const string NotificationServiceQueue = "notification.service";
    }
}