namespace BTS.Data.Logs
{
    public class LogDbFactory : LogDisposable, ILogDbFactory
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