namespace BTS.Data.Infrastructure
{
    public class DbFactory : Disposable, IDbFactory
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