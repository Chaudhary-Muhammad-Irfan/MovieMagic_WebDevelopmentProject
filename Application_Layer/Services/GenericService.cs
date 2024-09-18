using CORE.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application_Layer.Services
{
    public class GenericService <Tentity>
    {
        private readonly IRepository<Tentity> repo;
        public GenericService(IRepository<Tentity> repo)
        {
            this.repo = repo;
        }
        public async Task Add(Tentity entity)
        {
            await repo.Add(entity);
        }
        public async Task Update(Tentity entity)
        {
            await repo.Update(entity);
        }
        public async Task Delete(int id)
        {
            await repo.Delete(id);
        }
        public async Task<List<Tentity>> viewAll()
        {
            return await repo.viewAll();
        }
        public async Task<Tentity> findById(int id)
        {
            return await repo.findById(id);
        }
    }
}
