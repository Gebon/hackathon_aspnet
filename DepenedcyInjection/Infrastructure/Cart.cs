using System.Collections.Generic;

namespace DepenedcyInjection.Infrastructure
{
    public class Cart
    {
        private const int DefaultPoints = 15;
        public HashSet<int> Votes { get; set; }
        public int Points { get; set; }

        public Cart()
        {
            Points = DefaultPoints;
            Votes = new HashSet<int>();
        }


        public Cart(HashSet<int> votes, int points)
        {
            Votes = votes;
            Points = points;
        }
    }
}