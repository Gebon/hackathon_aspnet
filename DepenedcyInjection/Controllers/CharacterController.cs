using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using DependencyInjection.Models;
using DepenedcyInjection.Infrastructure;
using DepenedcyInjection.Repositories;
using Domain;
using Domain.Models;
using Microsoft.AspNet.Identity;
using Ninject.Infrastructure.Language;
using PagedList;

namespace DepenedcyInjection.Controllers
{
    public class CharacterController : Controller
    {
        private static int PageSize = 2;
        private IRepository<Character> charactersRepository;
        private IRepository<Vote> votesRepository;
        private IRepository<VoteItem> voteItemsRepository;
        private IRepository<ApplicationUser> usersRepository;
        private readonly IUserProvider userProvider;
        private readonly ICartProvider cartProvider;
        private IWeekProvider weekProvider;

        private int Week => weekProvider.GetWeek();
        private bool WeeklyVoted(string userId) => votesRepository.Items.ToList().FirstOrDefault(x => x.User.Id == userId && x.Week == Week) != null;
        public CharacterController(ICartProvider cartProvider, IUserProvider userProvider,
            IRepository<Character> charactersRepository, IRepository<Vote> votesRepository, IRepository<VoteItem> voteItemsRepository,
            IRepository<ApplicationUser> usersRepository, IWeekProvider weekProvider)
        {
            this.charactersRepository = charactersRepository;
            this.votesRepository = votesRepository;
            this.voteItemsRepository = voteItemsRepository;
            this.usersRepository = usersRepository;
            this.weekProvider = weekProvider;
            this.cartProvider = cartProvider;
            this.userProvider = userProvider;
        }

        public ViewResult List(int? page)
        {
            page = page ?? 1;
            ViewBag.Action = "List";
            ViewBag.Points = cartProvider.GetCart(this).Points;
            var userId = userProvider.GetUser(this).Identity.GetUserId();
            return View(charactersRepository.Items.ToEnumerable().Select(x => new CharacterViewModel
            {
                CanVote = !WeeklyVoted(userId) && cartProvider.GetCart(this).Points >= x.Cost,
                IsVoted = cartProvider.GetCart(this).Votes.Contains(x.Id),
                WeeklyVoted = WeeklyVoted(userId),
                Character = x
            }).ToPagedList(page.Value, PageSize));
        }

        public IEnumerable<Character> FilterInternal(string fieldName, string fieldValue)
        {
            var result = charactersRepository.Items.ToList().Where(x => x.GetType()
                .GetProperties()
                .First(property => property.Name == fieldName).GetValue(x).ToString().Equals(fieldValue, StringComparison.InvariantCultureIgnoreCase));
            return result.OrderBy(x => x.Name);
        }

        public ActionResult Filter(string fieldName, string fieldValue, int? page)
        {
            ViewBag.Action = "Filter";
            return View("List", FilterInternal(fieldName, fieldValue).Select(x => new CharacterViewModel
            {
                CanVote = !WeeklyVoted(userProvider.GetUser(this).Identity.GetUserId()) && cartProvider.GetCart(this).Points >= x.Cost,
                IsVoted = cartProvider.GetCart(this).Votes.Contains(x.Id),
                WeeklyVoted = WeeklyVoted(userProvider.GetUser(this).Identity.GetUserId()),
                Character = x
            }).ToPagedList(page ?? 1, PageSize));
        }

        [HttpPost]
        public ActionResult Search(Character characterWithMismatches)
        {
            var userId = userProvider.GetUser(this).Identity.GetUserId();
            var result = charactersRepository.Items.ToList();
            ViewBag.Points = cartProvider.GetCart(this).Points;
            foreach (var o in characterWithMismatches.GetType().GetProperties().Where(x => x.Name != "Id"))
            {
                if (string.IsNullOrEmpty(o.GetValue(characterWithMismatches)?.ToString()))
                    continue;
                IEnumerable<Character> filtered = FilterInternal(o.Name, o.GetValue(characterWithMismatches).ToString());
                result.RemoveAll(x => !filtered.Contains(x));
            }
            return PartialView("_CharacterList", result.Select(x => new CharacterViewModel
            {
                CanVote = !WeeklyVoted(userId) && cartProvider.GetCart(this).Points >= x.Cost,
                IsVoted = cartProvider.GetCart(this).Votes.Contains(x.Id),
                WeeklyVoted = WeeklyVoted(userId),
                Character = x
            }));
        }

        public ActionResult Search(IEnumerable<Character> characters)
        {
            return View(characters ?? new List<Character>());
        }

        [HttpPost]
        public ActionResult Vote(int id)
        {
            var cost = charactersRepository.Items.First(x => x.Id == id).Cost;
            var points = cartProvider.GetCart(this).Points;

            if (!WeeklyVoted(userProvider.GetUser(this).Identity.GetUserId()) && points >= cost)
            {
                var votes = cartProvider.GetCart(this).Votes;
                if (!votes.Contains(id))
                {
                    cartProvider.GetCart(this).Points = (int)(points - cost);
                    votes.Add(id);
                }
                return PartialView("_CharacterCard",
                    new CharacterViewModel
                    {
                        Character = charactersRepository.Items.First(x => x.Id == id),
                        IsVoted = true,
                        WeeklyVoted = false
                    });
            }

            return PartialView("_CharacterCard", new CharacterViewModel
            {
                Character = charactersRepository.Items.First(x => x.Id == id),
                CanVote = false,
                IsVoted = false,
                WeeklyVoted = true
            });
        }

        [HttpPost]
        public ActionResult Unvote(int id)
        {
            var isWeeklyVoted = WeeklyVoted(userProvider.GetUser(this).Identity.GetUserId());
            if (!isWeeklyVoted)
            {
                var votes = cartProvider.GetCart(this).Votes;
                if (votes.Contains(id))
                {
                    votes.Remove(id);
                    var cost = charactersRepository.Items.First(x => x.Id == id).Cost;
                    var points = cartProvider.GetCart(this).Points;
                    cartProvider.GetCart(this).Points = (int)(points + cost);
                }
            }
            return PartialView("_CharacterCard", new CharacterViewModel
            {
                Character = charactersRepository.Items.First(x => x.Id == id),
                IsVoted = false,
                CanVote = !isWeeklyVoted
            });
        }

        public RedirectToRouteResult Submit()
        {
            if (!userProvider.GetUser(this).Identity.IsAuthenticated)
                return RedirectToAction("Register", "Account", new { returnUrl = Url.Action("Submit") });

            if (WeeklyVoted(userProvider.GetUser(this).Identity.GetUserId()))
                return RedirectToAction("List");

            var userId = userProvider.GetUser(this).Identity.GetUserId();
            ApplicationUser user = usersRepository.Items.First(x => x.Id == userId);
            foreach (var characterId in cartProvider.GetCart(this).Votes)
            {
                var vote = new Vote { User = user, Week = Week };
                votesRepository.Add(vote);

                var character = charactersRepository.Items.First(x => x.Id == characterId);
                var voteItem = new VoteItem
                {
                    Position = 1,
                    Vote = vote,
                    Hero = character
                };
                voteItemsRepository.Add(voteItem);
            }
            cartProvider.SetCart(this, new Cart(new HashSet<int>(), cartProvider.GetCart(this).Points));
            return RedirectToAction("List");
        }
    }
}
