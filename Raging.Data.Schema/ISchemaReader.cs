using System.Collections.Generic;

namespace Raging.Data.Schema
{
    public interface ISchemaReader
    {
        IReadOnlyList<ISchemaTable> Read();
    }
}