using Autofac;
using Microsoft.Extensions.DependencyInjection;

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
            builder.Register<IHttpClientFactory>(_ =>
            {
                var services = new ServiceCollection();
                services.AddHttpClient("PaymentApiClient", c =>
                {
                    c.BaseAddress = new Uri(PaymentApiUrl);
                    c.DefaultRequestHeaders.Add("Accept", "application/json");
                });
                var provider = services.BuildServiceProvider();
                return provider.GetRequiredService<IHttpClientFactory>();
            });
        }
        
    }
}
