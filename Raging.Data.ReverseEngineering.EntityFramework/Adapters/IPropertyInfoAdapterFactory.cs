using Raging.Data.ReverseEngineering.Infrastructure;
using Raging.Data.ReverseEngineering.Infrastructure.Metadata;
using Raging.Data.Schema;

namespace Raging.Data.ReverseEngineering.EntityFramework.Adapters
{
    public interface IPropertyInfoAdapterFactory
    {
        IPropertyInfoAdapter Create(ISchemaColumn column, ISchemaTable table, IIdentifierGenerationService identifierGenerationService);
    }
}