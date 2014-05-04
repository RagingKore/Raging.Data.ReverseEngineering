using Raging.Data.ReverseEngineering.Infrastructure;
using Raging.Data.ReverseEngineering.Infrastructure.Metadata;
using Raging.Data.Schema;

namespace Raging.Data.ReverseEngineering.EntityFramework.Adapters
{
    public class NavigationPropertyInfoAdapterFactory : INavigationPropertyInfoAdapterFactory
    {
        public INavigationPropertyInfoAdapter Create(ISchemaForeignKey foreignKey, ISchemaTable table, IIdentifierGenerationService identifierGenerationService)
        {
            return new NavigationPropertyInfoAdapter(foreignKey, table, identifierGenerationService);
        }
    }
}