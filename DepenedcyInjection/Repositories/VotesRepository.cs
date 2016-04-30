using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using DependencyInjection.Models;
using Domain;

namespace DepenedcyInjection.Repositories
{
    public class VotesRepository : BaseRepository, IRepository<Vote>
    {
        public IQueryable<Vote> Items => context.Votes.AsQueryable();
        public void Add(Vote item)
        {
            context.Votes.Add(item);
            context.SaveChanges();
        }

        public VotesRepository(ApplicationDbContext context) : base(context)
        {
        }
    }

    public class VoteItemsRepository : BaseRepository, IRepository<VoteItem>
    {
        public IQueryable<VoteItem> Items => context.VoteItems.AsQueryable();
        public void Add(VoteItem item)
        {
            context.VoteItems.Add(item);
            context.SaveChanges();
        }

        public VoteItemsRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
    public class CharactersRepository : BaseRepository, IRepository<Character>
    {
        public IQueryable<Character> Items => context.Characters.AsQueryable();
        public void Add(Character item)
        {
            context.Characters.Add(item);
            context.SaveChanges();
        }

        public CharactersRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
    public class UsersRepository : BaseRepository, IRepository<ApplicationUser>
    {
        public IQueryable<ApplicationUser> Items => context.Users.AsQueryable();
        public void Add(ApplicationUser item)
        {
            context.Users.Add(item);
            context.SaveChanges();
        }

        public UsersRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}