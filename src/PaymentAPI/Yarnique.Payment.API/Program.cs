using Yarnique.Payment.API.PaymentProcessing;

namespace Yarnique.Payment.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers();
            builder.Services.AddScoped<IPaymentService, PaymentService>();

            var app = builder.Build();

            app.MapControllers();

            app.Run();
        }
    }
}
