using Raging.Data.ReverseEngineering.Infrastructure;
using Raging.Data.ReverseEngineering.Infrastructure.Metadata;
using Raging.Data.Schema;

namespace Raging.Data.ReverseEngineering.EntityFramework.Adapters
{
    public class EntityInfoAdapterFactory : IEntityInfoAdapterFactory
    {
        public IEntityInfoAdapter Create(ISchemaTable table, IIdentifierGenerationService identifierGenerationService)
        {
            return new EntityInfoAdapter(table, identifierGenerationService);
        }
    }
}