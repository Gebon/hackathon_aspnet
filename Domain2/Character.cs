using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity.EntityFramework;


namespace Domain
{
    public enum Gender
    {
        Male,
        Female
    }
    public class Character
    {
        [Key]
        public int Id { get; set; }
        [StringLength(200), Required]
        public string Name { get; set; }
        [Required]
        public Gender? Gender { get; set; }
        [StringLength(100)]
        public string Born { get; set; }
        [Required]
        public bool? Died { get; set; }
        [Required, DefaultValue(2)]
        public int? Cost { get; set; }

        public override string ToString()
        {
            return Name;
        }

        public override bool Equals(object obj)
        {
            var o = obj as Character;
            return Id == (o?.Id ?? -1);
        }
    }

}
