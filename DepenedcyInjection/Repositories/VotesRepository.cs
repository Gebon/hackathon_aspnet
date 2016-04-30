using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DependencyInjection.Models;
using Domain;

namespace DepenedcyInjection.Repositories
{
    public class VotesRepository : BaseRepository, IRepository<Vote>
    {
        public IQueryable<Vote> Items => Context.Votes.AsQueryable();
        public void Add(Vote item)
        {
            Context.Votes.Add(item);
            Context.SaveChanges();
        }
    }

    public class BaseRepository
    {
        protected readonly ApplicationDbContext Context = ApplicationDbContext.Create();
    }

    public class VoteItemsRepository : BaseRepository, IRepository<VoteItem>
    {
        public IQueryable<VoteItem> Items => Context.VoteItems.AsQueryable();
        public void Add(VoteItem item)
        {
            Context.VoteItems.Add(item);
            Context.SaveChanges();
        }
    }
    public class CharactersRepository : BaseRepository, IRepository<Character>
    {
        public IQueryable<Character> Items => Context.Characters.AsQueryable();
        public void Add(Character item)
        {
            Context.Characters.Add(item);
            Context.SaveChanges();
        }
    }
    public class UsersRepository : BaseRepository, IRepository<ApplicationUser>
    {
        public IQueryable<ApplicationUser> Items => Context.Users.AsQueryable();
        public void Add(ApplicationUser item)
        {
            Context.Users.Add(item);
            Context.SaveChanges();
        }
    }
}