using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain;

namespace Domain2
{
    public interface ICharactersRepository
    {
        IQueryable<Character> Characters { get; }
    }
}
