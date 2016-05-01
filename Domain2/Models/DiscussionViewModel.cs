using System.Collections.Generic;
using DependencyInjection.Models;
using Domain;
using Domain2.Models;

namespace DepenedcyInjection
{
    public class DiscussionViewModel
    {
        public Character Character { get; set; }
        public IEnumerable<Comment> Comments { get; set; }
        public string UserId { get; set; }
    }
}