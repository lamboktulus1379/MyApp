using System;
using System.Threading.Tasks;
using Confluent.Kafka;

namespace EventBusKafka
{
    public static class Producer
    {
        public static readonly string topic = Environment.GetEnvironmentVariable("KAFKA_TOPIC");
        public static async Task Produce(string message)
        {
            var config = new ProducerConfig
            {
                BootstrapServers = Environment.GetEnvironmentVariable("KAFKA_BROKER"),
                SecurityProtocol = SecurityProtocol.SaslSsl,
                SaslMechanism = SaslMechanism.Plain,
                SaslUsername = Environment.GetEnvironmentVariable("KAFKA_USERNAME"),
                SaslPassword = Environment.GetEnvironmentVariable("KAFKA_PASSWORD")
            };

            using (var p = new ProducerBuilder<Null, string>(config).Build())
            {
                try
                {
                    var dr = await p.ProduceAsync(topic, new Message<Null, string> { Value = message });
                    Console.WriteLine($"Delievered '{dr.Value}' to '{dr.TopicPartitionOffset}'");
                }
                catch (ProduceException<Null, string> e)
                {
                    Console.WriteLine($"Delivery failed: {e.Error.Reason}");
                }
            }
        }
    }
}
