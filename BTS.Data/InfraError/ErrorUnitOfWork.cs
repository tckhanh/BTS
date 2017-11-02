namespace BTS.Data.InfraError
{
    public class ErrorUnitOfWork : IErrorUnitOfWork
    {
        private readonly IErrorDbFactory dbFactory;
        private BTSDbContext dbContext;

        public ErrorUnitOfWork(IErrorDbFactory dbFactory)
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