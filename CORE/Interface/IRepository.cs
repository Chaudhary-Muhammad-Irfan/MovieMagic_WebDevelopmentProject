using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CORE.Interface
{
    public interface IRepository<Tentity>
    {
        Task Add(Tentity entity);
        Task Update(Tentity entity);
        Task Delete(int id);
        Task<List<Tentity>> viewAll();
        Task<Tentity> findById(int id);
    }
}
