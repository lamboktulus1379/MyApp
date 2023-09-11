using Google.Cloud.PubSub.V1;
using Grpc.Core;

namespace GooglePubSub
{
    public class CreateSubscription
    {
        public Subscription CreateSubscriptionWithOrdering(string projectId, string subscriptionId, string topicId)
        {
            SubscriberServiceApiClient subscriber = SubscriberServiceApiClient.Create();
            var topicName = TopicName.FromProjectTopic(projectId, topicId);
            var subscriptionName = SubscriptionName.FromProjectSubscription(projectId, subscriptionId);

            var subscriptionRequest = new Subscription
            {
                SubscriptionName = subscriptionName,
                TopicAsTopicName = topicName,
                EnableMessageOrdering = true
            };

            Subscription subscription = null;
            try
            {
                subscription = subscriber.CreateSubscription(subscriptionRequest);
            }
            catch (RpcException e) when (e.Status.StatusCode == StatusCode.AlreadyExists)
            {
                // Already exists.  That's fine.
            }
            return subscription;
        }
    }
}
