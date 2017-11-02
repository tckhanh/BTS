using System;

namespace BTS.Data.InfraError
{
    public interface IErrorDbFactory : IDisposable
    {
        BTSDbContext Init();
    }
}