namespace Yarnique.Modules.OrderSubmitting.Application.Callbacks
{
    public interface IOrderStatusHub
    {
        Task OnOrderStatusChanged(string status);
    }
}
