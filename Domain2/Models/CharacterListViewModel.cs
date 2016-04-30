using System.Collections.Generic;

namespace Domain.Models
{
    public class CharacterListViewModel
    {
        public IEnumerable<CharacterViewModel> CharacterViewModels { get; set; }
        public int Points { get; set; }
    }
}
