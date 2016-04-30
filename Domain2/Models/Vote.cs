using System.ComponentModel.DataAnnotations;
using DependencyInjection.Models;

namespace Domain
{
    public class Vote
    {
        [Key]
        public int Id { get; set; }
        public virtual ApplicationUser User { get; set; }
        public int Week { get; set; }
    }
}
