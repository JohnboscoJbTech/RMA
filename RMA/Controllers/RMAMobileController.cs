using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using DataModel.DataModel;
using DataModel.IRepository;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Newtonsoft.Json;
using RMA.Models.MobileModel;
using RMA.Utility;

namespace RMA.Controllers
{
    public class RMAMobileController : ApiController
    {
        private readonly IPartyRepository _partyRepository;
        private readonly IModuleRepository _moduleRepository;
        private readonly IResultRepository _resultRepository;
        private readonly IVoteRepository _voteRepository;
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;

        public RMAMobileController(IPartyRepository partyRepository, IModuleRepository moduleRepository,
            IResultRepository resultRepository, IVoteRepository voteRepository)
        {
            _partyRepository = partyRepository;
            _moduleRepository = moduleRepository;
            _resultRepository = resultRepository;
            _voteRepository = voteRepository;
            //UserManager = userManager;
            //SignInManager = signInManager;
        }

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.Current.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.Current.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        [HttpGet]
        public IHttpActionResult GetParties(string userId)
        {
            var partyUserLnks = _partyRepository.GetUserParties(userId);
            List<MobileParty> parties = new List<MobileParty>();
            foreach (var p in partyUserLnks)
            {
                parties.Add(new MobileParty()
                {
                    Id = p.Party.Id,
                    Name = p.Party.Name,
                    Code = p.Party.Code
                });
            }
            //List<MobileParty> parties = new List<MobileParty>();
            //foreach (var p in partyUserLnks)
            //{
            //    parties.Add(new MobileParty()
            //    {
            //        Id = p.Id,
            //        Name = p.Name,
            //        Code = p.Code
            //    });
            //}
            return Json(parties);
        }

        [HttpGet]
        public IHttpActionResult GetParties()
        {
            var partyUserLnks = _partyRepository.GetParties();
            List<MobileParty> parties = new List<MobileParty>();
            foreach (var p in partyUserLnks)
            {
                parties.Add(new MobileParty()
                {
                    Id = p.Id,
                    Name = p.Name,
                    Code = p.Code
                });
            }
            return Json(parties);
        }

        [HttpGet]
        public IHttpActionResult GetModules(string userId)
        {
            var moduleUserLnks = _moduleRepository.GetUserModules(userId);
            List<MobileModule> modules = new List<MobileModule>();
            foreach (var p in moduleUserLnks)
            {
                modules.Add(new MobileModule()
                {
                    Id = p.Module.Id,
                    Name = p.Module.Module1
                });
            }
            //List<MobileModle> modules = new List<MobileModle>();
            //foreach (var p in moduleUserLnks)
            //{
            //    modules.Add(new MobileModle()
            //    {
            //        Id = p.Id,
            //        Name = p.Module1
            //    });
            //}
            return Json(modules);
        }

        [HttpGet]
        public IHttpActionResult GetModules()
        {
            var moduleUserLnks = _moduleRepository.GetModules();
            List<MobileModule> modules = new List<MobileModule>();
            foreach (var p in moduleUserLnks)
            {
                modules.Add(new MobileModule()
                {
                    Id = p.Id,
                    Name = p.Module1
                });
            }
            return Json(modules);
        }

        [HttpPost]
        public MobileLoginModel ConfirmPerson(HttpRequestMessage message)
        {
            try
            {
                var postText = message.Content.ReadAsStringAsync().Result;
                if (string.IsNullOrEmpty(postText))
                {
                    return new MobileLoginModel { Message = "Log in parameters could not be retrieved." };
                }

                var model = JsonConvert.DeserializeObject<MobileLoginDetail>(postText);
                if (string.IsNullOrEmpty(model.Code) || string.IsNullOrEmpty(model.Email))
                {
                    return new MobileLoginModel { Message = "Something is wrong with the provided credentials. Please try again later" };
                }

                model.Password = "Johnbosco1990";
                var user = UserManager.FindAsync(model.Email, model.Password).Result;
                if (user == null)
                {
                    return new MobileLoginModel { Message = "User not found" };
                }
                var result = SignInManager.PasswordSignInAsync(model.Email, model.Password, false, false).Result;
                if (result == SignInStatus.Success)
                {
                    return new MobileLoginModel
                    {
                        Message = "Successfully logged in",
                        UserId = user.Id,
                        Code = model.Code
                    };

                }
                return new MobileLoginModel { Message = "Login failed." };
            }
            catch (Exception e)
            {
                return new MobileLoginModel { Message = "Login failed due to internal server error." };
            }
        }

        [HttpPost]
        public MobileLoginModel PostResult(HttpRequestMessage message)
        {
            try
            {
                var postText = message.Content.ReadAsStringAsync().Result;
                if (string.IsNullOrEmpty(postText))
                {
                    return new MobileLoginModel { Message = "No data synced." };
                }

                var models = JsonConvert.DeserializeObject<List<MoiblePostResult>>(postText);
                //if (model.Module > 0)
                //{
                //    return new MobileLoginModel { Message = "Something is wrong with the provided data. Please try again later" };
                //}
                List<int> ints = new List<int>();
                foreach (var model in models)
                {
                    string[] codes = model.PersonCode.Split('/');
                    var path = "";
                    if (model.ProofImage.Length != 0)
                    {
                        var image = Utils.ConvertToImage(model.ProofImage);
                        path = Path.Combine(HttpContext.Current.Server.MapPath("~/Images/Proof/"), model.PersonCode.Replace('/', '_') + ".jpg");
                        image.Save(path);
                    }
                    
                    var result_id = _resultRepository.AddResult(new Result
                    {
                        Module = model.Module,
                        RegVotes = model.RegVotes,
                        AccredVotes = model.AccredVotes,
                        CastVotes = model.CastVotes,
                        InvalidVotes = model.InvVotes,
                        ReportedBy = model.ReportedBy,
                        ProofUrl = path,
                        State = int.Parse(codes[0]),
                        Lga = int.Parse(codes[1]),
                        Ward = int.Parse(codes[2]),
                        PollingUnit = int.Parse(codes[3])
                    });
                    if (result_id > 0)
                    {
                        foreach (var m in model.Votes)
                        {
                            _voteRepository.AddVote(new Vote
                            {
                                ResultId = result_id,
                                PartyId = m.Party,
                                Vote1 = m.Vote
                            });
                        }

                        _voteRepository.AddBytePhoto(new Photo {
                            ResultId = result_id,
                            Image = model.ProofImage,
                            Reporter = model.ReportedBy
                        });
                    }
                    ints.Add(model.Id);
                }                
                
                if (ints.Count > 0)
                {
                    return new MobileLoginModel
                    {
                        Message = "Successfully Synced",
                        Result = ints
                    };

                }
                return new MobileLoginModel { Message = "synchronization failed." };
            }
            catch (Exception e)
            {
                return new MobileLoginModel { Message = "Synchronization failed due to internal server error." };
            }
        }
    }
}
