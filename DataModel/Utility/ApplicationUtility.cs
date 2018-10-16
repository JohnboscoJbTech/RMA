using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataModel.DataModel;

namespace DataModel.Utility
{
    public static class ApplicationUtility
    {
        private static readonly RMAEntities _entities = new RMAEntities();

        public static IQueryable<State> GetStates()
        {
            return _entities.States.AsQueryable();
        }

        public static IQueryable<Module> GetModules()
        {
            return _entities.Modules.AsQueryable();
        }

        public static IQueryable<Lga> GetLga(string stateCode)
        {
            return _entities.Lgas.Where(m => m.StateCode == stateCode);
        }

        public static Lga GetLga(int id, string stateCode)
        {
            return _entities.Lgas.FirstOrDefault(m => m.Id == id && m.StateCode == stateCode);
        }
    }
}
