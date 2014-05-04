using System.Collections.Generic;

namespace Raging.Data.ReverseEngineering
{
    public interface IDomainModelProvider
    {
        IEnumerable<EntityInfo> GetInformation();
    }
}