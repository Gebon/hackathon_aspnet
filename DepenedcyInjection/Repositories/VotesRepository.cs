using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using DependencyInjection.Models;
using Domain;

namespace DepenedcyInjection.Repositories
{
    public class VotesRepository : BaseRepository<Vote>
    {
        public override IQueryable<Vote> Items => context.Votes.AsQueryable();
        public override void Add(Vote item)
        {
            context.Votes.Add(item);
        }

        public VotesRepository(ApplicationDbContext context) : base(context)
        {
        }
    }

    public class VoteItemsRepository : BaseRepository<VoteItem>
    {
        public override IQueryable<VoteItem> Items => context.VoteItems.AsQueryable();
        public override void Add(VoteItem item)
        {
            context.VoteItems.Add(item);
        }

        public VoteItemsRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
    public class CharactersRepository : BaseRepository<Character>
    {
        public override IQueryable<Character> Items => context.Characters.AsQueryable();
        public override void Add(Character item)
        {
            context.Characters.Add(item);
        }

        public CharactersRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
    public class UsersRepository : BaseRepository<ApplicationUser>
    {
        public override IQueryable<ApplicationUser> Items => context.Users.AsQueryable();
        public override void Add(ApplicationUser item)
        {
            context.Users.Add(item);
        }

        public UsersRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}