namespace Yarnique.BackgroundService.Helpers
{
    public interface IProcessOrderService
    {
        public Task ProcessDueOrders();
    }
}
