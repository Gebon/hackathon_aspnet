using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DependencyInjection.Models;
using Domain;
using Microsoft.VisualBasic.FileIO;
using Newtonsoft.Json;
using Ninject.Infrastructure.Language;
using PagedList;

namespace DepenedcyInjection.Controllers
{
    public class CharacterController : Controller
    {
        private static int PageSize = 2;
        private ICharactersRepository repository;
        private ApplicationDbContext context = new ApplicationDbContext();
        public static int DefaultPoints = 15;

        public CharacterController(ICharactersRepository productRepository)
        {
            repository = productRepository;
        }

        public ViewResult List(int? page)
        {
            page = page ?? 1;
            ViewBag.Action = "List";
            return View(repository.Characters.ToEnumerable().ToPagedList(page.Value, PageSize));
        }

        public string Import(string filePath)
        {
            return "";
        }


        public IEnumerable<Character> FilterInternal(string fieldName, string fieldValue)
        {
            var result = repository.Characters.ToList().Where(x => x.GetType()
                .GetProperties()
                .First(property => property.Name == fieldName).GetValue(x).ToString().Equals(fieldValue, StringComparison.InvariantCultureIgnoreCase));
            return result.OrderBy(x => x.Name);
        }

        public ActionResult Filter(string fieldName, string fieldValue, int? page)
        {
            ViewBag.Action = "Filter";
            return View("List", FilterInternal(fieldName, fieldValue).ToPagedList(page ?? 1, PageSize));
        }

        [HttpPost]
        public JsonResult Search(string filterData)
        {
            var json = JsonConvert.DeserializeObject<dynamic[]>(filterData);
            var result = repository.Characters.ToList();
            foreach (var o in json)
            {
                if (string.IsNullOrEmpty(o.fieldValue.ToObject<string>()))
                    continue;
                IEnumerable<Character> filtered = FilterInternal(o.fieldName.ToObject<string>(), o.fieldValue.ToObject<string>());
                result.RemoveAll(x => !filtered.Contains(x));
            }
            return Json(result);
        }

        public ActionResult Search(IEnumerable<Character> characters)
        {
            return View(characters ?? new List<Character>());
        }

        [HttpPost]
        public void Vote(int id)
        {
            var votes = GetVotesFromSession();
            var cost = repository.Characters.First(x => x.Id == id).Cost;
            var points = GetPointsFromSession();
            Session["points"] = points - cost;
            votes.Add(id);
        }
        private int GetPointsFromSession()
        {
            if (Session["points"] == null)
            {
                Session["points"] = DefaultPoints;
            }
            var votes = (int)Session["points"];
            return votes;
        }

        private HashSet<int> GetVotesFromSession()
        {
            if (Session["votes"] == null)
                Session["votes"] = new HashSet<int>();
            var votes = Session["votes"] as HashSet<int>;
            return votes;
        }

        [HttpPost]
        public void Unvote(int id)
        {
            var votes = GetVotesFromSession();
            if (votes.Contains(id))
            {
                votes.Remove(id);
                var cost = repository.Characters.First(x => x.Id == id).Cost;
                var points = GetPointsFromSession();
                Session["points"] = points + cost;
            }
        }
    }
}
