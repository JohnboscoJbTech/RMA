using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataModel.DataModel;

namespace DataModel.IRepository
{
    public interface IResultRepository
    {
        int AddResult(Result result);
        IQueryable<Result> GetResults();
        IQueryable<Result> GetResults(int moduleId);
        State GetState(string code);
    }
}
