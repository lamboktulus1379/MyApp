using System;
using System.Threading;
using Confluent.Kafka;

namespace EventBusKafka
{
    public class Consumer
    {
        public static string Consume()
        {
            var conf = new ConsumerConfig
            {
                GroupId = Environment.GetEnvironmentVariable("KAFKA_CONSUMER_GROUP"),
                BootstrapServers = Environment.GetEnvironmentVariable("KAFKA_BROKER"),
                AutoOffsetReset = AutoOffsetReset.Earliest,
                SecurityProtocol = SecurityProtocol.SaslSsl,
                SaslMechanism = SaslMechanism.Plain,
                SaslUsername = Environment.GetEnvironmentVariable("KAFKA_USERNAME"),
                SaslPassword = Environment.GetEnvironmentVariable("KAFKA_PASSWORD")
            };

            using (var c = new ConsumerBuilder<Ignore, string>(conf).Build())
            {
                c.Subscribe(Environment.GetEnvironmentVariable("KAFKA_TOPIC"));

                CancellationTokenSource cts = new CancellationTokenSource();

                Console.CancelKeyPress += (_, e) =>
                            {
                                e.Cancel = true;
                                cts.Cancel();
                            };

                try
                {
                    while (true)
                    {
                        try
                        {
                            var cr = c.Consume(cts.Token);
                            c.Commit();

                            return cr.Message.Value;
                        }
                        catch (ConsumeException e)
                        {
                            Console.WriteLine($"Error occurred: {e.Error.Reason}");
                        }
                    }
                }
                catch (OperationCanceledException)
                {
                    c.Close();
                }
            }
            return "";
        }
    }
}
