using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DependencyInjection.Models;
using Domain;

namespace Domain2.Models
{
    public class Comment
    {
        public int Id { get; set; }
        public virtual ApplicationUser User { get; set; }
        public virtual Character Character { get; set; }

        [MaxLength(1000)]
        public string Text { get; set; }
    }
}
