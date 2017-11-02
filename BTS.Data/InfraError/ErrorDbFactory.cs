namespace BTS.Data.InfraError
{
    public class ErrorDbFactory : ErrorDisposable, IErrorDbFactory
    {
        private BTSDbContext dbContext;

        public BTSDbContext Init()
        {
            return dbContext ?? (dbContext = new BTSDbContext());
        }

        protected override void DisposeCore()
        {
            if (dbContext != null)
                dbContext.Dispose();
        }
    }
}