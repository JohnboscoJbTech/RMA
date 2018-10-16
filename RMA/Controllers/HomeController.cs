using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DataModel.IRepository;
using RMA.Models.MobileModel;
using RMA.Utility;

namespace RMA.Controllers
{
    public class HomeController : Controller
    {
        private readonly IPartyRepository _partyRepository;
        private readonly IModuleRepository _moduleRepository;
        private readonly IResultRepository _resultRepository;
        private readonly IVoteRepository _voteRepository;

        public HomeController(IPartyRepository partyRepository, IModuleRepository moduleRepository, 
            IResultRepository resultRepository, IVoteRepository voteRepository)
        {
            _partyRepository = partyRepository;
            _moduleRepository = moduleRepository;
            _resultRepository = resultRepository;
            _voteRepository = voteRepository;
        }

        public ActionResult Index()
        {
            var results = _resultRepository.GetResults();
            foreach (var result in results)
            {

            }
            ViewBag.Result = results;
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult Result(int id)
        {
            //var votes = _voteRepository.GetVotes(id);//.GroupBy(m=>m.PartyId);
            var sortted = _voteRepository.GetVotes(id).GroupBy(m => m.Party.Name).Select(m => new {Count = m.Sum(g => g.Vote1), Party = m.Key});
            var result = _resultRepository.GetResults(id);
            List<WebVote> webVotes = new List<WebVote>();
            //foreach (var vote in votes)
            //{
            //    webVotes.Add(new WebVote()
            //    {
            //        Party = vote.Party.Code,
            //        Vote = vote.Vote1
            //    });
            //}

            foreach (var vote in sortted)
            {
                webVotes.Add(new WebVote()
                {
                    Party = vote.Party,
                    Vote = vote.Count
                });
            }

            var output = new WebResult();

            if (result.Any())
            {
                output.RegVotes = result.Sum(m => m.RegVotes);
                output.AccredVotes = (int)result.Sum(m => m.AccredVotes);
                output.CastVotes = (int)result.Sum(m => m.CastVotes);
                output.Votes = webVotes;
                output.InvVotes = result.Sum(m => m.InvalidVotes);
            }

            //var returnResult = new WebResult()
            //{
            //    RegVotes = result.RegVotes,
            //    AccredVotes = (int)result.AccredVotes,
            //    CastVotes = (int)result.CastVotes,
            //    Votes = webVotes
            //};
            ViewBag.Result = output;
            return View();
        }

        public ActionResult AccreditationResult()
        {
            return View();
        }

        [HttpGet]
        public ActionResult GetLga(string stateCode)
        {
            var states = Utils.GetLga(stateCode);
            List<SelectListItem> formatted = new List<SelectListItem>();
            foreach (var f in states)
            {
                formatted.Add(new SelectListItem
                {
                    Value = f.LgaCode,
                    Text = f.LgaName
                });
            }
            return Json(formatted, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetScrollData(string state="", string lga="", string ward="")
        {
            if (!string.IsNullOrEmpty(ward))
            {
                //query for the pu's in the ward in the lga and the state selected
                //return result
            }
            else if(!string.IsNullOrEmpty(lga))
            {
                //query for the ward in the lga and state selected
                //return result
            }
            else if (!string.IsNullOrEmpty(state))
            {
                //query for the lga in the state selected
                //return result
                var selState = _resultRepository.GetState(state);
                if (selState != null)
                {
                    var result = _resultRepository.GetResults(1).Where(m => m.State == selState.Id)
                        .GroupBy(g => g.Lga).Select(s => new { RegVotes = s.Sum(a => a.RegVotes), AccredVotes = s.Sum(a => a.AccredVotes), Lga = s.Key});
                    
                    var output = new List<WebResult>();

                    if (result.Any())
                    {
                        foreach(var r in result)
                        {
                            output.Add(new WebResult {
                                RegVotes = r.RegVotes,
                                AccredVotes = r.AccredVotes,
                                State = selState.StateName,
                                Lga = Utils.GetLgaName(r.Lga, selState.StateCode)
                            });
                        }
                        
                    }

                    return Json(output, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                //query for the states in the country
                var result = _resultRepository.GetResults(1).GroupBy(m => m.State1.StateName)
                    .Select(s => new { RegVotes = s.Sum(g => g.RegVotes), AccredVotes = s.Sum(g => g.AccredVotes), StateName = s.Key });

                var output = new List<WebResult>();

                if (result.Any())
                {
                    foreach (var r in result)
                    {
                        output.Add(new WebResult
                        {
                            RegVotes = r.RegVotes,
                            AccredVotes = r.AccredVotes,
                            State = "Nigeria",
                            Lga = r.StateName
                        });
                    }

                }

                return Json(output, JsonRequestBehavior.AllowGet);
            }
            return Json("", JsonRequestBehavior.AllowGet);
        }
    }
}