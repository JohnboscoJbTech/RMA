using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using DataModel.Utility;
using RMA.Models.MobileModel;

namespace RMA.Utility
{
    public static class Utils
    {
        public static IQueryable<dynamic> GetStates()
        {
            var states = ApplicationUtility.GetStates();
            List<dynamic> formatted = new List<dynamic>();
            foreach (var f in states)
            {
                formatted.Add(new
                {
                    Code = f.StateCode,
                    Name = f.StateName
                });
            }
            return formatted.AsQueryable();
        }

        public static List<MobileModule> GetModule()
        {
            var modules = ApplicationUtility.GetModules();
            List<MobileModule> formatted = new List<MobileModule>();
            if (modules.Count() > 0)
            {
                foreach (var f in modules)
                {
                    formatted.Add(new MobileModule()
                    {
                        Name = f.Module1,
                        Id = f.Id
                    });
                }
            }
            return formatted;
        }

        public static IQueryable<MobileModule> GetModuleDropdown()
        {
            var modules = ApplicationUtility.GetModules();
            List<MobileModule> formatted = new List<MobileModule>();
            if (modules.Count() > 0)
            {
                foreach (var f in modules)
                {
                    formatted.Add(new MobileModule()
                    {
                        Name = f.Module1,
                        Id = f.Id
                    });
                }
            }
            return formatted.AsQueryable();
        }

        public static Image ConvertToImage(byte[] byteArrayIn)
        {
            using (var ms = new MemoryStream(byteArrayIn))
            {
                return Image.FromStream(ms);
            }
        }

        public static IQueryable<dynamic> GetLga(string stateCode)
        {
            return ApplicationUtility.GetLga(stateCode);            
        }

        public static string GetLgaName(int id, string stateCode)
        {
            var lga = ApplicationUtility.GetLga(id, stateCode);
            if (lga != null) return lga.LgaName;
            else return "";
        }

        public static List<ExportModel> BuildExportModel(this List<WebResult> model)
        {
            //initialize list of longitudial model
            List<ExportModel> exportModels = new List<ExportModel>();

            model.ForEach(m => {
                ExportModel exportModel = new ExportModel();
                exportModel.RegVotes = m.RegVotes;
                exportModel.AccredVotes = m.AccredVotes;
                exportModel.CastVotes = m.CastVotes;
                exportModel.InvVotes = m.InvVotes;
                exportModel.PDPVote = m.Votes.FirstOrDefault(mo => mo.Party == "PDP") != null ? m.Votes.FirstOrDefault(mo => mo.Party == "PDP").Vote : 0;
                exportModel.APCVote = m.Votes.FirstOrDefault(mo => mo.Party == "APC") != null ? m.Votes.FirstOrDefault(mo => mo.Party == "APC").Vote : 0;
                exportModel.APGAVote = m.Votes.FirstOrDefault(mo => mo.Party == "APGA") != null ? m.Votes.FirstOrDefault(mo => mo.Party == "APGA").Vote : 0;
                exportModel.SDPVote = m.Votes.FirstOrDefault(mo => mo.Party == "SDP") != null ? m.Votes.FirstOrDefault(mo => mo.Party == "SDP").Vote : 0;
                exportModel.State = m.State;
                exportModel.ReportedBy = m.ReportedBy;
                exportModels.Add(exportModel);                
            });
            return exportModels;
        }
    }
}