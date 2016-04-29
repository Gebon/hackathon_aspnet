using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Domain;

namespace DepenedcyInjection.Infrastructure
{
    public class CartProvider : ICartProvider
    {
        private const int DefaultPoints = 15;
        private const string Points = "points";
        private const string Votes = "votes";

        public IEnumerable<int> GetCart(Controller c)
        {
            if (c.Session[Votes] == null)
                SetCart(c, new HashSet<int>());

            return c.Session[Votes] as HashSet<int>;
        }

        public void SetCart(Controller c, HashSet<int> cart)
        {
            c.Session[Votes] = cart;
        }

        public int GetPoints(Controller c)
        {
            if (c.Session[Points] == null)
                SetPoints(c, DefaultPoints);
            return (int) c.Session[Points];
        }

        public void SetPoints(Controller c, int points)
        {
            c.Session[Points] = points;
        }
    }
}