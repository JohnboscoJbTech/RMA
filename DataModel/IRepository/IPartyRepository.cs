﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataModel.DataModel;

namespace DataModel.IRepository
{
    public interface IPartyRepository
    {
        IQueryable<Party> GetParties();
        IQueryable<PartyPersonLnk> GetUserParties(string userId);
    }
}
