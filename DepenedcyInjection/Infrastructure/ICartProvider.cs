using System.Collections.Generic;

namespace DepenedcyInjection.Infrastructure
{
    public interface ICartProvider
    {
        IEnumerable<int> Cart { get; } 
    }
}
