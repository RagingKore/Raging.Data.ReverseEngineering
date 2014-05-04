namespace Raging.Data.Schema
{
    public interface ISchemaColumn
    {
        string Name           { get; }
        string Type           { get; }
        string SystemType     { get; }
        int DateTimePrecision { get; }
        string DefaultValue   { get; }
        int MaxLength         { get; }
        int Precision         { get; }
        int Scale             { get; }
        int Ordinal           { get; }
        bool IsIdentity       { get; }
        bool IsNullable       { get; }
        bool IsPrimaryKey     { get; }
        bool IsStoreGenerated { get; }
        bool IsRowVersion     { get; }
    }
}