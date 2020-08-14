using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BTS.Data.Logs
{
    public interface ILogUnitOfWork
    {
        void Commit();
    }
}
