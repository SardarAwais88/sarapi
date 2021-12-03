using sarapi.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace sarapi.IRepository
{
   public interface IUnitOfWork:IDisposable
    {
        IGenericRepository<User> users { get; }
        Task Save();
    }
}
