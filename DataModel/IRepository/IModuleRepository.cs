using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataModel.DataModel;

namespace DataModel.IRepository
{
    public interface IModuleRepository
    {
        IQueryable<Module> GetModules();
        IQueryable<ModulePersonLnk> GetUserModules(string userId);
    }
}
