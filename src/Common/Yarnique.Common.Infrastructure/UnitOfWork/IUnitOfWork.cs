namespace Yarnique.Common.Infrastructure.UnitOfWork
{
    public interface IUnitOfWork
    {
        Task<int> CommitAsync(CancellationToken cancellationToken = default, Guid? internalCommandId = null);
    }
}
