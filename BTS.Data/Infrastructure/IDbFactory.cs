using System;

namespace BTS.Data.Infrastructure
{
    public interface IDbFactory : IDisposable
    {
        BTSDbContext Init();
    }
}