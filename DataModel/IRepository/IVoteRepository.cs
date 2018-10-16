using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataModel.DataModel;

namespace DataModel.IRepository
{
    public interface IVoteRepository
    {
        int AddVote(Vote vote);
        IQueryable<Vote> GetVotes(int moduleId);
        void AddBytePhoto(Photo photo);
    }
}
