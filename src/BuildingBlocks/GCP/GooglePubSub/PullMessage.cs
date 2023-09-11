using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Google.Cloud.PubSub.V1;
using Grpc.Core;

namespace GooglePubSub
{
    public static class PullMessage
    {
        public static async Task<int> PullMessagesAsync(string projectId, string subscriptionId, bool acknowledge)
        {
            SubscriptionName subscriptionName = SubscriptionName.FromProjectSubscription(projectId, subscriptionId);
            SubscriberClient subscriber = await SubscriberClient.CreateAsync(subscriptionName);
            // SubscriberClient runs your message handle function on multiple
            // threads to maximize throughput.
            int messageCount = 0;


            Task startTask = subscriber.StartAsync((PubsubMessage message, CancellationToken cancel) =>
            {
                string text = System.Text.Encoding.UTF8.GetString(message.Data.ToArray());
                Console.WriteLine($"Message {message.MessageId}: {text}");
                Interlocked.Increment(ref messageCount);
                return Task.FromResult(acknowledge ? SubscriberClient.Reply.Ack : SubscriberClient.Reply.Nack);
            });
            // Run for 5 seconds.
            await Task.Delay(5000);
            await subscriber.StopAsync(CancellationToken.None);
            // Lets make sure that the start task finished successfully after the call to stop.
            await startTask;
            return messageCount;
        }


        public static string PullMessagesSync(string projectId, string subscriptionId, bool acknowledge)
        {
            SubscriptionName subscriptionName = SubscriptionName.FromProjectSubscription(projectId, subscriptionId);
            SubscriberServiceApiClient subscriberClient = SubscriberServiceApiClient.Create();
            int messageCount = 0;
            string text = string.Empty;
            try
            {
                // Pull messages from server,
                // allowing an immediate response if there are no messages.
                PullResponse response = subscriberClient.Pull(subscriptionName, maxMessages: 20);
                // Print out each received message.

                foreach (ReceivedMessage msg in response.ReceivedMessages)
                {
                    text = System.Text.Encoding.UTF8.GetString(msg.Message.Data.ToArray());
                    Console.WriteLine($"Message Id {msg.Message.MessageId}");

                    Interlocked.Increment(ref messageCount);
                }
                // If acknowledgement required, send to server.
                if (acknowledge && messageCount > 0 && text != string.Empty)
                {
                    subscriberClient.Acknowledge(subscriptionName, response.ReceivedMessages.Select(msg => msg.AckId));
                }

                return text;
            }
            catch (RpcException ex) when (ex.Status.StatusCode == StatusCode.Unavailable)
            {
                Console.WriteLine($"Error: {ex.Message}");
                // UNAVAILABLE due to too many concurrent pull requests pending for the given subscription.
            }
            return string.Empty;
        }
    }
}
