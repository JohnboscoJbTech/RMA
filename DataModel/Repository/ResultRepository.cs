using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using DataModel.DataModel;
using DataModel.IRepository;

namespace DataModel.Repository
{
    public class ResultRepository : IResultRepository
    {
        private readonly RMAEntities _entities = new RMAEntities();

        public int AddResult(Result result)
        {
            var existing = _entities.Results.FirstOrDefault(m => m.ReportedBy == result.ReportedBy);
            if (existing == null)
            {
                _entities.Results.Add(result);
            }
            else
            {
                existing.AccredVotes = result.AccredVotes;
                existing.RegVotes = result.RegVotes;
                existing.CastVotes = result.CastVotes;
                existing.InvalidVotes = result.InvalidVotes;
                existing.ProofUrl = result.ProofUrl;
                _entities.Entry(existing).State = EntityState.Modified;
                result.Id = existing.Id;
            }

            _entities.SaveChanges();
            return result.Id;
        }

        public IQueryable<Result> GetResults()
        {
            return _entities.Results.AsQueryable();
        }

        public IQueryable<Result> GetResults(int moduleId)
        {
            return _entities.Results.Where(m => m.Module == moduleId);
        }

        public State GetState(string code)
        {
            return _entities.States.FirstOrDefault(m => m.StateCode == code);
        }
    }
}
