using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataModel.DataModel;
using DataModel.IRepository;

namespace DataModel.Repository
{
    public class PartyRepository : IPartyRepository
    {
        private readonly RMAEntities _entities = new RMAEntities();

        public IQueryable<PartyPersonLnk> GetUserParties(string userId)
        {
            return _entities.PartyPersonLnks.Where(m => m.UserId == userId);
        }

        public IQueryable<Party> GetParties()
        {
            return _entities.Parties.AsQueryable();
        }
    }
}
