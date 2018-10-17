using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web;

namespace RMA.Models.MobileModel
{
    public class MobileModel
    {
    }

    public class MobileParty
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
    }

    public class MobileModule
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class MobileLoginModel
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Message { get; set; }
        public string UserId { get; set; }
        public List<int> Result { get; set; }
    }

    public class MobileLoginDetail
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string Code { get; set; }
    }

    public class MoiblePostResult
    {
        public int Id { get; set; }
        public int DeviceId { get; set; }
        public int RegVotes { get; set; }
        public int AccredVotes { get; set; }
        public int CastVotes { get; set; }
        public int InvVotes { get; set; }
        public string DeviceUniqueId { get; set; }
        public int Module { get; set; }
        public string PersonCode { get; set; }
        public List<MobileVote> Votes { get; set; }
        public string ReportedBy { get; set; }
        public byte[] ProofImage { get; set; }
    }

    public class MobileVote
    {
        public int Vote { get; set; }
        public int Module { get; set; }
        public int Party { get; set; }
    }
}