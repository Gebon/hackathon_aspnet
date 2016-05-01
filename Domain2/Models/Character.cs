using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Net.Http.Headers;


namespace Domain
{
    public enum Gender
    {
        [Display(Name = "Мужчина")]
        Male,

        [Display(Name = "Женщина")]
        Female
    }
    public class Character
    {
        [Key]
        public int Id { get; set; }

        [StringLength(200, MinimumLength = 3, ErrorMessage = "Name must have at least 3 letters"), Required]
        [Display(Name = "Имя персонажа", AutoGenerateField = true, AutoGenerateFilter = true, ShortName = "Имя")]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Пол персонажа", AutoGenerateField = true, AutoGenerateFilter = true, ShortName = "Пол")]
        public Gender? Gender { get; set; }

        [StringLength(100)]
        [Display(Name = "Дата рождения", AutoGenerateField = true, AutoGenerateFilter = true)]
        public string Born { get; set; }

        [Required]
        [Display(Name = "Умер ли персонаж", AutoGenerateField = true, AutoGenerateFilter = true)]
        public bool? Died { get; set; }

        [Required, DefaultValue(5)]
        [UIHint("Price")]
        [Display(Name = "Стоимость персонажа", AutoGenerateField = true, AutoGenerateFilter = true, ShortName = "Стоимость")]
        [Range(1, 20, ErrorMessage = "Should be between 1:20")]
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
