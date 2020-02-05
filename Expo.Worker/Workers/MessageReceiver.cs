using System;
using System.Text;
using RabbitMQ.Client;

namespace Expo.Worker.Workers
{
    public class MessageReceiver : DefaultBasicConsumer
    {
        public IModel model { get; }
        public MessageReceiver(IModel model) : base(model)
        {
            this.model = model;
        }


        public override void HandleBasicDeliver(string consumerTag, ulong deliveryTag, bool redelivered, string exchange, string routingKey, IBasicProperties properties, byte[] body)
        {

            Console.WriteLine($"Consuming Topic Message");
            Console.WriteLine(string.Concat("Message received from the exchange ", exchange));
            Console.WriteLine(string.Concat("Consumer tag: ", consumerTag));
            Console.WriteLine(string.Concat("Delivery tag: ", deliveryTag));
            Console.WriteLine(string.Concat("Routing tag: ", routingKey));
            Console.WriteLine(string.Concat("Message: ", Encoding.UTF8.GetString(body)));
            model.BasicAck(deliveryTag, false);
        }
    }
}