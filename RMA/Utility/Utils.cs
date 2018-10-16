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
    }
}