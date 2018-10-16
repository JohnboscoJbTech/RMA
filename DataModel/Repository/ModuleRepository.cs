using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataModel.DataModel;
using DataModel.IRepository;

namespace DataModel.Repository
{
    public class ModuleRepository : IModuleRepository
    {
        private readonly RMAEntities _entities = new RMAEntities();

        public IQueryable<ModulePersonLnk> GetUserModules(string userId)
        {
            return _entities.ModulePersonLnks.Where(m => m.ReporterId == userId);
        }

        public IQueryable<Module> GetModules()
        {
            return _entities.Modules.AsQueryable();
        }
    }
}
