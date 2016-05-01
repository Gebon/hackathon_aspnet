using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Web;
using DependencyInjection.Models;
using Domain;
using Domain2.Models;

namespace DepenedcyInjection.Repositories
{
    public class VotesRepository : BaseRepository<Vote>
    {
        public override IQueryable<Vote> Items => context.Votes.AsQueryable();
        public override void Add(Vote item)
        {
            context.Votes.Add(item);
        }

        public override void Update(Vote item)
        {
            throw new InvalidOperationException();
        }

        public override void Remove(Vote item)
        {
            throw new InvalidOperationException();
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

        public override void Update(VoteItem item)
        {
            throw new InvalidOperationException();
        }

        public override void Remove(VoteItem item)
        {
            throw new InvalidOperationException();
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

        public override void Update(Character item)
        {
            //context.Entry(item).State = EntityState.Modified;
            context.Characters.AddOrUpdate(item);
        }

        public override void Remove(Character item)
        {
            context.Characters.Remove(item);
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

        public override void Update(ApplicationUser item)
        {
            throw new InvalidOperationException();
        }

        public override void Remove(ApplicationUser item)
        {
            throw new InvalidOperationException();
        }

        public UsersRepository(ApplicationDbContext context) : base(context)
        {
        }
    }

    public class CommentsRepository : BaseRepository<Comment>
    {
        public CommentsRepository(ApplicationDbContext context) : base(context)
        {
        }

        public override IQueryable<Comment> Items => context.Comments.AsQueryable();
        public override void Add(Comment item)
        {
            context.Comments.Add(item);
        }

        public override void Update(Comment item)
        {
            context.Comments.AddOrUpdate(item);
        }

        public override void Remove(Comment item)
        {
            context.Comments.Remove(item);
        }
    }
}