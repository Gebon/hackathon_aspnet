using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DependencyInjection.Models;
using DepenedcyInjection.Repositories;
using Domain;
using Domain2.Models;
using Microsoft.AspNet.Identity;

namespace DepenedcyInjection.Controllers
{
    public class DiscussionController : Controller
    {
        private readonly IRepository<Comment> commentsRepository;
        private readonly IRepository<Character> charactersRepository;
        private readonly IRepository<ApplicationUser> usersRepository;

        public DiscussionController(IRepository<Comment> commentsRepository, IRepository<Character> charactersRepository, IRepository<ApplicationUser> usersRepository)
        {
            this.commentsRepository = commentsRepository;
            this.charactersRepository = charactersRepository;
            this.usersRepository = usersRepository;
        }

        public ActionResult ForCharacter(int id)
        {
            var character = charactersRepository.Items.FirstOrDefault(x => x.Id == id);
            var comments = commentsRepository.Items.Where(x => x.Character.Id == id);
            var userId = User.Identity.GetUserId();
            return View(new DiscussionViewModel {Character = character, Comments = comments.ToList(), UserId = userId});
        }

        [HttpPost]
        [Authorize]
        public ActionResult Add(Comment comment, string userId, int characterId)
        {
            if (ModelState.IsValid)
            {
                var user = usersRepository.Items.FirstOrDefault(x => x.Id == userId);
                var character = charactersRepository.Items.FirstOrDefault(x => x.Id == characterId);
                comment.Character = character;
                comment.User = user;
                commentsRepository.Add(comment);
                commentsRepository.SaveChanges();
                return PartialView("_Comment", comment);
            }
            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }
    }
}