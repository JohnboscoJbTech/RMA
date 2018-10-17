using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RMA.Models.MobileModel
{
    public class UtilModel
    {
    }

    public class WebResult
    {
        public int DeviceId { get; set; }
        public int RegVotes { get; set; }
        public int? AccredVotes { get; set; }
        public int? CastVotes { get; set; }
        public int? InvVotes { get; set; }
        public string DeviceUniqueId { get; set; }
        public int Module { get; set; }
        public string PersonCode { get; set; }
        public List<WebVote> Votes { get; set; }
        public string ReportedBy { get; set; }
        public string State { get; set; }
        public string Lga { get; set; }
        public string Ward { get; set; }
        public string Pu { get; set; }
    }

    public class WebVote
    {
        public int Vote { get; set; }
        public string Module { get; set; }
        public string Party { get; set; }
    }

    public class ExportModel
    {
        public int RegVotes { get; set; }
        public int? AccredVotes { get; set; }
        public int? CastVotes { get; set; }
        public int? InvVotes { get; set; }
        public string ReportedBy { get; set; }
        public string State { get; set; }
        public string Lga { get; set; }
        public string Ward { get; set; }
        public string Pu { get; set; }
        public int PDPVote { get; set; }
        public int APCVote { get; set; }
        public int APGAVote { get; set; }
        public int SDPVote { get; set; }
    }
}