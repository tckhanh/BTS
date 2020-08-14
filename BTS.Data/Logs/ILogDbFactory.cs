using System;

namespace BTS.Data.Logs
{
    public interface ILogDbFactory : IDisposable
    {
        BTSDbContext Init();
    }
}