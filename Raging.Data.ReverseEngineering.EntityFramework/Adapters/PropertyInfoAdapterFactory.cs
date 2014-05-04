using Raging.Data.ReverseEngineering.Infrastructure;
using Raging.Data.ReverseEngineering.Infrastructure.Metadata;
using Raging.Data.Schema;

namespace Raging.Data.ReverseEngineering.EntityFramework.Adapters
{
    public class PropertyInfoAdapterFactory : IPropertyInfoAdapterFactory
    {
        public IPropertyInfoAdapter Create(ISchemaColumn column, ISchemaTable table, IIdentifierGenerationService identifierGenerationService)
        {
            return new PropertyInfoAdapter(column, table, identifierGenerationService);
        }
    }
}