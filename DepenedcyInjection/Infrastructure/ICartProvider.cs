using System.Collections.Generic;
using System.Web.Mvc;

namespace DepenedcyInjection.Infrastructure
{
    public interface ICartProvider
    {
        IEnumerable<int> GetCart(Controller c);
        void SetCart(Controller c, HashSet<int> cart);
        int GetPoints(Controller c);
        void SetPoints(Controller c, int points);
    }
}
