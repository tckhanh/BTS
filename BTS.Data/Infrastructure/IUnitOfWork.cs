namespace BTS.Data.Infrastructure
{
    public interface IUnitOfWork
    {
        void Commit();
    }
}