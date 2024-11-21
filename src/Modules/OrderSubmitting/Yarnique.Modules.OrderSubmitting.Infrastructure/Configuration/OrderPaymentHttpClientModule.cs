using Autofac;
using RestSharp;

namespace Yarnique.Modules.OrderSubmitting.Infrastructure.Configuration
{
    internal class OrderPaymentHttpClientModule : Autofac.Module
    {
        private readonly string PaymentApiUrl;

        public OrderPaymentHttpClientModule(string paymentApiUrl)
        {
            PaymentApiUrl = paymentApiUrl;
        }

        protected override void Load(ContainerBuilder builder)
        {
            builder.Register(_ =>
            {
                var baseUrl = new Uri(PaymentApiUrl);
                var client = new RestClient(baseUrl);
                return client;
            })
            .As<RestClient>()
            .SingleInstance();
        }
    }
}
