using System.Collections.Generic;

namespace DepenedcyInjection.Infrastructure
{
    public interface ISessionProvider
    {
        IEnumerable<int> Cart { get; } 
    }
}
