namespace Raging.Data.Schema
{
    public interface ISchemaForeignKey
    {
        string ForeignKeySchema        { get; }
        string ForeignKeyTableName     { get; }
        string ForeignKeyTableFullName { get; }
        string ForeignKeyColumn        { get; }
        string PrimaryKeySchema        { get; }
        string PrimaryKeyTableName     { get; }
        string PrimaryKeyTableFullName { get; }     
        string PrimaryKeyColumn        { get; }
        string ConstraintName          { get; } 
    }
}