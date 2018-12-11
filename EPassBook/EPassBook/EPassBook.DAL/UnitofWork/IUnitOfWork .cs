using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPassBook.DAL.Interface
{
    public interface IUnitOfWork : IDisposable
    {
        void SaveChanges();
    }
}
