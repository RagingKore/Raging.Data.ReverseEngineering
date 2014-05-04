using System.Collections.Generic;

namespace Raging.Data.Schema
{
    public interface ISchemaTable
    {
        string Schema                          { get; }
        string Name                            { get; }     
        string FullName                        { get; }
        bool IsView                            { get; }
        List<ISchemaColumn> Columns            { get; }
        List<ISchemaForeignKey> ForeignKeys    { get; }
        IEnumerable<ISchemaColumn> PrimaryKeys { get; }
        ISchemaColumn this[string columnName]  { get; }
    }
}