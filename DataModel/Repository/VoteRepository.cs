using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataModel.DataModel;
using DataModel.IRepository;

namespace DataModel.Repository
{
    public class VoteRepository : IVoteRepository
    {
        private readonly RMAEntities _entities = new RMAEntities();

        public int AddVote(Vote vote)
        {
            var existing = _entities.Votes.FirstOrDefault(m => m.PartyId == vote.PartyId && m.ResultId == vote.ResultId);
            if (existing == null)
            {
                _entities.Votes.Add(vote);
            }
            else
            {
                existing.Vote1 = vote.Vote1;
                _entities.Entry(existing).State = EntityState.Modified;
            }

            _entities.SaveChanges();
            return vote.Id;
        }

        public IQueryable<Vote> GetVotes(int moduleId)
        {
            return _entities.Votes.Where(m => m.Result.Module == moduleId);
        }

        public void AddBytePhoto(Photo photo)
        {
            var existing = _entities.Photos.FirstOrDefault(m => m.ResultId == photo.ResultId && m.Reporter == photo.Reporter);
            if (existing == null)
            {
                _entities.Photos.Add(photo);
            }
            else
            {
                existing.Image = photo.Image;
                _entities.Entry(existing).State = EntityState.Modified;
            }

            _entities.SaveChanges();
        }
    }
}
