using BTS.Data.Infrastructure;
using BTS.Model.Models;

namespace BTS.Data.Repositories
{
    public interface IPageRepository : IRepository<WebPage>
    {
    }

    public class PageRepository : RepositoryBase<WebPage>, IPageRepository
    {
        public PageRepository(IDbFactory dbFactory) : base(dbFactory)
        {
        }
    }
}