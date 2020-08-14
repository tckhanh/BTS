namespace BTS.Data.Logs
{
    public class LogUnitOfWork : ILogUnitOfWork
    {
        private readonly ILogDbFactory dbFactory;
        private BTSDbContext dbContext;

        public LogUnitOfWork(ILogDbFactory dbFactory)
        {
            this.dbFactory = dbFactory;
        }

        public BTSDbContext DbContext
        {
            get { return dbContext ?? (dbContext = dbFactory.Init()); }
        }

        public void Commit()
        {
            DbContext.SaveChanges();
        }
    }
}